using CommunityToolkit.Diagnostics;
using GoPOS.Common.PrinterLib.protocol;
using Microsoft.AspNetCore.SignalR.Protocol;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using System.Diagnostics;
using System.Drawing;
using System.Text;
using System.Text.RegularExpressions;
using static GoPOS.Common.PrinterLib.protocol.BasicProtocol;
using GoShared.DeviceLib.Printer.Models;
using GoShared.DeviceLib;
using GoPOS.ViewModels;
using System.Windows.Media;
using System.IO;
using System.Windows.Media.Imaging;
using GoShared.Helpers;
using System.Windows.Markup;

namespace GoPOS.Common.PrinterLib
{
    /// <summary>
    /// SEWOO SLK-TS100 Printer
    /// </summary>
    public class GeneralPrinter : IPrinterBase, IDisposable
    {
        byte[] FeedLines1 = new byte[3] { 27, 100, 1 };
        byte[] FeedLines2 = new byte[3] { 27, 100, 2 };
        byte[] SetCharacterSize0 = new byte[3] { 29, 33, 0 };
        byte[] SetCharacterSize1 = new byte[3] { 29, 33, 17 };
        byte[] FeedLines5 = new byte[3] { 27, 100, 5 };
        byte[] CutPaper10 = new byte[3] { 29, 86, 1 };
        byte[] AlignRight = new byte[3] { 27, 97, 2 };
        byte[] AlignLeft = new byte[3] { 27, 97, 0 };
        byte[] AlignCenter = new byte[3] { 27, 97, 1 };

        public PrintStatus _status = null;

        public bool IsOpen
        {
            get
            {
                return serialPort == null ? false : serialPort.IsOpen;
            }
        }

        readonly BasicProtocol sewooProtocol = new BasicProtocol();
        SerialPortWrapper? serialPort;

        public GeneralPrinter()
        {
            _status = new PrintStatus();
        }

        public bool TryOpen(int portNo, int bitrate)
        {
            _status = new PrintStatus();
            serialPort = new SerialPortWrapper();
            bool openOk = serialPort.Open(portNo, bitrate);
            serialPort.OnRead += SerialPort_OnRead;

            if (openOk)
            {
                AfterConnect();
                GetPrinterStatus();
            }

            return openOk;
        }

        public void ClosePrinter()
        {
            _status = new PrintStatus();
            serialPort?.Close();
            serialPort = null;
        }

        AutoResetEvent receiveSignal = new AutoResetEvent(false);

        byte lastReceivedData;
        private void SerialPort_OnRead(object sender, byte[] buffer)
        {
            foreach (byte b in buffer)
                lastReceivedData = b;
            receiveSignal.Set();
        }

        void AfterConnect()
        {
            serialPort?.Write(sewooProtocol.InitializePrinter());
            serialPort?.Write(sewooProtocol.SelectJustification(BasicProtocol.Justification.Left));
            SetPrinterMode(PrinterModeEnum.StandardMode);
        }

        public enum PrinterModeEnum
        {
            StandardMode,
            PageMode
        }

        void SetPrinterMode(PrinterModeEnum printerMode)
        {
            Debug.Assert(printerMode == PrinterModeEnum.StandardMode || printerMode == PrinterModeEnum.PageMode);

            if (printerMode == PrinterModeEnum.StandardMode)
            {
                serialPort?.Write(sewooProtocol.SelectStandardMode());
            }
            else if (printerMode == PrinterModeEnum.PageMode)
            {
                serialPort?.Write(sewooProtocol.SelectPageMode());
            }
        }
        public bool SetDownloadImage(Bitmap src)
        {
            var downloadOk = false;
            try
            {
                Guard.IsBetween(src.Width, 1, 255);
                Guard.IsBetween(src.Height, 1, 48);
                Guard.IsBetween(src.Width * src.Height, 1, 912);

                serialPort?.Write(sewooProtocol.DefineDownloadBitImage(src));
                downloadOk = true;
            }
            catch (Exception e)
            {
                //LogHelper.Logger.Error(e.ToFormattedString());
            }
            return downloadOk;
        }
        public bool PrintBytes(List<byte[]> datas)
        {
            //if (_status.isOnline)
            foreach (var item in datas)
            {
                serialPort?.Write(item);
            }
            return true;
        }
        public bool PrintDownloadImage()
        {
            serialPort?.Write(sewooProtocol.PrintDownloadBitImage(BasicProtocol.ImagePrintMode.Normal));
            return true;
        }

        public bool PrintNvImage(int imageNo)
        {
            var cmd1 = sewooProtocol.PrintNvImage(imageNo, BasicProtocol.ImagePrintMode.Normal);
            serialPort.Write(cmd1);
            serialPort.Write(sewooProtocol.InitializePrinter());
            return true;
        }

        readonly Encoding TextEncoder = Encoding.GetEncoding(949);
        public bool PrintText(string text)
        {
            //if (_status.isOnline)
            {
                serialPort?.Write(TextEncoder.GetBytes(text));
                serialPort?.Write(new byte[] { 0x0d, 0x0a });
            }

            return _status.isOnline;
        }

        public bool PrintByte(byte[] data)
        {
            //if (_status.isOnline)
                serialPort?.Write(data);
            return true;
        }
        public bool CutPaper()
        {
            if (_status.isOnline)
            serialPort.Write(sewooProtocol.SelectCutModeAndCutPaper(CutModeEnum.PartialCut));
            return _status.isOnline;
        }

        public byte[] CutPaper1()
        {
            return sewooProtocol.SelectCutModeAndCutPaper(CutModeEnum.PartialCut);
        }

        public byte[] FeedLines(int lines)
        {
            byte[] data = new byte[lines];
            data = sewooProtocol.FeedLines(lines);
            //if (_status.isOnline)
                serialPort?.Write(sewooProtocol.FeedLines(lines));
            return data;
        }
        public void FeedPaper(int feed)
        {
            //if (_status.isOnline)
                serialPort?.Write(sewooProtocol.SelectCutModeAndCutPaper(CutModeEnum.FeedsPaper, feed));
        }

        public bool PrintBarcode(string data)
        {
            //if (_status.isOnline)
                serialPort?.Write(sewooProtocol.PrintBarcode(BasicProtocol.BarcodeSystemEnum.CODE128, data));
            return _status.isOnline;
        }
        public bool PrintBarcodeWithHeight(string data)
        {
            //if (_status.isOnline)
            {
                serialPort?.Write(sewooProtocol.SetBarcodeHeight(81));
                serialPort?.Write(sewooProtocol.PrintBarcode(BasicProtocol.BarcodeSystemEnum.CODE128, data));
            }
            return _status.isOnline;
        }
        public bool PrintBarcodeWithHeight(string data, int height)
        {
            //if (_status.isOnline)
            {
                serialPort?.Write(sewooProtocol.SetBarcodeHeight(height));
                serialPort?.Write(sewooProtocol.PrintBarcode(BasicProtocol.BarcodeSystemEnum.CODE128, data));
            }
            return _status.isOnline;
        }

        public void Dispose()
        {
            serialPort?.Close();
            serialPort = null;
        }

        public void UploadNvImage(int imageNo, Bitmap src)
        {
            var data = sewooProtocol.DefineNvImage(imageNo, src);
            serialPort?.Write(data);
        }

        public void PrintRasterImage(Bitmap src, ImagePrintMode mode)
        {
            var data = sewooProtocol.PrintRasterImage(src, mode);
            serialPort?.Write(data);
        }

        public void SetCharacterSize(SizeEnlargeEnum size)
        {
            var data = sewooProtocol.SelectCharacterSize((size == SizeEnlargeEnum.DoubleWidth || size == SizeEnlargeEnum.BothDouble) ? 1 : 0,
                                                            (size == SizeEnlargeEnum.DoubleHeight || size == SizeEnlargeEnum.BothDouble) ? 1 : 0);
            serialPort?.Write(data);
        }

        public byte[] SetCharacterSize(int width, int height)
        {
            var data = sewooProtocol.SelectCharacterSize(width, height);
            serialPort?.Write(data);
            return data;
        }

        public void SetPrintHRI(BasicProtocol.HRIPrintPositionEnum position = HRIPrintPositionEnum.BelowTheBarcode)
        {
            serialPort?.Write(sewooProtocol.SelectHRICharacter(position));
        }


        #region //20230612 add by phong begin

        /// <summary>
        /// 
        /// </summary>
        /// <param name="logo"></param>
        /// <param name="logoType"></param>
        /// <param name="textWithFormat"></param>
        /// <returns></returns>
        public string PrintForm(ImageSource logo, int logoType, string textWithFormat)
        {
            StringBuilder sbLines = new StringBuilder();
            string[] lines = textWithFormat.Split(new string[] { "\r\n", "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries);

            for (int i = 0; i < lines.Length; i++)
            {
                List<string> elements = new List<string>();
                Regex regex = new Regex(@"\{([^{}]+)\}|([^{}]+)");
                MatchCollection matches = regex.Matches(lines[i]);

                string left = "", center = "", right = "";
                bool font3 = false, feedLine = false;
                Align _align = Align.Left;
                foreach (Match match in matches)
                {
                    string element = match.Groups[1].Success ? match.Groups[0].Value : match.Groups[2].Value;

                    if (element == "{FONT1}" || element == "{FONT2}" || element == "{글자크기1}" || element == "{글자크기2}")
                    {
                        SetCharacterSize(0, 0);
                    }
                    else if (element == "{FONT3}" || element == "{FONT4}" || element == "{글자크기3}" || element == "{글자크기4}")
                    {
                        font3 = true;
                        SetCharacterSize(1, 1);
                    }
                    else if (element == "{CUT}")
                    {
                        FeedLines(5);
                        CutPaper();
                    }
                    else if (element.Length >= 3 && element.Substring(1, 1) == ":")
                    {
                        string str = element.Substring(2, 1);
                        if (str == "C")
                        {
                            _align = Align.Center;
                        }
                        else if (str == "R")
                        {
                            _align = Align.Right;
                        }
                        else if (str == "L")
                        {
                            _align = Align.Center;
                        }
                    }
                    else if (element.Length >= 3 && element.Substring(1, 1) == "/")
                    {
                        _align = Align.Left;
                    }
                    else if (element == "{CRLF}")
                    {
                        feedLine = true;
                    }
                    else if (element == "{BARCODE}")
                    {
                        PrintBarcode("BARCODE");
                    }
                    else if (element == "{LOG}")
                    {
                        serialPort?.Write(new byte[] { ASCIITable.ESC, 0x61, (byte)1 });
                        PrintLogo(logo, logoType);
                    }
                    else
                    {
                        if (_align == Align.Left)
                        {
                            left += element;
                        }
                        else if (_align == Align.Right)
                        {
                            right += element;
                        }
                        else if (_align == Align.Center)
                        {
                            center += element;
                        }
                    }
                }

                string lineText = string.Empty;

                if (font3)
                {
                    SetCharacterSize(1, 1);
                    PrintText(lineText = ProcessTextLine(left, center, right, font3));
                    FeedLines(1);
                    font3 = false;
                }
                else
                {
                    SetCharacterSize(0, 0);
                    PrintText(lineText = ProcessTextLine(left, center, right, font3));
                }

                sbLines.AppendLine(lineText);
                if (feedLine)
                {
                    FeedLines(1);
                    feedLine = false;
                }
                _align = Align.Left;
                SetCharacterSize(0, 0);
            }

            return sbLines.ToString();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="logo"></param>
        /// <param name="logoType"></param>
        /// <param name="textWithFormat"></param>
        /// <returns></returns>
        public List<byte[]> PrintFormToByte(ImageSource logo, int logoType, string textWithFormat, out string printedText)
        {
            printedText = string.Empty;
            List<byte[]> sbLines = new List<byte[]>();
            string[] lines = textWithFormat.Split(new string[] { "\r\n", "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries);
            Encoding encoding949 = Encoding.GetEncoding(949);
            for (int i = 0; i < lines.Length; i++)
            {
                List<string> elements = new List<string>();
                Regex regex = new Regex(@"\{([^{}]+)\}|([^{}]+)");
                MatchCollection matches = regex.Matches(lines[i]);

                string left = "", center = "", right = "";
                bool font3 = false, feedLine = false;
                Align _align = Align.Left;
                foreach (Match match in matches)
                {
                    string element = match.Groups[1].Success ? match.Groups[0].Value : match.Groups[2].Value;

                    if (element == "{FONT1}" || element == "{FONT2}" || element == "{글자크기1}" || element == "{글자크기2}")
                    {
                        sbLines.Add(SetCharacterSize0);
                    }
                    else if (element == "{FONT3}" || element == "{FONT4}" || element == "{글자크기3}" || element == "{글자크기4}")
                    {
                        font3 = true;
                        sbLines.Add(SetCharacterSize1);
                    }
                    else if (element == "{CUT}")
                    {
                        sbLines.Add(FeedLines5);
                        sbLines.Add(CutPaper10);
                    }
                    else if (element.Length >= 3 && element.Substring(1, 1) == ":")
                    {
                        string str = element.Substring(2, 1);
                        if (str == "C")
                        {
                            _align = Align.Center;
                        }
                        else if (str == "R")
                        {
                            _align = Align.Right;
                        }
                        else if (str == "L")
                        {
                            _align = Align.Left;
                        }
                    }
                    else if (element.Length >= 3 && element.Substring(1, 1) == "/")
                    {
                        _align = Align.Left;
                    }
                    else if (element == "{CRLF}")
                    {
                        feedLine = true;
                    }
                    else if (element == "{BARCODE}")
                    {
                        //sbLines.Add(sewooProtocol.PrintBarcode(BasicProtocol.BarcodeSystemEnum.CODE128, "BARCODE"));
                        //PrintBarcode("BARCODE");
                    }
                    else if (element == "{LOG}")
                    {
                        sbLines.Add(new byte[] { ASCIITable.ESC, 0x61, (byte)1 });
                        //PrintLogo(logo, logoType);
                    }
                    else
                    {
                        if (_align == Align.Left)
                        {
                            left += element;
                        }
                        else if (_align == Align.Right)
                        {
                            right += element;
                        }
                        else if (_align == Align.Center)
                        {
                            center += element;
                        }
                    }
                }


                if (font3)
                {
                    sbLines.Add(SetCharacterSize1);
                    var bytetemp = encoding949.GetBytes(ProcessTextLine(left, center, right, font3).TrimEnd());
                    sbLines.Add(bytetemp);
                    sbLines.Add(FeedLines1);

                    font3 = false;
                }
                else
                {
                    sbLines.Add(SetCharacterSize0);
                    var bytetemp = encoding949.GetBytes(ProcessTextLine(left, center, right, font3).TrimEnd());
                    sbLines.Add(bytetemp);
                }
                sbLines.Add(new byte[] { 0x0A });
                if (feedLine)
                {
                    sbLines.Add(FeedLines1);
                    feedLine = false;
                }
                _align = Align.Left;
                sbLines.Add(SetCharacterSize0);
            }

            StringBuilder sbPrintText = new StringBuilder();
            var arbs = sbLines.ToArray();
            for (int i = 0; i < arbs.Length; i++)
            {
                sbPrintText.Append(encoding949.GetString(arbs[i]));
            }

            printedText = sbPrintText.ToString();
            return sbLines;
        }

        public List<Dictionary<string, string>> PreviewBill(string processedText)
        {
            List<Dictionary<string, string>> Preview = new List<Dictionary<string, string>>();
            string[] lines = processedText.Split(new string[] { "\r\n", "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < lines.Length; i++)
            {
                Regex regex = new Regex(@"\{([^{}]+)\}|([^{}]+)");
                MatchCollection matches = regex.Matches(lines[i]);

                string left = "";
                string center = "";
                string right = "";
                Align _align = Align.Left;

                foreach (Match match in matches)
                {
                    string element = match.Groups[1].Success ? match.Groups[0].Value : match.Groups[2].Value;

                    if (element == "{FONT1}" || element == "{FONT2}" || element == "{FONT3}" || element == "{FONT4}" || element == "{CUT}"
                        || element == "{글자크기1}" || element == "{글자크기2}" || element == "{글자크기3}" || element == "{글자크기4}"
                                             || element == "{BARCODE}" || element == "{LOG}" || element == "{CRLF}"
                       )
                        continue;

                    if (element.Length >= 3 && element.Substring(1, 1) == ":")
                    {
                        string str = element.Substring(2, 1);
                        if (str == "C")
                        {
                            _align = Align.Center;

                        }
                        else if (str == "R")
                        {
                            _align = Align.Right;
                        }
                        else if (str == "L")
                        {
                            _align = Align.Center;
                        }
                    }
                    else if (element.Length >= 3 && element.Substring(1, 1) == "/")
                    {
                        _align = Align.Left;
                    }
                    else
                    {
                        if (_align == Align.Left)
                        {
                            left += element;
                        }
                        else if (_align == Align.Right)
                        {
                            right += element;
                        }
                        else if (_align == Align.Center)
                        {
                            center += element;
                        }
                    }
                }

                Preview.Add(new Dictionary<string, string> { { "Left",
                                ProcessTextLine(left, center, "", false).TrimEnd() }, { "Right",
                                ProcessTextLine("", "", right, false).Trim() } });
                _align = Align.Left;
            }
            return Preview;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="textWithFormat"></param>
        /// <returns></returns>
        public string PrintReceiptPlain(string textWithFormat)
        {
            StringBuilder sbLines = new StringBuilder();
            string[] lines = textWithFormat.Split(new string[] { "\r\n", "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < lines.Length; i++)
            {
                Regex regex = new Regex(@"\{([^{}]+)\}|([^{}]+)");
                MatchCollection matches = regex.Matches(lines[i]);

                string left = "";
                string center = "";
                string right = "";
                Align _align = Align.Left;

                foreach (Match match in matches)
                {
                    string element = match.Groups[1].Success ? match.Groups[0].Value : match.Groups[2].Value;

                    if (element == "{FONT1}" || element == "{FONT2}" || element == "{FONT3}" || element == "{FONT4}" || element == "{CUT}"
                        || element == "{글자크기1}" || element == "{글자크기2}" || element == "{글자크기3}" || element == "{글자크기4}"
                                             || element == "{BARCODE}" || element == "{LOG}" || element == "{CRLF}")
                        continue;

                    if (element.Length >= 3 && element.Substring(1, 1) == ":")
                    {
                        string str = element.Substring(2, 1);
                        if (str == "C")
                        {
                            _align = Align.Center;
                        }
                        else if (str == "R")
                        {
                            _align = Align.Right;
                        }
                        else if (str == "L")
                        {
                            _align = Align.Center;
                        }
                    }
                    else if (element.Length >= 3 && element.Substring(1, 1) == "/")
                    {
                        _align = Align.Left;
                    }
                    else
                    {
                        if (_align == Align.Left)
                        {
                            left += element;
                        }
                        else if (_align == Align.Right)
                        {
                            right += element;
                        }
                        else if (_align == Align.Center)
                        {
                            center += element;
                        }
                    }
                }

                var lineText = ProcessTextLine(left, center, right, false).TrimEnd();
                sbLines.AppendLine(lineText);
                _align = Align.Left;
            }

            return sbLines.ToString();
        }

        private void PrintLogo(ImageSource bitmap, int ImagePrintMode)
        {
            try
            {
                PrintRasterImage(ConvertImageSourceToBitmap(bitmap), GetEnumValueByIndex<BasicProtocol.ImagePrintMode>(ImagePrintMode));
            }
            catch
            {
                //ghi ra file log
            }
        }

        public T GetEnumValueByIndex<T>(int index) where T : Enum
        {
            T[] enumValues = (T[])Enum.GetValues(typeof(T));
            if (index >= 0 && index < enumValues.Length)
            {
                return enumValues[index];
            }
            return default(T);
        }

        public Bitmap ConvertImageSourceToBitmap(ImageSource imageSource)
        {
            if (imageSource is BitmapSource bitmapSource)
            {
                BitmapFrame bitmapFrame = BitmapFrame.Create(bitmapSource);
                BitmapEncoder encoder = new BmpBitmapEncoder();
                encoder.Frames.Add(bitmapFrame);

                using (MemoryStream stream = new MemoryStream())
                {
                    encoder.Save(stream);
                    Bitmap bitmap = new Bitmap(stream);

                    return bitmap;
                }
            }
            return null;
        }

        private void ProcessBarcode(string key, string data)
        {
            JObject jsonObject = JObject.Parse(data);
            string barcodeValue = (string)jsonObject["BARCODE"];

            if (string.IsNullOrEmpty(barcodeValue))
                return;
            SetPrintHRI(BasicProtocol.HRIPrintPositionEnum.BelowTheBarcode);
            int size = 0;
            int.TryParse(barcodeValue, out size);
            if (size < 1 || size > 255)
            {
                return;
            }
            PrintBarcodeWithHeight(barcodeValue, size);
        }

        private enum Align
        {
            Left,
            Right,
            Center
        }

        public void GetPrinterStatus()
        {
            serialPort?.Write(sewooProtocol.TransmitRealtimeStatus(RealTimeStatusCodeEnum.PrinterStatus));
            bool received = receiveSignal.WaitOne(100);
            byte printerStatus = received ? lastReceivedData : (byte)0;

            serialPort?.Write(sewooProtocol.TransmitRealtimeStatus(RealTimeStatusCodeEnum.OfflineStatus));
            received = receiveSignal.WaitOne(100);
            byte offlineStatus = received ? lastReceivedData : (byte)0;

            serialPort?.Write(sewooProtocol.TransmitRealtimeStatus(RealTimeStatusCodeEnum.ErrorStatus));
            received = receiveSignal.WaitOne(100);
            byte errorStatus = received ? lastReceivedData : (byte)0;

            _status = received ? sewooProtocol.ParsePrinterStatus(printerStatus, offlineStatus, errorStatus) : new PrintStatus();
            //_status = sewooProtocol.ParsePrinterStatus(printerStatus, offlineStatus, errorStatus);
        }

        public enum PrinterStatus
        {
            isOnline,
            isOffline,
            error
        }

        //public void GetPrinterStatus()
        //{
        //    serialPort?.Write(sewooProtocol.TransmitRealtimeStatus(RealTimeStatusCodeEnum.PrinterStatus));
        //    bool received = receiveSignal.WaitOne(100);
        //    byte printerStatus = received ? lastReceivedData : (byte)0;

        //    serialPort?.Write(sewooProtocol.TransmitRealtimeStatus(RealTimeStatusCodeEnum.OfflineStatus));
        //    received = receiveSignal.WaitOne(100);
        //    byte offlineStatus = received ? lastReceivedData : (byte)0;

        //    serialPort?.Write(sewooProtocol.TransmitRealtimeStatus(RealTimeStatusCodeEnum.ErrorStatus));
        //    received = receiveSignal.WaitOne(100);
        //    byte errorStatus = received ? lastReceivedData : (byte)0;

        //    var printStatus = received ? sewooProtocol.ParsePrinterStatus(printerStatus, offlineStatus, errorStatus) : null;

        //    _status = sewooProtocol.ParsePrinterStatus(printerStatus, offlineStatus, errorStatus);
        //    logger.LogInformation("프린터 상태 : " + printStatus);
        //}


        public void OpenCashDraw()
        {
            serialPort?.Write(new byte[] { 27, 112, 0, 100, 250 });
        }

        public byte[] SelectJustification(Justification justification)
        {
            return new byte[] { ASCIITable.ESC, 0x61, (byte)( justification == Justification.Right ? 2 :
                                                        justification == Justification.Center ? 1: 0)};
        }

        #endregion //20230612 add by phong end

        #region Helper functions

        private string ProcessTextLine(string left, string center, string right, bool isFont3)
        {
            string desired = "                                          ";

            int leftByteCount = TextEncoder.GetByteCount(left.TrimEnd());
            int centerByteCount = TextEncoder.GetByteCount(center.Trim());
            int rightByteCount = TextEncoder.GetByteCount(right.Trim());

            if (leftByteCount + centerByteCount + rightByteCount > 42) return left.TrimEnd() + right.Trim() + center.Trim();

            StringBuilder sb = new StringBuilder(desired);
            sb.Remove(0, TextEncoder.GetByteCount(sb.ToString(), 0, leftByteCount));
            sb.Insert(0, left.TrimEnd());

            if (centerByteCount > 0)
            {
                if (isFont3)
                {
                    int centerInsertIndex = (desired.Length - centerByteCount * 2) / 4;
                    sb.Remove(centerInsertIndex - (TextEncoder.GetByteCount(sb.ToString()) - sb.ToString().Length), centerByteCount * 2);
                    sb.Insert(centerInsertIndex - (TextEncoder.GetByteCount(sb.ToString()) - sb.ToString().Length), center.Trim());
                }
                else
                {
                    int centerInsertIndex = (desired.Length - centerByteCount) / 2;
                    sb.Remove(centerInsertIndex - (TextEncoder.GetByteCount(sb.ToString()) - sb.ToString().Length), centerByteCount);
                    sb.Insert(centerInsertIndex - (TextEncoder.GetByteCount(sb.ToString()) - sb.ToString().Length), center.Trim());
                }
            }
            if (rightByteCount > 0)
            {
                sb.Remove(42 - rightByteCount - (TextEncoder.GetByteCount(sb.ToString()) - sb.ToString().Length), rightByteCount);
                sb.Insert(42 - rightByteCount - (TextEncoder.GetByteCount(sb.ToString()) - sb.ToString().Length), right.Trim());

            }
            string result = sb.ToString();
            return result;
        }


        #endregion
    }
}

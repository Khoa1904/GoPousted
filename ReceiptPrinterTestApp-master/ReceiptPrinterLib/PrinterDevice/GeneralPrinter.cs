using CommunityToolkit.Diagnostics;

using Microsoft.AspNetCore.SignalR.Protocol;
using Microsoft.Extensions.Logging;

using ReceiptPrinterLib.Models;
using ReceiptPrinterLib.PrinterDevice.protocol;
using ReceiptPrinterLib.Service;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;

using static ReceiptPrinterLib.PrinterDevice.protocol.BasicProtocol;

namespace ReceiptPrinterLib.PrinterDevice
{
    /// <summary>
    /// SEWOO SLK-TS100 Printer
    /// </summary>
    public class GeneralPrinter : IPrinterBase, IDisposable
    {
        readonly ILogger<GeneralPrinter> log;
        readonly BasicProtocol sewooProtocol = new BasicProtocol();

        SerialPortWrapper? serialPort;
        
        public GeneralPrinter(ILogger<GeneralPrinter> log)
        {
            this.log = log;
        }

        public bool TryOpen(int portNo, int bitrate)
        {
            serialPort = new SerialPortWrapper(log);
            bool openOk = serialPort.Open(portNo, bitrate);
            serialPort.OnRead += SerialPort_OnRead;
            if (openOk)
            {
                log.LogInformation("프린터 포트=[" + portNo + "] 접속");
                AfterConnect();
            }
            return openOk;
        }

        AutoResetEvent receiveSignal = new AutoResetEvent(false);

        byte lastReceivedData;
        private void SerialPort_OnRead(object sender, byte[] buffer)
        {
            log.LogInformation("buffer length = " + buffer.Length);
            foreach (byte b in buffer)
                lastReceivedData = b;
            receiveSignal.Set();
        }

        void AfterConnect()
        {
            serialPort?.Write(sewooProtocol.InitializePrinter());
            //serialPort?.Write(sewooProtocol.SelectHRICharacter(SewooProtocol.HRIPrintPositionEnum.BelowTheBarcode));
            //serialPort?.Write(sewooProtocol.SetKoreanCharset());
            serialPort?.Write(sewooProtocol.SelectJustification(BasicProtocol.Justification.Left));
            SetPrinterMode(PrinterModeEnum.StandardMode);

            /*
            serialPort?.Write(sewooProtocol.TransmitRealtimeStatus( RealTimeStatusCodeEnum.PrinterStatus));
            bool received = receiveSignal.WaitOne(100);
            byte printerStatus = received ? lastReceivedData : (byte)0;

            serialPort?.Write(sewooProtocol.TransmitRealtimeStatus(RealTimeStatusCodeEnum.OfflineStatus));
            received = receiveSignal.WaitOne(100);
            byte offlineStatus = received ? lastReceivedData : (byte)0;

            serialPort?.Write(sewooProtocol.TransmitRealtimeStatus(RealTimeStatusCodeEnum.ErrorStatus));
            received = receiveSignal.WaitOne(100);
            byte errorStatus = received ? lastReceivedData : (byte)0;

            var printStatus = received ? sewooProtocol.ParsePrinterStatus(printerStatus, offlineStatus, errorStatus) : null;
            log.LogInformation("프린터 상태 :: " + printStatus);
            */
        }

        public enum PrinterModeEnum 
        {
            StandardMode,
            PageMode
        }

        void SetPrinterMode(PrinterModeEnum printerMode)
        {
            Debug.Assert(printerMode == PrinterModeEnum.StandardMode || printerMode == PrinterModeEnum.PageMode);

            if(printerMode == PrinterModeEnum.StandardMode)
            {
                serialPort?.Write(sewooProtocol.SelectStandardMode());
            }
            else if(printerMode == PrinterModeEnum.PageMode)
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
                log.LogError(e, "다운로드 이미지 등록 중 오류");
            }
            return downloadOk;
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
            serialPort?.Write(TextEncoder.GetBytes(text));
            serialPort?.Write(new byte[] { 0x0d, 0x0a });
            return true;
        }

        public bool CutPaper()
        {
            serialPort?.Write(sewooProtocol.SelectCutModeAndCutPaper(CutModeEnum.PartialCut));
            return true;
        }

        public void FeedLines(int lines)
        {
            serialPort.Write(sewooProtocol.FeedLines(lines));
        }
        public void FeedPaper(int feed)
        {
            serialPort?.Write(sewooProtocol.SelectCutModeAndCutPaper(CutModeEnum.FeedsPaper, feed));            
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
        public bool PrintBarcode(string data)
        {
            serialPort?.Write(sewooProtocol.PrintBarcode(BasicProtocol.BarcodeSystemEnum.CODE128, data));
            return true;
        }
        public bool PrintBarcodeWithHeight(string data)
        {
            serialPort?.Write(sewooProtocol.SetBarcodeHeight(81));
            serialPort?.Write(sewooProtocol.PrintBarcode(BasicProtocol.BarcodeSystemEnum.CODE128, data));
            return true;
        }
        public bool PrintBarcodeWithHeight(string data, int height)
        {
            serialPort?.Write(sewooProtocol.SetBarcodeHeight(height));
            serialPort?.Write(sewooProtocol.PrintBarcode(BasicProtocol.BarcodeSystemEnum.CODE128, data));
            return true;
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
        public void SetCharacterSize(int width, int height)
        {
            var data = sewooProtocol.SelectCharacterSize(width, height);
            serialPort?.Write(data);
        }

        public void SetPrintHRI(BasicProtocol.HRIPrintPositionEnum position = HRIPrintPositionEnum.BelowTheBarcode)
        {
            serialPort?.Write(sewooProtocol.SelectHRICharacter(position));
        }
    }
}

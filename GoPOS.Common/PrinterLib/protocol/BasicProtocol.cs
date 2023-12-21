using CommunityToolkit.Diagnostics;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using GoShared.DeviceLib.Printer.Models;
using GoShared;
using GoShared.DeviceLib;
using static GoPOS.Common.PrinterLib.GeneralPrinter;
using System.IO.Ports;

namespace GoPOS.Common.PrinterLib.protocol
{
    /// <summary>
    /// 세우 SLK-TS100 기준 프로토콜
    /// </summary>
    public class BasicProtocol
    {
        public enum RealTimeStatusCodeEnum
        {
            PrinterStatus,
            OfflineStatus,
            ErrorStatus,
            PaperRollSensorStatus
        }
        /// <summary>
        /// transmit real-time status
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        public byte[] TransmitRealtimeStatus(RealTimeStatusCodeEnum statusCode)
        {
            int n = statusCode == RealTimeStatusCodeEnum.PrinterStatus ? 1 :
                    statusCode == RealTimeStatusCodeEnum.OfflineStatus ? 2 :
                    statusCode == RealTimeStatusCodeEnum.ErrorStatus ? 3 : 4;

            var packet = new byte[] { ASCIITable.DLE, ASCIITable.EOT, (byte)n };
            return packet;
        }

        /// <summary>
        /// Transmits paper sensor status 
        /// </summary>
        /// <returns></returns>
        public byte[] TransmitPaperStatus()
        {
            var packet = new byte[] { ASCIITable.GS, 0x72, 0x00 };
            return packet;
        }

        /// <summary>
        /// Print data in page mode
        ///  In page mode, prints all buffered data in the printing area collectively.
        ///  This command is enabled only in page mode. 
        ///  After printing, the printer does not clear the buffered data, setting values for ESC T and ESC W, and the position for buffering character data.
        /// </summary>
        /// <returns></returns>
        public byte[] PrintDataInPageMode()
        {
            byte[] packet = new byte[] { ASCIITable.ESC, 0x0C };
            return packet;
        }

        public enum UnderlineModeEnum
        {
            None,
            OneDotUnderline,
            TwoDotUnderline
        }
        public byte[] TurnUnderlineMode(UnderlineModeEnum mode)
        {
            return new byte[] { ASCIITable.ESC,
                                mode == UnderlineModeEnum.OneDotUnderline ? (byte)1 :
                                mode == UnderlineModeEnum.TwoDotUnderline ? (byte)2 : (byte)0 };
        }

        /// <summary>
        /// Clears the data in the print buffer and resets the printer mode to the mode that was in effect when the power was turned on.
        /// ESC @
        /// </summary>
        /// <returns></returns>
        public byte[] InitializePrinter()
        {
            return new byte[] { ASCIITable.ESC, 0x40 };
        }

        /// <summary>
        /// Select page mode. Switches from standard mode to page mode. 
        /// This command is enabled only when processed at the beginning of a line in standard mode.
        /// ESC L 
        /// </summary>
        /// <returns></returns>
        public byte[] SelectPageMode()
        {
            var packet = new byte[] { ASCIITable.ESC, 0x4C };
            return packet;
        }

        /// <summary>
        /// Switches from page mode to standard mode. This command is effective only in page mod
        ///  Data buffered in page mode are cleared. This command sets the print position to the beginning of the line
        ///  ESC S
        /// </summary>
        /// <returns></returns>
        public byte[] SelectStandardMode()
        {
            var packet = new byte[] { ASCIITable.ESC, 0x53 };
            return packet;
        }

        public enum PrintDirectionEnum
        {
            LeftToRight_FromUpperLeft,
            BottomToUp_LowerLeft,
            RightToLeft_LowerRight,
            TopToBottom_UpperRight
        }
        /// <summary>
        /// Select print direction in page mode.  
        /// Selects the print direction and starting position in page mode. n specifies the print direction and starting position as follows:
        /// When the command is input in standard mode, the printer executes only internal flag operation. This command does not affect printing in standard mode
        /// </summary>
        /// <returns></returns>
        public byte[] SelectPrintDirection(PrintDirectionEnum printDirection)
        {
            return new byte[]
            {
                ASCIITable.ESC,
                printDirection == PrintDirectionEnum.LeftToRight_FromUpperLeft ? (byte)0 :
                printDirection == PrintDirectionEnum.BottomToUp_LowerLeft ? (byte)1 :
                printDirection == PrintDirectionEnum.RightToLeft_LowerRight ? (byte)2 :
                printDirection == PrintDirectionEnum.TopToBottom_UpperRight ? (byte)3 : (byte)0
            };
        }

        public enum Justification
        {
            Left,
            Center,
            Right
        }

        /// <summary>
        ///  Select justification
        ///  Aligns all the data in one line to the specified position n selects the justification as follows
        /// </summary>
        /// <param name="justification"></param>
        /// <returns></returns>
        public byte[] SelectJustification(Justification justification)
        {
            return new byte[] { ASCIITable.ESC, 0x61, (byte)( justification == Justification.Right ? 2 :
                                                        justification == Justification.Center ? 1: 0)};
        }

        public byte[] FeedLines(int lines)
        {
            Guard.IsInRange(lines, 0, 255);
            return new byte[]
            {
                ASCIITable.ESC,
                0x64,
                (byte)lines
            };
        }

        public enum ImagePrintMode
        {
            Normal,
            DoublWidth,
            DoubleHeight,
            Quadruple
        }
        readonly Dictionary<ImagePrintMode, byte> _ImagePrintModeMap = new Dictionary<ImagePrintMode, byte>()
        {
            { ImagePrintMode.Normal, 0 },
            { ImagePrintMode.DoublWidth, 1 },
            { ImagePrintMode.DoubleHeight, 2 },
            { ImagePrintMode.Quadruple, 3 }
        };

        /// <summary>
        /// Print NV bit image
        /// </summary>
        /// <param name="imageNo">is the number of the NV bit image (defined using the FS q command).</param>
        /// <param name="printMode">specifies the bit image mode</param>
        /// <returns></returns>
        public byte[] PrintNvImage(int imageNo, ImagePrintMode printMode)
        {
            Guard.IsInRange(imageNo, 0, 255);

            return new byte[]
            {
                ASCIITable.FS,
                0x70,
                (byte)imageNo,
                (byte) _ImagePrintModeMap[printMode]
            };
        }

        /// <summary>
        /// Define NV bit image
        /// </summary>
        /// <param name="imageNo"></param>
        /// <param name="src"></param>
        /// <returns></returns>
        public byte[] DefineNvImage(int imageNo, Bitmap src)
        {
            Guard.IsInRange(imageNo, 1, 255);

            int widthBytes = (int)Math.Ceiling(src.Width / (double)8);
            int heightBytes = (int)Math.Ceiling(src.Height / (double)8);

            BitSerializer img = new BitSerializer(src.Width * heightBytes);
            for (int col = 0; col < src.Width; col++)
            {
                for (int row = 0; row < src.Height; row++)
                {
                    Color pixelColor = src.GetPixel(col, row);
                    bool validColor = pixelColor.R == 0 && pixelColor.G == 0 && pixelColor.B == 0;
                    img.Enqueue(validColor);
                }
                img.NextByte();
            }

            List<byte> result = new List<byte>();
            result.Add(ASCIITable.FS);
            result.Add(0x71);
            result.Add((byte)imageNo);
            result.Add((byte)widthBytes);
            result.Add(0);
            result.Add((byte)heightBytes);
            result.Add(0);
            result.AddRange(img.Retrieve());
            result.Add(0);
            result.Add(0);
            result.Add(0);

            return result.ToArray();
        }

        /// <summary>
        /// Select character size
        /// </summary>
        /// <param name="widthSize">0 : 기본, 1:2배, 2:3배.. 7:8배</param>
        /// <param name="heightSize">0 : 기본, 1:2배, 2:3배.. 7:8배</param>
        /// <returns></returns>
        public byte[] SelectCharacterSize(int widthSize, int heightSize)
        {
            Guard.IsBetweenOrEqualTo<int>(widthSize, 0, 7, "가로방향 크기");
            Guard.IsBetweenOrEqualTo<int>(heightSize, 0, 7, "세로방향 크기");

            var packet = new List<byte>();
            packet.Add(ASCIITable.GS);
            packet.Add(0x21);

            byte value = 0;
            value |= (byte)(widthSize & 0xF);
            value <<= 4;
            value |= (byte)(heightSize & 0xF);
            packet.Add(value);

            return packet.ToArray();
        }

        /// <summary>
        /// Define downloaded bit image.
        /// The downloaded bit image definition is cleared when 
        /// ESC @, ESC & FS q, or Printer is reset or the power is turned off.
        /// </summary>
        /// <param name="src"></param>
        /// <returns></returns>
        public byte[] DefineDownloadBitImage(Bitmap src)
        {
            Guard.IsBetweenOrEqualTo(src.Width, 1, 255);
            Guard.IsBetweenOrEqualTo(src.Height, 1, 48);
            Guard.IsBetweenOrEqualTo(src.Width * src.Height, 1, 912);

            int widthBytes = src.Width;
            int heightBytes = (int)Math.Ceiling(src.Height / (double)8);

            BitSerializer img = new BitSerializer(widthBytes * heightBytes);
            for (int col = 0; col < src.Width; col++)
            {
                for (int row = 0; row < src.Height; row++)
                {
                    Color pixelColor = src.GetPixel(col, row);
                    img.Enqueue(pixelColor != Color.White);
                }
                img.NextByte();
            }

            var result = new List<byte>();
            result.Add(ASCIITable.GS);
            result.Add(0x2A);
            result.Add((byte)src.Width);
            result.Add((byte)src.Height);
            result.AddRange(img.Retrieve());
            return result.ToArray();
        }

        /// <summary>
        ///  Print downloaded bit image
        /// </summary>
        /// <param name="mode"></param>
        /// <returns></returns>
        public byte[] PrintDownloadBitImage(ImagePrintMode mode)
        {
            return new byte[]
            {
                ASCIITable.GS,
                0x2F,
                _ImagePrintModeMap[mode]
            };
        }

        /// <summary>
        /// Select bar code height 
        /// </summary>
        /// <param name="barcodeHeight">Selects the height of the bar code. specifies the number of dots in the vertical direction. default value is 162 </param>
        /// <returns></returns>
        public byte[] SetBarcodeHeight(int barcodeHeight = 162)
        {
            Guard.IsBetweenOrEqualTo(barcodeHeight, 1, 255);
            return new byte[]
            {
                ASCIITable.GS,
                0x68,
                (byte)barcodeHeight
            };
        }

        /// <summary>
        /// Set horizontal and vertical motion units
        /// </summary>
        /// <param name="horizMotionUnit">기본값 180</param>
        /// <param name="verticalMotionUnit">기본값 360</param>
        /// <returns></returns>
        public byte[] SetMotionUnits(int horizMotionUnit = 0, int verticalMotionUnit = 0)
        {
            Guard.IsBetweenOrEqualTo(horizMotionUnit, 0, 255);
            Guard.IsBetweenOrEqualTo(verticalMotionUnit, 0, 255);

            return new byte[]
            {
                ASCIITable.GS,
                0x50,
                (byte)horizMotionUnit,
                (byte)verticalMotionUnit
            };
        }

        public enum BarcodeSystemEnum
        {
            UPC_A,
            UPC_E,
            JAN13,
            JAN8,
            CODE39,
            ITF,
            CODABAR,
            CODE93,
            CODE128
        };
        Dictionary<BarcodeSystemEnum, byte> _barcodeSystemMap = new Dictionary<BarcodeSystemEnum, byte>() {
            {BarcodeSystemEnum.UPC_A, 65 },
            {BarcodeSystemEnum.UPC_E, 66 },
            {BarcodeSystemEnum.JAN13, 67 },
            {BarcodeSystemEnum.JAN8, 68 },
            {BarcodeSystemEnum.CODE39, 69 },
            {BarcodeSystemEnum.ITF, 70 },
            {BarcodeSystemEnum.CODABAR, 71 },
            {BarcodeSystemEnum.CODE93, 72 },
            {BarcodeSystemEnum.CODE128, 73 }
            };

        /// <summary>
        /// Print bar code 
        /// </summary>
        /// <param name="coding">사용할 바코드 인코딩</param>
        /// <param name="barcodeData">데이터</param>
        /// <returns></returns>
        public byte[] PrintBarcode(BarcodeSystemEnum coding, string barcodeData)
        {
            Guard.IsTrue(_barcodeSystemMap.ContainsKey(coding));
            Guard.IsBetweenOrEqualTo(barcodeData.Length, 0, 255);

            List<byte> data = new List<byte>();
            data.Add(ASCIITable.GS);
            data.Add(0x6B);
            data.Add(_barcodeSystemMap[coding]);
            data.Add((byte)barcodeData.Length);
            data.AddRange(Encoding.ASCII.GetBytes(barcodeData));
            return data.ToArray();
        }

        /// <summary>
        /// Select bar code height
        /// </summary>
        /// <param name="height"></param>
        /// <returns></returns>
        public byte[] SelectBarcodeHeight(int height)
        {
            Guard.IsBetweenOrEqualTo(height, 1, 255);
            return new byte[]
            {
                ASCIITable.GS,
                0x68,
                (byte)height
            };
        }

        /// <summary>
        /// Set bar code width.
        /// default width is 3 (0.5mm)
        /// </summary>
        /// <param name="width"></param>
        /// <returns></returns>
        public byte[] SelectBarcodeWidth(int width)
        {
            Guard.IsBetweenOrEqualTo(width, 2, 6);
            return new byte[]
            {
                ASCIITable.GS,
                0x77,
                (byte)width
            };
        }

        public enum HRIPrintPositionEnum
        {
            DoNotPrint,
            TopOfTheBarCode,
            BelowTheBarcode,
            BarcodeArePrintedBelow
        }
        /// <summary>
        /// Select printing position for HRI(Human Readable Interpretation) characters
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        public byte[] SelectHRICharacter(HRIPrintPositionEnum position)
        {
            return new byte[]
            {
                ASCIITable.GS,
                0x48,
                (byte)
                (
                position == HRIPrintPositionEnum.BarcodeArePrintedBelow ? 3 :
                position == HRIPrintPositionEnum.BelowTheBarcode ? 2 :
                position == HRIPrintPositionEnum.TopOfTheBarCode ? 1 : 0
                )
            };
        }

        /// <summary>
        /// Select an international character set
        /// </summary>
        /// <returns></returns>
        public byte[] SetKoreanCharset()
        {
            return new byte[] { ASCIITable.ESC, 0x52, 13 };
        }

        public byte[] PrintPageModeData()
        {
            return new byte[] { ASCIITable.ESC, 0x0C };
        }

        bool IsValid(Color pixelColor)
        {
            return pixelColor.R == 0 && pixelColor.G == 0 && pixelColor.B == 0;
        }

        /// <summary>
        /// Print raster bit image
        /// </summary>
        /// <param name="src"></param>
        /// <param name="mode"></param>
        /// <returns></returns>
        public byte[] PrintRasterImage(Bitmap src, ImagePrintMode mode)
        {
            int widthBytes = (int)Math.Ceiling(src.Width / (double)8);
            int heightBytes = (int)Math.Ceiling(src.Height / (double)8);

            BitSerializer img = new BitSerializer(widthBytes * src.Height);
            for (int row = 0; row < src.Height; row++)
            {
                for (int col = 0; col < src.Width; col++)
                {
                    Color pixelColor = src.GetPixel(col, row);
                    bool validColor = pixelColor.R == 0 && pixelColor.G == 0 && pixelColor.B == 0;
                    img.Enqueue(validColor);
                }
                img.NextByte();
            }

            var rawImage = img.Retrieve();
            Debug.Assert(rawImage.Length == widthBytes * src.Height);

            var result = new List<byte>();
            result.Add(ASCIITable.GS);
            result.Add(0x76);
            result.Add(0x30);
            result.Add(_ImagePrintModeMap[mode]);
            result.Add((byte)(widthBytes % 256));
            result.Add((byte)(widthBytes / 256));
            result.Add((byte)(src.Height % 256));
            result.Add((byte)(src.Height / 256));
            result.AddRange(rawImage);
            return result.ToArray();
        }

        public byte[] LineFeed(int lines)
        {
            List<byte> arr = new List<byte>();
            for (int i = 0; i < lines; i++)
                arr[i] = 0x0A;
            return arr.ToArray();
        }

        public enum CutModeEnum
        {
            OnePointLeftUncut,
            PartialCut,
            FeedsPaper
        }
        public byte[] SelectCutModeAndCutPaper(CutModeEnum cutMode, int verticalFeed = 0)
        {
            var raw = new List<byte>();
            raw.Add(ASCIITable.GS);
            raw.Add(0x56);
            switch (cutMode)
            {
                case CutModeEnum.OnePointLeftUncut:
                    raw.Add(0x00);
                    break;
                case CutModeEnum.PartialCut:
                    raw.Add(0x01);
                    break;
                case CutModeEnum.FeedsPaper:
                    Guard.IsBetweenOrEqualTo(verticalFeed, 0, 255);
                    raw.Add(0x66);
                    raw.Add((byte)verticalFeed);
                    break;
                default:
                    ThrowHelper.ThrowArgumentException("범위가 잘못 됨");
                    break;
            }
            return raw.ToArray();
        }

        public class PrintStatus
        {
            public bool isCashDrawerOpened { get; set; }
            public bool isOnline { get; set; }
            public bool isCoverOpened { get; set; }
            public bool isPaperIsBeingFedByFEEDButton { get; set; }

            public bool isAutoCutterError { get; set; }
            public bool isUnRecoverableError { get; set; }

        }
        public PrintStatus ParsePrinterStatus(byte printerStatusValue, byte offlineStatus, byte errorStatus)
        {
            // Bit 0, Bit 7 은 항상 0
            // Bit 1 은 항상 1
            bool fixedValueCheck1 = ((printerStatusValue & 0x01) == 0x00)
                                    && ((printerStatusValue & 0x02) == 0x02)
                                    && ((offlineStatus & 0x10) == 0x10)
                                    && ((printerStatusValue & 0x80) == 0x00);

            bool fixedValueCheck2 = ((offlineStatus & 0x01) == 0x00)
                                    && ((offlineStatus & 0x02) == 0x2)
                                    && ((offlineStatus & 0x10) == 0x10)
                                    && ((offlineStatus & 0x80) == 0x00);

            bool fixedValueCheck3 = ((errorStatus & 0x01) == 0x00)
                        && ((errorStatus & 0x02) == 0x2)
                        && ((errorStatus & 0x10) == 0x10)
                        && ((errorStatus & 0x80) == 0x00);

            PrintStatus val = new PrintStatus();
            if (fixedValueCheck1 && fixedValueCheck2 && fixedValueCheck3)
            {
                //PrintStatus val = new PrintStatus();
                val.isCashDrawerOpened = (printerStatusValue & 0x04) == 0x00;
                val.isOnline = (printerStatusValue & 0x08) == 0x00;
                val.isCoverOpened = (offlineStatus & 0x04) == 0x04;
                val.isPaperIsBeingFedByFEEDButton = (offlineStatus & 0x08) == 0x08;
                val.isAutoCutterError = (errorStatus & 0x08) == 0x08;
                val.isUnRecoverableError = (errorStatus & 0x20) == 0x20;

                return val;
            }
            else
            {
                return new PrintStatus()
                {
                    isOnline = true
                };
            }
        }
    }
}

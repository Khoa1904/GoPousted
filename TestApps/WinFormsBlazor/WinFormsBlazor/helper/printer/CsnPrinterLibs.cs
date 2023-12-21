using System;
using System.Runtime.InteropServices;

namespace CsnPrinterLibs
{
	public class CsnPrinterLibs
	{
		[DllImport("Dll/CsnPrinterLibs.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
		public static extern uint Port_EnumCOM(byte[] buffer, int length);
		[DllImport("Dll/CsnPrinterLibs.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
		public static extern uint Port_EnumUSB(byte[] buffer, int length);
		[DllImport("Dll/CsnPrinterLibs.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
		public static extern uint Port_EnumLPT(byte[] buffer, int length);
		[DllImport("Dll/CsnPrinterLibs.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
		public static extern uint Port_EnumPRN(byte[] buffer, int length);
		[DllImport("Dll/CsnPrinterLibs.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
		public static extern int Port_OpenCOMIO(string name, int baudrate = 9600, int flowcontrol = 0, int parity = 0, int databis = 8, int stopbits = 0);
		[DllImport("Dll/CsnPrinterLibs.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
		public static extern int Port_OpenUSBIO(string name);
		[DllImport("Dll/CsnPrinterLibs.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
		public static extern int Port_OpenLPTIO(string name);
		[DllImport("Dll/CsnPrinterLibs.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
		public static extern int Port_OpenPRNIO(string name);
		[DllImport("Dll/CsnPrinterLibs.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
		public static extern int Port_OpenTCPIO(string name, int port);
		[DllImport("Dll/CsnPrinterLibs.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
		public static extern bool Port_SetPort(int handle);
		[DllImport("Dll/CsnPrinterLibs.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
		public static extern void Port_ClosePort(int handle);

		[DllImport("Dll/CsnPrinterLibs.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
		public static extern bool Pos_Reset();
		[DllImport("Dll/CsnPrinterLibs.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
		public static extern bool Pos_SelfTest();
		[DllImport("Dll/CsnPrinterLibs.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
		public static extern bool Pos_FeedLine();
		[DllImport("Dll/CsnPrinterLibs.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
		public static extern bool Pos_FeedHot(int n);
		[DllImport("Dll/CsnPrinterLibs.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
		public static extern bool Pos_Feed_N_Line(int n);
		[DllImport("Dll/CsnPrinterLibs.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
		public static extern bool Pos_FeedNextLable();
		[DllImport("Dll/CsnPrinterLibs.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
		public static extern bool Pos_BlackMark();
		[DllImport("Dll/CsnPrinterLibs.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
		public static extern bool Pos_Align(int value);
		[DllImport("Dll/CsnPrinterLibs.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
		public static extern bool Pos_SetLineHeight(int value);
		[DllImport("Dll/CsnPrinterLibs.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
		public static extern bool Pos_Text(byte[] prnText, int nLan, int nOrgx, int nWidthTimes, int nHeightTimes, int FontType, int nFontStyle);
		[DllImport("Dll/CsnPrinterLibs.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
		public static extern bool Pos_Beep(byte nCount, byte nMillis);
		[DllImport("Dll/CsnPrinterLibs.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
		public static extern bool Pos_KiskOutDrawer(int nId, int nHightTime = 20, int nLowTime = 60);
		[DllImport("Dll/CsnPrinterLibs.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
		public static extern bool Pos_FullCutPaper();
		[DllImport("Dll/CsnPrinterLibs.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
		public static extern bool Pos_HalfCutPaper();
		[DllImport("Dll/CsnPrinterLibs.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
		public static extern bool Pos_Barcode(string BarcodeData, int nBarcodeType, int nOrgx, int nUnitWidth, int nUnitHeight, int nFontStyle, int FontPosition);
		[DllImport("Dll/CsnPrinterLibs.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
		public static extern bool Pos_Qrcode(byte[] QrcodeData, int nWidth = 2, int nVersion = 0, int nErrlevenl = 4);
		[DllImport("Dll/CsnPrinterLibs.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
		public static extern bool Pos_EscQrcode(byte[] QrcodeData, int nWidth = 4, int nErrlevenl = 4);
		[DllImport("Dll/CsnPrinterLibs.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
		public static extern bool Pos_DoubleQrcode(byte[] QrcodeData1,int QR1Position,int QR1Version, int QR1Ecc, byte[] QrcodeData2, int QR2Position, int QR2Version, int QR2Ecc, int ModuleSize);
		[DllImport("Dll/CsnPrinterLibs.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
		public static extern bool Pos_ImagePrint(byte[] FileName, int nWidth = 384, int nBinaryAlgorithm = 0);
		[DllImport("Dll/CsnPrinterLibs.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
		public static extern bool Pos_PrintNVLogo(ushort nLogo, ushort nWidth = 0);
		[DllImport("Dll/CsnPrinterLibs.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
		public static extern int Pos_QueryPrinterErr(uint nTimeout = 3000);
		[DllImport("Dll/CsnPrinterLibs.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
		public static extern bool Pos_QueryStstus(byte[] rBuffer, int type, uint nTimeout);
		[DllImport("Dll/CsnPrinterLibs.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
		public static extern bool Pos_SetPrinterBaudrate(int nBaudrate);
		[DllImport("Dll/CsnPrinterLibs.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
		public static extern bool Pos_SetPrinterBasic(int nFontStyle, int nDensity, int nLine, int nBeep, int nCut);
		[DllImport("Dll/CsnPrinterLibs.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
		public static extern bool Page_SelectPageMode();
		[DllImport("Dll/CsnPrinterLibs.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
		public static extern bool Page_PrintPage();
		[DllImport("Dll/CsnPrinterLibs.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
		public static extern bool Page_ExitPageMode();
		[DllImport("Dll/CsnPrinterLibs.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
		public static extern bool Page_SetVerticalAbsolutePrintPosition(ushort nPosition);
		[DllImport("Dll/CsnPrinterLibs.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
		public static extern bool Page_SetHorizontalAbsolutePrintPosition(ushort nPosition);
		[DllImport("Dll/CsnPrinterLibs.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
		public static extern bool Page_SetVerticalRelativePrintPosition(ushort nPosition);
		[DllImport("Dll/CsnPrinterLibs.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
		public static extern bool Page_SetHorizontalRelativePrintPosition(ushort nPosition);
		[DllImport("Dll/CsnPrinterLibs.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
		public static extern bool Page_SetPageArea(ushort x, ushort y, ushort w, ushort h);
		[DllImport("Dll/CsnPrinterLibs.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
		public static extern bool Page_SetPageModeDrawDirection(ushort nPosition);
		[DllImport("Dll/CsnPrinterLibs.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
		public static extern bool Page_Text(byte[] prnText, int nLan, int nOrgx, int nWidthTimes, int nHeightTimes, int FontType, int nFontStyle);
		[DllImport("Dll/CsnPrinterLibs.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
		public static extern bool Page_Barcode(string BarcodeData, int nBarcodeType, int nOrgx, int nUnitWidth, int nUnitHeight, int nFontStyle, int FontPosition);
		[DllImport("Dll/CsnPrinterLibs.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
		public static extern bool Page_Qrcode(byte[] QrcodeData, int nWidth = 4, int nErrlevenl = 4);
		[DllImport("Dll/CsnPrinterLibs.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
		public static extern bool Page_ImagePrint(byte[] FileName, int nWidth = 384, int nBinaryAlgorithm = 0);

	}
}
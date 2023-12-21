using System.Reflection.Metadata;
using System.Text;
using WinFormsBlazor.Model;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;


namespace WinFormsBlazor.Service
{
	public class PrintService
	{
		public static int handle;

		public static void OpenPort()
		{
			handle = (int)CsnPrinterLibs.CsnPrinterLibs.Port_OpenCOMIO("COM4", int.Parse("9600"), 0);

			if (handle != 0)
			{
				CsnPrinterLibs.CsnPrinterLibs.Port_SetPort(handle);
				// Initopen();
				return;
			}
			else
			{
				MessageBox.Show("Open failed, Please confirm if the port is occupied");
				return;
			}
		}

		public static void Print()
		{
			CsnPrinterLibs.CsnPrinterLibs.Pos_Reset();
			CsnPrinterLibs.CsnPrinterLibs.Pos_Text(Encoding.Unicode.GetBytes("Self Test\n"), 1, -2, 1, 1, 0, 0);
			CsnPrinterLibs.CsnPrinterLibs.Pos_Text(Encoding.Unicode.GetBytes("测试小票\n"), 0, -2, 1, 1, 0, 0);
			CsnPrinterLibs.CsnPrinterLibs.Pos_Text(Encoding.Unicode.GetBytes("測試小票\n"), 3, -2, 1, 1, 0, 0);
			CsnPrinterLibs.CsnPrinterLibs.Pos_Text(Encoding.Unicode.GetBytes("テストシート\n"), 4, -2, 1, 1, 0, 0);
			CsnPrinterLibs.CsnPrinterLibs.Pos_Text(Encoding.Unicode.GetBytes("셀프 테스트\n"), 5, -2, 1, 1, 0, 0);
			CsnPrinterLibs.CsnPrinterLibs.Pos_FeedLine();
			CsnPrinterLibs.CsnPrinterLibs.Pos_Text(Encoding.Unicode.GetBytes("Test BarCode\n"), 1, -2, 0, 0, 0, 0);
			CsnPrinterLibs.CsnPrinterLibs.Pos_Barcode("123456", 0x45, -2, 3, 96, 0, 2);
			CsnPrinterLibs.CsnPrinterLibs.Pos_FeedLine();
			CsnPrinterLibs.CsnPrinterLibs.Pos_Text(Encoding.Unicode.GetBytes("Test QrCode\n"), 1, -2, 0, 0, 0, 0);
			CsnPrinterLibs.CsnPrinterLibs.Pos_Reset();
			CsnPrinterLibs.CsnPrinterLibs.Pos_Align(1);
			CsnPrinterLibs.CsnPrinterLibs.Pos_Qrcode(Encoding.Unicode.GetBytes("welcome to use\n"), 1, 9);
			CsnPrinterLibs.CsnPrinterLibs.Pos_Feed_N_Line(5);
			CsnPrinterLibs.CsnPrinterLibs.Pos_FullCutPaper();
			CsnPrinterLibs.CsnPrinterLibs.Pos_Reset();
		}
	}
}

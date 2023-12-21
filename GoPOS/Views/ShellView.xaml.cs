using Caliburn.Micro;
using GoPOS.Common;
using GoPOS.Common.Helpers;
using GoPOS.Common.Views;
using GoPOS.Helpers;
using GoPOS.Models.Common;
using GoPOS.SerialPacketProcess;
using GoPOS.Servers;
using GoPOS.ViewModels;
using GoShared.Helpers;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Interop;

namespace GoPOS.Views
{
    public partial class ShellView : Window, IShellView
    {
        private readonly IEventAggregator eventAggregator;
        private SynchronizationContext _context;

        public SynchronizationContext SynContext => _context;

        public ShellView(IEventAggregator eventAggregator)
        {
            _context = SynchronizationContext.Current;
            InitializeComponent();            
            InitPOSInstance();

            this.KeyDown += ShellView_KeyDown;
            this.eventAggregator = eventAggregator;
        }

        private void ShellView_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter)
            {
                BaseKeyPad.InvokeKey(eventAggregator, Key.Enter);
                SimKeyboard.Press(System.Windows.Input.Key.Tab);
            }
        }

        private void InitPOSInstance()
        {
            if (System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.Width > 1024)
            {
                WindowStartupLocation = WindowStartupLocation.Manual;
                var firstScreen = System.Windows.Forms.Screen.PrimaryScreen.Bounds;
                this.Top = (firstScreen.Height - SystemHelper.MainHeight) / 2;
                this.Left = firstScreen.Left + (firstScreen.Width - SystemHelper.MainWidth) / 2;
            }
            else
            {
                this.WindowState = WindowState.Maximized;
                this.WindowStyle = WindowStyle.None;
            }

            try
            {
                Process[] p = Process.GetProcessesByName(Process.GetCurrentProcess().ProcessName.ToUpper());
                if (p.Length > 1)
                {
                    var cp = Process.GetCurrentProcess();
                    string runningFileName = p[0].MainModule.FileName.ToLower();
                    string myFileName = Process.GetCurrentProcess().MainModule.FileName.ToLower();

                    if (runningFileName.Equals(myFileName) && p[0].Handle != cp.Handle)
                    {
                        LogHelper.Logger.Info("##### 프로그램 중복 실행 시도 - 프로그램 최상위로 가져오기 #####");
                        Close();

                        WindowsHelper.BringToFront(Title);
                        return;
                    }
                }

                LogHelper.Logger.Info("***** 프로그램 시작 *****");
            }
            catch (Exception ex)
            {
                LogHelper.Logger.Error("프로그램 시작 오류 : " + ex.Message);
            }
        }

        protected override async void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);

            try
            {
                await CloseMainWindow();
            }
            catch (Exception ex)
            {
                LogHelper.Logger.Error("ShellView Closing Exception : " + ex.Message);
            }
        }

        public static async Task CloseMainWindow()
        {
            // 서버, 시리얼, 서비스 해제
            //SerialPortManager.Go.Close();
            //SerialPortWriter.Go.Close();
            //PacketReceive.Go.PacketReceiveThreadStop();
            //PacketPrinter.Go.PacketPrinterThreadStop();
            WebApiServer.Go.StopWebApiServer();
            IoC.Get<ShellViewModel>().Items.Clear();
            LogHelper.Logger.Info("====== 프로그램 종료 =====");
            LogHelper.ShutdownLogManager();

            for (var windowsCount = System.Windows.Application.Current.Windows.Count - 1; windowsCount > 0; windowsCount--)
            {
                System.Windows.Application.Current.Windows[windowsCount]?.Close();
            }
        }

        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);
            HwndSource? source = PresentationSource.FromVisual(this) as HwndSource;
            source?.AddHook(WndProc);
        }

        private IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            try
            {
                switch (msg)
                {
                    case 0x0011: // WM_QUERYENDSESSION
                        LogHelper.Logger.Info("====== Windows 종료에 따른 프로그램 강제 종료 =====");
                        Close();
                        handled = true;
                        break;
                }

                return IntPtr.Zero;
            }
            catch (Exception ex)
            {
                LogHelper.Logger.Error("WndProc Error : " + ex.Message);
                return IntPtr.Zero;
            }
        }

        protected override void OnStateChanged(EventArgs e)
        {
            base.OnStateChanged(e);
            MaxMinScreen();
        }

        private void MaxMinScreen()
        {

            if (WindowState == WindowState.Maximized)
            {
                BorderThickness = new Thickness(8, 8, 8, 8);
            }
            else
            {
                BorderThickness = new Thickness(0, 0, 0, 0);
            }
        }
    }
}

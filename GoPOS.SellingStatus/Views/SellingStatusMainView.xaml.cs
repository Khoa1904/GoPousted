using Caliburn.Micro;
using CoreWCF.Runtime;
using GoPOS.Common.Views;
using GoPOS.Helpers;
using GoPOS.Interface;
using GoPOS.SerialPacketProcess;
using GoPOS.Servers;
using GoPOS.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace GoPOS.Views
{
    /// <summary>
    /// SellingStatusMainView.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class SellingStatusMainView : UCViewBase, ISellingStatusMainView
    {
        public SellingStatusMainView()
        {
            InitializeComponent();

        }


        public static async Task CloseMainWindow()
        {

            //LogHelper.Logger.Info("====== 프로그램 종료 =====");
            //LogHelper.ShutdownLogManager();

            await Task.Delay(1);

            //for (var windowsCount = Application.Current.Windows.Count - 1; windowsCount > 0; windowsCount--)
            //{
            //    Application.Current.Windows[windowsCount]?.Close();
            //}
        }

        public void DisableElements(string childActivatedTypes, bool activated)
        {
            throw new NotImplementedException();
        }
    }
}

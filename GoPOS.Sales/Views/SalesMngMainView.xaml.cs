using Caliburn.Micro;
using CoreWCF.Runtime;
using GoPOS.Common.Views;
using GoPOS.Helpers;
using GoPOS.Models;
using GoPOS.SalesMng.Interface.View;
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
    /// FormTest.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class SalesMngMainView : UCViewBase, ISalesMngMainView
    {
        public SalesMngMainView()
        {
            InitializeComponent();
        }

    }
}

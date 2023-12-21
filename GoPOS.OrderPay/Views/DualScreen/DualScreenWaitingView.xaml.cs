using Caliburn.Micro;
using GoPOS.Common.Interface.View;
using GoPOS.Common.Views;
using GoPOS.ViewModels;
using GoPOS.ViewModels.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GoPOS.Views
{
    /// <summary>
    /// MainView.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class DualScreenWaitingView : UCViewBase
    {
        public DualScreenWaitingView()
        {
            InitializeComponent();

            this.Loaded += View_Loaded;
        }

        private void View_Loaded(object sender, RoutedEventArgs e)
        {
        }

        public void Dispose()
        {
        }

    }
}

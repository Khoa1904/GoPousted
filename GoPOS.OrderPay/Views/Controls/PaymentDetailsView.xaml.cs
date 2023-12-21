using Caliburn.Micro;
using GoPOS.Common.Views;
using GoPOS.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

namespace GoPOS.OrderPay.Views.Controls
{
    /// <summary>
    /// Interaction logic for PaymentInfoView.xaml
    /// </summary>
    public partial class PaymentDetailsView : UCViewBase
    {
        public PaymentDetailsView()
        {
            InitializeComponent();
        }

        private void ListViewItem_PreviewMouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            ListViewItem item = sender as ListViewItem;
            if (item != null && item.IsSelected)
            {
                // do stuff
                IoC.Get<OrderPayMainViewModel>().ItemGrid_CheckShowSideMenu();
            }
        }
    }
}

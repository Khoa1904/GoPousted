using GoPOS.Common.Views;
using GoPOS.OrderPay.Interface.View;
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
using static System.Net.Mime.MediaTypeNames;

namespace GoPOS.Views
{
    /// <summary>
    /// Interaction logic for OrderpayReceiptDetails.xaml
    /// </summary>
    public partial class OrderPayReceiptReturnView : UCViewBase, IOrderPayReceiptReturnView
    {
        public OrderPayReceiptReturnView()
        {
            InitializeComponent();
        }

    }
}

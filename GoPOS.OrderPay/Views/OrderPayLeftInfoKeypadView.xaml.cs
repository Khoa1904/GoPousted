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

namespace GoPOS.Views
{
    /// <summary>
    /// Interaction logic for OrderPayLeftInfoKeypadView.xaml
    /// </summary>
    public partial class OrderPayLeftInfoKeypadView : UCViewBase, IOrderPayLeftInfoKeypadView
    {
        public OrderPayLeftInfoKeypadView()
        {
            InitializeComponent();
            KeyPad.Focus();
            GotFocus += OrderPayLeftInfoKeypadView_GotFocus;
        }

        public void SetKeyPadFocus()
        {
            KeyPad.Focus();            
        }

        private void OrderPayLeftInfoKeypadView_GotFocus(object sender, RoutedEventArgs e)
        {
            KeyPad.Focus();
            KeyPad.Text = string.Empty; 
        }       
        
    }
}

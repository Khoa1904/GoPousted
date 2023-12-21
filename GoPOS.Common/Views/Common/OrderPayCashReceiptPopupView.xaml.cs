using GoPOS.Common.Views;
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

    public partial class OrderPayCashReceiptPopupView : UserControl
    {
        public OrderPayCashReceiptPopupView()
        {
            InitializeComponent();
            this.Loaded += OrderPayCashReceiptPopupView_Loaded;
        }

        private void OrderPayCashReceiptPopupView_Loaded(object sender, RoutedEventArgs e)
        {
            txtInviNo.Focus();
        }

        private void Radiotic_Checkbox(object sender, RoutedEventArgs e)
        {
            // A radio-like checkbox
            if (sender is CheckBox clickedCheckbox)
            {
                foreach (var checkbox in new[] { check1, check2 })
                {
                    if (checkbox != clickedCheckbox)
                    {
                        checkbox.IsChecked = false;
                    }
                }
            }
        }

        private void NotNull_Checkbox(object sender, RoutedEventArgs e)
        {
            //Prevent both unchecked
            if (sender is CheckBox clickedCheckbox)
            {
                if (check1.IsChecked == false && check2.IsChecked == false)
                {
                    clickedCheckbox.IsChecked = true;
                }
            }
        }
    }

}

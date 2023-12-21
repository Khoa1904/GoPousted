using GoPOS.Common.Views;
using GoPOS.Payment.Interface.View;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace GoPOS.Views
{
    public partial class OrderPayCashView : UCViewBase, IOrderPayCashView
    {
        public OrderPayCashView()
        {
            InitializeComponent();
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

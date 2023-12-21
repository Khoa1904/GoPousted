using GoPOS.Common.Views;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
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
    /// SalesTmPrCheckView.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class SalesTmPrCheckView : UCViewBase
    {
        public SalesTmPrCheckView()
        {
            InitializeComponent();
        }
        private void PreviousMowDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is TextBox textBox)
            {
                textBox.SelectAll();
            }

        }

       private void selectAllFocus(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox textBox)
            {
                textBox.SelectAll();
            }
        }

        private void ZeroBehaviour(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox textBox && string.IsNullOrWhiteSpace(textBox.Text))
            {
                if (textBox.Text.IsNullOrEmpty())
                {
                    textBox.Text = "0";
                }

            }
        }
    }
}

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
using static GoPOS.Function;
using Caliburn.Micro;
using GoPOS.Common.Views;
using Microsoft.IdentityModel.Tokens;

namespace GoPOS.Views
{
    /// <summary>
    /// SalesResvMneyRegistView.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class SalesResvMneyRegistView : UCViewBase
    {
        public SalesResvMneyRegistView()
        {
            InitializeComponent();
        }

        private void txtPosReadyAmt_GotFocus(object sender, RoutedEventArgs e)
        {
            IoC.Get<SalesResvMneyRegistView>().txtPosReadyAmt.Text = CommaDelStr(IoC.Get<SalesResvMneyRegistView>().txtPosReadyAmt.Text);
        }

        //private void txtPosReadyAmt_LostFocus(object sender, RoutedEventArgs e)
        //{
        //    IoC.Get<SalesResvMneyRegistView>().txtPosReadyAmt.Text = Comma(IoC.Get<SalesResvMneyRegistView>().txtPosReadyAmt.Text);
        //}
        private void PreviousMowDown(object sender, MouseButtonEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (textBox.Text != null)
            {
                textBox.Text = "";
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

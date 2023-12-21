using GoPOS.Common.Views;
using GoPOS.SellingStatus.Interface;
using Microsoft.IdentityModel.Tokens;
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
using GoPOS.Common.Helpers.Controls;

namespace GoPOS.Views
{
    /// <summary>
    /// ExcclcSttusView.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class ExcclcSttusView : UCViewBase, IExcclcSttusView
    {
        public ExcclcSttusView()
        {
            InitializeComponent();
            Loaded += ExcClcView_Loaded;
        }
        private void ExcClcView_Loaded(object sender, RoutedEventArgs e)
        {
            txtCheckAmt.Focus();
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
        private void initZero(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox textBox)
            {
                if (string.IsNullOrEmpty(textBox.Text))
                {
                    textBox.Text = "0";
                }
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
        public Point buttonPosition { get; set; }
   
        private void ButtonClick(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            buttonPosition = PointHelper.GetPointOfButton(btn,this);
        }
    }

}

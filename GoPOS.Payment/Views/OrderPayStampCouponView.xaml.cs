using Google.Protobuf.WellKnownTypes;
using GoPOS.Common.Views;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace GoPOS.Views
{
    public partial class OrderPayStampCouponView : UCViewBase
    {
        public decimal Min { get; set; } = 0;
        public decimal Max { get; set; } = 0;
        public OrderPayStampCouponView()
        {
            InitializeComponent();
            Loaded += init;
        }
        private void init(object sender, EventArgs e)
        {
            this.txtSearch_Cst_Tel.Focus();
            this.txtSearch_Cst_Name.Text = "";
            this.txtSearch_Cst_NO.Text = "";
            this.txtSearch_Cst_Tel.Text = "";
        }
        public void ValidationRule(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            string text = textBox.Text;

            // Remove any non-numeric characters
            textBox.Text = Regex.Replace(text, "[^0-9,]+", "");
        }
        private void NumberValidationTextBox(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            e.Handled = !IsTextAllowed(e.Text);
        }

        private bool IsTextAllowed(string text)
        {
            Regex regex = new Regex("[^0-9]+");
            return !regex.IsMatch(text);
        }
        private void DecimalessRule(object sender, TextChangedEventArgs e)
        {
            if (sender is TextBox textBox)
            {
                if (double.TryParse(textBox.Text, out double value))
                {
                    string formattedValue = value.ToString("0");
                    textBox.Text = formattedValue;
                }
            }
        }
        private void Value_Restrict(object sender, KeyEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            // Restrict to min max value
            if (e.Key == Key.Enter)
            {
                e.Handled = true; // Prevent a newline character from being added


                if (decimal.TryParse(textBox.Text, out decimal value))
                {
                    if (value > Max)
                    {
                        textBox.Text = Max.ToString();
                    }
                    if (value < Min)
                    {
                        textBox.Text = Min.ToString();
                    }
                }
            }
        }

        private void keydown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter && sender is TextBox tb)
            {
                if (tb.Name == "txtSearch_Cst_Tel")
                {
                    this.btnsearch.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
                }
            }
        }
    }
}

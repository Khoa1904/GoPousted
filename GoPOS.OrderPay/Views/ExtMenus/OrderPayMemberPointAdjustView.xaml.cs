using Google.Protobuf.WellKnownTypes;
using GoPOS.Common.Views;
using GoPOS.OrderPay.Interface.View;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text.RegularExpressions;
using System.Windows.Controls;
using System.Windows.Input;

namespace GoPOS.Views
{
    public partial class OrderPayMemberPointAdjustView : UCViewBase, IOrderPayPointAdjustView
    {
        public CheckBox Chagam => this.CheckboxChagam;
        public OrderPayMemberPointAdjustView()
        {
            InitializeComponent();
            Loaded += init;
        }
        private void init(object sender, EventArgs e) 
        {
            this.txtSearch_Cst_Name.Text = string.Empty;
            this.txtSearch_Cst_NO.Text = string.Empty;
            this.txtSearch_Cst_Tel.Text = string.Empty;
            this.txtAMT.Text = string.Empty;
            if (!this.txtCst_Name.Text.Equals("")){
                this.txtAMT.Focus();
            } else {
                this.txtSearch_Cst_Tel.Focus();
            }
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
    }
}

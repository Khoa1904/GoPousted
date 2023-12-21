using GoPOS.Common.Views;
using GoPOS.Interface;
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

    /// </summary>
    public partial class POSinterimView : UCViewBase, IPOSOpeningView
    {
        public POSinterimView()
        {

            InitializeComponent();
        }
        private void NumberValidationRule(object sender, TextCompositionEventArgs e)
        {
            if (!IsTextAllowed(e.Text))
            {
                e.Handled = true; // Cancel the input.
            }
        }

        private bool IsTextAllowed(string text)
        {
            // Use a regular expression to check if the input contains only digits.
            System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex("^[0-9]+$");
            return regex.IsMatch(text);
        }
    }
}

using GoPOS.Common.Helpers.Controls;
using GoPOS.Common.Views;
using GoPOS.Sales.Interface.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Interaction logic for OrderPayPrepaymentView.xaml
    /// </summary>
    public partial class SalesPayPrepaymentView : UCViewBase, ISalesPayPrepaymentView
    {
        public SalesPayPrepaymentView()
        {
            InitializeComponent();
            Loaded += Screen_Load;
        }
        public Point ButtonPosition { get; set; }
        private void ButtonClick(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            ButtonPosition = PointHelper.GetPointOfButton(btn, this);
        }
        private void Screen_Load(object sender, RoutedEventArgs e)
        {
            this.Contact_No.Focus();
        }
        public void onPressValueUpdate(object sender, TextChangedEventArgs e)
        { // update value on the spot
            TextBox textBox = (TextBox)sender;
            string text = textBox.Text;
        }

    }
}

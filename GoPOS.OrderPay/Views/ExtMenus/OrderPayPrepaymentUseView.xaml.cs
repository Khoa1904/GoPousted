using Google.Protobuf.WellKnownTypes;
using GoPOS.Common.Views;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text.RegularExpressions;
using System.Windows.Controls;
using System.Windows.Input;

namespace GoPOS.Views
{
    public partial class OrderPayPrepaymentUseView : UCViewBase
    {
        public OrderPayPrepaymentUseView()
        {
            InitializeComponent();
            Loaded += OrderPayPrepaymentUseView_Loaded;
        }

        private void OrderPayPrepaymentUseView_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            this.txtSearch_Cst_Tel.Focus();
        }

    }
}

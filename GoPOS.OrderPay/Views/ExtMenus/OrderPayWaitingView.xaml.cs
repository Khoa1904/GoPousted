using GoPOS.Common.Views;
using GoPOS.OrderPay.Interface.View;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Controls;

namespace GoPOS.Views
{ 
    public partial class OrderPayWaitingView : UCViewBase, IOrderPayWaitingView
    {
        public OrderPayWaitingView()
        {
            InitializeComponent();
        }
    }
}

using GoPOS.Common.Views;
using GoPOS.Models;
using GoPOS.OrderPay.Interface.View;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace GoPOS.Views
{ 
    public partial class OrderPaySoldOutView : UCViewBase, IOrderPaySoldOutView
    {
        public OrderPaySoldOutView()
        {
            InitializeComponent();
        }
    }
}
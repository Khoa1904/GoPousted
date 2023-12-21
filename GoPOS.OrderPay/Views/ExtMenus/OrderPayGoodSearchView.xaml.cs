using Caliburn.Micro;
using GoPOS.Common.Views;
using GoPOS.Common.Views.Controls;
using GoPOS.OrderPay.Interface.View;
using GoPOS.ViewModels;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace GoPOS.Views
{
    public partial class OrderPayGoodSearchView : UCViewBase
    {
        public OrderPayGoodSearchView() : base()
        {
            InitializeComponent();
            this.Loaded += Reset;
        }

        public TextBox tbPRDCODE => this.txtPrdCode;

        public TextBox tbPRDNAME => this.txtPrdName;

        private void Reset(object sender, EventArgs e)
        {
            tbPRDCODE.Text = "";
            tbPRDNAME.Text = "";
        }

        private void tbPRDCODE_GotFocus(object sender, RoutedEventArgs e)
        {
            tbPRDCODE.Focus();
        }

        private void tbPRDNAME_GotFocus(object sender, RoutedEventArgs e)
        {
            tbPRDNAME.Focus();
        }

        private void ClickAddEvent(object sender, MouseButtonEventArgs e)
        {

            IoC.Get<OrderPayGoodSearchViewModel>().ClickAdditem();
        }
    }
}

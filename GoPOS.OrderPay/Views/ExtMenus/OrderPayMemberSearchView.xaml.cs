using Caliburn.Micro;
using GoPOS.Common.Views;
using GoPOS.OrderPay.ViewModels.Controls;
using GoPOS.Services;
using GoPOS.ViewModels;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace GoPOS.Views
{ 
    public partial class OrderPayMemberSearchView : UCViewBase
    {
        public OrderPayMemberSearchView()
        {
            InitializeComponent();
            this.Loaded += OrderPayMemberSearchView_Loaded;
        }

        private void OrderPayMemberSearchView_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            txtTel.Focus();
        }

        private void text_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter)
            {
                if(sender is TextBox tb)
                {
                    switch(tb.Name)
                    {
                        case "txtMemberCard":
                            this.txtTel.Focus();
                            break;
                        case "txtTel":
                            this.txtMemberName.Focus();
                            break;
                        case "txtMemberName":
                            ((IOrderPayMemberSearchViewModel)ViewModel).SearchMember(txtTel.Text, txtMemberCard.Text, txtMemberName.Text);
                            break;
                        default: break;
                    }
                }
            }
        }
    }
}

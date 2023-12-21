using Caliburn.Micro;
using GoPOS.Common.Helpers;
using GoPOS.Common.Views;
using GoPOS.Common.Views.Controls;
using GoPOS.Models;
using GoPOS.Payment.Interface.View;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace GoPOS.Views
{
    public partial class OrderPayCompPayView : UCViewBase, IOrderPayCompPayView
    {
        public OrderPayCompPayView()
        {
            InitializeComponent();
            Loaded += OrderPayCompPayView_Loaded; ;
        }

        private void OrderPayCompPayView_Loaded(object sender, RoutedEventArgs e)
        {
            this.PayAmount.Focus();
        }

        /// <summary>
        /// TO DO
        /// 1. REnder buttons
        /// 2. 
        /// </summary>
        /// <param name="payButtons"></param>
        public void RenderPayButtons(ORDER_FUNC_KEY[] payButtons)
        {
            var btns = this.FindVisualChildren<Button>().Where(p => p.Name.StartsWith("btnPayAmt")).ToArray();
            foreach (var funcKey in payButtons)
            {
                var btn = this.FindVisualChildren<Button>().FirstOrDefault(p => p.Name == "btnPay" + funcKey.POSITION_NO);
                if (btn == null)
                {
                    continue;
                }

                btn.Tag = funcKey;
                TextBlock tb = (TextBlock)btn.Content;
                if (tb != null)
                {
                    tb.Text = funcKey.FK_NAME;
                }

                var tbPayAmt = this.FindVisualChildren<TextBlock>().FirstOrDefault(p => p.Name == "txtPayAmt" + funcKey.POSITION_NO.Trim());
                if (tbPayAmt != null)
                {
                    tbPayAmt.Text = string.Empty;
                }
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="positionNo"></param>
        /// <param name="paidAmt"></param>
        /// <param name="resetAmt">0: reset to 0, 1: add, -1: minus</param>
        public void UpdatePaidAmount(string positionNo, decimal? paidAmt, int resetAmt)
        {
            var txt = this.FindVisualChildren<TextBlock>().FirstOrDefault(p => p.Name == "txtPayAmt" + positionNo.Trim());
            if (txt == null)
            {
                return;
            }

            decimal? amt = txt.Tag == null ? 0 : (decimal)txt.Tag;
            switch (resetAmt)
            {
                case 1:
                    amt += paidAmt;
                    break;
                case -1:
                    amt -= paidAmt;
                    break;
                default:
                    amt = paidAmt;
                    break;
            }

            txt.Text = string.Format("{0:#,##0}", amt);
            txt.Tag = amt;
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (int.TryParse(PayAmount.Text, out int value))
            {
                PayAmount.Text = value == 0 ? "" : value.ToString();
            }
        }
    }

}

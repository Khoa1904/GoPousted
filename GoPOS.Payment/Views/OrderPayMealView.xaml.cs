using GoPOS.Common.Helpers;
using GoPOS.Common.Views;
using GoPOS.Models;
using GoPOS.Payment.Interface.View;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace GoPOS.Views
{
    public partial class OrderPayMealView : UCViewBase, IOrderPayMealView
    {
        public OrderPayMealView() : base()
        {
            InitializeComponent();
            Loaded += LoginView_Loaded;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lsItem"></param>
        /// <exception cref="NotImplementedException"></exception>
        public void SetListItemList(List<SALES_GIFT_SALE> lsItem)
        {
            ListView.ItemsSource = lsItem;
            if (lsItem.Count > 0)
            {
                ListView.SelectedItem = lsItem[lsItem.Count - 1];
            }
            else
            {
                ListView.SelectedItem = null;
            }
            ListView.Items.Refresh();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lsTicket"></param>
        /// <param name="isSelected"></param>
        /// <exception cref="NotImplementedException"></exception>
        public void SetTicketValue(List<MST_INFO_TICKET> lsTicket, int isSelected)
        {
            var btns = form.FindVisualChildren<Button>().ToList();
            if (lsTicket == null || lsTicket.Count == 0) return;
            for (int j = 0; j < btns.Count; j++)
            {
                var tb = (TextBlock)btns[j].Content;
                if (tb != null)
                {
                    if (j + 1 > lsTicket.Count)
                    {
                        tb.Text = "";
                        btns[j].Tag = 0;
                        btns[j].BorderBrush = new SolidColorBrush(Color.FromRgb(29, 52, 3));
                        btns[j].Background = new SolidColorBrush(Color.FromRgb(249, 249, 249));
                    }
                    else
                    {
                        tb.Text = lsTicket[j].TK_GFT_NAME;
                        btns[j].Tag = "btnTk_" + lsTicket[j].TK_GFT_CODE;
                        if (j + 1 == isSelected)
                        {
                            btns[j].BorderBrush = new SolidColorBrush(Color.FromRgb(29, 52, 3));
                            btns[j].Background = new SolidColorBrush(Color.FromRgb(101, 139, 20));
                        }
                        else
                        {
                            btns[j].BorderBrush = new SolidColorBrush(Color.FromRgb(29, 52, 3));
                            btns[j].Background = new SolidColorBrush(Color.FromRgb(249, 249, 249));
                        }
                    }
                }
            }
        }

        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !IsNumber(e.Text);
        }

        private bool IsNumber(string text)
        {
            return int.TryParse(text, out _);
        }

        private void LoginView_Loaded(object sender, RoutedEventArgs e)
        {
            this.Remark.Focus();
        }
    }
}

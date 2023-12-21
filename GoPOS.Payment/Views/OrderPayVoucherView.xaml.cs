using GoPOS.Common.Helpers;
using GoPOS.Common.Views;
using GoPOS.Models;
using GoPOS.Payment.Interface.View;
using System.ComponentModel;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace GoPOS.Views
{
    public partial class OrderPayVoucherView : UCViewBase, IOrderPayVoucherView
    {
        public OrderPayVoucherView() :base()
        {
            InitializeComponent();
            Loaded += LoginView_Loaded;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Refun.Focus();
        }

        public void SetListItem1Value(List<SALES_GIFT_SALE> lsItem)
        {
            SetNo(lsItem);
            PayListView.ItemsSource = lsItem;
            if (lsItem.Count > 0)
            {
                PayListView.SelectedItem = lsItem[lsItem.Count - 1];
            }
            else
            {
                PayListView.SelectedItem = null;
            }
            PayListView.Items.Refresh();
        }

        public void SetListItem2Value(List<SALES_GIFT_SALE> lsItem)
        {
            SetNo(lsItem);
            PayListViewRefun.ItemsSource = lsItem;
            if (lsItem.Count > 0)
            {
                PayListViewRefun.SelectedItem = lsItem[lsItem.Count - 1];
            }
            else
            {
                PayListViewRefun.SelectedItem = null;
            }
            PayListViewRefun.Items.Refresh();
        }

        public void SetTicketClassValue(List<MST_INFO_TICKET_CLASS> lsTicketClass, int isSelected)
        {
            var btns = Form1.FindVisualChildren<Button>().ToList();
            if (lsTicketClass == null || lsTicketClass.Count == 0) return;               
            for (int j = 0; j < btns.Count; j++)
            {
                var tb = (TextBlock)btns[j].Content;
                if (tb != null)
                {
                    if (j + 1 > lsTicketClass.Count)
                    {
                        tb.Text = "";
                        btns[j].Tag = 0;
                        btns[j].BorderBrush = new SolidColorBrush(Color.FromRgb(29, 52, 3));
                        btns[j].Background = new SolidColorBrush(Color.FromRgb(249, 249, 249));
                    }
                    else
                    {
                        tb.Text = lsTicketClass[j].TK_CLASS_NAME;
                        btns[j].Tag = "btnTicketClass_" + lsTicketClass[j].TK_CLASS_CODE;
                        if (j + 1  == isSelected)
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

        public void SetTicketValue(List<MST_INFO_TICKET> lsTicket, int isSelected)
        {
            var btns = Form2.FindVisualChildren<Button>().ToList();
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
                        btns[j].Tag = "btnTicket_" + lsTicket[j].TK_GFT_CODE;
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

        public void ScrollToEnd()
        {
            var scrollViewer = FindScrollViewer(PayListView);
            if (scrollViewer != null)
            {
                scrollViewer.ScrollToEnd();
            }
        }

        public void ScrollToEnd1()
        {
            var scrollViewer = FindScrollViewer(PayListViewRefun);
            if (scrollViewer != null)
            {
                scrollViewer.ScrollToEnd();
            }
        }

        private ScrollViewer FindScrollViewer(DependencyObject depObj)
        {
            if (depObj is ScrollViewer scrollViewer)
            {
                return scrollViewer;
            }

            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
            {
                var child = VisualTreeHelper.GetChild(depObj, i);
                var result = FindScrollViewer(child);
                if (result != null)
                {
                    return result;
                }
            }

            return null;
        }

        private void SetNo(List<SALES_GIFT_SALE> lsItem)
        {
            for (int i = 0; i < lsItem.Count; i++)
            {
                lsItem[i].NO = (i + 1).ToString();
            }
        }

        public void ResetTicketValue(List<MST_INFO_TICKET> lsTicket, int isSelected)
        {
            var btns = Form2.FindVisualChildren<Button>().ToList();
            for (int j = 0; j < btns.Count; j++)
            {
                var tb = (TextBlock)btns[j].Content;
                if (tb != null)
                {
                    tb.Text = "";
                    btns[j].Tag = "";
                    btns[j].BorderBrush = new SolidColorBrush(Color.FromRgb(29, 52, 3));
                    btns[j].Background = new SolidColorBrush(Color.FromRgb(249, 249, 249));
                }
            }
        }

        public void ResetListItem1Value(List<SALES_GIFT_SALE> lsItem)
        {
            PayListView.ItemsSource = null;
            PayListView.SelectedItem = null;
        }

        public void ResetListItem2Value(List<SALES_GIFT_SALE> lsItem)
        {
            PayListViewRefun.ItemsSource = null;
            PayListViewRefun.SelectedItem = null;
        }

        private void LoginView_Loaded(object sender, RoutedEventArgs e)
        {
            this.Remark.Focus();
        }

        public void NotNullCheckbox (object sender, RoutedEventArgs e)
        {
            if (sender is CheckBox checkedbox)
            {
                if(check1.IsChecked == false && check2.IsChecked == false)
                {
                    checkedbox.IsChecked = true;
                }
            }
        }

        private void Radiotic_Checkbox(object sender, RoutedEventArgs e)
        {
            // A radio-like checkbox
            if (sender is CheckBox clickedCheckbox)
            {
                foreach (var checkbox in new[] { check1, check2 })
                {
                    if (checkbox != clickedCheckbox)
                    {
                        checkbox.IsChecked = false;
                    }
                }
            }
        }
    }
}

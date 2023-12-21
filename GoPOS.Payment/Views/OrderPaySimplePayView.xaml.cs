using GoPOS.Common.Helpers;
using GoPOS.Common.Views;
using GoPOS.Models;
using GoPOS.Payment.Interface.View;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Media;

namespace GoPOS.Views
{
    public partial class OrderPaySimplePayView : UCViewBase, IOrderPaySimplePayView
    {
        private Button lastClicked = null;
        public OrderPaySimplePayView()
        {
            InitializeComponent();
            this.Loaded += OrderPaySimplePayView_Loaded;
        }

        private void OrderPaySimplePayView_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            txtPayAmt.Focus();
        }

        public void RenderPayCPList(MST_INFO_EASYPAY[] payCps)
        {
            var btns = gridPayCPList.FindVisualChildren<GoButton>().ToArray();
            int idx = 0;
            foreach (var btn in btns)
            {
                btn.Tag = idx <= payCps.Length - 1 ? payCps[idx] : null;
                var bg = new BitmapImage(new Uri(@"pack://application:,,,/GoPOS.Resources;component/resource/images/btn_ord_pay_grid_w.png", UriKind.RelativeOrAbsolute));
                btn.Background = new ImageBrush(bg);

                TextBlock txt = (TextBlock)btn.Content;
                txt.Text = idx <= payCps.Length - 1 ? payCps[idx].PAYCP_NAME : string.Empty;
                txt.Foreground = new SolidColorBrush(Colors.Black);

                idx++;
                btn.Click -= BtnClick;
                btn.Click += BtnClick;
            }

            if (lastClicked != null)
            {
                lastClicked = null;
            }
        }

        private void BtnClick(object sender, RoutedEventArgs e)
        {
            if (lastClicked != null)
            {
                var nbg = new BitmapImage(new Uri(@"pack://application:,,,/GoPOS.Resources;component/resource/images/btn_ord_pay_grid_w.png",
                    UriKind.RelativeOrAbsolute));
                lastClicked.Background = new ImageBrush(nbg);
                ((TextBlock)lastClicked.Content).Foreground = new SolidColorBrush(Colors.Black);
            }

            BitmapImage bg = new BitmapImage(new Uri(@"pack://application:,,,/GoPOS.Resources;component/resource/images/btn_ord_pay_grid_o.png",
                    UriKind.RelativeOrAbsolute));
            Button btn = (Button)sender;
            btn.Background = new ImageBrush(bg);
            TextBlock tb2 = (TextBlock)btn.Content;
            tb2.Foreground = new SolidColorBrush(Colors.White);
            lastClicked = btn;

            ////TextBlock tb = (TextBlock)btn.Content;
            ////tb.Foreground = new SolidColorBrush(Colors.Black);
            //var btns = gridPayCPList.FindVisualChildren<Button>().Where(p => p.Tag != null).ToArray();
            //lastClicked = btn;
            //foreach (var Button in btns)
            //{
            //     bg = new BitmapImage(new Uri(@"pack://application:,,,/GoPOS.Resources;component/resource/images/btn_ord_pay_grid_w.png", UriKind.RelativeOrAbsolute));
            //    Button.Background = new ImageBrush(bg);
            //    TextBlock tb2 = (TextBlock)Button.Content;
            //    tb2.Foreground = new SolidColorBrush(Colors.Black);
            //}

        }
    }
}

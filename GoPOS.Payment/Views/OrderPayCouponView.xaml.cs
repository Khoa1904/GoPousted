using GoPOS.Common.Helpers;
using GoPOS.Common.Interface.Model;
using GoPOS.Common.Views;
using GoPOS.Models;
using GoPOS.Payment.Interface.View;
using System.ComponentModel;
using System.Windows.Controls;
using System.Windows.Media;

namespace GoPOS.Views
{
    public partial class OrderPayCouponView : UCViewBase, IOrderPayCouponView
    {
        public OrderPayCouponView() : base()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lsItem"></param>
        //public void SetListItemList(ORDER_COUPON_DETAIL item)
        //{
        //    if (item == null)
        //    {
        //        ListView.Items.Clear();
        //        ListView.Items.Refresh();
        //    }
        //    else
        //    {
        //        ListView.Items.Clear();
        //        ListView.Items.Add(item);
        //        ListView.Items.Refresh();
        //    }
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lsItem"></param>
        public void AddListItemList(List<ORDER_COUPON_DETAIL> item)
        {
            ListView.ItemsSource = item;
            ListView.Items.Refresh();
        }

        public void SetTableCouponValue(List<MST_INFO_COUPON> lsTableCoupon, int isSelected)
        {
            var btns = form.FindVisualChildren<Button>().ToList();
            if (lsTableCoupon == null || lsTableCoupon.Count == 0) return;
            for (int j = 0; j < btns.Count; j++)
            {
                var tb = (TextBlock)btns[j].Content;
                if (tb != null)
                {
                    if (j + 1 > lsTableCoupon.Count)
                    {
                        tb.Text = "";
                        btns[j].Tag = 0;
                        btns[j].BorderBrush = new SolidColorBrush(Color.FromRgb(29, 52, 3));
                        btns[j].Background = new SolidColorBrush(Color.FromRgb(249, 249, 249));
                    }
                    else
                    {
                        tb.Text = lsTableCoupon[j].TK_CPN_NAME;
                        btns[j].Tag = "btnCp_" + lsTableCoupon[j].TK_CPN_CODE;
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
    }
}

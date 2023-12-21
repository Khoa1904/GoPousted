using GoPOS.Common.Helpers;
using GoPOS.Common.Views;
using GoPOS.Models;
using GoPOS.OrderPay.Interface.View;
using System.ComponentModel;
using System.Windows.Controls;

namespace GoPOS.Views
{
    public partial class OrderPayExtMenuView : UCViewBase, IOrderPayExtMenuView
    {
        public OrderPayExtMenuView() : base()
        {
            InitializeComponent();
        }

        public void RenderExtButtons(ORDER_FUNC_KEY[] funcKeys)
        {
            var btns = buttonList.FindVisualChildren<Button>().ToList();
            for (int i = 0; i < btns.Count; i++)
            {
                var tb = (TextBlock)btns[i].Content;
                if (tb != null)
                {
                    tb.Text = i < funcKeys.Length ? funcKeys[i].FK_NAME : string.Empty;
                }

                btns[i].Tag = i < funcKeys.Length ? "FK_" + funcKeys[i].FK_NO : null;
            }


        }
    }
}

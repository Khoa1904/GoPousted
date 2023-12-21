using GoPOS.Common.Views;
using GoPOS.Models;
using GoPOS.OrderPay.Interface.View;
using System.ComponentModel;
using System.Windows.Controls;
using GoPOS.Common.Helpers;
using SixLabors.ImageSharp.Processing;
using System.Data.Entity.Core.Mapping;

namespace GoPOS.Views
{
    public partial class OrderPayPrintMngView : UCViewBase, IOrderPayPrintMngView
    {
        public OrderPayPrintMngView()
        {
            InitializeComponent();
        }

        public void RenderFuncButtons(ORDER_FUNC_KEY[] funcButtons)
        {
            var allSps = btnForm.FindVisualChildren<StackPanel>().ToArray();
            for (int i = 0; i < funcButtons.Length; i++)
            {
                var sp = allSps[i];
                var btn = sp.Children[0] as GoButton;
                if (btn != null)
                {
                    btn.Tag = funcButtons[i];
                    TextBlock tb = btn.Content as TextBlock;
                    tb.Text = funcButtons[i].FK_NAME;
                }
            }
        }
    }
}

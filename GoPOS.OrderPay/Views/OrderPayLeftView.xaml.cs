using GoPOS.Common.Helpers;
using GoPOS.Common.Views;
using GoPOS.Models;
using GoPOS.OrderPay.Interface.View;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.ComponentModel;
using System.Windows.Controls;

namespace GoPOS.Views
{
    public partial class OrderPayLeftView : UCViewBase, IOrderPayLeftView
    {
        public OrderPayLeftView() : base()
        {
            InitializeComponent();
        }

        public void RenderLeftFuncButtons(ORDER_FUNC_KEY[] funcKeys)
        {
            var btns = spExtButtons.FindVisualChildren<Button>().ToArray();
            for (int i = 0; i < btns.Length; i++)
            {
                var tb = (TextBlock)btns[i].Content;
                if (tb != null)
                {
                    tb.Text = i < funcKeys.Length ? funcKeys[i].FK_NAME : string.Empty;
                }

                btns[i].Tag = i < funcKeys.Length ? "FK_" + funcKeys[i].FK_NO : null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type">0: extend menu shown</param>
        /// <param name="disable"></param>
        public void DisableElements(int type, bool disable)
        {
            switch (type)
            {
                case 0:
                    this.spItemActions.IsEnabled = !disable;
                    this.spItemGrid.IsEnabled = !disable;
                    this.spExtButtons.IsEnabled = !disable;
                    break;
                default:
                    break;
            }
        }

        public void Translate()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ListOrder_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            
        }
    }
}

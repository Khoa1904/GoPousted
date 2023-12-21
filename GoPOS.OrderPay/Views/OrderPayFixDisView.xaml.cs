using GoPOS.Common.Helpers;
using GoPOS.Common.Views;
using GoPOS.Models;
using GoPOS.Payment.Interface.View;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Controls;

namespace GoPOS.Views
{
    public partial class OrderPayFixDisView : UCViewBase, IOrderPayFixDisView
    {
        public OrderPayFixDisView()
        {
            InitializeComponent();
        }

        public void RenderDiscountOptions(MST_INFO_FIX_DC[] fixDcs)
        {
            var allAmts = fixDcs.Where(p => p.DC_DIV_FLAG == "A" && p.DC_TYPE_FLAG == "A").OrderBy(p => p.DISP_SEQ_NO).ToArray();
            var allAmtBtns = spAllAmtBtns.FindVisualChildren<Button>().ToArray();
            for (int i = 0; i < allAmtBtns.Length; i++)
            {
                var btn = allAmtBtns[i];
                var amt = i <= allAmts.Length - 1 ? allAmts[i] : null;
                TextBlock tb = (TextBlock)btn.Content;
                tb.Text = amt == null ? string.Empty : string.Format("전체 {0:#,##0}원", amt.DC_VALUE);
                btn.Tag = amt;
            }

            var allPers = fixDcs.Where(p => p.DC_DIV_FLAG == "A" && p.DC_TYPE_FLAG == "P").OrderBy(p => p.DISP_SEQ_NO).ToArray();
            var allPerBtns = spAllPerBtns.FindVisualChildren<Button>().ToArray();
            for (int i = 0; i < allPerBtns.Length; i++)
            {
                var btn = allPerBtns[i];
                var per = i <= allPers.Length - 1 ? allPers[i] : null;
                TextBlock tb = (TextBlock)btn.Content;
                tb.Text = per == null ? string.Empty : string.Format("전체 {0:#,##0}%", per.DC_VALUE);
                btn.Tag = per;
            }

            var selAmts = fixDcs.Where(p => p.DC_DIV_FLAG == "S" && p.DC_TYPE_FLAG == "A").OrderBy(p => p.DISP_SEQ_NO).ToArray();
            var selAmtBtns = spSelAmtBtns.FindVisualChildren<Button>().ToArray();
            for (int i = 0; i < selAmtBtns.Length; i++)
            {
                var btn = selAmtBtns[i];
                var amt = i <= selAmts.Length - 1 ? selAmts[i] : null;
                TextBlock tb = (TextBlock)btn.Content;
                tb.Text = amt == null ? string.Empty : string.Format("선택 {0:#,##0}원", amt.DC_VALUE);
                btn.Tag = amt;
            }

            var selPers = fixDcs.Where(p => p.DC_DIV_FLAG == "S" && p.DC_TYPE_FLAG == "P").OrderBy(p => p.DISP_SEQ_NO).ToArray();
            var selPerBtns = spSelPerBtns.FindVisualChildren<Button>().ToArray();
            for (int i = 0; i < selPerBtns.Length; i++)
            {
                var btn = selPerBtns[i];
                var per = i <= selPers.Length - 1 ? selPers[i] : null;
                TextBlock tb = (TextBlock)btn.Content;
                tb.Text = per == null ? string.Empty : string.Format("선택 {0:#,##0}%", per.DC_VALUE);
                btn.Tag = per;
            }
        }
    }
}

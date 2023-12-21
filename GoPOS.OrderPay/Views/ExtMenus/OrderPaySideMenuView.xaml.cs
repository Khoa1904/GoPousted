using GoPOS.Common.Helpers;
using GoPOS.Common.Views;
using GoPOS.Models;
using GoPOS.OrderPay.Interface.View;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.ComponentModel;
using System.Windows.Controls;
using System.Windows.Controls.Ribbon;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace GoPOS.Views
{
    public partial class OrderPaySideMenuView : UCViewBase, IOrderPaySideMenuView
    {
        public OrderPaySideMenuView()
        {
            InitializeComponent();
        }

        public void RenderSideClasses(ORDER_SIDE_CLS_MENU[] sideClassess)
        {
            if (lastSelSideClsBtn != null)
            {
                var bgn = new BitmapImage(new Uri(@"pack://application:,,,/GoPOS.Resources;component/resource/images/btn_side_main_off.png", UriKind.RelativeOrAbsolute));
                lastSelSideClsBtn.Background = new ImageBrush(bgn);
            }

            lastSelSideClsBtn = null;

            ORDER_SIDE_CLS_MENU selSideCls = null;
            var allBtns = gridClassMenus.FindVisualChildren<Button>().Where(p => p.Name.StartsWith("btnSideClass")).ToArray();
            for (int i = 0; i < allBtns.Length; i++)
            {
                var sd = i <= sideClassess.Length - 1 ? sideClassess[i] : null;

                Button btn = allBtns[i];
                btn.Click += SideClsBtn_Click;
                btn.Tag = sd;

                if (lastSelSideClsBtn == null)
                {
                    var bg = new BitmapImage(new
                        Uri(@"pack://application:,,,/GoPOS.Resources;component/resource/images/btn_side_main_on.png", UriKind.RelativeOrAbsolute));
                    btn.Background = new ImageBrush(bg);
                    lastSelSideClsBtn = btn;
                    selSideCls = sd;
                }

                TextBlock tb = (TextBlock)btn.Content;
                tb.Text = sd?.CLASS_NAME;
            }
        }

        private Button lastSelSideClsBtn = null;
        private void SideClsBtn_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            if (btn.Tag == null
                || btn == lastSelSideClsBtn) return;

            // SET SELECTED BG
            var bg = new BitmapImage(new Uri(@"pack://application:,,,/GoPOS.Resources;component/resource/images/btn_side_main_on.png", UriKind.RelativeOrAbsolute));
            btn.Background = new ImageBrush(bg);

            if (lastSelSideClsBtn != null)
            {
                var bgn = new BitmapImage(new Uri(@"pack://application:,,,/GoPOS.Resources;component/resource/images/btn_side_main_off.png", UriKind.RelativeOrAbsolute));
                lastSelSideClsBtn.Background = new ImageBrush(bgn);
            }

            lastSelSideClsBtn = btn;
        }

        public void RenderSideMenus(ORDER_SIDE_CLS_MENU[] sideMenus)
        {
            var allBtns = gridClassMenus.FindVisualChildren<Button>().Where(p => p.Name.StartsWith("btnSideMenu")).ToArray();
            for (int i = 0; i < allBtns.Length; i++)
            {
                Button btn = allBtns[i];
                var sd = i <= sideMenus.Length - 1 ? sideMenus[i] : null;
                //var bg = new BitmapImage(new Uri(@"pack://application:,,,/GoPOS.Resources;component/resource/images/btn_side_main_off.png", UriKind.RelativeOrAbsolute));
                /////<ImageBrush ImageSource="pack://application:,,,/GoPOS.Resources;component/resource/images/btn_side_main_off.png" />
                //btn.Background = new ImageBrush(bg);

                StackPanel sp = (StackPanel)btn.Content;
                TextBlock tbName = (TextBlock)sp.Children[0];
                tbName.Text = sd?.SUB_NAME;

                TextBlock tbPrice = (TextBlock)sp.Children[1];
                tbPrice.Text = sd?.CLASS_TYPE == "S" ? string.Format("{0:#,##0}", sd?.UNIT_PRICE) : string.Empty;
                btn.Tag = sd;
            }
        }
    }
}

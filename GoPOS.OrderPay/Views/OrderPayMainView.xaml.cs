using Caliburn.Micro;
using CoreWCF.Runtime;
using GoPOS.Common.Helpers;
using GoPOS.Common.Views;
using GoPOS.Common.Views.Controls;
using GoPOS.Helpers;
using GoPOS.Models;
using GoPOS.OrderPay.Interface.View;
using GoPOS.OrderPay.Models;
using GoPOS.SerialPacketProcess;
using GoPOS.Servers;
using GoPOS.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace GoPOS.Views
{
    /// <summary>
    /// FormTest.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class OrderPayMainView : UCViewBase, IOrderPayMainView
    {
        public Visibility ShowHideMessage { get => StatusMessage.Visibility; set => StatusMessage.Visibility = value; }
        public ListView OrderItemsLV => this.ListOrder;

        public ToggleButton billText { get => this.Bill; set {value= billText;}}

        public ToggleButton itemText { get => this.Item; set { value = itemText; } }

        public ToggleButton kitchenText { get => this.Kitchen; set { value = kitchenText; } }
        public ToggleButton customerText { get => this.Customer; set { value = customerText; } }

        public OrderPayMainView()
        {
            InitializeComponent();
            cttTime.IsRunning = true;

        }
        private Button ClickedBtn = null;
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
                btns[i].Click += spExt_Click;
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="activatedTypes"></param>
        /// <param name="activated"></param>
        public void DisableElements(string activeType, bool activated)
        {
            string keypadcheck = ActiveItemLB.Content.ToString().Substring(ActiveItemLB.Content.ToString().LastIndexOf(".") + 1);
            switch (activeType)
            {
                case "All":
                    dpLeftPart.Children[0].Visibility = activated ? Visibility.Visible : Visibility.Collapsed;
                    dpRightPart.Children[0].Visibility = activated ? Visibility.Visible : Visibility.Collapsed;
                    break;
                case "Left":
                    dpLeftPart.Children[0].Visibility = activated ? Visibility.Visible : Visibility.Collapsed;
                    break;
                case "Right":
                    dpRightPart.Children[0].Visibility = activated ? Visibility.Visible : Visibility.Collapsed;

                    break;
                case "ExceptKeyPad":
                    spItemGrid.Children[0].Visibility = activated ? Visibility.Visible : Visibility.Collapsed;
                    dpItemPrints.Children[0].Visibility = activated ? Visibility.Visible : Visibility.Collapsed;
                    spItemActions.Children[0].Visibility = activated ? Visibility.Visible : Visibility.Collapsed;
                    spExtButtons.Children[0].Visibility = activated ? Visibility.Visible : Visibility.Collapsed;
                    //dpLeftKeyPad.Children[0].Visibility = activated ? Visibility.Visible : Visibility.Collapsed;
                    break;

                case "OnlyItems":
                    dpItemPrints.Children[0].Visibility = activated ? Visibility.Visible : Visibility.Collapsed;
                    spItemActions.Children[0].Visibility = activated ? Visibility.Visible : Visibility.Collapsed;
                    spExtButtons.Children[0].Visibility = activated ? Visibility.Visible : Visibility.Collapsed;
                    break;

                case "ExceptActiveItemLB":
                    dpRightPart.Children[0].Visibility = activated ? Visibility.Visible : Visibility.Collapsed;
                    spItemGrid.Children[0].Visibility = activated ? Visibility.Visible : Visibility.Collapsed;
                    dpItemPrints.Children[0].Visibility = activated ? Visibility.Visible : Visibility.Collapsed;
                    spItemActions.Children[0].Visibility = activated ? Visibility.Visible : Visibility.Collapsed;
                    spExtButtons.Children[0].Visibility = activated ? Visibility.Visible : Visibility.Collapsed;
                    break;

                case "ShowDiscHandler":
                    // spItemGrid.Children[0].Visibility = activated ? Visibility.Visible : Visibility.Collapsed;
                    dpItemPrints.Children[0].Visibility = activated ? Visibility.Visible : Visibility.Collapsed;
                    spItemActions.Children[0].Visibility = activated ? Visibility.Visible : Visibility.Collapsed;
                    spExtButtons.Children[0].Visibility = activated ? Visibility.Visible : Visibility.Collapsed;
                    break;
                default:
                    break;
            }
        }

        #region Grid of Items binding

        private void ListViewItem_PreviewMouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            ListViewItem item = sender as ListViewItem;
            if (item != null && item.IsSelected)
            {
                // do stuff
                IoC.Get<OrderPayMainViewModel>().ItemGrid_CheckShowSideMenu();
            }
        }
        #endregion

        private void spExt_Click(object sender, RoutedEventArgs e)
        {
            if (ClickedBtn != null)
            {
                LinearGradientBrush gradientBrush = new LinearGradientBrush();
                gradientBrush.StartPoint = new Point(0, 1);
                gradientBrush.EndPoint = new Point(0, 0);
                gradientBrush.GradientStops.Add(new GradientStop(Color.FromRgb(236, 236, 236), 0.45));
                gradientBrush.GradientStops.Add(new GradientStop(Color.FromRgb(223, 223, 223), 0.55));
                ClickedBtn.Background = gradientBrush;
            }

            Button btn = (Button)sender;
            ClickedBtn = btn;

            var bg = new BitmapImage(new Uri(@"pack://application:,,,/GoPOS.Resources;component/resource/images/btn_main_right_on.png", UriKind.RelativeOrAbsolute));
            ClickedBtn.Background = new ImageBrush(bg);
        }

        private void EnableButton(bool aaa)
        {
            var Extbtn = spExtButtons.FindVisualChildren<Button>().ToArray();
            foreach (Button button in Extbtn)
            {
                button.IsEnabled = aaa;
            }
        }
 
        public void ResetSelectedExtButton()
        {
            if (ClickedBtn != null)
            {
                LinearGradientBrush gradientBrush = new LinearGradientBrush();
                gradientBrush.StartPoint = new Point(0, 1);
                gradientBrush.EndPoint = new Point(0, 0);
                gradientBrush.GradientStops.Add(new GradientStop(Color.FromRgb(236, 236, 236), 0.45));
                gradientBrush.GradientStops.Add(new GradientStop(Color.FromRgb(223, 223, 223), 0.55));
                ClickedBtn.Background = gradientBrush;
            }

            ClickedBtn = null;
        }

        public void sizeMini(object sender, RoutedEventArgs e)
        {
            Application.Current.Windows[0].WindowState = WindowState.Minimized;
        }
    }
}

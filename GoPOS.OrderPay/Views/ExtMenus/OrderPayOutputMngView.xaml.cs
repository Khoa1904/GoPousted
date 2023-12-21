using GoPOS.Common.Views;
using GoPOS.Models;
using GoPOS.OrderPay.Interface.View;
using GoPOS.ViewModels;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace GoPOS.Views
{
    public partial class OrderPayOutputMngView : UCViewBase, IOrderPayOutputMngView
    {

        public OrderPayOutputMngView()
        {
            InitializeComponent();
        }

        public void RenderPayReceiptMng(List<TRN_HEADER> lsHeader)
        {
            BillListView.Items.Clear();
            BillListView.ItemsSource = lsHeader;
        }

        private Point GetPointOfButton(UIElement element)
        {
            Point relativePoint = new Point();
            if (element is Button)
            {
                Button btn = element as Button;
                relativePoint = btn.TranslatePoint(new Point(0, 0), this);
                Point screenPoint = GetParentPoint();
                relativePoint.X += screenPoint.X + btn.Width;
                relativePoint.Y += screenPoint.Y;

                return relativePoint;
            }
            return relativePoint;
        }

        private Point GetParentPoint()
        {
            Point ParentPoint = this.TranslatePoint(new Point(0, 0), (UIElement)this.Parent);
            Point screenPoint = this.PointToScreen(ParentPoint);
            return screenPoint;
        }

        public Point buttonPosition { get; set; }

        private void ButtonClick(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            buttonPosition = GetPointOfButton(btn);
        }

        public void ScrollToEnd()
        {
            var scrollViewer = FindScrollViewer(BillListView);
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

    }

}

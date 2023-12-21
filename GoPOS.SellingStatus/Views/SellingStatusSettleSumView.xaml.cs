using GoPOS.Common.Views;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using GoPOS.Common.Helpers.Controls;
using GoPOS.SellingStatus.Interface;

namespace GoPOS.Views
{
    public partial class SellingStatusSettleSumView : UCViewBase,ISellingStatusSettleSumView
    {
        public SellingStatusSettleSumView()
        {
            InitializeComponent();
        }

        public Point buttonPosition { get; set; }

        private void ButtonClick(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            buttonPosition =PointHelper.GetPointOfButton(btn,this);
        }
    }
}

using Caliburn.Micro;
using GoPOS.Common.Helpers;
using GoPOS.Common.Interface.View;
using GoPOS.Common.Views;
using GoPOS.OrderPay.ViewModels.Controls;
using GoPOS.ViewModels;
using System.Windows.Controls;
using GoPOS.OrderPay.Interface.View;
using System.Windows;
using GoPOS.Models;

namespace GoPOS.OrderPay.Views.Controls
{
    public partial class PopupGradeView : UCViewBase
    {
        public PopupGradeView()
        {
            InitializeComponent();

        }

        private void chooseItem(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {

        }

        private void MemListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count == 0) return;

            var data = e.AddedItems[0] as MEMBER_GRADE;
            IoC.Get<PopupGradeViewModel>().SetGrades(data.grdCode);
        }
    }
}

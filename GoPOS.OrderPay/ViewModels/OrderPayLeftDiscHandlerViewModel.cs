using Caliburn.Micro;
using CoreWCF;
using GoPOS.Common.Interface.Model;
using GoPOS.Common.Interface.View;
using GoPOS.Common.ViewModels;
using GoPOS.Common.Views;
using GoPOS.Helpers.CommandHelper;
using GoPOS.Models;
using GoPOS.OrderPay.Interface.View;
using GoPOS.OrderPay.Interface.ViewModel;
using GoPOS.OrderPay.Models;
using GoPOS.OrderPay.ViewModels.Controls;
using GoPOS.Payment.Interface.View;
using GoPOS.Service;
using GoPOS.Views;
using GoShared.Helpers;
using System;
using System.Collections.Generic;
using System.DirectoryServices.Protocols;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace GoPOS.ViewModels
{
    public class OrderPayLeftDiscHandlerViewModel : BaseItemViewModel
    {
        private IOrderPayLeftDiscHandlerView _view;
        private IOrderPayMainViewModel orderPayMainViewModel;

        public OrderPayLeftDiscHandlerViewModel(IWindowManager windowManager, IEventAggregator eventAggregator, IOrderPayMainViewModel mainViewModel)
            : base(windowManager, eventAggregator)
        {
            this.ViewLoaded += OrderPayLeftDiscHandlerViewModel_ViewLoaded;
        }

        private void OrderPayLeftDiscHandlerViewModel_ViewLoaded(object? sender, EventArgs e)
        {
            orderPayMainViewModel = (IOrderPayMainViewModel)Parent;
        }


        public override bool SetIView(IView view)
        {
            _view = (IOrderPayLeftDiscHandlerView)view;
            return base.SetIView(view);
        }

        public ICommand ButtonCommand => new RelayCommand<Button>(ButtonCommandClicked);
        private void ButtonCommandClicked(Button button)
        {
            if (button == null) { return; }

            string actionName = (string)button.Tag;
            _eventAggregator.PublishOnUIThreadAsync(new OrderPayDiscHandleEventArgs()
            {
                Action = "ButtonClose".Equals(actionName) ? OrderPayDiscHandleActions.CloseRight : OrderPayDiscHandleActions.Apply,
                IsAll = actionName.StartsWith("All"),
                IsAmt = actionName.Contains("Amt"),
                IsApply = !actionName.Contains("Cancel")
            });
        }
    }
}

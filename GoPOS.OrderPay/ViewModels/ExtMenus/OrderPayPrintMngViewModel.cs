using Caliburn.Micro;
using Dapper;
using GoPOS.Common.Interface.Model;
using GoPOS.Common.Interface.View;
using GoPOS.Common.Service;
using GoPOS.Common.ViewModels;
using GoPOS.Helpers;
using GoPOS.Helpers.CommandHelper;
using GoPOS.Models;
using GoPOS.Models.Common;
using GoPOS.OrderPay.Interface.View;
using GoPOS.OrderPay.Interface.ViewModel;
using GoPOS.Service.Service;
using GoPOS.Services;

using GoPOS.ViewModels;
using GoPOS.Views;
using GoShared.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;


namespace GoPOS.ViewModels
{

    public class OrderPayPrintMngViewModel : OrderPayChildViewModel
    {
        private IOrderPayMainViewModel orderPayMainViewModel;
        private IOrderPayPrintMngView _view;
        private readonly IOrderPayService orderPayService;

        public override object ActivateType => "ExceptActiveItemLB";
        public OrderPayPrintMngViewModel(IWindowManager windowManager, 
            IEventAggregator eventAggregator, IOrderPayService orderPayService) : base(windowManager, eventAggregator)
        {
            this.ViewLoaded += OrderPayPrintMngViewModel_ViewLoaded;
            this.orderPayService = orderPayService;
        }

        private void OrderPayPrintMngViewModel_ViewLoaded(object? sender, EventArgs e)
        {
            orderPayMainViewModel = (IOrderPayMainViewModel)Parent;
            var funcKeys = orderPayService.GetOrderFuncKey("09").Result.Item1.ToArray();
            _view.RenderFuncButtons(funcKeys);
        }

        public override bool SetIView(IView view)
        {
            _view = (IOrderPayPrintMngView)view;
            return base.SetIView(view);
        }

        public ICommand ButtonCommand => new RelayCommand<Button>(ButtonCommandClicked);
        private async void ButtonCommandClicked(Button btn)
        {
            if (btn.Tag == null)
            {
                return;
            }
            this.DeactivateClose(true);
            this.orderPayMainViewModel.ProcessFuncKeyClicked(btn.Tag as ORDER_FUNC_KEY);
        }
    }
}
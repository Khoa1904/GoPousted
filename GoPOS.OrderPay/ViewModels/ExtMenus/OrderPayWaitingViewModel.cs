using Caliburn.Micro;
using Dapper;
using GoPOS.Common.Interface.Model;
using GoPOS.Common.ViewModels;
using GoPOS.Helpers;
using GoPOS.Helpers.CommandHelper;
using GoPOS.Models;
using GoPOS.Models.Common;
using GoPOS.Services;

using GoPOS.ViewModels;
using GoPOS.Views;
using GoShared.Helpers;
using Grpc.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using static GoPOS.Function;


/*
 주문 > 확장메뉴 > 대기

 */

namespace GoPOS.ViewModels
{

    public class OrderPayWaitingViewModel : OrderPayChildViewModel
    {
        private IOrderPayMainViewModel orderPayMainViewModel;

        private int selectedItemIndex;
        private ObservableCollection<ORD_WAIT_ITEM> ordWaitingList;
        private readonly IOrderPayWaitingService orderPayWaitingService;
        public override object ActivateType { get => "ExceptKeyPad"; }

        public ObservableCollection<ORD_WAIT_ITEM> OrdWaitingList
        {
            get => ordWaitingList; set
            {
                ordWaitingList = value;
                NotifyOfPropertyChange(nameof(OrdWaitingList));
            }
        }
        public int SelectedItemIndex
        {
            get => selectedItemIndex;
            set
            {
                selectedItemIndex = value;
                NotifyOfPropertyChange(nameof(SelectedItemIndex));
            }
        }

        public ICommand SelectCommand => new RelayCommand<Button>(async button =>
        {
            var model = IoC.Get<DualScreenMainViewModel>();

            var name = model.ActiveItem.DisplayName;
            if (!name.Contains("DualScreenOrderingViewModel"))
            {
                model.ActiveItem = IoC.Get<DualScreenOrderingViewModel>();
            }

            var trDatas = await orderPayWaitingService.LoadWaitTRData(OrdWaitingList[SelectedItemIndex]);
            orderPayMainViewModel.LoadExistOrderTRs(trDatas);
            orderPayWaitingService.RemoveWaitingOrder(OrdWaitingList[SelectedItemIndex]);
            this.DeactivateClose(true);
        });

        public OrderPayWaitingViewModel(IWindowManager windowManager, IEventAggregator eventAggregator,
            IOrderPayWaitingService orderPayWaitingService) : base(windowManager, eventAggregator)
        {
            this.ViewInitialized += OrderPayWaitingViewModel_ViewInitialized;
            this.ViewLoaded += OrderPayWaitingViewModel_ViewLoaded;
            this.orderPayWaitingService = orderPayWaitingService;
        }

        private async void OrderPayWaitingViewModel_ViewLoaded(object? sender, EventArgs e)
        {
            var ords = await orderPayWaitingService.LoadWaitOrders();
            OrdWaitingList = new ObservableCollection<ORD_WAIT_ITEM>(ords);            
        }

        private async void OrderPayWaitingViewModel_ViewInitialized(object? sender, EventArgs e)
        {
            orderPayMainViewModel = (IOrderPayMainViewModel)Parent;            
        }
    }
}
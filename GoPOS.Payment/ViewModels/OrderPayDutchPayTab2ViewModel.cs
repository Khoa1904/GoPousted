using Caliburn.Micro;
using Dapper;
using GoPOS.Helpers;
using GoPOS.Helpers.CommandHelper;
using GoPOS.Models;
using GoPOS.Services;

using GoPOS.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;


/*
 주문 > 확장메뉴 > 더치페이tab2

 */

namespace GoPOS.ViewModels
{

    public class OrderPayDutchPayTab2ViewModel : Screen
    {
        private readonly IOrderPayService orderPayService;
        //public List<ORDER_FUNC_KEY> orderFuncKey;
        //public List<ORDER_LIST> orderList;
        //STRING SHOP_CODE, STRING SALE_DATE, STRING ORDER_NO

        private IEventAggregator _eventAggregator;

        public OrderPayDutchPayTab2ViewModel(IEventAggregator eventAggregator, IOrderPayService orderPayService)
        {
            this.orderPayService = orderPayService;
            this._eventAggregator = eventAggregator;
            this._eventAggregator.SubscribeOnPublishedThread(this);

            //_activeItem = (object)"";
            //ActiveItem = IoC.Get<OrderPayCalDlvrViewModel>();

            Init();
        }

        //private object _activeItem;
        //public object ActiveItem
        //{
        //    get { return _activeItem; }
        //    set
        //    {
        //        _activeItem = value;
        //        NotifyOfPropertyChange(() => ActiveItem);
        //    }
        //}

        private async void Init()
        {
            await Task.Delay(100);
        }


    }
}
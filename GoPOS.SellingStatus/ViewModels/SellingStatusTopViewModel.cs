using Caliburn.Micro;
using Dapper;
using GoPOS.Common.ViewModels;
using GoPOS.Helpers;
using GoPOS.Helpers.CommandHelper;
using GoPOS.Models;
using GoPOS.Services;

using GoPOS.Views;
using GoShared.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;


/*
 

 */

namespace GoPOS.ViewModels
{

    public class SellingStatusTopViewModel : BaseItemViewModel
    {
        private readonly IOrderPayService orderPayService;
        private IEventAggregator _eventAggregator;

        public SellingStatusTopViewModel(IWindowManager windowManager, IEventAggregator eventAggregator, IOrderPayService orderPayService)
            :base (windowManager, eventAggregator) 
        {
            this.orderPayService = orderPayService;
            this._eventAggregator = eventAggregator;
            this._eventAggregator.SubscribeOnPublishedThread(this);
            Init();
        }

        private async void Init()
        {
            await Task.Delay(100);
        }


        public void ButtonMenu(object sender, RoutedEventArgs e)
        {
            if (sender is not Button btn)
            {
                return;
            }
        }
    }
}
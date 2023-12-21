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

    public class SalePurchaseTopViewModel : BaseItemViewModel
    {
        private readonly IOrderPayService orderPayService;

        public SalePurchaseTopViewModel(IWindowManager windowManager, IEventAggregator eventAggregator, IOrderPayService orderPayService)
            :base (windowManager, eventAggregator) 
        {
            this.orderPayService = orderPayService;
        }
    }
}
using Caliburn.Micro;
using Dapper;
using GoPOS.Common.Interface.Model;
using GoPOS.Common.ViewModels;
using GoPOS.Helpers;
using GoPOS.Helpers.CommandHelper;
using GoPOS.Models;
using GoPOS.Models.Common;
using GoPOS.OrderPay.Interface.ViewModel;
using GoPOS.Services;

using GoPOS.ViewModels;
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
using System.Windows.Media;
using static GoPOS.Function;

/*
 주문 > 확장메뉴 > 배달 처리

 */

namespace GoPOS.ViewModels
{

    public class OrderPayDlvrProcessViewModel : BaseItemViewModel
    {
        public OrderPayDlvrProcessViewModel(IWindowManager windowManager, IEventAggregator eventAggregator) : base(windowManager, eventAggregator)
        {
        }
    }
}
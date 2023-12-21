using Caliburn.Micro;
using Dapper;
using GoPOS.Common.ViewModels;
using GoPOS.Helpers;
using GoPOS.Helpers.CommandHelper;
using GoPOS.Models;
using GoPOS.Models.Common;
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
using static GoPOS.Function;


/*
 주문 > 확장메뉴 > 예약조회

 */

namespace GoPOS.ViewModels
{

    public class OrderPayResveInqireViewModel : BaseItemViewModel
    {
        public OrderPayResveInqireViewModel(IWindowManager windowManager, IEventAggregator eventAggregator) : 
            base(windowManager, eventAggregator)
        {
        }
    }
}
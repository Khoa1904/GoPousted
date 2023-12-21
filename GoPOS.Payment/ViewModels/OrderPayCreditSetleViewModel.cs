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
 주문 > 확장메뉴 > 외상결제

 */
/// <summary>
/// 화면명 : 외상결제 확장 239
/// 작성자 : 김형석 
/// </summary>
namespace GoPOS.ViewModels
{

    public class OrderPayCreditSetleViewModel : OrderPayChildViewModel
    {
        public OrderPayCreditSetleViewModel(IWindowManager windowManager,
            IEventAggregator eventAggregator) : base(windowManager, eventAggregator)
        {
        }
    }
}
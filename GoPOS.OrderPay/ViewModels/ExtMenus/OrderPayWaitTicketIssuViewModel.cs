using Caliburn.Micro;
using Dapper;
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
using System.Windows.Media;
using static GoPOS.Function;

/*
 주문 > 확장메뉴 > 대기접수

 */
/// <summary>
/// 화면명 : 대기표발급
/// 작성자 : 김형석
/// </summary>
namespace GoPOS.ViewModels
{
    public class OrderPayWaitTicketIssuViewModel : Screen
    {

    }
}
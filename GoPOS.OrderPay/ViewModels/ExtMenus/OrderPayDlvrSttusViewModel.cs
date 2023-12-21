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
 주문 > 확장메뉴 > 배달 현황 및 관리

 */

/// <summary>
/// 화면명 : 배달주문내역  / 배달 현황 및 관리
/// 작성자 : 김형석
/// </summary>

namespace GoPOS.ViewModels
{
    public class OrderPayDlvrSttusViewModel : Screen
    {
    }
}
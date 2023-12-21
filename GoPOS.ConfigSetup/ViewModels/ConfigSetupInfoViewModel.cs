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
using System.Windows.Media;
using static GoPOS.Function;
using System.IO;
using GoShared.Helpers;
using GoPOS.Common.ViewModels;

/*
/// 화면명 : 환경설정 > 환경설정정보
/// 작성자 : 김형석
/// 작성일 : 20230319 - 20230401
 */

namespace GoPOS.ViewModels
{

    public class ConfigSetupInfoViewModel : BaseItemViewModel
    {
        public ConfigSetupInfoViewModel(IWindowManager windowManager, IEventAggregator eventAggregator) : base(windowManager, eventAggregator)
        {
        }
    }
}
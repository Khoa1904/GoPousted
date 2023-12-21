using Caliburn.Micro;
using Dapper;
using GoPOS.Common.Interface.View;
using GoPOS.Common.ViewModels;
using GoPOS.Helpers;
using GoPOS.Helpers.CommandHelper;
using GoPOS.Models;
using GoPOS.SalesMng.Interface.View;
using GoPOS.Services;

using GoPOS.Views;
using GoShared.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;


/*
 

 */

namespace GoPOS.ViewModels
{

    public class SalesMngTopViewModel : BaseItemViewModel
    {
        private ISalesMngTopView _view;
        public SalesMngTopViewModel(IWindowManager windowManager, IEventAggregator eventAggregator) : base(windowManager, eventAggregator)
        {
            this.ViewUnloaded += SalesMngTopViewModel_ViewUnloaded;
        }

        private void SalesMngTopViewModel_ViewUnloaded(object? sender, EventArgs e)
        {
            NotifyClosePage(typeof(SalesMngMainViewModel));
        }

        public override bool SetIView(IView view)
        {
            _view = (ISalesMngTopView)view;
            return base.SetIView(view);
        }
    }
}
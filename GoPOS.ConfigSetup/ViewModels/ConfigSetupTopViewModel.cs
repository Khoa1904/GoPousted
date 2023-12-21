using Caliburn.Micro;
using Dapper;
using GoPOS.Common.Interface.View;
using GoPOS.Common.ViewModels;
using GoPOS.ConfigSetup.Interface.View;
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
    public class ConfigSetupTopViewModel : BaseItemViewModel
    {
        private IConfigSetupTopView _view;
        public ConfigSetupTopViewModel(IWindowManager windowManager, IEventAggregator eventAggregator) : base(windowManager, eventAggregator)
        {
            this.ViewUnloaded += ConfigSetupTopViewModel_ViewUnloaded;
        }

        public override bool SetIView(IView view)
        {
            _view = (IConfigSetupTopView)view;
            return base.SetIView(view);
        }

        private void ConfigSetupTopViewModel_ViewUnloaded(object? sender, EventArgs e)
        {
            NotifyClosePage(typeof(ConfigSetupMainViewModel));
        }
    }
}
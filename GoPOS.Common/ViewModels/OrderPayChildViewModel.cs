using Caliburn.Micro;
using Dapper;
using GoPOS.Common.Interface.Model;
using GoPOS.Common.Interface.View;
using GoPOS.Common.ViewModels;
using GoPOS.Helpers;
using GoPOS.Helpers.CommandHelper;
using GoPOS.Models;
using GoPOS.Models.Common;
using GoPOS.Services;

using GoPOS.Views;
using GoShared.Events;
using GoShared.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using static GoPOS.Function;


namespace GoPOS.ViewModels
{
    public class OrderPayChildViewModel : BaseItemViewModel
    {
        public string AreaName { get; set; }
        public virtual object ActivateType { get; }

        public OrderPayChildViewModel(IWindowManager windowManager, IEventAggregator eventAggregator) : base(windowManager, eventAggregator)
        {
            this.ViewLoaded += OrderPayChildViewModel_ViewLoaded;
            this.Deactivated += OrderPayChildViewModel_Deactivated;
        }

        private async Task OrderPayChildViewModel_Deactivated(object sender, DeactivationEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine(this.GetType().Name + " - deactivated");
            ((IPageViewModel)this.Parent).ChildActivated(AreaName, false, ActivateType);
        }

        private void OrderPayChildViewModel_ViewLoaded(object? sender, EventArgs e)
        {
            System.Diagnostics.Debug.WriteLine(this.GetType().Name + " - viewloaded");
            ((IPageViewModel)this.Parent).ChildActivated(AreaName, true, ActivateType);
        }
    }

}
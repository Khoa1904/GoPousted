using Caliburn.Micro;
using Dapper;
using GoPOS.Common.Helpers;
using GoPOS.Common.Interface.Model;
using GoPOS.Common.Interface.View;
using GoPOS.Common.ViewModels;
using GoPOS.Helpers;
using GoPOS.Helpers.CommandHelper;
using GoPOS.Models;
using GoPOS.Models.Custom.SalesMng;
using GoPOS.Service;
using GoPOS.Services;

using GoPOS.Views;
using GoShared.Helpers;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Reflection;
using System.Reflection.Metadata;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using GoPOS.SalePurchase.ViewModels;
using GoPOS.SalePurchase.Interface.View;
using GoPOS.SalePurchase.Interface.ViewModel;

namespace GoPOS.ViewModels;

public class SalePurchaseMainViewModel : MainBasePageViewModel, ISalePurchaseMainViewModel, IHandle<SalePurchaseMainEventArgs>
{
    private ISalePurchaseMainView _view;

    private IScreen? _activeItemM = null;
    public IScreen? ActiveItemM
    {
        get { return _activeItemM; }
        set
        {
            PutScreen(nameof(ActiveItemM), _activeItemM);
            _activeItemM = value;
            _activeItemM?.ActivateAsync();

            NotifyOfPropertyChange(() => ActiveItemM);
        }
    }

    private IScreen? _activeItemTop = null;
    public IScreen? ActiveItemTop
    {
        get { return _activeItemTop; }
        set
        {
            PutScreen(nameof(ActiveItemTop), _activeItemTop);
            _activeItemTop = value;
            _activeItemTop?.ActivateAsync();

            NotifyOfPropertyChange(() => ActiveItemTop);
        }
    }
    private IScreen? _ActiveItemR = null;
    public IScreen? ActiveItemR
    {
        get { return _ActiveItemR; }
        set
        {
            PutScreen(nameof(ActiveItemR), _ActiveItemR);
            _ActiveItemR = value;
            _ActiveItemR?.ActivateAsync();
            NotifyOfPropertyChange(() => ActiveItemR);        }
    }

    public SalePurchaseMainViewModel(IWindowManager windowManager, IEventAggregator eventAggregator)
        : base(windowManager, eventAggregator)
    {
        Initiate();
    }

    public string AreaName { get; set; }
    public virtual object ActivateType { get; }

    public void Initiate()
    {
        this.ViewLoaded += SalePurchaseMainViewModel_ViewLoaded;
    }

    private void SalePurchaseMainViewModel_ViewLoaded(object? sender, EventArgs e)
    {
        ActiveForm("ActiveItemTop", typeof(SalePurchaseTopViewModel));
        ActiveForm("ActiveItemR", typeof(SalePurchaseRightViewModel));
    }

    public override bool SetIView(IView view)
    {
        _view = (ISalePurchaseMainView)view;
        return false;
    }

    public ICommand ButtonCommand => new RelayCommand<Button>(ButtonCommandClicked);
    private void ButtonCommandClicked(Button btn)
    {
        switch (btn.Tag)
        {
            case "ButtonClose":
                ClosePage(new string[] { "ActiveItem", "ActiveItemR", "ActiveItemTop" });
                break;

            default:
                break;
        }
    }

    private void ValidateOnChildActivated(object sender, ChildActivatedEventArgs e)
    {
        switch (e.ChildVMType)
        {

            default:
                break;
        }
    }
    public Task HandleAsync(SalePurchaseMainEventArgs message, CancellationToken cancellationToken)
    {
        if (message.EventType == "ExtButton")
        {
            ORDER_FUNC_KEY fnKey = (ORDER_FUNC_KEY)message.EventData;
            ProcessFuncKeyClicked(fnKey);
        }
        else if (message.EventType == "OpenVM")
        {
            string vmName = message.EventData.ToString();
            string pam = vmName.Contains("-") ? vmName.Substring(vmName.IndexOf("-") + 1) : string.Empty;
            if (vmName.Contains("-"))
                vmName = vmName.Substring(0, vmName.IndexOf("-"));
            ActiveForm("ActiveItemM", vmName, ValidateOnChildActivated, pam, message.EventData2, message.EventFlag);

        }

        return Task.CompletedTask;
    }
}
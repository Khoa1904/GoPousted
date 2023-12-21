using Caliburn.Micro;
using GoPOS.Common.Helpers;
using GoPOS.Common.ViewModels;
using GoPOS.ConfigSetup.Interface.ViewModel;
using GoPOS.ConfigSetup.ViewModels;
using GoPOS.Helpers;
using GoPOS.Helpers.CommandHelper;
using GoPOS.Models;
using GoPOS.Services;
using GoPOS.Views;
using GoShared.Helpers;
using System;
using System.Diagnostics;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace GoPOS.ViewModels;

/*
 환경설정 > 메인

 */
public class ConfigSetupMainViewModel : MainBasePageViewModel, IConfigSetupMainViewModel, IHandle<ConfigSetupMainViewEventArgs>
{
    private readonly IOrderPayService orderPayService;

    private IScreen? _ActiveItemM = null;
    public IScreen? ActiveItemM
    {
        get
        {
            return _ActiveItemM;
        }
        set
        {
            PutScreen(nameof(ActiveItemM), value);

            _ActiveItemM = value;
            _ActiveItemM?.ActivateAsync();

            NotifyOfPropertyChange(() => ActiveItemM);
        }
    }

    private IScreen? _ActiveItemR = null;
    public IScreen? ActiveItemR
    {
        get
        {
            return _ActiveItemR;
        }
        set
        {
            PutScreen(nameof(ActiveItemR), value);

            _ActiveItemR = value;
            _ActiveItemR?.ActivateAsync();

            NotifyOfPropertyChange(() => ActiveItemR);
        }
    }

    private IScreen? _ActiveItemTop = null;
    public IScreen? ActiveItemTop
    {
        get
        {
            return _ActiveItemTop;
        }
        set
        {
            PutScreen(nameof(ActiveItemTop), value);

            _ActiveItemTop = value;
            _ActiveItemTop?.ActivateAsync();

            NotifyOfPropertyChange(() => ActiveItemTop);
        }
    }

    public ConfigSetupMainViewModel(IWindowManager windowManager, IEventAggregator eventAggregator,
        IOrderPayService orderPayService) : base(windowManager, eventAggregator)
    {
        this.ViewLoaded += ConfigSetupMainViewModel_ViewLoaded;
        this.ViewInitialized += ConfigSetupMainViewModel_ViewInitialized;
        this.orderPayService = orderPayService;
    }

    private void ConfigSetupMainViewModel_ViewInitialized(object? sender, EventArgs e)
    {
        ActiveForm("ActiveItemTop", typeof(ConfigSetupTopViewModel));
        ActiveForm("ActiveItemR", typeof(ConfigSetupRightViewModel));
        ExtMenus = orderPayService.GetOrderFuncKey("05").Result.Item1;
    }

    private void ConfigSetupMainViewModel_ViewLoaded(object? sender, EventArgs e)
    {
    }

    public Task HandleAsync(ConfigSetupMainViewEventArgs message, CancellationToken cancellationToken)
    {
        if (message.EventType == "ExtButton")
        {
            ORDER_FUNC_KEY fnKey = (ORDER_FUNC_KEY)message.EventData;
            ProcessFuncKeyClicked(fnKey);
        }

        return Task.CompletedTask;
    }


    public ICommand ButtonCommand => new RelayCommand<Button>(ButtonCommandClicked);
    private void ButtonCommandClicked(Button btn)
    {
        switch (btn.Tag)
        {
            case "ButtonClose":
                ClosePage(new string[] { "ActiveItemR", "ActiveItemTop", "ActiveItemM" });
                break;

            default:
                break;
        }
    }

    /// <summary>
    /// validate before moving to child, or popup
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected override void ValidateOnChildActivated(object sender, ChildActivatedEventArgs e)
    {
        switch (e.ChildVMType)
        {

            default:
                break;
        }
    }

}
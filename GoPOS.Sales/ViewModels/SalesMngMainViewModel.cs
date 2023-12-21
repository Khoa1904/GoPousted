using Caliburn.Micro;
using GoPOS.Common.Helpers;
using GoPOS.Common.Interface.Model;
using GoPOS.Common.Interface.View;
using GoPOS.Common.ViewModels;
using GoPOS.Common.ViewModels.Controls;
using GoPOS.Helpers;
using GoPOS.Helpers.CommandHelper;
using GoPOS.Models;
using GoPOS.Models.Common;
using GoPOS.Sales.Interface.ViewModel;
using GoPOS.SalesMng.Interface.View;
using GoPOS.SalesMng.ViewModels;
using GoPOS.Services;

using GoPOS.Views;
using GoShared.Helpers;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;

namespace GoPOS.ViewModels;

public class SalesMngMainViewModel : MainBasePageViewModel, ISalesMngMainViewModel, IHandle<SalesMngMainViewEventArgs>
{
    private readonly IOrderPayService orderPayService;
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

    public SalesMngMainViewModel(IWindowManager windowManager, IEventAggregator eventAggregator,
        IOrderPayService orderPayService) : base(windowManager, eventAggregator)
    {
        this.ViewLoaded += SalesMngMainViewModel_ViewLoaded;
        this.ViewInitialized += SalesMngMainViewModel_ViewInitialized;
        this.orderPayService = orderPayService;
    }

    private void SalesMngMainViewModel_ViewInitialized(object? sender, EventArgs e)
    {
        ExtMenus = orderPayService.GetOrderFuncKey("01").Result.Item1;
        ActiveForm("ActiveItemTop", typeof(SalesMngTopViewModel));
        ActiveForm("ActiveItemR", typeof(SalesMngRightViewModel));
    }

    private void SalesMngMainViewModel_ViewLoaded(object? sender, EventArgs e)
    {
        ProcessFuncKeyClicked(ExtMenus[0]);
    }

    public Task HandleAsync(SalesMngMainViewEventArgs message, CancellationToken cancellationToken)
    {
        if (message.EventType == "ExtButton")
        {
            ORDER_FUNC_KEY fnKey = (ORDER_FUNC_KEY)message.EventData;
            if (fnKey.FK_NO == "112")
            {
                DialogHelper.MessageBox("중간정산 사용가능한 매장이 아닙니다.", GMessageBoxButton.OK, MessageBoxImage.Stop);
                return Task.CompletedTask;
            }
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

    public ICommand ButtonCommand => new RelayCommand<Button>(ButtonCommandClicked);
    private void ButtonCommandClicked(Button btn)
    {
        switch (btn.Tag)
        {
            case "ButtonClose":


                var time = DateTime.ParseExact(DataLocals.PosStatus != null ? DataLocals.PosStatus.SALE_DATE : DateTime.Now.ToString("yyyyMMdd"), "yyyyMMdd",
                Thread.CurrentThread.CurrentUICulture);

                IoC.Get<SalesPayPrepaymentViewModel>().DateFrom = time;
                IoC.Get<SalesPayPrepaymentViewModel>().DateTo = time;

                decimal reset = 0;
                IoC.Get<SalesPayPrepaymentViewModel>().PAY_AMT = 0;

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
        if (e.ChildVMType == "SalesNoticeViewModel")
        {
            return;
        }

        if (!ValidateSaleDate())
        {
            e.Cancelled = true;
            return;
        }

        switch (e.ChildVMType)
        {
            case "SalesClosingCancelViewModel":
                if ("2".Equals(DataLocals.PosStatus.CLOSE_FLAG) ||
                    ("1".Equals(DataLocals.PosStatus.CLOSE_FLAG) && Convert.ToInt32(DataLocals.PosStatus.BILL_NO) > 0))
                {
                    DialogHelper.MessageBox("마감전 상태입니다.", GMessageBoxButton.OK, MessageBoxImage.Warning);
                    e.Cancelled = true;
                    return;
                }
                break;
            case "SalesClosingSettleViewModel":

                e.Cancelled = true;
                
                // opening Middle Settle
                _eventAggregator.PublishOnUIThreadAsync(new SalesMngMainViewEventArgs()
                {
                    EventType = "OpenVM",
                    EventData = "SalesMiddleExcClcViewModel-FCLOSE"
                });
                break;

            case "SalesPayPrepaymentViewModel":
            case "SalesMiddleExcClcViewModel":
                if ("2".Equals(DataLocals.PosStatus.CLOSE_FLAG) && (e.CSData == null ||
                    (e.CSData as object[]).Length == 0))
                {
                    DialogHelper.MessageBox("개점전 상태입니다. 개설처리 후 사용하시기 바랍니다.", GMessageBoxButton.OK, MessageBoxImage.Warning);
                    e.Cancelled = true;
                    break;
                }

                if ("3".Equals(DataLocals.PosStatus.CLOSE_FLAG))
                {
                    DialogHelper.MessageBox("이미 마감된 상태입니다.", GMessageBoxButton.OK, MessageBoxImage.Warning);
                    e.Cancelled = true;
                    break;
                }
                break;
            default:
                break;
        }
    }

}
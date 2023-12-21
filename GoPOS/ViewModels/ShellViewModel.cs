using System;
using System.Diagnostics;
using System.Dynamic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Forms;
using Caliburn.Micro;
using GoPOS.Common.Helpers;
using GoPOS.Common.Interface.View;
using GoPOS.Common.Service;
using GoPOS.Common.Views.Controls;
using GoPOS.Helpers;
using GoPOS.Models.Common;
using GoPOS.Models.Custom.API;
using GoPOS.Properties;
using GoPOS.Servers;
using GoPOS.Service.Common;
using GoPOS.Service.Service.API;
using GoPOS.Services;
using GoPOS.Views;
using GoShared.Contract;
using GoShared.Events;
using GoShared.Helpers;
using MathNet.Numerics.RootFinding;
using Application = System.Windows.Application;

namespace GoPOS.ViewModels;

public class ShellViewModel : Conductor<IScreen>.Collection.OneActive, IShellViewModel, IHandle<GoShared.Events.CloseEventArgs>,
    IHandle<GoShared.Events.PageItemEventArgs>
{
    private readonly IWindowManager windowManager;
    private readonly IPOSInitService initService;
    //private readonly IPOSPrintService pOSPrintService;
    private IShellView _view;
    private ShellView mainView { get; set; }
    private SynchronizationContext _context;
    private ProgressBarView _progressBarView;
    public ShellViewModel(IEventAggregator eventAggregator, IWindowManager windowManager,
        IInfoShopService infoShopService, IPOSInitService initService,
        IPOSPrintService pOSPrintService)
    {

        eventAggregator.SubscribeOnUIThread(this);
        this.windowManager = windowManager;
        this.initService = initService;
        this.Deactivated += ShellViewModel_Deactivated;
    }

    protected override void OnViewLoaded(object view)
    {
        _view = view as IShellView;
        base.OnViewLoaded(view);
        _view.SynContext.Send(state => {
            try
            {
                mainView = IoC.Get<ShellView>();
                string errorMessage = string.Empty;

                var can = initService.POSCanStart(out errorMessage);

                if (can == 4)
                {
                    mainView.Hide();
                    //_view.Hide();
                    var diaResults = DialogHelper.ShowDialog_ShowInTaskbar(typeof(AuthenticLoginViewModel), 500, 750, 450, 150);
                    if (diaResults != null && diaResults.Count > 0 && diaResults.ContainsKey("RETURN_DATA"))
                    {
                        var response = diaResults["RETURN_DATA"] as Tuple<bool, ResponseModel>;
                        if (response.Item1 == true)
                        {
                            var processModel = IoC.Get<AuthenticProcessingViewModel>();
                            processModel.DataModel = response.Item2;

                            var header = new AuthenRequestHeader();
                            header.Token = response.Item2?.results?.token;
                            header.AuthId = response.Item2?.results?.authId;
                            processModel.headerData = header;

                            var processResult = DialogHelper.ShowDialog_ShowInTaskbar(typeof(AuthenticProcessingViewModel), 500, 750, 450, 150);

                            if (processResult != null && processResult.Count > 0 && processResult.ContainsKey("RETURN_DATA"))
                            {
                                var response2 = processResult["RETURN_DATA"];
                                if ((bool)response2)
                                {
                                    Settings.Default.SaveEmpNo = string.Empty;
                                    ActiveItem = IoC.Get<MainPageViewModel>();
                                    mainView.Show();

                                    return;
                                }
                                else
                                {
                                    Application.Current.Shutdown();
                                }
                            }
                        }
                        else
                        {
                            Application.Current.Shutdown();
                            return;
                        }
                    }
                }
                else if (can != 1)
                {
                    DialogHelper.MessageBox(errorMessage);
                    Application.Current.Shutdown();
                    return;
                }

                ActiveItem = IoC.Get<MainPageViewModel>();
            }
            catch (Exception)
            {

            }

        }, null);

    }

    private Task ShellViewModel_Deactivated(object sender, DeactivationEventArgs e)
    {
        return Task.CompletedTask;
    }

    #region Event
    public Task HandleAsync(GoShared.Events.CloseEventArgs message, CancellationToken cancellationToken)
    {
        return this.TryCloseAsync();
    }

    public Task HandleAsync(GoShared.Events.PageItemEventArgs data, CancellationToken cancellationToken)
    {
        if (data == null) return Task.CompletedTask;
        if (!"ShellViewModel".Equals(data.ParentType)) return Task.CompletedTask;

        if (data.EventType == GoShared.Events.PageItemEventTypes.ClosePage)
        {
            TryCloseAsync(true);
            return Task.CompletedTask;
        }

        var menu = DataLocals.TryGetMenu((string)data.EventData[0]);
        if (menu != null)
        {
            var viewModelType = System.Type.GetType(menu.ModelNamespace + "." + menu.ViewModelNm + ", " + menu.Assembly);
            var ins = (IScreen)IoC.GetInstance(viewModelType, null);
            ActiveItem = ins;
        }

        return Task.CompletedTask;
    }

    public void SetData(object data)
    {

    }

    public bool SetIView(IView view)
    {
        return true;
    }

    #endregion

}
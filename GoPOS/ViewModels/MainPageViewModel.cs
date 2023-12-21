using Caliburn.Micro;
using GoPOS.Common.ViewModels;
using GoPOS.Database;
using GoPOS.Models;
using GoPOS.Models.Common;
using GoPOS.Service;
using GoPOS.Services;
using GoShared.Events;
using GoShared.Helpers;
using GoShared.Interface;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using static GoShared.Events.GoPOSEventHandler;
using GoPOS.Common.ViewModels.Controls;
using System.IO;
using NPOI.HSSF.Record.Chart;
using System.Dynamic;
using System.Windows;
using GoPOS.OrderPay.Views;
using System.Windows.Controls;
using System.Windows.Media.Media3D;
using GoPOS.Helpers.CommandHelper;
using System.Windows.Input;

namespace GoPOS.ViewModels
{
    public class MainPageViewModel : BasePageViewModel, IHandle<BasePageClosedEventArgs>
    {
        #region Property
        public BasicHeaderControlViewModel TopBar { get; private set; }
        // public WebBrowserControlViewModel WebBrowser { get; private set; }
        public string Url { get; set; }
        private IScreen? _webBrowser = null;
        public IScreen? WebBrowser
        {
            get
            {
                return _webBrowser;
            }
            set
            {
                PutScreen(nameof(WebBrowser), value);
                _webBrowser = value;
                _webBrowser?.ActivateAsync();
                NotifyOfPropertyChange(() => WebBrowser);
            }
        }

        string mainPageUrl = string.Empty;
        #endregion
        public MainPageViewModel(IWindowManager windowManager, IEventAggregator eventAggregator) : base(windowManager, eventAggregator)
        {
            this.TopBar = new BasicHeaderControlViewModel(windowManager, eventAggregator);

            string mainFolder = new DirectoryInfo(System.AppDomain.CurrentDomain.BaseDirectory).Parent.FullName;
            mainFolder = mainFolder.Replace("\\", "/");
            Url = string.Format("file:///{0}/data/res/POSlogo.html", mainFolder);
            ActiveItem = (IScreen)IoC.Get<InitViewModel>();
            this.Activated += MainPageViewModel_Activated;
        }

        public void ClozeProgram(Button btn)
        {
            this.TryCloseAsync();
            Application.Current.Shutdown();
        }

        private void MainPageViewModel_Activated(object? sender, ActivationEventArgs e)
        {
            this.WebBrowser = new WebBrowserControlViewModel(_windowManager,_eventAggregator, Url);
        }

        public ICommand CloseProgram => new RelayCommand<Button>(ClozeProgram);

        public Task HandleAsync(BasePageClosedEventArgs message, CancellationToken cancellationToken)
        {
            //this.WebBrowser.BrowserUrl = mainPageUrl;
            return Task.CompletedTask;
        }



    }
}

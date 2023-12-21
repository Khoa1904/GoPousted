using Caliburn.Micro;
using GoPOS.Common.ViewModels;
using GoPOS.Common.ViewModels.Controls;
using GoPOS.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoPOS.ViewModels
{
    public class DualScreenOrderingViewModel : BasePageViewModel
    {
        #region Member
        private readonly OrderPayMainViewModel _orderPayMainViewModel;
        private IScreen? _webBrowser = null;
        private ORDER_SUM_INFO orderSumInfo;
        #endregion

        #region Property
        public OrderPayMainViewModel EditEntity => _orderPayMainViewModel;

        public ORDER_SUM_INFO OrderSumInfo
        {
            get => orderSumInfo; set
            {
                orderSumInfo = value;
                NotifyOfPropertyChange(() => OrderSumInfo);
            }
        }

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
        public string Url { get; set; }

        #endregion       


        public DualScreenOrderingViewModel(IWindowManager windowManager, IEventAggregator eventAggregator, OrderPayMainViewModel orderPayMainViewModel) : base(windowManager, eventAggregator)
        {
            string mainFolder = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory).Parent.FullName;
            mainFolder = mainFolder.Replace("\\", "/");
            Url = string.Format("file:///{0}/data/res/POSlogo.html", mainFolder);
            Activated += Waiting_Activated;
            _orderPayMainViewModel = orderPayMainViewModel;
        }

        private void Waiting_Activated(object? sender, ActivationEventArgs e)
        {
            WebBrowser = new WebBrowserControlViewModel(_windowManager, _eventAggregator, Url);
        }
    }
}

using Caliburn.Micro;
using GoPOS.Common.ViewModels.Controls;
using GoPOS.Models;
using GoPOS.Models.Common;
using GoPOS.Services;
using GoShared.Helpers;
using System.Windows;
using System.Windows.Controls;
/*
 

 */

namespace GoPOS.ViewModels
{

    public class SalesNoticeViewModel : OrderPayChildViewModel
    {
        private string defaultBrowserUrl;
        public SalesNoticeViewModel(IWindowManager windowManager, IEventAggregator eventAggregator) : base(windowManager, eventAggregator)
        {
            //  this.ViewLoaded += NoticeViewloaded;
            this.ViewLoaded += SalesNoticeViewModel_ViewLoaded;
            
            //this.Activated += SalesNoticeViewModel_Activated;
            Init();
        }

        private void SalesNoticeViewModel_ViewLoaded(object? sender, EventArgs e)
        {
            string token = DataLocals.TokenInfo.TOKEN;
            defaultBrowserUrl = DataLocals.AppConfig.PosComm.AspURLServer + "/pos/login?accessTkn=" + token;
            LogHelper.Logger.Info("SalesNoticeViewModel: " + defaultBrowserUrl);
            this.WebBrowser = new WebBrowserControlViewModel(_windowManager, _eventAggregator, defaultBrowserUrl);
        }

        private void SalesNoticeViewModel_Activated(object? sender, ActivationEventArgs e)
        {
            
        }

        //public WebBrowserControlViewModel WebBrowser { get; private set; }
        private IScreen? _webBrowser = null;
        public IScreen? WebBrowser
        {
            get
            {
                return _webBrowser;
            }
            set
            {
                //PutScreen(nameof(WebBrowser), value);
                _webBrowser = value;
                _webBrowser?.ActivateAsync();
                NotifyOfPropertyChange(() => WebBrowser);
            }
        }

        private void NoticeViewloaded (object? sender, EventArgs e)
        {
            
        }

        private async void Init()
        {
            await Task.Delay(100);
        }


        public void ButtonMenu(object sender, RoutedEventArgs e)
        {
            if (sender is not Button btn)
            {
                return;
            }
        }
        public void ButtonClose(object sender, RoutedEventArgs e)
        {
            try
            {
                MST_INFO_EMP data = new MST_INFO_EMP();

                //ShellViewModel.menu_nm = "MainViewModel";//어느 페이지로 이동할지 셋팅

                //_eventAggregator.PublishOnUIThreadAsync(data);
            }
            catch (Exception ex)
            {
                LogHelper.Logger.Error("프로그램 종료 오류 : " + ex.Message);
            }
        }
    }
}
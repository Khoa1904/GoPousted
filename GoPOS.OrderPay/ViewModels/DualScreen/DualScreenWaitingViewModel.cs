using Caliburn.Micro;
using GoPOS.Common.ViewModels;
using GoPOS.Common.ViewModels.Controls;
using GoPOS.Common.Views.Controls;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace GoPOS.ViewModels
{
    public class DualScreenWaitingViewModel : BasePageViewModel
    {
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
        private SubWebBrowserControlViewModel _gifImage = null;
        public SubWebBrowserControlViewModel? GifImage
        {
            get
            {
                return _gifImage;
            }
            set
            {
                _gifImage = value;
                NotifyOfPropertyChange(() => GifImage);
            }
        }
        public string Url { get; set; }
        public DualScreenWaitingViewModel(IWindowManager windowManager, IEventAggregator eventAggregator) : base(windowManager, eventAggregator)
        {
            string mainFolder = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory).Parent.FullName;
            mainFolder = mainFolder.Replace("\\", "/");
            Url = string.Format("file:///{0}/data/res/POSlogo_D.html", mainFolder);
            Activated += Waiting_Activated;
        }

        private void Waiting_Activated(object? sender, ActivationEventArgs e)
        {
            WebBrowser = new SubWebBrowserControlViewModel(_windowManager, _eventAggregator, Url);
            GifImage = new (_windowManager, _eventAggregator, Url);
        }
    }
}

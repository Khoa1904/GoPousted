using Caliburn.Micro;
using GoPOS.Common.Interface.View;

namespace GoPOS.Common.ViewModels.Controls
{
    public class SubWebBrowserControlViewModel : BaseItemViewModel
    {
        private IWebBrowserControlView _view;

        private string defaultBrowserUrl = "https://wordoc.outlier.kr/pos/ad1";     // 정사각형 광고(AD)
        private string defaultBrowserUrl2 = "https://wordoc.outlier.kr/pos/ad2";    // 4:3 광고(AD)

        private string browserUrl;

        public SubWebBrowserControlViewModel(IWindowManager windowManager, IEventAggregator eventAggregator,
            string browserUrl) : base(windowManager, eventAggregator)
        {
            this.BrowserUrl = browserUrl;

        }

        public override bool SetIView(IView view)
        {
            _view = (IWebBrowserControlView)view;
            return base.SetIView(view);
        }

        public string BrowserUrl
        {
            get
            {
                if (string.IsNullOrEmpty(browserUrl))
                    browserUrl = defaultBrowserUrl;

                return browserUrl;
            }
            set
            {
                browserUrl = value;
                NotifyOfPropertyChange(() => BrowserUrl);
                if (_view != null) _view.RefreshBrowser();
            }
        }

    }
}

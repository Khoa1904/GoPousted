using GoPOS.Common.Interface.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GoPOS.Common.Views.Controls
{
    /// <summary>
    /// WebBrowserControlView.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class SubWebBrowserControlView : UCViewBase, IWebBrowserControlView
    {
        public SubWebBrowserControlView()
        {
            InitializeComponent();
            MyWebView.EnsureCoreWebView2Async();
        }

        public void RefreshBrowser()
        {
            MyWebView.Reload();
        }

        private void WebView2_NavigationCompleted(object sender, Microsoft.Web.WebView2.Core.CoreWebView2NavigationCompletedEventArgs e)
        {
            // Web 페이지 보여질 때 Scrollbar 제거 코드
            //if (e.IsSuccess)
            //{
            //    ((Microsoft.Web.WebView2.Wpf.WebView2)sender).ExecuteScriptAsync("document.querySelector('body').style.overflow='hidden'");
            //}
            if (e.IsSuccess)
            {
                ((Microsoft.Web.WebView2.Wpf.WebView2)sender).ExecuteScriptAsync("document.querySelector('body').style.overflow='scroll';var style=document.createElement('style');style.type='text/css';style.innerHTML='::-webkit-scrollbar{display:none}';document.getElementsByTagName('body')[0].appendChild(style)");
            }
        }
    }
}

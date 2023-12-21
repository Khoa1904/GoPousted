using GoPOS.Common.Views;
using System.ComponentModel;
using System.Windows.Controls;

namespace GoPOS.Views
{
    public partial class SalesNoticeView : UCViewBase
    {
        public SalesNoticeView()
        {
            InitializeComponent();
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
        //[Category("ImageUrl"), Description("이미지Url")]
        //public System.Windows.Media.ImageSource ImageUrl
        //{
        //    get
        //    {
        //        return this.img.Source;
        //    }
        //    set
        //    {
        //        this.img.Source = value;
        //    }
        //}
    }
}

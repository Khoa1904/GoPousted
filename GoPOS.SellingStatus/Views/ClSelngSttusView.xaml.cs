using GoPOS.Common.Views;
using GoPOS.Models;
using GoPOS.SellingStatus.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
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
using GoPOS.Common.Helpers.Controls;

namespace GoPOS.Views
{
    /// <summary>
    /// ClSelngSttusView.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class ClSelngSttusView : UCViewBase, IClSelngSttusView
    {
        public ClSelngSttusView()
        {
            InitializeComponent();
        }

        public void LoadLineGrid()
        {
            throw new NotImplementedException();
        }

        public void LoadMainGrid()
        {
            throw new NotImplementedException();
        }

        public void RenderExtButtons(ORDER_FUNC_KEY[] funcKeys)
        {
            throw new NotImplementedException();
        }
       

        public Point buttonPosition { get; set; }

        private void ButtonClick(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            buttonPosition = PointHelper.GetPointOfButton(btn,this);
        }

        public void ScrollToEnd()
        {
            throw new NotImplementedException();
        }
    }
}

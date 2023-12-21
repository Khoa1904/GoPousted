using GoPOS.Common.Helpers.Controls;
using GoPOS.Common.Views;
using GoPOS.SellingStatus.Interface;
using System.Collections.Generic;
using System.Linq;
using System.Printing;
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
using Caliburn.Micro;
using GoPOS.OrderPay.ViewModels.Controls;
using GoPOS.ViewModels;
using System.Collections.Generic;
using System.ComponentModel;

namespace GoPOS.Views
{
    /// <summary>
    /// PaymntSelngSttusView.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class PaymntSelngSttusView : UCViewBase, IPaymentSelngStatusView
    {
        public PaymntSelngSttusView():base()
        {
            InitializeComponent();
        }
        
        public Point buttonPosition { get; set; }
        private void ButtonClick(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            buttonPosition = PointHelper.GetPointOfButton(btn, this);
        }


    }
}

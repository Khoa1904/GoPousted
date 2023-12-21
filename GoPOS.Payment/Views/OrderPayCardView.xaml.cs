using GoPOS.Common.Views;
using GoPOS.Payment.Interface.View;
using System.ComponentModel;
using System.Windows.Controls;

namespace GoPOS.Views
{
    public partial class OrderPayCardView : UCViewBase, IOrderPayCardView
    {
        public OrderPayCardView()
        {
            InitializeComponent();
        }

        public void RenderListOfCardCompanies()
        {
            
        }
    }
}

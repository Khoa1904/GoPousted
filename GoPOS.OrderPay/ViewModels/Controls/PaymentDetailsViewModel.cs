using Caliburn.Micro;
using GoPOS.Common.Interface.Model;
using GoPOS.Common.Interface.View;
using GoPOS.Common.ViewModels;
using GoPOS.Models;
using GoPOS.Models.Custom.Payment;
using GoPOS.Service;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoPOS.OrderPay.ViewModels.Controls
{
    public class PaymentDetailsViewModel : BaseItemViewModel
    {
        public ObservableCollection<COMPPAY_PAY_INFO> TENDER { get; set; }

        public PaymentDetailsViewModel(IWindowManager windowManager, IEventAggregator eventAggregator) : 
            base(windowManager, eventAggregator)
        {

        }

        public void UpdateTranTenderSeq(List<COMPPAY_PAY_INFO> tRN_TENDERSEQs)
        {
            this.TENDER = new ObservableCollection<COMPPAY_PAY_INFO>(tRN_TENDERSEQs);
            NotifyOfPropertyChange(() => TENDER);
        }
    }
}

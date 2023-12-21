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
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace GoPOS.OrderPay.ViewModels.Controls
{
    public class PaymentInfoViewModel : BaseItemViewModel
    {
        private ORDER_SUM_INFO oRDER_INFO;

        public ORDER_SUM_INFO ORDER_INFO
        {
            get => oRDER_INFO; set
            {
                oRDER_INFO = value;
                NotifyOfPropertyChange(() => ORDER_INFO);
            }
        }
        public PaymentInfoViewModel(IWindowManager windowManager, IEventAggregator eventAggregator) : base(windowManager, eventAggregator)
        {

        }

        //public Task HandleAsync(OrderPayChildDoUpdateEventArgs message, CancellationToken cancellationToken)
        //{
        //    if (this.GetType().Name.Equals(message.ChildName))
        //    {
        //        this.ORDER_INFO = (ORDER_SUM_INFO)message.SendData;
        //    }

        //    return Task.CompletedTask;
        //}

        public void UpdateOrderValue(ORDER_SUM_INFO orderitems)
        {
            this.ORDER_INFO = orderitems;
        }
    }
}

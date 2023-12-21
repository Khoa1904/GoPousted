using Caliburn.Micro;
using GoPOS.Common.Interface.View;
using GoPOS.Models.Common;
using GoPOS.OrderPay.Interface.View;
using GoPOS.Services;
using GoPOS.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoPOS.ViewModels
{
    public class OrderPayReceiptDetailsViewModel : OrderPayChildViewModel
    {
        private IOrderPayReceiptDetailsView _view;
        private readonly IOrderPayReceiptDetailsService _receiptDetailService;

        public OrderPayReceiptDetailsViewModel(IWindowManager windowManager, IEventAggregator eventAggregator)
            : base(windowManager, eventAggregator)
        {            
        }

        public override bool SetIView(IView view)
        {
            _view = (IOrderPayReceiptDetailsView)view;
            return base.SetIView(view);
        }

        public string ReceiptContents { get; set; }

        public override void SetData(object data)
        {
            object[] datas = data as object[];
            ReceiptContents = (string)datas[0];
            NotifyOfPropertyChange(nameof(ReceiptContents));
        }
    }
}

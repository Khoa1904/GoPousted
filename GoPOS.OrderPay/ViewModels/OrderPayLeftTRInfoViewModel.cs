using Caliburn.Micro;
using GoPOS.Common.ViewModels;
using GoPOS.Common.Views;
using GoPOS.Helpers.CommandHelper;
using GoPOS.OrderPay.Interface.ViewModel;
using GoPOS.OrderPay.Models;
using GoPOS.OrderPay.ViewModels.Controls;
using System;
using System.Collections.Generic;
using System.DirectoryServices.Protocols;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace GoPOS.ViewModels
{
    public class OrderPayLeftTRInfoViewModel : BaseItemViewModel
    {
        private bool _buttonClick { get; set; } = true;
        public bool ButtonClick
        {
            get => _buttonClick;
            set
            {
                _buttonClick = value;
                NotifyOfPropertyChange(() => ButtonClick);
            }
        }
        public OrderPayLeftTRInfoViewModel(IWindowManager windowManager, IEventAggregator eventAggregator) : base(windowManager, eventAggregator)
        {
            this.ActiveItem = IoC.Get<PaymentInfoViewModel>();
            this.ViewLoaded += Screen_viewload;
        }

        public ICommand ButtonCommand => new RelayCommand<Button>(ButtonClicked);

        private void Screen_viewload(object sender, EventArgs e)
        {
            this.ActiveItem = IoC.Get<PaymentInfoViewModel>();
        }
        private void ButtonClicked(Button button)
        {
            if (!ButtonClick)
            {
                return;
            }
            switch (button.Tag)
            {
                /// 결제정보
                case "PaymentInfo": // 결제정보
                    this.ActiveItem.TryCloseAsync();
                    this.ActiveItem = IoC.Get<PaymentInfoViewModel>();
                    break;

                case "PaymentDetails": // 결제내역
                    this.ActiveItem.TryCloseAsync();
                    this.ActiveItem = IoC.Get<PaymentDetailsViewModel>();
                    break;

                case "MemberInfo": // 멤버정보
                    this.ActiveItem.TryCloseAsync();
                    this.ActiveItem = IoC.Get<MemberInfoViewModel>();
                    break;

                default:
                    break;
            }
        }

    }
}

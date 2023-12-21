using Caliburn.Micro;
using GoPOS.Common.Interface.View;
using GoPOS.Common.ViewModels;
using GoPOS.Common.Views;
using GoPOS.Helpers.CommandHelper;
using GoPOS.OrderPay.Interface.View;
using GoPOS.OrderPay.Interface.ViewModel;
using GoPOS.OrderPay.Models;
using GoPOS.OrderPay.ViewModels.Controls;
using GoPOS.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using IView = GoPOS.Common.Interface.View.IView;

namespace GoPOS.ViewModels
{
    public class OrderPayLeftInfoKeypadViewModel : OrderPayChildViewModel, IHandle<OrderPayDiscHandleEventArgs>
    {
        private IOrderPayLeftInfoKeypadView _view;

        public InputBoxKeyPadViewModel KeyPad { get; set; }
        public string Text
        {
            get
            {
                string t = KeyPad.Text;
                KeyPad.ClearText();
                return string.IsNullOrEmpty(t) ? "0" : t;
            }
        }

        public override bool SetIView(IView view)
        {
            _view = (IOrderPayLeftInfoKeypadView)view;
            return base.SetIView(view);
        }


        public void SetKeyPadFocus()
        {
            _view.SetKeyPadFocus();
        }

        public void ClearText()
        {
            KeyPad.ClearText();
        }

        public OrderPayLeftInfoKeypadViewModel(IWindowManager windowManager, IEventAggregator eventAggregator) : base(windowManager, eventAggregator)
        {
            KeyPad = IoC.Get<InputBoxKeyPadViewModel>();
            ActiveItem = IoC.Get<OrderPayLeftTRInfoViewModel>();
            ViewLoaded += OrderPayLeftInfoKeypadViewModel_ViewLoaded;
        }

        private void OrderPayLeftInfoKeypadViewModel_ViewLoaded(object? sender, EventArgs e)
        {
            _view.Focus();
        }

        public Task HandleAsync(OrderPayDiscHandleEventArgs message, CancellationToken cancellationToken)
        {
            if (message.Action == OrderPayDiscHandleActions.Show)
            {
                this.ActiveItem.TryCloseAsync();
                this.ActiveItem = IoC.Get<OrderPayLeftDiscHandlerViewModel>();
            }
            else if (message.Action == OrderPayDiscHandleActions.CloseLeft ||
                message.Action == OrderPayDiscHandleActions.CloseRight)
            {
                this.ActiveItem.TryCloseAsync();
                this.ActiveItem = IoC.Get<OrderPayLeftTRInfoViewModel>();
            }

            return Task.CompletedTask;
        }
    }
}


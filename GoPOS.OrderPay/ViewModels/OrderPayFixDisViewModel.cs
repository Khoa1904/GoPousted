using Caliburn.Micro;
using Dapper;
using GoPOS.Common.Interface.Model;
using GoPOS.Common.Interface.View;
using GoPOS.Common.ViewModels;
using GoPOS.Helpers;
using GoPOS.Helpers.CommandHelper;
using GoPOS.Models;
using GoPOS.Models.Common;
using GoPOS.OrderPay.Interface.ViewModel;
using GoPOS.OrderPay.Models;
using GoPOS.Payment.Interface.View;
using GoPOS.Services;

using GoPOS.Views;
using GoShared.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using static GoPOS.Function;

/*
 
    ---------------------------------------------------------------------------------------------------------------------------------
    고정할인
    ---------------------------------------------------------------------------------------------------------------------------------


 */

namespace GoPOS.ViewModels
{
    public class OrderPayFixDisViewModel : OrderPayChildViewModel
    {
        private readonly IOrderPayService orderPayService;
        private IOrderPayFixDisView _view;

        public override object ActivateType => "ShowDiscHandler";

        public OrderPayFixDisViewModel(IWindowManager windowManager,
            IEventAggregator eventAggregator,
            IOrderPayService orderPayService) :
            base(windowManager, eventAggregator)
        {
            this.orderPayService = orderPayService;
            this.ViewInitialized += OrderPayFixDisViewModel_ViewInitialized;
            this.ViewLoaded += OrderPayFixDisViewModel_ViewLoaded;
            this.ViewUnloaded += OrderPayFixDisViewModel_ViewUnloaded;
        }

        private void OrderPayFixDisViewModel_ViewInitialized(object? sender, EventArgs e)
        {
            var fixDcs = orderPayService.GetFixDcList().Result.ToArray();
            _view.RenderDiscountOptions(fixDcs);
        }

        private void OrderPayFixDisViewModel_ViewUnloaded(object? sender, EventArgs e)
        {
            _eventAggregator.PublishOnUIThreadAsync(new OrderPayDiscHandleEventArgs()
            {
                Action = OrderPayDiscHandleActions.CloseLeft
            });
        }

        private void OrderPayFixDisViewModel_ViewLoaded(object? sender, EventArgs e)
        {
            _eventAggregator.PublishOnUIThreadAsync(new OrderPayDiscHandleEventArgs()
            {
                Action = OrderPayDiscHandleActions.Show
            });
        }

        public override bool SetIView(IView view)
        {
            _view = (IOrderPayFixDisView)view;
            return base.SetIView(view);
        }

        #region Commands and public

        /// <summary>
        /// 
        /// </summary>
        public ICommand ButtonCommand => new RelayCommand<Button>((button) =>
        {
            switch (button.Tag)
            {
                case "AllDiscCancel":
                    ((IOrderPayMainViewModel)this.Parent).DiscountApply(true, false, false, 0);
                    DeactivateClose(true);
                    break;
                case "SelDiscCancel":
                    ((IOrderPayMainViewModel)this.Parent).DiscountApply(false, false, false, 0);
                    DeactivateClose(true);
                    break;
                default:
                    break;
            }
        });

        public ICommand AllAmtDiscCommand => new RelayCommand<Button>((button) =>
        {
            MST_INFO_FIX_DC fixDc = (MST_INFO_FIX_DC)button.Tag;
            if (fixDc != null)
            {
                ((IOrderPayMainViewModel)this.Parent).DiscountApply(true, true, true, fixDc.DC_VALUE);
                if ("1".Equals(fixDc.CLOSE_YN) || "Y".Equals(fixDc.CLOSE_YN)) this.DeactivateClose(true);
            }
        });

        public ICommand AllPerDiscCommand => new RelayCommand<Button>((button) =>
        {
            MST_INFO_FIX_DC fixDc = (MST_INFO_FIX_DC)button.Tag;
            if (fixDc != null)
            {
                ((IOrderPayMainViewModel)this.Parent).DiscountApply(true, false, true, fixDc.DC_VALUE);
                if ("1".Equals(fixDc.CLOSE_YN) || "Y".Equals(fixDc.CLOSE_YN)) this.DeactivateClose(true);
            }
        });

        public ICommand SelAmtDiscCommand => new RelayCommand<Button>((button) =>
        {
            MST_INFO_FIX_DC fixDc = (MST_INFO_FIX_DC)button.Tag;
            if (fixDc != null)
            {
                ((IOrderPayMainViewModel)this.Parent).DiscountApply(false, true, true, fixDc.DC_VALUE);
                if ("1".Equals(fixDc.CLOSE_YN) || "Y".Equals(fixDc.CLOSE_YN)) this.DeactivateClose(true);
            }
        });

        public ICommand SelPerDiscCommand => new RelayCommand<Button>((button) =>
        {
            MST_INFO_FIX_DC fixDc = (MST_INFO_FIX_DC)button.Tag;
            if (fixDc != null)
            {
                ((IOrderPayMainViewModel)this.Parent).DiscountApply(false, false, true, fixDc.DC_VALUE);
                if ("1".Equals(fixDc.CLOSE_YN) || "Y".Equals(fixDc.CLOSE_YN)) this.DeactivateClose(true);
            }
        });

        #endregion
    }

}

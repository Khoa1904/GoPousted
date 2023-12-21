using Caliburn.Micro;
using GoPOS.Common.Interface.Model;
using GoPOS.Common.Service;
using GoPOS.Common.ViewModels;
using GoPOS.Services;
using GoShared.Helpers;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace GoPOS.ViewModels
{
    public enum EDislayType { Logo, Order }
    public class DualScreenMainViewModel : Conductor<IScreen>.Collection.OneActive
    {

        //**---------------------------------------------------------------------

        #region Member
        private readonly OrderPayMainViewModel _orderPayMainViewModel;
        //DispatcherTimer _timer;
        private EDislayType _eDislayType = EDislayType.Logo;

        public double MainHeight { get; set; } = 768;
        public double MainWidth { get; set; } = 1024;
        #endregion

        //**---------------------------------------------------------------------

        #region Property
        #endregion

        //**---------------------------------------------------------------------

        #region Contructor

        public DualScreenMainViewModel(IWindowManager windowManager, IEventAggregator eventAggregator,
           IInfoShopService infoShopService, IPOSInitService initService,
           IPOSPrintService pOSPrintService, OrderPayMainViewModel orderPayMainViewModel)
        {
            ActiveItem = IoC.Get<DualScreenOrderingViewModel>();
            _eDislayType = EDislayType.Logo;

            eventAggregator.SubscribeOnUIThread(this);
            _orderPayMainViewModel = orderPayMainViewModel;
            _orderPayMainViewModel.Deactivated += OrderPayMainViewModel_Deactivated;
            _orderPayMainViewModel.Activated += OrderPayMainViewModel_Activated;

            if (_orderPayMainViewModel.IsActive)
            {
                ActiveItem = IoC.Get<DualScreenOrderingViewModel>();
                _eDislayType = EDislayType.Order;
            }
            else
            {
                ActiveItem = IoC.Get<DualScreenWaitingViewModel>();
                _eDislayType = EDislayType.Logo;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="eDislayType"></param>
        public void SwitchMode(EDislayType eDislayType)
        {
            if (_eDislayType == eDislayType)
            {
                return;
            }

            if (eDislayType == EDislayType.Order)
            {
                ActiveItem = IoC.Get<DualScreenOrderingViewModel>();
            }
            else
            {
                ActiveItem = IoC.Get<DualScreenWaitingViewModel>();
            }

            _eDislayType = eDislayType;
        }

        private void OrderPayMainViewModel_Activated(object? sender, ActivationEventArgs e)
        {
         //   ActiveItem = IoC.Get<DualScreenOrderingViewModel>();
         //   _eDislayType = EDislayType.Order;
        }

        private Task OrderPayMainViewModel_Deactivated(object sender, DeactivationEventArgs e)
        {
            ActiveItem = IoC.Get<DualScreenWaitingViewModel>();
            _eDislayType = EDislayType.Logo;
            return Task.CompletedTask;
        }
        #endregion

        //**---------------------------------------------------------------------

    }
}

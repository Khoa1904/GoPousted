using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

using Caliburn.Micro;
using GoPOS.Models;
using GoPOS.Services;
using GoPOS.Common.ViewModels.Controls;

namespace GoPOS.ViewModels
{
    public class ExcclcSttusConfirmDetailViewModel : Screen
    {
        private IWindowManager _windowManager;
        private IEventAggregator _eventAggregator;

        private IInfoEmpService _empMService;
        private IInfoShopService _shopMService;

        public ExcclcSttusConfirmDetailViewModel(IWindowManager windowManager, IEventAggregator eventAggregator, IInfoEmpService empMService, IInfoShopService shopMService)
        {
            _windowManager = windowManager;
            _eventAggregator = eventAggregator;

            _empMService = empMService;
            _shopMService = shopMService;

            _eventAggregator.SubscribeOnUIThread(this);
        }

        public void Close()
        {
            IoC.Get<ExcclcSttusConfirmDetailViewModel>().TryCloseAsync(true);
        }
    }
}

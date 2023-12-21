using Caliburn.Micro;
using Dapper;
using GoPOS.Common.Interface.Model;
using GoPOS.Common.Interface.View;
using GoPOS.Common.ViewModels;
using GoPOS.Helpers;
using GoPOS.Helpers.CommandHelper;
using GoPOS.Models;
using GoPOS.Models.Common;
using GoPOS.SalePurchase.Interface.View;
using GoPOS.SalePurchase.Interface.ViewModel;
using GoPOS.SalePurchase.ViewModels;
using GoPOS.Services;

using GoPOS.Views;
using GoShared.Helpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Services.Description;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace GoPOS.ViewModels
{
    public class SalePurchaseRightViewModel : BaseItemViewModel, IMainMgrPageRightViewModel
    {
        private readonly IOrderPayService orderPayService;
        private ISalePurchaseMainViewModel _salePurchaseMainViewModel;
        private ISalePurchaseRightView _view;
        private int totalPage = 0;
        private int currentPage = -1;
        public InputKeyPadViewModel KeyPad { get; set; }

        List<ORDER_FUNC_KEY>? ExtMenus { get; set; }
        public SalePurchaseRightViewModel(IWindowManager windowManager, IEventAggregator eventAggregator,
            IOrderPayService orderPayService) : base(windowManager, eventAggregator)
        {            
            this.ViewInitialized += SalePurchaseRightViewInit;
            this.ViewLoaded += SalePurchaseRightViewLoad;
            this.orderPayService = orderPayService;
        }


        public int CurrentPage
        {
            get
            {
                return currentPage;
            }
            set
            {
                currentPage = value;
                if (currentPage < 0)
                {
                    currentPage = 0;
                }
                if (currentPage > totalPage - 1)
                {
                    currentPage = totalPage - 1;
                }
                var funcKeys = ExtMenus.Page(10, currentPage).ToArray();
                _view.RenderExtButtons(funcKeys);
            }
        }

        public override bool SetIView(IView view)
        {
            _view = (ISalePurchaseRightView)view;
            return base.SetIView(view);
        }

        private void SalePurchaseRightViewLoad(object? sender, EventArgs e)
        {
            totalPage = ExtMenus.Count / 10 + (ExtMenus.Count % 10 == 0 ? 0 : 1);
            CurrentPage = 0;
        }

        private void SalePurchaseRightViewInit(object? sender, EventArgs e)
        {
            _salePurchaseMainViewModel = (ISalePurchaseMainViewModel)Parent;
            ExtMenus = orderPayService.GetOrderFuncKey("00").Result.Item1;
        }

        public void SelectFirstExtMenu(ORDER_FUNC_KEY funcKey)
        {
            _eventAggregator.PublishOnUIThreadAsync(new SalePurchaseMainEventArgs()
            {
                EventType = "ExtButton",
                EventData = funcKey
            });
        }

        public ICommand ButtonCommand => new RelayCommand<Button>((button) =>
        {
            if (button.Tag == null) return;

            switch (button.Tag)
            {
                case "Prev":
                    CurrentPage--;
                    break;
                case "Next":
                    CurrentPage++;
                    break;
                default:
                    _eventAggregator.PublishOnUIThreadAsync(new SalePurchaseMainEventArgs()
                    {
                        EventType = "ExtButton",
                        EventData = button.Tag
                    });
                    break;
            }
        });



    }
}
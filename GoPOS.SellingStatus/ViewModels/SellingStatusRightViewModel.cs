using Caliburn.Micro;
using Dapper;
using GoPOS.Common.Interface.Model;
using GoPOS.Common.Interface.View;
using GoPOS.Common.ViewModels;
using GoPOS.Helpers;
using GoPOS.Helpers.CommandHelper;
using GoPOS.Interface;
using GoPOS.Models;
using GoPOS.Models.Common;
using GoPOS.SellingStatus.Interface;
using GoPOS.SellingStatus.ViewModels;
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
    /*
     매출현황 > 우측 메뉴바

     */


    public class SellingStatusRightViewModel : BaseItemViewModel, IMainMgrPageRightViewModel
    {
        private readonly IOrderPayService ImportedOrderService;
        private SellingStatusMainViewModel _sellingStatusMainViewModel;
        private int totalPage = 0;
        private int currentPage = -1;
        private ISellingStatusRightView _view;
        public InputKeyPadViewModel KeyPad { get; set; }

        List<ORDER_FUNC_KEY>? ExtMenus { get; set; }
        public SellingStatusRightViewModel(IWindowManager windowManager, IEventAggregator eventAggregator,
            IOrderPayService ImportedOrderService) : base(windowManager, eventAggregator)
        {
            this.ImportedOrderService = ImportedOrderService;
            this.ViewInitialized += SellingsStatusRightViewInit;
            this.ViewLoaded += SellingsStatusRightViewLoad;
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
            _view = (ISellingStatusRightView)view;
            return base.SetIView(view);
        }

        private void SellingsStatusRightViewLoad(object? sender, EventArgs e)
        {
            totalPage = ExtMenus.Count / 10 + (ExtMenus.Count % 10 == 0 ? 0 : 1);
            CurrentPage = 0;
        }

        private void SellingsStatusRightViewInit(object? sender, EventArgs e)
        {
            ExtMenus = ImportedOrderService.GetOrderFuncKey("03").Result.Item1;
        }

        public void SelectFirstExtMenu(ORDER_FUNC_KEY funcKey)
        {
            _eventAggregator.PublishOnUIThreadAsync(new SellingStatusMainEventArgs()
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
                    _eventAggregator.PublishOnUIThreadAsync(new SellingStatusMainEventArgs()
                    {
                        EventType = "ExtButton",
                        EventData = button.Tag
                    });
                    break;
            }
        });



    }
}
using Caliburn.Micro;
using Dapper;
using GoPOS.Common.Interface.Model;
using GoPOS.Common.Interface.View;
using GoPOS.Common.ViewModels;
using GoPOS.Helpers;
using GoPOS.Helpers.CommandHelper;
using GoPOS.Models;
using GoPOS.Models.Common;
using GoPOS.Sales.Interface.ViewModel;
using GoPOS.SalesMng.Interface.View;
using GoPOS.SalesMng.ViewModels;
using GoPOS.Services;

using GoPOS.Views;
using GoShared.Helpers;
using Microsoft.AspNetCore.Server.IIS.Core;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace GoPOS.ViewModels
{
    /*
     영업관리 > 우측 메뉴바
     */
    public class SalesMngRightViewModel : BaseItemViewModel, IMainMgrPageRightViewModel
    {
        public InputKeyPadViewModel KeyPad { get; set; }
        private ISalesMngRightView _view;
        private ISalesMngMainViewModel _parentViewModel;
        public SalesMngRightViewModel(IWindowManager windowManager, IEventAggregator eventAggregator,
            IOrderPayService orderPayService) : base(windowManager, eventAggregator)
        {
            this.orderPayService = orderPayService;
            this.ViewLoaded += SalesMngRightViewModel_ViewLoaded;
            this.KeyPad = IoC.Get<InputKeyPadViewModel>();
        }

        private void SalesMngRightViewModel_ViewLoaded(object? sender, EventArgs e)
        {
            _parentViewModel = (ISalesMngMainViewModel)Parent;
            ExtMenus = _parentViewModel.ExtMenus;
            totalPage = ExtMenus.Count / 10 + (ExtMenus.Count % 10 == 0 ? 0 : 1);
            CurrentPage = 0;
        }

        public override bool SetIView(IView view)
        {
            _view = (ISalesMngRightView)view;
            return base.SetIView(view);
        }

        public void SelectFirstExtMenu(ORDER_FUNC_KEY funcKey)
        {
            _eventAggregator.PublishOnUIThreadAsync(new SalesMngMainViewEventArgs()
            {
                EventType = "ExtButton",
                EventData = funcKey
            });
        }

        List<ORDER_FUNC_KEY>? ExtMenus { get; set; }

        private int totalPage = 0;
        private int currentPage = -1;
        private readonly IOrderPayService orderPayService;

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
                    _eventAggregator.PublishOnUIThreadAsync(new SalesMngMainViewEventArgs()
                    {
                        EventType = "ExtButton",
                        EventData = button.Tag
                    });
                    break;
            }
        });
    }
}
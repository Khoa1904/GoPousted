using Caliburn.Micro;
using Dapper;
using GoPOS.Common.Interface.View;
using GoPOS.Common.ViewModels;
using GoPOS.Helpers;
using GoPOS.Helpers.CommandHelper;
using GoPOS.Models;
using GoPOS.Models.Common;
using GoPOS.ConfigSetup.Interface.View;
using GoPOS.ConfigSetup.ViewModels;
using GoPOS.Services;
using GoPOS.Views;
using GoShared.Helpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using GoPOS.Common.Interface.Model;

namespace GoPOS.ViewModels
{
    /*
     환경설정 > 우측 메뉴바

     */


    public class ConfigSetupRightViewModel : OrderPayChildViewModel, IMainMgrPageRightViewModel
    {
        public InputKeyPadViewModel KeyPad { get; set; }
        private IConfigSetupRightView _view;
        public ConfigSetupRightViewModel(IWindowManager windowManager, IEventAggregator eventAggregator,
            IOrderPayService orderPayService) : base(windowManager, eventAggregator)
        {
            KeyPad = IoC.Get<InputKeyPadViewModel>();
            this.orderPayService = orderPayService;
            this.ViewInitialized += ConfigSetupRightViewModel_ViewInitialized;
            this.ViewLoaded += ConfigSetupRightViewModel_ViewLoaded;
        }

        private void ConfigSetupRightViewModel_ViewLoaded(object? sender, EventArgs e)
        {
            ExtMenus = IoC.Get<ConfigSetupMainViewModel>().ExtMenus;
            totalPage = ExtMenus.Count / 10 + (ExtMenus.Count % 10 == 0 ? 0 : 1);
            CurrentPage = 0;
        }

        private void ConfigSetupRightViewModel_ViewInitialized(object? sender, EventArgs e)
        {
            //ExtMenus = orderPayService.GetOrderFuncKey("05").Result.Item1;
        }

        public override bool SetIView(IView view)
        {
            _view = (IConfigSetupRightView)view;
            return base.SetIView(view);
        }

        public void SelectFirstExtMenu(ORDER_FUNC_KEY funcKey)
        {
            SelectExtMenu(funcKey);
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

            switch (button.Tag.ToString())
            {
                case "Prev":
                    CurrentPage--;
                    break;
                case "Next":
                    CurrentPage++;
                    break;
                default:
                    SelectExtMenu(button.Tag);
                    break;
            }
        });

        private void SelectExtMenu(object funcKey)
        {
            _eventAggregator.PublishOnUIThreadAsync(new ConfigSetupMainViewEventArgs()
            {
                EventType = "ExtButton",
                EventData = funcKey
            });
        }
    }
}
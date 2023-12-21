using Caliburn.Micro;
using Dapper;
using GoPOS.Common.Interface.Model;
using GoPOS.Common.Interface.View;
using GoPOS.Common.ViewModels;
using GoPOS.Helpers;
using GoPOS.Helpers.CommandHelper;
using GoPOS.Models;
using GoPOS.Models.Common;
using GoPOS.OrderPay.Interface.View;
using GoPOS.OrderPay.Interface.ViewModel;
using GoPOS.OrderPay.Views;
using GoPOS.Services;

using GoPOS.Views;
using GoShared.Events;
using GoShared.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using static GoPOS.Function;
using System.ComponentModel;
using System.Windows.Media.Imaging;
using System.Windows.Media;

namespace GoPOS.ViewModels
{

    public class OrderPayLeftViewModel : BaseItemViewModel, IOrderPayLeftViewModel
    {
        private readonly IOrderPayService orderPayService;
        private IOrderPayMainViewModel orderPayMainViewModel;
        private IOrderPayLeftView _view;
        private List<ORDER_FUNC_KEY>? ExtMenus = null;

        public override bool SetIView(IView view)
        {
            _view = (IOrderPayLeftView)view;
            return base.SetIView(view);
        }

        public ICommand ButtonCommand => new RelayCommand<Button>(ButtonCommandClicked);

        /// <summary>
        /// Grid의 버튼
        /// BUtton for Grid handling
        /// </summary>
        /// <param name="button"></param>
        private void ButtonCommandClicked(Button button)
        {
            if (button.Tag == null)
            {
                return;
            }            
        }

        public ICommand ExtMenuCommand => new RelayCommand<Button>(ExtMenuCommandClicked);

        /// <summary>
        /// 기능버튼 클릭
        /// </summary>
        /// <param name="button"></param>
        private void ExtMenuCommandClicked(Button button)
        {

            if (button.Tag == null)
            {
                return;
            }

            string tagKey = (string)button.Tag;
            if (tagKey.StartsWith("FK_"))
            {
                string fnKeyNo = tagKey.Substring(3);
                var mapKey = orderPayMainViewModel.FunKeyMaps.FirstOrDefault(p => p.FK_NO == fnKeyNo);
                if (mapKey == null)
                {
                    return;
                }

                /// Show extmenu Popup
                if ("FN_EXTEND".Equals(mapKey.ViewModelName))
                {
                    // disaels
                    _view.DisableElements(0, true);

                    //var orderPayMainViewHeight = IoC.Get<OrderPayMainView>().Height;                    
                    //DialogHelper.ShowDialog(typeof(OrderPayExtMenuViewModel), 405, 307, 0, orderPayMainViewHeight - 307);
                    var odpxmVm = IoC.Get<OrderPayExtMenuViewModel>();
                    this.ActiveItem = odpxmVm;
                    odpxmVm.ViewClosed += delegate {
                        this.ActiveItem = IoC.Get<OrderPayLeftInfoKeypadViewModel>();
                        _view.DisableElements(0, false);
                    };
                    return;
                }

                IoC.Get<OrderPayMainViewModel>().ProcessFuncKeyClicked(fnKeyNo);
            }
        }


        public OrderPayLeftViewModel(IWindowManager windowManager, IEventAggregator eventAggregator,
            IOrderPayService service,
            IOrderPayMainViewModel orderPayMainViewModel) : base(windowManager, eventAggregator)
        {
            this.orderPayService = service;
            this.ActiveItem = IoC.Get<OrderPayLeftInfoKeypadViewModel>();
            this.ViewLoaded += OrderPayLeftViewModel_ViewLoaded;
        }

        private void OrderPayLeftViewModel_ViewLoaded(object? sender, EventArgs e)
        {
            orderPayMainViewModel = IoC.Get<OrderPayMainViewModel>();
            ExtMenus = orderPayMainViewModel.ExtMenus.Take(5).ToList();
            _view.RenderLeftFuncButtons(this.ExtMenus.ToArray());
        }


    }
}
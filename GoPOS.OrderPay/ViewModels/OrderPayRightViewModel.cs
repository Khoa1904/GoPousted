using Caliburn.Micro;
using Dapper;
using GoPOS.Common.Helpers;
using GoPOS.Common.Interface.View;
using GoPOS.Common.ViewModels;
using GoPOS.Helpers;
using GoPOS.Helpers.CommandHelper;
using GoPOS.Models;
using GoPOS.Models.Common;
using GoPOS.OrderPay.Interface.View;
using GoPOS.OrderPay.Interface.ViewModel;
using GoPOS.OrderPay.Models;
using GoPOS.OrderPay.ViewModels.Controls;
using GoPOS.OrderPay.Views.Controls;
using GoPOS.Services;

using GoPOS.Views;
using Microsoft.EntityFrameworkCore.Query.Internal;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace GoPOS.ViewModels
{
    public class OrderPayRightViewModel : OrderPayChildViewModel, IOrderPayRightViewModel
    {
        private IOrderPayRightView _view;
        private readonly IOrderPayService orderPayService;
        bool firstTime = false;
        private DualScreenMainViewModel dualScreenModel { get; set; }

        public OrderPayRightViewModel(IWindowManager windowManager, IEventAggregator eventAggregator,
            IOrderPayService orderPayService) : base(windowManager, eventAggregator)
        {
            this.ViewLoaded += OrderPayRightViewModel_ViewLoaded;
            this.ViewInitialized += OrderPayRightViewModel_ViewInitialized;
            this.ViewUnloaded += Unload;

            this.orderPayService = orderPayService;
            dualScreenModel = IoC.Get<DualScreenMainViewModel>();
        }

        private void OrderPayRightViewModel_ViewInitialized(object? sender, EventArgs e)
        {
            firstTime = false;
            var tuchKeys = orderPayService.GetTouchClasses().Result;
            _view.BindTouchClasses(tuchKeys);
            //firstTuchClass = tuchKeys.Where(p => p.TU_PAGE == "1").OrderBy(p => p.POSITION_NO).FirstOrDefault();
        }

        private void OrderPayRightViewModel_ViewLoaded(object? sender, EventArgs e)
        {
            var payFuncKeys = IoC.Get<OrderPayMainViewModel>().PayBtnKeys;
            _view.RenderFuncButtons(payFuncKeys);

            //if (firstTuchClass != null && firstTime == false) _TouchClsClicked(firstTuchClass);
        }

        private void Unload(object? sender, EventArgs e)
        {
            //Dont need to reset to firstTuchClass
            firstTime = true;
        }

        public override bool SetIView(IView view)
        {
            _view = (IOrderPayRightView)view;
            return base.SetIView(view);
        }

        #region Touch Classes

        private MST_TUCH_CLASS firstTuchClass;
        public ICommand TouchClsCommand => new RelayCommand<Button>(TouchClsClicked);

        private void TouchClsClicked(Button button)
        {
            if (button.Tag == null) return;
            if (button.Tag.ToString() == "TouchPrev")
            {
                _view.TuchClsPage--;
                return;
            }
            if (button.Tag.ToString() == "TouchNext")
            {
                _view.TuchClsPage++;
                return;
            }

            MST_TUCH_CLASS tuc = (MST_TUCH_CLASS)button.Tag;
            TouchClsClicked(tuc);
        }

        public void TouchClsClicked(MST_TUCH_CLASS tuc)
        {
            var prods = orderPayService.GetTouchProducts(tuc.TU_CLASS_CODE).Result;
            _view.BindTouchProdList(prods);
        }
        #endregion

        #region Touch Products

        public ICommand TouchProdCommand => new RelayCommand<Button>(TouchProdClicked);

        private void TouchProdClicked(Button button)
        {
            var name = dualScreenModel.ActiveItem.DisplayName;
            if (!name.Contains("DualScreenOrderingViewModel"))
            {
                dualScreenModel.ActiveItem = IoC.Get<DualScreenOrderingViewModel>();
            }

            if (button.Tag == null) return;
            if (button.Tag.ToString() == "ProdPrev")
            {
                _view.TuchProdPage--;
                return;
            }
            if (button.Tag.ToString() == "ProdNext")
            {
                _view.TuchProdPage++;
                return;
            }

            var tup = button.Tag as ORDER_TU_PRODUCT;
            IoC.Get<OrderPayMainViewModel>().TouchProductClicked(tup);
        }

        #endregion

        #region Pay Buttons

        public ICommand PayBtnCommand => new RelayCommand<Button>(PayBtnClicked);


        private void PayBtnClicked(Button button)
        {
            // check and disable left info screen button if activeitemR is payment window
            if (button.Tag == null) return;
            var FK = (ORDER_FUNC_KEY)button.Tag;
            var FunKeyMaps = ResourceHelpers.FunKeyMaps;
            var mapKey = FunKeyMaps.FirstOrDefault(p => p.FK_NO == FK.FK_NO);
            if (mapKey.ItemArea == "ActiveItemR")
            {
                IoC.Get<OrderPayLeftTRInfoViewModel>().ButtonClick = false;
                IoC.Get<MemberInfoViewModel>().ButtonClick = false;
            }

            IoC.Get<OrderPayMainViewModel>().ProcessFuncKeyClicked(FK);
        }

        #endregion

    }
}
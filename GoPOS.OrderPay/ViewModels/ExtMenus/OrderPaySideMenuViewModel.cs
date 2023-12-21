using Caliburn.Micro;
using Dapper;
using GoPOS.Common.Interface.View;
using GoPOS.Helpers;
using GoPOS.Helpers.CommandHelper;
using GoPOS.Models;
using GoPOS.OrderPay.Interface.View;
using GoPOS.OrderPay.Models;
using GoPOS.Services;

using GoPOS.ViewModels;
using GoPOS.Views;
using GoShared.Events;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;


namespace GoPOS.ViewModels
{
    public class OrderPaySideMenuViewModel : OrderPayChildViewModel
    {
        private readonly IOrderPayService orderPayService;
        private List<ORDER_SIDE_CLS_MENU> sideMenuList = null;
        private IOrderPaySideMenuView _view;
        private bool _confirmSelection = false;

        public override object ActivateType => "OnlyItems";
        public OrderPaySideMenuViewModel(IWindowManager windowManager, IEventAggregator eventAggregator,
            IOrderPayService orderPayService) :
            base(windowManager, eventAggregator)
        {
            this.orderPayService = orderPayService;
            ViewInitialized += OrderPaySideMenuViewModel_ViewInitialized;
            ViewLoaded += OrderPaySideMenuViewModel_ViewLoaded;
            ViewUnloaded += OrderPaySideMenuViewModel_ViewUnloaded;
        }

        private void OrderPaySideMenuViewModel_ViewLoaded(object? sender, EventArgs e)
        {
            _confirmSelection = false;
        }

        private void OrderPaySideMenuViewModel_ViewUnloaded(object? sender, EventArgs e)
        {
            _eventAggregator.PublishOnUIThreadAsync(new SideMenuConfirmEventArgs() { Confirmed = _confirmSelection });
            _confirmSelection = false;
        }

        private async void OrderPaySideMenuViewModel_ViewInitialized(object? sender, EventArgs e)
        {
        }

        public override bool SetIView(IView view)
        {
            _view = (IOrderPaySideMenuView)view;
            return base.SetIView(view);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        public async void LoadSideMenus(ORDER_GRID_ITEM item)
        {
            sideMenuList = await orderPayService.GetSideCLSMenus(item.PRD_CODE);
            var clsOnlys = sideMenuList.Select(p =>
                new ORDER_SIDE_CLS_MENU()
                {
                    CLASS_CODE = p.CLASS_CODE,
                    CLASS_NAME = p.CLASS_NAME,
                    CLASS_TYPE = p.CLASS_TYPE
                }).DistinctBy(p => new { p.CLASS_CODE, p.CLASS_TYPE }).OrderBy(p => p.CLASS_TYPE).ThenBy(p => p.CLASS_CODE).ToArray();
            if (clsOnlys.Length == 0)
            {
                return;
            }
            _view.RenderSideClasses(clsOnlys);
            _SideClassClicked(clsOnlys[0]);
        }

        public ICommand SideClassCommand => new RelayCommand<Button>(SideClassClicked);

        private void SideClassClicked(Button button)
        {
            ORDER_SIDE_CLS_MENU clsMenu = button.Tag as ORDER_SIDE_CLS_MENU;
            _SideClassClicked(clsMenu);
        }

        private void _SideClassClicked(ORDER_SIDE_CLS_MENU clsMenu)
        {
            if (clsMenu == null)
            {
                return;
            }

            var menus = sideMenuList.Where(p => p.CLASS_CODE == clsMenu.CLASS_CODE && p.CLASS_TYPE == clsMenu.CLASS_TYPE).Select(p =>
                new ORDER_SIDE_CLS_MENU()
                {


                    SUB_CODE = p.SUB_CODE,
                    SUB_NAME = p.SUB_NAME,
                    ORDER_SEQ = p.ORDER_SEQ,
                    PRD_TYPE_FLAG = p.PRD_TYPE_FLAG,
                    CLASS_TYPE = p.CLASS_TYPE,
                    CLASS_CODE = p.CLASS_CODE,
                    CLASS_NAME = p.CLASS_NAME,
                    MAX_QTY = p.MAX_QTY,
                    UNIT_PRICE = p.UNIT_PRICE,
                    TAX_YN = p.TAX_YN
                }).DistinctBy(p => p.SUB_CODE).OrderBy(p => p.ORDER_SEQ).ToArray();
            _view.RenderSideMenus(menus);

        }

        public ICommand SideMenuCommand => new RelayCommand<Button>(SideMenuClicked);

        private void SideMenuClicked(Button button)
        {
            if (button.Tag == null) return;
            ORDER_SIDE_CLS_MENU sideProd = button.Tag as ORDER_SIDE_CLS_MENU;
            IoC.Get<OrderPayMainViewModel>().ItemGrid_SideMenuSelected(sideProd);
        }

        public ICommand ButtonCommand => new RelayCommand<Button>(ButtonClicked);

        private void ButtonClicked(Button button)
        {
            switch (button.Tag)
            {
                case "Cancel":
                    _confirmSelection = false;
                    this.DeactivateAsync(true);
                    break;
                case "Save":
                    _confirmSelection = true;
                    this.DeactivateAsync(true);
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        public override void SetData(object data)
        {
            object[] dataTypes = (object[])data;
            if (dataTypes[0].ToString() == "CLASS_CODE")
            {

            }
        }
    }
}
using Caliburn.Micro;
using Dapper;
using GoPOS.Common.Interface.Model;
using GoPOS.Common.Interface.View;
using GoPOS.Common.Service;
using GoPOS.Common.ViewModels;
using GoPOS.Helpers;
using GoPOS.Helpers.CommandHelper;
using GoPOS.Models;
using GoPOS.Models.Common;
using GoPOS.OrderPay.Interface.View;
using GoPOS.OrderPay.Interface.ViewModel;
using GoPOS.OrderPay.Models;
using GoPOS.Service;
using GoPOS.Services;

using GoPOS.Views;
using GoShared.Events;
using GoShared.Helpers;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;


/*
 주문 > 확장메뉴 셋 화면
 */

namespace GoPOS.ViewModels
{

    public class OrderPayExtMenuViewModel : OrderPayChildViewModel //, IDialogViewModel
    {
        private readonly IOrderPayService orderPayService;
        private IOrderPayMainViewModel orderPayMainViewModel;
        private IOrderPayExtMenuView _view = null;
        private OrderPayChildUpdatedEventArgs closeEventARRGGG = null;
        private IPOSPrintService _printer;


        public OrderPayExtMenuViewModel(IWindowManager windowManager, IEventAggregator eventAggregator,
            IOrderPayService orderPayService, IPOSPrintService pOSPrintService) : base(windowManager, eventAggregator)
        {
            this.orderPayService = orderPayService;
            this.ViewLoaded += OrderPayExtMenuViewModel_ViewLoaded;
            this.ViewUnloaded += OrderPayExtMenuViewModel_ViewUnloaded;
            this._printer = pOSPrintService;
        }

        private void OrderPayExtMenuViewModel_ViewUnloaded(object? sender, EventArgs e)
        {
            _eventAggregator.PublishOnUIThreadAsync(closeEventARRGGG);
        }

        public ICommand ButtonCommand => new RelayCommand<Button>(ButtonClicked);

        public override object ActivateType => "ExceptActiveItemLB";
        public event EventHandler ViewClosed;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <exception cref="NotImplementedException"></exception>
        private async void ButtonClicked(Button button)
        {
            if (button.Tag == null)
            {
                return;
            }
            //301
            switch (button.Tag)
            {
                case "ButtonClose":
                    await this.DeactivateAsync(true);
                    ViewClosed?.Invoke(this, EventArgs.Empty);
                    break;
                case "Prev":
                    CurrentPage--;
                    break;
                case "Next":
                    CurrentPage++;
                    break;
                default:
                    string tagKey = (string)button.Tag;
                    string fnKeyNo = tagKey.Substring(3);
                    await this.DeactivateAsync(true);
                    this.orderPayMainViewModel.ProcessFuncKeyClicked(fnKeyNo);
                    //if (tagKey.StartsWith("FK_"))
                    //{
                    //    string fnKeyNo = tagKey.Substring(3);
                    //    if (fnKeyNo == "272")
                    //    {
                    //        int PreBill = Convert.ToInt32(DataLocals.PosStatus.BILL_NO);
                    //        string str = (PreBill).ToString("0000");
                    //        _printer.PrintBillWithKey(DataLocals.AppConfig.PosInfo.StoreNo, DataLocals.PosStatus.POS_NO,
                    //            DataLocals.PosStatus.SALE_DATE, PreBill.ToString("0000"), true, true);
                    //    }
                    //    else if (tagKey == "")
                    //    await this.DeactivateAsync(true);
                    //    this.orderPayMainViewModel.ProcessFuncKeyClicked(fnKeyNo);
                    //}
                    break;
            }
        }
        private void OrderPayExtMenuViewModel_ViewLoaded(object? sender, EventArgs e)
        {
            closeEventARRGGG = new OrderPayChildUpdatedEventArgs();
            this.orderPayMainViewModel = IoC.Get<OrderPayMainViewModel>();
            ExtMenus = this.orderPayMainViewModel.ExtMenus?.Skip(5).ToList();
            totalPage = ExtMenus.Count / 16 + (ExtMenus.Count % 16 == 0 ? 0 : 1);
            CurrentPage = 0;
        }

        List<ORDER_FUNC_KEY>? ExtMenus { get; set; }

        private int totalPage = 0;
        private int currentPage = -1;
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
                var funcKeys = ExtMenus.Page(16, currentPage).ToArray();
                _view.RenderExtButtons(funcKeys);
            }
        }
        public override bool SetIView(IView view)
        {
            _view = (IOrderPayExtMenuView)view;
            return base.SetIView(view);
        }

    }

}
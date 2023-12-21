using Caliburn.Micro;
using Dapper;
using GoPOS.Common.Interface.Model;
using GoPOS.Common.Interface.View;
using GoPOS.Helpers;
using GoPOS.Helpers.CommandHelper;
using GoPOS.Models;
using GoPOS.Models.Common;
using GoPOS.Models.Custom.Payment;
using GoPOS.Payment.Interface.View;
using GoPOS.Payment.Models;
using GoPOS.Service;
using GoPOS.Services;

using GoPOS.Views;
using GoShared.Helpers;
using GoShared.Interface;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Diagnostics;
using System.DirectoryServices.Protocols;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

/*
 주문 > 확장메뉴 > 복합결제
 */

/// <summary>
/// 화면명 : 복합결제
/// 작성자 : 김형석
/// 작성일 : 20230312
/// </summary>
/// 
namespace GoPOS.ViewModels
{
    public class OrderPayCompPayViewModel : OrderPayChildViewModel
    {
        private IOrderPayMainViewModel _orderPayMainViewModel;
        private readonly IOrderPayCompPayService compPayService;
        private readonly IOrderPayService orderPayService;
        private IOrderPayMainViewModel orderPayMainViewModel;
        private ORDER_FUNC_KEY[] comPayMethods = new ORDER_FUNC_KEY[0];
        private IOrderPayCompPayView orderPayCompPayView;
        private decimal? payAmount;
        private decimal? remainAmount;
        private Dictionary<string, string> funcKeyMapNames = null;

        private ObservableCollection<COMPPAY_PAY_INFO> payList;
        private bool _viewLoaded = false;

        public override bool SetIView(IView view)
        {
            orderPayCompPayView = (OrderPayCompPayView)view;
            return base.SetIView(view);
        }

        public override object ActivateType { get => "ExceptKeyPad"; }

        public ObservableCollection<COMPPAY_PAY_INFO> PayList
        {
            get => payList; set
            {
                payList = value;
                NotifyOfPropertyChange(() => PayList);
            }
        }

        public decimal? PayAmount
        {
            get => payAmount; set
            {
                payAmount = value;
                NotifyOfPropertyChange(nameof(PayAmount));
            }
        }

        private COMPPAY_PAY_INFO _selectedItem;
        public COMPPAY_PAY_INFO SelectedItem
        {
            get => _selectedItem; set
            {
                _selectedItem = value;
                NotifyOfPropertyChange(nameof(SelectedItem));
            }
        }

        public OrderPayCompPayViewModel(IWindowManager windowManager, IEventAggregator eventAggregator,
            IOrderPayService orderPayService) : base(windowManager, eventAggregator)
        {
            this.orderPayService = orderPayService;
            this.ViewInitialized += OrderPayCompPayViewModel_ViewInitialized;
            this.ViewLoaded += OrderPayCompPayViewModel_ViewLoaded;
            this.OnClosedCommand += OrderPayCompPayViewModel_OnClosedCommand;
        }

        private void OrderPayCompPayViewModel_OnClosedCommand(object? sender, EventArgs e)
        {
            _viewLoaded = false;
        }

        private void OrderPayCompPayViewModel_ViewLoaded(object? sender, EventArgs e)
        {
            if (_viewLoaded)
            {
                return;
            }

            PayList = new ObservableCollection<COMPPAY_PAY_INFO>();

            funcKeyMapNames = new Dictionary<string, string>();
            foreach (var item in comPayMethods)
            {
                var map = orderPayMainViewModel.FunKeyMaps.FirstOrDefault(p => p.FK_NO == item.FK_NO);
                if (map != null && !string.IsNullOrEmpty(map.ViewModelName))
                {
                    funcKeyMapNames.Add(map.ViewModelName, item.POSITION_NO);
                    item.ExternData = map.ViewModelName;
                }
            }

            orderPayCompPayView.RenderPayButtons(comPayMethods);
            _viewLoaded = true;
            PayAmount = 0;


            // Load existing pays
            BindExistingPays();
        }

        private void OrderPayCompPayViewModel_ViewInitialized(object? sender, EventArgs e)
        {
            orderPayMainViewModel = (IOrderPayMainViewModel)Parent;
            var ret = orderPayService.GetOrderFuncKey("08").Result;
            if (ret.Item2.ResultType == EResultType.SUCCESS)
            {
                comPayMethods = ret.Item1.ToArray();
            }
        }

        ORDER_FUNC_KEY funcKey;
        public ICommand ButtonCommand => new RelayCommand<Button>(button =>
        {
            if (button == null || button.Tag == null) { return; }
            string msg = string.Empty;

            if (button.Tag.ToString() == "btnReqSelCancle")
            {
                if (SelectedItem == null) return;
                if (SelectedItem.APPR_PROC_FLAG == "0")
                {
                    DialogHelper.MessageBox("이미 취소된 결제입니다.", GMessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // if cash receipt, or card
                // if coupone have cashReceipt
                // cannot cancel
                string payTypeFlag = OrderPayConsts.PayTypeFlagByTRNClass(SelectedItem.PAY_CLASS_NAME, SelectedItem.PayDatas);
                if (payTypeFlag == OrderPayConsts.PAY_CASHREC ||
                        payTypeFlag == OrderPayConsts.PAY_CARD ||
                        payTypeFlag == OrderPayConsts.PAY_CARD_ER)
                {
                    DialogHelper.MessageBox("밴 승인된 결제는 취소 불가능합니다.", GMessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (payTypeFlag == OrderPayConsts.PAY_POINTS)
                {
                    DialogHelper.MessageBox("포인트사용은 취소 불가능합니다.", GMessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                SelectedItem.APPR_PROC_FLAG = "0";
                var viewModelName = "OrderPayStampCouponViewModel".Equals(SelectedItem.PAY_VM_NANE) ? "OrderPayMemberPointUseViewModel" : SelectedItem.PAY_VM_NANE;

                if (funcKeyMapNames.ContainsKey(viewModelName))
                {
                    orderPayCompPayView.UpdatePaidAmount(funcKeyMapNames[viewModelName], SelectedItem.APPR_AMT, -1);
                }
                orderPayMainViewModel.RemovePaymentTRN(SelectedItem);
                return;
            }

            funcKey = (ORDER_FUNC_KEY)button.Tag;
            var mapKey = orderPayMainViewModel.FunKeyMaps.FirstOrDefault(p => p.FK_NO == funcKey.FK_NO);
            if (CheckExistsRow(mapKey, out msg))
            {
                orderPayMainViewModel.StatusMessage = msg;
                return;
            }
            int paySeq = orderPayMainViewModel.GetTRPaySeq(mapKey.ViewModelName);
            orderPayMainViewModel.ProcessFuncKeyClicked(funcKey, "COMP_PAY", payAmount, paySeq, orderPayMainViewModel.MemberInfo);
            PayAmount = 0;
        });

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mapKey"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        private bool CheckExistsRow(FkMapInfo? mapKey, out string msg)
        {
            msg = string.Empty;
            if (mapKey == null ||
                "OrderPayCashViewModel".Equals(mapKey.ViewModelName) ||
                "OrderPayCardViewModel".Equals(mapKey.ViewModelName) ||
                "OrderPayPrepaymentUseViewModel".Equals(mapKey.ViewModelName)) return false;

            var ext = payList.Where(p => p.PAY_VM_NANE == mapKey.ViewModelName && "1".Equals(p.APPR_PROC_FLAG)).Count() > 0;
            if (ext)
                msg = string.Format("{0} 결제는 한번만 결제가능합니다", OrderPayConsts.PayTypeFlagNameByVM(mapKey.ViewModelName));
            return ext;
        }

        public override void SetData(object data)
        {
            object[] datas = (object[])data;
            PayAmount = (decimal)datas[1];
            remainAmount = orderPayMainViewModel.OrderSumInfo.EXP_PAY_AMT;

            _viewLoaded = false;
        }

        /// <summary>
        /// 
        /// </summary>
        void BindExistingPays()
        {
            for (int i = 1; i <= 10; i++)
            {
                orderPayCompPayView.UpdatePaidAmount(i.ToString(), 0, 0);
            }

            foreach (var payInfo in orderPayMainViewModel.payInfos)
            {
                PayList.Add(payInfo);
                remainAmount -= payInfo.APPR_AMT;

                var viewModelName = "OrderPayStampCouponViewModel".Equals(payInfo.PAY_VM_NANE) ? "OrderPayMemberPointUseViewModel" : payInfo.PAY_VM_NANE;

                if (funcKeyMapNames.ContainsKey(viewModelName))
                {
                    orderPayCompPayView.UpdatePaidAmount(funcKeyMapNames[viewModelName], payInfo.APPR_AMT, 1);
                }
            }
            NotifyOfPropertyChange(() => PayList);
        }
        public void EnableControl(bool enable)
        {
            throw new NotImplementedException();
        }

        public void Translate()
        {
            throw new NotImplementedException();
        }

        public bool UpdatePaymentTRN(string viewModelName, COMPPAY_PAY_INFO compPay)
        {
            compPay.PAY_SEQ = (PayList.Count + 1).ToString();

            viewModelName = "OrderPayStampCouponViewModel".Equals(viewModelName) ? "OrderPayMemberPointUseViewModel" : viewModelName;

            if (funcKeyMapNames.ContainsKey(viewModelName))
            {
                orderPayCompPayView.UpdatePaidAmount(funcKeyMapNames[viewModelName], compPay.APPR_AMT, 1);
            }

            remainAmount -= compPay.APPR_AMT;
            PayList.Add(compPay);

            NotifyOfPropertyChange(() => PayList);
            var res = orderPayMainViewModel.UpdatePaymentTRN(viewModelName, compPay);
            if (res)
            {
                _viewLoaded = false;
                this.DeactivateClose(true);
            }

            return res;
        }
    }
}
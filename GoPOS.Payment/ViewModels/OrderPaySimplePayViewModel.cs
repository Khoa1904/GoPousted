using Caliburn.Micro;
using Dapper;
using GoPOS.Common.Interface.Model;
using GoPOS.Common.Interface.View;
using GoPOS.Helpers;
using GoPOS.Helpers.CommandHelper;
using GoPOS.Models;
using GoPOS.Models.Common;
using GoPOS.Models.Config;
using GoPOS.Models.Custom.Payment;
using GoPOS.Payment.Interface.View;
using GoPOS.Payment.Services;
using GoPOS.Service;
using GoPOS.Service.Common;
using GoPOS.Services;

using GoPOS.Views;
using GoPosVanAPI.Api;
using GoPosVanAPI.Msg;
using GoPosVanAPI.Van;
using GoShared.Helpers;
using log4net.Repository.Hierarchy;
using Microsoft.Xaml.Behaviors.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Xml.Linq;
using static GoPosVanAPI.Api.VanAPI;


/// <summary>
/// 간편결제
/// </summary>

namespace GoPOS.ViewModels
{

    public class OrderPaySimplePayViewModel : OrderPayChildViewModel
    {
        private IOrderPayMainViewModel orderPayMainViewModel;
        private IOrderPaySimplePayView _view;
        private readonly IOrderPaySimpleMobileService orderPaySimpleMobileService;
        private MST_INFO_EASYPAY _selectEsPay = null;
        private int _seqNo = 0;
        private string CALLER_ID = string.Empty;
        private string barCode;
        private decimal payAmount;
        
        public override object ActivateType { get => "ExceptKeyPad"; }

        public decimal PayAmount
        {
            get => payAmount; set
            {
                payAmount = value;
                if (value <= 0 || payAmount > orderPayMainViewModel.OrderSumInfo.EXP_PAY_AMT)
                {
                    payAmount = orderPayMainViewModel.OrderSumInfo.EXP_PAY_AMT;
                }
                NotifyOfPropertyChange(() => PayAmount);
            }
        }

        public string BarCode
        {
            get => barCode; set
            {
                barCode = value;
                NotifyOfPropertyChange(() => BarCode);
            }
        }

        public ICommand PAYCPSelCommand => new RelayCommand<Button>(button =>
        {
            _selectEsPay = (MST_INFO_EASYPAY)button.Tag;
        });

        public ICommand ReqApprCommand => new RelayCommand<Button>(button =>
        {
            if (_selectEsPay == null)
            {
                DialogHelper.MessageBox("결제방식을 먼저 선택하세요.", GMessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            string errorMsg = string.Empty;
            VanResponseMsg result = null;
            bool isUserCancelled = false;

            try
            {
                // CHANGE_DUTY ==>
                decimal nonTaxAmt = 0;
                decimal incTaxAmtExcTax = 0;
                decimal taxAmt = 0;
                decimal rPayAmt = 0;
                orderPayMainViewModel.CalcAmtsWithDuty(PayAmount, out nonTaxAmt, out incTaxAmtExcTax, out taxAmt, out rPayAmt);

                VanAPI vanAPI = new VanAPI();

                #region Request QR

                VanRequestMsg msg = new VanRequestMsg()
                {
                    VAN_CODE = DataLocals.POSVanConfig.VAN_CODE,  // Smartro 밴코드
                    TID = DataLocals.POSVanConfig.VAN_TERM_NO,
                    POS_NO = DataLocals.AppConfig.PosInfo.PosNo,
                    REQ_SIGN_PAD_CODE = "BQ"
                };

                result = vanAPI.RequestSignPad(msg);

                isUserCancelled = result.RESPONSE_CODE == ResultCode.UserCancelled;
                if (isUserCancelled)
                {
                    return;
                }

                this.BarCode = result.BARCODE;

                Thread.Sleep(1500);
                #endregion

                msg = new VanRequestMsg()
                {
                    VAN_CODE = DataLocals.POSVanConfig.VAN_CODE,  // Smartro 밴코드
                    POS_NO = DataLocals.AppConfig.PosInfo.PosNo,
                    TID = DataLocals.POSVanConfig.VAN_TERM_NO,
                    BIZ_NO = DataLocals.POSVanConfig.BIZ_NO,
                    // CHANGE_DUTY ==>
                    TRAN_AMT = rPayAmt.ToString(),
                    ORG_AMT = incTaxAmtExcTax.ToString(),
                    TAX_AMT = taxAmt.ToString(),
                    DUTY_AMT = nonTaxAmt.ToString(),
                    SVC_AMT = "0",
                    INS_MON = "00",
                    SIGN_AT = "2",
                    BARCODE = BarCode,
                    WORKING_KEY = string.Empty,
                    WORKING_KEY_INDEX = string.Empty,
                    PAY_CODE = _selectEsPay.PAYCP_CODE
                };

                result = vanAPI.ApprovalPay(msg);
                errorMsg = "[" + result.RESPONSE_CODE + "] " + result.DISPLAY_MSG;
                //isUserCancelled = result.RESPONSE_CODE == "BA";
                isUserCancelled = result.RESPONSE_CODE == ResultCode.UserCancelled;
                LogHelper.Logger.Trace(result.ToString());               
            }
            catch (Exception ex)
            {
                LogHelper.Logger.Error(ex.ToFormattedString());
                errorMsg = ex.Message;
            }

            if (result == null || result.RESPONSE_CODE != ResultCode.VanSuccess)
            {
                DialogHelper.MessageBox(errorMsg, GMessageBoxButton.OK, MessageBoxImage.Error);

                // Comment tempo
                if (isUserCancelled)
                    return;
            }

            if (result != null && result.RESPONSE_CODE == ResultCode.VanSuccess)
            {
                TRN_EASYPAY easyPay = new TRN_EASYPAY()
                {
                    SHOP_CODE = DataLocals.AppConfig.PosInfo.StoreNo,
                    SALE_DATE = DataLocals.PosStatus.SALE_DATE,
                    POS_NO = DataLocals.PosStatus.POS_NO,
                    BILL_NO = DataLocals.PosStatus.BILL_NO,
                    SEQ_NO = _seqNo.ToString("d2"),
                    REGI_SEQ = DataLocals.PosStatus.REGI_SEQ,
                    PAY_CARD_NO = result.CARD_NO,
                    APPR_REQ_AMT = result.AUTH_AMT.ToDecimal(),
                    APPR_PROC_FLAG = "1",
                    VAT_AMT = result.TAX_AMT.ToDecimal(),
                    // CHANGE_DUTY ==>
                    DUTY_AMT = result.DUTY_AMT.ToDecimal(),
                    INST_MM_FLAG = "0",
                    INST_MM_CNT = 0,
                    PAY_METHOD_FLAG = "B",
                    APPR_DATE = result.TRAN_REQ_DATE,
                    APPR_TIME = result.TRAN_REQ_TIME,
                    APPR_NO = result.AUTH_NO,
                    VAN_CODE = DataLocals.POSVanConfig.VAN_CODE,
                    PAYCP_CODE = _selectEsPay.PAYCP_CODE,
                    VAN_PAYCP_CODE = result.PAY_CODE,
                    ISS_CRDCP_CODE = result.ISSUER_CODE.Substring(0, 4),
                    ISS_CRDCP_NAME = result.ISSUER_NM,
                    PUR_CRDCP_CODE = result.ACQUIRER_CODE.Substring(0, 4),
                    PUR_CRDCP_NAME = result.ACQUIRER_NM,
                    APPR_MSG = result.RESPONSE_MSG,
                    CRDCP_TERM_NO = string.Empty,
                    ORG_APPR_DATE = string.Empty,
                    ORG_APPR_NO = string.Empty,
                    INSERT_DT = DateTime.Now.ToString("yyyyMMddHHmmss"),
                    SALE_YN = "Y"
                };

                var compPayInfo = new COMPPAY_PAY_INFO()
                {
                    PAY_TYPE_CODE = OrderPayConsts.PAY_EASY,
                    PAY_CLASS_NAME = nameof(TRN_EASYPAY),
                    PAY_VM_NANE = this.GetType().Name,
                    APPR_NO = easyPay.APPR_NO,
                    APPR_AMT = easyPay.APPR_REQ_AMT,
                    APPR_PROC_FLAG = "1",
                    PayDatas = new object[] { easyPay }
                };

                if ("COMP_PAY".Equals(CALLER_ID))
                {
                    IoC.Get<OrderPayCompPayViewModel>().UpdatePaymentTRN(this.GetType().Name, compPayInfo);
                }
                else
                {
                    orderPayMainViewModel.UpdatePaymentTRN(this.GetType().Name, compPayInfo);
                }

                this.DeactivateClose(true);
            }
        });

        public OrderPaySimplePayViewModel(IWindowManager windowManager,
                IEventAggregator eventAggregator, IOrderPaySimpleMobileService orderPaySimpleMobileService) : base(windowManager, eventAggregator)
        {
            this.ViewLoaded += OrderPaySimplePayViewModel_ViewLoaded;
            this.ViewInitialized += OrderPaySimplePayViewModel_ViewInitialized;
            this.orderPaySimpleMobileService = orderPaySimpleMobileService;
        }

        private void OrderPaySimplePayViewModel_ViewInitialized(object? sender, EventArgs e)
        {
            orderPayMainViewModel = (IOrderPayMainViewModel)Parent;
        }

        private async void OrderPaySimplePayViewModel_ViewLoaded(object? sender, EventArgs e)
        {
            var list = await orderPaySimpleMobileService.GetPayCPList();
            _view.RenderPayCPList(list);
            BarCode = string.Empty;
            _selectEsPay = null;
        }

        public override bool SetIView(IView view)
        {
            _view = (IOrderPaySimplePayView)view;
            return base.SetIView(view);
        }

        public override void SetData(object data)
        {
            object[] datas = (object[])data;
            CALLER_ID = (string)datas[0];
            PayAmount = Convert.ToDecimal(datas[1]);
            _seqNo = (int)datas[2];
            _seqNo++;
        }


        public ICommand ButtonCommand => new RelayCommand<Button>(ButtonCommandStation);

        private void ButtonCommandStation(Button btn)
        {
            switch (btn.Tag)
            {
                default: break;
            }
        }


    }
}
using Caliburn.Micro;
using FirebirdSql.Data.Services;
using GoPOS.Helpers;
using GoPOS.Payment.Interface.View;
using GoPOS.Service.Service.Payment;
using GoPOS.Services;
using GoPOS.Views;
using GoPosVanAPI.Api;
using GoPosVanAPI.Msg;
using GoPosVanAPI.Van;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Globalization;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using GoShared.Helpers;
using GoPOS.Models.Common;
using GoPOS.Models;
using GoPOS.Service;
using GoPOS.Helpers.CommandHelper;
using GoPOS.Common.Interface.View;
using GoPOS.Models.Custom.Payment;
using GoPOS.Common.Interface.Model;
using GoPOS.Service.Common;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

/// <summary>
/// 화면명 : 카드결제
/// 작성자 : 김형석
/// 작성일 : 20230312
/// </summary>

namespace GoPOS.ViewModels
{
    public class OrderPayCardViewModel : OrderPayChildViewModel, IHandle<OrderPayCardCardCompReturnEventArgs>
    {
        public decimal PAY_AMT
        {
            get => pAY_AMT; set
            {
                pAY_AMT = value;
                if (value <= 0 || pAY_AMT > orderPayMainViewModel.OrderSumInfo.EXP_PAY_AMT)
                {
                    pAY_AMT = orderPayMainViewModel.OrderSumInfo.EXP_PAY_AMT;
                }
                NotifyOfPropertyChange(nameof(PAY_AMT));
            }
        }

        public string CARD_NO
        {
            get => cARD_NO; set
            {
                cARD_NO = value;
                NotifyOfPropertyChange(nameof(CARD_NO));
            }
        }
        public string INSTALL_MONTH
        {
            get => iNSTALL_MONTH; set
            {
                iNSTALL_MONTH = value;
                NotifyOfPropertyChange(nameof(INSTALL_MONTH));
            }
        }

        public Visibility ShowManualCardPay
        {
            get
            {
                return DataLocals.AppConfig.PosOption.PayCardManualUseYN != "0" ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        private readonly IOrderPayCardService cardService;
        private IOrderPayMainViewModel orderPayMainViewModel;
        private IOrderPayCardView _view;

        private TRN_CARD payCard = null;
        private int seqNo = 1;
        private string CALLER_ID = string.Empty;
        private decimal pAY_AMT;
        private string iNSTALL_MONTH;
        private string cARD_NO;

        public override object ActivateType { get => "ExceptKeyPad"; }


        public OrderPayCardViewModel(IWindowManager windowManager, IEventAggregator eventAggregator,
            IOrderPayCardService cardService) : base(windowManager, eventAggregator)
        {
            this.cardService = cardService;
            this.ViewInitialized += OrderPayCardViewModel_ViewInitialized;
        }

        private void OrderPayCardViewModel_ViewInitialized(object? sender, EventArgs e)
        {
            orderPayMainViewModel = (IOrderPayMainViewModel)this.Parent;
        }

        public override bool SetIView(IView view)
        {
            _view = (IOrderPayCardView)view;
            return base.SetIView(view);
        }

        public override void SetData(object data)
        {
            payCard = new TRN_CARD()
            {
                SHOP_CODE = DataLocals.AppConfig.PosInfo.StoreNo,
                SALE_DATE = DataLocals.PosStatus.SALE_DATE,
                POS_NO = DataLocals.AppConfig.PosInfo.PosNo,
                BILL_NO = DataLocals.PosStatus.BILL_NO,
                SEQ_NO = seqNo.ToString("d4"),
                REGI_SEQ = DataLocals.PosStatus.REGI_SEQ,
                SALE_YN = "Y",
                APPR_PROC_FLAG = "2",
                VALID_TERM = "",
                APPR_DC_AMT = "0".ToDecimal(),
                SVC_TIP_AMT = "0".ToDecimal(),
                SIGN_PAD_YN = "Y",
                CARD_IN_FLAG = "I",
                APPR_FLAG = "1"
            };

            object[] datas = (object[])data;
            CALLER_ID = datas[0] as string;
            PAY_AMT = Convert.ToDecimal(datas[1]);

            seqNo = (int)datas[2];
            seqNo++;
            payCard.SEQ_NO = seqNo.ToString("d4");

            INSTALL_MONTH = string.Empty;
            CARD_NO = string.Empty;
        }

        public ICommand ButtonCommand => new RelayCommand<Button>((button) =>
        {
            switch (button.Tag.ToString())
            {
                case "TempRegist":
                    var res = DialogHelper.MessageBox("임의등록 후 반드시 단말기 승인 처리해야 합니다.\n(본 거래는 실제 카드 승인이 되지 않습니다.)", GMessageBoxButton.OK, MessageBoxImage.Warning);
                    if (res != MessageBoxResult.OK)
                    {
                        return;
                    }
                    orderPayMainViewModel.ActiveForm("ActiveItemR", typeof(OrderPayCardCompListViewModel));
                    break;
                case "ReqAppr":
                    RequestApproval();
                    break;
                default:
                    break;
            }
        });

        /// <summary>
        /// 
        /// </summary>
        void RequestApproval()
        {
            int insMonth = INSTALL_MONTH.ToInt32();            

            if (PAY_AMT <= 50000)
            {
                if (insMonth >= 2)
                {
                    DialogHelper.MessageBox("결제금액 5만원이상 일 경우에 할부개월 입력가능합니다.", GMessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
            }

            // CHANGE_DUTY ==>
            decimal nonTaxAmt = 0;
            decimal incTaxAmtExcTax = 0;
            decimal taxAmt = 0;
            decimal rPayAmt = 0;
            orderPayMainViewModel.CalcAmtsWithDuty(pAY_AMT, out nonTaxAmt, out incTaxAmtExcTax, out taxAmt, out rPayAmt);

            VanAPI vanAPI = new VanAPI();
            VanRequestMsg msg = new VanRequestMsg()
            {
                VAN_CODE = DataLocals.POSVanConfig.VAN_CODE,  // Smartro 밴코드
                POS_NO = DataLocals.AppConfig.PosInfo.PosNo,
                TID = DataLocals.POSVanConfig.VAN_TERM_NO,
                BIZ_NO = DataLocals.POSVanConfig.BIZ_NO,
                TRAN_AMT = rPayAmt.ToString(),
                // CHANGE_DUTY ==>
                ORG_AMT = incTaxAmtExcTax.ToString(),
                TAX_AMT = taxAmt.ToString(),
                DUTY_AMT = nonTaxAmt.ToString(), 
                SVC_AMT = "0",
                INS_MON = INSTALL_MONTH.ToInt32().ToString("d2"),
                SIGN_AT = "2",
                WORKING_KEY = "",
                WORKING_KEY_INDEX = ""
            };

            VanResponseMsg result = null;
            string errorMsg = string.Empty;
            bool isUserCancelled = false;
            try
            {
                result = vanAPI.ApprovalCreditCard(msg);
                errorMsg = "[" + result.RESPONSE_CODE + "] " + result.DISPLAY_MSG;
                isUserCancelled = result.RESPONSE_CODE == ResultCode.UserCancelled;
                LogHelper.Logger.Trace(result.ToString());
            }
            catch (Exception ex)
            {
                LogHelper.Logger.Error(ex);
                errorMsg = ex.ToFormattedString();
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
                payCard.CRD_CARD_NO = result?.CARD_NO;
                payCard.APPR_REQ_AMT = result.AUTH_AMT.ToDecimal();
                payCard.APPR_AMT = result.AUTH_AMT.ToDecimal();
                payCard.VAT_AMT = result.TAX_AMT.ToDecimal();
                // CHANGE_DUTY ==>
                payCard.DUTY_AMT = result.DUTY_AMT.ToDecimal();
                payCard.SVC_TIP_AMT = result.SVC_AMT.ToDecimal();
                payCard.INST_MM_FLAG = INSTALL_MONTH.ToInt32() == 0 ? "0" : "1";
                payCard.INST_MM_CNT = result.INS_MON.ToInt16();

                payCard.APPR_NO = result?.AUTH_NO;
                payCard.APPR_DATE = result?.TRAN_REQ_DATE;
                payCard.APPR_TIME = result.TRAN_REQ_TIME;
                payCard.VAN_SLIP_NO = result.TRAN_UNIQ_NO;
                payCard.APPR_MSG = result?.DISPLAY_MSG;
                payCard.VAN_CODE = DataLocals.POSVanConfig.VAN_CODE;

                payCard.ISS_CRDCP_CODE = result.ISSUER_CODE.Substring(0, 4);
                payCard.ISS_CRDCP_NAME = result.ISSUER_NM;
                payCard.PUR_CRDCP_CODE = result.ACQUIRER_CODE.Substring(0, 4);
                payCard.PUR_CRDCP_NAME = result.ACQUIRER_NM;
                payCard.CRDCP_TERM_NO = result.MER_NO;
                payCard.APPR_LOG_NO = result.TRAN_UNIQ_NO;
                payCard.CRDCP_CODE = payCard.ISS_CRDCP_CODE;
                payCard.INSERT_DT = DateTime.Now.ToString("yyyyMMddHHmmss");

                var compPayInfo = new COMPPAY_PAY_INFO()
                {
                    PAY_TYPE_CODE = OrderPayConsts.PAY_CARD,
                    PAY_CLASS_NAME = nameof(TRN_CARD),
                    PAY_VM_NANE = this.GetType().Name,
                    APPR_IDT_NO = payCard.CRD_CARD_NO,
                    APPR_NO = payCard.APPR_NO,
                    APPR_AMT = payCard.APPR_AMT,
                    APPR_PROC_FLAG = "1",
                    PayDatas = new object[] { payCard }
                };

                if ("COMP_PAY".Equals(CALLER_ID))
                {
                    IoC.Get<OrderPayCompPayViewModel>().UpdatePaymentTRN(this.GetType().Name, compPayInfo);
                }
                else
                {
                    orderPayMainViewModel.UpdatePaymentTRN(this.GetType().Name, compPayInfo);
                }
            }

            this.DeactivateClose(false);
        }

        void TempCardApproval(TRN_CARD tempCard)
        {
            payCard.CRD_CARD_NO = tempCard.CRD_CARD_NO;
            payCard.APPR_REQ_AMT = PAY_AMT;
            payCard.APPR_AMT = PAY_AMT;
            payCard.VAT_AMT = PAY_AMT.AmtTax();
            payCard.INST_MM_FLAG = INSTALL_MONTH.ToInt32() == 0 ? "0" : "1";
            payCard.INST_MM_CNT = (short)INSTALL_MONTH.ToInt32();
            payCard.APPR_DATE = DateTime.Today.ToString("yyyyMMdd");
            payCard.APPR_TIME = DateTime.Now.ToString("HHmmss");
            payCard.APPR_NO = tempCard.APPR_NO;
            payCard.APPR_MSG = string.Empty;
            payCard.APPR_PROC_FLAG = "0";
            payCard.VAN_CODE = tempCard.VAN_CODE;
            payCard.CRDCP_CODE = tempCard.CRDCP_CODE;
            payCard.INSERT_DT = DateTime.Now.ToString("yyyyMMddHHmm");

            var compPayInfo = new COMPPAY_PAY_INFO()
            {
                PAY_TYPE_CODE = OrderPayConsts.PAY_CARD,
                PAY_CLASS_NAME = nameof(TRN_CARD),
                PAY_VM_NANE = this.GetType().Name,
                APPR_IDT_NO = payCard.CRD_CARD_NO,
                APPR_NO = payCard.APPR_NO,
                APPR_AMT = payCard.APPR_AMT,
                APPR_PROC_FLAG = "1",
                PayDatas = new object[] { payCard }
            };

            if ("COMP_PAY".Equals(CALLER_ID))
            {
                IoC.Get<OrderPayCompPayViewModel>().UpdatePaymentTRN(this.GetType().Name, compPayInfo);
            }
            else
            {
                orderPayMainViewModel.UpdatePaymentTRN(this.GetType().Name, compPayInfo);
            }
            this.DeactivateClose(false);
        }

        public Task HandleAsync(OrderPayCardCardCompReturnEventArgs message, CancellationToken cancellationToken)
        {
            TempCardApproval(message.TempCardPay);
            return Task.CompletedTask;
        }
    }

    public class OrderPayCardCardCompReturnEventArgs : EventArgs
    {
        public TRN_CARD TempCardPay { get; set; }
    }
}
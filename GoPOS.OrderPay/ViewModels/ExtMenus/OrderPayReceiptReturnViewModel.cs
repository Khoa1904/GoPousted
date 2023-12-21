using Caliburn.Micro;
using GoPOS.Common.Interface.View;
using GoPOS.Models.Custom.OrderMng;
using GoPOS.Models.Custom.Payment;
using GoPOS.OrderPay.Interface.View;
using GoPOS.Service.Service.API;
using GoPOS.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GoPOS.OrderPay.Models;
using System.Collections.ObjectModel;
using GoPOS.Helpers.CommandHelper;
using System.Windows.Controls;
using System.Windows.Input;
using GoPOS.Models;
using GoPOS.Helpers;
using GoPosVanAPI.Van;
using GoPosVanAPI.Msg;
using GoPosVanAPI.Api;
using GoPOS.Models.Common;
using GoShared.Helpers;
using System.Windows;
using GoPOS.Models.Custom.API;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Reflection;
using GoPOS.Services;
using GoPOS.Service;
using GoPOS.Service.Common;
using System.Windows.Controls.Ribbon;
using GoPOS.Common.Interface.Model;
using Newtonsoft.Json;

namespace GoPOS.ViewModels
{
    public class OrderPayReceiptReturnViewModel : OrderPayChildViewModel
    {
        private IOrderPayReceiptReturnView _view;
        private COMPPAY_PAY_INFO selectedPay;
        private RECEIPT_MANAGER_ITEM selectedBill;
        private readonly ITranApiService tranApiService;
        private readonly IOrderPayService orderPayService;
        private IOrderPayMainViewModel orderPayMainViewModel;

        public ObservableCollection<COMPPAY_PAY_INFO> PayList { get; set; }
        public ObservableCollection<COMPPAY_PAY_INFO> HandlePayList { get; set; } = new ObservableCollection<COMPPAY_PAY_INFO>();

        public COMPPAY_PAY_INFO SelectedPay
        {
            get => selectedPay; set
            {
                selectedPay = value;
                if (value == null)
                {
                    return;
                }

                HandlePayList.Clear();
                if ("A".Equals(value.PROC_METHOD))
                {
                    return;
                }

                HandlePayList.Add(value);
                ApprListIndex = HandlePayList.Count > 0 ? 0 : -1;

                NotifyOfPropertyChange(nameof(SelectedPay));
                NotifyOfPropertyChange(nameof(HandlePayList));
                NotifyOfPropertyChange(nameof(SelectedPay.CANC_INPUT_CARDNO));
            }
        }

        public RECEIPT_MANAGER_ITEM SelectedBill
        {
            get => selectedBill; set
            {
                selectedBill = value;
                NotifyOfPropertyChange(nameof(SelectedBill));
            }
        }

        public string HANDLE_AFF_CARD_NO { get; set; }
        public string HANDLE_TEMP_NO { get; set; }
        public string HANDLE_CPN_NO { get; set; }
        public override object ActivateType => "ExceptKeyPad";
        private TranData tranData = null;
        private int payListIndex;
        private int apprListIndex;

        public int PayListIndex
        {
            get => payListIndex; set
            {
                payListIndex = value;
                NotifyOfPropertyChange(() => PayListIndex);
            }
        }
        public int ApprListIndex
        {
            get => apprListIndex; set
            {
                apprListIndex = value;
                NotifyOfPropertyChange(() => ApprListIndex);
            }
        }

        public OrderPayReceiptReturnViewModel(IWindowManager windowManager,
            IEventAggregator eventAggregator,
            ITranApiService tranApiService,
            IOrderPayService orderPayService) : base(windowManager, eventAggregator)

        {
            this.ViewLoaded += OrderPayReceiptReturnViewModel_ViewLoaded;
            this.tranApiService = tranApiService;
            this.orderPayService = orderPayService;

            orderPayMainViewModel = (IOrderPayMainViewModel)IoC.Get<OrderPayMainViewModel>();
        }

        private void OrderPayReceiptReturnViewModel_ViewLoaded(object? sender, EventArgs e)
        {
            tranData = tranApiService.GetTRData(SelectedBill.SHOP_CODE, SelectedBill.POS_NO,
                SelectedBill.SALE_DATE, SelectedBill.REGI_SEQ, SelectedBill.BILL_NO);

            var compList = tranData.TranTenderSeq.ToList().ToPayInfos(tranData.TranCard, tranData.TranCash, tranData.TranCashRec,
                tranData.TranGift, tranData.TranFoodCpn, tranData.TranPartnerCard, tranData.TranEasyPay, tranData.TranPointuse,
                tranData.TranPointSave, tranData.TranPpCard); // later add prepay

            PayList = new ObservableCollection<COMPPAY_PAY_INFO>(compList);
            NotifyOfPropertyChange(nameof(PayList));
            PayListIndex = PayList.Count > 0 ? 0 : -1;
        }

        public override bool SetIView(IView view)
        {
            _view = (IOrderPayReceiptReturnView)view;
            return base.SetIView(view);
        }

        public override void SetData(object data)
        {
            object[] datas = data as object[];
            SelectedBill = datas[0] as RECEIPT_MANAGER_ITEM;
        }

        public ICommand ButtonCommand => new RelayCommand<Button>(button =>
        {
            switch (button.Tag.ToString())
            {
                case "TempRegist": // 임의등록
                    break;

                case "RequestInput": // 입력요청
                    RequestInput();
                    break;

                case "RequestReturn": // 반품처리
                    RequestReturn();
                    break;
                default:
                    break;
            }
        });


        /// <summary>
        /// 
        /// </summary>
        private void RequestInput()
        {
            if (selectedPay == null)
            {
                return;
            }

            VanAPI vanAPI = new VanAPI();
            if (OrderPayConsts.PAY_CASHREC.Equals(selectedPay.PAY_TYPE_CODE))
            {
                VanRequestMsg msg = new VanRequestMsg()
                {
                    VAN_CODE = DataLocals.POSVanConfig.VAN_CODE,  // Smartro 밴코드
                    REQ_SIGN_PAD_CODE = SystemHelper.ReqSignPadCode,
                    TID = DataLocals.POSVanConfig.VAN_TERM_NO,
                    POS_NO = DataLocals.AppConfig.PosInfo.PosNo,
                };
                VanResponseMsg result = vanAPI.RequestSignPad(msg);
                if (result.RESPONSE_CODE == ResultCode.VanSuccess)
                {
                    selectedPay.CANC_INPUT_CARDNO = result.PIN;
                }
                else
                {
                    DialogHelper.MessageBox(result.DISPLAY_MSG, GMessageBoxButton.OK, MessageBoxImage.Error);
                }

                return;
            }
        }


        /// <summary>
        ///  - 현금영수증인 경우 "입력요청" 버튼을 Click하여 핸드폰 번호를 카드리더기로 
        ///         부터 입력 받아 "카드번호" Edit창에 표시 한 후 취소승인 처리를 한다.(수기등록도 가능한다)
        ///  - 신용카드인 경우 "취소승인' 버튼을 Click 한 후 리더기에 신용카드를 Reading(IC 카드 삽입, 또는 MSR Reading) 하면 승인 취소 처리를 한다.
        ///   - 간편결제인 경우 "입력요청" 버튼을 Click하여 간편결제 바코드 정보를 입력 받아 "카드번호" Edit창에 표시 한 후 취소승인 처리를 한다.(수기등록도 가능한다)
        /// </summary>
        private async void RequestReturn()
        {
            if (selectedPay != null)
            {
                if (OrderPayConsts.PAY_CASHREC.Equals(selectedPay.PAY_TYPE_CODE) &&
                    string.IsNullOrEmpty(selectedPay.CANC_INPUT_CARDNO))
                {
                    DialogHelper.MessageBox("입력요청 먼저 하십시오.", GMessageBoxButton.OK, System.Windows.MessageBoxImage.Warning);
                    return;
                }

                if (OrderPayConsts.PAY_EASY.Equals(selectedPay.PAY_TYPE_CODE))
                {
                    ProcessCancel_SimplePay();
                }
                if (OrderPayConsts.PAY_CASHREC.Equals(selectedPay.PAY_TYPE_CODE))
                {
                    ProcessCancel_CashRec();
                }
                if (OrderPayConsts.PAY_CARD.Equals(selectedPay.PAY_TYPE_CODE))
                {
                    ProcessCancel_Card();
                }
                if (OrderPayConsts.PAY_PPCARD.Equals(selectedPay.PAY_TYPE_CODE))
                {
                    await ProcessCancel_Prepayment();
                }
            }

            CheckCancelCompleted();
        }

        /// <summary>
        /// if all pays are canceld?
        /// if any failed, ask to process FORCE CANCEL
        /// finalize TR and returns to caller UI
        /// </summary>
        private void CheckCancelCompleted()
        {
            var cntRemainAppr = PayList.Count(p => p.PROC_STATUS == "N" && p.PROC_METHOD == "M");
            if (cntRemainAppr > 0)
            {
                //DialogHelper.MessageBox("수동취소 먼저 하십시오.", GMessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            bool forceCancel = false;
            var hasErrors = PayList.Count(p => p.PROC_STATUS == "E" && p.PROC_METHOD == "M");
            if (hasErrors > 0)
            {
                var res = DialogHelper.MessageBox("승인취소 오류가 있습니다. 강제 취소처리 하시겠습니까?", GMessageBoxButton.OKCancel, MessageBoxImage.Question);
                if (res != MessageBoxResult.OK)
                {
                    return;
                }

                forceCancel = true;
            }

            // prepare headers
            string normShopCode = tranData.TranHeader.SHOP_CODE;
            string normPosNo = tranData.TranHeader.POS_NO;
            string normSaleDate = tranData.TranHeader.SALE_DATE;
            string normRegiSeq = tranData.TranHeader.REGI_SEQ;
            string normBillNo = tranData.TranHeader.BILL_NO;

            tranData.TranHeader.SALE_DATE = DataLocals.PosStatus.SALE_DATE;
            tranData.TranHeader.REGI_SEQ = DataLocals.PosStatus.REGI_SEQ;
            tranData.TranHeader.ORG_BILL_NO = tranData.TranHeader.SHOP_CODE + tranData.TranHeader.SALE_DATE + tranData.TranHeader.POS_NO + tranData.TranHeader.BILL_NO;
            tranData.TranHeader.SALE_YN = "N";

            // pays

            /*
             * 
             *    - 신용카드, 간편결제 : 신용카드, 간편결제 TRAN → 승인처리구분이 "1"(포스승인), 
             *    "2"(단말기승인)인 경우는 "수동취소" 표시를 하고 아닌 경우 "자동취소" 표시를 한다.

             * */
            // for loop and and close returns
            for (int i = 0; i < PayList.Count; i++)
            {
                var pay = PayList[i];
                if (pay.PayDatas == null)
                {
                    continue;
                }

                foreach (var payItem in pay.PayDatas)
                {
                    PropertyInfo pi = null;
                    pi = payItem.GetType().GetProperty("SALE_YN", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
                    if (pi != null)
                    {
                        pi.SetValue(payItem, "N");
                    }
                }
            }

            // orderPayService
            /// TO DO
            ///     - 반품이면 -정산
            ///     - 출력안함 
            ///     - 전송확인
            /// 

            var trRes = orderPayService.SaveOrderPayTR(false, tranData.TranHeader, tranData.TranProduct, tranData.TranTenderSeq,
                tranData.TranCash ?? new TRN_CASH[0], tranData.TranCashRec ?? new TRN_CASHREC[0], tranData.TranCard ?? new TRN_CARD[0],
                tranData.TranPartnerCard ?? new TRN_PARTCARD[0], tranData.TranGift ?? new TRN_GIFT[0],
                tranData.TranFoodCpn ?? new TRN_FOODCPN[0], tranData.TranEasyPay ?? new TRN_EASYPAY[0],
                tranData.TranPointuse, tranData.TranPointSave, null, tranData.TranPpCard ?? new TRN_PPCARD[0], null).Result;

            // update new BILL NO of return receipt
            string returnBillNo = tranData.TranHeader.SHOP_CODE + tranData.TranHeader.SALE_DATE + tranData.TranHeader.POS_NO + tranData.TranHeader.BILL_NO;

            if (trRes.Item1.ResultType != EResultType.SUCCESS)
            {
                DialogHelper.MessageBox(trRes.Item1.ResultMessage, GMessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            orderPayService.UpdateOrderPayReturnTR(normShopCode, normPosNo, normSaleDate, normRegiSeq, normBillNo, returnBillNo);
            selectedBill.ORG_BILL_NO = returnBillNo;

            _eventAggregator.PublishOnUIThreadAsync(new ReceiptMngListEvent()
            {
                Item = selectedBill,
                EventType = 0
            });

            this.DeactivateClose(true);
        }

        /// <summary>
        /// 카드결제
        /// </summary>
        private void ProcessCancel_Card()
        {

            VanAPI vanAPI = new VanAPI();
            var payCard = (TRN_CARD)selectedPay.PayDatas[0];

            VanRequestMsg msg = new VanRequestMsg()
            {
                VAN_CODE = DataLocals.POSVanConfig.VAN_CODE,  // Smartro 밴코드
                POS_NO = DataLocals.AppConfig.PosInfo.PosNo,
                TID = DataLocals.POSVanConfig.VAN_TERM_NO,
                BIZ_NO = DataLocals.POSVanConfig.BIZ_NO,
                // CHANGE_DUTY ==>
                TRAN_AMT = payCard.APPR_AMT.ToString(),
                ORG_AMT = (payCard.APPR_AMT - payCard.DUTY_AMT - payCard.VAT_AMT).ToString(),
                TAX_AMT = payCard.VAT_AMT.ToString(),
                SVC_AMT = "0",
                DUTY_AMT = payCard.DUTY_AMT.ToString(),
                INS_MON = payCard.INST_MM_CNT.ToString("d2"),
                AUTH_NO = payCard.APPR_NO,
                TRAN_DATE = payCard.APPR_DATE,
                SIGN_AT = "2",
                WORKING_KEY = "",
                WORKING_KEY_INDEX = ""
            };

            VanResponseMsg result = null;
            string errorMsg = string.Empty;
            try
            {
                result = vanAPI.CancelCreditCard(msg);
                errorMsg = result.DISPLAY_MSG;
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
                if (result.RESPONSE_CODE == ResultCode.UserCancelled)
                    return;
            }

            payCard.ORG_APPR_DATE = payCard.APPR_DATE;
            payCard.ORG_APPR_NO = payCard.APPR_NO;

            payCard.CRD_CARD_NO = result?.CARD_NO;
            payCard.APPR_REQ_AMT = result.RESPONSE_CODE != ResultCode.VanSuccess ? payCard.APPR_REQ_AMT : result.AUTH_AMT.ToDecimal();
            payCard.APPR_AMT = result.RESPONSE_CODE != ResultCode.VanSuccess ? payCard.APPR_AMT : result.AUTH_AMT.ToDecimal();
            payCard.VAT_AMT = result.RESPONSE_CODE != ResultCode.VanSuccess ? payCard.VAT_AMT : result.TAX_AMT.ToDecimal();
            // CHANGE_DUTY ==>
            payCard.DUTY_AMT = result.RESPONSE_CODE != ResultCode.VanSuccess ? payCard.DUTY_AMT : result.DUTY_AMT.ToDecimal();

            payCard.SVC_TIP_AMT = result.RESPONSE_CODE != ResultCode.VanSuccess ? payCard.SVC_TIP_AMT : result.SVC_AMT.ToDecimal();
            payCard.INST_MM_FLAG = payCard.INST_MM_CNT == 0 ? "0" : "1";
            payCard.INST_MM_CNT = result.INS_MON.ToInt16();

            payCard.APPR_NO = result?.AUTH_NO;
            payCard.APPR_DATE = result.RESPONSE_CODE != ResultCode.VanSuccess ? DateTime.Today.ToString("yyyyMMdd") : result?.TRAN_REQ_DATE;
            payCard.APPR_TIME = result.RESPONSE_CODE != ResultCode.VanSuccess ? DateTime.Now.ToString("HHmmss") : result.TRAN_REQ_TIME;
            payCard.APPR_PROC_FLAG = result.RESPONSE_CODE == ResultCode.VanSuccess ? "2" : "0";
            payCard.VAN_SLIP_NO = result.TRAN_UNIQ_NO;
            payCard.APPR_MSG = result?.DISPLAY_MSG;
            payCard.VAN_CODE = DataLocals.POSVanConfig.VAN_CODE;

            payCard.ISS_CRDCP_CODE = result.ISSUER_CODE.Substring(0, Math.Min(4, result.ISSUER_CODE.Length));
            payCard.ISS_CRDCP_NAME = result.ISSUER_NM;
            payCard.PUR_CRDCP_CODE = result.ACQUIRER_CODE.Substring(0, Math.Min(4, result.ACQUIRER_CODE.Length));
            payCard.PUR_CRDCP_NAME = result.ACQUIRER_NM;
            payCard.CRDCP_TERM_NO = result.MER_NO;
            payCard.APPR_LOG_NO = result.TRAN_UNIQ_NO;
            payCard.CRDCP_CODE = payCard.ISS_CRDCP_CODE;
            payCard.INSERT_DT = DateTime.Now.ToString("yyyyMMddHHmmss");
            payCard.SALE_YN = "N";

            SelectedPay.PROC_STATUS = result.RESPONSE_CODE == ResultCode.VanSuccess ? "C" : "E";
            SelectedPay.APPR_PROC_FLAG = SelectedPay.PROC_STATUS == "C" ? "1" : "0";
            SelectedPay.PayDatas[0] = payCard;
        }

        private async Task ProcessCancel_Prepayment()
        {
            var payPpCard = (TRN_PPCARD)selectedPay.PayDatas[0];
            var _apiRequest = new ApiRequest(DataLocals.AppConfig.PosComm.SvrURLServer,
                   DataLocals.AppConfig.PosInfo.StoreNo,
                   DataLocals.AppConfig.PosInfo.PosNo,
                   DataLocals.TokenInfo.LICENSE_ID,
                   DataLocals.TokenInfo.LICENSE_KEY);

            var pairs = new
            {
                storeCode = DataLocals.AppConfig.PosInfo.StoreNo,
                salesDt = DataLocals.PosStatus.SALE_DATE,
                posNo = DataLocals.AppConfig.PosInfo.PosNo,
                billNo = DataLocals.PosStatus.BILL_NO,
                saleSeCode = "N",
                mbrCode = payPpCard.PPC_CST_NO,
                useAmt = payPpCard.PPC_AMT,
                createdAt = DateTime.Now.ToString("yyyyMMddHHmmss"),
                orgApprInfo = payPpCard.SHOP_CODE + payPpCard.SALE_DATE + payPpCard.POS_NO + payPpCard.BILL_NO,
                orgApprNo = payPpCard.APPR_NO
                //orgApprInfo = DataLocals.AppConfig.PosInfo.StoreNo + DataLocals.PosStatus.SALE_DATE.ToString() + DataLocals.PosStatus.POS_NO
                //+ DataLocals.PosStatus.BILL_NO,
            };

            string errorMessage = string.Empty;
            var w = await _apiRequest.Request("/client/inquiry/prepaid/use").PostBodyAsync(pairs);
            if (w.status == "200")
            {
                try
                {
                    var members = JsonHelper.JsonToModel<PREPAYMENT_USEINFO>(Convert.ToString(w.results));
                    PREPAYMENT_USEINFO retUseInfo = null;
                    if (members.result == ResultCode.Ok)
                    {
                        retUseInfo = members.model;

                        payPpCard.APPR_NO = retUseInfo.apprNo;
                        payPpCard.ORG_APPR_INFO = retUseInfo.orgApprInfo;
                        payPpCard.ORG_APPR_NO = retUseInfo.orgApprNo;
                        payPpCard.PPC_BALANCE = retUseInfo.chargeRemAmt;
                    }
                }
                catch (Exception ex)
                {
                    errorMessage = "API 오류. 관리자에게 문의하세요. !" + Environment.NewLine + ex.ToFormattedString();
                }
            }
            else
            {
                if (w.ResultMsg.Length > 0)
                {
                    errorMessage = w.ResultMsg.ToString() + "\nError Code: " + w.status;
                }
                else
                {
                    errorMessage = w.results.ToString() + "\nError Code: " + w.status;
                }

            }

            SelectedPay.PROC_STATUS = string.IsNullOrEmpty(errorMessage) ? "C" : "E";            
            SelectedPay.APPR_PROC_FLAG = SelectedPay.PROC_STATUS == "C" ? "1" : "0";
            SelectedPay.PayDatas[0] = payPpCard;
        }

        /// <summary>
        /// 
        /// </summary>
        private void ProcessCancel_SimplePay()
        {
            try
            {
                VanAPI vanAPI = new VanAPI();
                VanRequestMsg msg = new VanRequestMsg()
                {
                    VAN_CODE = DataLocals.POSVanConfig.VAN_CODE,  // Smartro 밴코드
                    TID = DataLocals.POSVanConfig.VAN_TERM_NO,
                    POS_NO = DataLocals.AppConfig.PosInfo.PosNo,
                    REQ_SIGN_PAD_CODE = "BQ",
                };

                VanResponseMsg result = vanAPI.RequestSignPad(msg);

                if (result.RESPONSE_CODE == ResultCode.UserCancelled)
                {
                    return;
                }

                this.selectedPay.CANC_INPUT_CARDNO = result.BARCODE;
                TRN_EASYPAY easyPay = (TRN_EASYPAY)selectedPay.PayDatas[0];
                msg = new VanRequestMsg()
                {
                    VAN_CODE = DataLocals.POSVanConfig.VAN_CODE,  // Smartro 밴코드
                    POS_NO = DataLocals.AppConfig.PosInfo.PosNo,
                    TID = DataLocals.POSVanConfig.VAN_TERM_NO,
                    BIZ_NO = DataLocals.POSVanConfig.BIZ_NO,
                    // CHANGE_DUTY ==>
                    TRAN_AMT = easyPay.APPR_REQ_AMT.ToString(),
                    ORG_AMT = (easyPay.APPR_REQ_AMT - easyPay.DUTY_AMT - easyPay.VAT_AMT).ToString(),
                    TAX_AMT = easyPay.VAT_AMT.ToString(),
                    DUTY_AMT = easyPay.DUTY_AMT.ToString(),
                    SVC_AMT = "0",
                    INS_MON = "00",
                    SIGN_AT = "2",
                    BARCODE = this.selectedPay.CANC_INPUT_CARDNO,
                    AUTH_NO = easyPay.APPR_NO,
                    TRAN_DATE = easyPay.APPR_DATE,
                    WORKING_KEY = string.Empty,
                    WORKING_KEY_INDEX = string.Empty,
                    PAY_CODE = easyPay.PAYCP_CODE
                };

                result = vanAPI.CancelPay(msg);
                if (result.RESPONSE_CODE != ResultCode.VanSuccess)
                {
                    DialogHelper.MessageBox(result.DISPLAY_MSG, GMessageBoxButton.OK, System.Windows.MessageBoxImage.Warning);
                    if (result.RESPONSE_CODE == ResultCode.UserCancelled)
                        return;
                }

                #region Make TRN_EASY

                easyPay.ORG_APPR_DATE = easyPay.APPR_DATE;
                easyPay.ORG_APPR_NO = easyPay.APPR_NO;

                easyPay.PAY_CARD_NO = result.CARD_NO;
                easyPay.APPR_REQ_AMT = result.RESPONSE_CODE != ResultCode.VanSuccess ? easyPay.APPR_REQ_AMT : result.AUTH_AMT.ToDecimal();
                easyPay.APPR_PROC_FLAG = result.RESPONSE_CODE == ResultCode.VanSuccess ? "2" : "0";
                easyPay.VAT_AMT = result.RESPONSE_CODE != ResultCode.VanSuccess ? easyPay.VAT_AMT : result.TAX_AMT.ToDecimal();
                // CHANGE_DUTY ==>
                easyPay.DUTY_AMT = result.RESPONSE_CODE != ResultCode.VanSuccess ? easyPay.DUTY_AMT : result.DUTY_AMT.ToDecimal();
                easyPay.INST_MM_FLAG = "0";
                easyPay.INST_MM_CNT = 0;
                easyPay.PAY_METHOD_FLAG = "B";
                easyPay.APPR_DATE = result.RESPONSE_CODE != ResultCode.VanSuccess ? DateTime.Today.ToString("yyyyMMdd") : result.TRAN_REQ_DATE;
                easyPay.APPR_TIME = result.RESPONSE_CODE != ResultCode.VanSuccess ? DateTime.Now.ToString("HHmmss") : result.TRAN_REQ_TIME;
                easyPay.APPR_NO = result.AUTH_NO;
                easyPay.VAN_CODE = DataLocals.POSVanConfig.VAN_CODE;
                easyPay.VAN_PAYCP_CODE = result.PAY_CODE;
                easyPay.ISS_CRDCP_CODE = result.ISSUER_CODE.Substring(0, Math.Min(4, result.ISSUER_CODE.Length));
                easyPay.ISS_CRDCP_NAME = result.ISSUER_NM;
                easyPay.PUR_CRDCP_CODE = result.ACQUIRER_CODE.Substring(0, Math.Min(4, result.ACQUIRER_CODE.Length));
                easyPay.PUR_CRDCP_NAME = result.ACQUIRER_NM;
                easyPay.APPR_MSG = result.RESPONSE_MSG;
                easyPay.CRDCP_TERM_NO = string.Empty;
                easyPay.INSERT_DT = DateTime.Now.ToString("yyyyMMddHHmmss");
                easyPay.SALE_YN = "N";

                #endregion

                SelectedPay.PROC_STATUS = result.RESPONSE_CODE == ResultCode.VanSuccess ? "C" : "E";
                SelectedPay.APPR_PROC_FLAG = SelectedPay.PROC_STATUS == "C" ? "1" : "0";
                SelectedPay.PayDatas[0] = easyPay;
            }
            catch (Exception ex)
            {
                LogHelper.Logger.Error(ex.ToFormattedString());
                DialogHelper.MessageBox(ex.Message, GMessageBoxButton.OK, System.Windows.MessageBoxImage.Warning);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void ProcessCancel_CashRec()
        {
            try
            {
                VanAPI vanAPI = new VanAPI();
                var cashRecTR = (TRN_CASHREC)selectedPay.PayDatas[0];

                VanRequestMsg msg = new VanRequestMsg()
                {
                    VAN_CODE = DataLocals.POSVanConfig.VAN_CODE,  // Smartro 밴코드
                    POS_NO = DataLocals.AppConfig.PosInfo.PosNo,
                    TID = DataLocals.POSVanConfig.VAN_TERM_NO,
                    BIZ_NO = DataLocals.POSVanConfig.BIZ_NO,
                    // CHANGE_DUTY ==>
                    TRAN_AMT = cashRecTR.APPR_AMT.ToString(),
                    ORG_AMT = (cashRecTR.APPR_AMT - cashRecTR.DUTY_AMT - cashRecTR.VAT_AMT).ToString(),
                    TAX_AMT = cashRecTR.VAT_AMT.ToString(),
                    DUTY_AMT = cashRecTR.DUTY_AMT.ToString(),
                    SVC_AMT = "0",
                    SIGN_AT = selectedPay.CANC_INPUT_CARDNO == "" ? "0" : "3",
                    CASH_TRAN_CODE = cashRecTR.APPR_IDT_TYPE == "1" ? "03" : "13",
                    CASH_CANCEL_CODE = "3",
                    AUTH_NO = cashRecTR.APPR_NO,
                    TRAN_DATE = cashRecTR.APPR_DATE,
                    SIGN_DATA = selectedPay.CANC_INPUT_CARDNO
                };

                VanResponseMsg result = vanAPI.CancelCash(msg);
                if (result.RESPONSE_CODE != ResultCode.VanSuccess)
                {
                    DialogHelper.MessageBox(result.DISPLAY_MSG, GMessageBoxButton.OK, System.Windows.MessageBoxImage.Warning);
                    if (result.RESPONSE_CODE == ResultCode.UserCancelled)
                        return;
                }

                #region Make TRN_CASHREC

                cashRecTR.ORG_APPR_DATE = cashRecTR.APPR_DATE;
                cashRecTR.ORG_APPR_NO = cashRecTR.APPR_NO;
                cashRecTR.APPR_AMT = result.RESPONSE_CODE != ResultCode.VanSuccess ? cashRecTR.APPR_AMT : result.AUTH_AMT.ToDecimal();
                cashRecTR.APPR_FLAG = "0";
                cashRecTR.APPR_IDT_FLAG = "2";
                cashRecTR.APPR_IDT_NO = selectedPay.CANC_INPUT_CARDNO == null ? result.CARD_NO : selectedPay.CANC_INPUT_CARDNO;
                cashRecTR.APPR_MSG = result.DISPLAY_MSG;
                cashRecTR.APPR_NO = result.AUTH_NO;
                cashRecTR.APPR_PROC_FLAG = result.RESPONSE_CODE == ResultCode.VanSuccess ? "2" : "0";
                cashRecTR.APPR_DATE = result.RESPONSE_CODE != ResultCode.VanSuccess ? DateTime.Today.ToString("yyyyMMdd") : result.TRAN_REQ_DATE;
                cashRecTR.APPR_TIME = result.RESPONSE_CODE != ResultCode.VanSuccess ? DateTime.Now.ToString("HHmmss") : result.TRAN_REQ_TIME;
                cashRecTR.CARD_IN_FLAG = "K";
                cashRecTR.INSERT_DT = DateTime.Now.ToString("yyyyMMddHHmmss");
                cashRecTR.SALE_YN = "N";
                cashRecTR.VAN_CODE = DataLocals.POSVanConfig.VAN_CODE;
                cashRecTR.VAN_SLIP_NO = result.TRAN_UNIQ_NO;
                cashRecTR.VAT_AMT = result.TAX_AMT.ToDecimal();
                // CHANGE_DUTY ==>
                cashRecTR.DUTY_AMT = result.DUTY_AMT.ToDecimal();
                cashRecTR.SVC_TIP_AMT = result.SVC_AMT.ToDecimal();

                #endregion

                SelectedPay.PROC_STATUS = result.RESPONSE_CODE == ResultCode.VanSuccess ? "C" : "E";
                SelectedPay.APPR_PROC_FLAG = SelectedPay.PROC_STATUS == "C" ? "1" : "0";
                SelectedPay.PayDatas[0] = cashRecTR;
            }
            catch (Exception ex)
            {
                LogHelper.Logger.Error(ex.ToFormattedString());
                DialogHelper.MessageBox(ex.Message, GMessageBoxButton.OK, System.Windows.MessageBoxImage.Warning);
            }

        }
    }
}

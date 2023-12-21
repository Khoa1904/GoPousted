using Caliburn.Micro;
using GoPOS.Common.Interface.Model;
using GoPOS.Common.ViewModels;
using GoPOS.Helpers.CommandHelper;
using GoPOS.Helpers;
using GoPOS.Models;
using GoPOS.Models.Common;
using GoPOS.Models.Custom.Payment;
using GoPOS.Service.Service.Payment;
using GoPosVanAPI.Api;
using GoPosVanAPI.Msg;
using GoPosVanAPI.Van;
using GoPOS.Service.Common;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows;
using GoShared.Helpers;
using GoPOS.Models.Custom.API;
using GoPOS.Service;
using GoPOS.Services;
using System.Diagnostics;
using System.Reflection;
using GoPOS.Sales.Services;
using GoPOS.Service.Service.API;
using Newtonsoft.Json;

namespace GoPOS.ViewModels
{
    public class CreditCardCancelPopupViewModel : BaseItemViewModel, IDialogViewModel
    {
        private readonly ITranApiService tranApiService;
        private readonly ISalesMngService salesMngService;

        public decimal PAY_AMT
        {
            get => pAY_AMT; set
            {
                pAY_AMT = value;
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

        public NTRN_PRECHARGE_CARD dataGet
        {
            get;
            set;
        }

        public COMPPAY_PAY_INFO compPayInfo;
        private readonly IOrderPayCardService cardService;
        private IOrderPayMainViewModel orderPayMainViewModel;

        private NTRN_PRECHARGE_CARD payCard = null;
        private int seqNo = 1;
        private string CALLER_ID = string.Empty;
        private decimal pAY_AMT;
        private string iNSTALL_MONTH;
        private string cARD_NO;
        public CreditCardCancelPopupViewModel(IWindowManager windowManager, IEventAggregator eventAggregator, ITranApiService tranApiService, ISalesMngService salesMngService) : base(windowManager, eventAggregator)
        {
            this.tranApiService = tranApiService;
            this.salesMngService = salesMngService;
        }
        public Dictionary<string, object> DialogResult { get; set; }

        public ICommand ButtonCommand => new RelayCommand<Button> (async(button) =>
        {
            switch (button.Tag.ToString())
            {
                case "ReqAppr":
                    RequestApproval();
                    break;
                default:
                    break;
            }
        });


        public override void SetData(object data)
        {
            object[] pams = data as object[];
            dataGet = pams.Length > 0 ? (NTRN_PRECHARGE_CARD)pams[0] : null; //JsonConvert.DeserializeObject<NTRN_PRECHARGE_CARD>(x);

            if (dataGet != null)
            {
                PAY_AMT = dataGet.APPR_AMT;
            }
            this.DialogResult = new Dictionary<string, object>();
            //isCancelTR = Convert.ToInt32(pams[0]) == 1 ? true : false;
            //CardChargeTitle = isCancelTR ? "신용카드 취소" : "신용카드";
            //NotifyOfPropertyChange(() => CardChargeTitle);
            //PAY_AMT = pams.Length > 1 ? (decimal)pams[0] : 0;
        }


        /// <summary>
        /// 
        /// </summary>
        async void RequestApproval()
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

            VanAPI vanAPI = new VanAPI();
            VanRequestMsg msg = new VanRequestMsg()
            {
                VAN_CODE = DataLocals.POSVanConfig.VAN_CODE, // Smartro 밴코드
                POS_NO = DataLocals.AppConfig.PosInfo.PosNo,
                TID = DataLocals.POSVanConfig.VAN_TERM_NO,
                BIZ_NO = DataLocals.POSVanConfig.BIZ_NO,
                TRAN_AMT = Convert.ToInt32(PAY_AMT).ToString(),
                ORG_AMT = PAY_AMT.AmtNoTax().ToString(),
                TAX_AMT = PAY_AMT.AmtTax().ToString(),
                SVC_AMT = "0",
                INS_MON = 0.ToString("d2"),
                SIGN_AT = "2",
                WORKING_KEY = "",
                WORKING_KEY_INDEX = "",
                TRAN_DATE = dataGet.APPR_DATE,
                AUTH_NO = dataGet.APPR_NO
                 ,
                DUTY_AMT = "0"
            };

            VanResponseMsg result = null;
            string errorMsg = string.Empty;
            bool isUserCancelled = false;
            try
            {
                result = vanAPI.CancelCreditCard(msg);
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
                payCard = new NTRN_PRECHARGE_CARD();
                payCard.CRD_CARD_NO = result?.CARD_NO;
                payCard.APPR_REQ_AMT = result.AUTH_AMT.ToDecimal();
                payCard.APPR_AMT = result.AUTH_AMT.ToDecimal();
                payCard.VAT_AMT = result.TAX_AMT.ToDecimal();
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

                compPayInfo = new COMPPAY_PAY_INFO()
                {
                    PAY_TYPE_CODE = OrderPayConsts.PAY_CARD,
                    PAY_CLASS_NAME = nameof(TRN_CARD),
                    PAY_VM_NANE = this.GetType().Name,
                    APPR_IDT_NO = payCard.CRD_CARD_NO,
                    APPR_NO = payCard.APPR_NO,
                    APPR_AMT = payCard.APPR_AMT,
                    APPR_PROC_FLAG = "1",
                    PROC_STATUS = result.RESPONSE_CODE == ResultCode.VanSuccess || string.IsNullOrEmpty(result.DISPLAY_MSG) ? "C" : "E",
                };

                compPayInfo.PayDatas = new object[] { payCard };
                compPayInfo.APPR_PROC_FLAG = compPayInfo.PROC_STATUS == "C" ? "1" : "0";
                compPayInfo.PROC_STATUS = result.RESPONSE_CODE == ResultCode.VanSuccess || string.IsNullOrEmpty(result.DISPLAY_MSG) ? "C" : "E";

           //     CheckCancelCompleted();
                this.DialogResult = new Dictionary<string, object>();
                this.DialogResult.Add("RETURN_CANCEL_DATA", payCard);
            }
            this.DeactivateClose(false);
        }

        /// <summary>
        /// if all pays are canceld?
        /// if any failed, ask to process FORCE CANCEL
        /// finalize TR and returns to caller UI
        /// </summary>
        private async void CheckCancelCompleted()
        {
            var cntRemainAppr = compPayInfo.PROC_STATUS == "N" && compPayInfo.PROC_METHOD == "M";
            if (cntRemainAppr)
            {
                //DialogHelper.MessageBox("수동취소 먼저 하십시오.", GMessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            bool forceCancel = false;
            var hasErrors = compPayInfo.PROC_STATUS == "E" && compPayInfo.PROC_METHOD == "M";
            if (hasErrors)
            {
                var res = DialogHelper.MessageBox("승인취소 오류가 있습니다. 강제 취소처리 하시겠습니까?", GMessageBoxButton.OKCancel, MessageBoxImage.Question);
                if (res != MessageBoxResult.OK)
                {
                    return;
                }

                forceCancel = true;
            }

            NonTransModel tranData = tranApiService.GetNonTRData(DataLocals.AppConfig.PosInfo.StoreNo,
                DataLocals.AppConfig.PosInfo.PosNo,
                DataLocals.PosStatus.SALE_DATE,
                DataLocals.PosStatus.REGI_SEQ,
                dataGet.SALE_NO
            );

            // prepare headers
            string normShopCode = tranData.SHOP_CODE;
            string normPosNo = tranData.POS_NO;
            string normSaleDate = tranData.SALE_DATE;
            string normRegiSeq = DataLocals.PosStatus.REGI_SEQ;
            string normBillNo = tranData.SALE_NO;


            // pays
            /*
             * 
             *    - 신용카드, 간편결제 : 신용카드, 간편결제 TRAN → 승인처리구분이 "1"(포스승인), 
             *    "2"(단말기승인)인 경우는 "수동취소" 표시를 하고 아닌 경우 "자동취소" 표시를 한다.
             * */
            // for loop and and close returns
        
            // orderPayService
            /// TO DO
            ///     - 반품이면 -정산
            ///     - 출력안함 
            ///     - 전송확인
            /// 

     

            // update new BILL NO of return receipt
            string returnBillNo = tranData.SHOP_CODE + tranData.SALE_DATE + tranData.POS_NO + tranData.SALE_NO;
            salesMngService.UpdateOrderPayReturnTR(normShopCode, normPosNo, normSaleDate, normRegiSeq, normBillNo, returnBillNo);
            this.DeactivateClose(true);
        }
    }
}

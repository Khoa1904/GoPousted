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
using GoPOS.Models.Config;
using GoPOS.Models.Custom.API;
using static GoPosVanAPI.Api.VanAPI;
using System.Security.Cryptography;

namespace GoPOS.ViewModels
{
    public class CreditCardPopupViewModel : BaseItemViewModel, IDialogViewModel
    {
        private bool isCancelTR = false;
        public decimal PAY_AMT
        {
            get => pAY_AMT; set
            {
                pAY_AMT = IoC.Get<SalesPayPrepaymentViewModel>().PAY_AMT;

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
        public decimal Set_PAY_AMT { get; set; } = 0;
        public string CardChargeTitle { get; set; }

        private readonly IOrderPayCardService cardService;
        private IOrderPayMainViewModel orderPayMainViewModel;

        private int seqNo = 1;
        private string CALLER_ID = string.Empty;
        private decimal pAY_AMT;
        private string iNSTALL_MONTH;
        private string cARD_NO;
        public CreditCardPopupViewModel(IWindowManager windowManager, IEventAggregator eventAggregator) : base(windowManager, eventAggregator)
        {
            this.ViewLoaded += Screen_Viewload;
        }

        public Dictionary<string, object> DialogResult { get; set; }

        public ICommand ButtonCommand => new RelayCommand<Button>(async (button) =>
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
        private void Screen_Viewload(object? sender, EventArgs e)
        {
            PAY_AMT = Set_PAY_AMT == 0 ? 0 : Set_PAY_AMT;
            INSTALL_MONTH = string.Empty;
            payCard = null;
        }

        private NTRN_PRECHARGE_CARD payCard = null;
        public override void SetData(object data)
        {
            //object[] pams = data as object[];
            //isCancelTR = Convert.ToInt32(pams[0]) == 1 ?true: false;
            //CardChargeTitle = isCancelTR ? "신용카드 취소" : "신용카드";
            //NotifyOfPropertyChange(() => CardChargeTitle);

            //if (isCancelTR)
            //{
            //    payCard = pams.Length > 1 ? (NTRN_PRECHARGE_CARD)pams[1] : null;
            //}
            //else
            //{
            //    PAY_AMT = pams.Length > 1 ? (decimal)pams[1] : 0;
            //}

            object[] pams = data as object[];
            isCancelTR = Convert.ToInt32(pams[0]) == 1 ? true : false;
            CardChargeTitle = isCancelTR ? "신용카드 취소" : "신용카드";
            NotifyOfPropertyChange(() => CardChargeTitle);
            PAY_AMT = pams.Length > 1 ? (decimal)pams[0] : 0;
            this.DialogResult = new Dictionary<string, object>();
            this.DialogResult.Add("RETURN_DATA", null);
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
                VAN_CODE = DataLocals.POSVanConfig.VAN_CODE,  // Smartro 밴코드
                POS_NO = DataLocals.AppConfig.PosInfo.PosNo,
                TID = DataLocals.POSVanConfig.VAN_TERM_NO,
                BIZ_NO = DataLocals.POSVanConfig.BIZ_NO,
                TRAN_AMT = PAY_AMT.ToString(),
                ORG_AMT = PAY_AMT.AmtNoTax().ToString(),
                TAX_AMT = PAY_AMT.AmtTax().ToString(),
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
                payCard.CHARGE_YN = isCancelTR ? "N" : "Y";
                payCard.ISS_CRDCP_CODE = result.ISSUER_CODE.Substring(0, 4);
                payCard.ISS_CRDCP_NAME = result.ISSUER_NM;
                payCard.PUR_CRDCP_CODE = result.ACQUIRER_CODE.Substring(0, 4);
                payCard.PUR_CRDCP_NAME = result.ACQUIRER_NM;
                payCard.CRDCP_TERM_NO = result.MER_NO;
                payCard.APPR_LOG_NO = result.TRAN_UNIQ_NO;
                payCard.CRDCP_CODE = payCard.ISS_CRDCP_CODE;
                payCard.INSERT_DT = DateTime.Now.ToString("yyyyMMddHHmmss");

                //var compPayInfo = new COMPPAY_PAY_INFO()
                //{
                //    PAY_TYPE_CODE = OrderPayConsts.PAY_CARD,
                //    PAY_CLASS_NAME = nameof(TRN_CARD),
                //    PAY_VM_NANE = this.GetType().Name,
                //    APPR_IDT_NO = payCard.CRD_CARD_NO,
                //    APPR_NO = payCard.APPR_NO,
                //    APPR_AMT = payCard.APPR_AMT,
                //    APPR_PROC_FLAG = "1",
                //    PayDatas = new object[] { payCard }
                //};

                //orderPayMainViewModel.UpdatePaymentTRN(this.GetType().Name, compPayInfo);
                this.DialogResult = new Dictionary<string, object>();
                this.DialogResult.Add("RETURN_DATA", payCard);
            }
            IoC.Get<SalesPayPrepaymentViewModel>().ChargeHistory();

            this.DeactivateClose(false);
        }
    }
}

using Caliburn.Micro;
using GoPOS.Common.Interface.Model;
using GoPOS.Common.ViewModels;
using GoPOS.Helpers;
using GoPOS.Helpers.CommandHelper;
using GoPOS.Models;
using GoPOS.Models.Common;
using GoPOS.Models.Custom.Payment;
using GoPOS.Service;
using GoPOS.Service.Common;
using GoPosVanAPI.Api;
using GoPosVanAPI.Msg;
using GoPosVanAPI.Van;
using GoShared.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace GoPOS.ViewModels
{
    public class OrderPayCashReceiptPopupViewModel : BaseItemViewModel, IDialogViewModel
    {
        private string invidualNo;
        private bool privateChecked = true;
        private bool companyChecked;
        private decimal paidMoney;

        public OrderPayCashReceiptPopupViewModel(IWindowManager windowManager, IEventAggregator eventAggregator) : base(windowManager, eventAggregator)
        {
            this.ViewLoaded += OrderPayCashReceiptPopupViewModel_ViewLoaded;
        }

        private void OrderPayCashReceiptPopupViewModel_ViewLoaded(object? sender, EventArgs e)
        {
            this.InvidualNo = string.Empty;
            this.PrivateChecked = true;
            DialogResult = new Dictionary<string, object>();
        }

        public string InvidualNo
        {
            get => invidualNo; set
            {
                invidualNo = value;
                NotifyOfPropertyChange(nameof(InvidualNo));
            }
        }

        public bool PrivateChecked
        {
            get => privateChecked; set
            {
                {
                    privateChecked = value;
                    NotifyOfPropertyChange(nameof(PrivateChecked));
                }
            }
        }

        public bool CompanyChecked
        {
            get => companyChecked; set
            {
                companyChecked = value;
                NotifyOfPropertyChange(nameof(CompanyChecked));
            }
        }

        /// <summary>
        /// 받은금액
        /// </summary>
        public decimal PaidMoney
        {
            get => paidMoney; set
            {
                paidMoney = value;
                NotifyOfPropertyChange(nameof(PaidMoney));
            }
        }


        // CHANGE_DUTY ==>
        private decimal DutyAmt;

        /// <summary>
        /// 
        /// </summary>
        private TRN_CASHREC cashRecTR;

        public ICommand ButtonCommand => new RelayCommand<Button>((button) =>
        {
            if (button.Tag == null) return;

            VanAPI vanAPI = new VanAPI();
            switch (button.Tag.ToString())
            {
                case "InputRequest": // request to input number from SignPad
                    #region 입력요청

                    try
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
                            this.InvidualNo = result.PIN;
                            
                        }
                        else
                        {
                            DialogHelper.MessageBox(result.DISPLAY_MSG, GMessageBoxButton.OK, MessageBoxImage.Error);
                        }

                    }
                    catch (Exception ex)
                    {
                        DialogHelper.MessageBox(ex.ToFormattedString(), GMessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    #endregion

                    break;
                #region 현금영수증 요청

                case "CashRecApprRequest": // 현금영수증 요청
                    #region 현금영수증

                    DialogResult = new();

                    try
                    {
                        VanRequestMsg msg = new VanRequestMsg()
                        {
                            VAN_CODE = DataLocals.POSVanConfig.VAN_CODE,  // Smartro 밴코드
                            POS_NO = DataLocals.AppConfig.PosInfo.PosNo,
                            TID = DataLocals.POSVanConfig.VAN_TERM_NO,
                            BIZ_NO = DataLocals.POSVanConfig.BIZ_NO,
                            TRAN_AMT = Convert.ToInt32(PaidMoney).ToString(),
                            ORG_AMT = PaidMoney.AmtNoTax().ToString(),
                            TAX_AMT = PaidMoney.AmtTax().ToString(),
                            // CHANGE_DUTY ==>
                            DUTY_AMT = DutyAmt.ToString(),
                            SVC_AMT = "0",
                            SIGN_AT = this.InvidualNo == "" ? "0" : "3",
                            CASH_TRAN_CODE = privateChecked ? "03" : "13",
                            SIGN_DATA = this.invidualNo
                        };

                        VanResponseMsg result = vanAPI.ApprovalCash(msg);
                        if (result.RESPONSE_CODE == ResultCode.VanSuccess)
                        {
                            string apprNo = result.AUTH_NO;
                            cashRecTR.APPR_AMT = result.AUTH_AMT.ToDecimal();
                            cashRecTR.APPR_DATE = result.TRAN_REQ_DATE;
                            cashRecTR.APPR_FLAG = "0";
                            cashRecTR.APPR_IDT_FLAG = "2";
                            cashRecTR.APPR_IDT_NO = this.InvidualNo == "" ? result.CARD_NO : this.InvidualNo;
                            cashRecTR.APPR_IDT_TYPE = privateChecked ? "1" : "2";
                            cashRecTR.APPR_MSG = result.DISPLAY_MSG;
                            cashRecTR.APPR_NO = result.AUTH_NO;
                            cashRecTR.APPR_PROC_FLAG = "2";
                            cashRecTR.APPR_TIME = result.TRAN_REQ_TIME;
                            cashRecTR.CARD_IN_FLAG = "K";
                            cashRecTR.INSERT_DT = DateTime.Now.ToString("yyyyMMddHHmmss");
                            cashRecTR.VAN_CODE = DataLocals.POSVanConfig.VAN_CODE;
                            cashRecTR.VAN_SLIP_NO = result.TRAN_UNIQ_NO;
                            cashRecTR.VAT_AMT = result.TAX_AMT.ToDecimal();
                            // CHANGE_DUTY ==>
                            cashRecTR.DUTY_AMT = result.DUTY_AMT.ToDecimal();
                            cashRecTR.SVC_TIP_AMT = result.SVC_AMT.ToDecimal();

                            DialogResult.Add("RETURN_DATA", cashRecTR);
                            this.TryCloseAsync(true);
                        }
                        else
                        {
                            throw new Exception(result.DISPLAY_MSG);
                        }
                    }
                    catch (Exception ex)
                    {
                        DialogHelper.MessageBox(ex.ToFormattedString(), GMessageBoxButton.OK, MessageBoxImage.Error);
                    }

                    #endregion
                    break;

                #endregion

                default:
                    break;
            }
        });

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        public override void SetData(object data)
        {
            object[] datas = data as object[];
            PaidMoney = (decimal)datas[0];
            int seqNo = (int)datas[1];
            seqNo++;

            //
            cashRecTR = new TRN_CASHREC()
            {
                SHOP_CODE = DataLocals.PosStatus.SHOP_CODE,
                POS_NO = DataLocals.PosStatus.POS_NO,
                SALE_DATE = DataLocals.PosStatus.SALE_DATE,
                BILL_NO = DataLocals.PosStatus.BILL_NO,
                SEQ_NO = seqNo.ToString("d4"),
                REGI_SEQ = DataLocals.PosStatus.REGI_SEQ,
                SALE_YN = "Y"
            };

            this.InvidualNo = string.Empty;
            this.PrivateChecked = true;
        }

        public Dictionary<string, object> DialogResult { get; set; }
    }
}

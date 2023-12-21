using AutoMapper;
using Caliburn.Micro;
using CoreWCF.Channels;
using Dapper;
using FirebirdSql.Data.FirebirdClient;
using FirebirdSql.Data.Services;
using GoPOS.Common.Interface.Model;
using GoPOS.Common.ViewModels;
using GoPOS.Common.Views.Controls;
using GoPOS.Database;
using GoPOS.Helpers;
using GoPOS.Helpers.CommandHelper;
using GoPOS.Models;
using GoPOS.Models.Common;
using GoPOS.Models.Config;
using GoPOS.Models.Custom.Payment;
using GoPOS.Payment.Models;
using GoPOS.Service;
using GoPOS.Service.Common;
using GoPOS.Services;
using GoPOS.Views;
using GoPosVanAPI.Api;
using GoPosVanAPI.Msg;
using GoPosVanAPI.Van;
using GoShared.Events;
using GoShared.Helpers;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NLog.Fluent;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;
using System.Web.Services.Description;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Interop;
using System.Xml.Linq;
using static GoPOS.Function;

namespace GoPOS.ViewModels;

/// <summary>
/// 작성자: 김형석 
/// 화면명: 현금 결제
/// </summary>

public class OrderPayCashViewModel : OrderPayChildViewModel
{
    private decimal paidMoney;
    private IOrderPayMainViewModel orderPayMainViewModel;
    private decimal receiveMoney = 0;
    private decimal remainMoney;
    private string invidualNo = string.Empty;
    private string apprNo = string.Empty;
    private bool privateChecked = true;
    private bool companyChecked = false;
    private string CALLER_ID = string.Empty;
    private bool fgFirstLoad = false;
    private TRN_CASH cashTR = null;
    private TRN_CASHREC cashRecTR = null;

    private int seqNo = 1;

    public OrderPayCashViewModel(IWindowManager windowManager, IEventAggregator eventAggregator) :
            base(windowManager, eventAggregator)
    {
        this.ViewInitialized += OrderPayCashViewModel_ViewInitialized;
        this.ViewLoaded += OrderPayCashViewModel_ViewLoaded;
    }

    private void OrderPayCashViewModel_ViewLoaded(object? sender, EventArgs e)
    {
        fgFirstLoad = false;
        InvidualNo = string.Empty;
        ApprNo = string.Empty;
        PrivateChecked = true;
        ReceiveMoney = orderPayMainViewModel.OrderSumInfo.EXP_PAY_AMT;
        cashRecTR = null;
    }

    private void OrderPayCashViewModel_ViewInitialized(object? sender, EventArgs e)
    {
        orderPayMainViewModel = (IOrderPayMainViewModel)Parent;
    }

    public override object ActivateType { get => "ExceptKeyPad"; }

    /// <summary>
    /// 받을금액
    /// </summary>
    public decimal ReceiveMoney
    {
        get => receiveMoney; set
        {
            receiveMoney = value;
            NotifyOfPropertyChange(nameof(ReceiveMoney));
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
            RemainMoney = value > ReceiveMoney ? value - ReceiveMoney : 0;
            NotifyOfPropertyChange(nameof(PaidMoney));
        }
    }

    /// <summary>
    /// 거스름돈
    /// </summary>
    public decimal RemainMoney
    {
        get => remainMoney; set
        {
            remainMoney = value;
            NotifyOfPropertyChange(nameof(RemainMoney));
        }
    }
    public string InvidualNo
    {
        get => invidualNo; set
        {
            invidualNo = value;
            NotifyOfPropertyChange(nameof(InvidualNo));
        }
    }
    public string ApprNo
    {
        get => apprNo; set
        {
            apprNo = value;
            NotifyOfPropertyChange(nameof(ApprNo));
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
    /// ① 입력요청 버튼 Click을 하면 서명패드에 휴대폰 번호를 입력 할 수 있게 요청을 한다.(VAN API를 통해 요청)
    /// ② 임의등록 버튼 Click을 하면 현금영수증 승인을 받지 않고 단순현금 처리를 한다.
    /// ③ 승인요청 버튼 Click을 하면 "확인번호"에 입력한 번호를 이용하여 현금영수증 승인 요청을 한다.(VAN API를 통해 요청)
    /// 확인번호가 없는 상태에서 승인요청 버튼을 Click 하게되면 현금영수증 자진발급 승인 요청을 한다.(자진빌급번호 : 010-000-1234)
    /// ① 단순혐금 버튼을 Click 하면 현금영수증 승인없이 바로 현금 결제가 이루어지게 한다. (화면의 승인요청이 잘못된 것임)
    /// </summary>
    public ICommand ButtonCommand => new RelayCommand<Button>((button) =>
    {
        if (button.Tag == null) return;

        VanAPI vanAPI = new VanAPI();
        VanResponseMsg result = null;
        switch (button.Tag.ToString())
        {
            case "SimpleAppr": // oNLY Save Cash, not request 현금영수증
                IoC.Get<InputBoxKeyPadView>().ClearText();
                ReturnClose();
                break;

            case "InputRequest": // request to input number from SignPad
                #region 입력요청

                try
                {
                    VanRequestMsg msg = new VanRequestMsg()
                    {
                        VAN_CODE = DataLocals.POSVanConfig.VAN_CODE,  // Smartro 밴코드
                        TID = DataLocals.POSVanConfig.VAN_TERM_NO,
                        POS_NO = DataLocals.AppConfig.PosInfo.PosNo,
                        REQ_SIGN_PAD_CODE = SystemHelper.ReqSignPadCode,
                    };

                    result = vanAPI.RequestSignPad(msg);
                    if (result.RESPONSE_CODE == ResultCode.VanSuccess)
                    {
                        this.InvidualNo = result.PIN;
                    }
                }
                catch (Exception ex)
                {
                    DialogHelper.MessageBox(ex.ToFormattedString(), GMessageBoxButton.OK, MessageBoxImage.Error);
                }
                #endregion

                break;
            case "CashRecApprRequest": // 현금영수증 요청
                #region 현금영수증

                string errorMsg = string.Empty;
                bool isUserCancelled = false;
                try
                {
                    // CHANGE_DUTY ==>
                    decimal nonTaxAmt = 0;
                    decimal incTaxAmtExcTax = 0;
                    decimal taxAmt = 0;
                    decimal rPayAmt = 0;
                    decimal rAppAmt = PaidMoney - remainMoney;
                    orderPayMainViewModel.CalcAmtsWithDuty(rAppAmt, out nonTaxAmt, out incTaxAmtExcTax, out taxAmt, out rPayAmt);

                    VanRequestMsg msg = new VanRequestMsg()
                    {
                        VAN_CODE = DataLocals.POSVanConfig.VAN_CODE,  // Smartro 밴코드
                        POS_NO = DataLocals.AppConfig.PosInfo.PosNo,
                        TID = DataLocals.POSVanConfig.VAN_TERM_NO,
                        BIZ_NO = DataLocals.POSVanConfig.BIZ_NO,
                        TRAN_AMT = rPayAmt.ToString(),
                        ORG_AMT = incTaxAmtExcTax.ToString(),
                        TAX_AMT = taxAmt.ToString(),
                        SVC_AMT = "0",
                        // CHANGE_DUTY ==>
                        DUTY_AMT = nonTaxAmt.ToString(),
                        SIGN_AT = this.InvidualNo == "" ? "0" : "3",
                        CASH_TRAN_CODE = privateChecked ? "03" : "13",
                        SIGN_DATA = this.InvidualNo
                    };

                    result = vanAPI.ApprovalCash(msg);
                    errorMsg = "[" + result.RESPONSE_CODE + "] " + result.DISPLAY_MSG;
                    isUserCancelled = result.RESPONSE_CODE == ResultCode.UserCancelled;
                    LogHelper.Logger.Trace(result.ToString());
                }
                catch (Exception ex)
                {
                    //DialogHelper.MessageBox(ex.ToFormattedString(), GMessageBoxButton.OK, MessageBoxImage.Error);
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
                    ApprNo = result.AUTH_NO;
                    int paySeq = orderPayMainViewModel.GetTRPaySeq("OrderPayCashRecViewModel");
                    paySeq++;

                    cashRecTR = new TRN_CASHREC()
                    {
                        SHOP_CODE = DataLocals.PosStatus.SHOP_CODE,
                        POS_NO = DataLocals.PosStatus.POS_NO,
                        SALE_DATE = DataLocals.PosStatus.SALE_DATE,
                        BILL_NO = DataLocals.PosStatus.BILL_NO,
                        SEQ_NO = paySeq.ToString("d4"),
                        REGI_SEQ = DataLocals.PosStatus.REGI_SEQ,
                        APPR_AMT = result.AUTH_AMT.ToDecimal(),
                        APPR_DATE = result.TRAN_REQ_DATE,
                        APPR_FLAG = "0",
                        APPR_IDT_FLAG = "2",
                        APPR_IDT_NO = this.InvidualNo == "" ? result.CARD_NO : this.InvidualNo,
                        APPR_IDT_TYPE = privateChecked ? "1" : "2",
                        APPR_MSG = result.DISPLAY_MSG,
                        APPR_NO = result.AUTH_NO,
                        APPR_PROC_FLAG = "2",
                        APPR_TIME = result.TRAN_REQ_TIME,
                        CARD_IN_FLAG = "K",
                        INSERT_DT = DateTime.Now.ToString("yyyyMMddHHmmss"),
                        SALE_YN = "Y",
                        VAN_CODE = DataLocals.POSVanConfig.VAN_CODE,
                        VAN_SLIP_NO = result.TRAN_UNIQ_NO,
                        VAT_AMT = result.TAX_AMT.ToDecimal(),                        
                        // CHANGE_DUTY ==>
                        DUTY_AMT = result.DUTY_AMT.ToDecimal(),
                        SVC_TIP_AMT = result.SVC_AMT.ToDecimal()
                    };

                    ReturnClose();
                }

                #endregion
                break;
            default:
                int amt = Convert.ToInt32(button.Tag.ToString());
                if (!fgFirstLoad)
                {
                    PaidMoney = 0;
                    fgFirstLoad = true;
                }
                PaidMoney += amt;
                break;
        }
    });

    /// <summary>
    /// 
    /// </summary>
    private void ReturnClose()
    {
        cashTR.BILL_NO = DataLocals.PosStatus.BILL_NO;
        cashTR.CASH_AMT = PaidMoney;
        cashTR.RET_AMT = RemainMoney;
        cashTR.INSERT_DT = DateTime.Now.ToString("yyyyMMddHHmmss");

        var compPayInfo = new COMPPAY_PAY_INFO()
        {
            PAY_TYPE_CODE = cashRecTR != null ? OrderPayConsts.PAY_CASHREC : OrderPayConsts.PAY_CASH,
            PAY_CLASS_NAME = nameof(TRN_CASH),
            PAY_VM_NANE = this.GetType().Name,
            APPR_IDT_NO = cashRecTR != null ? cashRecTR.APPR_IDT_NO : string.Empty,
            APPR_NO = cashRecTR != null ? cashRecTR.APPR_NO : string.Empty,
            APPR_AMT = cashTR.CASH_AMT,
            APPR_PROC_FLAG = "1",
            PayDatas = new object[] { cashTR, cashRecTR }
        };

        ApprNo = "";
        InvidualNo = "";
        this.DeactivateClose(false);

        if ("COMP_PAY".Equals(CALLER_ID))
        {
            IoC.Get<OrderPayCompPayViewModel>().UpdatePaymentTRN(this.GetType().Name, compPayInfo);
        }
        else
        {
            orderPayMainViewModel.UpdatePaymentTRN(this.GetType().Name, compPayInfo);
        }
    }

    /// <summary>
    /// RecvMoney
    /// Count of CASH Pay
    /// </summary>
    /// <param name="data"></param>
    public override void SetData(object data)
    {
        cashTR = new TRN_CASH()
        {
            SHOP_CODE = DataLocals.PosStatus.SHOP_CODE,
            POS_NO = DataLocals.PosStatus.POS_NO,
            SALE_DATE = DataLocals.PosStatus.SALE_DATE,
            BILL_NO = DataLocals.PosStatus.BILL_NO,
            SEQ_NO = seqNo.ToString("d4"),
            REGI_SEQ = DataLocals.PosStatus.REGI_SEQ,
            CASH_AMT = 0,
            RET_AMT = 0,
            EX_CODE = string.Empty,
            EX_KRW = 0,
            EX_EXP_AMT = 0,
            EX_IN_AMT = 0,
            EX_RET_AMT = 0,
            KR_RET_AMT = 0,
            EX_PAY_AMT = 0,
            KR_PAY_AMT = 0,
            KR_ETC_AMT = 0,
            SALE_YN = "Y",
            INSERT_DT = DateTime.Now.ToString("yyyyMMddHHmmss")
        };

        object[] datas = (object[])data;
        CALLER_ID = (string)datas[0];

        ReceiveMoney = orderPayMainViewModel.OrderSumInfo.EXP_PAY_AMT;
        PaidMoney = (decimal)datas[1];
        if (PaidMoney <= 0) PaidMoney = receiveMoney;

        seqNo = (int)datas[2];
        seqNo++;
        cashTR.SEQ_NO = seqNo.ToString("d4");
    }
}
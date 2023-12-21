using Caliburn.Micro;
using GoPOS.Helpers;
using GoPOS.Models.Common;
using GoPOS.Models;
using GoPOS.Service.Common;
using GoPOS.ViewModels;
using GoShared.Helpers;
using System.Windows;
using AutoMapper;
using System.Xml.Linq;
using GoPOS.Services;
using System.Windows.Input;
using GoPOS.Helpers.CommandHelper;
using System.Windows.Controls;
using GoPOS.Common.Interface.Model;
using log4net.Core;
using GoPOS.Models.Custom.Payment;
using GoPOS.Service;
using Newtonsoft.Json;
using GoPOS.OrderPay.ViewModels.Controls;
using GoPOS.Views;
using GoPOS.OrderPay.Models;
using GoPOS.Payment.Services;

namespace GoPOS.ViewModels
{
    public class OrderPayPrepaymentUseViewModel : OrderPayChildViewModel, IHandle<MemberInfoPass>
    {
        private IOrderPayMainViewModel orderPayMainViewModel;
        public override object ActivateType { get => "ExceptKeyPad"; }
        private TRN_PPCARD trnPPCard = null;
        private decimal _amtTobe;
        private decimal _paidAmt = 0;
        private decimal _changeAmt;
        private int seqNo = 1;
        private string CALLER_ID = string.Empty;
        private MEMBER_CLASH memberInfo { get; set; }
        public MEMBER_CLASH MemberInfo
        {
            get => memberInfo; set { memberInfo = value; NotifyOfPropertyChange(() => MemberInfo); }

        }
        private List<MEMBERPOINT_HISTORY> ptsHistory { get; set; }
        public List<MEMBERPOINT_HISTORY> PTS_HISTORY
        {
            get => ptsHistory; set { ptsHistory = value; NotifyOfPropertyChange(() => PTS_HISTORY); }

        }
        public void ResetData()
        {
            trnPPCard = null;
            MemberInfo = null;
            TEL_NO = string.Empty;
            CARD_NO = string.Empty;
            MaxAmt = 0;
        }
        public OrderPayPrepaymentUseViewModel(IWindowManager windowManager, IEventAggregator eventAggregator,
            IOrderPayPointStampService orderPayPointStampService) : base(windowManager, eventAggregator)
        {
            this.ViewLoaded += ViewLoadedImpl;
            this.ViewInitialized += ScreenInit;
            this.orderPayPointStampService = orderPayPointStampService;
        }
        private string _txtTelNo = "";
        public string TEL_NO
        {
            get { return _txtTelNo; }
            set
            {
                if (_txtTelNo != value)
                {
                    _txtTelNo = value;
                    NotifyOfPropertyChange(() => TEL_NO);
                }
            }
        }
        private void ViewLoadedImpl(object? sender, EventArgs e)
        {
            TEL_NO = string.Empty;
            CARD_NO = string.Empty;
            MEMBER_NAME = string.Empty;
            if (PTS_HISTORY != null) { PTS_HISTORY.Clear(); }
            if (MemberInfo == null) IoC.Get<OrderPayPrepaymentUseView>().txtSearch_Cst_Tel.Focus();
        }
        private void ScreenInit(object? sender, EventArgs e)
        {
            orderPayMainViewModel = (IOrderPayMainViewModel)Parent;
        }
        private string _cardno = "";
        public string CARD_NO
        {
            get { return _cardno; }
            set { _cardno = value; NotifyOfPropertyChange(() => CARD_NO); }
        }

        public string MmberName2
        {
            get => mmberName2; set
            {
                mmberName2 = value;
                NotifyOfPropertyChange(nameof(MmberName2));
            }
        }

        private string _membername = "";
        private string mmberName2;
        private readonly IOrderPayPointStampService orderPayPointStampService;

        public string MEMBER_NAME
        {
            get { return _membername; }
            set { _membername = value; NotifyOfPropertyChange(() => MEMBER_NAME); }
        }
        public decimal MaxAmt
        {
            get => _amtTobe; set
            {
                _amtTobe = value;
                NotifyOfPropertyChange(nameof(MaxAmt));
            }
        }

        /// <summary>
        /// 받은금액
        /// </summary>
        public decimal PaidAmt
        {
            get => _paidAmt; set
            {
                _paidAmt = value > MaxAmt || value == 0 ? MaxAmt : value;
                NotifyOfPropertyChange(nameof(PaidAmt));
            }
        }
        /// <summary>
        /// 거스름돈
        /// </summary>
        public decimal ChangeAmt
        {
            get => _changeAmt; set
            {
                _changeAmt = value;
                NotifyOfPropertyChange(nameof(ChangeAmt));
            }
        }
        public ICommand ButtonCommand => new RelayCommand<Button>(ButtonCommandCenter);
        private void ButtonCommandCenter(Button btn)
        {
            switch (btn.Tag.ToString())
            {
                case "btnSearch":
                    if (orderPayMainViewModel.payPpCards.Count > 0 || orderPayMainViewModel.payPoints.Count > 0)
                    {
                        DialogHelper.MessageBox("포인트 사용 내역이 있어 회원정보를\r\n초기화 할 수 없습니다.", GMessageBoxButton.OK, System.Windows.MessageBoxImage.Warning);
                        return;
                    }
                    if (string.IsNullOrEmpty(TEL_NO) && string.IsNullOrEmpty(CARD_NO) && string.IsNullOrEmpty(MEMBER_NAME))
                    {
                        IoC.Get<OrderPayMainViewModel>().StatusMessage = "검색조건을 입력해 주세요.";
                        return;
                    }
                    orderPayMainViewModel.ActiveForm("ActiveItemR", "OrderPayMemberSearchViewModel", CARD_NO, TEL_NO, MEMBER_NAME);
                    break;

                case "btnUse":
                    if (memberInfo == null)
                    {
                        DialogHelper.MessageBox("회원을 선택해주세요", GMessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }
                    if (PaidAmt < 1)
                    {
                        DialogHelper.MessageBox("결제 할 내역이 없습니다.", GMessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }
                    if (MemberInfo.ppcAmt < PaidAmt)
                    {
                        DialogHelper.MessageBox("잔액부족합니다.", GMessageBoxButton.OK, MessageBoxImage.Warning); return;
                    }
                    RedeemPrepayment();
                    break;

                default:
                    break;
            }
        }

        public override void SetData(object data)
        {
            object[] datas = (object[])data;
            if (datas[datas.Length - 1] != null)
            {
                this.MemberInfo = (MEMBER_CLASH)datas[datas.Length - 1];
                NotifyOfPropertyChange(() => MemberInfo);
            }

            CALLER_ID = (string)datas[0];

            MaxAmt = orderPayMainViewModel.OrderSumInfo.EXP_PAY_AMT;
            PaidAmt = (decimal)datas[1];
            seqNo = (int)datas[2];
            seqNo++;

            trnPPCard = new TRN_PPCARD()
            {
                SHOP_CODE = DataLocals.PosStatus.SHOP_CODE,
                POS_NO = DataLocals.PosStatus.POS_NO,
                SALE_DATE = DataLocals.PosStatus.SALE_DATE,
                BILL_NO = DataLocals.PosStatus.BILL_NO.StrIntInc(4),
                SEQ_NO = seqNo.ToString("d4"),
                REGI_SEQ = DataLocals.PosStatus.REGI_SEQ,
                PPC_CST_NO = memberInfo != null ? memberInfo.mbrCode : null,
                SALE_YN = "Y",
                INSERT_DT = DateTime.Now.ToString("yyyyMMddHHmmss"),
            };
        }

        private async void RedeemPrepayment()
        {
            if (MemberInfo == null)
            {
                DialogHelper.MessageBox("회원을 선택해 주세요.", GMessageBoxButton.OK, MessageBoxImage.Stop);
                return;
            }

            string message = "";
            var _apiRequest = new ApiRequest(DataLocals.AppConfig.PosComm.SvrURLServer,
                   DataLocals.AppConfig.PosInfo.StoreNo,
                   DataLocals.AppConfig.PosInfo.PosNo,
                   DataLocals.TokenInfo.LICENSE_ID,
                   DataLocals.TokenInfo.LICENSE_KEY);

            var newSaleNo = DataLocals.PosStatus.BILL_NO.StrIntInc(4);
            var checktoken = DataLocals.TokenInfo.TOKEN;
            var pairs = new
            {
                storeCode = DataLocals.AppConfig.PosInfo.StoreNo,
                salesDt = DataLocals.PosStatus.SALE_DATE,
                posNo = DataLocals.AppConfig.PosInfo.PosNo,
                billNo = newSaleNo,
                saleSeCode = "Y",
                mbrCode = memberInfo.mbrCode,
                useAmt = PaidAmt,
                createdAt = DateTime.Now.ToString("yyyyMMddHHmmss"),
                orgApprInfo = "",
                orgApprNo = ""
            };
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
                    }
                    trnPPCard.PPC_AMT = retUseInfo.useAmt;
                    trnPPCard.PPC_BALANCE = retUseInfo.chargeRemAmt;
                    trnPPCard.PPC_CST_NO = retUseInfo.mbrCode;
                    trnPPCard.APPR_NO = retUseInfo.apprNo;
                    //trnPPCard.ORG_APPR_INFO = retUseInfo.orgApprInfo;
                    //trnPPCard.ORG_APPR_NO = retUseInfo.orgApprNo;
                }
                catch (Exception ex)
                {
                    DialogHelper.MessageBox("API 오류. 관리자에게 문의하세요. !", GMessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            else
            {
                if (w.ResultMsg.Length > 0)
                {
                    message = w.ResultMsg.ToString() + "\nError Code: " + w.status;
                }
                else
                {
                    message = w.results.ToString() + "\nError Code: " + w.status;
                }

                DialogHelper.MessageBox(message, GMessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            /// 
            /// 선결제 2번째 이상 사용 시 화면에 선결제 잔액 Refrsh
            /// 
            if (orderPayMainViewModel.payPpCards.Count > 1)
            {
                var memRes = await orderPayPointStampService.GetMemberDetail(DataLocals.PosStatus.POS_NO, DataLocals.PosStatus.SALE_DATE, memberInfo.mbrCode);
                if (memRes.Item2 != null)
                {
                    await _eventAggregator.PublishOnUIThreadAsync(new MemberInfoPass()
                    {
                        memberInfo = memRes.Item2
                    });
                }
            }

            var compPayInfo = new COMPPAY_PAY_INFO()
            {
                PAY_TYPE_CODE = OrderPayConsts.PAY_PPCARD,
                PAY_CLASS_NAME = nameof(TRN_PPCARD),
                PAY_VM_NANE = this.GetType().Name,
                APPR_IDT_NO = string.Empty,
                APPR_NO = string.Empty,
                APPR_AMT = trnPPCard.PPC_AMT,
                APPR_PROC_FLAG = "1",
                PayDatas = new object[] { trnPPCard }
            };

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



        public Task HandleAsync(MemberInfoPass message, CancellationToken cancellationToken)
        {
            MemberInfo = message.memberInfo;
            return Task.CompletedTask;
        }
    }
}

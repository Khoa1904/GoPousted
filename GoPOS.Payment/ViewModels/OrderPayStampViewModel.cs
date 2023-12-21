using Caliburn.Micro;
using GoPOS.Helpers;
using GoPOS.Models.Common;
using GoPOS.Models;
using GoPOS.Service.Common;
using GoPOS.ViewModels;
using GoShared.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
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
using System.Security.Cryptography.X509Certificates;

namespace GoPOS.ViewModels
{
    public class OrderPayStampViewModel : OrderPayChildViewModel
    {
        private IOrderPayMainViewModel orderPayMainViewModel;
        public override object ActivateType { get => "ExceptKeyPad"; }
        private TRN_POINTUSE trnPointUse = null;
        private decimal _amtTobe;
        private decimal _paidAmt = 0;
        private decimal _changeAmt;
        private int seqNo = 1;
        private string callerID = string.Empty;
        public string CALLER_ID
        {
            get => callerID; set { callerID = value; NotifyOfPropertyChange(nameof(CALLER_ID)); }
        }
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
            trnPointUse = null;
            MemberInfo = null;
            CALLER_ID = string.Empty;
            TEL_NO = string.Empty;
            CARD_NO = string.Empty;
            MaxAmt = 0;
        }
        public OrderPayStampViewModel(IWindowManager windowManager, IEventAggregator eventAggregator) : base(windowManager, eventAggregator)
        {
            this.ViewLoaded += ViewLoadedImpl;
            this.ViewInitialized += ScreenInit;
            this.ViewUnloaded += ViewUnLoadedImpl;
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
            if (PTS_HISTORY != null) { PTS_HISTORY.Clear(); }
            orderPayMainViewModel = (IOrderPayMainViewModel)Parent;
            NotifyOfPropertyChange(() => MemberInfo);

        }
        private void ViewUnLoadedImpl(object? sender, EventArgs e)
        {
            memberInfo = null;
            NotifyOfPropertyChange(() => MemberInfo);

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
                _paidAmt = value > MaxAmt ? MaxAmt : value;
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
                    if (string.IsNullOrEmpty(TEL_NO) && string.IsNullOrEmpty(CARD_NO) && string.IsNullOrEmpty(MEMBER_NAME))
                    {
                        orderPayMainViewModel.StatusMessage = "검색조건을 입력해 주세요.";
                        return;
                    }
                    break;
                    if(PTS_HISTORY!= null) { PTS_HISTORY.Clear(); }
                case "btnSearch2":
                    if (MemberInfo == null)
                    {
                        orderPayMainViewModel.StatusMessage = "회원코드를 선택해주세요";
                        return;
                    }
                    else
                    {
                        PointHistory();
                    }
                    break;

                case "btnUse":
                    RedeemPoint();
                    break;

                default:
                    break;
            }
        }

        public override void SetData(object data)
        {
            trnPointUse = new TRN_POINTUSE()
            {
                SHOP_CODE = DataLocals.PosStatus.SHOP_CODE,
                POS_NO = DataLocals.PosStatus.POS_NO,
                SALE_DATE = DataLocals.PosStatus.SALE_DATE,
                BILL_NO = DataLocals.PosStatus.BILL_NO,
                SEQ_NO = seqNo.ToString("d4"),
                REGI_SEQ = DataLocals.PosStatus.REGI_SEQ,
                CST_NO = string.Empty,
                CARD_NO = string.Empty,
                LEVEL = string.Empty,
                TOT_SALE_AMT = 0,
                TOT_DC_AMT = 0,
                TOT_PNT = 0,
                TOT_USE_PNT = 0,
                USE_PNT = 0,
                LAST_PNT = 0,
                SALE_YN = "Y",
                INSERT_DT = DateTime.Now.ToString("yyyyMMddHHmmss")
            };

            object[] datas = (object[])data;
            CALLER_ID = (string)datas[0];

            MaxAmt = orderPayMainViewModel.OrderSumInfo.EXP_PAY_AMT;
            PaidAmt = (decimal)datas[1];
            if (PaidAmt < 1) PaidAmt = MaxAmt;
            seqNo = (int)datas[2];
            seqNo++;
            trnPointUse.SEQ_NO = seqNo.ToString("d4");
            trnPointUse.TOT_SALE_AMT = orderPayMainViewModel.OrderSumInfo.TOT_SALE_AMT;
            trnPointUse.TOT_DC_AMT = orderPayMainViewModel.OrderSumInfo.TOT_DC_AMT;
        }

        private async void RedeemPoint()
        {
            if (MemberInfo == null)
            {
                DialogHelper.MessageBox("회원을 선택해 주세요.", GMessageBoxButton.OK, MessageBoxImage.Stop);
                return;
            }
            if(memberInfo.minUsePt > PaidAmt )
            {
                PaidAmt = memberInfo.minUsePt;
                DialogHelper.MessageBox("결제할 포인트가 최소사용액보다 커야 합니다.", GMessageBoxButton.OK, MessageBoxImage.Information);
            //    orderPayMainViewModel.StatusMessage = " 결제할 포인트가 최소사용액보다 커야 합니다.";
                return;
            }
            if (memberInfo.maxUsePt < PaidAmt)
            {
                PaidAmt = memberInfo.maxUsePt;
                DialogHelper.MessageBox("결제할 포인트 금액은 최대사용액보다 적어야합니다.", GMessageBoxButton.OK, MessageBoxImage.Information);
         //       orderPayMainViewModel.StatusMessage = "결제할 포인트 금액은 최대사용액보다 적어야합니다.";
                return;
            }
            var pointRemain = MemberInfo.avalidPoint;
            //if (pointRemain <= MaxAmt)
            if (pointRemain < PaidAmt)
            {
                var confirm = DialogHelper.MessageBox("가용포인트가 결제할 포인트보다 작습니다", GMessageBoxButton.OK, MessageBoxImage.Stop);
                return;
            }

            string message = "";
            var _apiRequest = new ApiRequest(DataLocals.AppConfig.PosComm.SvrURLServer,
                   DataLocals.AppConfig.PosInfo.StoreNo,
                   DataLocals.AppConfig.PosInfo.PosNo,
                   DataLocals.TokenInfo.LICENSE_ID,
                   DataLocals.TokenInfo.LICENSE_KEY);

            var pairs = new
            {
                posNo = DataLocals.AppConfig.PosInfo.PosNo,
                salesDt = DataLocals.PosStatus.SALE_DATE,
                mbrCode = MemberInfo.mbrCode,
                billNo = DataLocals.PosStatus.BILL_NO.StrIntInc(4),
                totalSaleAmt = trnPointUse.TOT_SALE_AMT,
                totalDcAmt = trnPointUse.TOT_DC_AMT,
                totalPoint = decimal.Truncate(MemberInfo.totalPoint),
                avalidPoint = decimal.Truncate(MemberInfo.avalidPoint),
                usePoint = PaidAmt,
                createdAt = DateTime.Now.ToString("yyyyMMddHHmmss"),
            };
            var token = DataLocals.TokenInfo.TOKEN;
            var w = await _apiRequest.Request("/client/inquiry/point/use").PostBodyAsync(pairs);
            if (w.status == "200")
            {
                try
                {
                    var members = JsonHelper.JsonToModel<MEMBER_CLASH>(Convert.ToString(w.results));
                    if (members.result == ResultCode.Ok)
                    {
                        MemberInfo = members.model;
                    }

                    trnPointUse.CST_NO = MemberInfo.mbrCode;
                    trnPointUse.USE_PNT = PaidAmt;
                    trnPointUse.TOT_USE_PNT = MemberInfo.totalUsePoint;
                    trnPointUse.TOT_PNT = decimal.Truncate(MemberInfo.totalPoint);
                    trnPointUse.LAST_PNT = decimal.Truncate(MemberInfo.avalidPoint);

                    orderPayMainViewModel.MemberInfo.totalUsePoint = MemberInfo.totalUsePoint;
                    orderPayMainViewModel.MemberInfo.totalPoint = decimal.Truncate(MemberInfo.totalPoint);
                    orderPayMainViewModel.MemberInfo.avalidPoint = decimal.Truncate(MemberInfo.avalidPoint);

                    trnPointUse.LEVEL = MemberInfo.mbrGrdCode;
                    trnPointUse.CARD_NO = MemberInfo.mbrCardno;
           //         IoC.Get<OrderPayLeftTRInfoViewModel>().ActiveItem = IoC.Get<PaymentInfoViewModel>();
                }
                catch (Exception ex)
                {
                    DialogHelper.MessageBox("API 오류. 관리자에게 문의하세요. !", GMessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            else
            {
                if(w.ResultMsg.Length > 0)
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

            var compPayInfo = new COMPPAY_PAY_INFO()
            {
                PAY_TYPE_CODE = OrderPayConsts.PAY_POINTS,
                PAY_CLASS_NAME = nameof(TRN_POINTUSE),
                PAY_VM_NANE = this.GetType().Name,
                APPR_IDT_NO = string.Empty,
                APPR_NO = string.Empty,
                APPR_AMT = trnPointUse.USE_PNT,
                APPR_PROC_FLAG = "1",
                PayDatas = new object[] { trnPointUse }
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
        private class MemberPointHistory
        {
            public string mbrCode { get; set; }
            public List<MEMBERPOINT_HISTORY> pointList { get; set; }  // ListMembHistory
        }

        /// <summary>
        /// Fetch point history from API
        /// </summary>
        private async void PointHistory()
        {
            // PTS_HISTORY = new List<MEMBERPOINT_HISTORY>();
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
            var checkchoi = DataLocals.TokenInfo.TOKEN;
            var pairs = new
            {
                posNo = DataLocals.AppConfig.PosInfo.PosNo,
                salesDt = DataLocals.PosStatus.SALE_DATE,
                mbrCode = MemberInfo.mbrCode,
                createdAt = MemberInfo.createdAt

            };
            var w = await _apiRequest.Request("/client/inquiry/point/list").GetDatasBodyAsync(null, pairs);
            if (w.status == "200")
            {
                try
                {
                    string jsonData = Convert.ToString(w.results);
                    MemberPointHistory ObjectData = JsonConvert.DeserializeObject<MemberPointHistory>(jsonData);
                    var data = ObjectData.pointList;
                    PTS_HISTORY = data;
                }
                catch (Exception ex)
                {
                    DialogHelper.MessageBox("API 오류. 관리자에게 문의하세요. !", GMessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                message = w.ResultMsg.ToString() + "\nError Code: " + w.status;
                DialogHelper.MessageBox(message, GMessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }
    }
}

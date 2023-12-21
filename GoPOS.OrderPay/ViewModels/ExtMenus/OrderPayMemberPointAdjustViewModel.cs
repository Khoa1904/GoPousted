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
using GoPOS.OrderPay.ViewModels.Controls;
using GoPOS.Payment.Services;
using GoPOS.OrderPay.Models;
using GoPOS.Common.Interface.View;
using GoPOS.OrderPay.Interface.View;
using System.Diagnostics.Eventing.Reader;

namespace GoPOS.ViewModels
{
    public class OrderPayMemberPointAdjustViewModel : OrderPayChildViewModel, IHandle<MemberInfoSinglePass>
    {
        private IOrderPayMainViewModel orderPayMainViewModel;
        public override object ActivateType { get => "ExceptKeyPad"; }
        private decimal _adjAmt = 0;
        private MEMBER_CLASH memberInfo ;
        public string adJustFlag
        {
            get
            {
                if((bool)_view.Chagam.IsChecked)
                {
                    return "1";
                }
                else { return "0"; }
            }
        }
        public string pointFlag
        {
            get
            {
                if(DataLocals.AppConfig.PosOption.PointStampFlag == "0")
                {
                    return "0";
                }
                else if (DataLocals.AppConfig.PosOption.PointStampFlag == "1" && DataLocals.AppConfig.PosOption.StampUseMethod == "0")
                {
                    return "1";
                }
                else if (DataLocals.AppConfig.PosOption.PointStampFlag == "1" && DataLocals.AppConfig.PosOption.StampUseMethod == "1")
                {
                    return "2";
                }
                else { return " "; }
            }
        }
        public MEMBER_CLASH MemberInfo
        {
            get => memberInfo; set { memberInfo = value; NotifyOfPropertyChange(() => MemberInfo); }

        }

        private string _pointTitle { get; set; } = string.Empty;
        public string PointTitle
        {
            get
            {
                return DataLocals.AppConfig.PosOption.PointStampFlag == "0" ? "회원포인트 조정" : "회원스탬프 조정";
            }
            set
            {
                _pointTitle = value; NotifyOfPropertyChange(() => PointTitle);
            }
        }
        private string _payTitle { get; set; } = string.Empty;
        public string PayTitle
        {
            get
            {
                return DataLocals.AppConfig.PosOption.PointStampFlag == "0" ? "포인트사용액" : "스탬프 금액사용액";
            }
            set
            {
                _payTitle = value; NotifyOfPropertyChange(() => PayTitle);
            }
        }
        private List<MEMBERPOINT_HISTORY> ptsHistory { get; set; }
        public List<MEMBERPOINT_HISTORY> PTS_HISTORY
        {
            get => ptsHistory; set { ptsHistory = value; NotifyOfPropertyChange(() => PTS_HISTORY); }

        }
        void ResetData()
        {
            TEL_NO = string.Empty;
            CARD_NO = string.Empty;
            MEMBER_NAME = string.Empty;
            AdjAmt = 0;
        }
        public OrderPayMemberPointAdjustViewModel(IWindowManager windowManager, IEventAggregator eventAggregator, IOrderPayPointStampService orderPayPointStampService) 
            : base(windowManager, eventAggregator)
        {
            this.ViewLoaded += ViewLoadedImpl;
            this.ViewInitialized += ScreenInit;
            this.ViewUnloaded += ViewUnLoadedImpl;
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
        }
        private void ViewUnLoadedImpl(object? sender, EventArgs e)
        {
            ResetData();
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
        private IOrderPayPointAdjustView? _view;
        private readonly IOrderPayPointStampService orderPayPointStampService;
        public override bool SetIView(IView view)
        {
            _view = view as IOrderPayPointAdjustView;
            return base.SetIView(view);
        }
        public string MEMBER_NAME
        {
            get { return _membername; }
            set { _membername = value; NotifyOfPropertyChange(() => MEMBER_NAME); }
        }

        /// <summary>
        /// 받은금액
        /// </summary>
        public decimal AdjAmt
        {
            get => _adjAmt; set
            {
                _adjAmt = value;
                NotifyOfPropertyChange(nameof(AdjAmt));
            }
        }

        /// <summary>
        /// 거스름돈
        /// </summary>
        /// 
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

                    orderPayMainViewModel.ActiveForm("ActiveItemR", "OrderPayMemberSearchViewModel", CARD_NO, TEL_NO, MEMBER_NAME, false);
                    if (PTS_HISTORY != null) { PTS_HISTORY.Clear(); }
                    break;

                case "btnUse":
                    if (MemberInfo == null)
                    {
                        orderPayMainViewModel.StatusMessage = "회원코드를 선택해주세요";
                        return;
                    }
                    ExecuteAdjust();
                    break;

                default:
                    break;
            }
        }

        public override void SetData(object data)
        {
            orderPayMainViewModel = (IOrderPayMainViewModel)Parent;
        }

        private async void ExecuteAdjust()
        {
            string errorMessage = string.Empty;
            var _apiRequest = new ApiRequest(DataLocals.AppConfig.PosComm.SvrURLServer,
                   DataLocals.AppConfig.PosInfo.StoreNo,
                   DataLocals.AppConfig.PosInfo.PosNo,
                   DataLocals.TokenInfo.LICENSE_ID,
                   DataLocals.TokenInfo.LICENSE_KEY);

            var pairs = new
            {
                posNo       = DataLocals.AppConfig.PosInfo.PosNo,
                salesDt     = DataLocals.PosStatus.SALE_DATE,
                mbrCode     = MemberInfo.mbrCode,
                pointFg     = pointFlag ,
                adJustFg    = adJustFlag,
                adJustPoint = AdjAmt
            };
            var check = DataLocals.TokenInfo.TOKEN;
            var w = await _apiRequest.Request("client/inquiry/point/adjust").PostBodyAsync(pairs);
            if (w.status == ResultCode.Success)
            {
                DialogHelper.MessageBox("포인트/스탬프 조정 처리가 완료 되었습니다", GMessageBoxButton.OK, MessageBoxImage.Information);
                // Bonus function: refresh member info if adjustment successfully took place.
                var resultX = await orderPayPointStampService.GetMemberDetail(DataLocals.PosStatus.POS_NO, DataLocals.PosStatus.SALE_DATE, MemberInfo.mbrCode);
                if (string.IsNullOrEmpty(resultX.Item1))
                {
                    this.MemberInfo = resultX.Item2;
                }
                AdjAmt = 0;
            }
            else
            {
                errorMessage = w.results.ToString();
                DialogHelper.MessageBox(errorMessage, GMessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Fetch point history from API
        /// </summary>

        public Task HandleAsync(MemberInfoSinglePass message, CancellationToken cancellationToken)
        {
            this.MemberInfo = message.memberInfo;
            return Task.CompletedTask;
        }

        public async Task<(string, MEMBER_CLASH)> GetMemberDetail(string posNo, string saleDate, string memberCode)
        {
            MEMBER_CLASH memberInfo = null;
            string errorMessage = string.Empty;
            var _apiRequest = new ApiRequest(DataLocals.AppConfig.PosComm.SvrURLServer,
                   DataLocals.AppConfig.PosInfo.StoreNo,
                   DataLocals.AppConfig.PosInfo.PosNo,
                   DataLocals.TokenInfo.LICENSE_ID,
                   DataLocals.TokenInfo.LICENSE_KEY);

            var pairs = new
            {
                posNo = posNo,
                salesDt = saleDate,
                mbrCode = memberCode
            };
            var check = DataLocals.TokenInfo.TOKEN;
            var w = await _apiRequest.Request("client/inquiry/member/detail").GetDatasBodyAsync(null, pairs);
            if (w.status == ResultCode.Success)
            {
                try
                {
                    var members = JsonHelper.JsonToModel<MEMBER_CLASH>(Convert.ToString(w.results));
                    if (members.result == ResultCode.Ok)
                    {
                        memberInfo = members.model;
                    }
                }
                catch (Exception ex)
                {
                    errorMessage = "API 오류. 관리자에게 문의하세요!" + Environment.NewLine;
                    errorMessage += ex.ToFormattedString();
                }
            }
            else
            {
                errorMessage = w.results.ToString();
            }

            return await Task.FromResult((errorMessage, memberInfo));
        }
    }
}

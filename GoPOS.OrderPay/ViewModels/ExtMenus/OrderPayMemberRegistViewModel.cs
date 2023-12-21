using Caliburn.Micro;
using Dapper;
using GoPOS.Common.Interface.Model;
using GoPOS.Common.Interface.View;
using GoPOS.Common.ViewModels;
using GoPOS.Helpers;
using GoPOS.Helpers.CommandHelper;
using GoPOS.Models;
using GoPOS.Models.Common;
using GoPOS.Models.Custom.API;
using GoPOS.Service.Common;
using GoPOS.Services;
using System.Net.Http;
using GoPOS.Views;
using GoShared.Contract;
using GoShared.Helpers;
using Microsoft.IdentityModel.Tokens;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using static AutoMapper.Internal.ExpressionFactory;
using static Dapper.SqlMapper;
using GoPosVanAPI.Api;
using GoPosVanAPI.Msg;
using GoPosVanAPI.Van;
using System.Data.Entity.Core.Metadata.Edm;
using System.Xml.Linq;
using System.Reflection;
using static System.Runtime.CompilerServices.RuntimeHelpers;
using GoPOS.Service;
using GoPOS.OrderPay.ViewModels.Controls;
using GoPOS.Payment.Services;

/*
 주문 > 확장메뉴 > 회원등록

 */

namespace GoPOS.ViewModels
{

    public class OrderPayMemberRegistViewModel : OrderPayChildViewModel, IHandle<GradePass>
    {
        private readonly IOrderPayPointStampService orderPayPointStampService;
        private IOrderPayMemberRegistView _view = null;
        IOrderPayMainViewModel orderpaymain;

        public OrderPayMemberRegistViewModel(IWindowManager windowManager, IEventAggregator eventAggregator, IOrderPayPointStampService orderPayPointStampService)
            : base(windowManager, eventAggregator)
        {
            this.orderPayPointStampService = orderPayPointStampService;
            this.ViewLoaded += OrderPayMemberRegistViewModel_Viewload;
            this.ViewInitialized += OrderPayMemberRegistViewModel_ViewInitialized;
        }

        public override bool SetIView(IView view)
        {
            _view = view as IOrderPayMemberRegistView;
            return base.SetIView(view);
        }
        private void OrderPayMemberRegistViewModel_ViewInitialized(object? sender, EventArgs e)
        {
            orderpaymain = (IOrderPayMainViewModel)Parent;
        }
        private void OrderPayMemberRegistViewModel_Viewload(object? sender, EventArgs e)
        {
            RetrieveMemberGrades();
        }


        private async void RetrieveMemberGrades()
        {
            string errorMessage = string.Empty;
            var results = await orderPayPointStampService.GetMEMBER_GRADEs(); ;
            DataLocals.MemberGrades = results.Item1;

            if (DataLocals.MemberGrades != null && DataLocals.MemberGrades.Count > 0)
            {
                this.MemberGrade = DataLocals.MemberGrades[0];
            }
            else
            {
                this.MemberGrade = new()
                {
                    grdCode = "G000007",
                    grdNm = "기본등급",
                    grdSeq = "1"
                };
            }
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

        private string _DoBType = "";
        public string DOB_TYPE
        {
            get
            {
                if ((bool)_view.iLuna.IsChecked)
                {
                    return "N";
                }
                else if ((bool)_view.iSolar.IsChecked)
                {
                    return "Y";
                }
                else
                {
                    return "";
                }
            }
            set
            {
                _DoBType = value;
                NotifyOfPropertyChange(() => DOB_TYPE);
            }
        }
        private string _gender = "";
        public string GENDER
        {
            get
            {
                if ((bool)_view.iCheck1.IsChecked)
                {
                    return "M";
                }
                else if ((bool)_view.iCheck2.IsChecked)
                {
                    return "F";
                }
                else
                {
                    return "";
                }
            }
            set
            {
                _gender = value;
                NotifyOfPropertyChange(() => GENDER);
            }
        }

        private string _cardno = "";
        public string CARD_NO
        {
            get { return _cardno; }
            set { _cardno = value; NotifyOfPropertyChange(() => CARD_NO); }
        }
        private string _membername = "";
        public string MEMBER_NAME
        {
            get { return _membername; }
            set { _membername = value; NotifyOfPropertyChange(() => MEMBER_NAME); }
        }
        private string _dob = "";
        public string DOB
        {
            get { return _dob; }
            set { _dob = value; NotifyOfPropertyChange(() => DOB); }
        }

        private MEMBER_GRADE _MemberGrade = null;
        public MEMBER_GRADE MemberGrade
        {
            get { return _MemberGrade; }
            set { _MemberGrade = value; NotifyOfPropertyChange(() => MemberGrade); }
        }
        public ICommand ButtonCommand => new RelayCommand<Button>(ButtonCommandCenter);
        public void ButtonCommandCenter(Button btn)
        {
            System.Windows.Point point = _view.ButtonPost;
            switch (btn.Tag)
            {
                case "Register":
                    if (TEL_NO.IsNullOrEmpty())
                    {
                        orderpaymain.StatusMessage = "휴대폰 번호를 입력하세요!";
                        return;
                    }
                    RequestMembership(TEL_NO, CARD_NO, MEMBER_NAME, DOB, DOB_TYPE, GENDER, MemberGrade?.grdCode);
                    break;
                case "SetRate":
                    DialogHelper.ShowDialogWithCoords(typeof(PopupGradeViewModel), 400, 250, point.X - 350, point.Y + 40);
                    break;
                case "ToSignPad":
                    //      DialogHelper.MessageBox("ohoho", GMessageBoxButton.OK, MessageBoxImage.Information);
                    VanAPI vanAPI = new VanAPI();
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
                            this.TEL_NO = result.PIN;
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
                    break;

                default: break;
            }
        }


        private async void RequestMembership(string Telno, string Cardno, string name, string dob, string dobType, string gender, string GrCode)
        {
            string message = "";
            var _apiRequest = new ApiRequest(DataLocals.AppConfig.PosComm.SvrURLServer,
                   DataLocals.AppConfig.PosInfo.StoreNo,
                   DataLocals.AppConfig.PosInfo.PosNo,
                   DataLocals.TokenInfo.LICENSE_ID,
                   DataLocals.TokenInfo.LICENSE_KEY);

            var pairs = new
            {
                grdCode = GrCode,
                posNo = DataLocals.AppConfig.PosInfo.PosNo,
                salesDt = DataLocals.PosStatus.SALE_DATE,
                mbrCelno = Telno,
                mbrCardno = Cardno,
                mbrNm = name,
                birthDe = dob,
                birthSeCode = dobType,
                gender = gender
            };

            var w = await _apiRequest.Request("client/inquiry/member").PostBodyAsync(pairs);
            if (w.status == "200")
            {
                message = "회원 등록이 정상적으로 되었습니다";
                var ret = DialogHelper.MessageBox(message, GMessageBoxButton.OK, MessageBoxImage.Information, new string[] { "확인" });
                if (ret == MessageBoxResult.OK)
                {
                    this.DeactivateAsync(true);
                }
            }
            else
            {
                message = "서버에 요청한 자료를 정상 수신 받지 못하였습니다.\n" + w.results.ToString();
                DialogHelper.MessageBox(message, GMessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        public Task HandleAsync(GradePass message, CancellationToken cancellationToken)
        {
            MemberGrade = message.grade;
            return Task.CompletedTask;
        }
    }
}
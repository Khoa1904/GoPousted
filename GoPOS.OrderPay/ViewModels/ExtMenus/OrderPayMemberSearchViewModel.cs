using Caliburn.Micro;
using Dapper;
using FontAwesome6.Fonts;
using GoPOS.Common.Interface.Model;
using GoPOS.Helpers;
using GoPOS.Helpers.CommandHelper;
using GoPOS.Models;
using GoPOS.Models.Common;
using GoPOS.OrderPay.Models;
using GoPOS.OrderPay.ViewModels.Controls;
using GoPOS.Payment.Services;
using GoPOS.Service;
using GoPOS.Service.Common;
using GoPOS.Services;

using GoPOS.Views;
using GoPosVanAPI.Api;
using GoPosVanAPI.Msg;
using GoPosVanAPI.Van;
using GoShared.Helpers;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Diagnostics.Metrics;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using static AutoMapper.Internal.ExpressionFactory;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ToolTip;


/*
 주문 > 확장메뉴 > 회원검색

 */
/// <summary>
/// 화면명 : 회원검색 - 245 확장
/// 작성자 : 김형석
/// </summary>
namespace GoPOS.ViewModels
{

    public class OrderPayMemberSearchViewModel : OrderPayChildViewModel, IOrderPayMemberSearchViewModel
    {
        private bool ApplyMemberInfoGlobal = true;

        public OrderPayMemberSearchViewModel(IWindowManager windowManager, IEventAggregator eventAggregator,
            IOrderPayPointStampService orderPayPointStampService) : base(windowManager, eventAggregator)
        {
            this.ViewInitialized += OrderPayMemberSearchViewModel_ViewInitialized;
            this.ViewLoaded += ViewLoadedImpl;
            this.ViewUnloaded += ViewUnloadedImpl;
            this.orderPayPointStampService = orderPayPointStampService;
        }

        private void OrderPayMemberSearchViewModel_ViewInitialized(object? sender, EventArgs e)
        {
            orderPayMainViewModel = (IOrderPayMainViewModel)this.Parent;
        }

        private ObservableCollection<MEMBER_CLASH> _foundMembers;
        private MEMBER_CLASH _selectedMember;
        private IOrderPayMainViewModel orderPayMainViewModel;

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
        private string _txtMemberCard = "";
        public string MEMBER_CARD
        {
            get { return _txtMemberCard; }
            set
            {
                if (_txtMemberCard != value)
                {
                    _txtMemberCard = value;
                    NotifyOfPropertyChange(() => MEMBER_CARD);
                }
            }
        }

        private string _txtMemberName = "";
        private readonly IOrderPayPointStampService orderPayPointStampService;

        public string MEMBER_NM
        {
            get { return _txtMemberName; }
            set
            {
                if (_txtMemberName != value)
                {
                    _txtMemberName = value;
                    NotifyOfPropertyChange(() => MEMBER_NM);
                }
            }
        }
        private void ViewLoadedImpl(object? sender, EventArgs e)
        {
        }

        private void ViewUnloadedImpl(object? sender, EventArgs e)
        {
            MEMBER_CARD = "";
            TEL_NO = "";
            MEMBER_NM = "";
            FoundMembers = null;
            SelectedMember = null;
        }

        public override void SetData(object data)
        {
            object[] pams = (object[])data;
            MEMBER_CARD = pams.Length > 0 ? (string)pams[0] : string.Empty;
            TEL_NO = pams.Length > 1 ? (string)pams[1] : string.Empty;
            MEMBER_NM = pams.Length > 2 ? (string)pams[2] : string.Empty;
            ApplyMemberInfoGlobal = pams.Length > 3 ? (bool)pams[3] : true;

            if (!string.IsNullOrEmpty(TEL_NO) || !string.IsNullOrEmpty(MEMBER_CARD) || !string.IsNullOrEmpty(MEMBER_NM))
            {
                SearchMember(TEL_NO, MEMBER_CARD, MEMBER_NM);
            }
        }

        public ICommand ButtonCommand => new RelayCommand<Button>(ButtonCommandCenter);
        private async void ButtonCommandCenter(Button btn)
        {
            switch (btn.Tag)
            {
                case "btnSearchMember":
                    if (string.IsNullOrEmpty(TEL_NO) && string.IsNullOrEmpty(MEMBER_CARD) && string.IsNullOrEmpty(MEMBER_NM)) { IoC.Get<OrderPayMainViewModel>().StatusMessage = "검색 조건을 입력하세요"; return; }
                    SearchMember(TEL_NO, MEMBER_CARD, MEMBER_NM);
                    break;

                case "btnReqInput":
                    VanAPI vanAPI = new VanAPI();
                    try
                    {
                        VanRequestMsg msg = new VanRequestMsg()
                        {
                            VAN_CODE = DataLocals.POSVanConfig.VAN_CODE,  // Smartro 밴코드
                            REQ_SIGN_PAD_CODE = SystemHelper.ReqSignPadCode,
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

                case "btnSelMember":                   
                    if (SelectedMember == null)
                    {
                        orderPayMainViewModel.StatusMessage = "항목을 선택해주세요.";
                        return;
                    }

                    if (ApplyMemberInfoGlobal)
                    {
                        if (orderPayMainViewModel.payPpCards.Count > 0 ||
                        orderPayMainViewModel.payPoints.Count > 0)
                        {
                            //회원적립여부구분에 따라서 메세지변경
                            var nTypeNm = "";
                            if (DataLocals.AppConfig.PosOption.PointStampFlag == "0")
                            {
                                nTypeNm = "포인트/선결제";
                            }
                            else
                            {
                                nTypeNm = "스탬프/선결제";
                            }

                            DialogHelper.MessageBox(nTypeNm + " 사용 내역이 있어 회원정보를\r\n초기화 할 수 없습니다.", GMessageBoxButton.OK, System.Windows.MessageBoxImage.Warning);
                            return;
                        }
                    }

                    var mres = await orderPayPointStampService.GetMemberDetail(DataLocals.PosStatus.POS_NO, DataLocals.PosStatus.SALE_DATE, SelectedMember.mbrCode);
                    if (string.IsNullOrEmpty(mres.Item1))
                    {
                        await _eventAggregator.PublishOnUIThreadAsync(ApplyMemberInfoGlobal ?
                            new MemberInfoPass()
                            {
                                memberInfo = mres.Item2
                            } :
                            new MemberInfoSinglePass() // send to point adjust screen only
                            {
                                memberInfo = mres.Item2
                            });
                        this.DeactivateClose(true);
                    }
                    else
                    {
                        DialogHelper.MessageBox(mres.Item1, GMessageBoxButton.OK, MessageBoxImage.Warning);
                    }

                    break;
                default: break;
            }
        }
        public ObservableCollection<MEMBER_CLASH>? FoundMembers
        {
            get => _foundMembers;
            set
            {
                _foundMembers = value;
                NotifyOfPropertyChange(() => FoundMembers);
                SelectedMember = FoundMembers != null && FoundMembers.Count > 0 ? FoundMembers[0] : null;
            }
        }

        public MEMBER_CLASH? SelectedMember
        {
            get => _selectedMember;
            set
            {
                _selectedMember = value;
                NotifyOfPropertyChange(() => SelectedMember);
            }
        }

        public async void SearchMember(string telNo, string cardNo, string mbName)
        {
            var fres = await orderPayPointStampService.SearchMembers(DataLocals.PosStatus.POS_NO, DataLocals.PosStatus.SALE_DATE, telNo, cardNo, mbName);

            if (string.IsNullOrEmpty(fres.Item1))
            {
                FoundMembers = new ObservableCollection<MEMBER_CLASH>(fres.Item2);
            }
            else
            {
                DialogHelper.MessageBox(fres.Item1, GMessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}
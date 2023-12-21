using Caliburn.Micro;
using GoPOS.Common.Interface.Model;
using GoPOS.Common.ViewModels;
using GoPOS.Helpers.CommandHelper;
using GoPOS.Helpers;
using GoPOS.Models.Common;
using GoPOS.Models;
using GoPOS.Service.Common;
using GoPOS.Service;
using GoShared.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows;
using GoPosVanAPI.Api;
using GoPosVanAPI.Van;
using GoPosVanAPI.Msg;
using GoPOS.Services;
using Flurl.Util;
using System.Xml.Linq;
using GoPOS.Payment.Services;

namespace GoPOS.ViewModels
{
    public class PopupMemberSearchViewModel : BaseItemViewModel, IDialogViewModel
    {
        public PopupMemberSearchViewModel(IWindowManager windowManager, IEventAggregator eventAggregator,
            IOrderPayPointStampService orderPayPointStampService) : base(windowManager, eventAggregator)
        {
            this.ViewLoaded += ViewLoadedImpl;
            this.ViewUnloaded += ViewUnloadedImpl;
            this.orderPayPointStampService = orderPayPointStampService;
        }

        public Dictionary<string, object> DialogResult { get; set; }


        private readonly IOrderPayService orderPayService;
        private readonly IOrderPayPointStampService orderPayPointStampService;
        private ObservableCollection<MEMBER_CLASH> _foundMembers;
        private MEMBER_CLASH _selectedMember;

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
            FoundMembers = null;
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
            //base.SetData(data);
            MEMBER_CARD = pams.Length > 0 ? (string)pams[0] : string.Empty;
            TEL_NO = pams.Length > 1 ? (string)pams[1] : string.Empty;
            MEMBER_NM = pams.Length > 2 ? (string)pams[2] : string.Empty;
            SearchMember(TEL_NO, MEMBER_CARD, MEMBER_NM);
            DialogResult = new Dictionary<string, object>();
            DialogResult.Add("RETURN_DATA", null);
        }

        public ICommand ButtonCommand => new RelayCommand<Button>(ButtonCommandCenter);
        private async void ButtonCommandCenter(Button btn)
        {
            switch (btn.Tag)
            {
                case "btnSearchMember":
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
                        DialogHelper.MessageBox("항목을 선택해주세요.", GMessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }

                    var mres = await orderPayPointStampService.GetMemberDetail(DataLocals.PosStatus.POS_NO, DataLocals.PosStatus.SALE_DATE, 
                        SelectedMember.mbrCode);
                    if (string.IsNullOrEmpty(mres.Item1))
                    {
                        SelectedMember = mres.Item2;
                        DialogResult = new Dictionary<string, object>();
                        DialogResult.Add("RETURN_DATA", SelectedMember);
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
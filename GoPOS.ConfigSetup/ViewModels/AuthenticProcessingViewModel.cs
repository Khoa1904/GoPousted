using Caliburn.Micro;
using GoPOS.Common.ViewModels;
using GoPOS.Helpers.CommandHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using GoPOS.Common.Interface.Model;
using GoPOS.Common.Interface.View;
using GoPOS.ConfigSetup.Interface.View;
using GoPOS.Models.Custom.API;
using GoPOS.Models;
using GoPOS.Models.Common;
using GoPOS.Service.Common;
using GoPOS.Service.Service;
using GoPOS.Views;
using GoShared.Events;
using GoPOS.Models.Config;
using GoPOS.Helpers;
using GoPOS.Models.Custom.SellingStatus;
using Google.Protobuf.WellKnownTypes;
using Newtonsoft.Json.Linq;
using Microsoft.IdentityModel.Tokens;
using GoPOS.Service.Service.MST;
using NLog.Layouts;
using GoShared.Helpers;
using System.Dynamic;
using GoPOS.Common.ViewModels.Controls;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace GoPOS.ViewModels
{
    public class AuthenticProcessingViewModel : BaseItemViewModel, IDialogViewModel
    {
        private int _Index;
        private IAuthenticProcessingService _service;
        private readonly IWebInquiryService inquiryAPIService;

        private IAuthenticProcessingView _view;
        List<ListStoreModel> MainList = new List<ListStoreModel>();

        public AuthenRequestHeader headerData { get; set; }


        public ResponseModel dataModel;

        public ResponseModel DataModel
        {
            get => dataModel;
            set
            {
                dataModel = value;
                List<ListStoreModel> data = new List<ListStoreModel>();
                if (value != null)
                {
                    if (value.results != null && value.results.stores.Any())
                    {
                        int index = 0;
                        foreach (var item in value.results.stores)
                        {
                            index++;
                            var x = new ListStoreModel();
                            x.NO = index;
                            x.PosNo = item.posNo;
                            x.FchqCode = item.fchqCode;
                            x.LocalPosAt = item.localPosAt;
                            x.StoreCode = item.storeCode;
                            x.StoreNm = item.storeNm;
                            x.posGb = item.posGb == "0" ? "메인" : "서브";
                            data.Add(x);
                        }

                    }
                }
                MainList = data;
                IoC.Get<AuthenticProcessingView>().StoreListview.ItemsSource = MainList;
            }
        }


        private ListStoreModel _dataModel;

        public ListStoreModel SelectedItem
        {
            get => _dataModel; set
            {
                _dataModel = value;
                NotifyOfPropertyChange(() => SelectedItem);
                OnSelectedItemChanged(value);
            }
        }

        public int Index
        {
            get { return _Index; }
            set { _Index = value; NotifyOfPropertyChange(() => Index); }
        }

        public AuthenticProcessingViewModel(IWindowManager windowManager, IEventAggregator eventAggregator, IAuthenticProcessingService service, IWebInquiryService inquiryAPIService) : base(windowManager, eventAggregator)
        {
            _service = service;
            this.inquiryAPIService = inquiryAPIService;
        }

        private void OnSelectedItemChanged(ListStoreModel value)
        {
            SelectedItem.StoreCode = value.StoreCode;
            SelectedItem.StoreNm = value.StoreNm;
            SelectedItem.posNo = value.PosNo;
            SelectedItem.MainPosId = "";
            _view.PosIP.Text = "localhost";

        }



        public ICommand ButtonCommand => new RelayCommand<Button>(ButtonCommandCenter);
        private async void ButtonCommandCenter(Button button)
        {
            var viewModel = IoC.Get<MessageViewModel>(); // Replace with your ViewModel
            //  var loginModel = IoC.Get<AuthenticLoginViewModel>();
            try
            {
                if (button.Tag == null || button == null)
                    return;


                switch (button.Tag)
                {
                    case "Confirm":
                        if (SelectedItem == null || string.IsNullOrEmpty(SelectedItem.StoreCode) || string.IsNullOrEmpty(SelectedItem.PosNo))
                        {
                            DialogHelper.MessageBox("매장을 하나 선택하여 주십시오.", GMessageBoxButton.OK, System.Windows.MessageBoxImage.Warning);
                            return;
                        }

                        viewModel.MsgSet("처리중입니다. 잠시만 기다려 주세요...", GMessageBoxButton.OK);

                        dynamic settings = new ExpandoObject();
                        settings.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                        settings.ResizeMode = ResizeMode.NoResize;
                        settings.MinWidth = 402;
                        settings.MinHeight = 212;
                        settings.WindowStyle = WindowStyle.None;

                        IWindowManager manager = new WindowManager();

                        // Show the window asynchronously
                        manager.ShowWindowAsync(viewModel, null, settings);

                        bool returnSuccess = false;
                        DataLocals.AppConfig.PosInfo.StoreNo = SelectedItem.StoreCode;
                        DataLocals.AppConfig.PosInfo.PosNo = SelectedItem.PosNo;
                        DataLocals.AppConfig.PosInfo.HD_SHOP_CODE = SelectedItem.fchqCode;
                        DataLocals.AppConfig.PosComm.MainPOSIP = (_view.PosIP.Text.Contains("localhost") || _view.PosIP.Text == "") ? "127.0.0.1" : _view.PosIP.Text;


                        var data = await _service.AuthenticateValidateLicense(SelectedItem, headerData);
                        if (data != null)
                        {
                            if (string.IsNullOrEmpty(data.results.LicenseId) || string.IsNullOrEmpty(data.results.LicenseKey))
                            {
                                LogHelper.Logger.Trace("인증키 수신 오류.");
                                break;
                            }

                            LogHelper.Logger.Trace("인증성공: " + data.results.LicenseId + "/" + data.results.LicenseKey);

                            DataLocals.TokenInfo = new POS_KEY_MANG();
                            DataLocals.TokenInfo.LICENSE_ID = data.results.LicenseId;
                            DataLocals.TokenInfo.LICENSE_KEY = data.results.LicenseKey;
                            DataLocals.TokenInfo.TOKEN = headerData.Token;
                            DataLocals.TokenInfo.VALIDDT = DateTime.Now.AddDays(-7).ToString("yyyyMMddHHmms");
                            DataLocals.AppConfig.PosComm.LicenseId = data.results.LicenseId;
                            DataLocals.AppConfig.PosComm.LicenseKey = data.results.LicenseKey;

                            var saveDataToDBb = await inquiryAPIService.InqAccessToken();
                            viewModel.TryCloseAsync();
                            if (saveDataToDBb.Item2.ResultType != EResultType.SUCCESS)
                            {
                                LogHelper.Logger.Trace("토큰정보 수신 오류.");
                                break;
                            }

                            LogHelper.Logger.Trace("토큰수신: " + saveDataToDBb.Item1);
                            DataLocals.AppConfig.Save();

                            LogHelper.Logger.Trace("테이블정리");
                            await inquiryAPIService.InqTruncateTableAsync();

                            returnSuccess = true;
                        }
                        else
                        {
                            viewModel.TryCloseAsync();
                            DialogHelper.MessageBox("서버문제가 발생했습니다");
                            LogHelper.Logger.Trace("인증 오류");
                        }

                        DialogResult = new Dictionary<string, object>
                                 {
                                     { "RETURN_DATA", returnSuccess }
                                 };
                        this.TryCloseAsync(true);

                        break;
                    case "Close":
                        DialogResult = new Dictionary<string, object>
                                 {
                                     { "RETURN_DATA", false}
                                 };
                        this.TryCloseAsync(true);
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                viewModel.TryCloseAsync();
                DialogHelper.MessageBox(ex.Message, GMessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
                LogHelper.Logger.Error(ex.ToFormattedString());
            }

        }

        public override bool SetIView(IView view)
        {
            _view = (IAuthenticProcessingView)view;
            return base.SetIView(view);
        }

        public Dictionary<string, object> DialogResult
        {
            //get
            //{
            //    var eventData = new Tuple<string, ResponseModel>("Close", null);
            //    ShowActiveItem?.Invoke(this, new EventArgs<Tuple<string, ResponseModel>>(eventData));
            //    return null;
            //}
            get; set;
        }
    }
}

using AutoMapper;
using Caliburn.Micro;
using GoPOS.Common.Interface.Model;
using GoPOS.Common.Interface.View;
using GoPOS.Common.PrinterLib;
using GoPOS.Common.Service;
using GoPOS.Common.ViewModels;
using GoPOS.Helpers;
using GoPOS.Helpers.CommandHelper;
using GoPOS.Models;
using GoPOS.Models.Common;
using GoPOS.Models.Config;
using GoPOS.Sales.Interface.View;
using GoPOS.Sales.Services;
using GoPOS.Service;
using GoPOS.Service.Common;
using GoShared.Helpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using GoPOS.Service.Service.POS;
using GoPOS.Models.Custom.API;
using GoPOS.Payment.Services;

namespace GoPOS.ViewModels
{
    public class SalesPayPrepaymentViewModel : BaseItemViewModel, IDialogViewModel, IHandle<CalendarEventArgs>
    {
        private MEMBER_CLASH _MemberInfo = null;
        private List<PREPAYMENT_FLOWS> _ChargeList = null;
        private List<PREPAYMENT_FLOWS> _SpendList = null;
        private string _mbrCard = string.Empty;
        private string _mbrTEL = string.Empty;
        private string _mbrName = string.Empty;
        private ISalesPayPrepaymentView _view = null;

        private DateTime _datefrom { get; set; } = DateTime.ParseExact(

                    DataLocals.PosStatus != null ? DataLocals.PosStatus.SALE_DATE : DateTime.Now.ToString("yyyyMMdd"), "yyyyMMdd",
                    Thread.CurrentThread.CurrentUICulture);

        private DateTime _dateto { get; set; } = DateTime.ParseExact(
            DataLocals.PosStatus != null ? DataLocals.PosStatus.SALE_DATE : DateTime.Now.ToString("yyyyMMdd"), "yyyyMMdd",
            Thread.CurrentThread.CurrentUICulture);

        public DateTime DateFrom
        {
            get => _datefrom;
            set
            {
                _datefrom = value;
                NotifyOfPropertyChange(() => DateFrom);
            }
        }

        private decimal pAY_AMT;


        public decimal PAY_AMT
        {
            get => pAY_AMT; set
            {
                pAY_AMT = value;
                NotifyOfPropertyChange(nameof(PAY_AMT));
            }
        }

        public DateTime DateTo
        {
            get => _dateto;
            set
            {
                _dateto = value;
                NotifyOfPropertyChange(() => DateTo);
            }
        }

        public MEMBER_CLASH MemberInfo
        {
            get => _MemberInfo;
            set
            {
                _MemberInfo = value;
                NotifyOfPropertyChange(() => MemberInfo);
            }
        }

        public PREPAYMENT_FLOWS _selectedItemMainList;

        public PREPAYMENT_FLOWS SelectedItemMainList
        {
            set
            {
                _selectedItemMainList = value;
                NotifyOfPropertyChange(nameof(SelectedItemMainList));
            }
            get { return _selectedItemMainList; }
        }

        public List<PREPAYMENT_FLOWS> ChargeList
        {
            get => _ChargeList;
            set
            {
                _ChargeList = value;
                NotifyOfPropertyChange(() => ChargeList);
            }
        }

        public List<PREPAYMENT_FLOWS> SpendList
        {
            get => _SpendList;
            set
            {
                _SpendList = value;
                NotifyOfPropertyChange(() => SpendList);
            }
        }

        public string MbrCard
        {
            get => _mbrCard;
            set
            {
                _mbrCard = value;
                NotifyOfPropertyChange(() => MbrCard);
            }
        }

        public string MbrTEL
        {
            get => _mbrTEL;
            set
            {
                _mbrTEL = value;
                NotifyOfPropertyChange(() => MbrTEL);
            }
        }

        public string MbrName
        {
            get => _mbrName;
            set
            {
                _mbrName = value;
                NotifyOfPropertyChange(() => MbrName);
            }
        }


        private class APIinfoFetcher // use to fetch relevant data from API
        {
            public List<PREPAYMENT_FLOWS> chargeList { get; set; }
            public List<PREPAYMENT_FLOWS> useList { get; set; }
        }

        private IPOSPrintService printService;
        private readonly IOrderPayPointStampService orderPayPointStampService;

        public SalesPayPrepaymentViewModel(IWindowManager windowManager, IEventAggregator eventAggregator,
            ISalesMngService salesMngService, IPOSPrintService printService, IOrderPayPointStampService orderPayPointStampService) :
            base(windowManager, eventAggregator)
        {
            this.salesMngService = salesMngService;
            this.printService = printService;
            this.orderPayPointStampService = orderPayPointStampService;
            this.ViewLoaded += SalesPayPrepaymentViewModel_viewload;
        }

        private void SalesPayPrepaymentViewModel_viewload(object sender, EventArgs arrrgg)
        {
            MbrCard = string.Empty;
            MbrName = string.Empty;
            MbrTEL = string.Empty;
            ChargeList = null;
            SpendList = null;
            MemberInfo = null;
            ChargeList = null;
        }

        public Dictionary<string, object> DialogResult { get; set; }

        public override bool SetIView(IView view)
        {
            _view = (ISalesPayPrepaymentView)view;
            return base.SetIView(view);
        }

        public ICommand ButtonCommand => new RelayCommand<Button>(ButtonCommandCenter);

        private async void ButtonCommandCenter(Button btn)
        {
            System.Windows.Point point = _view.ButtonPosition;
            if (btn == null)
            {
                return;
            }

            if (string.IsNullOrEmpty(btn.Tag.ToString()))
            {
                return;
            }

            switch (btn.Tag.ToString())
            {
                case "SeachMember":
                    if(string.IsNullOrEmpty(MbrTEL) && string.IsNullOrEmpty(MbrName))
                    {
                        DialogHelper.MessageBox("검색조건을 입력하세요", GMessageBoxButton.OK,
                        System.Windows.MessageBoxImage.Exclamation);
                        break;
                    }

                    var sres = DialogHelper.ShowDialog(typeof(PopupMemberSearchViewModel), 613, 522, MbrCard, MbrTEL, MbrName);
                    if (sres != null && sres.ContainsKey("RETURN_DATA"))
                    {
                        MemberInfo = sres["RETURN_DATA"] as MEMBER_CLASH;

                        //cloase 창으로 선택을 안했을 경우
                        if (MemberInfo == null) {
                            //초기화
                            MbrCard = string.Empty;
                            MbrName = string.Empty;
                            MbrTEL = string.Empty;
                            return;
                        }
                        ChargeHistory();
                    }

                    MbrTEL = "";

                    break;

                case "PaymentMethod":

                    #region Charge by card

                    if (PAY_AMT == 0) //PAY_AMT == 0 
                    {
                        DialogHelper.MessageBox("충전금액을 입력하세요", GMessageBoxButton.OK,
                            System.Windows.MessageBoxImage.Exclamation);
                        break;
                    }

                    if (this.MemberInfo == null)
                    {
                        DialogHelper.MessageBox("회원정보를 먼저 조회 하십시오.", GMessageBoxButton.OK,
                            System.Windows.MessageBoxImage.Exclamation);
                        break;
                    }

                    var res = DialogHelper.ShowDialog(typeof(CreditCardPopupViewModel), 613, 522, PAY_AMT);
                    PAY_AMT = 0;
                    if (res != null && res.Keys.Count > 0 && res["RETURN_DATA"] != null)
                    {

                        // using CardPay for making TRN
                        NTRN_PRECHARGE_CARD payCard = (NTRN_PRECHARGE_CARD)res["RETURN_DATA"];
                        string errorMessage = string.Empty;
                        var dataReturn = salesMngService.SaveNonTrnPrecharge(_MemberInfo, payCard, false, out errorMessage);
                        
                        // TRN DATA
                        var orderInfor = new OrderInfo
                        {
                            ShopCode = DataLocals.AppConfig.PosInfo.StoreNo,
                            PosNo = DataLocals.AppConfig.PosInfo.PosNo,
                            SaleDate = DataLocals.PosStatus.SALE_DATE,
                            BillNo = dataReturn.Item1,
                        };

                        printService.PrintPrePaymentInput(orderInfor);
                        printService.EndPrinting();

                        DialogHelper.MessageBox(string.IsNullOrEmpty(errorMessage) ? "충전 완료 되었습니다." : errorMessage,
                            GMessageBoxButton.OK, System.Windows.MessageBoxImage.Information);

                        var memRes = await orderPayPointStampService.GetMemberDetail(DataLocals.PosStatus.POS_NO, DataLocals.PosStatus.SALE_DATE, MemberInfo.mbrCode);
                        if (!string.IsNullOrEmpty(memRes.Item1))
                        {
                            DialogHelper.MessageBox(memRes.Item1, GMessageBoxButton.OK, MessageBoxImage.Warning);
                        }
                        else
                        {
                            this.MemberInfo = memRes.Item2;
                        }

                        PAY_AMT = 0;
                    }

                    #endregion

                    // refresh list
                    ChargeHistory();

                    break;

                case "Refund":
                    if (SelectedItemMainList == null)
                        return;
                    if (SelectedItemMainList.crntAvlblAmt != SelectedItemMainList.payAmt)
                    {
                        DialogHelper.MessageBox("선결제 사용 내역이 있습니다. \n 반품이 불가 합니다", GMessageBoxButton.OK,
                            System.Windows.MessageBoxImage.Information);
                        return;
                    }

                    //var result =  salesMngService.DoRefundRePayment(SelectedItemMainList.storeCode, SelectedItemMainList.posNo, SelectedItemMainList.salesDt, SelectedItemMainList.billNo).Result;
                    NTRN_PRECHARGE_CARD ntrnPrechargeCard = salesMngService.GetOrgPrechargeCard(SelectedItemMainList.storeCode, SelectedItemMainList.posNo, SelectedItemMainList.salesDt, SelectedItemMainList.billNo);
                    
                    if(ntrnPrechargeCard == null)
                    {
                        DialogHelper.MessageBox(" 선결제 충전내역이 없어서 환불처리가 불가합니다.", GMessageBoxButton.OK,
                           System.Windows.MessageBoxImage.Information);
                        return;
                       
                    }
                    
                    var resCancel = DialogHelper.ShowDialog(typeof(CreditCardCancelPopupViewModel), 613, 522,
                        ntrnPrechargeCard);
                    if (resCancel != null && resCancel.Keys.Count > 0 && resCancel["RETURN_CANCEL_DATA"] != null)
                    {
                        NTRN_PRECHARGE_CARD payCard = (NTRN_PRECHARGE_CARD)resCancel["RETURN_CANCEL_DATA"];
                        string errorMessage = string.Empty;
                        var dataReturn=salesMngService.SaveNonTrnPrecharge(_MemberInfo, payCard, true, out errorMessage);
                        // update new BILL NO of return receipt

                        string returnBillNo = ntrnPrechargeCard.SHOP_CODE + ntrnPrechargeCard.SALE_DATE + ntrnPrechargeCard.POS_NO + ntrnPrechargeCard.SALE_NO;
                        salesMngService.UpdateOrderPayReturnTR(ntrnPrechargeCard.SHOP_CODE, ntrnPrechargeCard.POS_NO, ntrnPrechargeCard.SALE_DATE, ntrnPrechargeCard.REGI_SEQ, dataReturn.Item1,returnBillNo);

                        var orderInfor = new OrderInfo
                        {
                            ShopCode = DataLocals.AppConfig.PosInfo.StoreNo,
                            PosNo = DataLocals.AppConfig.PosInfo.PosNo,
                            SaleDate = DataLocals.PosStatus.SALE_DATE,
                            BillNo = dataReturn.Item1,
                        };

                        printService.PrintPrePaymentInput(orderInfor,true);
                        printService.EndPrinting();

                        DialogHelper.MessageBox(string.IsNullOrEmpty(errorMessage) ? "충전취소 완료되었습니다" : errorMessage,
                            GMessageBoxButton.OK, System.Windows.MessageBoxImage.Information);
                    }

                    ChargeHistory();

                    // refresh memberinfo
                    var mres = await orderPayPointStampService.GetMemberDetail(DataLocals.PosStatus.POS_NO, DataLocals.PosStatus.SALE_DATE,
                        MemberInfo.mbrCode);
                    if (string.IsNullOrEmpty(mres.Item1))
                    {
                        MemberInfo = mres.Item2;
                    }
                    else
                    {
                        DialogHelper.MessageBox(mres.Item1, GMessageBoxButton.OK, MessageBoxImage.Warning);
                    }

                    break;

                case "CalendarFrom":
                    istxtSaleDateFrom = true;
                    System.Windows.Point pointDateF = _view.ButtonPosition;
                    DialogHelper.ShowDialogWithCoords(typeof(CalendarViewModel), 450, 450, pointDateF.X - 485,
                        pointDateF.Y - 145, DateFrom);
                    break;

                case "CalendarTo":
                    istxtSaleDateFrom = false;
                    System.Windows.Point pointDateT = _view.ButtonPosition;
                    DialogHelper.ShowDialogWithCoords(typeof(CalendarViewModel), 450, 450, pointDateT.X - 485,
                        pointDateT.Y - 145, DateTo);
                    break;

                case "CheckCharge":
                    if (MemberInfo == null)
                    {
                        DialogHelper.MessageBox("회원을 선택해 주세요", GMessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }

                    if (DateFrom != null && DateTo != null)
                    {
                        ChargeHistory();
                    }

                    break;
                case "CheckSpend":
                    if (MemberInfo == null)
                    {
                        DialogHelper.MessageBox("회원을 선택해 주세요", GMessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }

                    if (DateFrom != null && DateTo != null)
                    {
                        SpendHistory();
                    }

                    break;
                default: break;
            }
        }

        private bool istxtSaleDateFrom;
        private readonly ISalesMngService salesMngService;


        private void PrintReturnBill(string returnBillNo)
        {
            string retSaleDate = returnBillNo.Substring(7, 8);
            string retPosNo = returnBillNo.Substring(15, 2);
            string retBillNo = returnBillNo.Substring(returnBillNo.Length - 4);
            printService.PrintReceipt(PrintOptions.Normal, DataLocals.AppConfig.PosInfo.StoreNo, retPosNo, retSaleDate,
                retBillNo, false, false);
            printService.EndPrinting();
        }

        public Task HandleAsync(CalendarEventArgs message, CancellationToken cancellationToken)
        {
            if (base.Views.Keys.Count() != 0)
            {
                if (message.EventType == "ExtButton")
                {
                    if (istxtSaleDateFrom)
                    {
                        DateFrom = message.Date;
                        if (DateFrom >= DateTo)
                        {
                            DateFrom = DateTo;
                        }
                    }
                    else
                    {
                        DateTo = message.Date;
                        if (DateTo <= DateFrom)
                        {
                            DateTo = DateFrom;
                        }
                    }
                }

                switch (message.EventType)
                {
                    case "btnConfirm":
                        if (istxtSaleDateFrom)
                        {
                            DateTo = DateFrom;
                        }
                        else
                        {
                            DateFrom = DateTo;
                        }

                        break;
                    case "btnCancel7":
                        if (istxtSaleDateFrom)
                        {
                            DateTo = DateFrom.AddDays(7);
                        }
                        else
                        {
                            DateFrom = DateTo.AddDays(-7);
                        }

                        break;
                    case "btnCancel15":
                        if (istxtSaleDateFrom)
                        {
                            DateTo = DateFrom.AddDays(15);
                        }
                        else
                        {
                            DateFrom = DateTo.AddDays(-15);
                        }

                        break;
                    case "btnCancel1":
                        if (istxtSaleDateFrom)
                        {
                            DateTo = DateFrom.AddMonths(1);
                        }
                        else
                        {
                            DateFrom = DateTo.AddMonths(-1);
                        }

                        break;
                    case "btnCancel3":
                        if (istxtSaleDateFrom)
                        {
                            DateTo = DateFrom.AddMonths(3);
                        }
                        else
                        {
                            DateFrom = DateTo.AddMonths(-3);
                        }

                        break;
                }
            }

            return Task.CompletedTask;
        }

        public async void ChargeHistory()
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

            var pairs_real = new // real data from current server
            {
                posNo = DataLocals.AppConfig.PosInfo.PosNo,
                salesDt = DataLocals.PosStatus.SALE_DATE,
                startDe = DateFrom.ToString("yyyyMMdd"),
                endDe = DateTo.ToString("yyyyMMdd"),
                mbrCode = MemberInfo.mbrCode,
                createdAt = MemberInfo.createdAt
            };

            var w = await _apiRequest.Request("/client/inquiry/prepaid/list").GetDatasBodyAsync(null, pairs_real);
            if (w.status == "200")
            {
                try
                {
                    string jsonData = Convert.ToString(w.results);
                    APIinfoFetcher ObjectData = JsonConvert.DeserializeObject<APIinfoFetcher>(jsonData);
                    var data = ObjectData.chargeList;
                    ChargeList = data;
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

        private async void SpendHistory()
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
            var pairs = new
            {
                posNo = DataLocals.AppConfig.PosInfo.PosNo,
                salesDt = DataLocals.PosStatus.SALE_DATE,
                startDe = DateFrom.ToString("yyyyMMdd"),
                endDe = DateTo.ToString("yyyyMMdd"),
                mbrCode = MemberInfo.mbrCode,
                createdAt = MemberInfo.createdAt
            };
            var w = await _apiRequest.Request("/client/inquiry/prepaid/list").GetDatasBodyAsync(null, pairs);
            if (w.status == "200")
            {
                try
                {
                    string jsonData = Convert.ToString(w.results);
                    APIinfoFetcher ObjectData = JsonConvert.DeserializeObject<APIinfoFetcher>(jsonData);
                    var data = ObjectData.useList;
                    SpendList = data;
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
using Caliburn.Micro;
using Dapper;
using GoPOS.Common.Interface.View;
using GoPOS.Common.PrinterLib;
using GoPOS.Common.ViewModels;
using GoPOS.Helpers;
using GoPOS.Helpers.CommandHelper;
using GoPOS.Models;
using GoPOS.Models.Common;
using GoPOS.SellingStatus.Interface;
using GoPOS.Service;
using GoPOS.Service.Service;
using GoPOS.Services;

using GoPOS.Views;
using GoShared.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using static GoPOS.Function;

/*
 매출현황 > 결제유형별 매출현황

 */

namespace GoPOS.ViewModels
{

    public class PaymntSelngSttusViewModel : BaseItemViewModel, IHandle<CalendarEventArgs>, IHandle<SelectPosEventArgs>
    {
        private readonly ISellingStatusService sellingStatusService;

        private List<SALE_BY_TYPE2> _MainList { get; set; }
        private List<SALE_BY_TYPE2> _DetailList { get; set; }
        private IEventAggregator _eventAggregator;
        private IPaymentSelngStatusView _PaymentSttusView;
        private SETT_POSACCOUNT LastSettAccount = null;
        private DateTime _txtSaleFrom;
        private DateTime _txtSaleTo;
        private readonly ISellingStatusService _sellingStatusService;
        private readonly ISettAccountService settAccountService;

        public PaymntSelngSttusViewModel(IEventAggregator eventAggregator, ISellingStatusService service, IWindowManager window)
            : base(window, eventAggregator)
        {
            this.sellingStatusService = service;
            this.ViewLoaded += ScreenLoad;
            this._eventAggregator = eventAggregator;
            this._eventAggregator.SubscribeOnPublishedThread(this);
            this._selectedItemMainList = new SALE_BY_TYPE2();
            this.FirstMenuVisibility = Visibility.Visible;
            this.SecondMenuVisibility = Visibility.Collapsed;
            Init();
        }
        private string _DetailTitle { get; set; }
        public string DetailTitle
        {
            get => _DetailTitle;
            set
            {
                _DetailTitle = value;
                NotifyOfPropertyChange(nameof(DetailTitle));
            }
        }
        public DateTime txtSaleDateFrom
        {
            get { return _txtSaleFrom; }
            set
            {
                _txtSaleFrom = value;
                NotifyOfPropertyChange(() => txtSaleDateFrom);
            }
        }
        public DateTime txtSaleDateTo
        {
            get { return _txtSaleTo; }
            set
            {
                _txtSaleTo = value;
                NotifyOfPropertyChange(() => txtSaleDateTo);
            }
        }
        private string _payCode { get; set; }
        public string PayCode
        {
            get { return _payCode; }
            set
            {
                _payCode = value;
                NotifyOfPropertyChange(() => PayCode);
            }
        }
        public List<SALE_BY_TYPE2> MainList
        {
            get { return _MainList; }
            set
            {
                _MainList = value;
                NotifyOfPropertyChange(() => MainList);
            }
        }
        public List<SALE_BY_TYPE2> DetailList
        {
            get { return _DetailList; }
            set
            {
                _DetailList = value;
                NotifyOfPropertyChange(() => DetailList);
            }
        }
        private string _pos;
        public string POS
        {
            get { return _pos; }
            set
            {
                _pos = value;
                NotifyOfPropertyChange(() => POS);
            }
        }

        private Visibility firstMenuVisibility = Visibility.Visible;
        public Visibility FirstMenuVisibility
        {
            get { return firstMenuVisibility; }
            set
            {
                firstMenuVisibility = value;
                NotifyOfPropertyChange(nameof(FirstMenuVisibility));
            }
        }

        private Visibility secondMenuVisibility = Visibility.Collapsed;
        public Visibility SecondMenuVisibility
        {
            get { return secondMenuVisibility; }
            set
            {
                secondMenuVisibility = value;
                NotifyOfPropertyChange(nameof(SecondMenuVisibility));
            }
        }
        private SALE_BY_TYPE2 _selectedItemMainList;

        public SALE_BY_TYPE2 SelectedItemMainList
        {
            get { return _selectedItemMainList; }
            set
            {
                _selectedItemMainList = value;
                NotifyOfPropertyChange(() => SelectedItemMainList);
                // 선택 이벤트 처리 메서드 호출
                OnSelectedItemChanged(value);
            }
        }
        /// <summary>
        /// Aggregate propteries
        /// </summary>
        private decimal _totQty { get; set; }
        private decimal _totAmt { get; set; }
        private decimal _totRate { get; set; }
        public decimal TotalQty
        {
            get => _totQty;
            set { _totQty = value; NotifyOfPropertyChange(() => TotalQty); }
        }
        public decimal TotalAmt
        {
            get => _totAmt;
            set { _totAmt = value; NotifyOfPropertyChange(() => TotalAmt); }
        }
        public decimal TotalRate
        {
            get => _totRate;
            set { _totRate = value; NotifyOfPropertyChange(() => TotalRate); }
        }
        public void AggregateReset()
        {
            TotalAmt = 0;
            TotalQty = 0;
            TotalRate = 0;
            SelectedItemMainList = new();
        }
        public async void Init()
        {
            txtSaleDateFrom = txtSaleDateTo = Convert.ToDateTime(SalesDateString);
            POS = "전체";
            await GetPaymntSelngSttusMainList();
        }
        private void ScreenLoad(object? sender, EventArgs e)
        {
            DetailList = null;
            this.txtSaleDateFrom = DateTime.ParseExact(DataLocals.PosStatus.SALE_DATE, "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture);
            this.txtSaleDateTo = DateTime.ParseExact(DataLocals.PosStatus.SALE_DATE, "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture);
        }

        private async Task GetPaymntSelngSttusMainList()
        {
            AggregateReset();
            try
            {
                DynamicParameters param = new DynamicParameters();
                param.Add("@SHOPCODE", DataLocals.AppConfig.PosInfo.StoreNo, DbType.String, ParameterDirection.Input, 10);
                param.Add("@DATEFROM", txtSaleDateFrom.ToString("yyyyMMdd"), DbType.String, ParameterDirection.Input, 10);
                param.Add("@DATETO", txtSaleDateTo.ToString("yyyyMMdd"), DbType.String, ParameterDirection.Input, 10);
                param.Add("@POSNO", POS == "전체" ? null : POS, DbType.String, ParameterDirection.Input, 10);
                (MainList, SpResult spResult) = await sellingStatusService.GetPaymntSelngSttusMainList(param);
                IoC.Get<PaymntSelngSttusView>().lstViewMain.ItemsSource = MainList;
                if (spResult.ResultType != EResultType.SUCCESS)
                {
                    return;
                }

                for (int i = 0; i < MainList.Count; i++)
                {
                    TotalAmt += MainList[i].Pay_Amt;
                    TotalQty += MainList[i].Qty;
                    TotalRate += MainList[i].OCC_RATE;
                }
            //    OnSelectedItemChanged(null);
            }
            catch (Exception ex)
            {
                LogHelper.Logger.Error("Main 리스트 가져오기 오류 : " + ex.Message);
            }
        }

        public async void OnSelectedItemChanged(SALE_BY_TYPE2 selectedItem)
        {
            try
            {
                if (DetailList != null) { DetailList.Clear(); }
                DetailTitle = selectedItem.PayType.ToString();
                PayCode = selectedItem.Pay_Flag;
                DynamicParameters param = new DynamicParameters();
                param.Add("@SHOPCODE", DataLocals.AppConfig.PosInfo.StoreNo, DbType.String, ParameterDirection.Input, 10);
                param.Add("@DATEFROM", txtSaleDateFrom.ToString("yyyyMMdd"), DbType.String, ParameterDirection.Input, 10);
                param.Add("@DATETO", txtSaleDateTo.ToString("yyyyMMdd"), DbType.String, ParameterDirection.Input, 10);
                param.Add("@PAYCODE", PayCode, DbType.String, ParameterDirection.Input, 10);
                param.Add("@POSNO", POS == "전체" ? null : POS, DbType.String, ParameterDirection.Input, 10);
                (DetailList, SpResult spResult) = await sellingStatusService.GetPaymntSelngSttusDetailList(param);
                IoC.Get<PaymntSelngSttusView>().lstViewSub.ItemsSource = DetailList;
                if (spResult.ResultType != EResultType.SUCCESS)
                {
                    return;
                }
            }
            catch (Exception ex)
            {
                LogHelper.Logger.Error("Detail List 가져오기 오류 : " + ex.Message);
            }
        }


        public void ButtonMenu(object sender, RoutedEventArgs e)
        {
            if (sender is not Button btn)
            {
                return;
            }
        }

        public void ButtonClose(object sender, RoutedEventArgs e)
        {
            if (sender is not Button btn)
            {
                return;
            }

            string sValue = (string)btn.Tag;

            //CBE


            //IoC.Get<OrderPayMainView>().DockAllPop.Visibility = Visibility.Hidden;
        }
        public ICommand ButtonCommand => new RelayCommand<Button>(ButtonCommandCenter);
        private void ButtonCommandCenter(Button btn)
        {
            System.Windows.Point point = _PaymentSttusView.buttonPosition;
            if (btn.Tag == null)
            {
                return;
            }
            switch (btn.Tag)
            {
                case "searchPOS":
                    IoC.Get<SelectBoxViewModel>();
                    _eventAggregator?.PublishOnUIThreadAsync(new SelectboxEvent()
                    {
                        Type = "POS"
                    });
                    point = _PaymentSttusView.buttonPosition;
                    DialogHelper.ShowDialogWithCoords(typeof(SelectBoxViewModel), 50, 350, point.X - 30, point.Y + 35, POS);
                    break;
                case "btnSaleDateF":
                    istxtSaleDateFrom = true;
                    System.Windows.Point pointDateF = _PaymentSttusView.buttonPosition;
                    //DialogHelper.ShowDialogWithCoords(typeof(CalendarViewModel), 450, 480, pointDateF.X - 450, pointDateF.Y + 30, FDate);
                    DialogHelper.ShowDialogWithCoords(typeof(CalendarViewModel), 450, 480, pointDateF.X, pointDateF.Y, txtSaleDateFrom);
                    break;
                case "btnSaleDateT":
                    istxtSaleDateFrom = false;
                    System.Windows.Point pointDateT = _PaymentSttusView.buttonPosition;
                    //DialogHelper.ShowDialogWithCoords(typeof(CalendarViewModel), 450, 480, pointDateT.X - 450, pointDateT.Y + 30, TDate);
                    DialogHelper.ShowDialogWithCoords(typeof(CalendarViewModel), 450, 480, pointDateT.X, pointDateT.Y, txtSaleDateTo);
                    break;
                case "SearchNow":
                    if (DetailList != null) { DetailList.Clear(); }
                    GetPaymntSelngSttusMainList();
                    //            IoC.Get<PaymntSelngSttusView>().list
                    break;
                default:
                    break;
            }
        }

        public void ChangeView(string viewModelName)
        {
            //ShellViewModel.menu_nm = viewModelName;

            //_eventAggregator.PublishOnUIThreadAsync(IoC.Get<ShellViewModel>().LoggedInEmployee);
        }

        public void ChangeMenu(int menuNumber)
        {
            if (menuNumber == 0)
            {
                this.FirstMenuVisibility = Visibility.Visible;
                this.SecondMenuVisibility = Visibility.Collapsed;
            }
            else if (menuNumber == 1)
            {
                this.FirstMenuVisibility = Visibility.Collapsed;
                this.SecondMenuVisibility = Visibility.Visible;
            }
        }
        private bool istxtSaleDateFrom;
        public Task HandleAsync(CalendarEventArgs message, CancellationToken cancellationToken)
        {

            if (base.Views.Keys.Count() != 0)
            {
                if (message.EventType == "ExtButton")
                {
                    if (istxtSaleDateFrom)
                        txtSaleDateFrom = message.Date;
                    else
                        txtSaleDateTo = message.Date;

                    return GetPaymntSelngSttusMainList();

                }

                switch (message.EventType)
                {
                    case "btnConfirm":
                        if (istxtSaleDateFrom)
                        {
                            txtSaleDateTo = txtSaleDateFrom;
                        }
                        else
                        {
                            txtSaleDateFrom = txtSaleDateTo;
                        }

                        return GetPaymntSelngSttusMainList();
                        break;
                    case "btnCancel7":
                        if (istxtSaleDateFrom)
                        {
                            txtSaleDateTo = txtSaleDateFrom.AddDays(7);
                        }
                        else
                        {
                            txtSaleDateFrom = txtSaleDateTo.AddDays(-7);
                        }

                        return GetPaymntSelngSttusMainList();

                        break;
                    case "btnCancel15":
                        if (istxtSaleDateFrom)
                        {
                            txtSaleDateTo = txtSaleDateFrom.AddDays(15);
                        }
                        else
                        {
                            txtSaleDateFrom = txtSaleDateTo.AddDays(-15);
                        }

                        return GetPaymntSelngSttusMainList();

                        break;
                    case "btnCancel1":
                        if (istxtSaleDateFrom)
                        {
                            txtSaleDateTo = txtSaleDateFrom.AddMonths(1);
                        }
                        else
                        {
                            txtSaleDateFrom = txtSaleDateTo.AddMonths(-1);
                        }

                        return GetPaymntSelngSttusMainList();

                        break;
                    case "btnCancel3":
                        if (istxtSaleDateFrom)
                        {
                            txtSaleDateTo = txtSaleDateFrom.AddMonths(3);
                        }
                        else
                        {
                            txtSaleDateFrom = txtSaleDateTo.AddMonths(-3);
                        }

                        return GetPaymntSelngSttusMainList();

                        break;
                }
            }

            return Task.CompletedTask;
        }

        public Task HandleAsync(SelectPosEventArgs message, CancellationToken cancellationToken)
        {
            if (base.Views.Keys.Count() != 0)
            {
                if (message == null) { return Task.CompletedTask; }
                POS = message.PosNo == "전체" ? "전체" : message.PosNo;
                return GetPaymntSelngSttusMainList();
            }
            return Task.CompletedTask;
        }
        public override bool SetIView(IView view)
        {
            _PaymentSttusView = (IPaymentSelngStatusView)view;
            return base.SetIView(view);
        }

    }
}
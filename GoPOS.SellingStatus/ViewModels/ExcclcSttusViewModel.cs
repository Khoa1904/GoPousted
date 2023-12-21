using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

using Caliburn.Micro;
using GoPOS.Models;
using GoPOS.Services;
using GoPOS.Common.ViewModels.Controls;
using GoPOS.Common.ViewModels;
using System.Windows.Input;
using GoPOS.Helpers.CommandHelper;
using System.Windows.Controls;
using Dapper;
using GoPOS.Common.Interface.Model;
using GoPOS.OrderPay.Interface.View;
using GoPOS.SellingStatus.Interface;
using GoPOS.Common.Interface.View;
using GoPOS.Helpers;
using GoPOS.Models.Common;
using GoPOS.OrderPay.Models;
using GoPOS.Service;
using GoShared.Helpers;
using GoPOS.Models.Custom.SalesMng;
using GoPOS.Models.Config;
using GoPOS.Common.Service;
using GoPOS.SalesMng.ViewModels;
using GoPOS.Service.Service;
using System.Globalization;
using GoPOS.SellingStatus.ViewModels;

namespace GoPOS.ViewModels
{
    public class ExcclcSttusViewModel : BaseItemViewModel, IHandle<CalendarEventArgs>, IHandle<SelectSEQEventArgs>
    {
        private IWindowManager _windowManager;
        private IEventAggregator _eventAggregator;
        private IInfoEmpService _empMService;
        private IInfoShopService _shopMService;
        private IExcclcSttusView _ExcclcSttusView;
        private DateTime _txtSaleFrom;
        private DateTime _txtSaleTo;
        private string _seq;
        private bool isFDate;

        private Visibility firstMenuVisibility = Visibility.Visible;

        private SETT_POSACCOUNT LastSettAccount = null;
        private readonly ISellingStatusService _sellingStatusService;
        private readonly ISettAccountService settAccountService;

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
        public string SEQ
        {
            get { return _seq; }
            set
            {
                if (_seq != value)
                {
                    _seq = value;
                    NotifyOfPropertyChange(() => SEQ);
                }
            }
        }
        public ExcclcSttusViewModel(IWindowManager windowManager, ISettAccountService settAccountService, IEventAggregator eventAggregator, IInfoEmpService empMService, IInfoShopService shopMService, ISellingStatusService sellingStatusService)
            : base(windowManager, eventAggregator)
        {
            _windowManager = windowManager;
            _eventAggregator = eventAggregator;

            _empMService = empMService;
            _shopMService = shopMService;
            this.settAccountService = settAccountService;

            this.FirstMenuVisibility = Visibility.Visible;
            this.SecondMenuVisibility = Visibility.Collapsed;

            _eventAggregator.SubscribeOnUIThread(this);
            _sellingStatusService = sellingStatusService;
            this.ViewLoaded += ExcclcSttusViewModel_ViewLoaded;
            //Init();
        }

        private void ExcclcSttusViewModel_ViewLoaded(object? sender, EventArgs e)
        {
            Init();
        }

        public async void Init()
        {
            if (DataLocals.PosStatus == null)
            {
                DialogHelper.MessageBox("영업일자가 설정 되지 않습니다.");
                return;
            }
            txtSaleDateFrom = txtSaleDateTo =Convert.ToDateTime(SalesDateString);
            SEQ = DataLocals.PosStatus.REGI_SEQ != null ? DataLocals.PosStatus.REGI_SEQ : "전체";
            await GetExcclgSttusMainList();

        }
        private ExcclcSttusModel dataModel;
        public ExcclcSttusModel DataModel
        {
            get => dataModel; set
            {
                dataModel = value;
                NotifyOfPropertyChange(() => DataModel);
            }
        }

        private async Task GetExcclgSttusMainList()
        {
            try
            {
                LastSettAccount = settAccountService.GetSingleAsync(DataLocals.PosStatus).Result;

                DynamicParameters param = new DynamicParameters();
                param.Add("@SHOP_CODE", DataLocals.AppConfig.PosInfo.StoreNo, DbType.String, ParameterDirection.Input, DataLocals.AppConfig.PosInfo.StoreNo.Length);
                param.Add("@POS_NO", DataLocals.AppConfig.PosInfo.PosNo, DbType.String, ParameterDirection.Input, DataLocals.AppConfig.PosInfo.PosNo.Length);
                var tDate = txtSaleDateTo.ToString("yyyyMMdd");
                var fDate = txtSaleDateFrom.ToString("yyyyMMdd");
                param.Add("@START_DATE", fDate, DbType.String, ParameterDirection.Input, 10);
                param.Add("@FINISH_DATE", tDate, DbType.String, ParameterDirection.Input, 10);
                param.Add("@REGI_SEQ", SEQ == "전체" ?null : SEQ, DbType.String, ParameterDirection.Input, 10);


                (DataModel, SpResult spResult) = await _sellingStatusService.GetExcclcSttusMainList(param);

                if (spResult.ResultType != EResultType.SUCCESS)
                {
                    return;
                }
            }
            catch (Exception ex)
            {
                LogHelper.Logger.Error("Main 리스트 가져오기 오류 : " + ex.Message);
            }
        }

        public ICommand ButtonCommand => new RelayCommand<Button>(ButtonCommandCenter);
        private async void ButtonCommandCenter(Button btn)
        {
            System.Windows.Point point = _ExcclcSttusView.buttonPosition;
            if (btn.Tag is null) { return; }
            var tDate = txtSaleDateTo.ToString("yyyyMMdd");
            var fDate = txtSaleDateFrom.ToString("yyyyMMdd");
            switch (btn.Tag)
            {
                case "DateFrom":
                    isFDate = true;
                    point = _ExcclcSttusView.buttonPosition;
                    DialogHelper.ShowDialogWithCoords(typeof(CalendarViewModel), 450, 480, point.X , point.Y , txtSaleDateFrom, txtSaleDateTo, "DateFrom");
                    break;

                case "DateTo":
                    isFDate = false;
                    point = _ExcclcSttusView.buttonPosition;
                    DialogHelper.ShowDialogWithCoords(typeof(CalendarViewModel), 450, 480, point.X, point.Y, txtSaleDateTo, txtSaleDateFrom, "DateTo");
                    break;

                case "SelectSEQ":
                    IoC.Get<SelectBoxViewModel>();
                    _eventAggregator?.PublishOnUIThreadAsync(new SelectboxEvent()
                    {
                        Type = "SEQ"
                    });
                    point = _ExcclcSttusView.buttonPosition;
                    DialogHelper.ShowDialogWithCoords(typeof(SelectBoxViewModel), 50, 350, point.X - 89, point.Y + 30, SEQ);
                    break;

                case "Check":
                    await GetExcclgSttusMainList();
                    break;

                case "Print":
                    break;

                case "GiftCardDetails":
                    this.DataModel.SALE_DATE = fDate + " " + tDate;
                    LastSettAccount = LastSettAccount.CopyFieldsFrom<SETT_POSACCOUNT>(this.DataModel, null);
                    DialogHelper.ShowDialog(typeof(SellingStatusGiftCertDetailsViewModel), 650, 500, LastSettAccount);
                    break;

                case "ApprDetails":
                
                    this.DataModel.SALE_DATE = fDate + " " + tDate;
                    LastSettAccount = LastSettAccount.CopyFieldsFrom<SETT_POSACCOUNT>(this.DataModel, null);

                    DialogHelper.ShowDialog(typeof(SellingStatusApprDetailsViewModel), 650, 500, LastSettAccount);
                    break;

                case "Next":
                    var mapFields = new Dictionary<string, string>();
                    mapFields.Add("ACCOUNT_TOTAL_AMT", "REM_CASH_AMT");

                    var data = IoC.Get<SellingStatusSettleSumViewModel>();
                    data.txtSaleDateFrom=txtSaleDateFrom;
                    data.txtSaleDateTo=txtSaleDateTo;
                    data.SEQ = SEQ;
                    data.GetFinalStatus();
                    LastSettAccount = LastSettAccount.CopyFieldsFrom<SETT_POSACCOUNT>(this.DataModel, null);
                    {
                        _eventAggregator.PublishOnUIThreadAsync(new SellingStatusMainEventArgs()
                        {
                            EventType = "OpenVM",
                            EventData = "SellingStatusSettleSumViewModel",
                            EventData2 = LastSettAccount,
                            EventFlag = false,
                        });
                    }
                    this.DeactivateClose(true);
                    break;
                default:
                    break;
            }
        }

        public override bool SetIView(IView view)
        {
            _ExcclcSttusView = (IExcclcSttusView)view;
            return base.SetIView(view);
        }

        public Task HandleAsync(CalendarEventArgs message, CancellationToken cancellationToken)
        {
            if (base.Views.Keys.Count() != 0)
            {
                if (message.EventType == "ExtButton")
                {
                    if (isFDate)
                        txtSaleDateFrom = message.Date;
                    else
                        txtSaleDateTo = message.Date;

                    return GetExcclgSttusMainList();

                }

                switch (message.EventType)
                {
                    case "btnConfirm":
                        if (isFDate)
                        {
                            txtSaleDateFrom = txtSaleDateTo;
                        }
                        else
                        {
                            txtSaleDateFrom = txtSaleDateTo;
                        }

                        return GetExcclgSttusMainList();
                        break;
                    case "btnCancel7":
                        if (isFDate)
                        {
                            txtSaleDateTo = txtSaleDateFrom.AddDays(7);
                        }
                        else
                        {
                            txtSaleDateFrom = txtSaleDateTo.AddDays(-7);
                        }

                        return GetExcclgSttusMainList();

                        break;
                    case "btnCancel15":
                        if (isFDate)
                        {
                            txtSaleDateTo = txtSaleDateFrom.AddDays(15);
                        }
                        else
                        {
                            txtSaleDateFrom = txtSaleDateTo.AddDays(-15);
                        }

                        return GetExcclgSttusMainList();

                        break;
                    case "btnCancel1":
                        if (isFDate)
                        {
                            txtSaleDateTo = txtSaleDateFrom.AddMonths(1);
                        }
                        else
                        {
                            txtSaleDateFrom = txtSaleDateTo.AddMonths(-1);
                        }

                        return GetExcclgSttusMainList();

                        break;
                    case "btnCancel3":
                        if (isFDate)
                        {
                            txtSaleDateTo = txtSaleDateFrom.AddMonths(3);
                        }
                        else
                        {
                            txtSaleDateFrom = txtSaleDateTo.AddMonths(-3);
                        }

                        return GetExcclgSttusMainList();

                        break;
                }
            }

            return Task.CompletedTask;
        }

        public Task HandleAsync(SelectSEQEventArgs message, CancellationToken cancellationToken)
        {
            if (base.Views.Keys.Count() != 0)
            {

                SEQ = message.SEQNo;
                return GetExcclgSttusMainList();
            }
            return Task.CompletedTask;
        }

    
    }
}

using Caliburn.Micro;
using Dapper;
using GoPOS.Common.Interface.View;
using GoPOS.Common.ViewModels;
using GoPOS.Helpers;
using GoPOS.Helpers.CommandHelper;
using GoPOS.Interface;
using GoPOS.Models;
using GoPOS.Models.Common;
using GoPOS.OrderPay.Models;
using GoPOS.ViewModels;
using GoPOS.Views;
using GoPOS.SalesMng.Interface.View;
using GoPOS.Service;
using GoPOS.Services;

using GoPOS.Views;
using GoShared.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Dynamic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using GoPOS.Common.Helpers;
using static GoPOS.Function;
using GoPOS.SellingStatus.Interface;
using GoPOS.Common.Views.Controls;
using GoPOS.Models.Config;
using GoPOS.Models.Custom.SalesMng;
using GoPOS.Models.Custom.SellingStatus;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using LiveCharts;

/*
 매출현황 > 월별 매출현황

 */

namespace GoPOS.ViewModels
{

    public class MtSelngSttusViewModel : BaseItemViewModel, INotifyPropertyChanged, IHandle<SelectPosEventArgs>, IHandle<SelectCommonCodeArgs>
    {
        private readonly ISellingStatusService sellingStatusService;
        List<SELLING_STATUS_INFO> MainList = new List<SELLING_STATUS_INFO>();
        List<SELLING_STATUS_INFO> DetailList = new List<SELLING_STATUS_INFO>();
        private IEventAggregator _eventAggregator;
        private IMtSelngSttusView _view = null;



        public MtSelngSttusViewModel(IWindowManager windowManager, IEventAggregator eventAggregator, ISellingStatusService service)
             : base(windowManager, eventAggregator)
        {
            ActiveItem = (IScreen)IoC.Get<IMtSelngSttusView>();
            this.sellingStatusService = service;
            this._selectedItemMainList = new SELLING_STATUS_INFO();
            this._eventAggregator = eventAggregator;
            this._eventAggregator.SubscribeOnPublishedThread(this);
            this.FirstMenuVisibility = Visibility.Visible;
            this.SecondMenuVisibility = Visibility.Collapsed;
            this.ViewInitialized += MtSelngSttusViewModel_ViewInitialized;
            this.ViewLoaded += MtSelngSttusViewModel_ViewLoaded;

        }
        private async void MtSelngSttusViewModel_ViewInitialized(object? sender, EventArgs e)
        {
            PosNo = DataLocals.AppConfig.PosInfo.PosNo;

            CalendarDays = new ObservableCollection<int>();
            GenerateCalendarDays();
            await GetMtSelngSttusMainList();
        }

        private MtSelngSttusModel _dataModel;

        public MtSelngSttusModel DataModel
        {
            get => _dataModel; set
            {
                _dataModel = value;
                NotifyOfPropertyChange(() => DataModel);
            }
        }

        private void MtSelngSttusViewModel_ViewLoaded(object? sender, EventArgs e)
        {
            Init();
        }

        public DrawnGraph DrawnGraph { get; set; }


        private SELLING_STATUS_INFO _selectedItemMainList;
        public SELLING_STATUS_INFO SelectedItemMainList
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

        public async void Init()
        {
            if (DataLocals.PosStatus == null)
            {
                DialogHelper.MessageBox("영업일자가 설정 되지 않습니다.");
                return;
            }
            Code = "ommon00";
            CodeName = "전체";
            PosNo = DataLocals.AppConfig.PosInfo.PosNo;
            CurrentDate = Convert.ToDateTime(SalesDateString);
            GenerateCalendarDays();
            await Task.Delay(100);
            await GetMtSelngSttusMainList();


        }

        private List<String> _Text = new List<string>();
        public List<string> Text
        {
            get
            {
                return _Text;
            }
            set
            {
                _Text = value;
                NotifyOfPropertyChange(() => Text);
            }
        }


        // Calendar prop
        private DateTime _currentDate = DateTime.Now;
        public DateTime CurrentDate
        {
            get { return _currentDate; }
            set
            {
                if (_currentDate != value)
                {
                    _currentDate = value;
                    NotifyOfPropertyChange(nameof(CurrentDate));
                }
            }
        }

        private string _code;

        public string Code
        {
            get { return _code; }
            set
            {
                if (_code != value)
                {
                    _code = value;
                    NotifyOfPropertyChange(() => Code);
                }
            }
        }

        private List<string> _listTotalDays;

        public List<string> ListTotalDays
        {
            get => _listTotalDays;
            set
            {
                _listTotalDays = value;
                NotifyOfPropertyChange(() => ListTotalDays);
            }
        }

        public ChartValues<double> CharValues
        {
            get;
            set;
        }



        private string _codename;

        public string CodeName
        {
            get { return _codename; }
            set
            {
                if (_codename != value)
                {
                    _codename = value;
                    NotifyOfPropertyChange(() => CodeName);
                }
            }
        }
        public ObservableCollection<int> CalendarDays { get; set; }

        private void GenerateCalendarDays()
        {
            CalendarDays.Clear();
            int daysInMonth = DateTime.DaysInMonth(CurrentDate.Year, CurrentDate.Month);
            DateTime startDate = new DateTime(CurrentDate.Year, CurrentDate.Month, 1);
            int startDayOffset = (int)startDate.DayOfWeek;

            // Rem-day from last month
            DateTime previousMonth = startDate.AddMonths(-1);
            int previousMonthDays = DateTime.DaysInMonth(previousMonth.Year, previousMonth.Month);
            for (int i = startDayOffset - 1; i >= 0; i--)
            {
                CalendarDays.Add(previousMonthDays - i);
            }

            // Days of current month
            for (int i = 1; i <= daysInMonth; i++)
            {
                CalendarDays.Add(i);
            }

            // Add days from the next month
            int remainingDays = 42 - (daysInMonth + startDayOffset);
            DateTime nextMonth = startDate.AddMonths(1);
            for (int i = 1; i <= remainingDays; i++)
            {
                CalendarDays.Add(i);
            }
            NotifyOfPropertyChange(() => CalendarDays);
        }

        public ICommand BorderCommand => new RelayCommand<Border>(BorderCommandCtrl);

        private async void BorderCommandCtrl(Border obj)
        {
            switch (obj.Tag)
            {
                case "weeks1":
                    Graph("weeks1");
                    break;
                case "weeks2":
                    Graph("weeks2");
                    break;
                case "weeks3":
                    Graph("weeks3");
                    break;
                case "weeks4":
                    Graph("weeks4");
                    break;
                case "weeks5":
                    Graph("weeks5");
                    break;
                case "weeks6":
                    Graph("weeks6");
                    break;
                case "class1":
                    Graph("sunday");
                    break;
                case "class2":
                    Graph("monday");
                    break;
                case "class3":
                    Graph("tuesday");
                    break;
                case "class4":
                    Graph("wednesday");
                    break;
                case "class5":
                    Graph("thursday");
                    break;
                case "class6":
                    Graph("friday");
                    break;
                case "class7":
                    Graph("saturday");
                    break;
                case "months":
                    Graph("months");
                    break;
                default:
                    break;
            }
        }

        public ICommand DaysCommand => new RelayCommand<Border>(DaysCommandCtrl);

       

        private async void DaysCommandCtrl(Border obj)
        {
            try
            {
                if (!string.IsNullOrEmpty(obj.Tag.ToString()))
                {
                    var control = obj.FindVisualChildren<TextBlock>();
                    var childControl = control.FirstOrDefault();
                    var day = childControl.Tag;

                    Graph("per",day.ToString());
                }
             
            }
            catch (Exception ex)
            {

            }
        }






        public ICommand ButtonCommand => new RelayCommand<Button>(ButtonCommandCtrl);
        private async void ButtonCommandCtrl(Button btn)
        {

            switch (btn.Tag)
            {
                case "Previous":
                    CurrentDate = CurrentDate.AddMonths(-1);
                    GenerateCalendarDays();
                    await GetMtSelngSttusMainList();
                    //_view.RenderCalendarDays(CalendarDays, MainList);
                    break;
                case "Next":
                    if (CurrentDate.AddHours(23) >= DateTime.Now)
                    {
                        return;
                    }
                    else
                    {
                        CurrentDate = CurrentDate.AddMonths(1);
                        GenerateCalendarDays();
                        await GetMtSelngSttusMainList();
                        //_view.RenderCalendarDays(CalendarDays, MainList);
                    }
                    break;
                case "search1":
                    IoC.Get<SelectBoxViewModel>();
                    _eventAggregator?.PublishOnUIThreadAsync(new SelectboxEvent()
                    {
                        Type = "POS"
                    });
                    DialogHelper.ShowDialogWithCoords(typeof(SelectBoxViewModel), 50, 350, _view.buttonPosition.X - 85, _view.buttonPosition.Y + 30);
                    break;
                case "search2":
                    IoC.Get<SelectBoxViewModel>();
                    _eventAggregator?.PublishOnUIThreadAsync(new SelectboxEvent()
                    {
                        Type = "COMCODE"
                    });

                    DialogHelper.ShowDialogWithCoords(typeof(SelectBoxViewModel), 50, 350, _view.buttonPosition.X - 89, _view.buttonPosition.Y + 30, Code);
                    break;
                case "":
                    MessageBox.Show("ok");
                    break;
                default:
                    break;
            }
        }

        public bool SearchPOS
        {
            get; set;
        }
        public bool SearchEct
        {
            get; set;
        }
        public override bool SetIView(IView view)
        {
            _view = (IMtSelngSttusView)view;
            return base.SetIView(view);
        }

        private void PreviousButton()
        {
            CurrentDate = CurrentDate.AddMonths(-1);
            GenerateCalendarDays();
        }

        private void NextButton()
        {
            CurrentDate = CurrentDate.AddMonths(-1);
            GenerateCalendarDays();
        }
        private async Task GetMtSelngSttusMainList()
        {
            try
            {
                DynamicParameters param = new DynamicParameters();

                var date = CurrentDate.ToString("yyyyMM");

                param.Add("@CURRENT_DAY", date, DbType.String, ParameterDirection.Input, 6);
                param.Add("@SHOP_CODE", DataLocals.AppConfig.PosInfo.StoreNo, DbType.String, ParameterDirection.Input, DataLocals.AppConfig.PosInfo.StoreNo.Length);
                param.Add("@POS_NO", PosNo == "전체" ? "00" : PosNo, DbType.String, ParameterDirection.Input, 10);
                param.Add("@PAY_TYPE_AMT", payTypeCol);

                //param.Add("@SALE_DATE", DataLocals.PosStatus.SALE_DATE, DbType.String, ParameterDirection.Input, DataLocals.PosStatus.SALE_DATE.Length);
                //param.Add("@ORDER_NO", DataLocals.PosStatus.ORDER_NO, DbType.String, ParameterDirection.Input, DataLocals.PosStatus.ORDER_NO.Length);

                (MainList, SpResult spResult) = await sellingStatusService.GetClSelngSttusMainList(param);

                if (spResult.ResultType != EResultType.SUCCESS)
                {
                    //WindowHelper.InfoMessage("사용자 List 가져오기", spResult.ResultMessage);
                    return;
                }

                for (int i = 0; i < MainList.Count; i++)
                {

                    MainList[i].TOT_SALE_AMT = Comma(MainList[i].TOT_SALE_AMT);
                    MainList[i].SALE_DATE = MainList[i].SALE_DATE;

                }

                _view.RenderCalendarDays(CalendarDays, MainList,date);
                //DataModel = new MtSelngSttusModel();
                //DataModel.Week1 = 30;
                //DataModel.Week2 = 30;
                //IoC.Get<ClSelngSttusView>().lstViewMain.ItemsSource = MainList;

                //string jsonResult = JsonHelper.ModelToJson(resultList);

                //WindowHelper.InfoMessageRaw("조회결과", jsonResult.JsonPrettify());
                //LogHelper.Logger.Info("조회결과 : " + jsonResult);
            }
            catch (Exception ex)
            {
                LogHelper.Logger.Error("Main 리스트 가져오기 오류 : " + ex.Message);
            }
        }

        public async void OnSelectedItemChanged(SELLING_STATUS_INFO selectedItem)
        {
            // 선택된 항목 처리 로직 구현

            try
            {
                DetailList.Clear();
                //IoC.Get<ClSelngSttusView>().lstViewSub.Items.Refresh();

                DynamicParameters param = new DynamicParameters();
                param.Add("@SHOP_CODE", DataLocals.AppConfig.PosInfo.StoreNo, DbType.String, ParameterDirection.Input, DataLocals.AppConfig.PosInfo.StoreNo.Length);
                param.Add("@POS_NO", PosNo == "전체" ? "00" : PosNo, DbType.String, ParameterDirection.Input, 10);

                //param.Add("@SALE_DATE", DataLocals.PosStatus.SALE_DATE, DbType.String, ParameterDirection.Input, DataLocals.PosStatus.SALE_DATE.Length);

                (DetailList, SpResult spResult) = await sellingStatusService.GetClSelngSttusDetailList(param);

                if (spResult.ResultType != EResultType.SUCCESS)
                {
                    //WindowHelper.InfoMessage("사용자 List 가져오기", spResult.ResultMessage);
                    return;
                }


                //public string NORMAL_UPRC { get; set; } = string.Empty; //NUMERIC(10,2),
                //    public string SALE_QTY { get; set; } = string.Empty; //NUMERIC(12,2),
                //    public string DC_AMT { get; set; } = string.Empty; //NUMERIC(12,2),
                //    public string DCM_SALE_AMT { get; set; } = string.Empty; //NUMERIC(12,2),
                for (int i = 0; i < DetailList.Count; i++)
                {

                    MainList[i].TOT_SALE_QTY = Comma(MainList[i].TOT_SALE_QTY);
                    MainList[i].TOT_SALE_AMT = Comma(MainList[i].TOT_SALE_AMT);
                    MainList[i].TOT_DC_AMT = Comma(MainList[i].TOT_DC_AMT);
                    MainList[i].DCM_SALE_AMT = Comma(MainList[i].DCM_SALE_AMT);
                    MainList[i].VAT_AMT = Comma(MainList[i].VAT_AMT);
                    MainList[i].TOT_AMT = Comma(MainList[i].TOT_AMT);
                    MainList[i].PAY_CNT = Comma(MainList[i].PAY_CNT);
                    MainList[i].SALE_AMT = Comma(MainList[i].SALE_AMT);

                }

                //IoC.Get<ClSelngSttusView>().lstViewSub.ItemsSource = DetailList;
                //string jsonResult = JsonHelper.ModelToJson(resultList);

                //WindowHelper.InfoMessageRaw("조회결과", jsonResult.JsonPrettify());
                //LogHelper.Logger.Info("조회결과 : " + jsonResult);
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

        private string _posNo;
        public string PosNo
        {
            get => _posNo;
            set
            {
                if (_posNo != value)
                {
                    _posNo = value;
                    NotifyOfPropertyChange(nameof(PosNo));
                }
            }
        }
        public string payTypeCol
        {
            get
            {
                switch (Code)
                {
                    case "00":
                        return "TOT_SALE_AMT";
                        break;

                    case "0380001":
                        return "CASH_AMT";
                        break;

                    case "0380002":
                        return "CRD_CARD_AMT";
                        break;

                    case "0380003":
                        return "TK_GFT_AMT";
                        break;

                    case "0380004":
                        return "TK_FOD_AMT";
                        break;

                    case "0380005":
                        return "SP_PAY_AMT";
                        break;

                    default:
                        return "TOT_SALE_AMT";
                        break;
                }
            }
        }
        private Dictionary<string, string> nameDay = new Dictionary<string, string>()
        {
            { "sunday","일요일"},
            { "monday","월요일"},
            { "tuesday","화요일"},
            { "wednesday","수요일"},
            { "thursday","목요일"},
            { "friday","금요일"},
            { "saturday","토요일 "},
        };



        public void Graph(string type = "days",string day="")
        {
            dynamic settings = new ExpandoObject();
            settings.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            settings.ResizeMode = ResizeMode.NoResize;
            settings.MinWidth = 952;
            settings.WindowStyle = WindowStyle.None;

            var mtSelngSttusGrphView = IoC.Get<MtSelngSttusGrphViewModel>();
            mtSelngSttusGrphView.Pos_No = PosNo;
            mtSelngSttusGrphView.CurrentDate = CurrentDate;

            string yearMonth = CurrentDate.ToString("yyyyMM");

            if (type.Contains("day"))
            {
                if (Code != "ommon00")
                    return;
                mtSelngSttusGrphView.Text = yearMonth[..^2] + "년 " + yearMonth[^2..] + "월 " + nameDay[type] + " 매출현항";
            }
            else if (type.Contains("weeks"))
            {
                mtSelngSttusGrphView.Text = yearMonth[..^2] + "년 " + yearMonth[^2..] + "월 " + type[^1..] + "주차 매출현항";
                if (Code != "ommon00")
                    return;
            }
            else if (type.Contains("per"))
                mtSelngSttusGrphView.Text = yearMonth[..^2] + "년 " + yearMonth[^2..] + "월 " + day[^2..] + "일 매출현항";

            switch (type)
            {
                case "days":
                    mtSelngSttusGrphView.CharValues = CharValues;
                    mtSelngSttusGrphView.Init(payTypeCol);
                    break;
                case "weeks1":
                    var dayInMonth = _view.day1.Tag.ToString();
                    mtSelngSttusGrphView.CharValues = DrawnGraph.Week1;
                    mtSelngSttusGrphView.Init(payTypeCol, "weeks1", dayInMonth);
                    break;
                case "weeks2":
                    dayInMonth = _view.day8.Tag.ToString();
                    mtSelngSttusGrphView.CharValues = DrawnGraph.Week2;
                    mtSelngSttusGrphView.Init(payTypeCol, "weeks2", dayInMonth);

                    break;
                case "weeks3":
                    dayInMonth = _view.day15.Tag.ToString();

                    mtSelngSttusGrphView.CharValues = DrawnGraph.Week3;
                    mtSelngSttusGrphView.Init(payTypeCol, "weeks3", dayInMonth);

                    break;
                case "weeks4":
                    dayInMonth = _view.day22.Tag.ToString();

                    mtSelngSttusGrphView.CharValues = DrawnGraph.Week4;
                    mtSelngSttusGrphView.Init(payTypeCol, "weeks4", dayInMonth);

                    break;
                case "weeks5":
                    dayInMonth = _view.day29.Tag.ToString();
                    mtSelngSttusGrphView.CharValues = DrawnGraph.Week5;
                    mtSelngSttusGrphView.Init(payTypeCol, "weeks5", dayInMonth);

                    break;
                case "weeks6":
                    dayInMonth = _view.day36.Tag.ToString();
                    mtSelngSttusGrphView.CharValues = DrawnGraph.Week6;
                    mtSelngSttusGrphView.Init(payTypeCol, "weeks6");
                    break;
                case "sunday":
                    mtSelngSttusGrphView.CharValues = DrawnGraph.Class1;
                    mtSelngSttusGrphView.Init(payTypeCol,"sunday");
                    break;
                case "monday":
                    mtSelngSttusGrphView.CharValues = DrawnGraph.Class2;
                    mtSelngSttusGrphView.Init(payTypeCol, "monday");

                    break;
                case "tuesday":
                    mtSelngSttusGrphView.CharValues = DrawnGraph.Class3;
                    mtSelngSttusGrphView.Init(payTypeCol, "tuesday");

                    break;
                case "wednesday":
                    mtSelngSttusGrphView.CharValues = DrawnGraph.Class4;
                    mtSelngSttusGrphView.Init(payTypeCol, "wednesday");

                    break;
                case "thursday":
                    mtSelngSttusGrphView.CharValues = DrawnGraph.Class5;
                    mtSelngSttusGrphView.Init(payTypeCol, "thursday");

                    break;
                case "friday":
                    mtSelngSttusGrphView.CharValues = DrawnGraph.Class6;
                    mtSelngSttusGrphView.Init(payTypeCol, "friday");
                    break;
                case "saturday":
                    mtSelngSttusGrphView.CharValues = DrawnGraph.Class7;
                    mtSelngSttusGrphView.Init(payTypeCol, "saturday");
                    break;
                case "months":
                    mtSelngSttusGrphView.CharValues = CharValues;
                    mtSelngSttusGrphView.Init(payTypeCol);
                    mtSelngSttusGrphView.Text = yearMonth[..^2] + "년 " + yearMonth[^2..] + "월 월간 매출현황";

                    break;
                case "per":
                    if (Code != "ommon00")
                        return;
                    mtSelngSttusGrphView.LoadDay(day, payTypeCol);
                    break;
                default:
                    mtSelngSttusGrphView.CharValues = CharValues;
                    mtSelngSttusGrphView.Init(payTypeCol);
                    break;
                    
            }


            IWindowManager manager = new WindowManager();
            manager.ShowDialogAsync(mtSelngSttusGrphView, null, settings);
        }

        public Task HandleAsync(SelectPosEventArgs message, CancellationToken cancellationToken)
        {
            if (base.Views.Keys.Count() != 0)
            {

                PosNo = message.PosNo;
                return GetMtSelngSttusMainList();
            }
            return Task.CompletedTask;
        }

        public Task HandleAsync(SelectCommonCodeArgs message, CancellationToken cancellationToken)
        {
            if (base.Views.Keys.Count() != 0)
            {

                Code = message.CommonCode;
                CodeName = message.ComcodeName;
                return GetMtSelngSttusMainList();
            }
            return Task.CompletedTask;
        }
    }

    public class DrawnGraph
    {
        public ChartValues<double> Week1 { get; set; }
        public ChartValues<double> Week2 { get; set; }
        public ChartValues<double> Week3 { get; set; }
        public ChartValues<double> Week4 { get; set; }
        public ChartValues<double> Week5 { get; set; }
        public ChartValues<double> Week6 { get; set; }

        public ChartValues<double> Class1 { get; set; }
        public ChartValues<double> Class2 { get; set; }

        public ChartValues<double> Class3 { get; set; }

        public ChartValues<double> Class4 { get; set; }

        public ChartValues<double> Class5 { get; set; }

        public ChartValues<double> Class6 { get; set; }

        public ChartValues<double> Class7 { get; set; }

        public ChartValues<double> Total { get; set; }




    }


}
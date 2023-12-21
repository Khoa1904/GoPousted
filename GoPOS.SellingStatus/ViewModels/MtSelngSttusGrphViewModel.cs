using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

using Caliburn.Micro;
using GoPOS.Models;
using GoPOS.Services;
using GoPOS.Common.ViewModels.Controls;
using GoPOS.Models.Common;
using GoPOS.Models.Custom.SellingStatus;
using Dapper;
using GoPOS.Service.Service;
using System.Data;
using GoPOS.Common.Interface.View;
using GoPOS.Common.Views;
using GoShared.Helpers;
using GoPOS.Common.ViewModels;
using GoPOS.SellingStatus.Interface;
using GoPOS.SellingStatus.ViewModels;
using LiveCharts;

namespace GoPOS.ViewModels
{
    public class MtSelngSttusGrphViewModel : BaseItemViewModel
    {
        private IWindowManager _windowManager;
        private IEventAggregator _eventAggregator;
        private GRAPH_SELNG_STTUS _closingData;
        private IInfoEmpService _empMService;
        private IInfoShopService _shopMService;
        private ISellingStatusService _sellingService;
        private IMtSelngSttusGrphView _view;



        private string saleDate;

      
        public MtSelngSttusGrphViewModel(IWindowManager windowManager, IEventAggregator eventAggregator, IInfoEmpService empMService, 
            IInfoShopService shopMService, ISellingStatusService sellingService) : base(windowManager, eventAggregator)
        {
            _windowManager = windowManager;
            _eventAggregator = eventAggregator;

            _empMService = empMService;
            _shopMService = shopMService;

            _eventAggregator.SubscribeOnUIThread(this);
            _sellingService = sellingService;
            ScreenViewLoad();
            this.Graph = IoC.Get<LiveChartViewModel>();
        }

        public string text;
        public string Text
        {
            get { return text; }
            set
            {
                text = value;
                NotifyOfPropertyChange(nameof(Text));
            }
        }

        private DateTime _currentDate;
        public DateTime CurrentDate
        {
            get { return _currentDate; }
            set
            {
                if (_currentDate != value)
                {//2022년 11월 월간 매출현황
                    _currentDate = value;
                    NotifyOfPropertyChange(nameof(CurrentDate));
                }
            }
        }
        public LiveChartViewModel Graph { get; private set; }

        private List<string> listNameWeeks = new List<string>
        {
            "일요일","월요일","화요일","수요일","목요일","금요일","토요일"
        };

        private List<string> listNameClasss = new List<string>
        {
            "1주 ", "2주 ", "3주 ", "4주 ", "5주 ",
        };
        //public async void Init(string payTypeCol, string type = "month", string dayofweek = "")
        //{
        //    var ChartModel = IoC.Get<LiveChartViewModel>();

        //    switch (type)
        //    {
        //        case "month":
        //            await LoadMonthlyData(payTypeCol);
        //            ChartModel.CharValues = CharValues;
        //            return;
        //        case "sunday":
        //            await LoadClassData("0", payTypeCol);
        //            var listName = listNameClasss.Select(x =>
        //            {
        //                x += "월요일";
        //                return x;
        //            }).ToList();
        //            ChartModel.CharValues = CharValues;
        //            ChartModel.ListNameColumn = listName; ChartModel.ListNameColumn = listNameClasss;
        //            return;
        //        case "monday":
        //            await LoadClassData("1", payTypeCol);

        //            listName = listNameClasss.Select(x =>
        //           {
        //               x += "월요일";
        //               return x;
        //           }).ToList();
        //            ChartModel.CharValues = CharValues;
        //            ChartModel.ListNameColumn = listName;
        //            return;
        //        case "tuesday":
        //            await LoadClassData("2", payTypeCol);
        //            listName = listNameClasss.Select(x =>
        //            {
        //                x += "화요일";
        //                return x;
        //            }).ToList();
        //            ChartModel.ListNameColumn = listName;
        //            ChartModel.CharValues = CharValues;
        //            return;
        //        case "wednesday":
        //            await LoadClassData("3", payTypeCol);
        //            listName = listNameClasss.Select(x =>
        //            {
        //                x += "수요일";
        //                return x;
        //            }).ToList();
        //            ChartModel.CharValues = CharValues;
        //            ChartModel.ListNameColumn = listName;
        //            return;
        //        case "thursday":
        //            await LoadClassData("4", payTypeCol);
        //            listName = listNameClasss.Select(x =>
        //            {
        //                x += "목요일";
        //                return x;
        //            }).ToList();
        //            ChartModel.CharValues = CharValues;
        //            ChartModel.ListNameColumn = listName;
        //            return;
        //        case "friday":
        //            await LoadClassData("5", payTypeCol);
        //            listName = listNameClasss.Select(x =>
        //            {
        //                x += "금요일";
        //                return x;
        //            }).ToList();
        //            ChartModel.CharValues = CharValues;
        //            ChartModel.ListNameColumn = listName;
        //            return;
        //        case "saturday":
        //            await LoadClassData("6", payTypeCol);
        //            listName = listNameClasss.Select(x =>
        //            {
        //                x += "토요일";
        //                return x;
        //            }).ToList();
        //            ChartModel.CharValues = CharValues;
        //            ChartModel.ListNameColumn = listName;
        //            return;
        //        case "weeks1":
        //            await LoadWeekData("1", payTypeCol, dayofweek);

        //            ChartModel.ListNameColumn = listNameWeeks;
        //            break;
        //        case "weeks2":
        //            await LoadWeekData("2", payTypeCol, dayofweek);

        //            ChartModel.ListNameColumn = listNameWeeks;
        //            break;
        //        case "weeks3":
        //            await LoadWeekData("3", payTypeCol, dayofweek);

        //            ChartModel.ListNameColumn = listNameWeeks;
        //            break;
        //        case "weeks4":
        //            await LoadWeekData("4", payTypeCol, dayofweek);

        //            ChartModel.ListNameColumn = listNameWeeks;
        //            break;
        //        case "weeks5":
        //            await LoadWeekData("5", payTypeCol, dayofweek);

        //            ChartModel.ListNameColumn = listNameWeeks;
        //            break;
        //        case "weeks6":
        //            await LoadWeekData("6", payTypeCol, dayofweek);
        //            ChartModel.ListNameColumn = listNameWeeks;
        //            break;
        //    }
        //    ChartModel.CharValues = new ChartValues<double>
        //    {
        //        (double)ClosingData.CASH_AMT,
        //        (double)ClosingData.CRD_CARD_AMT,
        //        (double)ClosingData.JCD_CARD_AMT,
        //        (double)ClosingData.WES_AMT,
        //        (double)ClosingData.CST_POINTUSE_AMT,
        //        (double)ClosingData.TK_GFT_AMT,
        //        0,
        //        (double)ClosingData.ETC_AMT,
        //        (double)ClosingData.TOT_SALE_AMT,
        //    };
        //}

        public async void Init(string payTypeCol, string type = "month", string dayofweek = "")

        {
            var ChartModel = IoC.Get<LiveChartViewModel>();

            switch (type)
            {
                case "month":
                    await LoadMonthlyData(payTypeCol);
                    ChartModel.CharValues = CharValues;
                    return;
                case "sunday":
                    await LoadClassData("0", payTypeCol);
                    var listName = listNameClasss.Select(x =>
                    {
                        x += "일요일";
                        return x;
                    }).ToList();

                    ChartModel.ListNameColumn = listName; ChartModel.ListNameColumn = listNameClasss;
                    break;
                case "monday":
                    await LoadClassData("1", payTypeCol);

                    listName = listNameClasss.Select(x =>
                    {
                        x += "월요일";
                        return x;
                    }).ToList();

                    ChartModel.ListNameColumn = listName;
                    break;
                case "tuesday":
                    await LoadClassData("2", payTypeCol);
                    listName = listNameClasss.Select(x =>
                    {
                        x += "화요일";
                        return x;
                    }).ToList();
                    ChartModel.ListNameColumn = listName;

                    break;
                case "wednesday":
                    await LoadClassData("3", payTypeCol);
                    listName = listNameClasss.Select(x =>
                    {
                        x += "수요일";
                        return x;
                    }).ToList();

                    ChartModel.ListNameColumn = listName;
                    break;
                case "thursday":
                    await LoadClassData("4", payTypeCol);
                    listName = listNameClasss.Select(x =>
                    {
                        x += "목요일";
                        return x;
                    }).ToList();

                    ChartModel.ListNameColumn = listName;
                    break;
                case "friday":
                    await LoadClassData("5", payTypeCol);
                    listName = listNameClasss.Select(x =>
                    {
                        x += "금요일";
                        return x;
                    }).ToList();

                    ChartModel.ListNameColumn = listName;
                    break;
                case "saturday":
                    await LoadClassData("6", payTypeCol);
                    listName = listNameClasss.Select(x =>
                    {
                        x += "토요일";
                        return x;
                    }).ToList();

                    ChartModel.ListNameColumn = listName;
                    break;
                case "weeks1":
                    await LoadWeekData("1", payTypeCol, dayofweek);

                    ChartModel.ListNameColumn = listNameWeeks;
                    break;
                case "weeks2":
                    await LoadWeekData("2", payTypeCol, dayofweek);

                    ChartModel.ListNameColumn = listNameWeeks;
                    break;
                case "weeks3":
                    await LoadWeekData("3", payTypeCol, dayofweek);

                    ChartModel.ListNameColumn = listNameWeeks;
                    break;
                case "weeks4":
                    await LoadWeekData("4", payTypeCol, dayofweek);

                    ChartModel.ListNameColumn = listNameWeeks;
                    break;
                case "weeks5":
                    await LoadWeekData("5", payTypeCol, dayofweek);

                    ChartModel.ListNameColumn = listNameWeeks;
                    break;
                case "weeks6":
                    await LoadWeekData("6", payTypeCol, dayofweek);
                    ChartModel.ListNameColumn = listNameWeeks;
                    break;
            }
            ChartModel.CharValues = CharValues;


        }
        private List<string> DayName = new List<string>
        {
            "현      금",
            "신 용 카 드" ,
           // "제 휴 카 드" , 그래프 제휴, 온라인삭제
            "외      상" ,
            "포인트스탬프"  , //회원스탬프 -> 포인트스탬프 명칭수정
            "상 품 권"   ,
           // "온라인매출"  , 그래프 제휴, 온라인삭제
            "할      인" ,
        };

        public async void LoadDay(string day, string payTypeCol)
        {
            await LoadDayData(day, payTypeCol);
            var ChartModel = IoC.Get<LiveChartViewModel>();
            ChartModel.CharValues = new ChartValues<double>
            {
                (double)ClosingData.CASH_AMT,
                (double)ClosingData.CRD_CARD_AMT,
                //(double)ClosingData.JCD_CARD_AMT,
                (double)ClosingData.WES_AMT,
                (double)ClosingData.CST_POINTUSE_AMT,
                (double)ClosingData.TK_GFT_AMT,
                //0,
                //(double)ClosingData.ETC_AMT
                (double)ClosingData.TOT_DC_AMT //할인총액
            };
            ChartModel.ListNameColumn = DayName;
        }

        public override bool SetIView(IView view)
        {
            _view = (IMtSelngSttusGrphView)view;
            return base.SetIView(view);
        }

        public ChartValues<double> CharValues
        {
            get;
            set;
        }

        public string Pos_No { get; set; }

        private void ScreenViewLoad()
        {
            SaleDate = DataLocals.PosStatus.SALE_DATE;
        }
        public GRAPH_SELNG_STTUS ClosingData
        {
            get => _closingData; set
            {
                _closingData = value;
                NotifyOfPropertyChange(() => ClosingData);
            }
        }
        public string SaleDate
        {
            get => saleDate; set
            {
                saleDate = value;
                //  NotifyOfPropertyChange(() => ClosingData);
            }
        }

        private async Task LoadMonthlyData(string payTypeCol)
        {
            try
            {

                DynamicParameters param = new DynamicParameters();
                param.Add("@SHOP_CODE", DataLocals.AppConfig.PosInfo.StoreNo, DbType.String, ParameterDirection.Input, DataLocals.AppConfig.PosInfo.StoreNo.Length);
                param.Add("@POS_NO", Pos_No == "전체" ? "00" : Pos_No, DbType.String, ParameterDirection.Input, 10);

                var month = CurrentDate.ToString("yyyyMM");
                //var fDate = txtSaleDateFrom.ToString("yyyyMMdd");
                param.Add("@MONTH", month, DbType.String, ParameterDirection.Input, 10);
                param.Add("@PAY_TYPE_AMT", payTypeCol);
                //param.Add("@FINISH_DATE", tDate, DbType.String, ParameterDirection.Input, 10);
                //param.Add("@REGI_SEQ", string.IsNullOrEmpty(SEQ) ? "00" : SEQ, DbType.String, ParameterDirection.Input, 10);


                (ClosingData, SpResult spResult) = await _sellingService.GetMonthlyData(param);

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

        private async Task LoadDayData(string day, string payTypeCol)
        {
            try

            {
                DynamicParameters param = new DynamicParameters();
                param.Add("@SHOP_CODE", DataLocals.AppConfig.PosInfo.StoreNo, DbType.String, ParameterDirection.Input, DataLocals.AppConfig.PosInfo.StoreNo.Length);
                param.Add("@POS_NO", Pos_No == "전체" ? "00" : Pos_No, DbType.String, ParameterDirection.Input, 10);

                var month = CurrentDate.ToString("yyyyMM");
                //var fDate = txtSaleDateFrom.ToString("yyyyMMdd");
                param.Add("@MONTH", month, DbType.String, ParameterDirection.Input, 10);
                param.Add("@DAY", day, DbType.String, ParameterDirection.Input, 10);
                param.Add("@PAY_TYPE_AMT", payTypeCol);

                //param.Add("@FINISH_DATE", tDate, DbType.String, ParameterDirection.Input, 10);
                //param.Add("@REGI_SEQ", string.IsNullOrEmpty(SEQ) ? "00" : SEQ, DbType.String, ParameterDirection.Input, 10);


                (ClosingData, SpResult spResult) = await _sellingService.GetDayData(param);

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

        private async Task LoadClassData(string rank, string payTypeCol)
        {
            try

            {
                DynamicParameters param = new DynamicParameters();
                param.Add("@SHOP_CODE", DataLocals.AppConfig.PosInfo.StoreNo, DbType.String, ParameterDirection.Input, DataLocals.AppConfig.PosInfo.StoreNo.Length);
                param.Add("@POS_NO", Pos_No == "전체" ? "00" : Pos_No, DbType.String, ParameterDirection.Input, 10);

                var month = CurrentDate.ToString("yyyyMM");
                //var fDate = txtSaleDateFrom.ToString("yyyyMMdd");
                param.Add("@MONTH", month, DbType.String, ParameterDirection.Input, 10);
                param.Add("@RANK", rank, DbType.String, ParameterDirection.Input, 10);
                param.Add("@PAY_TYPE_AMT", payTypeCol);

                //param.Add("@FINISH_DATE", tDate, DbType.String, ParameterDirection.Input, 10);
                //param.Add("@REGI_SEQ", string.IsNullOrEmpty(SEQ) ? "00" : SEQ, DbType.String, ParameterDirection.Input, 10);


                (ClosingData, SpResult spResult) = await _sellingService.GetClassData(param);

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

        private async Task LoadWeekData(string week, string payTypeCol,string dayofweek = "")
        {
            try

            {
                if (string.IsNullOrEmpty(dayofweek) || dayofweek == "N/A")
                {
                    ClosingData = new GRAPH_SELNG_STTUS();
                    return;
                }
                DynamicParameters param = new DynamicParameters();

                var fr = new System.Globalization.CultureInfo("fr-FR");
                var resultingDate = DateTime.ParseExact(dayofweek, "yyyyMMdd", System.Globalization.CultureInfo.CurrentCulture);
                var xx = Convert.ToDateTime(resultingDate);
                xx = xx.AddDays(6);
                var tDate = xx.ToString("yyyyMMdd");

                param.Add("@SHOP_CODE", DataLocals.AppConfig.PosInfo.StoreNo, DbType.String, ParameterDirection.Input, DataLocals.AppConfig.PosInfo.StoreNo.Length);
                param.Add("@POS_NO", Pos_No == "전체" ? "00" : Pos_No, DbType.String, ParameterDirection.Input, 10);

                var month = CurrentDate.ToString("yyyyMM");
                //var fDate = txtSaleDateFrom.ToString("yyyyMMdd");
                param.Add("@MONTH", month, DbType.String, ParameterDirection.Input, 10);
                param.Add("@WEEK", week, DbType.String, ParameterDirection.Input, 10);
                param.Add("@PAY_TYPE_AMT", payTypeCol);
                param.Add("@START_DATE", dayofweek, DbType.String, ParameterDirection.Input, 10);
                param.Add("@FINISH_DATE", tDate, DbType.String, ParameterDirection.Input, 10);

                //param.Add("@FINISH_DATE", tDate, DbType.String, ParameterDirection.Input, 10);
                //param.Add("@REGI_SEQ", string.IsNullOrEmpty(SEQ) ? "00" : SEQ, DbType.String, ParameterDirection.Input, 10);


                (ClosingData, SpResult spResult) = await _sellingService.GetWeekData(param);

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

        public void Close()
        {
            IoC.Get<MtSelngSttusGrphViewModel>().TryCloseAsync(true);
        }
    }
}

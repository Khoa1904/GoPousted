using Caliburn.Micro;
using Dapper;
using GoPOS.Common.ViewModels;
using GoPOS.Helpers;
using GoPOS.Helpers.CommandHelper;
using GoPOS.Models;
using GoPOS.Models.Common;
using GoPOS.OrderPay.Interface.View;
using GoPOS.SellingStatus.Interface;
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
using GoPOS.Common.Interface.View;
using static GoPOS.Function;
using GoPOS.Service;

/*
 매출현황 > 상품별 매출현황   

 */

namespace GoPOS.ViewModels
{

    public class GoodsSelngSttusViewModel : BaseItemViewModel, IHandle<CalendarEventArgs>, IHandle<SelectPosEventArgs>
    {
        private readonly ISellingStatusService sellingStatusService;
        private IGoodsSelngSttusView _view;

        List<SELLING_STATUS_INFO> MainList = new List<SELLING_STATUS_INFO>();
        List<SELLING_STATUS_INFO> DetailList = new List<SELLING_STATUS_INFO>();

        private IEventAggregator _eventAggregator;
        private bool isFDate;
        private Tuple<string, bool> headerSelect = new Tuple<string, bool>("", false);
        private Tuple<string, bool> subHeaderSelect = new Tuple<string, bool>("", false);

        public override bool SetIView(IView view)
        {
            _view = (IGoodsSelngSttusView)view;
            return base.SetIView(view);
        }

        public GoodsSelngSttusViewModel(IWindowManager windowManager, IEventAggregator eventAggregator, ISellingStatusService service) : base(windowManager, eventAggregator)
        {
            this.sellingStatusService = service;
            this._selectedItemMainList = new SELLING_STATUS_INFO();
            this._eventAggregator = eventAggregator;
            this._eventAggregator.SubscribeOnPublishedThread(this);

            this.FirstMenuVisibility = Visibility.Visible;
            this.SecondMenuVisibility = Visibility.Collapsed;

            //Init();
            this.ViewLoaded += GoodsSelngSttusViewModel_ViewLoaded;
        }

        private void GoodsSelngSttusViewModel_ViewLoaded(object? sender, EventArgs e)
        {
            Init();
        }

        private DateTime _txtSaleDate;
        public DateTime txtSaleDate
        {
            get { return _txtSaleDate; }
            set
            {
                if (_txtSaleDate != value)
                {
                    _txtSaleDate = value;
                    NotifyOfPropertyChange(() => txtSaleDate);
                }
            }
        }

        private DateTime _fDate;
        public DateTime FDate
        {
            get { return _fDate; }
            set
            {
                _fDate = value;
                if (value > TDate)
                {
                    DateTime dt = TDate;
                    TDate = value;
                    FDate = dt;
                }
                NotifyOfPropertyChange(nameof(FDate));
            }
        }

        private string _productName;
        public string ProductName
        {
            get { return _productName; }
            set
            {
                _productName = value;
                NotifyOfPropertyChange(nameof(ProductName));
            }
        }

        private DateTime _tDate;
        public DateTime TDate
        {
            get { return _tDate; }
            set
            {
                _tDate = value;
                if (value < FDate)
                {
                    DateTime dt = FDate;
                    FDate = value;
                    TDate = dt;
                }
                NotifyOfPropertyChange(nameof(TDate));
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
            //1.메인 리스트
            if (DataLocals.PosStatus == null)
            {
                DialogHelper.MessageBox("영업일자가 설정 되지 않습니다.");
                return;
            }
            FDate = TDate = Convert.ToDateTime(SalesDateString);
            PosNo = DataLocals.AppConfig.PosInfo.PosNo;

            await GetGoodsSelngSttusMainList();
        }

        private async Task GetGoodsSelngSttusMainList()
        {
            try
            {
                MainList.Clear();
                reset();
                DynamicParameters param = new DynamicParameters();
                param.Add("@SHOP_CODE", DataLocals.AppConfig.PosInfo.StoreNo, DbType.String, ParameterDirection.Input, DataLocals.AppConfig.PosInfo.StoreNo.Length);
                var tDate = TDate.ToString("yyyyMMdd");
                var fDate = FDate.ToString("yyyyMMdd");
                param.Add("@START_DATE", fDate, DbType.String, ParameterDirection.Input, 10);
                param.Add("@FINISH_DATE", tDate, DbType.String, ParameterDirection.Input, 10);
                param.Add("@POS_NO", PosNo == "전체" ? "00" : PosNo, DbType.String, ParameterDirection.Input, 10);


                (MainList, SpResult spResult) = await sellingStatusService.GetGoodsSelngSttusMainList(param);

                if (spResult.ResultType != EResultType.SUCCESS)
                {
                    return;
                }


                for (int i = 0; i < MainList.Count; i++)
                {

                    MainList[i].SCLASS_NAME = MainList[i].PRD_NAME;
                    MainList[i].TOT_SALE_QTY = Comma(MainList[i].TOT_SALE_QTY);
                    MainList[i].TOT_SALE_AMT = Comma(MainList[i].TOT_SALE_AMT);
                    MainList[i].TOT_DC_AMT = Comma(MainList[i].TOT_DC_AMT);
                    MainList[i].DCM_SALE_AMT = Comma(MainList[i].DCM_SALE_AMT);
                    MainList[i].VAT_AMT = Comma(MainList[i].VAT_AMT);
                    MainList[i].TOT_AMT = Comma(MainList[i].TOT_SALE_AMT);
                    //MainList[i].PAY_CNT = Comma(MainList[i].PAY_CNT);
                    //MainList[i].SALE_AMT = Comma(MainList[i].SALE_AMT);
                    MainList[i].OCC_RATE = MainList[i].OCC_RATE + "%";
                    MainList[i].PRD_CODE = MainList[i].PRD_CODE;
                    MainList[i].PRD_NAME = MainList[i].PRD_NAME;

                    SUM_OF_QTY += Decimal.Parse(MainList[i].TOT_SALE_QTY);
                    SUM_OF_SALE_AMT += Decimal.Parse(MainList[i].TOT_SALE_AMT);
                    SUM_OF_DC_AMT += Decimal.Parse(MainList[i].TOT_DC_AMT);
                    SUM_OF_NETSALE_AMT += Decimal.Parse(MainList[i].DCM_SALE_AMT);
                    SUM_OF_VAT_AMT += Decimal.Parse(MainList[i].VAT_AMT);
                    SUM_OF_SALEAMT_AFTERTAX += Decimal.Parse(MainList[i].TOT_AMT);
                    RATE += Decimal.Parse(MainList[i].OCC_RATE.Replace("%", "")) / 100;
                }
                RATE = RATE>1 ? 1 : RATE;

                IoC.Get<GoodsSelngSttusView>().lstViewMain.ItemsSource = MainList;

                //string jsonResult = JsonHelper.ModelToJson(resultList);

                //WindowHelper.InfoMessageRaw("조회결과", jsonResult.JsonPrettify());
                //LogHelper.Logger.Info("조회결과 : " + jsonResult);
            }
            catch (Exception ex)
            {
                LogHelper.Logger.Error("Main 리스트 가져오기 오류 : " + ex.Message);
            }
        }



        private void reset()
        {
            SUM_OF_QTY = 0;
            SUM_OF_SALE_AMT = 0;
            SUM_OF_DC_AMT = 0;
            SUM_OF_NETSALE_AMT = 0;
            SUM_OF_VAT_AMT = 0;
            SUM_OF_SALEAMT_AFTERTAX = 0;
            RATE = 0;
             SUM_OF_QTY_SGL = 0;
             SUM_OF_SALE_AMT_SGL = 0;
             SUM_OF_DC_AMT_SGL = 0;
             SUM_OF_NETSALE_AMT_SGL = 0;
             SUM_OF_VAT_AMT_SGL = 0;
             SUM_OF_SALEAMT_AFTERTAX_SGL = 0;
             RATE_SGL = 0;
        }

        private void softReset()
        {
            SUM_OF_QTY_SGL = 0;
            SUM_OF_SALE_AMT_SGL = 0;
            SUM_OF_DC_AMT_SGL = 0;
            SUM_OF_NETSALE_AMT_SGL = 0;
            SUM_OF_VAT_AMT_SGL = 0;
            SUM_OF_SALEAMT_AFTERTAX_SGL = 0;
            RATE_SGL = 0;
        }
        public async void OnSelectedItemChanged(SELLING_STATUS_INFO selectedItem)
        {
            // 선택된 항목 처리 로직 구현
            softReset();
            try
            {
                DetailList.Clear();
                ProductName = selectedItem == null ? " " : selectedItem.PRD_NAME;
                //IoC.Get<GoodsSelngSttusView>().lstViewSub.Items.Refresh();
                DynamicParameters param = new DynamicParameters();
                param.Add("@SHOP_CODE", DataLocals.AppConfig.PosInfo.StoreNo, DbType.String, ParameterDirection.Input, DataLocals.AppConfig.PosInfo.StoreNo.Length);
                var tDate = TDate.ToString("yyyyMMdd");
                var fDate = FDate.ToString("yyyyMMdd");
                param.Add("@START_DATE", fDate, DbType.String, ParameterDirection.Input, 10);
                param.Add("@FINISH_DATE", tDate, DbType.String, ParameterDirection.Input, 10);
                param.Add("@PRD_CODE", selectedItem == null ? "" : selectedItem.PRD_CODE, DbType.String, ParameterDirection.Input, 10);
                param.Add("@POS_NO", PosNo == "전체" ? "00" : PosNo, DbType.String, ParameterDirection.Input, 10);
                //param.Add("@SALE_DATE", DataLocals.PosStatus.SALE_DATE, DbType.String, ParameterDirection.Input, DataLocals.PosStatus.SALE_DATE.Length);

                (DetailList, SpResult spResult) = await sellingStatusService.GetGoodsSelngSttusDetailList(param);

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

                    DetailList[i].SALE_DATE = DetailList[i].SALE_DATE;
                    DetailList[i].TOT_SALE_QTY = Comma(DetailList[i].TOT_SALE_QTY);
                    DetailList[i].TOT_SALE_AMT = Comma(DetailList[i].TOT_SALE_AMT);
                    DetailList[i].TOT_DC_AMT = Comma(DetailList[i].TOT_DC_AMT);
                    DetailList[i].DCM_SALE_AMT = Comma(DetailList[i].DCM_SALE_AMT);
                    DetailList[i].VAT_AMT = Comma(DetailList[i].VAT_AMT);
                    DetailList[i].TOT_AMT = Comma(DetailList[i].TOT_SALE_AMT);
                    //   DetailList[i].PAY_CNT = Comma(DetailList[i].TOT_SALE_AMT);
                    //  DetailList[i].SALE_AMT = Comma(DetailList[i].TOT_SALE_AMT);
                    DetailList[i].OCC_RATE =  DetailList[i].OCC_RATE==""? "0%" : DetailList[i].OCC_RATE +"%" ;

                    SUM_OF_QTY_SGL              += Decimal.Parse(DetailList[i].TOT_SALE_QTY);
                    SUM_OF_SALE_AMT_SGL         += Decimal.Parse(DetailList[i].TOT_SALE_AMT);
                    SUM_OF_DC_AMT_SGL           += Decimal.Parse(DetailList[i].TOT_DC_AMT);
                    SUM_OF_NETSALE_AMT_SGL      += Decimal.Parse(DetailList[i].DCM_SALE_AMT);
                    SUM_OF_VAT_AMT_SGL          += Decimal.Parse(DetailList[i].VAT_AMT);
                    SUM_OF_SALEAMT_AFTERTAX_SGL += Decimal.Parse(DetailList[i].TOT_AMT);
                    RATE_SGL                    += Decimal.Parse(DetailList[i].OCC_RATE.Replace("%", "")) / 100;
                }

                IoC.Get<GoodsSelngSttusView>().lstViewSub.ItemsSource = DetailList;
                //string jsonResult = JsonHelper.ModelToJson(resultList);

                //WindowHelper.InfoMessageRaw("조회결과", jsonResult.JsonPrettify());
                //LogHelper.Logger.Info("조회결과 : " + jsonResult);
            }
            catch (Exception ex)
            {
                LogHelper.Logger.Error("Detail List 가져오기 오류 : " + ex.Message);
            }
        }


        public async void ButtonMenu(object sender, RoutedEventArgs e)
        {
            if (sender is not Button btn)
            {
                return;
            }
            await GetGoodsSelngSttusMainList();
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

        public ICommand HeaderCommand => new RelayCommand<GridViewColumnHeader>(HeaderCommandCtrl);

        private async void HeaderCommandCtrl(GridViewColumnHeader obj)
        {
            IoC.Get<GoodsSelngSttusView>().lstViewSub.ItemsSource = null;
            subHeaderSelect = new Tuple<string, bool>("", true);
       
            switch (obj.Tag)
            {
                case "className":
                    if (headerSelect.Item1 == "className" && headerSelect.Item2 == false)
                    {
                        MainList = MainList.OrderBy(x => x.SCLASS_NAME).Select((item, index) =>
                        {
                            item.NO = (index + 1).ToString();
                            return item;
                        }).ToList(); ;
                        IoC.Get<GoodsSelngSttusView>().lstViewMain.ItemsSource = MainList;
                        headerSelect = new Tuple<string, bool>("className", true);
    
                        break;
                    }
                    MainList = MainList.OrderByDescending(x => x.SCLASS_NAME).Select((item, index) =>
                    {
                        item.NO = (index + 1).ToString();
                        return item;
                    }).ToList();
                    IoC.Get<GoodsSelngSttusView>().lstViewMain.ItemsSource = MainList;
                    headerSelect = new Tuple<string, bool>("className", false);
                    for (int i = 0; i < MainList.Count(); i++)
                    {
                        MainList[i].NO = (i + 1).ToString();
                    }
                    break;
                case "totSaleQuantity":
                    if (headerSelect.Item1 == "totSaleQuantity" && headerSelect.Item2 == false)
                    {
                        MainList = MainList.OrderBy(x => ChangeToNumber(x.TOT_SALE_QTY)).Select((item, index) =>
                        {
                            item.NO = (index + 1).ToString();
                            return item;
                        }).ToList(); 
                        IoC.Get<GoodsSelngSttusView>().lstViewMain.ItemsSource = MainList;
                        headerSelect = new Tuple<string, bool>("totSaleQuantity", true);
                        break;
                    }
                    MainList = MainList.OrderByDescending(x => ChangeToNumber(x.TOT_SALE_QTY)).Select((item, index) => { item.NO = (index + 1).ToString(); return item; }).ToList();
                    IoC.Get<GoodsSelngSttusView>().lstViewMain.ItemsSource = MainList;
                    headerSelect = new Tuple<string, bool>("totSaleQuantity", false);
                    break;
                case "totSaleAMT":
                    if (headerSelect.Item1 == "totSaleAMT" && headerSelect.Item2 == false)
                    {
                        MainList = MainList.OrderBy(x => ChangeToNumber(x.TOT_SALE_AMT)).Select((item, index) =>
                        {
                            item.NO = (index + 1).ToString();
                            return item;
                        }).ToList();
                        IoC.Get<GoodsSelngSttusView>().lstViewMain.ItemsSource = MainList;
                        headerSelect = new Tuple<string, bool>("totSaleAMT", true);
                        break;
                    }
                    MainList = MainList.OrderByDescending(x => ChangeToNumber(x.TOT_SALE_AMT)).Select((item, index) => { item.NO = (index + 1).ToString(); return item; }).ToList();
                    IoC.Get<GoodsSelngSttusView>().lstViewMain.ItemsSource = MainList;
                    headerSelect = new Tuple<string, bool>("totSaleAMT", false);
                    break;
                case "totDCAMT":
                    if (headerSelect.Item1 == "totDCAMT" && headerSelect.Item2 == false)
                    {
                        MainList = MainList.OrderBy(x => ChangeToNumber(x.TOT_DC_AMT)).Select((item, index) => { item.NO = (index + 1).ToString(); return item; }).ToList();
                        IoC.Get<GoodsSelngSttusView>().lstViewMain.ItemsSource = MainList;
                        headerSelect = new Tuple<string, bool>("totDCAMT", true);
                        break;
                    }
                    MainList = MainList.OrderByDescending(x => ChangeToNumber(x.TOT_DC_AMT)).Select((item, index) => { item.NO = (index + 1).ToString(); return item; }).ToList();
                    IoC.Get<GoodsSelngSttusView>().lstViewMain.ItemsSource = MainList;
                    headerSelect = new Tuple<string, bool>("totDCAMT", false);
                    break;
                case "dcmSALEAMT":
                    if (headerSelect.Item1 == "dcmSALEAMT" && headerSelect.Item2 == false)
                    {
                        MainList = MainList.OrderBy(x => ChangeToNumber(x.DCM_SALE_AMT)).Select((item, index) =>
                        {
                            item.NO = (index + 1).ToString();
                            return item;
                        }).ToList();
                        IoC.Get<GoodsSelngSttusView>().lstViewMain.ItemsSource = MainList;
                        headerSelect = new Tuple<string, bool>("dcmSALEAMT", true);
                        break;
                    }
                    MainList = MainList.OrderByDescending(x => ChangeToNumber(x.DCM_SALE_AMT)).Select((item, index) => { item.NO = (index + 1).ToString(); return item; }).ToList();
                    IoC.Get<GoodsSelngSttusView>().lstViewMain.ItemsSource = MainList;
                    headerSelect = new Tuple<string, bool>("dcmSALEAMT", false);
                    break;
                case "vatAMT":
                    if (headerSelect.Item1 == "vatAMT" && headerSelect.Item2 == false)
                    {
                        MainList = MainList.OrderBy(x => ChangeToNumber(x.VAT_AMT)).Select((item, index) => { item.NO = (index + 1).ToString(); return item; }).ToList();
                        IoC.Get<GoodsSelngSttusView>().lstViewMain.ItemsSource = MainList;
                        headerSelect = new Tuple<string, bool>("vatAMT", true);
                        break;
                    }
                    MainList = MainList.OrderByDescending(x => ChangeToNumber(x.VAT_AMT)).Select((item, index) => { item.NO = (index + 1).ToString(); return item; }).ToList();
                    IoC.Get<GoodsSelngSttusView>().lstViewMain.ItemsSource = MainList;
                    headerSelect = new Tuple<string, bool>("vatAMT", false);
                    break;
                case "totAMT":
                    if (headerSelect.Item1 == "totAMT" && headerSelect.Item2 == false)
                    {
                        MainList = MainList.OrderBy(x => ChangeToNumber(x.TOT_AMT)).Select((item, index) =>
                        {
                            item.NO = (index + 1).ToString();
                            return item;
                        }).ToList();
                        IoC.Get<GoodsSelngSttusView>().lstViewMain.ItemsSource = MainList;
                        headerSelect = new Tuple<string, bool>("totAMT", true);
                        break;
                    }
                    MainList = MainList.OrderByDescending(x => ChangeToNumber(x.TOT_AMT)).Select((item, index) => { item.NO = (index + 1).ToString(); return item; }).ToList();
                    IoC.Get<GoodsSelngSttusView>().lstViewMain.ItemsSource = MainList;
                    headerSelect = new Tuple<string, bool>("totAMT", false);
                    break;
                case "occRATE":
                    if (headerSelect.Item1 == "occRATE" && headerSelect.Item2 == false)
                    {
                        MainList = MainList.OrderBy(x => ChangeToNumber(x.OCC_RATE[..^1].Replace(".", ""))).Select((item, index) =>
                        {
                            item.NO = (index + 1).ToString();
                            return item;
                        }).ToList();
                        IoC.Get<GoodsSelngSttusView>().lstViewMain.ItemsSource = MainList;
                        headerSelect = new Tuple<string, bool>("occRATE", true);
                        break;
                    }
                    MainList = MainList.OrderByDescending(x => ChangeToNumber(x.OCC_RATE[..^1].Replace(".", ""))).Select((item, index) => { item.NO = (index + 1).ToString(); return item; }).ToList();
                    IoC.Get<GoodsSelngSttusView>().lstViewMain.ItemsSource = MainList;
                    headerSelect = new Tuple<string, bool>("occRATE", false);
                    break;
            }

        }

        public ICommand SubHeaderCommand => new RelayCommand<GridViewColumnHeader>(SubHeaderCommandCtrl);

        private async void SubHeaderCommandCtrl(GridViewColumnHeader obj)
        {
            if (!DetailList.Any()) return;

            switch (obj.Tag)
            {
                case "saleDATE":
                    if (subHeaderSelect.Item1 == "saleDATE" && subHeaderSelect.Item2 == false)
                    {
                        DetailList = DetailList.OrderBy(x => ChangeToNumber(x.SALE_DATE)).Select((item, index) =>
                        {
                            item.NO = (index + 1).ToString();
                            return item;
                        }).ToList(); ;
                        IoC.Get<GoodsSelngSttusView>().lstViewSub.ItemsSource = DetailList;
                        subHeaderSelect = new Tuple<string, bool>("saleDATE", true);
                        break;
                    }
                    DetailList = DetailList.OrderByDescending(x => ChangeToNumber(x.SALE_DATE)).Select((item, index) =>
                    {
                        item.NO = (index + 1).ToString();
                        return item;
                    }).ToList(); ;
                    IoC.Get<GoodsSelngSttusView>().lstViewSub.ItemsSource = DetailList;
                    subHeaderSelect = new Tuple<string, bool>("saleDATE", false);
                    break;
                case "totSALEQTY":
                    if (subHeaderSelect.Item1 == "totSALEQTY" && subHeaderSelect.Item2 == false)
                    {
                        DetailList = DetailList.OrderBy(x => ChangeToNumber(x.TOT_SALE_QTY)).Select((item, index) =>
                        {
                            item.NO = (index + 1).ToString();
                            return item;
                        }).ToList(); ;
                        IoC.Get<GoodsSelngSttusView>().lstViewSub.ItemsSource = DetailList;
                        subHeaderSelect = new Tuple<string, bool>("totSALEQTY", true);
                        break;
                    }
                    DetailList = DetailList.OrderByDescending(x => ChangeToNumber(x.TOT_SALE_QTY)).Select((item, index) =>
                    {
                        item.NO = (index + 1).ToString();
                        return item;
                    }).ToList(); ;
                    IoC.Get<GoodsSelngSttusView>().lstViewSub.ItemsSource = DetailList;
                    subHeaderSelect = new Tuple<string, bool>("totSALEQTY", false);
                    break;
                case "totSALEAMT":
                    if (subHeaderSelect.Item1 == "totSaleAMT" && subHeaderSelect.Item2 == false)
                    {
                        DetailList = DetailList.OrderBy(x => ChangeToNumber(x.TOT_SALE_AMT)).Select((item, index) =>
                        {
                            item.NO = (index + 1).ToString();
                            return item;
                        }).ToList(); ;
                        IoC.Get<GoodsSelngSttusView>().lstViewSub.ItemsSource = DetailList;
                        subHeaderSelect = new Tuple<string, bool>("totSaleAMT", true);
                        break;
                    }
                    DetailList = DetailList.OrderByDescending(x => ChangeToNumber(x.TOT_SALE_AMT)).Select((item, index) =>
                    {
                        item.NO = (index + 1).ToString();
                        return item;
                    }).ToList(); ;
                    IoC.Get<GoodsSelngSttusView>().lstViewSub.ItemsSource = DetailList;
                    subHeaderSelect = new Tuple<string, bool>("totSaleAMT", false);
                    break;
                case "totDCAMT":
                    if (subHeaderSelect.Item1 == "totDCAMT" && subHeaderSelect.Item2 == false)
                    {
                        DetailList = DetailList.OrderBy(x => ChangeToNumber(x.TOT_DC_AMT)).Select((item, index) =>
                        {
                            item.NO = (index + 1).ToString();
                            return item;
                        }).ToList(); ;
                        IoC.Get<GoodsSelngSttusView>().lstViewSub.ItemsSource = DetailList;
                        subHeaderSelect = new Tuple<string, bool>("totDCAMT", true);
                        break;
                    }
                    DetailList = DetailList.OrderByDescending(x => ChangeToNumber(x.TOT_DC_AMT)).Select((item, index) =>
                    {
                        item.NO = (index + 1).ToString();
                        return item;
                    }).ToList(); ;
                    IoC.Get<GoodsSelngSttusView>().lstViewSub.ItemsSource = DetailList;
                    subHeaderSelect = new Tuple<string, bool>("totDCAMT", false);
                    break;
                case "dcmSALEAMT":
                    if (subHeaderSelect.Item1 == "dcmSALEAMT" && subHeaderSelect.Item2 == false)
                    {
                        DetailList = DetailList.OrderBy(x => ChangeToNumber(x.DCM_SALE_AMT)).Select((item, index) =>
                        {
                            item.NO = (index + 1).ToString();
                            return item;
                        }).ToList(); ;
                        IoC.Get<GoodsSelngSttusView>().lstViewSub.ItemsSource = DetailList;
                        subHeaderSelect = new Tuple<string, bool>("dcmSALEAMT", true);
                        break;
                    }
                    DetailList = DetailList.OrderByDescending(x => ChangeToNumber(x.DCM_SALE_AMT)).Select((item, index) =>
                    {
                        item.NO = (index + 1).ToString();
                        return item;
                    }).ToList(); ;
                    IoC.Get<GoodsSelngSttusView>().lstViewSub.ItemsSource = DetailList;
                    subHeaderSelect = new Tuple<string, bool>("dcmSALEAMT", false);
                    break;
                case "vatAMT":
                    if (subHeaderSelect.Item1 == "vatAMT" && subHeaderSelect.Item2 == false)
                    {
                        DetailList = DetailList.OrderBy(x => ChangeToNumber(x.VAT_AMT)).Select((item, index) =>
                        {
                            item.NO = (index + 1).ToString();
                            return item;
                        }).ToList(); ;
                        IoC.Get<GoodsSelngSttusView>().lstViewSub.ItemsSource = DetailList;
                        subHeaderSelect = new Tuple<string, bool>("vatAMT", true);
                        break;
                    }
                    DetailList = DetailList.OrderByDescending(x => ChangeToNumber(x.VAT_AMT)).Select((item, index) =>
                    {
                        item.NO = (index + 1).ToString();
                        return item;
                    }).ToList(); ;
                    IoC.Get<GoodsSelngSttusView>().lstViewSub.ItemsSource = DetailList;
                    subHeaderSelect = new Tuple<string, bool>("vatAMT", false);
                    break;
                case "totAMT":
                    if (subHeaderSelect.Item1 == "totAMT" && subHeaderSelect.Item2 == false)
                    {
                        DetailList = DetailList.OrderBy(x => ChangeToNumber(x.TOT_AMT)).Select((item, index) =>
                        {
                            item.NO = (index + 1).ToString();
                            return item;
                        }).ToList(); ;
                        IoC.Get<GoodsSelngSttusView>().lstViewSub.ItemsSource = DetailList;
                        subHeaderSelect = new Tuple<string, bool>("totAMT", true);
                        break;
                    }
                    DetailList = DetailList.OrderByDescending(x => ChangeToNumber(x.TOT_AMT)).Select((item, index) =>
                    {
                        item.NO = (index + 1).ToString();
                        return item;
                    }).ToList(); ;
                    IoC.Get<GoodsSelngSttusView>().lstViewSub.ItemsSource = DetailList;
                    subHeaderSelect = new Tuple<string, bool>("totAMT", false);
                    break;
                case "occRATE":
                    if (subHeaderSelect.Item1 == "occRATE" && subHeaderSelect.Item2 == false)
                    {
                        DetailList = DetailList.OrderBy(x => ChangeToNumber(x.OCC_RATE[..^1].Replace(".", ""))).Select((item, index) =>
                        {
                            item.NO = (index + 1).ToString();
                            return item;
                        }).ToList(); ;
                        IoC.Get<GoodsSelngSttusView>().lstViewSub.ItemsSource = DetailList;
                        subHeaderSelect = new Tuple<string, bool>("occRATE", true);
                        break;
                    }
                    DetailList = DetailList.OrderByDescending(x => ChangeToNumber(x.OCC_RATE[..^1].Replace(".", ""))).Select((item, index) =>
                    {
                        item.NO = (index + 1).ToString();
                        return item;
                    }).ToList(); ;
                    IoC.Get<GoodsSelngSttusView>().lstViewSub.ItemsSource = DetailList;
                    subHeaderSelect = new Tuple<string, bool>("occRATE", false);
                    break;
            }

        }


        public ICommand ButtonCommand => new RelayCommand<Button>(async (button) =>
        {
            if (button.Tag == null) return;

            switch (button.Tag.ToString())
            {
                case "btnSearchPos":
                    System.Windows.Point pointPos = _view.buttonPosition;                    
                    IoC.Get<SelectBoxViewModel>();
                    _eventAggregator?.PublishOnUIThreadAsync(new SelectboxEvent()
                    {
                        Type = "POS"
                    });
                    DialogHelper.ShowDialogWithCoords(typeof(SelectBoxViewModel), 50, 350, pointPos.X - 94, pointPos.Y + 30);
                    break;
                case "btnSaleDateF":
                    isFDate = true;
                    System.Windows.Point pointDateF = _view.buttonPosition;
                    //DialogHelper.ShowDialogWithCoords(typeof(CalendarViewModel), 450, 480, pointDateF.X - 450, pointDateF.Y + 30, FDate);
                    DialogHelper.ShowDialogWithCoords(typeof(CalendarViewModel), 450, 480, pointDateF.X, pointDateF.Y, FDate);
                    break;
                case "btnSaleDateT":
                    isFDate = false;
                    System.Windows.Point pointDateT = _view.buttonPosition;
                    //DialogHelper.ShowDialogWithCoords(typeof(CalendarViewModel), 450, 480, pointDateT.X - 450, pointDateT.Y + 30, TDate);
                    DialogHelper.ShowDialogWithCoords(typeof(CalendarViewModel), 450, 480, pointDateT.X, pointDateT.Y, TDate);
                    break;
                case "btnSearch":
                    GetClSelngSttusMainList();
                    break;
                default:
                    break;
            }
        });

        private async Task GetClSelngSttusMainList()
        {
            try
            {
                DynamicParameters param = new DynamicParameters();
                param.Add("@SHOP_CODE", DataLocals.AppConfig.PosInfo.StoreNo, DbType.String, ParameterDirection.Input, DataLocals.AppConfig.PosInfo.StoreNo.Length);
                //param.Add("@SALE_DATE", DataLocals.PosStatus.SALE_DATE, DbType.String, ParameterDirection.Input, DataLocals.PosStatus.SALE_DATE.Length);
                //param.Add("@ORDER_NO", DataLocals.PosStatus.ORDER_NO, DbType.String, ParameterDirection.Input, DataLocals.PosStatus.ORDER_NO.Length);

                (MainList, SpResult spResult) = await sellingStatusService.GetGoodsSelngSttusMainList(param);

                if (spResult.ResultType != EResultType.SUCCESS)
                {
                    //WindowHelper.InfoMessage("사용자 List 가져오기", spResult.ResultMessage);
                    return;
                }



                for (int i = 0; i < MainList.Count; i++)
                {


                    MainList[i].SCLASS_NAME = "분류명" + (i + 1).ToString();
                    MainList[i].TOT_SALE_QTY = Comma(MainList[i].TOT_SALE_QTY);
                    MainList[i].TOT_SALE_AMT = Comma(MainList[i].TOT_SALE_AMT);
                    MainList[i].TOT_DC_AMT = Comma(MainList[i].TOT_DC_AMT);
                    MainList[i].DCM_SALE_AMT = Comma(MainList[i].DCM_SALE_AMT);
                    MainList[i].VAT_AMT = Comma(MainList[i].VAT_AMT);
                    MainList[i].TOT_AMT = Comma(MainList[i].TOT_AMT);
                    MainList[i].PAY_CNT = Comma(MainList[i].PAY_CNT);
                    MainList[i].SALE_AMT = Comma(MainList[i].SALE_AMT);


                }

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

        public Task HandleAsync(CalendarEventArgs message, CancellationToken cancellationToken)
        {
            if (base.Views.Keys.Count() != 0)
            {
                if (message.EventType == "ExtButton")
                {
                    if (isFDate)
                        FDate = message.Date;
                    else
                        TDate = message.Date;

                    return GetGoodsSelngSttusMainList();

                }

                switch (message.EventType)
                {
                    case "btnConfirm":
                        if (isFDate)
                        {
                            TDate = FDate;
                        }
                        else
                        {
                            FDate = TDate;
                        }

                        return GetGoodsSelngSttusMainList();
                        break;
                    case "btnCancel7":
                        if (isFDate)
                        {
                            TDate = FDate.AddDays(7);
                        }
                        else
                        {
                            FDate = TDate.AddDays(-7);
                        }

                        return GetGoodsSelngSttusMainList();

                        break;
                    case "btnCancel15":
                        if (isFDate)
                        {
                            TDate = FDate.AddDays(15);
                        }
                        else
                        {
                            FDate = TDate.AddDays(-15);
                        }

                        return GetGoodsSelngSttusMainList();

                        break;
                    case "btnCancel1":
                        if (isFDate)
                        {
                            TDate = FDate.AddMonths(1);
                        }
                        else
                        {
                            FDate = TDate.AddMonths(-1);
                        }

                        return GetGoodsSelngSttusMainList();

                        break;
                    case "btnCancel3":
                        if (isFDate)
                        {
                            TDate = FDate.AddMonths(3);
                        }
                        else
                        {
                            FDate = TDate.AddMonths(-3);
                        }

                        return GetGoodsSelngSttusMainList();

                        break;
                }
            }

            return Task.CompletedTask;


        }

        public Task HandleAsync(SelectPosEventArgs message, CancellationToken cancellationToken)
        {
            if (base.Views.Keys.Count() != 0)
            {

                PosNo = message.PosNo;
                return GetGoodsSelngSttusMainList();
            }
            return Task.CompletedTask;
            // return Task.CompletedTask;

        }
        private decimal _SUM_OF_QTY = 0;
        private decimal _SUM_OF_SALE_AMT = 0;
        private decimal _SUM_OF_DC_AMT = 0;
        private decimal _SUM_OF_NETSALE_AMT = 0;
        private decimal _SUM_OF_VAT_AMT = 0;
        private decimal _SUM_OF_SALEAMT_AFTERTAX = 0;
        private decimal _RATE = 0;
        public decimal SUM_OF_QTY { get => _SUM_OF_QTY; set { _SUM_OF_QTY = value; NotifyOfPropertyChange(nameof(SUM_OF_QTY)); } }
        public decimal SUM_OF_SALE_AMT { get => _SUM_OF_SALE_AMT; set { _SUM_OF_SALE_AMT = value; NotifyOfPropertyChange(nameof(SUM_OF_SALE_AMT)); } }
        public decimal SUM_OF_DC_AMT { get => _SUM_OF_DC_AMT; set { _SUM_OF_DC_AMT = value; NotifyOfPropertyChange(nameof(SUM_OF_DC_AMT)); } }
        public decimal SUM_OF_NETSALE_AMT { get => _SUM_OF_NETSALE_AMT; set { _SUM_OF_NETSALE_AMT = value; NotifyOfPropertyChange(nameof(SUM_OF_NETSALE_AMT)); } }
        public decimal SUM_OF_VAT_AMT { get => _SUM_OF_VAT_AMT; set { _SUM_OF_VAT_AMT = value; NotifyOfPropertyChange(nameof(SUM_OF_VAT_AMT)); } }
        public decimal SUM_OF_SALEAMT_AFTERTAX { get => _SUM_OF_SALEAMT_AFTERTAX; set { _SUM_OF_SALEAMT_AFTERTAX = value; NotifyOfPropertyChange(nameof(SUM_OF_SALEAMT_AFTERTAX)); } }
        public decimal RATE { get => _RATE; set { _RATE = value; NotifyOfPropertyChange(nameof(RATE)); } }

        private decimal _SUM_OF_QTY_SGL = 0;
        private decimal _SUM_OF_SALE_AMT_SGL = 0;
        private decimal _SUM_OF_DC_AMT_SGL = 0;
        private decimal _SUM_OF_NETSALE_AMT_SGL = 0;
        private decimal _SUM_OF_VAT_AMT_SGL = 0;
        private decimal _SUM_OF_SALEAMT_AFTERTAX_SGL = 0;
        private decimal _RATE_SGL = 0;
        public decimal SUM_OF_QTY_SGL               { get => _SUM_OF_QTY_SGL;               set { _SUM_OF_QTY_SGL               = value; NotifyOfPropertyChange(nameof(SUM_OF_QTY_SGL)); } }
        public decimal SUM_OF_SALE_AMT_SGL          { get => _SUM_OF_SALE_AMT_SGL;          set { _SUM_OF_SALE_AMT_SGL          = value; NotifyOfPropertyChange(nameof(SUM_OF_SALE_AMT_SGL)); } }
        public decimal SUM_OF_DC_AMT_SGL            { get => _SUM_OF_DC_AMT_SGL;            set { _SUM_OF_DC_AMT_SGL            = value; NotifyOfPropertyChange(nameof(SUM_OF_DC_AMT_SGL)); } }
        public decimal SUM_OF_NETSALE_AMT_SGL       { get => _SUM_OF_NETSALE_AMT_SGL;       set { _SUM_OF_NETSALE_AMT_SGL       = value; NotifyOfPropertyChange(nameof(SUM_OF_NETSALE_AMT_SGL)); } }
        public decimal SUM_OF_VAT_AMT_SGL           { get => _SUM_OF_VAT_AMT_SGL;           set { _SUM_OF_VAT_AMT_SGL           = value; NotifyOfPropertyChange(nameof(SUM_OF_VAT_AMT_SGL)); } }
        public decimal SUM_OF_SALEAMT_AFTERTAX_SGL  { get => _SUM_OF_SALEAMT_AFTERTAX_SGL;  set { _SUM_OF_SALEAMT_AFTERTAX_SGL  = value; NotifyOfPropertyChange(nameof(SUM_OF_SALEAMT_AFTERTAX_SGL)); } }
        public decimal RATE_SGL                     { get => _RATE_SGL;                     set { _RATE_SGL                     = value; NotifyOfPropertyChange(nameof(RATE_SGL)); } }
    }
}
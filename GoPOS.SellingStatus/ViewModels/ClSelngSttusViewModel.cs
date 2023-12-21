﻿using Caliburn.Micro;
using Dapper;
using GoPOS.Common.Service;
using GoPOS.Common.ViewModels;
using GoPOS.Helpers;
using GoPOS.Helpers.CommandHelper;
using GoPOS.Models;
using GoPOS.Models.Common;
using GoPOS.SellingStatus.Interface;
using GoPOS.Service;
using GoPOS.Services;

using GoPOS.Views;
using GoShared.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Diagnostics.Metrics;
using System.Dynamic;
using System.Globalization;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using static GoPOS.Function;

/*
 매출현황 > 분류별 매출현황

 */

namespace GoPOS.ViewModels
{
    public class ClSelngSttusViewModel : OrderPayChildViewModel, IHandle<CalendarEventArgs>
    {
        private readonly ISellingStatusService sellingStatusService;
        private readonly IClSelngSttusView _view;

        List<SELLING_STATUS_INFO> MainList = new List<SELLING_STATUS_INFO>();
        List<SELLING_STATUS_INFO> DetailList = new List<SELLING_STATUS_INFO>();
        private bool isFDate;
        private IEventAggregator _eventAggregator;

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


        public ClSelngSttusViewModel(IWindowManager windowManager, IEventAggregator eventAggregator, ISellingStatusService service, IClSelngSttusView clSelngSttusView) : base(windowManager, eventAggregator)
        {
            this.sellingStatusService = service;
            this._view = clSelngSttusView;
            this._selectedItemMainList = new SELLING_STATUS_INFO();
            this._eventAggregator = eventAggregator;
            this._eventAggregator.SubscribeOnPublishedThread(this);

            this.FirstMenuVisibility = Visibility.Visible;
            this.SecondMenuVisibility = Visibility.Collapsed;
            this.ViewLoaded += ClSelngSttusViewModel_ViewLoaded;
            //Init(); 화면 오픈시 초기화수정 23.11.08
        }
        private void ClSelngSttusViewModel_ViewLoaded(object? sender, EventArgs e)
        {
            txtSaleDate = DateTime.ParseExact(DataLocals.PosStatus.SALE_DATE, "yyyyMMdd", CultureInfo.InvariantCulture);
            Init();
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

        private async void Init()
        {
            //1.메인 리스트
            await GetClSelngSttusMainList();
            FDate = TDate = DateTime.Now;
        }

        private async Task GetClSelngSttusMainList()
        {
            try
            {
                DynamicParameters param = new DynamicParameters();
                param.Add("@SHOP_CODE", DataLocals.AppConfig.PosInfo.StoreNo, DbType.String, ParameterDirection.Input, DataLocals.AppConfig.PosInfo.StoreNo.Length);
                //param.Add("@SALE_DATE", DataLocals.PosStatus.SALE_DATE, DbType.String, ParameterDirection.Input, DataLocals.PosStatus.SALE_DATE.Length);
                //param.Add("@ORDER_NO", DataLocals.PosStatus.ORDER_NO, DbType.String, ParameterDirection.Input, DataLocals.PosStatus.ORDER_NO.Length);

                (MainList, SpResult spResult) = await sellingStatusService.GetClSelngSttusMainList(param);

                if (spResult.ResultType != EResultType.SUCCESS)
                {
                    //WindowHelper.InfoMessage("사용자 List 가져오기", spResult.ResultMessage);
                    return;
                }


                //public string NORMAL_UPRC { get; set; } = string.Empty; //NUMERIC(10,2),
                //    public string SALE_QTY { get; set; } = string.Empty; //NUMERIC(12,2),
                //    public string DC_AMT { get; set; } = string.Empty; //NUMERIC(12,2),
                //    public string DCM_SALE_AMT { get; set; } = string.Empty; //NUMERIC(12,2),
                for (int i = 0; i < MainList.Count; i++)
                {
                    /*
                ClSelngSttusView      분류별 매출현황       -NO 분류명 총수량 총매출 총할인 실매출 부가세 합계 점유율  -NO 영업일자 총수량 총매출 총할인 실매출 부가세 합계 점유율
                GoodsSelngSttusView   상품별 매출현황       -NO 분류명 총수량 총매출 총할인 실매출 부가세 합계 점유율  -NO 분류명 총수량 총매출 총할인 실매출 부가세 합계 점유율
                PaymntSelngSttusView  결제유형별 매출현황   -NO 결제수단 결제건수 결제금액 점유율                      -NO 영업일자 결제건수 결제금액 점유율
                DscntSelngSttusView   할인유형별 매출현황   -NO 할인유형 건수 금액 점유율(건수)                        -NO 영업일자 건수 금액 점유율(건수) 
                MtSelngSttusView      월별 매출현황
                ExcclcSttusView       정산현황               
                TimeSelngSttusView    시간대별 매출현황     -NO 영업일자 총건수 총매출 총할인 실매출 부가세 합계 점유율 -NO 영업일자 총건수 총매출 총할인 실매출 부가세 합계 점유율
                    
            //NO
        public string NO { get; set; } = string.Empty;
            //분류명
        public string SCLASS_NAME { get; set; } = string.Empty;	
            //총수량
        public string TOT_SALE_QTY { get; set; } = string.Empty;	
            //총매출
        public string TOT_SALE_AMT { get; set; } = string.Empty;	
            //총할인
        public string TOT_DC_AMT { get; set; } = string.Empty;	
            //실매출
        public string DCM_SALE_AMT { get; set; } = string.Empty;	
            //부가세
        public string VAT_AMT { get; set; } = string.Empty;	
            //합계
        public string TOT_AMT { get; set; } = string.Empty;	
            //점유율
        public string OCC_RATE { get; set; } = string.Empty;	
            //영업일자
        public string SALE_DATE { get; set; } = string.Empty;	
            //결제수단
        public string PAYMENT_METHOD { get; set; } = string.Empty;	
            //결제건수
        public string PAY_CNT { get; set; } = string.Empty;	
            //결제금액
        public string SALE_AMT { get; set; } = string.Empty;	
            //할인유형
        public string DIS_CLS { get; set; } = string.Empty;	

                     */

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

        public async void OnSelectedItemChanged(SELLING_STATUS_INFO selectedItem)
        {
            // 선택된 항목 처리 로직 구현

            try
            {
                DetailList.Clear();
                //IoC.Get<ClSelngSttusView>().lstViewSub.Items.Refresh();

                DynamicParameters param = new DynamicParameters();
                param.Add("@SHOP_CODE", DataLocals.AppConfig.PosInfo.StoreNo, DbType.String, ParameterDirection.Input, DataLocals.AppConfig.PosInfo.StoreNo.Length);
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
                    /*
                ClSelngSttusView      분류별 매출현황       -NO 분류명 총수량 총매출 총할인 실매출 부가세 합계 점유율  -NO 영업일자 총수량 총매출 총할인 실매출 부가세 합계 점유율
                GoodsSelngSttusView   상품별 매출현황       -NO 분류명 총수량 총매출 총할인 실매출 부가세 합계 점유율  -NO 분류명 총수량 총매출 총할인 실매출 부가세 합계 점유율
                PaymntSelngSttusView  결제유형별 매출현황   -NO 결제수단 결제건수 결제금액 점유율                      -NO 영업일자 결제건수 결제금액 점유율
                DscntSelngSttusView   할인유형별 매출현황   -NO 할인유형 건수 금액 점유율(건수)                        -NO 영업일자 건수 금액 점유율(건수) 
                MtSelngSttusView      월별 매출현황
                ExcclcSttusView       정산현황               
                TimeSelngSttusView    시간대별 매출현황     -NO 영업일자 총건수 총매출 총할인 실매출 부가세 합계 점유율 -NO 영업일자 총건수 총매출 총할인 실매출 부가세 합계 점유율
                    
                    //NO
                public string NO { get; set; } = string.Empty;
                    //분류명
                public string SCLASS_NAME { get; set; } = string.Empty;	
                    //총수량
                public string TOT_SALE_QTY { get; set; } = string.Empty;	
                    //총매출
                public string TOT_SALE_AMT { get; set; } = string.Empty;	
                    //총할인
                public string TOT_DC_AMT { get; set; } = string.Empty;	
                    //실매출
                public string DCM_SALE_AMT { get; set; } = string.Empty;	
                    //부가세
                public string VAT_AMT { get; set; } = string.Empty;	
                    //합계
                public string TOT_AMT { get; set; } = string.Empty;	
                    //점유율
                public string OCC_RATE { get; set; } = string.Empty;	
                    //영업일자
                public string SALE_DATE { get; set; } = string.Empty;	
                    //결제수단
                public string PAYMENT_METHOD { get; set; } = string.Empty;	
                    //결제건수
                public string PAY_CNT { get; set; } = string.Empty;	
                    //결제금액
                public string SALE_AMT { get; set; } = string.Empty;	
                    //할인유형
                public string DIS_CLS { get; set; } = string.Empty;	

                     */

                    DetailList[i].TOT_SALE_QTY = Comma(DetailList[i].TOT_SALE_QTY);
                    DetailList[i].TOT_SALE_AMT = Comma(DetailList[i].TOT_SALE_AMT);
                    DetailList[i].TOT_DC_AMT = Comma(DetailList[i].TOT_DC_AMT);
                    DetailList[i].DCM_SALE_AMT = Comma(DetailList[i].DCM_SALE_AMT);
                    DetailList[i].VAT_AMT = Comma(DetailList[i].VAT_AMT);
                    DetailList[i].TOT_AMT = Comma(DetailList[i].TOT_AMT);
                    DetailList[i].PAY_CNT = Comma(DetailList[i].PAY_CNT);
                    DetailList[i].SALE_AMT = Comma(DetailList[i].SALE_AMT);

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

        public Task HandleAsync(CalendarEventArgs message, CancellationToken cancellationToken)
        {
            if (isFDate)
                FDate = message.Date;
            else
                TDate = message.Date;
            return Task.CompletedTask;
        }

        public ICommand ButtonCommand => new RelayCommand<Button>(async (button) =>
        {
            if (button.Tag == null) return;

            switch (button.Tag.ToString())
            {
                case "btnSearchPos":
                    System.Windows.Point pointPos = _view.buttonPosition;
                    DialogHelper.ShowDialogWithCoords(typeof(SelectBoxViewModel), 50, 350, pointPos.X - 50, pointPos.Y + 30);
                    break;
                case "btnSaleDateF":
                    isFDate = true;
                    System.Windows.Point pointDateF = _view.buttonPosition;
                    DialogHelper.ShowDialogWithCoords(typeof(CalendarViewModel), 450, 480, pointDateF.X - 450, pointDateF.Y + 30, txtSaleDate);
                    break;
                case "btnSaleDateT":
                    isFDate = false;
                    System.Windows.Point pointDateT = _view.buttonPosition;
                    DialogHelper.ShowDialogWithCoords(typeof(CalendarViewModel), 450, 480, pointDateT.X - 450, pointDateT.Y + 30, txtSaleDate);
                    break;
                case "btnSearch":
                    GetClSelngSttusMainList();
                    break;
                default:
                    break;
            }
        });
    }
}
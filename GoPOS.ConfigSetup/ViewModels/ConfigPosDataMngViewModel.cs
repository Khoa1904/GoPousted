using Caliburn.Micro;
using Dapper;
using GoPOS.Helpers;
using GoPOS.Helpers.CommandHelper;
using GoPOS.Models;
using GoPOS.Models.Common;
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


/// <summary>
/// 환경설정 > 포스데이터관리
/// 작성자 : 김형석
/// 작성일 : 20230319
/// </summary>
namespace GoPOS.ViewModels
{    
    public class ConfigPosDataMngViewModel : Screen
    {
        private readonly IConfigSetupService _configSetupService;

        List<POS_ORD_M> MainList = new List<POS_ORD_M>();
        List<POS_ORD_M> DetailList = new List<POS_ORD_M>();

        private IEventAggregator _eventAggregator;

        public ConfigPosDataMngViewModel(IEventAggregator eventAggregator, IConfigSetupService configSetupService)
        {
            this._configSetupService = configSetupService;
            this._selectedItemMainList = new POS_ORD_M();
            this._eventAggregator = eventAggregator;
            this._eventAggregator.SubscribeOnPublishedThread(this);

            this.FirstMenuVisibility = Visibility.Visible;
            this.SecondMenuVisibility = Visibility.Collapsed;

            Init();
        }


        private POS_ORD_M _selectedItemMainList;

        public POS_ORD_M SelectedItemMainList
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
            await GetPaymntSelngSttusMainList();
        }

        private async Task GetPaymntSelngSttusMainList()
        {
            try
            {
                DynamicParameters param = new DynamicParameters();
                param.Add("@SHOP_CODE", DataLocals.AppConfig.PosInfo.StoreNo, DbType.String, ParameterDirection.Input, DataLocals.AppConfig.PosInfo.StoreNo.Length);
                param.Add("@SALE_DATE", DataLocals.PosStatus.SALE_DATE, DbType.String, ParameterDirection.Input, DataLocals.PosStatus.SALE_DATE.Length);
                //param.Add("@ORDER_NO", ORDER_INFO.ORDER_NO, DbType.String, ParameterDirection.Input, ORDER_INFO.ORDER_NO.Length);

                (MainList, SpResult spResult) = await _configSetupService.ConfigPosDataMng_GetList1(param);

                if (spResult.ResultType != EResultType.SUCCESS)
                {
                    ////WindowHelper.InfoMessage("사용자 List 가져오기", spResult.ResultMessage);
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
                ConfigPosDataMngView  결제유형별 매출현황   -NO 결제수단 결제건수 결제금액 점유율                      -NO 영업일자 결제건수 결제금액 점유율
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

                    //MainList[i].TOT_SALE_QTY = Comma(MainList[i].TOT_SALE_QTY);
                    //MainList[i].TOT_SALE_AMT = Comma(MainList[i].TOT_SALE_AMT);
                    //MainList[i].TOT_DC_AMT = Comma(MainList[i].TOT_DC_AMT);
                    //MainList[i].DCM_SALE_AMT = Comma(MainList[i].DCM_SALE_AMT);
                    //MainList[i].VAT_AMT = Comma(MainList[i].VAT_AMT);
                    //MainList[i].TOT_AMT = Comma(MainList[i].TOT_AMT);
                    //MainList[i].PAY_CNT = Comma(MainList[i].PAY_CNT);
                    //MainList[i].SALE_AMT = Comma(MainList[i].SALE_AMT);


                }

                //IoC.Get<ConfigPosDataMngView>().lstViewMain.ItemsSource = MainList;

                //string jsonResult = JsonHelper.ModelToJson(resultList);

                //WindowHelper.InfoMessageRaw("조회결과", jsonResult.JsonPrettify());
                //LogHelper.Logger.Info("조회결과 : " + jsonResult);
            }
            catch (Exception ex)
            {
                DialogHelper.MessageBox("Main 리스트 가져오기 오류"+ex.Message);
                //WindowHelper.InfoMessage("Main 리스트 가져오기 오류", ex.Message);
                LogHelper.Logger.Error("Main 리스트 가져오기 오류 : " + ex.Message);
            }
        }

        public async void OnSelectedItemChanged(POS_ORD_M selectedItem)
        {
            // 선택된 항목 처리 로직 구현

            try
            {
               //DetailList.Clear();
               ////IoC.Get<ConfigPosDataMngView>().lstViewSub.Items.Refresh();
               //
               //DynamicParameters param = new DynamicParameters();
               //param.Add("@SHOP_CODE", DataLocals.AppConfig.PosInfo.StoreNo, DbType.String, ParameterDirection.Input, DataLocals.AppConfig.PosInfo.StoreNo.Length);
               //param.Add("@SALE_DATE", DataLocals.PosStatus.SALE_DATE, DbType.String, ParameterDirection.Input, DataLocals.PosStatus.SALE_DATE.Length);
               //
               //(DetailList, SpResult spResult) = await _configSetupService.ConfigPosDataMng_GetList1(param);
               //
               //if (spResult.ReusltType != EResultType.SUCCESS)
               //{
               //    ////WindowHelper.InfoMessage("사용자 List 가져오기", spResult.ResultMessage);
               //    return;
               //}
               //
               //
               ////public string NORMAL_UPRC { get; set; } = string.Empty; //NUMERIC(10,2),
               ////    public string SALE_QTY { get; set; } = string.Empty; //NUMERIC(12,2),
               ////    public string DC_AMT { get; set; } = string.Empty; //NUMERIC(12,2),
               ////    public string DCM_SALE_AMT { get; set; } = string.Empty; //NUMERIC(12,2),
               //for (int i = 0; i < DetailList.Count; i++)
               //{
               //    ///*
               //
               //    // */
               //
               //    //DetailList[i].TOT_SALE_QTY = Comma(DetailList[i].TOT_SALE_QTY);
               //    //DetailList[i].TOT_SALE_AMT = Comma(DetailList[i].TOT_SALE_AMT);
               //    //DetailList[i].TOT_DC_AMT = Comma(DetailList[i].TOT_DC_AMT);
               //    //DetailList[i].DCM_SALE_AMT = Comma(DetailList[i].DCM_SALE_AMT);
               //    //DetailList[i].VAT_AMT = Comma(DetailList[i].VAT_AMT);
               //    //DetailList[i].TOT_AMT = Comma(DetailList[i].TOT_AMT);
               //    //DetailList[i].PAY_CNT = Comma(DetailList[i].PAY_CNT);
               //    //DetailList[i].SALE_AMT = Comma(DetailList[i].SALE_AMT);
               //
               //}

                //IoC.Get<ConfigPosDataMngView>().lstViewSub.ItemsSource = DetailList;
                //string jsonResult = JsonHelperConfigPosDataMngViewModelToJson(resultList);

                //WindowHelper.InfoMessageRaw("조회결과", jsonResult.JsonPrettify());
                //LogHelper.Logger.Info("조회결과 : " + jsonResult);
            }
            catch (Exception ex)
            {
                DialogHelper.MessageBox("Detail List 가져오기 오류"+ex.Message);
                //WindowHelper.InfoMessage("Detail List 가져오기 오류", ex.Message);
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
           // ShellViewModel.menu_nm = viewModelName;

            //eventAggregator.PublishOnUIThreadAsync(IoC.Get<ShellViewModel>().LoggedInEmployee);
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
    }
}
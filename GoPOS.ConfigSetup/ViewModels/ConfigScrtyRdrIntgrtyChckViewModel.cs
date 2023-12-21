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
/// 환경설정 > 보안리더기 무결성 점검
/// 작성자 : 김형석 
/// 작성일 : 20230319 - 20230402
/// </summary>
namespace GoPOS.ViewModels
{
    public class ConfigScrtyRdrIntgrtyChckViewModel : Screen
    {
        private readonly IConfigSetupService _configSetupService;

        List<POS_LGSRC_T> MainList = new List<POS_LGSRC_T>();
        List<POS_LGSRC_T> DetailList = new List<POS_LGSRC_T>();

        private IEventAggregator _eventAggregator;

        public ConfigScrtyRdrIntgrtyChckViewModel(IEventAggregator eventAggregator, IConfigSetupService configSetupService)
        {
            this._configSetupService = configSetupService;
            this._selectedItemMainList = new CONFIG_SETUP_COM();
            this._eventAggregator = eventAggregator;
            this._eventAggregator.SubscribeOnPublishedThread(this);

            this.FirstMenuVisibility = Visibility.Visible;
            this.SecondMenuVisibility = Visibility.Collapsed;

            Init();
        }


        private CONFIG_SETUP_COM _selectedItemMainList;

        public CONFIG_SETUP_COM SelectedItemMainList
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
                param.Add("@POS_NO", DataLocals.AppConfig.PosInfo.PosNo, DbType.String, ParameterDirection.Input, DataLocals.AppConfig.PosInfo.PosNo.Length);

                (MainList, SpResult spResult) = await _configSetupService.ConfigScrtyRdrIntgrtyChck_GetList1(param);

                if (spResult.ResultType != EResultType.SUCCESS)
                {
                    ////WindowHelper.InfoMessage("사용자 List 가져오기", spResult.ResultMessage);
                    return;
                }

                for (int i = 0; i < MainList.Count; i++)
                {
                    MainList[i].NO = (i + 1).ToString();
                }

                //IoC.Get<ConfigScrtyRdrIntgrtyChckView>().lstViewMain.ItemsSource = MainList;
            }
            catch (Exception ex)
            {
                DialogHelper.MessageBox("Main 리스트 가져오기 오류"+ex.Message);
                //WindowHelper.InfoMessage("Main 리스트 가져오기 오류", ex.Message);
                LogHelper.Logger.Error("Main 리스트 가져오기 오류 : " + ex.Message);
            }
        }

        public async void OnSelectedItemChanged(CONFIG_SETUP_COM selectedItem)
        {
            // 선택된 항목 처리 로직 구현

            try
            {
                DetailList.Clear();
                //IoC.Get<ConfigScrtyRdrIntgrtyChckView>().lstViewSub.Items.Refresh();

                DynamicParameters param = new DynamicParameters();
                param.Add("@SHOP_CODE", DataLocals.AppConfig.PosInfo.StoreNo, DbType.String, ParameterDirection.Input, DataLocals.AppConfig.PosInfo.StoreNo.Length);
                param.Add("@SALE_DATE", DataLocals.PosStatus.SALE_DATE, DbType.String, ParameterDirection.Input, DataLocals.PosStatus.SALE_DATE.Length);

                (DetailList, SpResult spResult) = await _configSetupService.ConfigScrtyRdrIntgrtyChck_GetList1(param);

                if (spResult.ResultType != EResultType.SUCCESS)
                {
                    ////WindowHelper.InfoMessage("사용자 List 가져오기", spResult.ResultMessage);
                    return;
                }


                //public string NORMAL_UPRC { get; set; } = string.Empty; //NUMERIC(10,2),
                //    public string SALE_QTY { get; set; } = string.Empty; //NUMERIC(12,2),
                //    public string DC_AMT { get; set; } = string.Empty; //NUMERIC(12,2),
                //    public string DCM_SALE_AMT { get; set; } = string.Empty; //NUMERIC(12,2),
                //for (int i = 0; i < DetailList.Count; i++)
                {
        

                    //DetailList[i].TOT_SALE_QTY = Comma(DetailList[i].TOT_SALE_QTY);
                    //DetailList[i].TOT_SALE_AMT = Comma(DetailList[i].TOT_SALE_AMT);
                    //DetailList[i].TOT_DC_AMT = Comma(DetailList[i].TOT_DC_AMT);
                    //DetailList[i].DCM_SALE_AMT = Comma(DetailList[i].DCM_SALE_AMT);
                    //DetailList[i].VAT_AMT = Comma(DetailList[i].VAT_AMT);
                    //DetailList[i].TOT_AMT = Comma(DetailList[i].TOT_AMT);
                    //DetailList[i].PAY_CNT = Comma(DetailList[i].PAY_CNT);
                    //DetailList[i].SALE_AMT = Comma(DetailList[i].SALE_AMT);

                }

                //IoC.Get<ConfigScrtyRdrIntgrtyChckView>().lstViewSub.ItemsSource = DetailList;
                //string jsonResult = JsonHelper.ModelToJson(resultList);

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
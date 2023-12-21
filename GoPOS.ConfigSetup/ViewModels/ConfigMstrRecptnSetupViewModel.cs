using Caliburn.Micro;
using Dapper;
using GoPOS.Helpers;
using GoPOS.Helpers.CommandHelper;
using GoPOS.Models;
using GoPOS.Models.Common;
using GoPOS.Services;

using GoPOS.Views;
using GoShared.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Globalization;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using static GoPOS.Function;

/// <summary>
/// 환경설정 > 마스터수신설정
/// 작성자 : 김형석
/// 작성일 : 20230319
/// </summary>
namespace GoPOS.ViewModels
{
    public class ConfigMstrRecptnSetupViewModel : Screen
    {
        private readonly IConfigSetupService _configSetupService;

        List<CMM_MASTER_RECV> MainList = new List<CMM_MASTER_RECV>();

        private IEventAggregator _eventAggregator;

        public ConfigMstrRecptnSetupViewModel(IEventAggregator eventAggregator, IConfigSetupService configSetupService)
        {
            this._configSetupService = configSetupService;
            this._selectedItemMainList = new CMM_MASTER_RECV();
            this._eventAggregator = eventAggregator;
            this._eventAggregator.SubscribeOnPublishedThread(this);

            this.FirstMenuVisibility = Visibility.Visible;
            this.SecondMenuVisibility = Visibility.Collapsed;

            Init();
        }


        private CMM_MASTER_RECV _selectedItemMainList;

        public CMM_MASTER_RECV SelectedItemMainList
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
                //param.Add("@SALE_DATE", DataLocals.PosStatus.SALE_DATE, DbType.String, ParameterDirection.Input, DataLocals.PosStatus.SALE_DATE.Length);
                //param.Add("@ORDER_NO", ORDER_INFO.ORDER_NO, DbType.String, ParameterDirection.Input, ORDER_INFO.ORDER_NO.Length);

                (MainList, SpResult spResult) = await _configSetupService.ConfigMstrRecptnSetup_GetList1(param);

                if (spResult.ResultType != EResultType.SUCCESS)
                {
                    ////WindowHelper.InfoMessage("사용자 List 가져오기", spResult.ResultMessage);
                    return;
                }
                //RECV_ID
                //REQT_RM
                //LAST_SEQ
                //UPDATE_DT
                for (int i = 0; i < MainList.Count; i++)
                {
                    MainList[i].NO = (i + 1).ToString();
                    MainList[i].LAST_SEQ = Comma(MainList[i].LAST_SEQ);
                    MainList[i].RECV_ID = Comma(MainList[i].RECV_ID) + " 건수"; //데이터건수 0 건
                    //MainList[i].UPDATE_DT = (MainList[i].UPDATE_DT).ToString("YYYY-MM-DD HH:MM:SS")  // 시간 YYYY-MM-DD HH:MM:SS

                    DateTime Dresult;
                    if (DateTime.TryParseExact(MainList[i].UPDATE_DT, "yyyyMMddHHmmss", null, DateTimeStyles.None, out Dresult))
                        MainList[i].UPDATE_DT = Dresult.ToString("yyyy-MM-dd HH:mm:ss"); // 캐스팅 성공
                    else // 캐스팅 실패
                        MainList[i].UPDATE_DT = "";
                }
                //IoC.Get<ConfigMstrRecptnSetupView>().lstViewMain.ItemsSource = MainList;
            }
            catch (Exception ex)
            {
                DialogHelper.MessageBox("Main 리스트 가져오기 오류" + ex.Message);
                //WindowHelper.InfoMessage("Main 리스트 가져오기 오류", ex.Message);
                LogHelper.Logger.Error("Main 리스트 가져오기 오류 : " + ex.Message);
            }
        }

        public async void OnSelectedItemChanged(CMM_MASTER_RECV selectedItem)
        {
            // 선택된 항목 처리 로직 구현

            try
            {
                //DetailList.Clear();
                //IoC.Get<ConfigMstrRecptnSetupView>().lstViewSub.Items.Refresh();
                //
                //DynamicParameters param = new DynamicParameters();
                //param.Add("@SHOP_CODE", ORDER_INFO.SHOP_CODE, DbType.String, ParameterDirection.Input, ORDER_INFO.SHOP_CODE.Length);
                //param.Add("@SALE_DATE", ORDER_INFO.SALE_DATE, DbType.String, ParameterDirection.Input, ORDER_INFO.SALE_DATE.Length);
                //
                //(DetailList, SpResult spResult) = await _configSetupService.GetClSelngSttusDetailList(param);
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
                //    DetailList[i].TOT_SALE_QTY = Comma(DetailList[i].TOT_SALE_QTY);
                //    DetailList[i].TOT_SALE_AMT = Comma(DetailList[i].TOT_SALE_AMT);
                //    DetailList[i].TOT_DC_AMT = Comma(DetailList[i].TOT_DC_AMT);
                //    DetailList[i].DCM_SALE_AMT = Comma(DetailList[i].DCM_SALE_AMT);
                //    DetailList[i].VAT_AMT = Comma(DetailList[i].VAT_AMT);
                //    DetailList[i].TOT_AMT = Comma(DetailList[i].TOT_AMT);
                //    DetailList[i].PAY_CNT = Comma(DetailList[i].PAY_CNT);
                //    DetailList[i].SALE_AMT = Comma(DetailList[i].SALE_AMT);
                //
                //}
                //
                //IoC.Get<ConfigMstrRecptnSetupView>().lstViewSub.ItemsSource = DetailList;
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

            string sValue = (string)btn.Tag;

            if (sValue == "전체초기화")
            {
                //if (IoC.Get<ConfigMstrRecptnSetupView>().lstViewMain.Items.Count > 0)
                //{
                //    for (int i = 0; i < IoC.Get<ConfigMstrRecptnSetupView>().lstViewMain.Items.Count; i++)
                //    {
                //        MainList[i].LAST_SEQ = "0";
                //        IoC.Get<ConfigMstrRecptnSetupView>().lstViewMain.ItemsSource = MainList;
                //        IoC.Get<ConfigMstrRecptnSetupView>().lstViewMain.Items.Refresh();
                //    }
                //}

                //DialogHelper.Show(sValue);
            }
            else if (sValue == "선택초기화")
            {
                //if (IoC.Get<ConfigMstrRecptnSetupView>().lstViewMain.SelectedIndex != -1)
                //{
                //    int iIndex = IoC.Get<ConfigMstrRecptnSetupView>().lstViewMain.SelectedIndex;
                //    MainList[iIndex].LAST_SEQ = "0";
                //    IoC.Get<ConfigMstrRecptnSetupView>().lstViewMain.ItemsSource = MainList;
                //    IoC.Get<ConfigMstrRecptnSetupView>().lstViewMain.Items.Refresh();
                //}

                //DialogHelper.Show(sValue);
            }
            else if (sValue == "저장")
            {

                try
                {
                    DynamicParameters param = new DynamicParameters();
                    param.Add("@SHOP_CODE", DataLocals.AppConfig.PosInfo.StoreNo, DbType.String, ParameterDirection.Input, DataLocals.AppConfig.PosInfo.StoreNo.Length);
                    //param.Add("@POS_NO", IoC.Get<ConfigPosConfigSetupView>().txtPosNo.Text, DbType.String, ParameterDirection.Input, IoC.Get<ConfigPosConfigSetupView>().txtPosNo.Text.Length);

                    //param.Add("@LAST_SEQ", LAST_SEQ, DbType.String, ParameterDirection.Input, ORDER_INFO.SALE_DATE.Length);
                    //param.Add("@RECV_ID", RECV_ID, DbType.String, ParameterDirection.Input, ORDER_INFO.SALE_DATE.Length);
                    param.Add("@MainList", MainList);

                    CMM_MASTER_RECV result = new CMM_MASTER_RECV();

                    (result, SpResult spResult) = await _configSetupService.SP_CMM_MASTER_RECV_U(param);

                    if (spResult.ResultType != EResultType.SUCCESS)
                    {
                        //W indowHelper.InfoMessage("사용자 List 가져오기", spResult.ResultMessage);
                        return;
                    }

                    DialogHelper.MessageBox("저장 성공");
                    Init();
                }
                catch (Exception ex)
                {
                    LogHelper.Logger.Error("환경정보 UPDATE 오류 : " + ex.Message);
                }

            }
            else
            {
                DialogHelper.MessageBox(sValue);
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
            ////IoC.Get<OrderPayMainView>().DockAllPop.Visibility = Visibility.Hidden;
        }

        public void ChangeView(string viewModelName)
        {
            //ShellViewModel.menu_nm = viewModelName;

            ////eventAggregator.PublishOnUIThreadAsync(IoC.Get<ShellViewModel>().LoggedInEmployee);
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
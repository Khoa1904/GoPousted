using Caliburn.Micro;
using Dapper;
using GoPOS.Helpers;
using GoPOS.Helpers.CommandHelper;
using GoPOS.Models;
using GoPOS.Models._0_Common;
using GoPOS.Models.Common;
using GoPOS.Services;

using GoPOS.Views;
using GoShared.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using static GoPOS.Function;

/*
 환경설정 > 포스 환경설정 
 작성자 : 김형석
 작성일 : 20230402
 */

namespace GoPOS.ViewModels
{

    public class ConfigPosConfigSetupViewModel : Screen
    {
        private readonly IConfigPosConfigSetupService _configPosConfigSetupService;

        List<ENV_CONFIG> MainList = new List<ENV_CONFIG>();
        List<ENV_CONFIG> SublList = new List<ENV_CONFIG>();

        private IEventAggregator _eventAggregator;

        public ConfigPosConfigSetupViewModel(IEventAggregator eventAggregator, IConfigPosConfigSetupService configPosConfigSetupService)
        {
            this._configPosConfigSetupService = configPosConfigSetupService;
            this._selectedItemMainList = new ENV_CONFIG();
            this._eventAggregator = eventAggregator;
            this._eventAggregator.SubscribeOnPublishedThread(this);

            this.FirstMenuVisibility = Visibility.Visible;
            this.SecondMenuVisibility = Visibility.Collapsed;

            Init();
        }

        private ENV_CONFIG _selectedItemMainList;

        public ENV_CONFIG SelectedItemMainList
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

            //IoC.Get<ConfigPosConfigSetupView>().txtPosNo.Text = "01";
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
                //param.Add("@POS_NO", IoC.Get<ConfigPosConfigSetupView>().txtPosNo.Text , DbType.String, ParameterDirection.Input, IoC.Get<ConfigPosConfigSetupView>().txtPosNo.Text.Length);
                //param.Add("@ORDER_NO", ORDER_INFO.ORDER_NO, DbType.String, ParameterDirection.Input, ORDER_INFO.ORDER_NO.Length);

                (MainList, SpResult spResult) = await _configPosConfigSetupService.GetList1(param);

                if (spResult.ResultType != EResultType.SUCCESS)
                {
                    ////WindowHelper.InfoMessage("사용자 List 가져오기", spResult.ResultMessage);
                    return;
                }

                for (int i = 0; i < MainList.Count; i++)
                {
                    MainList[i].NO = (i + 1).ToString();
                    //MainList[i].ENV_GROUP_NAME
                    //MainList[i].ENV_SET_NAME
                    //MainList[i].ENV_VALUE_NAME
                    //MainList[i].ENV_SET_VALUE 
                }

                //IoC.Get<ConfigPosConfigSetupView>().lstViewMain.ItemsSource = MainList;

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

        public async void OnSelectedItemChanged(ENV_CONFIG selectedItem)
        {
            // 선택된 항목 처리 로직 구현

            if (selectedItem == null)
                return;

            try
            {
                SublList.Clear();
                //IoC.Get<ConfigPosConfigSetupView>().lstViewSub.Items.Refresh();

                List<ENV_CONFIG> tmpSublList = new List<ENV_CONFIG>();

                DynamicParameters param = new DynamicParameters();
                param.Add("@SHOP_CODE", DataLocals.AppConfig.PosInfo.StoreNo, DbType.String, ParameterDirection.Input, DataLocals.AppConfig.PosInfo.StoreNo.Length);
                param.Add("@SALE_DATE", DataLocals.PosStatus.SALE_DATE, DbType.String, ParameterDirection.Input, DataLocals.PosStatus.SALE_DATE.Length);
                //param.Add("@POS_NO", ORDER_INFO.POS_NO, DbType.String, ParameterDirection.Input, ORDER_INFO.SALE_DATE.Length);
                //param.Add("@POS_NO", IoC.Get<ConfigPosConfigSetupView>().txtPosNo.Text, DbType.String, ParameterDirection.Input, IoC.Get<ConfigPosConfigSetupView>().txtPosNo.Text.Length);                
                param.Add("@ENV_SET_CODE", selectedItem.ENV_SET_CODE, DbType.String, ParameterDirection.Input, selectedItem.ENV_SET_CODE.Length);

                //selectedItem.ENV_SET_CODE;
                (tmpSublList, SpResult spResult) = await _configPosConfigSetupService.GetList2(param);

                if (spResult.ResultType != EResultType.SUCCESS)
                {
                    //W indowHelper.InfoMessage("사용자 List 가져오기", spResult.ResultMessage);
                    return;
                }

                SublList = tmpSublList;

                /*
                ENV_GROUP_NAME
                ENV_SET_NAME
                ENV_VALUE_NAME
                ENV_SET_VALUE  
                 
                 */
                //IoC.Get<ConfigPosConfigSetupView>().txtConfigSetup.Text = selectedItem.ENV_SET_NAME;           // txtConfigSetup   환결설정                
                //IoC.Get<ConfigPosConfigSetupView>().txtConfigSetupContent.Text = selectedItem.ENV_VALUE_NAME;    // txtConfigSetupContent  환경설정 내역
                //IoC.Get<ConfigPosConfigSetupView>().txtChgConfigSetupContent.Text = ""; // txtChgConfigSetupContent 번경설정 내역

                for (int i = 0; i < SublList.Count; i++)
                {
                    //if(selectedItem.NO == i.ToString())
                    {
                        SublList[i].NO = (i+1).ToString();
                        //SublList.Add(tmpSublList[i]);
                        //SublList[i].ENV_VALUE_NAME = SublList[i].ENV_VALUE_NAME;
                        
                        //ENV_SET_CODE
                    }
                }

                //IoC.Get<ConfigPosConfigSetupView>().lstViewSub.ItemsSource = SublList;
                //IoC.Get<ConfigPosConfigSetupView>().lstViewSub.Items.Refresh();
            }
            catch (Exception ex)
            {
                DialogHelper.MessageBox("lstViewSub List 가져오기 오류"+ex.Message);
                //WindowHelper.InfoMessage("lstViewSub List 가져오기 오류", ex.Message);
                LogHelper.Logger.Error("lstViewSub List 가져오기 오류 : " + ex.Message);
            }
        }


       

        public async void ButtonMenu(object sender, RoutedEventArgs e)
        {
            if (sender is not Button btn)
            {
                return;
            }

            string sValue = (string)btn.Tag;

            //CBE

            if (sValue == "btnSave")
            {
                ////IoC.Get<OrderPayMainView>().DockAllPop.Visibility = Visibility.Hidden;

                //@ENV_SET_VALUE
                //@SHOP_CODE
                //@POS_NO
                //@ENV_SET_CODE";
                //if (IoC.Get<ConfigPosConfigSetupView>().txtPosNo.Text.Trim() == string.Empty)
                //{
                //    DialogHelper.Show("POS No를 입력해주세요 ex)01 ");
                //    return;
                //}

                try
                {
                    string strENV_SET_VALUE = string.Empty;// IoC.Get<ConfigPosConfigSetupView>().txtENV_SET_VALUE.Text;// txtConfigSetupContent  환경설정 내역
                    string strENV_SET_CODE = string.Empty;//IoC.Get<ConfigPosConfigSetupView>().txtENV_SET_CODE.Text; // txtChgConfigSetupContent 번경설정 내역

                    DynamicParameters param = new DynamicParameters();
                    param.Add("@SHOP_CODE", DataLocals.AppConfig.PosInfo.StoreNo, DbType.String, ParameterDirection.Input, DataLocals.AppConfig.PosInfo.StoreNo.Length);
                    //param.Add("@POS_NO", IoC.Get<ConfigPosConfigSetupView>().txtPosNo.Text, DbType.String, ParameterDirection.Input, IoC.Get<ConfigPosConfigSetupView>().txtPosNo.Text.Length);
                    //param.Add("@POS_NO", ORDER_INFO.POS_NO, DbType.String, ParameterDirection.Input, ORDER_INFO.SALE_DATE.Length);
                    
                    param.Add("@ENV_SET_VALUE", strENV_SET_VALUE, DbType.String, ParameterDirection.Input, strENV_SET_VALUE.Length);
                    param.Add("@ENV_SET_CODE", strENV_SET_CODE, DbType.String, ParameterDirection.Input, strENV_SET_CODE.Length);


                    //POS_ORD_M orderPayInfo = new POS_ORD_M();
                    //(orderPayInfo, SpResult spResult) = await orderPayCashService.InsertProdOrderHdr(param);
                    SHOP_POS_ENV result = new SHOP_POS_ENV();
                    (result, SpResult spResult) = await _configPosConfigSetupService.SP_SCD_ENVPS_U(param);

                    if (spResult.ResultType != EResultType.SUCCESS)
                    {
                        //W indowHelper.InfoMessage("사용자 List 가져오기", spResult.ResultMessage);
                        return;
                    }

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


        private ENV_CONFIG _selectedItemSubList;
        public ENV_CONFIG SelectedItemSubList
        {
            get { return _selectedItemSubList; }
            set
            {
                _selectedItemSubList = value;
                NotifyOfPropertyChange(() => SelectedItemSubList);
                // 선택 이벤트 처리 메서드 호출
                lstViewSub_SelectionChanged(value);
            }
        }
        private void lstViewSub_SelectionChanged(ENV_CONFIG selectedItem)
        {
            //if (IoC.Get<ConfigPosConfigSetupView>().lstViewSub.SelectedIndex > -1)
            //{
            //    IoC.Get<ConfigPosConfigSetupView>().txtChgConfigSetupContent.Text = selectedItem.ENV_VALUE_NAME; // txtChgConfigSetupContent 번경설정 내역
            //    IoC.Get<ConfigPosConfigSetupView>().txtENV_SET_VALUE.Text = selectedItem.ENV_VALUE_CODE;
            //    IoC.Get<ConfigPosConfigSetupView>().txtENV_SET_CODE.Text = selectedItem.ENV_SET_CODE;
            //    // 선택한 항목의 값을 가져옴
            //    //var selectedValue = ((List<ENV_CONFIG>)IoC.Get<ConfigPosConfigSetupView>().lstViewSub.SelectedItem)[0].NO;
            //    // 이벤트 처리
            //    // ...
            //}
        }
    }
}
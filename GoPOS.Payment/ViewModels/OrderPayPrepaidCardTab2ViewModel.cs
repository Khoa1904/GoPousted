using Caliburn.Micro;
using Dapper;
using GoPOS.Helpers;
using GoPOS.Helpers.CommandHelper;
using GoPOS.Models;
using GoPOS.Services;

using GoPOS.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Globalization;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

/*
 주문 > 확장메뉴 > 선불카드tab2

 */
/// <summary>
/// 화면명 : 출력관리   선불카드tab2
/// 작성자 : 김형석
/// 작성일 : 20230312
/// </summary>
namespace GoPOS.ViewModels
{

    public class OrderPayPrepaidCardTab2ViewModel : Screen
    {
        private readonly IOrderPayPrepaidCardTab2Service _orderPayPrepaidCardTab2Service;
        private IEventAggregator _eventAggregator;

        public OrderPayPrepaidCardTab2ViewModel(IEventAggregator eventAggregator, IOrderPayPrepaidCardTab2Service orderPayPrepaidCardTab2Service)
        {
            this._orderPayPrepaidCardTab2Service = orderPayPrepaidCardTab2Service;
            this._eventAggregator = eventAggregator;
            this._eventAggregator.SubscribeOnPublishedThread(this);

            Init();
        }

        private async void Init()
        {
            SetData();
            await Task.Delay(100);
        }


        public void ButtonMenu(object sender, RoutedEventArgs e)
        {
            if (sender is not Button btn)
            {
                return;
            }

            string sValue = (string)btn.Tag;

            //카드번호             
            //잔액
            //충전한도액 
            //유효기간
            //결제대상 금액 

            string strCard_No = IoC.Get<OrderPayPrepaidCardTab2View>().txtCard_No.Text;
            string strRem_Amt = IoC.Get<OrderPayPrepaidCardTab2View>().txtRem_Amt.Text;
            string strLim_Amt = IoC.Get<OrderPayPrepaidCardTab2View>().txtLim_Amt.Text;
            string strVal_TERM = IoC.Get<OrderPayPrepaidCardTab2View>().txtVal_Term.Text;
            string strPay_Amt = IoC.Get<OrderPayPrepaidCardTab2View>().txtPay_Amt.Text;

            //_orderPayPrepaidCardTab1Service.GetList();

            if (sValue == "btnTab1")
            {
                //DialogHelper.Show("btnTab1");
                //IoC.Get<OrderPayMainViewModel>().ActiveForm("OrderPayPrepaidCardTab1ViewModel");
            }
            else if (sValue == "btnTab2")
            {
                ///DialogHelper.Show("btnTab2");
                //IoC.Get<OrderPayMainViewModel>().ActiveForm("OrderPayPrepaidCardTab2ViewModel");
            }
            else if (sValue == "btnSearch") // 조회
            {
                //DialogHelper.Show("조회");
                if (strCard_No.Length > 0)
                {
                    IoC.Get<OrderPayPrepaidCardTab2View>().txtRem_Amt.Text = "50,500"; // 잔액
                    IoC.Get<OrderPayPrepaidCardTab2View>().txtRem_Amt.Text = "100,000";   // gkseh
                    IoC.Get<OrderPayPrepaidCardTab2View>().txtVal_Term.Text = "6";
                }
                else
                    DialogHelper.MessageBox("카드번호를 입력해주세요");
            }
            else if (sValue == "btnChaList") // 충전내역
            {
                DialogHelper.MessageBox("충전내역");
            }
        }

        public void ButtonClose(object sender, RoutedEventArgs e)
        {
            if (sender is not Button btn)
            {
                return;
            }

            string sValue = (string)btn.Tag;

            //IoC.Get<OrderPayMainView>().DockAllPop.Visibility = Visibility.Hidden;
            //IoC.Get<OrderPayMainViewModel>().ActiveForm("OrderPayRightViewModel");
        }

        /// <summary>
        /// 처음 화면 들어갈때 좌측 결젱정보에서 값 가져오는 부분
        /// </summary>
        public void SetData()
        {
        }
    }
}
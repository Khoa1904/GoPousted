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
 주문 > 확장메뉴 > 선불카드tab1

 */
/// <summary>
/// 화면명 : 출력관리   선불카드tab1
/// 작성자 : 김형석
/// 작성일 : 20230312
/// </summary>
namespace GoPOS.ViewModels
{

    public class OrderPayPrepaidCardTab1ViewModel : Screen
    {
        private readonly IOrderPayPrepaidCardTab1Service _orderPayPrepaidCardTab1Service;
        private IEventAggregator _eventAggregator;

        public OrderPayPrepaidCardTab1ViewModel(IEventAggregator eventAggregator, IOrderPayPrepaidCardTab1Service orderPayPrepaidCardTab1Service)
        {
            this._orderPayPrepaidCardTab1Service = orderPayPrepaidCardTab1Service;
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
            //유효기간
            //결제대상 금액 

            string strCard_No  = IoC.Get<OrderPayPrepaidCardTab1View>().txtCard_No.Text;            
            string strRem_Amt  = IoC.Get<OrderPayPrepaidCardTab1View>().txtRem_Amt.Text;            
            string strVal_TERM = IoC.Get<OrderPayPrepaidCardTab1View>().txtVal_Term.Text;
            string strPay_Amt = IoC.Get<OrderPayPrepaidCardTab1View>().txtPay_Amt.Text;

            //_orderPayPrepaidCardTab1Service.GetList();

            if (sValue == "btnTab1")
            {
                //DialogHelper.Show("btnTab1");
                //IoC.Get<OrderPayMainViewModel>().ActiveForm("OrderPayPrepaidCardTab1ViewModel");
            }
            else if (sValue == "btnTab2")
            {
                //DialogHelper.Show("btnTab2");
                //IoC.Get<OrderPayMainViewModel>().ActiveForm("OrderPayPrepaidCardTab2ViewModel");
            }
            else if (sValue == "btnSearch") // 조회
            {
                //DialogHelper.Show("조회");
                if(strCard_No.Length > 0)
                {
                    IoC.Get<OrderPayPrepaidCardTab1View>().txtRem_Amt.Text = "50,500";
                    IoC.Get<OrderPayPrepaidCardTab1View>().txtVal_Term.Text = "10";
                }
                else
                    DialogHelper.MessageBox("카드번호를 입력해주세요");
            }
            else if (sValue == "btnReqAppr") // 승인요청
            {
                //IoC.Get<OrderPayMainViewModel>().ActiveForm("OrderPayRightViewModel");
                //DialogHelper.Show("btnReqAppr");
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
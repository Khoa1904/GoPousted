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


/// <summary>
/// 화면명 : 유니오페이 은련카드 UnionPay 613
/// 작성자 : 김형석
/// </summary>

namespace GoPOS.ViewModels
{
    

    public class OrderPayUnionViewModel : Screen
    {
        //private readonly IOrderPayCardService orderPayCardService;

        private IEventAggregator _eventAggregator;

        public OrderPayUnionViewModel(IEventAggregator eventAggregator) //IOrderPayCardService orderPayCardService
        {
            //this.orderPayCardService = orderPayCardService;
            this._eventAggregator = eventAggregator;
            this._eventAggregator.SubscribeOnPublishedThread(this);
            Init();

            //dTOT_SALE_AMT.ToString("N0", CultureInfo.InvariantCulture);
        }

        private async void Init()
        {
            await Task.Delay(100);
        }


        public void ButtonMenu(object sender, RoutedEventArgs e)
        {
            if (sender is not Button btn)
            {
                return;
            }

            string sValue = (string)btn.Tag;
            
            if (sValue == "btnReqSign")
            {
                DialogHelper.MessageBox("서명요청");
                // 서명 요청
            }
            else if (sValue == "btnReqAppr")
            {
                DialogHelper.MessageBox("승인요청");
                //승인 요청
            }
            else if (sValue == "btnRegDirect")
            {
                DialogHelper.MessageBox("임의 등록 후 반드시 단말기 승인 처리해야합니다. \r\n 본 거래는 실제 카드 승인이 되지 않습니다.");
                //임의등록임의등록
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


            //IoC.Get<OrderPayMainViewModel>().ActiveForm("OrderPayRightViewModel");



        }
    }
}
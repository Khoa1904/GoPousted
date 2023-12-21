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
using System.ComponentModel;
using System.Data;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Xml.Linq;


/// <summary>
/// 화면명 모바일 
/// </summary>

namespace GoPOS.ViewModels
{
    

    public class OrderPayMobileSetleViewModel : Screen
    {
        private readonly ISellingStatusRciptSelngSttusService orderPayCardService;

        private IEventAggregator _eventAggregator;

        public OrderPayMobileSetleViewModel(IEventAggregator eventAggregator, ISellingStatusRciptSelngSttusService orderPayCardService)
        {
            this.orderPayCardService = orderPayCardService;
            this._eventAggregator = eventAggregator;
            this._eventAggregator.SubscribeOnPublishedThread(this);
            Init();
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

            if (sValue == "btnReqAppr") // 승인요청
            {
                if (TextValue != null && TextValue.Trim().Length > 1)
                {
                    DialogHelper.MessageBox("승인되었습니다.");
                    //IoC.Get<OrderPayMainViewModel>().ActiveForm("OrderPayRightViewModel");

                    //IoC.Get<OrderPayMobileSetleView>().txtPayMoeny.Text = IoC.Get<OrderPayLeftView>().txtTOT_SALE_AMT.Text;
                    IoC.Get<OrderPayMobileSetleView>().txBarCode.Text = "";
                }
                else
                {
                    DialogHelper.MessageBox("바코드 값을 확인해주세요");
                    return;
                }
            }
            else if (sValue == "btnClose") // 닫기버튼
            {
                //IoC.Get<OrderPayMainViewModel>().ActiveForm("OrderPayRightViewModel");
                //IoC.Get<OrderPayMobileSetleView>().txtPayMoeny.Text = IoC.Get<OrderPayLeftView>().txtTOT_SALE_AMT.Text;
                IoC.Get<OrderPayMobileSetleView>().txBarCode.Text = "";
            }
                
        }

        public void getData()
        {

        }

        private string _textValue;

        public string TextValue
        {
            get { return _textValue; }
            set
            {
                if (_textValue != value)
                {
                    _textValue = value;
                    NotifyOfPropertyChange(() => TextValue);
                }
            }
        }
    }
}
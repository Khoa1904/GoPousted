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
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;


/*
 주문 > 확장메뉴 > 모바일운전면허조회

 */
/// <summary>
/// 화면명 : 모바일운전면허 조회
/// 작성자 : 김형석
/// </summary>
namespace GoPOS.ViewModels
{


    public class OrderPayMobileDrvlsInqireViewModel : Screen
    {
        private readonly IOrderPayService orderPayService;

        private IEventAggregator _eventAggregator;

        public OrderPayMobileDrvlsInqireViewModel(IEventAggregator eventAggregator, IOrderPayService orderPayService)
        {
            this.orderPayService = orderPayService;
            _eventAggregator = eventAggregator;
            _eventAggregator.SubscribeOnPublishedThread(this);

            //_activeItem = (object)"";
            //ActiveItem = IoC.Get<OrderPayCalDlvrViewModel>();

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

            //CBE

            if (sValue == "btnSearch")
            {
                if (IoC.Get<OrderPayMobileDrvlsInqireView>().txtBarCode.Text.Trim().Length > 0)
                {
                    DialogHelper.MessageBox("조회");
                    IoC.Get<OrderPayMobileDrvlsInqireView>().txtBarCode.Text = "";
                    //IoC.Get<OrderPayMainViewModel>().ActiveForm("OrderPayRightViewModel");
                }
                else
                {
                    DialogHelper.MessageBox("바코드 확인해주세요.");
                }

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
    }
}
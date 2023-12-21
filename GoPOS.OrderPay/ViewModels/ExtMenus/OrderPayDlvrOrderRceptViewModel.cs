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
 주문 > 확장메뉴 > 배달주문접수

 */
/// <summary>
/// 화면명 : 배달주문접수 확장 223 
/// 작성자 : 김형석
/// </summary>
namespace GoPOS.ViewModels
{


    public class OrderPayDlvrOrderRceptViewModel : Screen
    {
        private readonly IOrderPayService orderPayService;

        private IEventAggregator _eventAggregator;

        public OrderPayDlvrOrderRceptViewModel(IEventAggregator eventAggregator, IOrderPayService orderPayService)
        {
            this.orderPayService = orderPayService;
            _eventAggregator = eventAggregator;
            _eventAggregator.SubscribeOnPublishedThread(this);

            //_activeItem = (object)"";
            //ActiveItem = IoC.Get<OrderPayCalDlvrViewModel>();

            Init();
        }

        //private object _activeItem;
        //public object ActiveItem
        //{
        //    get { return _activeItem; }
        //    set
        //    {
        //        _activeItem = value;
        //        NotifyOfPropertyChange(() => ActiveItem);
        //    }
        //}

        private async void Init()
        {
            await Task.Delay(100);


            IoC.Get<OrderPayDlvrOrderRceptView>().txtCashReceiptNo.Text = "11";
            IoC.Get<OrderPayDlvrOrderRceptView>().txtName1.Text = "11";
            IoC.Get<OrderPayDlvrOrderRceptView>().txtName2.Text = "22";
        }


        public void ButtonMenu(object sender, RoutedEventArgs e)
        {
            if (sender is not Button btn)
            {
                return;
            }

            string sValue = (string)btn.Tag;

            if (sValue == "btnCash") // 현금버튼
            {
            }
            else if (sValue == "btnCreditCard") // 크래딧카드 버튼
            {
            }
            else if (sValue == "btnCashReceipt") // 현금영수증 버튼
            {
            }
            else if (sValue == "btnCancle") // 상단 취소 버튼
            {
            }
            else if (sValue == "btnUp") // 상단 페이징 Up
            {
            }
            else if (sValue == "btnDn") // 상단 페이징Down
            {
            }
            else if (sValue == "btnOrderReciver1") // 주문자1?
            {
            }
            else if (sValue == "btnOrderReciver2")  // 주문자2?
            {
            }
            else if (sValue == "btnOrderReciver3")  // 주문자3?
            {
            }
            else if (sValue == "btnOrderReciver4")  // 주문자4?
            {
            }
            else if (sValue == "btnOK") // 하단 확인 버튼
            {
            }
            else if (sValue == "btnCancle2") // 하단 취소버튼 
            {
            }
            else if (sValue == "btnChangeAddr") // 하단 소변경
            {
            }
            else if (sValue == "btnUp2") // 하단 페이징 Up2
            {
            }
            else if (sValue == "btnDn2") // 하단 페이징 Dn2
            {
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
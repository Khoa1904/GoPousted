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
 주문 > 확장메뉴 > TRS

 */
/// <summary>
/// 화면명 : TRS 확장 238
/// 작성자 : 김형석
/// </summary>
namespace GoPOS.ViewModels
{

    public class OrderPayTrsViewModel : Screen
    {
        private readonly IOrderPayService orderPayService;
        //public List<ORDER_FUNC_KEY> orderFuncKey;
        //public List<ORDER_LIST> orderList;
        //STRING SHOP_CODE, STRING SALE_DATE, STRING ORDER_NO

        private IEventAggregator _eventAggregator;

        public OrderPayTrsViewModel(IEventAggregator eventAggregator, IOrderPayService orderPayService)
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
        }

        public void ButtonMenu(object sender, RoutedEventArgs e)
        {
            if (sender is not Button btn)
            {
                return;
            }

            string sValue = (string)btn.Tag;

            string strSaleDate = IoC.Get<OrderPayTrsView>().txtSaleDate.Text;
            string strPosNo = IoC.Get<OrderPayTrsView>().txtPosNo.Text;
            string strBillNo = IoC.Get<OrderPayTrsView>().txtBillNo.Text;

            ObservableCollection<TrsData> myDataList = new ObservableCollection<TrsData>();

            if (sValue == "btnSearch") // 조회버튼
            {
                if (strSaleDate.Trim().Length > 0 || strPosNo.Trim().Length > 0 || strBillNo.Trim().Length > 0)
                {
                    getCreList();
                }
                else
                {
                    DialogHelper.MessageBox("정보를 입력해주세요");
                    return;
                }
            }
            else if (sValue == "btnApprReq") // 승인 요청
            {
                if (IoC.Get<OrderPayTrsView>().RecListView.Items.Count > 0 && IoC.Get<OrderPayTrsView>().RecListView.SelectedIndex != -1)
                {
                    DialogHelper.MessageBox("승인되었습니다.");
                    //IoC.Get<OrderPayMainViewModel>().ActiveForm("OrderPayRightViewModel");
                    clearDate();
                }
                else
                {
                    DialogHelper.MessageBox("항목을 선택해주세요");
                }
            }
        }

        private static void getCreList()
        {
            // RecListView
            // NO 
            // POS_NO 포스번호
            // BILL_NO 영수번호
            // TOT_SALE_AMT 결제금액
            // CASH_AMT 현금
            // CRD_CARD_AMT 신용카드
            // DC_FLAG 할인구분
            // SATUS 상태

            IoC.Get<OrderPayTrsView>().RecListView.Items.Clear();
            IoC.Get<OrderPayTrsView>().RecListView.Items.Add(new TrsData { NO = "1", POS_NO = "0001", TOT_SALE_AMT = "25,000", CASH_AMT = "0", CRD_CARD_AMT = "25,000", DC_FLAG = "일반", SATUS = "상태" });
            IoC.Get<OrderPayTrsView>().RecListView.Items.Add(new TrsData { NO = "2", POS_NO = "0001", TOT_SALE_AMT = "5,000", CASH_AMT = "5,000", CRD_CARD_AMT = "0", DC_FLAG = "일반", SATUS = "상태" });
            IoC.Get<OrderPayTrsView>().RecListView.Items.Add(new TrsData { NO = "3", POS_NO = "0001", TOT_SALE_AMT = "55,000", CASH_AMT = "0", CRD_CARD_AMT = "55,000", DC_FLAG = "일반", SATUS = "상태" });
            IoC.Get<OrderPayTrsView>().RecListView.Items.Add(new TrsData { NO = "4", POS_NO = "0001", TOT_SALE_AMT = "35,000", CASH_AMT = "3,0000", CRD_CARD_AMT = "25,000", DC_FLAG = "일반", SATUS = "상태" });

        }

        /// <summary>
        /// x 버튼 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void ButtonClose(object sender, RoutedEventArgs e)
        {
            if (sender is not Button btn)
            {
                return;
            }

            string sValue = (string)btn.Tag;

            //IoC.Get<OrderPayMainView>().DockAllPop.Visibility = Visibility.Hidden;
            //IoC.Get<OrderPayMainViewModel>().ActiveForm("OrderPayRightViewModel");
            clearDate();
        }

        /// <summary>
        /// 필드 초기화
        /// </summary>
        private void clearDate()
        {
            IoC.Get<OrderPayTrsView>().RecListView.Items.Clear();
            IoC.Get<OrderPayTrsView>().txtSaleDate.Text = "";
            IoC.Get<OrderPayTrsView>().txtPosNo.Text = "";
            IoC.Get<OrderPayTrsView>().txtBillNo.Text = "";
        }


        /// <summary>
        /// Model에 통합 시켜야함
        /// </summary>
        public class TrsData
        {
            public string NO { get; set; }
            public string POS_NO { get; set; }
            public string BILL_NO { get; set; }
            public string TOT_SALE_AMT { get; set; }
            public string CASH_AMT { get; set; }
            public string CRD_CARD_AMT { get; set; }
            public string DC_FLAG { get; set; }
            public string SATUS { get; set; }

        }
    }
}
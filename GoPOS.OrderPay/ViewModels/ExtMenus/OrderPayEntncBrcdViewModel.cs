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
using System.Xml.Linq;


/*
 주문 > 확장메뉴 > 입장(바코드)

 */
/// <summary>
/// 화면명 : 입장(바코드) 확장 235
/// 작성자 : 김형석
/// </summary>
namespace GoPOS.ViewModels
{

    public class OrderPayEntncBrcdViewModel : Screen
    {
        private readonly IOrderPayService orderPayService;
        //public List<ORDER_FUNC_KEY> orderFuncKey;
        //public List<ORDER_LIST> orderList;
        //STRING SHOP_CODE, STRING SALE_DATE, STRING ORDER_NO

        private IEventAggregator _eventAggregator;

        public OrderPayEntncBrcdViewModel(IEventAggregator eventAggregator, IOrderPayService orderPayService)
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

            //txtEntPerson
            //txtCurPerson
            //txtOutPerson
            IoC.Get<OrderPayEntncBrcdView>().txtEntPerson.Text = "2";
            IoC.Get<OrderPayEntncBrcdView>().txtCurPerson.Text = "0";
            IoC.Get<OrderPayEntncBrcdView>().txtOutPerson.Text = "2";
        }

        public void ButtonMenu(object sender, RoutedEventArgs e)
        {
            if (sender is not Button btn)
            {
                return;
            }

            string sValue = (string)btn.Tag;
            //EntListView
            //POS_IO_BARCODE
            //txtBarCode            
            //txtEntPerson
            //txtCurPerson
            //txtOutPerson

            //btnEnter
            //btnCancle

            ObservableCollection<POS_IO_BARCODE> myDataList = new ObservableCollection<POS_IO_BARCODE>();
            if (sValue == "btnEnter")
            {
                if (IoC.Get<OrderPayEntncBrcdView>().txtBarCode.Text.Trim().Length > 0)
                {

                    // ListView에 아이템 추가
                    //ObservableCollection<txtBarCode> myDataList = new ObservableCollection<txtBarCode>();
                    //myDataList.Add(new MyData { NO = "1", LOCKER_NO = "1" });
                    //myDataList.Add(new MyData { NO = "2", LOCKER_NO = "2" });
                    //myDataList.Add(new MyData { NO = "3", LOCKER_NO = "3" });

                    /*
                    SHOP_CODE          VARCHAR(6) NOT NULL,
                    SALE_DATE          VARCHAR(8) NOT NULL,
                    BAR_CODE           VARCHAR(26) NOT NULL,
                    IN_DT              VARCHAR(14),
                    OUT_DT             VARCHAR(14),
                    OVER_DT            INTEGER DEFAULT 0 NOT NULL,
                    ADD_PAY            NUMERIC(12,2) DEFAULT 0 NOT NULL,
                    BILL_NO            VARCHAR(4),
                    POS_NO             VARCHAR(2),
                    ORDER_NO           VARCHAR(4),
                    EXIT_CODE          VARCHAR(3),
                    EXIT_ADD_PRD_CODE  VARCHAR(26),
                    SEQ_BAR_CODE       NUMERIC(3,0) DEFAULT 0 NOT NULL 
                    */
                    string BAR_CODE = IoC.Get<OrderPayEntncBrcdView>().txtBarCode.Text;

                    int txtEnt = int.Parse(IoC.Get<OrderPayEntncBrcdView>().txtEntPerson.Text);
                    int txtOut = int.Parse(IoC.Get<OrderPayEntncBrcdView>().txtOutPerson.Text);
                    int txtCur = int.Parse(IoC.Get<OrderPayEntncBrcdView>().txtCurPerson.Text);

                    if (IoC.Get<OrderPayEntncBrcdView>().txtBarCode.Text.Trim().Length > 0)
                    {
                        if (IoC.Get<OrderPayEntncBrcdView>().EntListView.Items.Count < 1)
                        {
                            IoC.Get<OrderPayEntncBrcdView>().EntListView.Items.Add(new POS_IO_BARCODE { NO = "1", BAR_CODE = BAR_CODE, IN_DT = DateTime.Now.ToString(), SEQ_BAR_CODE = "1" });

                            IoC.Get<OrderPayEntncBrcdView>().txtEntPerson.Text = (txtEnt + 1).ToString();
                            IoC.Get<OrderPayEntncBrcdView>().txtCurPerson.Text = (txtCur + 1).ToString();
                            //IoC.Get<OrderPayEntncBrcdView>().txtOutPerson.Text = (txtOut + 1).ToString();
                        }
                        else
                        {
                            int iNo = IoC.Get<OrderPayEntncBrcdView>().EntListView.Items.Count + 1;
                            IoC.Get<OrderPayEntncBrcdView>().EntListView.Items.Add(new POS_IO_BARCODE { NO = iNo.ToString(), BAR_CODE = BAR_CODE, IN_DT = DateTime.Now.ToString(), SEQ_BAR_CODE = "1" });
                            IoC.Get<OrderPayEntncBrcdView>().txtEntPerson.Text = (txtEnt + 1).ToString();
                            IoC.Get<OrderPayEntncBrcdView>().txtCurPerson.Text = (txtCur + 1).ToString();
                        }
                    }
                    else
                        DialogHelper.MessageBox("바코드를 확인해주세요");
                }


            }
            else if (sValue == "btnCancle") // 선택 삭제
            {
                if (IoC.Get<OrderPayEntncBrcdView>().EntListView.Items.Count > 0 && IoC.Get<OrderPayEntncBrcdView>().EntListView.SelectedIndex != -1)
                {
                    int iCnt = 1;
                    int txtEnt = int.Parse(IoC.Get<OrderPayEntncBrcdView>().txtEntPerson.Text);
                    int txtOut = int.Parse(IoC.Get<OrderPayEntncBrcdView>().txtOutPerson.Text);
                    int txtCur = int.Parse(IoC.Get<OrderPayEntncBrcdView>().txtCurPerson.Text);

                    // ListView 선택 삭제
                    int iSel = IoC.Get<OrderPayEntncBrcdView>().EntListView.SelectedIndex;
                    IoC.Get<OrderPayEntncBrcdView>().EntListView.Items.RemoveAt(iSel);

                    // ListVIew 값 가져와서 myDataList 담기
                    var orderPayEntncBrcdView = IoC.Get<OrderPayEntncBrcdView>().EntListView.Items;
                    string BAR_CODE = IoC.Get<OrderPayEntncBrcdView>().txtBarCode.Text;
                    foreach (var item in orderPayEntncBrcdView)
                    {
                        POS_IO_BARCODE order = item as POS_IO_BARCODE;
                        myDataList.Add(new POS_IO_BARCODE { NO = iCnt.ToString(), BAR_CODE = BAR_CODE, IN_DT = DateTime.Now.ToString(), SEQ_BAR_CODE = "1" });
                        //IoC.Get<OrderPayEntncBrcdView>().EntListView.Items.Add();
                        iCnt++;
                    }

                    // ListView 삭제
                    IoC.Get<OrderPayEntncBrcdView>().EntListView.Items.Clear();

                    // ListView 순서대로 담기
                    for (int i = 0; i < myDataList.Count; i++)
                    {
                        IoC.Get<OrderPayEntncBrcdView>().EntListView.Items.Add(myDataList[i]);
                    }

                    //IoC.Get<OrderPayEntncBrcdView>().txtEntPerson.Text = (txtEnt -1).ToString();
                    IoC.Get<OrderPayEntncBrcdView>().txtCurPerson.Text = (txtCur - 1).ToString();
                    IoC.Get<OrderPayEntncBrcdView>().txtOutPerson.Text = (txtOut + 1).ToString();
                }
                else
                    DialogHelper.MessageBox("삭제할 항목을 선택해주세요");

            }
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
        }

        // ListView에 표시할 데이터 클래스 정의
        public class LockerData
        {
            public string NO { get; set; }
            public string BAR_CODE { get; set; }
            public string IN_DT { get; set; }
            public string SEQ_BAR_CODE { get; set; }
        }
    }
}
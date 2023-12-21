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
 주문 > 확장메뉴 > 퇴장(바코드)

 */
/// <summary>
/// 화면명 : 퇴장(바코드) 확장 236
/// 작성자 : 김형석
/// </summary>
namespace GoPOS.ViewModels
{
    public class OrderPayLvBrcdViewModel : Screen
    {
        private readonly IOrderPayEntncBrcdService orderPayEntncBrcdService;
        //public List<ORDER_FUNC_KEY> orderFuncKey;
        //public List<ORDER_LIST> orderList;
        //STRING SHOP_CODE, STRING SALE_DATE, STRING ORDER_NO

        private IEventAggregator _eventAggregator;

        public OrderPayLvBrcdViewModel(IEventAggregator eventAggregator, IOrderPayEntncBrcdService orderPayEntncBrcdService)
        {
            this.orderPayEntncBrcdService = orderPayEntncBrcdService;
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

            //string BAR_CODE = IoC.Get<OrderPayLvBrcdView>().txtBarCode.Text;

            /// 서비스 호출로 변경
            GetExList();

        }

        private static void GetExList()
        {
            IoC.Get<OrderPayLvBrcdView>().txtEntPerson.Text = "4";
            IoC.Get<OrderPayLvBrcdView>().txtOutPerson.Text = "2";
            IoC.Get<OrderPayLvBrcdView>().txtCurPerson.Text = "2";

            int txtEnt = int.Parse(IoC.Get<OrderPayLvBrcdView>().txtEntPerson.Text);
            int txtOut = int.Parse(IoC.Get<OrderPayLvBrcdView>().txtOutPerson.Text);
            int txtCur = int.Parse(IoC.Get<OrderPayLvBrcdView>().txtCurPerson.Text);
            double txtADDPAY = 0;

            for (int i = 0; i < 2; i++)
            {
                if (IoC.Get<OrderPayLvBrcdView>().EntListView.Items.Count < 1)
                {
                    IoC.Get<OrderPayLvBrcdView>().EntListView.Items.Add(new POS_IO_BARCODE { NO = "1", BAR_CODE = "A1", IN_DT = DateTime.Now.ToString("hh:mm"), OVER_DT = "2시간 40분", ADD_PAY = "1,000" });

                    IoC.Get<OrderPayLvBrcdView>().txtEntPerson.Text = (txtEnt + 1).ToString();
                    IoC.Get<OrderPayLvBrcdView>().txtCurPerson.Text = (txtCur + 1).ToString();
                }
                else
                {
                    int iNo = IoC.Get<OrderPayLvBrcdView>().EntListView.Items.Count + 1;
                    IoC.Get<OrderPayLvBrcdView>().EntListView.Items.Add(new POS_IO_BARCODE { NO = iNo.ToString(), BAR_CODE = "A2", IN_DT = DateTime.Now.ToString("hh:mm"), OVER_DT = i.ToString() + "시간 40분", ADD_PAY = "500" });
                    IoC.Get<OrderPayLvBrcdView>().txtEntPerson.Text = (txtEnt + 1).ToString();
                    IoC.Get<OrderPayLvBrcdView>().txtCurPerson.Text = (txtCur + 1).ToString();
                }
            }
            IoC.Get<OrderPayLvBrcdView>().txtAddMoney.Text = Function.Comma(txtADDPAY);
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
            //btnOut
            //btnCancle
            //txtAddMoney

            ObservableCollection<POS_IO_BARCODE> myDataList = new ObservableCollection<POS_IO_BARCODE>();
            if (sValue == "btnOut")
            {
                string BAR_CODE = IoC.Get<OrderPayLvBrcdView>().txtBarCode.Text;

                int txtEnt = int.Parse(IoC.Get<OrderPayLvBrcdView>().txtEntPerson.Text);
                int txtOut = int.Parse(IoC.Get<OrderPayLvBrcdView>().txtOutPerson.Text);
                int txtCur = int.Parse(IoC.Get<OrderPayLvBrcdView>().txtCurPerson.Text);

                double txtADDPAY = 0;

                //for (int i = 0; i < 2; i++)
                {
                    //if (IoC.Get<OrderPayLvBrcdView>().EntListView.Items.Count < 1)
                    //{
                    //    IoC.Get<OrderPayLvBrcdView>().EntListView.Items.Add((new POS_IO_BARCODE { NO = "1", BAR_CODE = BAR_CODE, IN_DT = DateTime.Now.ToString("hh:mm"), OVER_DT = "2시간 40분", ADD_PAY = "1,000" }));
                    //
                    //    IoC.Get<OrderPayLvBrcdView>().txtCurPerson.Text = (txtCur - 1).ToString();
                    //    IoC.Get<OrderPayLvBrcdView>().txtOutPerson.Text = (txtOut + 1).ToString();                        
                    //}
                    //else
                    {
                        int iNo = IoC.Get<OrderPayLvBrcdView>().EntListView.Items.Count + 1;
                        IoC.Get<OrderPayLvBrcdView>().EntListView.Items.Add(new POS_IO_BARCODE { NO = iNo.ToString(), BAR_CODE = BAR_CODE, IN_DT = DateTime.Now.ToString("hh:mm"), OVER_DT = iNo.ToString() + "시간 40분", ADD_PAY = "500" });
                        IoC.Get<OrderPayLvBrcdView>().txtCurPerson.Text = (txtCur - 1).ToString();
                        IoC.Get<OrderPayLvBrcdView>().txtOutPerson.Text = (txtOut + 1).ToString();

                        // ListView 삭제
                        //IoC.Get<OrderPayLvBrcdView>().EntListView.Items.Clear();

                        // ListView 순서대로 담기
                        for (int i = 0; i < myDataList.Count; i++)
                        {
                            IoC.Get<OrderPayLvBrcdView>().EntListView.Items.Add(myDataList[i]);
                        }

                        //IoC.Get<OrderPayLvBrcdView>().txtEntPerson.Text = (txtEnt -1).ToString();
                        IoC.Get<OrderPayLvBrcdView>().txtCurPerson.Text = (txtCur + 1).ToString();
                        IoC.Get<OrderPayLvBrcdView>().txtOutPerson.Text = (txtOut - 1).ToString();

                        // ListVIew 값 가져와서 myDataList 담기
                        var orderPayLvBrcdView = IoC.Get<OrderPayLvBrcdView>().EntListView.Items;
                        foreach (var item in orderPayLvBrcdView)
                        {
                            POS_IO_BARCODE order = item as POS_IO_BARCODE;
                            txtADDPAY += double.Parse(order.ADD_PAY.Replace(",", ""));
                        }

                        IoC.Get<OrderPayLvBrcdView>().txtAddMoney.Text = Function.Comma(txtADDPAY);

                    }
                }
            }
            else if (sValue == "btnCancle") // 선택 삭제
            {
                if (IoC.Get<OrderPayLvBrcdView>().EntListView.Items.Count > 0 && IoC.Get<OrderPayLvBrcdView>().EntListView.SelectedIndex != -1)
                {
                    int iCnt = 1;
                    int txtEnt = int.Parse(IoC.Get<OrderPayLvBrcdView>().txtEntPerson.Text);
                    int txtOut = int.Parse(IoC.Get<OrderPayLvBrcdView>().txtOutPerson.Text);
                    int txtCur = int.Parse(IoC.Get<OrderPayLvBrcdView>().txtCurPerson.Text);

                    double txtADDPAY = 0;

                    // ListView 선택 삭제
                    int iSel = IoC.Get<OrderPayLvBrcdView>().EntListView.SelectedIndex;
                    IoC.Get<OrderPayLvBrcdView>().EntListView.Items.RemoveAt(iSel);

                    // ListVIew 값 가져와서 myDataList 담기
                    var orderPayLvBrcdView = IoC.Get<OrderPayLvBrcdView>().EntListView.Items;
                    string BAR_CODE = IoC.Get<OrderPayLvBrcdView>().txtBarCode.Text;
                    foreach (var item in orderPayLvBrcdView)
                    {
                        POS_IO_BARCODE order = item as POS_IO_BARCODE;
                        myDataList.Add(new POS_IO_BARCODE { NO = iCnt.ToString(), BAR_CODE = order.BAR_CODE, IN_DT = order.IN_DT, OVER_DT = order.OVER_DT, ADD_PAY = order.ADD_PAY });
                        txtADDPAY += double.Parse(order.ADD_PAY.Replace(",", ""));
                        iCnt++;
                    }

                    // ListView 삭제
                    IoC.Get<OrderPayLvBrcdView>().EntListView.Items.Clear();

                    // ListView 순서대로 담기
                    for (int i = 0; i < myDataList.Count; i++)
                    {
                        IoC.Get<OrderPayLvBrcdView>().EntListView.Items.Add(myDataList[i]);
                    }

                    //IoC.Get<OrderPayLvBrcdView>().txtEntPerson.Text = (txtEnt -1).ToString();
                    IoC.Get<OrderPayLvBrcdView>().txtCurPerson.Text = (txtCur + 1).ToString();
                    IoC.Get<OrderPayLvBrcdView>().txtOutPerson.Text = (txtOut - 1).ToString();
                    IoC.Get<OrderPayLvBrcdView>().txtAddMoney.Text = Function.Comma(txtADDPAY);
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
    }
}
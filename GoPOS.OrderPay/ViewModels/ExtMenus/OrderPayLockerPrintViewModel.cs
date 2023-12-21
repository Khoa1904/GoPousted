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
 주문 > 확장메뉴 > 보관함출력
 */
/// <summary>
/// 화면명 : 보관함출력 확장 231
/// 작성자 : 김형석
/// </summary>
namespace GoPOS.ViewModels
{

    public class OrderPayLockerPrintViewModel : Screen
    {
        private readonly IOrderPayService orderPayService;

        private IEventAggregator _eventAggregator;

        public OrderPayLockerPrintViewModel(IEventAggregator eventAggregator, IOrderPayService orderPayService)
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
            //txtLockerNo 

        }


        public void ButtonMenu(object sender, RoutedEventArgs e)
        {
            if (sender is not Button btn)
            {
                return;
            }

            string sValue = (string)btn.Tag;

            ObservableCollection<LockerData> myDataList = new ObservableCollection<LockerData>();

            if (sValue == "btnAdd") // 셀 추가
            {
                // ListView에 아이템 추가
                //ObservableCollection<LockerData> myDataList = new ObservableCollection<LockerData>();
                //myDataList.Add(new MyData { NO = "1", LOCKER_NO = "1" });
                //myDataList.Add(new MyData { NO = "2", LOCKER_NO = "2" });
                //myDataList.Add(new MyData { NO = "3", LOCKER_NO = "3" });

                string LOCKER_NO = IoC.Get<OrderPayLockerPrintView>().txtLockerNo.Text;

                if (IoC.Get<OrderPayLockerPrintView>().txtLockerNo.Text.Trim().Length > 0)
                {
                    if (IoC.Get<OrderPayLockerPrintView>().LockerListView.Items.Count < 1)
                    {
                        IoC.Get<OrderPayLockerPrintView>().LockerListView.Items.Add(new LockerData { NO = "1", LOCKER_NO = LOCKER_NO });
                    }
                    else
                    {
                        int iNo = IoC.Get<OrderPayLockerPrintView>().LockerListView.Items.Count + 1;
                        IoC.Get<OrderPayLockerPrintView>().LockerListView.Items.Add(new LockerData { NO = iNo.ToString(), LOCKER_NO = LOCKER_NO });
                    }

                }
                else
                    DialogHelper.MessageBox("락커번호를 입력해주세요");

                //IoC.Get<OrderPayLockerPrintView>().LockerListView.Items.Add((new LockerData { NO = "2", LOCKER_NO = "22" }));
                //IoC.Get<OrderPayLockerPrintView>().LockerListView.Items.Add((new LockerData { NO = "3", LOCKER_NO = "33" }));

                //OrderPayLockerPrintView view = IoC.Get<OrderPayLockerPrintView>();
                //view.LockerListView.Items.Clear();
                //view.LockerListView.Items.Add(myDataList); //.Add("New Item");

                //
                //LOCKER_NO

            }
            else if (sValue == "btnSelectDel") // 선택 삭제
            {
                if (IoC.Get<OrderPayLockerPrintView>().LockerListView.Items.Count > 0 && IoC.Get<OrderPayLockerPrintView>().LockerListView.SelectedIndex != -1)
                {
                    int iCnt = 1;

                    // ListView 선택 삭제
                    int iSel = IoC.Get<OrderPayLockerPrintView>().LockerListView.SelectedIndex;
                    IoC.Get<OrderPayLockerPrintView>().LockerListView.Items.RemoveAt(iSel);

                    // ListVIew 값 가져와서 myDataList 담기
                    var orderPayLockerPrintView = IoC.Get<OrderPayLockerPrintView>().LockerListView.Items;
                    foreach (var item in orderPayLockerPrintView)
                    {
                        LockerData order = item as LockerData;
                        myDataList.Add(new LockerData { NO = iCnt.ToString(), LOCKER_NO = order.LOCKER_NO });
                        iCnt++;
                    }

                    // ListView 삭제
                    IoC.Get<OrderPayLockerPrintView>().LockerListView.Items.Clear();

                    // ListView 순서대로 담기
                    for (int i = 0; i < myDataList.Count; i++)
                    {
                        IoC.Get<OrderPayLockerPrintView>().LockerListView.Items.Add(myDataList[i]);
                    }
                }
                else
                    DialogHelper.MessageBox("삭제할 항목을 선택해주세요");

            }
            else if (sValue == "btnAllDel") // 전체삭제
            {
                // ListView 삭제
                IoC.Get<OrderPayLockerPrintView>().LockerListView.Items.Clear();
            }
            else if (sValue == "btnRelase") // 발행
            {
                // ListView 삭제
                IoC.Get<OrderPayLockerPrintView>().LockerListView.Items.Clear();
                DialogHelper.MessageBox("발행");
            }
        }

        //public ListView LockerListView { get; set; }

        // ListView에 표시할 데이터 클래스 정의
        public class LockerData
        {
            public string NO { get; set; }
            public string LOCKER_NO { get; set; }
        }

        /// <summary>
        /// x버튼 이벤트
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
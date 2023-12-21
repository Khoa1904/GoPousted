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


/*
 주문 > 확장메뉴 > 입장/퇴장 내역

 */
/// <summary>
///  화면명 : 입장/퇴장 내역 확장 257
///  작성자 : 김형석  
/// </summary>
namespace GoPOS.ViewModels
{

    public class OrderPayEntncLvDtlsViewModel : Screen, INotifyPropertyChanged
    {
        private readonly IOrderPayService orderPayService;
        //public List<ORDER_FUNC_KEY> orderFuncKey;
        //public List<ORDER_LIST> orderList;
        //STRING SHOP_CODE, STRING SALE_DATE, STRING ORDER_NO

        private IEventAggregator _eventAggregator;

        public OrderPayEntncLvDtlsViewModel(IEventAggregator eventAggregator, IOrderPayService orderPayService)
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

            InitData();
            CheckBox1Checked = true;

            //SelectedDate = DateTime.Today;
            //SelectedDate = 

        }

        public void ButtonMenu(object sender, RoutedEventArgs e)
        {
            if (sender is not Button btn)
            {
                return;
            }

            string sValue = (string)btn.Tag;

            if (sValue == "btnSearch") // 조회 버튼
            {
                GetList();
            }
            else if (sValue == "btnSelect") // 선택 버튼
            {

                int iSel = IoC.Get<OrderPayEntncLvDtlsView>().EntListView.SelectedIndex;
                if (iSel > 0)
                    DialogHelper.MessageBox("영수증 없습니다.");
                else
                    DialogHelper.MessageBox("영수증 없습니다.");
                //GetList();

            }

        }

        private void InitData()
        {
            IoC.Get<OrderPayEntncLvDtlsView>().EntListView.Items.Clear();
            IoC.Get<OrderPayEntncLvDtlsView>().EntListView.Items.Add(new POS_IO_BARCODE { NO = "1", BAR_CODE = "A_1", IN_DT = DateTime.Now.AddHours(1).ToString("hh:mm"), OUT_DT = DateTime.Now.ToString("hh:mm"), OVER_DT = 1 + "시간", ADD_PAY = 1 + ",000", BILL_NO = "" }); ;
            IoC.Get<OrderPayEntncLvDtlsView>().EntListView.Items.Add(new POS_IO_BARCODE { NO = "2", BAR_CODE = "A_2", IN_DT = DateTime.Now.AddHours(2).ToString("hh:mm"), OUT_DT = DateTime.Now.ToString("hh:mm"), OVER_DT = 2 + "시간", ADD_PAY = 2 + ",000", BILL_NO = "" }); ;
            IoC.Get<OrderPayEntncLvDtlsView>().EntListView.Items.Add(new POS_IO_BARCODE { NO = "3", BAR_CODE = "B_3", IN_DT = DateTime.Now.ToString("hh:mm"), OUT_DT = "", OVER_DT = "", ADD_PAY = "", BILL_NO = "" });
            IoC.Get<OrderPayEntncLvDtlsView>().EntListView.Items.Add(new POS_IO_BARCODE { NO = "4", BAR_CODE = "B_4", IN_DT = DateTime.Now.ToString("hh:mm"), OUT_DT = "", OVER_DT = "", ADD_PAY = "", BILL_NO = "" });
        }
        private void GetList()
        {
            ObservableCollection<POS_IO_BARCODE> myDataList = new ObservableCollection<POS_IO_BARCODE>();

            if (CheckBox1Checked == true)
            {
                var orderPayEntncLvDtlsView = IoC.Get<OrderPayEntncLvDtlsView>().EntListView.Items;

                InitData(); // 조회

            }
            else if (CheckBox2Checked == true)
            {
                int iCnt = 1;

                // ListView 선택 
                //iSel = IoC.Get<OrderPayEntncLvDtlsView>().EntListView.SelectedIndex;

                // ListVIew 값 가져와서 myDataList 담기
                InitData();

                var orderPayEntncLvDtlsView = IoC.Get<OrderPayEntncLvDtlsView>().EntListView.Items;

                foreach (var item in orderPayEntncLvDtlsView)
                {
                    POS_IO_BARCODE order = item as POS_IO_BARCODE;
                    if (order.BAR_CODE.Contains("B_"))
                        myDataList.Add(new POS_IO_BARCODE { NO = iCnt.ToString(), BAR_CODE = "B_" + iCnt, IN_DT = DateTime.Now.AddHours(iCnt).ToString("hh:mm"), OUT_DT = "", OVER_DT = "", ADD_PAY = "", BILL_NO = "" }); ;

                    iCnt++;
                }

                // ListView 삭제
                IoC.Get<OrderPayEntncLvDtlsView>().EntListView.Items.Clear();

                // ListView 순서대로 담기
                for (int i = 0; i < myDataList.Count; i++)
                {
                    IoC.Get<OrderPayEntncLvDtlsView>().EntListView.Items.Add(myDataList[i]);
                }
            }
            else if (CheckBox3Checked == true)
            {
                int iCnt = 1;

                // ListView 선택
                //iSel = IoC.Get<OrderPayEntncLvDtlsView>().EntListView.SelectedIndex;
                InitData();

                // ListVIew 값 가져와서 myDataList 담기
                var orderPayEntncLvDtlsView = IoC.Get<OrderPayEntncLvDtlsView>().EntListView.Items;
                //string BAR_CODE = IoC.Get<OrderPayEntncLvDtlsView>().txtBarCode.Text;
                foreach (var item in orderPayEntncLvDtlsView)
                {
                    POS_IO_BARCODE order = item as POS_IO_BARCODE;

                    if (order.BAR_CODE.Contains("A_"))
                        myDataList.Add(new POS_IO_BARCODE { NO = iCnt.ToString(), BAR_CODE = "A_" + iCnt, IN_DT = DateTime.Now.AddHours(iCnt).ToString("hh:mm"), OUT_DT = DateTime.Now.ToString("hh:mm"), OVER_DT = iCnt + "시간", ADD_PAY = iCnt + ",000", BILL_NO = "" }); ;

                    //txtADDPAY += double.Parse(order.ADD_PAY.Replace(",", ""));
                    iCnt++;
                }

                // ListView 삭제
                IoC.Get<OrderPayEntncLvDtlsView>().EntListView.Items.Clear();

                // ListView 순서대로 담기
                for (int i = 0; i < myDataList.Count; i++)
                {
                    IoC.Get<OrderPayEntncLvDtlsView>().EntListView.Items.Add(myDataList[i]);
                }

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

        /// <summary>
        /// 체크박스 이벤트
        /// </summary>
        /// <summary>
        /// 체크박스 이벤트
        /// </summary>
        private bool _checkBox1Checked = true;
        private bool _checkBox2Checked = false;
        private bool _checkBox3Checked = false;

        public bool CheckBox1Checked
        {
            get { return _checkBox1Checked; }
            set
            {
                if (value)
                {
                    _checkBox1Checked = true;
                    CheckBox2Checked = false;
                    CheckBox3Checked = false;
                    GetList();
                }
                else
                {
                    if (CheckBox2Checked == false && CheckBox3Checked == false)
                    {
                        _checkBox1Checked = true;
                    }
                    else
                        _checkBox1Checked = false;
                }
                NotifyOfPropertyChange(() => CheckBox1Checked);
                NotifyOfPropertyChange(() => CheckBox2Checked);
                NotifyOfPropertyChange(() => CheckBox3Checked);
            }
        }

        public bool CheckBox2Checked
        {
            get { return _checkBox2Checked; }
            set
            {
                if (value)
                {
                    _checkBox2Checked = true;
                    CheckBox1Checked = false;
                    CheckBox3Checked = false;
                    GetList();
                }
                else
                {
                    if (CheckBox1Checked == false && CheckBox3Checked == false)
                    {
                        _checkBox2Checked = true;
                    }
                    else
                        _checkBox2Checked = false;
                }

                //_checkBox2Checked = false;

                NotifyOfPropertyChange(() => CheckBox1Checked);
                NotifyOfPropertyChange(() => CheckBox2Checked);
                NotifyOfPropertyChange(() => CheckBox3Checked);
                //NotifyOfPropertyChange(() => IsOneCheckBoxChecked);
            }
        }

        public bool CheckBox3Checked
        {
            get { return _checkBox3Checked; }
            set
            {
                if (value)
                {
                    _checkBox3Checked = true;
                    CheckBox1Checked = false;
                    CheckBox2Checked = false;
                    GetList();
                }
                else
                {
                    if (CheckBox1Checked == false && CheckBox2Checked == false)
                    {
                        _checkBox3Checked = true;
                    }
                    else
                        _checkBox3Checked = false;
                }

                NotifyOfPropertyChange(() => CheckBox1Checked);
                NotifyOfPropertyChange(() => CheckBox2Checked);
                NotifyOfPropertyChange(() => CheckBox3Checked);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


        /// <summary>
        /// 달력 컨트롤
        /// </summary>
        public DateTime? _selectedDate;
        public DateTime? SelectedDate
        {
            get { return _selectedDate; }
            set
            {
                if (value == null)
                {
                    _selectedDate = null;
                }
                else if (value != _selectedDate)
                {
                    _selectedDate = value;
                    NotifyOfPropertyChange(() => SelectedDate);
                }
            }
        }
    }
}
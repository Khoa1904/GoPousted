using Caliburn.Micro;
using GoPOS.Common.Helpers;
using GoPOS.Common.Interface.Model;
using GoPOS.Common.Interface.View;
using GoPOS.Common.PrinterLib;
using GoPOS.Common.Service;
using GoPOS.Common.ViewModels;
using GoPOS.Database;
using GoPOS.Helpers;
using GoPOS.Helpers.CommandHelper;
using GoPOS.Models;
using GoPOS.Models.Common;
using GoPOS.Models.Config;
using GoPOS.Models.Custom.OrderMng;
using GoPOS.OrderPay.Interface.View;
using GoPOS.Service;
using GoPOS.Service.Common;
using GoPOS.Services;
using GoShared.Helpers;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

/*
 주문 > 확장메뉴 > 영수증관리

 */

namespace GoPOS.ViewModels
{

    public class OrderPayReceiptMngViewModel : OrderPayChildViewModel, IHandle<CalendarEventArgs>, IHandle<SelectPosEventArgs>, IHandle<ReceiptMngListEvent>
    {
        //**----------------------------------------------------------------------------------

        #region Member
        private IOrderPayReceiptMngService _OrderPayReceiptMngService;
        private IOrderPayMainViewModel orderPayMainViewModel;
        private IOrderPayReceiptMngView _view;
        private DateTime _txtSaleDate;
        private IPOSPrintService printService;
        private readonly IOrderPayService orderPayService;
        private bool _isOpenDetail = false;
        //private IPOSPrintService _pOSPrintService;
        private SynchronizationContext _sysContext;
        #endregion

        //**----------------------------------------------------------------------------------

        #region Property

        public override object ActivateType => "ExceptKeyPad";

        public DateTime txtSaleDate
        {
            get { return _txtSaleDate; }
            set
            {
                if (_txtSaleDate != value)
                {
                    _txtSaleDate = value;
                    NotifyOfPropertyChange(() => txtSaleDate);
                }
            }
        }

        private string _txtPosNo;
        public string txtPosNo
        {
            get { return _txtPosNo; }
            set
            {
                if (_txtPosNo != value)
                {
                    _txtPosNo = value;
                    NotifyOfPropertyChange(() => txtPosNo);
                }
            }
        }

        private string _txtBillNo;
        public string txtBillNo
        {
            get { return _txtBillNo; }
            set
            {
                if (_txtBillNo != value)
                {
                    _txtBillNo = value;
                    NotifyOfPropertyChange(() => txtBillNo);
                }
            }
        }

        private string _txtTableNm;
        public string txtTableNm
        {
            get { return _txtTableNm; }
            set
            {
                if (_txtTableNm != value)
                {
                    _txtTableNm = value;
                    NotifyOfPropertyChange(() => txtTableNm);
                }
            }
        }

        private string _txtTelNo;
        public string txtTelNo
        {
            get { return _txtTelNo; }
            set
            {
                if (_txtTelNo != value)
                {
                    _txtTelNo = value;
                    NotifyOfPropertyChange(() => txtTelNo);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool DontPrintItem
        {
            get => dontPrintItem; set
            {
                dontPrintItem = value;
                NotifyOfPropertyChange(nameof(DontPrintItem));
                ShowPreviewPrint();
            }
        }

        public string ReceiptContents
        {
            get => receiptContents; set
            {
                receiptContents = value;
                NotifyOfPropertyChange(nameof(ReceiptContents));
            }
        }

        private RECEIPT_MANAGER_ITEM _selectedItem;
        public RECEIPT_MANAGER_ITEM SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                if (_selectedItem != value)
                {
                    _selectedItem = value;
                    ShowPreviewPrint();
                    NotifyOfPropertyChange(() => SelectedItem);
                }
            }
        }
        //private int counter = 1;
        //public int AutoIncrementNo => counter++;

        private ObservableCollection<RECEIPT_MANAGER_ITEM> _trnTenderSeqItemList;
        public ObservableCollection<RECEIPT_MANAGER_ITEM> TrnTenderSeqItemList
        {
            get { return _trnTenderSeqItemList; }
            set
            {
                if (_trnTenderSeqItemList != value)
                {
                    _trnTenderSeqItemList = value;
                    NotifyOfPropertyChange(() => TrnTenderSeqItemList);
                }
            }
        }

        #endregion

        //**----------------------------------------------------------------------------------

        #region Contructor

        public OrderPayReceiptMngViewModel(IWindowManager windowManager, IEventAggregator eventAggregator, IOrderPayReceiptMngService OrderPayReceiptMngService,
                                           IPOSPrintService printService, IOrderPayService orderPayService)
            : base(windowManager, eventAggregator)
        {
            _sysContext = SynchronizationContext.Current;
            this._OrderPayReceiptMngService = OrderPayReceiptMngService;
            this.ViewLoaded += OrderPayReceiptMng_ViewLoaded;
            this.printService = printService;
            this.orderPayService = orderPayService;
        }
        #endregion

        //**----------------------------------------------------------------------------------

        #region Public Method
        public override bool SetIView(IView view)
        {
            orderPayMainViewModel = (IOrderPayMainViewModel)Parent;
            _view = (IOrderPayReceiptMngView)view;
            return base.SetIView(view);
        }

        public Task HandleAsync(CalendarEventArgs message, CancellationToken cancellationToken)
        {
            txtSaleDate = message.Date;
            return Task.CompletedTask;
        }

        public Task HandleAsync(SelectPosEventArgs message, CancellationToken cancellationToken)
        {
            txtPosNo = message.PosNo;
            return Task.CompletedTask;
        }
        public Task HandleAsync(ReceiptMngListEvent message, CancellationToken cancellationToken)
        {
            if (message.EventType == 1)
            {
                SelectedItem = message.Item;
            }
            else
            {
                SelectedItem.ORG_BILL_NO = message.Item.ORG_BILL_NO;
                PrintReturnBill(SelectedItem.ORG_BILL_NO);
                GetTrnTenderSeqList();
            }
            return Task.CompletedTask;
        }

        ORDER_FUNC_KEY funcKey;
        private bool dontPrintItem;
        private string receiptContents;

        public ICommand ButtonCommand => new RelayCommand<Button>(async (button) =>
        {
            if (button.Tag == null) return;
            switch (button.Tag.ToString())
            {
                case "btnCal":
                    System.Windows.Point point = _view.buttonPosition;
                    DialogHelper.ShowDialogWithCoords(typeof(CalendarViewModel), 450, 480, point.X - 450, point.Y + 30, txtSaleDate);
                    break;
                case "btnSearchPos":
                    IoC.Get<SelectBoxViewModel>();
                    _eventAggregator?.PublishOnUIThreadAsync(new SelectboxEvent()
                    {
                        Type = "POS"
                    });
                    System.Windows.Point pointPos = _view.buttonPosition;
                    DialogHelper.ShowDialogWithCoords(typeof(SelectBoxViewModel), 50, 350, pointPos.X - 50, pointPos.Y + 30);
                    break;
                case "btnSearchTable":

                    break;
                case "btnReceiptList":
                    if (TrnTenderSeqItemList == null) return;
                    DialogHelper.ShowDialog(typeof(OrderpayReceiptListViewModel), 768, 500, TrnTenderSeqItemList);
                    break;
                case "btnPrintReceipt":
                    //in
                    break;
                case "btnSearch":
                    GetTrnTenderSeqList();
                    break;
                case "btnToggleReceipt":
                    funcKey = new ORDER_FUNC_KEY();
                    funcKey.FK_NO = "609";
                    var mapKey = orderPayMainViewModel.FunKeyMaps.FirstOrDefault(p => p.FK_NO == funcKey.FK_NO);
                    _isOpenDetail = true;
                    orderPayMainViewModel.ProcessFuncKeyClicked(funcKey, ReceiptContents);
                    break;
                case "btnRePrint":
                    if (SelectedItem == null) return;
                    printService.PrintReceipt(PrintOptions.Normal, SelectedItem.SHOP_CODE, SelectedItem.POS_NO, SelectedItem.SALE_DATE, SelectedItem.BILL_NO, !DontPrintItem, true);
                    printService.EndPrinting();
                    break;
                case "btnReturn":
                    #region Return Receipt - 반품처리
                    if (SelectedItem == null) break;
                    if (DataLocals.AppConfig.PosOption.POSFlag != "0")
                    {
                        DialogHelper.MessageBox("메인포스에만 반품가능합니다.", GMessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }

                    if ("N".Equals(SelectedItem.SALE_YN))
                    {
                        DialogHelper.MessageBox("반품거래입니다. 확인하시기 바랍니다.", GMessageBoxButton.OK, MessageBoxImage.Warning);
                        break;
                    }

                    if ("Y".Equals(SelectedItem.SALE_YN) && !string.IsNullOrEmpty(SelectedItem.ORG_BILL_NO))
                    {
                        DialogHelper.MessageBox("이미 반품된 거래입니다. 확인하시기 바랍니다.", GMessageBoxButton.OK, MessageBoxImage.Warning);
                        break;
                    }

                    // Has card, simple pay, cash receipt, need to show popup
                    var res = DialogHelper.MessageBox("선택한 매출을 반품처리 하시겠습니까?", GMessageBoxButton.OKCancel, MessageBoxImage.Question);
                    if (res == MessageBoxResult.Cancel)
                    {
                        break;
                    }

                    if (SelectedItem.PAY_HAS_APPR <= 0)
                    {
                        var result = orderPayService.DoReturnReceipt(SelectedItem.SHOP_CODE, SelectedItem.POS_NO, SelectedItem.SALE_DATE, SelectedItem.REGI_SEQ, SelectedItem.BILL_NO).Result;
                        if (result.Item1.ResultType != EResultType.SUCCESS)
                        {
                            DialogHelper.MessageBox(result.Item1.ResultMessage, GMessageBoxButton.OK, MessageBoxImage.Error);
                            break;
                        }

                        // returnBillNo = tranData.TranHeader.SHOP_CODE + tranData.TranHeader.SALE_DATE + tranData.TranHeader.POS_NO + tranData.TranHeader.BILL_NO;
                        PrintReturnBill(result.Item2);
                        GetTrnTenderSeqList();
                    }
                    else
                    {
                        _isOpenDetail = true;
                        orderPayMainViewModel.ProcessFuncKeyClicked(new FkMapInfo(string.Empty, string.Empty)
                        {
                            ItemArea = "ReceiptReturn",
                            IsPopup = FkMapActionTypes.Action
                        }, SelectedItem);
                    }

                    #endregion
                    break;

                case "btnChangePay":
                    break;
                case "btnCashReceipt":
                    break;
                case "btnNoReturnPayment":
                    break;
                case "chkExProdList":
                    break;
                case "btnKitchenRePrint":
                    break;
                case "cloze":
                    this.TryCloseAsync();
                    break;
                case "ReSendKitchen":
                    OnReSendKitchen();
                    break;
                default:
                    break;
            }
        });

        #endregion

        //**----------------------------------------------------------------------------------

        #region Protected

        #endregion

        //**----------------------------------------------------------------------------------

        #region Private
        private void OnReSendKitchen()
        {
            if (SelectedItem == null) return;

            printService.PrintKitchenOrderAsyn(new OrderInfo()
            {
                ShopCode = SelectedItem.SHOP_CODE,
                BillNo = SelectedItem.BILL_NO,
                PosNo = SelectedItem.POS_NO,
                SaleDate = SelectedItem.SALE_DATE
            }).ContinueWith(
                             t =>
                             {
                                 if (!t.IsFaulted)
                                 {
                                     _sysContext.Send(state =>
                                     {
                                         if (t.Result.Success)
                                         {
                                             var mess = "" + Application.Current.Resources["0719"];
                                             //DialogHelper.MessageBox(mess);
                                         }
                                         else
                                         {
                                             DialogHelper.MessageBox(t.Result.Message);
                                         }
                                     }, null);
                                 }
                             }
                            );

        }

        private void PrintReturnBill(string returnBillNo)
        {
            string retSaleDate = returnBillNo.Substring(7, 8);
            string retPosNo = returnBillNo.Substring(15, 2);
            string retBillNo = returnBillNo.Substring(returnBillNo.Length - 4);
            printService.PrintReceipt(PrintOptions.Normal, SelectedItem.SHOP_CODE, retPosNo, retSaleDate, retBillNo, !DontPrintItem, false);
            printService.EndPrinting();
        }

        private void GetTrnTenderSeqList()
        {
            ReceiptContents = string.Empty;
            var result = _OrderPayReceiptMngService.GetTrnTenderSeqList(txtSaleDate.ToString("yyyyMMdd"), txtPosNo == "전체" ? "" : txtPosNo, txtBillNo).Result;
            if (result == null)
            {
                TrnTenderSeqItemList.Clear();
                return;
            }

            TrnTenderSeqItemList = new ObservableCollection<RECEIPT_MANAGER_ITEM>(result);
            if (TrnTenderSeqItemList.Count > 0)
            {
                SelectedItem = TrnTenderSeqItemList[TrnTenderSeqItemList.Count - 1];
                _view.ScrollToEnd();
            }

        }

        private void ShowPreviewPrint()
        {
            if (SelectedItem == null) return;
            ReceiptContents = printService.PreviewReceipt(SelectedItem.SHOP_CODE, SelectedItem.POS_NO, SelectedItem.SALE_DATE, SelectedItem.BILL_NO, !DontPrintItem);
        }
        #endregion

        //**----------------------------------------------------------------------------------

        #region Event
        private void OrderPayReceiptMng_ViewLoaded(object? sender, EventArgs e)
        {
            txtSaleDate = DateTime.ParseExact(DataLocals.PosStatus.SALE_DATE, "yyyyMMdd", CultureInfo.InvariantCulture);
            if (_isOpenDetail)
            {
                _isOpenDetail = false;
                return;
            }
            txtBillNo = "";
            txtPosNo = DataLocals.PosStatus.POS_NO;
            DontPrintItem = false;
            GetTrnTenderSeqList();
        }

        #endregion

        //**----------------------------------------------------------------------------------

    }
}
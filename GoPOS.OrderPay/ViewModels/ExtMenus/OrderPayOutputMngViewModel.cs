using Caliburn.Micro;
using Dapper;
using GoPOS.Common.Interface.Model;
using GoPOS.Common.Interface.View;
using GoPOS.Common.PrinterLib;
using GoPOS.Common.Service;
using GoPOS.Common.ViewModels;
using GoPOS.Helpers;
using GoPOS.Helpers.CommandHelper;
using GoPOS.Models;
using GoPOS.Models.Common;
using GoPOS.Models.Custom.OrderMng;
using GoPOS.OrderPay.Interface.View;
using GoPOS.OrderPay.Models;
using GoPOS.Service;
using GoPOS.Services;

using GoPOS.Views;
using GoShared.Helpers;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.Entity.Core.Metadata.Edm;
using System.Diagnostics.Metrics;
using System.Dynamic;
using System.Globalization;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using static System.Net.Mime.MediaTypeNames;


namespace GoPOS.ViewModels
{

    public class OrderPayOutputMngViewModel : OrderPayChildViewModel, IHandle<CalendarEventArgs>, IHandle<SelectPosEventArgs>
    {
        private IOrderPayOutputMngView _view;
        private IPOSPrintService printService;
        private readonly IOrderPayOutputMngService orderPayOutputMngService;
        private DateTime _txtSaleDate;
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

        public string ReceiptContents { get; set; }

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

        public override object ActivateType => "ExceptKeyPad";
        public OrderPayOutputMngViewModel(IWindowManager windowManager, IEventAggregator eventAggregator, IPOSPrintService printService,
            IOrderPayOutputMngService orderPayOutputMngService) :
            base(windowManager, eventAggregator)
        {
            this.ViewLoaded += OrderPayOutputMngViewModel_ViewLoaded;
            this.printService = printService;
            this.orderPayOutputMngService = orderPayOutputMngService;
        }

        private void OrderPayOutputMngViewModel_ViewLoaded(object? sender, EventArgs e)
        {
            txtSaleDate = DateTime.ParseExact(DataLocals.PosStatus.SALE_DATE, "yyyyMMdd", CultureInfo.InvariantCulture);
            txtPosNo = DataLocals.PosStatus.POS_NO;
            txtBillNo = "";
            GetTrnTenderSeqList();
        }

        public override bool SetIView(IView view)
        {
            _view = (IOrderPayOutputMngView)view;
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

        public ICommand ButtonCommand => new RelayCommand<Button>(async (button) =>
        {
            if (button.Tag == null) return;

            switch (button.Tag.ToString())
            {
                case "btnSearchPos":
                    System.Windows.Point pointPos = _view.buttonPosition;
                    DialogHelper.ShowDialogWithCoords(typeof(SelectBoxViewModel), 50, 350, pointPos.X - 50, pointPos.Y + 30);
                    break;
                case "btnCal":
                    System.Windows.Point point = _view.buttonPosition;
                    DialogHelper.ShowDialogWithCoords(typeof(CalendarViewModel), 450, 480, point.X - 450, point.Y + 30, txtSaleDate);
                    break;
                case "btnSearch":
                    GetTrnTenderSeqList();
                    break;
                case "btnRePrint":
                    if (SelectedItem == null) return;
                    printService.PrintReceipt(PrintOptions.Normal, SelectedItem.SHOP_CODE, SelectedItem.POS_NO, SelectedItem.SALE_DATE, SelectedItem.BILL_NO, true, true);
                    printService.EndPrinting();
                    break;
                default:
                    break;
            }
        });

        private void GetTrnTenderSeqList()
        {
            var result = orderPayOutputMngService.GetTrnTenderSeqList(txtSaleDate.ToString("yyyyMMdd"), txtPosNo == "전체" ? "" : txtPosNo, txtBillNo).Result;
            if (result == null)
            {
                TrnTenderSeqItemList.Clear();
                return;
            }

            TrnTenderSeqItemList = new ObservableCollection<RECEIPT_MANAGER_ITEM>(result);           
            if (TrnTenderSeqItemList.Count > 0)
            {
                _view.ScrollToEnd();
                SelectedItem = TrnTenderSeqItemList[TrnTenderSeqItemList.Count - 1];
            }
        }

        private void ShowPreviewPrint()
        {
            if (SelectedItem == null) return;
            ReceiptContents = printService.PreviewReceipt(SelectedItem.SHOP_CODE, SelectedItem.POS_NO, SelectedItem.SALE_DATE, SelectedItem.BILL_NO, true);
            NotifyOfPropertyChange(nameof(ReceiptContents));
        }
    }
}
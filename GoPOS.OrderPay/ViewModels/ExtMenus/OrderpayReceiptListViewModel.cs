using Caliburn.Micro;
using GoPOS.Common.Interface.Model;
using GoPOS.Common.ViewModels;
using GoPOS.Helpers;
using GoPOS.Helpers.CommandHelper;
using GoPOS.Models;
using GoPOS.Models.Common;
using GoPOS.Models.Custom.OrderMng;
using GoPOS.Service;
using GoPOS.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;

namespace GoPOS.ViewModels
{
    public class OrderpayReceiptListViewModel : BaseItemViewModel, IDialogViewModel
    {
        //private IOrderpayReceiptListService _services;

        private ObservableCollection<RECEIPT_MANAGER_ITEM> _whateverItems;
        public ObservableCollection<RECEIPT_MANAGER_ITEM> WhateverItems
        {
            get { return _whateverItems; }
            set
            {
                if (_whateverItems != value)
                {
                    _whateverItems = value;
                    NotifyOfPropertyChange(() => WhateverItems);
                }
            }
        }

        private RECEIPT_MANAGER_ITEM _selectedItem;
        public RECEIPT_MANAGER_ITEM SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                _selectedItem = value;
                NotifyOfPropertyChange(() => SelectedItem);
            }
        }

        public RECEIPT_MANAGER_ITEM trN_TENDERSEQ { get; set; }

        public OrderpayReceiptListViewModel(IWindowManager windowManager, IEventAggregator eventAggregator) : base(windowManager, eventAggregator)
        {
            this._eventAggregator.SubscribeOnPublishedThread(this);
            this.ViewInitialized += OrderpayReceiptListModel_ViewInitialized;
            this.ViewLoaded += OrderpayReceiptListModel_ViewLoaded;
        }

        private void OrderpayReceiptListModel_ViewInitialized(object? sender, EventArgs e)
        {
            NotifyOfPropertyChange(() => WhateverItems);
        }

        private void OrderpayReceiptListModel_ViewLoaded(object? sender, EventArgs e)
        {
            NotifyOfPropertyChange(() => WhateverItems);
        }

        public override async void SetData(object data)
        {
            object[] datas = (object[])data;
            _whateverItems = new ObservableCollection<RECEIPT_MANAGER_ITEM>((IEnumerable<RECEIPT_MANAGER_ITEM>)datas[0]);
        }

        public ICommand ButtonCommand => new RelayCommand<Button>(async (button) =>
        {
            if (button.Tag == null) return;

            switch (button.Tag.ToString())
            {
                case "btnOk":
                    if (SelectedItem == null) return;
                    await _eventAggregator.PublishOnUIThreadAsync(new ReceiptMngListEvent()
                    {
                        Item = SelectedItem,
                        EventType = 1
                    });
                    this.TryCloseAsync();
                    break;
                default:
                    break;
            }
        });

        public Dictionary<string, object> DialogResult
        {
            get; set;
        }
    }
}

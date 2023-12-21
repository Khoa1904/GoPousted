using Caliburn.Micro;
using Dapper;
using GoPOS.Common.Interface.View;
using GoPOS.Helpers;
using GoPOS.Helpers.CommandHelper;
using GoPOS.Models;
using GoPOS.Models.Common;
using GoPOS.OrderPay.Interface.View;
using GoPOS.Payment.Interface.View;
using GoPOS.Services;

using GoPOS.Views;
using GoShared.Helpers;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Numerics;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using static AutoMapper.Internal.ExpressionFactory;
using System.Windows.Markup;
using System.Data.Entity.Core.Metadata.Edm;
using GoPOS.Models.Custom.OrderMng;
using GoPOS.Common.Interface.Model;

namespace GoPOS.ViewModels
{
    public class OrderPaySoldOutViewModel : OrderPayChildViewModel
    {

        private IOrderPaySoldOutView _view;
        private readonly IOrderPaySoldOutService _orderPaySoldOutService;
        private IOrderPayMainViewModel orderPayMainViewModel;

        public override object ActivateType { get => "ExceptKeyPad"; }

        private int counter = 1;
        public int AutoIncrementNo => counter++;

        private bool _isSoldOut;
        public bool IsSoldOut
        {
            get { return _isSoldOut; }
            set
            {
                if (!_isSell && !_isAll) return;
                _isSoldOut = value;
                if (value)
                {
                    IsSell = false;
                    IsAll = false;
                }
                NotifyOfPropertyChange(() => IsSell);
                NotifyOfPropertyChange(() => IsAll);
                NotifyOfPropertyChange(() => IsSoldOut);
            }
        }
        private bool _isSell;
        public bool IsSell
        {
            get { return _isSell; }
            set
            {
                if (!_isSoldOut && !_isAll) return;
                _isSell = value;
                if (value)
                {
                    IsSoldOut = false;
                    IsAll = false;
                }
                NotifyOfPropertyChange(() => IsSell);
                NotifyOfPropertyChange(() => IsAll);
                NotifyOfPropertyChange(() => IsSoldOut);
            }
        }

        private bool _isAll;
        public bool IsAll
        {
            get { return _isAll; }
            set
            {
                if (!_isSoldOut && !_isSell && !firstTime) return;
                _isAll = value;
                if (value)
                {
                    firstTime = false;
                    IsSoldOut = false;
                    IsSell = false;
                }
                NotifyOfPropertyChange(() => IsSell);
                NotifyOfPropertyChange(() => IsAll);
                NotifyOfPropertyChange(() => IsSoldOut);
            }
        }

        private string _txtPrdName;
        public string txtPrdName
        {
            get { return _txtPrdName; }
            set
            {
                _txtPrdName = value;
                if (txtPrdName != "")
                    txtPrdCode = "";
                NotifyOfPropertyChange(() => txtPrdName);
            }
        }

        private string _txtPrdCode;
        public string txtPrdCode
        {
            get { return _txtPrdCode; }
            set
            {
                _txtPrdCode = value;
                if (txtPrdCode != "")
                    txtPrdName = "";
                NotifyOfPropertyChange(() => txtPrdCode);
            }
        }

        private SOLD_OUT_ITEM _selectedItem;
        public SOLD_OUT_ITEM SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                _selectedItem = value;
                NotifyOfPropertyChange(() => SelectedItem);
            }
        }

        public OrderPaySoldOutViewModel(IWindowManager windowManager, IEventAggregator eventAggregator,
            IOrderPaySoldOutService orderPaySoldOutService) :
            base(windowManager, eventAggregator)
        {
            this.ViewInitialized += OrderPaySoldOutViewModel_ViewInitialized;
            this.ViewLoaded += OrderPaySoldOutViewModel_ViewLoaded;
            _orderPaySoldOutService = orderPaySoldOutService;
        }

        public override bool SetIView(IView view)
        {
            _view = (IOrderPaySoldOutView)view;
            return base.SetIView(view);
        }

        private void OrderPaySoldOutViewModel_ViewInitialized(object? sender, EventArgs e)
        {
            orderPayMainViewModel = (IOrderPayMainViewModel)Parent;
        }

        private void OrderPaySoldOutViewModel_ViewLoaded(object? sender, EventArgs e)
        {
            LoadView();
        }

        private bool firstTime = true;
        private async void LoadView()
        {
            firstTime = true;
            if (ItemsCollection != null)
                ItemsCollection.Clear();
            IsAll = true;
            counter = 1;
            txtPrdCode = string.Empty;
            txtPrdName = string.Empty;
        }

        private ObservableCollection<SOLD_OUT_ITEM> _itemList;
        public ObservableCollection<SOLD_OUT_ITEM> ItemsCollection
        {
            set
            {
                _itemList = value;

                NotifyOfPropertyChange(() => ItemsCollection);

            }
            get
            {
                if (_itemList == null)
                {
                    _itemList = new ObservableCollection<SOLD_OUT_ITEM>();
                }
                return _itemList;
            }
        }

        private async void GetProductList()
        {
            List<SOLD_OUT_ITEM> result = null;
            try
            {
                result = await _orderPaySoldOutService.GetOrderPaySoldOutProductsAsync(IsAll ? null : (IsSell ? "N" : "Y"), txtPrdCode, txtPrdName);
                ItemsCollection = new ObservableCollection<SOLD_OUT_ITEM>(result);
                counter = 1;
            }
            catch (Exception a)
            {
                LogHelper.Logger.Error(a.Message);

            }
        }

        private bool SaveProductStock()
        {
            var result = _orderPaySoldOutService.SaveProductStock(ItemsCollection).Result;
            if (result.ResultType != EResultType.SUCCESS)
            {
                DialogHelper.MessageBox(result.ResultMessage, GMessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            return true;
        }

        public ICommand ButtonCommand => new RelayCommand<Button>(async (button) =>
        {
            if (button.Tag == null) return;

            switch (button.Tag.ToString())
            {
                case "btnSearch":
                    GetProductList();
                    break;
                case "btnCancle":
                    this.TryCloseAsync(true);
                    //ItemsCollection = LastItemList;
                    //NotifyOfPropertyChange(() => ItemsCollection);
                    break;
                case "btnSetSale":
                    if (SelectedItem == null) return;
                    SelectedItem.STOCK_OUT_YN = "N";
                    break;
                case "btnSetOut":
                    if (SelectedItem == null) return;
                    SelectedItem.STOCK_OUT_YN = "Y";
                    break;
                case "btnApply":
                    if (SaveProductStock())
                    {
                        this.TryCloseAsync(true);
                    }
                    break;
                default:
                    break;
            }
        });
    }
}

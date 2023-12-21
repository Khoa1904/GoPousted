using Caliburn.Micro;
using Dapper;
using GoPOS.Common.Interface.Model;
using GoPOS.Common.Interface.View;
using GoPOS.Common.ViewModels;
using GoPOS.Helpers;
using GoPOS.Helpers.CommandHelper;
using GoPOS.Models;
using GoPOS.Models.Common;
using GoPOS.Models.Config;
using GoPOS.OrderPay.Interface.View;
using GoPOS.Service;
using GoPOS.Services;

using GoPOS.ViewModels;
using GoPOS.Views;
using GoShared.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Security.Policy;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Markup;
using static System.Net.Mime.MediaTypeNames;


/* 
주문 > 확장메뉴 > 상품조회
*/
/// <summary>
/// 화면명 : 상품조회 201 (확장)
/// 작성자 : 박현재
/// </summary>
namespace GoPOS.ViewModels
{

    public class OrderPayGoodSearchViewModel : OrderPayChildViewModel, IOrderPayGoodSearchView
    {
        private IOrderPayGoodsService goodsService;
        private List<MST_INFO_PRODUCT>? _ProductList = null;
        //private MST_INFO_PRODUCT ChosenPrd = null;
        private IOrderPayGoodSearchView menuView = null;
        private int _index = -1;
        //private OrderPayChildUpdatedEventArgs closeEventARRGGG = null;
        private string pRD_CODE = "";
        private string pRD_NAME ="";
        private IOrderPayMainViewModel orderPayMainViewModel;

        public override object ActivateType { get => "ExceptKeyPad"; }

        public List<MST_INFO_PRODUCT>? ProductList
        {
            get => _ProductList;
            set
            {
                _ProductList = value;
                NotifyOfPropertyChange(() => ProductList);
            }
        }
        public string ProductCode
        {
            get  { return pRD_CODE; }
            set
            {
                try
                {
                    pRD_CODE = value;
                    if (!string.IsNullOrEmpty(pRD_CODE))
                        ProductName = "";
                    NotifyOfPropertyChange(() => ProductCode);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            }
        }
        public string ProductName
        {
            get {return pRD_NAME; }
            set
            {
                try
                {
                    pRD_NAME = value;
                    if (!string.IsNullOrEmpty(pRD_NAME))
                        ProductCode = "";
                    NotifyOfPropertyChange(() => ProductName);
                }
                catch(Exception ex) {
                    Console.WriteLine(ex.ToString());   
                }
            }
        }
        public int Index
        {
            get
            {
                return _index;
            }
            set
            {
                _index = value;
                NotifyOfPropertyChange(() => Index);
            }
        }
        public OrderPayGoodSearchViewModel(IWindowManager windowManager, IEventAggregator eventAggregator,
            IOrderPayGoodsService goodsService)
            : base(windowManager, eventAggregator)
        {
            this.ProductList = new List<MST_INFO_PRODUCT>();
            this.goodsService = goodsService;
            this.ViewInitialized += OrderPayGoodSearchViewModel_ViewInitialized;
            this.ViewUnloaded += UnloadData;
        }

        private void OrderPayGoodSearchViewModel_ViewInitialized(object? sender, EventArgs e)
        {
            orderPayMainViewModel = (IOrderPayMainViewModel)Parent;
        }

        private void UnloadData (object? sender, EventArgs e)
        {
            ProductList = null;
        }

        public ICommand ButtonCommand => new RelayCommand<Button>(ButtonCommandControl);

        public TextBox tbPRDCODE => throw new NotImplementedException();

        public TextBox tbPRDNAME => throw new NotImplementedException();

        public SynchronizationContext SyncContext => throw new NotImplementedException();

        public IViewModel ViewModel => throw new NotImplementedException();

        private void ButtonCommandControl(Button button)
        {

            switch (button.Tag.ToString())
            {
                case "ButtonClose":
                    this.TryCloseAsync();
                    break;

                case "btnSearch":
                    if ((this.ProductCode == null || this.ProductCode =="") && (this.ProductName == null || this.ProductName == "") )
                    {
                        orderPayMainViewModel.StatusMessage = "조회하고자 하는 상품 코드/명을 입려하여 주십시오.";
                        return;
                    }
                    else
                    {
                        ProductList = goodsService.GetProductList2(this.ProductCode, this.ProductName).Result.Item1;
                    }
                    break;

                case "btnSelect":
                    ClickAdditem();

                    break;

                default:
                    break;
            }
        }

        public override bool SetIView(IView view)
        {
            menuView = view as IOrderPayGoodSearchView;
            return base.SetIView(view);
        }

        public void EnableControl(bool enable)
        {
            throw new NotImplementedException();
        }

        public void Translate()
        {
            throw new NotImplementedException();
        }

        public void ClickAdditem()
        {
            if (Index == -1)
            {
                orderPayMainViewModel.StatusMessage = "상품은 먼저 선택하여 주십시오.";
            }
            else
            {
                orderPayMainViewModel.ItemGrid_OnItemAdded(ProductList[Index]);
            }
            
        }
    }
}
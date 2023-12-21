using Caliburn.Micro;
using Dapper;
using ESCPOS_NET.Emitters.BaseCommandValues;
using GoPOS.Common.Interface.Model;
using GoPOS.Common.Interface.View;
using GoPOS.Common.Service;
using GoPOS.Common.ViewModels;
using GoPOS.Helpers;
using GoPOS.Helpers.CommandHelper;
using GoPOS.Models;
using GoPOS.Models.Common;
using GoPOS.Models.Custom.OrderMng;
using GoPOS.Models.Custom.Payment;
using GoPOS.OrderPay.Interface.View;
using GoPOS.Service.Service;
using GoPOS.Services;

using GoPOS.Views;
using GoShared.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Data.Entity.Core.Metadata.Edm;
using System.Diagnostics.Metrics;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Input;
using static GoPosVanAPI.Api.VanAPI;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;


/*
 주문 > 확장메뉴 > 판매현황

 */
/// <summary>
/// 화면명 : 판매 현황 확장 253
/// 작성자 : 김형석 
/// </summary>
namespace GoPOS.ViewModels
{

    public class OrderPaySaleStatusViewModel : BaseItemViewModel, IDialogViewModel
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        private IPOSPrintService posPrintService;
        private ISalesGiftSaleService saleService;
        /// <summary>
        /// Property
        /// </summary>

        private List<FINAL_SETT> finalss = null;
        public List<FINAL_SETT>? FINAL
        {
            get => finalss;
            set
            {
                finalss = value;
                NotifyOfPropertyChange(() => FINAL);
            }
        }
        private List<PRODUCT_SALE> productList = null;
        public List<PRODUCT_SALE>? PRODUCTLIST
        {
            get => productList;
            set
            {
                productList = value;
                NotifyOfPropertyChange(() => PRODUCTLIST);
            }
        }
        private List<SALE_BY_TYPE2> listBySaleType = null;
        public List<SALE_BY_TYPE2> LISTBYSALETYPE
        {
            get => listBySaleType;
            set
            {
                listBySaleType = value;
                NotifyOfPropertyChange(() => LISTBYSALETYPE);
            }
        }
        private ObservableCollection<PRODUCT_SALE> _CollectionProductList;
        public ObservableCollection<PRODUCT_SALE> CollectionProductList
        {
            get => _CollectionProductList;
            set
            {
                _CollectionProductList = value;
                NotifyOfPropertyChange(() => CollectionProductList);
            }
        }
        private ObservableCollection<SALE_BY_TYPE2> _CollectionSaletypeList;
        public ObservableCollection<SALE_BY_TYPE2> CollectionSaletypeList
        {
            get => _CollectionSaletypeList;
            set
            {
                _CollectionSaletypeList = value;
                NotifyOfPropertyChange(() => CollectionSaletypeList);
            }
        }
        private List<TRN_HEADER> _trnHeader  = null;
        public List<TRN_HEADER> TRNHeader
        {
            get { return _trnHeader; } set
            {
                _trnHeader = value;
                NotifyOfPropertyChange(() => TRNHeader);
            }
        }
        private SHOP_SALE_STATS _Receivable  {get;set;} 
        private SHOP_SALE_STATS _Sales       {get;set;} 
        private SHOP_SALE_STATS _Unsale      {get;set;} 
        private SHOP_SALE_STATS _ReceiptCnt  {get;set;} 
        private SHOP_SALE_STATS _ActualSale  {get;set;} 
        private SHOP_SALE_STATS _Subtotal    {get;set;} 
        public SHOP_SALE_STATS Receivable {get => _Receivable; set { _Receivable =value; NotifyOfPropertyChange(()=> Receivable);}}
        public SHOP_SALE_STATS Sales      {get => _Sales     ; set { _Sales      =value; NotifyOfPropertyChange(()=> Sales     );}}
        public SHOP_SALE_STATS Unsale     {get => _Unsale    ; set { _Unsale     =value; NotifyOfPropertyChange(()=> Unsale    );}}
        public SHOP_SALE_STATS ReceiptCnt {get => _ReceiptCnt; set { _ReceiptCnt =value; NotifyOfPropertyChange(()=> ReceiptCnt);}}
        public SHOP_SALE_STATS ActualSale {get => _ActualSale; set { _ActualSale =value; NotifyOfPropertyChange(()=> ActualSale);}}
        public SHOP_SALE_STATS Subtotal   {get => _Subtotal  ; set { _Subtotal = value; NotifyOfPropertyChange(() => Subtotal)  ;}}
        private decimal  _totalProQty {get;set;} = 0;
        private decimal  _totalProAmt {get;set;} = 0;
        public decimal TotalProQty { get => _totalProQty ; set { _totalProQty = value; NotifyOfPropertyChange (() => TotalProQty); }}
        public decimal TotalProAmt { get => _totalProAmt ; set { _totalProAmt = value; NotifyOfPropertyChange (() => TotalProAmt); }}
        private decimal _totalPaytQty { get; set; } = 0;
        private decimal _totalPaytAmt { get; set; } = 0;
        public decimal TotalPaytQty { get => _totalPaytQty; set { _totalPaytQty = value; NotifyOfPropertyChange(() => TotalPaytQty); } }
        public decimal TotalPaytAmt { get => _totalPaytAmt; set { _totalPaytAmt = value; NotifyOfPropertyChange(() => TotalPaytAmt); } }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="windowManager"></param>
        /// <param name="eventAggregator"></param>
        /// <param name="saleService"></param>
        /// <param name="orderPaySaleStatusService"></param>
        public OrderPaySaleStatusViewModel(IWindowManager windowManager, IEventAggregator eventAggregator, ISalesGiftSaleService saleService,
            IOrderPaySaleStatusService orderPaySaleStatusService) : base(windowManager, eventAggregator)
        {
            this.ViewInitialized += OrderPaySaleStatusViewModel_ViewInitialized;
            this.ViewLoaded += OrderPaySaleStatusViewModel_ViewLoaded;
            this.saleService = saleService;
        }

        private void OrderPaySaleStatusViewModel_ViewInitialized(object? sender, EventArgs e)
        {
            GetDataGrid();
            GetDetailData();
        }

        private bool firstTime = true;
        private void OrderPaySaleStatusViewModel_ViewLoaded(object? sender, EventArgs e)
        {
            firstTime = true;
            GetDataGrid();
            GetDetailData();

        }

        public ICommand ButtonCommand => new RelayCommand<System.Windows.Controls.Button>(async (button) =>
        {
            if (button.Tag == null) return;

            switch (button.Tag.ToString())
            {
                case "btnSearch":
                    break;
                case "btnCancle":
                    break;
                default:
                    break;
            }
        });
        public Dictionary<string, object> DialogResult { get; set; }
        public override void SetData(object data)
        {

            object[] datas = (object[])data;
        }

        /// <summary>
        /// Function
        /// </summary>
        private  void GetDataGrid()
        {
            //TotalProAmt = 0;
            //TotalProQty = 0;
            //LISTBYSALETYPE = saleService.PmttypeStatusOfDay().Result.Item1;
            //IoC.Get<OrderPaySaleStatusView>().PmdListView.ItemsSource = LISTBYSALETYPE;
            //PRODUCTLIST    = saleService.ProductSaleOfDay(RadioProd).Result.Item1;
            //IoC.Get<OrderPaySaleStatusView>().IncListView.ItemsSource = PRODUCTLIST;
            //TRNHeader      = saleService.GetTodayTransaction().Result.Item1;       
            //for(int i = 0; i < PRODUCTLIST.Count; i++)
            //{
            //    TotalProAmt += PRODUCTLIST[i].Sale_amt;
            //    TotalProQty += PRODUCTLIST[i].Qty;
            //}
            //for (int i = 0; i < LISTBYSALETYPE.Count; i++)
            //{
            //    TotalPaytAmt += LISTBYSALETYPE[i].Pay_Amt;
            //    TotalPaytQty += LISTBYSALETYPE[i].Qty;
            //}
            getTransaction();
            GetProdList(null);
            GetPaymentList();
        }
        private void GetProdList(string Radio)
        {
            if(PRODUCTLIST!=null)
            {
                PRODUCTLIST.Clear();
            }      
            TotalProAmt = 0;
            TotalProQty = 0;
            PRODUCTLIST = saleService.ProductSaleOfDay(Radio).Result.Item1;
            for (int i = 0; i < PRODUCTLIST.Count; i++)
            {
                TotalProAmt += PRODUCTLIST[i].Sale_amt;
                TotalProQty += PRODUCTLIST[i].Qty;
            }
            IoC.Get<OrderPaySaleStatusView>().IncListView.ItemsSource = PRODUCTLIST;
        }
        private void GetPaymentList()
        {
            if (LISTBYSALETYPE != null)
            {
                LISTBYSALETYPE.Clear();
            }
            TotalPaytAmt = 0;
            TotalPaytQty = 0;
            LISTBYSALETYPE = saleService.PmttypeStatusOfDay().Result.Item1;
            for (int i = 0; i < LISTBYSALETYPE.Count; i++)
            {
                TotalPaytAmt += LISTBYSALETYPE[i].Pay_Amt;
                TotalPaytQty += LISTBYSALETYPE[i].Qty;
            }
            IoC.Get<OrderPaySaleStatusView>().PmdListView.ItemsSource = LISTBYSALETYPE;
        }
        private void getTransaction()
        {
            if (TRNHeader != null) { TRNHeader.Clear(); }
            TRNHeader = saleService.GetTodayTransaction().Result.Item1;
            GetDetailData();
        }

        private void GetDetailData()
        {

             Receivable = new()
            {
                Gubun = "미 결 제",
                Case_Count = 0,
                Customer_Count = 0,
                Amount = 0
            };
             Sales = new()
            {
                Gubun = "매 출",
                Case_Count = TRNHeader.Where(z => z.SALE_YN == "Y").Count(),
                Customer_Count = TRNHeader.Where(z => z.SALE_YN == "Y").Count(),
                Amount = TRNHeader.Where(z => z.SALE_YN == "Y").Sum(z => z.TOT_SALE_AMT)
            };
            NotifyOfPropertyChange(() => Sales);
             Unsale = new()
            {
                Gubun = "반 품",
                Case_Count = TRNHeader.Where(z => z.SALE_YN == "N").Count(),
                Customer_Count = TRNHeader.Where(z => z.SALE_YN == "N").Count(),
                Amount = TRNHeader.Where(z => z.SALE_YN == "N").Sum(z => z.TOT_SALE_AMT)
            };
             ReceiptCnt = new()
            {
                Gubun = "영수건수",
                Case_Count = TRNHeader.Count(),
                Customer_Count = TRNHeader.Count(),
                Amount = TRNHeader.Sum(z => z.TOT_SALE_AMT)
            };
             ActualSale = new()
            {
                Gubun = "실 매 출",
                Case_Count = TRNHeader.Where(z => z.SALE_YN == "Y").Count() - TRNHeader.Where(z => z.SALE_YN == "N").Count(),
                Customer_Count = TRNHeader.Where(z => z.SALE_YN == "Y").Count() - TRNHeader.Where(z => z.SALE_YN == "N").Count(),
                Amount = TRNHeader.Where(z => z.SALE_YN == "Y").Sum(z => z.TOT_SALE_AMT) - TRNHeader.Where(z => z.SALE_YN == "N").Sum(z => z.TOT_SALE_AMT)
            };
             Subtotal = new()
            {
                Gubun = "(A)소계 (실매출+미결제)",
                Case_Count = ActualSale.Case_Count + Receivable.Case_Count,
                Customer_Count = ActualSale.Customer_Count + Receivable.Customer_Count,
                Amount = ActualSale.Amount + Receivable.Amount
            };            
        }

        public ICommand CheckboxCommand => new RelayCommand<CheckBox>(CheckboxCommandCenter);
        private void CheckboxCommandCenter(CheckBox ck)
        {
            if(ck.Name == null)
            {
                return;
            }
            switch(ck.Name.ToString())
            {
                case "Pro_All":
                    GetProdList(null);
                    break;
                case "Pro_Undone":
                    GetProdList("00");
                    break;
                case "Pro_Done":
                    GetProdList(null);
                    break;
                case "Stats_all":
                    getTransaction();
                    break;
                case "Stats_process":
                    getTransaction();
                    break;
                case "Stats_undone":
                    getTransaction();
                    break;
                case "Stats_done":
                    getTransaction();
                    break;

                default: break;
            }
        }

        public void EnableControl(bool enable)
        {
            throw new NotImplementedException();
        }

        public void Translate()
        {
            throw new NotImplementedException();
        }
    }
}
            
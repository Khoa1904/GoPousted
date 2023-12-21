using Caliburn.Micro;
using FirebirdSql.Data.Services;
using GoPOS.Common.Interface.Model;
using GoPOS.Common.Interface.View;
using GoPOS.Helpers;
using GoPOS.Helpers.CommandHelper;
using GoPOS.Models;
using GoPOS.Models.Common;
using GoPOS.Models.Custom.Payment;
using GoPOS.Payment.Interface.View;
using GoPOS.Service;
using GoPOS.Service.Service.Payment;
using GoPOS.Services;

using GoPOS.Views;
using GoShared.Helpers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Globalization;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;


namespace GoPOS.ViewModels
{
    public class OrderPayCouponViewModel : OrderPayChildViewModel
    {
        private readonly IOrderPayCouponService CouponService;
        private IOrderPayMainViewModel orderPayMainViewModel;
        private IOrderPayCouponView _view;

        private int couponCode = 0;
        private int totalPage = 0;
        private int pageCnt = 12;
        private int iSelected = 1;

        private int seqNo = 1;
        private OrderPayChildUpdatedEventArgs closeEventArgs = null;

        List<MST_INFO_COUPON> pInfoCouponList = new List<MST_INFO_COUPON>();
        private MST_INFO_COUPON _selectedItem;
        public MST_INFO_COUPON SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                _selectedItem = value;
                GetProductName(Convert.ToInt32(_selectedItem.TK_CPN_CODE));
                ClassificDiscount = GetClassificDiscount(Convert.ToInt32(_selectedItem.CPN_DC_FLAG), _selectedItem.CPN_DC_RATE);
                NotifyOfPropertyChange(() => SelectedItem);
            }
        }
        private string _couponName;
        public string COUPON_NAME
        {
            get { return _couponName; }
            set
            {
                _couponName = value;
                NotifyOfPropertyChange(() => COUPON_NAME);
            }
        }

        public string ClassificDiscount { get; set; }

        private string _proName;
        public string ProName
        {
            get { return _proName; }
            set
            {
                _proName = value;
                NotifyOfPropertyChange(() => ProName);
            }
        }

        private int _currentPage;
        public int CurrentPage
        {
            get { return _currentPage; }
            set
            {
                _currentPage = value;
                SetTableCouponTicket();
                NotifyOfPropertyChange(() => CurrentPage);
            }
        }

        public OrderPayCouponViewModel(IWindowManager windowManager, IEventAggregator eventAggregator,
            IOrderPayCouponService couponService) : base(windowManager, eventAggregator)
        {
            closeEventArgs = new OrderPayChildUpdatedEventArgs();
            this.ViewInitialized += OrderPayCardViewModel_ViewInitialized;
            this.ViewLoaded += OrderPayCouponViewModel_ViewLoaded;
            this.ViewUnloaded += OrderPayCouponViewModel_ViewUnloaded;
            this.CouponService = couponService;
        }

        private void OrderPayCouponViewModel_ViewLoaded(object? sender, EventArgs e)
        {
            closeEventArgs = new OrderPayChildUpdatedEventArgs();
            Init();
        }

        public override bool SetIView(IView view)
        {
            _view = (IOrderPayCouponView)view;
            return base.SetIView(view);
        }
        private void OrderPayCouponViewModel_ViewUnloaded(object? sender, EventArgs e)
        {
            _eventAggregator.PublishOnUIThreadAsync(closeEventArgs);
        }

        private async void Init()
        {
            CurrentPage = 1;
            await GetTableCoupon();
            await Task.Delay(1000);
        }

        private void OrderPayCardViewModel_ViewInitialized(object? sender, EventArgs e)
        {
            orderPayMainViewModel = (IOrderPayMainViewModel)this.Parent;
        }

        private async Task GetTableCoupon()
        {
            pInfoCouponList.Clear();
            try
            {
                List<MST_INFO_COUPON> pCouponList = await CouponService.GetCouponList();

                for (int i = 0; i < pCouponList.Count; i++)
                    pInfoCouponList.Add(pCouponList[i]);

                if (pInfoCouponList.Count > 0)
                {
                    SetTableCouponTicket();
                }
            }
            catch (Exception ex)
            {
                LogHelper.Logger.Error("Error : " + ex.Message);
            }
        }

        private void SetTableCouponTicket()
        {
            if (pInfoCouponList == null || pInfoCouponList.Count() == 0) return;
            if (pInfoCouponList.Count >= CurrentPage * pageCnt)
            {
                List<MST_INFO_COUPON> subList = pInfoCouponList.GetRange((CurrentPage - 1) * pageCnt + 1, 12);
                _view.SetTableCouponValue(subList, iSelected);
            }
            else
            {
                List<MST_INFO_COUPON> subList = pInfoCouponList;
                _view.SetTableCouponValue(subList, iSelected);
            }
        }

        List<ORDER_COUPON_DETAIL> lsCouponDetail = new List<ORDER_COUPON_DETAIL>();
        private void SetListItemList(int Coupon)
        {
            if (pInfoCouponList == null || pInfoCouponList.Count == 0)
            {
                return;
            }
            else
            {
                SelectedItem = pInfoCouponList.FirstOrDefault(x => x.TK_CPN_CODE == Coupon.ToString());
                if (SelectedItem != null)
                {
                    if (SelectedItem.CPN_DC_FLAG == "1" || SelectedItem.CPN_DC_FLAG == "3" || SelectedItem.CPN_DC_FLAG == "4")
                    {
                        lsCouponDetail.Add(new ORDER_COUPON_DETAIL(SelectedItem) { NO = lsCouponDetail.Count.ToString(), TOT_TK_CPN_UAMT = "1", REMARK = "신규" });
                        _view.AddListItemList(lsCouponDetail);
                    }
                    else if (SelectedItem.CPN_DC_FLAG == "0" || SelectedItem.CPN_DC_FLAG == "2" || SelectedItem.CPN_DC_FLAG == "5")
                    {
                        lsCouponDetail.Clear();
                        lsCouponDetail.Add(new ORDER_COUPON_DETAIL(SelectedItem) { NO = "1", TOT_TK_CPN_UAMT = "1", REMARK = "신규" });
                        _view.AddListItemList(lsCouponDetail);
                    }
                }
            }
        }


        private void RemoveItemList()
        {
            if (pInfoCouponList == null || pInfoCouponList.Count == 0)
            {
                return;
            }
            else
            {
                pInfoCouponList.Remove(SelectedItem);
                _view.AddListItemList(lsCouponDetail);
            }
        }

        List<MST_INFO_PRODUCT> plsUsefulProduct = new List<MST_INFO_PRODUCT>();
        List<MST_INFO_PRODUCT>  plsEliminateProduct = new List<MST_INFO_PRODUCT>();
        private async void GetProductName(int cpnCode)
        {
            try
            {
                List<MST_INFO_PRODUCT> lsProductList = await CouponService.GetProductList(cpnCode.ToString());

                if (lsProductList == null || lsProductList.Count == 0) return;
                ProName = lsProductList[0].PRD_NAME + " 외" + (lsProductList.Count- 1).ToString() + "건";
            }
            catch (Exception ex)
            {
                LogHelper.Logger.Error("Error : " + ex.Message);
            }
        }

        private string GetClassificDiscount(int cPN_DC_FLAG, decimal rateOrMoney)
        {
            switch (cPN_DC_FLAG)
            {
                case 0:
                    return $"전체 - {rateOrMoney}% 할인";
                case 1:
                    return $"상품 - {rateOrMoney}% 할인";
                case 2:
                    return "전체 - " + rateOrMoney.ToString("N2");
                case 3: 
                    return "상품 - " + rateOrMoney.ToString("N2");
                case 4: 
                    return "상품 - " + rateOrMoney.ToString("N2");
                case 5:
                    return "전체 - 상품금액 - 할인";
                default:
                    return "";
            }
        }

        List<MST_INFO_COUPON> tmpInfoCoupon = new List<MST_INFO_COUPON>();
        public ICommand ButtonCommand => new RelayCommand<Button>((button) =>
        {
            if (button.Tag == null) return;
            switch (button.Tag.ToString())
            {
                case "btnRemove":
                    if (SelectedItem != null)
                        RemoveItemList();
                    break;
                case "btUp":
                    if (CurrentPage > 1)
                        CurrentPage--;
                    break;
                case "btDn":
                    if (CurrentPage < totalPage)
                        CurrentPage++;
                    break;
                case "btnResult":
                    lsCouponDetail.Clear();
                    ReturnClose();
                    //_view.AddListItemList(lsCouponDetail);
                    break;
                case string s when s.StartsWith("btnCp_"):
                    iSelected = Convert.ToInt32(button.Name.Substring(button.Name.Length - 2));
                    string[] arrCouponCode = button.Tag.ToString().Split('_');
                    if (arrCouponCode.Length > 1)
                    {
                        int couponCode = Convert.ToInt32(arrCouponCode[1]);
                        COUPON_NAME = pInfoCouponList.FirstOrDefault(x => x.TK_CPN_CODE == couponCode.ToString()).TK_CPN_NAME.ToString();
                        SetTableCouponTicket();
                        SetListItemList(couponCode);
                        GetProductName(couponCode);
                    }
                    break;

                default:
                    break;
            }
        });

        /// <summary>
        /// gift_sale
        /// </summary>
        /// <param name="data"></param>
        private TRN_FOODCPN footTR = null;
        public override void SetData(object data)
        {
            footTR = new TRN_FOODCPN()
            {
                SHOP_CODE = DataLocals.PosStatus.SHOP_CODE,
                POS_NO = DataLocals.PosStatus.POS_NO,
                SALE_DATE = DataLocals.PosStatus.SALE_DATE,
                BILL_NO = DataLocals.PosStatus.BILL_NO,
                SEQ_NO = seqNo.ToString("d4"),
                REGI_SEQ = DataLocals.PosStatus.REGI_SEQ,
                TK_GFT_CODE = string.Empty,
                TK_GTF_BARCODE = string.Empty,
                //TK_GFT_UAMT = 0,
                //TK_GFT_AMT = 0,
                //REPAY_CSH_AMT = 0,
                //REPAY_GFT_AMT = 0,
                INSERT_DT = DateTime.Now.ToString("yyyyMMddHHmmss")
            };

            //object[] datas = (object[])data;
            //closeEventArgs.CallerId = (string)datas[0];
            //Amount = (decimal)datas[1];
            //giftTR.REPAY_GFT_AMT = Refun;

            //seqNo = (int)datas[2];
            seqNo++;
            //giftTR.SEQ_NO = seqNo.ToString("d4");
        }

        private void ReturnClose()
        {
            footTR.BILL_NO = DataLocals.PosStatus.BILL_NO;
            //footTR.REPAY_CSH_AMT = Balance;
            //footTR.REPAY_GFT_AMT = Refun;
            //footTR.TK_GFT_AMT = TotalVoucher;
            footTR.INSERT_DT = DateTime.Now.ToString("yyyyMMddHHmmss");

            var compPayInfo = new COMPPAY_PAY_INFO()
            {
                PAY_TYPE_CODE = "식권",
                PAY_CLASS_NAME = nameof(TRN_FOODCPN),
                APPR_NO = footTR.TK_GTF_BARCODE,
                APPR_AMT = footTR.TK_FOD_AMT,
                APPR_PROC_FLAG = "1"
            };

            List<object> returnPays = new List<object>();
            returnPays.Add(compPayInfo);
            returnPays.Add(footTR);

            closeEventArgs.Cancelled = false;
            closeEventArgs.ReturnData = returnPays.ToArray();
            closeEventArgs.ChildName = this.GetType().Name;

            this.DeactivateClose(false);
        }
    }
}
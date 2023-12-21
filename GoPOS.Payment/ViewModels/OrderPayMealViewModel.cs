using AutoMapper.Configuration.Conventions;
using Caliburn.Micro;
using Dapper;
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
using GoPOS.Services;

using GoPOS.Views;
using GoPosVanAPI.Common.Van.KOCES;
using GoShared.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Globalization;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;

/// <summary>
/// 화면명 : 카드결제
/// 작성자 : 김형석
/// 작성일 : 20230312
/// </summary>

namespace GoPOS.ViewModels
{
    public class OrderPayMealViewModel : OrderPayChildViewModel
    {
        private readonly IOrderPayMealService _mealService;
        private IOrderPayMainViewModel orderPayMainViewModel;
        private IOrderPayMealView _view;
        private string CALLER_ID = string.Empty;

        private TRN_FOODCPN MealTR = null;

        private int currentPage = 1;
        private int totalPage = 0;
        private int pageCnt = 12;
        private int iSelected = -1;

        private int seqNo = 1;

        List<MST_INFO_TICKET> pInfoTicketList = new List<MST_INFO_TICKET>();
        private OrderPayChildUpdatedEventArgs closeEventArgs = null;

        private SALES_GIFT_SALE _selectedItemRemove;
        public SALES_GIFT_SALE SelectedItemRemove
        {
            get { return _selectedItemRemove; }
            set
            {
                _selectedItemRemove = value;
                NotifyOfPropertyChange(() => SelectedItemRemove);
            }
        }

        private string mealTicketNo;
        public string MealTicketNo
        {
            get { return mealTicketNo; }
            set
            {
                mealTicketNo = value;
                NotifyOfPropertyChange(() => MealTicketNo);
            }
        }

        private decimal _remaining;
        public decimal Remaining
        {
            get { return _remaining; }
            set
            {
                if (value >= 0)
                {
                    _remaining = value;
                }
                else
                {
                    _remaining = 0;
                }
                NotifyOfPropertyChange(() => Remaining);
            }
        }

        private string _remark;
        public string Remark
        {
            get { return _remark; }
            set
            {
                _remark = value;
                NotifyOfPropertyChange(() => Remark);
            }
        }

        private decimal _amount;
        public decimal Amount
        {
            get { return _amount; }
            set
            {
                _amount = value;
                NotifyOfPropertyChange(() => Amount);
            }
        }

        public OrderPayMealViewModel(IWindowManager windowManager, IEventAggregator eventAggregator,
            IOrderPayMealService mealService) : base(windowManager, eventAggregator)
        {
            this.ViewInitialized += OrderPayMealViewModel_ViewInitialized;
            this.ViewLoaded += OrderPayMealViewModel_ViewLoaded;
            this._mealService = mealService;
        }

        private void OrderPayMealViewModel_ViewInitialized(object? sender, EventArgs e)
        {
            orderPayMainViewModel = (IOrderPayMainViewModel)Parent;
            closeEventArgs = new OrderPayChildUpdatedEventArgs();
        }

        private void OrderPayMealViewModel_ViewLoaded(object? sender, EventArgs e)
        {
            Init();
        }

        private async void Init()
        {
            iSelected = -1;
            Remark = "";
            Remaining = 0;
            MealTicketNo = "";
            tmpSALES_GIFT_SALE.Clear();
            _view.SetListItemList(tmpSALES_GIFT_SALE);
            await GetMealTicketList();
            await Task.Delay(1000);
        }

        public override bool SetIView(IView view)
        {
            _view = (IOrderPayMealView)view;
            return base.SetIView(view);
        }

        private async Task GetMealTicketList()
        {
            pInfoTicketList.Clear();
            try
            {
                List<MST_INFO_TICKET> pMealTickList = await _mealService.GetMealTicketList();

                for (int i = 0; i < pMealTickList.Count; i++)
                    pInfoTicketList.Add(pMealTickList[i]);

                if (pInfoTicketList.Count > 0)
                {
                    SetMealTicket();
                }
            }
            catch (Exception ex)
            {
                LogHelper.Logger.Error("Error : " + ex.Message);
            }
        }

        private void SetMealTicket()
        {
            if (pInfoTicketList == null || pInfoTicketList.Count() == 0) return;
            if (pInfoTicketList.Count >= currentPage * pageCnt)
            {
                List<MST_INFO_TICKET> subList = pInfoTicketList.GetRange((currentPage - 1) * pageCnt + 1, 12);
                _view.SetTicketValue(subList, iSelected);
            }
            else
            {
                List<MST_INFO_TICKET> subList = pInfoTicketList;
                _view.SetTicketValue(subList, iSelected);
            }
        }

        public ICommand ButtonCommand => new RelayCommand<Button>((button) =>
        {
            switch (button.Tag.ToString())
            {
                case "btnRemove":
                    if (SelectedItemRemove != null)
                        RemoteItemList(SelectedItemRemove.NO);
                    break;
                case "btUp":
                    if (currentPage > 1)
                        currentPage --;
                    break;
                case "btDn":
                    if (currentPage < totalPage)
                        currentPage++;
                    break;
                case string s when s.StartsWith("btnTk_"):
                    iSelected = Convert.ToInt32(button.Name.Substring(button.Name.Length - 1));
                    string[] arrGiftCode = button.Tag.ToString().Split('_');
                    if (arrGiftCode.Length > 1)
                    {
                        string GiftCode = arrGiftCode[1];
                        MealTR.TK_GFT_CODE = GiftCode;
                        SetMealTicket();
                        AddToList(GiftCode);
                        _view.SetListItemList(tmpSALES_GIFT_SALE);
                    }
                    break;
                case "btnResult":
                    if(tmpSALES_GIFT_SALE.Count == 0)
                    {
                        orderPayMainViewModel.StatusMessage = "식권 선택을 한 후 식권 결제 처리를 하세요";
                        return;
                    }
                    ReturnClose();
                    break;
                default:
                    break;
            }
        });

        #region Add Or Remove Item From list
        List<SALES_GIFT_SALE> tmpSALES_GIFT_SALE = new List<SALES_GIFT_SALE>();
        public void AddToList(string iIndex)
        {
            try
            {
                if (tmpSALES_GIFT_SALE.Sum(x => Convert.ToDecimal(x.TK_GFT_UPRC)) >= Amount) 
                {
                    orderPayMainViewModel.StatusMessage = "더 이상 식권등록을 할 수 없습니다.";
                    return;
                }
                MST_INFO_TICKET? InfoTicket = pInfoTicketList.FirstOrDefault(x => x.TK_GFT_CODE == iIndex.ToString());
                if (InfoTicket != null)
                {
                    tmpSALES_GIFT_SALE.Add(new SALES_GIFT_SALE
                    {
                        TK_GFT_UPRC = InfoTicket.TK_GFT_UPRC.ToString("N0"),
                        TK_GFT_CODE = InfoTicket.TK_GFT_CODE,
                        TK_GFT_NAME = InfoTicket.TK_GFT_NAME,
                        REMARK = Remark,
                        TK_GFT_UAMT = "1",
                        TOT_TK_GFT_UAMT = InfoTicket.TK_GFT_UPRC.ToString("N0")
                    });
                }

                Remaining = tmpSALES_GIFT_SALE.Sum(x => Convert.ToDecimal(x.TK_GFT_UPRC)) - Amount;
                Remark = "";
                SetNo();

            }
            catch (Exception ex)
            {
                LogHelper.Logger.Error("Error : " + ex.Message);
            }
        }

        public void RemoteItemList(string no)
        {
            try
            {
                tmpSALES_GIFT_SALE.Remove(tmpSALES_GIFT_SALE[Convert.ToInt16(no) -1]);
                SetNo();
                _view.SetListItemList(tmpSALES_GIFT_SALE);
                Remaining = tmpSALES_GIFT_SALE.Sum(x => Convert.ToDecimal(x.TK_GFT_UPRC)) - Amount;
            }
            catch (Exception ex)
            {
                LogHelper.Logger.Error("Error : " + ex.Message);
            }
        }

        private void SetNo()
        {
            if (tmpSALES_GIFT_SALE != null && tmpSALES_GIFT_SALE.Count > 0)
            {
                for (int i = 0; i < tmpSALES_GIFT_SALE.Count(); i++)
                {
                    tmpSALES_GIFT_SALE[i].NO = (i + 1).ToString();
                }
            }
        }
        #endregion
        public override void SetData(object data)
        {
            MealTR = new TRN_FOODCPN()
            {
                SHOP_CODE = DataLocals.PosStatus.SHOP_CODE,
                POS_NO = DataLocals.PosStatus.POS_NO,
                SALE_DATE = DataLocals.PosStatus.SALE_DATE,
                BILL_NO = DataLocals.PosStatus.BILL_NO,
                SEQ_NO = seqNo.ToString("d4"),
                REGI_SEQ = DataLocals.PosStatus.REGI_SEQ,
                TK_GFT_CODE = string.Empty,
                TK_GTF_BARCODE = string.Empty,
                TK_FOD_AMT = 0,
                TK_FOD_DC_AMT = 0,
                REPAY_CASH_AMT = 0,     //  remain
                ETC_AMT = 0,            // 짜투리액 

                INSERT_DT = DateTime.Now.ToString("yyyyMMddHHmmss")
            };

            object[] datas = (object[])data;
            CALLER_ID = (string)datas[0];
            Amount = (decimal)datas[1] == 0 ? orderPayMainViewModel.OrderSumInfo.EXP_PAY_AMT : (decimal)datas[1];

            seqNo = (int)datas[2];
            MealTR.SEQ_NO = seqNo.ToString("d4");
        }

        /// <summary>
        /// 
        /// </summary>
        private void ReturnClose()
        {
            bool isRepay = false;
            decimal tk_FotAmt = 0;
            if (Remaining > 0)
            {
                var res = DialogHelper.MessageBox("식권 결제액이 결제금액 보다 많습니다. 잔액처리를 선택하세요.", GMessageBoxButton.OKCancel, 
                    MessageBoxImage.Question, new string[] { "잔액환불", "자투리" });
                if(res != MessageBoxResult.OK && res != MessageBoxResult.Cancel)
                {
                    return;
                }
                isRepay = res == MessageBoxResult.OK;
            }

            decimal runningAmount = Amount;

            List<TRN_FOODCPN> arrMealTR = new List<TRN_FOODCPN>();

            for (int i = 0; i < tmpSALES_GIFT_SALE.Count; i++)
            {
                var mealItem = tmpSALES_GIFT_SALE[i];
                var mealStdAmt = mealItem.TK_GFT_UPRC.ToDecimal();
                TRN_FOODCPN arr = new TRN_FOODCPN {
                    SHOP_CODE = DataLocals.PosStatus.SHOP_CODE,
                    POS_NO = DataLocals.PosStatus.POS_NO,
                    SALE_DATE = DataLocals.PosStatus.SALE_DATE,
                    BILL_NO = DataLocals.PosStatus.BILL_NO,
                    REGI_SEQ = DataLocals.PosStatus.REGI_SEQ,
                    SALE_YN = "Y",
                    TK_GTF_BARCODE = string.Empty,
                    TK_FOD_DC_AMT = 0,
                    TK_GFT_CODE = mealItem.TK_GFT_CODE,
                    TK_FOD_UAMT = mealStdAmt,
                    TK_FOD_AMT = runningAmount > mealStdAmt ? mealStdAmt : runningAmount,
                    REPAY_CASH_AMT = runningAmount > mealStdAmt ? 0 : mealStdAmt - runningAmount,
                    ETC_AMT = 0,
                    SEQ_NO = (Convert.ToInt32(MealTR.SEQ_NO) + i).ToString("d4"),
                    INSERT_DT = DateTime.Now.ToString("yyyyMMddHHmmss")
                };

                runningAmount -= arr.TK_FOD_AMT;

                arrMealTR.Add(arr);
            }
            var last = arrMealTR.LastOrDefault();
            if (last != null)
            {
                last.ETC_AMT = isRepay ? 0 : Remaining;
                last.REPAY_CASH_AMT = isRepay ? Remaining : 0;
            }

            var compPayInfo = new COMPPAY_PAY_INFO()
            {
                PAY_TYPE_CODE = OrderPayConsts.PAY_MEAL,
                PAY_CLASS_NAME = nameof(TRN_FOODCPN),
                PAY_VM_NANE = this.GetType().Name,
                APPR_AMT = runningAmount > 0 ? Amount - runningAmount : Amount,
                APPR_PROC_FLAG = "1",
                PayDatas = arrMealTR.ToArray(),
            };

            if ("COMP_PAY".Equals(CALLER_ID))
            {
                IoC.Get<OrderPayCompPayViewModel>().UpdatePaymentTRN(this.GetType().Name, compPayInfo);
            }
            else
            {
                orderPayMainViewModel.UpdatePaymentTRN(this.GetType().Name, compPayInfo);
            }

            this.DeactivateClose(false);
        }
    }
}
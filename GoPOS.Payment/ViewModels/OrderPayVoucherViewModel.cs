using Caliburn.Micro;
using Dapper;
using FirebirdSql.Data.Services;
using GoPOS.Common.Interface.Model;
using GoPOS.Common.Interface.View;
using GoPOS.Helpers;
using GoPOS.Helpers.CommandHelper;
using GoPOS.Models;
using GoPOS.Models.Common;
using GoPOS.Models.Config;
using GoPOS.Models.Custom.Payment;
using GoPOS.Payment.Interface.View;
using GoPOS.Service;
using GoPOS.Services;
using GoPOS.ViewModels;
using GoPOS.Views;
using GoShared.Events;
using GoShared.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data;
using System.Diagnostics.Metrics;
using System.Globalization;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using static GoPOS.Function;

namespace GoPOS.ViewModels
{
    public class OrderPayVoucherViewModel : OrderPayChildViewModel
    {
        private IOrderPayVoucherView orderPayVoucherView;
        private readonly IOrderPayVoucherService _orderPayVoucherService;
        private IOrderPayMainViewModel orderPayMainViewModel;

        List<MST_INFO_TICKET_CLASS> pVoucherList1 = new List<MST_INFO_TICKET_CLASS>();
        List<MST_INFO_TICKET> pVoucherList2 = new List<MST_INFO_TICKET>();
        private string CALLER_ID = string.Empty;
        private TRN_GIFT giftTR = null;

        private int totalPage1 = 0;
        private int pageCnt1 = 6;
        private int iSelected = -1;

        private int totalPage2 = 0;
        private int pageCnt2 = 6;
        private int iSelected2 = -1;

        private int seqNo = 1;
        public OrderPayVoucherViewModel(IWindowManager windowManager, IEventAggregator eventAggregator,
               IOrderPayVoucherService orderPayVoucherService) : base(windowManager, eventAggregator)
        {
            this.ViewInitialized += OrderPayVoucherViewModel_ViewInitialized;
            this.ViewLoaded += OrderPayVoucherViewModel_ViewLoaded;
            _orderPayVoucherService = orderPayVoucherService;
            IsRemoveChecked = true;
        }

        public override bool SetIView(IView view)
        {
            orderPayVoucherView = (IOrderPayVoucherView)view;
            return base.SetIView(view);
        }

        private void OrderPayVoucherViewModel_ViewInitialized(object? sender, EventArgs e)
        {
            orderPayMainViewModel = (IOrderPayMainViewModel)Parent;
        }

        private void OrderPayVoucherViewModel_ViewLoaded(object? sender, EventArgs e)
        {
            LoadView();
        }

        private bool firstTime = true;
        private async void LoadView()
        {
            firstTime = true;
            Refun = 0;
            TotalVoucher = 0;
            Remark = "";
            IsRemoveChecked = true;
            iSelected = -1;
            iSelected2 = -1;
            tmpSALES_GIFT_SALE.Clear();
            tmpSALES_GIFT_SALE_RE_FUN.Clear();
            pVoucherList1.Clear();
            pVoucherList2.Clear();
            orderPayVoucherView.ResetTicketValue(pVoucherList2, iSelected2);
            orderPayVoucherView.ResetListItem1Value(tmpSALES_GIFT_SALE);
            orderPayVoucherView.ResetListItem2Value(tmpSALES_GIFT_SALE_RE_FUN);
            await GetTicketClassList();
            Balance = 0;
            CashRecOn = false;
            await Task.Delay(100);
        }

        private SALES_GIFT_SALE _selectedItem;
        public SALES_GIFT_SALE SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                _selectedItem = value;
                NotifyOfPropertyChange(() => SelectedItem);
            }
        }

        private SALES_GIFT_SALE _selectedItem1;
        public SALES_GIFT_SALE SelectedItem1
        {
            get { return _selectedItem1; }
            set
            {
                _selectedItem1 = value;
                NotifyOfPropertyChange(() => SelectedItem1);
            }
        }

        private int _currentPage1;
        public int CurrentPage1
        {
            get { return _currentPage1 < 1 ? 1 : _currentPage1; }
            set
            {
                _currentPage1 = value;
                iSelected = -1;
                SetTicketClass();
                NotifyOfPropertyChange(() => CurrentPage1);
            }
        }

        private int _currentPage2;
        public int CurrentPage2
        {
            get { return _currentPage2 < 1 ? 1 : _currentPage2; }
            set
            {
                _currentPage2 = value;
                iSelected2 = -1;
                SetTicket();
                NotifyOfPropertyChange(() => CurrentPage2);
            }
        }

        private bool _isRemoveChecked;
        public bool IsRemoveChecked
        {
            get { return _isRemoveChecked; }
            set
            {
                if (!_isReFunChecked && !firstTime) return;
                _isRemoveChecked = value;
                NotifyOfPropertyChange(() => IsRemoveChecked);
                if (value)
                {
                    IsReFunChecked = false;
                    firstTime = false;
                }
            }
        }

        private bool _isReFunChecked;
        public bool IsReFunChecked
        {
            get { return _isReFunChecked; }
            set
            {
                if (!_isRemoveChecked) return;
                _isReFunChecked = value;
                NotifyOfPropertyChange(() => IsReFunChecked);

                if (value)
                {
                    IsRemoveChecked = false;
                }
            }
        }

        private decimal _amount;
        public decimal Amount
        {
            get { return _amount; }
            set
            {
                if (_amount != value)
                {
                    _amount = value;
                    NotifyOfPropertyChange(() => Amount);
                    Balance = TotalVoucher - Amount - Refun;
                }
            }
        }

        private decimal _refun;
        public decimal Refun
        {
            get { return _refun; }
            set
            {
                if (_refun != value)
                {
                    _refun = value;
                    NotifyOfPropertyChange(() => Refun);
                    Balance = TotalVoucher - Amount - Refun;
                }
            }
        }

        private decimal _balance;
        public decimal Balance
        {
            get { return _balance; }
            set
            {
                if (value >= 0)
                {
                    _balance = value;
                }
                else
                {
                    _balance = 0;
                }
                NotifyOfPropertyChange(() => Balance);
            }
        }

        private decimal _totalVoucher;
        public decimal TotalVoucher
        {
            get { return _totalVoucher; }
            set
            {
                if (_totalVoucher != value)
                {
                    _totalVoucher = value;
                    NotifyOfPropertyChange(() => TotalVoucher);
                    Balance = TotalVoucher - Amount - Refun;
                }
            }
        }

        private string _remark;
        public string Remark
        {
            get { return _remark; }
            set
            {
                if (_remark != value)
                {
                    _remark = value;
                    NotifyOfPropertyChange(() => Remark);
                }
            }
        }

        public ICommand CheckBoxCommand => new RelayCommand<CheckBox>((chk) =>
        {
            CheckBox checkBox = (CheckBox)chk;
            bool isChecked = checkBox.IsChecked ?? false;
            if (checkBox.Name == "RemoveChecked")
                IsRemoveChecked = isChecked;
            else if (checkBox.Name == "ReFunChecked")
                IsReFunChecked = isChecked;
        });

        /// <summary>
        /// GetList
        /// </summary>
        public async Task GetTicketClassList()
        {
            try
            {
                pVoucherList1.Clear();
                DynamicParameters param = new();
                List<MST_INFO_TICKET_CLASS> result = new List<MST_INFO_TICKET_CLASS>();
                result = await _orderPayVoucherService.GetInfoTicketClass(param);

                for (int i = 0; i < result.Count; i++)
                    pVoucherList1.Add(result[i]);

                if (pVoucherList1.Count > 0)
                {
                    SetTicketClass();
                }
            }
            catch (Exception a)
            {
                LogHelper.Logger.Error(a.Message);

            }
        }

        public async Task GetTicketList(string classCode)
        {
            pVoucherList2.Clear();
            try
            {
                List<MST_INFO_TICKET> result = await _orderPayVoucherService.GetInfoTicket(classCode);
                for (int i = 0; i < result.Count; i++)
                    pVoucherList2.Add(result[i]);

                if (pVoucherList2.Count > 0)
                {
                    SetTicket();
                }
            }
            catch (Exception a)
            {
                LogHelper.Logger.Error(a.Message);

            }
        }

        private void SetTicketClass()
        {
            if (pVoucherList1 == null || pVoucherList1.Count() == 0) return;
            if (pVoucherList1.Count >= CurrentPage1 * pageCnt1)
            {
                List<MST_INFO_TICKET_CLASS> subList = pVoucherList1.GetRange((CurrentPage1 - 1) * pageCnt1 + 1, 6);
                orderPayVoucherView.SetTicketClassValue(subList, iSelected);
            }
            else
            {
                List<MST_INFO_TICKET_CLASS> subList = pVoucherList1;
                orderPayVoucherView.SetTicketClassValue(subList, iSelected);
            }
        }

        private void SetTicket()
        {
            if (pVoucherList2 == null || pVoucherList2.Count() == 0) return;
            if (pVoucherList2.Count >= CurrentPage1 * pageCnt1)
            {
                List<MST_INFO_TICKET> subList = pVoucherList2.GetRange((CurrentPage1 - 1) * pageCnt1 + 1, 6);
                orderPayVoucherView.SetTicketValue(subList, iSelected2);
            }
            else
            {
                List<MST_INFO_TICKET> subList = pVoucherList2;
                orderPayVoucherView.SetTicketValue(subList, iSelected2);
            }
        }

        List<SALES_GIFT_SALE> tmpSALES_GIFT_SALE = new List<SALES_GIFT_SALE>();
        List<SALES_GIFT_SALE> tmpSALES_GIFT_SALE_RE_FUN = new List<SALES_GIFT_SALE>();
        private bool cashRecOn;

        public async void RemoveList1(SALES_GIFT_SALE selectedItem)
        {
            try
            {
                tmpSALES_GIFT_SALE.Remove(selectedItem);
                orderPayVoucherView.SetListItem1Value(tmpSALES_GIFT_SALE);
                TotalVoucher = tmpSALES_GIFT_SALE.Sum(x => Convert.ToDecimal(x.TK_GFT_UPRC));
            }
            catch (Exception ex)
            {
                LogHelper.Logger.Error("상품권 List 가져오기 오류 : " + ex.Message);
            }
        }
        public async void AddToList1(string iIndex)
        {
            try
            {
                if (TotalVoucher >= Amount)
                {
                    orderPayMainViewModel.StatusMessage = "더 이상 상품권등록을 할 수 없습니다.";
                    return;
                }

                List<SALES_GIFT_SALE> lst = new List<SALES_GIFT_SALE>();
                MST_INFO_TICKET? InfoTicket = pVoucherList2.FirstOrDefault(x => x.TK_GFT_CODE == iIndex.ToString());
                if (InfoTicket != null)
                {
                    tmpSALES_GIFT_SALE.Add(new SALES_GIFT_SALE
                    {
                        TK_GFT_UPRC = InfoTicket.TK_GFT_UPRC.ToString("N0"),
                        TK_GFT_CODE = InfoTicket.TK_GFT_CODE,
                        TK_GFT_NAME = InfoTicket.TK_GFT_NAME,
                        REMARK = Remark,
                        TK_GFT_UAMT = InfoTicket.TK_GFT_UPRC.ToString(),
                        TOT_TK_GFT_UAMT = InfoTicket.TK_GFT_UPRC.ToString("N0")
                    }); ;
                }
                orderPayVoucherView.SetListItem1Value(tmpSALES_GIFT_SALE);
                TotalVoucher = tmpSALES_GIFT_SALE.Sum(x => Convert.ToDecimal(x.TK_GFT_UPRC));
                orderPayVoucherView.ScrollToEnd();
                Remark = "";

            }
            catch (Exception ex)
            {
                LogHelper.Logger.Error("상품권 List 가져오기 오류 : " + ex.Message);
            }
        }

        public async void RemoveList2(SALES_GIFT_SALE selectedItem1)
        {
            try
            {
                tmpSALES_GIFT_SALE_RE_FUN.Remove(selectedItem1);
                orderPayVoucherView.SetListItem2Value(tmpSALES_GIFT_SALE_RE_FUN);
                Refun = tmpSALES_GIFT_SALE_RE_FUN.Sum(x => Convert.ToDecimal(x.TK_GFT_UPRC));
            }
            catch (Exception ex)
            {
                LogHelper.Logger.Error("상품권 List 가져오기 오류 : " + ex.Message);
            }
        }
        public async void AddToList2(string iIndex)
        {
            try
            {
                List<SALES_GIFT_SALE> lst = new List<SALES_GIFT_SALE>();
                MST_INFO_TICKET? InfoTicket = pVoucherList2.FirstOrDefault(x => x.TK_GFT_CODE == iIndex.ToString());

                if (InfoTicket != null)
                {
                    if (InfoTicket.TK_GFT_UPRC > Balance)
                    {
                        var res = DialogHelper.MessageBox("상품권 환불액을 확인하세요 !", GMessageBoxButton.OK, System.Windows.MessageBoxImage.Warning);
                        return;
                    }
                    tmpSALES_GIFT_SALE_RE_FUN.Add(new SALES_GIFT_SALE
                    {
                        TK_GFT_UPRC = InfoTicket.TK_GFT_UPRC.ToString("N0"),
                        TK_GFT_CODE = InfoTicket.TK_GFT_CODE,
                        TK_GFT_NAME = InfoTicket.TK_GFT_NAME,
                        REMARK = _remark,
                        TK_GFT_UAMT = InfoTicket.TK_GFT_UPRC.ToString(),
                        TOT_TK_GFT_UAMT = InfoTicket.TK_GFT_UPRC.ToString("N0")
                    }); ;
                }
                orderPayVoucherView.SetListItem2Value(tmpSALES_GIFT_SALE_RE_FUN);
                Refun = tmpSALES_GIFT_SALE_RE_FUN.Sum(x => Convert.ToDecimal(x.TK_GFT_UPRC));
                orderPayVoucherView.ScrollToEnd1();
                Remark = "";
            }
            catch (Exception ex)
            {
                LogHelper.Logger.Error("상품권 List 가져오기 오류 : " + ex.Message);
            }
        }

        public ICommand ButtonCommand => new RelayCommand<Button>(async (button) =>
        {
            if (button.Tag == null) return;

            switch (button.Tag.ToString())
            {
                case "Remove":
                    if (SelectedItem != null)
                    {
                        RemoveList1(SelectedItem);
                    }
                    break;
                case "ReFun":
                    if (SelectedItem1 != null)
                        RemoveList2(SelectedItem1);
                    break;
                case "btUp1":
                    if (CurrentPage1 == 1)
                        break;
                    CurrentPage1--;
                    break;
                case "btDn1":
                    if (CurrentPage1 == totalPage1)
                        break;
                    CurrentPage1++;
                    break;
                case "btUp2":
                    if (CurrentPage2 == 1)
                        break;
                    CurrentPage2--;
                    break;
                case "btDn2":
                    if (CurrentPage2 == totalPage2)
                        break;
                    CurrentPage2++;
                    break;
                case string s when s.StartsWith("btnTicketClass_"):
                    iSelected = Convert.ToInt32(button.Name.Substring(button.Name.Length - 2));
                    string[] ClassCode = button.Tag.ToString().Split('_');
                    if (ClassCode.Length > 1)
                    {
                        string result = ClassCode[1];
                        SetTicketClass();
                        await GetTicketList(result);
                    }
                    break;
                case string s when s.StartsWith("btnTicket_"):
                    iSelected2 = Convert.ToInt32(button.Name.Substring(button.Name.Length - 2));
                    string[] arrGiftCode = button.Tag.ToString().Split('_');
                    if (arrGiftCode.Length > 1)
                    {
                        string GiftCode = arrGiftCode[1];
                        giftTR.TK_GFT_CODE = GiftCode;
                        SetTicket();
                        if (IsRemoveChecked)
                        {
                            AddToList1(GiftCode);
                            break;
                        }
                        else
                        {
                            AddToList2(GiftCode);
                            break;
                        }
                    }
                    break;
                case "Result":
                    ReturnClose();
                    break;
                default:
                    break;
            }
        });


        /// <summary>
        /// gift_sale
        /// </summary>
        /// <param name="data"></param>

        public override async void SetData(object data)
        {
            giftTR = new TRN_GIFT()
            {
                SHOP_CODE = DataLocals.PosStatus.SHOP_CODE,
                POS_NO = DataLocals.PosStatus.POS_NO,
                SALE_DATE = DataLocals.PosStatus.SALE_DATE,
                BILL_NO = DataLocals.PosStatus.BILL_NO,
                SEQ_NO = seqNo.ToString("d4"),
                REGI_SEQ = DataLocals.PosStatus.REGI_SEQ,
                TK_GFT_CODE = string.Empty,
                TK_GTF_BARCODE = string.Empty,
                TK_GFT_SALE_FLAG = string.Empty,
                SALE_YN = "Y",
                TK_GFT_UAMT = 0,
                TK_GFT_AMT = 0,
                REPAY_CSH_AMT = 0,
                REPAY_GFT_AMT = 0,
                INSERT_DT = DateTime.Now.ToString("yyyyMMddHHmmss")
            };

            object[] datas = (object[])data;
            CALLER_ID = (string)datas[0];
            Amount = (decimal)datas[1] == 0 ? orderPayMainViewModel.OrderSumInfo.EXP_PAY_AMT : (decimal)datas[1];
            if (Balance < 0) Balance = 0;
            giftTR.REPAY_GFT_AMT = Refun;
            seqNo = (int)datas[2];
            giftTR.SEQ_NO = seqNo.ToString("d4");
        }

        /// <summary>
        /// 
        /// </summary>
        private void ReturnClose()
        {
            decimal runningAmount = Amount;
            TRN_GIFT[] arrGiftTR = new TRN_GIFT[tmpSALES_GIFT_SALE.Count + tmpSALES_GIFT_SALE_RE_FUN.Count];
            if (arrGiftTR.Length <= 0)
            {
                orderPayMainViewModel.StatusMessage = "상품권을 선택해주세요";
                return;
            }
            for (int i = 0; i < tmpSALES_GIFT_SALE.Count; i++)
            {
                var giftItem = tmpSALES_GIFT_SALE[i];

                var giftStdAmt = giftItem.TK_GFT_UPRC.ToDecimal();
                TRN_GIFT arr = new TRN_GIFT
                {
                    SHOP_CODE = DataLocals.PosStatus.SHOP_CODE,
                    POS_NO = DataLocals.PosStatus.POS_NO,
                    SALE_DATE = DataLocals.PosStatus.SALE_DATE,
                    BILL_NO = DataLocals.PosStatus.BILL_NO,
                    REGI_SEQ = DataLocals.PosStatus.REGI_SEQ,
                    SALE_YN = "Y",
                    TK_GTF_BARCODE = string.Empty,
                    TK_GFT_CODE = giftItem.TK_GFT_CODE,
                    TK_GFT_AMT = tmpSALES_GIFT_SALE[i].TK_GFT_UAMT.ToDecimal() > runningAmount ? runningAmount : tmpSALES_GIFT_SALE[i].TK_GFT_UAMT.ToDecimal(),
                    TK_GFT_UAMT = tmpSALES_GIFT_SALE[i].TK_GFT_UAMT.ToDecimal(),

                    REPAY_CSH_AMT = runningAmount > giftStdAmt ? 0 : giftStdAmt - runningAmount,
                    REPAY_GFT_AMT = 0,

                    SEQ_NO = (Convert.ToInt32(giftTR.SEQ_NO) + i).ToString("d4"),
                    INSERT_DT = DateTime.Now.ToString("yyyyMMddHHmmss"),
                    TK_GFT_SALE_FLAG = "0"
                };

                runningAmount = arr.TK_GFT_AMT >= runningAmount ? runningAmount : runningAmount - arr.TK_GFT_AMT;
                arrGiftTR[i] = arr;
            }

            arrGiftTR[tmpSALES_GIFT_SALE.Count - 1].REPAY_CSH_AMT = Balance;
            arrGiftTR[tmpSALES_GIFT_SALE.Count - 1].REPAY_GFT_AMT = Refun;

            int j = 0;
            for (int i = tmpSALES_GIFT_SALE.Count; i < tmpSALES_GIFT_SALE.Count + tmpSALES_GIFT_SALE_RE_FUN.Count; i++)
            {
                var giftItem = tmpSALES_GIFT_SALE_RE_FUN[j];
                TRN_GIFT arr = new TRN_GIFT
                {
                    SHOP_CODE = DataLocals.PosStatus.SHOP_CODE,
                    POS_NO = DataLocals.PosStatus.POS_NO,
                    SALE_DATE = DataLocals.PosStatus.SALE_DATE,
                    BILL_NO = DataLocals.PosStatus.BILL_NO,
                    REGI_SEQ = DataLocals.PosStatus.REGI_SEQ,
                    SALE_YN = "Y",
                    TK_GTF_BARCODE = string.Empty,
                    TK_GFT_CODE = giftItem.TK_GFT_CODE,
                    TK_GFT_AMT = tmpSALES_GIFT_SALE_RE_FUN[j].TK_GFT_AMT.ToDecimal(),
                    TK_GFT_UAMT = tmpSALES_GIFT_SALE_RE_FUN[j].TK_GFT_UAMT.ToDecimal(),
                    SEQ_NO = (Convert.ToInt32(giftTR.SEQ_NO) + i).ToString("d4"),
                    INSERT_DT = DateTime.Now.ToString("yyyyMMddHHmmss"),
                    TK_GFT_SALE_FLAG = "3",
                };

                runningAmount = arr.REPAY_GFT_AMT >= runningAmount ? runningAmount : runningAmount - arr.REPAY_GFT_AMT;
                arrGiftTR[i] = arr;
                j++;
            }

            #region CashReceipt for Voucher - 현금영수증


            // Loc added 09/07
            // Process Cash Receipt for Voucher
            // COMPPAY_PAY_INFO cashRecCompay = null;
            List<object> payList = new List<object>();
            payList.AddRange(arrGiftTR);

            TRN_CASHREC cashRec = null;
            if (CashRecOn)
            {
                cashRec = CheckCashReceiptOn(arrGiftTR.Sum(x => x.TK_GFT_AMT));
                if (cashRec != null) payList.Add(cashRec);
            }

            #endregion

            var compPayInfo = new COMPPAY_PAY_INFO()
            {
                PAY_TYPE_CODE = OrderPayConsts.PAY_GIFT,
                PAY_CLASS_NAME = nameof(TRN_GIFT),
                PAY_VM_NANE = this.GetType().Name,
                APPR_AMT = arrGiftTR.Sum(x => x.TK_GFT_AMT),
                APPR_PROC_FLAG = "1",
                APPR_IDT_NO = cashRec != null ? cashRec.APPR_IDT_NO : string.Empty,
                APPR_NO = cashRec != null ? cashRec.APPR_NO : string.Empty,
                PayDatas = payList.ToArray()
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

        #region CashReceipt On / Off

        public bool CashRecOn
        {
            get => cashRecOn; set
            {
                cashRecOn = value;
                NotifyOfPropertyChange(nameof(CashRecOn));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private TRN_CASHREC CheckCashReceiptOn(decimal totalPaidAmt)
        {
            var diaResults = DialogHelper.ShowDialog(typeof(OrderPayCashReceiptPopupViewModel), 600, 450, totalPaidAmt,
                orderPayMainViewModel.payCashRecs.Count);

            if (diaResults != null && diaResults.Count > 0 && diaResults.ContainsKey("RETURN_DATA"))
            {
                return diaResults["RETURN_DATA"] as TRN_CASHREC;
            }

            return null;
        }

        #endregion
    }
}
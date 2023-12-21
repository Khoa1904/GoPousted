using Caliburn.Micro;
using GoPOS.Helpers;
using GoPOS.Models.Common;
using GoPOS.Models;
using GoPOS.Service.Common;
using GoPOS.ViewModels;
using GoShared.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using AutoMapper;
using System.Xml.Linq;
using GoPOS.Services;
using System.Windows.Input;
using GoPOS.Helpers.CommandHelper;
using System.Windows.Controls;
using GoPOS.Common.Interface.Model;
using log4net.Core;
using GoPOS.Models.Custom.Payment;
using GoPOS.Service;
using Newtonsoft.Json;
using System.Security.Cryptography.X509Certificates;
using GoPOS.Payment.Models;
using GoPOS.Payment.Services;
using System.Security.Cryptography.Xml;
using System.Windows.Markup.Localizer;
using NLog.LayoutRenderers.Wrappers;
using System.Printing;

namespace GoPOS.ViewModels
{
    public class OrderPayStampCouponViewModel : OrderPayChildViewModel, IHandle<MemberInfoPass>
    {
        private IOrderPayMainViewModel orderPayMainViewModel;

        public override object ActivateType => "ExceptKeyPad";

        private TRN_POINTUSE trnPointUse = null;
        private decimal _amtTobe;
        private decimal _paidAmt = 0;
        private decimal _changeAmt;
        private int seqNo = 1;
        private string callerID = string.Empty;
        private int _index = -1;
        public int Index
        {
            get { return _index; }
            set
            {
                _index = value;
                NotifyOfPropertyChange(() => Index);
            }
        }
        private int _index2 = -1;
        public int Index2
        {
            get { return _index2; }
            set
            {
                _index2 = value;
                NotifyOfPropertyChange(() => Index2);
            }
        }
        public string CALLER_ID
        {
            get => callerID; set { callerID = value; NotifyOfPropertyChange(nameof(CALLER_ID)); }
        }
        private MEMBER_CLASH memberInfo { get; set; }
        public MEMBER_CLASH MemberInfo
        {
            get => memberInfo; set
            {
                memberInfo = value;
                NotifyOfPropertyChange(() => MemberInfo);
            }

        }

        /// <summary>
        /// Stammpable items in selling
        /// </summary>
        public ObservableCollection<ORDER_GRID_ITEM> StampAvailItems { get; set; }
        public ObservableCollection<ORDER_GRID_ITEM> StampCpnItems { get; set; }
       
        public OrderPayStampCouponViewModel(IWindowManager windowManager, IEventAggregator eventAggregator,
            IOrderPayPointStampService orderPayPointStampService) : base(windowManager, eventAggregator)
        {
            this.ViewInitialized += ScreenInit;
            this.ViewLoaded += ViewLoadedImpl;
            this.orderPayPointStampService = orderPayPointStampService;
        }
        private string _txtTelNo = "";
        public string TEL_NO
        {
            get { return _txtTelNo; }
            set
            {
                if (_txtTelNo != value)
                {
                    _txtTelNo = value;
                    NotifyOfPropertyChange(() => TEL_NO);
                }
            }
        }
        private void ScreenInit(object? sender, EventArgs e)
        {
            orderPayMainViewModel = (IOrderPayMainViewModel)Parent;
            StampAvailItems = new();
            StampCpnItems = new();
        }
        private void ViewLoadedImpl(object? sender, EventArgs e)
        {
            TEL_NO = string.Empty;
            CARD_NO = MmberName2 = MEMBER_NAME = string.Empty;
            ChagamStamp = 0;
            NotifyOfPropertyChange(() => MemberInfo);
            NotifyOfPropertyChange(() => StampCpnItems);
        }
        private string _cardno = "";
        public string CARD_NO
        {
            get { return _cardno; }
            set { _cardno = value; NotifyOfPropertyChange(() => CARD_NO); }
        }

        public string MmberName2
        {
            get => mmberName2; set
            {
                mmberName2 = value;
                NotifyOfPropertyChange(nameof(MmberName2));
            }
        }

        private string _membername = "";
        private string mmberName2;
        private readonly IOrderPayPointStampService orderPayPointStampService;

        public string MEMBER_NAME
        {
            get { return _membername; }
            set { _membername = value; NotifyOfPropertyChange(() => MEMBER_NAME); }
        }
        public decimal MaxAmt
        {
            get => _amtTobe; set
            {
                _amtTobe = value;
                NotifyOfPropertyChange(nameof(MaxAmt));
            }
        }

        /// <summary>
        /// 받은금액
        /// </summary>
        public decimal PaidAmt
        {
            get => _paidAmt; set
            {
                _paidAmt = value > MaxAmt ? MaxAmt : value;
                NotifyOfPropertyChange(nameof(PaidAmt));
            }
        }
        private decimal _chagamStamp { get; set; } = 0;
        public decimal ChagamStamp
        {
            get => _chagamStamp;
            set
            {
                _chagamStamp = value;
                NotifyOfPropertyChange(() => ChagamStamp);
            }
        }

        public ICommand ButtonCommand => new RelayCommand<Button>(ButtonCommandCenter);
        private void ButtonCommandCenter(Button btn)
        {
            switch (btn.Tag.ToString())
            {
                case "btnSearch":
                    if (orderPayMainViewModel.payPpCards.Count > 0 || orderPayMainViewModel.payPoints.Count > 0)
                    {
                        DialogHelper.MessageBox("포인트 사용 내역이 있어 회원정보를\r\n초기화 할 수 없습니다.", GMessageBoxButton.OK, System.Windows.MessageBoxImage.Warning);
                        return;
                    }
                    if (string.IsNullOrEmpty(TEL_NO) && string.IsNullOrEmpty(CARD_NO) && string.IsNullOrEmpty(MEMBER_NAME))
                    {
                        orderPayMainViewModel.StatusMessage = "검색조건을 입력해 주세요.";
                        return;
                    }
                    orderPayMainViewModel.ActiveForm("ActiveItemR", "OrderPayMemberSearchViewModel", CARD_NO, TEL_NO, MEMBER_NAME);
                    break;

                case "btnUse":
                    RedeemStampCnt();
                    break;

                case "AddUseCpn":
                    AddUseCpn(Index);
                    break;

                case "RemoveUseCpn":
                    RemoveUseCpn(Index2);
                    break;
                default:
                    break;
            }
        }

        public decimal UsingCpns
        {
            get
            {
                return StampCpnItems.Sum(p => p.STAMP_USE_QTY * p.SALE_QTY) ?? 0;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        private void AddUseCpn(int dex)
        {
            if (dex < 0 || dex > StampAvailItems.Count - 1 || StampAvailItems.Count == 0) { return; }
            if (memberInfo == null)
            {
                DialogHelper.MessageBox("회원을 선택해주세요", GMessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            // can add or not?
            // no money in acc, disable to test  (avalidStampAmt = 0)
            int useCpn = StampAvailItems[dex].STAMP_ACC_QTY ?? 0;
            if (UsingCpns + useCpn > memberInfo.avalidPoint)
            {
                DialogHelper.MessageBox("가용스탬프보다 많습니다.", GMessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (UsingCpns + useCpn > memberInfo.maxUsePt)
            {
                DialogHelper.MessageBox("최대가용스탬프수 초과 되었습니다.", GMessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var exs = StampCpnItems.FirstOrDefault(p => p.PRD_CODE == StampAvailItems[dex].PRD_CODE);
            if (exs != null)
            {
                exs.SALE_QTY++;
                Index2 = StampCpnItems.IndexOf(exs);
            }
            else
            {
                var cpnItem = StampAvailItems[dex].Copy();
                cpnItem.SALE_QTY = 1;
                cpnItem.NO = StampCpnItems.Count + 1;
                StampCpnItems.Add(cpnItem);
                Index2 = StampCpnItems.Count - 1;
            }

            // adding
            ChagamStamp += (decimal)StampAvailItems[dex].STAMP_USE_QTY;
            NotifyOfPropertyChange(() => ChagamStamp);
            StampAvailItems[dex].SALE_QTY--;
            if (StampAvailItems[dex].SALE_QTY <= 0)
            {
                StampAvailItems.RemoveAt(dex);
                Index = dex <= 0 ? 0 : (dex > StampAvailItems.Count - 1 ? StampAvailItems.Count - 1 : dex);
            }

            PaidAmt = StampCpnItems.Sum(p => p.SALE_AMT - p.SALE_QTY * p.DC_AMT_PERQTY);


            NotifyOfPropertyChange(() => StampAvailItems);
            NotifyOfPropertyChange(() => StampCpnItems);
            ItemGrid_Refresh();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        private void RemoveUseCpn(int dex)
        {
            if (dex < 0 || dex > StampCpnItems.Count - 1 || StampCpnItems.Count == 0) { return; }
            if (memberInfo == null)
            {
                DialogHelper.MessageBox("회원을 선택해주세요", GMessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            var availExs = StampAvailItems.FirstOrDefault(p => p.PRD_CODE == StampCpnItems[dex].PRD_CODE);
            if (availExs != null)
            {
                availExs.SALE_QTY++;
            }
            else
            {
                var sel = StampCpnItems[dex].Copy();
                sel.SALE_QTY = 1;
                StampAvailItems.Add(sel);
            }

            //subtracting        
            ChagamStamp -= (decimal)StampCpnItems[dex].STAMP_USE_QTY;
            NotifyOfPropertyChange(() => ChagamStamp);
            StampCpnItems[dex].SALE_QTY--;
            if (StampCpnItems[dex].SALE_QTY <= 0)
            {
                StampCpnItems.RemoveAt(dex);
                Index2 = dex <= 0 ? 0 : (dex > StampCpnItems.Count - 1 ? StampCpnItems.Count - 1 : dex);
            }

            NotifyOfPropertyChange(() => StampAvailItems);
            NotifyOfPropertyChange(() => StampCpnItems);
            PaidAmt = StampCpnItems.Sum(p => p.SALE_AMT - p.SALE_QTY * p.DC_AMT_PERQTY);
            ItemGrid_Refresh();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        public override void SetData(object data)
        {
            StampAvailItems = new();
            StampCpnItems = new();
            trnPointUse = new TRN_POINTUSE()
            {
                SHOP_CODE = DataLocals.PosStatus.SHOP_CODE,
                POS_NO = DataLocals.PosStatus.POS_NO,
                SALE_DATE = DataLocals.PosStatus.SALE_DATE,
                BILL_NO = DataLocals.PosStatus.BILL_NO,
                SEQ_NO = seqNo.ToString("d4"),
                REGI_SEQ = DataLocals.PosStatus.REGI_SEQ,
                CST_NO = string.Empty,
                CARD_NO = string.Empty,
                LEVEL = string.Empty,
                TOT_SALE_AMT = 0,
                TOT_DC_AMT = 0,
                TOT_PNT = 0,
                TOT_USE_PNT = 0,
                USE_PNT = 0,
                LAST_PNT = 0,
                SALE_YN = "Y",
                USE_TYPE = "03",
                INSERT_DT = DateTime.Now.ToString("yyyyMMddHHmmss")
            };

            object[] datas = (object[])data;
            CALLER_ID = (string)datas[0];
            LoadOrderItems();

            NotifyOfPropertyChange(() => StampAvailItems);
            MaxAmt = orderPayMainViewModel.OrderSumInfo.EXP_PAY_AMT;
            PaidAmt = 0;
            seqNo = (int)datas[2];
            seqNo++;
            trnPointUse.SEQ_NO = seqNo.ToString("d4");
            trnPointUse.TOT_SALE_AMT = orderPayMainViewModel.OrderSumInfo.TOT_SALE_AMT;
            trnPointUse.TOT_DC_AMT = orderPayMainViewModel.OrderSumInfo.TOT_DC_AMT;

            this.MemberInfo = (MEMBER_CLASH)datas[datas.Length - 1];
        }

        /// <summary>
        /// 
        /// </summary>
        private async void RedeemStampCnt()
        {
            string newBillNo = DataLocals.PosStatus.BILL_NO.StrIntInc(4);
            var res = await orderPayPointStampService.RequestUsePointStamp(newBillNo, memberInfo, trnPointUse, PaidAmt, UsingCpns);

            object[] retValues = res.Item2;
            string errorMessage = retValues[0].ToString();
            if (!string.IsNullOrEmpty(errorMessage))
            {
                DialogHelper.MessageBox(errorMessage, GMessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var compPayInfo = new COMPPAY_PAY_INFO()
            {
                PAY_TYPE_CODE = OrderPayConsts.PAY_POINTS,
                PAY_TYPE_CODE_FG = "쿠폰",
                PAY_CLASS_NAME = nameof(TRN_POINTUSE),
                PAY_VM_NANE = this.GetType().Name,
                APPR_IDT_NO = string.Empty,
                APPR_NO = string.Empty,
                APPR_AMT = PaidAmt,
                APPR_PROC_FLAG = "1",
                PayDatas = new object[] { trnPointUse }
            };

            if ("COMP_PAY".Equals(CALLER_ID))
            {
                IoC.Get<OrderPayCompPayViewModel>().UpdatePaymentTRN(this.GetType().Name, compPayInfo);
            }
            else
            {
                orderPayMainViewModel.UpdatePaymentTRN(this.GetType().Name, compPayInfo);
            }

            this.DeactivateClose(true);
        }

        private void LoadOrderItems()
        {
            StampAvailItems.Clear();
            StampCpnItems.Clear();
            var stampsItems = orderPayMainViewModel.OrderItems.Where(p => p.STAMP_USE_YN == "Y");
            foreach (var item in stampsItems)
            {
                var ci = item.Copy();
                ci.SALE_QTY_TMP = ci.SALE_QTY;
                StampAvailItems.Add(ci);
            }

            Index = StampAvailItems.Count > 0 ? 0 : -1;
        }

        public Task HandleAsync(MemberInfoPass message, CancellationToken cancellationToken)
        {
            MemberInfo = message.memberInfo;
            LoadOrderItems();
            return Task.CompletedTask;
        }
        private void ItemGrid_Refresh()
        {
            // reordering
            for (int i = 0; i < StampAvailItems.Count; i++)
            {
                StampAvailItems[i].NO = i + 1;
            }
            NotifyOfPropertyChange(() => StampAvailItems);

            for (int i = 0; i < StampCpnItems.Count; i++)
            {
                StampCpnItems[i].NO = i + 1;
            }
            NotifyOfPropertyChange(() => StampCpnItems);
        }
    }
}

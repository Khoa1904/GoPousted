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
using GoPOS.Models.Custom.Payment;
using GoPOS.Payment.Interface.View;
using GoPOS.Service;
using GoPOS.Services;

using GoPOS.Views;
using GoShared.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Xml.Linq;
using static GoPosVanAPI.Api.VanAPI;

namespace GoPOS.ViewModels
{
    /// <summary>    
    /// 화면명 제휴카드 할인 ( 제휴할인 ) 604
    /// 작성자 김형석
    /// </summary>
    public class OrderPayCoprtnDscntViewModel : OrderPayChildViewModel
    {
        private IOrderPayCoprtnDscntView _view;
        private readonly IOrderPayCoprtnDscntService _orderPayCoprtnDscntService;
        private IOrderPayMainViewModel orderPayMainViewModel;

        private string CALLER_ID = string.Empty;
        private List<MST_INFO_JOINCARD> _allJoinCards;
        private decimal pAY_AMT;
        private decimal dISC_AMT;

        public override object ActivateType { get => "ExceptKeyPad"; }

        #region Binding & Properties


        private MST_INFO_JOINCARD _selectedItem;
        public MST_INFO_JOINCARD SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                _selectedItem = value;
                NotifyOfPropertyChange(() => SelectedItem);
                NotifyOfPropertyChange(() => DISC_ITEM_TYPE);
                LoadDiscTargetItems(value);
            }
        }

        private int _currentPage = -1;
        public int CurrentPage
        {
            get { return _currentPage; }
            set
            {
                int lastPage = _currentPage;
                _currentPage = value < 0 ? 0 : value;
                _currentPage = _currentPage + 1 > totalPage ? totalPage - 1 : _currentPage;

                NotifyOfPropertyChange(() => CurrentPage);

                //if (lastPage != _currentPage)
                {
                    int fromIdx = _currentPage * 10;
                    int toIdx = Math.Min(_allJoinCards.Count, (_currentPage + 1) * 10);
                    List<MST_INFO_JOINCARD> pageItems = new List<MST_INFO_JOINCARD>();
                    for (int i = fromIdx; i < toIdx; i++)
                    {
                        pageItems.Add(_allJoinCards[i]);
                    }

                    _view.BindAffiliatedCards(pageItems);
                    SelectedItem = pageItems.Count > 0 ? pageItems[0] : null;
                }
            }
        }


        public ObservableCollection<MST_INFO_PRD_JOINCARD> DiscTargetItems { get; set; }

        /// <summary>
        /// 할인상품
        /// 제외상품
        /// </summary>
        public string DISC_ITEM_TYPE
        {
            get
            {
                return _selectedItem == null ? "할인(제외)상품" : (
                    "A".Equals(_selectedItem.DC_PRD_FLAG) ? "할인제외상품" : "할인상품");
            }
        }

        public string JOIN_CARD_NO { get; set; }
        public string APPR_NO { get; set; } = string.Empty;
        public decimal PAY_AMT
        {
            get => pAY_AMT; set
            {
                pAY_AMT = value;
                NotifyOfPropertyChange(nameof(PAY_AMT));
            }
        }

        public decimal DISC_AMT
        {
            get => dISC_AMT; set
            {
                if (value > decimal.Parse(_view.LimitMoney.Text))
                {
                    dISC_AMT = decimal.Parse(_view.LimitMoney.Text);
                }
                else
                {
                    dISC_AMT = value;
                }
                NotifyOfPropertyChange(nameof(DISC_AMT));
            }
        }

        public ICommand ButtonCommand => new RelayCommand<Button>(async (button) =>
        {
            if (button.Tag == null) return;

            switch (button.Tag.ToString())
            {
                case "btnDcProc":
                    ReturnClose();
                    break;
                case "Prev":
                    CurrentPage--;
                    break;
                case "Next":
                    CurrentPage++;
                    break;
                default:
                    break;
            }
        });

        public ICommand JoinCardButtonCommand => new RelayCommand<Button>(button =>
        {
            var joinCard = button.Tag as MST_INFO_JOINCARD;
            SelectedItem = joinCard;
        });


        #endregion

        private int totalPage = 0;

        public OrderPayCoprtnDscntViewModel(IWindowManager windowManager, IEventAggregator eventAggregator,
            IOrderPayCoprtnDscntService orderPayCoprtnDscntService) : base(windowManager, eventAggregator)
        {
            this.ViewInitialized += OrderPayCoprtnDscntViewModel_ViewInitialized;
            this.ViewLoaded += OrderPayCoprtnDscntViewModel_ViewLoaded;
            _orderPayCoprtnDscntService = orderPayCoprtnDscntService;
        }

        private void OrderPayCoprtnDscntViewModel_ViewInitialized(object? sender, EventArgs e)
        {
            orderPayMainViewModel = (IOrderPayMainViewModel)Parent;
        }

        private void OrderPayCoprtnDscntViewModel_ViewLoaded(object? sender, EventArgs e)
        {
            _allJoinCards = _orderPayCoprtnDscntService.GetInfoJoinCardTable().Result;
            totalPage = _allJoinCards.Count / 10 + (_allJoinCards.Count % 10 != 0 ? 1 : 0);
            CurrentPage = 0;
        }

        public override bool SetIView(IView view)
        {
            _view = (IOrderPayCoprtnDscntView)view;
            return base.SetIView(view);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="joinCard"></param>
        private void LoadDiscTargetItems(MST_INFO_JOINCARD joinCard)
        {
            var joinProducts = new ObservableCollection<MST_INFO_PRD_JOINCARD>(_orderPayCoprtnDscntService.GetInfoPrdJoinCardTable(joinCard?.JCD_CODE).Result);
            /// 
            /// TO DO
            /// 1. Grid shown only related items
            /// 1. If excepts items  shown:
            ///     - 
            /// 2. If applied items shown
            /// 
            // discount apply items
            List<MST_INFO_PRD_JOINCARD> discAppItems = new List<MST_INFO_PRD_JOINCARD>();
            List<MST_INFO_PRD_JOINCARD> showItems = new List<MST_INFO_PRD_JOINCARD>();
            /// 
            /// CHECK
            /// OF 
            /// addItems is discount applied items
            /// 
            bool bShowDiscItems = SelectedItem == null || "A".Equals(SelectedItem.DC_PRD_FLAG) ? false : true;

            if (SelectedItem != null)
            {
                if (bShowDiscItems)
                {
                    foreach (var joinProduct in joinProducts)
                    {
                        // New joinproduct in list of items, add to addItems
                        // Join Product is included in discounting
                        var exsItems = orderPayMainViewModel.OrderItems.Where(p => p.PRD_CODE == joinProduct.STYLE_PRD_CODE).ToArray();
                        if (exsItems.Length > 0)
                        {
                            joinProduct.DC_ORDER_AMT += exsItems.Sum(p => p.DCM_SALE_AMT);
                            showItems.Add(joinProduct);
                            discAppItems.Add(joinProduct);
                        }
                    }
                }
                else
                {
                    // join products are excepts items
                    // add all in orderitems excepts item in joinProducts
                    foreach (var joinProduct in joinProducts)
                    {
                        var cnt = orderPayMainViewModel.OrderItems.Count(p => p.PRD_CODE == joinProduct.STYLE_PRD_CODE);
                        if (cnt > 0)
                        {
                            showItems.Add(joinProduct);
                        }
                    }

                    foreach (var orderItem in orderPayMainViewModel.OrderItems)
                    {
                        var joinProduct = showItems.FirstOrDefault(p => p.STYLE_PRD_CODE == orderItem.PRD_CODE);

                        /// 
                        /// if orderItems contains excepted item
                        /// - dont add to applyList
                        /// - add to showItem list
                        /// 
                        if (joinProduct == null)
                        {
                            discAppItems.Add(new MST_INFO_PRD_JOINCARD()
                            {
                                SHOP_CODE = DataLocals.PosStatus.SHOP_CODE,
                                STYLE_PRD_CODE = orderItem.PRD_CODE,
                                STYLE_PRD_NAME = orderItem.PRD_NAME,
                                DC_ORDER_AMT = orderItem.DCM_SALE_AMT
                            });
                        }
                    }
                }
            }


            ///
            /// show in raw data
            /// 
            DiscTargetItems = new ObservableCollection<MST_INFO_PRD_JOINCARD>(showItems);

            ///
            /// Calcuate DISC_AMT, PAY_AMT
            /// 

            PAY_AMT = discAppItems.Sum(p => p.DC_ORDER_AMT ?? 0);
            var rate = SelectedItem == null ? 0 : Convert.ToDouble(SelectedItem.DC_RATE) / 100;
            var tempDiscAmt = Convert.ToDecimal(Convert.ToDouble(PAY_AMT) * rate);
            if (SelectedItem != null && "1".Equals(SelectedItem.DC_LIMIT_FLAG))
            {
                tempDiscAmt = Math.Min(tempDiscAmt, SelectedItem.DC_LIMIT_AMT);
            }

            DISC_AMT = tempDiscAmt;
            NotifyOfPropertyChange(nameof(DiscTargetItems));
        }

        private TRN_PARTCARD partCardTR = null;

        public override void SetData(object data)
        {
            partCardTR = new TRN_PARTCARD()
            {
                SHOP_CODE = DataLocals.PosStatus.SHOP_CODE,
                POS_NO = DataLocals.PosStatus.POS_NO,
                SALE_DATE = DataLocals.PosStatus.SALE_DATE,
                BILL_NO = DataLocals.PosStatus.BILL_NO,
                SEQ_NO = 1.ToString("d4"),
                SALE_YN = "Y",
                VAN_CODE = DataLocals.POSVanConfig.VAN_CODE,
                JCD_CARD_NO = string.Empty,
                VALID_TERM = string.Empty,
                CARD_IN_FLAG = string.Empty,
                SIGN_PAD_YN = string.Empty,
                APPR_PROC_FLAG = string.Empty,
                JCD_CODE = string.Empty,
                APPR_AMT = 0,
                REGI_SEQ = DataLocals.PosStatus.REGI_SEQ,
                INSERT_DT = DateTime.Now.ToString("yyyyMMddHHmmss")
            };

            object[] datas = (object[])data;
            CALLER_ID = (string)datas[0];
        }

        /// <summary>
        /// 
        /// </summary>
        private void ReturnClose()
        {
            if (string.IsNullOrEmpty(JOIN_CARD_NO))
            {
                DialogHelper.MessageBox("카드번호를 확인하여 주십시오!", GMessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (DISC_AMT > 0)
            {
                partCardTR.JCD_CARD_NO = JOIN_CARD_NO == null ? "" : JOIN_CARD_NO;
                partCardTR.APPR_AMT = DISC_AMT;
                partCardTR.APPR_DATE = DateTime.Now.ToString("yyyyMMdd");
                partCardTR.APPR_NO = APPR_NO;
                partCardTR.APPR_PROC_FLAG = string.IsNullOrEmpty(APPR_NO) ? "0" : "1";
                partCardTR.APPR_TIME = DateTime.Now.ToString("HHmmss");
                partCardTR.JCD_CODE = SelectedItem.JCD_CODE;
                partCardTR.JCD_DC_AMT = DISC_AMT;
                partCardTR.JCD_PROC_FLAG = "0";
                partCardTR.JCD_TYPE_FLAG = SelectedItem.JCD_TYPE_FLAG;

                var compPayInfo = new COMPPAY_PAY_INFO()
                {
                    PAY_TYPE_CODE = OrderPayConsts.PAY_PARTCARD,
                    PAY_CLASS_NAME = "TRN_PARTCARD",
                    PAY_VM_NANE = this.GetType().Name,
                    APPR_IDT_NO = partCardTR.JCD_CARD_NO,
                    APPR_NO = partCardTR.APPR_NO,
                    APPR_AMT = partCardTR.APPR_AMT,
                    APPR_PROC_FLAG = "1",
                    PayDatas = new object[] { partCardTR }
                };


                if ("COMP_PAY".Equals(CALLER_ID))
                {
                    IoC.Get<OrderPayCompPayViewModel>().UpdatePaymentTRN(this.GetType().Name, compPayInfo);
                }
                else
                {
                    orderPayMainViewModel.UpdatePaymentTRN(this.GetType().Name, compPayInfo);
                }
            }

            this.DeactivateClose(false);
        }
    }
}
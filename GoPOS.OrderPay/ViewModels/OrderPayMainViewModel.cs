using Caliburn.Micro;
using GoPOS.Services;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Reflection;
using GoPOS.OrderPay.Interface.View;
using GoPOS.OrderPay.Models;
using System.Collections.ObjectModel;
using System.Data;
using System.Windows.Media;
using System.Windows.Threading;
using GoPOS.OrderPay.ViewModels.Controls;
using Dapper;
using GoPOS.Models;
using GoPOS.Common.ViewModels;
using GoPOS.Service;
using GoPOS.Common.Service;
using GoShared.Helpers;
using GoPOS.Models.Common;
using GoPOS.Helpers;
using GoPOS.Helpers.CommandHelper;
using GoPOS.Common.Interface.View;
using GoPOS.Models.Custom.Payment;
using GoPOS.Common.Interface.Model;
using GoPOS.Common.PrinterLib;
using GoPOS.Common.Views.Controls;
using System.Diagnostics;
using GoPOS.Models.Custom.API;
using Google.Protobuf.WellKnownTypes;
using System.Windows.Media.Animation;
using FirebirdSql.Data.FirebirdClient;
using System.Collections.Generic;
using GoPOS.Payment.Services;
using System.Diagnostics.Metrics;
using System.DirectoryServices.ActiveDirectory;
using GoPOS.Common;
using GoPOS.Views;

namespace GoPOS.ViewModels;
public partial class OrderPayMainViewModel : MainBasePageViewModel, IOrderPayMainViewModel,
    IHandle<OrderPayDiscHandleEventArgs>,
    IHandle<OrderPayChildUpdatedEventArgs>, IHandle<KeyboardEventData>, IHandle<SideMenuConfirmEventArgs>, IHandle<TableOrder>
{
    private readonly IOrderPayService orderPayService;
    private readonly IOrderPayWaitingService orderPayWaitingService;
    private readonly IEmployeeService employeeService;
    private readonly IOrderPayPointStampService orderPayPointStampService;
    private IOrderPayMainView _view = null;
    private readonly IPOSPrintService pOSPrintService;
    private bool PostPay = DataLocals.AppConfig.PosOption.POSOrderMethod == "000001" ? true : false;
    public OrderPayMainViewModel(IWindowManager windowManager,
        IEventAggregator eventAggregator,
        IOrderPayService orderPayService,
        IOrderPayWaitingService orderPayWaitingService,
        IPOSPrintService pOSPrintService,
        IEmployeeService employeeService,
        IOrderPayPointStampService orderPayPointStampService) : base(windowManager, eventAggregator)
    {
        this.orderPayService = orderPayService;
        this.orderPayWaitingService = orderPayWaitingService;
        this.pOSPrintService = pOSPrintService;
        this.employeeService = employeeService;
        this.orderPayPointStampService = orderPayPointStampService;
        Initialize();
    }

    #region Properties

    public List<ORDER_FUNC_KEY>? PayBtnKeys { get; set; }

    DispatcherTimer _timer;
    TimeSpan _time;
    private int Point_SeqNo = 1;
    private string statusMessage;
    private bool memberLock { get; set; } = false;
    public string StatusMessage
    {
        get => statusMessage; set
        {
            statusMessage = value;
            NotifyOfPropertyChange(nameof(StatusMessage));

            if (string.IsNullOrEmpty(value))
            {
                return;
            }

            // Set timer to disappear
            _time = TimeSpan.FromSeconds(4);
            _timer = new DispatcherTimer(new TimeSpan(0, 0, 1), DispatcherPriority.Normal, delegate
            {
                //tbTime.Text = _time.ToString("c");
                if (_time == TimeSpan.Zero)
                {
                    _timer.Stop();
                    //_view.StatusMessage.Visibility = Visibility.Hidden;
                    StatusMessage = string.Empty;
                    return;
                }

                _time = _time.Add(TimeSpan.FromSeconds(-1));
            }, Application.Current.Dispatcher);

            _timer.Start();
        }
    }
    private string _tableCd { get; set; } = string.Empty;
    public string TableCd { get => _tableCd; set { _tableCd = value; NotifyOfPropertyChange(() => TableCd); } }
    public string StatusMessageNoReset
    {
        get => statusMessage; set
        {
            statusMessage = value;
            NotifyOfPropertyChange(nameof(StatusMessage));

            if (string.IsNullOrEmpty(value))
            {
                return;
            }
        }
    }

    private IScreen? _ActiveItemR = null;
    public IScreen? ActiveItemR
    {
        get
        {
            return _ActiveItemR;
        }
        set
        {
            PutScreen(nameof(ActiveItemR), value);
            _ActiveItemR = value;
            _ActiveItemR?.ActivateAsync();
            if (_ActiveItemR.ToString() == "GoPOS.ViewModels.OrderPayRightViewModel")
            {
                IoC.Get<OrderPayLeftTRInfoViewModel>().ButtonClick = true;
                IoC.Get<MemberInfoViewModel>().ButtonClick = true;
            }
            NotifyOfPropertyChange(() => ActiveItemR);
        }
    }

    private IScreen? _ActiveItemLB = null;
    public IScreen? ActiveItemLB
    {
        get
        {
            return _ActiveItemLB;
        }
        set
        {
            PutScreen(nameof(ActiveItemLB), value);

            _ActiveItemLB = value;
            _ActiveItemLB?.ActivateAsync();

            NotifyOfPropertyChange(() => ActiveItemLB);
        }
    }

    public ORDER_SUM_INFO OrderSumInfo
    {
        get => orderSumInfo; set
        {
            orderSumInfo = value;
            NotifyOfPropertyChange(() => OrderSumInfo);
        }
    }
    #endregion

    private void Initialize()
    {
        #region Extended Menus

        var ret = orderPayService.GetOrderFuncKey("02").Result;
        if (ret.Item2.ResultType == EResultType.SUCCESS)
        {
            ExtMenus = ret.Item1;
        }

        ret = orderPayService.GetOrderFuncKey("06").Result;
        if (ret.Item2.ResultType == EResultType.SUCCESS)
        {
            PayBtnKeys = ret.Item1;
        }

        #endregion

        this.OrderItems.CollectionChanged += OrderItems_CollectionChanged;
        this.ViewLoaded += OrderPayMainViewModel_ViewLoaded;
        this.ViewInitialized += OrderPayMainViewModel_ViewInitialized;
        this.ViewUnloaded += OrderPayMainViewModel_ViewUnloaded;
    }
    private void OrderItems_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
    {
        if (VisualTreeHelper.GetChildrenCount(_view.OrderItemsLV) > 0)
        {
            Decorator border = VisualTreeHelper.GetChild(_view.OrderItemsLV, 0) as Decorator;
            ScrollViewer scrollViewer = border.Child as ScrollViewer;
            scrollViewer.ScrollToBottom();
        }
    }

    private void OrderPayMainViewModel_ViewInitialized(object? sender, EventArgs e)
    {
        var extMenus = ExtMenus != null && ExtMenus.Any() ? ExtMenus.Take(5).ToArray() : new ORDER_FUNC_KEY[0];
        _view.RenderLeftFuncButtons(extMenus);
        Initialize_Trans();

    }

    /// <summary>
    /// TO DO
    /// - INit data for Grid 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void OrderPayMainViewModel_ViewLoaded(object? sender, EventArgs e)
    {
        if(!PostPay)
        {
            ItemGrid_Refresh();
            UpdateTRSummary();
            LoadLastHoldTR();
        }
        ActiveForm("ActiveItemLB", typeof(OrderPayLeftInfoKeypadViewModel));
        ActiveForm("ActiveItemR", typeof(OrderPayRightViewModel));
        IoC.Get<OrderPayLeftTRInfoViewModel>().ActiveItem = IoC.Get<PaymentInfoViewModel>();

        // Check 대기 

    }
    private void OrderPayMainViewModel_ViewUnloaded(object? sender, EventArgs e)
    {
        if (!PostPay)
        {
            OrderItems.Clear();
        }
    }

    public override bool SetIView(IView view)
    {
        _view = (IOrderPayMainView)view;
        return false;
    }

    #region Event handling

    public ICommand ButtonCommand => new RelayCommand<Button>(ButtonCommandClicked);

    private void ButtonCommandClicked(Button btn)
    {
        switch (btn.Tag)
        {
            case "ButtonClose":
                CheckClosePage();
                break;

            case "openlink":
                string url = "https://dndnstore.com/?utm_source=dndnstore_pos_main&utm_medium=pos_topbtn&utm_campaign=dndnstore_pos_topbtn&utm_id=dndnstore_pos";
                Process.Start(new ProcessStartInfo(url)
                {
                    UseShellExecute = true
                });
                break;

            default:
                string sTag = (string)btn.Tag;
                if (sTag.StartsWith("LeftBtn"))
                {
                    ItemGrid_Command(sTag.Substring(sTag.IndexOf("|") + 1));
                }
                break;
        }
    }

    public async void CheckPermission(string fk)
    {
        List<MST_EMP_FUNC_KEY> PermissonList = new List<MST_EMP_FUNC_KEY>();
        DynamicParameters param = new DynamicParameters();
        param.Add("@SHOP_CODE", DataLocals.AppConfig.PosInfo.StoreNo, DbType.String, ParameterDirection.Input, DataLocals.AppConfig.PosInfo.StoreNo.Length);
        param.Add("@EMP_NO", DataLocals.Employee.EMP_NO, DbType.String, ParameterDirection.Input, DataLocals.Employee.EMP_NO.Length);
        (PermissonList, SpResult spResult) = await employeeService.GetEmpFuncKey(param);

        var fkKey = PermissonList?.FirstOrDefault(p => p.FK_NO == fk);
        if (fkKey == null)
        {
            if (DialogHelper.MessageBox("웹 영업정보 시스템으로 로그인하시겠습니까?") != MessageBoxResult.OK)
            {
                return;
            }
            else
            {
                DialogHelper.ShowDialog(typeof(PermissionMngViewModel), 255, 410);
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    private void CheckClosePage()
    {
        bool isRetained = OrderCompleted && RetainLastOrder.ToString().StartsWith("Retain");
        bool hasTR = !isRetained && _OrderItemCount > 0;
        bool hasPayment = !isRetained && payTenders.Count > 0;

        var res = MessageBoxResult.OK;


        if (hasPayment)
        {
            res = DialogHelper.MessageBox("현재 결제처리중에 종료하려고 합니다.\r\n결제중 종료처리를 하시겠습니까 ?\r\n(해당 내역을 대기 처리되어집니다.)",
                GMessageBoxButton.OKCancel, MessageBoxImage.Question);

            if (res == MessageBoxResult.OK)
            {
                ProcessHoldTR(true);
            }
        }
        //else if (hasTR)
        //{
        //    res = DialogHelper.MessageBox("현재 판매등록중에 종료하려고 합니다.\r\n판매등록내역을 무시하고 종료하시겠습니까?",
        //        GMessageBoxButton.OKCancel, MessageBoxImage.Question);
        //}

        if (res == MessageBoxResult.Cancel)
        {
            return;
        }
        ClosePage(new string[] { "ActiveItem", "ActiveItemR", "ActiveItemLB" });
    }

    public ICommand ExtMenuCommand => new RelayCommand<Button>(ExtMenuCommandClicked);

    /// <summary>
    /// 기능버튼 클릭
    /// </summary>
    /// <param name="button"></param>
    private async void ExtMenuCommandClicked(Button button)
    {
        if (button.Tag == null)
        {
            return;
        }
        string tagKey = (string)button.Tag;
        if (tagKey.StartsWith("FK_"))
        {
            string fnKeyNo = tagKey.Substring(3);
            ProcessFuncKeyClicked(fnKeyNo);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="fnKey"></param>
    /// <param name="fkMap"></param>
    protected override void OnFuncKeyClicked(ORDER_FUNC_KEY fnKey, FkMapInfo fkMap)
    {
        ResetLastTR();
    }

    /// <summary>
    /// validate before moving to child, or popup
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected override void ValidateOnChildActivated(object sender, ChildActivatedEventArgs e)
    {
        string callerId = "ODP_MAIN";
        var payAmt = OrderSumInfo.EXP_PAY_AMT;
        if (!ValidateAmount(e, payAmt))
        {
            IoC.Get<OrderPayLeftTRInfoViewModel>().ButtonClick = true;
            IoC.Get<MemberInfoViewModel>().ButtonClick = true;
            return;

        }

        var inputAmt = Convert.ToDecimal(IoC.Get<OrderPayLeftInfoKeypadViewModel>().Text);
        if (inputAmt > 0)
        {
            payAmt = inputAmt;
        }


        switch (e.ChildVMType)
        {

            // reservation search, need to clear all items in grid
            case "OrderPayResveInqireViewModel":
                if (_OrderItemCount > 0)
                {
                    DialogHelper.MessageBox("등록된 상품이 있습니다. 전체취소 후 처리하세요.");
                    e.Cancelled = true;
                }
                break;


            case "OrderPayCardViewModel":
            case "OrderPaySimplePayViewModel":
                var pams = (object[])e.CSData;
                if (pams.Length == 0)
                {
                    e.CSData = new object[] { callerId, payAmt, e.ChildVMType == "OrderPayCardViewModel" ? payCards.Count : payEasys.Count };
                }
                break;

            case "OrderPayWaitingViewModel":
                ProcessHoldTR(false);
                e.Cancelled = false;
                break;

            case "OrderPayCompPayViewModel":
                e.CSData = new object[] { callerId, payAmt };
                break;

            // CashPayment, 현금결제
            case "OrderPayCashViewModel":
            case "OrderPayCoprtnDscntViewModel":
                var cpams = (object[])e.CSData;
                if (cpams.Length > 0)
                {
                    var insAmts = ((object[])e.CSData);
                    if (insAmts.Length > 0)
                    {
                        if (insAmts.Length > 1)
                        {
                            callerId = (string)insAmts[0];
                            payAmt = (decimal)insAmts[1];
                        }
                        else
                        {
                            payAmt = (decimal)insAmts[0];
                        }
                    }
                }

                e.CSData = new object[] {
                    callerId,
                    payAmt,
                    e.ChildVMType == "OrderPayCashViewModel" ? payCashs.Count : payPartCards.Count
                };

                break;
            case "OrderPayMemberPointUseViewModel":
                if (DataLocals.AppConfig.PosOption.StampUseMethod == "1" &&
                    DataLocals.AppConfig.PosOption.PointStampFlag == "1")
                {
                    e.Cancelled = true;
                    List<object> pams1 = new List<object>();
                    if (e.CSData != null)
                    {
                        if (e.CSData.GetType().IsArray)
                        {
                            pams1.AddRange(e.CSData as object[]);
                        }
                        else
                        {
                            pams1.Add(e.CSData);
                        }
                    }

                    ActiveForm("ActiveItemR", "OrderPayStampCouponViewModel", ValidateOnChildActivated, pams1.ToArray());
                }
                else
                {
                    var pamsPU = (object[])e.CSData;
                    if (pamsPU.Length == 0)
                    {
                        e.CSData = new object[] { callerId, payAmt, payPoints.Count, memberInfo };
                    }
                }
                break;

            case "OrderPayStampCouponViewModel":
                var pamsSC = (object[])e.CSData;
                if (pamsSC.Length == 0)
                {
                    e.CSData = new object[] { callerId, payAmt, payPoints.Count, OrderItems, memberInfo };
                }

                break;

            case "OrderPayPrepaymentUseViewModel":
                var PS = (object[])e.CSData;
                if (PS.Length == 0)
                {
                    e.CSData = new object[] { callerId, payAmt, payPpCards.Count, memberInfo };
                }
                break;
            default:
                break;
        }
    }

    bool ValidateAmount(ChildActivatedEventArgs e, decimal payAmt)
    {
        string childVM = e.ChildVMType.Replace("OrderPay", string.Empty);
        bool canContinue = true;
        if (_OrderItemCount == 0)
        {
            canContinue = !OrderPayExtensions.IsPayViewModel(e.ChildVMType);
        }
        else
        {
            if (payAmt <= 0 && !childVM.Contains("CashViewModel") &&
                OrderPayExtensions.IsPayViewModel(e.ChildVMType))
            {
                canContinue = false;
            }
        }

        if (!canContinue)
        {
            DialogHelper.MessageBox("결제 할 내역이 없습니다.", GMessageBoxButton.OK, MessageBoxImage.Warning);
            e.Cancelled = true;
        }

        return canContinue;
    }

    /// <summary>
    /// touch Product click
    /// Handle left grid
    /// </summary>
    /// <param name="tuProd"></param>
    public void TouchProductClicked(ORDER_TU_PRODUCT tuProd)
    {
        if (!ItemGrid_ValidatedOnAdded(tuProd))
        {
            return;
        }

        var exist = OrderItems.FirstOrDefault(p => p.PRD_CODE == tuProd.PRD_CODE);
        if (exist != null && exist.SIDE_MENU_YN == "N")
        {
            exist.SALE_QTY++;
            SelectedItemIndex = exist.NO - 1;
            ItemGrid_AddItem(true, null, 0);
            ItemGrid_Refresh();
            UpdateTRSummary();
        }
        else
        {
            var addingProd = new ORDER_GRID_ITEM()
            {
                NO = OrderItems.Count + 1,
                PRD_CODE = tuProd.PRD_CODE,
                PRD_NAME = tuProd.PRD_NAME,
                SALE_QTY = 1,
                //DC_AMT = 0,
                NORMAL_UPRC = tuProd.SALE_UPRC,
                //DCM_SALE_AMT = tuProd.SALE_UPRC,
                SIDE_MENU_YN = tuProd.SIDE_MENU_YN,
                PRD_TYPE_FLAG = tuProd.PRD_TYPE_CODE,
                //SIDE_MENU_INCL = false,
                TAX_YN = tuProd.TAX_YN,
                CLASS_CODE = string.Empty,
                CLASS_TYPE = "M",
                MAX_QTY = 0,
                PRD_DC_FLAG = tuProd.PRD_DC_FLAG,
                CST_ACCDC_YN = tuProd.CST_ACCDC_YN,
                STAMP_ACC_QTY = tuProd.STAMP_ACC_QTY,
                STAMP_ACC_YN = tuProd.STAMP_ACC_YN,
                STAMP_USE_YN = tuProd.STAMP_USE_YN,
                STAMP_USE_QTY = tuProd.STAMP_USE_QTY,
                PRICE_MGR_FLAG = tuProd.PRICE_MGR_FLAG
            };

            ItemGrid_AddItem(false, addingProd, 0);
        }
        ItemGrid_CheckShowSideMenu();
        NotifyOfPropertyChange(() => SelectedItemIndex);
        ItemGrid_Refresh();
        UpdateTRSummary();
    }

    /// <summary>
    /// 싯가상품 확인 flag
    /// </summary>
    private PriceMgrProdAddingStatus addingPriceMgr = PriceMgrProdAddingStatus.NoneAdd;
    private ORDER_GRID_ITEM addingPriceMgrItem = null;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="areaName"></param>
    /// <param name="activated"></param>
    /// <param name="data"></param>
    public override void ChildActivated(string areaName, bool activated, object data)
    {
        if (!activated)
        {
            // get last ActiveItem
            var scr = GetScreen(areaName);
            if (scr != null)
            {
                ActiveForm(areaName, scr, true);
            }

            IoC.Get<OrderPayLeftInfoKeypadViewModel>().SetKeyPadFocus();
            _view.ResetSelectedExtButton();
        }

        _view.DisableElements((string)data, activated);
    }

    #endregion

    #region Item Grid

    private int _OrderItemCount
    {

        get
        {
            return OrderItems.Count;
        }
    }

    private int _OrderItemCountNotProperty
    {

        get
        {
            return OrderItems.Count(p => p.PRD_TYPE_FLAG != "2");
        }
    }

    public ObservableCollection<ORDER_GRID_ITEM> OrderItems
    {
        get; set;
    } = new ObservableCollection<ORDER_GRID_ITEM>();

    private int _SelectedItemIndex = -1;
    public int SelectedItemIndex
    {
        get
        {
            return _SelectedItemIndex;
        }
        set
        {
            _SelectedItemIndex = value;
            NotifyOfPropertyChange(() => SelectedItemIndex);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="commandId"></param>
    private bool ItemGrid_ValidateAction(string commandId)
    {
        switch (commandId)
        {
            case "Discount" or "AllCancel" or "SelCancel" or "Minus" or "Plus" or "CntChange":
                if (payTenders.Count > 0)
                {
                    DialogHelper.MessageBox("일부 결제내역이 있음으로 사용이 불가능합니다.", GMessageBoxButton.OK, MessageBoxImage.Warning);
                    return false;
                }

                if (OrderItems.Count == 0)
                {
                    return false;
                }

                //if (OrderItems.Count == 1 && OrderItems.Sale && commandId.Equals("Minus") && memberInfo == null)
                //{
                //   

                //}

                return true;
            default:
                return true;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="commandId"></param>
    private void ItemGrid_Command(string commandId)
    {
        if (!ItemGrid_ValidateAction(commandId)) return;
        switch (commandId)
        {
            case "AllCancel":
                ItemGrid_OnItemCancelled(-1);
                break;
            case "SelCancel":
                if (ItemGrid_ValidateSelection())
                    ItemGrid_OnSelectedItemQtyChange(OrderItems[SelectedItemIndex], 0);
                break;
            case "Discount":
                if (m_showingSideMenu)
                {
                    break;
                }
                ActiveForm("ActiveItemR", typeof(OrderPayFixDisViewModel));
                break;
            case "CntChange":
                if (ItemGrid_ValidateSelection())
                {
                    string textInput = IoC.Get<OrderPayLeftInfoKeypadViewModel>().Text.Trim();
                    int qty = Convert.ToInt32(textInput);
                    if (qty <= 0) break;
                    ItemGrid_OnSelectedItemQtyChange(OrderItems[SelectedItemIndex], qty);
                }
                break;
            case "Minus":
                if (ItemGrid_ValidateSelection())
                    ItemGrid_OnSelectedItemQtyChange(OrderItems[SelectedItemIndex], -1);
                break;
            case "Plus":
                if (ItemGrid_ValidateSelection())
                    ItemGrid_OnSelectedItemQtyChange(OrderItems[SelectedItemIndex], -2);
                break;
            case "Up":
                if (SelectedItemIndex > 0)
                {
                    SelectedItemIndex--;
                }
                break;
            case "Down":
                if (SelectedItemIndex < _OrderItemCount - 1)
                {
                    SelectedItemIndex++;
                }
                break;
            default:
                break;
        }

        ////Here
    }

    void ItemGrid_OnSelectedItemQtyChange(ORDER_GRID_ITEM item, int qty)
    {
        decimal saleQty = item.SALE_QTY;
        /// +1
        if (qty == -2)
        {
            saleQty++;
        }
        else if (qty == -1)
        {
            saleQty--;
        }
        else
        {
            saleQty = qty;
        }

        /// cancel selected item
        if (saleQty == 0)
        {
            int idx = SelectedItemIndex;
            ItemGrid_OnItemCancelled(idx);

            if (_OrderItemCount == 0)
            {
                idx = -1;
            }

            SelectedItemIndex = Math.Min(idx, _OrderItemCount - 1);
        }
        else
        {
            if (saleQty > 999)
            {
                StatusMessage = "최대수량 999입니다.";
                return;
            }

            decimal beforeQty = item.SALE_QTY;
            ItemGrid_OnSaleQtyChanged(item, SelectedItemIndex, beforeQty, saleQty);
        }

    }

    void ItemGrid_Refresh()
    {
        // reordering
        int index_dis = 1;
        for (int i = 0; i < this.OrderItems.Count; i++)
        {
            this.OrderItems[i].NO = i + 1;
            if (this.OrderItems[i].CLASS_TYPE.Equals("M"))
            {
                this.OrderItems[i].NODIS = index_dis + "";
                index_dis++;
            }
            else
            {
                this.OrderItems[i].NODIS = "";
            }
        }

        if (_OrderItemCount == 0 && MemberInfo == null)
        {
            IoC.Get<DualScreenMainViewModel>().SwitchMode(EDislayType.Logo);
        }

        if (memberInfo != null) { MemberDiscount(memberInfo, true); }
        NotifyOfPropertyChange(() => OrderItems);
    }

    /// <summary>
    /// cancelIdx = -1 = cancel all
    /// </summary>
    /// <param name="cancelIdx"></param>
    void ItemGrid_OnItemCancelled(int cancelIdx)
    {
        // 전체취소
        if (cancelIdx == -1)
        {
            if (CanCancelAll())
            {
                this.OrderItems.Clear();
                NotifyOfPropertyChange(() => OrderItems);
                ItemGrid_AddItem(true, null, 0);
            }
        }
        else
        {
            var newList = new ObservableCollection<ORDER_GRID_ITEM>();
            for (int i = 0; i < cancelIdx; i++)
            {
                newList.Add(OrderItems[i]);
            }

            int nextIdx = cancelIdx + 1;
            var item = OrderItems[cancelIdx];
            if ("0".Equals(item.PRD_TYPE_FLAG))
            {
                for (int i = cancelIdx + 1; i < OrderItems.Count; i++)
                {
                    var sitem = OrderItems[i];
                    if (!"0".Equals(sitem.PRD_TYPE_FLAG))
                    {
                        nextIdx++;
                    }
                    else
                    {
                        break;
                    }
                }
            }

            // remove child items
            for (int i = nextIdx; i < OrderItems.Count; i++)
            {
                newList.Add(OrderItems[i]);
            }

            OrderItems.Clear();
            OrderItems.CollectionChanged -= OrderItems_CollectionChanged;
            OrderItems = new ObservableCollection<ORDER_GRID_ITEM>(newList);
            OrderItems.CollectionChanged += OrderItems_CollectionChanged;
        }

        ItemGrid_Refresh();
        UpdateTRSummary();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="infoProducts"></param>
    /// <returns></returns>
    protected bool ItemGrid_ValidatedOnAdded(params object[] infoProducts)
    {
        ResetLastTR();

        if (payTenders.Count > 0)
        {
            DialogHelper.MessageBox("일부결제 내역이 있음으로 상품을 추가 할 수 없습니다.", GMessageBoxButton.OK, MessageBoxImage.Warning);
            return false;
        }

        if (infoProducts != null && infoProducts.Length > 0)
        {
            var firstObj = infoProducts[0];
            if (firstObj != null)
            {
                var pi = firstObj.GetType().GetProperty("STOCK_OUT_YN", BindingFlags.Public | BindingFlags.Instance);
                var stkOut = pi.GetValue(firstObj, null);
                bool stockOut = stkOut == null ? false : stkOut.ToString() == "Y";

                pi = firstObj.GetType().GetProperty("PRD_NAME", BindingFlags.Public | BindingFlags.Instance);
                string prdName = pi.GetValue(firstObj, null).ToString();

                if (stockOut)
                {
                    var insStkOutPrd = DialogHelper.MessageBox(prdName + "은(는) 품절 되었습니다. 그래도 주문하시겠습니까?", GMessageBoxButton.OKCancel, MessageBoxImage.Question);
                    if (insStkOutPrd == MessageBoxResult.Cancel) return false;
                }
            }
        }

        IoC.Get<DualScreenMainViewModel>().SwitchMode(EDislayType.Order);

        return true;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="infoProducts"></param>
    public void ItemGrid_OnItemAdded(params MST_INFO_PRODUCT[] infoProducts)
    {
        if (!ItemGrid_ValidatedOnAdded(infoProducts) || infoProducts.Length == 0)
        {
            return;
        }

        var item = infoProducts[0];
        var exist = OrderItems.FirstOrDefault(p => p.PRD_CODE == item.PRD_CODE);
        if (exist != null && exist.SIDE_MENU_YN == "N")
        {
            exist.SALE_QTY++;
            SelectedItemIndex = exist.NO - 1;
            ItemGrid_AddItem(true, null, 0);
        }
        else
        {
            var addingProd = new ORDER_GRID_ITEM()
            {
                NO = OrderItems.Count + 1,
                PRD_CODE = item.PRD_CODE,
                PRD_NAME = item.PRD_NAME,
                SALE_QTY = 1,
                NORMAL_UPRC = item.SALE_UPRC ?? 0,
                // DCM_SALE_AMT = item.SALE_UPRC ?? 0,
                SIDE_MENU_YN = item.SIDE_MENU_YN,
                PRD_TYPE_FLAG = "0",
                TAX_YN = item.TAX_YN,
                CLASS_CODE = string.Empty,
                CLASS_TYPE = "M",
                MAX_QTY = 0,
                PRD_DC_FLAG = item.PRD_DC_FLAG,
                CST_ACCDC_YN = item.CST_ACCDC_YN,
                STAMP_ACC_QTY = item.STAMP_ACC_QTY,
                STAMP_ACC_YN = item.STAMP_ACC_YN,
                STAMP_USE_YN = item.STAMP_USE_YN,
                PRICE_MGR_FLAG = item.PRICE_MGR_FLAG
            };

            ItemGrid_AddItem(false, addingProd, 0);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="addItem"></param>
    private void ItemGrid_AddItem(bool resetAdd, ORDER_GRID_ITEM addItem, decimal inputPrice)
    {
        if (resetAdd)
        {
            addingPriceMgr = PriceMgrProdAddingStatus.NoneAdd;
            StatusMessageNoReset = string.Empty;
            addingPriceMgrItem = null;
            return;
        }

        if (addItem != null)
        {
            // 싯가상품
            if ("1".Equals(addItem.PRICE_MGR_FLAG) && inputPrice == 0)
            {
                addingPriceMgr = PriceMgrProdAddingStatus.AddingTuch;
                addingPriceMgrItem = addItem;
                StatusMessageNoReset = "현재 판매단가를 입력하세요.";
            }
            else
            {
                OrderItems.Add(addItem);
                SelectedItemIndex = OrderItems.Count - 1;
                ItemGrid_CheckShowSideMenu();
                ItemGrid_Refresh();
                UpdateTRSummary();

                addingPriceMgr = PriceMgrProdAddingStatus.NoneAdd;
                StatusMessageNoReset = string.Empty;
            }
        }
        else
        {
            if (addingPriceMgrItem != null)
            {
                addingPriceMgrItem.NORMAL_UPRC = inputPrice;
                ItemGrid_AddItem(false, addingPriceMgrItem, inputPrice);
                addingPriceMgrItem = null;
            }

            addingPriceMgr = PriceMgrProdAddingStatus.NoneAdd;
        }

    }

    private bool ItemGrid_ValidateSelection()
    {
        if (SelectedItemIndex < 0)
        {
            DialogHelper.MessageBox("상품은 먼저 선택하여 주십시오.");
            return false;
        }
        return true;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    private bool CanCancelAll()
    {
        if (_OrderItemCount == 0) return true;
        var hasPayment = payCards.Count > 0 ||
                            payCashs.Count > 0;
        if (hasPayment)
        {
            DialogHelper.MessageBox("이미 결제처리된 내역이 있어 취소처리를 할 수 없습니다.");
            return false;
        }

        var res = DialogHelper.MessageBox("해당 등록되어진 판매상품 내역을\n전체취소 하시겠습니까?",
            GMessageBoxButton.OKCancel, MessageBoxImage.Question);
        return res == MessageBoxResult.OK;
    }

    /// <summary>
    /// if normal item
    ///     + auto inc side if any
    /// if side item
    ///     = change but check max qty
    /// </summary>
    /// <param name="item"></param>
    /// <param name="beforeQty"></param>
    void ItemGrid_OnSaleQtyChanged(ORDER_GRID_ITEM item, int itemIndex, decimal beforeQty, decimal newQty)
    {
        // main item
        if ("0".Equals(item.PRD_TYPE_FLAG))
        {
            item.SALE_QTY = Convert.ToInt32(newQty);

            /// 
            /// Option 처리
            /// 미사용일 경우 옵션 수량 변경 안된다.(Default)
            /// 사용일 경우는 옵션 수량이 따로 변경 되어야 한다.
            /// SideMenuQtyIncOpt = 0: auto change to selection and property item
            /// SideMenuQtyIncOpt = 1: each item (main and sub) change differently
            /// 
            if (DataLocals.AppConfig.PosOption.SideMenuQtyIncOpt == "0")
            {
                // auto update side
                var childItems = GetChildItems(item.PRD_CODE, itemIndex, 0);
                foreach (var si in childItems.Item1)
                {
                    si.SALE_QTY = item.SALE_QTY;
                }
            }
        }
        else
        {
            /// 
            /// Option 처리
            /// 미사용일 경우 옵션 수량 변경 안된다.(Default)
            /// 사용일 경우는 옵션 수량이 따로 변경 되어야 한다.
            /// SideMenuQtyIncOpt = 0: auto change to selection and property item
            /// SideMenuQtyIncOpt = 1: each item (main and sub) change differently
            /// 
            if (DataLocals.AppConfig.PosOption.SideMenuQtyIncOpt != "0")
            {
                item.SALE_QTY = Convert.ToInt32(newQty);

                var mainItem = FindParentItem(item.PRD_CODE, itemIndex);
                if (item.SALE_QTY > mainItem?.SALE_QTY)
                {
                    item.SALE_QTY = Convert.ToInt32(beforeQty);
                }
                if (item.SALE_QTY > item.MAX_QTY && item.MAX_QTY != 0)
                {
                    item.SALE_QTY = item.MAX_QTY;
                }
            }
        }

        // update amount
        // item.DCM_SALE_AMT = item.SALE_QTY * item.NORMAL_UPRC - item.DC_AMT;
        ItemGrid_Refresh();
        UpdateTRSummary();
    }

    /// <summary>
    /// 전체 금액할인, 전체 퍼센트 할인은 같이 적용 가능
    /// 그리고 선택 금액할인, 선택 퍼센트 할인은 같이 적용 가능
    /// 하지만 전체와 선택 할인은 중복 할인이 안된다.
    /// </summary>
    /// <param name="isAll"></param>
    /// <param name="iaAmt"></param>
    /// <param name="isApply"></param>
    /// <param name="fixDc"></param>
    public bool DiscountApply(bool isAll, bool isAmt, bool isApply, decimal fixDc)
    {
        if (SelectedItemIndex < 0 && !isAll)
        {
            DialogHelper.MessageBox("상품은 먼저 선택하여 주십시오.");
            return false;
        }

        if (fixDc <= 0 && isApply) return false;

        // Check duplicated apply discount
        bool hasAppDisc = false;
        if (!isAll) // only selected item to apply disc
        {
            var mainItem = OrderItems[SelectedItemIndex];
            List<ORDER_GRID_ITEM> applyItems = new List<ORDER_GRID_ITEM>();
            applyItems.Add(OrderItems[SelectedItemIndex]);

            var subItems = GetChildItems(mainItem.PRD_CODE, SelectedItemIndex, 1).Item1;
            applyItems.AddRange(subItems);

            foreach (var item in applyItems)
            {
                if ("P".Equals(item.CLASS_TYPE))
                    continue;

                if (!isApply)
                {
                    item.UpdateRemarkItem("DC", string.Empty, false);
                    item.UpdateRemarkItem("DCP", string.Empty, false);

                    item.DC_VALUE = 0;
                    item.DC_AMT_GEN = 0;
                    item.DiscSelAmt = false;
                    item.DiscSelPer = false;
                    item.DiscAllAmt = item.DiscAllPer = false;

                    continue;
                }

                /*
                 *
                 *전체 금액할인, 전체 퍼센트 할인은 같이 적용 가능
                 *그리고 선택 금액할인, 선택 퍼센트 할인은 같이 적용 가능
                 *하지만 전체와 선택 할인은 중복 할인이 안된다.
                */
                //if (item.HasRemarkItem("DC") && isApply)
                if ((item.DiscAllAmt || item.DiscAllPer) && isApply)
                {
                    hasAppDisc = true;
                    // DialogHelper.MessageBox("이미 할인처리 된상품에 할인처리 불가능합니다.", GMessageBoxButton.OK, MessageBoxImage.Warning);
                    continue;
                }

                item.DC_VALUE = fixDc;
                var amtToApply = item.DCM_SALE_AMT;
                if (amtToApply <= 0)
                {
                    continue;
                }

                var dcAmt = isApply ? (isAmt ? fixDc : fixDc * amtToApply / 100) : (memberInfo != null ? item.DC_AMT_CST : 0);
                if (dcAmt > item.DCM_SALE_AMT)
                {
                    DialogHelper.MessageBox("할인금액이 상품금액보다 클 수 없습니다.", GMessageBoxButton.OK, MessageBoxImage.Warning);
                    return false;
                }


                if (isAmt & item.DiscSelAmt)
                {
                    hasAppDisc = true;
                    continue;
                }

                if (!isAmt && item.DiscSelPer)
                {
                    hasAppDisc = true;
                    continue;
                }

                item.DC_AMT_GEN = isApply ? item.DC_AMT_GEN + dcAmt : 0;
                if (isAmt) item.DiscSelAmt = true;
                if (!isAmt) item.DiscSelPer = true;

                item.UpdateRemarkItem(isAmt ? "DC" : "DCP", string.Format("{0:#,##0}{1}DC", fixDc, isAmt ? "원" : "%"), true);
            }

            if (hasAppDisc)
            {
                DialogHelper.MessageBox("이미 할인처리 된상품에 할인처리 불가능합니다.", GMessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
        }
        else
        {
            if (!isApply)
            {
                for (int i = 0; i < OrderItems.Count; i++)
                {
                    var item = OrderItems[i];
                    item.DC_VALUE = 0;
                    item.DC_AMT_GEN = 0;
                    item.DiscAllAmt = item.DiscAllPer = item.DiscSelPer = item.DiscSelAmt = false;
                    item.UpdateRemarkItem("DC", string.Empty, false);
                    item.UpdateRemarkItem("DCP", string.Empty, false);
                }
            }
            else
            {
                #region check all amt

                // check all amt
                decimal dcValue = isAmt ? fixDc : fixDc;
                var dcAmt = isAmt ? dcValue : dcValue * OrderSumInfo.DCM_SALE_AMT / 100;

                if (dcAmt > OrderSumInfo.DCM_SALE_AMT)
                {
                    DialogHelper.MessageBox("할인금액이 받을금액보다 클 수 없습니다.", GMessageBoxButton.OK, MessageBoxImage.Warning);
                    return false;
                }

                #endregion

                #region collect items to aply

                List<ORDER_GRID_ITEM> applyItems = new List<ORDER_GRID_ITEM>();

                bool applyMinus = false;
                bool hasAllApplied = false;
                for (int i = 0; i < OrderItems.Count; i++)
                {
                    var item = OrderItems[i];
                    if ("P".Equals(item.CLASS_TYPE))
                        continue;

                    //if (item.HasRemarkItem("DC"))
                    if (item.DiscSelAmt || item.DiscSelPer)
                    {
                        hasAppDisc = true;
                        continue;
                    }

                    if (item.DiscAllAmt || item.DiscAllPer)
                    {
                        hasAllApplied = true;
                        break;
                    }

                    var amtToApply = item.DCM_SALE_AMT;
                    if (amtToApply <= 0)
                    {
                        continue;
                    }

                    if (!isAmt)
                    {
                        var aDcValue = fixDc;
                        var aDcAmt = aDcValue * amtToApply / 100;
                        var remainAmt = amtToApply - aDcAmt;

                        if (remainAmt < 0)
                        {
                            applyMinus = true;
                            continue;
                        }
                    }

                    applyItems.Add(item);
                }

                if (applyMinus)
                {
                    DialogHelper.MessageBox("상품금액보다 할인금액 클 수 없습니다.", GMessageBoxButton.OK, MessageBoxImage.Warning);
                    return false;
                }

                if (hasAppDisc)
                {
                    DialogHelper.MessageBox("이미 할인처리 된상품에 할인처리 불가능합니다.", GMessageBoxButton.OK, MessageBoxImage.Warning);
                    return false;
                }

                if (hasAllApplied)
                {
                    DialogHelper.MessageBox("이미 할인처리 된상품에 할인처리 불가능합니다.", GMessageBoxButton.OK, MessageBoxImage.Warning);
                    return false;
                }

                #endregion

                #region Apply percentage

                if (!isAmt)
                {
                    foreach (var item in applyItems)
                    {
                        item.DC_VALUE = fixDc;
                        var amtToApply = item.DCM_SALE_AMT;
                        item.DC_AMT_GEN += item.DC_VALUE * amtToApply / 100;

                        // if (item.REMARK.Contains("%")) { return; }
                        item.UpdateRemarkItem("DCP", string.Format("{0:#,##0}{1}DC", item.DC_VALUE, "%"), true);

                        item.DiscAllPer = true;
                    }

                    UpdateTRSummary();
                    return true;
                }

                #endregion

                #region Apply amount equally based on each amount of item

                decimal appliedAmt = 0;
                for (int i = 0; i < applyItems.Count - 1; i++)
                {
                    var item = applyItems[i];
                    var amtToApply = item.DCM_SALE_AMT;
                    decimal percentOfItem = amtToApply / OrderSumInfo.DCM_SALE_AMT;
                    decimal amountToDC = (int)TypeHelper.RoundNear(percentOfItem * fixDc, 1);
                    appliedAmt += amountToDC;

                    item.DC_VALUE = amountToDC;
                    item.DC_AMT_GEN += amountToDC;

                    //if (item.REMARK.Contains("금액")) { continue; }
                    item.UpdateRemarkItem("DC", "금액DC", true);

                    item.DiscAllAmt = true;
                }

                #region last item

                decimal lastAmtToDC = fixDc - appliedAmt;

                applyItems[applyItems.Count - 1].DC_VALUE = lastAmtToDC;
                applyItems[applyItems.Count - 1].DC_AMT_GEN += lastAmtToDC;
                applyItems[applyItems.Count - 1].UpdateRemarkItem("DC", "금액DC", true);

                applyItems[applyItems.Count - 1].DiscAllAmt = true;

                #endregion

                #endregion
            }
        }

        IoC.Get<InputBoxKeyPadView>().ClearText();
        UpdateTRSummary();

        return true;
    }

    private bool m_showingSideMenu
    {
        get
        {
            return ActiveFormIs("ActiveItemR", typeof(OrderPaySideMenuViewModel));
        }
    }

    private bool isDiscountMode
    {
        get
        {
            return ActiveFormIs("ActiveItemR", typeof(OrderPayFixDisViewModel));
        }
    }

    private int m_lastItemSelIndex = -1;
    /// <summary>
    /// 
    /// </summary>
    public void ItemGrid_CheckShowSideMenu()
    {
        if (isDiscountMode || SelectedItemIndex < 0 || SelectedItemIndex > OrderItems.Count - 1)
        {
            return;
        }

        var item = OrderItems[SelectedItemIndex];
        if ("Y".Equals(item.SIDE_MENU_YN))
        {
            if (!m_showingSideMenu)
            {
                //Nhu
                ActiveForm("ActiveItemR", typeof(OrderPaySideMenuViewModel));
                IoC.Get<OrderPaySideMenuViewModel>().LoadSideMenus(item);
                m_lastItemSelIndex = SelectedItemIndex;
                return;
            }

            if (m_lastItemSelIndex != SelectedItemIndex)
            {
                m_lastItemSelIndex = SelectedItemIndex;
                IoC.Get<OrderPaySideMenuViewModel>().LoadSideMenus(item);
                return;
            }
        }

        if (m_showingSideMenu)
        {
            IoC.Get<OrderPaySideMenuViewModel>().TryCloseAsync(false);
        }
        m_lastItemSelIndex = SelectedItemIndex;
    }

    private ORDER_GRID_ITEM[] OrderItemsBackup = null;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="prdCode"></param>
    /// <param name="itemIndex"></param>
    /// <param name="childType">0:전체;1:선택:2:속성</param>
    /// <param name="subCodeCTypes"></param>
    /// <returns></returns>
    private (ORDER_GRID_ITEM[], ORDER_GRID_ITEM) GetChildItems(string prdCode, int itemIndex,
        int childType, params string[] subCodeCTypes)
    {
        List<ORDER_GRID_ITEM> childItems = new List<ORDER_GRID_ITEM>();
        for (int i = itemIndex + 1; i < _OrderItemCount; i++)
        {
            var si = OrderItems[i];
            if (si.PRD_TYPE_FLAG == "0")
            {
                break;
            }

            if (childType > 0 & si.PRD_TYPE_FLAG == childType.ToString())
            {
                continue;
            }

            childItems.Add(si);
        }

        ORDER_GRID_ITEM findItem = null;
        if (subCodeCTypes.Length > 0)
        {
            findItem = childItems.FirstOrDefault(p => p.PRD_CODE == subCodeCTypes[0]);
        }
        return (childItems.ToArray(), findItem);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="subCode"></param>
    /// <param name="itemIndex"></param>
    /// <returns></returns>
    private ORDER_GRID_ITEM FindParentItem(string subCode, int itemIndex)
    {
        ORDER_GRID_ITEM parentItem = null;
        for (int i = itemIndex - 1; i >= 0; i--)
        {
            var si = OrderItems[i];
            if (si.PRD_TYPE_FLAG == "0")
            {
                parentItem = si;
                break;
            }
        }
        return parentItem;
    }

    /// <summary>
    /// Add to grid
    ///     belong to selected item;
    ///     - selected index
    ///     - having side or not, add insert at 
    ///     - remark
    ///  if exist, add 1 qty
    /// </summary>
    /// <param name="sideProd"></param>
    public void ItemGrid_SideMenuSelected(ORDER_SIDE_CLS_MENU? sideProd)
    {
        if (sideProd == null)
        {
            return;
        }

        if (OrderItemsBackup == null)
        {
            var lOrderItemsBackup = new List<ORDER_GRID_ITEM>();
            for (int i = 0; i < OrderItems.Count; i++)
            {
                lOrderItemsBackup.Add(OrderItems[i].Copy());
            }

            OrderItemsBackup = lOrderItemsBackup.ToArray();
        }

        var mainItem = OrderItems[SelectedItemIndex];

        /// 
        /// TO DO
        /// 1) check dup in 속성
        /// 2) inc in same sub, check max qty
        /// 
        var childItems = GetChildItems(mainItem.PRD_CODE, SelectedItemIndex, 0,
                /*sideProd.CLASS_CODE +*/ sideProd.SUB_CODE);

        ///
        ///  SideMenuProOpt = 0, single select for Prop and Selection / item
        ///  =1, multi
        ///  [532] 사이드메뉴 처리구분
        ///  
        if (DataLocals.AppConfig.PosOption.SideMenuProOpt == "0" &&
            childItems.Item1.Length > 0)
        {
            return;
        }

        var existItem = childItems.Item2;

        if (existItem == null)
        {
            existItem = new ORDER_GRID_ITEM()
            {
                PRD_CODE = sideProd.SUB_CODE,
                MARK_ICON = "u",
                PRD_NAME = sideProd.SUB_NAME,
                NORMAL_UPRC = sideProd.UNIT_PRICE,
                SALE_QTY = 1,
                CLASS_TYPE = sideProd.CLASS_TYPE,
                CLASS_CODE = sideProd.CLASS_CODE,
                UP_PRD_CODE = mainItem.PRD_CODE,
                PRD_TYPE_FLAG = sideProd.PRD_TYPE_FLAG,
                TAX_YN = sideProd.TAX_YN,
                MAX_QTY = sideProd.MAX_QTY
            };

            existItem.UpdateRemarkItem("CLASS", "2".Equals(sideProd.PRD_TYPE_FLAG) ? "속성" : "선택", true);

            int insIndex = childItems.Item1.Length == 0 ? SelectedItemIndex + 1 : childItems.Item1.Max(p => p.NO);
            OrderItems.Insert(Math.Min(insIndex, _OrderItemCount), existItem);
        }
        else
        {
            if (/*"P".Equals(sideProd.CLASS_TYPE) ||*/
                (sideProd.MAX_QTY == 0 || existItem.SALE_QTY < sideProd.MAX_QTY))
            {
                // no dup
                existItem.SALE_QTY++;
            }
        }

        ItemGrid_Refresh();
        UpdateTRSummary();
    }

    Task ItemGrid_SideMenuConfirmed(bool confirmed)
    {
        if (!confirmed && OrderItemsBackup != null)
        {
            OrderItems.Clear();
            for (int i = 0; i < OrderItemsBackup.Length; i++)
            {
                OrderItems.Add(OrderItemsBackup[i].Copy());
            }

            ItemGrid_Refresh();
            UpdateTRSummary();
        }

        OrderItemsBackup = null;
        return Task.CompletedTask;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="message"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task HandleAsync(SideMenuConfirmEventArgs message, CancellationToken cancellationToken)
    {
        return ItemGrid_SideMenuConfirmed(message.Confirmed);
    }

    /// <summary>
    /// 할인처리 팝업
    /// </summary>
    /// <param name="message"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public Task HandleAsync(OrderPayDiscHandleEventArgs message, CancellationToken cancellationToken)
    {
        if (message.Action == OrderPayDiscHandleActions.Apply)
        {
            string textInput = IoC.Get<OrderPayLeftInfoKeypadViewModel>().Text;
            decimal fixDc = string.IsNullOrEmpty(textInput) ? 0 : Convert.ToDecimal(textInput);
            var res = DiscountApply(message.IsAll, message.IsAmt, message.IsApply, fixDc);

            if (res)
            {
                _eventAggregator.PublishOnUIThreadAsync(new OrderPayDiscHandleEventArgs()
                {
                    Action = OrderPayDiscHandleActions.CloseRight
                });
            }
        }
        else if (message.Action == OrderPayDiscHandleActions.CloseRight)
        {
            IoC.Get<OrderPayFixDisViewModel>().TryCloseAsync(false);
        }
        return Task.CompletedTask;
    }

    #endregion

    #region Payment and TR

    private RetainLastOrderTypes RetainLastOrder = RetainLastOrderTypes.None;
    private bool OrderCompleted = false;

    public TRN_HEADER trHeader { get; private set; }
    public List<TRN_PRDT> trProducts { get; private set; }
    public List<COMPPAY_PAY_INFO> payInfos { get; private set; }
    public List<TRN_TENDERSEQ> payTenders { get; private set; }
    public List<TRN_CASH> payCashs { get; private set; }
    public List<TRN_CASHREC> payCashRecs { get; private set; }
    public List<TRN_CARD> payCards { get; private set; }
    public List<TRN_PARTCARD> payPartCards { get; private set; }
    public List<TRN_POINTUSE> payPoints { get; private set; }
    public List<TRN_POINTSAVE> savePoints { get; private set; }
    public List<TRN_GIFT> payGifts { get; private set; }
    public List<TRN_FOODCPN> payFoodCpns { get; private set; }
    public List<TRN_EASYPAY> payEasys { get; private set; }
    public List<TRN_PPCARD> payPpCards { get; private set; }

    private ORDER_SUM_INFO orderSumInfo;

    private void Initialize_Trans()
    {
        OrderItems.Clear();
        OrderCompleted = false;
        trProducts = new List<TRN_PRDT>();
        payInfos = new List<COMPPAY_PAY_INFO>();
        //IoC.Get<OrderPayPrepaymentUseViewModel>().ResetData();        
        trHeader = new TRN_HEADER()
        {
            SALE_DATE = DataLocals.PosStatus.SALE_DATE,
            SHOP_CODE = DataLocals.PosStatus.SHOP_CODE,
            POS_NO = DataLocals.PosStatus.POS_NO,
            REGI_SEQ = DataLocals.PosStatus.REGI_SEQ,
            SALE_YN = "Y",
            DLV_ORDER_FLAG = "0",
            EMP_NO = DataLocals.Employee.EMP_NO
        };

        payTenders = new List<TRN_TENDERSEQ>();
        payCashs = new List<TRN_CASH>();
        payCashRecs = new List<TRN_CASHREC>();
        payCards = new List<TRN_CARD>();
        payPartCards = new List<TRN_PARTCARD>();
        payGifts = new List<TRN_GIFT>();
        payFoodCpns = new List<TRN_FOODCPN>();
        payEasys = new List<TRN_EASYPAY>();
        payPoints = new List<TRN_POINTUSE>();
        savePoints = new List<TRN_POINTSAVE>();
        payPpCards = new List<TRN_PPCARD>();
        OrderSumInfo = new ORDER_SUM_INFO();
        memberInfo = null;
        StatusMessageNoReset = string.Empty;

        _eventAggregator.PublishOnUIThreadAsync(new MemberInfoPass()
        {
            memberInfo = null

        });
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="viewModelName"></param>
    /// <param name="trData"></param>
    public bool UpdatePaymentTRN(string viewModelName, COMPPAY_PAY_INFO payInfo)
    {
        var paySeq = (payTenders.Count > 0 ? payTenders.Max(p => p.PAY_SEQ_NO) : 0);
        payInfo.PAY_SEQ = (payInfos.Count + 1).ToString();
        payInfos.Add(payInfo);

        #region CASH

        if ("OrderPayCashViewModel".Equals(viewModelName))
        {
            object[] payDatas = (object[])payInfo.PayDatas;
            var cashPay = payDatas[0] as TRN_CASH;
            var cashRecPay = payDatas.Length > 1 ? (TRN_CASHREC)payDatas[1] : null;
            AddPaymentCashTR(cashPay, cashRecPay, paySeq, 0);
        }

        #endregion

        #region CASH RECEIPT ONLY

        if ("OrderPayCashReceiptViewModel".Equals(viewModelName))
        {
            object[] payDatas = (object[])payInfo.PayDatas;
            var cashRecPay = (TRN_CASHREC)payDatas[0];
            payCashRecs.Add(cashRecPay);

            paySeq++;
            payTenders.Add(new TRN_TENDERSEQ()
            {
                SHOP_CODE = DataLocals.PosStatus.SHOP_CODE,
                SALE_DATE = DataLocals.PosStatus.SALE_DATE,
                POS_NO = DataLocals.PosStatus.POS_NO,
                BILL_NO = DataLocals.PosStatus.BILL_NO,
                PAY_SEQ_NO = (Int16)paySeq,
                REGI_SEQ = DataLocals.PosStatus.REGI_SEQ,
                SALE_YN = "Y",
                PAY_TYPE_FLAG = OrderPayConsts.PAY_CASHREC,
                PAY_AMT = cashRecPay.APPR_AMT,
                LINE_NO = cashRecPay.SEQ_NO,
                EMP_NO = DataLocals.Employee.EMP_NO,
                INSERT_DT = DateTime.Now.ToString("yyyyMMddHHmmss"),
                UPDATE_DT = DateTime.Now.ToString("yyyyMMddHHmmss")
            });
        }

        #endregion

        #region CARD

        if ("OrderPayCardViewModel".Equals(viewModelName))
        {
            var cardPay = (TRN_CARD)payInfo.PayDatas[0];
            payCards.Add(cardPay);
            paySeq++;
            payTenders.Add(new TRN_TENDERSEQ()
            {
                SHOP_CODE = DataLocals.PosStatus.SHOP_CODE,
                SALE_DATE = DataLocals.PosStatus.SALE_DATE,
                POS_NO = DataLocals.PosStatus.POS_NO,
                BILL_NO = DataLocals.PosStatus.BILL_NO,
                PAY_SEQ_NO = (Int16)paySeq,
                REGI_SEQ = DataLocals.PosStatus.REGI_SEQ,
                SALE_YN = "Y",
                PAY_TYPE_FLAG = OrderPayConsts.PAY_CARD,
                PAY_AMT = cardPay.APPR_AMT,
                LINE_NO = cardPay.SEQ_NO,
                EMP_NO = DataLocals.Employee.EMP_NO,
                INSERT_DT = DateTime.Now.ToString("yyyyMMddHHmmss"),
                UPDATE_DT = DateTime.Now.ToString("yyyyMMddHHmmss")
            });
        }

        #region PartCard - 제휴할인카드

        if ("OrderPayCoprtnDscntViewModel".Equals(viewModelName))
        {
            var partCardPay = (TRN_PARTCARD)payInfo.PayDatas[0];
            payPartCards.Add(partCardPay);
            paySeq++;
            payTenders.Add(new TRN_TENDERSEQ()
            {
                SHOP_CODE = DataLocals.PosStatus.SHOP_CODE,
                SALE_DATE = DataLocals.PosStatus.SALE_DATE,
                POS_NO = DataLocals.PosStatus.POS_NO,
                BILL_NO = DataLocals.PosStatus.BILL_NO,
                PAY_SEQ_NO = (Int16)paySeq,
                REGI_SEQ = DataLocals.PosStatus.REGI_SEQ,
                SALE_YN = "Y",
                PAY_TYPE_FLAG = OrderPayConsts.PAY_PARTCARD,
                PAY_AMT = partCardPay.APPR_AMT,
                LINE_NO = partCardPay.SEQ_NO,
                EMP_NO = DataLocals.Employee.EMP_NO,
                INSERT_DT = DateTime.Now.ToString("yyyyMMddHHmmss"),
                UPDATE_DT = DateTime.Now.ToString("yyyyMMddHHmmss")
            });
        }

        #endregion

        #endregion

        #region Voucher

        if ("OrderPayVoucherViewModel".Equals(viewModelName))
        {
            foreach (var gPay in payInfo.PayDatas)
            {
                if (gPay is TRN_CASHREC)
                {
                    var cashRecPay = (TRN_CASHREC)gPay;
                    payCashRecs.Add(cashRecPay);

                    paySeq++;
                    payTenders.Add(new TRN_TENDERSEQ()
                    {
                        SHOP_CODE = DataLocals.PosStatus.SHOP_CODE,
                        SALE_DATE = DataLocals.PosStatus.SALE_DATE,
                        POS_NO = DataLocals.PosStatus.POS_NO,
                        BILL_NO = DataLocals.PosStatus.BILL_NO,
                        PAY_SEQ_NO = (Int16)paySeq,
                        REGI_SEQ = DataLocals.PosStatus.REGI_SEQ,
                        SALE_YN = "Y",
                        PAY_TYPE_FLAG = OrderPayConsts.PAY_CASHREC,
                        PAY_AMT = cashRecPay.APPR_AMT,
                        LINE_NO = cashRecPay.SEQ_NO,
                        EMP_NO = DataLocals.Employee.EMP_NO,
                        INSERT_DT = DateTime.Now.ToString("yyyyMMddHHmmss"),
                        UPDATE_DT = DateTime.Now.ToString("yyyyMMddHHmmss")
                    });
                }
                else
                {
                    var trn = (TRN_GIFT)gPay;
                    payGifts.Add(trn);
                    paySeq++;
                    payTenders.Add(new TRN_TENDERSEQ()
                    {
                        SHOP_CODE = DataLocals.PosStatus.SHOP_CODE,
                        SALE_DATE = DataLocals.PosStatus.SALE_DATE,
                        POS_NO = DataLocals.PosStatus.POS_NO,
                        BILL_NO = DataLocals.PosStatus.BILL_NO,
                        PAY_SEQ_NO = (Int16)paySeq,
                        REGI_SEQ = DataLocals.PosStatus.REGI_SEQ,
                        SALE_YN = "Y",
                        PAY_TYPE_FLAG = OrderPayConsts.PAY_GIFT,
                        PAY_AMT = trn.TK_GFT_AMT,
                        LINE_NO = trn.SEQ_NO,
                        EMP_NO = DataLocals.Employee.EMP_NO,
                        INSERT_DT = DateTime.Now.ToString("yyyyMMddHHmmss"),
                        UPDATE_DT = DateTime.Now.ToString("yyyyMMddHHmmss")
                    });
                }
            }
        }

        #endregion

        #region Meal

        if ("OrderPayMealViewModel".Equals(viewModelName))
        {
            TRN_FOODCPN[] foodCpnPays = (TRN_FOODCPN[])payInfo.PayDatas;
            payFoodCpns.AddRange(foodCpnPays);

            foreach (var foodCpnPay in foodCpnPays)
            {
                paySeq++;
                payTenders.Add(new TRN_TENDERSEQ()
                {
                    SHOP_CODE = DataLocals.PosStatus.SHOP_CODE,
                    SALE_DATE = DataLocals.PosStatus.SALE_DATE,
                    POS_NO = DataLocals.PosStatus.POS_NO,
                    BILL_NO = DataLocals.PosStatus.BILL_NO,
                    PAY_SEQ_NO = (Int16)paySeq,
                    REGI_SEQ = DataLocals.PosStatus.REGI_SEQ,
                    SALE_YN = "Y",
                    PAY_TYPE_FLAG = OrderPayConsts.PAY_MEAL,
                    PAY_AMT = foodCpnPay.TK_FOD_AMT,
                    LINE_NO = foodCpnPay.SEQ_NO,
                    EMP_NO = DataLocals.Employee.EMP_NO,
                    INSERT_DT = DateTime.Now.ToString("yyyyMMddHHmmss"),
                    UPDATE_DT = DateTime.Now.ToString("yyyyMMddHHmmss")
                });
            }
        }

        #endregion

        #region 간편결제 - EasyPay

        if ("OrderPaySimplePayViewModel".Equals(viewModelName))
        {
            var easyPay = (TRN_EASYPAY)payInfo.PayDatas[0];
            payEasys.Add(easyPay);
            paySeq++;
            payTenders.Add(new TRN_TENDERSEQ()
            {
                SHOP_CODE = DataLocals.PosStatus.SHOP_CODE,
                SALE_DATE = DataLocals.PosStatus.SALE_DATE,
                POS_NO = DataLocals.PosStatus.POS_NO,
                BILL_NO = DataLocals.PosStatus.BILL_NO,
                PAY_SEQ_NO = (Int16)paySeq,
                REGI_SEQ = DataLocals.PosStatus.REGI_SEQ,
                SALE_YN = "Y",
                PAY_TYPE_FLAG = OrderPayConsts.PAY_EASY,
                PAY_AMT = easyPay.APPR_REQ_AMT,
                LINE_NO = easyPay.SEQ_NO,
                EMP_NO = DataLocals.Employee.EMP_NO,
                INSERT_DT = DateTime.Now.ToString("yyyyMMddHHmmss"),
                UPDATE_DT = DateTime.Now.ToString("yyyyMMddHHmmss")
            });
        }

        #endregion

        #region REDEEM POINTS

        if ("OrderPayMemberPointUseViewModel".Equals(viewModelName) || "OrderPayStampCouponViewModel".Equals(viewModelName))
        {
            object[] payDatas = (object[])payInfo.PayDatas;
            var redeemPoints = (TRN_POINTUSE)payDatas[0];
            payPoints.Add(redeemPoints);

            paySeq++;
            payTenders.Add(new TRN_TENDERSEQ()
            {
                SHOP_CODE = DataLocals.PosStatus.SHOP_CODE,
                SALE_DATE = DataLocals.PosStatus.SALE_DATE,
                POS_NO = DataLocals.PosStatus.POS_NO,
                BILL_NO = DataLocals.PosStatus.BILL_NO,
                PAY_SEQ_NO = (Int16)paySeq,
                REGI_SEQ = DataLocals.PosStatus.REGI_SEQ,
                SALE_YN = "Y",
                PAY_TYPE_FLAG = OrderPayConsts.PAY_POINTS,
                PAY_AMT = redeemPoints.USE_PNT,
                LINE_NO = redeemPoints.SEQ_NO,
                EMP_NO = DataLocals.Employee.EMP_NO,
                INSERT_DT = DateTime.Now.ToString("yyyyMMddHHmmss"),
                UPDATE_DT = DateTime.Now.ToString("yyyyMMddHHmmss")
            });
        }
        #endregion

        #region Redeem Prepayment - 선결제

        if ("OrderPayPrepaymentUseViewModel".Equals(viewModelName))
        {
            object[] payDatas = (object[])payInfo.PayDatas;
            var payPpCard = (TRN_PPCARD)payDatas[0];
            payPpCards.Add(payPpCard);

            paySeq++;
            payTenders.Add(new TRN_TENDERSEQ()
            {
                SHOP_CODE = DataLocals.PosStatus.SHOP_CODE,
                SALE_DATE = DataLocals.PosStatus.SALE_DATE,
                POS_NO = DataLocals.PosStatus.POS_NO,
                BILL_NO = DataLocals.PosStatus.BILL_NO,
                PAY_SEQ_NO = (Int16)paySeq,
                REGI_SEQ = DataLocals.PosStatus.REGI_SEQ,
                SALE_YN = "Y",
                PAY_TYPE_FLAG = OrderPayConsts.PAY_PPCARD,
                PAY_AMT = payPpCard.PPC_AMT,
                LINE_NO = payPpCard.SEQ_NO,
                EMP_NO = DataLocals.Employee.EMP_NO,
                INSERT_DT = DateTime.Now.ToString("yyyyMMddHHmmss"),
                UPDATE_DT = DateTime.Now.ToString("yyyyMMddHHmmss")
            });
        }

        #endregion

        UpdateTRSummary();
        return ProcessTRComplete();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="viewModelName"></param>
    /// <param name="trnName"></param>
    /// <param name="seqNo"></param>
    /// <returns></returns>
    public bool RemovePaymentTRN(COMPPAY_PAY_INFO payInfo)
    {
        var payTypeFlag = OrderPayConsts.PayTypeFlagByTRNClass(payInfo.PAY_CLASS_NAME, payInfo.PayDatas);
        foreach (var pay in payInfo.PayDatas)
        {
            if (pay != null)
            {
                var pi = pay.GetType().GetProperty("SEQ_NO", BindingFlags.Public | BindingFlags.Instance);
                string seqNo = (string)pi.GetValue(pay, null);

                var tender = payTenders.FirstOrDefault(p => p.PAY_TYPE_FLAG == payTypeFlag && p.LINE_NO == seqNo.ToString());
                payTenders.Remove(tender);

                switch (payInfo.PAY_CLASS_NAME)
                {
                    case nameof(TRN_CASH):
                        var cash = payCashs.FirstOrDefault(p => p.SEQ_NO == seqNo);
                        payCashs.Remove(cash);
                        break;
                    case nameof(TRN_CASHREC):
                        var cashRec = payCashRecs.FirstOrDefault(p => p.SEQ_NO == seqNo);
                        payCashRecs.Remove(cashRec);
                        break;
                    case nameof(TRN_CARD):
                        var card = payCards.FirstOrDefault(p => p.SEQ_NO == seqNo);
                        payCards.Remove(card);
                        break;
                    case nameof(TRN_EASYPAY):
                        var easyPay = payEasys.FirstOrDefault(p => p.SEQ_NO == seqNo);
                        payEasys.Remove(easyPay);
                        break;
                    case nameof(TRN_PARTCARD):
                        var partCard = payPartCards.FirstOrDefault(p => p.SEQ_NO == seqNo);
                        payPartCards.Remove(partCard);
                        break;
                    case nameof(TRN_GIFT):
                        var gift = payGifts.FirstOrDefault(p => p.SEQ_NO == seqNo);
                        payGifts.Remove(gift);
                        if (payInfo.PayDatas[payInfo.PayDatas.Length - 1] is TRN_CASHREC)
                        {
                            var payCashRec = payInfo.PayDatas[payInfo.PayDatas.Length - 1];
                            pi = payCashRec.GetType().GetProperty("SEQ_NO", BindingFlags.Public | BindingFlags.Instance);
                            seqNo = (string)pi.GetValue(payCashRec, null);

                            var cashRecVoucher = payCashRecs.FirstOrDefault(p => p.SEQ_NO == seqNo);
                            if (cashRecVoucher != null) payCashRecs.Remove(cashRecVoucher);
                        }
                        break;
                    case nameof(TRN_FOODCPN):
                        var foodCpn = payFoodCpns.FirstOrDefault(p => p.SEQ_NO == seqNo);
                        payFoodCpns.Remove(foodCpn);
                        break;
                    case nameof(TRN_POINTUSE):
                        var pointRedeem = payPoints.FirstOrDefault(p => p.SEQ_NO == seqNo);
                        payPoints.Remove(pointRedeem);
                        break;
                    case nameof(TRN_PPCARD):
                        var payPpCard = payPpCards.FirstOrDefault(p => p.SEQ_NO == seqNo);
                        payPpCards.Remove(payPpCard);
                        break;
                    default:
                        break;
                }
            }
        }

        // remove payment info list
        payInfos.Remove(payInfo);
        UpdateTRSummary();
        return ProcessTRComplete();
    }

    /// <summary>
    /// Add CASHREC TRN
    /// </summary>
    /// <param name="cashPay"></param>
    /// <param name="cashRecPay"></param>
    /// <param name="tenderPaySeq"></param>
    /// <param name="cashAmt"></param>
    private void AddPaymentCashTR(TRN_CASH cashPay, TRN_CASHREC cashRecPay, int tenderPaySeq, decimal cashAmt)
    {
        if (cashPay == null)
        {
            var cashPaySeq = payCashs.Count + 1;
            cashPay = new TRN_CASH()
            {
                SHOP_CODE = DataLocals.PosStatus.SHOP_CODE,
                POS_NO = DataLocals.PosStatus.POS_NO,
                SALE_DATE = DataLocals.PosStatus.SALE_DATE,
                BILL_NO = DataLocals.PosStatus.BILL_NO,
                SEQ_NO = cashPaySeq.ToString("d4"),
                REGI_SEQ = DataLocals.PosStatus.REGI_SEQ,
                CASH_AMT = 0,
                RET_AMT = 0,
                EX_CODE = string.Empty,
                EX_KRW = 0,
                EX_EXP_AMT = 0,
                EX_IN_AMT = 0,
                EX_RET_AMT = 0,
                KR_RET_AMT = 0,
                EX_PAY_AMT = 0,
                KR_PAY_AMT = 0,
                KR_ETC_AMT = 0,
                SALE_YN = "Y",
                INSERT_DT = DateTime.Now.ToString("yyyyMMddHHmmss")
            };

            var remainMoney = cashAmt > OrderSumInfo.EXP_PAY_AMT ? cashAmt - OrderSumInfo.EXP_PAY_AMT : 0;
            cashPay.BILL_NO = DataLocals.PosStatus.BILL_NO;
            cashPay.CASH_AMT = cashAmt;
            cashPay.RET_AMT = remainMoney;
        }

        payCashs.Add(cashPay);
        tenderPaySeq++;
        payTenders.Add(new TRN_TENDERSEQ()
        {
            SHOP_CODE = DataLocals.PosStatus.SHOP_CODE,
            SALE_DATE = DataLocals.PosStatus.SALE_DATE,
            POS_NO = DataLocals.PosStatus.POS_NO,
            BILL_NO = DataLocals.PosStatus.BILL_NO,
            PAY_SEQ_NO = (Int16)tenderPaySeq,
            REGI_SEQ = DataLocals.PosStatus.REGI_SEQ,
            SALE_YN = "Y",
            PAY_TYPE_FLAG = OrderPayConsts.PAY_CASH,
            PAY_AMT = cashPay.CASH_AMT - cashPay.RET_AMT,   //현금 - 거스름돈 버그수정 2023.11.13 ;
            LINE_NO = cashPay.SEQ_NO,
            EMP_NO = DataLocals.Employee.EMP_NO,
            INSERT_DT = DateTime.Now.ToString("yyyyMMddHHmmss"),
            UPDATE_DT = DateTime.Now.ToString("yyyyMMddHHmmss")
        });

        if (cashRecPay != null)
        {
            payCashRecs.Add(cashRecPay);

            tenderPaySeq++;
            payTenders.Add(new TRN_TENDERSEQ()
            {
                SHOP_CODE = DataLocals.PosStatus.SHOP_CODE,
                SALE_DATE = DataLocals.PosStatus.SALE_DATE,
                POS_NO = DataLocals.PosStatus.POS_NO,
                BILL_NO = DataLocals.PosStatus.BILL_NO,
                PAY_SEQ_NO = (Int16)tenderPaySeq,
                REGI_SEQ = DataLocals.PosStatus.REGI_SEQ,
                SALE_YN = "Y",
                PAY_TYPE_FLAG = OrderPayConsts.PAY_CASHREC,
                PAY_AMT = cashRecPay.APPR_AMT,
                LINE_NO = cashRecPay.SEQ_NO,
                EMP_NO = DataLocals.Employee.EMP_NO,
                INSERT_DT = DateTime.Now.ToString("yyyyMMddHHmmss"),
                UPDATE_DT = DateTime.Now.ToString("yyyyMMddHHmmss")
            });
        }
    }

    /// <summary>
    /// CHANGE_DUTY
    /// Calc amount of each based on appr amt
    /// using for CARD, CASHREC, EASYPAY
    /// </summary>
    /// <param name="payAmt"></param>
    /// <param name="nonTaxAmt"></param>
    /// <param name="inclTaxAmt"></param>
    /// <param name="taxAmt"></param>
    /// <param name="rPayAmt"></param>
    /// <exception cref="NotImplementedException"></exception>
    public void CalcAmtsWithDuty(decimal payAmt, out decimal nonTaxAmt, out decimal wTaxSaleAmtExTax, out decimal taxAmt, out decimal rPayAmt)
    {
        nonTaxAmt = wTaxSaleAmtExTax = taxAmt = 0;
        rPayAmt = payAmt;

        decimal nonTaxProdAmts = OrderItems.Where(p => p.TAX_YN == "N").Sum(p => p.NORMAL_UPRC * p.SALE_QTY - p.DC_AMT);
        decimal taxInclProdAmts = OrderSumInfo.DCM_SALE_AMT - nonTaxProdAmts;

        if (OrderItems.Count <= 0 || OrderSumInfo.DCM_SALE_AMT <= 0 || (nonTaxProdAmts + taxInclProdAmts == 0))
        {
            return;
        }

        ///                     
        /// discUtSprc += (double)Math.Round(sPrc * EventDc.DcRate / 100, MidpointRounding.AwayFromZero);
        /// 
        decimal nonP = nonTaxProdAmts / (nonTaxProdAmts + taxInclProdAmts);
        decimal taxP = 1 - nonP;

        rPayAmt = decimal.Truncate(payAmt);
        nonTaxAmt = (int)TypeHelper.RoundNear10(payAmt * nonP);
        decimal wTaxSaleAmt = rPayAmt - nonTaxAmt;
        taxAmt = decimal.Truncate(wTaxSaleAmt.AmtTax());
        wTaxSaleAmtExTax = decimal.Truncate(wTaxSaleAmt.AmtNoTax());

        Debug.WriteLine("총금액: {0}, 면세품액 {1}, 과제품액 {2}, \r\n결제금액: {3}, 면세금액: {4}, 과세금액 {5}, 세금 {6}",
            orderSumInfo.DCM_SALE_AMT, nonTaxProdAmts, taxInclProdAmts,
            rPayAmt, nonTaxAmt, wTaxSaleAmtExTax, taxAmt);
    }

    /// <summary>
    /// Update order summary
    /// </summary>
    private void UpdateTRSummary()
    {
        #region Update TR Products

        trProducts.Clear();
        for (int i = 0; i < OrderItems.Count; i++)
        {
            var p = OrderItems[i];
            trProducts.Add(new TRN_PRDT()
            {
                SHOP_CODE = DataLocals.PosStatus.SHOP_CODE,
                SALE_DATE = DataLocals.PosStatus.SALE_DATE,
                POS_NO = DataLocals.PosStatus.POS_NO,
                BILL_NO = DataLocals.PosStatus.BILL_NO,
                SEQ_NO = p.NO.ToString("d4"),
                REGI_SEQ = DataLocals.PosStatus.REGI_SEQ,
                SALE_YN = "Y",
                PRD_CODE = p.PRD_CODE,
                PRD_NAME = p.PRD_NAME, //2023.11.17 상품명추가
                PRD_TYPE_FLAG = p.PRD_TYPE_FLAG,
                CORNER_CODE = string.Empty,
                SALE_QTY = Convert.ToInt32(p.SALE_QTY),
                SALE_UPRC = p.NORMAL_UPRC,
                SALE_AMT = p.SALE_AMT,
                DC_AMT = p.DC_AMT,
                VAT_AMT = p.TAX_YN == "N" ? 0 : p.SALE_AMT.AmtTax(),
                DCM_SALE_AMT = p.DCM_SALE_AMT,
                CHG_BILL_NO = string.Empty,
                TAX_YN = p.TAX_YN,
                DLV_PACK_FLAG = p.DLV_PACK_FLAG,
                SDS_CLASS_CODE = p.CLASS_CODE,
                SDS_PARENT_CODE = p.UP_PRD_CODE,
                SIDE_MENU_YN = p.SIDE_MENU_YN,
                INSERT_DT = DateTime.Now.ToString("yyyyMMddHHmmss"),

                DC_AMT_CST = p.DC_AMT_CST,
                DC_AMT_CPN = p.DC_AMT_CPN,
                DC_AMT_SVC = p.DC_AMT_SVC,
                DC_AMT_CRD = p.DC_AMT_CRD,
                DC_AMT_FOD = p.DC_AMT_FOD,
                DC_AMT_GEN = p.DC_AMT_GEN,
                DC_AMT_JCD = p.DC_AMT_JCD,
                DC_AMT_PACK = p.DC_AMT_PACK,
                DC_AMT_PRM = p.DC_AMT_PRM
            });
        }

        #endregion

        OrderSumInfo.TOT_SALE_AMT = OrderItems.Sum(p => p.SALE_AMT);
        OrderSumInfo.TOT_DC_AMT = OrderItems.Sum(p => p.DC_AMT);
        OrderSumInfo.TOT_QTY = OrderItems.Sum(p => p.SALE_QTY);

        decimal paidAmt = 0;

        // CASH
        paidAmt = payCashs.Sum(p => p.CASH_AMT - p.RET_AMT);

        // CARD
        paidAmt += payCards.Sum(p => p.APPR_AMT);

        // VOUCHER
        paidAmt += payGifts.Sum(p => p.TK_GFT_AMT);

        // MEAL
        paidAmt += payFoodCpns.Sum(p => p.TK_FOD_AMT);


        // DISCOUNT CARD
        paidAmt += payPartCards.Sum(p => p.APPR_AMT);

        // easy pay
        paidAmt += payEasys.Sum(p => p.APPR_REQ_AMT);

        // point redemption
        paidAmt += payPoints.Sum(p => p.USE_PNT);

        // prepaid
        paidAmt += payPpCards.Sum(p => p.PPC_AMT);
        // check all payments and deduce
        // REPAY_CASH_AMT return back if voucher uamt all > need to pay amt
        // 현금 거스름
        OrderSumInfo.REPAY_CASH_AMT = payFoodCpns.Sum(p => p.REPAY_CASH_AMT) + payGifts.Where(p => p.TK_GFT_SALE_FLAG == "0").Sum(p => p.REPAY_CSH_AMT) + payCashs.Sum(p => p.RET_AMT);
        // 받은 금액
        OrderSumInfo.GST_PAY_AMT = paidAmt;// + OrderSumInfo.REPAY_CASH_AMT;

        // 받을금액
        OrderSumInfo.ETC_AMT = payFoodCpns.Sum(p => p.ETC_AMT);

        #region Update Header for sum amts

        /// 
        /// TO DO
        /// 1. SAVE TR
        /// 2. SAVE PAYMENT
        /// 
        trHeader.TOT_SALE_AMT = OrderSumInfo.TOT_SALE_AMT;
        trHeader.TOT_DC_AMT = OrderSumInfo.TOT_DC_AMT;
        // 받을금액
        trHeader.EXP_PAY_AMT = trHeader.TOT_SALE_AMT - trHeader.TOT_DC_AMT;

        #region Discount - 할인합계

        //trHeader.DC_CPN_AMT = 
        trHeader.DC_CST_AMT = trProducts.Sum(p => p.DC_AMT_CST);
        trHeader.DC_CPN_AMT = trProducts.Sum(p => p.DC_AMT_CPN);
        trHeader.DC_SVC_AMT = trProducts.Sum(p => p.DC_AMT_SVC);
        trHeader.DC_CRD_AMT = trProducts.Sum(p => p.DC_AMT_CRD);
        trHeader.DC_GEN_AMT = trProducts.Sum(p => p.DC_AMT_GEN);
        //trHeader.DC_PCD_AMT = trProducts.Sum(p => p.DC_AMT_JCD);
        //trHeader.DC_TFD_AMT = trProducts.Sum(p => p.DC_AMT_FOD);
        trHeader.DC_PACK_AMT = trProducts.Sum(p => p.DC_AMT_PACK);
        trHeader.DC_PRM_AMT = trProducts.Sum(p => p.DC_AMT_PRM);

        trHeader.DC_PCD_AMT = payPartCards.Sum(p => p.JCD_DC_AMT);
        trHeader.DC_TFD_AMT = payFoodCpns.Sum(p => p.TK_FOD_DC_AMT);
        #endregion

        // CHANGE_DUTY ==>
        // 실매출액
        decimal nonTaxAmt = 0;
        decimal incTaxAmtExcTax = 0;
        decimal taxAmt = 0;
        decimal rPayAmt = 0;
        CalcAmtsWithDuty(OrderSumInfo.DCM_SALE_AMT, out nonTaxAmt, out incTaxAmtExcTax, out taxAmt, out rPayAmt);
        trHeader.DCM_SALE_AMT = rPayAmt;// OrderSumInfo.DCM_SALE_AMT;

        trHeader.VAT_AMT = taxAmt;
        trHeader.VAT_SALE_AMT = incTaxAmtExcTax;

        trHeader.NO_VAT_SALE_AMT = nonTaxAmt;
        trHeader.NO_TAX_SALE_AMT = incTaxAmtExcTax + nonTaxAmt;

        /// CHANGE_DUTY <==
        /// 
        trHeader.GST_PAY_AMT = OrderSumInfo.GST_PAY_AMT;

        //trHeader.RET_PAY_AMT = orderSumInfo.RET_PAY_AMT;
        trHeader.RET_PAY_AMT = OrderSumInfo.REPAY_CASH_AMT;

        trHeader.TOT_ETC_AMT = OrderSumInfo.ETC_AMT;

        trHeader.CASH_BILL_AMT = payCashRecs.Sum(p => p.APPR_AMT);
        trHeader.DLV_ORDER_FLAG = "0";
        trHeader.FD_GST_FLAG_YN = "0";

        #endregion

        if (!OrderCompleted)
        {
            IoC.Get<PaymentInfoViewModel>().UpdateOrderValue(OrderSumInfo);
            IoC.Get<PaymentDetailsViewModel>().UpdateTranTenderSeq(payInfos);
            IoC.Get<DualScreenOrderingViewModel>().OrderSumInfo = OrderSumInfo;
            IoC.Get<OrderPayPrepaymentUseViewModel>().PaidAmt = OrderSumInfo.DCM_SALE_AMT;
            IoC.Get<OrderPayMemberPointUseViewModel>().PaidAmt = OrderSumInfo.DCM_SALE_AMT;
        }

        IoC.Get<InputBoxKeyPadView>().ClearText();
    }


    private void UpdateRetainedInfo()
    {
        switch (this.RetainLastOrder)
        {
            case RetainLastOrderTypes.None:
                // 결제정보
                IoC.Get<PaymentInfoViewModel>().UpdateOrderValue(OrderSumInfo);
                IoC.Get<PaymentDetailsViewModel>().UpdateTranTenderSeq(payInfos);

                // 듀얼모니터
                IoC.Get<DualScreenOrderingViewModel>().OrderSumInfo = OrderSumInfo;
                IoC.Get<DualScreenMainViewModel>().SwitchMode(EDislayType.Logo);
                break;
            case RetainLastOrderTypes.RetainInMain:
                // 듀얼모니터
                IoC.Get<DualScreenOrderingViewModel>().OrderSumInfo = OrderSumInfo;
                IoC.Get<DualScreenMainViewModel>().SwitchMode(EDislayType.Logo);
                break;
            case RetainLastOrderTypes.RetainInDual:
                // 결제정보
                OrderItems.Clear();
                IoC.Get<PaymentInfoViewModel>().UpdateOrderValue(new ORDER_SUM_INFO());
                IoC.Get<PaymentDetailsViewModel>().UpdateTranTenderSeq(new List<COMPPAY_PAY_INFO>());

                // 회원정보
                break;
            case RetainLastOrderTypes.RetainBoth:
                break;
            default:
                break;
        }
    }

    private bool ProcessTRComplete()
    {
        if (OrderSumInfo.EXP_PAY_AMT > 0)
        {
            return false;
        }
        OrderCompleted = true;
        if(!PostPay)
        {
            orderPayWaitingService.LoadTableOrders(this.TableCd, false);
        }
        
        ProcessTRCompleteSave();

        return true;
    }

    private async void ProcessTRCompleteSave()
    {
        StatusMessageNoReset = "처리중입니다. 잠시만 기다려주세요.";

        if (memberInfo != null)
        {
            #region 포인트 적립

            string newBillNo = DataLocals.PosStatus.BILL_NO.StrIntInc(4); //trHeader.BILL_NO.StrIntInc(4);
            var pointSaveRes = await orderPayPointStampService.RequestSavePoint(newBillNo, memberInfo, trHeader, payTenders.ToArray(), trProducts.ToArray());
            decimal accAmt = 0;
            var x = DataLocals.PosStatus.BILL_NO;
            object[] retDatas = (object[])pointSaveRes.Item2;
            accAmt = (decimal)retDatas[0];
            var excAmt = (decimal)retDatas[0];
            string errorMessage = (string)retDatas[2];

            if (string.IsNullOrEmpty(errorMessage))
            {
                string useType = DataLocals.AppConfig.PosOption.PointStampFlag == "0" ? "01" :
                            (DataLocals.AppConfig.PosOption.StampUseMethod == "0" ? "02" : "03");
                var trPointSave = new TRN_POINTSAVE()
                {
                    SHOP_CODE = trHeader.SHOP_CODE,
                    SALE_DATE = trHeader.SALE_DATE,
                    POS_NO = trHeader.POS_NO,
                    BILL_NO = trHeader.BILL_NO,
                    SEQ_NO = "0001",
                    REGI_SEQ = trHeader.REGI_SEQ,
                    SALE_YN = "Y",
                    CST_NO = memberInfo.mbrCode,
                    CARD_NO = memberInfo.mbrCardno,
                    LEVEL = memberInfo.mbrGrdCode,
                    TOT_SALE_AMT = trHeader.TOT_SALE_AMT,
                    TOT_DC_AMT = trHeader.TOT_DC_AMT,
                    SAVE_AMT = accAmt,
                    NO_SAVE_AMT = trHeader.TOT_SALE_AMT - trHeader.TOT_DC_AMT - accAmt,
                    USE_TYPE = useType,
                    TOT_PNT = pointSaveRes.Item1 != null ? pointSaveRes.Item1.totalPoint : 0,
                    TOT_USE_PNT = pointSaveRes.Item1 != null ? pointSaveRes.Item1.totalUsePoint : 0,
                    LAST_PNT = pointSaveRes.Item1 != null ? pointSaveRes.Item1.avalidPoint : 0,
                    PRE_PNT = pointSaveRes.Item1 != null ? pointSaveRes.Item1.prevAvalidPoint : 0,
                    SAVE_PNT = pointSaveRes.Item1 != null ? pointSaveRes.Item1.savePoint : 0,
                    INSERT_DT = pointSaveRes.Item1 != null ? pointSaveRes.Item1.createdAt : DateTime.Now.ToString("yyyyMmddHHmmss")
                };

                savePoints.Add(trPointSave);
            }
            else
            {
                DialogHelper.MessageBox(errorMessage, GMessageBoxButton.OK, MessageBoxImage.Error);
            }

            #endregion
        }

        #region Process saving TR

        var result = await orderPayService.SaveOrderPayTR(false, trHeader, trProducts.ToArray(),
                payTenders.ToArray(), payCashs.ToArray(), payCashRecs.ToArray(), payCards.ToArray(),
                payPartCards.ToArray(), payGifts.ToArray(), payFoodCpns.ToArray(), payEasys.ToArray(),
                payPoints.ToArray(), savePoints.FirstOrDefault(), memberInfo, payPpCards.ToArray(), TableCd);

        if (result.Item1.ResultType != EResultType.SUCCESS)
        {
            DialogHelper.MessageBox(result.Item1.ResultMessage, GMessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }

        // 푸드교환권 출력 → 고객 주문서 출력 → 주방 주문서 출력 → 일반영수증 출력

        // 영수증 출력 여부 확인
        bool printReceipt = true;
        if (DataLocals.AppConfig.PosOption.PrintReceiptType == "0" &&
            DataLocals.AppConfig.PosOption.ReceiptPrintAsk == "1")
        {
            var askPrint = DialogHelper.MessageBox(DataLocals.AppConfig.PosOption.ReceiptPrintAskMsg, GMessageBoxButton.OKCancel, MessageBoxImage.Question, new string[]
            {
                "예", "아니요"
            });
            printReceipt = askPrint == MessageBoxResult.OK;
        }

        ///
        /// 푸드교환권출력
        /// 
        pOSPrintService.PrintFoodCoupon(DataLocals.AppConfig.PosOption.FoodCouponPrintYN == "1" ? PrintOptions.Normal : PrintOptions.JournalOnly,
            trHeader, orderSumInfo, trProducts.ToArray());

        ///
        /// 고객주문서
        /// 
        pOSPrintService.PrintCustomerOrder(PrintCustOrder && DataLocals.AppConfig.PosOption.CustOrderPrintYN == "1" ?
               PrintOptions.Normal : PrintOptions.JournalOnly, trHeader, orderSumInfo, PrintItem,
               trProducts.ToArray(), payTenders.ToArray(), payCashs.ToArray(), payCashRecs.ToArray(),
               payCards.ToArray(), payPartCards.ToArray(), payGifts.ToArray(), payFoodCpns.ToArray(), payEasys.ToArray(),
               payPoints.ToArray(), savePoints.FirstOrDefault(), payPpCards.ToArray());

        /// 
        /// 일반영수증
        /// 
        pOSPrintService.PrintReceipt(PrintBill && printReceipt ? PrintOptions.Normal : PrintOptions.JournalOnly, trHeader, orderSumInfo, PrintItem,
            trProducts.ToArray(), payTenders.ToArray(), payCashs.ToArray(), payCashRecs.ToArray(),
            payCards.ToArray(), payPartCards.ToArray(), payGifts.ToArray(), payFoodCpns.ToArray(), payEasys.ToArray(),
            payPoints.ToArray(), savePoints.FirstOrDefault(), payPpCards.ToArray(), memberInfo);

        // EndPrinting
        pOSPrintService.EndPrinting();

        // Print KDS
        // 주방 주문서 출력
        if (result.Item2 != null) DataLocals.RaiseKDSEvent(result.Item2,
                PrintKitchen && DataLocals.AppConfig.PosOption.KitOrderPrintYN == "1");

        StatusMessageNoReset = string.Empty;
        if (this.RetainLastOrder == RetainLastOrderTypes.None)
        {
            Initialize_Trans();
            UpdateTRSummary();
        }
        IoC.Get<OrderPayLeftTRInfoViewModel>().ActiveItem = IoC.Get<PaymentInfoViewModel>();
        UpdateRetainedInfo();
        #endregion
    }

    private DispatcherTimer trCompleteTimer = null;
    private TimeSpan trCompElapseTime;
    private SynchronizationContext _syncContext;


    /// <summary>
    /// 보류 / 대기처리
    /// 1. Check if having now in sales mode    
    ///     - Save to ORD 
    /// 2. Load Hold List, Waiting
    ///
    /// </summary>
    private void ProcessHoldTR(bool onClose)
    {
        bool saveORD = onClose;
        // if click ExtMenu, Hold button
        if (!onClose)
        {
            // save TR to ORD tables
            saveORD = _OrderItemCount > 0;
        }

        // save TR to ORD tables
        if (saveORD)
        {
            var res = DialogHelper.MessageBox("해당 판매등록중인 내역을 대기 처리하시겠습니까?", GMessageBoxButton.OKCancel,
                    MessageBoxImage.Question);
            if (res == MessageBoxResult.Cancel) return;

            var result = orderPayService.SaveOrderPayTR(true, trHeader, trProducts.ToArray(),
                payTenders.ToArray(), payCashs.ToArray(), payCashRecs.ToArray(), payCards.ToArray(),
                payPartCards.ToArray(), payGifts.ToArray(), payFoodCpns.ToArray(), payEasys.ToArray(),
                payPoints.ToArray(), null, MemberInfo, payPpCards.ToArray(), TableCd).Result;
            if (result.Item1.ResultType != EResultType.SUCCESS)
            {
                DialogHelper.MessageBox(result.Item1.ResultMessage, GMessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (!string.IsNullOrEmpty(TableCd))
            {
                _eventAggregator.PublishOnUIThreadAsync(new OrderReceived()
                {
                    tableCode = TableCd,
                    OrderItems = trProducts
                });
            }
            /// 
            /// reset to new TR
            /// 
            Initialize_Trans();
            ItemGrid_Refresh();
            UpdateTRSummary();
        }
    }

    /// <summary>
    /// 
    /// </summary>
    async void LoadLastHoldTR()
    {
        var hasWaitOrders = await orderPayWaitingService.HasWaitingTR();
        if (hasWaitOrders)
        {
            var res = DialogHelper.MessageBox("햔재 일부 결제된 대기 내역이 있습니다.\r\n대기 내역을 확인 하시겠습니까?",
                        GMessageBoxButton.OKCancel, MessageBoxImage.Question);
            if (res == MessageBoxResult.Cancel)
            {
                return;
            }
            ActiveForm("ActiveItemR", typeof(OrderPayWaitingViewModel));
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="waitOrds"></param>
    public void LoadExistOrderTRs(Dictionary<string, object> waitOrds)
    {
        Initialize_Trans();

        // Load
        foreach (var key in waitOrds.Keys)
        {
            switch (key)
            {
                case "TRN_HEADER":
                    trHeader = trHeader.CopyFieldsFrom<TRN_HEADER>(waitOrds[key], null);
                    trHeader.DLV_ORDER_FLAG = "0";
                    break;

                case "TRN_PRDT":
                    trProducts = new List<TRN_PRDT>();
                    OrderItems.Clear();
                    foreach (var oPrd in (IEnumerable<ORD_PRDT>)waitOrds[key])
                    {
                        var trPrd = oPrd.CopyFields<TRN_PRDT>();
                        trProducts.Add(trPrd);
                        OrderItems.Add(ORDER_GRID_ITEM.FromTRNProduct(trPrd, OrderItems.Count));
                    }
                    break;

                case "TRN_TENDERSEQ":
                    payTenders = new List<TRN_TENDERSEQ>();
                    foreach (var oTender in (IEnumerable<ORD_TENDERSEQ>)waitOrds[key])
                    {
                        var trTender = oTender.CopyFields<TRN_TENDERSEQ>();
                        payTenders.Add(trTender);
                    }
                    break;

                case "TRN_CASH":
                    payCashs = new List<TRN_CASH>();
                    foreach (var oCash in (IEnumerable<ORD_CASH>)waitOrds[key])
                    {
                        var trCash = oCash.CopyFields<TRN_CASH>();
                        payCashs.Add(trCash);
                    }
                    break;

                case "TRN_CARD":
                    payCards = new List<TRN_CARD>();
                    foreach (var oCard in (IEnumerable<ORD_CARD>)waitOrds[key])
                    {
                        var trCard = oCard.CopyFields<TRN_CARD>();
                        payCards.Add(trCard);
                    }
                    break;

                case "TRN_GIFT":
                    //trHeader.TK_GFT_AMT = 
                    payGifts = new List<TRN_GIFT>();
                    foreach (var oGift in (IEnumerable<ORD_GIFT>)waitOrds[key])
                    {
                        var trGift = oGift.CopyFields<TRN_GIFT>();
                        payGifts.Add(trGift);
                    }
                    break;

                case "TRN_FOODCPN":
                    payFoodCpns = new List<TRN_FOODCPN>();
                    foreach (var oFood in (IEnumerable<ORD_FOODCPN>)waitOrds[key])
                    {
                        var trFood = oFood.CopyFields<TRN_FOODCPN>();
                        payFoodCpns.Add(trFood);
                    }
                    break;

                case "TRN_EASYPAY":
                    payEasys = new List<TRN_EASYPAY>();
                    foreach (var oEasy in (IEnumerable<ORD_EASYPAY>)waitOrds[key])
                    {
                        var trEasy = oEasy.CopyFields<TRN_EASYPAY>();
                        payEasys.Add(trEasy);
                    }
                    break;

                case "TRN_POINTUSE":
                    payPoints = new List<TRN_POINTUSE>();
                    foreach (var orPoints in (IEnumerable<ORD_POINTUSE>)waitOrds[key])
                    {
                        var trPts = orPoints.CopyFields<TRN_POINTUSE>();
                        payPoints.Add(trPts);
                    }
                    break;

                case "TRN_PPCARD":
                    payPpCards = new List<TRN_PPCARD>();
                    foreach (var orPoints in (IEnumerable<ORD_PPCARD>)waitOrds[key])
                    {
                        var trPrepaid = orPoints.CopyFields<TRN_PPCARD>();
                        payPpCards.Add(trPrepaid);
                    }
                    break;
                default:
                    break;
            }
        }


        // Readd payInfos
        payInfos = payTenders.ToPayInfos(this.payCards.ToArray(), this.payCashs.ToArray(), this.payCashRecs.ToArray(),
            this.payGifts.ToArray(), this.payFoodCpns.ToArray(), this.payPartCards.ToArray(), this.payEasys.ToArray(),
            this.payPoints.ToArray(), null, payPpCards.ToArray());

        ItemGrid_Refresh();
        UpdateTRSummary();

    }

    /// <summary>
    /// 
    /// </summary>
    private bool ResetLastTR()
    {
        if (!OrderCompleted || this.RetainLastOrder == RetainLastOrderTypes.None)
        {
            return true;
        }

        if (OrderCompleted && this.RetainLastOrder.ToString().StartsWith("Retain"))
        {
            Initialize_Trans();
        }
        UpdateTRSummary();
        UpdateRetainedInfo();

        return false;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="payViewModel"></param>
    /// <returns></returns>
    public int GetTRPaySeq(string payViewModel)
    {
        int paySeq = 0;
        switch (payViewModel)
        {
            case "OrderPayCashViewModel":
                paySeq = payCashs.Count;
                break;
            case "OrderPayCashRecViewModel":
                paySeq = payCashRecs.Count;
                break;
            case "OrderPayCardViewModel":
                paySeq = payCards.Count;
                break;
            case "OrderPayVoucherViewModel":
                paySeq = payGifts.Count;
                break;
            case "OrderPayMealViewModel":
                paySeq = payFoodCpns.Count;
                break;
            case "OrderPayMemberPointUseViewModel":
                paySeq = payPoints.Count;
                break;
            case "OrderPaySimplePayViewModel":
                paySeq = payEasys.Count;
                break;
            case "OrderPayPrepaymentUseViewModel":
                paySeq = payPpCards.Count;
                break;
            default:
                break;
        }
        return paySeq;
    }

    #endregion

    #region DualScreen

    bool _DualTimer = false;
    DispatcherTimer _dualTimer = null;
    TimeSpan _dualTime;

    void StartDualTimer()
    {
        if (_DualTimer)
        {
            _dualTimer.Stop();
        }

        _DualTimer = true;
        _dualTime = TimeSpan.FromSeconds(0);
        _dualTimer = new DispatcherTimer(new TimeSpan(0, 0, 1), DispatcherPriority.Normal, delegate
        {
            //tbTime.Text = _time.ToString("c");
            if (_dualTime == TimeSpan.Zero)
            {
                _dualTimer.Stop();
                ResetLastTR();
                if (this.OrderItems.Count == 0)
                {
                    IoC.Get<DualScreenMainViewModel>().SwitchMode(EDislayType.Logo);
                }

                _DualTimer = false;
                return;
            }

            _dualTime = _dualTime.Add(TimeSpan.FromSeconds(-1));
        }, Application.Current.Dispatcher);

        _dualTimer.Start();
    }

    #endregion

    #region Member discount

    public void MemberDiscount(MEMBER_CLASH member, bool discountYN)
    {
        if (member == null) { return; }
        if (member.dscRt <= 0 || member.dscLmtWage <= 0) { return; }
        List<ORDER_GRID_ITEM> applyItems = new List<ORDER_GRID_ITEM>();
        decimal discountedAmt = 0;

        if (!discountYN) // undo member discount (usually happen when memberinfo is removed or reset ... etc)
        {
            foreach (var item in OrderItems)
            {
                if (!item.DiscMbr)
                {
                    continue;
                }

                item.DC_AMT_CST = 0;
                item.DiscMbr = false;
                item.DiscMbrRate = 0;
                item.UpdateRemarkItem("DCM", string.Empty, false);
            }
            // UpdateTRSummary();
        }
        else // apply for member exclusive discount
        {
            for (int i = 0; i < OrderItems.Count; i++)
            {
                var item = OrderItems[i];
                if ("N".Equals(item.CST_ACCDC_YN))
                    continue;

                var amtToApply = item.DCM_SALE_AMT - item.DC_AMT_CST;
                if (amtToApply <= 0)
                {
                    continue;
                }

                var aDcValue = member.dscRt;
                var aDcAmt = aDcValue * amtToApply / 100;
                applyItems.Add(item);
                discountedAmt += aDcAmt;
            }

            if (discountedAmt > member.dscLmtWage)
            {
                decimal appliedAmt = 0;
                discountedAmt = member.dscLmtWage;
                var totalExcDcCst = OrderSumInfo.DCM_SALE_AMT + OrderItems.Sum(p => p.DC_AMT_CST);
                // chia deu theo ty le gia tri cua tung item
                //foreach (var item in applyItems)
                for (int i = 0; i < applyItems.Count - 1; i++)
                {
                    //var amtToApply = item.DCM_SALE_AMT;
                    //item.DiscMbrRate = ((amtToApply * member.dscLmtWage) / discountedAmt) * (100 / amtToApply);
                    //item.DC_AMT_CST = item.DiscMbrRate * amtToApply / 100;
                    //item.DiscMbr = true;
                    //item.UpdateRemarkItem("DCM", "회원DC", true);



                    // percent of each item amount / total amount * discount all amount
                    var item = applyItems[i];
                    var amtToApply = item.DCM_SALE_AMT - item.DC_AMT_CST;
                    decimal percentOfItem = amtToApply / totalExcDcCst;

                    decimal amountToDC = (int)TypeHelper.RoundNear(percentOfItem * discountedAmt, 1);
                    appliedAmt += amountToDC;

                    item.DiscMbrRate = member.dscRt;
                    item.DC_AMT_CST = amountToDC;
                    item.UpdateRemarkItem("DCM", string.Format("회원DC{0}%", member.dscRt), true);
                }

                // last item, take remains
                #region Last item in applied List, take remain amount
                decimal lastAmtToDC = discountedAmt - appliedAmt;

                applyItems[applyItems.Count - 1].DiscMbrRate = member.dscRt;
                applyItems[applyItems.Count - 1].DC_AMT_CST += lastAmtToDC;
                applyItems[applyItems.Count - 1].UpdateRemarkItem("DCM", "회원DC", true);

                #endregion

            }
            else
            {
                foreach (var item in applyItems)
                {
                    var amtToApply = item.DCM_SALE_AMT + item.DC_AMT_CST;
                    item.DiscMbrRate = member.dscRt;
                    item.DC_AMT_CST = item.DiscMbrRate * amtToApply / 100;
                    item.DiscMbr = true;
                    item.UpdateRemarkItem("DCM", string.Format("회원DC{0}%", member.dscRt), true);
                }
                //          return;
            }
        }
        UpdateTRSummary();
    }
    public Task HandleAsync(KeyboardEventData message, CancellationToken cancellationToken)
    {
        if (message.PressedKey == Key.Enter)
        {
            if (addingPriceMgr != PriceMgrProdAddingStatus.NoneAdd)
            {
                var inputAmt = Convert.ToInt32(IoC.Get<OrderPayLeftInfoKeypadViewModel>().Text);
                ItemGrid_AddItem(false, null, inputAmt);
            }
        }
        else if (message.PressedKey == Key.Clear)
        {
            ItemGrid_AddItem(true, null, 0);
        }
        return Task.CompletedTask;
    }

    public async Task LoadTableOrderAsync(string tbCode)
    {
        var model = IoC.Get<DualScreenMainViewModel>();

        var name = model.ActiveItem.DisplayName;
        if (!name.Contains("DualScreenOrderingViewModel"))
        {
            model.ActiveItem = IoC.Get<DualScreenOrderingViewModel>();
        }

        var waitdata = await orderPayWaitingService.LoadTableOrders(tbCode,true);
        var trDatas = await orderPayWaitingService.LoadWaitTRData(waitdata);
        if (trDatas != null) { LoadExistOrderTRs(trDatas); }
    }

    public Task HandleAsync(TableOrder message, CancellationToken cancellationToken)
    {
        var z = message.tableCode;
        this.TableCd = z;
        LoadTableOrderAsync(z);
        return Task.CompletedTask;
    }
    #endregion
}

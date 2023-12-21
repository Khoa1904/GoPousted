using Caliburn.Micro;
using GoPOS.Common.Helpers;
using GoPOS.Common.Interface.Model;
using GoPOS.Common.Interface.View;
using GoPOS.Common.ViewModels;
using GoPOS.Helpers.CommandHelper;
using GoPOS.Models;
using GoPOS.OrderPay.Interface.View;
using GoPOS.OrderPay.ViewModels.Controls;
using GoPOS.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using GoPOS.Helpers;
using GoPOS.Models.Common;
using System.Diagnostics;
using System.Windows;
using Microsoft.EntityFrameworkCore;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;
using Microsoft.IdentityModel.Tokens;
using GoShared.Helpers;
using System.Printing;
using GoPOS.Service;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using GoPOS.Views;

namespace GoPOS.ViewModels
{
    public partial class PostPayTableManagermentViewModel : BaseItemViewModel, IHandle<OrderReceived>
    {
        private bool _menuNote { get; set; } = false;
        private readonly IOrderPayMainViewModel _mainviewmodel;
        private readonly IOrderPayService orderPayService;
        private readonly IPostPayTableManagementService tableService;
        private IPostPayTableManagementView _view = null;
        public List<MST_TABLE_INFO> TableMaster { get; set; }
        public List<TABLE_THR> Tablethr { get; set; }
        public List<ORDER_FUNC_KEY>? RightFuncKeys { get; set; }
        public PostPayTableManagermentViewModel(IWindowManager windowManager, IEventAggregator eventAggregator, IOrderPayService orderPayService,
            IPostPayTableManagementService tableService)
            : base(windowManager, eventAggregator)
        {
            this.tableService = tableService;
            this.orderPayService = orderPayService;
            ViewInitialized += this.PostPayTableManagermentViewModel_ViteInit;
            ViewLoaded += this.PostPayTableManagermentViewModel_Viewload;
        }

        public bool MenuNote
        {
            get => _menuNote;
            set
            {
                _menuNote = value;
                NotifyOfPropertyChange(() => MenuNote);
            }
        }
        private void PostPayTableManagermentViewModel_ViteInit(Object sender, EventArgs e)
        {
            RightFuncKeys = new();
            TableMaster = new();
            GenerateFuncKey("10");
            TableMaster = tableService.GetTableInfo().Result.Item1.Where(z => z.SHAPE_FLAG == "1").ToList();
            InitTablethr();
        }
        private void PostPayTableManagermentViewModel_Viewload(Object sender, EventArgs e)
        {

            GenerateFuncKey("10");
            CurrentPage = 1;

            _view.RenderTables(Tablethr);
            _view.ShowOrderDetails(false);
        }
        public ICommand ButtonCommand => new RelayCommand<Button>(ButtonCommandCenter);
        private void ButtonCommandCenter(Button button)
        {
            if (button.Tag is null) { return; }
            switch (button.Tag)
            {
                case "Prev":
                    CurrentPage--;
                    break;
                case "Next":
                    CurrentPage++;
                    break;
                case "ButtonClose":
                    this.TryCloseAsync();
                    break;
                case "Basic":
                    GenerateFuncKey("10");
                    break;
                case "Deliver":
                    GenerateFuncKey("11");
                    break;
                default:
                    break;
            }
        }

        public ICommand FunctionCommand => new RelayCommand<Button>(FunctionCommandCenter);
        private void FunctionCommandCenter(Button button)
        {
            if (button.Tag == null) return;
            var FK = (ORDER_FUNC_KEY)button.Tag;
            var FunKeyMaps = ResourceHelpers.FunKeyMaps;
            var mapKey = FunKeyMaps.FirstOrDefault(p => p.FK_NO == FK.FK_NO);
            if (mapKey.ItemArea == "WebInfo")
            {
                string token = DataLocals.TokenInfo.TOKEN;
                string empNo = DataLocals.PosStatus.EMP_NO;
                string url = DataLocals.AppConfig.PosComm.AspURLServer + "/pos/login/store?accessTkn=" + token + "&staffNo=" + empNo;
                var OMG = DialogHelper.MessageBox("웹 영업정보 시스템으로 로그인하시겠습니까 ?", GMessageBoxButton.OKCancel, MessageBoxImage.Question);
                if (OMG == MessageBoxResult.OK)
                {
                    Process.Start(new ProcessStartInfo(url)
                    {
                        UseShellExecute = true
                    });
                }
                else
                {
                    return;
                }
                return;
            }

            if (mapKey.FK_NO == "001")
            {
                DialogHelper.ShowDialog(typeof(TableArrangePopupViewModel), 616, 233);
            }
            else if (mapKey.FK_NO == "008")
            {
                DialogHelper.ShowDialog(typeof(RePrintBillViewModel), 510, 212);
            }
            else
            {
                IoC.Get<OrderPayMainViewModel>();
                NotifyChangePage("OrderPayMainViewModel");
                if (mapKey.ItemArea == "ActiveItemR")
                {
                    IoC.Get<OrderPayLeftTRInfoViewModel>().ButtonClick = false;
                    IoC.Get<MemberInfoViewModel>().ButtonClick = false;
                }
                IoC.Get<OrderPayMainViewModel>().ProcessFuncKeyClicked(FK);
            }
        }

        private int totalPage = 1;
        private int _currentPage = 0;
        public int CurrentPage
        {
            get
            {
                return _currentPage;
            }
            set
            {
                _currentPage = value;
                if (_currentPage < 0)
                {
                    _currentPage = 0;
                }
                if (_currentPage > totalPage - 1)
                {
                    _currentPage = totalPage - 1;
                }
                if (!RightFuncKeys.IsNullOrEmpty())
                {
                    var RegList = RightFuncKeys.Page(10, _currentPage).ToList();
                    _view.RenderFuncButtons(RegList);
                }
                NotifyOfPropertyChange(() => CurrentPage);
            }
        }
        public ICommand ToggleCommand => new RelayCommand<ToggleButton>(button =>
        {
            switch (button.Tag.ToString())
            {
                case "MenuNote":
                    MenuNote = !MenuNote;
                    ToggleBg(button, MenuNote);
                    break;
                default:
                    break;
            }
        });
        private void ToggleBg(ToggleButton button, bool enable)
        {
            if (enable)
            {
                button.Background = new ImageBrush(new BitmapImage(new Uri("pack://application:,,,/GoPOS.Resources;component/Resource/Images/btn_base_orange.png", UriKind.RelativeOrAbsolute)));
                _view.ShowOrderDetails(true);
            }
            else
            {
                LinearGradientBrush gradientBrush = new LinearGradientBrush();
                gradientBrush.StartPoint = new Point(0, 1);
                gradientBrush.EndPoint = new Point(0, 0);
                gradientBrush.GradientStops.Add(new GradientStop(Color.FromRgb(57, 71, 101), 0.48));
                gradientBrush.GradientStops.Add(new GradientStop(Color.FromRgb(26, 42, 77), 0.52));
                button.Background = gradientBrush;
                _view.ShowOrderDetails(false);
            }
        }

        public override bool SetIView(IView view)
        {
            _view = (IPostPayTableManagementView)view;
            return base.SetIView(view);
        }
        private void GenerateFuncKey(string code)
        {
            var ret = orderPayService.GetOrderFuncKey(code).Result;
            if (ret.Item2.ResultType == EResultType.SUCCESS)
            {
                RightFuncKeys = ret.Item1;
            }
            else
            {
                RightFuncKeys.Clear();
            }
            totalPage = (int)Math.Ceiling((decimal)RightFuncKeys.Count / 10);
            _view.RenderFuncButtons(RightFuncKeys);
        }

        private void InitTablethr()
        {
            Tablethr = new();
            foreach (var thr in TableMaster)
            {
                var tTable = new TABLE_THR()
                {
                    SHOP_CODE = thr.SHOP_CODE,
                    TABLE_FLAG = thr.SHAPE_FLAG,
                    X = thr.X,
                    Y = thr.Y,
                    WIDTH = thr.WIDTH,
                    HEIGHT = thr.HEIGHT,
                    TABLE_CODE = thr.TABLE_CODE,
                    TABLE_NAME = thr.TABLE_NAME,
                    ORDER_ITEMS = new()
                };
                Tablethr.Add(tTable);
            }
        }

        public void MakeTableOrderAsync(string tblCode)
        {
            IoC.Get<OrderPayMainViewModel>();
            NotifyChangePage("OrderPayMainViewModel");
            _eventAggregator.PublishOnUIThreadAsync(new TableOrder()
            {
                tableCode = tblCode
            });
        }

        public Task HandleAsync(OrderReceived message, CancellationToken cancellationToken)
        {
            var TableReceived = Tablethr.FirstOrDefault(z => z.TABLE_CODE == message.tableCode);
            foreach (var t in message.OrderItems)
            {
                ORDER_GRID_ITEM oRDER = new ORDER_GRID_ITEM()
                {
                    PRD_NAME = t.PRD_NAME,
                    SALE_QTY = t.SALE_QTY,
                    NORMAL_UPRC = t.SALE_UPRC,
                    DC_AMT_CPN = t.DC_AMT_CPN,
                    DC_AMT_CRD = t.DC_AMT_CRD,
                    DC_AMT_CST = t.DC_AMT_CST,
                    DC_AMT_FOD = t.DC_AMT_FOD,
                    DC_AMT_GEN = t.DC_AMT_GEN,
                    DC_AMT_JCD = t.DC_AMT_JCD,
                    DC_AMT_PACK = t.DC_AMT_PACK,
                    DC_AMT_PRM = t.DC_AMT_PRM,
                    DC_AMT_SVC = t.DC_AMT_SVC,
                };
                TableReceived.ORDER_ITEMS.Add(oRDER);
            }
            NotifyOfPropertyChange(() => Tablethr);
            return Task.CompletedTask;
        }
    }
}
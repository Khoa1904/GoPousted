using Caliburn.Micro;
using Dapper;
using GoPOS.Common.Service;
using GoPOS.Common.ViewModels;
using GoPOS.Helpers;
using GoPOS.Helpers.CommandHelper;
using GoPOS.Models;
using GoPOS.Models.Common;
using GoPOS.Models.Config;
using GoPOS.Sales.Services;
using GoPOS.SalesMng.ViewModels;
using GoPOS.Service.Service;
using GoPOS.Services;

using GoPOS.Views;
using GoShared.Helpers;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;


/*
 영업관리 > 중간정산
 */

namespace GoPOS.ViewModels
{

    public class SalesMiddleExcClcViewModel : OrderPayChildViewModel
    {
        private SETT_POSACCOUNT LastSettAccount = null;
        private SalesMiddleExcClcModel dataModel;
        private IPOSPrintService posPrintService;

        public string InitDateTime
        {
            get => initDateTime; set
            {
                initDateTime = value;
                NotifyOfPropertyChange(nameof(InitDateTime));
            }
        }

        public SalesMiddleExcClcModel DataModel
        {
            get => dataModel; set
            {
                dataModel = value;
                NotifyOfPropertyChange(() => DataModel);
            }
        }

        public SalesMiddleExcClcViewModel(IWindowManager windowManager, IEventAggregator eventAggregator,
            ISettAccountService settAccountService,
            ISalesMngService salesMngService,
            IPOSInitService pOSInitService, IPOSPrintService pOSPrintService) : base(windowManager, eventAggregator)
        {
            this.settAccountService = settAccountService;
            this.salesMngService = salesMngService;
            this.pOSInitService = pOSInitService;
            this.posPrintService = pOSPrintService;
            this.ViewLoaded += SalesMiddleExcClcViewModel_ViewLoaded;
        }

        private bool createNewMiddleSett = false;
        private void SalesMiddleExcClcViewModel_ViewLoaded(object? sender, EventArgs e)
        {
            LastSettAccount = settAccountService.GetSingleAsync(DataLocals.PosStatus).Result;

            if (LastSettAccount != null && LastSettAccount.OPEN_DT != null)
            {
                var ci = Thread.CurrentThread.CurrentCulture;
                string[] names = ci.DateTimeFormat.DayNames;
                var openDt = DateTime.ParseExact(LastSettAccount.OPEN_DT, "yyyyMMddHHmmss", ci);
                string dn = names[(int)ci.Calendar.GetDayOfWeek(openDt)];
                InitDateTime = string.Format("개시시간:  {0:yyyy-MM-dd HH:mm:ss} {1}", openDt, dn);
            }

            createNewMiddleSett = false;
            if (LastSettAccount == null || LastSettAccount.CLOSE_FLAG == "2")
            {
                createNewMiddleSett = true;
                LastSettAccount = new SETT_POSACCOUNT()
                {
                    SHOP_CODE = DataLocals.PosStatus.SHOP_CODE,
                    POS_NO = DataLocals.PosStatus.POS_NO,
                    SALE_DATE = DataLocals.PosStatus.SALE_DATE,
                    REGI_SEQ = DataLocals.PosStatus.REGI_SEQ.StrIntInc(2),
                    EMP_NO = DataLocals.PosStatus.EMP_NO,
                    CLOSE_FLAG = DataLocals.PosStatus.CLOSE_FLAG,
                    OPEN_DT = LastSettAccount != null ? LastSettAccount.OPEN_DT :
                                DateTime.Now.ToString("yyyyMMddHHmmss"),
                    POS_READY_AMT = 0,
                    SEND_DT = DateTime.Now.ToString("yyyyMMddHHmmss"),
                    SEND_FLAG = "0",
                    INSERT_DT = DateTime.Now.ToString("yyyyMMddHHmmss")
                };
            }

            DataModel = salesMngService.GetMiddleSettData(LastSettAccount.SALE_DATE, LastSettAccount.REGI_SEQ);
            DataModel.CopyFieldsFrom<SalesMiddleExcClcModel>(LastSettAccount, null);
        }

        public ICommand ButtonCommand => new RelayCommand<Button>(button =>
        {
            switch (button.Tag.ToString())
            {
                case "MiddleSett":
                    #region Middle Settle - 중간정산

                    var res = DialogHelper.MessageBox("중간정산 하시겠습니까?", GMessageBoxButton.OKCancel, MessageBoxImage.Question);
                    if (res == MessageBoxResult.Cancel)
                    {
                        break;
                    }

                    var mapFields = new Dictionary<string, string>();
                    mapFields.Add("ACCOUNT_TOTAL_AMT", "REM_CASH_AMT");
                    LastSettAccount = LastSettAccount.CopyFieldsFrom<SETT_POSACCOUNT>(this.DataModel, null);

                    bool printSettle = DataLocals.AppConfig.PosOption.PrintSettleFlag != "1";
                    if (DataLocals.AppConfig.PosOption.PrintSettleFlag == "2")
                    {
                        var printRes = DialogHelper.MessageBox("정산지 출력하시겠습니까?", GMessageBoxButton.OKCancel, MessageBoxImage.Question);
                        printSettle = printRes == MessageBoxResult.OK;
                    }

                    if (printSettle)
                    {
                        posPrintService.PrintMiddleSettle(Common.PrinterLib.PrintOptions.Normal, LastSettAccount);
                        posPrintService.EndPrinting();
                    }

                    if (_fromSalesClose)
                    {
                        var confirm = DialogHelper.MessageBox("중간정산 완료 되었습니다.", GMessageBoxButton.OK, MessageBoxImage.Information);
                        if (confirm == MessageBoxResult.OK)
                        {
                            _eventAggregator.PublishOnUIThreadAsync(new SalesMngMainViewEventArgs()
                            {
                                EventType = "OpenVM",
                                EventData = "SalesClosingSettleSumViewModel",
                                EventData2 = LastSettAccount,
                                EventFlag = createNewMiddleSett
                            });
                        }
                    }
                    else
                    {
                        pOSInitService.DoMiddleSettAccount(LastSettAccount);
                        DialogHelper.MessageBox("중간정산 완료 되었습니다.", GMessageBoxButton.OK, MessageBoxImage.Information);
                        Application.Current.MainWindow.Close();
                        SystemHelper.ProgramRestart(false);
                    }

                    this.DeactivateClose(true);

                    #endregion

                    break;

                case "PrePrint":
                    LastSettAccount = LastSettAccount.CopyFieldsFrom<SETT_POSACCOUNT>(this.DataModel, null);
                    posPrintService.PrintMiddleSettle(Common.PrinterLib.PrintOptions.Normal, LastSettAccount);
                    posPrintService.EndPrinting();
                    break;

                case "GiftCardDetails":
                    LastSettAccount = LastSettAccount.CopyFieldsFrom<SETT_POSACCOUNT>(this.DataModel, null);
                    DialogHelper.ShowDialog(typeof(SalesMiddleExcGiftCertDetailsViewModel), 650, 500, LastSettAccount);
                    break;

                case "ApprDetails":
                    LastSettAccount = LastSettAccount.CopyFieldsFrom<SETT_POSACCOUNT>(this.DataModel, null);
                    DialogHelper.ShowDialog(typeof(SalesMiddleExcApprDetailsViewModel), 650, 500, LastSettAccount);
                    break;

                case "TrialReg":
                    return;
                    //       DialogHelper.MessageBox("Ahihi", GMessageBoxButton.OK, MessageBoxImage.Information);
                    break;
                default:
                    break;
            }
        });

        private bool _fromSalesClose = false;
        private string initDateTime;

        public override void SetData(object data)
        {
            object[] datas = (object[])data;
            _fromSalesClose = datas.Length == 0 ? false : "FCLOSE".Equals(datas[0].ToString());
        }

        private readonly ISettAccountService settAccountService;
        private readonly ISalesMngService salesMngService;
        private readonly IPOSInitService pOSInitService;
    }


}
using Caliburn.Micro;
using Dapper;
using GoPOS.Common.Service;
using GoPOS.Helpers;
using GoPOS.Helpers.CommandHelper;
using GoPOS.Models;
using GoPOS.Models.Common;
using GoPOS.Sales.Services;
using GoPOS.Service.Service;
using GoPOS.Services;

using GoPOS.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;


/*
 영업관리 > 시재점검
 */

namespace GoPOS.ViewModels
{

    public class SalesTmPrCheckViewModel : OrderPayChildViewModel
    {
        private readonly IOrderPayService orderPayService;
        private readonly ISalesMngService salemngServant;
        private SETT_POSACCOUNT POSSettAccount = null;
        private SalesMiddleExcClcModel dataModel;
        private IPOSPrintService posPrintService;
        private readonly ISettAccountService accountServant;

        public SalesTmPrCheckViewModel(IWindowManager windowManager, IEventAggregator eventAggregator, IOrderPayService payServant, ISettAccountService settAccountService,
            ISalesMngService salesMngService, IPOSPrintService pOSPrintService) : base(windowManager, eventAggregator)
        {
            this.orderPayService = payServant;
            this.posPrintService = pOSPrintService;
            this.accountServant = settAccountService;
            this.salemngServant = salesMngService;
            this.ViewLoaded += TrialViewLoaded;
        }

        public SalesMiddleExcClcModel DataModel
        {
            get => dataModel; set
            {
                dataModel = value;
                NotifyOfPropertyChange(() => DataModel);
            }
        }
        public ICommand ButtonCommand => new RelayCommand<Button>(ButtonCommandCenter);
        private void ButtonCommandCenter(Button btn)
        {
            switch(btn.Tag)
            {
                case "PRINT":
                    posPrintService.PrintTrialCheck(Common.PrinterLib.PrintOptions.Normal, POSSettAccount.SHOP_CODE,
                        POSSettAccount.POS_NO, POSSettAccount.SALE_DATE, POSSettAccount.REGI_SEQ);
                    posPrintService.EndPrinting();
                    break;

                default: break;
            }
        }

        private void TrialViewLoaded(object? sender, EventArgs e)
        {
            POSSettAccount = accountServant.GetSingleAsync(DataLocals.PosStatus).Result;

            if (POSSettAccount != null && POSSettAccount.OPEN_DT != null)
            {
                var ci = Thread.CurrentThread.CurrentCulture;
                string[] names = ci.DateTimeFormat.DayNames;
                var openDt = DateTime.ParseExact(POSSettAccount.OPEN_DT, "yyyyMMddHHmmss", ci);
                string dn = names[(int)ci.Calendar.GetDayOfWeek(openDt)];
            }
            DataModel = salemngServant.GetMiddleSettData(DataLocals.PosStatus.SALE_DATE, DataLocals.PosStatus.REGI_SEQ);
        }
    }
}
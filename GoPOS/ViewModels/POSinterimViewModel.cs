using System.Windows;
using Caliburn.Micro;
using GoPOS.Models;
using GoPOS.Views;
using GoPOS.Services;
using GoPOS.Common.ViewModels;
using GoPOS.Interface;
using GoPOS.Common.Interface.View;
using GoPOS.Models.Common;
using static GoShared.Events.GoPOSEventHandler;
using GoShared.Events;
using GoPOS.Common.Interface.Model;
using System.Windows.Input;
using GoPOS.Helpers.CommandHelper;
using System;
using System.Windows.Controls;
using System.Diagnostics;
using System.Threading.Tasks;
using GoPOS.Helpers;
using System.Threading;
using System.Collections.Generic;
using Dapper;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Data;
using GoShared.Helpers;
using System.Windows.Documents;

namespace GoPOS.ViewModels
{
    /// <summary>
    /// 개시화면
    /// </summary>
    public class POSinterimViewModel : BaseItemViewModel, IDialogViewModel
    {
        public POS_STATUS? PosStatus { get; set; }
        public SETT_POSACCOUNT? LastSettAccount { get; set; }
        public decimal POS_RDY_AMT { get; set; }

        public string LAST_SALE_DATE
        {
            get
            {
                var ci = System.Threading.Thread.CurrentThread.CurrentCulture;
                var saleDt = DateTime.ParseExact(LastSettAccount?.SALE_DATE, "yyyyMMdd", ci);
                string[] names = ci.DateTimeFormat.DayNames;
                string dn = names[(int)ci.Calendar.GetDayOfWeek(saleDt)];

                return string.Format(@"{0:yyyy}-{0:MM}-{0:dd} {1}", saleDt, dn);
            }
        }

        public string LAST_CLOSE_DT
        {
            get
            {
                var closeDt = DateTime.ParseExact(LastSettAccount?.CLOSE_DT, "yyyyMMddHHmmss", Thread.CurrentThread.CurrentCulture);
                return string.Format(@"{0:yyyy}-{0:MM}-{0:dd} {0:HH:mm:ss}", closeDt);
            }
        }

        public string LAST_SETT_EMP
        {
            get
            {
                string empNo = LastSettAccount?.EMP_NO;
                var emp = empService.GetEmpInfo(empNo);
                return emp != null ? string.Format("{0}-{1} {2}차", emp.EMP_NAME, LastSettAccount?.EMP_NO, Convert.ToInt32(LastSettAccount?.REGI_SEQ)) : string.Empty;
            }
        }

        public string INIT_EMP
        {
            get
            {
                return string.Format("{0}-{1}", DataLocals.Employee.EMP_NAME, DataLocals.Employee.EMP_NO);
            }
        }


        public string SYS_DATE
        {
            get
            {
                return DateTime.Today.ToString("yyyy-MM-dd");
            }
        }

        public string SALE_DATE
        {
            get
            {
                var cur = Thread.CurrentThread.CurrentUICulture;
                var saleDt = DateTime.ParseExact(PosStatus.SALE_DATE, "yyyyMMdd", cur);
                var dayName = cur.DateTimeFormat.GetAbbreviatedDayName(saleDt.DayOfWeek);
                return saleDt.ToString("yyyy-MM-dd") + " " + dayName;
            }
        }

        public string REGI_SEQ
        {
            get
            {
                return string.Format("{0:d1}차", Convert.ToInt32(PosStatus.REGI_SEQ));
            }
        }

        public InputKeyPadViewModel? InputKeyPad { get; set; }

        public Dictionary<string, object> DialogResult { get; set; }

        private IPOSInitService _initService;
        private readonly IInfoEmpService empService;

        public POSinterimViewModel(IWindowManager windowManager, IEventAggregator eventAggregator,
            IPOSInitService initService, IInfoEmpService empService) :
            base(windowManager, eventAggregator)
        {
            InputKeyPad = IoC.Get<InputKeyPadViewModel>();
            _initService = initService;
            this.empService = empService;
            this.DialogResult = new Dictionary<string, object>();
            this.DialogResult.Add("RETURN", false);
        }

        private IPOSOpeningView _view;
        public override bool SetIView(IView view)
        {
            _view = (IPOSOpeningView)view;
            return base.SetIView(view);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        public override void SetData(object data)
        {
            object[] datas = (object[])data;
            PosStatus = (POS_STATUS)datas[0];
            LastSettAccount = (SETT_POSACCOUNT)datas[1];
        }


        public ICommand ButtonCommand => new RelayCommand<Button>((button) =>
        {
            if (button.Tag == null)
            {
                return;
            }

            if (button.Tag.ToString() == "Open")
            {
                var ret = _initService.DoPOSOpen(PosStatus, POS_RDY_AMT);
                DialogResult["RETURN"] = ret;
                this.DeactivateClose(true);
            }
        });
    }
}

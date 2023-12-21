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
using Microsoft.VisualBasic;
using Dapper;
using System.Data;
using GoShared.Helpers;
using Microsoft.AspNetCore.Http.Features;
using Flurl.Http;

namespace GoPOS.ViewModels
{
    /// <summary>
    /// 개시화면
    /// </summary>
    public class POSOpeningViewModel : BaseItemViewModel, IDialogViewModel
    {
        public POS_STATUS? PosStatus { get; set; }
        public SETT_POSACCOUNT? LastSettAccount { get; set; }
        public decimal POS_RDY_AMT { get; set; }

        public string LAST_SALE_DATE
        {
            get
            {
                if (LastSettAccount == null) return string.Empty;
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
                if (LastSettAccount == null) return string.Empty;
                var closeDt = DateTime.ParseExact(LastSettAccount?.CLOSE_DT, "yyyyMMddHHmmss", Thread.CurrentThread.CurrentCulture);
                return string.Format(@"{0:yyyy}-{0:MM}-{0:dd} {0:HH:mm:ss}", closeDt);
            }
        }

        public string LAST_SETT_EMP
        {
            get
            {
                if (LastSettAccount == null) return string.Empty;
                string empNo = LastSettAccount?.EMP_NO;
                var emp = empService.GetEmpInfo(empNo);
                return emp != null ? string.Format("{0}-{1} {2}차", emp.EMP_NAME, LastSettAccount?.EMP_NO, Convert.ToInt32(LastSettAccount?.REGI_SEQ)) : string.Empty;
            }
        }

        private string _empNo;
        public string EmpNo
        {
            get { return string.Format("{0}-{1}", DataLocals.Employee.EMP_NAME, DataLocals.Employee.EMP_NO); }
            set
            {
                if (_empNo != value)
                {
                    _empNo = value;
                    NotifyOfPropertyChange(() => EmpNo);
                }
            }
        }

        /*
        public string INIT_EMP
        {
            get
            {
                emp_name = string.Format("{0}-{1}", DataLocals.Employee.EMP_NAME, DataLocals.Employee.EMP_NO);
                return emp_name;
            }
            set 
            {
                INIT_EMP = emp_name; 
            }
        }
        */

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
                int regSeq = PosStatus != null ? 1 : TypeHelper.ToInt32(PosStatus.REGI_SEQ);
                return saleDt.ToString("yyyy-MM-dd") + " " + dayName + string.Format(" [{0}차]", regSeq);
            }
        }

        public InputKeyPadViewModel? InputKeyPad { get; set; }

        public Dictionary<string, object> DialogResult { get; set; }

        private IPOSInitService _initService;
        private readonly IInfoEmpService empService;

        public POSOpeningViewModel(IWindowManager windowManager, IEventAggregator eventAggregator,
            IPOSInitService initService, IInfoEmpService empService) :
            base(windowManager, eventAggregator)
        {
            this.InputKeyPad = IoC.Get<InputKeyPadViewModel>();
            _initService = initService;
            this.empService = empService;
            this.DialogResult = new Dictionary<string, object>();
            this.DialogResult.Add("RETURN", false);
            this.ViewLoaded += POSOpeningViewModel_ViewLoaded;
        }

        private void POSOpeningViewModel_ViewLoaded(object? sender, EventArgs e)
        {
            EmpNo = string.Format("{0}-{1}", DataLocals.Employee.EMP_NAME, DataLocals.Employee.EMP_NO);
        }

        private POSOpeningView _view;
        public override bool SetIView(IView view)
        {
            var xview = view as POSOpeningView;
            if (xview != null)
            {
                _view = xview;
                return base.SetIView(view);
            }
            return false;

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
                var res = DialogHelper.MessageBox("개점처리 하시겠습니까?", GMessageBoxButton.OKCancel, MessageBoxImage.Question);
                if (res == MessageBoxResult.Cancel)
                {
                    return;
                }
                var ret = _initService.DoPOSOpen(PosStatus, POS_RDY_AMT);
                DialogResult["RETURN"] = ret;
                // NotifyChangePage("OrderPayMainViewModel");
                this.DeactivateClose(true);
            }
        });

    }
}

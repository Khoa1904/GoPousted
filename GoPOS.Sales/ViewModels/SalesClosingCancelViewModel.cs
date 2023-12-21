using Caliburn.Micro;
using Dapper;
using GoPOS.Common.Interface.View;
using GoPOS.Helpers;
using GoPOS.Helpers.CommandHelper;
using GoPOS.Models;
using GoPOS.Models.Common;
using GoPOS.Sales.Interface.View;
using GoPOS.Service.Service;
using GoPOS.Services;

using GoPOS.Views;
using GoShared.Helpers;
using Microsoft.Xaml.Behaviors.Media;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;


/*
 영업관리 > 마감취소
 */

namespace GoPOS.ViewModels
{
    public class SalesClosingCancelViewModel : OrderPayChildViewModel
    {
        private readonly IInfoEmpService empService;
        private readonly ISettAccountService settAccountService;
        private readonly IPOSInitService pOSInitService;
        private ISalesClosingCancelView _view;
        //private string _empName;
        private SETT_POSACCOUNT _lastSettAccount;

        /// <summary>
        /// true: cancel settle, false: cancel open, 
        /// </summary>
        private bool doCancelSettle = false;
        private string closeTitle = "마감";
        private string saleDateTitle = "마감";
        private string closeEmpTitle = "마감";
        private string closeTimeTitle = "마감";

        public SalesClosingCancelViewModel(IWindowManager windowManager, IEventAggregator eventAggregator,
            IInfoEmpService empService, ISettAccountService settAccountService,
            IPOSInitService pOSInitService) : base(windowManager, eventAggregator)
        {
            this.empService = empService;
            this.settAccountService = settAccountService;
            this.pOSInitService = pOSInitService;
            this.ViewLoaded += SalesClosingCancelViewModel_ViewLoaded;
        }

        private void SalesClosingCancelViewModel_ViewLoaded(object? sender, EventArgs e)
        {
            _lastSettAccount = settAccountService.GetSingleAsync(DataLocals.PosStatus).Result;
            //_empName = empService.TryGet(p => p.SHOP_CODE == DataLocals.AppConfig.PosInfo.StoreNo && p.EMP_NO == DataLocals.PosStatus.EMP_NO).EMP_NAME;

            if ("1".Equals(DataLocals.PosStatus.CLOSE_FLAG) && Convert.ToInt32(DataLocals.PosStatus.BILL_NO) == 0)
            {
                doCancelSettle = false;
            }
            else
            {
                doCancelSettle = true;
            }

            string closeAction = doCancelSettle ? "마감" : "개점";
            CloseTitle = string.Format("{0}현황", closeAction);
            SaleDateTitle = string.Format("{0} 영업일자", closeAction);
            CloseTimeTitle = string.Format("{0} 처리시간", closeAction);
            CloseEmpTitle = string.Format("{0} 판매원", closeAction);

            NotifyOfPropertyChange(() => ActionText);
            NotifyOfPropertyChange(() => LAST_CLOSE_DT);
            NotifyOfPropertyChange(() => LAST_SALE_DATE);
            NotifyOfPropertyChange(() => LAST_SETT_EMP);
            NotifyOfPropertyChange(() => ActionText);
            NotifyOfPropertyChange(() => OPENING_EMP);
        }


        public string CloseTitle
        {
            get => closeTitle; set
            {
                closeTitle = value;
                NotifyOfPropertyChange(nameof(CloseTitle));
            }
        }
        public string SaleDateTitle
        {
            get => saleDateTitle; set
            {
                saleDateTitle = value;
                NotifyOfPropertyChange(() => SaleDateTitle);
            }
        }
        public string CloseEmpTitle
        {
            get => closeEmpTitle; set
            {
                closeEmpTitle = value;
                NotifyOfPropertyChange(() => CloseEmpTitle);
            }
        }
        public string CloseTimeTitle
        {
            get => closeTimeTitle; set
            {
                closeTimeTitle = value;
                NotifyOfPropertyChange(() => CloseTimeTitle);
            }
        }

        public string LAST_SALE_DATE
        {
            get
            {
                if (_lastSettAccount == null) return string.Empty;
                var ci = System.Threading.Thread.CurrentThread.CurrentCulture;
                var saleDt = DateTime.ParseExact(_lastSettAccount?.SALE_DATE, "yyyyMMdd", ci);
                string[] names = ci.DateTimeFormat.DayNames;
                string dn = names[(int)ci.Calendar.GetDayOfWeek(saleDt)];
                return string.Format(@"{0:yyyy}-{0:MM}-{0:dd} {1}", saleDt, dn);
            }
        }

        public string LAST_CLOSE_DT
        {
            get
            {
                if (_lastSettAccount == null) return string.Empty;

                string handleDt = doCancelSettle ? _lastSettAccount.CLOSE_DT : _lastSettAccount.OPEN_DT;
                if (string.IsNullOrEmpty(handleDt)) return string.Empty;

                var closeDt = DateTime.ParseExact(handleDt, "yyyyMMddHHmmss", Thread.CurrentThread.CurrentCulture);
                return string.Format(@"{0:yyyy}-{0:MM}-{0:dd} {0:HH:mm:ss}", closeDt);
            }
        }

        public string LAST_SETT_EMP
        {
            get
            {
                if (doCancelSettle)
                {
                    if (_lastSettAccount == null) return string.Empty;
                    string empNo = _lastSettAccount?.EMP_NO;
                    var emp = empService.GetEmpInfo(empNo);
                    return emp != null ? string.Format("{0}-{1} {2}차", emp.EMP_NAME, _lastSettAccount?.EMP_NO, Convert.ToInt32(_lastSettAccount?.REGI_SEQ)) : string.Empty;

                }
                else
                {
                    return DataLocals.Employee.EMP_NO + " " + DataLocals.Employee.EMP_NAME;
                }

            }
        }
        private string openingEMP { get; set; }

        public string OPENING_EMP
        {
            get
            {
                if (DataLocals.Employee.EMP_NAME != null && DataLocals.Employee.EMP_NO != null)
                {
                    return DataLocals.Employee.EMP_NO + " " + DataLocals.Employee.EMP_NAME;
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        public string CANCEL_SETTLE_EMP
        {
            get
            {
                return string.Format("{0}-{1}", DataLocals.Employee.EMP_NAME, DataLocals.Employee.EMP_NO);
            }
        }

        public string ActionText
        {
            get
            {
                return doCancelSettle ? "마감취소" : "개점취소";
            }
        }

        public override bool SetIView(IView view)
        {
            _view = (ISalesClosingCancelView)view;
            return base.SetIView(view);
        }

        public ICommand CloseSettAccount => new RelayCommand<Button>(button =>
        {
            // Check password first
            //_view.txtEMP_PWD.Text
            var exs = empService.CheckExist(p => p.SHOP_CODE == DataLocals.AppConfig.PosInfo.StoreNo &&
                            p.EMP_NO == _lastSettAccount.EMP_NO &&
                            p.EMP_PWD == _view.txtEMP_PWD.Text);
            if (!exs)
            {
                DialogHelper.MessageBox("비밀번호가 일치하지 않습니다.", GMessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var ask = DialogHelper.MessageBox((doCancelSettle ? "마감" : "개점") + "취소하시겠습니까?.", GMessageBoxButton.OKCancel, MessageBoxImage.Question);
            if (ask == MessageBoxResult.Cancel)
            {
                return;
            }

            string errorMsg = pOSInitService.DoCloseSettAccountCancel(_lastSettAccount, !doCancelSettle);
            if (string.IsNullOrEmpty(errorMsg))
            {
                DialogHelper.MessageBox((doCancelSettle ? "마감" : "개점") + "취소 완료 되었습니다. 포스 재시작합니다.", GMessageBoxButton.OK, MessageBoxImage.Information);
                Application.Current.MainWindow.Close();
                SystemHelper.ProgramRestart();
            }
            else
            {
                DialogHelper.MessageBox("마감취소 오류: " + errorMsg, GMessageBoxButton.OK, MessageBoxImage.Error);
            }
        });
    }


}
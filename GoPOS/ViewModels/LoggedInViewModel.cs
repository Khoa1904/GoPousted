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
using GoPOS.Helpers;
using System.Threading.Tasks;
using Dapper;
using GoShared.Helpers;
using System.Data;
using System.Security.Cryptography.X509Certificates;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using GoPOS.Common.Helpers;
using GoPOS.Database;

namespace GoPOS.ViewModels
{
    /// <summary>
    /// 개시화면
    /// </summary>
    public class LoggedInViewModel : BaseItemViewModel
    {
        //private int OpeningFlag;
        private IPOSInitService _initService;

        //bool res = false;
        public LoggedInViewModel(IWindowManager windowManager, IEventAggregator eventAggregator, IPOSInitService IniService) :
            base(windowManager, eventAggregator)
        {
            _initService = IniService;
        }

        public ICommand ButtonCommand => new RelayCommand<Button>(ButtonCommandImpl);

        private void ButtonCommandImpl(Button button)
        {
            switch (button.Tag)
            {
                case "MenuShutdown":
                    DialogHelper.ShowDialog(typeof(ShutdownViewModel), 400, 250);
                    break;
                case "OrderPayMainViewModel":
                    var orderMethod = "0000001";// DataLocals.AppConfig.PosOption.POSOrderMethod;
                    POS_STATUS posStatus = null;
                    SETT_POSACCOUNT settAccount = null;
                    var retValue = _initService.CheckPOSOpen(out posStatus, out settAccount);
                    if (retValue != 0)
                    {
                        var dr = DialogHelper.ShowDialog(retValue == 1 ? typeof(POSOpeningViewModel) : typeof(POSinterimViewModel), 680, 530, posStatus, settAccount);
                        if (dr == null || !(bool)dr["RETURN"])
                        {
                            return;
                        }
                    }

                    CheckSaleDate();

                    //  continue MainPage
                    if (orderMethod == "0000001") // proceed with table management screen - Postpay order
                    {
                        NotifyChangePage("PostPayTableManagermentViewModel");
                    }
                    else // proceed with normal orderpay screen - prepay order
                    {
                        NotifyChangePage((string)button.Tag);
                    }
            
                    break;
                case "SalePurchaseMainViewModel":
                    return;
                default:
                    NotifyChangePage((string)button.Tag);
                    break;
            }
        }

        private void CheckSaleDate()
        {
            string today = DateTime.Today.ToString("yyyyMMdd");

            if (Convert.ToInt32(today) > Convert.ToInt32(DataLocals.PosStatus.SALE_DATE))
            {
                var ci = System.Threading.Thread.CurrentThread.CurrentCulture;
                var saleDt = DateTime.ParseExact(DataLocals.PosStatus.SALE_DATE, "yyyyMMdd", ci);
                string[] names = ci.DateTimeFormat.DayNames;
                string dn = names[(int)ci.Calendar.GetDayOfWeek(saleDt)];

                string msg = string.Format("해당 영업일자 [{0:yyyy/MM/dd} {1}] 개점하고 마감하지 않아 현재 시스템일자와 서로 다릅니다.\r\n포스의 영엉관리에서 마감정산을 하셔야 영업일자를 당일로 변경되며  당일 영업매출이 집계 처리되어집니다.",
                            saleDt, dn);
                DialogHelper.MessageBox(msg, GMessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}

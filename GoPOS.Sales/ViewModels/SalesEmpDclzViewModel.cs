using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

using Caliburn.Micro;
using GoPOS.Helpers.CommandHelper;
using GoPOS.Helpers;
using GoPOS.Models;
using GoPOS.Services;
using GoPOS.ViewModels.Controls;
using Dapper;
using GoPOS.Views;
using static GoPOS.Function;

namespace GoPOS.ViewModels
{
    public class SalesEmpDclzViewModel : Screen
    {
        //private IWindowManager _windowManager;
        //private IEventAggregator _eventAggregator;

        private IInfoEmpService _empMService;
        private ISalesEmpDclzViewService salesEmpDclzViewService;

        //private string time = string.Empty;
        //private string salesDateString = string.Empty;
        //private string currentDateString = string.Empty;
        //private string currentTimeString = string.Empty;

        //public string EMP_NO
        //{
        //    get; set;
        //} = string.Empty;

        //public string EMP_PWD
        //{
        //    get; set;
        //} = string.Empty;

        //private string emp_name = string.Empty;
        //public string EMP_NAME
        //{
        //    get { return emp_name; }
        //    set
        //    {
        //        emp_name = value;
        //        NotifyOfPropertyChange(nameof(EMP_NAME));

        //        NotifyOfPropertyChange(nameof(EMP_NO));
        //        NotifyOfPropertyChange(nameof(EMP_PWD));
        //    }
        //}

        //public string EMP_IO_FLAG_STR
        //{
        //    get; set;
        //} = string.Empty;

        //public string SalesDateString
        //{
        //    get
        //    {
        //        return salesDateString;
        //    }
        //    set
        //    {
        //        salesDateString = value;
        //        SalesMngHaeder.SalesDateString = salesDateString;
        //        NotifyOfPropertyChange(nameof(SalesDateString));
        //    }
        //}

        //public string CurrentDateString
        //{
        //    get
        //    {
        //        return currentDateString;
        //    }
        //    set
        //    {
        //        currentDateString = value;
        //        SalesMngHaeder.CurrentDateString = currentDateString;
        //        NotifyOfPropertyChange(nameof(CurrentDateString));
        //    }
        //}

        //public string CurrentTimeString
        //{
        //    get
        //    {
        //        return currentTimeString;
        //    }
        //    set
        //    {
        //        currentTimeString = value;
        //        SalesMngHaeder.CurrentTimeString = currentTimeString;
        //        NotifyOfPropertyChange(nameof(CurrentTimeString));
        //    }
        //}

        //public SalesManagerHeaderControlViewModel SalesMngHaeder { get; }
        //public SalesManagerMenuControlViewModel SalesMngMenu { get; }

        public SalesEmpDclzViewModel(IInfoEmpService empMService, ISalesEmpDclzViewService service)
        {
            //_windowManager = windowManager;
            //_eventAggregator = eventAggregator;

            _empMService = empMService;
            salesEmpDclzViewService = service;

            //_eventAggregator.SubscribeOnUIThread(this);

            //// 영업관리 Header 컨트롤 초기화
            //this.SalesMngHaeder = new SalesManagerHeaderControlViewModel(windowManager, eventAggregator, empMService, shopMService);
            //this.SalesMngHaeder.LoggedInEmployee = IoC.Get<ShellViewModel>().LoggedInEmployee;

            //// 영업관리 오른쪽 메뉴 초기화
            //this.SalesMngMenu = new SalesManagerMenuControlViewModel(windowManager, eventAggregator, "EmpDclzViewModel");
        }

        public void Init()
        {

        }

        public void ButtonMenu(object sender, RoutedEventArgs e)
        {
            if (sender is not Button btn)
            {
                return;
            }


            string sValue = (string)btn.Tag;

            if (sValue == "IN")
            {
                if (chkInput(sValue))
                {
                    //출근
                    EmployeeCheckIn();
                }
            }
            else if (sValue == "OUT")
            {
                if (chkInput(sValue))
                {
                    //퇴근
                    EmployeeCheckOut();
                }
            }
        }

        public bool chkInput(string sType)
        {
            if (IoC.Get<SalesEmpDclzView>().txtEMP_CARD_NO.Text.Trim() == "" && IoC.Get<SalesEmpDclzView>().txtEMP_NO.Text.Trim() == "")
            {
                DialogHelper.MessageBox("사원카드 또는 사원번호를 입력하세요.");
                return false;
            }

            if (IoC.Get<SalesEmpDclzView>().txtEMP_PWD.Text.Trim() == "")
            {
                DialogHelper.MessageBox("비밀번호를 입력하세요.");
                return false;
            }

            if (sType == "IN")
            {
                //bool ret = this._empMService.ContainEmpM(username, password);
                //2023-04-01 일자에 출근등록 후 퇴근 처리를 하지 않아, 출근 처리를 할 수 없습니다.
                /*
                 
                    select *
                    FROM SCD_EMPMS_T A     LEFT OUTER JOIN ( SELECT SHOP_CD                            , EMP_NO                            , EMP_IO_DT AS EMP_IO_DT                            , EMP_IO_FG                       
                    FROM POS_EMPIO_T                       
                                          ORDER BY EMP_IO_DT DESC                       ROWS 1 TO 1 ) B
                    ON  A.SHOP_CD = B.SHOP_CD    
                    AND A.EMP_NO  = B.EMP_NO
                    WHERE A.EMP_NO  = '0000'
                 
                 
                 */
            }
            else if (sType == "OUT")
            {
                //bool ret = this._empMService.ContainEmpM(username, password);
                //2023-04-01 일자에 퇴근등록 후 출근 처리를 하지 않아, 퇴근 처리를 할 수 없습니다.
            }

            return true;
        }

        // 출근 버튼
        public async void EmployeeCheckIn()
        {
            ////bool ret = this._empMService.ContainEmpM(username, password);

            ////if (ret)
            ////{
            ////    MST_INFO_EMP data = this._empMService.GetEmpM(username, password);

            ////    EMP_NAME = data.EMP_NAME;

            ////    EMP_IO_FLAG_STR = "출근";

            ////    //사원카드 또는 사원번호를 입력하세요.
            ////    //비밀번호를 입력하세요.
            ////    //2023-04-01 일자에 출근등록 후 퇴근 처리를 하지 않아, 출근 처리를 할 수 없습니다.


            ////    if (DialogHelper.Show("입력하신 사원번호, 비밀번호로 출근 등록하시겠습니까?"))
            ////    {
            ////        EMP_INOUT_HISTORY emp_inout_history = new EMP_INOUT_HISTORY()
            ////        {
            ////            SHOP_CODE = data.SHOP_CODE,
            ////            EMP_IO_DT = string.Format("{0:00}", DateTime.Now.Hour),
            ////            EMP_NO = data.EMP_NO,
            ////            SALE_DATE = string.Format("{0:0000}{1:00}{2:00}", DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),
            ////            EMP_IO_FLAG = "0",  // 출근                        

            ////            EMP_NAME = data.EMP_NAME,
            ////            POS_NO = "01",      // 현재 POS번호 01
            ////            SEND_FLAG = "0",    // 미전송
            ////            SEND_DT = "",       // 미전송으로 전송일자 없음
            ////            EMP_IO_REMARK = "출근"
            ////        };

            ////        _empInoutHistoryService.AddEmpInoutHistory(emp_inout_history);

            ////        DialogHelper.Show("입력하신 사원번호, 비밀번호로 출근 등록하였습니다.");

            ////        EMP_NO = string.Empty;
            ////        EMP_PWD = string.Empty;
            ////        EMP_NAME = string.Empty;                    

            ////        return true;
            ////    }
            ////}
            ////else
            ////{
            ////    DialogHelper.Show("입력하신 사원번호, 비밀번호로 사원 정보를 조회할 수 없습니다.");
            ////}


            //try
            //{
            //    if (!DialogHelper.Show("출근 등록하시겠습니까?"))
            //    {
            //        return;
            //    }

            //    /*
            //    CREATE OR ALTER PROCEDURE SP_READY_AMOUNT_U (
            //    SHOP_CODE VARCHAR(6),
            //    SALE_DATE VARCHAR(8),
            //    POS_NO VARCHAR(2),
            //    REGI_SEQ VARCHAR(2),
            //    POS_READY_AMT NUMERIC(12,2))
            //    */

            //    POS_SETTLEMENT_DETAIL result = new POS_SETTLEMENT_DETAIL();

            //    DynamicParameters param = new();
            //    param.Add("@SHOP_CODE", DataLocals.AppConfig.PosInfo.StoreNo);
            //    param.Add("@SALE_DATE", DataLocals.PosStatus.SALE_DATE);
            //    param.Add("@POS_NO", DataLocals.AppConfig.PosInfo.PosNo);
            //    param.Add("@REGI_SEQ", DataLocals.PosStatus.REGI_SEQ);
            //    param.Add("@POS_READY_AMT", CommaDel(IoC.Get<SalesResvMneyRegistView>().txtPosReadyAmt.Text));

            //    (result, SpResult spResult) = await salesEmpDclzViewService.AddEmpInoutHistory(param);

            //    if (spResult.ReusltType != EResultType.SUCCESS)
            //    {
            //        LogHelper.Logger.Error("Insert" + spResult.ReusltType);
            //        return;
            //    }

            //    IoC.Get<SalesResvMneyRegistView>().txtPosReadyAmt.Text = Comma(IoC.Get<SalesResvMneyRegistView>().txtPosReadyAmt.Text);

            //    DialogHelper.Show("등록하였습니다.");

            //}
            //catch (Exception a)
            //{
            //    //Console.WriteLine(a.Message);
            //    LogHelper.Logger.Error(a.Message);
            //}

        }

        // 퇴근 버튼
        public async void EmployeeCheckOut()
        {
            ////bool ret = this._empMService.ContainEmpM(username, password);

            ////if (ret)
            ////{
            ////    MST_INFO_EMP data = this._empMService.GetEmpM(username, password);

            ////    EMP_NAME = data.EMP_NAME;

            ////    EMP_IO_FLAG_STR = "퇴근";

            ////    //사원카드 또는 사원번호를 입력하세요.
            ////    //비밀번호를 입력하세요.
            ////    //2023-04-01 일자에 퇴근등록 후 출근 처리를 하지 않아, 퇴근 처리를 할 수 없습니다.
            ////    if (DialogHelper.Show("퇴근 등록하시겠습니까?"))
            ////    {
            ////        EMP_INOUT_HISTORY emp_inout_history = new EMP_INOUT_HISTORY()
            ////        {
            ////            SHOP_CODE = data.SHOP_CODE,
            ////            EMP_IO_DT = string.Format("{0:00}", DateTime.Now.Hour),
            ////            EMP_NO = data.EMP_NO,
            ////            SALE_DATE = string.Format("{0:0000}{1:00}{2:00}", DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),
            ////            EMP_IO_FLAG = "1",  // 퇴근

            ////            EMP_NAME = data.EMP_NAME,
            ////            POS_NO = "01",      // 현재 POS번호 01
            ////            SEND_FLAG = "0",    // 미전송
            ////            SEND_DT = "",       // 미전송으로 전송일자 없음
            ////            EMP_IO_REMARK = "퇴근"
            ////        };

            ////        _empInoutHistoryService.AddEmpInoutHistory(emp_inout_history);

            ////        DialogHelper.Show("등록하였습니다.");

            ////        EMP_NO = string.Empty;
            ////        EMP_PWD = string.Empty;
            ////        EMP_NAME = string.Empty;

            ////        return true;
            ////    }                    
            ////}
            ////else
            ////{
            ////    DialogHelper.Show("입력하신 사원번호, 비밀번호로 사원 정보를 조회할 수 없습니다.");
            ////}

            //try
            //{
            //    if (!DialogHelper.Show("퇴근 등록하시겠습니까?"))
            //    {
            //        return;
            //    }

            //    /*
            //    CREATE OR ALTER PROCEDURE SP_READY_AMOUNT_U (
            //    SHOP_CODE VARCHAR(6),
            //    SALE_DATE VARCHAR(8),
            //    POS_NO VARCHAR(2),
            //    REGI_SEQ VARCHAR(2),
            //    POS_READY_AMT NUMERIC(12,2))
            //    */

            //    POS_SETTLEMENT_DETAIL result = new POS_SETTLEMENT_DETAIL();

            //    DynamicParameters param = new();
            //    param.Add("@SHOP_CODE", DataLocals.AppConfig.PosInfo.StoreNo);
            //    param.Add("@SALE_DATE", DataLocals.PosStatus.SALE_DATE);
            //    param.Add("@POS_NO", DataLocals.AppConfig.PosInfo.PosNo);
            //    param.Add("@REGI_SEQ", DataLocals.PosStatus.REGI_SEQ);
            //    //param.Add("@POS_READY_AMT", CommaDel(IoC.Get<SalesResvMneyRegistView>().txtPosReadyAmt.Text));

            //    (result, SpResult spResult) = await salesEmpDclzViewService.AddEmpInoutHistory(param);

            //    if (spResult.ReusltType != EResultType.SUCCESS)
            //    {
            //        LogHelper.Logger.Error("Insert" + spResult.ReusltType);
            //        return;
            //    }

            //    IoC.Get<SalesResvMneyRegistView>().txtPosReadyAmt.Text = Comma(IoC.Get<SalesResvMneyRegistView>().txtPosReadyAmt.Text);

            //    DialogHelper.Show("등록하였습니다.");

            //}
            //catch (Exception a)
            //{
            //    //Console.WriteLine(a.Message);
            //    LogHelper.Logger.Error(a.Message);
            //}

        }
    }
}

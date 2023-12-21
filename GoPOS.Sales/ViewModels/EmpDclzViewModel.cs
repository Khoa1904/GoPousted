using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

using Caliburn.Micro;
using GoPOS.Common.Interface.Model;
using GoPOS.Common.ViewModels;
using GoPOS.Helpers;
using GoPOS.Helpers.CommandHelper;
using GoPOS.Models;
using GoPOS.Models.Common;
using GoPOS.Models.Config;
using GoPOS.Services;
using GoPOS.ViewModels.Controls;
using GoShared.Helpers;

namespace GoPOS.ViewModels
{
    public class EmpDclzViewModel : BasePageViewModel
    {        
        private IInfoEmpService _empMService;
        private IEmpInoutHistoryService _empInoutHistoryService;

        
        public string EMP_NO
        {
            get; set;
        } = string.Empty;

        public string EMP_PWD
        {
            get; set;
        } = string.Empty;

        private string emp_name = string.Empty;
        public string EMP_NAME
        {
            get { return emp_name; }
            set
            {
                emp_name = value;
                NotifyOfPropertyChange(nameof(EMP_NAME));

                NotifyOfPropertyChange(nameof(EMP_NO));
                NotifyOfPropertyChange(nameof(EMP_PWD));
            }
        }

        public SalesManagerHeaderControlViewModel SalesMngHaeder { get; }

        public EmpDclzViewModel(IWindowManager windowManager, IEventAggregator eventAggregator, IInfoEmpService empMService, 
            IInfoShopService shopMService, IEmpInoutHistoryService empInoutHistoryService) : base(windowManager, eventAggregator)
        {
            _empMService = empMService;
            _empInoutHistoryService = empInoutHistoryService;
            _eventAggregator.SubscribeOnUIThread(this);

            // Header 컨트롤 초기화
            this.SalesMngHaeder = new SalesManagerHeaderControlViewModel(windowManager, eventAggregator);
        }

        // 출근 버튼
        public bool EmployeeCheckIn(string username, string password)
        {
            var data = this._empMService.TryGet(p => p.USER_ID == username && p.SHOP_CODE ==
                DataLocals.AppConfig.PosInfo.StoreNo);

            if (data != null)
            {
                EMP_NAME = data.EMP_NAME;

                if (MessageBox.Show("입력하신 사원번호, 비밀번호로 출근 등록하시겠습니까?", "출근", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    MST_EMP_INOUT_HISTORY emp_inout_history = new MST_EMP_INOUT_HISTORY()
                    {
                        SHOP_CODE = data.SHOP_CODE,
                        EMP_IO_DT = string.Format("{0:00}", DateTime.Now.Hour),
                        EMP_NO = data.EMP_NO,
                        SALE_DATE = string.Format("{0:0000}{1:00}{2:00}", DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),
                        EMP_IO_FLAG = "0",  // 출근

                        EMP_NAME = data.EMP_NAME,
                        POS_NO = "01",      // 현재 POS번호 01
                        SEND_FLAG = "0",    // 미전송
                        SEND_DT = "",       // 미전송으로 전송일자 없음
                        EMP_IO_REMARK = "출근"
                    };

                    _empInoutHistoryService.Add(emp_inout_history, DataLocals.Employee.USER_ID);

                    MessageBox.Show("입력하신 사원번호, 비밀번호로 출근 등록하였습니다.");

                    EMP_NO = string.Empty;
                    EMP_PWD = string.Empty;
                    EMP_NAME = string.Empty;

                    return true;
                }
            }
            else
            {
                MessageBox.Show("입력하신 사원번호, 비밀번호로 사원 정보를 조회할 수 없습니다.");
            }
            return false;
        }

        // 퇴근 버튼
        public bool EmployeeCheckOut(string username, string password)
        {
            var data = this._empMService.TryGet(p => p.USER_ID == username && p.SHOP_CODE ==
                DataLocals.AppConfig.PosInfo.StoreNo);

            if (data != null)
            {

                EMP_NAME = data.EMP_NAME;

                if (MessageBox.Show("입력하신 사원번호, 비밀번호로 퇴근 등록하시겠습니까?", "퇴근", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    MST_EMP_INOUT_HISTORY emp_inout_history = new MST_EMP_INOUT_HISTORY()
                    {
                        SHOP_CODE = data.SHOP_CODE,
                        EMP_IO_DT = string.Format("{0:00}", DateTime.Now.Hour),
                        EMP_NO = data.EMP_NO,
                        SALE_DATE = string.Format("{0:0000}{1:00}{2:00}", DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),
                        EMP_IO_FLAG = "1",  // 퇴근
                        EMP_NAME = data.EMP_NAME,
                        POS_NO = "01",      // 현재 POS번호 01
                        SEND_FLAG = "0",    // 미전송
                        SEND_DT = "",       // 미전송으로 전송일자 없음
                        EMP_IO_REMARK = "퇴근"
                    };

                    _empInoutHistoryService.Add(emp_inout_history, DataLocals.Employee.USER_ID);

                    MessageBox.Show("입력하신 사원번호, 비밀번호로 퇴근 등록하였습니다.");

                    EMP_NO = string.Empty;
                    EMP_PWD = string.Empty;
                    EMP_NAME = string.Empty;

                    return true;
                }
            }
            else
            {
                MessageBox.Show("입력하신 사원번호, 비밀번호로 사원 정보를 조회할 수 없습니다.");
            }
            return false;
        }


    }
}

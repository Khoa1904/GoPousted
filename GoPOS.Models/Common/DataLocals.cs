using GoPOS.Models.Config;
using GoShared.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using static GoShared.Events.GoPOSEventHandler;

namespace GoPOS.Models.Common
{
    public enum EPOSFlag { MainPOS = '0', SubPos = '1' }
    public enum EKDSSendNoConfig { AllDevice, NoSend }
    public class DataLocals
    {
        public static EKDSSendNoConfig eKDSSendType = EKDSSendNoConfig.NoSend;
        public static IList<MenuConfigInfo> MenuList { get; set; } = new List<MenuConfigInfo>();
        public static MenuConfigInfo TryGetMenu(string menuCode)
        {
            if (MenuList != null && MenuList.Any())
            {
                return MenuList.FirstOrDefault(t => t.MenuCode == menuCode);
            }

            return null;
        }

        public static MST_INFO_EMP Employee
        {
            get => employee; set
            {
                employee = value;
            }
        }
        public static event GeneralEventHandler<MST_INFO_EMP> EmployeeChanged;

        public static event GeneralEventHandler<POS_POST_MANG> KDSDataChanged;

        public static event GeneralEventHandler<POS_POST_MANG> KPrinterDataChanged;
        public static event GeneralEventHandler<MST_INFO_SHOP> ShopInfoChanged;
        public static void RaiseKDSEvent(POS_POST_MANG sendData, bool printKitchen)
        {
            if (KDSDataChanged != null) KDSDataChanged(sendData, new GoShared.Events.EventArgs<POS_POST_MANG>(sendData));
            if (KPrinterDataChanged != null && printKitchen) KPrinterDataChanged(sendData, new GoShared.Events.EventArgs<POS_POST_MANG>(sendData));
        }

        public static void UserLogIn(MST_INFO_EMP emp)
        {
            Employee = emp;
            if (EmployeeChanged != null) EmployeeChanged(emp, new GoShared.Events.EventArgs<MST_INFO_EMP>(Employee));
        }

        public static MST_INFO_SHOP ShopInfo { get; private set; }
        public static void SetShopInfo(MST_INFO_SHOP shopM)
        {
            ShopInfo = shopM;
            if (ShopInfoChanged != null) ShopInfoChanged(ShopInfo, new GoShared.Events.EventArgs<MST_INFO_SHOP>(ShopInfo));
        }

        public static MST_INFO_VAN_POS POSVanConfig { get; set; }

        public static POS_KEY_MANG TokenInfo { get; set; }

        public static AppConfig AppConfig
        {
            get; private set;
        }

        private static POS_STATUS _posStatus;
        private static MST_INFO_EMP employee;

        public static POS_STATUS PosStatus
        {
            get { return _posStatus; }
            set { _posStatus = value; PoStatusChang(); }
        }

        public static List<MEMBER_GRADE> MemberGrades { get; set; }

        #region Bill no
        public static event GeneralEventHandler<POS_STATUS> PoStatusChanged;
        //private static object _lockBillNo = new object();
        //public static string GetNew_BILL_NO()
        //{
        //    lock (_lockBillNo)
        //    {
        //        int billNo = Convert.ToInt32(PosStatus.BILL_NO);
        //        billNo++;

        //        return billNo.ToString("0000");
        //    }
        //}
        public static void PoStatusChang()
        {
            if (PoStatusChanged != null) PoStatusChanged(PosStatus, new GoShared.Events.EventArgs<POS_STATUS>(PosStatus));
        }
        #endregion

        public static void LoadConfigs()
        {
            DataLocals.AppConfig = AppConfig.Load();
        }

        public static MST_EMP_FUNC_KEY EmpFunction { get; private set; }

        public static void SetEmpFunc(MST_EMP_FUNC_KEY empFunction) { EmpFunction = empFunction; }
    }
    public struct FotmatPrintType
    {
        public static string KitchenPrint { get; set; } = "2";
    }
}

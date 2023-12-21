using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoPOS.Models.Common
{
    public enum EMenuType { None,Main,Sub }
    public struct SMenuCode
    {
        public const string MenuShutdown = "MenuShutdown";
        public const string OrderPayCashViewModel = "OrderPayCashViewModel";
        public const string OrderPayRightViewModel = "OrderPayRightViewModel";

        #region ExtMenu
        public const string ClSelngSttusViewModel = "ClSelngSttusViewModel";
        public const string CrnSelngSttusViewModel = "CrnSelngSttusViewModel";
        public const string DscntSelngSttusViewModel = "DscntSelngSttusViewModel";
        public const string ExcclcSttusViewModel = "ExcclcSttusViewModel";
        public const string ExcclcSttusConfirmDetailViewModel = "ExcclcSttusConfirmDetailViewModel";
        public const string ExcclcSttusVoucherDetailViewModel = "ExcclcSttusVoucherDetailViewModel";
        public const string GoodsOrderCanclSttusViewModel = "GoodsOrderCanclSttusViewModel";
        public const string GoodsSelngSttusViewModel = "GoodsSelngSttusViewModel";
        public const string PaymntSelngSttusViewModel = "PaymntSelngSttusViewModel";
        public const string RciptSelngSttusViewModel = "RciptSelngSttusViewModel";
        #endregion
    }
}

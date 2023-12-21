using GoPOS.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoPOS.Payment.Models
{

    public class CompPayChildPayDataEventArgs : EventArgs
    {
        /// <summary>
        /// ViewModelName 
        /// Ex: OrderPayVoucherVIewModel
        /// </summary>
        public string ChildName { get; set; }
        public object ChildData { get; set; }
    }
    public class InvokeModels
    { // Add more when needed
        public const string OrderMain = "OrderPayMainViewModel";
        public const string MemberSearch = "OrderPayMemberSearchViewModel";
        public const string LeftIntro = "OrderPayLeftTRInfoViewModel";
        public const string PayInfo = "PaymentInfoViewModel";
    }
    public class InvokeCommand
    { // Add more when needed
        public const string InitData = "InitData";
        public const string ActiveForm = "ActiveForm";
        public const string SendData = "SendData";
        public const string ActiveItem = "ActiveItem";
    }
    public class ItemPost
    {// Add more when needed
        public const string ActiveItemR = "ActiveItemR";
        public const string ActiveItemM = "ActiveItemM";
        public const string ActiveItemLB = "ActiveItemLB";
    }

    public class MemberData
    {
        public string TEL_NO { get; set; } = string.Empty;
        public string MEMBER_CARD { get; set; } = string.Empty;
        public string MEMBER_NM { get; set; } = string.Empty;
        public string GUBUN { get; set; } = string.Empty;
    }

}
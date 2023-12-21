using GoPOS.Models;
using GoPOS.Models.Custom.OrderMng;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoPOS.Service
{
    public class SelectboxEvent
    {
        public string Type { get; set; }
    }

    public class ReceiptMngListEvent : EventArgs
    {
        /// <summary>
        /// 0: return receipt status
        /// </summary>
        public int EventType { get; set; }
        public RECEIPT_MANAGER_ITEM Item { get; set; }
    }

    public class CalendarEventArgs : EventArgs
    {
        /// <summary>
        /// ExtButton
        /// ...
        /// </summary>
        public string EventType { get; set; }

        /// <summary>
        /// FK Key No
        /// </summary>
        public DateTime Date { get; set; }
    }
    public class CalendarEventArgs2 : EventArgs
    {
        /// <summary>
        /// ExtButton
        /// ...
        /// </summary>
        public string EventType { get; set; }

        /// <summary>
        /// FK Key No
        /// </summary>
        public DateTime Date { get; set; }
    }

    public class SelectPosEventArgs : EventArgs
    {
        /// <summary>
        /// ExtButton
        /// ...
        /// </summary>
        public string EventType { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string PosNo { get; set; }
    }

    public class SelectSEQEventArgs : EventArgs
    {
        /// <summary>
        /// ExtButton
        /// ...
        /// </summary>
        public string EventType { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string SEQNo { get; set; }
    }

    public class SelectCommonCodeArgs : EventArgs
    {
        /// <summary>
        /// ExtButton
        /// ...
        /// </summary>
        public string EventType { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string CommonCode { get; set; }
        public string ComcodeName { get; set; }
    }

    public class MemberInfoPass : EventArgs
    {
        public MEMBER_CLASH memberInfo { get; set; }
    }
    public class MemberInfoSinglePass : EventArgs
    {
        public MEMBER_CLASH memberInfo { get; set; }
    }
    public class GradePass : EventArgs
    {
        public MEMBER_GRADE grade { get; set; }
    }
    public class TableOrder :EventArgs
    {
        public string tableCode { get; set; }
    }
    public class OrderReceived : EventArgs
    {
        public string tableCode { get; set; }
        public List<TRN_PRDT> OrderItems { get; set; }
    }
}
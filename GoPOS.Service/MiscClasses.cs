using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoPOS.Service
{
    /// <summary>
    /// Data from child to main OrderPay
    /// </summary>
    public class OrderPayChildUpdatedEventArgs
    {
        /// <summary>
        /// finish or cancel
        /// </summary>
        public bool Cancelled { get; set; } = true;
        public string ChildName { get; set; }
        public object ReturnData { get; set; }
        public object ChildData { get; set; }
        /// <summary>
        /// MAIN / COMPPAY
        /// </summary>
        public string CallerId { get; set; } = "ODP_MAIN";
    }

    /// <summary>
    /// Data push to child
    /// </summary>
    public class OrderPayChildDoUpdateEventArgs
    {
        public string ChildName { get; set; }
        public object SendData { get; set; }
    }

}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoPOS.SellingStatus.ViewModels
{
    public class SellingStatusMainEventArgs : EventArgs
    {
        /// <summary>
        /// ExtButton
        /// ...
        /// </summary>
        public string EventType { get; set; }

        /// <summary>
        /// FK Key No
        /// </summary>
        public object EventData { get; set; }
        public object EventData2 { get; set; }
        public object EventFlag { get; set; }
    }
}

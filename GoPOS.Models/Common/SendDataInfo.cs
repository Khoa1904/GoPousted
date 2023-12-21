using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoPOS.Models.Common
{
    public class SendDataInfo
    {
        public POS_POST_MANG Data { get; set; }
        public int SendCount { get; set; } = 0;
        public DateTime LastSend { get; set; } = DateTime.MinValue;

        public SendDataInfo(POS_POST_MANG data)
        {
            Data = data;
            SendCount = 0;
            LastSend = DateTime.MinValue;
        }


        public void Caculater()
        {
            SendCount += 1;
            LastSend = DateTime.UtcNow;
        }
    }

}

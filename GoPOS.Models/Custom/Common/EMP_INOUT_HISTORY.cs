using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoPOS.Models
{
    public class EMP_INOUT_HISTORY
    {
        public EMP_INOUT_HISTORY() : this(string.Empty, string.Empty, string.Empty)
        { 
        }

        public EMP_INOUT_HISTORY(string shop_code, string emp_io_dt, string emp_no)
        {
            this.SHOP_CODE = shop_code;
            this.EMP_IO_DT = emp_io_dt;
            this.EMP_NO = emp_no;
        }

        //eih.SHOP_CODE, eih.EMP_IO_DT, eih.EMP_NO, eih.SALE_DATE, eih.EMP_IO_FLAG, 
        //eih.EMP_NAME, eih.POS_NO, eih.SEND_FLAG, eih.SEND_DT, eih.EMP_IO_REMARK
        public string SHOP_CODE { get; set; } = string.Empty;
        public string EMP_IO_DT { get; set; } = string.Empty;
        public string EMP_NO { get; set; } = string.Empty;
        public string SALE_DATE { get; set; } = string.Empty;
        public string EMP_IO_FLAG { get; set; } = string.Empty;

        public string EMP_NAME { get; set; } = string.Empty;     
        public string POS_NO { get; set; } = string.Empty; 
        public string SEND_FLAG { get; set; } = string.Empty;
        public string SEND_DT { get; set; } = string.Empty;
        public string EMP_IO_REMARK { get; set; } = string.Empty;
    }   
}

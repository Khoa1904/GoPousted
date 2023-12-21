using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoPOS.Models
{
    public class SALE_BILL_NO
    {
        /// <summary>
        /// SALE_BILL_NO 채번 전용 모델
        /// SP_SALE_GET_BILL_NO

        /// </summary>
        public SALE_BILL_NO()
        {
        }

        public SALE_BILL_NO(string shop_code, string sale_date)
        {
            this.SHOP_CODE = shop_code;
            this.SALE_DATE = sale_date;
        }

        public string SHOP_CODE { get; set; } = string.Empty;
        public string SALE_DATE { get; set; } = string.Empty;
        public string R_BILL_NO { get; set; } = string.Empty;        
    }
}

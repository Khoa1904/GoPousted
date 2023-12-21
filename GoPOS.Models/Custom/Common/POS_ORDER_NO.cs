using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoPOS.Models
{
    public class POS_ORDER_NO
    {
        /// <summary>
        /// POS_ORDER_NO 채번 전용 모델

        /// </summary>
        public POS_ORDER_NO()
        {
        }

        public POS_ORDER_NO(string shop_code, string sale_date)
        {
            this.SHOP_CODE = shop_code;
            this.SALE_DATE = sale_date;
        }

        public string SHOP_CODE { get; set; } = string.Empty;
        public string SALE_DATE { get; set; } = string.Empty;
        public string PO_ORDER_NO { get; set; } = string.Empty;        
    }
}

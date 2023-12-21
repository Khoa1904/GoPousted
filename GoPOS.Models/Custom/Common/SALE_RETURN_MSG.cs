using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoPOS.Models
{
    public class SALE_RETURN_MSG
    {
        /// <summary>
        /// 매출TR-반품사유

        /// </summary>
        public SALE_RETURN_MSG()
        {
            /*
            SHOP_CODE       VARCHAR(6) NOT NULL,
            SALE_DATE       VARCHAR(8) NOT NULL,
            POS_NO          VARCHAR(2) NOT NULL,
            BILL_NO         VARCHAR(4) NOT NULL,
            MSG_CODE        VARCHAR(100) NOT NULL,
            MSG_NAME        VARCHAR(1100) NOT NULL,
            INSERT_DT       VARCHAR(14) NOT NULL,
            EMP_NO          VARCHAR(4) NOT NULL

            public string SHOP_CODE           { get; set; } = string.Empty;            
            public string SALE_DATE           { get; set; } = string.Empty;            
            public string POS_NO              { get; set; } = string.Empty;            
            public string BILL_NO             { get; set; } = string.Empty;            
            public string MSG_CODE            { get; set; } = string.Empty;            
            public string MSG_NAME            { get; set; } = string.Empty;            
            public string INSERT_DT           { get; set; } = string.Empty;            
            public string EMP_NO              { get; set; } = string.Empty;            
            */
        }

        public SALE_RETURN_MSG(string shop_code, string prd_code, string sale_date ,string pos_no, string  bill_no, string line_no, string seq_no)                                               
        {                                                   
            this.SHOP_CODE = shop_code;                     
            this.SALE_DATE = sale_date;
            this.POS_NO    = pos_no;
            this.BILL_NO   = bill_no;
        }

        public string SHOP_CODE { get; set; } = string.Empty;
        public string SALE_DATE { get; set; } = string.Empty;
        public string POS_NO { get; set; } = string.Empty;
        public string BILL_NO { get; set; } = string.Empty;
        public string MSG_CODE { get; set; } = string.Empty;
        public string MSG_NAME { get; set; } = string.Empty;
        public string INSERT_DT { get; set; } = string.Empty;
        public string EMP_NO { get; set; } = string.Empty;
    }
}

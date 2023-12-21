using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoPOS.Models
{
    public class PRINT_DATA
    {
        /// <summary>
        /// 매장주방출력물관리

        /// </summary>
        public PRINT_DATA()
        {
            /*
            CREATE TABLE PRINT_DATA (
            SHOP_CODE      VARCHAR(6) NOT NULL,
            SALE_DATE      VARCHAR(8) NOT NULL,
            PRINT_SEQ      NUMERIC(5,0) NOT NULL,
            ORDER_NO       VARCHAR(4),
            PRT_NO         VARCHAR(2) NOT NULL,
            CONTENTS       VARCHAR(4000),
            PRINT_YN       VARCHAR(1) NOT NULL,
            INSERT_DT      VARCHAR(14) NOT NULL,
            PRT_DT         VARCHAR(14) NOT NULL,
            ORDER_PRT_SEQ  VARCHAR(4),
            ORDER_SEQ_NO   VARCHAR(4)
            );

            public string SHOP_CODE      { get; set; } = string.Empty;
            public string SALE_DATE      { get; set; } = string.Empty;
            public string PRINT_SEQ      { get; set; } = string.Empty;
            public string ORDER_NO       { get; set; } = string.Empty;
            public string PRT_NO         { get; set; } = string.Empty;
            public string CONTENTS       { get; set; } = string.Empty;
            public string PRINT_YN       { get; set; } = string.Empty;
            public string INSERT_DT      { get; set; } = string.Empty;
            public string PRT_DT         { get; set; } = string.Empty;
            public string ORDER_PRT_SEQ  { get; set; } = string.Empty;
            public string ORDER_SEQ_NO   { get; set; } = string.Empty;

            */
        }

        public PRINT_DATA(string shop_code, string print_seq, string order_no, string prt_no)
        {
            this.SHOP_CODE = shop_code;
            this.PRINT_SEQ = print_seq;
            this.ORDER_NO  = order_no;
            this.PRT_NO    = prt_no;
        }

        public string SHOP_CODE { get; set; } = string.Empty;
        public string SALE_DATE { get; set; } = string.Empty;
        public string PRINT_SEQ { get; set; } = string.Empty;
        public string ORDER_NO { get; set; } = string.Empty;
        public string PRT_NO { get; set; } = string.Empty;
        public string CONTENTS { get; set; } = string.Empty;
        public string PRINT_YN { get; set; } = string.Empty;
        public string INSERT_DT { get; set; } = string.Empty;
        public string PRT_DT { get; set; } = string.Empty;
        public string ORDER_PRT_SEQ { get; set; } = string.Empty;
        public string ORDER_SEQ_NO { get; set; } = string.Empty;
    }
}

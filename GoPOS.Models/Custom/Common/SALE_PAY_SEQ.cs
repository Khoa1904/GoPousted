using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoPOS.Models
{
    public class SALE_PAY_SEQ
    {
        /// <summary>
        /// 매출TR-결제순서

        /// </summary>
        public SALE_PAY_SEQ()
        {
            /*
            SHOP_CODE      VARCHAR(6) NOT NULL,
            SALE_DATE      VARCHAR(8) NOT NULL,
            POS_NO         VARCHAR(2) NOT NULL,
            BILL_NO        VARCHAR(4) NOT NULL,
            PAY_SEQ_NO     NUMERIC(2,0) NOT NULL,
            REGI_SEQ       VARCHAR(2) NOT NULL,
            SALE_YN        VARCHAR(1) DEFAULT 'Y' NOT NULL,
            PAY_TYPE_FLAG  VARCHAR(2) NOT NULL,
            PAY_AMT        NUMERIC(12,2) DEFAULT 0 NOT NULL,
            LINE_NO        VARCHAR(4) NOT NULL,
            INSERT_DT      VARCHAR(14) NOT NULL,
            UPDATE_DT      VARCHAR(14),
            EMP_NO         VARCHAR(4) NOT NULL,
            JCD_DC_AMT     NUMERIC(8,0),
            JCD_USE_POINT  NUMERIC(8,0)

            public string SHOP_CODE           { get; set; } = string.Empty;
            public string SALE_DATE           { get; set; } = string.Empty;
            public string POS_NO              { get; set; } = string.Empty;
            public string BILL_NO             { get; set; } = string.Empty;
            public string PAY_SEQ_NO          { get; set; } = string.Empty;
            public string REGI_SEQ            { get; set; } = string.Empty;
            public string SALE_YN             { get; set; } = string.Empty;
            public string PAY_TYPE_FLAG       { get; set; } = string.Empty;
            public string PAY_AMT             { get; set; } = string.Empty;
            public string LINE_NO             { get; set; } = string.Empty;
            public string INSERT_DT           { get; set; } = string.Empty;
            public string UPDATE_DT           { get; set; } = string.Empty;
            public string EMP_NO              { get; set; } = string.Empty;
            public string JCD_DC_AMT          { get; set; } = string.Empty;
            public string JCD_USE_POINT       { get; set; } = string.Empty;
            */
        }

        public SALE_PAY_SEQ(string shop_code, string sale_date, string pos_no, string bill_no, string line_no)
        {
            this.SHOP_CODE = shop_code;
            this.SALE_DATE = sale_date;
            this.POS_NO = pos_no;
            this.BILL_NO = bill_no;
            this.LINE_NO = line_no;
        }

        public string SHOP_CODE { get; set; } = string.Empty;
        public string SALE_DATE { get; set; } = string.Empty;
        public string POS_NO { get; set; } = string.Empty;
        public string BILL_NO { get; set; } = string.Empty;
        public string PAY_SEQ_NO { get; set; } = string.Empty;
        public string REGI_SEQ { get; set; } = string.Empty;
        public string SALE_YN { get; set; } = string.Empty;
        public string PAY_TYPE_FLAG { get; set; } = string.Empty;
        public string PAY_AMT { get; set; } = "0";
        public string LINE_NO { get; set; } = string.Empty;
        public string INSERT_DT { get; set; } = string.Empty;
        public string UPDATE_DT { get; set; } = string.Empty;
        public string EMP_NO { get; set; } = string.Empty;
        public string JCD_DC_AMT { get; set; } = "0";
        public string JCD_USE_POINT { get; set; } = "0";
    }
}

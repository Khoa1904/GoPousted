using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


// 주문 결제 순서 테이블 모델

namespace GoPOS.Models
{
    public class POS_ORD_SEQ
    {
        public POS_ORD_SEQ()
        {
            /*
            SHOP_CODE      VARCHAR(6) NOT NULL,
    SALE_DATE      VARCHAR(8) NOT NULL,
    ORDER_NO       VARCHAR(4) NOT NULL,
    PAY_SEQ_NO     NUMERIC(2,0) NOT NULL,
    POS_NO         VARCHAR(2) NOT NULL,
    SALE_YN        VARCHAR(1) DEFAULT 'Y' NOT NULL,
    PAY_TYPE_FLAG  VARCHAR(2) NOT NULL,
    PAY_AMT        NUMERIC(12,2) DEFAULT 0 NOT NULL,
    LINE_NO        VARCHAR(4) NOT NULL,
    INSERT_DT      VARCHAR(14) NOT NULL,
    UPDATE_DT      VARCHAR(14),
    EMP_NO         VARCHAR(4) NOT NULL,
    JCD_DC_AMT     NUMERIC(8,0),
    JCD_USE_POINT  NUMERIC(8,0)
            */
        }

        public POS_ORD_SEQ(string shop_code, string sale_date, string order_no, string line_no, string pay_seq_no)
        {
            this.SHOP_CODE = shop_code;
            this.SALE_DATE = sale_date;
            this.ORDER_NO = order_no;
            this.PAY_SEQ_NO = pay_seq_no;
        }

        public string SHOP_CODE { get; set; } = string.Empty; //  VARCHAR(6) NOT NULL,
        public string SALE_DATE { get; set; } = string.Empty; //  VARCHAR(8) NOT NULL,
        public string ORDER_NO { get; set; } = string.Empty; //  VARCHAR(4) NOT NULL,
        public string PAY_SEQ_NO { get; set; } = string.Empty; //  NUMERIC(2,0) NOT NULL,
        public string POS_NO { get; set; } = string.Empty; //  VARCHAR(2) NOT NULL,
        public string SALE_YN { get; set; } = "Y"; //  VARCHAR(1) DEFAULT 'Y' NOT NULL,
        public string PAY_TYPE_FLAG { get; set; } = string.Empty; //  VARCHAR(2) NOT NULL,
        public decimal PAY_AMT { get; set; } = 0; //  NUMERIC(12,2) DEFAULT 0 NOT NULL,
        public string LINE_NO { get; set; } = string.Empty; //  VARCHAR(4) NOT NULL,
        public string INSERT_DT { get; set; } = string.Empty; //  VARCHAR(14) NOT NULL,
        public string UPDATE_DT { get; set; } = string.Empty; //  VARCHAR(14),
        public string EMP_NO { get; set; } = string.Empty; //  VARCHAR(4) NOT NULL,
        public decimal JCD_DC_AMT { get; set; } = 0; //     NUMERIC(8,0),
        public decimal JCD_USE_POINT { get; set; } = 0;// NUMERIC(8,0)
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


// 오더 디테일 테이블 모델

namespace GoPOS.Models
{
    public class POS_ORD_CASH
    {
        public POS_ORD_CASH()
        {
            /*
            SHOP_CODE      VARCHAR(6) NOT NULL,
            SALE_DATE      VARCHAR(8) NOT NULL,
            ORDER_NO       VARCHAR(4) NOT NULL,
            LINE_NO        VARCHAR(4) NOT NULL,
            SEQ_NO         VARCHAR(2) NOT NULL,
            POS_NO         VARCHAR(2) NOT NULL,
            SALE_YN        VARCHAR(1) DEFAULT 'Y' NOT NULL,
            CORNER_CODE    VARCHAR(2) DEFAULT '00' NOT NULL,
            CASH_AMT       NUMERIC(12,2) DEFAULT 0 NOT NULL,
            SVC_TIP_AMT    NUMERIC(12,2) DEFAULT 0 NOT NULL,
            VAT_AMT        NUMERIC(12,2) DEFAULT 0 NOT NULL,
            APPR_LOG_NO    VARCHAR(5),
            INSERT_DT      VARCHAR(14) NOT NULL,
            EMP_NO         VARCHAR(4) NOT NULL,
            EX_CODE        VARCHAR(2),
            EX_KRW         NUMERIC(12,2) DEFAULT 0 NOT NULL,
            EX_EXP_AMT     NUMERIC(12,2) DEFAULT 0 NOT NULL,
            EX_IN_AMT      NUMERIC(12,2) DEFAULT 0 NOT NULL,
            EX_RET_AMT     NUMERIC(12,2) DEFAULT 0 NOT NULL,
            KR_RET_AMT     NUMERIC(12,2) DEFAULT 0 NOT NULL,
            EX_PAY_AMT     NUMERIC(12,2) DEFAULT 0 NOT NULL,
            KR_PAY_AMT     NUMERIC(12,2) DEFAULT 0 NOT NULL,
            KR_ETC_AMT     NUMERIC(12,2) DEFAULT 0 NOT NULL,
            PRE_PAY_FLAG   VARCHAR(1),
            JCD_SAVE_FLAG  VARCHAR(1),
            NO_VAT_AMT     NUMERIC(12,2) DEFAULT 0 NOT NULL,
            DEPOSIT_AMT    NUMERIC(12,2) DEFAULT 0 NOT NULL
            */
        }

        public POS_ORD_CASH(string shop_code, string sale_date, string order_no, string line_no, string seq_no)
        {
            this.SHOP_CODE = shop_code;
            this.SALE_DATE = sale_date;
            this.ORDER_NO = order_no;
            this.LINE_NO = line_no;
            this.SEQ_NO = seq_no;
        }

        public string SHOP_CODE { get; set; } = string.Empty; //  VARCHAR(6) NOT NULL,
        public string SALE_DATE { get; set; } = string.Empty; //      VARCHAR(8) NOT NULL,
        public string ORDER_NO { get; set; } = string.Empty; //  VARCHAR(4) NOT NULL,
        public string LINE_NO { get; set; } = string.Empty; //  VARCHAR(4) NOT NULL,
        public string SEQ_NO { get; set; } = string.Empty; //  VARCHAR(2) NOT NULL,
        public string POS_NO { get; set; } = string.Empty; //  VARCHAR(2) NOT NULL,
        public string SALE_YN { get; set; } = "U"; //  VARCHAR(1) DEFAULT 'Y' NOT NULL,
        public string CORNER_CODE { get; set; } = "00"; //  VARCHAR(2) DEFAULT '00' NOT NULL,
        public string CASH_AMT { get; set; } = "0"; //  NUMERIC(12,2) DEFAULT 0 NOT NULL,
        public string SVC_TIP_AMT { get; set; } = string.Empty; //  NUMERIC(12,2) DEFAULT 0 NOT NULL,
        public string VAT_AMT { get; set; } = "0"; //  NUMERIC(12,2) DEFAULT 0 NOT NULL,
        public string APPR_LOG_NO { get; set; } = string.Empty; //  VARCHAR(5),
        public string INSERT_DT { get; set; } = string.Empty; //  VARCHAR(14) NOT NULL,
        public string EMP_NO { get; set; } = string.Empty; //      VARCHAR(4) NOT NULL,
        public string EX_CODE { get; set; } = string.Empty; //      VARCHAR(2),
        public string EX_KRW { get; set; } = string.Empty; // NUMERIC(12,2) DEFAULT 0 NOT NULL,
        public string EX_EXP_AMT { get; set; } = "0"; //     NUMERIC(12,2) DEFAULT 0 NOT NULL,
        public string EX_IN_AMT { get; set; } = "0"; //     NUMERIC(12,2) DEFAULT 0 NOT NULL,
        public string EX_RET_AMT { get; set; } = "0"; //    NUMERIC(12,2) DEFAULT 0 NOT NULL,
        public string KR_RET_AMT { get; set; } = "0"; //  NUMERIC(12,2) DEFAULT 0 NOT NULL,
        public string EX_PAY_AMT { get; set; } = "0"; //  NUMERIC(12,2) DEFAULT 0 NOT NULL,
        public string KR_PAY_AMT { get; set; } = "0"; //  NUMERIC(12,2) DEFAULT 0 NOT NULL,
        public string KR_ETC_AMT { get; set; } = "0"; //  NUMERIC(12,2) DEFAULT 0 NOT NULL,
        public string PRE_PAY_FLAG { get; set; } = string.Empty; //  VARCHAR(1),
        public string JCD_SAVE_FLAG { get; set; } = string.Empty; // VARCHAR(1),
        public string NO_VAT_AMT { get; set; } = "0"; // NUMERIC(12,2) DEFAULT 0 NOT NULL,
        public string DEPOSIT_AMT { get; set; } = "0"; //  NUMERIC(12,2) DEFAULT 0 NOT NULL
    }
}
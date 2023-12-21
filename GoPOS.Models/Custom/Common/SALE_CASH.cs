using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoPOS.Models
{
    public class SALE_CASH
    {
        /// <summary>
        /// 매출TR-현금&영수증

        /// </summary>
        public SALE_CASH()
        {
            /*
            SHOP_CODE       VARCHAR(6) NOT NULL,
            SALE_DATE       VARCHAR(8) NOT NULL,
            POS_NO          VARCHAR(2) NOT NULL,
            BILL_NO         VARCHAR(4) NOT NULL,
            LINE_NO         VARCHAR(4) NOT NULL,
            SEQ_NO          VARCHAR(2) NOT NULL,
            REGI_SEQ        VARCHAR(2) DEFAULT '01' NOT NULL,
            SALE_YN         VARCHAR(1) DEFAULT 'Y' NOT NULL,
            VAN_CODE        VARCHAR(2) NOT NULL,
            CORNER_CODE     VARCHAR(2) DEFAULT '00' NOT NULL,
            CASH_AMT        NUMERIC(12,2) DEFAULT 0 NOT NULL,
            APPR_AMT        NUMERIC(12,2) DEFAULT 0 NOT NULL,
            SVC_TIP_AMT     NUMERIC(12,2) DEFAULT 0 NOT NULL,
            VAT_AMT         NUMERIC(12,2) DEFAULT 0 NOT NULL,
            APPR_PROC_FLAG  VARCHAR(1) NOT NULL,
            APPR_IDT_TYPE   VARCHAR(1) NOT NULL,
            APPR_IDT_FLAG   VARCHAR(1) NOT NULL,
            CARD_IN_FLAG    VARCHAR(1) NOT NULL,
            APPR_IDT_NO     VARCHAR(20) DEFAULT '*' NOT NULL,
            APPR_DATE       VARCHAR(8) DEFAULT '*' NOT NULL,
            APPR_TIME       VARCHAR(6) DEFAULT '*' NOT NULL,
            APPR_NO         VARCHAR(20) DEFAULT '*' NOT NULL,
            APPR_MSG        VARCHAR(200),
            VAN_TERM_NO     VARCHAR(16),
            VAN_SLIP_NO     VARCHAR(12),
            ORG_APPR_DATE   VARCHAR(8),
            ORG_APPR_NO     VARCHAR(20),
            APPR_LOG_NO     VARCHAR(5),
            INSERT_DT       VARCHAR(14) NOT NULL,
            EMP_NO          VARCHAR(4) NOT NULL,
            EX_CODE         VARCHAR(2),
            EX_KRW          NUMERIC(12,2) DEFAULT 0 NOT NULL,
            EX_EXP_AMT      NUMERIC(12,2) DEFAULT 0 NOT NULL,
            EX_IN_AMT       NUMERIC(12,2) DEFAULT 0 NOT NULL,
            EX_RET_AMT      NUMERIC(12,2) DEFAULT 0 NOT NULL,
            KR_RET_AMT      NUMERIC(12,2) DEFAULT 0 NOT NULL,
            EX_PAY_AMT      NUMERIC(12,2) DEFAULT 0 NOT NULL,
            KR_PAY_AMT      NUMERIC(12,2) DEFAULT 0 NOT NULL,
            KR_ETC_AMT      NUMERIC(12,2) DEFAULT 0 NOT NULL,
            PRE_PAY_FLAG    VARCHAR(1),
            JCD_SAVE_FLAG   VARCHAR(1),
            CNMK_CODE       VARCHAR(20),
            PRINT_MSG       VARCHAR(1024),
            NO_VAT_AMT      NUMERIC(12,2) DEFAULT 0 NOT NULL,
            VAN_SER_NO      VARCHAR(20),
            DEPOSIT_AMT     NUMERIC(12,2) DEFAULT 0 NOT NULL
            );

            public string SHOP_CODE           { get; set; } = string.Empty;            
            public string SALE_DATE           { get; set; } = string.Empty; 
            public string POS_NO              { get; set; } = string.Empty;
            public string BILL_NO             { get; set; } = string.Empty;
            public string LINE_NO             { get; set; } = string.Empty;
            public string SEQ_NO              { get; set; } = string.Empty;
            public string REGI_SEQ            { get; set; } = string.Empty;
            public string SALE_YN             { get; set; } = string.Empty;
            public string VAN_CODE            { get; set; } = string.Empty;
            public string CORNER_CODE         { get; set; } = string.Empty;
            public string CASH_AMT            { get; set; } = string.Empty;
            public string APPR_AMT            { get; set; } = string.Empty;
            public string SVC_TIP_AMT         { get; set; } = string.Empty;
            public string VAT_AMT             { get; set; } = string.Empty;
            public string APPR_PROC_FLAG      { get; set; } = string.Empty;
            public string APPR_IDT_TYPE       { get; set; } = string.Empty;
            public string APPR_IDT_FLAG       { get; set; } = string.Empty;
            public string CARD_IN_FLAG        { get; set; } = string.Empty;
            public string APPR_IDT_NO         { get; set; } = string.Empty;
            public string APPR_DATE           { get; set; } = string.Empty;
            public string APPR_TIME           { get; set; } = string.Empty;
            public string APPR_NO             { get; set; } = string.Empty;
            public string APPR_MSG            { get; set; } = string.Empty;
            public string VAN_TERM_NO         { get; set; } = string.Empty;
            public string VAN_SLIP_NO         { get; set; } = string.Empty;
            public string ORG_APPR_DATE       { get; set; } = string.Empty;
            public string ORG_APPR_NO         { get; set; } = string.Empty;
            public string APPR_LOG_NO         { get; set; } = string.Empty;
            public string INSERT_DT           { get; set; } = string.Empty;
            public string EMP_NO              { get; set; } = string.Empty;
            public string EX_CODE             { get; set; } = string.Empty;
            public string EX_KRW              { get; set; } = string.Empty;
            public string EX_EXP_AMT          { get; set; } = string.Empty;
            public string EX_IN_AMT           { get; set; } = string.Empty;
            public string EX_RET_AMT          { get; set; } = string.Empty;
            public string KR_RET_AMT          { get; set; } = string.Empty;
            public string EX_PAY_AMT          { get; set; } = string.Empty;
            public string KR_PAY_AMT          { get; set; } = string.Empty;
            public string KR_ETC_AMT          { get; set; } = string.Empty;
            public string PRE_PAY_FLAG        { get; set; } = string.Empty;
            public string JCD_SAVE_FLAG       { get; set; } = string.Empty;
            public string CNMK_CODE           { get; set; } = string.Empty;
            public string PRINT_MSG           { get; set; } = string.Empty;
            public string NO_VAT_AMT          { get; set; } = string.Empty;
            public string VAN_SER_NO          { get; set; } = string.Empty;
            public string DEPOSIT_AMT         { get; set; } = string.Empty;
            */
        }

        public SALE_CASH(string shop_code, string prd_code, string sale_date ,string pos_no, string  bill_no, string line_no, string seq_no)                                               
        {                                                   
            this.SHOP_CODE = shop_code;                     
            this.SALE_DATE = sale_date;
            this.POS_NO    = pos_no;
            this.BILL_NO   = bill_no;
            this.LINE_NO   = line_no;
            this.SEQ_NO    = seq_no;

        }

        public string SHOP_CODE { get; set; } = string.Empty;
        public string SALE_DATE { get; set; } = string.Empty;
        public string POS_NO { get; set; } = string.Empty;
        public string BILL_NO { get; set; } = string.Empty;
        public string LINE_NO { get; set; } = string.Empty;
        public string SEQ_NO { get; set; } = string.Empty;
        public string REGI_SEQ { get; set; } = string.Empty;
        public string SALE_YN { get; set; } = string.Empty;
        public string VAN_CODE { get; set; } = string.Empty;
        public string CORNER_CODE { get; set; } = string.Empty;
        public string CASH_AMT { get; set; } = "0";
        public string APPR_AMT { get; set; } = "0";
        public string SVC_TIP_AMT { get; set; } = "0";
        public string VAT_AMT { get; set; } = "0";
        public string APPR_PROC_FLAG { get; set; } = string.Empty;
        public string APPR_IDT_TYPE { get; set; } = string.Empty;
        public string APPR_IDT_FLAG { get; set; } = string.Empty;
        public string CARD_IN_FLAG { get; set; } = string.Empty;
        public string APPR_IDT_NO { get; set; } = string.Empty;
        public string APPR_DATE { get; set; } = string.Empty;
        public string APPR_TIME { get; set; } = string.Empty;
        public string APPR_NO { get; set; } = string.Empty;
        public string APPR_MSG { get; set; } = string.Empty;
        public string VAN_TERM_NO { get; set; } = string.Empty;
        public string VAN_SLIP_NO { get; set; } = string.Empty;
        public string ORG_APPR_DATE { get; set; } = string.Empty;
        public string ORG_APPR_NO { get; set; } = string.Empty;
        public string APPR_LOG_NO { get; set; } = string.Empty;
        public string INSERT_DT { get; set; } = string.Empty;
        public string EMP_NO { get; set; } = string.Empty;
        public string EX_CODE { get; set; } = string.Empty;
        public string EX_KRW { get; set; } = string.Empty;
        public string EX_EXP_AMT { get; set; } = "0";
        public string EX_IN_AMT { get; set; } = "0";
        public string EX_PAY_AMT { get; set; } = "0";
        public string EX_RET_AMT { get; set; } = "0";
        public string KR_RET_AMT { get; set; } = "0";
        public string KR_PAY_AMT { get; set; } = "0";
        public string KR_ETC_AMT { get; set; } = "0";
        public string PRE_PAY_FLAG { get; set; } = string.Empty;
        public string JCD_SAVE_FLAG { get; set; } = string.Empty;
        public string CNMK_CODE { get; set; } = string.Empty;
        public string PRINT_MSG { get; set; } = string.Empty;
        public string NO_VAT_AMT { get; set; } = "0";
        public string VAN_SER_NO { get; set; } = string.Empty;
        public string DEPOSIT_AMT { get; set; } = "0";
    }
}

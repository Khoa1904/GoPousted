using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoPOS.Models
{
    public class SALE_CARD
    {
        /// <summary>
        /// 매출TR-신용카드

        /// </summary>
        public SALE_CARD()
        {
            /*
            SHOP_CODE          VARCHAR(6) NOT NULL,
            SALE_DATE          VARCHAR(8) NOT NULL,
            POS_NO             VARCHAR(2) NOT NULL,
            BILL_NO            VARCHAR(4) NOT NULL,
            LINE_NO            VARCHAR(4) NOT NULL,
            SEQ_NO             VARCHAR(2) NOT NULL,
            REGI_SEQ           VARCHAR(2) DEFAULT '01' NOT NULL,
            SALE_YN            VARCHAR(1) DEFAULT 'Y' NOT NULL,
            VAN_CODE           VARCHAR(2) NOT NULL,
            CORNER_CODE        VARCHAR(2) DEFAULT '00' NOT NULL,
            APPR_PROC_FLAG     VARCHAR(1) NOT NULL,
            CRD_CARD_NO        VARCHAR(20) DEFAULT '*' NOT NULL,
            APPR_REQ_AMT       NUMERIC(12,2) DEFAULT 0 NOT NULL,
            SVC_TIP_AMT        NUMERIC(12,2) DEFAULT 0 NOT NULL,
            VAT_AMT            NUMERIC(12,2) DEFAULT 0 NOT NULL,
            INST_MM_FLAG       VARCHAR(1) DEFAULT '0' NOT NULL,
            INST_MM_CNT        NUMERIC(2,0) DEFAULT 0 NOT NULL,
            VALID_TERM         VARCHAR(6) DEFAULT '*' NOT NULL,
            SIGN_PAD_YN        VARCHAR(1) NOT NULL,
            CARD_IN_FLAG       VARCHAR(1) NOT NULL,
            APPR_DATE          VARCHAR(8) DEFAULT '*' NOT NULL,
            APPR_TIME          VARCHAR(6) DEFAULT '*' NOT NULL,
            APPR_NO            VARCHAR(20) DEFAULT '*' NOT NULL,
            CRDCP_CODE         VARCHAR(3) DEFAULT '*' NOT NULL,
            ISS_CRDCP_CODE     VARCHAR(10),
            ISS_CRDCP_NAME     VARCHAR(40),
            PUR_CRDCP_CODE     VARCHAR(4),
            PUR_CRDCP_NAME     VARCHAR(40),
            APPR_AMT           NUMERIC(12,2) DEFAULT 0 NOT NULL,
            APPR_DC_AMT        NUMERIC(12,2) DEFAULT 0 NOT NULL,
            APPR_MSG           VARCHAR(200),
            VAN_TERM_NO        VARCHAR(16),
            VAN_SLIP_NO        VARCHAR(12),
            CRDCP_TERM_NO      VARCHAR(15),
            ORG_APPR_DATE      VARCHAR(8),
            ORG_APPR_NO        VARCHAR(20),
            APPR_LOG_NO        VARCHAR(5),
            INSERT_DT          VARCHAR(14) NOT NULL,
            EMP_NO             VARCHAR(4) NOT NULL,
            PRE_PAY_FLAG       VARCHAR(1),
            LYT_APPR_FLAG      VARCHAR(1),
            LYT_APPR_INFO      VARCHAR(5000),
            SPECIAL_CARD_FLAG  VARCHAR(2) DEFAULT '0',
            CSN_BANK_NO        VARCHAR(39),
            JCD_SAVE_FLAG      VARCHAR(1),
            CNMK_CODE          VARCHAR(20),
            PRINT_MSG          VARCHAR(1024),
            NO_VAT_AMT         NUMERIC(12,2) DEFAULT 0 NOT NULL,
            TRAN_TYPE_FLAG     VARCHAR(2) DEFAULT '0' NOT NULL,
            TRAN_DC_NAME       VARCHAR(200),
            TRAN_DC_AMT        NUMERIC(12,2) DEFAULT 0 NOT NULL,
            VAN_SER_NO         VARCHAR(20),
            SLIP_TYPE_FLAG     VARCHAR(1),
            TRAN_UNIQUE_NO     VARCHAR(50),
            TRAN_ORDER_NO      VARCHAR(100),
            DCC_EX_RATE_CODE   VARCHAR(1),
            DCC_RCV_TEXT       VARCHAR(100),
            DEPOSIT_AMT        NUMERIC(12,2) DEFAULT 0 NOT NULL
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
            public string APPR_PROC_FLAG      { get; set; } = string.Empty;
            public string CRD_CARD_NO         { get; set; } = string.Empty;
            public string APPR_REQ_AMT        { get; set; } = string.Empty;
            public string SVC_TIP_AMT         { get; set; } = string.Empty;
            public string VAT_AMT             { get; set; } = string.Empty;
            public string INST_MM_FLAG        { get; set; } = string.Empty;
            public string INST_MM_CNT         { get; set; } = string.Empty;
            public string VALID_TERM          { get; set; } = string.Empty;
            public string SIGN_PAD_YN         { get; set; } = string.Empty;
            public string CARD_IN_FLAG        { get; set; } = string.Empty;
            public string APPR_DATE           { get; set; } = string.Empty;
            public string APPR_TIME           { get; set; } = string.Empty;
            public string APPR_NO             { get; set; } = string.Empty;
            public string CRDCP_CODE          { get; set; } = string.Empty;
            public string ISS_CRDCP_CODE      { get; set; } = string.Empty;
            public string ISS_CRDCP_NAME      { get; set; } = string.Empty;
            public string PUR_CRDCP_CODE      { get; set; } = string.Empty;
            public string PUR_CRDCP_NAME      { get; set; } = string.Empty;
            public string APPR_AMT            { get; set; } = string.Empty;
            public string APPR_DC_AMT         { get; set; } = string.Empty;
            public string APPR_MSG            { get; set; } = string.Empty;
            public string VAN_TERM_NO         { get; set; } = string.Empty;
            public string VAN_SLIP_NO         { get; set; } = string.Empty;
            public string CRDCP_TERM_NO       { get; set; } = string.Empty;
            public string ORG_APPR_DATE       { get; set; } = string.Empty;
            public string ORG_APPR_NO         { get; set; } = string.Empty;
            public string APPR_LOG_NO         { get; set; } = string.Empty;
            public string INSERT_DT           { get; set; } = string.Empty;
            public string EMP_NO              { get; set; } = string.Empty;
            public string PRE_PAY_FLAG        { get; set; } = string.Empty;
            public string LYT_APPR_FLAG       { get; set; } = string.Empty;
            public string LYT_APPR_INFO       { get; set; } = string.Empty;
            public string SPECIAL_CARD_FLAG   { get; set; } = string.Empty;
            public string CSN_BANK_NO         { get; set; } = string.Empty;
            public string JCD_SAVE_FLAG       { get; set; } = string.Empty;
            public string CNMK_CODE           { get; set; } = string.Empty;
            public string PRINT_MSG           { get; set; } = string.Empty;
            public string NO_VAT_AMT          { get; set; } = string.Empty;
            public string TRAN_TYPE_FLAG      { get; set; } = string.Empty;
            public string TRAN_DC_NAME        { get; set; } = string.Empty;
            public string TRAN_DC_AMT         { get; set; } = string.Empty;
            public string VAN_SER_NO          { get; set; } = string.Empty;
            public string SLIP_TYPE_FLAG      { get; set; } = string.Empty;
            public string TRAN_UNIQUE_NO      { get; set; } = string.Empty;
            public string TRAN_ORDER_NO       { get; set; } = string.Empty;
            public string DCC_EX_RATE_CODE    { get; set; } = string.Empty;
            public string DCC_RCV_TEXT        { get; set; } = string.Empty;
            public string DEPOSIT_AMT         { get; set; } = string.Empty;


            */
        }

        public SALE_CARD(string shop_code, string sale_date, string pos_no, string bill_no, string line_no, string seq_no)
        {
            this.SHOP_CODE = shop_code;
            this.SALE_DATE = sale_date;
            this.POS_NO = pos_no;
            this.BILL_NO = bill_no;
            this.LINE_NO = line_no;
            this.SEQ_NO = seq_no;
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
        public string APPR_PROC_FLAG { get; set; } = string.Empty;
        public string CRD_CARD_NO { get; set; } = string.Empty;
        public string APPR_REQ_AMT { get; set; } = string.Empty;
        public string SVC_TIP_AMT { get; set; } = string.Empty;
        public string VAT_AMT { get; set; } = string.Empty;
        public string INST_MM_FLAG { get; set; } = string.Empty;
        public string INST_MM_CNT { get; set; } = string.Empty;
        public string VALID_TERM { get; set; } = string.Empty;
        public string SIGN_PAD_YN { get; set; } = string.Empty;
        public string CARD_IN_FLAG { get; set; } = string.Empty;
        public string APPR_DATE { get; set; } = string.Empty;
        public string APPR_TIME { get; set; } = string.Empty;
        public string APPR_NO { get; set; } = string.Empty;
        public string CRDCP_CODE { get; set; } = string.Empty;
        public string ISS_CRDCP_CODE { get; set; } = string.Empty;
        public string ISS_CRDCP_NAME { get; set; } = string.Empty;
        public string PUR_CRDCP_CODE { get; set; } = string.Empty;
        public string PUR_CRDCP_NAME { get; set; } = string.Empty;
        public string APPR_AMT { get; set; } = string.Empty;
        public string APPR_DC_AMT { get; set; } = string.Empty;
        public string APPR_MSG { get; set; } = string.Empty;
        public string VAN_TERM_NO { get; set; } = string.Empty;
        public string VAN_SLIP_NO { get; set; } = string.Empty;
        public string CRDCP_TERM_NO { get; set; } = string.Empty;
        public string ORG_APPR_DATE { get; set; } = string.Empty;
        public string ORG_APPR_NO { get; set; } = string.Empty;
        public string APPR_LOG_NO { get; set; } = string.Empty;
        public string INSERT_DT { get; set; } = string.Empty;
        public string EMP_NO { get; set; } = string.Empty;
        public string PRE_PAY_FLAG { get; set; } = string.Empty;
        public string LYT_APPR_FLAG { get; set; } = string.Empty;
        public string LYT_APPR_INFO { get; set; } = string.Empty;
        public string SPECIAL_CARD_FLAG { get; set; } = string.Empty;
        public string CSN_BANK_NO { get; set; } = string.Empty;
        public string JCD_SAVE_FLAG { get; set; } = string.Empty;
        public string CNMK_CODE { get; set; } = string.Empty;
        public string PRINT_MSG { get; set; } = string.Empty;
        public string NO_VAT_AMT { get; set; } = string.Empty;
        public string TRAN_TYPE_FLAG { get; set; } = string.Empty;
        public string TRAN_DC_NAME { get; set; } = string.Empty;
        public string TRAN_DC_AMT { get; set; } = string.Empty;
        public string VAN_SER_NO { get; set; } = string.Empty;
        public string SLIP_TYPE_FLAG { get; set; } = string.Empty;
        public string TRAN_UNIQUE_NO { get; set; } = string.Empty;
        public string TRAN_ORDER_NO { get; set; } = string.Empty;
        public string DCC_EX_RATE_CODE { get; set; } = string.Empty;
        public string DCC_RCV_TEXT { get; set; } = string.Empty;
        public string DEPOSIT_AMT { get; set; } = string.Empty;
    }
}

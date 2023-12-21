using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace GoPOS.Models
{
    /// <summary>
    /// 모바일 쿠폰

    /// </summary>
    public class POS_ORD_MOBILE_COUPON
    {
        public POS_ORD_MOBILE_COUPON()
        {
            /*
            CREATE TABLE POS_ORD_MOBILE_COUPON(
            SHOP_CODE VARCHAR(6) NOT NULL,
            SALE_DATE         VARCHAR(8) NOT NULL,
            ORDER_NO          VARCHAR(4) NOT NULL,
            LINE_NO           VARCHAR(4) NOT NULL,
            SEQ_NO            VARCHAR(2) NOT NULL,
            POS_NO            VARCHAR(2) NOT NULL,
            SALE_YN           VARCHAR(1) DEFAULT 'Y' NOT NULL,
            MCP_CODE          VARCHAR(4) DEFAULT '0000' NOT NULL,
            MCP_PROC_FLAG     VARCHAR(1) NOT NULL,
            APPR_LOG_NO       VARCHAR(5) NOT NULL,
            APPR_REQ_AMT      NUMERIC(12,2) DEFAULT 0 NOT NULL,
            APPR_AMT          NUMERIC(12,2) DEFAULT 0 NOT NULL,
            APPR_DC_AMT       NUMERIC(12,2) DEFAULT 0 NOT NULL,
            INSERT_DT         VARCHAR(14) NOT NULL,
            EMP_NO            VARCHAR(4) NOT NULL,
            MCP_UAMT          NUMERIC(12,2) DEFAULT 0 NOT NULL,
            REPAY_MCP_AMT     NUMERIC(12,2) DEFAULT 0 NOT NULL,
            MCP_FLAG          VARCHAR(1) DEFAULT '0' NOT NULL,
            CASH_APPR_AMT     NUMERIC(12,2) DEFAULT 0 NOT NULL,
            CASH_APPR_LOG_NO  VARCHAR(5),
            DEPOSIT_AMT NUMERIC(12,2) DEFAULT 0 NOT NULL
            );

            Primary keys                                
            ALTER TABLE POS_ORD_MOBILE_COUPON ADD CONSTRAINT POS_ODMCP_PK PRIMARY KEY(SHOP_CODE, SALE_DATE, ORDER_NO, LINE_NO, SEQ_NO);
            Descriptions

            COMMENT ON TABLE POS_ORD_MOBILE_COUPON IS 'POS-주문결제-모바일쿠폰';
            */
        }

        public POS_ORD_MOBILE_COUPON(string shop_code, string sale_date, string order_no, string line_no, string seq_no)
        {
            this.SHOP_CODE = shop_code;
            this.SALE_DATE = sale_date;
            this.ORDER_NO = order_no;
            this.LINE_NO = line_no;
            this.SEQ_NO = seq_no;
        }

        public string SHOP_CODE { get; set; } = string.Empty;
        public string  SALE_DATE        { get; set; } = string.Empty;
        public string  ORDER_NO         { get; set; } = string.Empty;
        public string  LINE_NO          { get; set; } = string.Empty;
        public string  SEQ_NO           { get; set; } = string.Empty;
        public string  POS_NO           { get; set; } = string.Empty;
        public string  SALE_YN          { get; set; } = "Y";
        public string  MCP_CODE         { get; set; } = "0000";
        public string  MCP_PROC_FLAG    { get; set; } = string.Empty;
        public string  APPR_LOG_NO      { get; set; } = string.Empty;
        public string  APPR_REQ_AMT     { get; set; } = "0";
        public string  APPR_AMT         { get; set; } = "0";
        public string APPR_DC_AMT       { get; set; } = "0";
        public string  INSERT_DT        { get; set; } = string.Empty;
        public string  EMP_NO           { get; set; } = string.Empty;
        public string  MCP_UAMT         { get; set; } = "0";
        public string  REPAY_MCP_AMT    { get; set; } = "0";
        public string  MCP_FLAG         { get; set; } = "0";
        public string  CASH_APPR_AMT    { get; set; } = "0";
        public string  CASH_APPR_LOG_NO { get; set; } = string.Empty;
        public string DEPOSIT_AMT       { get; set; } = "0";

    }
}

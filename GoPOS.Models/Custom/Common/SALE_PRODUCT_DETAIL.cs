using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoPOS.Models
{
    public class SALE_PRODUCT_DETAIL
    {
        /// <summary>
        /// 매출TR-상품-DETAIL

        /// </summary>
        public SALE_PRODUCT_DETAIL()
        {
            /*            
            SHOP_CODE                VARCHAR(6) NOT NULL,    
            SALE_DATE                VARCHAR(8) NOT NULL,
            POS_NO                   VARCHAR(2) NOT NULL,
            BILL_NO                  VARCHAR(4) NOT NULL,
            DTL_NO                   VARCHAR(4) NOT NULL,
            REGI_SEQ                 VARCHAR(2) DEFAULT '01' NOT NULL,
            SALE_YN                  VARCHAR(1) DEFAULT 'Y' NOT NULL,
            PRD_CODE                 VARCHAR(26) NOT NULL,
            PRD_TYPE_FLAG            VARCHAR(1) DEFAULT '0' NOT NULL,
            CORNER_CODE              VARCHAR(2) DEFAULT '00' NOT NULL,
            CHG_BILL_NO              VARCHAR(4),
            TAX_YN                   VARCHAR(1) DEFAULT 'Y' NOT NULL,
            DLV_PACK_FLAG            VARCHAR(1) DEFAULT '0' NOT NULL,
            ORG_SALE_MG_CODE         VARCHAR(4),
            ORG_SALE_UPRC            NUMERIC(12,2) DEFAULT 0 NOT NULL,
            NORMAL_UPRC              NUMERIC(10,2) DEFAULT 0 NOT NULL,
            SALE_MG_CODE             VARCHAR(4),
            SALE_QTY                 NUMERIC(9,0) DEFAULT 0 NOT NULL,
            SALE_UPRC                NUMERIC(10,2) DEFAULT 0 NOT NULL,
            SALE_AMT                 NUMERIC(12,2) DEFAULT 0 NOT NULL,
            DC_AMT                   NUMERIC(12,2) DEFAULT 0 NOT NULL,
            ETC_AMT                  NUMERIC(12,2) DEFAULT 0 NOT NULL,
            SVC_TIP_AMT              NUMERIC(12,2) DEFAULT 0 NOT NULL,
            DCM_SALE_AMT             NUMERIC(12,2) DEFAULT 0 NOT NULL,
            VAT_AMT                  NUMERIC(12,2) DEFAULT 0 NOT NULL,
            SVC_CODE                 VARCHAR(1),
            TK_CPN_CODE              VARCHAR(3),
            DC_AMT_GEN               NUMERIC(12,2) DEFAULT 0 NOT NULL,
            DC_AMT_SVC               NUMERIC(12,2) DEFAULT 0 NOT NULL,
            DC_AMT_JCD               NUMERIC(12,2) DEFAULT 0 NOT NULL,
            DC_AMT_CPN               NUMERIC(12,2) DEFAULT 0 NOT NULL,
            DC_AMT_CST               NUMERIC(12,2) DEFAULT 0 NOT NULL,
            DC_AMT_FOD               NUMERIC(12,2) DEFAULT 0 NOT NULL,
            DC_AMT_PRM               NUMERIC(12,2) DEFAULT 0 NOT NULL,
            DC_AMT_CRD               NUMERIC(12,2) DEFAULT 0 NOT NULL,
            DC_AMT_PACK              NUMERIC(12,2) DEFAULT 0 NOT NULL,
            DC_AMT_LYT               NUMERIC(12,2) DEFAULT 0 NOT NULL,
            CST_SALE_POINT           NUMERIC(12,4) DEFAULT 0 NOT NULL,
            CST_USE_POINT            NUMERIC(12,4) DEFAULT 0 NOT NULL,
            PRM_PROC_YN              VARCHAR(1) DEFAULT 'N' NOT NULL,
            PRM_CODE                 VARCHAR(4),
            PRM_SEQ                  VARCHAR(2),
            SDA_CODE                 VARCHAR(10),
            SDS_ORG_DTL_NO           VARCHAR(4),
            INSERT_DT                VARCHAR(14) NOT NULL,
            EMP_NO                   VARCHAR(4) NOT NULL,
            AFFILIATE_UPRC           NUMERIC(12,2),
            MCP_BAR_CODE             VARCHAR(40),
            SDS_CLASS_CODE           VARCHAR(6),
            DOUBLE_CODE              VARCHAR(2),
            DOUBLE_AMT               NUMERIC(12,2) DEFAULT 0 NOT NULL,
            PRD_WEIGHT               NUMERIC(8,0) DEFAULT 0 NOT NULL,
            IF_CPN_CODE              VARCHAR(20),
            TAX_RFND_AMT             NUMERIC(12,2) DEFAULT 0 NOT NULL,
            TAX_RFND_FEE             NUMERIC(12,2) DEFAULT 0 NOT NULL,
            DC_AMT_TAX               NUMERIC(12,2) DEFAULT 0 NOT NULL,
            DC_REASON_CODE           VARCHAR(3),
            DC_REASON_NAME           VARCHAR(200),
            SDS_PARENT_CODE          VARCHAR(26),
            DC_SEQ_TYPE              VARCHAR(50),
            DC_SEQ_AMT               VARCHAR(500),
            DC_SEQ_FLAG              VARCHAR(50),
            DC_SEQ_RATE              VARCHAR(500)

            public string SHOP_CODE               { get; set; } = string.Empty;
            public string SALE_DATE               { get; set; } = string.Empty;
            public string POS_NO                  { get; set; } = string.Empty;
            public string BILL_NO                 { get; set; } = string.Empty;
            public string DTL_NO                  { get; set; } = string.Empty;
            public string REGI_SEQ                { get; set; } = string.Empty;
            public string SALE_YN                 { get; set; } = string.Empty;
            public string PRD_CODE                { get; set; } = string.Empty;
            public string PRD_TYPE_FLAG           { get; set; } = string.Empty;
            public string CORNER_CODE             { get; set; } = string.Empty;
            public string CHG_BILL_NO             { get; set; } = string.Empty;
            public string TAX_YN                  { get; set; } = string.Empty;
            public string DLV_PACK_FLAG           { get; set; } = string.Empty;
            public string ORG_SALE_MG_COD         { get; set; } = string.Empty;
            public string ORG_SALE_UPRC           { get; set; } = string.Empty;
            public string NORMAL_UPRC             { get; set; } = string.Empty;
            public string SALE_MG_CODE            { get; set; } = string.Empty;
            public string SALE_QTY                { get; set; } = string.Empty;
            public string SALE_UPRC               { get; set; } = string.Empty;
            public string SALE_AMT                { get; set; } = string.Empty;
            public string DC_AMT                  { get; set; } = string.Empty;
            public string ETC_AMT                 { get; set; } = string.Empty;
            public string SVC_TIP_AMT             { get; set; } = string.Empty;
            public string DCM_SALE_AMT            { get; set; } = string.Empty;
            public string VAT_AMT                 { get; set; } = string.Empty;
            public string SVC_CODE                { get; set; } = string.Empty;
            public string TK_CPN_CODE             { get; set; } = string.Empty;
            public string DC_AMT_GEN              { get; set; } = string.Empty;
            public string DC_AMT_SVC              { get; set; } = string.Empty;
            public string DC_AMT_JCD              { get; set; } = string.Empty;
            public string DC_AMT_CPN              { get; set; } = string.Empty;
            public string DC_AMT_CST              { get; set; } = string.Empty;
            public string DC_AMT_FOD              { get; set; } = string.Empty;
            public string DC_AMT_PRM              { get; set; } = string.Empty;
            public string DC_AMT_CRD              { get; set; } = string.Empty;
            public string DC_AMT_PACK             { get; set; } = string.Empty;
            public string DC_AMT_LYT              { get; set; } = string.Empty;
            public string CST_SALE_POINT          { get; set; } = string.Empty;
            public string CST_USE_POINT           { get; set; } = string.Empty;
            public string PRM_PROC_YN             { get; set; } = string.Empty;
            public string PRM_CODE                { get; set; } = string.Empty;
            public string PRM_SEQ                 { get; set; } = string.Empty;
            public string SDA_CODE                { get; set; } = string.Empty;
            public string SDS_ORG_DTL_NO          { get; set; } = string.Empty;
            public string INSERT_DT               { get; set; } = string.Empty;
            public string EMP_NO                  { get; set; } = string.Empty;
            public string AFFILIATE_UPRC          { get; set; } = string.Empty;
            public string MCP_BAR_CODE            { get; set; } = string.Empty;
            public string SDS_CLASS_CODE          { get; set; } = string.Empty;
            public string DOUBLE_CODE             { get; set; } = string.Empty;
            public string DOUBLE_AMT              { get; set; } = string.Empty;
            public string PRD_WEIGHT              { get; set; } = string.Empty;
            public string IF_CPN_CODE             { get; set; } = string.Empty;
            public string TAX_RFND_AMT            { get; set; } = string.Empty;
            public string TAX_RFND_FEE            { get; set; } = string.Empty;
            public string DC_AMT_TAX              { get; set; } = string.Empty;
            public string DC_REASON_CODE          { get; set; } = string.Empty;
            public string DC_REASON_NAME          { get; set; } = string.Empty;
            public string SDS_PARENT_CODE         { get; set; } = string.Empty;
            public string DC_SEQ_TYPE             { get; set; } = string.Empty;
            public string DC_SEQ_AMT              { get; set; } = string.Empty;
            public string DC_SEQ_FLAG             { get; set; } = string.Empty;
            public string DC_SEQ_RATE             { get; set; } = string.Empty;

            */
        }

        public SALE_PRODUCT_DETAIL(string shop_code, string sale_date, string pos_no, string bill_no)
        {
            this.SHOP_CODE = shop_code;
            this.SALE_DATE = sale_date;
            this.POS_NO = pos_no;
            this.BILL_NO = bill_no;         
        }

        public string SHOP_CODE { get; set; } = string.Empty;
        public string SALE_DATE { get; set; } = string.Empty;
        public string POS_NO { get; set; } = string.Empty;
        public string BILL_NO { get; set; } = string.Empty;
        public string DTL_NO { get; set; } = string.Empty;
        public string REGI_SEQ { get; set; } = string.Empty;
        public string SALE_YN { get; set; } = string.Empty;
        public string PRD_CODE { get; set; } = string.Empty;
        public string PRD_TYPE_FLAG { get; set; } = string.Empty;
        public string CORNER_CODE { get; set; } = string.Empty;
        public string CHG_BILL_NO { get; set; } = string.Empty;
        public string TAX_YN { get; set; } = string.Empty;
        public string DLV_PACK_FLAG { get; set; } = string.Empty;
        public string ORG_SALE_MG_CODE { get; set; } = string.Empty;
        public string ORG_SALE_UPRC { get; set; } = string.Empty;
        public string NORMAL_UPRC { get; set; } = string.Empty;
        public string SALE_MG_CODE { get; set; } = string.Empty;
        public string SALE_QTY { get; set; } = string.Empty;
        public string SALE_UPRC { get; set; } = string.Empty;
        public string SALE_AMT { get; set; } = string.Empty;
        public string DC_AMT { get; set; } = string.Empty;
        public string ETC_AMT { get; set; } = string.Empty;
        public string SVC_TIP_AMT { get; set; } = string.Empty;
        public string DCM_SALE_AMT { get; set; } = string.Empty;
        public string VAT_AMT { get; set; } = string.Empty;
        public string SVC_CODE { get; set; } = string.Empty;
        public string TK_CPN_CODE { get; set; } = string.Empty;
        public string DC_AMT_GEN { get; set; } = string.Empty;
        public string DC_AMT_SVC { get; set; } = string.Empty;
        public string DC_AMT_JCD { get; set; } = string.Empty;
        public string DC_AMT_CPN { get; set; } = string.Empty;
        public string DC_AMT_CST { get; set; } = string.Empty;
        public string DC_AMT_FOD { get; set; } = string.Empty;
        public string DC_AMT_PRM { get; set; } = string.Empty;
        public string DC_AMT_CRD { get; set; } = string.Empty;
        public string DC_AMT_PACK { get; set; } = string.Empty;
        public string DC_AMT_LYT { get; set; } = string.Empty;
        public string CST_SALE_POINT { get; set; } = string.Empty;
        public string CST_USE_POINT { get; set; } = string.Empty;
        public string PRM_PROC_YN { get; set; } = string.Empty;
        public string PRM_CODE { get; set; } = string.Empty;
        public string PRM_SEQ { get; set; } = string.Empty;
        public string SDA_CODE { get; set; } = string.Empty;
        public string SDS_ORG_DTL_NO { get; set; } = string.Empty;
        public string INSERT_DT { get; set; } = string.Empty;
        public string EMP_NO { get; set; } = string.Empty;
        public string AFFILIATE_UPRC { get; set; } = string.Empty;
        public string MCP_BAR_CODE { get; set; } = string.Empty;
        public string SDS_CLASS_CODE { get; set; } = string.Empty;
        public string DOUBLE_CODE { get; set; } = string.Empty;
        public string DOUBLE_AMT { get; set; } = string.Empty;
        public string PRD_WEIGHT { get; set; } = string.Empty;
        public string IF_CPN_CODE { get; set; } = string.Empty;
        public string TAX_RFND_AMT { get; set; } = string.Empty;
        public string TAX_RFND_FEE { get; set; } = string.Empty;
        public string DC_AMT_TAX { get; set; } = string.Empty;
        public string DC_REASON_CODE { get; set; } = string.Empty;
        public string DC_REASON_NAME { get; set; } = string.Empty;
        public string SDS_PARENT_CODE { get; set; } = string.Empty;
        public string DC_SEQ_TYPE { get; set; } = string.Empty;
        public string DC_SEQ_AMT { get; set; } = string.Empty;
        public string DC_SEQ_FLAG { get; set; } = string.Empty;
        public string DC_SEQ_RATE { get; set; } = string.Empty;
    }
}

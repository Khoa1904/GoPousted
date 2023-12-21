using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


// 오더 디테일 테이블 모델

namespace GoPOS.Models
{
    public class POS_ORD_DETAIL
    {
        public POS_ORD_DETAIL()
        {
            /*
            SHOP_CODE          VARCHAR(6) NOT NULL,
            SALE_DATE          VARCHAR(8) NOT NULL,
            ORDER_NO           VARCHAR(4) NOT NULL,
            ORDER_DTL_NO       VARCHAR(4) NOT NULL,
            ORDER_DTL_FLAG     VARCHAR(1) DEFAULT '0' NOT NULL,
            ORDER_SEQ_NO       VARCHAR(4) DEFAULT '01' NOT NULL,
            POS_NO             VARCHAR(2) NOT NULL,
            PRD_CODE           VARCHAR(26) NOT NULL,
            PRD_TYPE_FLAG      VARCHAR(1) DEFAULT '0' NOT NULL,
            CORNER_CODE        VARCHAR(2) DEFAULT '00' NOT NULL,
            CHG_BILL_NO        VARCHAR(4),
            TAX_YN             VARCHAR(1) DEFAULT 'Y' NOT NULL,
            DLV_PACK_FLAG      VARCHAR(1) DEFAULT '0' NOT NULL,
            ORG_SALE_MG_CODE   VARCHAR(4),
            ORG_SALE_UPRC      NUMERIC(12,2) DEFAULT 0 NOT NULL,
            NORMAL_UPRC        NUMERIC(10,2) DEFAULT 0 NOT NULL,
            SALE_MG_CODE       VARCHAR(4),
            SALE_QTY           NUMERIC(9,0) DEFAULT 0 NOT NULL,
            SALE_UPRC          NUMERIC(10,2) DEFAULT 0 NOT NULL,
            SALE_AMT           NUMERIC(12,2) DEFAULT 0 NOT NULL,
            DC_AMT             NUMERIC(12,2) DEFAULT 0 NOT NULL,
            ETC_AMT            NUMERIC(12,2) DEFAULT 0 NOT NULL,
            SVC_TIP_AMT        NUMERIC(12,2) DEFAULT 0 NOT NULL,
            DCM_SALE_AMT       NUMERIC(12,2) DEFAULT 0 NOT NULL,
            VAT_AMT            NUMERIC(12,2) DEFAULT 0 NOT NULL,
            SVC_CODE           VARCHAR(1),
            TK_CPN_CODE        VARCHAR(3),
            DC_TYPE_FLAG       VARCHAR(1) DEFAULT '0' NOT NULL,
            DC_AMT_GEN         NUMERIC(12,2) DEFAULT 0 NOT NULL,
            DC_AMT_SVC         NUMERIC(12,2) DEFAULT 0 NOT NULL,
            DC_AMT_JCD         NUMERIC(12,2) DEFAULT 0 NOT NULL,
            DC_AMT_CPN         NUMERIC(12,2) DEFAULT 0 NOT NULL,
            DC_AMT_CST         NUMERIC(12,2) DEFAULT 0 NOT NULL,
            DC_AMT_FOD         NUMERIC(12,2) DEFAULT 0 NOT NULL,
            DC_AMT_PRM         NUMERIC(12,2) DEFAULT 0 NOT NULL,
            DC_AMT_CRD         NUMERIC(12,2) DEFAULT 0 NOT NULL,
            DC_AMT_PACK        NUMERIC(12,2) DEFAULT 0 NOT NULL,
            DC_AMT_LYT         NUMERIC(12,2) DEFAULT 0 NOT NULL,
            CST_SALE_POINT     NUMERIC(12,4) DEFAULT 0 NOT NULL,
            CST_USE_POINT      NUMERIC(12,4) DEFAULT 0 NOT NULL,
            PRM_PROC_YN        VARCHAR(1) DEFAULT 'N' NOT NULL,
            PRM_CODE           VARCHAR(4),
            PRM_SEQ            VARCHAR(2),
            SDA_CODE           VARCHAR(10),
            SDS_ORG_DTL_NO     VARCHAR(4),
            ORDER_EMP_NO       VARCHAR(4),
            ORG_ORDER_NO       VARCHAR(4),
            ORG_FD_TBL_CODE    VARCHAR(3),
            REMARK             VARCHAR(30),
            INSERT_DT          VARCHAR(14) NOT NULL,
            EMP_NO             VARCHAR(4) NOT NULL,
            DC_SEQ_TYPE        VARCHAR(50),
            DC_SEQ_AMT         VARCHAR(500),
            DC_SEQ_FLAG        VARCHAR(50),
            DC_SEQ_RATE        VARCHAR(500),
            ORDER_SCN_FLAG     VARCHAR(1) DEFAULT '0' NOT NULL,
            ORDER_SCN_DT       VARCHAR(14),
            ORDER_SCN_POS_NO   VARCHAR(2),
            ORDER_CANCEL_FLAG  VARCHAR(1),
            AFFILIATE_UPRC     NUMERIC(12,2),
            MCP_BAR_CODE       VARCHAR(40),
            STAMP_SALE_QTY     NUMERIC(9,0) DEFAULT 0 NOT NULL,
            SDS_CLASS_CODE     VARCHAR(6),
            DOUBLE_CODE        VARCHAR(2),
            DOUBLE_AMT         NUMERIC(12,2) DEFAULT 0 NOT NULL,
            PRD_WEIGHT         NUMERIC(8,0) DEFAULT 0 NOT NULL,
            COOK_MEMO          VARCHAR(1024),
            IF_CPN_CODE        VARCHAR(20),
            ORDER_CANCEL_CODE  VARCHAR(3),
            ORDER_CANCEL_NAME  VARCHAR(200),
            TAX_RFND_AMT       NUMERIC(12,2) DEFAULT 0 NOT NULL,
            TAX_RFND_FEE       NUMERIC(12,2) DEFAULT 0 NOT NULL,
            DC_AMT_TAX         NUMERIC(12,2) DEFAULT 0 NOT NULL,
            DC_REASON_CODE     VARCHAR(3),
            DC_REASON_NAME     VARCHAR(200),
            SDS_PARENT_CODE    VARCHAR(26),
            DC_RULE_FLAG       VARCHAR(10)
            */
        }

        public POS_ORD_DETAIL(string shop_code, string sale_date, string order_no, string order_dtl_no)
        {
            this.SHOP_CODE = shop_code;
            this.SALE_DATE = sale_date;
            this.ORDER_NO = order_no;
            this.ORDER_DTL_NO = order_dtl_no;
        }
        public string NO { get; set; } = "";//그리드순번
        public string SHOP_CODE { get; set; } = string.Empty; //  VARCHAR(6) NOT NULL,
        public string SALE_DATE { get; set; } = string.Empty; //  VARCHAR(8) NOT NULL,
        public string ORDER_NO { get; set; } = string.Empty; //  VARCHAR(4) NOT NULL,
        public string ORDER_DTL_NO { get; set; } = string.Empty; //  VARCHAR(4) NOT NULL, ?
        public string ORDER_DTL_FLAG { get; set; } = "0"; //  VARCHAR(1) DEFAULT '0' NOT NULL, // 주문디테일구분 0:주문 1:주문취소 3:금액변경취소 4:금액변경주문
        public string ORDER_SEQ_NO { get; set; } = "01"; //  VARCHAR(4) DEFAULT '01' NOT NULL,  // 주문차수
        public string POS_NO { get; set; } = ""; //  VARCHAR(2) NOT NULL,
        public string PRD_CODE { get; set; } = ""; //  VARCHAR(26) NOT NULL,
        public string PRD_NAME { get; set; } = ""; // 상품명 join
        public string PRD_TYPE_FLAG { get; set; } = "0"; //  VARCHAR(1) DEFAULT '0' NOT NULL,  // 상품유형구분 ( CMM_CODE : 060 ) 0:일반 1:선택메뉴
        public string CORNER_CODE { get; set; } = "00"; //  VARCHAR(2) DEFAULT '00' NOT NULL,
        public string CHG_BILL_NO { get; set; } = string.Empty; //  VARCHAR(4),
        public string TAX_YN { get; set; } = "Y"; //  VARCHAR(1) DEFAULT 'Y' NOT NULL,
        public string DLV_PACK_FLAG { get; set; } = "0"; //  VARCHAR(1) DEFAULT '0' NOT NULL,
        public string ORG_SALE_MG_CODE { get; set; } = string.Empty; //  VARCHAR(4),
        public string ORG_SALE_UPRC { get; set; } = "0"; //  NUMERIC(12,2) DEFAULT 0 NOT NULL,
        public string NORMAL_UPRC { get; set; } = "0"; //       NUMERIC(10,2) DEFAULT 0 NOT NULL,
        public string SALE_MG_CODE { get; set; } = string.Empty; //  VARCHAR(4),
        public string SALE_QTY { get; set; } = "0"; //  NUMERIC(9,0) DEFAULT 0 NOT NULL,
        public string SALE_UPRC { get; set; } = "0"; //      NUMERIC(10,2) DEFAULT 0 NOT NULL,
        public string SALE_AMT { get; set; } = "0"; //      NUMERIC(12,2) DEFAULT 0 NOT NULL,
        public string DC_AMT { get; set; } = "0"; //      NUMERIC(12,2) DEFAULT 0 NOT NULL,
        public string ETC_AMT { get; set; } = "0"; //      NUMERIC(12,2) DEFAULT 0 NOT NULL,
        public string SVC_TIP_AMT { get; set; } = "0"; //     NUMERIC(12,2) DEFAULT 0 NOT NULL,
        public string DCM_SALE_AMT { get; set; } = "0"; //      NUMERIC(12,2) DEFAULT 0 NOT NULL,
        public string VAT_AMT { get; set; } = "0"; //      NUMERIC(12,2) DEFAULT 0 NOT NULL,
        public string SVC_CODE { get; set; } = string.Empty; //  VARCHAR(1),
        public string TK_CPN_CODE { get; set; } = string.Empty; //  VARCHAR(3),
        public string DC_TYPE_FLAG { get; set; } = "0"; //  VARCHAR(1) DEFAULT '0' NOT NULL, // 할인형태구분 ( 0:일반 1:%DC 2:금액DC 3:교환 9:할인취소 )
        public string DC_AMT_GEN { get; set; } = "0"; //    NUMERIC(12,2) DEFAULT 0 NOT NULL,
        public string DC_AMT_SVC { get; set; } = "0"; //     NUMERIC(12,2) DEFAULT 0 NOT NULL,
        public string DC_AMT_JCD { get; set; } = "0"; //     NUMERIC(12,2) DEFAULT 0 NOT NULL,
        public string DC_AMT_CPN { get; set; } = "0"; //     NUMERIC(12,2) DEFAULT 0 NOT NULL,
        public string DC_AMT_CST { get; set; } = "0"; //     NUMERIC(12,2) DEFAULT 0 NOT NULL,
        public string DC_AMT_FOD { get; set; } = "0"; //     NUMERIC(12,2) DEFAULT 0 NOT NULL,
        public string DC_AMT_PRM { get; set; } = "0"; //     NUMERIC(12,2) DEFAULT 0 NOT NULL,
        public string DC_AMT_CRD { get; set; } = "0"; //     NUMERIC(12,2) DEFAULT 0 NOT NULL,
        public string DC_AMT_PACK { get; set; } = "0"; //    NUMERIC(12,2) DEFAULT 0 NOT NULL,
        public string DC_AMT_LYT { get; set; } = "0"; //    NUMERIC(12,2) DEFAULT 0 NOT NULL,
        public string CST_SALE_POINT { get; set; } = "0"; //  NUMERIC(12,4) DEFAULT 0 NOT NULL,
        public string CST_USE_POINT { get; set; } = "0"; //  NUMERIC(12,4) DEFAULT 0 NOT NULL,
        public string PRM_PROC_YN { get; set; } = "N"; //  VARCHAR(1) DEFAULT 'N' NOT NULL,
        public string PRM_CODE { get; set; } = string.Empty; //  VARCHAR(4),
        public string PRM_SEQ { get; set; } = string.Empty; //  VARCHAR(2),
        public string SDA_CODE { get; set; } = string.Empty; //  VARCHAR(10),
        public string SDS_ORG_DTL_NO { get; set; } = string.Empty; //  VARCHAR(4),
        public string ORDER_EMP_NO { get; set; } = string.Empty; //  VARCHAR(4),
        public string ORG_ORDER_NO { get; set; } = string.Empty; //  VARCHAR(4),
        public string ORG_FD_TBL_CODE { get; set; } = string.Empty; //  VARCHAR(3),
        public string REMARK { get; set; } = string.Empty; //  VARCHAR(30),
        public string INSERT_DT { get; set; } = ""; //  VARCHAR(14) NOT NULL,
        public string EMP_NO { get; set; } = string.Empty; //  VARCHAR(4) NOT NULL,
        public string DC_SEQ_TYPE { get; set; } = string.Empty; //  VARCHAR(50),
        public string DC_SEQ_AMT { get; set; } = string.Empty; //  VARCHAR(500),
        public string DC_SEQ_FLAG { get; set; } = string.Empty; //  VARCHAR(50),
        public string DC_SEQ_RATE { get; set; } = string.Empty; //  VARCHAR(500),
        public string ORDER_SCN_FLAG { get; set; } = "0"; //  VARCHAR(1) DEFAULT '0' NOT NULL,
        public string ORDER_SCN_DT { get; set; } = string.Empty; //  VARCHAR(14),
        public string ORDER_SCN_POS_NO { get; set; } = string.Empty; //  VARCHAR(2),
        public string ORDER_CANCEL_FLAG { get; set; } = string.Empty; //  VARCHAR(1),   // // 주문취소구분(0:정상 1:취소 2:변경)
        public string AFFILIATE_UPRC { get; set; } = "0"; //  NUMERIC(12,2),
        public string MCP_BAR_CODE { get; set; } = string.Empty; //  VARCHAR(40),
        public string STAMP_SALE_QTY { get; set; } = "0"; //  NUMERIC(9,0) DEFAULT 0 NOT NULL,
        public string SDS_CLASS_CODE { get; set; } = string.Empty; //  VARCHAR(6),
        public string DOUBLE_CODE { get; set; } = string.Empty; //  VARCHAR(2),
        public string DOUBLE_AMT { get; set; } = "0"; //  NUMERIC(12,2) DEFAULT 0 NOT NULL,
        public string PRD_WEIGHT { get; set; } = "0"; //      NUMERIC(8,0) DEFAULT 0 NOT NULL,
        public string COOK_MEMO { get; set; } = string.Empty; //  VARCHAR(1024),
        public string IF_CPN_CODE { get; set; } = string.Empty; //  VARCHAR(20),
        public string ORDER_CANCEL_CODE { get; set; } = string.Empty; //  VARCHAR(3),
        public string ORDER_CANCEL_NAME { get; set; } = string.Empty; //  VARCHAR(200),
        public string TAX_RFND_AMT { get; set; } = "0"; // NUMERIC(12,2) DEFAULT 0 NOT NULL,
        public string TAX_RFND_FEE { get; set; } = "0"; // NUMERIC(12,2) DEFAULT 0 NOT NULL,
        public string DC_AMT_TAX { get; set; } = "0"; // NUMERIC(12,2) DEFAULT 0 NOT NULL,
        public string DC_REASON_CODE { get; set; } = string.Empty; //  VARCHAR(3),
        public string DC_REASON_NAME { get; set; } = string.Empty; //  VARCHAR(200),
        public string SDS_PARENT_CODE { get; set; } = string.Empty; //  VARCHAR(26),
        public string DC_RULE_FLAG { get; set; } = string.Empty; //  VARCHAR(10)

        // 리턴변수
        public int PO_RET_FLAG { get; set; } = -1;
        
        public string PO_ORDER_NO { get; set; } = "";

        //예약 조회 코너명 추가
        public string CORNER_NAME { get; set; } = "";
    }
}
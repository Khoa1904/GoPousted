using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoPOS.Models
{
    public class POS_ORD_M
    {
        /// <summary>
        /// 매장-포스마스터

        /// </summary>
        public POS_ORD_M()
        {
        }

        public POS_ORD_M(string shop_code, string sale_date, string pos_no)
        {
            this.SHOP_CODE = shop_code;
            this.SALE_DATE = sale_date;
            this.POS_NO = pos_no;
        }

        public string SHOP_CODE { get; set; } = string.Empty;//VARCHAR(6) NOT NULL,
        public string SALE_DATE { get; set; } = string.Empty;//VARCHAR(8) NOT NULL,
        public string ORDER_NO { get; set; } = string.Empty;//VARCHAR(4) NOT NULL,
        public string POS_NO { get; set; } = string.Empty;//VARCHAR(2) NOT NULL,
        public string DLV_ORDER_FLAG { get; set; } = "0";//VARCHAR(1) DEFAULT '0' NOT NULL,
        public string ORDER_END_FLAG { get; set; } = "0";//VARCHAR(1) DEFAULT '0' NOT NULL,
        public string FD_TBL_CODE { get; set; } = "000";//VARCHAR(3) DEFAULT '000' NOT NULL,
        public string TOT_SALE_AMT { get; set; } = "0";//NUMERIC(12,2) DEFAULT 0 NOT NULL,
        public string TOT_DC_AMT { get; set; } = "0";//NUMERIC(12,2) DEFAULT 0 NOT NULL,
        public string SVC_TIP_AMT { get; set; } = "0";//NUMERIC(12,2) DEFAULT 0 NOT NULL,
        public string TOT_ETC_AMT { get; set; } = "0";//NUMERIC(12,2) DEFAULT 0 NOT NULL,
        public string DCM_SALE_AMT { get; set; } = "0";//NUMERIC(12,2) DEFAULT 0 NOT NULL,
        public string VAT_SALE_AMT { get; set; } = "0";//NUMERIC(12,2) DEFAULT 0 NOT NULL,
        public string VAT_AMT { get; set; } = "0";//NUMERIC(12,2) DEFAULT 0 NOT NULL,
        public string NO_VAT_SALE_AMT { get; set; } = "0";//NUMERIC(12,2) DEFAULT 0 NOT NULL,
        public string NO_TAX_SALE_AMT { get; set; } = "0";//NUMERIC(12,2) DEFAULT 0 NOT NULL,
        public string EXP_PAY_AMT { get; set; } = "0";//NUMERIC(12,2) DEFAULT 0 NOT NULL,
        public string GST_PAY_AMT { get; set; } = "0";//NUMERIC(12,2) DEFAULT 0 NOT NULL,
        public string RET_PAY_AMT { get; set; } = "0";//NUMERIC(12,2) DEFAULT 0 NOT NULL,
        public string CASH_AMT { get; set; } = "0";//NUMERIC(12,2) DEFAULT 0 NOT NULL,
        public string CRD_CARD_AMT { get; set; } = "0";//NUMERIC(12,2) DEFAULT 0 NOT NULL,
        public string WES_AMT { get; set; } = "0";//NUMERIC(12,2) DEFAULT 0 NOT NULL,
        public string TK_GFT_AMT { get; set; } = "0";//NUMERIC(12,2) DEFAULT 0 NOT NULL,
        public string TK_FOD_AMT { get; set; } = "0";//NUMERIC(12,2) DEFAULT 0 NOT NULL,
        public string CST_POINT_AMT { get; set; } = "0";//NUMERIC(12,2) DEFAULT 0 NOT NULL,
        public string JCD_CARD_AMT { get; set; } = "0";//NUMERIC(12,2) DEFAULT 0 NOT NULL,
        public string RFC_AMT { get; set; } = "0";//NUMERIC(12,2) DEFAULT 0 NOT NULL,
        public string CST_NO { get; set; } = string.Empty;//VARCHAR(10), 
        public string CST_CARD_NO { get; set; } = string.Empty;//VARCHAR(40),
        public string CST_SALE_POINT { get; set; } = "0";//NUMERIC(9,0) DEFAULT 0 NOT NULL,
        public string DC_GEN_AMT { get; set; } = "0";//NUMERIC(12,2) DEFAULT 0 NOT NULL,
        public string DC_SVC_AMT { get; set; } = "0";//NUMERIC(12,2) DEFAULT 0 NOT NULL,
        public string DC_PCD_AMT { get; set; } = "0";//NUMERIC(12,2) DEFAULT 0 NOT NULL,
        public string DC_CPN_AMT { get; set; } = "0";//NUMERIC(12,2) DEFAULT 0 NOT NULL,
        public string DC_CST_AMT { get; set; } = "0";//NUMERIC(12,2) DEFAULT 0 NOT NULL,
        public string DC_TFD_AMT { get; set; } = "0";//NUMERIC(12,2) DEFAULT 0 NOT NULL,
        public string DC_PRM_AMT { get; set; } = "0";//NUMERIC(12,2) DEFAULT 0 NOT NULL,
        public string DC_CRD_AMT { get; set; } = "0";//NUMERIC(12,2) DEFAULT 0 NOT NULL,
        public string DC_PACK_AMT { get; set; } = "0";//NUMERIC(12,2) DEFAULT 0 NOT NULL,
        public string DC_LYT_AMT { get; set; } = "0";//NUMERIC(12,2) DEFAULT 0 NOT NULL,
        public string DC_YAP_AMT { get; set; } = "0";//NUMERIC(12,2) DEFAULT 0 NOT NULL,       

        public string REPAY_CASH_AMT { get; set; } = "0";//NUMERIC(12,2) DEFAULT 0 NOT NULL,
        public string REPAY_TK_GFT_AMT { get; set; } = "0";//NUMERIC(12,2) DEFAULT 0 NOT NULL,
        public string CASH_BILL_AMT { get; set; } = "0";//NUMERIC(12,2) DEFAULT 0 NOT NULL,
        public string FD_GST_CNT_T { get; set; } = "0";//NUMERIC(5,0) DEFAULT 0 NOT NULL,
        public string FD_GST_CNT_1 { get; set; } = "0";//NUMERIC(4,0) DEFAULT 0 NOT NULL,
        public string FD_GST_CNT_2 { get; set; } = "0";//NUMERIC(4,0) DEFAULT 0 NOT NULL,
        public string FD_GST_CNT_3 { get; set; } = "0";//NUMERIC(4,0) DEFAULT 0 NOT NULL,
        public string FD_GST_CNT_4 { get; set; } = "0";//NUMERIC(4,0) DEFAULT 0 NOT NULL,
        public string INSERT_DT { get; set; } = "";//VARCHAR(14) NOT NULL,
        public string EMP_NO { get; set; } = "";//VARCHAR(4) NOT NULL,
        public string ORDER_END_DT { get; set; } = string.Empty;//VARCHAR(14),
        public string BILL_NO { get; set; } = string.Empty;//VARCHAR(4),
        public string SEND_FLAG { get; set; } = "0";//VARCHAR(1) DEFAULT '0' NOT NULL,
        public string SEND_DT { get; set; } = string.Empty;//VARCHAR(14),
        public string DELIVER_NO { get; set; } = string.Empty;//VARCHAR(12),
        public string RESERVED_NO { get; set; } = string.Empty;//VARCHAR(10),
        public string KITCHEN_MEMO { get; set; } = string.Empty;//VARCHAR(4000),
        public string CST_NAME { get; set; } = string.Empty;//VARCHAR(24),
        public string NEW_DLV_ADDR_YN { get; set; } = "N";//VARCHAR(1) DEFAULT 'N' NOT NULL,
        public string DLV_ADDR { get; set; } = "";//VARCHAR(80),
        public string DLV_ADDR_DTL { get; set; } = "";//VARCHAR(88),
        public string NEW_DLV_TEL_NO_YN { get; set; } = "N";//VARCHAR(1) DEFAULT 'N' NOT NULL,
        public string DLV_TEL_NO { get; set; } = "";//VARCHAR(24),
        public string DLV_PAY_TYPE_FLAG { get; set; } = "0";//VARCHAR(1) DEFAULT '0' NOT NULL,
        public string DLV_IDT_NO { get; set; } = "";//VARCHAR(50),
        public string DLV_EMP_NO { get; set; } = "";//VARCHAR(4),
        public string DLV_REMARK { get; set; } = "";//VARCHAR(100),
        public string DLV_START_DT { get; set; } = "";//VARCHAR(14),
        public string DLV_PAYIN_EMP_NO { get; set; } = "";//VARCHAR(4),
        public string DLV_PAYIN_DT { get; set; } = "";//VARCHAR(14),
        public string DLV_BOWLIN_YN { get; set; } = "N";//VARCHAR(1) DEFAULT 'N' NOT NULL,
        public string DLV_BOWLIN_EMP_NO { get; set; } = "";//VARCHAR(4),
        public string DLV_BOWLIN_DT { get; set; } = "";//VARCHAR(14),
        public string DLV_CL_CODE { get; set; } = "";//VARCHAR(3),
        public string DLV_CM_CODE { get; set; } = "";//VARCHAR(5),
        public string TRAVEL_CODE { get; set; } = "";//VARCHAR(6),
        public string RSV_NO { get; set; } = "";//VARCHAR(10),
        public string PRE_PAY_CASH { get; set; } = "0";//string.Empty;//NUMERIC(12,2),
        public string PRE_PAY_CARD { get; set; } = "0";//string.Empty;//NUMERIC(12,2),
        public string RSV_USER_NAME { get; set; } = "";//VARCHAR(40),
        public string RSV_USER_TEL_NO { get; set; } = "";//VARCHAR(24),
        public string CST_AVL_POINT { get; set; } = "0"; //string.Empty;//NUMERIC(9,0),
        public string PAGER_BELL_NO { get; set; } = "";//VARCHAR(4),
        public string AFFILIATE_CODE { get; set; } = "";//VARCHAR(6),
        public string LOCAL_POINT_YN { get; set; } = "";//VARCHAR(1),
        public string MCP_AMT { get; set; } = "0";//string.Empty;//NUMERIC(12,2),
        public string PCD_CARD_AMT { get; set; } = "0";//string.Empty;//NUMERIC(12,2),
        public string CST_ANN_POINT { get; set; } = "0";//NUMERIC(9,0) DEFAULT 0 NOT NULL,
        public string CST_NEW_POINT { get; set; } = "0";//NUMERIC(9,0) DEFAULT 0 NOT NULL,
        public string CST_DATA { get; set; } = "";//VARCHAR(4096),
        public string CST_USE_POINT { get; set; } = "0";//NUMERIC(9,0) DEFAULT 0 NOT NULL,
        public string ORG_ORDER_NO { get; set; } = "";//VARCHAR(4),
        public string ORG_FD_TBL_CODE { get; set; } = "";//VARCHAR(3),
        public string FD_GST_FLAG_YN { get; set; } = "";//VARCHAR(1),
        public string FD_GST_FLAG_1 { get; set; } = "";//VARCHAR(2),
        public string FD_GST_FLAG_2 { get; set; } = "";//VARCHAR(2),
        public string PPC_CARD_AMT { get; set; } = "0";//NUMERIC(12,2) DEFAULT 0,
        public string EGIFT_AMT { get; set; } = "0";//NUMERIC(12,2) DEFAULT 0,
        public string MOBILE_ORDER_FLAG { get; set; } = "00";//VARCHAR(2) DEFAULT '00',
        public string MOBILE_ORDER_NO { get; set; } = "";//VARCHAR(20) DEFAULT '',
        public string MOBILE_REG_NO { get; set; } = "";//VARCHAR(4) DEFAULT '',
        public string RFND_TYPE_CODE { get; set; } = "";//VARCHAR(1),
        public string TAX_RFND_AMT { get; set; } = "0";//NUMERIC(12,2) DEFAULT 0 NOT NULL,
        public string TAX_RFND_FEE { get; set; } = "0";//NUMERIC(12,2) DEFAULT 0 NOT NULL,
        public string DC_TAX_AMT { get; set; } = "0";//NUMERIC(12,2) DEFAULT 0 NOT NULL,
        public string LG_SEND_FLAG { get; set; } = "0";//VARCHAR(1) DEFAULT '0' NOT NULL,
        public string LG_SEND_DT { get; set; } = "";//VARCHAR(14),
        public string DUTCH_ORDER_NO { get; set; } = "";//VARCHAR(4) DEFAULT '',
        public string O2O_AMT { get; set; } = "0";//NUMERIC(12,2) DEFAULT 0 NOT NULL,
        public string DLV_SAFE_TEL_NO { get; set; } = "";//VARCHAR(15),
        public string DLV_EXPIRE_TIME { get; set; } = "";//VARCHAR(14),
        public string DCC_EX_RATE_CODE { get; set; } = "";//VARCHAR(1),
        public string DCC_RCV_TEXT { get; set; } = "";//VARCHAR(100),
        public string EXTERN_GUEST_FLAG { get; set; } = "0";//VARCHAR(1) DEFAULT '0',
        public string SP_PAY_AMT { get; set; } = "0";//NUMERIC(12,2) DEFAULT 0 NOT NULL,
        public string HOOK_ORDER_CHANNEL_TYPE { get; set; } = null;//VARCHAR(30) DEFAULT '',
        public string DEPOSIT_AMT { get; set; } = "0";//NUMERIC(12,2) DEFAULT 0 NOT NULL

        public string MOBILE_PACK_AMT { get; set; } = "0";//NUMERIC(12,2) DEFAULT 0 NOT NULL


        // OUT PUT용 파라미터 추가
        public string PO_ORDER_NO { get; set; } = "";//VARCHAR(4)

        // 헤더 리턴값
        public int PO_RET_FLAG { get; set; } = 0;

        //배달 파라미터 추가
        public string NO { get; set; } = "";//그리드순번
        public string DLV_EMP_NAME { get; set; } = "";//배달기사명
        public string DLV_ADDR_TOTAL { get; set; } = "";//배달주소 합

        //오더 테이블 추가
        public string FD_TBL_NAME { get; set; } = "";//VARCHAR(3) DEFAULT '000' NOT NULL,

        //예약 조회 여행사명
        public string TRAVEL_NAME { get; set; } = "";//VARCHAR(6),

        // 영수증목록
        public string DC_FLAG { get; set; } = "";         // 할인 


        // 포스데이터 관리
        public string SALE_QTY { get; set; } = ""; // 영수건수        
        public string TOT_AMT { get; set; } = "";


        // 매출자료 수신 MAIN
        public string SALE_QTY_L { get; set; } = ""; // 로컬영수건수
        public string TOT_SALE_AMT_L { get; set; } = ""; // 로컬 총매출액
        public string DCM_SALE_AMT_L { get; set; } = ""; // 로컬 총할인액
        public string TOT_AMT_L { get; set; } = ""; // 로컬 실매출액


        // 매출자료 수신 DETAIL
        public string SALE_YN { get; set; } = ""; // 매출 /반품
    }


}

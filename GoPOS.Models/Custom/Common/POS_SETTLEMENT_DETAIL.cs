using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


// 주문 결제 순서 테이블 모델

namespace GoPOS.Models
{
    public class POS_SETTLEMENT_DETAIL
    {
        public POS_SETTLEMENT_DETAIL()
        {
            /*
            CREATE TABLE POS_SETTLEMENT_DETAIL (
            SHOP_CODE              VARCHAR(6) NOT NULL,
            SALE_DATE              VARCHAR(8) NOT NULL,
            POS_NO                 VARCHAR(2) NOT NULL,
            REGI_SEQ               VARCHAR(2) NOT NULL,
            EMP_NO                 VARCHAR(4) NOT NULL,
            CLOSE_FLAG             VARCHAR(1) NOT NULL,
            OPEN_DT                VARCHAR(14) NOT NULL,
            CLOSE_DT               VARCHAR(14),
            TOT_BILL_CNT           NUMERIC(5,0) DEFAULT 0 NOT NULL,
            TOT_SALE_AMT           NUMERIC(12,2) DEFAULT 0 NOT NULL,
            TOT_DC_AMT             NUMERIC(12,2) DEFAULT 0 NOT NULL,
            SVC_TIP_AMT            NUMERIC(12,2) DEFAULT 0 NOT NULL,
            TOT_ETC_AMT            NUMERIC(12,2) DEFAULT 0 NOT NULL,
            DCM_SALE_AMT           NUMERIC(12,2) DEFAULT 0 NOT NULL,
            VAT_SALE_AMT           NUMERIC(12,2) DEFAULT 0 NOT NULL,
            VAT_AMT                NUMERIC(12,2) DEFAULT 0 NOT NULL,
            NO_VAT_SALE_AMT        NUMERIC(12,2) DEFAULT 0 NOT NULL,
            NO_TAX_SALE_AMT        NUMERIC(12,2) DEFAULT 0 NOT NULL,
            RET_BILL_CNT           NUMERIC(5,0) DEFAULT 0 NOT NULL,
            RET_BILL_AMT           NUMERIC(12,2) DEFAULT 0 NOT NULL,
            VISIT_CST_CNT          NUMERIC(9,0) DEFAULT 0 NOT NULL,
            POS_READY_AMT          NUMERIC(12,2) DEFAULT 0 NOT NULL,
            POS_CSH_IN_AMT         NUMERIC(12,2) DEFAULT 0 NOT NULL,
            POS_CSH_OUT_AMT        NUMERIC(12,2) DEFAULT 0 NOT NULL,
            WEA_IN_CSH_AMT         NUMERIC(12,2) DEFAULT 0 NOT NULL,
            WEA_IN_CRD_AMT         NUMERIC(12,2) DEFAULT 0 NOT NULL,
            TK_GFT_SALE_CSH_AMT    NUMERIC(12,2) DEFAULT 0 NOT NULL,
            TK_GFT_SALE_CRD_AMT    NUMERIC(12,2) DEFAULT 0 NOT NULL,
            TK_FOD_SALE_CSH_AMT    NUMERIC(12,2) DEFAULT 0 NOT NULL,
            TK_FOD_SALE_CRD_AMT    NUMERIC(12,2) DEFAULT 0 NOT NULL,
            CASH_CNT               NUMERIC(9,0) DEFAULT 0 NOT NULL,
            CASH_AMT               NUMERIC(12,2) DEFAULT 0 NOT NULL,
            CASH_BILL_CNT          NUMERIC(9,0) DEFAULT 0 NOT NULL,
            CASH_BILL_AMT          NUMERIC(12,2) DEFAULT 0 NOT NULL,
            CRD_CARD_CNT           NUMERIC(9,0) DEFAULT 0 NOT NULL,
            CRD_CARD_AMT           NUMERIC(12,2) DEFAULT 0 NOT NULL,
            WES_CNT                NUMERIC(9,0) DEFAULT 0 NOT NULL,
            WES_AMT                NUMERIC(12,2) DEFAULT 0 NOT NULL,
            TK_GFT_CNT             NUMERIC(9,0) DEFAULT 0 NOT NULL,
            TK_GFT_AMT             NUMERIC(12,2) DEFAULT 0 NOT NULL,
            TK_FOD_CNT             NUMERIC(9,0) DEFAULT 0 NOT NULL,
            TK_FOD_AMT             NUMERIC(12,2) DEFAULT 0 NOT NULL,
            CST_POINT_CNT          NUMERIC(9,0) DEFAULT 0 NOT NULL,
            CST_POINT_AMT          NUMERIC(12,2) DEFAULT 0 NOT NULL,
            JCD_CARD_CNT           NUMERIC(9,0) DEFAULT 0 NOT NULL,
            JCD_CARD_AMT           NUMERIC(12,2) DEFAULT 0 NOT NULL,
            RFC_CNT                NUMERIC(9,0) DEFAULT 0 NOT NULL,
            RFC_AMT                NUMERIC(12,2) DEFAULT 0 NOT NULL,
            DC_GEN_CNT             NUMERIC(9,0) DEFAULT 0 NOT NULL,
            DC_GEN_AMT             NUMERIC(12,2) DEFAULT 0 NOT NULL,
            DC_SVC_CNT             NUMERIC(9,0) DEFAULT 0 NOT NULL,
            DC_SVC_AMT             NUMERIC(12,2) DEFAULT 0 NOT NULL,
            DC_JCD_CNT             NUMERIC(9,0) DEFAULT 0 NOT NULL,
            DC_JCD_AMT             NUMERIC(12,2) DEFAULT 0 NOT NULL,
            DC_CPN_CNT             NUMERIC(9,0) DEFAULT 0 NOT NULL,
            DC_CPN_AMT             NUMERIC(12,2) DEFAULT 0 NOT NULL,
            DC_CST_CNT             NUMERIC(9,0) DEFAULT 0 NOT NULL,
            DC_CST_AMT             NUMERIC(12,2) DEFAULT 0 NOT NULL,
            DC_TFD_CNT             NUMERIC(9,0) DEFAULT 0 NOT NULL,
            DC_TFD_AMT             NUMERIC(12,2) DEFAULT 0 NOT NULL,
            DC_PRM_CNT             NUMERIC(9,0) DEFAULT 0 NOT NULL,
            DC_PRM_AMT             NUMERIC(12,2) DEFAULT 0 NOT NULL,
            DC_CRD_CNT             NUMERIC(9,0) DEFAULT 0 NOT NULL,
            DC_CRD_AMT             NUMERIC(12,2) DEFAULT 0 NOT NULL,
            DC_PACK_CNT            NUMERIC(9,0) DEFAULT 0 NOT NULL,
            DC_PACK_AMT            NUMERIC(12,2) DEFAULT 0 NOT NULL,
            REM_CHECK_CNT          NUMERIC(9,0) DEFAULT 0 NOT NULL,
            REM_CHECK_AMT          NUMERIC(12,2) DEFAULT 0 NOT NULL,
            REM_W100000_CNT        NUMERIC(9,0) DEFAULT 0 NOT NULL,
            REM_W50000_CNT         NUMERIC(9,0) DEFAULT 0 NOT NULL,
            REM_W10000_CNT         NUMERIC(9,0) DEFAULT 0 NOT NULL,
            REM_W5000_CNT          NUMERIC(9,0) DEFAULT 0 NOT NULL,
            REM_W1000_CNT          NUMERIC(9,0) DEFAULT 0 NOT NULL,
            REM_W500_CNT           NUMERIC(9,0) DEFAULT 0 NOT NULL,
            REM_W100_CNT           NUMERIC(9,0) DEFAULT 0 NOT NULL,
            REM_W50_CNT            NUMERIC(9,0) DEFAULT 0 NOT NULL,
            REM_W10_CNT            NUMERIC(9,0) DEFAULT 0 NOT NULL,
            REM_CASH_AMT           NUMERIC(12,2) DEFAULT 0 NOT NULL,
            REM_TK_GFT_CNT         NUMERIC(9,0) DEFAULT 0 NOT NULL,
            REM_TK_GFT_AMT         NUMERIC(12,2) DEFAULT 0 NOT NULL,
            REM_TK_FOD_CNT         NUMERIC(9,0) DEFAULT 0 NOT NULL,
            REM_TK_FOD_AMT         NUMERIC(12,2) DEFAULT 0 NOT NULL,
            ETC_TK_FOD_AMT         NUMERIC(12,2) DEFAULT 0 NOT NULL,
            LOSS_CASH_AMT          NUMERIC(12,2) DEFAULT 0 NOT NULL,
            LOSS_TK_GFT_AMT        NUMERIC(12,2) DEFAULT 0 NOT NULL,
            LOSS_TK_FOD_AMT        NUMERIC(12,2) DEFAULT 0 NOT NULL,
            REPAY_CASH_CNT         NUMERIC(9,0) DEFAULT 0 NOT NULL,
            REPAY_CASH_AMT         NUMERIC(12,2) DEFAULT 0 NOT NULL,
            REPAY_TK_GFT_CNT       NUMERIC(9,0) DEFAULT 0 NOT NULL,
            REPAY_TK_GFT_AMT       NUMERIC(12,2) DEFAULT 0 NOT NULL,
            INSERT_DT              VARCHAR(14) NOT NULL,
            SEND_FLAG              VARCHAR(1) DEFAULT '0' NOT NULL,
            SEND_DT                VARCHAR(14),
            MCP_CNT                NUMERIC(9,0),
            MCP_AMT                NUMERIC(12,2),
            PCD_CARD_CNT           NUMERIC(9,0),
            PCD_CARD_AMT           NUMERIC(12,2),
            PPC_CARD_AMT           NUMERIC(12,2) DEFAULT 0,
            PPC_CARD_CNT           NUMERIC(9,0) DEFAULT 0,
            PPC_CARD_SALE_CSH_AMT  NUMERIC(12,2) DEFAULT 0,
            PPC_CARD_SALE_CRD_AMT  NUMERIC(12,2) DEFAULT 0,
            TAX_RFND_CNT           NUMERIC(9,0) DEFAULT 0 NOT NULL,
            TAX_RFND_AMT           NUMERIC(12,2) DEFAULT 0 NOT NULL,
            TAX_RFND_FEE           NUMERIC(12,2) DEFAULT 0 NOT NULL,
            DC_TAX_CNT             NUMERIC(12,2) DEFAULT 0 NOT NULL,
            DC_TAX_AMT             NUMERIC(12,2) DEFAULT 0 NOT NULL,
            O2O_CNT                NUMERIC(9,0) DEFAULT 0 NOT NULL,
            O2O_AMT                NUMERIC(12,2) DEFAULT 0 NOT NULL,
            EGIFT_CNT              NUMERIC(9,0) DEFAULT 0 NOT NULL,
            EGIFT_AMT              NUMERIC(12,2) DEFAULT 0 NOT NULL,
            SP_PAY_AMT             NUMERIC(12,2) DEFAULT 0,
            SP_PAY_CNT             NUMERIC(9,0) DEFAULT 0,
            DEPOSIT_AMT            NUMERIC(12,2) DEFAULT 0,
            DEPOSIT_CNT            NUMERIC(9,0) DEFAULT 0,
            REFUND_AMT             NUMERIC(12,2) DEFAULT 0,
            REFUND_CNT             NUMERIC(9,0) DEFAULT 0
            );

            Primary keys                                
            ALTER TABLE POS_SETTLEMENT_DETAIL ADD CONSTRAINT POS_REGIS_PK PRIMARY KEY(SHOP_CODE, SALE_DATE, POS_NO, REGI_SEQ);

            Descriptions
            COMMENT ON TABLE POS_SETTLEMENT_DETAIL  "POS-정산내역';

            Fields descriptions                             
            SHOP_CODE             //  "매장코드';
            SALE_DATE             //  "영업일자';
            POS_NO                //  "포스번호';
            REGI_SEQ              //  "정산차수';
            EMP_NO                //  "판매원번호';
            CLOSE_FLAG            //  "포스마감구분(CMM_CODE:062)1:개점2:중간마감3:일마감';
            OPEN_DT               //  "개점일시';
            CLOSE_DT              //  "마감일시';
            TOT_BILL_CNT          //  "총영수건수';
            TOT_SALE_AMT          //  "총매출액';
            TOT_DC_AMT            //  "총할인액';
            SVC_TIP_AMT           //  "봉사료';
            TOT_ETC_AMT           //  "기타에누리액(식권짜투리/에누리-끝전)';
            DCM_SALE_AMT          //  "실매출액';
            VAT_SALE_AMT          //  "과세매출액';
            VAT_AMT               //  "부가세액';
            NO_VAT_SALE_AMT       //  "면세매출액';
            NO_TAX_SALE_AMT       //  "순매출액';
            RET_BILL_CNT          //  "취소매출건수';
            RET_BILL_AMT          //  "취소매출액';
            VISIT_CST_CNT         //  "방문손님수';
            POS_READY_AMT         //  "영업준비금';
            POS_CSH_IN_AMT        //  "시재입금액';
            POS_CSH_OUT_AMT       //  "시재출금액';
            WEA_IN_CSH_AMT        //  "외상입금액-현금';
            WEA_IN_CRD_AMT        //  "외상입금액-신용카드';
            TK_GFT_SALE_CSH_AMT   //  "상품권판매액-현금';
            TK_GFT_SALE_CRD_AMT   //  "상품권판매액-신용카드';
            TK_FOD_SALE_CSH_AMT   //  "식권판매액-현금';
            TK_FOD_SALE_CRD_AMT   //  "식권판매액-신용카드';
            CASH_CNT              //  "결제건수-현금';
            CASH_AMT              //  "결제액-현금';
            CASH_BILL_CNT         //  "결제건수-현금영수증';
            CASH_BILL_AMT         //  "결제액-현금영수증';
            CRD_CARD_CNT          //  "결제건수-신용카드';
            CRD_CARD_AMT          //  "결제액-신용카드';
            WES_CNT               //  "결제건수-외상';
            WES_AMT               //  "결제액-외상';
            TK_GFT_CNT            //  "결제건수-상품권';
            TK_GFT_AMT            //  "결제액-상품권';
            TK_FOD_CNT            //  "결제건수-식권';
            TK_FOD_AMT            //  "결제액-식권';
            CST_POINT_CNT         //  "결제건수-회원포인트';
            CST_POINT_AMT         //  "결제액-회원포인트';
            JCD_CARD_CNT          //  "결제건수-제휴카드';
            JCD_CARD_AMT          //  "결제액-제휴카드';
            RFC_CNT               //  "결제건수-사원카드';
            RFC_AMT               //  "결제액-사원카드';
            DC_GEN_CNT            //  "할인건수-일반';
            DC_GEN_AMT            //  "할인액-일반';
            DC_SVC_CNT            //  "할인건수-서비스';
            DC_SVC_AMT            //  "할인액-서비스';
            DC_JCD_CNT            //  "할인건수-제휴카드';
            DC_JCD_AMT            //  "할인액-제휴카드';
            DC_CPN_CNT            //  "할인건수-쿠폰';
            DC_CPN_AMT            //  "할인액-쿠폰';
            DC_CST_CNT            //  "할인건수-회원';
            DC_CST_AMT            //  "할인액-회원';
            DC_TFD_CNT            //  "할인건수-식권';
            DC_TFD_AMT            //  "할인액-식권';
            DC_PRM_CNT            //  "할인건수-프로모션';
            DC_PRM_AMT            //  "할인액-프로모션';
            DC_CRD_CNT            //  "할인건수-신용카드현장할인';
            DC_CRD_AMT            //  "할인액-신용카드현장할인';
            DC_PACK_CNT           //  "할인건수-포장할인';
            DC_PACK_AMT           //  "할인액-포장할인';
            REM_CHECK_CNT         //  "수표수량';
            REM_CHECK_AMT         //  "수표금액';
            REM_W100000_CNT       //  "십만원권수량';
            REM_W50000_CNT        //  "오만원권수량';
            REM_W10000_CNT        //  "만원권수량';
            REM_W5000_CNT         //  "오천원권수량';
            REM_W1000_CNT         //  "천원권수량';
            REM_W500_CNT          //  "오백원권수량';
            REM_W100_CNT          //  "백원권수량';
            REM_W50_CNT           //  "오십원권수량';
            REM_W10_CNT           //  "십원권수량';
            REM_CASH_AMT          //  "마감현금시재액';
            REM_TK_GFT_CNT        //  "상품권시재건수';
            REM_TK_GFT_AMT        //  "상품권시재금액';
            REM_TK_FOD_CNT        //  "식권시재건수';
            REM_TK_FOD_AMT        //  "식권시재금액';
            ETC_TK_FOD_AMT        //  "식권-짜투리';
            LOSS_CASH_AMT         //  "현금과부족';
            LOSS_TK_GFT_AMT       //  "상품권과부족';
            LOSS_TK_FOD_AMT       //  "식권과부족';
            REPAY_CASH_CNT        //  "상품권-현금환불건수';
            REPAY_CASH_AMT        //  "상품권-현금환불액';
            REPAY_TK_GFT_CNT      //  "상품권-환불건수';
            REPAY_TK_GFT_AMT      //  "상품권-환불액';
            INSERT_DT             //  "등록일시';
            SEND_FLAG             //  "서버전송여부';
            SEND_DT               //  "서버전송일시';
            MCP_CNT               //  "결제건수-모바일쿠폰';
            MCP_AMT               //  "결제액-모바일쿠폰';
            PCD_CARD_CNT          //  "결제건수-교통선불카드(캐시비)';
            PCD_CARD_AMT          //  "결제액-교통선불카드(캐시비)';
            PPC_CARD_AMT          //  "결제액-선불카드';
            PPC_CARD_CNT          //  "결제건수-선불카드';
            PPC_CARD_SALE_CSH_AMT //  "선불카드충전액-현금';
            PPC_CARD_SALE_CRD_AMT //  "선불카드충전액-카드';
            TAX_RFND_CNT          //  "환급건수';
            TAX_RFND_AMT          //  "환급예정액';
            TAX_RFND_FEE          //  "환급수수료';
            DC_TAX_CNT            //  "환급할인건수';
            DC_TAX_AMT            //  "환급할인액';
            O2O_CNT               //  "결제건수-온라인매출';
            O2O_AMT               //  "결제액-온라인매출';
            EGIFT_CNT             //  "결제건수-전자상품권';
            EGIFT_AMT             //  "결제액-전자상품권';
            SP_PAY_AMT            //  "결제액-간편결제';
            SP_PAY_CNT            //  "결제건수-간편결제';
            DEPOSIT_AMT           //  "결제액-보증금결제';
            DEPOSIT_CNT           //  "결제건수-보증금결제';
            REFUND_AMT            //  "결제액-보증반환금액';
            REFUND_CNT            //  "결제건수-보증반환건수';

            */
        }

        public POS_SETTLEMENT_DETAIL(string shop_code, string sale_date, string order_no, string pos_no, string regi_seq)
        {
            this.SHOP_CODE = shop_code;
            this.SALE_DATE = sale_date;
            this.POS_NO = pos_no;
            this.REGI_SEQ = regi_seq;
        }

        public string SHOP_CODE             { get; set; } = string.Empty;     // "매장코드';
        public string SALE_DATE             { get; set; } = string.Empty;     // "영업일자';
        public string POS_NO                { get; set; } = string.Empty;     // "포스번호';
        public string REGI_SEQ              { get; set; } = string.Empty;     // "정산차수';
        public string NEW_REGI_SEQ          { get; set; } = string.Empty;     // customed field
        public string EMP_NO                { get; set; } = string.Empty;     // "판매원번호';
        public string EMP_NAME              { get; set; } = string.Empty;     // customed field
        public string CLOSE_FLAG            { get; set; } = string.Empty;     // "포스마감구분(CMM_CODE:062)1:개점2:중간마감3:일마감';
        public string OPEN_DT               { get; set; } = string.Empty;     // "개점일시';
        public string CLOSE_DT              { get; set; } = string.Empty;     // "마감일시';
        public string TOT_BILL_CNT          { get; set; } = string.Empty;     // "총영수건수';
        public string TOT_SALE_AMT          { get; set; } = string.Empty;     // "총매출액';
        public string TOT_DC_AMT            { get; set; } = string.Empty;     // "총할인액';
        public string SVC_TIP_AMT           { get; set; } = string.Empty;     // "봉사료';
        public string TOT_ETC_AMT           { get; set; } = string.Empty;     // "기타에누리액(식권짜투리/에누리-끝전)';
        public string DCM_SALE_AMT          { get; set; } = string.Empty;     // "실매출액';
        public string VAT_SALE_AMT          { get; set; } = string.Empty;     // "과세매출액';
        public string VAT_AMT               { get; set; } = string.Empty;     // "부가세액';
        public string NO_VAT_SALE_AMT       { get; set; } = string.Empty;     // "면세매출액';
        public string NO_TAX_SALE_AMT       { get; set; } = string.Empty;     // "순매출액';
        public string RET_BILL_CNT          { get; set; } = string.Empty;     // "취소매출건수';
        public string RET_BILL_AMT          { get; set; } = string.Empty;     // "취소매출액';
        public string VISIT_CST_CNT         { get; set; } = string.Empty;     // "방문손님수';
        public string POS_READY_AMT         { get; set; } = string.Empty;     // "영업준비금';
        public string POS_CSH_IN_AMT        { get; set; } = string.Empty;     // "시재입금액';
        public string POS_CSH_OUT_AMT       { get; set; } = string.Empty;     // "시재출금액';
        public string WEA_IN_CSH_AMT        { get; set; } = string.Empty;     // "외상입금액-현금';
        public string WEA_IN_CRD_AMT        { get; set; } = string.Empty;     // "외상입금액-신용카드';
        public string TK_GFT_SALE_CSH_AMT   { get; set; } = string.Empty;     // "상품권판매액-현금';
        public string TK_GFT_SALE_CRD_AMT   { get; set; } = string.Empty;     // "상품권판매액-신용카드';
        public string TK_FOD_SALE_CSH_AMT   { get; set; } = string.Empty;     // "식권판매액-현금';
        public string TK_FOD_SALE_CRD_AMT   { get; set; } = string.Empty;     // "식권판매액-신용카드';
        public string CASH_CNT              { get; set; } = string.Empty;     // "결제건수-현금';
        public string CASH_AMT              { get; set; } = string.Empty;     // "결제액-현금';
        public string CASH_BILL_CNT         { get; set; } = string.Empty;     // "결제건수-현금영수증';
        public string CASH_BILL_AMT         { get; set; } = string.Empty;     // "결제액-현금영수증';
        public string CRD_CARD_CNT          { get; set; } = string.Empty;     // "결제건수-신용카드';
        public string CRD_CARD_AMT          { get; set; } = string.Empty;     // "결제액-신용카드';
        public string WES_CNT               { get; set; } = string.Empty;     // "결제건수-외상';
        public string WES_AMT               { get; set; } = string.Empty;     // "결제액-외상';
        public string TK_GFT_CNT            { get; set; } = string.Empty;     // "결제건수-상품권';
        public string TK_GFT_AMT            { get; set; } = string.Empty;     // "결제액-상품권';
        public string TK_FOD_CNT            { get; set; } = string.Empty;     // "결제건수-식권';
        public string TK_FOD_AMT            { get; set; } = string.Empty;     // "결제액-식권';
        public string CST_POINT_CNT         { get; set; } = string.Empty;     // "결제건수-회원포인트';
        public string CST_POINT_AMT         { get; set; } = string.Empty;     // "결제액-회원포인트';
        public string JCD_CARD_CNT          { get; set; } = string.Empty;     // "결제건수-제휴카드';
        public string JCD_CARD_AMT          { get; set; } = string.Empty;     // "결제액-제휴카드';
        public string RFC_CNT               { get; set; } = string.Empty;     // "결제건수-사원카드';
        public string RFC_AMT               { get; set; } = string.Empty;     // "결제액-사원카드';
        public string DC_GEN_CNT            { get; set; } = string.Empty;     // "할인건수-일반';
        public string DC_GEN_AMT            { get; set; } = string.Empty;     // "할인액-일반';
        public string DC_SVC_CNT            { get; set; } = string.Empty;     // "할인건수-서비스';
        public string DC_SVC_AMT            { get; set; } = string.Empty;     // "할인액-서비스';
        public string DC_JCD_CNT            { get; set; } = string.Empty;     // "할인건수-제휴카드';
        public string DC_JCD_AMT            { get; set; } = string.Empty;     // "할인액-제휴카드';
        public string DC_CPN_CNT            { get; set; } = string.Empty;     // "할인건수-쿠폰';
        public string DC_CPN_AMT            { get; set; } = string.Empty;     // "할인액-쿠폰';
        public string DC_CST_CNT            { get; set; } = string.Empty;     // "할인건수-회원';
        public string DC_CST_AMT            { get; set; } = string.Empty;     // "할인액-회원';
        public string DC_TFD_CNT            { get; set; } = string.Empty;     // "할인건수-식권';
        public string DC_TFD_AMT            { get; set; } = string.Empty;     // "할인액-식권';
        public string DC_PRM_CNT            { get; set; } = string.Empty;     // "할인건수-프로모션';
        public string DC_PRM_AMT            { get; set; } = string.Empty;     // "할인액-프로모션';
        public string DC_CRD_CNT            { get; set; } = string.Empty;     // "할인건수-신용카드현장할인';
        public string DC_CRD_AMT            { get; set; } = string.Empty;     // "할인액-신용카드현장할인';
        public string DC_PACK_CNT           { get; set; } = string.Empty;     // "할인건수-포장할인';
        public string DC_PACK_AMT           { get; set; } = string.Empty;     // "할인액-포장할인';
        public string REM_CHECK_CNT         { get; set; } = string.Empty;     // "수표수량';
        public string REM_CHECK_AMT         { get; set; } = string.Empty;     // "수표금액';
        public string REM_W100000_CNT       { get; set; } = string.Empty;     // "십만원권수량';
        public string REM_W50000_CNT        { get; set; } = string.Empty;     // "오만원권수량';
        public string REM_W10000_CNT        { get; set; } = string.Empty;     // "만원권수량';
        public string REM_W5000_CNT         { get; set; } = string.Empty;     // "오천원권수량';
        public string REM_W1000_CNT         { get; set; } = string.Empty;     // "천원권수량';
        public string REM_W500_CNT          { get; set; } = string.Empty;     // "오백원권수량';
        public string REM_W100_CNT          { get; set; } = string.Empty;     // "백원권수량';
        public string REM_W50_CNT           { get; set; } = string.Empty;     // "오십원권수량';
        public string REM_W10_CNT           { get; set; } = string.Empty;     // "십원권수량';
        public string REM_CASH_AMT          { get; set; } = string.Empty;     // "마감현금시재액';
        public string REM_TK_GFT_CNT        { get; set; } = string.Empty;     // "상품권시재건수';
        public string REM_TK_GFT_AMT        { get; set; } = string.Empty;     // "상품권시재금액';
        public string REM_TK_FOD_CNT        { get; set; } = string.Empty;     // "식권시재건수';
        public string REM_TK_FOD_AMT        { get; set; } = string.Empty;     // "식권시재금액';
        public string ETC_TK_FOD_AMT        { get; set; } = string.Empty;     // "식권-짜투리';
        public string LOSS_CASH_AMT         { get; set; } = string.Empty;     // "현금과부족';
        public string LOSS_TK_GFT_AMT       { get; set; } = string.Empty;     // "상품권과부족';
        public string LOSS_TK_FOD_AMT       { get; set; } = string.Empty;     // "식권과부족';
        public string REPAY_CASH_CNT        { get; set; } = string.Empty;     // "상품권-현금환불건수';
        public string REPAY_CASH_AMT        { get; set; } = string.Empty;     // "상품권-현금환불액';
        public string REPAY_TK_GFT_CNT      { get; set; } = string.Empty;     // "상품권-환불건수';
        public string REPAY_TK_GFT_AMT      { get; set; } = string.Empty;     // "상품권-환불액';
        public string INSERT_DT             { get; set; } = string.Empty;     // "등록일시';
        public string SEND_FLAG             { get; set; } = string.Empty;     // "서버전송여부';
        public string SEND_DT               { get; set; } = string.Empty;     // "서버전송일시';
        public string MCP_CNT               { get; set; } = string.Empty;     // "결제건수-모바일쿠폰';
        public string MCP_AMT               { get; set; } = string.Empty;     // "결제액-모바일쿠폰';
        public string PCD_CARD_CNT          { get; set; } = string.Empty;     // "결제건수-교통선불카드(캐시비)';
        public string PCD_CARD_AMT          { get; set; } = string.Empty;     // "결제액-교통선불카드(캐시비)';
        public string PPC_CARD_AMT          { get; set; } = string.Empty;     // "결제액-선불카드';
        public string PPC_CARD_CNT          { get; set; } = string.Empty;     // "결제건수-선불카드';
        public string PPC_CARD_SALE_CSH_AMT { get; set; } = string.Empty;     // "선불카드충전액-현금';
        public string PPC_CARD_SALE_CRD_AMT { get; set; } = string.Empty;     // "선불카드충전액-카드';
        public string TAX_RFND_CNT          { get; set; } = string.Empty;     // "환급건수';
        public string TAX_RFND_AMT          { get; set; } = string.Empty;     // "환급예정액';
        public string TAX_RFND_FEE          { get; set; } = string.Empty;     // "환급수수료';
        public string DC_TAX_CNT            { get; set; } = string.Empty;     // "환급할인건수';
        public string DC_TAX_AMT            { get; set; } = string.Empty;     // "환급할인액';
        public string O2O_CNT               { get; set; } = string.Empty;     // "결제건수-온라인매출';
        public string O2O_AMT               { get; set; } = string.Empty;     // "결제액-온라인매출';
        public string EGIFT_CNT             { get; set; } = string.Empty;     // "결제건수-전자상품권';
        public string EGIFT_AMT             { get; set; } = string.Empty;     // "결제액-전자상품권';
        public string SP_PAY_AMT            { get; set; } = string.Empty;     // "결제액-간편결제';
        public string SP_PAY_CNT            { get; set; } = string.Empty;     // "결제건수-간편결제';
        public string DEPOSIT_AMT           { get; set; } = string.Empty;     // "결제액-보증금결제';
        public string DEPOSIT_CNT           { get; set; } = string.Empty;     // "결제건수-보증금결제';
        public string REFUND_AMT            { get; set; } = string.Empty;     // "결제액-보증반환금액';
        public string REFUND_CNT            { get; set; } = string.Empty;     // "결제건수-보증반환건수';

        //추가
        public string NO                    { get; set; } = string.Empty;     // "NO"
    }
}
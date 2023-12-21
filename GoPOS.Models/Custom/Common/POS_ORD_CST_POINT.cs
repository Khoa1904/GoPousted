using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace GoPOS.Models
{
    public class POS_ORD_CST_POINT
    {
        /// <summary>
        /// POS-주문결제-회원포인트

        /// </summary>
        public POS_ORD_CST_POINT()
        {
            /*
            CREATE TABLE POS_ORD_CST_POINT (
                SHOP_CODE       VARCHAR(6) NOT NULL,
                SALE_DATE       VARCHAR(8) NOT NULL,
                ORDER_NO        VARCHAR(4) NOT NULL,
                LINE_NO         VARCHAR(4) NOT NULL,
                POS_NO          VARCHAR(2) NOT NULL,
                SALE_YN         VARCHAR(1) DEFAULT 'Y' NOT NULL,
                CST_NO          VARCHAR(10),
                CST_CARD_NO     VARCHAR(40),
                CST_CLASS_CODE  VARCHAR(2),
                CST_USE_POINT   NUMERIC(12,2) DEFAULT 0 NOT NULL,
                INSERT_DT       VARCHAR(14) NOT NULL,
                EMP_NO          VARCHAR(4) NOT NULL,
                APPR_DATE       VARCHAR(8),
                APPR_NO         VARCHAR(16),
                CST_USE_AMT     NUMERIC(12,2) DEFAULT 0 NOT NULL,
                DEPOSIT_AMT     NUMERIC(12,2) DEFAULT 0 NOT NULL
            );


              //Primary keys                                

              ALTER TABLE POS_ORD_CST_POINT ADD CONSTRAINT POS_ODCST_PK PRIMARY KEY(SHOP_CODE, SALE_DATE, ORDER_NO, LINE_NO);


              //Descriptions                                

              COMMENT ON TABLE POS_ORD_CST_POINT IS'POS-주문결제-회원포인트';

              Fields descriptions                            

              COMMENT ON COLUMN POS_ORD_CST_POINT.SHOP_CODE      IS  '매장코드';
              COMMENT ON COLUMN POS_ORD_CST_POINT.SALE_DATE      IS  '영업일자';
              COMMENT ON COLUMN POS_ORD_CST_POINT.ORDER_NO       IS  '주문번호';
              COMMENT ON COLUMN POS_ORD_CST_POINT.LINE_NO        IS  '라인번호';
              COMMENT ON COLUMN POS_ORD_CST_POINT.POS_NO         IS  '포스번호';
              COMMENT ON COLUMN POS_ORD_CST_POINT.SALE_YN        IS  '판매구분 Y:정상 N:반품(취소)';
              COMMENT ON COLUMN POS_ORD_CST_POINT.CST_NO         IS  '회원-번호';
              COMMENT ON COLUMN POS_ORD_CST_POINT.CST_CARD_NO    IS  '회원-카드번호';
              COMMENT ON COLUMN POS_ORD_CST_POINT.CST_CLASS_CODE IS  '회원-등급분류코드';
              COMMENT ON COLUMN POS_ORD_CST_POINT.CST_USE_POINT  IS  '회원-사용포인트';
              COMMENT ON COLUMN POS_ORD_CST_POINT.INSERT_DT      IS  '등록일시';
              COMMENT ON COLUMN POS_ORD_CST_POINT.EMP_NO         IS  '판매원코드';
              COMMENT ON COLUMN POS_ORD_CST_POINT.APPR_NO        IS  '회원-사용-스탬프/포인트-금액';
              COMMENT ON COLUMN POS_ORD_CST_POINT.DEPOSIT_AMT    IS  '보증금결제금액';              */
        }

        public POS_ORD_CST_POINT(string shop_code, string sale_date, string order_no, string line_no)
        {
            this.SHOP_CODE = shop_code;
            this.SALE_DATE = sale_date;
            this.ORDER_NO = order_no;
            this.LINE_NO = line_no;
        }

        public string  SHOP_CODE        { get; set; } = string.Empty;   //'매장코드';
        public string  SALE_DATE        { get; set; } = string.Empty;   //'영업일자';
        public string  ORDER_NO         { get; set; } = string.Empty;   //'주문번호';
        public string  LINE_NO          { get; set; } = string.Empty;   //'라인번호';
        public string  POS_NO           { get; set; } = string.Empty;   //'포스번호';
        public string  SALE_YN          { get; set; } = string.Empty;   //'판매구분 Y:정상 N:반품(취소)';
        public string  CST_NO           { get; set; } = string.Empty;   //'회원-번호';
        public string  CST_CARD_NO      { get; set; } = string.Empty;   //'회원-카드번호';
        public string  CST_CLASS_CODE   { get; set; } = string.Empty;   //'회원-등급분류코드';
        public string  CST_USE_POINT    { get; set; } = string.Empty;   //'회원-사용포인트';
        public string  INSERT_DT        { get; set; } = string.Empty;   //'등록일시';
        public string  EMP_NO           { get; set; } = string.Empty;   //'판매원코드';
        public string  APPR_DATE        { get; set; } = string.Empty;   //
        public string  APPR_NO          { get; set; } = string.Empty;   //
        public string  CST_USE_AMT      { get; set; } = string.Empty;   //'회원-사용-스탬프/포인트-금액';
        public string  DEPOSIT_AMT      { get; set; } = string.Empty;   //'보증금결제금액';              


        // 추가        
        public string APPR_AMT { get; set; } = string.Empty; // //결제금액 APPR_AMT

        public string NO { get; set; } = string.Empty;
    }
}

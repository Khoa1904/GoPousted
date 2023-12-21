using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace GoPOS.Models
{
    public class POS_ORD_DLV
    {
        /// <summary>
        /// 매장-포스출력물관리

        /// </summary>
        public POS_ORD_DLV()
        {
            /*
            SHOP_CODE            VARCHAR(6) NOT NULL,
            SALE_DATE            VARCHAR(8) NOT NULL,
            POS_NO               VARCHAR(2) NOT NULL,
            ORDER_NO             VARCHAR(4) NOT NULL,
            LINE_NO              VARCHAR(4) NOT NULL,
            SALE_YN              VARCHAR(1) DEFAULT 'Y' NOT NULL,
            DLV_STATUS_FLAG      VARCHAR(2) NOT NULL,
            DLV_IF_FLAG          VARCHAR(2) NOT NULL,
            DLV_IF_NO            VARCHAR(60),
            DLV_EMP_TIME         VARCHAR(14),
            DLV_TIME             VARCHAR(14),
            DLV_EMP_NAME         VARCHAR(40),
            DLV_EMP_TEL          VARCHAR(40),
            DLV_IF_ADDR          VARCHAR(2048),
            DLV_REQ_TIME         VARCHAR(14),
            DLV_PICK_TIME        VARCHAR(14),
            DLV_FEE              NUMERIC(12,2),
            DLV_DISTANCE         NUMERIC(10,2),
            DLV_ADD_CHARGE       NUMERIC(12,2),
            DLV_REMARK           VARCHAR(500),
            INSERT_DT            VARCHAR(14) NOT NULL,
            EMP_NO               VARCHAR(4) NOT NULL,
            DLV_SUB_IF_FLAG      VARCHAR(10),
            DLV_SUB_AGENCY_NAME  VARCHAR(30)

            
            //Primary keys                                
            POS_ODDLV_PK PRIMARY KEY(SHOP_CODE, SALE_DATE, POS_NO, ORDER_NO, LINE_NO);
            //Descriptions                                
            COMMENT ON TABLE POS_ORD_DLV IS '매출TR-배달 I/F';

            Fields descriptions                             
            POS_ORD_DLV.SHOP_CODE IS'매장코드';
            POS_ORD_DLV.SALE_DATE IS'영업일자';
            POS_ORD_DLV.POS_NO IS'포스번호';
            POS_ORD_DLV.ORDER_NO IS'주문번호';
            POS_ORD_DLV.LINE_NO IS'라인번호';
            POS_ORD_DLV.SALE_YN IS'판매구분 Y:정상 N:반품(취소)';
            POS_ORD_DLV.DLV_STATUS_FLAG IS'배달 IF 상태 구분 ( 01:배송기사 지정중, 02:픽업중, 03:배달중, 04:배송완료, 99:배송취소)';
            POS_ORD_DLV.DLV_IF_FLAG IS'배달 IF 업체 구분 (CMM_CODE : 825) 01:부릉';
            POS_ORD_DLV.DLV_IF_NO IS'배송번호';
            POS_ORD_DLV.DLV_EMP_TIME IS'기사배정시간';
            POS_ORD_DLV.DLV_TIME IS'배송완료/취소 시간';
            POS_ORD_DLV.DLV_EMP_NAME IS'배송기사 이름';
            POS_ORD_DLV.DLV_EMP_TEL IS'배송기사 전화번호';
            POS_ORD_DLV.DLV_IF_ADDR IS'배달 IF 전송용 주소';
            POS_ORD_DLV.DLV_REQ_TIME IS'배달대행접수시간';
            POS_ORD_DLV.DLV_PICK_TIME IS'픽업시간';
            POS_ORD_DLV.DLV_FEE IS'배송료';
            POS_ORD_DLV.DLV_DISTANCE IS'배송거리';
            POS_ORD_DLV.DLV_ADD_CHARGE IS'할증';
            POS_ORD_DLV.DLV_REMARK IS'비고';
            POS_ORD_DLV.INSERT_DT IS'등록일시';
            POS_ORD_DLV.EMP_NO IS'판매원번호';
            POS_ORD_DLV.DLV_SUB_IF_FLAG IS'배달 IF 업체 하위 구분 (배달 업체가 OneToss인 경우 실제 배달업체 code)';
            POS_ORD_DLV.DLV_SUB_AGENCY_NAME IS'배달 IF 업체 하위 업체명 (배달 업체가 OneToss인 경우 실제 배달업체명)';
            */
        }

        public POS_ORD_DLV(string shop_code, string sale_date, string pos_no, string order_no, string line_no)
        {
            this.SHOP_CODE = shop_code;
            this.SALE_DATE = sale_date;
            this.POS_NO = pos_no;
            this.ORDER_NO = order_no;
            this.LINE_NO = line_no;
        }

        public string SHOP_CODE { get; set; } = string.Empty;//IS'매장코드';
        public string SALE_DATE { get; set; } = string.Empty;//IS'영업일자';
        public string POS_NO { get; set; } = string.Empty;//IS'포스번호';
        public string ORDER_NO { get; set; } = string.Empty;//IS'주문번호';
        public string LINE_NO { get; set; } = string.Empty;//IS'라인번호';
        public string SALE_YN { get; set; } = string.Empty;//IS'판매구분 Y:정상 N:반품(취소)';
        public string DLV_STATUS_FLAG { get; set; } = string.Empty;//IS'배달 IF 상태 구분 ( 01:배송기사 지정중, 02:픽업중, 03:배달중, 04:배송완료, 99:배송취소)';
        public string DLV_IF_FLAG { get; set; } = string.Empty;//IS'배달 IF 업체 구분 (CMM_CODE : 825) 01:부릉';
        public string DLV_IF_NO { get; set; } = string.Empty;//IS'배송번호';
        public string DLV_EMP_TIME { get; set; } = string.Empty;//IS'기사배정시간';
        public string DLV_TIME { get; set; } = string.Empty;//IS'배송완료/취소 시간';
        public string DLV_EMP_NAME { get; set; } = string.Empty;//IS'배송기사 이름';
        public string DLV_EMP_TEL { get; set; } = string.Empty;//IS'배송기사 전화번호';
        public string DLV_IF_ADDR { get; set; } = string.Empty;//IS'배달 IF 전송용 주소';
        public string DLV_REQ_TIME  { get; set; } = string.Empty;//IS'배달대행접수시간';
        public string DLV_PICK_TIME { get; set; } = string.Empty;//IS'픽업시간';
        public string DLV_FEE        { get; set; } = string.Empty;//IS'배송료';
        public string DLV_DISTANCE   { get; set; } = string.Empty;//IS'배송거리';
        public string DLV_ADD_CHARGE { get; set; } = string.Empty;//IS'할증';
        public string DLV_REMARK      { get; set; } = string.Empty;//IS'비고';
        public string INSERT_DT       { get; set; } = string.Empty;//IS'등록일시';
        public string EMP_NO          { get; set; } = string.Empty;//IS'판매원번호';
        public string DLV_SUB_IF_FLAG { get; set; } = string.Empty;//IS'배달 IF 업체 하위 구분 (배달 업체가 OneToss인 경우 실제 배달업체 code)';
        public string DLV_SUB_AGENCY_NAME { get; set; } = string.Empty;//IS'배달 IF 업체 하위 업체명 (배달 업체가 OneToss인 경우 실제 배달업체명)';

        //추가
        public string NO { get; set; } = string.Empty;

        public string ORDER_PRICE { get; set; } = string.Empty; // 주문금액

        public string WES_IN { get; set; } = string.Empty; // 입금

        public string PAY_METHOD { get; set; } = string.Empty; // 결제예정수단

        public string RETURN_FLAG { get; set; } = string.Empty; // 회수 상태

        public string RETURN_EMP_NAME { get; set; } = string.Empty; // 회수원

    }
}
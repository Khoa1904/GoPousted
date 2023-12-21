using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace GoPOS.Models
{
    public class GOODSORDERCANCLSTTUS
    {
        /// <summary>
        /// 상품별주문취소현황

        /// </summary>
        public GOODSORDERCANCLSTTUS()
        {
            /*
        상단:		
		SCLASS_NAME	   분류명	    
        TOT_SALE_QTY   총수량	        
        TOT_SALE_AMT   총매출	        
        TOT_DC_AMT	   총할인	
        DCM_SALE_AMT   실매출	        
        VAT_AMT	       부가세	
        TOT_AMT	       합계	
        OCC_RATE       점유율	
        
         하단:		
        
		SALE_DATE	  영업일자	
        ORDER_NO	  주문번호	
        ORDER_CNT	  주문자수	
        TOT_SALE_QTY  총수량	
        SALE_YN	      취소구분	
        BILL_NO	      영수증번호 	
        EMP_NO	      판매원	
        INSERT_DT     취소시각  

            */
        }

        public GOODSORDERCANCLSTTUS(string sale_date, string pos)
        {
            this.SALE_DATE = sale_date;
            this.POS = pos;
        }

        //상단:		
        public string SCLASS_NAME { get; set; } = string.Empty;// 분류명
        //public string TOT_SALE_QTY { get; set; } = string.Empty;// 총수량
        public string TOT_SALE_AMT { get; set; } = string.Empty;// 총매출
        public string TOT_DC_AMT { get; set; } = string.Empty;// 총할인
        public string TOT_DCM_SALE_AMT { get; set; } = string.Empty;// 실매출
        public string TOT_VAT_AMT { get; set; } = string.Empty;// 부가세
        public string TOT_AMT { get; set; } = string.Empty;// 합계
        public string OCC_RATE { get; set; } = string.Empty;// 점유율


        //하단:        
        public string SALE_DATE { get; set; } = string.Empty; // 영업일자
        public string ORDER_NO { get; set; } = string.Empty; // 주문번호
        public string ORDER_CNT { get; set; } = string.Empty; // 주문자수
        //public string TOT_SALE_QTY { get; set; } = string.Empty; // 총수량
        public string SALE_YN { get; set; } = string.Empty; // 취소구분
        public string BILL_NO { get; set; } = string.Empty; // 영수증번호
        public string EMP_NO { get; set; } = string.Empty; // 판매원
        public string INSERT_DT { get; set; } = string.Empty; // 취소시각


        // 추가
        public string TOT_SALE_QTY { get; set; } = string.Empty; // 총수량

        public string NO { get; set; } = string.Empty;

        public string POS { get; set; } = string.Empty;

    }
}

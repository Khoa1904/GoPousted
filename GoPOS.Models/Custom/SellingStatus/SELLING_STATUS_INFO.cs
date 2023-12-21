using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace GoPOS.Models
{
    public class SELLING_STATUS_INFO
    {
        /// <summary>
        /// 매출현황 공통 모델

        /// </summary>
        public SELLING_STATUS_INFO()
        {
            /*

            ClSelngSttusView      분류별 매출현황       -NO 분류명 총수량 총매출 총할인 실매출 부가세 합계 점유율  -NO 영업일자 총수량 총매출 총할인 실매출 부가세 합계 점유율

            GoodsSelngSttusView   상품별 매출현황       -NO 분류명 총수량 총매출 총할인 실매출 부가세 합계 점유율  -NO 분류명 총수량 총매출 총할인 실매출 부가세 합계 점유율
            PaymntSelngSttusView  결제유형별 매출현황   -NO 결제수단 결제건수 결제금액 점유율                      -NO 영업일자 결제건수 결제금액 점유율
            DscntSelngSttusView   할인유형별 매출현황   -NO 할인유형 건수 금액 점유율(건수)                        -NO 영업일자 건수 금액 점유율(건수) 
            MtSelngSttusView      월별 매출현황
            ExcclcSttusView       정산현황               
            TimeSelngSttusView    시간대별 매출현황     -NO 영업일자 총건수 총매출 총할인 실매출 부가세 합계 점유율 -NO 영업일자 총건수 총매출 총할인 실매출 부가세 합계 점유율
            

            //NO
        public string NO { get; set; } = string.Empty;
            //분류명
        public string SCLASS_NAME { get; set; } = string.Empty;	
            //총수량
        public string TOT_SALE_QTY { get; set; } = string.Empty;	
            //총매출
        public string TOT_SALE_AMT { get; set; } = string.Empty;	
            //총할인
        public string TOT_DC_AMT { get; set; } = string.Empty;	
            //실매출
        public string DCM_SALE_AMT { get; set; } = string.Empty;	
            //부가세
        public string VAT_AMT { get; set; } = string.Empty;	
            //합계
        public string TOT_AMT { get; set; } = string.Empty;	
            //점유율
        public string OCC_RATE { get; set; } = string.Empty;	
            //영업일자
        public string SALE_DATE { get; set; } = string.Empty;	
            //결제수단
        public string PAYMENT_METHOD { get; set; } = string.Empty;	
            //결제건수
        public string PAY_CNT { get; set; } = string.Empty;	
            //결제금액
        public string SALE_AMT { get; set; } = string.Empty;	
            //할인유형
        public string DIS_CLS { get; set; } = string.Empty;	

            */
        }

        public SELLING_STATUS_INFO(string sale_date)
        {
            this.SALE_DATE = sale_date;
        }



        //NO
        public string NO { get; set; } = string.Empty;
        //분류명
        public string SCLASS_NAME { get; set; } = string.Empty;
        //총수량
        public string TOT_SALE_QTY { get; set; } = string.Empty;
        //총매출
        public string TOT_SALE_AMT { get; set; } = string.Empty;
        //총할인
        public string TOT_DC_AMT { get; set; } = string.Empty;
        //실매출
        public string DCM_SALE_AMT { get; set; } = string.Empty;
        //부가세
        public string VAT_AMT { get; set; } = string.Empty;
        //합계
        public string TOT_AMT { get; set; } = string.Empty;
        //점유율
        public string OCC_RATE { get; set; } = string.Empty;
        //영업일자
        public string SALE_DATE { get; set; } = string.Empty;
        //결제수단
        public string PAYMENT_METHOD { get; set; } = string.Empty;
        //결제건수
        public string PAY_CNT { get; set; } = string.Empty;
        //결제금액
        public string SALE_AMT { get; set; } = string.Empty;
        //할인유형
        public string DIS_CLS { get; set; } = string.Empty;

        //시간대
        public string TIME_CLS { get; set; } = string.Empty;

        public string PRD_CODE { get; set; } = string.Empty;

        public string PRD_NAME { get; set; } = string.Empty;
        public string SUM_AMT   {get;set;}  = string.Empty;
        public string DC_AMT    {get;set;}  = string.Empty;
        public string SALE_QTY { get; set; } = string.Empty;
    }
}

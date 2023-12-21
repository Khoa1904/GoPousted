using Dapper;
using GoPOS.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using GoPOS.Models.Custom.SalesMng;
using GoPOS.Models.Custom.SellingStatus;

namespace GoPOS.Services;

/*
 2023-01-30 박현재 생성
 */
public interface ISellingStatusService
{
    /*
ClSelngSttusView      매출현황 > 분류별 매출현황       -NO 분류명 총수량 총매출 총할인 실매출 부가세 합계 점유율  -NO 영업일자 총수량 총매출 총할인 실매출 부가세 합계 점유율
GoodsSelngSttusView   매출현황 > 상품별 매출현황       -NO 분류명 총수량 총매출 총할인 실매출 부가세 합계 점유율  -NO 분류명 총수량 총매출 총할인 실매출 부가세 합계 점유율
PaymntSelngSttusView  매출현황 > 결제유형별 매출현황   -NO 결제수단 결제건수 결제금액 점유율                      -NO 영업일자 결제건수 결제금액 점유율
DscntSelngSttusView   매출현황 > 할인유형별 매출현황   -NO 할인유형 건수 금액 점유율(건수)                        -NO 영업일자 건수 금액 점유율(건수) 
MtSelngSttusView      매출현황 > 월별 매출현황
ExcclcSttusView       매출현황 > 정산현황               
TimeSelngSttusView    매출현황 > 시간대별 매출현황     -NO 영업일자 총건수 총매출 총할인 실매출 부가세 합계 점유율 -NO 영업일자 총건수 총매출 총할인 실매출 부가세 합계 점유율

        
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

    Task<(List<SELLING_STATUS_INFO>, SpResult)> GetClSelngSttusMainList(DynamicParameters param);
    Task<(List<SELLING_STATUS_INFO>, SpResult)> GetClSelngSttusDetailList(DynamicParameters param);
    Task<(List<SELLING_STATUS_INFO>, SpResult)> GetGoodsSelngSttusMainList(DynamicParameters param);
    Task<(List<SELLING_STATUS_INFO>, SpResult)> GetGoodsSelngSttusDetailList(DynamicParameters param);
    Task<(List<SALE_BY_TYPE2>, SpResult)> GetPaymntSelngSttusMainList(DynamicParameters param);
    Task<(List<SALE_BY_TYPE2>, SpResult)> GetPaymntSelngSttusDetailList(DynamicParameters param);
    Task<(List<SELLING_STATUS_INFO>, SpResult)> GetDscntSelngSttusMainList(DynamicParameters param);
    Task<(List<SELLING_STATUS_INFO>, SpResult)> GetDscntSelngSttusDetailList(DynamicParameters param);
    Task<(List<SELLING_STATUS_INFO>, SpResult)> GetTimeSelngSttusMainList(DynamicParameters param);
    Task<(List<SELLING_STATUS_INFO>, SpResult)> GetTimeSelngSttusDetailList(DynamicParameters param);

    Task<(List<SELLING_APPR>, SpResult)> GetMiddleCardAppr(string? SaleYN, string ResiSeq, string /*ClozeFlak, string*/ saleDT);

    Task<(List<SELLING_APPR>, SpResult)> GetMiddleCashAppr(string? SaleYN, string ResiSeq, /*string ClozeFlak,*/string saleDT);

    Task<(List<SELLING_STATUS_INFO>, SpResult)> GetMtSelngSttusList(DynamicParameters param);

    Task<(ExcclcSttusModel, SpResult)> GetExcclcSttusMainList(DynamicParameters param);

    Task<(List<SELLING_STATUS_INFO>, SpResult)> GetExcclcSttusDetailList(DynamicParameters param);

    Task<(List<FINAL_SETT>, SpResult)> GetFinalClosing(DynamicParameters param);

    Task<(GRAPH_SELNG_STTUS, SpResult)> GetMonthlyData(DynamicParameters param);
    Task<(GRAPH_SELNG_STTUS, SpResult)> GetDayData(DynamicParameters param);
    Task<(GRAPH_SELNG_STTUS, SpResult)> GetClassData(DynamicParameters param);
    Task<(GRAPH_SELNG_STTUS, SpResult)> GetWeekData(DynamicParameters param);

    Task<(List<CR_CARD_SALE>, SpResult)> GetCardDistinct(string ResiSeq, /*string ClozeFlak,*/string txtDateFrom, string txtDateTo);

    Task<(List<SALES_GIFT_SALE2>, SpResult)> GetMiddleGiftCard1(string? SaleYN, string ResiSeq, /*string ClozeFlak,*/ string saleDT);
    Task<(List<SALES_GIFT_SALE2>, SpResult)> GetMiddleGiftCard2(string? SaleYN, string ResiSeq, /*string ClozeFlak,*/ string saleDT);



}


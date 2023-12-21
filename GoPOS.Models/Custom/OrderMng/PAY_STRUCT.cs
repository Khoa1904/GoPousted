using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoPOS.Models
{
    public class PAY_STRUCT
    {

    }


    /// <summary>
    /// 현금 결제시 정보
    /// </summary>
    public static class PAY_INFO
    {
        public static string SHOP_CODE { get; set; } = string.Empty;

        /// <summary>
        /// 결제일
        /// </summary>
        public static string SALE_DATE { get; set; } = string.Empty;

        /// <summary>
        /// 오더 넘버는 채번시 교체  (List<POS_ORD_M> orderPayInfo2, SpResult spResult2) = await orderPayCashService.GetOrderNo();
        /// ORDER_INFO.ORDER_NO = orderPayInfo2[0].PO_ORDER_NO; // ORDER_INFO에 넣줌
        /// </summary>
        public static string ORDER_NO { get; set; } = string.Empty;

        /// <summary>
        /// 포스 번호 1대이면 01
        /// </summary>
        public static string POS_NO { get; set; } = string.Empty; // 김형석 추가 20230206 POS_NO

        /// <summary>
        /// 선택된 테이블 정보
        /// </summary>
        public static string TABLE_CODE { get; set; } = string.Empty; // 김형석 추가 20230212 TABLE_CODE

        /// <summary>
        /// 총 매출
        /// </summary>
        public static string TOT_SALE_AMT { get; set; } = "0"; // 김형석 추가 20230212 TOT_SALE_AMT 총판매가격

        /// <summary>
        ///  순수 매출
        /// </summary>
        public static string NO_VAT_SALE_AMT { get; set; } = "0"; // 김형석 추가 20230212 NO_VAT_SALE_AMT 부가세 없이

        /// <summary>
        /// 부가세
        /// </summary>
        public static string VAT_AMT { get; set; } = "0"; // 김형석 추가 20230212 VAT_AMT 부가세 포함

        /// <summary>
        /// BILL NO 채번용
        /// </summary>
        public static string BILL_NO { get; set; } = string.Empty; // 김형석 추가 20230215 SALE_BILL_NO  채번용


        /// <summary>
        /// 받을금액
        /// </summary>
        public static string EXP_PAY_AMT { get; set; } = "0"; // 김형석 추가 20230212  결제시 받을금액

        /// <summary>
        /// 결제시 거스름돈
        /// </summary>
        public static string RET_PAY_AMT { get; set; } = "0"; // 김형석 추가 20230212  결제시 거스름돈

        /// <summary>
        /// 결제시 현금 금액
        /// </summary>
        public static string CASH_AMT { get; set; } = "0"; // 김형석 추가 20230212 VAT_AMT 결제시 현금 금액

        /// <summary>
        /// 결제시 카드 금액
        /// </summary>
        public static string CRD_CARD_AMT { get; set; } = "0"; // 김형석 추가 20230212 VAT_AMT 결제시 카드 금액

        //EXP_PAY_AMT,     75000    받을금액
        //GST_PAY_AMT,     10000    받은돈
        //RET_PAY_AMT,     -65000   거스름
        //CASH_AMT,        10000    결제액현금

    };

    public class PAY_COPRTN_DSCNT_INFO
    {
        public SHOP_ALLIANCE_TARGET_PRODUCT pSHOP_ALLIANCE_TARGET_PRODUCT = new SHOP_ALLIANCE_TARGET_PRODUCT();
        public List<SHOP_ALLIANCE_TARGET_PRODUCT_DETAIL> pSHOP_ALLIANCE_TARGET_PRODUCT_DETAIL = new List<SHOP_ALLIANCE_TARGET_PRODUCT_DETAIL>();

        /*
        
        PAY_COPRTN_DSCNT_INFO pp = new PAY_COPRTN_DSCNT_INFO();
        List<SHOP_ALLIANCE_TARGET_PRODUCT_DETAIL> pSHOP_ALLIANCE_TARGET_PRODUCT_DETAIL = new List<SHOP_ALLIANCE_TARGET_PRODUCT_DETAIL>();
        pp.pSHOP_ALLIANCE_TARGET_PRODUCT_DETAIL = pSHOP_ALLIANCE_TARGET_PRODUCT_DETAIL;

        */

    }

}

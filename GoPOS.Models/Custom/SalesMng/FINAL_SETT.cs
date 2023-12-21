using GoPOS.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoPOS.Models
{
    public class FINAL_SETT
    {
        /// <summary>
        /// 상품권판매
        /// </summary>
        public FINAL_SETT()
        {
            /*
            NO	상품권명 TK_GFT_CODE	
            액면가  TK_GFT_UPRC	
            단가 상품권-TK_GFT_UAMT	
            할인 DC_RATE	
            금액 TK_GFT_UAMT	
            비고 REMARK

            numeric(10,2)

            */
            this.POS_NO = DataLocals.AppConfig.PosInfo.PosNo;
        }
        public string POS_NO { get; set; } = string.Empty;                                //    FINAL[0].GROSS_SALE };
        public decimal GROSS_SALE { get { return TOT_SALE_AMT + RET_BILL_AMT; } }         //    FINAL[0].TOT_SALE_AMT };
        public decimal RET_BILL_AMT { get; set; } = 0;                                    //    FINAL[0].TOT_SALE_AMT };
        public decimal TOT_SALE_AMT { get; set; } = 0;                                    //    FINAL[0].TOT_SALE_AMT };
        public decimal TOT_DC_AMT { get; set; } = 0;                                      //    FINAL[0].DCM_SALE_AMT };
        public decimal DCM_SALE_AMT { get; set; } = 0;                                    //    FINAL[0].VAT_SALE_AMT };
        public decimal VAT_SALE_AMT { get; set; } = 0;                                    //     FINAL[0].VAT_AMT };
        public decimal NO_VAT_SALE_AMT { get; set; } = 0;                                 //     FINAL[0].NO_VAT_AMT }; 면세매출액 추가
        public decimal VAT_AMT { get; set; } = 0;
        public decimal TOT_BILL_CNT { get; set; } = 0;
        public decimal VISIT_CST_CNT { get; set; } = 0;
        public decimal RET_BILL_CNT { get; set; } = 0;
        public decimal NET_BILL_CNT { get { return TOT_BILL_CNT - RET_BILL_CNT; } }
        public decimal CASH_AMT { get; set; } = 0;
        public decimal CASH_CNT { get; set; } = 0;
        public decimal CRD_CARD_AMT { get; set; } = 0;
        public decimal CRD_CARD_CNT { get; set; } = 0;
        public decimal WES_AMT { get; set; } = 0;
        public decimal TK_GFT_AMT { get; set; } = 0;
        public decimal TK_GFT_CNT { get; set; } = 0;
        public decimal O2O_AMT { get; set; } = 0;
        public decimal JCD_CARD_AMT { get; set; } = 0;
        public decimal JCD_CARD_CNT { get; set; } = 0;
        public decimal CST_POINT_AMT { get; set; } = 0;
        public decimal TK_FOD_AMT { get; set; } = 0;
        public decimal TK_FOD_CNT { get; set; } = 0;
        public decimal RFC_AMT { get; set; } = 0;
        public decimal SP_PAY_AMT { get; set; } = 0;
        public decimal PPC_CARD_AMT { get; set; } = 0;        //선결제추가                               // 선결제추가
        public decimal EGIFT_AMT { get; set; } = 0;
        public decimal EGIFT_CNT { get; set; } = 0;
        public decimal DC_GEN_AMT { get; set; } = 0;
        public decimal DC_SVC_AMT { get; set; } = 0;
        public decimal DC_JCD_AMT { get; set; } = 0;
        public decimal DC_CPN_AMT { get; set; } = 0;
        public decimal DC_CST_AMT { get; set; } = 0;
        public decimal DC_TFD_AMT { get; set; } = 0;
        public decimal DC_PRM_AMT { get; set; } = 0;
        public decimal DC_CRD_AMT { get; set; } = 0;
        public decimal DC_PACK_AMT { get; set; } = 0;
        public string ISS_CRDCP_NAME { get; set; } = string.Empty;
        public decimal APPR_AMT { get; set; } = 0;
        public decimal CST_POINT_CNT { get; set; } = 0;
        public string PAY_TYPE_FLAG { get; set; } = string.Empty;
        public string PAY_TYPE_NAME => PAY_TYPE_FLAG == "0" ? "현금" : "신용카드";  // 결제수단구분 (CCD_CODEM_T : 038) 0:현금 1:신용카드
        public decimal PRE_PNT_SALE_CRD_AMT { get; set; } = 0;                            //     FINAL[0].NO_VAT_AMT }; 선결제카드충전 추가


    }

    public class CLOSE_DETAILS1
    {
        public string Item { get; set; } = string.Empty;
        public decimal value { get; set; } = 0;
    }
    public class CLOSE_DETAILS2
    {
        public string Item { get; set; } = string.Empty;
        public decimal value { get; set; } = 0;
    }
    public class SALE_BY_TYPE
    {
        public string Item { get; set; } = string.Empty;
        public decimal value { get; set; } = 0;
    }
    public class SALE_DISCOUNT
    {
        public string Item { get; set; } = string.Empty;
        public decimal value { get; set; } = 0;
    }
    public class NSALE_RECORD
    {
        public string Item { get; set; } = string.Empty;
        public decimal value { get; set; } = 0;
    }
    public class MEMBER_SALE
    {
        public string Item { get; set; } = string.Empty;
        public decimal value { get; set; } = 0;
    }
    public class CR_CARD_SALE
    {
        public string CompanyName { get; set; } = string.Empty;
        public decimal amount { get; set; } = 0;
        public decimal SaleCount { get; set; } = 0;
    }
    public class PRODUCT_SALE
    {
        public string  ProdName {get;set;} = string.Empty;
        public decimal Qty      {get;set;} = 0;
        public decimal Sale_amt {get;set;} = 0;
    }
    public class SALE_BY_TYPE2
    {
        public string  PayType
        { // 01:현금,02:현금영수증,03:신용카드,04:은련카드,05:간편결제,06:제휴할인,07:상품권,08:식권,09:외상,10:선불카드,11:선결제,12:전자상품권,13:모바일상품권,14:포인트적립,15:포인트사용,16:사원카드
            get
            {
                switch(Pay_Flag)                                
                {
                    case "01":
                        return "현금";
                         
                    case "02":
                        return "현금영수증";
                         
                    case "03":
                        return "신용카드";
                         
                    case "04":
                        return "은련카드";
                         
                    case "05":
                        return "간편결제";
                         
                    case "06":
                        return "제휴할인";
                         
                    case "07":
                        return "상품권";
                         
                    case "08":
                        return "식권";
                         
                    case "09":
                        return "외상";
                         
                    case "10":
                        return "선불카드";
                         
                    case "11":
                        return "선결제";
                         
                    case "12":
                        return "전자상품권";
                         
                    case "13":
                        return "모바일상품권";
                         
                    case "14":
                        return "포인트적립";
                         
                    case "15":
                        return "포인트사용";
                         
                    case "16":
                        return "사원카드";
                         
                    default:
                        return string.Empty;                       
                }                   
            }
        }
        public decimal Qty      { get; set; } = 0;
        public decimal Pay_Amt  { get; set; } = 0;
        public string  Pay_Flag { get; set; } = string.Empty;
        public decimal OCC_RATE { get; set; } = 0;
        public string  SALE_DATE { get; set;} = string.Empty;
    }
    public class SHOP_SALE_STATS
    {
        public string  Gubun          {get;set;} = string.Empty;   
        public int     Case_Count     {get;set;} = 0;
        public int     Customer_Count {get;set;} = 0;
        public decimal Amount         {get;set;} = 0;
    }
}


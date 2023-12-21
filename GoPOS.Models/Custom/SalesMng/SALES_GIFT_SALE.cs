using GoPOS.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoPOS.Models
{
    public class SALES_GIFT_SALE
    {
        /// <summary>
        /// 상품권판매
        /// </summary>
        public SALES_GIFT_SALE()
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
            this.POS = DataLocals.AppConfig.PosInfo.PosNo;
        }

        public string SALE_DATE { get; set; } = string.Empty;
        public string POS { get; set; } = string.Empty;

        public string TK_GFT_CODE { get; set; } = string.Empty; //상품권명	
        public string TK_GFT_UPRC { get; set; } = string.Empty; // 액면가
        public string TK_GFT_UAMT { get; set; } = string.Empty; //단가	
        public string DC_RATE { get; set; } = string.Empty; //할인	
        public string TOT_TK_GFT_UAMT { get; set; } = string.Empty; //금액	 //총합
        public string TK_GFT_NAME { get; set; } = string.Empty;

        // 추가
        public string NO { get; set; } = string.Empty;
        public string REMARK { get; set; } = string.Empty;
        public string SALE_GIFT_SALE { get; set; }

        public string SALE_YN { get; set; } = string.Empty;
        public string CLOSE_FLAG { get; set; } = string.Empty;
        public string REGI_SEQ { get; set; } = string.Empty;
        public string TK_GFT_AMT { get; set; } = string.Empty;
        public string BILL_NO { get; set; } = string.Empty;
        public string APPR_AMT { get; set; } = string.Empty;
        public string PPC_PROC_FLAG { get; set; } = string.Empty;
        public string TK_GFT_CNT { get; set; } = string.Empty;
        public string PRC_PROC_NAME { get
            {
                switch (PPC_PROC_FLAG)
                {
                    case "0":
                        return "Charge";
                        break;

                    case "1":
                        return "Use";
                        break;

                    case "2":
                        return "Cancel charge";
                        break;

                    case "3":
                        return "Cancel use";
                        break;

                    case "9":
                        return "Query";
                        break;

                     default: 
                        return string.Empty;
                        break;
                }
            }        
        }
    }

    public class SALES_GIFT_SALE2
    {
        /// <summary>
        /// 상품권판매
        /// </summary>
        public SALES_GIFT_SALE2()
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
            this.POS = DataLocals.AppConfig.PosInfo.PosNo;
        }

        public string SALE_DATE { get; set; } = string.Empty;
        public string POS { get; set; } = string.Empty;

        public string TK_GFT_CODE { get; set; } = string.Empty; //상품권명	
        public decimal TK_GFT_UPRC { get; set; } = 0; // 액면가
        public decimal TK_GFT_UAMT { get; set; } = 0; //단가	
        public string DC_RATE { get; set; } = string.Empty; //할인	
        public decimal TOT_TK_GFT_UAMT { get; set; } = 0; //금액	 //총합
        public string TK_GFT_NAME { get; set; } = string.Empty;

        // 추가
        public string NO { get; set; } = string.Empty;
        public string REMARK { get; set; } = string.Empty;
        public string SALE_GIFT_SALE { get; set; }

        public string SALE_YN { get; set; } = string.Empty;
        public string SALE_TYPE
        {
            get
            {
                switch (TK_GFT_SALE_FLAG)
                {
                    case "0":
                        if (SALE_YN == "Y")
                            return "매출-결제";
                        return "반품-결제";
                        break;

                    case "3":
                        if (SALE_YN == "Y")
                            return "매출-환불";
                        return "반품-환불";
                        break;

                    default:
                        return string.Empty;
                        break;
                }
            }
        }
        public string TK_GFT_SALE_FLAG { get; set; } = string.Empty;
        public string CLOSE_FLAG { get; set; } = string.Empty;
        public string REGI_SEQ { get; set; } = string.Empty;
        public decimal TK_GFT_AMT { get; set; } = 0;
        public string BILL_NO { get; set; } = string.Empty;
        public decimal APPR_AMT { get; set; } = 0;
        public string PPC_PROC_FLAG { get; set; } = string.Empty;
        public decimal TK_GFT_CNT { get; set; } = 0;
        public decimal TOTAL_TK_GFT_UAMT
        {
            get => TK_GFT_CNT * TK_GFT_UAMT;
        }
        public string PRC_PROC_NAME
        {
            get // 0:충전 1:사용 2:충전취소 3:사용취소 9:조회
            {
                switch (PPC_PROC_FLAG)
                {
                    case "0":
                        return "충전";
                        break;

                    case "1":
                        return "사용";
                        break;

                    case "2":
                        return "충전취소e";
                        break;

                    case "3":
                        return "사용취소";
                        break;

                    case "9":
                        return "조회";
                        break;

                    default:
                        return string.Empty;
                        break;
                }
            }
        }


    }
}


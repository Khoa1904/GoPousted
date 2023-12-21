using GoPOS.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoPOS.Models.Custom.SellingStatus
{
    public class SELLING_APPR
    {
        public SELLING_APPR()
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
        public string POS_NO { get; set; } = string.Empty;
        public string BILL_NO { get; set; } = string.Empty;
        public string EQM_TYPE { get; set; } = string.Empty;
        public string APPR_IDT_TYPE { get; set; } = string.Empty; // transaction type
        public string APPR_IDT_NAME //0:비승인 1:개인 2:사업자 5:자진발급
        {
            get
            {
                switch (APPR_IDT_TYPE)
                {
                    case "0":
                        return "비승인";
                        break;

                    case "1":
                        return "개인";
                        break;

                    case "2":
                        return "사업자";
                        break;

                    case "5":
                        return "자진발급";
                        break;

                    default:
                        return string.Empty;
                        break;
                }
            }
        }
        public string APPR_IDT_NO { get; set; } = string.Empty;
        //     public string APPR_LOG_NO { get; set; } = string.Empty;
        public string SALE_YN { get; set; } = string.Empty;
        public string SALE_YN_NAME => SALE_YN == "Y" ? "정상" : "반품(취소)";
        public string APPR_NO { get; set; } = string.Empty;
        public decimal APPR_AMT { get; set; } = 0;
        public string CASH_AMT { get; set; } = string.Empty;
        public string APPR_PROC_FLAG { get; set; } = string.Empty;
        public string APPR_PROC_NAME
        { // 승인처리구분 (CCD_CODEM_T : 031) 0:비승인 1:포스승인 2:단말기승인 3:전화승인
            get
            {
                switch (APPR_IDT_TYPE)
                {
                    case "0":
                        return "비승인";
                        break;

                    case "1":
                        return "포스승인";
                        break;

                    case "2":
                        return "단말기승인";
                        break;

                    case "3":
                        return "전화승인";
                        break;

                    default:
                        return string.Empty;
                        break;
                }
            }
        }
    }
}

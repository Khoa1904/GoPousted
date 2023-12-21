using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace GoPOS.Models
{
    public class RCIPTSELNGSTTUS
    {
        /// <summary>
        /// 영수증별 매출현황

        /// </summary>
        public RCIPTSELNGSTTUS()
        {
            /*
           영수증별 매출현황												
           영수증		총매출	        총할인	    실매출	            부가세	    합계	점유율			
           BILL_NO		TOT_SALE_AMT	TOT_DC_AMT	TOT_DCM_SALE_AMT	TOT_VAT_AMT	TOT_AMT	OCC_RATE			
           */
        }

        public RCIPTSELNGSTTUS(string bill_no, string pos)
        {
            this.BILL_NO = bill_no;
            this.POS = pos;
        }

        public string BILL_NO { get; set; } = string.Empty; // 영수증
        public string TOT_SALE_AMT { get; set; } = string.Empty; // TOT_SALE_AMT
        public string TOT_DC_AMT { get; set; } = string.Empty; // DC_AMT
        public string TOT_DCM_SALE_AMT { get; set; } = string.Empty; // DCM_SALE_AMT
        public string TOT_VAT_AMT { get; set; } = string.Empty; // VAT_AMT
        public string TOT_AMT { get; set; } = string.Empty; // TOT_AMT
        public string OCC_RATE { get; set; } = string.Empty; // OCC_RATE

        // 추가
        public string NO { get; set; } = string.Empty;
        public string POS { get; set; } = string.Empty;
        public string SALE_DATE { get; set; } = string.Empty;//영업일자	

    }
}

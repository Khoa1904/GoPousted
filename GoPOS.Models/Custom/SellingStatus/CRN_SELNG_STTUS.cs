using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace GoPOS.Models
{
    public class CRNSELNGSTTUS
    {
        /// <summary>
        /// 코너별 매출현황

        /// </summary>
        public CRNSELNGSTTUS()
        {
            /*
           상단:
           CORNER_CODE	     ;//코너명	
           TOT_SALE_QTY	     ;//총수량	
           TOT_SALE_AMT	     ;//총매출	
           TOT_DC_AMT	     ;//총할인	
           TOT_DCM_SALE_AMT	 ;//실매출	
           TOT_VAT_AMT	     ;//부가세	
           TOT_AMT	         ;//합계
           OCC_RATE          ;//점유율


           하단:
           SALE_DATE	     ;//영업일자	
           TOT_SALE_QTY	     ;//총수량	
           TOT_SALE_AMT	     ;//총매출	
           TOT_DC_AMT	     ;//총할인	
           TOT_DCM_SALE_AMT	 ;//실매출
           TOT_VAT_AMT	     ;//부가세
           TOT_AMT	         ;//합계
           OCC_RATE	         ;//점유율
            
            */
        }

        public CRNSELNGSTTUS(string sale_date, string pos)
        {
            this.SALE_DATE = sale_date;
            this.POS = pos;
        }


        //상단:
        public string CORNER_CODE { get; set; } = string.Empty;//코너명
                                                               
        //public string TOT_SALE_QTY { get; set; } = string.Empty;//총수량	
        //public string TOT_SALE_AMT { get; set; } = string.Empty;//총매출	
        //public string TOT_DC_AMT { get; set; } = string.Empty;//총할인	
        //public string TOT_DCM_SALE_AMT { get; set; } = string.Empty;//실매출	
        //public string TOT_VAT_AMT { get; set; } = string.Empty;//부가세	
        //public string TOT_AMT { get; set; } = string.Empty;//합계
        //public string OCC_RATE { get; set; } = string.Empty;//점유율

        //하단:
        public string SALE_DATE { get; set; } = string.Empty;//영업일자	

        //public string TOT_SALE_QTY { get; set; } = string.Empty;//총수량	
        //public string TOT_SALE_AMT { get; set; } = string.Empty;//총매출	
        //public string TOT_DC_AMT { get; set; } = string.Empty;//총할인	
        //public string TOT_DCM_SALE_AMT { get; set; } = string.Empty;//실매출
        //public string TOT_VAT_AMT { get; set; } = string.Empty;//부가세
        //public string TOT_AMT { get; set; } = string.Empty;//합계
        //public string OCC_RATE { get; set; } = string.Empty;//점유율


        //공통
        public string TOT_SALE_QTY { get; set; } = string.Empty;//총수량	
        public string TOT_SALE_AMT { get; set; } = string.Empty;//총매출	
        public string TOT_DC_AMT { get; set; } = string.Empty;//총할인	
        public string TOT_DCM_SALE_AMT { get; set; } = string.Empty;//실매출	
        public string TOT_VAT_AMT { get; set; } = string.Empty;//부가세	
        public string TOT_AMT { get; set; } = string.Empty;//합계
        public string OCC_RATE { get; set; } = string.Empty;//점유율

        // 추가
        public string NO { get; set; } = string.Empty;
        public string POS { get; set; } = string.Empty;
    }
}

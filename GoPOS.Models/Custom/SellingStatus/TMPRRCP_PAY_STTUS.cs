using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace GoPOS.Models
{
    public class TMPRRCPPAYSTTUS
    {
        /// <summary>
        /// 시재입출금현황 

        /// </summary>
        public TMPRRCPPAYSTTUS()
        {
            /*
            영업일자	등록일시	계정명      	입금금액      	출금액          판매원	비고
            SALE_DATE	INSERT_DT	SHOP_ACCOUNT	POS_CSH_IN_AMT	POS_CSH_OUT_AMT	EMP_NO	REMARK
            */
        }

        public TMPRRCPPAYSTTUS(string sale_date, string pos)
        {
            this.SALE_DATE = sale_date;
            this.POS = pos;
        }

        public string SALE_DATE { get; set; } = string.Empty; // 영업일자
        public string INSERT_DT { get; set; } = string.Empty; // 등록일시
        public string SHOP_ACCOUNT { get; set; } = string.Empty; // 계정명
        public string POS_CSH_IN_AMT { get; set; } = string.Empty; //  입금금액
        public string POS_CSH_OUT_AMT { get; set; } = string.Empty; // 출금액
        public string EMP_NO { get; set; } = string.Empty; // 판매원
        public string REMARK { get; set; } = string.Empty; // 비고

        // 공통 추가
        public string NO { get; set; } = string.Empty;
        public string POS { get; set; } = string.Empty;

    }
}

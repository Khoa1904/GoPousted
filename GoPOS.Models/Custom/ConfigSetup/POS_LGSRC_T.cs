using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoPOS.Models
{
    public class POS_LGSRC_T
    {
        /// <summary>
        /// 무결성 점검
        /// </summary>
        public POS_LGSRC_T()
        {
            /*
            무결성 점검

            NO 
            SALE_DATE
            POS_NO
            SETUP_FLAG
            INSERT_DT
            STATUS_FLAG

            */
        }

        public POS_LGSRC_T(string sale_date, string pos_no)
        {
            this.SALE_DATE = sale_date;
            this.POS_NO = pos_no;
        }


        // 무결성 점검
        public string SALE_DATE { get; set; } = string.Empty;
        public string POS_NO { get; set; } = string.Empty;
                   
        public string INTEGRITY_SEQ  { get; set; } = string.Empty;
        public string INTEGRITY_FLAG { get; set; } = string.Empty;
        public string INTEGRITY_DATE { get; set; } = string.Empty;
        public string INTEGRITY_YN   { get; set; } = string.Empty;



        // 추가 공통
        public string NO { get; set; } = string.Empty;

    }
}


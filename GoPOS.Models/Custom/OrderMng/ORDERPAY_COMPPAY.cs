using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace GoPOS.Models
{
    public class ORDERPAYCOMPPAY
    {
        /// <summary>
        /// 테이블그룹 매출현황

        /// </summary>
        public ORDERPAYCOMPPAY()
        {
            /*
            */
        }

        public ORDERPAYCOMPPAY(string appr_no, string appr_proc_flag)
        {
            this.APPR_NO = appr_no;
            this.APPR_PROC_FLAG = appr_proc_flag;
        }

        //NO    NO
        //PAY_TYPE_FLAG  결제구분 - 추가
        //APPR_IDT_NO    카드/인식번호 
        //APPR_AMT 승인금액 
        //APPR_NO 승인번호 
        //APPR_PROC_FLAG 처리상태 

        public string NO              { get; set; } = string.Empty;
        public string PAY_TYPE_FLAG   { get; set; } = string.Empty; // 결제구분
        public string APPR_IDT_NO     { get; set; } = string.Empty;        // 카드/인식번호  
        public string APPR_AMT        { get; set; } = string.Empty;        // 승인금액
        public string APPR_NO         { get; set; } = string.Empty;        // 승인번호
        public string APPR_PROC_FLAG  { get; set; } = string.Empty;        // 처리상태

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoPOS.Models
{
    public class CONFIG_SETUP_COM
    {
        /// <summary>
        /// 무결성 점검
        /// </summary>
        public CONFIG_SETUP_COM()
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

        public CONFIG_SETUP_COM(string sale_date, string pos_no)
        {
            this.SALE_DATE = sale_date;
            this.POS_NO = pos_no;
        }


        // 무결성 점검
        public string SALE_DATE { get; set; } = string.Empty;
        public string POS_NO { get; set; } = string.Empty;

        public string SETUP_FLAG { get; set; } = string.Empty; //SETUP_FLAG	
        public string INSERT_DT { get; set; } = string.Empty; // INSERT_DT
        public string STATUS_FLAG { get; set; } = string.Empty; //STATUS_FLAG	 


        // 매출자료 송신
        public string BILL_NO { get; set; } = string.Empty;  // 영수증번호
        public string TOT_SALE_AMT { get; set; } = string.Empty; // 판매금액
        public string SALE_YN { get; set; } = string.Empty;   // 판매구분 반품 / 정상
        public string SEND_FLAG { get; set; } = string.Empty; // 전송구분

        public string PROGRAM_VALUE { get; set; } = string.Empty;
        public string PROGRAM_NAME  { get; set; } = string.Empty;
        public string FILE_SIZE     { get; set; } = string.Empty;
        public string UPDATE_DT     { get; set; } = string.Empty;

        // 추가 공통
        public string NO { get; set; } = string.Empty;

    }
}


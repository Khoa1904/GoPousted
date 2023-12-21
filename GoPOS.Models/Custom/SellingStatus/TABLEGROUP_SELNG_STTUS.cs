using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace GoPOS.Models
{
    public class TABLEGROUPSELNGSTTUS
    {
        /// <summary>
        /// 테이블그룹 매출현황

        /// </summary>
        public TABLEGROUPSELNGSTTUS()
        {
            /*
            */
        }

        public TABLEGROUPSELNGSTTUS(string sale_date, string pos)
        {
            this.SALE_DATE = sale_date;
            this.POS = pos;
        }

        //상단:             영업일자        영업요일       영업일수       실매출합계        회전수합계       고객수합계
        //                  SALE_DATE       DAY_SALE_DATE  TOT_SALE_DATE  TOT_DCM_SALE_AMT  TOT_CST_ROT_CNT  TOT_CST_CNT
                                                                                     
        //하단 좌측:		테이블그룹명    실매출         회전수         고객수
        //                  TG_NAME         DCM_SALE_AMT   CST_ROT_CNT    CST_CNT
                                            
        //하단 우측:		결제수단        건수           금액
        //                  PAYMENT_METHOD  PAY_CNT        SALE_AMT


        //상단 ListView
        public string SALE_DATE { get; set; } = string.Empty; //영업일자
        public string DAY_SALE_DATE { get; set; } = string.Empty; // 영업요일
        public string TOT_SALE_DATE { get; set; } = string.Empty; // 영업일수
        
        public string TOT_CST_CNT { get; set; } = string.Empty;  // 고객수합계


        //하단 좌측
        public string TG_NAME { get; set; } = string.Empty; //테이블그룹명
        //public string TOT_DCM_SALE_AMT { get; set; } = string.Empty; // 실매출
        //public string TOT_CST_ROT_CNT { get; set; } = string.Empty; // 회전수
        public string CST_CNT { get; set; } = string.Empty; // 고객수

        //하단 우측
        public string PAYMENT_METHOD { get; set; } = string.Empty; // 결제수단
        public string PAY_CNT { get; set; } = string.Empty; // 건수
        public string SALE_AMT { get; set; } = string.Empty; // 금액


        // 공통 추가
        public string NO { get; set; } = string.Empty;        
        public string POS { get; set; } = string.Empty;

        public string TOT_DCM_SALE_AMT { get; set; } = string.Empty; // 실매출합계
        public string TOT_CST_ROT_CNT { get; set; } = string.Empty; // 회전수합계

    }
}

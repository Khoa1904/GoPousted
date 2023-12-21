using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoPOS.Models.Custom.OrderMng
{
    public class RECEIPT_MANAGER_ITEM : INotifyPropertyChanged
    {
        private string? sALE_YN;
        private string? oRG_BILL_NO;

        public int NO { get; set; }
        /// <summary>
        /// 매장코드
        /// </summary>
        [Comment("매장코드")]
        public string? SHOP_CODE { get; set; }
        /// <summary>
        /// 영업일자
        /// </summary>
        [Comment("영업일자")]
        public string? SALE_DATE { get; set; }
        /// <summary>
        /// 포스번호
        /// </summary>
        [Comment("포스번호")]
        public string? POS_NO { get; set; }
        /// <summary>
        /// 정산차수
        /// </summary>
        [Comment("정산차수")]
        public string? REGI_SEQ { get; set; }
        /// <summary>
        /// 영수번호
        /// </summary>
        [Comment("영수번호")]
        [Key, Column(Order = 4)]
        [Required]
        public string? BILL_NO { get; set; }

        /// <summary>
        /// 반품여부
        /// </summary>
        public string? SALE_YN
        {
            get => sALE_YN; set
            {
                sALE_YN = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("SALE_YN"));
            }
        }
        /// <summary>
        /// 실매출액
        /// </summary>
        [Comment("실매출액")]
        public decimal DCM_SALE_AMT { get; set; }

        public decimal DCM_SALE_AMT_DISP
        {
            get
            {
                return ("N".Equals(SALE_YN) ? -1 : 1) * DCM_SALE_AMT;
            }
        }

        public decimal PAY_AMT_CASH { get; set; }
        public decimal PAY_AMT_CARD { get; set; }
        public decimal PAY_AMT_ALL { get; set; }

        public decimal PAY_AMT_CASH_DISP
        {
            get
            {
                return ("N".Equals(SALE_YN) ? -1 : 1) * PAY_AMT_CASH;
            }
        }
        public decimal PAY_AMT_CARD_DISP
        {
            get
            {
                return ("N".Equals(SALE_YN) ? -1 : 1) * PAY_AMT_CARD;
            }
        }
        public decimal PAY_AMT_ALL_DISP
        {
            get
            {
                return ("N".Equals(SALE_YN) ? -1 : 1) * PAY_AMT_ALL;
            }
        }

        public decimal PAY_HAS_APPR { get; set; }
        public decimal TOT_DC_AMT { get; set; }

        public string ORDER_END_FLAG
        {
            get
            {
                return TOT_DC_AMT > 0 ? "Y" : "N";
            }
        }

        public string RETURN_STATUS
        {
            get
            {
                if (!String.IsNullOrEmpty(ORG_BILL_NO))
                {
                    return sALE_YN == "Y" ? "반품원거래" : "반품매출";
                }

                return sALE_YN == "Y" ? "" : "반품매출";
            }
        }

        public string? ORG_BILL_NO
        {
            get => oRG_BILL_NO; set
            {
                oRG_BILL_NO = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("RETURN_STATUS"));
            }
        }

        public string SALE_TIME
        {
            get
            {
                if (INSERT_DT != "")
                {
                    DateTime dateTime = DateTime.ParseExact(INSERT_DT, "yyyyMMddHHmmss", null);
                    return dateTime.ToString("HH:mm:ss");
                }
                return "";
            }
        }
        /// <summary>
        /// 등록일시
        /// </summary>
        [Comment("등록일시")]
        public string? INSERT_DT { get; set; }
        public event PropertyChangedEventHandler? PropertyChanged;
    }
}

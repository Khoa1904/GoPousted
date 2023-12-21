using GoPOS.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoPOS.Models
{
    public class ORDER_COUPON_DETAIL
    {
        public ORDER_COUPON_DETAIL(MST_INFO_COUPON lsCoupon)
        {
            this.POS = DataLocals.AppConfig.PosInfo.PosNo;
            this.TK_CPN_CODE = lsCoupon.TK_CPN_CODE == null ? "" : lsCoupon.TK_CPN_CODE;
            this.TK_CPN_NAME = lsCoupon.TK_CPN_NAME == null ? "" : lsCoupon.TK_CPN_NAME;
            this.TK_CLASS_CODE = lsCoupon.TK_CLASS_CODE == null ? "" : lsCoupon.TK_CLASS_CODE;
            this.CPN_DC_FLAG = lsCoupon.CPN_DC_FLAG == null ? "" : lsCoupon.CPN_DC_FLAG;
            this.CPN_DC_RATE = lsCoupon.CPN_DC_RATE.ToString();
            this.CPN_DC_AMT = lsCoupon.CPN_DC_AMT.ToString();
            this.CPN_QTY_FLAG = lsCoupon.CPN_QTY_FLAG == null ? "" : lsCoupon.CPN_QTY_FLAG;
        }


        public string POS { get; set; } = string.Empty;

        public string TK_CPN_CODE { get; set; } = string.Empty; //상품권명	
        public string TK_CPN_NAME { get; set; } = string.Empty; // 액면가
        public string TK_CLASS_CODE { get; set; } = string.Empty; // 액면가
        public string CPN_DC_FLAG { get; set; } = string.Empty; //상품권명	
        public string CPN_DC_RATE { get; set; } = string.Empty; // 액면가
        public string CPN_DC_AMT { get; set; } = string.Empty; // 액면가
        public string CPN_QTY_FLAG { get; set; } = string.Empty; //상품권명	
        public string USE_YN { get; set; } = string.Empty; // 액면가
        public string INSERT_DT { get; set; } = string.Empty; // 액면가
        public string UPDATE_DT { get; set; } = string.Empty; //상품권명	
        public string NO { get; set; } = string.Empty; //단가	
        public string TOT_TK_CPN_UAMT { get; set; } = string.Empty; //금액	 //총합
        public string REMARK { get; set; } = string.Empty;
    }
}


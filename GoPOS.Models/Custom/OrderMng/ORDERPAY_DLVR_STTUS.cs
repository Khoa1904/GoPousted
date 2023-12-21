using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace GoPOS.Models
{
    /// <summary>
    /// 배달현황
    /// </summary>
    public class OrderPayDlvrSttus
    {

        /// <summary>
        /// OrderPayDlvrSttus

        /// </summary>
        public OrderPayDlvrSttus()
        {
            /*


            );

            Primary keys
            TABLE SHOP_ALLIANCE_TARGET_PRODUCT ADD CONSTRAINT SCD_JCDHD_PK PRIMARY KEY(SHOP_CODE, JCD_CODE);
            Descriptions
            COMMENT ON TABLE SHOP_ALLIANCE_TARGET_PRODUCT IS '[매장]제휴카드-정보';
            */
        }

        public OrderPayDlvrSttus(string shop_code, string jcd_code)
        {
            this.SHOP_CODE = shop_code;
            this.JCD_CODE = jcd_code;
            
        }

        public string SHOP_CODE { get; set; } = string.Empty;
        public string JCD_CODE { get; set; } = string.Empty;
        public string JCD_NAME { get; set; } = string.Empty;
        public string JCD_TYPE_FLAG { get; set; } = string.Empty;
        public string VALID_F_DATE { get; set; } = string.Empty;
        public string VALID_T_DATE { get; set; } = string.Empty;
        public string DC_PRD_FLAG { get; set; } = string.Empty; 
        public string DC_RATE { get; set; } = string.Empty;
        public string DC_LIMIT_FLAG { get; set; } = string.Empty;
        public string DC_LIMIT_AMT { get; set; } = string.Empty;
        public string APPR_PROC_FLAG { get; set; } = string.Empty;
        public string REMARK { get; set; } = string.Empty;
        public string INSERT_DT { get; set; } = string.Empty;
        public string UPDATE_DT { get; set; } = string.Empty;


        /// <summary>
        /// detail
        /// </summary>
        public string STYLE_PRD_CODE { get; set; } = string.Empty;

        /// <summary>
        /// 상품 정보
        /// </summary>
        public string PRD_NAME { get; set; } = string.Empty;

        public string NO { get; set; } = string.Empty;

    }
}

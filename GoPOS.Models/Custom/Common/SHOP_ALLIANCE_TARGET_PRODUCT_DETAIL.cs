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
    /// [매장]제휴카드-할인대상상품
    /// </summary>
    public class SHOP_ALLIANCE_TARGET_PRODUCT_DETAIL
    {

        /// <summary>
        /// SHOP_ALLIANCE_TARGET_PRODUCT_DETAIL

        /// </summary>
        public SHOP_ALLIANCE_TARGET_PRODUCT_DETAIL()
        {
            /*
                CREATE TABLE SHOP_ALLIANCE_TARGET_PRODUCT_DETAIL (
                SHOP_CODE       VARCHAR(6) NOT NULL,
                JCD_CODE        VARCHAR(4) NOT NULL,
                STYLE_PRD_CODE  VARCHAR(26) NOT NULL,
                DC_RATE         NUMERIC(3,0) DEFAULT 0 NOT NULL,
                INSERT_DT       VARCHAR(14) NOT NULL,
                UPDATE_DT       VARCHAR(14) NOT NULL
            );

            Primary keys
            TABLE SHOP_ALLIANCE_TARGET_PRODUCT_DETAIL ADD CONSTRAINT SCD_JCDDT_PK PRIMARY KEY(SHOP_CODE, JCD_CODE, STYLE_PRD_CODE);

            Descriptions                                
            COMMENT ON TABLE SHOP_ALLIANCE_TARGET_PRODUCT_DETAIL IS '[매장]제휴카드-할인대상상품';
            */
        }

        public SHOP_ALLIANCE_TARGET_PRODUCT_DETAIL(string shop_code, string sale_date, string jcd_code, string style_prd_code)
        {
            this.SHOP_CODE = shop_code;
            this.SHOP_CODE = sale_date;
            this.JCD_CODE = jcd_code;
            this.STYLE_PRD_CODE = style_prd_code;
        }

        public string SHOP_CODE { get; set; } = string.Empty;

        public string JCD_CODE { get; set; } = string.Empty;
        public string STYLE_PRD_CODE { get; set; } = string.Empty;
        public string DC_RATE { get; set; } = "0";
        public string INSERT_DT { get; set; } = string.Empty;
        public string UPDATE_DT { get; set; } = string.Empty;

    }
}

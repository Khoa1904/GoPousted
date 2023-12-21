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
    /// [매장]제휴카드-정보
    /// </summary>
    public class SHOP_ALLIANCE_TARGET_PRODUCT
    {

        /// <summary>
        /// SHOP_ALLIANCE_TARGET_PRODUCT

        /// </summary>
        public SHOP_ALLIANCE_TARGET_PRODUCT()
        {
            /*



            CREATE TABLE SHOP_ALLIANCE_TARGET_PRODUCT (
                SHOP_CODE       VARCHAR(6) NOT NULL,
                JCD_CODE        VARCHAR(4) NOT NULL,
                JCD_NAME        VARCHAR(50) NOT NULL,
                JCD_TYPE_FLAG   VARCHAR(1) NOT NULL,
                VALID_F_DATE    VARCHAR(8) NOT NULL,
                VALID_T_DATE    VARCHAR(8) NOT NULL,
                DC_PRD_FLAG     VARCHAR(1) DEFAULT '0' NOT NULL,
                DC_RATE         NUMERIC(3,0) DEFAULT 0 NOT NULL,
                DC_LIMIT_FLAG   VARCHAR(1) DEFAULT '0' NOT NULL,
                DC_LIMIT_AMT    NUMERIC(9,0) DEFAULT 0 NOT NULL,
                APPR_PROC_FLAG  VARCHAR(1) DEFAULT '0' NOT NULL,
                REMARK          VARCHAR(200),
                INSERT_DT       VARCHAR(14) NOT NULL,
                UPDATE_DT       VARCHAR(14) NOT NULL
            );

            Primary keys
            TABLE SHOP_ALLIANCE_TARGET_PRODUCT ADD CONSTRAINT SCD_JCDHD_PK PRIMARY KEY(SHOP_CODE, JCD_CODE);
            Descriptions
            COMMENT ON TABLE SHOP_ALLIANCE_TARGET_PRODUCT IS '[매장]제휴카드-정보';
            */
        }

        public SHOP_ALLIANCE_TARGET_PRODUCT(string shop_code, string jcd_code)
        {
            this.SHOP_CODE = shop_code;
            this.JCD_CODE = SHOP_CODE;
            
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
    }
}

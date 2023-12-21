using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace GoPOS.Models
{
    public class SHOP_TICKET_COUPON
    {
        /// <summary>
        /// 매장-터치분류

        /// </summary>
        public SHOP_TICKET_COUPON()
        {
            /*            
            CREATE TABLE SHOP_TICKET_COUPON (
                      SHOP_CODE      VARCHAR(6) NOT NULL,
                      TK_CPN_CODE    VARCHAR(3) NOT NULL,
                      TK_CPN_NAME    VARCHAR(30) NOT NULL,
                      TK_CLASS_CODE  VARCHAR(3) NOT NULL,
                      CPN_DC_FLAG    VARCHAR(1) NOT NULL,
                      CPN_DC_RATE    NUMERIC(3,0) DEFAULT 0 NOT NULL,
                      CPN_DC_AMT     NUMERIC(8,0) DEFAULT 0 NOT NULL,
                      CPN_QTY_FLAG   VARCHAR(1) DEFAULT '1' NOT NULL,
                      USE_YN         VARCHAR(1) DEFAULT 'Y' NOT NULL,
                      INSERT_DT      VARCHAR(14),
                      UPDATE_DT      VARCHAR(14)
            );

            Primary keys                               
            ALTER TABLE SHOP_TICKET_COUPON ADD CONSTRAINT SCD_TKCPN_PK PRIMARY KEY(SHOP_CODE, TK_CPN_CODE);
            Descriptions                                
            COMMENT ON TABLE SHOP_TICKET_COUPON IS
            '매장-티켓-쿠폰';
            */
        }

        public SHOP_TICKET_COUPON(string shop_code, string tk_cpn_code)
        {
            this.SHOP_CODE = shop_code;
            this.TK_CPN_CODE = tk_cpn_code;
        }
        public string SHOP_CODE { get; set; } = string.Empty;


        public string TK_CPN_CODE { get; set; } = string.Empty;
        public string TK_CPN_NAME { get; set; } = string.Empty;
        public string TK_CLASS_COD { get; set; } = string.Empty;
        public string CPN_DC_FLAG { get; set; } = string.Empty;
        public string CPN_DC_RATE { get; set; } = "0";
        public string CPN_DC_AMT { get; set; } = "0";
        public string CPN_QTY_FLAG { get; set; } = "1";
        public string USE_YN { get; set; } = "Y";
        public string INSERT_DT { get; set; } = string.Empty;
        public string UPDATE_DT { get; set; } = string.Empty;
    }
}

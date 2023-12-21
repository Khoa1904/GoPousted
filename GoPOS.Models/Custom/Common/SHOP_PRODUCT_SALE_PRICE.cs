using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoPOS.Models
{
    public class SHOP_PRODUCT_SALE_PRICE
    {
        /// <summary>
        /// 매장-상품판매단가

        /// </summary>
        public SHOP_PRODUCT_SALE_PRICE()
        {
            /*            
            SHOP_CODE        VARCHAR(6) NOT NULL,
            PRC_MG_CODE      VARCHAR(6) NOT NULL,
            PRC_MG_NAME      VARCHAR(50) NOT NULL,
            STYLE_PRD_CODE   VARCHAR(26) NOT NULL,
            S_DATE           VARCHAR(8) NOT NULL,
            E_DATE           VARCHAR(8) NOT NULL,
            SALE_PRICE_FLAG  VARCHAR(1) NOT NULL,
            SALE_MG_CODE     VARCHAR(4),
            NORMAL_UPRC      NUMERIC(10,2) DEFAULT 0 NOT NULL,
            SALE_UPRC        NUMERIC(10,2) DEFAULT 0 NOT NULL,
            SALE_MG_FLAG     VARCHAR(1) NOT NULL,
            SALE_DC_RATE     NUMERIC(3,0) DEFAULT 0 NOT NULL,
            MG_RATE          NUMERIC(3,0) DEFAULT 0 NOT NULL,
            INSERT_DT        VARCHAR(14),
            UPDATE_DT        VARCHAR(14)

            public string SHOP_CODE           { get; set; } = string.Empty;
            public string PRC_MG_CODE         { get; set; } = string.Empty;
            public string PRC_MG_NAME         { get; set; } = string.Empty;
            public string STYLE_PRD_CODE      { get; set; } = string.Empty;
            public string S_DATE              { get; set; } = string.Empty;
            public string E_DATE              { get; set; } = string.Empty;
            public string SALE_PRICE_FLAG     { get; set; } = string.Empty;
            public string SALE_MG_CODE        { get; set; } = string.Empty;
            public string NORMAL_UPRC         { get; set; } = string.Empty;
            public string SALE_UPRC           { get; set; } = string.Empty;
            public string SALE_MG_FLAG        { get; set; } = string.Empty;
            public string SALE_DC_RATE        { get; set; } = string.Empty;
            public string MG_RATE             { get; set; } = string.Empty;
            public string INSERT_DT           { get; set; } = string.Empty;
            public string UPDATE_DT           { get; set; } = string.Empty;
            */
        }

        public SHOP_PRODUCT_SALE_PRICE(string shop_code, string prc_mg_code)
        {
            this.SHOP_CODE = shop_code;
            this.PRC_MG_CODE = prc_mg_code;
        }

        public string SHOP_CODE { get; set; } = string.Empty;
        public string PRC_MG_CODE { get; set; } = string.Empty;
        public string PRC_MG_NAME { get; set; } = string.Empty;
        public string STYLE_PRD_CODE { get; set; } = string.Empty;
        public string S_DATE { get; set; } = string.Empty;
        public string E_DATE { get; set; } = string.Empty;
        public string SALE_PRICE_FLAG { get; set; } = string.Empty;
        public string SALE_MG_CODE { get; set; } = string.Empty;
        public string NORMAL_UPRC { get; set; } = string.Empty;
        public string SALE_UPRC { get; set; } = string.Empty;
        public string SALE_MG_FLAG { get; set; } = string.Empty;
        public string SALE_DC_RATE { get; set; } = string.Empty;
        public string MG_RATE { get; set; } = string.Empty;
        public string INSERT_DT { get; set; } = string.Empty;
        public string UPDATE_DT { get; set; } = string.Empty;
    }
}

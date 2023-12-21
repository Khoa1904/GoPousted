using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoPOS.Models
{
    public class SHOP_SALE_MARGIN
    {
        /// <summary>
        /// 매장-브랜드판매마진

        /// </summary>
        public SHOP_SALE_MARGIN()
        {
            /*
            SHOP_CODE       VARCHAR(6) NOT NULL,
            SALE_MG_CODE    VARCHAR(4) NOT NULL,
            BRAND_CODE      VARCHAR(8) NOT NULL,
            SALE_MG_FLAG    VARCHAR(1) NOT NULL,
            SALE_DC_RATE    NUMERIC(3,0) DEFAULT 0 NOT NULL,
            MG_RATE         NUMERIC(3,0) DEFAULT 0 NOT NULL,
            USE_YN          VARCHAR(1) DEFAULT 'Y' NOT NULL,
            INSERT_DT       VARCHAR(14),
            UPDATE_DT       VARCHAR(14)

            public string SHOP_CODE           { get; set; } = string.Empty;
            public string SALE_MG_CODE        { get; set; } = string.Empty;
            public string BRAND_CODE          { get; set; } = string.Empty;
            public string SALE_MG_FLAG        { get; set; } = string.Empty;
            public string SALE_DC_RATE        { get; set; } = string.Empty;
            public string MG_RATE             { get; set; } = string.Empty;
            public string USE_YN              { get; set; } = string.Empty;
            public string INSERT_DT           { get; set; } = string.Empty;
            public string UPDATE_DT           { get; set; } = string.Empty;
            
            */
        }

        public SHOP_SALE_MARGIN(string shop_code, string sale_mg_code)
        {
            this.SHOP_CODE = shop_code;
            this.SALE_MG_CODE = sale_mg_code;
        }

        public string SHOP_CODE { get; set; } = string.Empty;
        public string SALE_MG_CODE { get; set; } = string.Empty;
        public string BRAND_CODE { get; set; } = string.Empty;
        public string SALE_MG_FLAG { get; set; } = string.Empty;
        public string SALE_DC_RATE { get; set; } = string.Empty;
        public string MG_RATE { get; set; } = string.Empty;
        public string USE_YN { get; set; } = string.Empty;
        public string INSERT_DT { get; set; } = string.Empty;
        public string UPDATE_DT { get; set; } = string.Empty;
    }
}

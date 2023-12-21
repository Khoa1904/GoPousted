using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoPOS.Models
{
    public class PRODUCT_PRINTER_MAP
    {
        /// <summary>
        /// 상품 주방프린터 연결관리

        /// </summary>
        public PRODUCT_PRINTER_MAP()
        {
            /*
            CREATE TABLE PRODUCT_PRINTER_MAP (
            SHOP_CODE  VARCHAR(6) NOT NULL,
            PRD_CODE   VARCHAR(26) NOT NULL,
            PRT_NO     VARCHAR(2) NOT NULL,
            USE_YN     VARCHAR(1) NOT NULL,
            INSERT_DT  VARCHAR(14),
            UPDATE_DT  VARCHAR(14)
            );

            public string SHOP_CODE      { get; set; } = string.Empty;
            public string PRD_CODE       { get; set; } = string.Empty;
            public string PRT_NO         { get; set; } = string.Empty;
            public string USE_YN         { get; set; } = string.Empty;
            public string INSERT_DT      { get; set; } = string.Empty;
            public string UPDATE_DT      { get; set; } = string.Empty;            

            */
        }

        public PRODUCT_PRINTER_MAP(string shop_code, string prt_no)
        {
            this.SHOP_CODE = shop_code;
            this.PRT_NO = prt_no;
        }

        public string SHOP_CODE { get; set; } = string.Empty;
        public string PRD_CODE { get; set; } = string.Empty;
        public string PRT_NO { get; set; } = string.Empty;
        public string USE_YN { get; set; } = string.Empty;
        public string INSERT_DT { get; set; } = string.Empty;
        public string UPDATE_DT { get; set; } = string.Empty;
    }
}

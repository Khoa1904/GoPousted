using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoPOS.Models
{
    public class SHOP_FOOD_TABLE
    {
        /// <summary>
        /// 매장-외식-테이블정보

        /// </summary>
        public SHOP_FOOD_TABLE()
        {
            /*
            SHOP_CODE          VARCHAR(6) NOT NULL,
            FD_TBL_CODE        VARCHAR(3) NOT NULL,
            FD_TBL_NAME        VARCHAR(20) NOT NULL,
            FD_CLASS_CODE      VARCHAR(2) NOT NULL,
            USE_YN             VARCHAR(1) DEFAULT 'Y' NOT NULL,
            INSERT_DT          VARCHAR(14),
            UPDATE_DT          VARCHAR(14)

            public string SHOP_CODE           { get; set; } = string.Empty;            
            public string FD_TBL_CODE         { get; set; } = string.Empty;            
            public string FD_TBL_NAME         { get; set; } = string.Empty;            
            public string FD_CLASS_CODE       { get; set; } = string.Empty;            
            public string USE_YN              { get; set; } = string.Empty;            
            public string INSERT_DT           { get; set; } = string.Empty;            
            public string UPDATE_DT           { get; set; } = string.Empty;            
            */
        }

        public SHOP_FOOD_TABLE(string shop_code, string fd_tbl_code)
        {                                                   
            this.SHOP_CODE = shop_code;                     
            this.FD_TBL_CODE = fd_tbl_code;
        }

        public string SHOP_CODE { get; set; } = string.Empty;
        public string FD_TBL_CODE { get; set; } = string.Empty;
        public string FD_TBL_NAME { get; set; } = string.Empty;
        public string FD_CLASS_CODE { get; set; } = string.Empty;
        public string USE_YN { get; set; } = string.Empty;
        public string INSERT_DT { get; set; } = string.Empty;
        public string UPDATE_DT { get; set; } = string.Empty;
    }
}

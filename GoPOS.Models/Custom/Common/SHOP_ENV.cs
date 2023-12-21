using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoPOS.Models
{
    public class SHOP_ENV
    {
        /// <summary>
        /// 매장-매장환경설정

        /// </summary>
        public SHOP_ENV()
        {
            /*
            SHOP_CODE        VARCHAR(6) NOT NULL,
            ENV_SET_CODE   VARCHAR(3) NOT NULL,
            ENV_SET_VALUE  VARCHAR(100) NOT NULL,
            USE_YN         VARCHAR(1) DEFAULT 'Y' NOT NULL,
            INSERT_DT      VARCHAR(14),
            UPDATE_DT      VARCHAR(14)

            public string SHOP_CODE           { get; set; } = string.Empty;            
            public string ENV_SET_CODE        { get; set; } = string.Empty;            
            public string ENV_SET_VALUE       { get; set; } = string.Empty;            
            public string USE_YN              { get; set; } = string.Empty;            
            public string INSERT_DT           { get; set; } = string.Empty;            
            public string UPDATE_DT           { get; set; } = string.Empty;            
            */
        }

        public SHOP_ENV(string shop_code, string env_set_code)
        {
            this.SHOP_CODE = shop_code;
            this.ENV_SET_CODE = env_set_code;            
        }

        public string SHOP_CODE { get; set; } = string.Empty;
        public string ENV_SET_CODE { get; set; } = string.Empty;
        public string ENV_SET_VALUE { get; set; } = string.Empty;
        public string USE_YN { get; set; } = string.Empty;
        public string INSERT_DT { get; set; } = string.Empty;
        public string UPDATE_DT { get; set; } = string.Empty;
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoPOS.Models
{
    public class SHOP_POS_ENV
    {
        /// <summary>
        /// 매장-포스별환경설정

        /// </summary>
        public SHOP_POS_ENV()
        {
            /*
            SHOP_CODE        VARCHAR(6) NOT NULL,
            POS_NO           VARCHAR(2) NOT NULL,
            ENV_SET_CODE     VARCHAR(3) NOT NULL,
            ENV_SET_VALUE    VARCHAR(100) NOT NULL,
            USE_YN           VARCHAR(1) DEFAULT 'Y' NOT NULL,
            MODIFY_FLAG      VARCHAR(1) DEFAULT '0' NOT NULL,
            INSERT_DT        VARCHAR(14),
            UPDATE_DT        VARCHAR(14),
            SEND_FLAG        VARCHAR(1) DEFAULT '0' NOT NULL,
            SEND_DT          VARCHAR(14)

            public string SHOP_CODE           { get; set; } = string.Empty;            
            public string POS_NO              { get; set; } = string.Empty;            
            public string ENV_SET_CODE        { get; set; } = string.Empty;            
            public string ENV_SET_VALUE       { get; set; } = string.Empty;            
            public string USE_YN              { get; set; } = string.Empty;            
            public string MODIFY_FLAG         { get; set; } = string.Empty;            
            public string INSERT_DT           { get; set; } = string.Empty;            
            public string UPDATE_DT           { get; set; } = string.Empty;            
            public string SEND_FLAG           { get; set; } = string.Empty;            
            public string SEND_DT             { get; set; } = string.Empty;            
            */
        }

        public SHOP_POS_ENV(string shop_code, string pos_no, string env_set_code)
        {
            this.SHOP_CODE = shop_code;
            this.POS_NO = pos_no;
            this.ENV_SET_CODE = env_set_code;            

        }

        public string SHOP_CODE { get; set; } = string.Empty;
        public string POS_NO { get; set; } = string.Empty;
        public string ENV_SET_CODE { get; set; } = string.Empty;
        public string ENV_SET_VALUE { get; set; } = string.Empty;
        public string USE_YN { get; set; } = string.Empty;
        public string MODIFY_FLAG { get; set; } = string.Empty;
        public string INSERT_DT { get; set; } = string.Empty;
        public string UPDATE_DT { get; set; } = string.Empty;
        public string SEND_FLAG { get; set; } = string.Empty;
        public string SEND_DT { get; set; } = string.Empty;
    }
}

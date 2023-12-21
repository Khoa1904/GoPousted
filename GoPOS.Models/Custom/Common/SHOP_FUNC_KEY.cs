using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoPOS.Models
{
    public class SHOP_FUNC_KEY
    {
        /// <summary>
        /// 매장-기능키관리

        /// </summary>
        public SHOP_FUNC_KEY()
        {
            /*
            SHOP_CODE        VARCHAR(6) NOT NULL,
            FK_NO            VARCHAR(3) NOT NULL,
            FK_NAME          VARCHAR(30) NOT NULL,
            FK_FLAG          VARCHAR(2) NOT NULL,
            AUTH_YN          VARCHAR(1) DEFAULT 'N' NOT NULL,
            FK_USE_YN        VARCHAR(1) DEFAULT 'Y' NOT NULL,
            IMG_FILE_NAME    VARCHAR(50),
            POSITION_YN      VARCHAR(1) DEFAULT 'N' NOT NULL,
            PRD_CODE         VARCHAR(26) DEFAULT 'N' NOT NULL,
            USE_YN           VARCHAR(1) DEFAULT 'Y' NOT NULL,
            INSERT_DT        VARCHAR(14),
            UPDATE_DT        VARCHAR(14)

            public string SHOP_CODE           { get; set; } = string.Empty;            
            public string FK_NO               { get; set; } = string.Empty;            
            public string FK_NAME             { get; set; } = string.Empty;            
            public string FK_FLAG             { get; set; } = string.Empty;            
            public string AUTH_YN             { get; set; } = string.Empty;            
            public string FK_USE_YN           { get; set; } = string.Empty;            
            public string IMG_FILE_NAME       { get; set; } = string.Empty;            
            public string POSITION_YN         { get; set; } = string.Empty;            
            public string PRD_CODE            { get; set; } = string.Empty;            
            public string USE_YN              { get; set; } = string.Empty;            
            public string INSERT_DT           { get; set; } = string.Empty;            
            public string UPDATE_DT           { get; set; } = string.Empty;            
            */
        }

        public SHOP_FUNC_KEY(string shop_code, string fk_no)
        {
            this.SHOP_CODE = shop_code;
            this.FK_NO = fk_no;
        }

        public string SHOP_CODE { get; set; } = string.Empty;
        public string FK_NO { get; set; } = string.Empty;
        public string FK_NAME { get; set; } = string.Empty;
        public string FK_FLAG { get; set; } = string.Empty;
        public string AUTH_YN { get; set; } = string.Empty;
        public string FK_USE_YN { get; set; } = string.Empty;
        public string IMG_FILE_NAME { get; set; } = string.Empty;
        public string POSITION_YN { get; set; } = string.Empty;
        public string PRD_CODE { get; set; } = string.Empty;
        public string USE_YN { get; set; } = string.Empty;
        public string INSERT_DT { get; set; } = string.Empty;
        public string UPDATE_DT { get; set; } = string.Empty;
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoPOS.Models
{
    public class TABLE_GROUP
    {
        /// <summary>
        /// 테이블 그룹관리

        /// </summary>
        public TABLE_GROUP()
        {
            /*            
            SHOP_CODE                VARCHAR(6) NOT NULL,
            TG_CODE                  VARCHAR(2) NOT NULL,
            TG_NAME                  VARCHAR(20) NOT NULL,
            TG_FLAG                  VARCHAR(1) NOT NULL,
            TG_BGIMAGE               VARCHAR(30),
            TG_SORT                  NUMERIC(2,0) NOT NULL,
            TU_CLASS_CODE            VARCHAR(2),
            USE_YN                   VARCHAR(1) NOT NULL,
            INSERT_DT                VARCHAR(14),
            UPDATE_DT                VARCHAR(14)

            public string SHOP_CODE               { get; set; } = string.Empty;
            public string TG_CODE                 { get; set; } = string.Empty;
            public string TG_NAME                 { get; set; } = string.Empty;
            public string TG_FLAG                 { get; set; } = string.Empty;
            public string TG_BGIMAGE              { get; set; } = string.Empty;
            public string TG_SORT                 { get; set; } = string.Empty;
            public string TU_CLASS_CODE           { get; set; } = string.Empty;
            public string USE_YN                  { get; set; } = string.Empty;
            public string INSERT_DT               { get; set; } = string.Empty;
            public string UPDATE_DT               { get; set; } = string.Empty;
            */
        }

        public TABLE_GROUP(string shop_code, string tg_code, string tg_flag)
        {
            this.SHOP_CODE = shop_code;
            this.TG_CODE = tg_code;
            this.TG_FLAG = tg_flag;
        }
        public string SHOP_CODE { get; set; } = string.Empty;
        public string TG_CODE { get; set; } = string.Empty;
        public string TG_NAME { get; set; } = string.Empty;
        public string TG_FLAG { get; set; } = string.Empty;
        public string TG_BGIMAGE { get; set; } = string.Empty;
        public string TG_SORT { get; set; } = string.Empty;
        public string TU_CLASS_CODE { get; set; } = string.Empty;
        public string USE_YN { get; set; } = string.Empty;
        public string INSERT_DT { get; set; } = string.Empty;
        public string UPDATE_DT { get; set; } = string.Empty;
    }
}

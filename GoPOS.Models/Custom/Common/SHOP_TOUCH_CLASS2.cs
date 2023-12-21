using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoPOS.Models
{
    public class SHOP_TOUCH_CLASS2
    {
        /// <summary>
        /// 매장-터치분류

        /// </summary>
        public SHOP_TOUCH_CLASS2()
        {
            /*            
            SHOP_CODE                VARCHAR(6) NOT NULL,
            TU_FLAG                  VARCHAR(1) NOT NULL,
            TU_CLASS_CODE            VARCHAR(2) NOT NULL,
            TU_CLASS_NAME            VARCHAR(20) NOT NULL,
            TU_PAGE                  NUMERIC(2,0) NOT NULL,
            X                        NUMERIC(4,0) NOT NULL,
            Y                        NUMERIC(4,0) NOT NULL,
            WIDTH                    NUMERIC(4,0) NOT NULL,
            HEIGHT                   NUMERIC(4,0) NOT NULL,
            INSERT_DT                VARCHAR(14),
            UPDATE_DT                VARCHAR(14)

            public string SHOP_CODE               { get; set; } = string.Empty;
            public string TU_FLAG                 { get; set; } = string.Empty;
            public string TU_CLASS_CODE           { get; set; } = string.Empty;
            public string TU_CLASS_NAME           { get; set; } = string.Empty;
            public string TU_PAGE                 { get; set; } = string.Empty;
            public string X                       { get; set; } = string.Empty;
            public string Y                       { get; set; } = string.Empty;
            public string WIDTH                   { get; set; } = string.Empty;
            public string HEIGHT                  { get; set; } = string.Empty;
            public string INSERT_DT               { get; set; } = string.Empty;
            public string UPDATE_DT               { get; set; } = string.Empty;

            */
        }

        public SHOP_TOUCH_CLASS2(string shop_code, string tu_flag, string pos_no, string tu_class_code)
        {
            this.SHOP_CODE = shop_code;
            this.TU_FLAG = tu_flag;
            this.TU_CLASS_CODE = tu_class_code;
        }
        public string SHOP_CODE { get; set; } = string.Empty;
        public string TU_FLAG { get; set; } = string.Empty;
        public string TU_CLASS_CODE { get; set; } = string.Empty;
        public string TU_CLASS_NAME { get; set; } = string.Empty;
        public string TU_PAGE { get; set; } = string.Empty;
        public string X { get; set; } = string.Empty;
        public string Y { get; set; } = string.Empty;
        public string WIDTH { get; set; } = string.Empty;
        public string HEIGHT { get; set; } = string.Empty;
        public string INSERT_DT { get; set; } = string.Empty;
        public string UPDATE_DT { get; set; } = string.Empty;
    }
}

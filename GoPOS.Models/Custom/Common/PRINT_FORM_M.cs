using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoPOS.Models
{
    public class PRINT_FORM_M
    {
        /// <summary>
        /// 매장-포스출력물관리

        /// </summary>
        public PRINT_FORM_M()
        {
            /*
            CREATE TABLE PRINT_FORM_M (
             SHOP_CODE       VARCHAR(6) NOT NULL,
             PRT_CLASS_CODE  VARCHAR(2) NOT NULL,
             PRT_FORM        VARCHAR(4000) NOT NULL,
             INSERT_DT       VARCHAR(14),
             UPDATE_DT       VARCHAR(14)
            );

            public string SHOP_CODE      { get; set; } = string.Empty;
            public string PRT_CLASS_CODE { get; set; } = string.Empty;
            public string PRT_FORM       { get; set; } = string.Empty;
            public string INSERT_DT      { get; set; } = string.Empty;
            public string UPDATE_DT      { get; set; } = string.Empty;

            */
        }

        public PRINT_FORM_M(string shop_code, string prt_class_code)
        {
            this.SHOP_CODE = shop_code;
            this.PRT_CLASS_CODE = prt_class_code;
        }

        public string SHOP_CODE { get; set; } = string.Empty;
        public string PRT_CLASS_CODE { get; set; } = string.Empty;
        public string PRT_FORM { get; set; } = string.Empty;
        public string INSERT_DT { get; set; } = string.Empty;
        public string UPDATE_DT { get; set; } = string.Empty;
    }
}

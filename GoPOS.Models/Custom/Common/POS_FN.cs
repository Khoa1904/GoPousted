using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoPOS.Models
{
    /// <summary>
    /// 매장-포스기능키관리

    /// </summary>
    public class POS_FN
    {
        public POS_FN()
        {
            /*
            SHOP_CODE        VARCHAR(6) NOT NULL,
            EMP_FLAG         VARCHAR(1) NOT NULL,
            POS_FN_NO        VARCHAR(3) NOT NULL,
            POS_FN_NAME      VARCHAR(30) NOT NULL,
            POS_FN_FLAG      VARCHAR(1) NOT NULL,
            POS_FN_LOC       NUMERIC(2,0) NOT NULL,
            AUTH_YN          VARCHAR(1) DEFAULT 'Y' NOT NULL,
            ORG_POS_FN_NO    VARCHAR(3) NOT NULL,
            FN_USE_YN_0      VARCHAR(1) DEFAULT 'Y' NOT NULL,
            FN_USE_YN_1      VARCHAR(1) DEFAULT 'Y' NOT NULL,
            FN_USE_YN_2      VARCHAR(1) DEFAULT 'Y' NOT NULL,
            IMG_FILE_NAME_1  VARCHAR(50),
            IMG_FILE_NAME_2  VARCHAR(50),
            MAIN_DISP_FLAG   VARCHAR(1) NOT NULL,
            INSERT_DT        VARCHAR(14),
            UPDATE_DT        VARCHAR(14)

            public string SHOP_CODE           { get; set; } = string.Empty;            
            public string EMP_FLAG            { get; set; } = string.Empty;            
            public string POS_FN_NO           { get; set; } = string.Empty;            
            public string POS_FN_NAME         { get; set; } = string.Empty;            
            public string POS_FN_FLAG         { get; set; } = string.Empty;            
            public string POS_FN_LOC          { get; set; } = string.Empty;            
            public string AUTH_YN             { get; set; } = string.Empty;            
            public string ORG_POS_FN_NO       { get; set; } = string.Empty;            
            public string FN_USE_YN_0         { get; set; } = string.Empty;            
            public string FN_USE_YN_1         { get; set; } = string.Empty;            
            public string FN_USE_YN_2         { get; set; } = string.Empty;            
            public string IMG_FILE_NAME_1     { get; set; } = string.Empty;            
            public string IMG_FILE_NAME_2     { get; set; } = string.Empty;            
            public string MAIN_DISP_FLAG      { get; set; } = string.Empty;            
            public string INSERT_DT           { get; set; } = string.Empty;            
            public string UPDATE_DT           { get; set; } = string.Empty;            
            */
        }

        public POS_FN(string shop_code, string emp_flag, string pos_fn_no, string pos_fn_flag)
        {                                                   
            this.SHOP_CODE = shop_code;                     
            this.EMP_FLAG = emp_flag;
            this.POS_FN_NO = pos_fn_no;
            this.POS_FN_FLAG = pos_fn_flag;            

        }

        public string SHOP_CODE { get; set; } = string.Empty;
        public string EMP_FLAG { get; set; } = string.Empty;
        public string POS_FN_NO { get; set; } = string.Empty;
        public string POS_FN_NAME { get; set; } = string.Empty;
        public string POS_FN_FLAG { get; set; } = string.Empty;
        public string POS_FN_LOC { get; set; } = string.Empty;
        public string AUTH_YN { get; set; } = string.Empty;
        public string ORG_POS_FN_NO { get; set; } = string.Empty;
        public string FN_USE_YN_0 { get; set; } = string.Empty;
        public string FN_USE_YN_1 { get; set; } = string.Empty;
        public string FN_USE_YN_2 { get; set; } = string.Empty;
        public string IMG_FILE_NAME_1 { get; set; } = string.Empty;
        public string IMG_FILE_NAME_2 { get; set; } = string.Empty;
        public string MAIN_DISP_FLAG { get; set; } = string.Empty;
        public string INSERT_DT { get; set; } = string.Empty;
        public string UPDATE_DT { get; set; } = string.Empty;
    }
}

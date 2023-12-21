using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace GoPOS.Models
{
    public class POS_CREDIT_IN
    {
        /// <summary>
        /// POS-비매출-외상입금

        /// </summary>
        public POS_CREDIT_IN()
        {
            /*
           
    CREATE TABLE POS_CREDIT_IN (
    SHOP_CODE       VARCHAR(6) NOT NULL,
    SALE_DATE       VARCHAR(8) NOT NULL,
    CST_NO          VARCHAR(10) NOT NULL,
    WES_IN_NO       VARCHAR(4) NOT NULL,
    POS_NO          VARCHAR(2) NOT NULL,
    REGI_SEQ        VARCHAR(2) DEFAULT '01' NOT NULL,
    WES_IN_YN       VARCHAR(1) DEFAULT 'Y' NOT NULL,
    PAY_TYPE_FLAG   VARCHAR(2) NOT NULL,
    WES_IN_AMT      NUMERIC(12,2) DEFAULT 0 NOT NULL,
    WES_BF_REM_AMT  NUMERIC(12,2) DEFAULT 0 NOT NULL,
    WES_AF_REM_AMT  NUMERIC(12,2) DEFAULT 0 NOT NULL,
    REMARK          VARCHAR(100),
    APPR_LOG_NO     VARCHAR(5),
    ORG_WES_IN_NO   VARCHAR(12),
    INSERT_DT       VARCHAR(14) NOT NULL,
    EMP_NO          VARCHAR(4) NOT NULL,
    SEND_FLAG       VARCHAR(1) DEFAULT '0' NOT NULL,
    SEND_DT         VARCHAR(14),
    WES_FLAG        VARCHAR(1),
    BANK_IN_FLAG    VARCHAR(1),
    CORNER_FLAG     VARCHAR(1),
    CORNER_CODE     VARCHAR(2)
);
                                            Primary keys                                
            ALTER TABLE POS_CREDIT_IN ADD CONSTRAINT POS_INWES_PK PRIMARY KEY(SHOP_CODE, SALE_DATE, CST_NO, WES_IN_NO);

                                            Descriptions                                
            COMMENT ON TABLE POS_CREDIT_IN IS 'POS-비매출-외상입금';
            */
        }

        public POS_CREDIT_IN(string shop_code, string sale_date, string wes_in_no)
        {
            this.SHOP_CODE = shop_code;
            this.SALE_DATE = sale_date;
            this.WES_IN_NO = wes_in_no;
        }
        public string SHOP_CODE { get; set; } = string.Empty;
        public string SALE_DATE { get; set; } = string.Empty;
        public string CST_NO { get; set; } = string.Empty;
        public string WES_IN_NO { get; set; } = string.Empty;
        public string POS_NO { get; set; } = string.Empty;
        public string REGI_SEQ { get; set; } = string.Empty;
        public string WES_IN_YN { get; set; } = string.Empty;
        public string PAY_TYPE_FLAG { get; set; } = string.Empty;
        public string WES_IN_AMT { get; set; } = string.Empty;
        public string WES_BF_REM_AMT { get; set; } = string.Empty;
        public string WES_AF_REM_AMT { get; set; } = string.Empty;
        public string REMARK { get; set; } = string.Empty;
        public string APPR_LOG_NO { get; set; } = string.Empty;
        public string ORG_WES_IN_NO { get; set; } = string.Empty;
        public string INSERT_DT { get; set; } = string.Empty;
        public string EMP_NO { get; set; } = string.Empty;
        public string SEND_FLAG { get; set; } = string.Empty;
        public string SEND_DT { get; set; } = string.Empty;
        public string WES_FLAG { get; set; } = string.Empty;
        public string BANK_IN_FLAG { get; set; } = string.Empty;
        public string CORNER_FLAG { get; set; } = string.Empty;
        public string CORNER_CODE { get; set; } = string.Empty;

        // 추가
        public string NO { get; set; } = string.Empty;

        //가용포인트

    }
}

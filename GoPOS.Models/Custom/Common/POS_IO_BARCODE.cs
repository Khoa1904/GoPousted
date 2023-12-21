using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace GoPOS.Models
{
    /// <summary>
    /// POS-입장/퇴장(바코드) 모델 
    /// </summary>
    public class POS_IO_BARCODE
    {
        public POS_IO_BARCODE()
        {
            /*
             SHOP_CODE          VARCHAR(6) NOT NULL,
             SALE_DATE          VARCHAR(8) NOT NULL,
             BAR_CODE           VARCHAR(26) NOT NULL,
             IN_DT              VARCHAR(14),
             OUT_DT             VARCHAR(14),
             OVER_DT            INTEGER DEFAULT 0 NOT NULL,
             ADD_PAY            NUMERIC(12,2) DEFAULT 0 NOT NULL,
             BILL_NO            VARCHAR(4),
             POS_NO             VARCHAR(2),
             ORDER_NO           VARCHAR(4),
             EXIT_CODE          VARCHAR(3),
             EXIT_ADD_PRD_CODE  VARCHAR(26),
             SEQ_BAR_CODE       NUMERIC(3,0) DEFAULT 0 NOT NULL


            ALTER TABLE POS_IO_BARCODE ADD CONSTRAINT PK_POS_IO_BARCODE PRIMARY KEY (SHOP_CODE, SALE_DATE, BAR_CODE, SEQ_BAR_CODE);
            COMMENT ON TABLE POS_IO_BARCODE IS 'POS-입장/퇴장(바코드)';

            */
        }

        public POS_IO_BARCODE(string shop_code, string sale_date, string bar_code , string seq_bar_code)
        {
            this.SHOP_CODE = shop_code;
            this.SALE_DATE = sale_date;
            this.BAR_CODE = bar_code;
            this.SEQ_BAR_CODE = seq_bar_code;   
        }

        //em.SHOP_CODE, em.EMP_NO, em.EMP_NAME, em.EMP_PWD, em.EMP_FLAG, em.EMP_CLASS_CODE,
	    //em.WEB_USE_YN, em.USER_ID, em.USER_PWD, em.POSTING_YN, em.ORDER_FLAG, 
	    //em.EMP_CARD_NO, em.TEL_NO, em.POST_NO, em.ADDR, em.ADDR_DTL,
	    //em.RETIRE_FLAG, em.SMS_RECV_YN, em.USE_YN, em.INSERT_DT, em.UPDATE_DT
        public string SHOP_CODE         { get; set; } = string.Empty;
        public string SALE_DATE         { get; set; } = string.Empty;
        public string BAR_CODE          { get; set; } = string.Empty;
        public string IN_DT             { get; set; } = string.Empty;
        public string OUT_DT            { get; set; } = string.Empty;
        public string OVER_DT           { get; set; } = string.Empty;
        public string ADD_PAY           { get; set; } = string.Empty;
        public string BILL_NO           { get; set; } = string.Empty;
        public string POS_NO            { get; set; } = string.Empty;
        public string ORDER_NO          { get; set; } = string.Empty;
        public string EXIT_CODE         { get; set; } = string.Empty;
        public string EXIT_ADD_PRD_CODE { get; set; } = string.Empty;
        public string SEQ_BAR_CODE      { get; set; } = string.Empty;


        // 추가 
        public string NO { get; set; } = string.Empty;
    }
}

using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace GoPOS.Models
{
    /// <summary>
    /// POS-배달전화목록 모델 
    /// </summary>
    public class POS_DLV_CID
    {
        public POS_DLV_CID()
        {
            /*

            CREATE TABLE POS_DLV_CID (
                SHOP_CODE      VARCHAR(6) NOT NULL,
                SALE_DATE      VARCHAR(8) NOT NULL,
                CID_TEL_SEQ    INTEGER NOT NULL,
                CID_TEL_FLAG   VARCHAR(1) NOT NULL,
                CID_TEL_NO     VARCHAR(24) NOT NULL,
                TABLE_CODE     VARCHAR(3) NOT NULL,
                ORDER_NO       VARCHAR(4) NOT NULL,
                INSERT_DT      VARCHAR(14) NOT NULL,
                DLV_EXCEPT_YN  VARCHAR(1)
            );


            ALTER TABLE POS_DLV_CID ADD CONSTRAINT PK_POS_DLV_CID PRIMARY KEY(SHOP_CODE, SALE_DATE, CID_TEL_SEQ);

            COMMENT ON TABLE POS_DLV_CID IS'POS-배달전화목록';

            COMMENT ON COLUMN POS_DLV_CID.SHOP_CODE     IS'매장코드';
            COMMENT ON COLUMN POS_DLV_CID.SALE_DATE     IS'영업일자';
            COMMENT ON COLUMN POS_DLV_CID.CID_TEL_SEQ   IS'CID-CALL순번';
            COMMENT ON COLUMN POS_DLV_CID.CID_TEL_FLAG  IS'전화구분(0:수기입력 1:CID-CALL)';
            COMMENT ON COLUMN POS_DLV_CID.CID_TEL_NO    IS'전화번호';
            COMMENT ON COLUMN POS_DLV_CID.TABLE_CODE    IS'테이블코드';
            COMMENT ON COLUMN POS_DLV_CID.ORDER_NO      IS'주문번호';                                                     
            COMMENT ON COLUMN POS_DLV_CID.INSERT_DT     IS'CID_CALL시간';
            COMMENT ON COLUMN POS_DLV_CID.DLV_EXCEPT_YN IS'배달제외여부 Y:제외';
            */
        }

        public POS_DLV_CID(string shop_code, string sale_date, string cid_tel_seq)
        {
            this.SHOP_CODE = shop_code;
            this.SALE_DATE = sale_date;
            this.CID_TEL_SEQ = cid_tel_seq;

        }

        public string SHOP_CODE      { get; set; } = string.Empty;          // '매장코드';
        public string SALE_DATE      { get; set; } = string.Empty;     // '영업일자';
        public string CID_TEL_SEQ    { get; set; } = string.Empty;     // 'CID-CALL순번';
        public string CID_TEL_FLAG   { get; set; } = string.Empty;     // '전화구분(0:수기입력 1:CID-CALL)';
        public string CID_TEL_NO     { get; set; } = string.Empty;     // '전화번호';
        public string TABLE_CODE     { get; set; } = string.Empty;     // '테이블코드';
        public string ORDER_NO       { get; set; } = string.Empty;     // '주문번호';                       
        public string INSERT_DT      { get; set; } = string.Empty;     // 'CID_CALL시간';
        public string DLV_EXCEPT_YN  { get; set; } = string.Empty;      // '배달제외여부 Y:제외';


        // 추가 
        public string NO             { get; set; } = string.Empty;
        public string REMARK         { get; set; } = string.Empty;
        public string ADDR           { get; set; } = string.Empty;


    }
}

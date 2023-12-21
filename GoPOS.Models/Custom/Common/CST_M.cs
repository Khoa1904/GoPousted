using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace GoPOS.Models
{
    public class CST_M
    {
        /// <summary>
        /// 회원-회원정보

        /// </summary>
        public CST_M()
        {
            /*
           
    CREATE TABLE CST_M (
    CST_OGN_CODE       VARCHAR(6) NOT NULL,
    CST_NO             VARCHAR(10) NOT NULL,
    CST_NAME           VARCHAR(24) NOT NULL,
    CST_NAME_ENG       VARCHAR(44),
    CST_CLASS_CODE     VARCHAR(2) NOT NULL,
    INSERT_SHOP_CODE   VARCHAR(6) NOT NULL,
    CST_CARD_NO        VARCHAR(40),
    POST_NO            VARCHAR(6) NOT NULL,
    ADDR               VARCHAR(80) NOT NULL,
    ADDR_DTL           VARCHAR(88),
    RESI_NO            VARCHAR(13),
    BIRTH_DATE         VARCHAR(8),
    SOLAR_YN           VARCHAR(1) DEFAULT 'Y' NOT NULL,
    SEX_FLAG           VARCHAR(1) DEFAULT 'F' NOT NULL,
    EMAIL_ADDR         VARCHAR(30),
    HP_NO              VARCHAR(24) NOT NULL,
    TEL_NO             VARCHAR(24),
    S_TEL_NO           VARCHAR(24) NOT NULL,
    CSH_IDT_NO         VARCHAR(50),
    WEDDING_YN         VARCHAR(1) DEFAULT 'N' NOT NULL,
    WEDDING_DATE       VARCHAR(8),
    EMAIL_RECV_YN      VARCHAR(1) DEFAULT 'N' NOT NULL,
    SMS_RECV_YN        VARCHAR(1) DEFAULT 'N' NOT NULL,
    DM_RECV_YN         VARCHAR(1) DEFAULT 'N' NOT NULL,
    TMP_INSERT_YN      VARCHAR(1) DEFAULT 'N' NOT NULL,
    CST_CARD_USE_FLAG  VARCHAR(1) DEFAULT 0 NOT NULL,
    CST_CARD_ISS_CNT   NUMERIC(3,0) DEFAULT 0 NOT NULL,
    ORG_CST_CARD_NO    VARCHAR(40),
    CST_REMARK         VARCHAR(300),
    USE_YN             VARCHAR(1) DEFAULT 'N' NOT NULL,
    INSERT_DT          VARCHAR(14),
    UPDATE_DT          VARCHAR(14)
    );



                                            Primary keys                                
            ALTER TABLE CST_M ADD CONSTRAINT HCS_CSTMS_PK PRIMARY KEY(CST_OGN_CODE, CST_NO);
                                              Indices                                   
            CREATE INDEX CST_M_IDX1 ON CST_M(CST_NAME);
            CREATE INDEX CST_M_IDX2 ON CST_M(S_TEL_NO);
                                            Descriptions                                
            COMMENT ON TABLE CST_M IS '회원-회원정보';



            */
        }

        public CST_M(string cst_ogn_code, string cst_no)
        {
            this.CST_OGN_CODE = cst_ogn_code;
            this.CST_NO = cst_no;
        }

        public string CST_OGN_CODE        { get; set; } = string.Empty;
        public string CST_NO              { get; set; } = string.Empty;
        public string CST_NAME            { get; set; } = string.Empty;
        public string CST_NAME_ENG        { get; set; } = string.Empty;
        public string CST_CLASS_CODE      { get; set; } = string.Empty;
        public string INSERT_SHOP_CODE    { get; set; } = string.Empty;
        public string CST_CARD_NO         { get; set; } = string.Empty;
        public string POST_NO             { get; set; } = string.Empty;
        public string ADDR                { get; set; } = string.Empty;
        public string ADDR_DTL            { get; set; } = string.Empty;
        public string RESI_NO             { get; set; } = string.Empty;
        public string BIRTH_DATE          { get; set; } = string.Empty;
        public string SOLAR_YN            { get; set; } = string.Empty;
        public string SEX_FLAG            { get; set; } = string.Empty;
        public string EMAIL_ADDR          { get; set; } = string.Empty;
        public string HP_NO               { get; set; } = string.Empty;
        public string TEL_NO              { get; set; } = string.Empty;
        public string S_TEL_NO            { get; set; } = string.Empty;
        public string CSH_IDT_NO          { get; set; } = string.Empty;
        public string WEDDING_YN          { get; set; } = string.Empty;
        public string WEDDING_DATE        { get; set; } = string.Empty;
        public string EMAIL_RECV_YN       { get; set; } = string.Empty;
        public string SMS_RECV_YN         { get; set; } = string.Empty;
        public string DM_RECV_YN          { get; set; } = string.Empty;
        public string TMP_INSERT_YN       { get; set; } = string.Empty;
        public string CST_CARD_USE_FLAG   { get; set; } = string.Empty;
        public string CST_CARD_ISS_CNT    { get; set; } = string.Empty;
        public string ORG_CST_CARD_NO     { get; set; } = string.Empty;
        public string CST_REMARK          { get; set; } = string.Empty;
        public string USE_YN              { get; set; } = string.Empty;
        public string INSERT_DT           { get; set; } = string.Empty;
        public string UPDATE_DT           { get; set; } = string.Empty;

        // 추가
        public string NO                  { get; set; } = string.Empty;
        
        //가용포인트
        public string AVL_POINT           { get; set; } = string.Empty;

    }
}

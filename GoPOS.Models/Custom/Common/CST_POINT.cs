using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace GoPOS.Models
{
    public class CST_POINT
    {
        /// <summary>
        /// 회원-포인트정보

        /// </summary>
        public CST_POINT()
        {
            /*
            CREATE TABLE CST_POINT (
    CST_OGN_CODE         VARCHAR(6) NOT NULL,
    CST_NO               VARCHAR(10) NOT NULL,
    ACC_SALE_CNT         NUMERIC(15,0) DEFAULT 0 NOT NULL,
    ACC_SALE_AMT         NUMERIC(15,0) DEFAULT 0 NOT NULL,
    ACC_POINT            NUMERIC(15,0) DEFAULT 0 NOT NULL,
    USE_POINT            NUMERIC(15,0) DEFAULT 0 NOT NULL,
    AVL_POINT            NUMERIC(15,0) DEFAULT 0 NOT NULL,
    ADJ_POINT            NUMERIC(15,0) DEFAULT 0 NOT NULL,
    POINT_ACC_CNT        NUMERIC(15,0) DEFAULT 0 NOT NULL,
    POINT_USE_CNT        NUMERIC(15,0) DEFAULT 0 NOT NULL,
    F_SALE_DATE          VARCHAR(8),
    L_SALE_DATE          VARCHAR(8),
    INSERT_SHOP_CODE     VARCHAR(6) NOT NULL,
    INSERT_ACC_SALE_CNT  NUMERIC(15,0) DEFAULT 0 NOT NULL,
    INSERT_ACC_SALE_AMT  NUMERIC(15,0) DEFAULT 0 NOT NULL,
    INSERT_F_SALE_DATE   VARCHAR(8),
    INSERT_L_SALE_DATE   VARCHAR(8),
    INSERT_DT            VARCHAR(14),
    UPDATE_DT            VARCHAR(14)
);
                                            Primary keys                                

            ALTER TABLE CST_POINT ADD CONSTRAINT HCS_CSTPT_PK PRIMARY KEY(CST_OGN_CODE, CST_NO);


                                            Descriptions                               
            COMMENT ON TABLE CST_POINT IS '회원-포인트정보';

            */
        }

        public CST_POINT(string cst_ogn_code, string cst_no)
        {
            this.CST_OGN_CODE = cst_ogn_code;
            this.CST_NO = cst_no;
        }

        public string CST_OGN_CODE        { get; set; } = string.Empty;
        public string CST_NO              { get; set; } = string.Empty;
        public string ACC_SALE_CNT        { get; set; } = string.Empty;
        public string ACC_SALE_AMT        { get; set; } = string.Empty;
        public string ACC_POINT           { get; set; } = string.Empty;
        public string USE_POINT           { get; set; } = string.Empty;
        public string AVL_POINT           { get; set; } = string.Empty;
        public string ADJ_POINT           { get; set; } = string.Empty;
        public string POINT_ACC_CNT       { get; set; } = string.Empty;
        public string POINT_USE_CNT       { get; set; } = string.Empty;
        public string F_SALE_DATE         { get; set; } = string.Empty;
        public string L_SALE_DATE         { get; set; } = string.Empty;
        public string INSERT_SHOP_CODE    { get; set; } = string.Empty;
        public string INSERT_ACC_SALE_CNT { get; set; } = string.Empty;
        public string INSERT_ACC_SALE_AMT { get; set; } = string.Empty;
        public string INSERT_F_SALE_DATE  { get; set; } = string.Empty;
        public string INSERT_L_SALE_DATE  { get; set; } = string.Empty;
        public string INSERT_DT           { get; set; } = string.Empty;
        public string UPDATE_DT           { get; set; } = string.Empty;
                                          
        // 추가
        public string NO                  { get; set; } = string.Empty;

}
}

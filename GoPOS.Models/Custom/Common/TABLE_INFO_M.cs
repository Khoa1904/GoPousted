using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoPOS.Models
{
    public class TABLE_INFO_M
    {
        /// <summary>
        /// 테이블 정보

        /// </summary>
        public TABLE_INFO_M()
        {
            /*
            SHOP_CODE          VARCHAR(6) NOT NULL,
            TABLE_CODE         VARCHAR(3) NOT NULL,
            TABLE_NAME         VARCHAR(20) NOT NULL,
            TG_CODE            VARCHAR(2) NOT NULL,
            GROUP_CODE         VARCHAR(2),
            SEAT_NUM           INTEGER DEFAULT 0 NOT NULL,
            X                  NUMERIC(4,0) NOT NULL,
            Y                  NUMERIC(4,0) NOT NULL,
            WIDTH              NUMERIC(4,0) NOT NULL,
            HEIGHT             NUMERIC(4,0) NOT NULL,
            SHAPE_FLAG         VARCHAR(1) NOT NULL,
            TABLE_FLAG         VARCHAR(1),
            USE_YN             VARCHAR(1) DEFAULT 'Y' NOT NULL,
            STATUS_FLAG        VARCHAR(1) DEFAULT '1' NOT NULL,
            LOCK_POS_NO        VARCHAR(2),
            LOCK_FLAG          VARCHAR(1),
            INSERT_DT          VARCHAR(14),
            UPDATE_DT          VARCHAR(14),
            CID_TEL_NO         VARCHAR(24),
            CID_LINE_NO        VARCHAR(1),
            NEW_CST_FLAG       VARCHAR(1) DEFAULT '0' NOT NULL,
            CST_NO             VARCHAR(10),
            CST_NAME           VARCHAR(24),
            DLV_ADDR           VARCHAR(80),
            DLV_ADDR_DTL       VARCHAR(88),
            DLV_PROC_FLAG      NUMERIC(1,0) DEFAULT 0 NOT NULL,
            DLV_ORDER_NO       VARCHAR(4),
            DLV_CL_CODE        VARCHAR(3),
            DLV_CM_CODE        VARCHAR(5),
            CID_CALL_DT        VARCHAR(14),
            DLV_IF_ADDR        VARCHAR(2048)

            public string SHOP_CODE           { get; set; } = string.Empty;            
            public string TABLE_CODE          { get; set; } = string.Empty;            
            public string TABLE_NAME          { get; set; } = string.Empty;            
            public string TG_CODE             { get; set; } = string.Empty;            
            public string GROUP_CODE          { get; set; } = string.Empty;            
            public string SEAT_NUM            { get; set; } = string.Empty;            
            public string X                   { get; set; } = string.Empty;            
            public string Y                   { get; set; } = string.Empty;            
            public string WIDTH               { get; set; } = string.Empty;            
            public string HEIGHT              { get; set; } = string.Empty;            
            public string SHAPE_FLAG          { get; set; } = string.Empty;            
            public string TABLE_FLAG          { get; set; } = string.Empty;            
            public string USE_YN              { get; set; } = string.Empty;            
            public string STATUS_FLAG         { get; set; } = string.Empty;            
            public string LOCK_POS_NO         { get; set; } = string.Empty;            
            public string LOCK_FLAG           { get; set; } = string.Empty;            
            public string INSERT_DT           { get; set; } = string.Empty;            
            public string UPDATE_DT           { get; set; } = string.Empty;            
            public string CID_TEL_NO          { get; set; } = string.Empty;            
            public string CID_LINE_NO         { get; set; } = string.Empty;            
            public string NEW_CST_FLAG        { get; set; } = string.Empty;            
            public string CST_NO              { get; set; } = string.Empty;            
            public string CST_NAME            { get; set; } = string.Empty;            
            public string DLV_ADDR            { get; set; } = string.Empty;            
            public string DLV_ADDR_DTL        { get; set; } = string.Empty;            
            public string DLV_PROC_FLAG       { get; set; } = string.Empty;            
            public string DLV_ORDER_NO        { get; set; } = string.Empty;            
            public string DLV_CL_CODE         { get; set; } = string.Empty;            
            public string DLV_CM_CODE         { get; set; } = string.Empty;            
            public string CID_CALL_DT         { get; set; } = string.Empty;            
            public string DLV_IF_ADDR         { get; set; } = string.Empty;            
            */
        }

        public TABLE_INFO_M(string shop_code, string table_code, string tg_code)
        {                                                   
            this.SHOP_CODE = shop_code;                     
            this.TABLE_CODE = table_code;
            this.TG_CODE = tg_code;            
        }

        public string SHOP_CODE { get; set; } = string.Empty;
        public string TABLE_CODE { get; set; } = string.Empty;
        public string TABLE_NAME { get; set; } = string.Empty;
        public string TG_CODE { get; set; } = string.Empty;
        public string GROUP_CODE { get; set; } = string.Empty;
        public string SEAT_NUM { get; set; } = string.Empty;
        public string X { get; set; } = string.Empty;
        public string Y { get; set; } = string.Empty;
        public string WIDTH { get; set; } = string.Empty;
        public string HEIGHT { get; set; } = string.Empty;
        public string SHAPE_FLAG { get; set; } = string.Empty;
        public string TABLE_FLAG { get; set; } = string.Empty;
        public string USE_YN { get; set; } = string.Empty;
        public string STATUS_FLAG { get; set; } = string.Empty;
        public string LOCK_POS_NO { get; set; } = string.Empty;
        public string LOCK_FLAG { get; set; } = string.Empty;
        public string INSERT_DT { get; set; } = string.Empty;
        public string UPDATE_DT { get; set; } = string.Empty;
        public string CID_TEL_NO { get; set; } = string.Empty;
        public string CID_LINE_NO { get; set; } = string.Empty;
        public string NEW_CST_FLAG { get; set; } = string.Empty;
        public string CST_NO { get; set; } = string.Empty;
        public string CST_NAME { get; set; } = string.Empty;
        public string DLV_ADDR { get; set; } = string.Empty;
        public string DLV_ADDR_DTL { get; set; } = string.Empty;
        public string DLV_PROC_FLAG { get; set; } = string.Empty;
        public string DLV_ORDER_NO { get; set; } = string.Empty;
        public string DLV_CL_CODE { get; set; } = string.Empty;
        public string DLV_CM_CODE { get; set; } = string.Empty;
        public string CID_CALL_DT { get; set; } = string.Empty;
        public string DLV_IF_ADDR { get; set; } = string.Empty;


        // SP 호출후 RETURN
        public string R_LOCK_POS_NO { get; set; } = string.Empty; // r_lock_pos_no varchar(2)

        public string R_RESULT_CODE { get; set; } = string.Empty; // r_result_code smallint


    }
}

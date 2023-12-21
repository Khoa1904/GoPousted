using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace GoPOS.Models
{
    public class SHOP_CORNER
    {
        /// <summary>
        /// 매장-코너정보

        /// </summary>
        public SHOP_CORNER()
        {
            /*
            SHOP_CODE          VARCHAR(6) NOT NULL,
            CORNER_CODE        VARCHAR(2) NOT NULL,
            CORNER_NAME        VARCHAR(30) NOT NULL,
            OWNER_NAME         VARCHAR(30),
            BIZ_NO             VARCHAR(10),
            TEL_NO             VARCHAR(15),
            CSH_FEE_RATE       NUMERIC(5,2) DEFAULT 0 NOT NULL,
            CRD_FEE_RATE       NUMERIC(5,2) DEFAULT 0 NOT NULL,
            ETC_FEE_RATE       NUMERIC(5,2) DEFAULT 0 NOT NULL,
            VAN_CODE           VARCHAR(2),
            VAN_TERM_NO        VARCHAR(16),
            CASH_VAN_CODE      VARCHAR(2),
            CASH_VAN_TERM_NO   VARCHAR(16),
            USE_YN             VARCHAR(1) DEFAULT 'Y' NOT NULL,
            INSERT_DT          VARCHAR(14),
            UPDATE_DT          VARCHAR(14),
            VAN_SER_NO         VARCHAR(20),
            CASH_VAN_SER_NO    VARCHAR(20),
            VAN_CERT_YN        VARCHAR(1) DEFAULT 'N' NOT NULL,
            VAN_CERT_SDT       VARCHAR(14),
            VAN_CERT_EDT       VARCHAR(14),
            VAN_CERT_CNT       NUMERIC(6,0) DEFAULT 0 NOT NULL,
            SEND_FLAG          VARCHAR(1) DEFAULT 0 NOT NULL,
            SEND_DT            VARCHAR(14),
            WORK_INDEX         VARCHAR(2),
            WORK_KEY           VARCHAR(32),
            OKCBG_TERM_NO      VARCHAR(15),
            OKCBG_SER_NO       VARCHAR(16),
            W_KEY              VARCHAR(32),
            DAUMMSP_TERM_NO    VARCHAR(20),
            DAUMMSP_SER_NO     VARCHAR(20),
            VAN_SAM_ID         VARCHAR(16),
            VAN_SAM_NO         VARCHAR(16),
            VAN_SAM_RECV_FLAG  VARCHAR(1),
            MCP_TERM_NO        VARCHAR(20),
            MCP_SER_NO         VARCHAR(20)

            public string SHOP_CODE           { get; set; } = string.Empty;            
            public string CORNER_CODE         { get; set; } = string.Empty;
            public string CORNER_NAME         { get; set; } = string.Empty;
            public string OWNER_NAME          { get; set; } = string.Empty;
            public string BIZ_NO              { get; set; } = string.Empty;
            public string TEL_NO              { get; set; } = string.Empty;
            public string CSH_FEE_RATE        { get; set; } = string.Empty;
            public string CRD_FEE_RATE        { get; set; } = string.Empty;
            public string ETC_FEE_RATE        { get; set; } = string.Empty;
            public string VAN_CODE            { get; set; } = string.Empty;
            public string VAN_TERM_NO         { get; set; } = string.Empty;
            public string CASH_VAN_CODE       { get; set; } = string.Empty;
            public string CASH_VAN_TERM_NO    { get; set; } = string.Empty;
            public string USE_YN              { get; set; } = string.Empty;
            public string INSERT_DT           { get; set; } = string.Empty;
            public string UPDATE_DT           { get; set; } = string.Empty;
            public string VAN_SER_NO          { get; set; } = string.Empty;
            public string CASH_VAN_SER_NO     { get; set; } = string.Empty;
            public string VAN_CERT_YN         { get; set; } = string.Empty;
            public string VAN_CERT_SDT        { get; set; } = string.Empty;
            public string VAN_CERT_EDT        { get; set; } = string.Empty;
            public string VAN_CERT_CNT        { get; set; } = string.Empty;
            public string SEND_FLAG           { get; set; } = string.Empty;
            public string SEND_DT             { get; set; } = string.Empty;
            public string WORK_INDEX          { get; set; } = string.Empty;
            public string WORK_KEY            { get; set; } = string.Empty;
            public string OKCBG_TERM_NO       { get; set; } = string.Empty;
            public string OKCBG_SER_NO        { get; set; } = string.Empty;
            public string W_KEY               { get; set; } = string.Empty;
            public string DAUMMSP_TERM_NO     { get; set; } = string.Empty;
            public string DAUMMSP_SER_NO      { get; set; } = string.Empty;
            public string VAN_SAM_ID          { get; set; } = string.Empty;
            public string VAN_SAM_NO          { get; set; } = string.Empty;
            public string VAN_SAM_RECV_FLAG   { get; set; } = string.Empty;
            public string MCP_TERM_NO         { get; set; } = string.Empty;
            public string MCP_SER_NO          { get; set; } = string.Empty;
            */
        }

        public SHOP_CORNER(string shop_code, string corner_code)
        {                                                   
            this.SHOP_CODE = shop_code;                     
            this.CORNER_CODE = corner_code;
        }
        [JsonPropertyName("storeCode")]
        public string SHOP_CODE { get; set; } = string.Empty;
        [JsonPropertyName("cornerCode")]
        public string CORNER_CODE { get; set; } = string.Empty;
        [JsonPropertyName("cornerNm")]
        public string CORNER_NAME { get; set; } = string.Empty;
        [JsonPropertyName("repNm")]
        public string OWNER_NAME { get; set; } = string.Empty;
        [JsonPropertyName("storeCorpno")]
        public string BIZ_NO { get; set; } = string.Empty;
        [JsonPropertyName("storeTelno")]
        public string TEL_NO { get; set; } = string.Empty;
        [JsonPropertyName("cashCmsnRate")]
        public string CSH_FEE_RATE { get; set; } = string.Empty;
        [JsonPropertyName("cardCmsnRate")]
        public string CRD_FEE_RATE { get; set; } = string.Empty;
        [JsonPropertyName("etcCmsnRate")]
        public string ETC_FEE_RATE { get; set; } = string.Empty;
        [JsonPropertyName("defVanSerialNo")]
        public string VAN_CODE { get; set; } = string.Empty;
        [JsonPropertyName("defVanContractNo")]
        public string VAN_TERM_NO { get; set; } = string.Empty;
        [JsonPropertyName("cashVanSerialNo")]
        public string CASH_VAN_CODE { get; set; } = string.Empty;
        [JsonPropertyName("cashVanContractNo")]
        public string CASH_VAN_TERM_NO { get; set; } = string.Empty;
        [JsonPropertyName("useAt")]
        public string USE_YN { get; set; } = string.Empty;
        [JsonPropertyName("createdAt")]
        public string INSERT_DT { get; set; } = string.Empty;
        [JsonPropertyName("updatedAt")]
        public string UPDATE_DT { get; set; } = string.Empty;
        [JsonPropertyName("VAN_SER_NO")]
        public string VAN_SER_NO { get; set; } = string.Empty;
        [JsonPropertyName("CASH_VAN_SER_NO")]
        public string CASH_VAN_SER_NO { get; set; } = string.Empty;
        [JsonPropertyName("authAt")]
        public string VAN_CERT_YN { get; set; } = string.Empty;
        [JsonPropertyName("initAuthDt")]
        public string VAN_CERT_SDT { get; set; } = string.Empty;
        [JsonPropertyName("lastAuthDt")]
        public string VAN_CERT_EDT { get; set; } = string.Empty;
        [JsonPropertyName("authCnt")]
        public string VAN_CERT_CNT { get; set; } = string.Empty;
        [JsonPropertyName("sendSeCode")]
        public string SEND_FLAG { get; set; } = string.Empty;
        [JsonPropertyName("sendDt")]
        public string SEND_DT { get; set; } = string.Empty;
        [JsonPropertyName("WORK_INDEX")]
        public string WORK_INDEX { get; set; } = string.Empty;
        [JsonPropertyName("WORK_KEY")]
        public string WORK_KEY { get; set; } = string.Empty;
        [JsonPropertyName("OKCBG_TERM_NO")]
        public string OKCBG_TERM_NO { get; set; } = string.Empty;
        [JsonPropertyName("OKCBG_SER_NO")]
        public string OKCBG_SER_NO { get; set; } = string.Empty;
        [JsonPropertyName("W_KEY")]
        public string W_KEY { get; set; } = string.Empty;
        [JsonPropertyName("DAUMMSP_TERM_NO")]
        public string DAUMMSP_TERM_NO { get; set; } = string.Empty;
        [JsonPropertyName("DAUMMSP_SER_NO")]
        public string DAUMMSP_SER_NO { get; set; } = string.Empty;
        [JsonPropertyName("VAN_SAM_ID")]
        public string VAN_SAM_ID { get; set; } = string.Empty;
        [JsonPropertyName("VAN_SAM_NO")]
        public string VAN_SAM_NO { get; set; } = string.Empty;
        [JsonPropertyName("VAN_SAM_RECV_FLAG")]
        public string VAN_SAM_RECV_FLAG { get; set; } = string.Empty;
        [JsonPropertyName("MCP_TERM_NO")]
        public string MCP_TERM_NO { get; set; } = string.Empty;
        [JsonPropertyName("MCP_SER_NO")]
        public string MCP_SER_NO { get; set; } = string.Empty;
    }
}

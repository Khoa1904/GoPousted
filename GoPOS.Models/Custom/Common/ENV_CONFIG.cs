using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Text.Json.Serialization;

namespace GoPOS.Models._0_Common
{
    public class ENV_CONFIG
    {
        public ENV_CONFIG()
        {
            /*
            CREATE TABLE ENV_CONFIG (
            ENV_SET_CODE    VARCHAR(3) NOT NULL,
            ENV_SET_NAME    VARCHAR(100) NOT NULL,
            ENV_SET_FLAG    VARCHAR(1) NOT NULL,
            USE_YN          VARCHAR(1) DEFAULT 'Y' NOT NULL,
            ENV_GROUP_CODE  VARCHAR(3),
            REMARK          VARCHAR(500),
            INSERT_DT       VARCHAR(14),
            UPDATE_DT       VARCHAR(14)
            );

            //ALTER TABLE ENV_CONFIG ADD CONSTRAINT CCD_ENVHD_PK PRIMARY KEY(ENV_SET_CODE);
            
            //CREATE INDEX CCD_ENVHD_X1 ON ENV_CONFIG(ENV_SET_FLAG, ENV_SET_CODE);
            
            COMMENT ON TABLE ENV_CONFIG IS'시스템-환경설정-HD';
                                        
            COMMENT ON COLUMN ENV_CONFIG.ENV_SET_CODE   IS'환경세팅코드';
            COMMENT ON COLUMN ENV_CONFIG.ENV_SET_NAME   IS'환경세팅명';
            COMMENT ON COLUMN ENV_CONFIG.ENV_SET_FLAG   IS'환경세팅구분 ( CMM_CODE : 005 )';
            COMMENT ON COLUMN ENV_CONFIG.USE_YN         IS'사용여부';
            COMMENT ON COLUMN ENV_CONFIG.ENV_GROUP_CODE IS'환경그룹코드 CMM_CODE:117';
            COMMENT ON COLUMN ENV_CONFIG.REMARK         IS'비고';
            COMMENT ON COLUMN ENV_CONFIG.INSERT_DT      IS'최초등록일시';
            COMMENT ON COLUMN ENV_CONFIG.UPDATE_DT      IS'최종수정일시';
            */
        }

        public ENV_CONFIG(string env_set_code)
        {
            this.ENV_SET_CODE = env_set_code;
        }
        [JsonPropertyName("envCode")]
        public string ENV_SET_CODE   { get; set; } = string.Empty;     //'환경세팅코드';   
        [JsonPropertyName("envNm")]
        public string ENV_SET_NAME   { get; set; } = string.Empty;     //'환경세팅명';
        [JsonPropertyName("envSetFg")]
        public string ENV_SET_FLAG   { get; set; } = string.Empty;     //'환경세팅구분 ( CMM_CODE : 005 )';
        [JsonPropertyName("useAt")]
        public string USE_YN         { get; set; } = string.Empty;     //'사용여부';
        [JsonPropertyName("grpCode")]
        public string ENV_GROUP_CODE { get; set; } = string.Empty;     //'환경그룹코드 CMM_CODE:117';
        [JsonPropertyName("envCodeNote")]
        public string REMARK         { get; set; } = string.Empty;     //'비고';
        [JsonPropertyName("createdAt")]
        public string INSERT_DT      { get; set; } = string.Empty;     //'최초등록일시';
        [JsonPropertyName("updatedAt")]
        public string UPDATE_DT      { get; set; } = string.Empty;     //'최종수정일시';

        //추가
        public string NO             { get; set; } = string.Empty;     // NO

        public string ENV_VALUE_CODE { get; set; } = string.Empty;     // 환경세팅값코드 - ENV_CONFIG_VALUE 
        public string ENV_VALUE_NAME { get; set; } = string.Empty;     //환경세팅값명    - ENV_CONFIG_VALUE


        //public string ENV_GROUP_CODE  { get; set; } = string.Empty;
        public string  ENV_GROUP_NAME { get; set; } = string.Empty;
        //public string ENV_SET_NAME    { get; set; } = string.Empty;
        //public string ENV_VALUE_NAME  { get; set; } = string.Empty;
        public string ENV_SET_VALUE { get; set; } = string.Empty;


        //public string ENV_SET_CODE    { get; set; } = string.Empty;



    }
}

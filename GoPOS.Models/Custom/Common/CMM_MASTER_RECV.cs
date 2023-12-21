using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace GoPOS.Models
{
    public class CMM_MASTER_RECV
    {
        public CMM_MASTER_RECV()
        {
            //CREATE TABLE CMM_MASTER_RECV(
            //    RECV_ID    INTEGER NOT NULL,
            //    REQT_ID    VARCHAR(5) NOT NULL,
            //    REQT_NAME  VARCHAR(30) NOT NULL,
            //    LAST_SEQ   BIGINT NOT NULL,
            //    REQT_RM    VARCHAR(30) NOT NULL,
            //    UPDATE_DT  VARCHAR(14)
            //);

            //                                Primary keys                                
            //ALTER TABLE CMM_MASTER_RECV ADD CONSTRAINT PK_CMM_MASTER_RECV PRIMARY KEY(RECV_ID);
                                              //Indices                                   
            //CREATE UNIQUE INDEX CMM_MASTER_RECV_IDX1 ON CMM_MASTER_RECV(REQT_ID);

            //Descriptions                                
            //COMMENT ON TABLE CMM_MASTER_RECV IS 'POS-마스터수신관리';
            //Fields descriptions                             
            //COMMENT ON COLUMN CMM_MASTER_RECV.RECV_ID   IS '마스터일련번호';
            //COMMENT ON COLUMN CMM_MASTER_RECV.REQT_ID   IS '마스터수신-전문구문ID';
            //COMMENT ON COLUMN CMM_MASTER_RECV.REQT_NAME IS '마스터수신-전문명';
            //COMMENT ON COLUMN CMM_MASTER_RECV.LAST_SEQ  IS '최종수신-일련번호';
            //COMMENT ON COLUMN CMM_MASTER_RECV.REQT_RM   IS '마스터수신-전문비고';
            //COMMENT ON COLUMN CMM_MASTER_RECV.UPDATE_DT IS '최종수정일시';
        }

        public CMM_MASTER_RECV(string recv_id)
        {
            this.RECV_ID = recv_id;
        }

        public string RECV_ID   { get; set; } = string.Empty;   // '마스터일련번호';
        public string REQT_ID   { get; set; } = string.Empty;   // '마스터수신-전문구문ID';
        public string REQT_NAME { get; set; } = string.Empty;   // '마스터수신-전문명';
        public string LAST_SEQ  { get; set; } = string.Empty;   // '최종수신-일련번호';
        public string REQT_RM   { get; set; } = string.Empty;   // '마스터수신-전문비고';
        public string UPDATE_DT { get; set; } = string.Empty;   // '최종수정일시';

        //추가
        public string NO { get; set; } = string.Empty;
    }
}

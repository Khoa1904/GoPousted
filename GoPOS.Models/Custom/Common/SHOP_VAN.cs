using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoPOS.Models
{
    public class SHOP_VAN
    {
        public SHOP_VAN()
        {
            /*
            CREATE TABLE SHOP_VAN(
                        SHOP_CODE     VARCHAR(6) NOT NULL,
                        POS_NO        VARCHAR(2) NOT NULL,
                        POS_REMARK    VARCHAR(100) NOT NULL,
                        LOCAL_DB_YN   VARCHAR(1) NOT NULL,
                        USE_YN        VARCHAR(1) NOT NULL,
                        CRD_VAN_SEQ   VARCHAR(10),
                        CASH_VAN_SEQ  VARCHAR(10),
                        IP_ADDR       VARCHAR(15),
                        INSERT_DT     VARCHAR(14),
                        UPDATE_DT     VARCHAR(14),
                        VAN_CODE      VARCHAR(2),
                        BIZ_NO        VARCHAR(10),
                        VAN_TERM_NO   VARCHAR(16),
                        VAN_SER_NO    VARCHAR(20),
                        VAN_CERT_YN   VARCHAR(1) DEFAULT 'N' NOT NULL,
                        VAN_CERT_SDT  VARCHAR(14),
                        VAN_CERT_EDT  VARCHAR(14),
                        VAN_CERT_CNT  NUMERIC(6, 0) DEFAULT 0 NOT NULL,
                        SEND_FLAG     VARCHAR(1) DEFAULT 0 NOT NULL,
                        SEND_DT       VARCHAR(14),
                        POS_VER       VARCHAR(12),
                        W_KEY         VARCHAR(32),
                        WORK_INDEX    VARCHAR(2),
                        WORK_KEY      VARCHAR(32)
                    );
            ALTER TABLE SHOP_VAN ADD CONSTRAINT PK_SHOP_VAN PRIMARY KEY(SHOP_CODE, POS_NO);
            
            COMMENT ON TABLE SHOP_VAN                  IS '[매장]-포스VAN마스터';

            COMMENT ON COLUMN SHOP_VAN.SHOP_CODE       IS '매장코드';
            COMMENT ON COLUMN SHOP_VAN.POS_NO          IS '포스번호';
            COMMENT ON COLUMN SHOP_VAN.CRD_VAN_SEQ     IS '신용카드-승인일련번호';
            COMMENT ON COLUMN SHOP_VAN.CASH_VAN_SEQ    IS '현금영수증-승인일련번호';
            COMMENT ON COLUMN SHOP_VAN.IP_ADDR         IS '접속IP주소';
            COMMENT ON COLUMN SHOP_VAN.INSERT_DT       IS '최초등록일시';
            COMMENT ON COLUMN SHOP_VAN.UPDATE_DT       IS '최종수정일시';
            COMMENT ON COLUMN SHOP_VAN.VAN_CODE        IS '밴사코드';
            COMMENT ON COLUMN SHOP_VAN.BIZ_NO          IS '사업자번호';
            COMMENT ON COLUMN SHOP_VAN.VAN_TERM_NO     IS '단말기번호';
            COMMENT ON COLUMN SHOP_VAN.VAN_SER_NO      IS '단말기시리얼번호';
            COMMENT ON COLUMN SHOP_VAN.VAN_CERT_YN     IS '인증여부';
            COMMENT ON COLUMN SHOP_VAN.VAN_CERT_SDT    IS '최초인증일시';
            COMMENT ON COLUMN SHOP_VAN.VAN_CERT_EDT    IS '최종인증일시';
            COMMENT ON COLUMN SHOP_VAN.VAN_CERT_CNT    IS '인증횟수';
            COMMENT ON COLUMN SHOP_VAN.SEND_FLAG       IS '서버송신구분';
            COMMENT ON COLUMN SHOP_VAN.SEND_DT         IS '서버송신일시';
            COMMENT ON COLUMN SHOP_VAN.POS_VER         IS '모바일 버전체크시 사용';
            COMMENT ON COLUMN SHOP_VAN.WORK_INDEX      IS 'Work Index Key';
            COMMENT ON COLUMN SHOP_VAN.WORK_KEY        IS 'Work Key';
            */

        }
            
        public SHOP_VAN(string shop_code, string pos_no)
        {
            this.SHOP_CODE = shop_code;
            this.POS_NO    = pos_no;
        }

        public string SHOP_CODE              { get; set; } = string.Empty;      //IS '매장코드';
        public string POS_NO                 { get; set; } = string.Empty;      //IS '포스번호';
        public string POS_REMARK             { get; set; } = string.Empty;      
        public string LOCAL_DB_YN            { get; set; } = string.Empty;      
        public string USE_YN                 { get; set; } = string.Empty;     
        public string CRD_VAN_SEQ            { get; set; } = string.Empty;      //IS '신용카드-승인일련번호';
        public string CASH_VAN_SEQ           { get; set; } = string.Empty;      //IS '현금영수증-승인일련번호';
        public string IP_ADDR                { get; set; } = string.Empty;       //IS '접속IP주소';
        public string INSERT_DT              { get; set; } = string.Empty;      //IS '최초등록일시';
        public string UPDATE_DT              { get; set; } = string.Empty;      //IS '최종수정일시';
        public string VAN_CODE               { get; set; } = string.Empty;      //IS '밴사코드';
        public string BIZ_NO                 { get; set; } = string.Empty;      //IS '사업자번호';
        public string VAN_TERM_NO            { get; set; } = string.Empty;      //IS '단말기번호';
        public string VAN_SER_NO             { get; set; } = string.Empty;      //IS '단말기시리얼번호';
        public string VAN_CERT_YN            { get; set; } = string.Empty;      //IS '인증여부';
        public string VAN_CERT_SDT           { get; set; } = string.Empty;      //IS '최초인증일시';
        public string VAN_CERT_EDT           { get; set; } = string.Empty;      //IS '최종인증일시';
        public string VAN_CERT_CNT           { get; set; } = string.Empty;      //IS '인증횟수';
        public string SEND_FLAG              { get; set; } = string.Empty;      //IS '서버송신구분';
        public string SEND_DT                { get; set; } = string.Empty;      //IS '서버송신일시';
        public string POS_VER                { get; set; } = string.Empty;      //IS '모바일 버전체크시 사용';
        public string W_KEY                  { get; set; } = string.Empty;
        public string WORK_INDEX             { get; set; } = string.Empty;      //IS 'Work Index Key';
        public string WORK_KEY               { get; set; } = string.Empty;      //IS 'Work Key';


        // 추가

        public string CORNER_CODE { get; set; } = string.Empty;      //사업장 코드
        public string CORNER_NAME { get; set; } = string.Empty;      //사업장명

        public string NO { get; set; } = string.Empty;
        public string OWNER_NAME { get; set; } = string.Empty; // 대표자명
        public string VAN_SECU_YN { get; set; } = string.Empty; // 장비상태


    }
}

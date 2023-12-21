using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoPOS.Models
{
    public class SHOP_ACCOUNT
    {
        /// <summary>
        /// 매장-시재입출금계정
        /// </summary>
        public SHOP_ACCOUNT()
        {
            /*

COMMENT ON COLUMN SHOP_ACCOUNT.SHOP_CODE IS '매장코드';
COMMENT ON COLUMN SHOP_ACCOUNT.ACCNT_CODE IS '시재입출금-계정코드';
COMMENT ON COLUMN SHOP_ACCOUNT.ACCNT_NAME IS '시재입출금-계정명';
COMMENT ON COLUMN SHOP_ACCOUNT.ACCNT_FLAG IS '시재계정구분 0:입금 1:출금';
COMMENT ON COLUMN SHOP_ACCOUNT.USE_YN IS '사용여부';
COMMENT ON COLUMN SHOP_ACCOUNT.INSERT_DT IS '최초등록일시';
COMMENT ON COLUMN SHOP_ACCOUNT.UPDATE_DT IS '최종수정일시';                        
COMMENT ON COLUMN POS_CASH_IO.SHOP_CODE IS '매장코드';
COMMENT ON COLUMN POS_CASH_IO.SALE_DATE IS '영업일자';
COMMENT ON COLUMN POS_CASH_IO.POS_NO IS '포스번호';
COMMENT ON COLUMN POS_CASH_IO.CSH_IO_SEQ IS '입출금일련번호';
COMMENT ON COLUMN POS_CASH_IO.REGI_SEQ IS '정산차수';
COMMENT ON COLUMN POS_CASH_IO.ACCNT_FLAG IS '입출금계정구분 0:입금 1:출금';
COMMENT ON COLUMN POS_CASH_IO.ACCNT_CODE IS '입출금계정코드';
COMMENT ON COLUMN POS_CASH_IO.ACCNT_AMT IS '입출금계정금액';
COMMENT ON COLUMN POS_CASH_IO.REMARK IS '입출금비고';
COMMENT ON COLUMN POS_CASH_IO.USE_YN IS '사용구분 Y:사용 N:삭제';
COMMENT ON COLUMN POS_CASH_IO.INSERT_DT IS '등록일시';
COMMENT ON COLUMN POS_CASH_IO.EMP_NO IS '판매원코드';
COMMENT ON COLUMN POS_CASH_IO.SEND_FLAG IS '서버전송구분 0:미전송 1:전송 2:오류';
COMMENT ON COLUMN POS_CASH_IO.SEND_DT IS '서버전송일시';



            */
        }

        public SHOP_ACCOUNT(string shop_code, string accnt_code)
        {
            this.SHOP_CODE = shop_code;
            this.ACCNT_CODE = accnt_code;
        }

        public string SHOP_CODE { get; set; } = string.Empty;//매장코드';
        public string ACCNT_CODE { get; set; } = string.Empty;//시재입출금-계정코드';
        public string ACCNT_NAME { get; set; } = string.Empty;//시재입출금-계정명';
        public string ACCNT_FLAG { get; set; } = string.Empty;//시재계정구분 0:입금 1:출금';
        public string USE_YN { get; set; } = string.Empty;//사용여부';
        public string INSERT_DT { get; set; } = string.Empty;//최초등록일시';
        public string UPDATE_DT { get; set; } = string.Empty;//최종수정일시';


        public string SALE_DATE { get; set; } = string.Empty;//영업일자';
        public string POS_NO { get; set; } = string.Empty;//포스번호';
        public string CSH_IO_SEQ { get; set; } = string.Empty;//입출금일련번호';
        public string REGI_SEQ { get; set; } = string.Empty;//정산차수';
        public string ACCNT_AMT { get; set; } = string.Empty;//입출금계정금액';
        public string REMARK { get; set; } = string.Empty;//입출금비고';
        public string EMP_NO { get; set; } = string.Empty;//판매원코드';
        public string EMP_NAME { get; set; } = string.Empty;//판매원명';

        public string SEND_FLAG { get; set; } = string.Empty;//서버전송구분 0:미전송 1:전송 2:오류';
        public string SEND_DT { get; set; } = string.Empty;//서버전송일시';

        public string NO { get; set; } = string.Empty;//순번';

        public string ACCNT_AMT_IN { get; set; } = string.Empty;//입금계정금액';
        public string ACCNT_AMT_OUT { get; set; } = string.Empty;//출금계정금액';
    }
}


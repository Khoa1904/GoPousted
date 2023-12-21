using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoPOS.Models
{
    public class SHOP_KITCHEN_MEMO
    {
        public SHOP_KITCHEN_MEMO()
        {
        }

        public SHOP_KITCHEN_MEMO(string shop_code, string memo_code)
        {
            this.SHOP_CODE = shop_code;
            this.MEMO_CODE = memo_code;
        }
        /*
        
        COMMENT ON TABLE SHOP_KITCHEN_MEMO IS
        '매장-주방메모';


        
         */
        public string SHOP_CODE { get; set; } = string.Empty; //'매장코드';
        public string MEMO_CODE { get; set; } = string.Empty; //'주방메모코드';
        public string MEMO_NAME { get; set; } = string.Empty; //'주방메모명칭';
        public string USE_YN { get; set; } = string.Empty; //'사용유무';
        public string INSERT_DT { get; set; } = string.Empty; //'최초입력일시';
        public string UPDATE_DT { get; set; } = string.Empty; //'최종수정일시';

    }
}

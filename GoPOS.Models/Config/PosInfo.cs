using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace GoPOS.Models.Config
{
    public class PosInfo
    {
        public string HD_SHOP_CODE { get; set; }
        public string StoreNo { get; set; }             //점포코드
        public string PosNo { get; set; }               //POS 번호
        public string Version { get; set; }             //버전정보
        public string LangCode { get; set; }             //Language
    }
}

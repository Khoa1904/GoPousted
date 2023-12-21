using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace GoPOS.Models.Custom.API
{
    public class NonTransModel
    {
        public string SHOP_CODE { get; set; }
        public string SALE_DATE { get; set; }
        public string POS_NO { get; set; }
        public string SALE_NO { get; set; }

        [JsonPropertyName("NTranPreChargeHeader")]
        public NTRN_PRECHARGE_HEADER NTranPreChargeHeader { get; set; }

        [JsonPropertyName("NTranPreChargeCard")]
        public NTRN_PRECHARGE_CARD[] NTranPreChargeCard { get; set; }
    }
}

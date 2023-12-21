using GoShared.Interface;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace GoPOS.Models
{
    public class SHOP_M
    {
        public SHOP_M()
        {
        }

        public SHOP_M(string shop_code, string shop_name)
        {
            this.SHOP_CODE = shop_code;
            this.SHOP_NAME = shop_name;
        }
        [JsonPropertyName("storeCode")]
        public string SHOP_CODE { get; set; } = string.Empty;
        [JsonPropertyName("StoreNm")]
        public string SHOP_NAME { get; set; } = string.Empty;
        [JsonPropertyName("fchqCode")]
        public string HD_SHOP_CODE { get; set; } = string.Empty;
        [JsonPropertyName("groupCode")]
        public string SHOP_GROUP_CODE { get; set; } = string.Empty;
        [JsonPropertyName("formCode")]
        public string SHOP_TYPE_FLAG { get; set; } = string.Empty;
        [JsonPropertyName("storeCtgCode")]
        public string SHOP_CLASS_FLAG { get; set; } = string.Empty;
        [JsonPropertyName("useCode")]
        public string PGM_TYPE_FLAG { get; set; } = string.Empty;
        [JsonPropertyName("repNm")]
        public string OWNER_NAME { get; set; } = string.Empty;
        [JsonPropertyName("storeCorpno")]
        public string BIZ_NO { get; set; } = string.Empty;
        [JsonPropertyName("storeTypeNm")]
        public string BIZ_TYPE_NAME { get; set; } = string.Empty;
        [JsonPropertyName("storeItemNm")]
        public string BIZ_KIND_NAME { get; set; } = string.Empty;
        [JsonPropertyName("comNm")]
        public string BIZ_SHOP_NAME { get; set; } = string.Empty;
        [JsonPropertyName("storeTelno")]
        public string TEL_NO { get; set; } = string.Empty;
        [JsonPropertyName("storeCelno")]
        public string HP_NO { get; set; } = string.Empty;
        [JsonPropertyName("storeFaxno")]
        public string FAX_NO { get; set; } = string.Empty;
        [JsonPropertyName("storeEmail")]
        public string EMAIL_ADDR { get; set; } = string.Empty;
        [JsonPropertyName("storePostno")]
        public string POST_NO { get; set; } = string.Empty;
        [JsonPropertyName("roadnmAdres")]
        public string ADDR { get; set; } = string.Empty;
        [JsonPropertyName("detailAdres")]
        public string ADDR_DTL { get; set; } = string.Empty;
        [JsonPropertyName("sttCode")]
        public string SHOP_STAT_FLAG { get; set; } = string.Empty;
        [JsonPropertyName("storeOpenDt")]
        public string SHOP_OPEN_DATE { get; set; } = string.Empty;
        [JsonPropertyName("posOpenDt")]
        public string SYS_OPEN_DATE { get; set; } = string.Empty;
        [JsonPropertyName("posCloseDt")]
        public string SYS_CLOSE_DATE { get; set; } = string.Empty;
        [JsonPropertyName("goodsCode")]
        public string CLASS_MGR_LEVEL_FLAG { get; set; } = string.Empty;
        [JsonPropertyName("wareAt")]
        public string WARE_YN { get; set; } = string.Empty;
        [JsonPropertyName("brandAt")]
        public string BRAND_MGR_YN { get; set; } = string.Empty;
        [JsonPropertyName("marginMgmtAt")]
        public string SALEMG_MGR_YN { get; set; } = string.Empty;
        [JsonPropertyName("createdAt")]
        public string INSERT_DT { get; set; } = string.Empty;
        [JsonPropertyName("updatedAt")]
        public string UPDATE_DT { get; set; } = string.Empty;
        [JsonPropertyName("mgmtVanCode")]
        public string VAN_CORP_CODE { get; set; } = string.Empty;
        [JsonPropertyName("locaslPosAt")]
        public string LOCAL_POS_YN { get; set; } = string.Empty;
        [JsonPropertyName("agentNm")]
        public string CORP_NAME { get; set; } = string.Empty;
        [JsonPropertyName("agentTelno")]
        public string CORP_TEL_NO { get; set; } = string.Empty;
        [JsonPropertyName("swAgreeAt")]
        public string AGREE_YN { get; set; } = string.Empty;
        [JsonPropertyName("swAgreeDt")]
        public string AGREE_DT { get; set; } = string.Empty;
        [JsonPropertyName("swAgreeSendAt")]
        public string AGREE_SEND_FLAG { get; set; } = string.Empty;
    }
}

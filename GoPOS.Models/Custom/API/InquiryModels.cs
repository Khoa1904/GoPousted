using GoShared.Helpers;
using GoShared.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace GoPOS.Models.Custom.API
{
    public class APIAccessToken
    {
        [JsonPropertyName("accessTkn")]
        public string ACCESS_TOKEN { get; set; }

        [JsonPropertyName("expiryDt")]
        public string EXPIRY_DATE { get; set; }
    }
    public class ServerTime : IIdentifyEntity
    {
        /// <summary>
        /// "20230615120000"
        /// </summary>
        [JsonPropertyName("sysDt")]
        public string? SYS_DT { get; set; }

        public string Base_PrimaryName()
        {
            throw new NotImplementedException();
        }

        public void EntityDefValue(EEditType eEdit, string userID, object data)
        {
            
        }

        public string Resource()
        {
            return "/client/inquiry/time";
        }
    }

    public class MasterTBChangedData : IIdentifyEntity
    {
        [JsonPropertyName("fchqCode")]
        public string CHANGE_CODE { get; set; }
        [JsonPropertyName("storeCode")]
        public string SHOP_CODE { get; set; }
        [JsonPropertyName("posNo")]
        public string POS_NO { get; set; }        
        [JsonPropertyName("chgMst")]
        public MasterTBVersion[] CHANGE_MST { get; set; }

        public string Base_PrimaryName()
        {
            throw new NotImplementedException();
        }

        public void EntityDefValue(EEditType eEdit, string userID, object data)
        {
            throw new NotImplementedException();
        }

        public string Resource()
        {
            return "/client/inquiry/master-check";
        }
    }

    public class MasterTBVerList
    {
        [JsonPropertyName("mstVerList")]
        public LocalMasterTBVersion[] MstVerList { get; set; }
    }
    
    public class MasterTBVersion 
    {
        /// <summary>
        /// 
        /// </summary>
        [JsonPropertyName("mstId")]
        public string MST_ID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonPropertyName("svrLastVer")]
        public string POS_VER { get; set; }

    }

    public class LocalMasterTBVersion
    {
        /// <summary>
        /// 
        /// </summary>
        [JsonPropertyName("mstId")]
        public string MST_ID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonPropertyName("posVer")]
        public string POS_VER { get; set; }

        public override string ToString()
        {
            return MST_ID + " " + POS_VER;
        }
    }
}

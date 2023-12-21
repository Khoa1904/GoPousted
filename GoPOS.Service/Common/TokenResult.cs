using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace GoPOS.Service.Common
{
    public class TokenResult
    {
        [JsonPropertyName("accessTkn")]
        public string AccessToken { get; set; } = string.Empty;
        [JsonPropertyName("expiryDt")]
        public string ExpiryDt { get; set; } = string.Empty;
        public DateTime ExpiredDate
        {
            get
            {
                return string.IsNullOrEmpty(ExpiryDt) ?
                        DateTime.Now : DateTime.ParseExact(ExpiryDt, "yyyyMMddHHmmss", Thread.CurrentThread.CurrentCulture);
            }

        }
        public DateTime LastLogin { get; set; } = DateTime.MinValue;

        public void Set(TokenResult result)
        {
            AccessToken = result.AccessToken;
            ExpiryDt = result.ExpiryDt;
            LastLogin = DateTime.Now;
        }
    }
}

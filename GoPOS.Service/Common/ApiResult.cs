using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoPOS.Service.Common
{
    public class ApiResult
    {
        public string ResultCode { get; set; } = string.Empty;
        public string ResultMsg { get; set; } = string.Empty;
        public dynamic ResultMap { get; set; } = string.Empty;
        public string GET_TOKEN { get; set; }
        public string status { get; set; }
        public object results { get; set; }

        public ApiResult() {
            ResultCode = string.Empty;
            ResultMsg = string.Empty;
            ResultMap = string.Empty;
            status = string.Empty;
        }
       
    }
}


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoPOS.Service.Common
{
    public class ApiResponse
    {
        public string results { get; set; }
        public string status { get; set; }
        public errorInfo error { get; set; }
        public ApiResponse() { }

    }
    public class errorInfo
    {
        public string code { get; set; }
    }
}

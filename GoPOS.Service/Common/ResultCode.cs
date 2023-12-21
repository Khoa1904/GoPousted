using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoPOS.Service.Common
{
    public struct ResultCode
    {
        //**----------------------------------------------------------------

        public const string VanSuccess = "00";
        public const string UserCancelled = "01";


        //**----------------------------------------------------------------
        public const string Success = "200";
        public const string Fail = "9999";
        //**----------------------------------------------------------------

        public const string Ok = "OK";
        public const string ERROR = "ERROR";
        //**----------------------------------------------------------------
    }

}

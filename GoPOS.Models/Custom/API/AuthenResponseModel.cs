using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoPOS.Models.Custom.API
{

    public class AuhthenResponseModelResult
    {
        public AuhthenResponseModel results { get; set; }
        public string status { get; set; }
    }

    public class AuhthenResponseModel
    {
        public string LicenseId { get; set; }
        public string LicenseKey { get; set; }
    }



}

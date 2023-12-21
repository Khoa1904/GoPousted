using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Formats.Asn1.AsnWriter;

namespace GoPOS.Models.Custom.API
{
    public class Store
    {
        public string posNo { get; set; }
        public string storeNm { get; set; }
        public string localPosAt { get; set; }
        public string fchqCode { get; set; }
        public string storeCode { get; set; }
        public string posGb { get; set; }
    }

    public class Results
    {
        public string token { get; set; }
        public List<Store> stores { get; set; }
        public string authId { get; set; }
    }

    public class ResponseModel
    {
        public Results? results { get; set; }
        public string status { get; set; }
    }

    public class AuthenRequestHeader
    {
        public string Token { get; set; }
        public string AuthId { get; set; }
    }

    public class AuthenResponse
    {
        public string LicenseId { get; set; }
        public string LicenseKey { get; set; }
    }
}

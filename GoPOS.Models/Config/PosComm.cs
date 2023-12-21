using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace GoPOS.Models.Config
{
    public class PosComm
    {
        private string tRPort = "4000";

        public string SvrURLServer { get; set; }
        public string AspURLServer { get; set; } = "https://gpdev.outlier.kr";
        /// <summary>
        /// For SubPOS
        /// </summary>
        public string MainPOSIP { get; set; }

        /// <summary>
        /// 메인포스 TR LISTENING PORT
        /// <br/>
        /// </summary>
        public string TRPort { get => string.IsNullOrEmpty(tRPort) ? "4000" : tRPort; set => tRPort = value; }        

        public string LicenseId { get; set; }
        public string LicenseKey { get; set; }
    }

}

using Dapper;
using GoPOS.Models;
using GoPOS.Models.Custom.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoPOS.Service.Service.MST
{
    public interface IWebInquiryService
    {
        /*
         * ~/client/auth/store
            ~/client/auth/lisence
            ~/client/auth/auto-login
            ~/client/auth/token


            URL
            ~/client/inquiry/time
            ~/client/inquiry/status
            ~/client/inquiry/master-check
            ~/client/inquiry/member
            ~/client/inquiry/member
            ~/client/inquiry/point/acml
            ~/client/inquiry/point/use
            ~/client/inquiry/point/list
            ~/client/inquiry/point/credit
            */


        Task<(string, SpResult)> InqAccessToken();
        Task<(ServerTime, SpResult)> InqServerTime();
        Task<(POS_STATUS, SpResult)> InqPOSStatus();
        Task<(MasterTBChangedData, SpResult)> InqMasterTableVersions();
        Task<SpResult> InqTruncateTableAsync();
    }
}

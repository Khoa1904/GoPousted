using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Dapper;
using FirebirdSql.Data.FirebirdClient;

using GoShared.Helpers;
using GoPOS.Models;
using GoPOS.Service.Common;
using GoPOS.Models.Common;
using Google.Protobuf.WellKnownTypes;

namespace GoPOS.Services
{
    public class InfoEmpService : BaseDataService<MST_INFO_EMP>, IInfoEmpService
    {
        public InfoEmpService() : base() 
        {
        
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="empNo"></param>
        /// <returns></returns>
        public MST_INFO_EMP GetEmpInfo(string empNo)
        {
            return TryGet(p => p.SHOP_CODE == DataLocals.AppConfig.PosInfo.StoreNo &&                        
                        p.EMP_NO == empNo);
        }

        public MST_INFO_EMP GetEmpInfo(string userId, string password)
        {
            return TryGet(p => p.SHOP_CODE == DataLocals.AppConfig.PosInfo.StoreNo &&                        
                        p.USER_ID == userId && p.EMP_PWD == password);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Dapper;
using FirebirdSql.Data.FirebirdClient;
using GoShared.Helpers;
using GoPOS.Models;
using GoPOS.Service.Common;
using GoPOS.Models.Common;
using GoPOS.Models.Config;

namespace GoPOS.Services
{
    public class InfoShopService : BaseDataService<MST_INFO_SHOP>, IInfoShopService
    {
        public MST_INFO_SHOP GetShopInfo(string shopCode)
        {
            return TryGet(p =>p.SHOP_CODE == shopCode && p.HD_SHOP_CODE == DataLocals.AppConfig.PosInfo.HD_SHOP_CODE);
        }

        public MST_INFO_SHOP GetShopInfoCurrent()
        {
            return TryGet(p => p.SHOP_CODE == DataLocals.AppConfig.PosInfo.StoreNo && p.HD_SHOP_CODE == DataLocals.AppConfig.PosInfo.HD_SHOP_CODE);
        }
    }
}

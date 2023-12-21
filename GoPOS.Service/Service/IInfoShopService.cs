using GoPOS.Models;
using GoShared.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoPOS.Services
{
    public interface IInfoShopService : IDataService<MST_INFO_SHOP>
    {
        public MST_INFO_SHOP GetShopInfoCurrent();
        public MST_INFO_SHOP GetShopInfo(string shopCode);
    }
}

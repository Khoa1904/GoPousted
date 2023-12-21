using GoPOS.Models;
using GoShared.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoPOS.Service.Service
{
    public interface ISettAccountService : IDataService<SETT_POSACCOUNT>
    {
        Task<bool> Update(SETT_POSACCOUNT value);
        Task<SETT_POSACCOUNT> GetSingleAsync(POS_STATUS pOS_STATUS);
        Task<SETT_POSACCOUNT> GetSingleAsync(POS_STATUS pOS_STATUS, bool createIfNnull);
        SETT_POSACCOUNT GetLastMiddleSett();
    }
}

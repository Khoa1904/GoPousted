using GoPOS.Models;
using GoPOS.Service.Common;
using GoShared.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoPOS.Service.Service
{
    public class SettAccountService : BaseDataService<SETT_POSACCOUNT>, ISettAccountService
    {
        public POS_POST_MANG AddFromSettAccount(DataContext db, SETT_POSACCOUNT setPOSAcc)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public SETT_POSACCOUNT GetLastMiddleSett()
        {
            return null;
        }

        public Task<SETT_POSACCOUNT> GetSingleAsync(POS_STATUS pOS_STATUS)
        {
            return GetSingleAsync(pOS_STATUS, false);
        }

        public Task<SETT_POSACCOUNT> GetSingleAsync(POS_STATUS pOS_STATUS, bool createdIfNull)
        {
            using (var context = new DataContext())
            {
                var res = context.sETT_POSACCOUNTs.FirstOrDefault(p => p.SHOP_CODE == pOS_STATUS.SHOP_CODE &&
                            p.POS_NO == pOS_STATUS.POS_NO && p.SALE_DATE == pOS_STATUS.SALE_DATE && p.REGI_SEQ == pOS_STATUS.REGI_SEQ);
                if (res == null && createdIfNull)
                {
                    res = pOS_STATUS.CopyFields<SETT_POSACCOUNT>();
                    res.OPEN_DT = pOS_STATUS.SALE_DATE + "000000";
                    res.CLOSE_DT = "1".Equals(pOS_STATUS.CLOSE_FLAG) ? string.Empty : pOS_STATUS.SALE_DATE + "235959";
                    res.SEND_FLAG = "N";
                    res.INSERT_DT = DateTime.Now.ToString("yyyyMMddHHmmss");
                    context.sETT_POSACCOUNTs.Add(res);
                    context.SaveChanges();
                }

                return Task.FromResult(res);
            }
        }

        public Task<bool> Update(SETT_POSACCOUNT value)
        {
            var res = this.AddOrUpdate(value);
            return Task.FromResult(res.Success);
        }
    }
}

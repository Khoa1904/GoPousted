using GoPOS.Models;
using GoShared.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoPOS.Service.Service.POS
{
    public interface IPOSTMangService : IDataService<POS_POST_MANG>
    {
        Task<SpResult> AddPOSTMang(DataContext db, POS_POST_MANG pOS_POST_MANG);
        Task<(SpResult, POS_POST_MANG)> AddFromTrnHeader(DataContext db, TRN_HEADER trnHeader, bool isKDS);
        Task<(SpResult, POS_POST_MANG)> AddFromSettAccount(DataContext db, SETT_POSACCOUNT setPOSAcc);
        public Task<(SpResult, POS_POST_MANG)> AddPointRecord(DataContext db, TRN_POINTSAVE pointSave, SETT_POSACCOUNT settAcc);

        Task<(SpResult, POS_POST_MANG)> AddPOSMangNonTran(DataContext db,string billNo);

    }
}

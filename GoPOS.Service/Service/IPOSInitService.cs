using Dapper;
using GoPOS.Models;
using GoShared.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoPOS.Services
{
    public interface IPOSInitService : IDataService<POS_STATUS>
    {
        #region POS Init, Starting service functions

        /// <summary>
        /// 1: can, 2: db, 3: ini and pos status diff
        /// </summary>
        /// <returns></returns>
        int POSCanStart(out string errorMessage);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        int POSInitialize();
        int CheckPOSOpen(out POS_STATUS posStatus, out SETT_POSACCOUNT lastSettAcc);
        bool DoPOSOpen(POS_STATUS? posStatus, decimal posReadyAmt);

        #endregion

        #region API related

        void VerifyAPIToken();

        #endregion

        SETT_POSACCOUNT DoCloseSettAccount(SETT_POSACCOUNT lastSettAcc, bool createNewMiddle, bool dontSave);

        void DoMiddleSettAccount(SETT_POSACCOUNT lastSettAcc);

        /// <summary>
        /// 마감취소
        /// </summary>
        /// <param name="closeSettAcc"></param>
        string DoCloseSettAccountCancel(SETT_POSACCOUNT closeSettAcc, bool cancelOpen);

        MST_CNFG_DETAIL GetConfigValue(bool isShopValue, string setCode);
    }
}

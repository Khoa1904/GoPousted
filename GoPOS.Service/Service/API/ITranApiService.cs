using GoPOS.Models;
using GoPOS.Models.Custom.API;
using GoPOS.Service.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoPOS.Service.Service.API
{
    public interface ITranApiService
    {
        Task<bool> UpdateTranPOSTMangStatus(POS_POST_MANG pOS_POST_MANG, ApiResponse res);
        

        /// <summary>
        /// GET TRDatga in dagaset
        /// Convert to json
        /// ready to send to 
        ///     - MainPOS
        ///     - Web
        /// </summary>
        /// <param name="shopCode"></param>
        /// <param name="posNo"></param>
        /// <param name="saleDate"></param>
        /// <param name="billNo"></param>
        /// <returns></returns>
        TranData GetTRData(string shopCode, string posNo, string saleDate, string regiSeq, string billNo);

        NonTransModel GetNonTRData(string shopCode, string posNo, string saleDate, string regiSeq, string billNo);


        /// <summary>
        /// 
        /// </summary>
        /// <param name="postMang"></param>
        /// <returns></returns>
        TranAccount GetTranAccount(POS_POST_MANG postMang);

        TranData GetPointsData(string shopCode, string posNo, string saleDate, string regiSeq, string billNo);
        /// <summary>
        /// Receive data from
        ///     - SubPOS
        /// </summary>
        /// <param name="orgJsonData"></param>
        /// <param name="billFlag"></param>
        /// <returns></returns>
        SpResult SaveTRDataByJson(string orgJsonData);

        SpResult SaveNTRDataByJson(string orgJsonData);


        /// <summary>
        /// POS_SETTACCOUNT table
        /// </summary>
        /// <param name="orgJsonData"></param>
        /// <returns></returns>
        SpResult SaveSettAccountByJson(string orgJsonData);

        IList<POS_POST_MANG> PickTranDataToSend(int count, int errorTimes);
        IList<POS_POST_MANG> PickNonTranDataToSend(int count, int errorTimes);
        IList<POS_POST_MANG> PickSettAccDataToSend(int count, int errorTimes);
        IList<POS_POST_MANG> PickPointsToSend(int count, int errorTimes);

    }
}

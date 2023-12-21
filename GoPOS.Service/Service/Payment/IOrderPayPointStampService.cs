using GoPOS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace GoPOS.Payment.Services
{
    public interface IOrderPayPointStampService
    {
        Task<(MEMBER_CLASH, object[])> RequestSavePoint(string billNo, MEMBER_CLASH memberInfo, TRN_HEADER trnHeader, TRN_TENDERSEQ[] payTenders, TRN_PRDT[] trnPrdts);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="billNo"></param>
        /// <param name="memberInfo"></param>
        /// <param name="tRN_POINTUSE"></param>
        /// <returns></returns>
        Task<(MEMBER_CLASH, object[])> RequestUsePointStamp(string billNo, MEMBER_CLASH memberInfo, TRN_POINTUSE trnPointUse, decimal usePointStampAmt, decimal useStampQty);
        Task<(List<MEMBER_GRADE>, string)> GetMEMBER_GRADEs();
        Task<(string, MEMBER_CLASH)> GetMemberDetail(string posNo, string saleDate, string memberCode);
        Task<(string, List<MEMBER_CLASH>)> SearchMembers(string posNo, string saleDate, string cellNo, string cardNo, string memberNm);
    }
}

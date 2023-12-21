using GoPOS.Models;
using GoPOS.Service;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoPOS.Sales.Services
{
    public interface ISalesMngService
    {
        SalesMiddleExcClcModel GetMiddleSettData(string saleDate, string regiSeq);
        /// <summary>
        /// 비매출 - 선결제 Header TRAN
        /// </summary>
        Tuple<string, bool> SaveNonTrnPrecharge(MEMBER_CLASH mEMBER_CLASH, NTRN_PRECHARGE_CARD payCard, bool isCancelled, out string errorMessage);
        //Task<(SpResult, string)> DoRefundRePayment(string shopCode, string posNo, string saleDate, string billNo);
        void UpdateOrderPayReturnTR(string shopCode, string posNo, string saleDate, string regiSeq, string billNo,
            string refReturnBillNo);
        Task<(SpResult, POS_POST_MANG)> SaveNonTrnPrechargeAndHeader(NTRN_PRECHARGE_HEADER nTrnHeader, NTRN_PRECHARGE_CARD[] nTrnCard);
        NTRN_PRECHARGE_CARD GetApprNo(string salebillno);

        NTRN_PRECHARGE_CARD GetOrgPrechargeCard(string shopCode, string posNo, string saleDate, string saleNo);
    }
}

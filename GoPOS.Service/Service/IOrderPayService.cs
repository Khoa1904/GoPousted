using Dapper;
using GoPOS.Models;
using GoPOS.Models.Custom.API;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GoPOS.Services;

/*
    
 */
public interface IOrderPayService
{
    ////3.버튼 리스트
    Task<(List<ORDER_FUNC_KEY>, SpResult)> GetOrderFuncKey(string fkFlag);

    Task<List<MST_TUCH_CLASS>> GetTouchClasses();

    Task<List<ORDER_TU_PRODUCT>> GetTouchProducts(string tuClassCode);

    /// <summary>
    /// SIDE MENU CLASS AND SUB,
    /// INCLUDING Property and Selection
    /// </summary>
    /// <param name="prdCode"></param>
    /// <returns></returns>
    Task<List<ORDER_SIDE_CLS_MENU>> GetSideCLSMenus(string prdCode);

    /// <summary>
    /// Get all discount list
    /// </summary>
    /// <returns></returns>
    Task<List<MST_INFO_FIX_DC>> GetFixDcList();

    Task<(SpResult, POS_POST_MANG)> SaveOrderPayTR(bool saveHold, TRN_HEADER trHeader, TRN_PRDT[] trProducts, TRN_TENDERSEQ[] trTenders,
        TRN_CASH[] trCashs, TRN_CASHREC[] trCashRecs, TRN_CARD[] trCards, TRN_PARTCARD[] trPartCards, TRN_GIFT[] trGifts, TRN_FOODCPN[] trnMeals,
        TRN_EASYPAY[] trEasyPays, TRN_POINTUSE[] trPointUse, TRN_POINTSAVE trPointSave, MEMBER_CLASH memberInfo, TRN_PPCARD[] trPpCards, string? tableCd);

    /// <summary>
    /// Update ORG_BILL NO for original TR
    /// </summary>
    /// <param name="shopCode"></param>
    /// <param name="posNo"></param>
    /// <param name="saleDate"></param>
    /// <param name="regiSeq"></param>
    /// <param name="billNo"></param>
    /// <param name="refReturnBillNo"></param>
    /// <returns></returns>
    void UpdateOrderPayReturnTR(string shopCode, string posNo, string saleDate, string regiSeq, string billNo, string refReturnBillNo);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="shopCode"></param>
    /// <param name="posNo"></param>
    /// <param name="saleDate"></param>
    /// <param name="regiSeq"></param>
    /// <param name="billNo"></param>
    /// <returns></returns>
    Task<(SpResult, string)> DoReturnReceipt(string shopCode, string posNo, string saleDate, string regiSeq, string billNo);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="tranData"></param>
    /// <returns></returns>
    Task<(SpResult, string)> DoReturnReceipt(TranData tranData);
    bool ReplayOverlappedOrder(string tableCD);
}


using Dapper;
using GoPOS.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace GoPOS.Services;

/*
 2023-02-06 김형석 생성
 */
public interface ISalesGiftSaleService
{
    #region ISalesGiftSaleView

    /// <summary>
    /// 상품 리스트1
    /// </summary>
    /// <param name="param"></param>
    /// <returns></returns>
    Task<(List<SALES_GIFT_SALE>, SpResult)> GetList1(DynamicParameters param);

    Task<(List<SALES_GIFT_SALE>, SpResult)> GetList2(DynamicParameters param);

    Task<(List<SALES_GIFT_SALE>, SpResult)> GetList3(DynamicParameters param, int iIndex);
    #endregion
    Task<(List<SALES_GIFT_SALE2>, SpResult)> GetMiddleGiftCard1(string? SaleYN, string ResiSeq, /*string ClozeFlak,*/ string saleDT);
    Task<(List<SALES_GIFT_SALE2>, SpResult)> GetMiddleGiftCard2(string? SaleYN, string ResiSeq, /*string ClozeFlak,*/ string saleDT);
    Task<(List<SALES_APPR>, SpResult)> GetMiddleCashAppr(string? SaleYN, string ResiSeq, string /*ClozeFlak, string */saleDT);
    Task<(List<SALES_APPR>, SpResult)> GetMiddleCardAppr(string? SaleYN, string ResiSeq, string /*ClozeFlak, string*/ saleDT);
    Task<(List<FINAL_SETT>, SpResult)> GetFinalClosing(string ResiSeq,/* string ClozeFlak,*/ string saleDT);
    Task<(List<CR_CARD_SALE>, SpResult)> GetCardDistinct(string ResiSeq, /*string ClozeFlak,*/ string saleDT);
    Task<(List<PRODUCT_SALE>, SpResult)> ProductSaleOfDay(string type);
    Task<(List<SALE_BY_TYPE2>, SpResult)> PmtTypeSaleOfDay(DynamicParameters param);
    Task<(List<SALE_BY_TYPE2>, SpResult)> PmttypeStatusOfDay();
    Task<(List<TRN_HEADER>, SpResult)> GetTodayTransaction();
}
using Dapper;
using GoPOS.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GoPOS.Services;

/*
 2023-02-06 김형석 생성
 */
public interface ISellingStatusGoodsOrderCanclSttusService
{
    #region ISellingStatusGoodsOrderCanclSttusView

    /// <summary>
    /// 상품 리스트
    /// </summary>
    /// <param name="param"></param>
    /// <returns></returns>
    Task<(List<GOODSORDERCANCLSTTUS>, SpResult)> GetList1(DynamicParameters param);

    /// <summary>
    /// 상품 리스트
    /// </summary>
    /// <param name="param"></param>
    /// <returns></returns>
    Task<(List<GOODSORDERCANCLSTTUS>, SpResult)> GetList2(DynamicParameters param);


    /// <summary>
    /// 상품 리스트
    /// </summary>
    /// <param name="param"></param>
    /// <returns></returns>
    Task<(List<GOODSORDERCANCLSTTUS>, SpResult)> GetList3(DynamicParameters param);


    #endregion
}


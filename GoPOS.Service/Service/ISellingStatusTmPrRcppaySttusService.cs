using Dapper;
using GoPOS.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GoPOS.Services;

/*
 2023-02-06 김형석 생성
 */
public interface ISellingStatusTmPrRcppaySttusService
{
    #region SellingStatusTmPrRcppaySttusView

    /// <summary>
    ///상품 리스트1
    /// </summary>
    /// <param name="param"></param>
    /// <returns></returns>
    Task<(List<TMPRRCPPAYSTTUS>, SpResult)> GetList1(DynamicParameters param);

    /// <summary>
    ///상품 리스트1
    /// </summary>
    /// <param name="param"></param>
    /// <returns></returns>
    Task<(List<TMPRRCPPAYSTTUS>, SpResult)> GetList2(DynamicParameters param);

    /// <summary>
    ///상품 리스트1
    /// </summary>
    /// <param name="param"></param>
    /// <returns></returns>
    Task<(List<TMPRRCPPAYSTTUS>, SpResult)> GetList3(DynamicParameters param);

    #endregion
}


using Dapper;
using GoPOS.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GoPOS.Services;

/*
 2023-02-06 김형석 생성
 */
public interface ISellingStatusTableGroupSelngSttusService
{
    #region ISellingStatusTableGroupSelngSttusView

    /// <summary>
    /// 상품 리스트
    /// </summary>
    /// <param name="param"></param>
    /// <returns></returns>
    Task<(List<TABLEGROUPSELNGSTTUS>, SpResult)> GetList1(DynamicParameters param);

    /// <summary>
    /// 상품 리스트
    /// </summary>
    /// <param name="param"></param>
    /// <returns></returns>
    Task<(List<TABLEGROUPSELNGSTTUS>, SpResult)> GetList2(DynamicParameters param);

    /// <summary>
    /// 상품 리스트
    /// </summary>
    /// <param name="param"></param>
    /// <returns></returns>
    Task<(List<TABLEGROUPSELNGSTTUS>, SpResult)> GetList3(DynamicParameters param);

    #endregion
}


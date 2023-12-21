using Dapper;
using GoPOS.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GoPOS.Services;

/*
 2023-01-30 박현재 생성
 */
public interface ICommonService
{
    #region OrderPayDlvrProcess
    //공통 MST_COMM_CODE_SHOP
    Task<(List<MST_COMM_CODE_SHOP>, SpResult)> GetCommonList(DynamicParameters param);

    //주방메모 SHOP_KITCHEN_MEMO
    Task<(List<SHOP_KITCHEN_MEMO>, SpResult)> GetKitchenMemoList(DynamicParameters param);

    //직원 정보 가져오기
    Task<(List<MST_INFO_EMP>, SpResult)> GetEmpList(DynamicParameters param);
    #endregion



}


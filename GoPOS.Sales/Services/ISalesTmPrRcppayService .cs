using Dapper;
using GoPOS.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GoPOS.Services;

/*
 영업관리 - 시재입출금 2023-03-27 박현재 생성
 */
public interface ISalesTmPrRcppayService
{
    ////시재 동적 구성
    Task<(List<SHOP_ACCOUNT>, SpResult)> GetAccountInfo(DynamicParameters param);
    ////시재입출금 현황 리스트
    Task<(List<SHOP_ACCOUNT>, SpResult)> GetInOutAccountList(DynamicParameters param);
    ////시재입출금 등록
    Task<(SHOP_ACCOUNT, SpResult)> InsertInOutAccount(DynamicParameters param);
    ////시재입출금 삭제
    Task<(SHOP_ACCOUNT, SpResult)> DeleteInOutAccount(DynamicParameters param);

}


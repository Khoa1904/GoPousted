using Dapper;
using GoPOS.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace GoPOS.Services;

/*
 영업관리 - 준비금등록
 */

public interface ISalesResvMneyRegistService
{
    ////준비금 가져오기
    Task<POS_SETTLEMENT_DETAIL> GetResvMney();

    ////준비금 등록
    Task<(POS_SETTLEMENT_DETAIL, SpResult)> InsertResvMney(DynamicParameters param);

    Task<SpResult> SaveSalesResvMneyRegist(string saleDate, string seqReg, decimal readyAmt);


}
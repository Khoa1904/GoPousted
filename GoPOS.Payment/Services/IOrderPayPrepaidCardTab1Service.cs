using Dapper;
using GoPOS.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace GoPOS.Services;

/*
 2023-03-12 김형석 생성
 */
public interface IOrderPayPrepaidCardTab1Service
{
    #region OrderPayPrepaidCardTab1View

    /// <summary>
    /// 복합결제 리스트
    /// </summary>
    /// <param name="param"></param>
    /// <returns></returns>
    Task<(List<POS_ORD_M>, SpResult)> GetList(DynamicParameters param);

    #endregion
}


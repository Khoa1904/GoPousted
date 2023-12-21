using Dapper;
using GoPOS.Models;
using GoPOS.Models._0_Common;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace GoPOS.Services;

/*
 2023-03-19 생성
 */
public interface IConfigPosConfigSetupService
{
    #region IConfigPosConfigSetupService

    /// <summary>
    /// 환경설정 리스트1 MAIN
    /// </summary>
    /// <param name="param"></param>
    /// <returns></returns>
    Task<(List<ENV_CONFIG>, SpResult)> GetList1(DynamicParameters param);

    /// <summary>
    /// 환경설정 리스트2 SUB
    /// </summary>
    /// <param name="param"></param>
    /// <returns></returns>
    Task<(List<ENV_CONFIG>, SpResult)> GetList2(DynamicParameters param);


    /// <summary>
    /// UPDATE SHOP 환경정보
    /// </summary>
    /// <param name="param"></param>
    /// <returns></returns>
    Task<(SHOP_POS_ENV, SpResult)> SP_SCD_ENVPS_U(DynamicParameters param);

    #endregion

}
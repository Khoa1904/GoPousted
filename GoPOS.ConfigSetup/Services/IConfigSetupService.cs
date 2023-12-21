using Dapper;
using GoPOS.Models;
using GoPOS.Models._0_Common;
using GoPOS.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace GoPOS.Services;

/*
 2023-03-19 생성 통합
 */
public interface IConfigSetupService
{
    #region IConfigSetupService

    /// <summary>
    /// ConfigMstrRecptnSetupView 메인 리스트1
    /// </summary>
    /// <param name="param"></param>
    /// <returns></returns>
    Task<(List<CMM_MASTER_RECV>, SpResult)> ConfigMstrRecptnSetup_GetList1(DynamicParameters param);

    /// <summary>
    /// ConfigMstrRecptnSetupView 저장 버튼
    /// </summary>
    /// <param name="param"></param>
    /// <returns></returns>
    Task<(CMM_MASTER_RECV, SpResult)> SP_CMM_MASTER_RECV_U(DynamicParameters param);

    


    /// <summary>
    /// ConfigTrmnlCrtfcViewModel 단말기인증         메인 리스트1
    /// </summary>
    /// <param name="param"></param>
    /// <returns></returns>
    Task<(List<SHOP_VAN>, SpResult)> ConfigTrmnlCrtfc_GetList1(DynamicParameters param);

    

    /// <summary>
    /// 매출자료 수신 ConfigSellingDataRecptn 메인 리스트1
    /// </summary>
    /// <param name="param"></param>
    /// <returns></returns>
    Task<(List<POS_ORD_M>, SpResult)> ConfigSellingDataRecptn_GetList1(DynamicParameters param);

    /// <summary>
    /// 매출자료 수신 ConfigSellingDataRecptn 서브 리스트2
    /// </summary>
    /// <param name="param"></param>
    /// <returns></returns>
    Task<(List<POS_ORD_M>, SpResult)> ConfigSellingDataRecptn_GetList2(DynamicParameters param);


    /// </summary>
    /// 포스데이터관리 ConfigPosDataMng 메인 리스트1
    /// <param name="param"></param>
    /// <returns></returns>
    Task<(List<POS_ORD_M>, SpResult)> ConfigPosDataMng_GetList1(DynamicParameters param);


    /// </summary>
    /// 보안리더기 무결성 점검 ConfigScrtyRdrIntgrtyChck 메인 리스트1
    /// <param name="param"></param>
    /// <returns></returns>
    Task<(List<POS_LGSRC_T>, SpResult)> ConfigScrtyRdrIntgrtyChck_GetList1(DynamicParameters param);


    /// </summary>
    /// 환경설정	매출자료전송 (송신) ConfigSellingDataTrnsmis
    /// <param name="param"></param>
    /// <returns></returns>
    Task<(List<CONFIG_SETUP_COM>, SpResult)> ConfigSellingDataTrnsmis_GetList1(DynamicParameters param);

    #endregion

}
using Dapper;
using FirebirdSql.Data.FirebirdClient;
using GoPOS.Database;
using GoPOS.Helpers;
using GoPOS.Models;
using GoPOS.Models._0_Common;
using GoShared.Helpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using static System.Data.Entity.Infrastructure.Design.Executor;

namespace GoPOS.Services;

/// <summary>
/// 작성자: 
/// 화면명: POS 환경설정
/// </summary>

public class ConfigPosConfigSetupService : IConfigPosConfigSetupService
{
    #region ConfigPosConfigSetupService

    public async Task<(List<ENV_CONFIG>, SpResult)> GetList1(DynamicParameters param)
    {
        try
        {
            //코너명 총수량 총매출 총할인 실매출 부가세 합계 점유율
            //CORNER_CODE TOT_SALE_QTY    TOT_SALE_AMT DC_AMT  DCM_SALE_AMT VAT_AMT TOT_AMT OCC_RATE

            string SQL = @"SELECT MS.ENV_GROUP_CODE      , MS.ENV_GROUP_NAME      , MS.ENV_SET_CODE      , MS.ENV_SET_VALUE      , MS.ENV_SET_NAME       , IIF( DT.ENV_VALUE_NAME <> '', DT.ENV_VALUE_NAME , MS.ENV_SET_VALUE ) ENV_VALUE_NAME
                           FROM (
                           SELECT HD.ENV_SET_CODE             ,
                           HD.ENV_SET_NAME             ,
                           SH.ENV_SET_VALUE            ,
                           HD.ENV_GROUP_CODE             ,
                           HD.ENV_GROUP_NAME        
                           FROM ( SELECT HD.ENV_SET_CODE                    , HD.ENV_SET_NAME                    , HD.ENV_GROUP_CODE                    , CD.COM_CODE_NAME ENV_GROUP_NAME               
                           FROM MST_INFO_POS MS
                           , ENV_CONFIG HD
                           , ( SELECT COM_CODE , COM_CODE_NAME FROM CMM_CODE WHERE COM_CODE_FLAG = '117' ) CD
                           WHERE MS.POS_NO     = '01'
                           AND   MS.USE_YN     = 'Y'              
                           AND   HD.ENV_SET_FLAG = '3'              
                           AND   HD.USE_YN     = 'Y'              
                           AND   CD.COM_CODE     = HD.ENV_GROUP_CODE             ) HD LEFT OUTER JOIN SHOP_POS_ENV SH
                           ON  SH.SHOP_CODE    = 'V41712'    AND SH.POS_NO     = '01'
                           AND SH.ENV_SET_CODE = HD.ENV_SET_CODE      ) MS      LEFT OUTER JOIN ( SELECT ENV_SET_CODE                             , ENV_VALUE_CODE                             , ENV_VALUE_NAME
                           FROM ENV_CONFIG_VALUE ) DT     
                           ON  DT.ENV_SET_CODE = MS.ENV_SET_CODE     
                           AND DT.ENV_VALUE_CODE = MS.ENV_SET_VALUE ORDER BY MS.ENV_GROUP_CODE        , MS.ENV_SET_CODE
                           ";
            List<ENV_CONFIG> result = await DapperORM.ReturnListAsync<ENV_CONFIG>(SQL, param);

            return DapperORM.ReturnResult(result, EResultType.SUCCESS, "OK");

        }
        catch (FbException ex)
        {
            LogHelper.Logger.Error($"Database Error : {ex.Message.ReplacePlainText()}");
            return DapperORM.ReturnResult(new List<ENV_CONFIG>(), EResultType.ERROR, ex.Message.ReplacePlainText());
        }
        catch (Exception ex)
        {
            LogHelper.Logger.Error($"Services Error : {ex.Message.ReplacePlainText()}");
            return DapperORM.ReturnResult(new List<ENV_CONFIG>(), EResultType.ERROR, ex.Message.ReplacePlainText());
        }
    }

    public async Task<(List<ENV_CONFIG>, SpResult)> GetList2(DynamicParameters param)
    {
        try
        {
            string SQL = @"SELECT
                           A.ENV_SET_NAME
                          ,B.ENV_VALUE_NAME
                          ,B.ENV_VALUE_CODE
                          ,A.ENV_SET_CODE
                          ,A.ENV_GROUP_CODE
                          ,A.ENV_SET_FLAG
                          
                          FROM ENV_CONFIG A
                          LEFT JOIN ENV_CONFIG_VALUE B
                          ON A.ENV_SET_CODE = B.ENV_SET_CODE
                          WHERE 1 = 1
                          AND   A.ENV_SET_CODE = @ENV_SET_CODE--'240'
                          --AND   A.ENV_SET_NAME = @ENV_SET_NAME  --'VAN-승인방식'
                          AND A.USE_YN = 'Y'
                          AND B.USE_YN = 'Y'
                          AND A.ENV_SET_FLAG = '3'
                          ORDER BY ENV_GROUP_CODE
                          ";
            List<ENV_CONFIG> result = await DapperORM.ReturnListAsync<ENV_CONFIG>(SQL, param);

            return DapperORM.ReturnResult(result, EResultType.SUCCESS, "OK");
        }
        catch (FbException ex)
        {
            LogHelper.Logger.Error($"Database Error : {ex.Message.ReplacePlainText()}");
            return DapperORM.ReturnResult(new List<ENV_CONFIG>(), EResultType.ERROR, ex.Message.ReplacePlainText());
        }
        catch (Exception ex)
        {
            LogHelper.Logger.Error($"Services Error : {ex.Message.ReplacePlainText()}");
            return DapperORM.ReturnResult(new List<ENV_CONFIG>(), EResultType.ERROR, ex.Message.ReplacePlainText());
        }
    }

    public async Task<(SHOP_POS_ENV, SpResult)> SP_SCD_ENVPS_U(DynamicParameters param)
    {
        try
        {
            //SP_SCD_ENVPS_U 참고
            string SQL = @"UPDATE SHOP_POS_ENV
                           SET ENV_SET_VALUE  = @ENV_SET_VALUE
                             , MODIFY_FLAG    = '1'
                             , SEND_FLAG      = '0'
                             , UPDATE_DT      = SYSTEM_DATE('YYYYMMDDHHNNSS', 'NOW')
                           WHERE SHOP_CODE    = @SHOP_CODE
                           AND   POS_NO       = @POS_NO
                           AND   ENV_SET_CODE = @ENV_SET_CODE";

            int result = await DapperORM.ExecuteAsync(SQL, param);

            if (result > 0)
                LogHelper.Logger.Info("SHOP_POS_ENV Update Success");
            else
                LogHelper.Logger.Error("SHOP_POS_ENV Update Failed");

            return DapperORM.ReturnResult(new SHOP_POS_ENV(), EResultType.SUCCESS, "OK");

            //return DapperORM.ReturnResult(new List<ORDER_MENU>(), EResultType.SUCCESS, "OK");
        }
        catch (FbException ex)
        {
            LogHelper.Logger.Error($"Database Error : {ex.Message.ReplacePlainText()}");
            return DapperORM.ReturnResult(new SHOP_POS_ENV(), EResultType.ERROR, ex.Message.ReplacePlainText());
        }
        catch (Exception ex)
        {
            LogHelper.Logger.Error($"Services Error : {ex.Message.ReplacePlainText()}");
            return DapperORM.ReturnResult(new SHOP_POS_ENV(), EResultType.ERROR, ex.Message.ReplacePlainText());
        }

    }

    
    #endregion
}
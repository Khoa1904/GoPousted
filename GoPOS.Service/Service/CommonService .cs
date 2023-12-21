using Dapper;
using FirebirdSql.Data.FirebirdClient;
using GoPOS.Database;
using GoPOS.Models;
using GoShared.Helpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace GoPOS.Services;

public class CommonService : ICommonService
{
    //공통 리스트 가져오기
    public async Task<(List<MST_COMM_CODE_SHOP>, SpResult)> GetCommonList(DynamicParameters param)
    {
        try
        {

            string SQL = @"     
             SELECT *
               FROM MST_COMM_CODE_SHOP
              WHERE SHOP_CODE = @SHOP_CODE
                AND COM_CODE_FLAG = @COM_CODE_FLAG
                AND USE_YN = 'Y'
              ORDER BY COM_CODE
            ";

            List<MST_COMM_CODE_SHOP> result = await DapperORM.ReturnListAsync<MST_COMM_CODE_SHOP>(SQL, param); // stored procedure

            if (result == null)
            {
                LogHelper.Logger.Warn($"Database : 조회(DB) 결과가 없습니다.");
                return DapperORM.ReturnResult(new List<MST_COMM_CODE_SHOP>(), EResultType.EMPTY, "조회(DB) 결과가 없습니다.");
            }

            return DapperORM.ReturnResult(result, EResultType.SUCCESS, "OK");
        }
        catch (FbException ex)
        {
            LogHelper.Logger.Error($"Database Error : {ex.Message.ReplacePlainText()}");
            return DapperORM.ReturnResult(new List<MST_COMM_CODE_SHOP>(), EResultType.ERROR, ex.Message.ReplacePlainText());
        }
        catch (Exception ex)
        {
            LogHelper.Logger.Error($"Services Error : {ex.Message.ReplacePlainText()}");
            return DapperORM.ReturnResult(new List<MST_COMM_CODE_SHOP>(), EResultType.ERROR, ex.Message.ReplacePlainText());
        }
    }


    //주방메모 리스트 가져오기
    public async Task<(List<SHOP_KITCHEN_MEMO>, SpResult)> GetKitchenMemoList(DynamicParameters param)
    {
        try
        {

            string SQL = @"  
                  SELECT *
                    FROM SHOP_KITCHEN_MEMO
                   WHERE SHOP_CODE = @SHOP_CODE
                     AND USE_YN = 'Y'
                   ORDER BY MEMO_CODE
            ";

            List<SHOP_KITCHEN_MEMO> result = await DapperORM.ReturnListAsync<SHOP_KITCHEN_MEMO>(SQL, param); // stored procedure

            if (result == null)
            {
                LogHelper.Logger.Warn($"Database : 조회(DB) 결과가 없습니다.");
                return DapperORM.ReturnResult(new List<SHOP_KITCHEN_MEMO>(), EResultType.EMPTY, "조회(DB) 결과가 없습니다.");
            }

            return DapperORM.ReturnResult(result, EResultType.SUCCESS, "OK");
        }
        catch (FbException ex)
        {
            LogHelper.Logger.Error($"Database Error : {ex.Message.ReplacePlainText()}");
            return DapperORM.ReturnResult(new List<SHOP_KITCHEN_MEMO>(), EResultType.ERROR, ex.Message.ReplacePlainText());
        }
        catch (Exception ex)
        {
            LogHelper.Logger.Error($"Services Error : {ex.Message.ReplacePlainText()}");
            return DapperORM.ReturnResult(new List<SHOP_KITCHEN_MEMO>(), EResultType.ERROR, ex.Message.ReplacePlainText());
        }
    }

    //직원 정보 가져오기
    public async Task<(List<MST_INFO_EMP>, SpResult)> GetEmpList(DynamicParameters param)
    {
        try
        {
            string SQL = @"                           
            SELECT *
              FROM MST_INFO_EMP
             WHERE SHOP_CODE = @SHOP_CODE
             ORDER BY EMP_NO
            ";
            //EMP_FLAG 사원구분 ( CMM_CODE : 008 ) 0:점주 1:판매원 2:주문 3:배달
            List<MST_INFO_EMP> result = await DapperORM.ReturnListAsync<MST_INFO_EMP>(SQL, param); // stored procedure

            if (result == null)
            {
                LogHelper.Logger.Warn($"Database : 조회(DB) 결과가 없습니다.");
                return DapperORM.ReturnResult(new List<MST_INFO_EMP>(), EResultType.EMPTY, "조회(DB) 결과가 없습니다.");
            }

            return DapperORM.ReturnResult(result, EResultType.SUCCESS, "OK");
        }
        catch (FbException ex)
        {
            LogHelper.Logger.Error($"Database Error : {ex.Message.ReplacePlainText()}");
            return DapperORM.ReturnResult(new List<MST_INFO_EMP>(), EResultType.ERROR, ex.Message.ReplacePlainText());
        }
        catch (Exception ex)
        {
            LogHelper.Logger.Error($"Services Error : {ex.Message.ReplacePlainText()}");
            return DapperORM.ReturnResult(new List<MST_INFO_EMP>(), EResultType.ERROR, ex.Message.ReplacePlainText());
        }
    }
}
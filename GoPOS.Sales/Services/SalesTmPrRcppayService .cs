using Dapper;
using FirebirdSql.Data.FirebirdClient;
using GoPOS.Database;
using GoPOS.Helpers;
using GoPOS.Models;
using GoShared.Helpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace GoPOS.Services;

/*
 영업관리 - 시재입출금 2023-03-27 박현재 생성
 */

public class SalesTmPrRcppayService : ISalesTmPrRcppayService
{

    ////시재 동적 구성
    public async Task<(List<SHOP_ACCOUNT> , SpResult)> GetAccountInfo(DynamicParameters param)
    {
        try
        {
            string SQL = @"

                  SELECT SHOP_CODE
                       , ACCNT_CODE
                       , ACCNT_NAME
                       , ACCNT_FLAG
                       , USE_YN
                       , INSERT_DT
                       , UPDATE_DT
                    FROM SHOP_ACCOUNT
                   WHERE SHOP_CODE = @SHOP_CODE
                     AND ACCNT_FLAG = @ACCNT_FLAG
            ";

            List<SHOP_ACCOUNT> result = await DapperORM.ReturnListAsync<SHOP_ACCOUNT>(SQL, param); // stored procedure

            if (result == null)
            {
                LogHelper.Logger.Warn($"Database : 조회(DB) 결과가 없습니다.");
                return DapperORM.ReturnResult(new List<SHOP_ACCOUNT>(), EResultType.EMPTY, "조회(DB) 결과가 없습니다.");
            }

            return DapperORM.ReturnResult(result, EResultType.SUCCESS, "OK");
        }
        catch (FbException ex)
        {
            LogHelper.Logger.Error($"Database Error : {ex.Message.ReplacePlainText()}");
            return DapperORM.ReturnResult(new List<SHOP_ACCOUNT>(), EResultType.ERROR, ex.Message.ReplacePlainText());
        }
        catch (Exception ex)
        {
            LogHelper.Logger.Error($"Services Error : {ex.Message.ReplacePlainText()}");
            return DapperORM.ReturnResult(new List<SHOP_ACCOUNT>(), EResultType.ERROR, ex.Message.ReplacePlainText());
        }
    }

    ////시재입출금 현황 리스트
    public async Task<(List<SHOP_ACCOUNT>, SpResult)> GetInOutAccountList(DynamicParameters param)
    {
        try
        {
            string SQL = @"
                 
                   SELECT PCI.SHOP_CODE
                        , PCI.SALE_DATE
                        , PCI.POS_NO
                        , PCI.CSH_IO_SEQ
                        , PCI.REGI_SEQ
                        , PCI.ACCNT_FLAG
                        , PCI.ACCNT_CODE
                        , SA.ACCNT_NAME
                        , PCI.ACCNT_AMT
                        , PCI.REMARK
                        , PCI.USE_YN
                        , PCI.INSERT_DT
                        , PCI.EMP_NO    
                        , EM.EMP_NAME
                        , PCI.SEND_FLAG
                        , PCI.SEND_DT
                     FROM POS_CASH_IO PCI LEFT OUTER JOIN SHOP_ACCOUNT SA
                                                       ON PCI.SHOP_CODE = SA.SHOP_CODE
                                                      AND PCI.ACCNT_CODE = SA.ACCNT_CODE
                                          LEFT OUTER JOIN MST_INFO_EMP EM      
                                                       ON PCI.EMP_NO = EM.EMP_NO
                    WHERE PCI.SHOP_CODE = @SHOP_CODE
                      AND PCI.SALE_DATE = @SALE_DATE
                      AND PCI.POS_NO = @POS_NO
                      AND PCI.REGI_SEQ = @REGI_SEQ
                    ORDER BY PCI.SALE_DATE
            ";

            List<SHOP_ACCOUNT> result = await DapperORM.ReturnListAsync<SHOP_ACCOUNT>(SQL, param); // stored procedure

            if (result == null)
            {
                LogHelper.Logger.Warn($"Database : 조회(DB) 결과가 없습니다.");
                return DapperORM.ReturnResult(new List<SHOP_ACCOUNT>(), EResultType.EMPTY, "조회(DB) 결과가 없습니다.");
            }

            return DapperORM.ReturnResult(result, EResultType.SUCCESS, "OK");
        }
        catch (FbException ex)
        {
            LogHelper.Logger.Error($"Database Error : {ex.Message.ReplacePlainText()}");
            return DapperORM.ReturnResult(new List<SHOP_ACCOUNT>(), EResultType.ERROR, ex.Message.ReplacePlainText());
        }
        catch (Exception ex)
        {
            LogHelper.Logger.Error($"Services Error : {ex.Message.ReplacePlainText()}");
            return DapperORM.ReturnResult(new List<SHOP_ACCOUNT>(), EResultType.ERROR, ex.Message.ReplacePlainText());
        }
    }

    ////시재입출금 등록
    public async Task<(SHOP_ACCOUNT, SpResult)> InsertInOutAccount(DynamicParameters param)
    {
        try
        {

            int result = await DapperORM.ExecuteAsync("SP_POS_CASH_IO_AUD", param);

            if (result > -1)
                LogHelper.Logger.Info("InsertInOutAccount Success");
            else
                LogHelper.Logger.Error("InsertInOutAccount Failed");

            return DapperORM.ReturnResult(new SHOP_ACCOUNT(), EResultType.SUCCESS, "OK");

        }
        catch (FbException ex)
        {
            LogHelper.Logger.Error($"Database Error : {ex.Message.ReplacePlainText()}");
            return DapperORM.ReturnResult(new SHOP_ACCOUNT(), EResultType.ERROR, ex.Message.ReplacePlainText());
        }
        catch (Exception ex)
        {
            LogHelper.Logger.Error($"Services Error : {ex.Message.ReplacePlainText()}");
            return DapperORM.ReturnResult(new SHOP_ACCOUNT(), EResultType.ERROR, ex.Message.ReplacePlainText());
        }
    }

    public async Task<(SHOP_ACCOUNT, SpResult)> DeleteInOutAccount(DynamicParameters param)
    {
        try
        {

            int result = await DapperORM.ExecuteAsync("SP_POS_CASH_IO_D", param);

            if (result > -1)
                LogHelper.Logger.Info("DeleteInOutAccount Success");
            else
                LogHelper.Logger.Error("DeleteInOutAccount Failed");

            return DapperORM.ReturnResult(new SHOP_ACCOUNT(), EResultType.SUCCESS, "OK");

            //SHOP_ACCOUNT result = await DapperORM.ExecuteAsync<SHOP_ACCOUNT>("SP_POS_CASH_IO_D", param);

            //if (result == null)
            //{
            //    LogHelper.Logger.Error("DeleteInOutAccount Failed");
            //    return DapperORM.ReturnResult(new SHOP_ACCOUNT(), EResultType.ERROR, "DeleteInOutAccount Failed");
            //}
            //else

            //    return DapperORM.ReturnResult(result, EResultType.SUCCESS, "OK");

        }
        catch (FbException ex)
        {
            LogHelper.Logger.Error($"Database Error : {ex.Message.ReplacePlainText()}");
            return DapperORM.ReturnResult(new SHOP_ACCOUNT(), EResultType.ERROR, ex.Message.ReplacePlainText());
        }
        catch (Exception ex)
        {
            LogHelper.Logger.Error($"Services Error : {ex.Message.ReplacePlainText()}");
            return DapperORM.ReturnResult(new SHOP_ACCOUNT(), EResultType.ERROR, ex.Message.ReplacePlainText());
        }
    }


}

/*

    INSERT INTO POS_CASH_IO (SHOP_CODE , SALE_DATE , POS_NO , CSH_IO_SEQ , REGI_SEQ , ACCNT_FLAG , ACCNT_CODE , ACCNT_AMT , REMARK , USE_YN , INSERT_DT , EMP_NO , SEND_FLAG , SEND_DT)
                        VALUES (@SHOP_CODE, @SALE_DATE, @POS_NO, @CSH_IO_SEQ, @REGI_SEQ, @ACCNT_FLAG, @ACCNT_CODE, @ACCNT_AMT, @REMARK, @USE_YN, @INSERT_DT, @EMP_NO, @SEND_FLAG, @SEND_DT)
 */
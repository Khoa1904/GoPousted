using Dapper;
using FirebirdSql.Data.FirebirdClient;
using Google.Protobuf.Collections;
using GoPOS.Common.Helpers;
using GoPOS.Database;
using GoPOS.Helpers;
using GoPOS.Models;
using GoPOS.Models.Common;
using GoPOS.Service;
using GoShared.Helpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Migrations;
using System.Data.Entity.Validation;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using static System.Data.Entity.Infrastructure.Design.Executor;

namespace GoPOS.Services;

/*
 영업관리 - 준비금등록
 */

public class SalesResvMneyRegistService : ISalesResvMneyRegistService
{

    ////준비금 가져오기
    public async Task<POS_SETTLEMENT_DETAIL> GetResvMney()
    {
        try
        {
            string SQL = ResourceHelpers.LoadSqlCommand(SQLFileTypes.Sales, "GetSettPosAccount");
            List<POS_SETTLEMENT_DETAIL> result = await DapperORM.ReturnListAsync<POS_SETTLEMENT_DETAIL>(SQL,
            new string[]
            {
                            "@SHOP_CODE",
                            "@POS_NO",
                            "@SALE_DATE",
                            "@REGI_SEQ"
            },
            new object[]
            {
                            DataLocals.AppConfig.PosInfo.StoreNo,
                            DataLocals.AppConfig.PosInfo.PosNo,
                            DataLocals.PosStatus.SALE_DATE,
                            DataLocals.PosStatus.REGI_SEQ

            });

            return result.FirstOrDefault();
        }
        catch (FbException ex)
        {
            LogHelper.Logger.Error($"Database Error : {ex.Message.ReplacePlainText()}");
            return new POS_SETTLEMENT_DETAIL();
        }
        catch (Exception ex)
        {
            LogHelper.Logger.Error($"Services Error : {ex.Message.ReplacePlainText()}");
            return new POS_SETTLEMENT_DETAIL();
        }
    }

    ////준비금 등록
    public async Task<(POS_SETTLEMENT_DETAIL, SpResult)> InsertResvMney(DynamicParameters param)
    {
        try
        {
            int result = await DapperORM.ExecuteAsync("SP_READY_AMOUNT_U", param);

            if (result > -1)
                LogHelper.Logger.Info("InsertResvMney Success");
            else
                LogHelper.Logger.Error("InsertResvMney Failed");

            return DapperORM.ReturnResult(new POS_SETTLEMENT_DETAIL(), EResultType.SUCCESS, "OK");

        }
        catch (FbException ex)
        {
            LogHelper.Logger.Error($"Database Error : {ex.Message.ReplacePlainText()}");
            return DapperORM.ReturnResult(new POS_SETTLEMENT_DETAIL(), EResultType.ERROR, ex.Message.ReplacePlainText());
        }
        catch (Exception ex)
        {
            LogHelper.Logger.Error($"Services Error : {ex.Message.ReplacePlainText()}");
            return DapperORM.ReturnResult(new POS_SETTLEMENT_DETAIL(), EResultType.ERROR, ex.Message.ReplacePlainText());
        }
    }

    public async Task<SpResult> SaveSalesResvMneyRegist(string saleDate, string seqReg, decimal readyAmt)
    {
        var result = new SpResult()
        {
            ResultType = EResultType.SUCCESS,
            ResultCode = "0000"
        };

        using (var db = new DataContext())
        {
            try
            {
                /// 
                /// Update 정산 테이블
                /// 
                var posAcct = db.sETT_POSACCOUNTs.FirstOrDefault(p => p.SHOP_CODE ==
                        DataLocals.AppConfig.PosInfo.StoreNo && p.POS_NO == DataLocals.AppConfig.PosInfo.PosNo &&
                        p.SALE_DATE == saleDate && p.REGI_SEQ == seqReg);
                posAcct.POS_READY_AMT = readyAmt;
                db.sETT_POSACCOUNTs.AddOrUpdate(posAcct);
                db.SaveChanges();
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    LogHelper.Logger.Error("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        LogHelper.Logger.Error("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                result.ResultType = EResultType.ERROR;
                result.ResultCode = "9999";
                result.ResultMessage = e.ToFormattedString();
            }
            catch (Exception ex)
            {
                result.ResultType = EResultType.ERROR;
                result.ResultCode = "9999";
                result.ResultMessage = ex.ToFormattedString();
                LogHelper.Logger.Error(ex);
            }
        }
        return await Task.FromResult(result);
    }
}
using Dapper;
using FirebirdSql.Data.FirebirdClient;
using GoPOS.Common.Helpers;
using GoPOS.Database;
using GoPOS.Helpers;
using GoPOS.Models;
using GoPOS.Models.Common;
using GoPOS.Models.Custom.OrderMng;
using GoPOS.Service;
using GoShared.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.Entity.Migrations;
using System.Data.Entity.Validation;
using System.Linq;
using System.Threading.Tasks;
using static System.Data.Entity.Infrastructure.Design.Executor;

namespace GoPOS.Services;

/// <summary>
/// 작성자: 김형석 
/// 화면명: 품절 확장
/// </summary>

public class OrderPaySoldOutService : IOrderPaySoldOutService
{
    public async Task<List<SOLD_OUT_ITEM>> GetOrderPaySoldOutProductsAsync(string sTOCK_OUT_YN, string proCode, string proName)
    {
        try
        {
            string SQL = ResourceHelpers.LoadSqlCommand(SQLFileTypes.OrderPay, "GetsoldOutProductList");

            var result = await DapperORM.ReturnListAsync<SOLD_OUT_ITEM>(SQL,
            new string[]
            {
                            "@SHOP_CODE",
                            "@PRD_CODE",
                            "@PRD_NAME",
                            "@STOCK_OUT_YN"

            },
            new object[]
            {
                            DataLocals.AppConfig.PosInfo.StoreNo,
                            "%" + proCode + "%",
                            "%" + proName + "%",
                            sTOCK_OUT_YN
            });

            return result;

        }
        catch (FbException ex)
        {
            LogHelper.Logger.Error($"Database Error : {ex.Message.ReplacePlainText()}");
            return new List<SOLD_OUT_ITEM>();
        }
        catch (Exception ex)
        {
            LogHelper.Logger.Error($"Services Error : {ex.Message.ReplacePlainText()}");
            return new List<SOLD_OUT_ITEM>();
        }
    }

    public Task<SpResult> SaveProductStock(ObservableCollection<SOLD_OUT_ITEM> mST_INFO_PRODUCTs)
    {
        var result = new SpResult()
        {
            ResultType = EResultType.SUCCESS,
            ResultCode = "0000"
        };

        using (var db = new DataContext())
        {
            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    //foreach (SOLD_OUT_ITEM row in mST_INFO_PRODUCTs)
                    //{
                    //    db.mST_INFO_PRODUCTs.AddOrUpdate(row);
                    //}

                    foreach (SOLD_OUT_ITEM row in mST_INFO_PRODUCTs)
                    {
                        var item = db.mST_INFO_PRODUCTs.FirstOrDefault(p => p.SHOP_CODE == row.SHOP_CODE &&
                                p.PRD_CODE == row.PRD_CODE);
                        if (item != null)
                        {
                            item.STOCK_OUT_YN = row.STOCK_OUT_YN;
                        }
                    }


                    db.SaveChanges();
                    trans.Commit();
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
                    trans.Rollback();
                    result.ResultType = EResultType.ERROR;
                    result.ResultCode = "9999";
                    result.ResultMessage = e.ToFormattedString();
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    result.ResultType = EResultType.ERROR;
                    result.ResultCode = "9999";
                    result.ResultMessage = ex.ToFormattedString();
                    LogHelper.Logger.Error(ex);
                }
            }
        }

        return Task.FromResult(result);
    }
}
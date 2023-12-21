using Dapper;
using FirebirdSql.Data.FirebirdClient;
using GoPOS.Common.Helpers;
using GoPOS.Database;
using GoPOS.Helpers;
using GoPOS.Models;
using GoPOS.Models.Common;
using GoShared.Helpers;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using static System.Data.Entity.Infrastructure.Design.Executor;

namespace GoPOS.Services;

/// <summary>
/// 작성자: 김형석 
/// 화면명: 쿠폰 결제 서비스
/// </summary>

public class OrderPayCouponService : IOrderPayCouponService
{
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public async Task<List<MST_INFO_COUPON>> GetCouponList()
    {
        try
        {
            string SQL = ResourceHelpers.LoadSqlCommand(SQLFileTypes.OrderPay, "GetCouponList");
            List<MST_INFO_COUPON> result = await DapperORM.ReturnListAsync<MST_INFO_COUPON>(SQL,
            new string[]
            {
                            "@SHOP_CODE"
            },
            new object[]
            {
                            DataLocals.AppConfig.PosInfo.StoreNo,
            });

            return result;

        }
        catch (FbException ex)
        {
            LogHelper.Logger.Error($"Database Error : {ex.Message.ReplacePlainText()}");
            return new List<MST_INFO_COUPON>();
        }
        catch (Exception ex)
        {
            LogHelper.Logger.Error($"Services Error : {ex.Message.ReplacePlainText()}");
            return new List<MST_INFO_COUPON>();
        }
    }


    public async Task<List<MST_INFO_PRD_COUPON>> GetCouponDetailList(int couponCode)
    {
        try
        {
            string SQL = ResourceHelpers.LoadSqlCommand(SQLFileTypes.OrderPay, "GetCouponDetailList");
            List<MST_INFO_PRD_COUPON> result = await DapperORM.ReturnListAsync<MST_INFO_PRD_COUPON>(SQL,
            new string[]
            {
                            "@SHOP_CODE",
                            "TK_CPN_CODE",
            },
            new object[]
            {
                            DataLocals.AppConfig.PosInfo.StoreNo,
                            couponCode
            });

            return result;

        }
        catch (FbException ex)
        {
            LogHelper.Logger.Error($"Database Error : {ex.Message.ReplacePlainText()}");
            return new List<MST_INFO_PRD_COUPON>();
        }
        catch (Exception ex)
        {
            LogHelper.Logger.Error($"Services Error : {ex.Message.ReplacePlainText()}");
            return new List<MST_INFO_PRD_COUPON>();
        }
    }

    public async Task<List<MST_INFO_PRD_COUPON>> GetCouponPRDList(int cpnCode)
    {
        try
        {
            string SQL = ResourceHelpers.LoadSqlCommand(SQLFileTypes.OrderPay, "GetCouponPRDList");
            List<MST_INFO_PRD_COUPON> result = await DapperORM.ReturnListAsync<MST_INFO_PRD_COUPON>(SQL,
            new string[]
            {
                            "@SHOP_CODE",
                            "@TK_CPN_CODE"
            },
            new object[]
            {
                            DataLocals.AppConfig.PosInfo.StoreNo,
                            cpnCode
            });

            return result;

        }
        catch (FbException ex)
        {
            LogHelper.Logger.Error($"Database Error : {ex.Message.ReplacePlainText()}");
            return new List<MST_INFO_PRD_COUPON>();
        }
        catch (Exception ex)
        {
            LogHelper.Logger.Error($"Services Error : {ex.Message.ReplacePlainText()}");
            return new List<MST_INFO_PRD_COUPON>();
        }
    }

    public async Task<List<MST_INFO_PRODUCT>> GetProductList(string cpnCode)
    {
        try
        {
            string SQL = ResourceHelpers.LoadSqlCommand(SQLFileTypes.OrderPay, "GetProductList");
            List<MST_INFO_PRODUCT> result = await DapperORM.ReturnListAsync<MST_INFO_PRODUCT>(SQL,
            new string[]
            {
                            "@SHOP_CODE",
                            "@TK_CPN_CODE"
            },
            new object[]
            {
                            DataLocals.AppConfig.PosInfo.StoreNo,
                            cpnCode
            });

            return result;

        }
        catch (FbException ex)
        {
            LogHelper.Logger.Error($"Database Error : {ex.Message.ReplacePlainText()}");
            return new List<MST_INFO_PRODUCT>();
        }
        catch (Exception ex)
        {
            LogHelper.Logger.Error($"Services Error : {ex.Message.ReplacePlainText()}");
            return new List<MST_INFO_PRODUCT>();
        }
    }
}
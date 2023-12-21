using Dapper;
using FirebirdSql.Data.FirebirdClient;
using GoPOS.Common.Helpers;
using GoPOS.Database;
using GoPOS.Helpers;
using GoPOS.Models;
using GoPOS.Models.Common;
using GoShared.Helpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using static System.Data.Entity.Infrastructure.Design.Executor;

namespace GoPOS.Services;

/// <summary>
/// 작성자: 김형석 
/// 화면명: 현금 결제
/// </summary>

public class OrderPayMealService : IOrderPayMealService
{
    public async Task<List<MST_INFO_TICKET>> GetMealTicketList()
    {
        try
        {
            string SQL = ResourceHelpers.LoadSqlCommand(SQLFileTypes.OrderPay, "GetMealTicketList");
            List<MST_INFO_TICKET> result = await DapperORM.ReturnListAsync<MST_INFO_TICKET>(SQL,
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
            return new List<MST_INFO_TICKET>();
        }
        catch (Exception ex)
        {
            LogHelper.Logger.Error($"Services Error : {ex.Message.ReplacePlainText()}");
            return new List<MST_INFO_TICKET>();
        }
    }
}
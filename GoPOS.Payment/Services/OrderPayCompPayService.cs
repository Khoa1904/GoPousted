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
/// 화면명: 복함 결제
/// </summary>

public class OrderPayCompPayService : IOrderPayCompPayService
{
    public async Task<List<MST_INFO_CARD>> GetCreditCardList()
    {
        try
        {

            string SQL = @"
                                SELECT * FROM MST_INFO_CARD
                           ";
            List<MST_INFO_CARD> result = await DapperORM.ReturnListAsync<MST_INFO_CARD>(SQL, null);

            return result;

        }
        catch (FbException ex)
        {
            LogHelper.Logger.Error($"Database Error : {ex.Message.ReplacePlainText()}");
            return new List<MST_INFO_CARD>();
        }
        catch (Exception ex)
        {
            LogHelper.Logger.Error($"Database Error : {ex.Message.ReplacePlainText()}");
            return new List<MST_INFO_CARD>();
        }
    }
}
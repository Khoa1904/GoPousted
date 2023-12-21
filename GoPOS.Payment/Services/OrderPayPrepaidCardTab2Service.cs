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
using static System.Data.Entity.Infrastructure.Design.Executor;

namespace GoPOS.Services;

/// <summary>
/// 작성자: 김형석 
/// 화면명: 선불카드 TAB2
/// </summary>

public class OrderPayPrepaidCardTab2Service : IOrderPayPrepaidCardTab2Service
{
    #region OrderPayPrepaidCardTab2View

    public async Task<(List<POS_ORD_M>, SpResult)> GetList(DynamicParameters param)
    {
        try
        {
            string SQL = @"";
            List<POS_ORD_M> result = await DapperORM.ReturnListAsync<POS_ORD_M>(SQL, param);

            return DapperORM.ReturnResult(result, EResultType.SUCCESS, "OK");

            //return DapperORM.ReturnResult(new List<POS_ORD_M >(), EResultType.SUCCESS, "OK");
        }
        catch (FbException ex)
        {
            LogHelper.Logger.Error($"Database Error : {ex.Message.ReplacePlainText()}");
            return DapperORM.ReturnResult(new List<POS_ORD_M>(), EResultType.ERROR, ex.Message.ReplacePlainText());
        }
        catch (Exception ex)
        {
            LogHelper.Logger.Error($"Services Error : {ex.Message.ReplacePlainText()}");
            return DapperORM.ReturnResult(new List<POS_ORD_M>(), EResultType.ERROR, ex.Message.ReplacePlainText());
        }
    }

    #endregion
}
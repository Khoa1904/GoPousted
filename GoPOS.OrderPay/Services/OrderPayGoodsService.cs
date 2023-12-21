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
using System.Reflection;
using System.Threading.Tasks;

namespace GoPOS.Services;

public class OrderPayGoodsService : IOrderPayGoodsService
{
    public async Task<(List<MST_INFO_PRODUCT>, SpResult)> GetProductList2(string Pcode, string Pname)
    {

        try
        {
            string SQL = ResourceHelpers.LoadSqlCommand(SQLFileTypes.OrderPay, "GetProductList2");
            List<MST_INFO_PRODUCT> result = await DapperORM.ReturnListAsync<MST_INFO_PRODUCT>(SQL,
                new string[]
                {
                    "@SHOP_CODE",
                    "@PRD_NAME",
                    "@PRD_CODE"
                },
                new object[]
                {
                    DataLocals.AppConfig.PosInfo.StoreNo,
                    Pname,
                    Pcode
                });

            if (!result.Any())
            {
                LogHelper.Logger.Warn($"Database : 주문오더 : 왼쪽영역 하단 버튼 셋팅(DB) 결과가 없습니다.");
                return DapperORM.ReturnResult(new List<MST_INFO_PRODUCT>(), EResultType.EMPTY, "조회(DB) 결과(List)가 없습니다.");
            }

            return DapperORM.ReturnResult(result, EResultType.SUCCESS, "OK");
        }
        catch (FbException ex)
        {
            LogHelper.Logger.Error($"Database Error : {ex.Message.ReplacePlainText()}");
            return DapperORM.ReturnResult(new List<MST_INFO_PRODUCT>(), EResultType.ERROR, ex.Message.ReplacePlainText());
        }
        catch (Exception ex)
        {
            LogHelper.Logger.Error($"Services Error : {ex.Message.ReplacePlainText()}");
            return DapperORM.ReturnResult(new List<MST_INFO_PRODUCT>(), EResultType.ERROR, ex.Message.ReplacePlainText());
        }
    }
}
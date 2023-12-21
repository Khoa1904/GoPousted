using Dapper;
using GoPOS.Common.Helpers;
using GoPOS.Database;
using GoPOS.Models;
using GoPOS.Models.Common;
using GoPOS.Service;
using GoShared.Helpers;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace GoPOS.Services;

public class OrderPayCoprtnDscntService : IOrderPayCoprtnDscntService
{
    public async Task<List<MST_INFO_JOINCARD>> GetInfoJoinCardTable()
    {
        using (var context = new DataContext())
        {            
            int tDate = DateTime.Today.ToString("yyyyMMdd").ToInt32();
            string sql = ResourceHelpers.LoadSqlCommand(SQLFileTypes.Payment, "OrderPayCoprtnDscnt_JoinCardsSelect");

            List<MST_INFO_JOINCARD> result = await DapperORM.ReturnListAsync<MST_INFO_JOINCARD>(sql,
                new string[]
                {
                    "@SHOP_CODE",
                    "@TODAY"
                },
                new object[]
                {
                    DataLocals.AppConfig.PosInfo.StoreNo,
                    tDate
                });

            return result;
        }
    }

    public async Task<List<MST_INFO_PRD_JOINCARD>> GetInfoPrdJoinCardTable(string joinCard)
    {
        using (var context = new DataContext())
        {
            string sql = ResourceHelpers.LoadSqlCommand(SQLFileTypes.Payment, "OrderPayCoprtnDscnt_JoinProducts");

            List<MST_INFO_PRD_JOINCARD> result = await DapperORM.ReturnListAsync<MST_INFO_PRD_JOINCARD>(sql,
                new string[]
                {
                    "@SHOP_CODE",
                    "@JCD_CODE"
                },
                new object[]
                {
                    DataLocals.AppConfig.PosInfo.StoreNo,
                    joinCard
                });

            return await Task.FromResult(result);
        }
    }
}

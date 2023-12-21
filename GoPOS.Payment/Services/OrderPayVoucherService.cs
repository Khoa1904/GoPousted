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

public class OrderPayVoucherService : IOrderPayVoucherService
{
    #region OrderPayCashView

    #endregion
    public async Task<List<MST_INFO_TICKET>> GetInfoTicket(string classCode)
    {
        try
        {
            string SQL = ResourceHelpers.LoadSqlCommand(SQLFileTypes.OrderPay, "GetVoucherTicketList");
            List<MST_INFO_TICKET> result = await DapperORM.ReturnListAsync<MST_INFO_TICKET>(SQL,
            new string[]
            {
                                        "@SHOP_CODE",
                                        "@TK_CLASS_CODE"
            },
            new object[]
            {                                      
                                        DataLocals.AppConfig.PosInfo.StoreNo,
                                        classCode
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

    public async Task<List<MST_INFO_TICKET_CLASS>> GetInfoTicketClass(DynamicParameters param)
    {
        try
        {
            string SQL = ResourceHelpers.LoadSqlCommand(SQLFileTypes.OrderPay, "GetAllVoucherTicketClassList");
            List<MST_INFO_TICKET_CLASS> result = await DapperORM.ReturnListAsync<MST_INFO_TICKET_CLASS>(SQL,
            new string[]
            {
                            "@TK_CLASS_FLAG",
                            "@SHOP_CODE",
                            "@USE_YN"
            },
            new object[]
            {
                            "G",
                            DataLocals.AppConfig.PosInfo.StoreNo,
                            "Y"
            });

            return result;
        }
        catch (FbException ex)
        {
            LogHelper.Logger.Error($"Database Error : {ex.Message.ReplacePlainText()}");
            return new List<MST_INFO_TICKET_CLASS>();
        }
        catch (Exception ex)
        {
            LogHelper.Logger.Error($"Services Error : {ex.Message.ReplacePlainText()}");
            return new List<MST_INFO_TICKET_CLASS>();
        }
    }
}
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
using System.Linq;
using System.Threading.Tasks;
using static System.Data.Entity.Infrastructure.Design.Executor;

namespace GoPOS.Services;

/// <summary>
/// 작성자: 김형석 
/// 화면명: 선불카드 TAB1
/// </summary>

public class OrderPayReceiptMngService : IOrderPayReceiptMngService
{ 

    public async Task<List<RECEIPT_MANAGER_ITEM>> GetTrnTenderSeqList(string saleDate, string posNo, string billNo)
    {
        try
        {
            string SQL = ResourceHelpers.LoadSqlCommand(SQLFileTypes.OrderPay, "GetTrnTenderSeqList");
            var result = await DapperORM.ReturnListAsync<RECEIPT_MANAGER_ITEM>(SQL,
            new string[]
            {
                            "@SHOP_CODE",
                            "@SALE_DATE",
                            "@POS_NO",
                            "@BILL_NO"
            },
            new object[]
            {
                            DataLocals.AppConfig.PosInfo.StoreNo,
                            saleDate,
                            posNo,
                            billNo
            });

            return result;

        }
        catch (FbException ex)
        {
            LogHelper.Logger.Error($"Database Error : {ex.Message.ReplacePlainText()}");
            return new List<RECEIPT_MANAGER_ITEM>();
        }
        catch (Exception ex)
        {
            LogHelper.Logger.Error($"Services Error : {ex.Message.ReplacePlainText()}");
            return new List<RECEIPT_MANAGER_ITEM>();
        }

        //using (var db = new DataContext())
        //{
        //    return await Task.FromResult(db.tRN_TENDERSEQs.Where(x => x.SALE_DATE == saleDate && x.POS_NO == posNo && x.SHOP_CODE == DataLocals.AppConfig.PosInfo.StoreNo).ToList());
        //}
    }
}
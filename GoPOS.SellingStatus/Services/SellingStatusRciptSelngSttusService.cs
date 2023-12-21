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
/// 화면명: 영수증별 매출현황 - 308 매출현황
/// </summary>

public class SellingStatusRciptSelngSttusService : ISellingStatusRciptSelngSttusService
{
    #region SellingStatusRciptSelngSttusView

  
    public async Task<(List<RCIPTSELNGSTTUS>, SpResult)> GetList1(DynamicParameters param)
    {
        try
        {
            string SQL = @"";
            //List<RciptSelngSttus> result = await DapperORM.ReturnListAsync<RciptSelngSttus>(SQL, param);
            List<RCIPTSELNGSTTUS> result = new List<RCIPTSELNGSTTUS>();
            //영수증		총매출	        총할인     실매출	         부가세	     합계	 점유율
            //BILL_NO       TOT_SALE_AMT    TOT_DC_AMT TOT_DCM_SALE_AMT  TOT_VAT_AMT TOT_AMT OCC_RATE

            for (int i = 0; i < 5; i++)
            {
                string dt = DateTime.Now.ToString("yyyyMMdd");
                result.Add(new RCIPTSELNGSTTUS() { NO = (i + 1).ToString(), BILL_NO = "0000" + (i + 1).ToString(), TOT_SALE_AMT = (i + 1).ToString() + ",000", TOT_DC_AMT = (i + 1).ToString() + ",500", TOT_DCM_SALE_AMT = (i + 1).ToString() + ",000", TOT_VAT_AMT = (i + 1).ToString() + ",000", TOT_AMT = (i + 1).ToString() + ",000", OCC_RATE = (i + 1).ToString() + "0%" });
            }

            return DapperORM.ReturnResult(result, EResultType.SUCCESS, "OK");

            //return DapperORM.ReturnResult(new List<ORDER_MENU>(), EResultType.SUCCESS, "OK");
        }
        catch (FbException ex)
        {
            LogHelper.Logger.Error($"Database Error : {ex.Message.ReplacePlainText()}");
            return DapperORM.ReturnResult(new List<RCIPTSELNGSTTUS>(), EResultType.ERROR, ex.Message.ReplacePlainText());
        }
        catch (Exception ex)
        {
            LogHelper.Logger.Error($"Services Error : {ex.Message.ReplacePlainText()}");
            return DapperORM.ReturnResult(new List<RCIPTSELNGSTTUS>(), EResultType.ERROR, ex.Message.ReplacePlainText());
        }
    }

    //사용 x
    public async Task<(List<RCIPTSELNGSTTUS>, SpResult)> GetList2(DynamicParameters param)
    {
        try
        {
            string SQL = @"";
            //List<RciptSelngSttus> result = await DapperORM.ReturnListAsync<RciptSelngSttus>(SQL, param);
            List<RCIPTSELNGSTTUS> result = new List<RCIPTSELNGSTTUS>();

            return DapperORM.ReturnResult(result, EResultType.SUCCESS, "OK");

            //return DapperORM.ReturnResult(new List<ORDER_MENU>(), EResultType.SUCCESS, "OK");
        }
        catch (FbException ex)
        {
            LogHelper.Logger.Error($"Database Error : {ex.Message.ReplacePlainText()}");
            return DapperORM.ReturnResult(new List<RCIPTSELNGSTTUS>(), EResultType.ERROR, ex.Message.ReplacePlainText());
        }
        catch (Exception ex)
        {
            LogHelper.Logger.Error($"Services Error : {ex.Message.ReplacePlainText()}");
            return DapperORM.ReturnResult(new List<RCIPTSELNGSTTUS>(), EResultType.ERROR, ex.Message.ReplacePlainText());
        }
    }


    //사용 x
    public async Task<(List<RCIPTSELNGSTTUS>, SpResult)> GetList3(DynamicParameters param)
    {
        try
        {
            string SQL = @"";
            //List<RciptSelngSttus> result = await DapperORM.ReturnListAsync<RciptSelngSttus>(SQL, param);
            List<RCIPTSELNGSTTUS> result = new List<RCIPTSELNGSTTUS>();

            return DapperORM.ReturnResult(result, EResultType.SUCCESS, "OK");

            //return DapperORM.ReturnResult(new List<ORDER_MENU>(), EResultType.SUCCESS, "OK");
        }
        catch (FbException ex)
        {
            LogHelper.Logger.Error($"Database Error : {ex.Message.ReplacePlainText()}");
            return DapperORM.ReturnResult(new List<RCIPTSELNGSTTUS>(), EResultType.ERROR, ex.Message.ReplacePlainText());
        }
        catch (Exception ex)
        {
            LogHelper.Logger.Error($"Services Error : {ex.Message.ReplacePlainText()}");
            return DapperORM.ReturnResult(new List<RCIPTSELNGSTTUS>(), EResultType.ERROR, ex.Message.ReplacePlainText());
        }
    }

    #endregion
}
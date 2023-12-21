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
/// 화면명: 시재입출금현황 - 매출현황
/// </summary>

public class SellingStatusTmPrRcppaySttusService : ISellingStatusTmPrRcppaySttusService
{
    #region SellingStatusTmPrRcppaySttusView

    public async Task<(List<TMPRRCPPAYSTTUS>, SpResult)> GetList1(DynamicParameters param)
    {
        try
        {
            string SQL = @"";

            //영업일자    등록일시    계정명        입금금액       출금액          판매원  비고
            //SALE_DATE   INSERT_DT   SHOP_ACCOUNT  POS_CSH_IN_AMT POS_CSH_OUT_AMT EMP_NO  REMARK

            //List<TmPrRcppaySttus> result = await DapperORM.ReturnListAsync<TmPrRcppaySttus>(SQL, param);

            List<TMPRRCPPAYSTTUS> result = new List<TMPRRCPPAYSTTUS>();

            for (int i = 0; i < 5; i++)
            {
                string dt1 = DateTime.Now.ToString("yyyyMMdd");
                string dt2 = DateTime.Now.AddDays(-i).AddHours(-i).ToString("yyyyMMdd");

                //SALE_DATE  ORDER_NO    ORDER_CNT  TOT_SALE_QTY    SALE_YN     BILL_NO     EMP_NO  INSERT_DT
                result.Add(new TMPRRCPPAYSTTUS() { NO = (i + 1).ToString(), SALE_DATE = dt1, INSERT_DT = dt2, SHOP_ACCOUNT = (i + 1).ToString()+"55345" + (i + 1).ToString(), POS_CSH_IN_AMT = (i + 1).ToString() + "35,000" , POS_CSH_OUT_AMT = (i + 1).ToString()+"25,000", EMP_NO = "판매원_" + (i + 1).ToString(), REMARK = "비고_"+(i+1).ToString() });
            }

            return DapperORM.ReturnResult(result, EResultType.SUCCESS, "OK");

            //return DapperORM.ReturnResult(new List<ORDER_MENU>(), EResultType.SUCCESS, "OK");
        }
        catch (FbException ex)
        {
            LogHelper.Logger.Error($"Database Error : {ex.Message.ReplacePlainText()}");
            return DapperORM.ReturnResult(new List<TMPRRCPPAYSTTUS>(), EResultType.ERROR, ex.Message.ReplacePlainText());
        }
        catch (Exception ex)
        {
            LogHelper.Logger.Error($"Services Error : {ex.Message.ReplacePlainText()}");
            return DapperORM.ReturnResult(new List<TMPRRCPPAYSTTUS>(), EResultType.ERROR, ex.Message.ReplacePlainText());
        }
    }

    // 사용x
    public async Task<(List<TMPRRCPPAYSTTUS>, SpResult)> GetList2(DynamicParameters param)
    {
        try
        {
            string SQL = @"";
            //List<TmPrRcppaySttus> result = await DapperORM.ReturnListAsync<TmPrRcppaySttus>(SQL, param);
            List<TMPRRCPPAYSTTUS> result = new List<TMPRRCPPAYSTTUS>();

            return DapperORM.ReturnResult(result, EResultType.SUCCESS, "OK");

            //return DapperORM.ReturnResult(new List<ORDER_MENU>(), EResultType.SUCCESS, "OK");
        }
        catch (FbException ex)
        {
            LogHelper.Logger.Error($"Database Error : {ex.Message.ReplacePlainText()}");
            return DapperORM.ReturnResult(new List<TMPRRCPPAYSTTUS>(), EResultType.ERROR, ex.Message.ReplacePlainText());
        }
        catch (Exception ex)
        {
            LogHelper.Logger.Error($"Services Error : {ex.Message.ReplacePlainText()}");
            return DapperORM.ReturnResult(new List<TMPRRCPPAYSTTUS>(), EResultType.ERROR, ex.Message.ReplacePlainText());
        }
    }

    //사용 x
    public async Task<(List<TMPRRCPPAYSTTUS>, SpResult)> GetList3(DynamicParameters param)
    {
        try
        {
            string SQL = @"";
            //List<TmPrRcppaySttus> result = await DapperORM.ReturnListAsync<TmPrRcppaySttus>(SQL, param);
            List<TMPRRCPPAYSTTUS> result = new List<TMPRRCPPAYSTTUS>();

            return DapperORM.ReturnResult(result, EResultType.SUCCESS, "OK");

            //return DapperORM.ReturnResult(new List<ORDER_MENU>(), EResultType.SUCCESS, "OK");
        }
        catch (FbException ex)
        {
            LogHelper.Logger.Error($"Database Error : {ex.Message.ReplacePlainText()}");
            return DapperORM.ReturnResult(new List<TMPRRCPPAYSTTUS>(), EResultType.ERROR, ex.Message.ReplacePlainText());
        }
        catch (Exception ex)
        {
            LogHelper.Logger.Error($"Services Error : {ex.Message.ReplacePlainText()}");
            return DapperORM.ReturnResult(new List<TMPRRCPPAYSTTUS>(), EResultType.ERROR, ex.Message.ReplacePlainText());
        }
    }
    #endregion
}
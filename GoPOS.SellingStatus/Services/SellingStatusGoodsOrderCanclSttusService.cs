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
/// 화면명: 상품별주문취소현황 309 - 매출현황
/// </summary>

public class SellingStatusGoodsOrderCanclSttusService : ISellingStatusGoodsOrderCanclSttusService
{
    #region SellingStatusGoodsOrderCanclSttusView

    public async Task<(List<GOODSORDERCANCLSTTUS>, SpResult)> GetList1(DynamicParameters param)
    {
        try
        {
            string SQL = @"";
            //List<GoodsOrderCanclSttus> result = await DapperORM.ReturnListAsync<GoodsOrderCanclSttus>(SQL, param);
            List<GOODSORDERCANCLSTTUS> result = new List<GOODSORDERCANCLSTTUS>();

            //분류명      총수량          총매출       총할인      실매출           부가세      합계    점유율
            //SCLASS_NAME TOT_SALE_QTY    TOT_SALE_AMT TOT_DC_AMT  TOT_DCM_SALE_AMT TOT_VAT_AMT TOT_AMT OCC_RATE

            for (int i = 0; i < 5; i++)
            {
                string dt = DateTime.Now.ToString("yyyyMMdd");
                string dt2 = DateTime.Now.AddDays(-1).AddHours(-i).ToString("yyyyMMdd");
                result.Add(new GOODSORDERCANCLSTTUS() { NO = (i + 1).ToString(), SCLASS_NAME = "음료" + (i + 1).ToString(), TOT_SALE_QTY = (i + 1).ToString(), TOT_SALE_AMT = (i + 1).ToString() + ",000", TOT_DC_AMT = (i + 1).ToString() + ",500", TOT_DCM_SALE_AMT = (i + 1).ToString() + ",000", TOT_VAT_AMT = (i + 1).ToString() + ",000", TOT_AMT = (i + 1).ToString() + ",000", OCC_RATE = (i + 1).ToString() + "0%" });
            }
            
            return DapperORM.ReturnResult(result, EResultType.SUCCESS, "OK");

         
        }
        catch (FbException ex)
        {
            LogHelper.Logger.Error($"Database Error : {ex.Message.ReplacePlainText()}");
            return DapperORM.ReturnResult(new List<GOODSORDERCANCLSTTUS>(), EResultType.ERROR, ex.Message.ReplacePlainText());
        }
        catch (Exception ex)
        {
            LogHelper.Logger.Error($"Services Error : {ex.Message.ReplacePlainText()}");
            return DapperORM.ReturnResult(new List<GOODSORDERCANCLSTTUS>(), EResultType.ERROR, ex.Message.ReplacePlainText());
        }
    }

    public async Task<(List<GOODSORDERCANCLSTTUS>, SpResult)> GetList2(DynamicParameters param)
    {
        try
        {
            string SQL = @"";
            //List<GoodsOrderCanclSttus> result = await DapperORM.ReturnListAsync<GoodsOrderCanclSttus>(SQL, param);
            List<GOODSORDERCANCLSTTUS> result = new List<GOODSORDERCANCLSTTUS>();

            //영업일자	 주문번호	 주문자수	총수량	        취소구분	영수증번호	판매원	취소시각
            //SALE_DATE  ORDER_NO    ORDER_CNT  TOT_SALE_QTY    SALE_YN     BILL_NO     EMP_NO  INSERT_DT

            for (int i = 0; i < 5; i++)
            {
                string dt1 = DateTime.Now.ToString("yyyyMMdd");
                string dt2 = DateTime.Now.AddDays(-1).AddHours(-i).ToString("yyyyMMdd");

                //SALE_DATE  ORDER_NO    ORDER_CNT  TOT_SALE_QTY    SALE_YN     BILL_NO     EMP_NO  INSERT_DT
                result.Add(new GOODSORDERCANCLSTTUS() { NO = (i + 1).ToString(),  SALE_DATE = dt1, ORDER_NO = "0000"+(i + 1).ToString(), ORDER_CNT = "5"+(i + 1).ToString(), TOT_SALE_QTY = "50"+(i + 1).ToString(), SALE_YN = "판매", BILL_NO = "000"+(i + 1).ToString(), EMP_NO = "0001", INSERT_DT=dt2 });
            }

            return DapperORM.ReturnResult(result, EResultType.SUCCESS, "OK");

        }
        catch (FbException ex)
        {
            LogHelper.Logger.Error($"Database Error : {ex.Message.ReplacePlainText()}");
            return DapperORM.ReturnResult(new List<GOODSORDERCANCLSTTUS>(), EResultType.ERROR, ex.Message.ReplacePlainText());
        }
        catch (Exception ex)
        {
            LogHelper.Logger.Error($"Services Error : {ex.Message.ReplacePlainText()}");
            return DapperORM.ReturnResult(new List<GOODSORDERCANCLSTTUS>(), EResultType.ERROR, ex.Message.ReplacePlainText());
        }
    }


    // 사용 x
    public async Task<(List<GOODSORDERCANCLSTTUS>, SpResult)> GetList3(DynamicParameters param)
    {
        try
        {
            string SQL = @"";
            //List<GoodsOrderCanclSttus> result = await DapperORM.ReturnListAsync<GoodsOrderCanclSttus>(SQL, param);
            List<GOODSORDERCANCLSTTUS> result = new List<GOODSORDERCANCLSTTUS>();

            return DapperORM.ReturnResult(result, EResultType.SUCCESS, "OK");

            //return DapperORM.ReturnResult(new List<ORDER_MENU>(), EResultType.SUCCESS, "OK");
        }
        catch (FbException ex)
        {
            LogHelper.Logger.Error($"Database Error : {ex.Message.ReplacePlainText()}");
            return DapperORM.ReturnResult(new List<GOODSORDERCANCLSTTUS>(), EResultType.ERROR, ex.Message.ReplacePlainText());
        }
        catch (Exception ex)
        {
            LogHelper.Logger.Error($"Services Error : {ex.Message.ReplacePlainText()}");
            return DapperORM.ReturnResult(new List<GOODSORDERCANCLSTTUS>(), EResultType.ERROR, ex.Message.ReplacePlainText());
        }
    }

    #endregion
}
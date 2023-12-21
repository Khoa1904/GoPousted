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
using System.Reflection;
using System.Threading.Tasks;
using static System.Data.Entity.Infrastructure.Design.Executor;

namespace GoPOS.Services;

/// <summary>
/// 작성자: 김형석 
/// 화면명: 코너별 매출현황 - 310 매출현황
/// </summary>

public class SellingStatusCrnSelngSttusService : ISellingStatusCrnSelngSttusService
{
    #region SellingStatusCrnSelngSttusView

    public async Task<(List<CRNSELNGSTTUS>, SpResult)> GetList1(DynamicParameters param)
    {
        try
        {
            //코너명 총수량 총매출 총할인 실매출 부가세 합계 점유율
            //CORNER_CODE TOT_SALE_QTY    TOT_SALE_AMT DC_AMT  DCM_SALE_AMT VAT_AMT TOT_AMT OCC_RATE

            string SQL = @"";
            //List<CrnSelngSttus> result = await DapperORM.ReturnListAsync<CrnSelngSttus>(SQL, param);
            List<CRNSELNGSTTUS> result = new List<CRNSELNGSTTUS>();

            //result.Add(new CrnSelngSttus() { NO = "1", SALE_DATE = "20230304", TOT_SALE_QTY = "1", TOT_SALE_AMT = "1,000", TOT_DC_AMT = "500", TOT_DCM_SALE_AMT = "1,000", TOT_VAT_AMT = "1,000", TOT_AMT = "1,000", OCC_RATE = "10%" });
            //result.Add(new CrnSelngSttus() { NO = "2", SALE_DATE = "20230304", TOT_SALE_QTY = "2", TOT_SALE_AMT = "2,500", TOT_DC_AMT = "500", TOT_DCM_SALE_AMT = "2,500", TOT_VAT_AMT = "2,500", TOT_AMT = "2,500", OCC_RATE = "20%" });
            //result.Add(new CrnSelngSttus() { NO = "3", SALE_DATE = "20230304", TOT_SALE_QTY = "3", TOT_SALE_AMT = "3,500", TOT_DC_AMT = "500", TOT_DCM_SALE_AMT = "3,500", TOT_VAT_AMT = "3,500", TOT_AMT = "3,500", OCC_RATE = "30%" });
            //result.Add(new CrnSelngSttus() { NO = "4", SALE_DATE = "20230304", TOT_SALE_QTY = "4", TOT_SALE_AMT = "4,500", TOT_DC_AMT = "1,500", TOT_DCM_SALE_AMT = "4,500", TOT_VAT_AMT = "4,500", TOT_AMT = "4,500", OCC_RATE = "40%" });
            //result.Add(new CrnSelngSttus() { NO = "6", SALE_DATE = "20230304", TOT_SALE_QTY = "4", TOT_SALE_AMT = "4,500", TOT_DC_AMT = "1,500", TOT_DCM_SALE_AMT = "4,500", TOT_VAT_AMT = "4,500", TOT_AMT = "4,500", OCC_RATE = "40%" });
            //result.Add(new CrnSelngSttus() { NO = "7", SALE_DATE = "20230304", TOT_SALE_QTY = "4", TOT_SALE_AMT = "4,500", TOT_DC_AMT = "1,500", TOT_DCM_SALE_AMT = "4,500", TOT_VAT_AMT = "4,500", TOT_AMT = "4,500", OCC_RATE = "40%" });

            for (int i = 0; i < 5;i ++)
            {
                string dt = DateTime.Now.ToString("yyyyMMdd");
                result.Add(new CRNSELNGSTTUS() { NO = (i+1).ToString(), CORNER_CODE = "음료코너"+ (i + 1).ToString() , TOT_SALE_QTY = (i + 1).ToString(), TOT_SALE_AMT = (i + 1).ToString()+",000", TOT_DC_AMT = (i + 1).ToString()+",500", TOT_DCM_SALE_AMT = (i + 1).ToString()+",000", TOT_VAT_AMT = (i + 1).ToString()+",000", TOT_AMT = (i + 1).ToString()+",000", OCC_RATE = (i + 1).ToString()+"0%" });
            }
            return DapperORM.ReturnResult(result, EResultType.SUCCESS, "OK");

        }
        catch (FbException ex)
        {
            LogHelper.Logger.Error($"Database Error : {ex.Message.ReplacePlainText()}");
            return DapperORM.ReturnResult(new List<CRNSELNGSTTUS>(), EResultType.ERROR, ex.Message.ReplacePlainText());
        }
        catch (Exception ex)
        {
            LogHelper.Logger.Error($"Services Error : {ex.Message.ReplacePlainText()}");
            return DapperORM.ReturnResult(new List<CRNSELNGSTTUS>(), EResultType.ERROR, ex.Message.ReplacePlainText());
        }
    }

    public async Task<(List<CRNSELNGSTTUS>, SpResult)> GetList2(DynamicParameters param)
    {
        try
        {

            //영업일자  총수량          총매출       총할인      실매출           부가세      합계    점유율
            //SALE_DATE TOT_SALE_QTY    TOT_SALE_AMT TOT_DC_AMT  TOT_DCM_SALE_AMT TOT_VAT_AMT TOT_AMT OCC_RATE
            //{

            string SQL = @"";
            //List<CrnSelngSttus> result = await DapperORM.ReturnListAsync<CrnSelngSttus>(SQL, param);
            List<CRNSELNGSTTUS> result = new List<CRNSELNGSTTUS>();

            ///result.Add(new CrnSelngSttus() { NO = "1", SALE_DATE = "20230304", TOT_SALE_QTY = "10", TOT_SALE_AMT = "10,000", TOT_DC_AMT = "5,000",   TOT_DCM_SALE_AMT = "1,0000", TOT_VAT_AMT = "1,000", TOT_AMT = "1,0000", OCC_RATE = "15%" });
            ///result.Add(new CrnSelngSttus() { NO = "2", SALE_DATE = "20230304", TOT_SALE_QTY = "20", TOT_SALE_AMT = "20,500", TOT_DC_AMT = "5,000",   TOT_DCM_SALE_AMT = "2,5000", TOT_VAT_AMT = "2,500", TOT_AMT = "2,5000", OCC_RATE = "25%" });
            ///result.Add(new CrnSelngSttus() { NO = "3", SALE_DATE = "20230304", TOT_SALE_QTY = "30", TOT_SALE_AMT = "30,500", TOT_DC_AMT = "5,000",   TOT_DCM_SALE_AMT = "3,5000", TOT_VAT_AMT = "3,500", TOT_AMT = "3,5000", OCC_RATE = "35%" });
            ///result.Add(new CrnSelngSttus() { NO = "4", SALE_DATE = "20230304", TOT_SALE_QTY = "40", TOT_SALE_AMT = "40,500", TOT_DC_AMT = "10,500", TOT_DCM_SALE_AMT = "4,5000", TOT_VAT_AMT = "4,500", TOT_AMT = "4,5000", OCC_RATE = "45%" });

            for (int i = 0; i < 5; i++)
            {
                string dt = DateTime.Now.ToString("yyyyMMdd");
                result.Add(new CRNSELNGSTTUS() { NO = (i + 1).ToString(), SALE_DATE = dt, TOT_SALE_QTY = (i + 1).ToString(), TOT_SALE_AMT = (i + 1).ToString() + ",000", TOT_DC_AMT = (i + 1).ToString() + ",500", TOT_DCM_SALE_AMT = (i + 1).ToString() + ",000", TOT_VAT_AMT = (i + 1).ToString() + ",000", TOT_AMT = (i + 1).ToString() + ",000", OCC_RATE = (i + 1).ToString() + "0%" });
            }

            return DapperORM.ReturnResult(result, EResultType.SUCCESS, "OK");
        }
        catch (FbException ex)
        {
            LogHelper.Logger.Error($"Database Error : {ex.Message.ReplacePlainText()}");
            return DapperORM.ReturnResult(new List<CRNSELNGSTTUS>(), EResultType.ERROR, ex.Message.ReplacePlainText());
        }
        catch (Exception ex)
        {
            LogHelper.Logger.Error($"Services Error : {ex.Message.ReplacePlainText()}");
            return DapperORM.ReturnResult(new List<CRNSELNGSTTUS>(), EResultType.ERROR, ex.Message.ReplacePlainText());
        }
    }


    // 사용 x
    public async Task<(List<CRNSELNGSTTUS>, SpResult)> GetList3(DynamicParameters param)
    {
        try
        {
            string SQL = @"";
            //List<CrnSelngSttus> result = await DapperORM.ReturnListAsync<CrnSelngSttus>(SQL, param);
            List<CRNSELNGSTTUS> result = new List<CRNSELNGSTTUS>();

            //result.Add(new CrnSelngSttus() { NO = "1", SALE_DATE = "20230304", TOT_SALE_QTY = "1", TOT_SALE_AMT = "1,000", TOT_DC_AMT = "500", TOT_DCM_SALE_AMT = "1,000", TOT_VAT_AMT = "1,000", TOT_AMT = "1,000", OCC_RATE = "10%" });
            //result.Add(new CrnSelngSttus() { NO = "2", SALE_DATE = "20230304", TOT_SALE_QTY = "2", TOT_SALE_AMT = "2,500", TOT_DC_AMT = "500", TOT_DCM_SALE_AMT = "2,500", TOT_VAT_AMT = "2,500", TOT_AMT = "2,500", OCC_RATE = "20%" });
            //result.Add(new CrnSelngSttus() { NO = "4", SALE_DATE = "20230304", TOT_SALE_QTY = "3", TOT_SALE_AMT = "3,500", TOT_DC_AMT = "500", TOT_DCM_SALE_AMT = "3,500", TOT_VAT_AMT = "3,500", TOT_AMT = "3,500", OCC_RATE = "30%" });
            //result.Add(new CrnSelngSttus() { NO = "5", SALE_DATE = "20230304", TOT_SALE_QTY = "4", TOT_SALE_AMT = "4,500", TOT_DC_AMT = "1,500", TOT_DCM_SALE_AMT = "4,500", TOT_VAT_AMT = "4,500", TOT_AMT = "4,500", OCC_RATE = "40%" });

            return DapperORM.ReturnResult(result, EResultType.SUCCESS, "OK");

        }
        catch (FbException ex)
        {
            LogHelper.Logger.Error($"Database Error : {ex.Message.ReplacePlainText()}");
            return DapperORM.ReturnResult(new List<CRNSELNGSTTUS>(), EResultType.ERROR, ex.Message.ReplacePlainText());
        }
        catch (Exception ex)
        {
            LogHelper.Logger.Error($"Services Error : {ex.Message.ReplacePlainText()}");
            return DapperORM.ReturnResult(new List<CRNSELNGSTTUS>(), EResultType.ERROR, ex.Message.ReplacePlainText());
        }
    }

    #endregion
}
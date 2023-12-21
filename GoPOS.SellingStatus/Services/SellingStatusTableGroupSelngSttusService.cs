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
/// 화면명: 테이블그룹 매출현황 - 312 매출현황
/// </summary>

public class SellingStatusTableGroupSelngSttusService : ISellingStatusTableGroupSelngSttusService
{
    #region SellingStatusTableGroupSelngSttusView






    public async Task<(List<TABLEGROUPSELNGSTTUS>, SpResult)> GetList1(DynamicParameters param)
    {
        try
        {
            string SQL = @"";
            //List<TableGroupSelngSttus> result = await DapperORM.ReturnListAsync<TableGroupSelngSttus>(SQL, param);

            List<TABLEGROUPSELNGSTTUS> result = new List<TABLEGROUPSELNGSTTUS>();

            //상단:		
            //        영업일자  영업요일        영업일수      실매출합계          회전수합계      고객수합계
            //        SALE_DATE DAY_SALE_DATE   TOT_SALE_DATE TOT_DCM_SALE_AMT    TOT_CST_ROT_CNT TOT_CST_CNT

            for (int i = 0; i < 5; i++)
            {
                string dt1 = DateTime.Now.AddDays(+i).ToString("yyyyMMdd");
                string dt2 = DateTime.Now.AddDays(-1).AddHours(-i).ToString("yyyyMMdd");

                //SALE_DATE  ORDER_NO    ORDER_CNT  TOT_SALE_QTY    SALE_YN     BILL_NO     EMP_NO  INSERT_DT
                if (i == 0)
                {
                    result.Add(new TABLEGROUPSELNGSTTUS() { NO = (i + 1).ToString(), SALE_DATE = dt1, DAY_SALE_DATE = "일요일", TOT_SALE_DATE = (i + 10).ToString(), TOT_DCM_SALE_AMT = (i + 1).ToString() + "5,000", TOT_CST_ROT_CNT = (i + 1).ToString(), TOT_CST_CNT = (i + 1).ToString() + "0" });
                }
                else if (i == 1)
                {
                    result.Add(new TABLEGROUPSELNGSTTUS() { NO = (i + 1).ToString(), SALE_DATE = dt1, DAY_SALE_DATE = "월요일", TOT_SALE_DATE = (i + 11).ToString(), TOT_DCM_SALE_AMT = (i + 1).ToString() + "5,000", TOT_CST_ROT_CNT = (i + 1).ToString(), TOT_CST_CNT = (i + 1).ToString() + "0" });
                }
                else if (i == 2)
                {
                    result.Add(new TABLEGROUPSELNGSTTUS() { NO = (i + 1).ToString(), SALE_DATE = dt1, DAY_SALE_DATE = "화요일", TOT_SALE_DATE = (i + 12).ToString(), TOT_DCM_SALE_AMT = (i + 1).ToString() + "5,000", TOT_CST_ROT_CNT = (i + 1).ToString(), TOT_CST_CNT = (i + 1).ToString() + "0" });
                }
                else if (i == 3)
                {
                    result.Add(new TABLEGROUPSELNGSTTUS() { NO = (i + 1).ToString(), SALE_DATE = dt1, DAY_SALE_DATE = "수요일", TOT_SALE_DATE = (i + 13).ToString(), TOT_DCM_SALE_AMT = (i + 1).ToString() + "5,000", TOT_CST_ROT_CNT = (i + 1).ToString(), TOT_CST_CNT = (i + 1).ToString() + "0" });
                }
                else if (i == 4)
                {
                    result.Add(new TABLEGROUPSELNGSTTUS() { NO = (i + 1).ToString(), SALE_DATE = dt1, DAY_SALE_DATE = "목요일", TOT_SALE_DATE = (i + 14).ToString(), TOT_DCM_SALE_AMT = (i + 1).ToString() + "5,000", TOT_CST_ROT_CNT = (i + 1).ToString(), TOT_CST_CNT = (i + 1).ToString() + "0" });
                }
                else if (i == 5)
                {
                    result.Add(new TABLEGROUPSELNGSTTUS() { NO = (i + 1).ToString(), SALE_DATE = dt1, DAY_SALE_DATE = "금요일", TOT_SALE_DATE = (i + 15).ToString(), TOT_DCM_SALE_AMT = (i + 1).ToString() + "5,000", TOT_CST_ROT_CNT = (i + 1).ToString(), TOT_CST_CNT = (i + 1).ToString() + "0" });
                }
                else if (i == 6)
                {
                    result.Add(new TABLEGROUPSELNGSTTUS() { NO = (i + 1).ToString(), SALE_DATE = dt1, DAY_SALE_DATE = "토요일", TOT_SALE_DATE = (i + 16).ToString(), TOT_DCM_SALE_AMT = (i + 1).ToString() + "5,000", TOT_CST_ROT_CNT = (i + 1).ToString(), TOT_CST_CNT = (i + 1).ToString() + "0" });
                }
            }

            return DapperORM.ReturnResult(result, EResultType.SUCCESS, "OK");

        }
        catch (FbException ex)
        {
            LogHelper.Logger.Error($"Database Error : {ex.Message.ReplacePlainText()}");
            return DapperORM.ReturnResult(new List<TABLEGROUPSELNGSTTUS>(), EResultType.ERROR, ex.Message.ReplacePlainText());
        }
        catch (Exception ex)
        {
            LogHelper.Logger.Error($"Services Error : {ex.Message.ReplacePlainText()}");
            return DapperORM.ReturnResult(new List<TABLEGROUPSELNGSTTUS>(), EResultType.ERROR, ex.Message.ReplacePlainText());
        }
    }

    public async Task<(List<TABLEGROUPSELNGSTTUS>, SpResult)> GetList2(DynamicParameters param)
    {
        try
        {
            string SQL = @"";
            //List<TableGroupSelngSttus> result = await DapperORM.ReturnListAsync<TableGroupSelngSttus>(SQL, param);
            List<TABLEGROUPSELNGSTTUS> result = new List<TABLEGROUPSELNGSTTUS>();

            //하단 좌측:
            //        테이블그룹명 실매출            회전수          고객수
            //        TG_NAME      TOT_DCM_SALE_AMT  TOT_CST_ROT_CNT TOT_CST_CNT

            for (int i = 0; i < 5; i++)
            {
                string dt1 = DateTime.Now.ToString("yyyyMMdd");
                string dt2 = DateTime.Now.AddDays(-1).AddHours(-i).ToString("yyyyMMdd");

                //SALE_DATE  ORDER_NO    ORDER_CNT  TOT_SALE_QTY    SALE_YN     BILL_NO     EMP_NO  INSERT_DT
                result.Add(new TABLEGROUPSELNGSTTUS() { NO = (i + 1).ToString(), TG_NAME = "테이블_0" + (i + 1).ToString(), TOT_DCM_SALE_AMT = (i + 1).ToString() + "0,000", TOT_CST_ROT_CNT = "2" + (i + 1).ToString(), TOT_CST_CNT = "5" + (i + 1).ToString() });
            }

            return DapperORM.ReturnResult(result, EResultType.SUCCESS, "OK");

        }
        catch (FbException ex)
        {
            LogHelper.Logger.Error($"Database Error : {ex.Message.ReplacePlainText()}");
            return DapperORM.ReturnResult(new List<TABLEGROUPSELNGSTTUS>(), EResultType.ERROR, ex.Message.ReplacePlainText());
        }
        catch (Exception ex)
        {
            LogHelper.Logger.Error($"Services Error : {ex.Message.ReplacePlainText()}");
            return DapperORM.ReturnResult(new List<TABLEGROUPSELNGSTTUS>(), EResultType.ERROR, ex.Message.ReplacePlainText());
        }
    }

    public async Task<(List<TABLEGROUPSELNGSTTUS>, SpResult)> GetList3(DynamicParameters param)
    {
        try
        {
            string SQL = @"";
            //List<TableGroupSelngSttus> result = await DapperORM.ReturnListAsync<TableGroupSelngSttus>(SQL, param);
            List<TABLEGROUPSELNGSTTUS> result = new List<TABLEGROUPSELNGSTTUS>();

            //하단 우측:
            //        결제수단        건수    금액
            //        PAYMENT_METHOD  PAY_CNT SALE_AMT

            for (int i = 0; i < 3; i++)
            {
                string dt1 = DateTime.Now.ToString("yyyyMMdd");
                string dt2 = DateTime.Now.AddDays(-1).AddHours(-i).ToString("yyyyMMdd");

                //SALE_DATE  ORDER_NO    ORDER_CNT  TOT_SALE_QTY    SALE_YN     BILL_NO     EMP_NO  INSERT_DT

                if (i == 0)
                {
                    result.Add(new TABLEGROUPSELNGSTTUS() { NO = (i + 1).ToString(), PAYMENT_METHOD = "카드", PAY_CNT = (i + 1).ToString() + "00", SALE_AMT = (i + 1).ToString() + "0,000" });
                }
                else if (i == 1)
                {
                    result.Add(new TABLEGROUPSELNGSTTUS() { NO = (i + 1).ToString(), PAYMENT_METHOD = "현금", PAY_CNT = (i + 1).ToString() + "00", SALE_AMT = (i + 1).ToString() + "0,000" });
                }
                else if (i == 2)
                {
                    result.Add(new TABLEGROUPSELNGSTTUS() { NO = (i + 1).ToString(), PAYMENT_METHOD = "외상", PAY_CNT = (i + 1).ToString() + "00", SALE_AMT = (i + 1).ToString() + "0,000" });
                }
            }

            return DapperORM.ReturnResult(result, EResultType.SUCCESS, "OK");

            //return DapperORM.ReturnResult(new List<ORDER_MENU>(), EResultType.SUCCESS, "OK");
        }
        catch (FbException ex)
        {
            LogHelper.Logger.Error($"Database Error : {ex.Message.ReplacePlainText()}");
            return DapperORM.ReturnResult(new List<TABLEGROUPSELNGSTTUS>(), EResultType.ERROR, ex.Message.ReplacePlainText());
        }
        catch (Exception ex)
        {
            LogHelper.Logger.Error($"Services Error : {ex.Message.ReplacePlainText()}");
            return DapperORM.ReturnResult(new List<TABLEGROUPSELNGSTTUS>(), EResultType.ERROR, ex.Message.ReplacePlainText());
        }
    }

    #endregion
}
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
using GoPOS.Models.Custom.SalesMng;
using GoPOS.Models.Custom.SellingStatus;

namespace GoPOS.Services;

public class SellingStatusService : ISellingStatusService
{
    /*
    ClSelngSttusView      분류별 매출현황       -NO 분류명 총수량 총매출 총할인 실매출 부가세 합계 점유율  -NO 영업일자 총수량 총매출 총할인 실매출 부가세 합계 점유율
    GoodsSelngSttusView   상품별 매출현황       -NO 분류명 총수량 총매출 총할인 실매출 부가세 합계 점유율  -NO 분류명 총수량 총매출 총할인 실매출 부가세 합계 점유율
    PaymntSelngSttusView  결제유형별 매출현황   -NO 결제수단 결제건수 결제금액 점유율                      -NO 영업일자 결제건수 결제금액 점유율
    DscntSelngSttusView   할인유형별 매출현황   -NO 할인유형 건수 금액 점유율(건수)                        -NO 영업일자 건수 금액 점유율(건수) 
    MtSelngSttusView      월별 매출현황
    ExcclcSttusView       정산현황               
    TimeSelngSttusView    시간대별 매출현황     -NO 영업일자 총건수 총매출 총할인 실매출 부가세 합계 점유율 -NO 영업일자 총건수 총매출 총할인 실매출 부가세 합계 점유율
     
     
     */
    public async Task<(List<SELLING_STATUS_INFO>, SpResult)> GetClSelngSttusMainList(DynamicParameters param)
    {
        try
        {
            //string SQL = @"                           
            //SELECT *
            //  FROM POS_ORD_M
            //";

            //List<SELLING_STATUS_INFO> result = await DapperORM.ReturnListAsync<SELLING_STATUS_INFO>(SQL, param); // stored procedure

            string SQL = ResourceHelpers.LoadSqlCommand(SQLFileTypes.ProductSalesStatus, "GetSelngStatus");
            await Task.Delay(10);
            List<SELLING_STATUS_INFO> result2 = await DapperORM.ReturnListAsync<SELLING_STATUS_INFO>(SQL,
                param);


            List<SELLING_STATUS_INFO> result = new List<SELLING_STATUS_INFO>();

            if (result == null)
            {
                LogHelper.Logger.Warn($"Database : 조회(DB) 결과가 없습니다.");
                return DapperORM.ReturnResult(new List<SELLING_STATUS_INFO>(), EResultType.EMPTY, "조회(DB) 결과가 없습니다.");
            }

            //테스트 데이터


            result.Clear();
            SELLING_STATUS_INFO resulStruct = new SELLING_STATUS_INFO();
            for (int i = 0; i < result2.Count(); i++)
            {
                /*
                 
                 NO
                 SCLASS_NAME
                 TOT_SALE_QTY
                 TOT_SALE_AMT
                 TOT_DC_AMT
                 DCM_SALE_AMT
                 VAT_AMT
                 TOT_AMT
                 OCC_RATE
                 SALE_DATE
                 PAYMENT_METHOD
                 PAY_CNT
                 SALE_AMT
                 DIS_CLS
                 현금 신용카드 외상 회원포인트 상품권 식권
                 */
                resulStruct = new SELLING_STATUS_INFO();
                resulStruct.NO = (i + 1).ToString();
                resulStruct.SALE_DATE = result2[i].SALE_DATE;
                resulStruct.TOT_SALE_AMT = result2[i].TOT_SALE_AMT.ToString();


                result.Add(resulStruct);
            }



            return DapperORM.ReturnResult(result, EResultType.SUCCESS, "OK");
        }
        catch (FbException ex)
        {
            LogHelper.Logger.Error($"Database Error : {ex.Message.ReplacePlainText()}");
            return DapperORM.ReturnResult(new List<SELLING_STATUS_INFO>(), EResultType.ERROR, ex.Message.ReplacePlainText());
        }
        catch (Exception ex)
        {
            LogHelper.Logger.Error($"Services Error : {ex.Message.ReplacePlainText()}");
            return DapperORM.ReturnResult(new List<SELLING_STATUS_INFO>(), EResultType.ERROR, ex.Message.ReplacePlainText());
        }
    } // abc
    public async Task<(List<SELLING_STATUS_INFO>, SpResult)> GetClSelngSttusDetailList(DynamicParameters param)
    {
        try
        {
            //string SQL = @"                           
            //SELECT *
            //  FROM POS_ORD_M
            //";

            //List<SELLING_STATUS_INFO> result = await DapperORM.ReturnListAsync<SELLING_STATUS_INFO>(SQL, param); // stored procedure
            await Task.Delay(10);
            List<SELLING_STATUS_INFO> result = new List<SELLING_STATUS_INFO>();

            if (result == null)
            {
                LogHelper.Logger.Warn($"Database : 조회(DB) 결과가 없습니다.");
                return DapperORM.ReturnResult(new List<SELLING_STATUS_INFO>(), EResultType.EMPTY, "조회(DB) 결과가 없습니다.");
            }

            //테스트 데이터
            Random random = new Random();
            int randomNumber = random.Next(10000, 100001);
            string[] sPAYMENT_METHOD = new string[] { "현금", "신용카드", "외상", "회원포인트", "상품권", "식권" };
            string[] sDIS_CLS = new string[] { "일반할인", "식권할인" };

            result.Clear();
            SELLING_STATUS_INFO resulStruct = new SELLING_STATUS_INFO();
            for (int i = 0; i < 20; i++)
            {
                /*
                 
                 NO
                 SCLASS_NAME
                 TOT_SALE_QTY
                 TOT_SALE_AMT
                 TOT_DC_AMT
                 DCM_SALE_AMT
                 VAT_AMT
                 TOT_AMT
                 OCC_RATE
                 SALE_DATE
                 PAYMENT_METHOD
                 PAY_CNT
                 SALE_AMT
                 DIS_CLS
                 현금 신용카드 외상 회원포인트 상품권 식권
                 */
                resulStruct = new SELLING_STATUS_INFO();
                resulStruct.NO = (i + 1).ToString();
                resulStruct.SCLASS_NAME = DataLocals.PosStatus.SALE_DATE;
                resulStruct.TOT_SALE_QTY = random.Next(10, 100).ToString();
                resulStruct.TOT_SALE_AMT = random.Next(10000, 1000000).ToString();
                resulStruct.TOT_DC_AMT = random.Next(10000, 1000000).ToString();
                resulStruct.DCM_SALE_AMT = random.Next(10000, 1000000).ToString();
                resulStruct.VAT_AMT = random.Next(10000, 1000000).ToString();
                resulStruct.TOT_AMT = random.Next(10000, 1000000).ToString();
                resulStruct.OCC_RATE = random.Next(1, 99).ToString();
                resulStruct.SALE_DATE = DataLocals.PosStatus.SALE_DATE;
                resulStruct.PAYMENT_METHOD = sPAYMENT_METHOD[i % 6];
                resulStruct.PAY_CNT = random.Next(1, 200).ToString();
                resulStruct.SALE_AMT = random.Next(10000, 1000000).ToString();
                resulStruct.DIS_CLS = DataLocals.PosStatus.SALE_DATE;

                result.Add(resulStruct);
            }

            return DapperORM.ReturnResult(result, EResultType.SUCCESS, "OK");
        }
        catch (FbException ex)
        {
            LogHelper.Logger.Error($"Database Error : {ex.Message.ReplacePlainText()}");
            return DapperORM.ReturnResult(new List<SELLING_STATUS_INFO>(), EResultType.ERROR, ex.Message.ReplacePlainText());
        }
        catch (Exception ex)
        {
            LogHelper.Logger.Error($"Services Error : {ex.Message.ReplacePlainText()}");
            return DapperORM.ReturnResult(new List<SELLING_STATUS_INFO>(), EResultType.ERROR, ex.Message.ReplacePlainText());
        }
    }
    public async Task<(List<SELLING_STATUS_INFO>, SpResult)> GetGoodsSelngSttusMainList(DynamicParameters param)
    {
        try
        {
            //string SQL = @"                           
            //SELECT *
            //  FROM POS_ORD_M
            //";

            //List<SELLING_STATUS_INFO> result = await DapperORM.ReturnListAsync<SELLING_STATUS_INFO>(SQL, param); // stored procedure
            string SQL = ResourceHelpers.LoadSqlCommand(SQLFileTypes.ProductSalesStatus, "GetMasterProduct");
            await Task.Delay(10);
            List<SELLING_STATUS_INFO> result2 = await DapperORM.ReturnListAsync<SELLING_STATUS_INFO>(SQL,
                param);

            List<SELLING_STATUS_INFO> result = new List<SELLING_STATUS_INFO>();

            if (result == null)
            {
                LogHelper.Logger.Warn($"Database : 조회(DB) 결과가 없습니다.");
                return DapperORM.ReturnResult(new List<SELLING_STATUS_INFO>(), EResultType.EMPTY, "조회(DB) 결과가 없습니다.");
            }

            //테스트 데이터
            Random random = new Random();
            int randomNumber = random.Next(10000, 100001);
            string[] sPAYMENT_METHOD = new string[] { "현금", "신용카드", "외상", "회원포인트", "상품권", "식권" };
            string[] sDIS_CLS = new string[] { "일반할인", "식권할인" };

            result.Clear();
            SELLING_STATUS_INFO resulStruct = new SELLING_STATUS_INFO();
            for (int i = 0; i < result2.Count(); i++)
            {
                /*
                 
                 NO
                 SCLASS_NAME
                 TOT_SALE_QTY
                 TOT_SALE_AMT
                 TOT_DC_AMT
                 DCM_SALE_AMT
                 VAT_AMT
                 TOT_AMT
                 OCC_RATE
                 SALE_DATE
                 PAYMENT_METHOD
                 PAY_CNT
                 SALE_AMT
                 DIS_CLS
                 현금 신용카드 외상 회원포인트 상품권 식권
                 */
                resulStruct = new SELLING_STATUS_INFO();
                resulStruct.NO = (i + 1).ToString();
                resulStruct.SCLASS_NAME = DataLocals.PosStatus.SALE_DATE;
                resulStruct.TOT_SALE_QTY = result2[i].TOT_SALE_QTY;
                resulStruct.TOT_SALE_AMT = result2[i].TOT_SALE_AMT;
                resulStruct.TOT_DC_AMT = result2[i].TOT_DC_AMT;
                resulStruct.DCM_SALE_AMT = result2[i].DCM_SALE_AMT.ToString();
                resulStruct.VAT_AMT = result2[i].VAT_AMT.ToString();
                resulStruct.TOT_AMT = result2[i].TOT_AMT.ToString();
                resulStruct.OCC_RATE = result2[i].OCC_RATE.ToString();
                resulStruct.SALE_DATE = DataLocals.PosStatus.SALE_DATE;
                resulStruct.PAYMENT_METHOD = result2[i].PAYMENT_METHOD.ToString();
                resulStruct.PAY_CNT = result2[i].PAY_CNT.ToString();
                resulStruct.SALE_AMT = result2[i].SALE_AMT.ToString();
                resulStruct.DIS_CLS = DataLocals.PosStatus.SALE_DATE;
                resulStruct.PRD_CODE = result2[i].PRD_CODE;
                resulStruct.PRD_NAME = result2[i].PRD_NAME;

                result.Add(resulStruct);
            }



            return DapperORM.ReturnResult(result, EResultType.SUCCESS, "OK");
        }
        catch (FbException ex)
        {
            LogHelper.Logger.Error($"Database Error : {ex.Message.ReplacePlainText()}");
            return DapperORM.ReturnResult(new List<SELLING_STATUS_INFO>(), EResultType.ERROR, ex.Message.ReplacePlainText());
        }
        catch (Exception ex)
        {
            LogHelper.Logger.Error($"Services Error : {ex.Message.ReplacePlainText()}");
            return DapperORM.ReturnResult(new List<SELLING_STATUS_INFO>(), EResultType.ERROR, ex.Message.ReplacePlainText());
        }
    }
    public async Task<(List<SELLING_STATUS_INFO>, SpResult)> GetGoodsSelngSttusDetailList(DynamicParameters param)
    {
        try
        {
            //string SQL = @"                           
            //SELECT *
            //  FROM POS_ORD_M
            //";

            //List<SELLING_STATUS_INFO> result = await DapperORM.ReturnListAsync<SELLING_STATUS_INFO>(SQL, param); // stored procedure
            string SQL = ResourceHelpers.LoadSqlCommand(SQLFileTypes.ProductSalesStatus, "GetDetailProducts");
            await Task.Delay(10);
            List<SELLING_STATUS_INFO> result2 = await DapperORM.ReturnListAsync<SELLING_STATUS_INFO>(SQL,
                param);

            List<SELLING_STATUS_INFO> result = new List<SELLING_STATUS_INFO>();

            if (result == null)
            {
                LogHelper.Logger.Warn($"Database : 조회(DB) 결과가 없습니다.");
                return DapperORM.ReturnResult(new List<SELLING_STATUS_INFO>(), EResultType.EMPTY, "조회(DB) 결과가 없습니다.");
            }

     
            result.Clear();
            SELLING_STATUS_INFO resulStruct = new SELLING_STATUS_INFO();
            for (int i = 0; i < result2.Count(); i++)
            {
                /*
                 
                 NO
                 SCLASS_NAME
                 TOT_SALE_QTY
                 TOT_SALE_AMT
                 TOT_DC_AMT
                 DCM_SALE_AMT
                 VAT_AMT
                 TOT_AMT
                 OCC_RATE
                 SALE_DATE
                 PAYMENT_METHOD
                 PAY_CNT
                 SALE_AMT
                 DIS_CLS
                 현금 신용카드 외상 회원포인트 상품권 식권
                 */
                resulStruct = new SELLING_STATUS_INFO();
                resulStruct.NO = (i + 1).ToString();
                resulStruct.SCLASS_NAME = DataLocals.PosStatus.SALE_DATE;
                resulStruct.TOT_SALE_QTY = result2[i].TOT_SALE_QTY;
                resulStruct.TOT_SALE_AMT = result2[i].TOT_SALE_AMT;
                resulStruct.TOT_DC_AMT = result2[i].TOT_DC_AMT;
                resulStruct.DCM_SALE_AMT = result2[i].DCM_SALE_AMT.ToString();
                resulStruct.VAT_AMT = result2[i].VAT_AMT.ToString();
                resulStruct.TOT_AMT = result2[i].TOT_AMT.ToString();
                resulStruct.OCC_RATE = result2[i].OCC_RATE.ToString();
                resulStruct.SALE_DATE = result2[i].SALE_DATE;
                resulStruct.PAYMENT_METHOD = result2[i].PAYMENT_METHOD.ToString();
                resulStruct.PAY_CNT = result2[i].PAY_CNT.ToString();
                resulStruct.SALE_AMT = result2[i].SALE_AMT.ToString();
                resulStruct.DIS_CLS = DataLocals.PosStatus.SALE_DATE;
                resulStruct.PRD_CODE = result2[i].PRD_CODE;
                resulStruct.PRD_NAME = result2[i].PRD_NAME;


                result.Add(resulStruct);
            }



            return DapperORM.ReturnResult(result, EResultType.SUCCESS, "OK");
        }
        catch (FbException ex)
        {
            LogHelper.Logger.Error($"Database Error : {ex.Message.ReplacePlainText()}");
            return DapperORM.ReturnResult(new List<SELLING_STATUS_INFO>(), EResultType.ERROR, ex.Message.ReplacePlainText());
        }
        catch (Exception ex)
        {
            LogHelper.Logger.Error($"Services Error : {ex.Message.ReplacePlainText()}");
            return DapperORM.ReturnResult(new List<SELLING_STATUS_INFO>(), EResultType.ERROR, ex.Message.ReplacePlainText());
        }
    }
    public async Task<(List<SALE_BY_TYPE2>, SpResult)> GetPaymntSelngSttusMainList(DynamicParameters param)
    {
            try
            {
                string SQL = ResourceHelpers.LoadSqlCommand(SQLFileTypes.ProductSalesStatus, "GetPaymentSellingStats");
                List<SALE_BY_TYPE2> result = await DapperORM.ReturnListAsync<SALE_BY_TYPE2>(SQL,param);

                if (!result.Any())
                {
                    LogHelper.Logger.Warn($"Database : 주문오더 : 왼쪽영역 하단 버튼 셋팅(DB) 결과가 없습니다.");
                    return DapperORM.ReturnResult(new List<SALE_BY_TYPE2>(), EResultType.EMPTY, "조회(DB) 결과(List)가 없습니다.");
                }

                return DapperORM.ReturnResult(result, EResultType.SUCCESS, "OK");
            }
            catch (FbException ex)
            {
                LogHelper.Logger.Error($"Database Error : {ex.Message.ReplacePlainText()}");
                return DapperORM.ReturnResult(new List<SALE_BY_TYPE2>(), EResultType.ERROR, ex.Message.ReplacePlainText());
            }
            catch (Exception ex)
            {
                LogHelper.Logger.Error($"Services Error : {ex.Message.ReplacePlainText()}");
                return DapperORM.ReturnResult(new List<SALE_BY_TYPE2>(), EResultType.ERROR, ex.Message.ReplacePlainText());
            }
    }
    public async Task<(List<SALE_BY_TYPE2>, SpResult)> GetPaymntSelngSttusDetailList(DynamicParameters param)
    {
        try
        {
            string SQL = ResourceHelpers.LoadSqlCommand(SQLFileTypes.ProductSalesStatus, "GetPaymentDetailStats");
            List<SALE_BY_TYPE2> result = await DapperORM.ReturnListAsync<SALE_BY_TYPE2>(SQL, param);

            if (!result.Any())
            {
                LogHelper.Logger.Warn($"Database : 주문오더 : 왼쪽영역 하단 버튼 셋팅(DB) 결과가 없습니다.");
                return DapperORM.ReturnResult(new List<SALE_BY_TYPE2>(), EResultType.EMPTY, "조회(DB) 결과(List)가 없습니다.");
            }

            return DapperORM.ReturnResult(result, EResultType.SUCCESS, "OK");
        }
        catch (FbException ex)
        {
            LogHelper.Logger.Error($"Database Error : {ex.Message.ReplacePlainText()}");
            return DapperORM.ReturnResult(new List<SALE_BY_TYPE2>(), EResultType.ERROR, ex.Message.ReplacePlainText());
        }
        catch (Exception ex)
        {
            LogHelper.Logger.Error($"Services Error : {ex.Message.ReplacePlainText()}");
            return DapperORM.ReturnResult(new List<SALE_BY_TYPE2>(), EResultType.ERROR, ex.Message.ReplacePlainText());
        }
        //try
        //{
        //    await Task.Delay(10);
        //    List<SALE_BY_TYPE2> result = new List<SALE_BY_TYPE2>();

        //    if (result == null)
        //    {
        //        LogHelper.Logger.Warn($"Database : 조회(DB) 결과가 없습니다.");
        //        return DapperORM.ReturnResult(new List<SALE_BY_TYPE2>(), EResultType.EMPTY, "조회(DB) 결과가 없습니다.");
        //    }

        //    //테스트 데이터
        //    Random random = new Random();
        //    int randomNumber = random.Next(10000, 100001);
        //    string[] sPAYMENT_METHOD = new string[] { "현금", "신용카드", "외상", "회원포인트", "상품권", "식권" };
        //    string[] sDIS_CLS = new string[] { "일반할인", "식권할인" };

        //    result.Clear();
        //    SALE_BY_TYPE2 resulStruct = new SALE_BY_TYPE2();
        //    for (int i = 0; i < 20; i++)
        //    {
        //        resulStruct = new SALE_BY_TYPE2();
        //        resulStruct.Qty = DataLocals.PosStatus.SALE_DATE;
        //        resulStruct.Pay_Amt = random.Next(10, 100).ToString();
        //        resulStruct.Pay_Flag = random.Next(10000, 1000000).ToString();
        //        resulStruct.OCC_RATE = random.Next(10000, 1000000).ToString();
        //        resulStruct.SALE_DATE = random.Next(10000, 1000000).ToString();
        //        resulStruct.VAT_AMT = random.Next(10000, 1000000).ToString();
        //        resulStruct.TOT_AMT = random.Next(10000, 1000000).ToString();
        //        resulStruct.OCC_RATE = random.Next(1, 99).ToString();
        //        resulStruct.SALE_DATE = DataLocals.PosStatus.SALE_DATE;
        //        resulStruct.PAYMENT_METHOD = sPAYMENT_METHOD[i % 6];
        //        resulStruct.PAY_CNT = random.Next(1, 200).ToString();
        //        resulStruct.SALE_AMT = random.Next(10000, 1000000).ToString();
        //        resulStruct.DIS_CLS = DataLocals.PosStatus.SALE_DATE;

        //        result.Add(resulStruct);
        //    }

        //    return DapperORM.ReturnResult(result, EResultType.SUCCESS, "OK");
        //}
        //catch (FbException ex)
        //{
        //    LogHelper.Logger.Error($"Database Error : {ex.Message.ReplacePlainText()}");
        //    return DapperORM.ReturnResult(new List<SALE_BY_TYPE2>(), EResultType.ERROR, ex.Message.ReplacePlainText());
        //}
        //catch (Exception ex)
        //{
        //    LogHelper.Logger.Error($"Services Error : {ex.Message.ReplacePlainText()}");
        //    return DapperORM.ReturnResult(new List<SALE_BY_TYPE2>(), EResultType.ERROR, ex.Message.ReplacePlainText());
        //}
    }
    public async Task<(List<SELLING_STATUS_INFO>, SpResult)> GetDscntSelngSttusMainList(DynamicParameters param)
    {
        try
        {
            //string SQL = @"                           
            //SELECT *
            //  FROM POS_ORD_M
            //";

            //List<SELLING_STATUS_INFO> result = await DapperORM.ReturnListAsync<SELLING_STATUS_INFO>(SQL, param); // stored procedure
            await Task.Delay(10);
            List<SELLING_STATUS_INFO> result = new List<SELLING_STATUS_INFO>();

            if (result == null)
            {
                LogHelper.Logger.Warn($"Database : 조회(DB) 결과가 없습니다.");
                return DapperORM.ReturnResult(new List<SELLING_STATUS_INFO>(), EResultType.EMPTY, "조회(DB) 결과가 없습니다.");
            }

            //테스트 데이터
            Random random = new Random();
            int randomNumber = random.Next(10000, 100001);
            string[] sPAYMENT_METHOD = new string[] { "현금", "신용카드", "외상", "회원포인트", "상품권", "식권" };
            string[] sDIS_CLS = new string[] { "일반할인", "식권할인" };

            result.Clear();
            SELLING_STATUS_INFO resulStruct = new SELLING_STATUS_INFO();
            for (int i = 0; i < 20; i++)
            {
                /*
                 
                 NO
                 SCLASS_NAME
                 TOT_SALE_QTY
                 TOT_SALE_AMT
                 TOT_DC_AMT
                 DCM_SALE_AMT
                 VAT_AMT
                 TOT_AMT
                 OCC_RATE
                 SALE_DATE
                 PAYMENT_METHOD
                 PAY_CNT
                 SALE_AMT
                 DIS_CLS
                 현금 신용카드 외상 회원포인트 상품권 식권
                 */
                resulStruct = new SELLING_STATUS_INFO();
                resulStruct.NO = (i + 1).ToString();
                resulStruct.SCLASS_NAME = DataLocals.PosStatus.SALE_DATE;
                resulStruct.TOT_SALE_QTY = random.Next(10, 100).ToString();
                resulStruct.TOT_SALE_AMT = random.Next(10000, 1000000).ToString();
                resulStruct.TOT_DC_AMT = random.Next(10000, 1000000).ToString();
                resulStruct.DCM_SALE_AMT = random.Next(10000, 1000000).ToString();
                resulStruct.VAT_AMT = random.Next(10000, 1000000).ToString();
                resulStruct.TOT_AMT = random.Next(10000, 1000000).ToString();
                resulStruct.OCC_RATE = random.Next(1, 99).ToString();
                resulStruct.SALE_DATE = DataLocals.PosStatus.SALE_DATE;
                resulStruct.PAYMENT_METHOD = sPAYMENT_METHOD[i % 6];
                resulStruct.PAY_CNT = random.Next(1, 200).ToString();
                resulStruct.SALE_AMT = random.Next(10000, 1000000).ToString();
                resulStruct.DIS_CLS = sDIS_CLS[i % 2];

                result.Add(resulStruct);
            }



            return DapperORM.ReturnResult(result, EResultType.SUCCESS, "OK");
        }
        catch (FbException ex)
        {
            LogHelper.Logger.Error($"Database Error : {ex.Message.ReplacePlainText()}");
            return DapperORM.ReturnResult(new List<SELLING_STATUS_INFO>(), EResultType.ERROR, ex.Message.ReplacePlainText());
        }
        catch (Exception ex)
        {
            LogHelper.Logger.Error($"Services Error : {ex.Message.ReplacePlainText()}");
            return DapperORM.ReturnResult(new List<SELLING_STATUS_INFO>(), EResultType.ERROR, ex.Message.ReplacePlainText());
        }
    }
    public async Task<(List<SELLING_STATUS_INFO>, SpResult)> GetDscntSelngSttusDetailList(DynamicParameters param)
    {
        try
        {
            //string SQL = @"                           
            //SELECT *
            //  FROM POS_ORD_M
            //";

            //List<SELLING_STATUS_INFO> result = await DapperORM.ReturnListAsync<SELLING_STATUS_INFO>(SQL, param); // stored procedure
            await Task.Delay(10);
            List<SELLING_STATUS_INFO> result = new List<SELLING_STATUS_INFO>();

            if (result == null)
            {
                LogHelper.Logger.Warn($"Database : 조회(DB) 결과가 없습니다.");
                return DapperORM.ReturnResult(new List<SELLING_STATUS_INFO>(), EResultType.EMPTY, "조회(DB) 결과가 없습니다.");
            }

            //테스트 데이터
            Random random = new Random();
            int randomNumber = random.Next(10000, 100001);
            string[] sPAYMENT_METHOD = new string[] { "현금", "신용카드", "외상", "회원포인트", "상품권", "식권" };
            string[] sDIS_CLS = new string[] { "일반할인", "식권할인" };

            result.Clear();
            SELLING_STATUS_INFO resulStruct = new SELLING_STATUS_INFO();
            for (int i = 0; i < 20; i++)
            {
                /*
                 
                 NO
                 SCLASS_NAME
                 TOT_SALE_QTY
                 TOT_SALE_AMT
                 TOT_DC_AMT
                 DCM_SALE_AMT
                 VAT_AMT
                 TOT_AMT
                 OCC_RATE
                 SALE_DATE
                 PAYMENT_METHOD
                 PAY_CNT
                 SALE_AMT
                 DIS_CLS
                 현금 신용카드 외상 회원포인트 상품권 식권
                 */
                resulStruct = new SELLING_STATUS_INFO();
                resulStruct.NO = (i + 1).ToString();
                resulStruct.SCLASS_NAME = DataLocals.PosStatus.SALE_DATE;
                resulStruct.TOT_SALE_QTY = random.Next(10, 100).ToString();
                resulStruct.TOT_SALE_AMT = random.Next(10000, 1000000).ToString();
                resulStruct.TOT_DC_AMT = random.Next(10000, 1000000).ToString();
                resulStruct.DCM_SALE_AMT = random.Next(10000, 1000000).ToString();
                resulStruct.VAT_AMT = random.Next(10000, 1000000).ToString();
                resulStruct.TOT_AMT = random.Next(10000, 1000000).ToString();
                resulStruct.OCC_RATE = random.Next(1, 99).ToString();
                resulStruct.SALE_DATE = DataLocals.PosStatus.SALE_DATE;
                resulStruct.PAYMENT_METHOD = sPAYMENT_METHOD[i % 6];
                resulStruct.PAY_CNT = random.Next(1, 200).ToString();
                resulStruct.SALE_AMT = random.Next(10000, 1000000).ToString();
                resulStruct.DIS_CLS = DataLocals.PosStatus.SALE_DATE;

                result.Add(resulStruct);
            }



            return DapperORM.ReturnResult(result, EResultType.SUCCESS, "OK");
        }
        catch (FbException ex)
        {
            LogHelper.Logger.Error($"Database Error : {ex.Message.ReplacePlainText()}");
            return DapperORM.ReturnResult(new List<SELLING_STATUS_INFO>(), EResultType.ERROR, ex.Message.ReplacePlainText());
        }
        catch (Exception ex)
        {
            LogHelper.Logger.Error($"Services Error : {ex.Message.ReplacePlainText()}");
            return DapperORM.ReturnResult(new List<SELLING_STATUS_INFO>(), EResultType.ERROR, ex.Message.ReplacePlainText());
        }
    }
    public async Task<(List<SELLING_STATUS_INFO>, SpResult)> GetTimeSelngSttusMainList(DynamicParameters param)
    {
        try
        {
            //string SQL = @"                           
            //SELECT *
            //  FROM POS_ORD_M
            //";

            //List<SELLING_STATUS_INFO> result = await DapperORM.ReturnListAsync<SELLING_STATUS_INFO>(SQL, param); // stored procedure
            await Task.Delay(10);
            List<SELLING_STATUS_INFO> result = new List<SELLING_STATUS_INFO>();

            if (result == null)
            {
                LogHelper.Logger.Warn($"Database : 조회(DB) 결과가 없습니다.");
                return DapperORM.ReturnResult(new List<SELLING_STATUS_INFO>(), EResultType.EMPTY, "조회(DB) 결과가 없습니다.");
            }

            //테스트 데이터
            Random random = new Random();
            int randomNumber = random.Next(10000, 100001);
            string[] sPAYMENT_METHOD = new string[] { "현금", "신용카드", "외상", "회원포인트", "상품권", "식권" };
            string[] sDIS_CLS = new string[] { "일반할인", "식권할인" };
            string[] sTIME_CLS = new string[] { "심야", "아침", "점심", "저녁" };

            result.Clear();
            SELLING_STATUS_INFO resulStruct = new SELLING_STATUS_INFO();
            for (int i = 0; i < 20; i++)
            {
                /*
                 
                 NO
                 SCLASS_NAME
                 TOT_SALE_QTY
                 TOT_SALE_AMT
                 TOT_DC_AMT
                 DCM_SALE_AMT
                 VAT_AMT
                 TOT_AMT
                 OCC_RATE
                 SALE_DATE
                 PAYMENT_METHOD
                 PAY_CNT
                 SALE_AMT
                 DIS_CLS
                 현금 신용카드 외상 회원포인트 상품권 식권
                 */
                resulStruct = new SELLING_STATUS_INFO();
                resulStruct.NO = (i + 1).ToString();
                resulStruct.SCLASS_NAME = DataLocals.PosStatus.SALE_DATE;
                resulStruct.TOT_SALE_QTY = random.Next(10, 100).ToString();
                resulStruct.TOT_SALE_AMT = random.Next(10000, 1000000).ToString();
                resulStruct.TOT_DC_AMT = random.Next(10000, 1000000).ToString();
                resulStruct.DCM_SALE_AMT = random.Next(10000, 1000000).ToString();
                resulStruct.VAT_AMT = random.Next(10000, 1000000).ToString();
                resulStruct.TOT_AMT = random.Next(10000, 1000000).ToString();
                resulStruct.OCC_RATE = random.Next(1, 99).ToString();
                resulStruct.SALE_DATE = DataLocals.PosStatus.SALE_DATE;
                resulStruct.PAYMENT_METHOD = sPAYMENT_METHOD[i % 6];
                resulStruct.PAY_CNT = random.Next(1, 200).ToString();
                resulStruct.SALE_AMT = random.Next(10000, 1000000).ToString();
                resulStruct.DIS_CLS = DataLocals.PosStatus.SALE_DATE;
                resulStruct.TIME_CLS = sTIME_CLS[i % 4];

                result.Add(resulStruct);
            }



            return DapperORM.ReturnResult(result, EResultType.SUCCESS, "OK");
        }
        catch (FbException ex)
        {
            LogHelper.Logger.Error($"Database Error : {ex.Message.ReplacePlainText()}");
            return DapperORM.ReturnResult(new List<SELLING_STATUS_INFO>(), EResultType.ERROR, ex.Message.ReplacePlainText());
        }
        catch (Exception ex)
        {
            LogHelper.Logger.Error($"Services Error : {ex.Message.ReplacePlainText()}");
            return DapperORM.ReturnResult(new List<SELLING_STATUS_INFO>(), EResultType.ERROR, ex.Message.ReplacePlainText());
        }
    }
    public async Task<(List<SELLING_STATUS_INFO>, SpResult)> GetTimeSelngSttusDetailList(DynamicParameters param)
    {
        try
        {
            //string SQL = @"                           
            //SELECT *
            //  FROM POS_ORD_M
            //";

            //List<SELLING_STATUS_INFO> result = await DapperORM.ReturnListAsync<SELLING_STATUS_INFO>(SQL, param); // stored procedure
            await Task.Delay(10);
            List<SELLING_STATUS_INFO> result = new List<SELLING_STATUS_INFO>();

            if (result == null)
            {
                LogHelper.Logger.Warn($"Database : 조회(DB) 결과가 없습니다.");
                return DapperORM.ReturnResult(new List<SELLING_STATUS_INFO>(), EResultType.EMPTY, "조회(DB) 결과가 없습니다.");
            }

            //테스트 데이터
            Random random = new Random();
            int randomNumber = random.Next(10000, 100001);
            string[] sPAYMENT_METHOD = new string[] { "현금", "신용카드", "외상", "회원포인트", "상품권", "식권" };
            string[] sDIS_CLS = new string[] { "일반할인", "식권할인" };

            result.Clear();
            SELLING_STATUS_INFO resulStruct = new SELLING_STATUS_INFO();
            for (int i = 0; i < 20; i++)
            {
                /*
                 
                 NO
                 SCLASS_NAME
                 TOT_SALE_QTY
                 TOT_SALE_AMT
                 TOT_DC_AMT
                 DCM_SALE_AMT
                 VAT_AMT
                 TOT_AMT
                 OCC_RATE
                 SALE_DATE
                 PAYMENT_METHOD
                 PAY_CNT
                 SALE_AMT
                 DIS_CLS
                 현금 신용카드 외상 회원포인트 상품권 식권
                 */
                resulStruct = new SELLING_STATUS_INFO();
                resulStruct.NO = (i + 1).ToString();
                resulStruct.SCLASS_NAME = DataLocals.PosStatus.SALE_DATE;
                resulStruct.TOT_SALE_QTY = random.Next(10, 100).ToString();
                resulStruct.TOT_SALE_AMT = random.Next(10000, 1000000).ToString();
                resulStruct.TOT_DC_AMT = random.Next(10000, 1000000).ToString();
                resulStruct.DCM_SALE_AMT = random.Next(10000, 1000000).ToString();
                resulStruct.VAT_AMT = random.Next(10000, 1000000).ToString();
                resulStruct.TOT_AMT = random.Next(10000, 1000000).ToString();
                resulStruct.OCC_RATE = random.Next(1, 99).ToString();
                resulStruct.SALE_DATE = DataLocals.PosStatus.SALE_DATE;
                resulStruct.PAYMENT_METHOD = sPAYMENT_METHOD[i % 6];
                resulStruct.PAY_CNT = random.Next(1, 200).ToString();
                resulStruct.SALE_AMT = random.Next(10000, 1000000).ToString();
                resulStruct.DIS_CLS = DataLocals.PosStatus.SALE_DATE;

                result.Add(resulStruct);
            }



            return DapperORM.ReturnResult(result, EResultType.SUCCESS, "OK");
        }
        catch (FbException ex)
        {
            LogHelper.Logger.Error($"Database Error : {ex.Message.ReplacePlainText()}");
            return DapperORM.ReturnResult(new List<SELLING_STATUS_INFO>(), EResultType.ERROR, ex.Message.ReplacePlainText());
        }
        catch (Exception ex)
        {
            LogHelper.Logger.Error($"Services Error : {ex.Message.ReplacePlainText()}");
            return DapperORM.ReturnResult(new List<SELLING_STATUS_INFO>(), EResultType.ERROR, ex.Message.ReplacePlainText());
        }
    }

    public async Task<(List<SELLING_APPR>, SpResult)> GetMiddleCardAppr(string? SaleYN, string ResiSeq, string saleDT)
    {
        try
        {
            string SQL = ResourceHelpers.LoadSqlCommand(SQLFileTypes.ProductSalesStatus, "GetMiddleCardAppr");

            var startDate = "";
            var finishDate = "";
            if (!string.IsNullOrEmpty(saleDT))
            {
                var date = saleDT.Split(" ");
                startDate = date[0];
                finishDate = date[1];
            }

            List<SELLING_APPR> result = await DapperORM.ReturnListAsync<SELLING_APPR>(SQL,
                new string[]
                {
                    "@SHOP_CODE" ,
                    "@START_DATE" ,
                    "@FINISH_DATE",
                    "@POS_NO"    ,
                    "@REGI_SEQ"  ,
                    //           "@CLOSE_FLAG",
                    "@SALE_YN"
                },
                new object[]
                {
                    DataLocals.AppConfig.PosInfo.StoreNo,
                    startDate,
                    finishDate,
                    DataLocals.AppConfig.PosInfo.PosNo,
                    ResiSeq,
                    //                ClozeFlak,
                    SaleYN
                });

            if (!result.Any())
            {
                LogHelper.Logger.Warn($"Database : 주문오더 : 왼쪽영역 하단 버튼 셋팅(DB) 결과가 없습니다.");
                return DapperORM.ReturnResult(new List<SELLING_APPR>(), EResultType.EMPTY, "조회(DB) 결과(List)가 없습니다.");
            }

            return DapperORM.ReturnResult(result, EResultType.SUCCESS, "OK");
        }
        catch (FbException ex)
        {
            LogHelper.Logger.Error($"Database Error : {ex.Message.ReplacePlainText()}");
            return DapperORM.ReturnResult(new List<SELLING_APPR>(), EResultType.ERROR, ex.Message.ReplacePlainText());
        }
        catch (Exception ex)
        {
            LogHelper.Logger.Error($"Services Error : {ex.Message.ReplacePlainText()}");
            return DapperORM.ReturnResult(new List<SELLING_APPR>(), EResultType.ERROR, ex.Message.ReplacePlainText());
        }
    }

    public async Task<(List<SELLING_APPR>, SpResult)> GetMiddleCashAppr(string? SaleYN, string ResiSeq, string saleDT)
    {
        try
        {
            string SQL = ResourceHelpers.LoadSqlCommand(SQLFileTypes.ProductSalesStatus, "GetMiddleCashAppr");

            var startDate = "";
            var finishDate = "";
            if (!string.IsNullOrEmpty(saleDT))
            {
                var date = saleDT.Split(" ");
                startDate = date[0];
                finishDate = date[1];
            }
            List<SELLING_APPR> result = await DapperORM.ReturnListAsync<SELLING_APPR>(SQL,
             new string[]
             {
                    "@SHOP_CODE" ,
                    "@START_DATE" ,
                    "@FINISH_DATE",
                    "@POS_NO"    ,
                    "@REGI_SEQ"  ,
                    //           "@CLOSE_FLAG",
                    "@SALE_YN"
             },
             new object[]
             {
                    DataLocals.AppConfig.PosInfo.StoreNo,
                    startDate,
                    finishDate,
                    DataLocals.AppConfig.PosInfo.PosNo,
                    ResiSeq,
                    //                ClozeFlak,
                    SaleYN
             });

            if (!result.Any())
            {
                LogHelper.Logger.Warn($"Database : 주문오더 : 왼쪽영역 하단 버튼 셋팅(DB) 결과가 없습니다.");
                return DapperORM.ReturnResult(new List<SELLING_APPR>(), EResultType.EMPTY, "조회(DB) 결과(List)가 없습니다.");
            }

            return DapperORM.ReturnResult(result, EResultType.SUCCESS, "OK");
        }
        catch (FbException ex)
        {
            LogHelper.Logger.Error($"Database Error : {ex.Message.ReplacePlainText()}");
            return DapperORM.ReturnResult(new List<SELLING_APPR>(), EResultType.ERROR, ex.Message.ReplacePlainText());
        }
        catch (Exception ex)
        {
            LogHelper.Logger.Error($"Services Error : {ex.Message.ReplacePlainText()}");
            return DapperORM.ReturnResult(new List<SELLING_APPR>(), EResultType.ERROR, ex.Message.ReplacePlainText());
        }
    }

    public async Task<(List<SELLING_STATUS_INFO>, SpResult)> GetMtSelngSttusList(DynamicParameters param)
    {
        try
        {
            //string SQL = @"                           
            //SELECT *
            //  FROM POS_ORD_M
            //";

            //List<SELLING_STATUS_INFO> result = await DapperORM.ReturnListAsync<SELLING_STATUS_INFO>(SQL, param); // stored procedure
            await Task.Delay(10);
            List<SELLING_STATUS_INFO> result = new List<SELLING_STATUS_INFO>();

            if (result == null)
            {
                LogHelper.Logger.Warn($"Database : 조회(DB) 결과가 없습니다.");
                return DapperORM.ReturnResult(new List<SELLING_STATUS_INFO>(), EResultType.EMPTY, "조회(DB) 결과가 없습니다.");
            }

            //테스트 데이터
            Random random = new Random();
            int randomNumber = random.Next(10000, 100001);
            string[] sPAYMENT_METHOD = new string[] { "현금", "신용카드", "외상", "회원포인트", "상품권", "식권" };
            string[] sDIS_CLS = new string[] { "일반할인", "식권할인" };

            result.Clear();
            SELLING_STATUS_INFO resulStruct = new SELLING_STATUS_INFO();
            for (int i = 0; i < 20; i++)
            {
                /*
                 
                 NO
                 SCLASS_NAME
                 TOT_SALE_QTY
                 TOT_SALE_AMT
                 TOT_DC_AMT
                 DCM_SALE_AMT
                 VAT_AMT
                 TOT_AMT
                 OCC_RATE
                 SALE_DATE
                 PAYMENT_METHOD
                 PAY_CNT
                 SALE_AMT
                 DIS_CLS
                 현금 신용카드 외상 회원포인트 상품권 식권
                 */
                resulStruct = new SELLING_STATUS_INFO();
                resulStruct.NO = (i + 1).ToString();
                resulStruct.SCLASS_NAME = DataLocals.PosStatus.SALE_DATE;
                resulStruct.TOT_SALE_QTY = random.Next(10, 100).ToString();
                resulStruct.TOT_SALE_AMT = random.Next(10000, 1000000).ToString();
                resulStruct.TOT_DC_AMT = random.Next(10000, 1000000).ToString();
                resulStruct.DCM_SALE_AMT = random.Next(10000, 1000000).ToString();
                resulStruct.VAT_AMT = random.Next(10000, 1000000).ToString();
                resulStruct.TOT_AMT = random.Next(10000, 1000000).ToString();
                resulStruct.OCC_RATE = random.Next(1, 99).ToString();
                resulStruct.SALE_DATE = DataLocals.PosStatus.SALE_DATE;
                resulStruct.PAYMENT_METHOD = sPAYMENT_METHOD[i % 6];
                resulStruct.PAY_CNT = random.Next(1, 200).ToString();
                resulStruct.SALE_AMT = random.Next(10000, 1000000).ToString();
                resulStruct.DIS_CLS = DataLocals.PosStatus.SALE_DATE;

                result.Add(resulStruct);
            }



            return DapperORM.ReturnResult(result, EResultType.SUCCESS, "OK");
        }
        catch (FbException ex)
        {
            LogHelper.Logger.Error($"Database Error : {ex.Message.ReplacePlainText()}");
            return DapperORM.ReturnResult(new List<SELLING_STATUS_INFO>(), EResultType.ERROR, ex.Message.ReplacePlainText());
        }
        catch (Exception ex)
        {
            LogHelper.Logger.Error($"Services Error : {ex.Message.ReplacePlainText()}");
            return DapperORM.ReturnResult(new List<SELLING_STATUS_INFO>(), EResultType.ERROR, ex.Message.ReplacePlainText());
        }
    }
    public async Task<(ExcclcSttusModel, SpResult)> GetExcclcSttusMainList(DynamicParameters param)
    {
        try
        {
            //string SQL = @"                           
            //SELECT *
            //  FROM POS_ORD_M
            //";

            //List<SELLING_STATUS_INFO> result = await DapperORM.ReturnListAsync<SELLING_STATUS_INFO>(SQL, param); // stored procedure

            string SQL = ResourceHelpers.LoadSqlCommand(SQLFileTypes.ProductSalesStatus, "GetExcclcSttus");
          await Task.Delay(10);
            ExcclcSttusModel result2 = await DapperORM.ReturnSingleAsync<ExcclcSttusModel>(SQL,
                param);

            List<SELLING_STATUS_INFO> result = new List<SELLING_STATUS_INFO>();

            if (result == null)
            {
                LogHelper.Logger.Warn($"Database : 조회(DB) 결과가 없습니다.");
                return DapperORM.ReturnResult(new ExcclcSttusModel(), EResultType.EMPTY, "조회(DB) 결과가 없습니다.");
            }

            //테스트 데이터


            result.Clear();
            SELLING_STATUS_INFO resulStruct = new SELLING_STATUS_INFO();
            //for (int i = 0; i < 20; i++)
            //{
            //    /*

            //     NO
            //     SCLASS_NAME
            //     TOT_SALE_QTY
            //     TOT_SALE_AMT
            //     TOT_DC_AMT
            //     DCM_SALE_AMT
            //     VAT_AMT
            //     TOT_AMT
            //     OCC_RATE
            //     SALE_DATE
            //     PAYMENT_METHOD
            //     PAY_CNT
            //     SALE_AMT
            //     DIS_CLS
            //     현금 신용카드 외상 회원포인트 상품권 식권
            //     */
            //    resulStruct = new SELLING_STATUS_INFO();
            //    resulStruct.NO = (i + 1).ToString();
            //    resulStruct.SCLASS_NAME = DataLocals.PosStatus.SALE_DATE;
            //    resulStruct.TOT_SALE_QTY = random.Next(10, 100).ToString();
            //    resulStruct.TOT_SALE_AMT = random.Next(10000, 1000000).ToString();
            //    resulStruct.TOT_DC_AMT = random.Next(10000, 1000000).ToString();
            //    resulStruct.DCM_SALE_AMT = random.Next(10000, 1000000).ToString();
            //    resulStruct.VAT_AMT = random.Next(10000, 1000000).ToString();
            //    resulStruct.TOT_AMT = random.Next(10000, 1000000).ToString();
            //    resulStruct.OCC_RATE = random.Next(1, 99).ToString();
            //    resulStruct.SALE_DATE = DataLocals.PosStatus.SALE_DATE;
            //    resulStruct.PAYMENT_METHOD = sPAYMENT_METHOD[i % 6];
            //    resulStruct.PAY_CNT = random.Next(1, 200).ToString();
            //    resulStruct.SALE_AMT = random.Next(10000, 1000000).ToString();
            //    resulStruct.DIS_CLS = DataLocals.PosStatus.SALE_DATE;

            //    result.Add(resulStruct);
            //}

            if (result2 == null)
            {
                return DapperORM.ReturnResult(new ExcclcSttusModel(), EResultType.EMPTY, null);

            }

            return DapperORM.ReturnResult(result2, EResultType.SUCCESS, "OK");
        }
        catch (FbException ex)
        {
            LogHelper.Logger.Error($"Database Error : {ex.Message.ReplacePlainText()}");
            return DapperORM.ReturnResult(new ExcclcSttusModel(), EResultType.ERROR, ex.Message.ReplacePlainText());
        }
        catch (Exception ex)
        {
            LogHelper.Logger.Error($"Services Error : {ex.Message.ReplacePlainText()}");
            return DapperORM.ReturnResult(new ExcclcSttusModel(), EResultType.ERROR, ex.Message.ReplacePlainText());
        }
        //      return DapperORM.ReturnResult(new List<SELLING_STATUS_INFO>(), EResultType.ERROR, null);
    }
    public async Task<(List<SELLING_STATUS_INFO>, SpResult)> GetExcclcSttusDetailList(DynamicParameters param)
    {
        try
        {
            //string SQL = @"                           
            //SELECT *
            //  FROM POS_ORD_M
            //";

            //List<SELLING_STATUS_INFO> result = await DapperORM.ReturnListAsync<SELLING_STATUS_INFO>(SQL, param); // stored procedure
            await Task.Delay(10);
            List<SELLING_STATUS_INFO> result = new List<SELLING_STATUS_INFO>();

            if (result == null)
            {
                LogHelper.Logger.Warn($"Database : 조회(DB) 결과가 없습니다.");
                return DapperORM.ReturnResult(new List<SELLING_STATUS_INFO>(), EResultType.EMPTY, "조회(DB) 결과가 없습니다.");
            }

            //테스트 데이터
            Random random = new Random();
            int randomNumber = random.Next(10000, 100001);
            string[] sPAYMENT_METHOD = new string[] { "현금", "신용카드", "외상", "회원포인트", "상품권", "식권" };
            string[] sDIS_CLS = new string[] { "일반할인", "식권할인" };

            result.Clear();
            SELLING_STATUS_INFO resulStruct = new SELLING_STATUS_INFO();
            for (int i = 0; i < 20; i++)
            {
                /*
                 
                 NO
                 SCLASS_NAME
                 TOT_SALE_QTY
                 TOT_SALE_AMT
                 TOT_DC_AMT
                 DCM_SALE_AMT
                 VAT_AMT
                 TOT_AMT
                 OCC_RATE
                 SALE_DATE
                 PAYMENT_METHOD
                 PAY_CNT
                 SALE_AMT
                 DIS_CLS
                 현금 신용카드 외상 회원포인트 상품권 식권
                 */
                resulStruct = new SELLING_STATUS_INFO();
                resulStruct.NO = (i + 1).ToString();
                resulStruct.SCLASS_NAME = DataLocals.PosStatus.SALE_DATE;
                resulStruct.TOT_SALE_QTY = random.Next(10, 100).ToString();
                resulStruct.TOT_SALE_AMT = random.Next(10000, 1000000).ToString();
                resulStruct.TOT_DC_AMT = random.Next(10000, 1000000).ToString();
                resulStruct.DCM_SALE_AMT = random.Next(10000, 1000000).ToString();
                resulStruct.VAT_AMT = random.Next(10000, 1000000).ToString();
                resulStruct.TOT_AMT = random.Next(10000, 1000000).ToString();
                resulStruct.OCC_RATE = random.Next(1, 99).ToString();
                //resulStruct.SALE_DATE = DataLocals.PosStatus.SALE_DATE;
                resulStruct.PAYMENT_METHOD = sPAYMENT_METHOD[i % 6];
                resulStruct.PAY_CNT = random.Next(1, 200).ToString();
                resulStruct.SALE_AMT = random.Next(10000, 1000000).ToString();
                resulStruct.DIS_CLS = DataLocals.PosStatus.SALE_DATE;

                result.Add(resulStruct);
            }



            return DapperORM.ReturnResult(result, EResultType.SUCCESS, "OK");
        }
        catch (FbException ex)
        {
            LogHelper.Logger.Error($"Database Error : {ex.Message.ReplacePlainText()}");
            return DapperORM.ReturnResult(new List<SELLING_STATUS_INFO>(), EResultType.ERROR, ex.Message.ReplacePlainText());
        }
        catch (Exception ex)
        {
            LogHelper.Logger.Error($"Services Error : {ex.Message.ReplacePlainText()}");
            return DapperORM.ReturnResult(new List<SELLING_STATUS_INFO>(), EResultType.ERROR, ex.Message.ReplacePlainText());
        }
    }

    public async Task<(List<FINAL_SETT>, SpResult)> GetFinalClosing(DynamicParameters param)
    {
        string SQL = ResourceHelpers.LoadSqlCommand(SQLFileTypes.ProductSalesStatus, "GetDaySettle");
        List<FINAL_SETT> result = await DapperORM.ReturnListAsync<FINAL_SETT>(SQL, param);

        if (!result.Any())
        {
            LogHelper.Logger.Warn($"Database : 주문오더 : 왼쪽영역 하단 버튼 셋팅(DB) 결과가 없습니다.");
            return DapperORM.ReturnResult(new List<FINAL_SETT>(), EResultType.EMPTY, "조회(DB) 결과(List)가 없습니다.");
        }

        return DapperORM.ReturnResult(result, EResultType.SUCCESS, "OK");
    }

    public async Task<(GRAPH_SELNG_STTUS, SpResult)> GetMonthlyData(DynamicParameters param)
    {
        try
        {
            //string SQL = @"                           
            //SELECT *
            //  FROM POS_ORD_M
            //";

            //List<SELLING_STATUS_INFO> result = await DapperORM.ReturnListAsync<SELLING_STATUS_INFO>(SQL, param); // stored procedure

            string SQL = ResourceHelpers.LoadSqlCommand(SQLFileTypes.ProductSalesStatus, "GetMonthlyData");
            await Task.Delay(10);
            GRAPH_SELNG_STTUS result2 = await DapperORM.ReturnSingleAsync<GRAPH_SELNG_STTUS>(SQL,
                param);

            GRAPH_SELNG_STTUS result = new GRAPH_SELNG_STTUS();

            if (result == null)
            {
                LogHelper.Logger.Warn($"Database : 조회(DB) 결과가 없습니다.");
                return DapperORM.ReturnResult(new GRAPH_SELNG_STTUS(), EResultType.EMPTY, "조회(DB) 결과가 없습니다.");
            }

            //테스트 데이터


            SELLING_STATUS_INFO resulStruct = new SELLING_STATUS_INFO();
        

            if (result2 == null)
            {
                return DapperORM.ReturnResult(new GRAPH_SELNG_STTUS(), EResultType.EMPTY, null);

            }

            return DapperORM.ReturnResult(result2, EResultType.SUCCESS, "OK");
        }
        catch (FbException ex)
        {
            LogHelper.Logger.Error($"Database Error : {ex.Message.ReplacePlainText()}");
            return DapperORM.ReturnResult(new GRAPH_SELNG_STTUS(), EResultType.ERROR, ex.Message.ReplacePlainText());
        }
        catch (Exception ex)
        {
            LogHelper.Logger.Error($"Services Error : {ex.Message.ReplacePlainText()}");
            return DapperORM.ReturnResult(new GRAPH_SELNG_STTUS(), EResultType.ERROR, ex.Message.ReplacePlainText());
        }
        //      return DapperORM.ReturnResult(new List<SELLING_STATUS_INFO>(), EResultType.ERROR, null);
    }

    public async Task<(GRAPH_SELNG_STTUS, SpResult)> GetDayData(DynamicParameters param)
    {
        try
        {
            //string SQL = @"                           
            //SELECT *
            //  FROM POS_ORD_M
            //";

            //List<SELLING_STATUS_INFO> result = await DapperORM.ReturnListAsync<SELLING_STATUS_INFO>(SQL, param); // stored procedure

            string SQL = ResourceHelpers.LoadSqlCommand(SQLFileTypes.ProductSalesStatus, "GetDayData");
            await Task.Delay(10);
            GRAPH_SELNG_STTUS result2 = await DapperORM.ReturnSingleAsync<GRAPH_SELNG_STTUS>(SQL,
                param);

            GRAPH_SELNG_STTUS result = new GRAPH_SELNG_STTUS();

            if (result == null)
            {
                LogHelper.Logger.Warn($"Database : 조회(DB) 결과가 없습니다.");
                return DapperORM.ReturnResult(new GRAPH_SELNG_STTUS(), EResultType.EMPTY, "조회(DB) 결과가 없습니다.");
            }

            //테스트 데이터


            SELLING_STATUS_INFO resulStruct = new SELLING_STATUS_INFO();


            if (result2 == null)
            {
                return DapperORM.ReturnResult(new GRAPH_SELNG_STTUS(), EResultType.EMPTY, null);

            }

            return DapperORM.ReturnResult(result2, EResultType.SUCCESS, "OK");
        }
        catch (FbException ex)
        {
            LogHelper.Logger.Error($"Database Error : {ex.Message.ReplacePlainText()}");
            return DapperORM.ReturnResult(new GRAPH_SELNG_STTUS(), EResultType.ERROR, ex.Message.ReplacePlainText());
        }
        catch (Exception ex)
        {
            LogHelper.Logger.Error($"Services Error : {ex.Message.ReplacePlainText()}");
            return DapperORM.ReturnResult(new GRAPH_SELNG_STTUS(), EResultType.ERROR, ex.Message.ReplacePlainText());
        }
    }

    public async Task<(GRAPH_SELNG_STTUS, SpResult)> GetClassData(DynamicParameters param)
    {
        try
        {
            //string SQL = @"                           
            //SELECT *
            //  FROM POS_ORD_M
            //";

            //List<SELLING_STATUS_INFO> result = await DapperORM.ReturnListAsync<SELLING_STATUS_INFO>(SQL, param); // stored procedure

            string SQL = ResourceHelpers.LoadSqlCommand(SQLFileTypes.ProductSalesStatus, "GetClassData");
            await Task.Delay(10);
            GRAPH_SELNG_STTUS result2 = await DapperORM.ReturnSingleAsync<GRAPH_SELNG_STTUS>(SQL,
                param);

            GRAPH_SELNG_STTUS result = new GRAPH_SELNG_STTUS();

            if (result == null)
            {
                LogHelper.Logger.Warn($"Database : 조회(DB) 결과가 없습니다.");
                return DapperORM.ReturnResult(new GRAPH_SELNG_STTUS(), EResultType.EMPTY, "조회(DB) 결과가 없습니다.");
            }

            //테스트 데이터


            SELLING_STATUS_INFO resulStruct = new SELLING_STATUS_INFO();


            if (result2 == null)
            {
                return DapperORM.ReturnResult(new GRAPH_SELNG_STTUS(), EResultType.EMPTY, null);

            }

            return DapperORM.ReturnResult(result2, EResultType.SUCCESS, "OK");
        }
        catch (FbException ex)
        {
            LogHelper.Logger.Error($"Database Error : {ex.Message.ReplacePlainText()}");
            return DapperORM.ReturnResult(new GRAPH_SELNG_STTUS(), EResultType.ERROR, ex.Message.ReplacePlainText());
        }
        catch (Exception ex)
        {
            LogHelper.Logger.Error($"Services Error : {ex.Message.ReplacePlainText()}");
            return DapperORM.ReturnResult(new GRAPH_SELNG_STTUS(), EResultType.ERROR, ex.Message.ReplacePlainText());
        }
    }

    public async Task<(GRAPH_SELNG_STTUS, SpResult)> GetWeekData(DynamicParameters param)
    {
        try
        {
            //string SQL = @"                           
            //SELECT *
            //  FROM POS_ORD_M
            //";

            //List<SELLING_STATUS_INFO> result = await DapperORM.ReturnListAsync<SELLING_STATUS_INFO>(SQL, param); // stored procedure

            string SQL = ResourceHelpers.LoadSqlCommand(SQLFileTypes.ProductSalesStatus, "GetWeekData");
            await Task.Delay(10);
            GRAPH_SELNG_STTUS result2 = await DapperORM.ReturnSingleAsync<GRAPH_SELNG_STTUS>(SQL,
                param);

            GRAPH_SELNG_STTUS result = new GRAPH_SELNG_STTUS();

            if (result == null)
            {
                LogHelper.Logger.Warn($"Database : 조회(DB) 결과가 없습니다.");
                return DapperORM.ReturnResult(new GRAPH_SELNG_STTUS(), EResultType.EMPTY, "조회(DB) 결과가 없습니다.");
            }

            //테스트 데이터


            SELLING_STATUS_INFO resulStruct = new SELLING_STATUS_INFO();


            if (result2 == null)
            {
                return DapperORM.ReturnResult(new GRAPH_SELNG_STTUS(), EResultType.EMPTY, null);

            }

            return DapperORM.ReturnResult(result2, EResultType.SUCCESS, "OK");
        }
        catch (FbException ex)
        {
            LogHelper.Logger.Error($"Database Error : {ex.Message.ReplacePlainText()}");
            return DapperORM.ReturnResult(new GRAPH_SELNG_STTUS(), EResultType.ERROR, ex.Message.ReplacePlainText());
        }
        catch (Exception ex)
        {
            LogHelper.Logger.Error($"Services Error : {ex.Message.ReplacePlainText()}");
            return DapperORM.ReturnResult(new GRAPH_SELNG_STTUS(), EResultType.ERROR, ex.Message.ReplacePlainText());
        }
    }

    public async Task<(List<CR_CARD_SALE>, SpResult)> GetCardDistinct(string ResiSeq, /*string ClozeFlak,*/ string txtDateFrom, string txtDateTo)
    {
        try
        {
            string SQL = ResourceHelpers.LoadSqlCommand(SQLFileTypes.ProductSalesStatus, "GetCardDistinct");


            List<CR_CARD_SALE> result = await DapperORM.ReturnListAsync<CR_CARD_SALE>(SQL,
                new string[]
                {
                    "@SHOP_CODE" ,
                                  "@REGI_SEQ",
                    //             "@CLOSE_FLAG"
                     "@START_DATE" ,
                    "@FINISH_DATE"
                },
                new object[]
                {
                    DataLocals.AppConfig.PosInfo.StoreNo,
                    ResiSeq,
                    //              ClozeFlak
                    txtDateFrom,
                    txtDateTo
                });

            if (!result.Any())
            {
                LogHelper.Logger.Warn($"Database : 주문오더 : 왼쪽영역 하단 버튼 셋팅(DB) 결과가 없습니다.");
                return DapperORM.ReturnResult(new List<CR_CARD_SALE>(), EResultType.EMPTY, "조회(DB) 결과(List)가 없습니다.");
            }

            return DapperORM.ReturnResult(result, EResultType.SUCCESS, "OK");
        } catch (FbException ex)
        {
            LogHelper.Logger.Error($"Database Error : {ex.Message.ReplacePlainText()}");
            return DapperORM.ReturnResult(new List<CR_CARD_SALE>(), EResultType.ERROR, ex.Message.ReplacePlainText());
        } catch (Exception ex)
        {
            LogHelper.Logger.Error($"Services Error : {ex.Message.ReplacePlainText()}");
            return DapperORM.ReturnResult(new List<CR_CARD_SALE>(), EResultType.ERROR, ex.Message.ReplacePlainText());
        }
    }

    public async Task<(List<SALES_GIFT_SALE2>, SpResult)> GetMiddleGiftCard1(string? SaleYN, string ResiSeq, string saleDT)
    {
        try
        {
            string SQL = ResourceHelpers.LoadSqlCommand(SQLFileTypes.ProductSalesStatus, "GetMiddleGiftCard1");


            var startDate = "";
            var finishDate = "";
            if (!string.IsNullOrEmpty(saleDT))
            {
                var date = saleDT.Split(" ");
                startDate = date[0];
                finishDate = date[1];
            }

            List<SALES_GIFT_SALE2> result = await DapperORM.ReturnListAsync<SALES_GIFT_SALE2>(SQL,
                new string[]
                {
                    "@SHOP_CODE" ,
                    "@START_DATE" ,
                    "@FINISH_DATE",
                    "@POS_NO"    ,
                    "@REGI_SEQ"  ,
                    //           "@CLOSE_FLAG",
                    "@SALE_YN"
                },
                new object[]
                {
                    DataLocals.AppConfig.PosInfo.StoreNo,
                    startDate,
                    finishDate,
                    DataLocals.AppConfig.PosInfo.PosNo,
                    ResiSeq,
                    //                ClozeFlak,
                    SaleYN
                });

            if (!result.Any())
            {
                LogHelper.Logger.Warn($"Database : 주문오더 : 왼쪽영역 하단 버튼 셋팅(DB) 결과가 없습니다.");
                return DapperORM.ReturnResult(new List<SALES_GIFT_SALE2>(), EResultType.EMPTY, "조회(DB) 결과(List)가 없습니다.");
            }

            return DapperORM.ReturnResult(result, EResultType.SUCCESS, "OK");
        }
        catch (FbException ex)
        {
            LogHelper.Logger.Error($"Database Error : {ex.Message.ReplacePlainText()}");
            return DapperORM.ReturnResult(new List<SALES_GIFT_SALE2>(), EResultType.ERROR, ex.Message.ReplacePlainText());
        }
    }

    public async Task<(List<SALES_GIFT_SALE2>, SpResult)> GetMiddleGiftCard2(string? SaleYN, string ResiSeq, string saleDT)
    {
        try
        {
            string SQL = ResourceHelpers.LoadSqlCommand(SQLFileTypes.ProductSalesStatus, "GetMiddleGiftCard2");


            var startDate = "";
            var finishDate = "";
            if (!string.IsNullOrEmpty(saleDT))
            {
                var date = saleDT.Split(" ");
                startDate = date[0];
                finishDate = date[1];
            }

            List<SALES_GIFT_SALE2> result = await DapperORM.ReturnListAsync<SALES_GIFT_SALE2>(SQL,
                new string[]
                {
                    "@SHOP_CODE" ,
                    "@START_DATE" ,
                    "@FINISH_DATE",
                    "@POS_NO"    ,
                    "@REGI_SEQ"  ,
                    //           "@CLOSE_FLAG",
                    "@SALE_YN"
                },
                new object[]
                {
                    DataLocals.AppConfig.PosInfo.StoreNo,
                    startDate,
                    finishDate,
                    DataLocals.AppConfig.PosInfo.PosNo,
                    ResiSeq,
                    //                ClozeFlak,
                    SaleYN
                });

            if (!result.Any())
            {
                LogHelper.Logger.Warn($"Database : 주문오더 : 왼쪽영역 하단 버튼 셋팅(DB) 결과가 없습니다.");
                return DapperORM.ReturnResult(new List<SALES_GIFT_SALE2>(), EResultType.EMPTY, "조회(DB) 결과(List)가 없습니다.");
            }

            return DapperORM.ReturnResult(result, EResultType.SUCCESS, "OK");
        }
        catch (FbException ex)
        {
            LogHelper.Logger.Error($"Database Error : {ex.Message.ReplacePlainText()}");
            return DapperORM.ReturnResult(new List<SALES_GIFT_SALE2>(), EResultType.ERROR, ex.Message.ReplacePlainText());
        }
    }

    public async Task<(List<SELLING_STATUS_INFO>, SpResult)> GetMainDetailList(DynamicParameters param)
    {
        return DapperORM.ReturnResult(new List<SELLING_STATUS_INFO>(), EResultType.ERROR, "");
    }

    public Task<(List<CR_CARD_SALE>, SpResult)> GetCardDistinct(string ResiSeq, string saleDT)
    {
        throw new NotImplementedException();
    }
}
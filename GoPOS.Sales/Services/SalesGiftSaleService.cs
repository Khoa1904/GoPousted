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
using static System.Data.Entity.Infrastructure.Design.Executor;

namespace GoPOS.Services;

/// <summary>
/// 작성자: 김형석 
/// 화면명: 코너별 매출현황 - 310 매출현황
/// </summary>

public class SalesGiftSaleService : ISalesGiftSaleService
{
    #region SalesGiftSaleService

    public async Task<(List<SALES_GIFT_SALE>, SpResult)> GetList1(DynamicParameters param)
    {
        try
        {
            //코너명 총수량 총매출 총할인 실매출 부가세 합계 점유율
            //CORNER_CODE TOT_SALE_QTY    TOT_SALE_AMT DC_AMT  DCM_SALE_AMT VAT_AMT TOT_AMT OCC_RATE

            string SQL = @"";
            //List<SALES_GIFT_SALE> result = await DapperORM.ReturnListAsync<SALES_GIFT_SALE>(SQL, param);
            List<SALES_GIFT_SALE> result = new List<SALES_GIFT_SALE>();

            //상품권판매 NO
            //상품권명 TK_GFT_CODE
            //액면가 TK_GFT_UPRC
            //단가 상품권-TK_GFT_UAMT
            //할인 DC_RATE
            //금액 TOT_TK_GFT_UAMT
            //비고REMARK

            for (int i = 0; i < 20; i++)
            {
                string dt = DateTime.Now.ToString("yyyyMMdd");
                result.Add(new SALES_GIFT_SALE() { NO = (i + 1).ToString(), TK_GFT_CODE = "금액할인 상품권_" + (i + 1).ToString(), TK_GFT_UPRC = (i + 1).ToString() + "0,000", TK_GFT_UAMT = (i + 1).ToString() + "0,000", DC_RATE = (i + 1).ToString() + "%", TOT_TK_GFT_UAMT = (i + 1).ToString() + ",500", REMARK = "비고_" + (i + 1) });
            }
            return DapperORM.ReturnResult(result, EResultType.SUCCESS, "OK");

        }
        catch (FbException ex)
        {
            LogHelper.Logger.Error($"Database Error : {ex.Message.ReplacePlainText()}");
            return DapperORM.ReturnResult(new List<SALES_GIFT_SALE>(), EResultType.ERROR, ex.Message.ReplacePlainText());
        }
        catch (Exception ex)
        {
            LogHelper.Logger.Error($"Services Error : {ex.Message.ReplacePlainText()}");
            return DapperORM.ReturnResult(new List<SALES_GIFT_SALE>(), EResultType.ERROR, ex.Message.ReplacePlainText());
        }
    }

    public async Task<(List<SALES_GIFT_SALE>, SpResult)> GetList2(DynamicParameters param)
    {
        try
        {
            //코너명 총수량 총매출 총할인 실매출 부가세 합계 점유율
            //CORNER_CODE TOT_SALE_QTY    TOT_SALE_AMT DC_AMT  DCM_SALE_AMT VAT_AMT TOT_AMT OCC_RATE

            string SQL = @"";
            //List<SALES_GIFT_SALE> result = await DapperORM.ReturnListAsync<SALES_GIFT_SALE>(SQL, param);
            List<SALES_GIFT_SALE> result = new List<SALES_GIFT_SALE>();


            for (int i = 0; i < 20; i++)
            {
                string dt = DateTime.Now.ToString("yyyyMMdd");
                result.Add(new SALES_GIFT_SALE() { NO = (i + 1).ToString(), TK_GFT_CODE = "상품권명_" + (i + 1).ToString(), TK_GFT_UPRC = (i + 1).ToString() + "0,000", TK_GFT_UAMT = (i + 1).ToString() + "0,000", DC_RATE = (i + 1).ToString() + "%", TOT_TK_GFT_UAMT = (i + 1).ToString() + ",500", REMARK = "비고_" + (i + 1) });
            }
            return DapperORM.ReturnResult(result, EResultType.SUCCESS, "OK");

        }
        catch (FbException ex)
        {
            LogHelper.Logger.Error($"Database Error : {ex.Message.ReplacePlainText()}");
            return DapperORM.ReturnResult(new List<SALES_GIFT_SALE>(), EResultType.ERROR, ex.Message.ReplacePlainText());
        }
        catch (Exception ex)
        {
            LogHelper.Logger.Error($"Services Error : {ex.Message.ReplacePlainText()}");
            return DapperORM.ReturnResult(new List<SALES_GIFT_SALE>(), EResultType.ERROR, ex.Message.ReplacePlainText());
        }
    }

    public async Task<(List<SALES_GIFT_SALE>, SpResult)> GetList3(DynamicParameters param, int iIndex)
    {
        try
        {

            string SQL = @"";
            //List<SALES_GIFT_SALE> result = await DapperORM.ReturnListAsync<SALES_GIFT_SALE>(SQL, param);
            List<SALES_GIFT_SALE> result = new List<SALES_GIFT_SALE>();

            int i = iIndex;
            result.Add(new SALES_GIFT_SALE() { NO = (i + 1).ToString(), TK_GFT_CODE = "상품권명_" + (i + 1).ToString(), TK_GFT_UPRC = (i + 1).ToString() + "0,000", TK_GFT_UAMT = (i + 1).ToString() + "0,000", DC_RATE = (i + 1).ToString() + "%", TOT_TK_GFT_UAMT = (i + 1).ToString() + ",500", REMARK = "비고_" + (i + 1) });

            return DapperORM.ReturnResult(result, EResultType.SUCCESS, "OK");

        }
        catch (FbException ex)
        {
            LogHelper.Logger.Error($"Database Error : {ex.Message.ReplacePlainText()}");
            return DapperORM.ReturnResult(new List<SALES_GIFT_SALE>(), EResultType.ERROR, ex.Message.ReplacePlainText());
        }
        catch (Exception ex)
        {
            LogHelper.Logger.Error($"Services Error : {ex.Message.ReplacePlainText()}");
            return DapperORM.ReturnResult(new List<SALES_GIFT_SALE>(), EResultType.ERROR, ex.Message.ReplacePlainText());
        }
    }

    public async Task<(List<SALES_APPR>, SpResult)> GetMiddleCardAppr(string? SaleYN, string ResiSeq, /*string ClozeFlak,*/ string saleDT)
    {
        try
        {
            string SQL = ResourceHelpers.LoadSqlCommand(SQLFileTypes.Sales, "GetMiddleCardAppr");
            List<SALES_APPR> result = await DapperORM.ReturnListAsync<SALES_APPR>(SQL,
                new string[]
                {
                    "@SHOP_CODE" ,
                    "@SALE_DATE" ,
                    "@POS_NO"    ,
                    "@REGI_SEQ"  ,
         //           "@CLOSE_FLAG",
                    "@SALE_YN"
                },
                new object[]
                {
                    DataLocals.AppConfig.PosInfo.StoreNo,
                    saleDT,
                    DataLocals.AppConfig.PosInfo.PosNo,
                    ResiSeq,
    //                ClozeFlak,
                    SaleYN
                });

            if (!result.Any())
            {
                LogHelper.Logger.Warn($"Database : 주문오더 : 왼쪽영역 하단 버튼 셋팅(DB) 결과가 없습니다.");
                return DapperORM.ReturnResult(new List<SALES_APPR>(), EResultType.EMPTY, "조회(DB) 결과(List)가 없습니다.");
            }

            return DapperORM.ReturnResult(result, EResultType.SUCCESS, "OK");
        }
        catch (FbException ex)
        {
            LogHelper.Logger.Error($"Database Error : {ex.Message.ReplacePlainText()}");
            return DapperORM.ReturnResult(new List<SALES_APPR>(), EResultType.ERROR, ex.Message.ReplacePlainText());
        }
        catch (Exception ex)
        {
            LogHelper.Logger.Error($"Services Error : {ex.Message.ReplacePlainText()}");
            return DapperORM.ReturnResult(new List<SALES_APPR>(), EResultType.ERROR, ex.Message.ReplacePlainText());
        }
    }

    public async Task<(List<SALES_APPR>, SpResult)> GetMiddleCashAppr(string? SaleYN, string ResiSeq, /*string ClozeFlak,*/ string saleDT)
    {
        try
        {
            string SQL = ResourceHelpers.LoadSqlCommand(SQLFileTypes.Sales, "GetMiddleCashAppr");
            List<SALES_APPR> result = await DapperORM.ReturnListAsync<SALES_APPR>(SQL,
                new string[]
                {
                    "@SHOP_CODE" ,
                    "@SALE_DATE" ,
                    "@POS_NO"    ,
                    "@REGI_SEQ"  ,
        //            "@CLOSE_FLAG",
                    "@SALE_YN"
                },
                new object[]
                {
                    DataLocals.AppConfig.PosInfo.StoreNo,
                    saleDT,
                    DataLocals.AppConfig.PosInfo.PosNo,
                    ResiSeq,
          //          ClozeFlak,
                    SaleYN
                });

            if (!result.Any())
            {
                LogHelper.Logger.Warn($"Database : 주문오더 : 왼쪽영역 하단 버튼 셋팅(DB) 결과가 없습니다.");
                return DapperORM.ReturnResult(new List<SALES_APPR>(), EResultType.EMPTY, "조회(DB) 결과(List)가 없습니다.");
            }

            return DapperORM.ReturnResult(result, EResultType.SUCCESS, "OK");
        }
        catch (FbException ex)
        {
            LogHelper.Logger.Error($"Database Error : {ex.Message.ReplacePlainText()}");
            return DapperORM.ReturnResult(new List<SALES_APPR>(), EResultType.ERROR, ex.Message.ReplacePlainText());
        }
        catch (Exception ex)
        {
            LogHelper.Logger.Error($"Services Error : {ex.Message.ReplacePlainText()}");
            return DapperORM.ReturnResult(new List<SALES_APPR>(), EResultType.ERROR, ex.Message.ReplacePlainText());
        }
    }

    #endregion

    #region popup for middle & closing settlement

    public async Task<(List<SALES_GIFT_SALE2>, SpResult)> GetMiddleGiftCard1(string SaleYN, string ResiSeq, /*string ClozeFlak,*/ string saleDT)
    {

        try
        {
            string SQL = ResourceHelpers.LoadSqlCommand(SQLFileTypes.Sales, "GetMiddleGiftCard1");
            List<SALES_GIFT_SALE2> result = await DapperORM.ReturnListAsync<SALES_GIFT_SALE2>(SQL,
                new string[]
                {
                    "@SHOP_CODE" ,
                    "@SALE_DATE" ,
                    "@POS_NO"    ,
                    "@REGI_SEQ"  ,
             //       "@CLOSE_FLAG",
                    "@SALE_YN"
                },
                new object[]
                {
                    DataLocals.AppConfig.PosInfo.StoreNo,
                    saleDT,
                    DataLocals.AppConfig.PosInfo.PosNo,
                    ResiSeq,
                //    ClozeFlak,
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
        catch (Exception ex)
        {
            LogHelper.Logger.Error($"Services Error : {ex.Message.ReplacePlainText()}");
            return DapperORM.ReturnResult(new List<SALES_GIFT_SALE2>(), EResultType.ERROR, ex.Message.ReplacePlainText());
        }
    }

    public async Task<(List<SALES_GIFT_SALE2>, SpResult)> GetMiddleGiftCard2(string SaleYN, string ResiSeq, /*string ClozeFlak,*/ string saleDT)
    {

        try
        {
            string SQL = ResourceHelpers.LoadSqlCommand(SQLFileTypes.Sales, "GetMiddleGiftCard2");
            List<SALES_GIFT_SALE2> result = await DapperORM.ReturnListAsync<SALES_GIFT_SALE2>(SQL,
                new string[]
                {
                    "@SHOP_CODE" ,
                    "@SALE_DATE" ,
                    "@POS_NO"    ,
                    "@REGI_SEQ"  ,
           //         "@CLOSE_FLAG",
                    "@SALE_YN"
                },
                new object[]
                {
                    DataLocals.AppConfig.PosInfo.StoreNo,
                    saleDT,
                    DataLocals.AppConfig.PosInfo.PosNo,
                    ResiSeq,
            //        ClozeFlak,
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
        catch (Exception ex)
        {
            LogHelper.Logger.Error($"Services Error : {ex.Message.ReplacePlainText()}");
            return DapperORM.ReturnResult(new List<SALES_GIFT_SALE2>(), EResultType.ERROR, ex.Message.ReplacePlainText());
        }        
    }

    public async Task<(List<FINAL_SETT>, SpResult)> GetFinalClosing(string ResiSeq, /*string ClozeFlak,*/ string saleDT)
    {
        try
        {
            string SQL = ResourceHelpers.LoadSqlCommand(SQLFileTypes.Sales, "GetTodaySettle");
            List<FINAL_SETT> result = await DapperORM.ReturnListAsync<FINAL_SETT>(SQL,
                new string[]
                {
                    "@SHOP_CODE" ,
                    "@SALE_DATE" ,
                    "@POS_NO"    
           //         "@REGI_SEQ"  
          //          "@CLOSE_FLAG"
                },
                new object[]
                {
                    DataLocals.AppConfig.PosInfo.StoreNo,
                    saleDT,
                    DataLocals.AppConfig.PosInfo.PosNo
          //          ResiSeq
         //           ClozeFlak
                });

            if (!result.Any())
            {
                LogHelper.Logger.Warn($"Database : 주문오더 : 왼쪽영역 하단 버튼 셋팅(DB) 결과가 없습니다.");
                return DapperORM.ReturnResult(new List<FINAL_SETT>(), EResultType.EMPTY, "조회(DB) 결과(List)가 없습니다.");
            }

            return DapperORM.ReturnResult(result, EResultType.SUCCESS, "OK");
        }
        catch (FbException ex)
        {
            LogHelper.Logger.Error($"Database Error : {ex.Message.ReplacePlainText()}");
            return DapperORM.ReturnResult(new List<FINAL_SETT>(), EResultType.ERROR, ex.Message.ReplacePlainText());
        }
        catch (Exception ex)
        {
            LogHelper.Logger.Error($"Services Error : {ex.Message.ReplacePlainText()}");
            return DapperORM.ReturnResult(new List<FINAL_SETT>(), EResultType.ERROR, ex.Message.ReplacePlainText());
        }
    }

    public async Task<(List<CR_CARD_SALE>, SpResult)> GetCardDistinct(string ResiSeq, /*string ClozeFlak,*/ string saleDT)
    {
        try
        {
            string SQL = ResourceHelpers.LoadSqlCommand(SQLFileTypes.Sales, "GetCardDistinct");
            List<CR_CARD_SALE> result = await DapperORM.ReturnListAsync<CR_CARD_SALE>(SQL,
                new string[]
                {
                    "@SHOP_CODE" ,
                    "@SALE_DATE" ,
                    "@POS_NO"    ,
     //               "@REGI_SEQ"  
       //             "@CLOSE_FLAG"
                },
                new object[]
                {
                    DataLocals.AppConfig.PosInfo.StoreNo,
                    saleDT,
                    DataLocals.AppConfig.PosInfo.PosNo,
      //              ResiSeq
      //              ClozeFlak
                });

            if (!result.Any())
            {
                LogHelper.Logger.Warn($"Database : 주문오더 : 왼쪽영역 하단 버튼 셋팅(DB) 결과가 없습니다.");
                return DapperORM.ReturnResult(new List<CR_CARD_SALE>(), EResultType.EMPTY, "조회(DB) 결과(List)가 없습니다.");
            }

            return DapperORM.ReturnResult(result, EResultType.SUCCESS, "OK");
        }
        catch (FbException ex)
        {
            LogHelper.Logger.Error($"Database Error : {ex.Message.ReplacePlainText()}");
            return DapperORM.ReturnResult(new List<CR_CARD_SALE>(), EResultType.ERROR, ex.Message.ReplacePlainText());
        }
        catch (Exception ex)
        {
            LogHelper.Logger.Error($"Services Error : {ex.Message.ReplacePlainText()}");
            return DapperORM.ReturnResult(new List<CR_CARD_SALE>(), EResultType.ERROR, ex.Message.ReplacePlainText());
        }
    }
    #endregion

    public async Task<(List<PRODUCT_SALE>, SpResult)> ProductSaleOfDay(string type)
    {
        try
        {
            string SQL = ResourceHelpers.LoadSqlCommand(SQLFileTypes.ProductSalesStatus, "ProductSaleOfDay");
            List<PRODUCT_SALE> result = await DapperORM.ReturnListAsync<PRODUCT_SALE>(SQL,
                new string[]
                {
                    "@SHOPCODE" ,
                    "@SALEDATE" ,
                    "@POSNO",
                    "@PAYTYPE"
                },
                new object[]
                {
                    DataLocals.AppConfig.PosInfo.StoreNo,
                    DataLocals.PosStatus.SALE_DATE,
                    DataLocals.AppConfig.PosInfo.PosNo,
                    type
                });

            if (!result.Any())
            {
                LogHelper.Logger.Warn($"Database : 주문오더 : 왼쪽영역 하단 버튼 셋팅(DB) 결과가 없습니다.");
                return DapperORM.ReturnResult(new List<PRODUCT_SALE>(), EResultType.EMPTY, "조회(DB) 결과(List)가 없습니다.");
            }

            return DapperORM.ReturnResult(result, EResultType.SUCCESS, "OK");
        }
        catch (FbException ex)
        {
            LogHelper.Logger.Error($"Database Error : {ex.Message.ReplacePlainText()}");
            return DapperORM.ReturnResult(new List<PRODUCT_SALE>(), EResultType.ERROR, ex.Message.ReplacePlainText());
        }
        catch (Exception ex)
        {
            LogHelper.Logger.Error($"Services Error : {ex.Message.ReplacePlainText()}");
            return DapperORM.ReturnResult(new List<PRODUCT_SALE>(), EResultType.ERROR, ex.Message.ReplacePlainText());
        }
    }

    public async Task<(List<SALE_BY_TYPE2>, SpResult)> PmtTypeSaleOfDay(DynamicParameters param)
    {
        try
        {
            string SQL = ResourceHelpers.LoadSqlCommand(SQLFileTypes.ProductSalesStatus, "GetPaymentSellingStats");
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
    }

    public async Task<(List<SALE_BY_TYPE2>, SpResult)> PmttypeStatusOfDay()
    {
        try
        {
            string SQL = ResourceHelpers.LoadSqlCommand(SQLFileTypes.ProductSalesStatus, "GetPaymentSellingStats");
            List<SALE_BY_TYPE2> result = await DapperORM.ReturnListAsync<SALE_BY_TYPE2>(SQL, new string[]
                {
                    "@SHOPCODE" , "@DATEFROM" ,"@DATETO","@POSNO"
                },
                new object[]
                {
                    DataLocals.AppConfig.PosInfo.StoreNo,
                    DataLocals.PosStatus.SALE_DATE,
                    DataLocals.PosStatus.SALE_DATE,
                    DataLocals.PosStatus.POS_NO
                });

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

    public async Task<(List<TRN_HEADER>, SpResult)> GetTodayTransaction()
    {
        try
        {
            string SQL = @"SELECT * FROM TRN_HEADER 
                           WHERE SHOP_CODE    = @SHOP_CODE
                           AND   SALE_DATE    = @SALE_DATE                           
                           AND   POS_NO       = @POS_NO";
            List<TRN_HEADER> result = await DapperORM.ReturnListAsync<TRN_HEADER>(SQL, new string[]
                {
                    "@SHOP_CODE" , "@SALE_DATE" ,"@POS_NO"
                },
                new object[]
                {
                    DataLocals.AppConfig.PosInfo.StoreNo,
                    DataLocals.PosStatus.SALE_DATE,
                    DataLocals.PosStatus.POS_NO
                });

            if (!result.Any())
            {
                LogHelper.Logger.Warn($"Database : 주문오더 : 왼쪽영역 하단 버튼 셋팅(DB) 결과가 없습니다.");
                return DapperORM.ReturnResult(new List<TRN_HEADER>(), EResultType.EMPTY, "조회(DB) 결과(List)가 없습니다.");
            }

            return DapperORM.ReturnResult(result, EResultType.SUCCESS, "OK");
        }
        catch (FbException ex)
        {
            LogHelper.Logger.Error($"Database Error : {ex.Message.ReplacePlainText()}");
            return DapperORM.ReturnResult(new List<TRN_HEADER>(), EResultType.ERROR, ex.Message.ReplacePlainText());
        }
        catch (Exception ex)
        {
            LogHelper.Logger.Error($"Services Error : {ex.Message.ReplacePlainText()}");
            return DapperORM.ReturnResult(new List<TRN_HEADER>(), EResultType.ERROR, ex.Message.ReplacePlainText());
        }
    }
}
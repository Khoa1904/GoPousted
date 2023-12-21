using Caliburn.Micro;
using Dapper;
using FirebirdSql.Data.FirebirdClient;
using GoPOS.Common.Helpers;
using GoPOS.Database;
using GoPOS.Helpers;
using GoPOS.Models;
using GoPOS.Models.Common;
using GoPOS.Models.Custom.API;
using GoPOS.OrderPay.Models;
using GoPOS.Service;
using GoPOS.Service.Common;
using GoPOS.Service.Service.API;
using GoPOS.Service.Service.Payment;
using GoPOS.Service.Service.POS;
using GoPOS.ViewModels;
using GoShared.Helpers;
using log4net.Repository.Hierarchy;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Microsoft.SqlServer.Server;
using Microsoft.Web.WebView2.Core;
using Microsoft.Xaml.Behaviors.Media;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Formats.Asn1;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Animation;
using static System.Runtime.CompilerServices.RuntimeHelpers;

namespace GoPOS.Services;

public class OrderPayService : BaseDataService<TRN_HEADER>, IOrderPayService
{
    private readonly IOrderPayCashService orderPayCashService;
    private readonly IOrderPayCardService orderPayCardService;
    private readonly IPOSTMangService pOSTMangService;
    private readonly ITranApiService tranApiService;

    public OrderPayService(IOrderPayCashService orderPayCashService, IOrderPayCardService orderPayCardService, IPOSTMangService pOSTMangService,
        ITranApiService tranApiService)
    {
        this.orderPayCashService = orderPayCashService;
        this.orderPayCardService = orderPayCardService;
        this.pOSTMangService = pOSTMangService;
        this.tranApiService = tranApiService;
    }

    /// <summary>
    /// 3.버튼 리스트
    /// </summary>
    /// <param name="param"></param>
    /// <returns></returns>
    public async Task<(List<ORDER_FUNC_KEY>, SpResult)> GetOrderFuncKey(string fkFlag)
    {
        try
        {
            string SQL = ResourceHelpers.LoadSqlCommand(SQLFileTypes.OrderPay, "GetOrderFuncKey");
            //--AND B.POS_NO = @POS_NO

            //DynamicParameters param = new();
            //param.Add("@SHOP_CD", "V41712", DbType.String, ParameterDirection.Input);

            List<ORDER_FUNC_KEY> result = await DapperORM.ReturnListAsync<ORDER_FUNC_KEY>(SQL,
                new string[]
                {
                    "@SHOP_CODE",
                    "@FK_FLAG"
                },
                new object[]
                {
                    DataLocals.AppConfig.PosInfo.StoreNo,
                    fkFlag
                });

            if (!result.Any())
            {
                LogHelper.Logger.Warn($"Database : 주문오더 : 왼쪽영역 하단 버튼 셋팅(DB) 결과가 없습니다.");
                return DapperORM.ReturnResult(new List<ORDER_FUNC_KEY>(), EResultType.EMPTY, "조회(DB) 결과(List)가 없습니다.");
            }

            return DapperORM.ReturnResult(result, EResultType.SUCCESS, "OK");
        }
        catch (FbException ex)
        {
            LogHelper.Logger.Error($"Database Error : {ex.Message.ReplacePlainText()}");
            return DapperORM.ReturnResult(new List<ORDER_FUNC_KEY>(), EResultType.ERROR, ex.Message.ReplacePlainText());
        }
        catch (Exception ex)
        {
            LogHelper.Logger.Error($"Services Error : {ex.Message.ReplacePlainText()}");
            return DapperORM.ReturnResult(new List<ORDER_FUNC_KEY>(), EResultType.ERROR, ex.Message.ReplacePlainText());
        }


    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public async Task<List<MST_TUCH_CLASS>> GetTouchClasses()
    {
        try
        {
            string SQL = ResourceHelpers.LoadSqlCommand(SQLFileTypes.OrderPay, "GetTouchClasses");
            List<MST_TUCH_CLASS> result = await DapperORM.ReturnListAsync<MST_TUCH_CLASS>(SQL,
                new string[]
                {
                    "@SHOP_CODE"
                },
                new object[]
                {
                    DataLocals.AppConfig.PosInfo.StoreNo
                });

            return result;
        }
        catch (FbException ex)
        {
            LogHelper.Logger.Error($"Database Error : {ex.Message.ReplacePlainText()}");
            return new List<MST_TUCH_CLASS>();
        }
        catch (Exception ex)
        {
            LogHelper.Logger.Error($"Services Error : {ex.Message.ReplacePlainText()}");
            return new List<MST_TUCH_CLASS>();
        }
    }


    public async Task<List<ORDER_TU_PRODUCT>> GetTouchProducts(string tuClassCode)
    {
        try
        {
            string SQL = ResourceHelpers.LoadSqlCommand(SQLFileTypes.OrderPay, "GetTouchProducts");
            List<ORDER_TU_PRODUCT> result = await DapperORM.ReturnListAsync<ORDER_TU_PRODUCT>(SQL,
                new string[]
                {
                    "@SHOP_CODE",
                    "@TU_CLASS_CODE"
                },
                new object[]
                {
                    DataLocals.AppConfig.PosInfo.StoreNo,
                    tuClassCode
                });

            return result;
        }
        catch (FbException ex)
        {
            LogHelper.Logger.Error($"Database Error : {ex.Message.ReplacePlainText()}");
            return new List<ORDER_TU_PRODUCT>();
        }
        catch (Exception ex)
        {
            LogHelper.Logger.Error($"Services Error : {ex.Message.ReplacePlainText()}");
            return new List<ORDER_TU_PRODUCT>();
        }
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="prdCode"></param>
    /// <returns></returns>
    public async Task<List<ORDER_SIDE_CLS_MENU>> GetSideCLSMenus(string prdCode)
    {
        try
        {
            string SQL = ResourceHelpers.LoadSqlCommand(SQLFileTypes.OrderPay, "GetSideMenus");
            List<ORDER_SIDE_CLS_MENU> result = await DapperORM.ReturnListAsync<ORDER_SIDE_CLS_MENU>(SQL,
                new string[]
                {
                    "@SHOP_CODE",
                    "@PRD_CODE"
                },
                new object[]
                {
                    DataLocals.AppConfig.PosInfo.StoreNo,
                    prdCode
                });

            return result;
        }
        catch (FbException ex)
        {
            LogHelper.Logger.Error($"Database Error : {ex.Message.ReplacePlainText()}");
            return new List<ORDER_SIDE_CLS_MENU>();
        }
        catch (Exception ex)
        {
            LogHelper.Logger.Error($"Services Error : {ex.Message.ReplacePlainText()}");
            return new List<ORDER_SIDE_CLS_MENU>();
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public async Task<List<MST_INFO_FIX_DC>> GetFixDcList()
    {
        try
        {
            string SQL = ResourceHelpers.LoadSqlCommand(SQLFileTypes.OrderPay, "GetFixDiscountList");
            List<MST_INFO_FIX_DC> result = await DapperORM.ReturnListAsync<MST_INFO_FIX_DC>(SQL,
                new string[]
                {
                    "@SHOP_CODE"
                },
                new object[]
                {
                    DataLocals.AppConfig.PosInfo.StoreNo
                });

            return result;
        }
        catch (FbException ex)
        {
            LogHelper.Logger.Error($"Database Error : {ex.Message.ReplacePlainText()}");
            return new List<MST_INFO_FIX_DC>();
        }
        catch (Exception ex)
        {
            LogHelper.Logger.Error($"Services Error : {ex.Message.ReplacePlainText()}");
            return new List<MST_INFO_FIX_DC>();
        }
    }

    #region TR Saving, Save TR, Order Pay data all to tables

    /// <summary>
    /// 
    /// </summary>
    /// <param name="saveHold"></param>
    /// <param name="trHeader"></param>
    /// <param name="trProducts"></param>
    /// <param name="trTenders"></param>
    /// <param name="trCashs"></param>
    /// <param name="trCashRecs"></param>
    /// <param name="trCards"></param>
    /// <param name="trPartCards"></param>
    /// <param name="trGifts"></param>ㅣ
    /// <param name="trMeals"></param>
    /// <param name="trEasyPays"></param>
    /// <param name="trPointUse"></param>
    /// <param name="trPointSave"></param>
    /// <param name="memberInfo"></param>
    /// <param name="trPpCards"></param>
    /// <returns></returns>
    public async Task<(SpResult, POS_POST_MANG)> SaveOrderPayTR(bool saveHold, TRN_HEADER trHeader, TRN_PRDT[] trProducts, TRN_TENDERSEQ[] trTenders,
        TRN_CASH[] trCashs, TRN_CASHREC[] trCashRecs, TRN_CARD[] trCards, TRN_PARTCARD[] trPartCards, TRN_GIFT[] trGifts, TRN_FOODCPN[] trMeals,
        TRN_EASYPAY[] trEasyPays, TRN_POINTUSE[] trPointUse, TRN_POINTSAVE trPointSave, MEMBER_CLASH memberInfo, TRN_PPCARD[] trPpCards, string? tableCd)
    {
        var result = new SpResult()
        {
            ResultType = EResultType.SUCCESS,
            ResultCode = "0000"
        };

        // Return kds info
        POS_POST_MANG kdsPostMang = null;
        StringBuilder sbErrors = new StringBuilder();

        Dictionary<string, string> mapFields = new Dictionary<string, string>();
        mapFields.Add("BILL_NO", "ORDER_NO");

        using (var db = new DataContext())
        {
            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    /// Update POS Status
                    var poss = db.pOS_STATUSes.FirstOrDefault(t => t.SHOP_CODE == DataLocals.AppConfig.PosInfo.StoreNo &&
                    t.POS_NO == DataLocals.AppConfig.PosInfo.PosNo);
                    int billNo = Convert.ToInt32(poss.BILL_NO);
                    billNo++;
                    string newBillNo = billNo.ToString("0000");

                    poss.BILL_NO = newBillNo;
                    poss.ORDER_NO = newBillNo;
                    trHeader.BILL_NO = newBillNo;
                    trHeader.ORDER_NO = newBillNo;
                    trHeader.INSERT_DT = DateTime.Now.ToString(Formats.SystemDBDateTime);
                    if (memberInfo != null) { trHeader.CST_NO = memberInfo.mbrCode; }
                    #region 포인트 적립

                    if (memberInfo != null && trPointSave != null)
                    {
                        var pointSavePaySeq = (trTenders.Length > 0 ? trTenders.Max(p => p.PAY_SEQ_NO) : 0);
                        pointSavePaySeq++;
                        var newTenders = trTenders.ToList();
                        newTenders.Add(new TRN_TENDERSEQ()
                        {
                            SHOP_CODE = DataLocals.PosStatus.SHOP_CODE,
                            SALE_DATE = DataLocals.PosStatus.SALE_DATE,
                            POS_NO = DataLocals.PosStatus.POS_NO,
                            BILL_NO = DataLocals.PosStatus.BILL_NO,
                            PAY_SEQ_NO = (Int16)pointSavePaySeq,
                            REGI_SEQ = DataLocals.PosStatus.REGI_SEQ,
                            SALE_YN = "Y",
                            PAY_TYPE_FLAG = OrderPayConsts.SAVE_POINTS,
                            PAY_AMT = trPointSave.SAVE_AMT,
                            LINE_NO = trPointSave.SEQ_NO,
                            EMP_NO = DataLocals.Employee.EMP_NO,
                            INSERT_DT = DateTime.Now.ToString("yyyyMMddHHmmss"),
                            UPDATE_DT = DateTime.Now.ToString("yyyyMMddHHmmss")
                        });

                        trTenders = newTenders.ToArray();
                        trHeader.CST_NO = memberInfo.mbrCode; // 고객정보 있을경우에만 넣는다..위치변경
                    }

                    #endregion

                    OrderPayExtensions.UpdateFromHeader(trHeader, trProducts, "SALE_DATE", "SHOP_CODE", "POS_NO", "BILL_NO", "ORDER_NO", "REGI_SEQ", "INSERT_DT", "SALE_YN");
                    OrderPayExtensions.UpdateFromHeader(trHeader, trTenders, "SALE_DATE", "SHOP_CODE", "POS_NO", "BILL_NO", "ORDER_NO", "REGI_SEQ", "INSERT_DT", "SALE_YN");
                    OrderPayExtensions.UpdateFromHeader(trHeader, trCashs, "SALE_DATE", "SHOP_CODE", "POS_NO", "BILL_NO", "ORDER_NO", "REGI_SEQ", "INSERT_DT", "SALE_YN");
                    OrderPayExtensions.UpdateFromHeader(trHeader, trCashRecs, "SALE_DATE", "SHOP_CODE", "POS_NO", "BILL_NO", "ORDER_NO", "REGI_SEQ", "INSERT_DT");
                    OrderPayExtensions.UpdateFromHeader(trHeader, trCards, "SALE_DATE", "SHOP_CODE", "POS_NO", "BILL_NO", "ORDER_NO", "REGI_SEQ", "INSERT_DT", "SALE_YN");
                    OrderPayExtensions.UpdateFromHeader(trHeader, trPartCards, "SALE_DATE", "SHOP_CODE", "POS_NO", "BILL_NO", "ORDER_NO", "REGI_SEQ", "INSERT_DT", "SALE_YN");
                    OrderPayExtensions.UpdateFromHeader(trHeader, trGifts, "SALE_DATE", "SHOP_CODE", "POS_NO", "BILL_NO", "ORDER_NO", "REGI_SEQ", "INSERT_DT", "SALE_YN");
                    OrderPayExtensions.UpdateFromHeader(trHeader, trMeals, "SALE_DATE", "SHOP_CODE", "POS_NO", "BILL_NO", "ORDER_NO", "REGI_SEQ", "INSERT_DT");
                    OrderPayExtensions.UpdateFromHeader(trHeader, trEasyPays, "SALE_DATE", "SHOP_CODE", "POS_NO", "BILL_NO", "ORDER_NO", "REGI_SEQ", "INSERT_DT", "SALE_YN");

                    if (trPpCards != null)
                        OrderPayExtensions.UpdateFromHeader(trHeader, trPpCards, "SALE_DATE", "SHOP_CODE", "POS_NO", "BILL_NO", "ORDER_NO", "REGI_SEQ", "INSERT_DT", "SALE_YN");


                    if (trPointUse != null)
                    {
                        OrderPayExtensions.UpdateFromHeader(trHeader, trPointUse, "SALE_DATE", "SHOP_CODE", "POS_NO", "BILL_NO", "ORDER_NO", "REGI_SEQ", "INSERT_DT", "SALE_YN");
                    }
                    if (trPointSave != null)
                    {
                        OrderPayExtensions.UpdateFromHeader(trHeader, new TRN_POINTSAVE[] { trPointSave }, "SALE_DATE", "SHOP_CODE", "POS_NO", "BILL_NO", "ORDER_NO", "REGI_SEQ", "INSERT_DT", "SALE_YN");
                    }
                    db.SaveChanges();

                    if (!saveHold)
                    {
                        db.tRN_TENDERSEQs.AddRange(trTenders);
                        db.tRN_PRDTs.AddRange(trProducts);
                        db.tRN_CASHes.AddRange(trCashs);
                        db.tRN_CASHRECs.AddRange(trCashRecs);
                        db.tRN_CARDs.AddRange(trCards);
                        db.tRN_PARTCARDs.AddRange(trPartCards);
                        db.tRN_GIFTs.AddRange(trGifts);
                        db.tRN_FOODCPNs.AddRange(trMeals);
                        db.tRN_EASYPAYs.AddRange(trEasyPays);

                        if (trPpCards != null)
                            db.tRN_PPCARDs.AddRange(trPpCards);
                        db.tRN_POINTUSEs.AddRange(trPointUse);
                        if (trPointSave != null)
                        {
                            db.tRN_POINTSAVEs.Add(trPointSave);
                        }
                        db.tRN_HEADERs.Add(trHeader);
                    }
                    db.SaveChanges();
                    // Copy data to ORD tables
                    List<ORD_TENDERSEQ> oRD_TENDERSEQs = new List<ORD_TENDERSEQ>();
                    foreach (var tender in trTenders)
                    {
                        var otender = tender.CopyFields<ORD_TENDERSEQ>(mapFields);
                        oRD_TENDERSEQs.Add(otender);
                    }
                    db.SaveChanges();
                    List<ORD_PRDT> oRD_PRDTs = new List<ORD_PRDT>();
                    foreach (var prdt in trProducts)
                    {
                        var oprdt = prdt.CopyFields<ORD_PRDT>(mapFields);
                        oprdt.ORDER_SEQ_NO = "01";
                        oRD_PRDTs.Add(oprdt);
                    }
                    List<ORD_CASH> oRD_CASHes = new List<ORD_CASH>();
                    foreach (var cash in trCashs)
                    {
                        var ocash = cash.CopyFields<ORD_CASH>(mapFields);
                        oRD_CASHes.Add(ocash);
                    }
                    db.SaveChanges();
                    List<ORD_CARD> oRD_CARDs = new List<ORD_CARD>();
                    foreach (var card in trCards)
                    {
                        var ocard = card.CopyFields<ORD_CARD>(mapFields);
                        oRD_CARDs.Add(ocard);
                    }
                    List<ORD_PARTCARD> oRD_PARTCARDs = new List<ORD_PARTCARD>();
                    foreach (var partCard in trPartCards)
                    {
                        var oPartCard = partCard.CopyFields<ORD_PARTCARD>(mapFields);
                        oRD_PARTCARDs.Add(oPartCard);
                    }
                    List<ORD_GIFT> oRD_GIFTs = new();
                    foreach (var gift in trGifts)
                    {
                        var oGift = gift.CopyFields<ORD_GIFT>(mapFields);
                        oRD_GIFTs.Add(oGift);
                    }
                    db.SaveChanges();
                    List<ORD_FOODCPN> oRD_FOODCPNs = new();
                    foreach (var food in trMeals)
                    {
                        var oFood = food.CopyFields<ORD_FOODCPN>(mapFields);
                        oRD_FOODCPNs.Add(oFood);
                    }
                    List<ORD_EASYPAY> oRD_EASYPAYs = new();
                    foreach (var easyPay in trEasyPays)
                    {
                        var oEasyPay = easyPay.CopyFields<ORD_EASYPAY>(mapFields);
                        oRD_EASYPAYs.Add(oEasyPay);
                    }

                    List<ORD_POINTUSE> oRD_POINTUSE = new();
                    foreach (var pointed in trPointUse)
                    {
                        var oPointUse = pointed.CopyFields<ORD_POINTUSE>(mapFields);
                        db.oRD_POINTUSEs.Add(oPointUse);
                    }

                    if (trPointSave != null)
                    {
                        var oRD_POINTSAVE = trPointSave.CopyFields<ORD_POINTSAVE>(mapFields);
                        db.oRD_POINTSAVEs.Add(oRD_POINTSAVE);
                    }

                    List<ORD_PPCARD> oRD_PPCARDs = new();
                    foreach (var ppCard in trPpCards)
                    {
                        var oPpCard = ppCard.CopyFields<ORD_PPCARD>(mapFields);
                        oRD_PPCARDs.Add(oPpCard);
                    }
                    // đang làm
                    ORD_HEADER oRD_HEADER = trHeader.CopyFields<ORD_HEADER>(mapFields);
                    if (!string.IsNullOrEmpty(tableCd))
                    {
                        oRD_HEADER.FD_TBL_CODE = tableCd;
                        oRD_HEADER.DLV_ORDER_FLAG = saveHold ? "2" : "0";
                    }
                    else
                    {
                        oRD_HEADER.DLV_ORDER_FLAG = saveHold ? "1" : "0";
                    }
                        oRD_HEADER.ORDER_END_FLAG = saveHold ? "0" : "2";

                    oRD_HEADER.FD_GST_FLAG_YN = "0";

                    db.oRD_PRDTs.AddRange(oRD_PRDTs);
                    db.oRD_CASHes.AddRange(oRD_CASHes);
                    db.oRD_CARDs.AddRange(oRD_CARDs);
                    db.SaveChanges();

                    db.oRD_GIFTs.AddRange(oRD_GIFTs);
                    db.oRD_FOODCPNs.AddRange(oRD_FOODCPNs);
                    db.oRD_EASYPAYs.AddRange(oRD_EASYPAYs);
                    db.oRD_PPCARDs.AddRange(oRD_PPCARDs);

                    db.SaveChanges();
                    db.oRD_TENDERSEQs.AddRange(oRD_TENDERSEQs);
                    db.oRD_HEADERs.Add(oRD_HEADER);
                    if (!saveHold)
                    {
                        #region Update 정산 테이블

                        ///
                        /// Update 정산 테이블
                        ///
                        var posAcct = db.sETT_POSACCOUNTs.FirstOrDefault(p => p.SHOP_CODE ==
                                DataLocals.AppConfig.PosInfo.StoreNo && p.POS_NO == DataLocals.AppConfig.PosInfo.PosNo &&
                                p.SALE_DATE == DataLocals.PosStatus.SALE_DATE && p.REGI_SEQ == DataLocals.PosStatus.REGI_SEQ);

                        posAcct.TOT_BILL_CNT += 1;

                        int addUp = trHeader.SALE_YN == "Y" ? 1 : -1;
                        decimal totalDiscount = 0;

                        posAcct.TOT_SALE_AMT += addUp * trHeader.TOT_SALE_AMT;
                        posAcct.TOT_DC_AMT += addUp * trHeader.TOT_DC_AMT;
                        posAcct.SVC_TIP_AMT += addUp * trHeader.SVC_TIP_AMT;
                        posAcct.TOT_ETC_AMT += addUp * trHeader.TOT_ETC_AMT;
                        posAcct.DCM_SALE_AMT += addUp * trHeader.DCM_SALE_AMT;
                        posAcct.VAT_SALE_AMT += addUp * trHeader.VAT_SALE_AMT;
                        posAcct.VAT_AMT += addUp * trHeader.VAT_AMT;
                        posAcct.NO_VAT_SALE_AMT += addUp * trHeader.NO_VAT_SALE_AMT;
                        posAcct.NO_TAX_SALE_AMT += addUp * trHeader.NO_TAX_SALE_AMT;

                        #region Discount calculation

                        /*
                         *  DC_GEN_CNT
                         *  DC_GEN_AMT
                         *  DC_SVC_CNT
                         *  DC_SVC_AMT
                         *  DC_JCD_CNT
                         *  DC_JCD_AMT
                         *  DC_CPN_CNT
                         *  DC_CPN_AMT
                         *  DC_CST_CNT
                         *  DC_CST_AMT
                         *  DC_TFD_CNT
                         *  DC_TFD_AMT
                         *  DC_PRM_CNT
                         *  DC_PRM_AMT
                         *  DC_CRD_CNT
                         *  DC_CRD_AMT
                         *  DC_PACK_CNT
                         *  DC_PACK_AMT
                        */
                        CalcDiscount4Sett(posAcct, trHeader, "DC_GEN_AMT", "DC_GEN_CNT", addUp);
                        //posAcct.DC_TFD_CNT = addUp * trTenders.Where(p => p.PAY_TYPE_FLAG == OrderPayConsts.PAY_MEAL).Count();
                        //posAcct.DC_TFD_AMT += addUp * trHeader.DC_TFD_AMT;
                        CalcDiscount4Sett(posAcct, trHeader, "DC_SVC_AMT", "DC_SVC_CNT", addUp);
                        CalcDiscount4Sett(posAcct, trHeader, "DC_JCD_AMT", "DC_JCD_CNT", addUp);
                        CalcDiscount4Sett(posAcct, trHeader, "DC_CPN_AMT", "DC_CPN_CNT", addUp);
                        CalcDiscount4Sett(posAcct, trHeader, "DC_CST_AMT", "DC_CST_CNT", addUp);
                        CalcDiscount4Sett(posAcct, trHeader, "DC_TFD_AMT", "DC_TFD_CNT", addUp);
                        CalcDiscount4Sett(posAcct, trHeader, "DC_PRM_AMT", "DC_PRM_CNT", addUp);
                        CalcDiscount4Sett(posAcct, trHeader, "DC_CRD_AMT", "DC_CRD_CNT", addUp);
                        CalcDiscount4Sett(posAcct, trHeader, "DC_PACK_AMT", "DC_PACK_CNT", addUp);

                        #endregion

                        posAcct.CASH_CNT += trTenders.Where(p => p.PAY_TYPE_FLAG == OrderPayConsts.PAY_CASH).Count();
                        posAcct.CASH_AMT += addUp * trTenders.Where(p => p.PAY_TYPE_FLAG == OrderPayConsts.PAY_CASH).Sum(p => p.PAY_AMT);

                        posAcct.CASH_BILL_CNT += trTenders.Where(p => p.PAY_TYPE_FLAG == OrderPayConsts.PAY_CASHREC).Count();
                        posAcct.CASH_BILL_AMT += addUp * trTenders.Where(p => p.PAY_TYPE_FLAG == OrderPayConsts.PAY_CASHREC).Sum(p => p.PAY_AMT);

                        posAcct.CRD_CARD_CNT += trTenders.Where(p => p.PAY_TYPE_FLAG == OrderPayConsts.PAY_CARD).Count();
                        posAcct.CRD_CARD_AMT += addUp * trTenders.Where(p => p.PAY_TYPE_FLAG == OrderPayConsts.PAY_CARD).Sum(p => p.PAY_AMT);

                        posAcct.SP_PAY_CNT += trTenders.Where(p => p.PAY_TYPE_FLAG == OrderPayConsts.PAY_EASY).Count();
                        posAcct.SP_PAY_AMT += addUp * trTenders.Where(p => p.PAY_TYPE_FLAG == OrderPayConsts.PAY_EASY).Sum(p => p.PAY_AMT);

                        posAcct.JCD_CARD_CNT += trTenders.Where(p => p.PAY_TYPE_FLAG == OrderPayConsts.PAY_PARTCARD).Count();
                        posAcct.JCD_CARD_AMT += addUp * trTenders.Where(p => p.PAY_TYPE_FLAG == OrderPayConsts.PAY_PARTCARD).Sum(p => p.PAY_AMT);

                        posAcct.TK_GFT_CNT += trTenders.Where(p => p.PAY_TYPE_FLAG == OrderPayConsts.PAY_GIFT).Count();
                        posAcct.TK_GFT_AMT += addUp * trTenders.Where(p => p.PAY_TYPE_FLAG == OrderPayConsts.PAY_GIFT).Sum(p => p.PAY_AMT);
                        posAcct.TK_GFT_UAMT += addUp * trGifts.Where(p => p.TK_GFT_SALE_FLAG == "0").Sum(p => p.TK_GFT_UAMT);

                        posAcct.TK_FOD_CNT += trTenders.Where(p => p.PAY_TYPE_FLAG == OrderPayConsts.PAY_MEAL).Count();
                        posAcct.TK_FOD_AMT += addUp * trTenders.Where(p => p.PAY_TYPE_FLAG == OrderPayConsts.PAY_MEAL).Sum(p => p.PAY_AMT);
                        posAcct.TK_FOD_UAMT += addUp * trMeals.Sum(p => p.TK_FOD_UAMT);

                        posAcct.CST_POINTSAVE_CNT = trPointSave != null ? 1 : 0;
                        posAcct.CST_POINTSAVE_AMT = addUp * (trPointSave != null ? trPointSave.SAVE_AMT : 0);

                        posAcct.CST_POINTUSE_CNT += trTenders.Where(p => p.PAY_TYPE_FLAG == OrderPayConsts.PAY_POINTS).Count();
                        posAcct.CST_POINTUSE_AMT += addUp * trTenders.Where(p => p.PAY_TYPE_FLAG == OrderPayConsts.PAY_POINTS).Sum(p => p.PAY_AMT);

                        posAcct.PPC_CARD_CNT += trTenders.Where(p => p.PAY_TYPE_FLAG == OrderPayConsts.PAY_PPCARD).Count();
                        posAcct.PPC_CARD_AMT += addUp * trTenders.Where(p => p.PAY_TYPE_FLAG == OrderPayConsts.PAY_PPCARD).Sum(p => p.PAY_AMT);

                        /// 
                        /// 0:회수, 3:환입 = 환불
                        /// trGifts = 상품권 결제 리스트 (해당 거래)
                        /// 
                        // 상품권 - 현금환불건수
                        posAcct.REPAY_CASH_CNT += trGifts.Where(p => p.TK_GFT_SALE_FLAG == "0").Count();
                        // 상품권 - 현금환불액
                        posAcct.REPAY_CASH_AMT += addUp * trGifts.Where(p => p.TK_GFT_SALE_FLAG == "0").Sum(p => p.REPAY_CSH_AMT);
                        //상품권 - 환불건수
                        posAcct.REPAY_TK_GFT_CNT += trGifts.Where(p => p.TK_GFT_SALE_FLAG == "3").Count();
                        // 상품권 - 환불액
                        posAcct.REPAY_TK_GFT_AMT += addUp * trGifts.Where(p => p.TK_GFT_SALE_FLAG == "3").Sum(p => p.REPAY_GFT_AMT);

                        /*
                        posAcct.WES_CNT
                        posAcct.WES_AMT*/
                        #endregion
                    }

                    db.SaveChanges();

                    #region Update POS_POST_MANG

                    if (!saveHold)
                    {
                        var postRes = pOSTMangService.AddFromTrnHeader(db, trHeader, false).Result;
                        if (postRes.Item1.ResultType != EResultType.SUCCESS)
                        {
                            throw new Exception(postRes.Item1.ResultMessage);
                        }

                        #region KDS Sending & Printer Kitchen

                        var kdsRes = pOSTMangService.AddFromTrnHeader(db, trHeader, true).Result;
                        if (kdsRes.Item1.ResultType != EResultType.SUCCESS)
                        {
                            throw new Exception(kdsRes.Item1.ResultMessage);
                        }

                        kdsPostMang = kdsRes.Item2;
                        #endregion
                    }

                    #endregion

                    db.SaveChanges();
                    trans.Commit();

                    // update current
                    DataLocals.PosStatus = poss;

                }
                catch (DbEntityValidationException e)
                {
                    foreach (var eve in e.EntityValidationErrors)
                    {
                        LogHelper.Logger.Error("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                            eve.Entry.Entity.GetType().Name, eve.Entry.State);
                        foreach (var ve in eve.ValidationErrors)
                        {
                            LogHelper.Logger.Error("- Property: \"{0}\", Error: \"{1}\"",
                                ve.PropertyName, ve.ErrorMessage);
                        }
                    }

                    trans.Rollback();
                    result.ResultType = EResultType.ERROR;
                    result.ResultCode = "9999";

                    sbErrors.AppendLine();
                    sbErrors.Append(e.ToFormattedString());
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    result.ResultType = EResultType.ERROR;
                    result.ResultCode = "9999";

                    sbErrors.AppendLine();
                    sbErrors.Append(ex.ToFormattedString());
                    LogHelper.Logger.Error(ex.ToFormattedString());
                }
            }
        }

        result.ResultMessage = sbErrors.ToString();
        return (result, kdsPostMang);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sETT_POSACCOUNT"></param>
    /// <param name="tRN_HEADER"></param>
    /// <param name="amtField"></param>
    /// <param name="cntField"></param>
    /// <param name="addUp"></param>
    /// <returns></returns>
    private decimal CalcDiscount4Sett(SETT_POSACCOUNT sETT_POSACCOUNT, TRN_HEADER tRN_HEADER, string amtField, string cntField,
            int addUp)
    {
        var pisa = sETT_POSACCOUNT.GetType().GetProperty(amtField, BindingFlags.Instance | BindingFlags.Public);
        var piha = tRN_HEADER.GetType().GetProperty(amtField, BindingFlags.Instance | BindingFlags.Public);
        if (pisa == null || piha == null)
        {
            return 0;
        }

        var valueAdded = addUp * ((decimal)piha.GetValue(tRN_HEADER, null));
        if (valueAdded == 0)
        {
            return 0;
        }

        // so am thi tinh sao?
        var oldValue = ((decimal)pisa.GetValue(sETT_POSACCOUNT, null));
        oldValue += valueAdded;
        pisa.SetValue(sETT_POSACCOUNT, oldValue);

        var pisn = sETT_POSACCOUNT.GetType().GetProperty(cntField, BindingFlags.Instance | BindingFlags.Public);
        if (pisn != null)
        {
            var cntValue = (int)pisn.GetValue(sETT_POSACCOUNT, null);
            cntValue++;
            pisn.SetValue(sETT_POSACCOUNT, cntValue);
        }

        return valueAdded;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="shopCode"></param>
    /// <param name="posNo"></param>
    /// <param name="saleDate"></param>
    /// <param name="regiSeq"></param>
    /// <param name="billNo"></param>
    /// <param name="refReturnBillNo"></param>
    /// <returns></returns>
    public void UpdateOrderPayReturnTR(string shopCode, string posNo, string saleDate, string regiSeq, string billNo, string refReturnBillNo)
    {
        using (var context = new DataContext())
        {
            var trHeader = context.tRN_HEADERs.FirstOrDefault(p => p.SHOP_CODE == shopCode &&
                                    p.POS_NO == posNo && p.SALE_DATE == saleDate && p.REGI_SEQ == regiSeq &&
                                    p.BILL_NO == billNo);
            if (trHeader != null)
            {
                trHeader.ORG_BILL_NO = refReturnBillNo;
            }

            context.tRN_HEADERs.AddOrUpdate(trHeader);
            context.SaveChanges();
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="tranData"></param>
    /// <returns></returns>
    public Task<(SpResult, string)> DoReturnReceipt(TranData tranData)
    {
        SpResult res = new SpResult()
        {
            ResultType = EResultType.SUCCESS
        };
        string returnBillNo = string.Empty;
        try
        {

            // prepare headers
            string normShopCode = tranData.TranHeader.SHOP_CODE;
            string normPosNo = tranData.TranHeader.POS_NO;
            string normSaleDate = tranData.TranHeader.SALE_DATE;
            string normRegiSeq = tranData.TranHeader.REGI_SEQ;
            string normBillNo = tranData.TranHeader.BILL_NO;

            tranData.TranHeader.SALE_DATE = DataLocals.PosStatus.SALE_DATE;
            tranData.TranHeader.REGI_SEQ = DataLocals.PosStatus.REGI_SEQ;

            // ORG_BILL_NO la bill cua normal receipt
            tranData.TranHeader.ORG_BILL_NO = tranData.TranHeader.SHOP_CODE + tranData.TranHeader.SALE_DATE + tranData.TranHeader.POS_NO + tranData.TranHeader.BILL_NO;
            tranData.TranHeader.SALE_YN = "N";
            tranData.TranHeader.EMP_NO = DataLocals.Employee.EMP_NO;

            // orderPayService
            /// TO DO
            ///     - 반품이면 -정산
            ///     - 출력안함 
            ///     - 전송확인
            ///            

            var trRes = SaveOrderPayTR(false, tranData.TranHeader, tranData.TranProduct, tranData.TranTenderSeq,
            tranData.TranCash ?? new TRN_CASH[0], tranData.TranCashRec ?? new TRN_CASHREC[0], tranData.TranCard ?? new TRN_CARD[0],
            tranData.TranPartnerCard ?? new TRN_PARTCARD[0], tranData.TranGift ?? new TRN_GIFT[0],
            tranData.TranFoodCpn ?? new TRN_FOODCPN[0], tranData.TranEasyPay ?? new TRN_EASYPAY[0], tranData.TranPointuse ?? new TRN_POINTUSE[0],
            tranData.TranPointSave, null, tranData.TranPpCard ?? new TRN_PPCARD[0], null).Result;

            returnBillNo = tranData.TranHeader.SHOP_CODE + tranData.TranHeader.SALE_DATE + tranData.TranHeader.POS_NO + tranData.TranHeader.BILL_NO;

            if (trRes.Item1.ResultType != EResultType.SUCCESS)
            {
                res.ResultMessage = trRes.Item1.ResultMessage;
                res.ResultType = EResultType.ERROR;
            }
            else
            {
                UpdateOrderPayReturnTR(normShopCode, normPosNo, normSaleDate, normRegiSeq, normBillNo, returnBillNo);
            }
        }
        catch (Exception ex)
        {
            res.ResultType = EResultType.ERROR;
            res.ResultMessage = ex.Message;
            LogHelper.Logger.Error(ex.ToFormattedString());
        }

        return Task.FromResult((res, returnBillNo));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="shopCode"></param>
    /// <param name="posNo"></param>
    /// <param name="saleDate"></param>
    /// <param name="regiSeq"></param>
    /// <param name="billNo"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public Task<(SpResult, string)> DoReturnReceipt(string shopCode, string posNo, string saleDate, string regiSeq, string billNo)
    {
        var tranData = tranApiService.GetTRData(shopCode, posNo, saleDate, regiSeq, billNo);
        return DoReturnReceipt(tranData);
    }

    public bool ReplayOverlappedOrder(string tableCD)
    {
        // check existing order
        try
        {
            using (var db = new DataContext())
            {
                var ords = db.oRD_HEADERs.Where(p => p.DLV_ORDER_FLAG == "2" && p.ORDER_END_FLAG == "0" && p.FD_TBL_CODE == tableCD
                && p.POS_NO == DataLocals.AppConfig.PosInfo.PosNo.ToString()).FirstOrDefault();
                if (ords == null)
                {
                    return false;
                }
                else //remove existing order
                {
                    using (var trans = db.Database.BeginTransaction())
                    {
                        try
                        {
                            db.oRD_HEADERs.Remove(ords);
                            db.SaveChanges();
                            trans.Commit();
                            return true;
                        }
                        catch (DbEntityValidationException e)
                        {
                            foreach (var eve in e.EntityValidationErrors)
                            {
                                LogHelper.Logger.Error("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                                    eve.Entry.Entity.GetType().Name, eve.Entry.State);
                                foreach (var ve in eve.ValidationErrors)
                                {
                                    LogHelper.Logger.Error("- Property: \"{0}\", Error: \"{1}\"",
                                        ve.PropertyName, ve.ErrorMessage);
                                }
                            }

                            trans.Rollback();
                            return false;
                        }
                        catch (Exception ex)
                        {
                            trans.Rollback();
                            LogHelper.Logger.Error(ex.ToFormattedString());
                            return false;
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            LogHelper.Logger.Error(ex.ToString());
            return false;
        }
             
    }

    #endregion
}
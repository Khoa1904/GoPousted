using ESCPOS_NET.DataValidation;
using FirebirdSql.Data.FirebirdClient;
using GoPOS.Common.Helpers;
using GoPOS.Database;
using GoPOS.Models;
using GoPOS.Models.Common;
using GoPOS.Models.Custom.API;
using GoPOS.Service;
using GoPOS.Service.Common;
using GoPOS.Service.Service.API;
using GoPOS.Service.Service.POS;
using GoShared.Helpers;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Data.Entity.Validation;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace GoPOS.Sales.Services
{
    /// <summary>
    /// 영업관리 서비스
    /// </summary>
    public class SalesMngService : ISalesMngService
    {
        private readonly ITranApiService tranApiService;
        private readonly IPOSTMangService pOSTMangService;

        public SalesMngService(ITranApiService tranApiService, IPOSTMangService pOSTMangService)
        {
            this.tranApiService = tranApiService;
            this.pOSTMangService = pOSTMangService;

        }
        public Task<(SpResult, string)> DoRefundRePayment(string shopCode, string posNo, string saleDate, string billNo)
        {
            var tranData = tranApiService.GetNonTRData(shopCode, posNo, saleDate, DataLocals.PosStatus.REGI_SEQ, billNo);
            return DoReturnReceipt(tranData);
        }

        public Task<(SpResult, string)> DoReturnReceipt(NonTransModel tranData)
        {
            SpResult res = new SpResult()

            {
                ResultType = EResultType.SUCCESS
            };
            string returnBillNo = string.Empty;
            try
            {

                // prepare headers
                string normShopCode = tranData.SHOP_CODE;
                string normPosNo = tranData.POS_NO;
                string normSaleDate = tranData.SALE_DATE;
                string normBillNo = tranData.SALE_NO;
                string normRegiSeq = DataLocals.PosStatus.REGI_SEQ;

                //    tranData.REGI_SEQ = DataLocals.PosStatus.REGI_SEQ;

                // ORG_BILL_NO la bill cua normal receipt
                // tranData.TranHeader.ORG_BILL_NO = tranData.TranHeader.SHOP_CODE + tranData.TranHeader.SALE_DATE + tranData.TranHeader.POS_NO + tranData.TranHeader.BILL_NO;

                // orderPayService
                /// TO DO
                ///     - 반품이면 -정산
                ///     - 출력안함 
                ///     - 전송확인
                ///            


                returnBillNo = tranData.SHOP_CODE + tranData.SALE_DATE + tranData.POS_NO + tranData.SALE_NO;


                UpdateOrderPayReturnTR(normShopCode, normPosNo, normSaleDate, normRegiSeq, normBillNo, returnBillNo);
            }
            catch (Exception ex)
            {
                res.ResultType = EResultType.ERROR;
                res.ResultMessage = ex.Message;
                LogHelper.Logger.Error(ex.ToFormattedString());
            }

            return Task.FromResult((res, returnBillNo));
        }

        public void UpdateOrderPayReturnTR(string shopCode, string posNo, string saleDate, string regiSeq, string billNo, string refReturnBillNo)
        {
            using (var context = new DataContext())
            {
                var trHeader = context.nTRN_PRECHARGE_HEADERs.FirstOrDefault(p => p.SHOP_CODE == shopCode &&
                                        p.POS_NO == posNo && p.SALE_DATE == saleDate && p.REGI_SEQ == regiSeq &&
                                        p.SALE_NO == billNo);
                if (trHeader != null)
                {
                    trHeader.ORG_SALE_NO = refReturnBillNo;
                }

                context.nTRN_PRECHARGE_HEADERs.AddOrUpdate(trHeader);
                context.SaveChanges();
            }
        }

        public NTRN_PRECHARGE_CARD GetApprNo(string salebillno)
        {
            using (var db = new DataContext())
            {
                var data = db.nTRN_PRECHARGE_CARDs.FirstOrDefault(p =>
                    p.SHOP_CODE == DataLocals.AppConfig.PosInfo.StoreNo &&
                    p.POS_NO == DataLocals.AppConfig.PosInfo.PosNo &&
                    p.SALE_DATE == DataLocals.PosStatus.SALE_DATE && p.REGI_SEQ == DataLocals.PosStatus.REGI_SEQ &&
                    p.SALE_NO == salebillno);

                if (data != null)
                {
                    return data;
                }
            }
            return null;
        }

        public NTRN_PRECHARGE_CARD GetOrgPrechargeCard(string shopCode, string posNo, string saleDate, string saleNo)
        {
            using (var db = new DataContext())
            {
                return db.nTRN_PRECHARGE_CARDs.FirstOrDefault(p =>
                    p.SHOP_CODE == shopCode &&
                    p.POS_NO == posNo &&
                    p.SALE_DATE == saleDate && p.SALE_NO == saleNo);
            }
        }


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

        public static void UpdateFromHeader(TRN_HEADER trHeader, object[] childTRs, params string[] headerFields)
        {
            if (childTRs.Length == 0)
            {
                return;
            }

            List<PropertyInfo> pis = new List<PropertyInfo>();
            foreach (var hf in headerFields)
            {
                var pi = childTRs[0].GetType().GetProperty(hf, BindingFlags.Public | BindingFlags.Instance);
                if (pi != null) pis.Add(pi);
            }

            foreach (var childTR in childTRs)
            {
                foreach (var pi in pis)
                {
                    var hpi = trHeader.GetType().GetProperty(pi.Name, BindingFlags.Public | BindingFlags.Instance);
                    if (hpi == null)
                    {
                        continue;
                    }

                    var hval = hpi.GetValue(trHeader);
                    pi.SetValue(childTR, hval);
                }
            }
        }

        public SalesMiddleExcClcModel GetMiddleSettData(string saleDate, string regiSeq)
        {
            try
            {
                string SQL = ResourceHelpers.LoadSqlCommand(SQLFileTypes.Sales, "SalesMiddleSettItems");
                SalesMiddleExcClcModel result = DapperORM.ReturnSingleAsync<SalesMiddleExcClcModel>(SQL,
                        new string[]
                        {
                                    "@SHOP_CODE",
                                    "@POS_NO",
                                    "@SALE_DATE",
                                    "@REGI_SEQ"
                        },
                        new object[]
                        {
                                    DataLocals.AppConfig.PosInfo.StoreNo,
                                    DataLocals.AppConfig.PosInfo.PosNo,
                                    saleDate, regiSeq

                        }).Result;

                return result;
            }
            catch (FbException ex)
            {
                LogHelper.Logger.Error($"Database Error : {ex.Message.ReplacePlainText()}");
                return new SalesMiddleExcClcModel();
            }
            catch (Exception ex)
            {
                LogHelper.Logger.Error($"Services Error : {ex.Message.ReplacePlainText()}");
                return new SalesMiddleExcClcModel();
            }
        }

        /// <summary>
        /// 비매출 - 선결제 Header TRAN
        /// </summary>
        public Tuple<string, bool> SaveNonTrnPrecharge(MEMBER_CLASH mEMBER_CLASH, NTRN_PRECHARGE_CARD tRN_CARD, bool isCancelled, out string errorMessage)
        {
            Tuple<string, bool> returnValue = new Tuple<string, bool>("", false);
            errorMessage = string.Empty;
            using (var db = new DataContext())
            {
                var tran = db.Database.BeginTransaction();
                try
                {

                    var newSaleNo = DataLocals.PosStatus.SALE_NO.StrIntInc(4);
                    NTRN_PRECHARGE_HEADER header = new NTRN_PRECHARGE_HEADER()
                    {
                        SHOP_CODE = DataLocals.AppConfig.PosInfo.StoreNo,
                        POS_NO = DataLocals.AppConfig.PosInfo.PosNo,
                        SALE_DATE = DataLocals.PosStatus.SALE_DATE,
                        EMP_NO = DataLocals.Employee.EMP_NO,
                        REGI_SEQ = DataLocals.PosStatus.REGI_SEQ,
                        CST_NO = mEMBER_CLASH.mbrCode,
                        SALE_NO = newSaleNo,
                        CHARGE_YN = isCancelled ? "N" : "Y",
                        PAY_TYPE_FLAG = "01",
                        CHARGE_AMT = tRN_CARD.APPR_AMT,
                        CHARGE_REM_AMT = 0,
                        INSERT_DT = DateTime.Now.ToString("yyyyMMddHHmmss")
                    };
                    db.nTRN_PRECHARGE_HEADERs.Add(header);
                    db.SaveChanges();
                    var preCardPay = new NTRN_PRECHARGE_CARD()
                    {
                        SHOP_CODE = DataLocals.AppConfig.PosInfo.StoreNo,
                        SALE_DATE = DataLocals.PosStatus.SALE_DATE,
                        POS_NO = DataLocals.AppConfig.PosInfo.PosNo,
                        CST_NO = mEMBER_CLASH.mbrCode,
                        SALE_NO = newSaleNo,
                        SEQ_NO = "0001",
                        REGI_SEQ = DataLocals.PosStatus.REGI_SEQ,
                        CHARGE_YN = isCancelled ? "N" : "Y",
                        APPR_PROC_FLAG = tRN_CARD.APPR_PROC_FLAG,
                        CRD_CARD_NO = tRN_CARD.CRD_CARD_NO,
                        VALID_TERM = tRN_CARD.VALID_TERM,
                        APPR_REQ_AMT = tRN_CARD.APPR_REQ_AMT,
                        APPR_AMT = tRN_CARD.APPR_AMT,
                        APPR_DC_AMT = tRN_CARD.APPR_DC_AMT,
                        SVC_TIP_AMT = tRN_CARD.SVC_TIP_AMT,
                        VAT_AMT = tRN_CARD.VAT_AMT,
                        INST_MM_FLAG = tRN_CARD.INST_MM_FLAG,
                        INST_MM_CNT = tRN_CARD.INST_MM_CNT,
                        SIGN_PAD_YN = tRN_CARD.SIGN_PAD_YN,
                        CARD_IN_FLAG = tRN_CARD.CARD_IN_FLAG,
                        APPR_FLAG = tRN_CARD.APPR_FLAG,
                        APPR_DATE = tRN_CARD.APPR_DATE,
                        APPR_TIME = tRN_CARD.APPR_TIME,
                        APPR_NO = tRN_CARD.APPR_NO,
                        APPR_MSG = tRN_CARD.APPR_MSG,
                        VAN_CODE = tRN_CARD.VAN_CODE,
                        CRDCP_CODE = tRN_CARD.CRDCP_CODE,
                        ISS_CRDCP_CODE = tRN_CARD.ISS_CRDCP_CODE,
                        ISS_CRDCP_NAME = tRN_CARD.ISS_CRDCP_NAME,
                        PUR_CRDCP_CODE = tRN_CARD.PUR_CRDCP_CODE,
                        PUR_CRDCP_NAME = tRN_CARD.PUR_CRDCP_NAME,
                        VAN_TERM_NO = tRN_CARD.VAN_TERM_NO,
                        VAN_SLIP_NO = tRN_CARD.VAN_SLIP_NO,
                        CRDCP_TERM_NO = tRN_CARD.CRDCP_TERM_NO,
                        ORG_APPR_DATE = tRN_CARD.ORG_APPR_DATE,
                        ORG_APPR_NO = tRN_CARD.ORG_APPR_NO,
                        APPR_LOG_NO = tRN_CARD.APPR_LOG_NO,
                        INSERT_DT = tRN_CARD.INSERT_DT
                    };

                    preCardPay.APPR_LOG_NO = "";
                    preCardPay.CRDCP_CODE = "";
                    preCardPay.APPR_PROC_FLAG = "2";
                    preCardPay.VALID_TERM = "";
                    db.nTRN_PRECHARGE_CARDs.Add(preCardPay);
                    db.SaveChanges();

                    /// 
                    /// SALE_NO
                    /// 
                    var posStatus = db.pOS_STATUSes.FirstOrDefault(p => p.SHOP_CODE ==
                                DataLocals.AppConfig.PosInfo.StoreNo && p.POS_NO == DataLocals.AppConfig.PosInfo.PosNo &&
                                p.REGI_SEQ == DataLocals.PosStatus.REGI_SEQ && p.SALE_DATE == DataLocals.PosStatus.SALE_DATE);
                    posStatus.SALE_NO = newSaleNo;

                    /// 
                    /// 정산 - SET_POS_ACCOUNT
                    /// 
                    var posAcct = db.sETT_POSACCOUNTs.FirstOrDefault(p => p.SHOP_CODE ==
                                DataLocals.AppConfig.PosInfo.StoreNo && p.POS_NO == DataLocals.AppConfig.PosInfo.PosNo &&
                                p.SALE_DATE == DataLocals.PosStatus.SALE_DATE && p.REGI_SEQ == DataLocals.PosStatus.REGI_SEQ);
                    int addMin = preCardPay.CHARGE_YN == "N" ? -1 : 1;
                    posAcct.PRE_PNT_SALE_CRD_AMT += addMin * preCardPay.APPR_AMT;

                    db.SaveChanges();
                    tran.Commit();

                    DataLocals.PosStatus = posStatus;

                    var newPostRes = pOSTMangService.AddPOSMangNonTran(db, newSaleNo).Result;
                    if (newPostRes.Item1.ResultType != EResultType.SUCCESS)
                    {
                        throw new Exception(newPostRes.Item1.ResultMessage);
                    }

                    db.SaveChanges();

                    returnValue = new Tuple<string, bool>(newSaleNo, true);

                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    LogHelper.Logger.Error(ex.ToFormattedString());
                    errorMessage = ex.Message;
                }

            }
            return returnValue;
        }



        public async Task<(SpResult, POS_POST_MANG)> SaveNonTrnPrechargeAndHeader(NTRN_PRECHARGE_HEADER nTrnHeader, NTRN_PRECHARGE_CARD[] nTrnCard)
        {
            var result = new SpResult()
            {
                ResultType = EResultType.SUCCESS,
                ResultCode = "0000"
            };
            using (var db = new DataContext())
            {
                using (var trans = db.Database.BeginTransaction())
                {
                    try
                    {
                        var newSaleNo = DataLocals.PosStatus.SALE_NO.StrIntInc(4);
                        nTrnHeader.SALE_NO = newSaleNo;

                        nTrnHeader.CHARGE_YN = "N";
                        nTrnHeader.SALE_DATE = DataLocals.PosStatus.SALE_DATE;
                        nTrnHeader.REGI_SEQ = DataLocals.PosStatus.REGI_SEQ;
                        nTrnHeader.INSERT_DT = DateTime.Now.ToString(Formats.SystemDBDateTime);
                        db.nTRN_PRECHARGE_HEADERs.Add(nTrnHeader);
                        db.SaveChanges();

                        foreach ( var item in nTrnCard)
                        {
                            item.CHARGE_YN = "N";
                            item.SALE_NO=newSaleNo;
                        }
                        db.nTRN_PRECHARGE_CARDs.AddRange(nTrnCard);

                        db.SaveChanges();


                        var newPostRes = pOSTMangService.AddPOSMangNonTran(db, newSaleNo).Result;

                        if (newPostRes.Item1.ResultType != EResultType.SUCCESS)
                        {
                            throw new Exception(newPostRes.Item1.ResultMessage);
                        }

                        db.SaveChanges();

                        return (result, newPostRes.Item2);

                    }
                    catch (DbEntityValidationException ex)
                    {
                        trans.Rollback();
                        result.ResultType = EResultType.ERROR;
                        result.ResultCode = "9999";
                        LogHelper.Logger.Error(ex.ToFormattedString());
                        return (result, null);
                    }
                }
            }
        }

     
    }
}


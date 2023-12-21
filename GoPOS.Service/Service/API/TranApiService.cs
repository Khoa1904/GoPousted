using GoPOS.Database;
using GoPOS.Models;
using GoPOS.Models.Common;
using GoPOS.Models.Custom.API;
using GoPOS.Service.Common;
using GoShared.Helpers;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using GoPOS.Service.Service.POS;
using System.Reflection;

namespace GoPOS.Service.Service.API
{

    public class TranApiService : BaseDataService<POS_POST_MANG>, ITranApiService
    {
        private readonly IPOSTMangService iPOSTMangService;

        public TranApiService(IPOSTMangService iPOSTMangService)
        {
            this.iPOSTMangService = iPOSTMangService;
        }

        public IList<POS_POST_MANG> PickTranDataToSend(int count, int errorTimes)
        {
            IList<POS_POST_MANG> list = new List<POS_POST_MANG>();
            using (var db = new DataContext())
            {
                list = db.pOS_POST_MANGs.Where(p =>
                        p.POST_FLAG == "TR" &&
                        p.BILL_FLAG == "B" &&
                        "N".Equals(p.SEND_FLAG) && (p.ERR_CNT < errorTimes)
                        ).OrderBy(p => p.POS_NO).ThenBy(p =>
                        p.SALE_DATE).ThenBy(p => p.REGI_SEQ).ThenBy(p => p.BILL_FLAG).ThenBy(p => p.BILL_NO).Take(count).ToList();

            }

            return list;
        }

        public IList<POS_POST_MANG> PickNonTranDataToSend(int count, int errorTimes)
        {
            IList<POS_POST_MANG> list = new List<POS_POST_MANG>();
            using (var db = new DataContext())
            {
                list = db.pOS_POST_MANGs.Where(p =>
                        p.POST_FLAG == "NP" &&
                        "N".Equals(p.SEND_FLAG) && (p.ERR_CNT < errorTimes)
                        ).OrderBy(p => p.POS_NO).ThenBy(p =>
                        p.SALE_DATE).ThenBy(p => p.REGI_SEQ).ThenBy(p => p.BILL_FLAG).ThenBy(p => p.BILL_NO).Take(count).ToList();
            }
            return list;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="count"></param>
        /// <param name="errorTimes"></param>
        /// <returns></returns>
        public IList<POS_POST_MANG> PickSettAccDataToSend(int count, int errorTimes)
        {
            IList<POS_POST_MANG> list = new List<POS_POST_MANG>();
            using (var db = new DataContext())
            {
                if (DataLocals.AppConfig.PosOption.POSFlag != "0")
                {
                    list = db.pOS_POST_MANGs.Where(p =>
                            p.SHOP_CODE == DataLocals.AppConfig.PosInfo.StoreNo &&
                            p.POS_NO == DataLocals.AppConfig.PosInfo.PosNo &&
                            p.POST_FLAG == "AC" &&
                            ("N".Equals(p.SEND_FLAG) || "E".Equals(p.SEND_FLAG))
                            ).OrderBy(p => p.POS_NO).ThenBy(p =>
                            p.SALE_DATE).ThenBy(p => p.BILL_NO).ThenBy(p => p.BILL_FLAG).Take(count).ToList();
                }
                else
                {
                    list = db.pOS_POST_MANGs.Where(p =>
                            p.SHOP_CODE == DataLocals.AppConfig.PosInfo.StoreNo &&
                            p.POST_FLAG == "AC" &&
                            ("N".Equals(p.SEND_FLAG) || "E".Equals(p.SEND_FLAG))
                            ).OrderBy(p => p.POS_NO).ThenBy(p =>
                            p.SALE_DATE).ThenBy(p => p.BILL_NO).ThenBy(p => p.BILL_FLAG).Take(count).ToList();
                }

            }

            return list;
        }

        public IList<POS_POST_MANG> PickPointsToSend(int count, int errorTimes)
        {
            IList<POS_POST_MANG> list = new List<POS_POST_MANG>();
            using (var db = new DataContext())
            {
                    list = db.pOS_POST_MANGs.Where(p =>
                            p.SHOP_CODE == DataLocals.AppConfig.PosInfo.StoreNo &&
                            p.POST_FLAG == "PT" &&
                            ("N".Equals(p.SEND_FLAG) || "E".Equals(p.SEND_FLAG))
                            ).OrderBy(p => p.POS_NO).ThenBy(p =>
                            p.SALE_DATE).ThenBy(p => p.BILL_NO).ThenBy(p => p.BILL_FLAG).Take(count).ToList();
            }

            return list;
        }
        public TranData GetTRData(string shopCode, string posNo, string saleDate, string regiSeq, string billNo)
        {
            TranData tranData = null;
            using (var db = new DataContext())
            {
                tranData = new TranData()
                {
                    SHOP_CODE = shopCode,
                    POS_NO = posNo,
                    SALE_DATE = saleDate,
                    BILL_NO = billNo,
                    TranHeader = db.tRN_HEADERs.FirstOrDefault(p => p.SHOP_CODE == shopCode && p.POS_NO == posNo &&
                                p.SALE_DATE == saleDate && p.REGI_SEQ == regiSeq && p.BILL_NO == billNo)

                };

                var tranProduct = db.tRN_PRDTs.Where(p => p.SHOP_CODE == shopCode && p.POS_NO == posNo &&
                                 p.SALE_DATE == saleDate && p.REGI_SEQ == regiSeq && p.BILL_NO == billNo).ToArray();
                if (tranProduct != null && tranProduct.Any()) tranData.TranProduct = tranProduct;

                var tranTenderSeq = db.tRN_TENDERSEQs.Where(p => p.SHOP_CODE == shopCode && p.POS_NO == posNo &&
                           p.SALE_DATE == saleDate && p.REGI_SEQ == regiSeq && p.BILL_NO == billNo).ToArray();
                if (tranTenderSeq != null && tranTenderSeq.Any()) tranData.TranTenderSeq = tranTenderSeq;

                var tranCash = db.tRN_CASHes.Where(p => p.SHOP_CODE == shopCode && p.POS_NO == posNo &&
                           p.SALE_DATE == saleDate && p.REGI_SEQ == regiSeq && p.BILL_NO == billNo).ToArray();
                if (tranCash != null && tranCash.Any()) tranData.TranCash = tranCash;

                var tranCashRec = db.tRN_CASHRECs.Where(p => p.SHOP_CODE == shopCode && p.POS_NO == posNo &&
                           p.SALE_DATE == saleDate && p.REGI_SEQ == regiSeq && p.BILL_NO == billNo).ToArray();
                if (tranCashRec != null && tranCashRec.Any()) tranData.TranCashRec = tranCashRec;

                var tranCard = db.tRN_CARDs.Where(p => p.SHOP_CODE == shopCode && p.POS_NO == posNo &&
                           p.SALE_DATE == saleDate && p.REGI_SEQ == regiSeq && p.BILL_NO == billNo).ToArray();
                if (tranCard != null && tranCard.Any()) tranData.TranCard = tranCard;

                var tranPartnerCard = db.tRN_PARTCARDs.Where(p => p.SHOP_CODE == shopCode && p.POS_NO == posNo &&
                           p.SALE_DATE == saleDate && p.REGI_SEQ == regiSeq && p.BILL_NO == billNo).ToArray();
                if (tranPartnerCard != null && tranPartnerCard.Any()) tranData.TranPartnerCard = tranPartnerCard;

                var tranGift = db.tRN_GIFTs.Where(p => p.SHOP_CODE == shopCode && p.POS_NO == posNo &&
                           p.SALE_DATE == saleDate && p.REGI_SEQ == regiSeq && p.BILL_NO == billNo).ToArray();
                if (tranGift != null && tranGift.Any()) tranData.TranGift = tranGift;

                var tranFoodCpn = db.tRN_FOODCPNs.Where(p => p.SHOP_CODE == shopCode && p.POS_NO == posNo &&
                           p.SALE_DATE == saleDate && p.REGI_SEQ == regiSeq && p.BILL_NO == billNo).ToArray();
                if (tranFoodCpn != null && tranFoodCpn.Any()) tranData.TranFoodCpn = tranFoodCpn;

                var tranEasyPay = db.tRN_EASYPAYs.Where(p => p.SHOP_CODE == shopCode && p.POS_NO == posNo &&
                            p.SALE_DATE == saleDate && p.REGI_SEQ == regiSeq && p.BILL_NO == billNo).ToArray();
                if (tranEasyPay != null && tranEasyPay.Any()) tranData.TranEasyPay = tranEasyPay;

                var tranPointused = db.tRN_POINTUSEs.Where(p => p.SHOP_CODE == shopCode && p.POS_NO == posNo &&
                         p.SALE_DATE == saleDate && p.REGI_SEQ == regiSeq && p.BILL_NO == billNo).ToArray();
                if (tranPointused != null && tranPointused.Any()) tranData.TranPointuse = tranPointused;

                var tranPointsave = db.tRN_POINTSAVEs.Where(p => p.SHOP_CODE == shopCode && p.POS_NO == posNo &&
                        p.SALE_DATE == saleDate && p.REGI_SEQ == regiSeq && p.BILL_NO == billNo).ToArray();
                if (tranPointsave != null && tranPointsave.Any()) tranData.TranPointSave = tranPointsave[0];

                var tranPpCard = db.tRN_PPCARDs.Where(p => p.SHOP_CODE == shopCode && p.POS_NO == posNo &&
                        p.SALE_DATE == saleDate && p.REGI_SEQ == regiSeq && p.BILL_NO == billNo).ToArray();
                if (tranPpCard != null && tranPpCard.Any()) tranData.TranPpCard = tranPpCard;
            }

            return tranData;
        }

        public TranData GetPointsData(string shopCode, string posNo, string saleDate, string regiSeq, string billNo)
        {
            TranData tranData = null;
            using (var db = new DataContext())
            {
                tranData = new TranData()
                {
                    SHOP_CODE = shopCode,
                    POS_NO = posNo,
                    SALE_DATE = saleDate,
                    BILL_NO = billNo,
                };

                var tranPTsave = db.tRN_POINTSAVEs.Where(p => p.SHOP_CODE == shopCode && p.POS_NO == posNo &&
                                 p.SALE_DATE == saleDate && p.REGI_SEQ == regiSeq && p.BILL_NO == billNo).ToArray();
                if (tranPTsave != null && tranPTsave.Any()) tranData.TranPointSave = tranPTsave[0];

                var tranPTuse = db.tRN_POINTUSEs.Where(p => p.SHOP_CODE == shopCode && p.POS_NO == posNo &&
                           p.SALE_DATE == saleDate && p.REGI_SEQ == regiSeq && p.BILL_NO == billNo).ToArray();
                if (tranPTuse != null) tranData.TranPointuse = tranPTuse;
            }
            return tranData;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="orgJsonData"></param>
        /// <param name="billFlag"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public SpResult SaveTRDataByJson(string orgJsonData)
        {
            DbContextTransaction tran = null;
            try
            {
                LogHelper.Logger.Trace("TR Uploaded: " + orgJsonData);
                var tranData = JsonConvert.DeserializeObject<TranData>(orgJsonData);
                using (var db = new DataContext())
                {
                    var check = db.tRN_HEADERs.Any(t => t.SHOP_CODE == tranData.TranHeader.SHOP_CODE &&
                    t.SALE_DATE == tranData.TranHeader.SALE_DATE &&
                    t.POS_NO == tranData.TranHeader.POS_NO &&
                    t.BILL_NO == tranData.TranHeader.BILL_NO);

                    if (!check)
                    {


                        /// need try catch at each object table to return error code
                        tran = db.Database.BeginTransaction();
                        if (tranData.TranHeader != null)
                        {
                            db.tRN_HEADERs.Add(tranData.TranHeader);
                        }
                        if (tranData.TranProduct != null && tranData.TranProduct.Any())
                        {
                            db.tRN_PRDTs.AddRange(tranData.TranProduct);
                        }
                        if (tranData.TranTenderSeq != null && tranData.TranTenderSeq.Any())
                        {
                            db.tRN_TENDERSEQs.AddRange(tranData.TranTenderSeq);
                        }
                        if (tranData.TranCash != null && tranData.TranCash.Any())
                        {
                            db.tRN_CASHes.AddRange(tranData.TranCash);
                        }
                        if (tranData.TranCashRec != null && tranData.TranCashRec.Any())
                        {
                            db.tRN_CASHRECs.AddRange(tranData.TranCashRec);
                        }
                        if (tranData.TranCard != null && tranData.TranCard.Any())
                        {
                            db.tRN_CARDs.AddRange(tranData.TranCard);
                        }
                        if (tranData.TranPartnerCard != null && tranData.TranPartnerCard.Any())
                        {
                            db.tRN_PARTCARDs.AddRange(tranData.TranPartnerCard);
                        }
                        if (tranData.TranGift != null && tranData.TranGift.Any())
                        {
                            db.tRN_GIFTs.AddRange(tranData.TranGift);
                        }
                        if (tranData.TranFoodCpn != null && tranData.TranFoodCpn.Any())
                        {
                            db.tRN_FOODCPNs.AddRange(tranData.TranFoodCpn);
                        }
                        if (tranData.TranEasyPay != null && tranData.TranEasyPay.Any())
                        {
                            db.tRN_EASYPAYs.AddRange(tranData.TranEasyPay);
                        }
                        if (tranData.TranPointSave != null)
                        {
                            db.tRN_POINTSAVEs.Add(tranData.TranPointSave);
                        }
                        if (tranData.TranPointuse != null && tranData.TranPointuse.Any())
                        {
                            db.tRN_POINTUSEs.AddRange(tranData.TranPointuse);
                        }
                        // update table POS_POST_MANG
                        var newPostRes = iPOSTMangService.AddFromTrnHeader(db, tranData.TranHeader, false).Result;
                        if (newPostRes.Item1.ResultType != EResultType.SUCCESS)
                        {
                            throw new Exception(newPostRes.Item1.ResultMessage);
                        }

                        db.SaveChanges();
                        tran.Commit();

                        LogHelper.Logger.Trace(string.Format("TR Uploaded saved {0} {1} {2}", tranData.POS_NO,
                                tranData.BILL_NO, tranData.SALE_DATE));

                        return new SpResult()
                        {
                            ResultCode = ResultCode.Success,
                            ResultType = EResultType.SUCCESS
                        };
                    }
                    else
                    {
                        return new SpResult()
                        {
                            ResultCode = ResultCode.Success,
                            ResultType = EResultType.SUCCESS,
                            ResultMessage = "Data is Exsited."
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                tran.Rollback();
                tran.Dispose();

                LogHelper.Logger.Error(string.Format("TR Uploaded errored {0}", ex.ToFormattedString()));

                return new SpResult()
                {
                    ResultCode = ResultCode.Fail,
                    ResultType = EResultType.ERROR,
                    ResultMessage = ex.Message
                };
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pOS_POST_MANG"></param>
        /// <param name="res"></param>
        /// <returns></returns>
        public Task<bool> UpdateTranPOSTMangStatus(POS_POST_MANG pOS_POST_MANG, ApiResponse res)
        {
            var db = new DataContext();
            var postMang = db.pOS_POST_MANGs.FirstOrDefault(p =>
                        p.SHOP_CODE == pOS_POST_MANG.SHOP_CODE &&
                        p.POS_NO == pOS_POST_MANG.POS_NO &&
                        p.SALE_DATE == pOS_POST_MANG.SALE_DATE &&
                        p.REGI_SEQ == pOS_POST_MANG.REGI_SEQ &&
                        p.POST_FLAG == pOS_POST_MANG.POST_FLAG &&
                        p.BILL_FLAG == pOS_POST_MANG.BILL_FLAG &&
                        p.BILL_NO == pOS_POST_MANG.BILL_NO);

            if (res.status == ResultCode.Success)
            {
                postMang.SEND_FLAG = ((char)ESend_Flag.Yes).ToString();
                postMang.SEND_DT = DateTime.Now.ToString("yyyyMMddHHmmss");
                postMang.ERR_CODE = string.Empty;
                postMang.ERR_MSG = string.Empty;
            }
            else
            {
                var errorCode = "9999";
                if (res.error != null) errorCode = res.error.code;
                postMang.SEND_FLAG = ((char)ESend_Flag.Error).ToString();
                postMang.ERR_CODE = errorCode.Substring(0, Math.Min(10, errorCode.Length));
                string errorMsg = res.results;
                postMang.ERR_MSG = errorMsg.Substring(0, Math.Min(200, errorMsg.Length));
                postMang.ERR_CNT = ((pOS_POST_MANG.ERR_CNT ?? 0) + 1);
            }


            try
            {
                db.SaveChanges();
                return Task.FromResult(true);
            }
            catch (Exception ex)
            {
                LogHelper.Logger.Error(ex.ToFormattedString());
                return Task.FromResult(false);
            }
        }

        /// <summary>
        /// Save to POS_SETTACCOUNT
        /// </summary>
        /// <param name="orgJsonData"></param>
        /// <returns></returns>
        public SpResult SaveSettAccountByJson(string orgJsonData)
        {
            try
            {
                LogHelper.Logger.Trace("ACCOUNTTR Uploaded: " + orgJsonData);
                var model = JsonConvert.DeserializeObject<AccountModel>(orgJsonData);
                if (model != null)
                {
                    using (var db = new DataContext())
                    {
                        using (var tran = db.Database.BeginTransaction())
                        {
                            try
                            {
                                var posAcc = db.sETT_POSACCOUNTs.FirstOrDefault(p => p.SHOP_CODE == model.NTranAccount.SHOP_CODE &&
                                    p.POS_NO == model.NTranAccount.POS_NO && p.SALE_DATE == model.NTranAccount.SALE_DATE &&
                                    p.REGI_SEQ == model.NTranAccount.REGI_SEQ);
                                if (posAcc == null)
                                {
                                    db.sETT_POSACCOUNTs.Add(model.NTranAccount);
                                }
                                else
                                {
                                    posAcc.CopyFieldsFrom<SETT_POSACCOUNT>(model.NTranAccount, null);
                                }

                                // update table POS_POST_MANG
                                var setAccRes = iPOSTMangService.AddFromSettAccount(db, model.NTranAccount).Result;
                                if (setAccRes.Item1.ResultType != EResultType.SUCCESS)
                                {
                                    throw new Exception(setAccRes.Item1.ResultMessage);
                                }

                                db.SaveChanges();
                                tran.Commit();
                                LogHelper.Logger.Trace("ACCOUNTTR Uploaded: Ok");

                                return new SpResult()
                                {
                                    ResultCode = "200",
                                    ResultType = EResultType.SUCCESS
                                };
                            }
                            catch (Exception ex)
                            {
                                tran.Rollback();
                                LogHelper.Logger.Error("ACCOUNTTR Uploaded: Error", ex.ToFormattedString());
                                return new SpResult()
                                {
                                    ResultCode = "9999",
                                    ResultType = EResultType.ERROR,
                                    ResultMessage = ex.Message
                                };
                            }

                        }
                    }
                }
                else
                {
                    return new SpResult()
                    {
                        ResultCode = "9999",
                        ResultType = EResultType.ERROR,
                        ResultMessage = "format json invalid."
                    };
                }

            }
            catch (Exception ex)
            {
                LogHelper.Logger.Error(ex.ToFormattedString());
                return new SpResult()
                {
                    ResultCode = "9999",
                    ResultType = EResultType.ERROR,
                    ResultMessage = ex.Message
                };
            }
        }
        /// <summary>
        /// Save to POS_SETTACCOUNT
        /// </summary>
        /// <param name="orgJsonData"></param>
        /// <returns></returns>
        //public SpResult SavePointDataByJson(string orgJsonData)
        //{
        //    try
        //    {
        //        LogHelper.Logger.Info("Point Uploaded: " + orgJsonData);
        //        var model = JsonConvert.DeserializeObject<PointModels>(orgJsonData);
        //        if (model != null)
        //        {
        //            using (var db = new DataContext())
        //            {
        //                using (var tran = db.Database.BeginTransaction())
        //                {
        //                    try
        //                    {
        //                        var posAcc = db.sETT_POSACCOUNTs.FirstOrDefault(p => p.SHOP_CODE == model.NTranAccount.SHOP_CODE &&
        //                            p.POS_NO == model.NTranAccount.POS_NO && p.SALE_DATE == model.NTranAccount.SALE_DATE &&
        //                            p.REGI_SEQ == model.NTranAccount.REGI_SEQ);
        //                        if (posAcc == null)
        //                        {
        //                            db.sETT_POSACCOUNTs.Add(model.NTranAccount);
        //                        }
        //                        else
        //                        {
        //                            posAcc.CopyFieldsFrom<SETT_POSACCOUNT>(model.NTranAccount, null);
        //                        }

        //                        // update table POS_POST_MANG
        //                        var setAccRes = iPOSTMangService.AddFromSettAccount(db, model.NTranAccount).Result;
        //                        if (setAccRes.Item1.ResultType != EResultType.SUCCESS)
        //                        {
        //                            throw new Exception(setAccRes.Item1.ResultMessage);
        //                        }

        //                        db.SaveChanges();
        //                        tran.Commit();
        //                        LogHelper.Logger.Info("ACCOUNTTR Uploaded: Ok");

        //                        return new SpResult()
        //                        {
        //                            ResultCode = "200",
        //                            ResultType = EResultType.SUCCESS
        //                        };
        //                    }
        //                    catch (Exception ex)
        //                    {
        //                        tran.Rollback();
        //                        LogHelper.Logger.Info("ACCOUNTTR Uploaded: Error", ex.ToFormattedString());
        //                        return new SpResult()
        //                        {
        //                            ResultCode = "9999",
        //                            ResultType = EResultType.ERROR,
        //                            ResultMessage = ex.Message
        //                        };
        //                    }

        //                }
        //            }
        //        }
        //        else
        //        {
        //            return new SpResult()
        //            {
        //                ResultCode = "9999",
        //                ResultType = EResultType.ERROR,
        //                ResultMessage = "format json invalid."
        //            };
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        LogHelper.Logger.Error(ex.ToFormattedString());
        //        return new SpResult()
        //        {
        //            ResultCode = "9999",
        //            ResultType = EResultType.ERROR,
        //            ResultMessage = ex.Message
        //        };
        //    }
        //}
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="postMang"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public TranAccount GetTranAccount(POS_POST_MANG postMang)
        {
            using (var db = new DataContext())
            {
                //string regiSeq = postMang.BILL_NO == "99" ? "00" : postMang.BILL_NO;
                return new TranAccount()
                {
                    NTranAccount = db.sETT_POSACCOUNTs.FirstOrDefault(p =>
                            p.SHOP_CODE == postMang.SHOP_CODE &&
                            p.POS_NO == postMang.POS_NO &&
                            p.SALE_DATE == postMang.SALE_DATE &&
                            p.REGI_SEQ == postMang.REGI_SEQ)
                };
            }
        }

        public NonTransModel GetNonTRData(string shopCode, string posNo, string saleDate, string regiSeq, string saleno)
        {
            NonTransModel nonTranData = null;
            using (var db = new DataContext())
            {
                nonTranData = new NonTransModel()
                {
                    SHOP_CODE = shopCode,
                    POS_NO = posNo,
                    SALE_DATE = saleDate,
                    SALE_NO=saleno
                };

                var ntranPreChargeHeader = db.nTRN_PRECHARGE_HEADERs.Where(p => p.SHOP_CODE == shopCode && p.POS_NO == posNo &&
                   p.SALE_DATE == saleDate && p.REGI_SEQ == regiSeq && p.SALE_NO==saleno).ToArray();
                if (ntranPreChargeHeader != null && ntranPreChargeHeader.Any()) nonTranData.NTranPreChargeHeader = ntranPreChargeHeader[0];

                nonTranData.NTranPreChargeCard = db.nTRN_PRECHARGE_CARDs.Where(p => p.SHOP_CODE == shopCode && p.POS_NO == posNo &&
                        p.SALE_DATE == saleDate && p.REGI_SEQ == regiSeq && p.SALE_NO==saleno).ToArray();
            }
            return nonTranData;
        }

        public SpResult SaveNTRDataByJson(string orgJsonData)
        {
            DbContextTransaction tran = null;
            try
            {
                LogHelper.Logger.Trace("NTR Uploaded: " + orgJsonData);
                var tranData = JsonConvert.DeserializeObject<TranData>(orgJsonData);
                using (var db = new DataContext())
                {
                    var check = db.nTRN_PRECHARGE_HEADERs.Any(t => t.SHOP_CODE == tranData.SHOP_CODE &&
                    t.SALE_DATE == tranData.SALE_DATE &&
                    t.POS_NO == tranData.POS_NO &&
                    t.SALE_NO == tranData.BILL_NO);

                    if (!check)
                    {

                        /// need try catch at each object table to return error code
                        tran = db.Database.BeginTransaction();
                        if (tranData.NTranPreChargeHeader != null)
                        {
                            db.nTRN_PRECHARGE_HEADERs.Add(tranData.NTranPreChargeHeader);
                        }
                        if (tranData.NTranPreChargeCard != null && tranData.TranProduct.Any())
                        {
                            db.nTRN_PRECHARGE_CARDs.AddRange(tranData.NTranPreChargeCard);
                        }

                        db.SaveChanges();
                        tran.Commit();

                        LogHelper.Logger.Trace(string.Format("NTR Uploaded saved {0} {1} {2}", tranData.POS_NO,
                                tranData.BILL_NO, tranData.SALE_DATE));

                        return new SpResult()
                        {
                            ResultCode = ResultCode.Success,
                            ResultType = EResultType.SUCCESS
                        };
                    }
                    else
                    {
                        return new SpResult()
                        {
                            ResultCode = ResultCode.Success,
                            ResultType = EResultType.SUCCESS,
                            ResultMessage = "Data is Exsited."
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                tran.Rollback();
                tran.Dispose();

                LogHelper.Logger.Error(string.Format("TR Uploaded errored {0}", ex.ToFormattedString()));

                return new SpResult()
                {
                    ResultCode = ResultCode.Fail,
                    ResultType = EResultType.ERROR,
                    ResultMessage = ex.Message
                };
            }
        }
    }
}

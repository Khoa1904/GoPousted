using CoreWCF.Channels;
using GoPOS.Models;
using GoPOS.Models.Common;
using GoPOS.Service.Common;
using GoShared.Helpers;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;

namespace GoPOS.Service.Service.POS
{
    public class POSTMangService : BaseDataService<POS_POST_MANG>, IPOSTMangService
    {
        public Task<(SpResult, POS_POST_MANG)> AddFromSettAccount(DataContext db, SETT_POSACCOUNT setPOSAcc)
        {
            SpResult res = new SpResult()
            {
                ResultType = EResultType.SUCCESS
            };

            POS_POST_MANG postMang = null;
            try
            {
                string billNo = setPOSAcc.REGI_SEQ == "00" ? "99" : setPOSAcc.REGI_SEQ;
                postMang = db.pOS_POST_MANGs.FirstOrDefault(p => p.SHOP_CODE == setPOSAcc.SHOP_CODE &&
                        p.POS_NO == setPOSAcc.POS_NO &&
                        p.SALE_DATE == setPOSAcc.SALE_DATE &&
                        p.REGI_SEQ == setPOSAcc.REGI_SEQ &&
                        p.POST_FLAG == "AC" &&
                        p.BILL_FLAG == setPOSAcc.CLOSE_FLAG &&
                        p.BILL_NO == billNo);
                if (postMang == null)
                {
                    postMang = new POS_POST_MANG()
                    {
                        SHOP_CODE = setPOSAcc.SHOP_CODE,
                        POS_NO = setPOSAcc.POS_NO,
                        SALE_DATE = setPOSAcc.SALE_DATE,
                        REGI_SEQ = setPOSAcc.REGI_SEQ,
                        POST_FLAG = "AC",
                        BILL_FLAG = setPOSAcc.CLOSE_FLAG,
                        BILL_NO = billNo,
                        EMP_NO = setPOSAcc.EMP_NO,
                        SEND_FLAG = "N",
                        SEND_DT = string.Empty,
                        ERR_CNT = 0
                    };

                    db.pOS_POST_MANGs.Add(postMang);
                }
            } catch (Exception ex)
            {
                res.ResultMessage = ex.Message;
                res.ResultType = EResultType.ERROR;
                LogHelper.Logger.Error(ex.ToFormattedString());
            }

            return Task.FromResult((res, postMang));
        }

        public Task<(SpResult, POS_POST_MANG?)> AddFromTrnHeader(DataContext db, TRN_HEADER trnHeader, bool isKDS)
        {
            SpResult res = new SpResult()
            {
                ResultType = EResultType.SUCCESS
            };

            POS_POST_MANG postMang = null;
            try
            {
                postMang = db.pOS_POST_MANGs.FirstOrDefault(p => p.SHOP_CODE == trnHeader.SHOP_CODE &&
                        p.POS_NO == trnHeader.POS_NO &&
                        p.SALE_DATE == trnHeader.SALE_DATE &&
                        p.REGI_SEQ == trnHeader.REGI_SEQ &&
                        p.POST_FLAG == "TR" &&
                        p.BILL_FLAG == "B" &&
                        p.BILL_NO == trnHeader.BILL_NO);
                if (postMang == null)
                {
                    postMang = new POS_POST_MANG()
                    {
                        SHOP_CODE = trnHeader.SHOP_CODE,
                        POS_NO = trnHeader.POS_NO,
                        SALE_DATE = trnHeader.SALE_DATE,
                        REGI_SEQ = trnHeader.REGI_SEQ,
                        POST_FLAG = isKDS ? "KD" : "TR",
                        BILL_FLAG = "B",
                        BILL_NO = trnHeader.BILL_NO,
                        EMP_NO = trnHeader.EMP_NO,
                        SEND_FLAG = "N",
                        SEND_DT = string.Empty,
                        ERR_CNT = 0
                    };

                    db.pOS_POST_MANGs.Add(postMang);
                }
            } catch (Exception ex)
            {
                res.ResultMessage = ex.Message;
                res.ResultType = EResultType.ERROR;
                LogHelper.Logger.Error(ex.ToFormattedString());
            }
            return Task.FromResult((res, postMang));
        }

        public Task<(SpResult, POS_POST_MANG)> AddPOSMangNonTran(DataContext db, string saleNo)
        {
            SpResult res = new SpResult()
            {
                ResultType = EResultType.SUCCESS
            };

            POS_POST_MANG postMang = null;
            try
            {
                postMang = db.pOS_POST_MANGs.FirstOrDefault(p => p.SHOP_CODE == DataLocals.AppConfig.PosInfo.StoreNo &&
                                                                 p.POS_NO == DataLocals.AppConfig.PosInfo.PosNo &&
                                                                 p.SALE_DATE == DataLocals.PosStatus.SALE_DATE &&
                                                                 p.REGI_SEQ == DataLocals.PosStatus.REGI_SEQ &&
                                                                 p.POST_FLAG == "NP" &&
                                                                 p.BILL_FLAG == "B" &&
                                                                 p.BILL_NO == saleNo);
                if (postMang == null)
                {
                    postMang = new POS_POST_MANG()
                    {
                        SHOP_CODE = DataLocals.AppConfig.PosInfo.StoreNo,
                        POS_NO = DataLocals.AppConfig.PosInfo.PosNo,
                        SALE_DATE = DataLocals.PosStatus.SALE_DATE,
                        REGI_SEQ = DataLocals.PosStatus.REGI_SEQ,
                        POST_FLAG = "NP",
                        BILL_FLAG = "B",
                        SEND_FLAG = "N",
                        EMP_NO = DataLocals.Employee.EMP_NO,
                        SEND_DT = string.Empty,
                        ERR_CNT = 0,
                        BILL_NO = saleNo
                    };

                    db.pOS_POST_MANGs.Add(postMang);
                }
            } catch (Exception ex)
            {
                res.ResultMessage = ex.Message;
                res.ResultType = EResultType.ERROR;
                LogHelper.Logger.Error(ex.ToFormattedString());
            }
            return Task.FromResult((res, postMang));
        }

        public Task<SpResult> AddPOSTMang(DataContext db, POS_POST_MANG pOS_POST_MANG)
        {
            SpResult res = new SpResult()
            {
                ResultType = EResultType.SUCCESS
            };
            try
            {
                db.pOS_POST_MANGs.AddOrUpdate(pOS_POST_MANG);
                db.SaveChanges();
            } catch (Exception ex)
            {
                var list = ex.GetAllExceptions();
                if (list != null && list.Any())
                {
                    foreach (var item in list)
                    {
                        res.ResultMessage += item.InnerException == null ? item.Message : item.InnerException.Message;
                        res.ResultType = EResultType.ERROR;
                    }
                } else
                {
                    res.ResultMessage = ex.InnerException == null ? ex.Message : ex.InnerException.Message;
                    res.ResultType = EResultType.ERROR;
                }
            }
            return Task.FromResult(res);
        }
        public Task<(SpResult, POS_POST_MANG)> AddPointRecord(DataContext db, TRN_POINTSAVE pointSave, SETT_POSACCOUNT settAcc)
        {
            SpResult res = new SpResult()
            {
                ResultType = EResultType.SUCCESS
            };

            POS_POST_MANG postMang = null;
            try
            {
                postMang = db.pOS_POST_MANGs.FirstOrDefault(p => p.SHOP_CODE == pointSave.SHOP_CODE &&
                        p.POS_NO == pointSave.POS_NO &&
                        p.SALE_DATE == pointSave.SALE_DATE &&
                        p.REGI_SEQ == pointSave.REGI_SEQ &&
                        p.POST_FLAG == "PTS" &&
                        p.BILL_FLAG == settAcc.CLOSE_FLAG &&
                        p.BILL_NO == pointSave.BILL_NO);
                if (postMang == null)
                {
                    postMang = new POS_POST_MANG()
                    {
                        SHOP_CODE = settAcc.SHOP_CODE,
                        POS_NO = settAcc.POS_NO,
                        SALE_DATE = settAcc.SALE_DATE,
                        REGI_SEQ = settAcc.REGI_SEQ,
                        POST_FLAG = "PT",
                        BILL_FLAG = settAcc.CLOSE_FLAG,
                        BILL_NO = pointSave.BILL_NO,
                        EMP_NO = settAcc.EMP_NO,
                        SEND_FLAG = "N",
                        SEND_DT = string.Empty,
                        ERR_CNT = 0
                    };

                    db.pOS_POST_MANGs.Add(postMang);
                }
            } catch (Exception ex)
            {
                res.ResultMessage = ex.Message;
                res.ResultType = EResultType.ERROR;
                LogHelper.Logger.Error(ex.ToFormattedString());
            }

            return Task.FromResult((res, postMang));
        }
    }
}

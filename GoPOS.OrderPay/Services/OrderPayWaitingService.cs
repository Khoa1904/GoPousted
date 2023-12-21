using Dapper;
using FirebirdSql.Data.FirebirdClient;
using GoPOS.Common.Helpers;
using GoPOS.Database;
using GoPOS.Helpers;
using GoPOS.Models;
using GoPOS.Models.Common;
using GoPOS.Service;
using GoShared.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Internal;
using NLog;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using static GoPOS.Function;

namespace GoPOS.Services;

public class OrderPayWaitingService : IOrderPayWaitingService
{
    #region 대기 / Waiting Orders

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public Task<ORD_WAIT_ITEM[]> LoadWaitOrders()
    {
        using (var db = new DataContext())
        {
            var ords = db.oRD_HEADERs.Where(p => p.DLV_ORDER_FLAG == "1").ToArray();
            List<ORD_WAIT_ITEM> ordList = new List<ORD_WAIT_ITEM>();
            for (int i = 0; i < ords.Length; i++)
            {
                ordList.Add(
                    new ORD_WAIT_ITEM()
                    {
                        SEQ = i + 1,
                        WAIT_NO = ords[i].ORDER_NO,
                        EXP_PAY_AMT = ords[i].TOT_SALE_AMT,
                        GST_PAY_AMT = ords[i].DCM_SALE_AMT,
                        INSERT_DT = ords[i].INSERT_DT
                    });
            }

            return Task.FromResult(ordList.ToArray());
        }
    }

    /// <summary>
    /// Load all ORD related tables to dictionary
    /// </summary>
    /// <param name="waitItem"></param>
    /// <returns></returns>
    public Task<Dictionary<string, object?>> LoadWaitTRData(ORD_WAIT_ITEM waitItem)
    {
        if (waitItem == null)
        {
            using (var db = new DataContext())
            {
                var order = db.oRD_HEADERs.FirstOrDefault(p => p.DLV_ORDER_FLAG == "1" &&
                                p.SHOP_CODE == DataLocals.AppConfig.PosInfo.StoreNo &&
                                p.POS_NO == DataLocals.PosStatus.POS_NO &&
                                p.SALE_DATE == DataLocals.PosStatus.SALE_DATE);
                if (order != null)
                {
                    waitItem = new ORD_WAIT_ITEM()
                    {
                        SEQ = 1,
                        WAIT_NO = order.ORDER_NO,
                        EXP_PAY_AMT = order.TOT_SALE_AMT,
                        GST_PAY_AMT = order.DCM_SALE_AMT,
                        INSERT_DT = order.INSERT_DT.FormatDateString()
                    };
                }
            }
        }

        Dictionary<string, object> trDatas = new();

        if (waitItem == null)
        {
            return Task.FromResult(trDatas);
        }

        trDatas.Add("WAIT_ORDER", waitItem);

        string sql = ResourceHelpers.LoadSqlCommand(SQLFileTypes.OrderPay, "GetHoldProducts");

        var prdList = DapperORM.ReturnListAsync<ORD_PRDT>(sql,
            new string[]
            {
                "@SHOP_CODE",
                "@POS_NO",
                "@SALE_DATE",
                "@ORDER_NO"
            },
            new object[] {
                DataLocals.AppConfig.PosInfo.StoreNo,
                DataLocals.PosStatus.POS_NO,
                DataLocals.PosStatus.SALE_DATE,
                waitItem.WAIT_NO
            }).Result;

        using (var db = new DataContext())
        {
            trDatas.Add(nameof(TRN_HEADER), db.oRD_HEADERs.FirstOrDefault(p => p.SHOP_CODE == DataLocals.AppConfig.PosInfo.StoreNo &&
                p.POS_NO == DataLocals.PosStatus.POS_NO &&
                p.SALE_DATE == DataLocals.PosStatus.SALE_DATE &&
                p.ORDER_NO == waitItem.WAIT_NO));

            trDatas.Add(nameof(TRN_PRDT), prdList.ToArray());

            trDatas.Add(nameof(TRN_TENDERSEQ), db.oRD_TENDERSEQs.Where(p => p.SHOP_CODE == DataLocals.AppConfig.PosInfo.StoreNo &&
                p.POS_NO == DataLocals.PosStatus.POS_NO &&
                p.SALE_DATE == DataLocals.PosStatus.SALE_DATE &&
                p.ORDER_NO == waitItem.WAIT_NO).ToArray());
            trDatas.Add(nameof(TRN_CASH), db.oRD_CASHes.Where(p => p.SHOP_CODE == DataLocals.AppConfig.PosInfo.StoreNo &&
                p.POS_NO == DataLocals.PosStatus.POS_NO &&
                p.SALE_DATE == DataLocals.PosStatus.SALE_DATE &&
                p.ORDER_NO == waitItem.WAIT_NO).ToArray());
            trDatas.Add(nameof(TRN_CARD), db.oRD_CARDs.Where(p => p.SHOP_CODE == DataLocals.AppConfig.PosInfo.StoreNo &&
                p.POS_NO == DataLocals.PosStatus.POS_NO &&
                p.SALE_DATE == DataLocals.PosStatus.SALE_DATE &&
                p.ORDER_NO == waitItem.WAIT_NO).ToArray());
            trDatas.Add(nameof(TRN_GIFT), db.oRD_GIFTs.Where(p => p.SHOP_CODE == DataLocals.AppConfig.PosInfo.StoreNo &&
                p.POS_NO == DataLocals.PosStatus.POS_NO &&
                p.SALE_DATE == DataLocals.PosStatus.SALE_DATE &&
                p.ORDER_NO == waitItem.WAIT_NO).ToArray());
            trDatas.Add(nameof(TRN_FOODCPN), db.oRD_FOODCPNs.Where(p => p.SHOP_CODE == DataLocals.AppConfig.PosInfo.StoreNo &&
                p.POS_NO == DataLocals.PosStatus.POS_NO &&
                p.SALE_DATE == DataLocals.PosStatus.SALE_DATE &&
                p.ORDER_NO == waitItem.WAIT_NO).ToArray());
            trDatas.Add(nameof(TRN_EASYPAY), db.oRD_EASYPAYs.Where(p => p.SHOP_CODE == DataLocals.AppConfig.PosInfo.StoreNo &&
                p.POS_NO == DataLocals.PosStatus.POS_NO &&
                p.SALE_DATE == DataLocals.PosStatus.SALE_DATE &&
                p.ORDER_NO == waitItem.WAIT_NO).ToArray());
            trDatas.Add(nameof(TRN_POINTUSE), db.oRD_POINTUSEs.Where(p => p.SHOP_CODE == DataLocals.AppConfig.PosInfo.StoreNo &&
                p.POS_NO == DataLocals.PosStatus.POS_NO &&
                p.SALE_DATE == DataLocals.PosStatus.SALE_DATE &&
                p.ORDER_NO == waitItem.WAIT_NO).ToArray());
            trDatas.Add(nameof(TRN_PPCARD), db.oRD_PPCARDs.Where(p => p.SHOP_CODE == DataLocals.AppConfig.PosInfo.StoreNo &&
                p.POS_NO == DataLocals.PosStatus.POS_NO &&
                p.SALE_DATE == DataLocals.PosStatus.SALE_DATE &&
                p.ORDER_NO == waitItem.WAIT_NO).ToArray());
        }

        return Task.FromResult(trDatas);
    }

    public Task<bool> HasWaitingTR()
    {
        using (var db = new DataContext())
        {
            var order = db.oRD_HEADERs.Count(p => p.DLV_ORDER_FLAG == "1" &&
                            p.SHOP_CODE == DataLocals.AppConfig.PosInfo.StoreNo &&
                            p.POS_NO == DataLocals.PosStatus.POS_NO &&
                            p.SALE_DATE == DataLocals.PosStatus.SALE_DATE);
            return Task.FromResult(order > 0);
        }
    }

    public void RemoveWaitingOrder(ORD_WAIT_ITEM waitItem)
    {
        using (var db = new DataContext())
        {
            DbContextTransaction trans = null;
            try
            {
                trans = db.Database.BeginTransaction();
                db.Database.ExecuteSqlCommand(string.Format("DELETE FROM ORD_HEADER WHERE SHOP_CODE = " +
                    "'{0}' AND POS_NO = '{1}' AND SALE_DATE = '{2}' AND ORDER_NO = '{3}'",
                    DataLocals.AppConfig.PosInfo.StoreNo, DataLocals.PosStatus.POS_NO, DataLocals.PosStatus.SALE_DATE, waitItem.WAIT_NO));
                db.Database.ExecuteSqlCommand(string.Format("DELETE FROM ORD_PRDT WHERE SHOP_CODE = " +
                    "'{0}' AND POS_NO = '{1}' AND SALE_DATE = '{2}' AND ORDER_NO = '{3}'",
                    DataLocals.AppConfig.PosInfo.StoreNo, DataLocals.PosStatus.POS_NO, DataLocals.PosStatus.SALE_DATE, waitItem.WAIT_NO));
                db.Database.ExecuteSqlCommand(string.Format("DELETE FROM ORD_TENDERSEQ WHERE SHOP_CODE = " +
                    "'{0}' AND POS_NO = '{1}' AND SALE_DATE = '{2}' AND ORDER_NO = '{3}'",
                    DataLocals.AppConfig.PosInfo.StoreNo, DataLocals.PosStatus.POS_NO, DataLocals.PosStatus.SALE_DATE, waitItem.WAIT_NO));
                db.Database.ExecuteSqlCommand(string.Format("DELETE FROM ORD_CASH WHERE SHOP_CODE = " +
                    "'{0}' AND POS_NO = '{1}' AND SALE_DATE = '{2}' AND ORDER_NO = '{3}'",
                    DataLocals.AppConfig.PosInfo.StoreNo, DataLocals.PosStatus.POS_NO, DataLocals.PosStatus.SALE_DATE, waitItem.WAIT_NO));
                db.Database.ExecuteSqlCommand(string.Format("DELETE FROM ORD_CASHREC WHERE SHOP_CODE = " +
                    "'{0}' AND POS_NO = '{1}' AND SALE_DATE = '{2}' AND ORDER_NO = '{3}'",
                    DataLocals.AppConfig.PosInfo.StoreNo, DataLocals.PosStatus.POS_NO, DataLocals.PosStatus.SALE_DATE, waitItem.WAIT_NO));
                db.Database.ExecuteSqlCommand(string.Format("DELETE FROM ORD_CARD WHERE SHOP_CODE = " +
                    "'{0}' AND POS_NO = '{1}' AND SALE_DATE = '{2}' AND ORDER_NO = '{3}'",
                    DataLocals.AppConfig.PosInfo.StoreNo, DataLocals.PosStatus.POS_NO, DataLocals.PosStatus.SALE_DATE, waitItem.WAIT_NO));
                db.Database.ExecuteSqlCommand(string.Format("DELETE FROM ORD_GIFT WHERE SHOP_CODE = " +
                    "'{0}' AND POS_NO = '{1}' AND SALE_DATE = '{2}' AND ORDER_NO = '{3}'",
                    DataLocals.AppConfig.PosInfo.StoreNo, DataLocals.PosStatus.POS_NO, DataLocals.PosStatus.SALE_DATE, waitItem.WAIT_NO));
                db.Database.ExecuteSqlCommand(string.Format("DELETE FROM ORD_FOODCPN WHERE SHOP_CODE = " +
                    "'{0}' AND POS_NO = '{1}' AND SALE_DATE = '{2}' AND ORDER_NO = '{3}'",
                    DataLocals.AppConfig.PosInfo.StoreNo, DataLocals.PosStatus.POS_NO, DataLocals.PosStatus.SALE_DATE, waitItem.WAIT_NO));
                db.Database.ExecuteSqlCommand(string.Format("DELETE FROM ORD_EASYPAY WHERE SHOP_CODE = " +
                    "'{0}' AND POS_NO = '{1}' AND SALE_DATE = '{2}' AND ORDER_NO = '{3}'",
                    DataLocals.AppConfig.PosInfo.StoreNo, DataLocals.PosStatus.POS_NO, DataLocals.PosStatus.SALE_DATE, waitItem.WAIT_NO));
                trans.Commit();
            }
            catch (Exception ex)
            {
                trans.Rollback();
                LogHelper.Logger.Error(ex.ToFormattedString());
            }
        }
    }

    public Task<ORD_WAIT_ITEM> LoadTableOrders(string tableCD, bool load)
    {
        try
        {
            using (var db = new DataContext())
            {
                var ords = db.oRD_HEADERs.Where(p => p.DLV_ORDER_FLAG == "2" && p.ORDER_END_FLAG == "0" && p.FD_TBL_CODE == tableCD
                && p.POS_NO == DataLocals.AppConfig.PosInfo.PosNo.ToString()).FirstOrDefault();
                if (ords == null)
                {
                    ords = new();
                }
                 
                if(!load && ords!= null)
                {
                    using (var trans = db.Database.BeginTransaction())
                    {
                        try
                        {
                            db.oRD_HEADERs.Remove(ords);
                            db.SaveChanges();
                            trans.Commit();
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
                        }
                        catch (Exception ex)
                        {
                            trans.Rollback();
                            LogHelper.Logger.Error(ex.ToFormattedString());
                        }
                    }
                }

                ORD_WAIT_ITEM ordItem = new()
                {
                    TABLE_NO = tableCD,
                    SEQ = 1,
                    WAIT_NO = ords.ORDER_NO,
                    EXP_PAY_AMT = ords.TOT_SALE_AMT,
                    GST_PAY_AMT = ords.DCM_SALE_AMT,
                    INSERT_DT = ords.INSERT_DT
                };
                return Task.FromResult(ordItem);
            }
        }
        catch (Exception e)
        {
            return null;
        }
    }
    #endregion
}
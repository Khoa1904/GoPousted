using GoPOS.Service.Common;
using GoShared.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GoPOS.Models;
using FirebirdSql.Data.FirebirdClient;
using GoPOS.Database;
using GoPOS.Models.Common;
using GoShared.Helpers;
using Dapper;
using log4net.Util;
using GoShared.Events;
using GoShared.Contract;
using GoPOS.Service.Service;
using GoPOS.Models.Config;
using GoPOS.Service;
using System.IO;
using System.Security.Permissions;
using System.Threading;
using System.Net.Mime;
using NLog.LayoutRenderers.Wrappers;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using GoPOS.Views;
using GoPOS.Common.Helpers;
using System.Windows.Forms.Design;
using GoPosVanAPI.Api;
using GoPosVanAPI.Van;
using GoPosVanAPI.Msg;

namespace GoPOS.Services
{
    public class POSInitService : BaseDataService<POS_STATUS>, IPOSInitService
    {
        private readonly IInfoShopService shopService;
        private readonly ISettAccountService settAccountService;

        public POSInitService(IInfoShopService shopService, ISettAccountService settAccountService)
        {
            this.shopService = shopService;
            this.settAccountService = settAccountService;
        }

        /// <summary>
        /// TO DO
        /// </summary>
        private void BackupOnOpen()
        {
            var dbFile = DapperORM.DbFilePath(false);
            string backUpFile = DapperORM.DbFilePath(true);
            string backUpPath = Path.GetDirectoryName(backUpFile);
            if (!Directory.Exists(backUpPath))
            {
                Directory.CreateDirectory(backUpPath);
            }

            File.Copy(dbFile, backUpFile, true);
        }

        /// <summary>
        /// 1: can, 2: db, 3: ini and pos status diff
        /// </summary>
        /// <returns></returns>
        public int POSCanStart(out string errorMessage)
        {
            errorMessage = string.Empty;
            var dbError = DapperORM.DBConnectionTest();

            if (!dbError)
            {
                errorMessage = "디비 연결실패 하였습니다.\r\n포스 종료합니다.";
                return 2;
            }

            if (string.IsNullOrEmpty(DataLocals.AppConfig.PosInfo.StoreNo) ||
                string.IsNullOrEmpty(DataLocals.AppConfig.PosInfo.PosNo))
            {
                return 4;
            }

            using (var context = new DataContext())
            {
                var key = context.pOS_KEY_MANGs.FirstOrDefault(p =>
                    p.SHOP_CODE == DataLocals.AppConfig.PosInfo.StoreNo &&
                    p.POS_NO == DataLocals.AppConfig.PosInfo.PosNo &&
                    p.HD_SHOP_CODE == DataLocals.AppConfig.PosInfo.HD_SHOP_CODE);
                if (key == null)
                {
                    return 4;
                }
                if (string.IsNullOrEmpty(key.LICENSE_ID) ||
                    string.IsNullOrEmpty(key.LICENSE_KEY))
                {
                    return 4;
                }
            }
            return 1;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public int POSInitialize()
        {
            // Set current shop info
            DataLocals.SetShopInfo(shopService.GetShopInfoCurrent());

            using (var db = new DataContext())
            {
                DataLocals.POSVanConfig = db.mST_INFO_VAN_POs.FirstOrDefault() ??
                    new MST_INFO_VAN_POS()
                    {
                        VAN_CODE = "00"
                    };

                try
                {
                    var vanAPI = new VanAPI();

                    VanRequestMsg msg = new VanRequestMsg()
                    {
                        VAN_CODE = DataLocals.POSVanConfig.VAN_CODE,
                        TID = DataLocals.POSVanConfig.VAN_TERM_NO,
                        POS_NO = DataLocals.AppConfig.PosInfo.PosNo,
                        BIZ_NO = DataLocals.POSVanConfig.BIZ_NO,
                        SALE_DATE = DateTime.Today.ToString("yyyyMMdd")
                    };

                    vanAPI.PosRegistration(msg);
                }
                catch (Exception ex)
                {
                    LogHelper.Logger.Error(ex.ToFormattedString());
                }


                DataLocals.PosStatus = db.pOS_STATUSes.FirstOrDefault(p => p.SHOP_CODE == DataLocals.AppConfig.PosInfo.StoreNo &&
                                p.POS_NO == DataLocals.AppConfig.PosInfo.PosNo);
                string svrWebURL = GetConfigValue(true, "104")?.ENV_VALUE_NAME;
                if (!string.IsNullOrEmpty(svrWebURL))
                {
                    DataLocals.AppConfig.PosComm.SvrURLServer = svrWebURL;
                    DataLocals.AppConfig.Save();
                }

                LoadPOSOptions();
            }


            return 1;
        }

        /// <summary>
        /// Update properties in DataLocals.AppConfig.PosOption
        /// </summary>
        private void LoadPOSOptions()
        {
            DataLocals.AppConfig.PosOption = new PosOption();
            var pis = DataLocals.AppConfig.PosOption.GetType().GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
            foreach (var pi in pis)
            {
                var atts = pi.GetCustomAttributes(true);
                int envScope = -1;
                string envSetCode = string.Empty;
                bool useValue = false;
                bool parseInt = false;
                foreach (var att in atts)
                {
                    if (att is PosOptionAttr)
                    {
                        PosOptionAttr tg = (PosOptionAttr)att;
                        envScope = tg.EnvScope;
                        envSetCode = tg.EnvSetCode;
                        useValue = tg.UseValue;
                        parseInt = tg.ParseInt;
                        break;
                    }
                }

                // not contain that attr
                if (envScope < 0)
                {
                    continue;
                }

                var confgValue = GetConfigValue(envScope == 0, envSetCode);
                var propValue = useValue ? confgValue?.ENV_VALUE_CODE : confgValue?.ENV_VALUE_NAME;
                try
                {
                    if (parseInt)
                    {
                        var intValue = int.Parse(new string(propValue.Where(c => char.IsDigit(c)).ToArray())).ToString();
                        pi.SetValue(DataLocals.AppConfig.PosOption, intValue);
                    }
                    else
                    {
                        pi.SetValue(DataLocals.AppConfig.PosOption, propValue);
                    }
                }
                catch
                {
                }
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        public void VerifyAPIToken()
        {

        }

        #region 개설


        /// <summary>
        /// 
        /// </summary>
        /// <returns>0: 개시된상태, 영업가능; 1:마감된상태: 오늘날짜로 개설, 2: 중간정산됨 다음차 개설 (같은영업일), </returns>
        public int CheckPOSOpen(out POS_STATUS openPOSStatus, out SETT_POSACCOUNT lastSettAcc)
        {
            // Load POS_STATUS
            // last date            
            var posStatus = this.TryGetAsyn(p => p.SHOP_CODE ==
                    DataLocals.AppConfig.PosInfo.StoreNo && p.POS_NO ==
                    DataLocals.AppConfig.PosInfo.PosNo).Result;
            openPOSStatus = null;
            lastSettAcc = posStatus != null ? settAccountService.GetSingleAsync(posStatus, true).Result : null;
            int retValue = 0;

            /// NEED 개점
            if (posStatus == null || "3".Equals(posStatus.CLOSE_FLAG))
            {
                retValue = 1;

                DateTime saleDate = DateTime.Today;
                if (posStatus != null)
                {
                    if (Convert.ToInt32(posStatus.SALE_DATE) < Convert.ToInt32(DateTime.Today.ToString("yyyyMMdd")))
                    {
                        saleDate = DateTime.Today;
                    }
                    else
                    {
                        saleDate = DateTime.ParseExact(posStatus.SALE_DATE, "yyyyMMdd", Thread.CurrentThread.CurrentUICulture).AddDays(1);
                    }
                }

                posStatus = new POS_STATUS()
                {
                    SHOP_CODE = DataLocals.AppConfig.PosInfo.StoreNo,
                    POS_NO = DataLocals.AppConfig.PosInfo.PosNo,
                    SALE_DATE = saleDate.ToString("yyyyMMdd"),
                    REGI_SEQ = "01",
                    CLOSE_FLAG = "1",
                    BILL_NO = "0000",
                    ORDER_NO = "0000",
                    SALE_NO = "0000",
                    CREDIT_NO = "0000",
                    EMP_NO = DataLocals.Employee.EMP_NO
                };

            }
            else
            {
                if ("2".Equals(posStatus.CLOSE_FLAG))
                {
                    // 중간개설
                    retValue = 2;

                    int seq = Convert.ToInt32(posStatus.REGI_SEQ) + 1;
                    posStatus.REGI_SEQ = seq.ToString("d2");
                    posStatus.EMP_NO = DataLocals.Employee.EMP_NO;
                    posStatus.CLOSE_FLAG = "1";
                }
            }

            openPOSStatus = posStatus;
            return retValue;
        }

        /// <summary>
        /// Save to db
        /// </summary>
        /// <param name="posStatus"></param>
        /// <param name="posReadyAmt"></param>
        /// <returns></returns>
        public bool DoPOSOpen(POS_STATUS? posStatus, decimal posReadyAmt)
        {
            // TO DO IN TRANS
            bool openRes = true;

            using (var context = new DataContext())
            {
                DbContextTransaction tran = context.Database.BeginTransaction();
                try
                {
                    context.pOS_STATUSes.AddOrUpdate(posStatus);

                    // SAVE TO SETT_POSACCOUNT
                    var settAcc = new SETT_POSACCOUNT()
                    {
                        SHOP_CODE = posStatus.SHOP_CODE,
                        POS_NO = posStatus.POS_NO,
                        SALE_DATE = posStatus.SALE_DATE,
                        REGI_SEQ = posStatus.REGI_SEQ,
                        EMP_NO = posStatus.EMP_NO,
                        CLOSE_FLAG = posStatus.CLOSE_FLAG,
                        OPEN_DT = DateTime.Now.ToString("yyyyMMddHHmmss"),
                        POS_READY_AMT = posReadyAmt,
                        SEND_FLAG = "0",
                        INSERT_DT = DateTime.Now.ToString("yyyyMMddHHmmss")
                    };

                    openRes |= settAccountService.Add(context, settAcc, posStatus.EMP_NO).Success;

                    OnPOSSettAccountAdded(context, settAcc, true);
                    DataLocals.PosStatus = posStatus;
                    BackupOnOpen();

                    context.SaveChanges();
                    tran.Commit();
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    LogHelper.Logger.Error(ex.ToFormattedString());
                }
            }

            return openRes;
        }

        #endregion

        #region 중간정산, 마감

        /// <summary>
        /// 
        /// </summary>
        /// <param name="setAmounts"></param>
        public void DoMiddleSettAccount(SETT_POSACCOUNT lastSettAcc)
        {
            /// 
            /// Update amounts
            /// 
            lastSettAcc.CLOSE_FLAG = "2";
            lastSettAcc.CLOSE_DT = DateTime.Now.ToString("yyyyMMddHHmmss");

            using (var context = new DataContext())
            {
                var settAcc = context.sETT_POSACCOUNTs.FirstOrDefault(p => p.SHOP_CODE == lastSettAcc.SHOP_CODE &&
                                p.POS_NO == lastSettAcc.POS_NO && p.SALE_DATE == lastSettAcc.SALE_DATE && p.REGI_SEQ == lastSettAcc.REGI_SEQ);
                settAcc.CopyFieldsFrom<SETT_POSACCOUNT>(lastSettAcc, null);
                context.SaveChanges();

                OnPOSSettAccountAdded(context, lastSettAcc, true);
            }

            //settAccountService.Update(lastSettAcc);            
            var posStatus = TryGet(p => p.SHOP_CODE == DataLocals.PosStatus.SHOP_CODE &&
                                        p.POS_NO == DataLocals.PosStatus.POS_NO);
            posStatus.CLOSE_FLAG = "2";
            AddOrUpdate(posStatus);
        }

        void OnPOSSettAccountAdded(DataContext context, SETT_POSACCOUNT settPosAcc, bool isAdd)
        {
            if (context == null)
            {
                context = new DataContext();
            }

            POS_POST_MANG postMang = null;

            string billNo = "00".Equals(settPosAcc.REGI_SEQ) ? "99" : settPosAcc.REGI_SEQ;
            postMang = context.pOS_POST_MANGs.FirstOrDefault(p =>
                        p.SHOP_CODE == settPosAcc.SHOP_CODE &&
                        p.POS_NO == settPosAcc.POS_NO &&
                        p.SALE_DATE == settPosAcc.SALE_DATE &&
                        p.REGI_SEQ == settPosAcc.REGI_SEQ &&
                        p.POST_FLAG == "AC" &&
                        p.BILL_FLAG == settPosAcc.CLOSE_FLAG &&
                        p.BILL_NO == billNo);

            if (postMang == null)
            {
                postMang = new POS_POST_MANG()
                {
                    SHOP_CODE = settPosAcc.SHOP_CODE,
                    POS_NO = settPosAcc.POS_NO,
                    SALE_DATE = settPosAcc.SALE_DATE,
                    REGI_SEQ = settPosAcc.REGI_SEQ,
                    POST_FLAG = "AC",
                    BILL_FLAG = settPosAcc.CLOSE_FLAG,
                    BILL_NO = "00".Equals(settPosAcc.REGI_SEQ) ? "99" : settPosAcc.REGI_SEQ,
                    EMP_NO = settPosAcc.EMP_NO,
                    SEND_FLAG = "N",
                    SEND_DT = string.Empty,
                    ERR_CODE = string.Empty,
                    ERR_MSG = string.Empty,
                    ERR_CNT = 0
                };

                context.pOS_POST_MANGs.Add(postMang);
            }
            else
            {
                postMang.EMP_NO = settPosAcc.EMP_NO;
                postMang.SEND_FLAG = "N";
                postMang.SEND_DT = string.Empty;
                postMang.ERR_CODE = string.Empty;
                postMang.ERR_MSG = string.Empty;
                postMang.ERR_CNT = 0;
            }

            context.SaveChanges();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="setAmounts"></param>
        public SETT_POSACCOUNT DoCloseSettAccount(SETT_POSACCOUNT middleSettAccount, bool createNewMiddle, bool dontSave)
        {
            /// 
            /// Update amounts
            /// 
            SETT_POSACCOUNT closeSettAccount = null;
            DbContextTransaction tran = null;
            using (var context = new DataContext())
            {
                tran = context.Database.BeginTransaction();
                try
                {
                    if (!"2".Equals(middleSettAccount.CLOSE_FLAG))
                    {
                        middleSettAccount.CLOSE_FLAG = "2";
                        OnPOSSettAccountAdded(context, middleSettAccount, createNewMiddle);
                    }

                    middleSettAccount.CLOSE_DT = string.IsNullOrEmpty(middleSettAccount.CLOSE_DT) ? DateTime.Now.ToString("yyyyMMddHHmmss") : middleSettAccount.CLOSE_DT;
                    if (createNewMiddle)
                    {
                        context.sETT_POSACCOUNTs.Add(middleSettAccount);
                    }

                    var settAcc = context.sETT_POSACCOUNTs.FirstOrDefault(p => p.SHOP_CODE == middleSettAccount.SHOP_CODE &&
                                    p.POS_NO == middleSettAccount.POS_NO && p.SALE_DATE == middleSettAccount.SALE_DATE && p.REGI_SEQ == middleSettAccount.REGI_SEQ);
                    settAcc.CopyFieldsFrom<SETT_POSACCOUNT>(middleSettAccount, null);

                    // New SET ACC, 00
                    bool createCloseSett = false;
                    closeSettAccount = context.sETT_POSACCOUNTs.FirstOrDefault(p => p.SHOP_CODE == middleSettAccount.SHOP_CODE &&
                                    p.POS_NO == middleSettAccount.POS_NO && p.SALE_DATE == middleSettAccount.SALE_DATE && p.REGI_SEQ == "00");
                    if (closeSettAccount == null)
                    {
                        createCloseSett = true;
                        closeSettAccount = new();
                    }

                    closeSettAccount.CopyFieldsFrom<SETT_POSACCOUNT>(middleSettAccount, null);

                    // copy all data from Resi to this
                    var settAccounts = context.sETT_POSACCOUNTs.Where(p => p.SHOP_CODE == middleSettAccount.SHOP_CODE &&
                                    p.POS_NO == middleSettAccount.POS_NO && p.SALE_DATE == middleSettAccount.SALE_DATE &&
                                    p.REGI_SEQ != "00" &&
                                    p.CLOSE_FLAG == "2" &&
                                    p.REGI_SEQ != middleSettAccount.REGI_SEQ).OrderBy(p => p.REGI_SEQ).ToArray();

                    foreach (var acc in settAccounts)
                    {
                        closeSettAccount.AddFieldsFrom<SETT_POSACCOUNT>(acc, null);
                    }

                    closeSettAccount.CLOSE_FLAG = "3";
                    closeSettAccount.CLOSE_DT = DateTime.Now.ToString("yyyyMMddHHmmss");
                    closeSettAccount.REGI_SEQ = "00";
                    if (createCloseSett)
                    {
                        context.sETT_POSACCOUNTs.Add(closeSettAccount);
                    }

                    OnPOSSettAccountAdded(context, closeSettAccount, createCloseSett);

                    var posStatus = TryGet(p => p.SHOP_CODE == DataLocals.PosStatus.SHOP_CODE &&
                                                p.POS_NO == DataLocals.PosStatus.POS_NO &&
                                                p.SALE_DATE == DataLocals.PosStatus.SALE_DATE);
                    posStatus.CLOSE_FLAG = "3";
                    posStatus.REGI_SEQ = "00";
                    context.pOS_STATUSes.AddOrUpdate(posStatus);

                    context.SaveChanges();

                    if (dontSave)
                    {
                        tran.Rollback();
                    }
                    else
                    {
                        tran.Commit();
                    }
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    LogHelper.Logger.Error(ex.ToFormattedString());
                }
            }


            return closeSettAccount;
        }

        /// <summary>
        /// Cancel Open:
        ///     + Set back to open
        ///     + 
        /// </summary>
        /// <param name="closeSettAcc"></param>
        public string DoCloseSettAccountCancel(SETT_POSACCOUNT closeSettAcc, bool cancelOpen)
        {
            using (var context = new DataContext())
            {
                DbContextTransaction tran = null;
                try
                {
                    tran = context.Database.BeginTransaction();
                    if (cancelOpen)
                    {
                        /// 
                        /// TO DO
                        /// 1. DELETE REGI SETPOSACC HAVING IN POS_STATUS
                        /// 2. SET POS_STATUS IS LAST SETT ACC, REGI_SEQ = 00
                        /// 
                        /// 
                        var lastOpenSettAcc = context.sETT_POSACCOUNTs.FirstOrDefault(p => p.SHOP_CODE == DataLocals.PosStatus.SHOP_CODE &&
                                p.POS_NO == DataLocals.PosStatus.POS_NO &&
                                p.SALE_DATE == DataLocals.PosStatus.SALE_DATE &&
                                p.REGI_SEQ == DataLocals.PosStatus.REGI_SEQ);
                        context.sETT_POSACCOUNTs.Remove(lastOpenSettAcc);
                        context.SaveChanges();

                        var posStatus = context.pOS_STATUSes.FirstOrDefault(p =>
                                p.SHOP_CODE == lastOpenSettAcc.SHOP_CODE &&
                                p.POS_NO == lastOpenSettAcc.POS_NO &&
                                p.SALE_DATE == lastOpenSettAcc.SALE_DATE);

                        int openSaleDate = Convert.ToInt32(DataLocals.PosStatus.SALE_DATE);
                        var lastCloseSettAcc = context.sETT_POSACCOUNTs.Where(p => p.SHOP_CODE == DataLocals.PosStatus.SHOP_CODE &&
                                p.POS_NO == DataLocals.PosStatus.POS_NO &&
                                p.CLOSE_FLAG == "3" &&
                                p.REGI_SEQ == "00").OrderByDescending(p => p.SALE_DATE).Take(1).FirstOrDefault();

                        posStatus.SALE_DATE = lastCloseSettAcc.SALE_DATE;
                        posStatus.POS_NO = lastCloseSettAcc.POS_NO;
                        posStatus.REGI_SEQ = lastCloseSettAcc.REGI_SEQ;
                        posStatus.CLOSE_FLAG = lastCloseSettAcc.CLOSE_FLAG;
                        posStatus.BILL_NO = context.tRN_HEADERs.Where(p => p.SHOP_CODE == lastCloseSettAcc.SHOP_CODE &&
                                p.POS_NO == lastCloseSettAcc.POS_NO &&
                                p.SALE_DATE == lastCloseSettAcc.SALE_DATE).Max(p => p.BILL_NO);
                        DataLocals.PosStatus = posStatus;
                        context.SaveChanges();
                    }
                    else
                    {
                        var closeAcc = context.sETT_POSACCOUNTs.FirstOrDefault(p =>
                                p.SHOP_CODE == closeSettAcc.SHOP_CODE &&
                                p.POS_NO == closeSettAcc.POS_NO &&
                                p.SALE_DATE == closeSettAcc.SALE_DATE &&
                                p.REGI_SEQ == closeSettAcc.REGI_SEQ);
                        context.sETT_POSACCOUNTs.Remove(closeAcc);
                        // context.sETT_POSACCOUNTs.AddOrUpdate(closeSettAcc);
                        context.SaveChanges();

                        // Get last middle sett
                        // update POS_STATUS
                        var maxRegi = context.sETT_POSACCOUNTs.Where(p => p.SHOP_CODE == closeSettAcc.SHOP_CODE &&
                                            p.POS_NO == closeSettAcc.POS_NO && p.SALE_DATE == closeSettAcc.SALE_DATE).OrderByDescending(p => p.REGI_SEQ).Take(1).FirstOrDefault();

                        var posStatus = context.pOS_STATUSes.FirstOrDefault(p =>
                                p.SHOP_CODE == closeSettAcc.SHOP_CODE &&
                                p.POS_NO == closeSettAcc.POS_NO &&
                                p.SALE_DATE == closeSettAcc.SALE_DATE);

                        posStatus.REGI_SEQ = maxRegi.REGI_SEQ;
                        posStatus.CLOSE_FLAG = maxRegi.CLOSE_FLAG;
                        //context.pOS_STATUSes.AddOrUpdate(DataLocals.PosStatus);
                        DataLocals.PosStatus = posStatus;
                        context.SaveChanges();
                    }

                    tran.Commit();

                    return string.Empty;
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    return ex.Message;
                }

            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="isShopValue"></param>
        /// <param name="setCode"></param>
        /// <returns></returns>
        public MST_CNFG_DETAIL GetConfigValue(bool isShopValue, string setCode)
        {
            using (var context = new DataContext())
            {
                string sql = ResourceHelpers.LoadSqlCommand(SQLFileTypes.Printer, isShopValue ? "GetShopConfigValue" : "GetPosConfigValue");
                return DapperORM.ReturnSingleAsync<MST_CNFG_DETAIL>(sql, new string[]
                {
                    "@SET_CODE"
                },
                new object[]
                {
                    setCode
                }).Result;
            }
        }

        #endregion
    }

}

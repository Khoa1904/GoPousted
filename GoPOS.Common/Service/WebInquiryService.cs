using AutoMapper;
using GoPOS.Common.Helpers;
using GoPOS.Database;
using GoPOS.Models;
using GoPOS.Models.Common;
using GoPOS.Models.Config;
using GoPOS.Models.Custom.API;
using GoPOS.Service;
using GoPOS.Service.Common;
using GoPOS.Service.Service.MST;
using GoShared.Helpers;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace GoPOS.Common.Service
{
    public class WebInquiryService : IWebInquiryService
    {
        public async Task<(string, SpResult)> InqAccessToken()
        {
            var _apiRequest = new ApiRequest(DataLocals.AppConfig.PosComm.SvrURLServer,
                    DataLocals.AppConfig.PosInfo.StoreNo,
                    DataLocals.AppConfig.PosInfo.PosNo,
                    DataLocals.TokenInfo.LICENSE_ID,
                    DataLocals.TokenInfo.LICENSE_KEY);
            await _apiRequest.LoginAsync(false);

            var resType = !string.IsNullOrEmpty(_apiRequest.Token) ? EResultType.SUCCESS : EResultType.ERROR;
            if (resType == EResultType.SUCCESS)
            {
                using (var context = new DataContext())
                {
                    var trans = context.Database.BeginTransaction();
                    var token = context.pOS_KEY_MANGs.FirstOrDefault(t => t.HD_SHOP_CODE == DataLocals.AppConfig.PosInfo.HD_SHOP_CODE &&
                    t.SHOP_CODE == DataLocals.AppConfig.PosInfo.StoreNo
                    && t.POS_NO == DataLocals.AppConfig.PosInfo.PosNo);
                    if (token != null)
                    {
                        token.VALIDDT = _apiRequest.Expire_dt;
                        token.TOKEN = _apiRequest.Token;
                        context.SaveChanges();
                    }
                    else
                    {
                        token = new POS_KEY_MANG();
                        token.VALIDDT = _apiRequest.Expire_dt;
                        token.LICENSE_ID = DataLocals.TokenInfo.LICENSE_ID;
                        token.LICENSE_KEY = DataLocals.TokenInfo.LICENSE_KEY;
                        token.TOKEN = _apiRequest.Token;
                        token.HD_SHOP_CODE = DataLocals.AppConfig.PosInfo.HD_SHOP_CODE;
                        token.SHOP_CODE = DataLocals.AppConfig.PosInfo.StoreNo;
                        token.POS_NO = DataLocals.AppConfig.PosInfo.PosNo;
                        context.pOS_KEY_MANGs.Add(token);
                        context.SaveChanges();
                        DataLocals.TokenInfo = token;

                    }
                    trans.Commit();
                }
            }
            return (_apiRequest.Token, new SpResult()
            {
                ResultType = resType
            });
        }
        public async Task<(MasterTBChangedData, SpResult)> InqMasterTableVersions()
        {
            var ds = new BaseDataService<MasterTBChangedData>();
            List<LocalMasterTBVersion> localVersions = new List<LocalMasterTBVersion>();

            using (var db = new DataContext())
            {
                var list = (from setting in ResourceHelpers.MasterTableIds
                            join mangs in db.pOS_MST_MANGs
                            on setting.MST_ID equals mangs.MST_ID
                            into pd
                            from sybDev in pd.DefaultIfEmpty()
                            select new { Setting = setting, Mang = sybDev }).Select(s => new LocalMasterTBVersion() { MST_ID = s.Setting.MST_ID, POS_VER = s.Mang?.LASTVERSION ?? string.Empty }).ToList();

                localVersions = list;

                //foreach (var item in ResourceHelpers.MasterTableIds)
                //{
                //    var ver = db.pOS_MST_MANGs.FirstOrDefault(p => p.MST_ID == item.MST_ID);
                //    if (ver != null)
                //    {
                //        localVersions.Add(new LocalMasterTBVersion()
                //        {
                //            MST_ID = item.MST_ID,
                //            POS_VER = ver == null ? string.Empty : (ver.LASTVERSION ?? string.Empty)
                //        });
                //    }


                //}
            }

            Dictionary<string, string> addParams = new();

            var oB_mstVerList = new MasterTBVerList()
            {
                MstVerList = localVersions.ToArray()
            };

            addParams.Add("B_Body", JsonHelper.ModelToJson<MasterTBVerList>(oB_mstVerList));
            var result = await ds.SynchronizedData(addParams, false);
            return (result.Data as MasterTBChangedData, new SpResult()
            {
                ResultType = result.Success ? EResultType.SUCCESS : EResultType.ERROR,
                ResultCode = result.Code,
                ResultMessage = result.Message
            });
        }

        public Task<(POS_STATUS, SpResult)> InqPOSStatus()
        {
            throw new NotImplementedException();
        }

        public async Task<(ServerTime?, SpResult)> InqServerTime()
        {
            var ds = new BaseDataService<ServerTime>();
            var result = await ds.SynchronizedData(null, false);
            return (result.Data as ServerTime, new SpResult()
            {
                ResultType = result.Success ? EResultType.SUCCESS : EResultType.ERROR,
                ResultCode = result.Code,
                ResultMessage = result.Message
            });
        }

        public async Task<SpResult> InqTruncateTableAsync()
        {
            string SQL = ResourceHelpers.LoadSqlCommand(SQLFileTypes.ResetDB, "GetResetDB");
            var result2 = DapperORM.ExecuteNonQueryAsync(SQL);
            return new SpResult() { ResultCode = ResultCode.Ok };
        }
    }
}

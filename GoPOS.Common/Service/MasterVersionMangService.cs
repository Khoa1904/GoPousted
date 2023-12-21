using GoPOS.Common.Helpers;
using GoPOS.Models;
using GoPOS.Models.Common;
using GoPOS.Models.Custom.API;
using GoPOS.Service;
using GoPOS.Service.Common;
using GoPOS.Service.Interface.MST;

using GoShared.Contract;
using GoShared.Helpers;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;

namespace GoPOS.Common.Service;

public class MasterVersionMangService : BaseDataService<MST_INFO_POS>, IMasterVersionMangService
{
    public MasterVersionMangService() : base()
    {


    }

    /// <summary>
    /// TO DO
    /// 1. Compare and Insert if not
    /// </summary>
    /// <param name="serverVersions"></param>
    /// <returns></returns>
    public List<POS_MST_MANG> GetNeededUpdateMasterTBs(MasterTBVersion[] serverVersions)
    {
        //List<POS_MST_MANG> updateList = new List<POS_MST_MANG>();
        using (var db = new DataContext())
        {

            return (from mstId in ResourceHelpers.MasterTableIds
                    join localMang in db.pOS_MST_MANGs on mstId.MST_ID equals localMang.MST_ID
                          into pd
                    from localMangInfo in pd.DefaultIfEmpty()
                    join server in serverVersions on mstId.MST_ID equals server.MST_ID
                    into pd1
                    from mstIdInfo in pd1.DefaultIfEmpty()
                    where localMangInfo == null
                    || Convert.ToInt64(localMangInfo == null || string.IsNullOrEmpty(localMangInfo.LASTVERSION) ? "0" : localMangInfo.LASTVERSION) < Convert.ToInt64(mstIdInfo.POS_VER)
                    select new POS_MST_MANG()
                    {
                        MST_ID = mstId.MST_ID,
                        UPDATE_DT = string.Empty,
                        LASTVERSION = mstIdInfo?.POS_VER ?? "",
                        MST_TLNAME = mstId?.MST_TLNAME ?? "",
                        MST_TPNAME = mstId?.MST_TPNAME ?? ""
                    }
                   ).ToList();

            //foreach (var ver in serverVersions)
            //{
            //    var localMang = db.pOS_MST_MANGs.FirstOrDefault(p => p.MST_ID == ver.MST_ID);
            //    var localVer = Convert.ToInt64(localMang == null || string.IsNullOrEmpty(localMang.LASTVERSION) ? "0" : localMang.LASTVERSION);
            //    if (localMang == null || localVer < Convert.ToInt64(ver.POS_VER))
            //    {
            //        var mstIdInfo = ResourceHelpers.MasterTableIds.FirstOrDefault(p => p.MST_ID == ver.MST_ID);

            //        updateList.Add(new POS_MST_MANG()
            //        {
            //            MST_ID = ver.MST_ID,
            //            UPDATE_DT = string.Empty,
            //            LASTVERSION = ver.POS_VER,
            //            MST_TLNAME = mstIdInfo?.MST_TLNAME,
            //            MST_TPNAME = mstIdInfo?.MST_TPNAME
            //        });
            //    }
            //}
        }

        // return updateList;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="masterTBVersion"></param>
    /// <returns></returns>
    public async Task<(bool, string)> DownloadMasterTb(POS_MST_MANG masterTBVersion)
    {
        ResultInfo result = null;
        switch (masterTBVersion.MST_TPNAME)
        {
            case "MST_COMM_CODE":
                result = await (new BaseDataService<MST_COMM_CODE>()).SynchronizedData(null, false, true);
                break;
            case "MST_CNFG_CODE":
                result = await (new BaseDataService<MST_CNFG_CODE>()).SynchronizedData(null, false, true);
                break;
            case "MST_CNFG_DETAIL":
                result = await (new BaseDataService<MST_CNFG_DETAIL>()).SynchronizedData(null, false, true);
                break;
            case "MST_CNFG_SHOP":
                result = await (new BaseDataService<MST_CNFG_SHOP>()).SynchronizedData(null, false, true);
                break;
            case "MST_CNFG_POS":
                result = await (new BaseDataService<MST_CNFG_POS>()).SynchronizedData(null, false, true);
                break;
            case "MST_INFO_SHOP":
                result = await (new BaseDataService<MST_INFO_SHOP>()).SynchronizedData(null, true, true);
                break;
            case "MST_INFO_POS":
                result = await (new BaseDataService<MST_INFO_POS>()).SynchronizedData(null, false, true);
                break;
            case "MST_INFO_VAN_POS":
                result = await (new BaseDataService<MST_INFO_VAN_POS>()).SynchronizedData(null, true, true);
                break;
            case "MST_INFO_CARD":
                result = await (new BaseDataService<MST_INFO_CARD>()).SynchronizedData(null, false, true);
                break;
            case "MST_INFO_VAN_CARDMAP":
                result = await (new BaseDataService<MST_INFO_VAN_CARDMAP>()).SynchronizedData(null, false, true);
                break;
            case "MST_INFO_EASYPAY":
                result = await (new BaseDataService<MST_INFO_EASYPAY>()).SynchronizedData(null, false, true);
                break;
            //case "MST_INFO_VAN_EASYPAYMAP":
            //    result = await (new BaseDataService<MST_INFO_VAN_EASYPAYMAP>()).SynchronizedData(null, false, true);
            //    break;
            case "MST_INFO_EMP":
                result = await (new BaseDataService<MST_INFO_EMP>()).SynchronizedData(null, false, true);
                break;
            case "MST_INFO_PRODUCT":
                result = await (new BaseDataService<MST_INFO_PRODUCT>()).SynchronizedData(null, false, true);
                break;
            case "MST_INFO_LCLASS":
                result = await (new BaseDataService<MST_INFO_LCLASS>()).SynchronizedData(null, false, true);
                break;
            case "MST_INFO_MCLASS":
                result = await (new BaseDataService<MST_INFO_MCLASS>()).SynchronizedData(null, false, true);
                break;
            case "MST_INFO_SLCASS":
                result = await (new BaseDataService<MST_INFO_SLCASS>()).SynchronizedData(null, false, true);
                break;
            case "MST_TUCH_CLASS":
                result = await (new BaseDataService<MST_TUCH_CLASS>()).SynchronizedData(null, false, true);
                break;
            case "MST_TUCH_PRODUCT":
                result = await (new BaseDataService<MST_TUCH_PRODUCT>()).SynchronizedData(null, false, true);
                break;
            case "MST_SHOP_FUNC_KEY":
                result = await (new BaseDataService<MST_SHOP_FUNC_KEY>()).SynchronizedData(null, false, true);
                break;
            case "MST_POS_FUNC_KEY":
                result = await (new BaseDataService<MST_POS_FUNC_KEY>()).SynchronizedData(null, false, true);
                break;
            case "MST_EMP_FUNC_KEY":
                result = await (new BaseDataService<MST_EMP_FUNC_KEY>()).SynchronizedData(null, false, true);
                break;
            case "MST_SIDE_DEPT_CLASS":
                result = await (new BaseDataService<MST_SIDE_DEPT_CLASS>()).SynchronizedData(null, false, true);
                break;
            case "MST_SIDE_DETP_CODE":
                result = await (new BaseDataService<MST_SIDE_DETP_CODE>()).SynchronizedData(null, false, true);
                break;
            case "MST_SIDE_SEL_GROUP":
                result = await (new BaseDataService<MST_SIDE_SEL_GROUP>()).SynchronizedData(null, false, true);
                break;
            case "MST_SIDE_SEL_CLASS":
                result = await (new BaseDataService<MST_SIDE_SEL_CLASS>()).SynchronizedData(null, false, true);
                break;
            case "MST_SIDE_SEL_CODE":
                result = await (new BaseDataService<MST_SIDE_SEL_CODE>()).SynchronizedData(null, false, true);
                break;
            case "MST_INFO_FIX_DC":
                result = await (new BaseDataService<MST_INFO_FIX_DC>()).SynchronizedData(null, false, true);
                break;
            case "MST_INFO_TICKET_CLASS":
                result = await (new BaseDataService<MST_INFO_TICKET_CLASS>()).SynchronizedData(null, false, true);
                break;
            case "MST_INFO_TICKET":
                result = await (new BaseDataService<MST_INFO_TICKET>()).SynchronizedData(null, false, true);
                break;
            case "MST_INFO_JOINCARD":
                result = await (new BaseDataService<MST_INFO_JOINCARD>()).SynchronizedData(null, false, true);
                break;
            case "MST_INFO_PRD_JOINCARD":
                result = await (new BaseDataService<MST_INFO_PRD_JOINCARD>()).SynchronizedData(null, false, true);
                break;
            case "MST_INFO_COUPON":
                result = await (new BaseDataService<MST_INFO_COUPON>()).SynchronizedData(null, false, true);
                break;
            case "MST_INFO_PRD_COUPON":
                result = await (new BaseDataService<MST_INFO_PRD_COUPON>()).SynchronizedData(null, false, true);
                break;
            case "MST_FORM_PRINTER":
                result = await (new BaseDataService<MST_FORM_PRINTER>()).SynchronizedData(null, false, true);
                break;
            case "MST_INFO_ACCOUNT":
                result = await (new BaseDataService<MST_INFO_ACCOUNT>()).SynchronizedData(null, false, true);
                break;
            case "MST_INFO_EXRATE":
                result = await (new BaseDataService<MST_INFO_EXRATE>()).SynchronizedData(null, false, true);
                break;
            case "MST_INFO_RTN_REASON":
                result = await (new BaseDataService<MST_INFO_RTN_REASON>()).SynchronizedData(null, false, true);
                break;
            case "MST_INFO_KDS":
                result = await (new BaseDataService<MST_INFO_KDS>()).SynchronizedData(null, false, true);
                break;
            case "MST_INFO_KDS_PRD":
                result = await (new BaseDataService<MST_INFO_KDS_PRD>()).SynchronizedData(null, false, true);
                break;
            case "MST_TABLE_GROUP":
                result = await (new BaseDataService<MST_TABLE_GROUP>()).SynchronizedData(null, false, true);
                break;
            case "MST_TABLE_DEPT":
                result = await (new BaseDataService<MST_TABLE_DEPT>()).SynchronizedData(null, false, true);
                break;
            case "MST_TABLE_INFO":
                result = await (new BaseDataService<MST_TABLE_INFO>()).SynchronizedData(null, false, true);
                break;
            //case "MST_GLOB_PRODUCT_NAME":
            //    result = await (new BaseDataService<MST_GLOB_PRODUCT_NAME>()).SynchronizedData(null, false, true);
            //    break;
            //case "MST_GLOB_TOUH_CLASS":
            //    result = await (new BaseDataService<MST_GLOB_TOUH_CLASS>()).SynchronizedData(null, false, true);
            //    break;
            //case "MST_GLOB_SIDE_MENU":
            //    result = await (new BaseDataService<MST_GLOB_SIDE_MENU>()).SynchronizedData(null, false, true);
            //    break;
            default:
                return (false, string.Format("{0} {1} {2}", masterTBVersion.MST_ID, masterTBVersion.MST_TLNAME, "No Master Mapping."));
        }

        try
        {
            if (!result.Success)
            {
                return (false, result.Message);
            }

            if (result.TotalRecords == 0)
            {
                return (false, "No data returned");
            }

            // insert into db
            using (var db = new DataContext())
            {
                var exts = db.pOS_MST_MANGs.FirstOrDefault(p => p.MST_ID == masterTBVersion.MST_ID);
                if (exts != null)
                {
                    exts.LASTVERSION = masterTBVersion.LASTVERSION;
                    exts.DATA_COUNT = result.TotalRecords;
                    exts.UPDATE_DT = DateTime.Now.ToString(Formats.SystemDBDateTime);
                    db.pOS_MST_MANGs.AddOrUpdate(exts);
                }
                else
                {
                    string? maxSeq = db.pOS_MST_MANGs.Any() ? db.pOS_MST_MANGs.Max(p => p.RECV_SEQ) : "000";
                    int nMaxSeq = Convert.ToInt32(maxSeq) + 1;
                    var mstIdInfo = ResourceHelpers.MasterTableIds.FirstOrDefault(p => p.MST_ID == masterTBVersion.MST_ID);
                    var newMang = new POS_MST_MANG()
                    {
                        RECV_SEQ = nMaxSeq.ToString("d3"),
                        MST_ID = masterTBVersion.MST_ID,
                        LASTVERSION = masterTBVersion.LASTVERSION,
                        MST_TLNAME = mstIdInfo?.MST_TLNAME,
                        MST_TPNAME = mstIdInfo?.MST_TPNAME,
                        DATA_COUNT = result.TotalRecords,
                        UPDATE_DT = DateTime.Now.ToString(Formats.SystemDBDateTime)
                    };

                    db.pOS_MST_MANGs.Add(newMang);
                }

                db.SaveChanges();
            }

            return (true, string.Empty);
        }
        catch (Exception ex)
        {
            LogHelper.Logger.Error(ex);
            return (false, ex.ToFormattedString());
        }

    }
}


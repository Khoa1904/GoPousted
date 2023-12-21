using GoPOS.Models.Custom.API;
using GoPOS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoPOS.Service.Interface.MST
{
    public interface IMasterVersionMangService
    {
        List<POS_MST_MANG> GetNeededUpdateMasterTBs(MasterTBVersion[] serverVersions);
        Task<(bool, string)> DownloadMasterTb(POS_MST_MANG masterTBVersion);
    }
}

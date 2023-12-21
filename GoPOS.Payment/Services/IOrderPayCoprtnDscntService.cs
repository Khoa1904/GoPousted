using Dapper;
using GoPOS.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace GoPOS.Services;

public interface IOrderPayCoprtnDscntService
{
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public Task<List<MST_INFO_JOINCARD>> GetInfoJoinCardTable();
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public Task<List<MST_INFO_PRD_JOINCARD>> GetInfoPrdJoinCardTable(string joinCard);
}

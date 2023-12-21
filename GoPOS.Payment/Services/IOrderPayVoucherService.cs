using Dapper;
using GoPOS.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace GoPOS.Services;

public interface IOrderPayVoucherService
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="param"></param>
    /// <returns></returns>
    Task<List<MST_INFO_TICKET_CLASS>> GetInfoTicketClass(DynamicParameters param);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="param"></param>
    /// <returns></returns>
    Task<List<MST_INFO_TICKET>> GetInfoTicket(string classCode);

}

    
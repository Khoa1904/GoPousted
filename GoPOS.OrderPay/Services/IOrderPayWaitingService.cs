using Dapper;
using GoPOS.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GoPOS.Services;

/*
 2023-01-30 박현재 생성
 */
public interface IOrderPayWaitingService
{
    Task<ORD_WAIT_ITEM[]> LoadWaitOrders();
    Task<ORD_WAIT_ITEM> LoadTableOrders(string tableCD, bool load);
    Task<bool> HasWaitingTR();
    Task<Dictionary<string, object?>> LoadWaitTRData(ORD_WAIT_ITEM waitItem);
    void RemoveWaitingOrder(ORD_WAIT_ITEM waitItem);


}


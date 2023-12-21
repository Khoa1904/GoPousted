using Dapper;
using GoPOS.Models;
using GoPOS.Models.Custom.OrderMng;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace GoPOS.Services;

public interface IOrderPayOutputMngService
{
    public Task<List<RECEIPT_MANAGER_ITEM>> GetTrnTenderSeqList(string saleDate, string posNo, string billNo);
}


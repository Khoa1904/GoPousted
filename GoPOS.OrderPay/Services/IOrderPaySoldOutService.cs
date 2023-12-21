using Dapper;
using GoPOS.Models;
using GoPOS.Models.Custom.OrderMng;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace GoPOS.Services;

/*
 2023-02-06 김형석 생성
 */
public interface IOrderPaySoldOutService
{
    public Task<List<SOLD_OUT_ITEM>> GetOrderPaySoldOutProductsAsync(string sTOCK_OUT_YN, string proCode, string proName);
    public Task<SpResult> SaveProductStock(ObservableCollection<SOLD_OUT_ITEM> mST_INFO_PRODUCTs);
}
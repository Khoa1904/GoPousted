using Dapper;
using GoPOS.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GoPOS.Services;

/*
 2023-02-25 김형석 생성

 */
public interface IOrderPayGoodsService
{
    Task<(List<MST_INFO_PRODUCT>, SpResult)> GetProductList2(string Pcode, string Pname);
}

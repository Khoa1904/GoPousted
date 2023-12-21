using Dapper;
using GoPOS.Models;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows.Media.Media3D;
using System.Xml.Linq;

namespace GoPOS.Services;

public interface IOrderPayCouponService
{
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    Task<List<MST_INFO_COUPON>> GetCouponList();
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    Task<List<MST_INFO_PRD_COUPON>> GetCouponDetailList(int couponCode);
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    Task<List<MST_INFO_PRD_COUPON>> GetCouponPRDList(int cpnCode);
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    Task<List<MST_INFO_PRODUCT>> GetProductList(string cpnCode);

}


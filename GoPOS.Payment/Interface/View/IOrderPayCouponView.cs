using GoPOS.Common.Interface.View;
using GoPOS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoPOS.Payment.Interface.View
{
    public interface IOrderPayCouponView : IView
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="lsTableCoupon"></param>
        /// <param name="isSelected"></param>
        void SetTableCouponValue(List<MST_INFO_COUPON> lsTableCoupon, int isSelected);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lsItem"></param>
        void AddListItemList(List<ORDER_COUPON_DETAIL> item);
    }
}

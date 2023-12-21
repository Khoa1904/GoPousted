using GoPOS.Common.Interface.View;
using GoPOS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoPOS.Payment.Interface.View
{
    public interface IOrderPayMealView : IView
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="lsTicket"></param>
        /// <param name="isSelected"></param>
        void SetTicketValue(List<MST_INFO_TICKET> lsTicket, int isSelected);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="lsItem"></param>
        void SetListItemList(List<SALES_GIFT_SALE> lsItem);
    }
}

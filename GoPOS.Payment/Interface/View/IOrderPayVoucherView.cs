using GoPOS.Common.Interface.View;
using GoPOS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace GoPOS.Payment.Interface.View
{
    public interface IOrderPayVoucherView : IView
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="lsTicketClass"></param>
        /// <param name="isSelected"></param>
        void SetTicketClassValue(List<MST_INFO_TICKET_CLASS> lsTicketClass, int isSelected);

        void SetTicketValue(List<MST_INFO_TICKET> lsTicket, int isSelected);
        void SetListItem1Value(List<SALES_GIFT_SALE> lsItem);
        void SetListItem2Value(List<SALES_GIFT_SALE> lsItem);
        void ResetTicketValue(List<MST_INFO_TICKET> lsTicket, int isSelected);
        void ResetListItem1Value(List<SALES_GIFT_SALE> lsItem);
        void ResetListItem2Value(List<SALES_GIFT_SALE> lsItem);
        void ScrollToEnd();
        void ScrollToEnd1();
    }
}

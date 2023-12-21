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
    public interface IOrderPayCoprtnDscntView : IView
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="lsTicket"></param>
        void BindAffiliatedCards(List<MST_INFO_JOINCARD> affDiscCards);
        TextBox LimitMoney { get; }
    }
}

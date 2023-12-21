using GoPOS.Common.Interface.View;
using GoPOS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoPOS.OrderPay.Interface.View
{
    public interface IOrderPaySideMenuView : IView
    {
        void RenderSideClasses(ORDER_SIDE_CLS_MENU[] sideClassess);
        void RenderSideMenus(ORDER_SIDE_CLS_MENU[] sideMenus);

    }
}

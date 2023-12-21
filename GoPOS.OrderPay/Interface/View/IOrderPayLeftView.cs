using GoPOS.Common.Interface.View;
using GoPOS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoPOS.OrderPay.Interface.View
{
    public interface IOrderPayLeftView : IView
    {
        void RenderLeftFuncButtons(ORDER_FUNC_KEY[] funcKeys);
        void DisableElements(int type, bool disable);
    }
}

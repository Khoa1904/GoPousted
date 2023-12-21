using GoPOS.Common.Interface.View;
using GoPOS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoPOS.SalePurchase.Interface.View
{
    public interface ISalePurchaseRightView : IView
    {
        void RenderExtButtons(ORDER_FUNC_KEY[] funcKeys);
    }
}

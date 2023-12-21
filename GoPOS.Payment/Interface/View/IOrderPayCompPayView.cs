using GoPOS.Common.Interface.View;

using GoPOS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoPOS.Payment.Interface.View
{
    public interface IOrderPayCompPayView : IView
    {
        void RenderPayButtons(ORDER_FUNC_KEY[] payButtons);
        void UpdatePaidAmount(string positionNo, decimal? paidAmt, int resetAmt);
    }
}

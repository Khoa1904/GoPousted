﻿using GoPOS.Common.Interface.View;
using GoPOS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace GoPOS.OrderPay.Interface.View
{
    public interface IOrderPayPrintMngView : IView
    {
        void RenderFuncButtons(ORDER_FUNC_KEY[] funcButtons);
    }
}

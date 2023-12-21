﻿using GoPOS.Common.Interface.View;
using GoPOS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoPOS.Payment.Interface.View
{
    public interface IOrderPayFixDisView : IView
    {
        void RenderDiscountOptions(MST_INFO_FIX_DC[] fixDcs);
    }
}
﻿using GoPOS.Common.Interface.View;
using GoPOS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoPOS.SalesMng.Interface.View
{
    public interface ISalesMngRightView : IView
    {
        void RenderExtButtons(ORDER_FUNC_KEY[] funcKeys);

    }
}
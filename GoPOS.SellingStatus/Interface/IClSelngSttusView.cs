using GoPOS.Common.Interface.View;
using GoPOS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace GoPOS.SellingStatus.Interface
{
    public interface IClSelngSttusView : IView
    {
        void RenderExtButtons(ORDER_FUNC_KEY[] funcKeys);

        void LoadMainGrid();
        void LoadLineGrid();
        Point buttonPosition { get; set; }
        void ScrollToEnd();

    }
}

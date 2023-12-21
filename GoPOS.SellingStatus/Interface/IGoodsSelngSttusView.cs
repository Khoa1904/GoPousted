using GoPOS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using GoPOS.Common.Interface.View;

namespace GoPOS.SellingStatus.Interface
{
    public interface IGoodsSelngSttusView : IView
    {
        void RenderExtButtons(ORDER_FUNC_KEY[] funcKeys);
        void LoadMainGrid();
        void LoadLineGrid();
        Point buttonPosition { get; set; }
        void ScrollToEnd();
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using GoPOS.Common.Interface.View;

namespace GoPOS.Common.Helpers.Controls
{
    public static class PointHelper
    {
        public static Point GetPointOfButton(UIElement element,dynamic view)
        {
            Point relativePoint = new Point();
            if (element is Button)
            {
                Button btn = element as Button;
                relativePoint = btn.TranslatePoint(new Point(0, 0), view);
                Point screenPoint = GetParentPoint(view);
                relativePoint.X += screenPoint.X + btn.Width;
                relativePoint.Y += screenPoint.Y;

                return relativePoint;
            }
            return relativePoint;
        }

        private static Point GetParentPoint(dynamic view)
        {
            Point ParentPoint = view.TranslatePoint(new Point(0, 0), (UIElement)view.Parent);
            Point screenPoint = view.PointToScreen(ParentPoint);
            return screenPoint;
        }

  
    }
}

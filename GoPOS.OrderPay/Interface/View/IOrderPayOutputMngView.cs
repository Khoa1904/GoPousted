using GoPOS.Common.Interface.View;
using GoPOS.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace GoPOS.OrderPay.Interface.View
{
    public interface IOrderPayOutputMngView : IView
    {
        public Point buttonPosition { get; set; }
        //public Point GetPointOfButton(UIElement element);
        void ScrollToEnd();
    }
}

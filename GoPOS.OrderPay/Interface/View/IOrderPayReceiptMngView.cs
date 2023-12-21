using GoPOS.Common.Interface.View;
using GoPOS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace GoPOS.OrderPay.Interface.View
{
    public interface IOrderPayReceiptMngView : IView
    {
        public Point buttonPosition { get; set; }

        void ScrollToEnd();
    }
}

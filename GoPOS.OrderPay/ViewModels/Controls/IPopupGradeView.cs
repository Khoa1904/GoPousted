using GoPOS.Common.Interface.View;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoPOS.OrderPay.Interface.View
{
    public interface IPopupGradeView: IView
    {
        Point ButtonPost { get;set; }
    }
}

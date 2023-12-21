using GoPOS.Common.Interface.View;
using GoPOS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace GoPOS.OrderPay.Interface.View
{
    public interface IOrderPaySaleStatusView : IView
    {
        public CheckBox iPro1   {get;}
        public CheckBox iPro2   {get;}
        public CheckBox iPro3   {get;}
        public CheckBox iStats1 {get;}
        public CheckBox iStats2 {get;}
        public CheckBox iStats3 {get;}
        public CheckBox iStats4 {get;}

    }
}

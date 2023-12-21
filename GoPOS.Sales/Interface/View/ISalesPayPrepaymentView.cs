using GoPOS.Common.Interface.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace GoPOS.Sales.Interface.View
{
    public interface ISalesPayPrepaymentView: IView
    {
        public Point ButtonPosition { get; set; }
    }
}

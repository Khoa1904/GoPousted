using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace GoPOS.Sales.Interface.View
{
    internal interface ISaleMiddleApprDetailsPopup
    {
        CheckBox iRadio1 { get; }
        CheckBox iRadio2 { get; }
        CheckBox iCheck1 { get; }
        CheckBox iCheck2 { get; }
        CheckBox iCheck3 { get; }

    }
}

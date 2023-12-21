using GoPOS.Common.Interface.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace GoPOS.Sales.Interface.View
{
    public interface ISalesClosingCancelView : IView
    {
        TextBox txtEMP_PWD { get; }
    }
}

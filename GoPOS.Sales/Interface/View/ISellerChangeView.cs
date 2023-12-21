using GoPOS.Common.Interface.View;
using GoPOS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace GoPOS.SalesMng.Interface.View
{
    public interface ISellerChangeView : IView
    {
        TextBox txtEMP_PWD { get; }
    }
}

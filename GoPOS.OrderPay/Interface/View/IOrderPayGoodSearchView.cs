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
    public interface IOrderPayGoodSearchView: IView
    {
        TextBox tbPRDCODE { get;  }
        TextBox tbPRDNAME { get;  }

    }
}

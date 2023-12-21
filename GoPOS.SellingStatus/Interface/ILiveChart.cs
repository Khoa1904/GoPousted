using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GoPOS.Common.Interface.View;
using LiveCharts.Wpf;

namespace GoPOS.SellingStatus.Interface
{
    public interface ILiveChart : IView
    {
        CartesianChart Chart { get;  }
    }
}

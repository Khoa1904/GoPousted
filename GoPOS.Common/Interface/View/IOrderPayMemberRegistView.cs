using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace GoPOS.Common.Interface.View
{
    public interface IOrderPayMemberRegistView
    {
        CheckBox iCheck1 { get; }
        CheckBox iCheck2 { get; }
        CheckBox iLuna { get; }
        CheckBox iSolar { get; }
        Point ButtonPost { get; set; }
    }
}

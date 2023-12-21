using GoPOS.Common.Interface.View;
using GoPOS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using GoPOS.Common.Views;
using GoPOS.OrderPay.Views;

namespace GoPOS.OrderPay.Interface.View
{
    public interface IPostPayTableManagementView : IView
    {
        string table_code { get; set; }
        void RenderFuncButtons(List<ORDER_FUNC_KEY> FuncBtn);
        public void RenderTables(List<TABLE_THR> tables);
        public void ShowOrderDetails(bool check);
    }
}

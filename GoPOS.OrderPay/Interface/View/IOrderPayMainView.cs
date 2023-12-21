using GoPOS.Common.Interface.View;
using GoPOS.Models;
using GoPOS.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace GoPOS.OrderPay.Interface.View
{
    public interface IOrderPayMainView : IView
    {
        ListView OrderItemsLV { get; }
        void ResetSelectedExtButton();
        void RenderLeftFuncButtons(ORDER_FUNC_KEY[] funcKeys);
        void DisableElements(string childActivatedTypes, bool activated);
        Visibility ShowHideMessage { get; set; }
       ToggleButton billText {  get; set; }
       ToggleButton itemText { get; set; }
       ToggleButton kitchenText { get; set; }
       ToggleButton customerText { get; set; }

    }
}

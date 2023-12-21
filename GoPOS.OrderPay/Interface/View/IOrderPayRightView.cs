using GoPOS.Common.Interface.View;
using GoPOS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoPOS.OrderPay.Interface.View
{
    public interface IOrderPayRightView : IView
    {
        short TuchClsPage { get; set; }
        short TuchProdPage { get; set; }
        void BindTouchClasses(List<MST_TUCH_CLASS> tuClasses);
        void BindTouchProdList(List<ORDER_TU_PRODUCT> tuProducts);
        void RenderFuncButtons(List<ORDER_FUNC_KEY> payFuncKeys);
    }
}

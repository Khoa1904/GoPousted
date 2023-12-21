using GoPOS.Common.Interface.View;
using GoPOS.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace GoPOS.OrderPay.Interface.View
{
    public interface IOrderPayReceiptDetailsView : IView
    {
        //void RenderExtButtons(ORDER_FUNC_KEY[] funcKeys);

        //void SetListItemValue(ObservableCollection<MST_INFO_PRODUCT> lsItem);
        void RenderReceiptPreview(string receiptText);

        void SetGridPreview(List<Dictionary<string, string>> data);
    }
}

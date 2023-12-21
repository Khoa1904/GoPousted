using GoPOS.Common.Interface.Model;
using GoPOS.Models;

namespace GoPOS.OrderPay.Interface.ViewModel
{
    public interface IOrderPayRightViewModel : IViewModel
    {
        void TouchClsClicked(MST_TUCH_CLASS tuc);
    }
}

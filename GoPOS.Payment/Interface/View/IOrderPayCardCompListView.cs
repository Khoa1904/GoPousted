using GoPOS.Common.Interface.View;
using GoPOS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoPOS.Payment.Interface.View
{
    public interface IOrderPayCardCompListView : IView
    {
        void RenderCreditCardDeck(MST_INFO_CARD[] CrCard);
    }
}

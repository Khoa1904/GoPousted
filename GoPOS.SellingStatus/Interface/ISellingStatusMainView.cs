using GoPOS.Common.Interface.View;
using GoPOS.Models;
using GoPOS.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoPOS.Interface
{
    public interface ISellingStatusMainView : IView
    {
        void DisableElements(string childActivatedTypes, bool activated);
    }
}

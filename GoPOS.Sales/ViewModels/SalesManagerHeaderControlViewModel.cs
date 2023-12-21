using Caliburn.Micro;
using GoPOS.Common.Interface.Model;
using GoPOS.Common.ViewModels;
using GoPOS.Models;
using GoPOS.Models.Common;
using GoPOS.Services;

namespace GoPOS.ViewModels.Controls
{
    public class SalesManagerHeaderControlViewModel : BaseItemViewModel
    {
        public SalesManagerHeaderControlViewModel(IWindowManager windowManager, IEventAggregator eventAggregator) : 
            base(windowManager, eventAggregator)
        {
        }
    }
}

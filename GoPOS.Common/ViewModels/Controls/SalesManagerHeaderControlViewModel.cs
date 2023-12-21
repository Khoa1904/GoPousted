using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Caliburn.Micro;
using GoPOS.Models;
using GoPOS.Services;
using GoPOS.Views;
using GoPOS.Models.Common;
using GoPOS.Common.ViewModels;
using System.Windows.Controls;
using static GoShared.Events.GoPOSEventHandler;
using GoShared.Events;
using GoPOS.Common.Interface.Model;

namespace GoPOS.Common.ViewModels.Controls
{
    public class SalesManagerHeaderControlViewModel : BaseItemViewModel
    {
        public SalesManagerHeaderControlViewModel(IWindowManager windowManager, IEventAggregator eventAggregator) : base(windowManager, eventAggregator)
        {
        }
    }
}

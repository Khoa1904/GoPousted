using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoPOS.Common.ViewModels.Controls
{
    public class DialogWindowViewModel : Conductor<IScreen>.Collection.OneActive
    {
        public DialogWindowViewModel(IWindowManager windowManager, IEventAggregator eventAggregator)
        {

        }
    }
}

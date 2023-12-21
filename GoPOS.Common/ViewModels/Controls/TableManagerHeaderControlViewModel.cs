using Caliburn.Micro;
using GoPOS.Common.Interface.Model;
using GoPOS.Common.ViewModels;
using GoPOS.Helpers.CommandHelper;
using GoPOS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace GoPOS.Common.ViewModels.Controls
{
    public class TableManagerHeaderControlViewModel : MainBasePageViewModel
    {
        public TableManagerHeaderControlViewModel(IWindowManager windowManager, IEventAggregator eventAggregator) : 
            base(windowManager, eventAggregator)
        {

        }

        public ICommand ButtonCommand => new RelayCommand<Button>(ButtonCommandCenter);
        private void ButtonCommandCenter(Button button)
        {
            if(button.Tag is null) { return; }
            switch(button.Tag)
            {
                case "ButtonClose":
                    ClosePage(null);
                    break;
            }
        }
    }
}

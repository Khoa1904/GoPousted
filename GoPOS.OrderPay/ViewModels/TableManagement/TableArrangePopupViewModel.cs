using Caliburn.Micro;
using GoPOS.Common.Interface.Model;
using GoPOS.Common.ViewModels;
using GoPOS.Helpers.CommandHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace GoPOS.ViewModels
{
    public class TableArrangePopupViewModel : BaseItemViewModel, IDialogViewModel
    {
        public TableArrangePopupViewModel(IWindowManager windowManager, IEventAggregator eventAggregator) : base(windowManager, eventAggregator)
        {
            
        }
        public ICommand ButtonCommand => new RelayCommand<Button>(FunctionButton);

        public Dictionary<string, object> DialogResult { get; set; }

        private void FunctionButton(Button button)
        {
            switch (button.Tag) {
                case "ButtonClose":
                    this.TryCloseAsync();
                    break;
            }
        }
    }
}

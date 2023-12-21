using Caliburn.Micro;
using GoPOS.Common.Interface.Model;
using GoPOS.Common.ViewModels;
using GoPOS.Helpers.CommandHelper;
using GoPosVanAPI.Api.Van.NHNKCP.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace GoPOS.ViewModels
{
    public partial class RePrintBillViewModel : BaseItemViewModel, IDialogViewModel
    {
        public RePrintBillViewModel(IWindowManager windowManager, IEventAggregator eventAggregator) : base(windowManager, eventAggregator)
        {
           
        }

        public Dictionary<string, object> DialogResult { get; set; }
    }
}

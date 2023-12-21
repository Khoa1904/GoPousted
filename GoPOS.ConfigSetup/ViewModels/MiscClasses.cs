using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoPOS.ConfigSetup.ViewModels
{
    public class ConfigSetupMainViewEventArgs : EventArgs
    {
        /// <summary>
        /// ExtButton
        /// ...
        /// </summary>
        public string EventType { get; set; }

        /// <summary>
        /// FK Key No
        /// </summary>
        public object EventData { get; set; }
    }
}

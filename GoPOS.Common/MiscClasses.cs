using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace GoPOS.Common
{
    public class KeyboardEventData
    {
        public IInputElement FocusedControl { get; set; }
        public System.Windows.Input.Key PressedKey { get; set; }
        public bool FwdCancelled { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class LoggedInUserChange
    {
        public string ChangedEmpNo { get; set; }
    }

    public class ConfigUpdatedEventArgs
    {
        
    }
}

using GoPOS.Common.Interface.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoPOS.Common.Interface.View
{
    public interface IView
    {
        SynchronizationContext SyncContext { get; }
        IViewModel ViewModel { get; }
        void EnableControl(bool enable);
        void Translate();
    }
}

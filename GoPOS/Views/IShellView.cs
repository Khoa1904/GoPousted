using GoPOS.Common.Interface.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GoPOS.Views
{
    public interface IShellView
    {
        SynchronizationContext SynContext { get; }
        void Hide();
    }
}

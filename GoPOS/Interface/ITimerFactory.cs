using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoPOS.Interface
{
    public interface ITimerFactory
    {
        ITimer CreateTimer();
    }
}

using GoPOS.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoPOS.Services
{
    public class TimerFactory:ITimerFactory
    {
        public ITimer CreateTimer() => new Timer();
    }
}

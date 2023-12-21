using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoShared.Interface
{
    public interface ILog
    {
        void Info<T>(T value);
        void Warn<T>(T value);
        void Trace<T>(T value);
        void Error<T>(T value);
        void Fatal<T>(T value);
    }
}

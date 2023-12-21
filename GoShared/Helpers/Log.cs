using GoShared.Interface;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoShared.Helpers
{
    public class Log: ILog
    {
        //private static readonly Logger _logger;
        public Log() {
           
        }
        public void Info<T>(T value)
        {
            LogHelper.Logger.Info(value);
        }
        public void Warn<T>(T value)
        {
            LogHelper.Logger.Warn(value);
        }
        public void Trace<T>(T value)
        {
            LogHelper.Logger.Trace(value);
        }
        public void Error<T>(T value)
        {
            LogHelper.Logger.Error(value);
        }
        public void Fatal<T>(T value)
        {
            LogHelper.Logger.Fatal(value);
        }
    }
}

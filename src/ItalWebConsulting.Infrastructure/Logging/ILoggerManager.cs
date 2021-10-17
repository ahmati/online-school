using System;
using System.Collections.Generic;
using System.Text;

namespace ItalWebConsulting.Infrastructure.Logging
{
     public interface ILoggerManager
    {

        void Info(string message);
        void Warn(string message);
        void Debug(string message);
        void Error(string message);

        void ErrorFormat(string format, params object[] args);
        void InfoFormat(string format, params object[] args);
        void DebugFormat(string format, params object[] args);
        void WarnFormat(string format, params object[] args);

        void Debug(Exception exception, string message = null);
        void Info(Exception exception, string message = null);
        void Warn(Exception exception, string message = null);
        void Error(Exception exception, string message = null);
    }
}

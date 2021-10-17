using NLog;
using System;
using System.Collections.Generic;
using System.Text;

namespace ItalWebConsulting.Infrastructure.Logging
{
   public  class LoggerManager : ILoggerManager
    {

        private static ILogger logger = LogManager.GetCurrentClassLogger();

        public LoggerManager()
        {
        }

        public void Debug(string message)
        {
            logger.Debug(message);
        }

        public void Error(string message)
        {
            logger.Error(message);
        }

        public void Info(string message)
        {
            logger.Info(message);
        }

        public void Warn(string message)
        {
            logger.Warn(message);
        }

        public void ErrorFormat(string format, params object[] args)
        {
            logger.Error(string.Format(format, args));
        }

        public void InfoFormat(string format, params object[] args)
        {
            logger.Info(string.Format(format, args));
        }

        public void DebugFormat(string format, params object[] args)
        {
            logger.Debug(string.Format(format, args));
        }

        public void WarnFormat(string format, params object[] args)
        {
            logger.Warn(string.Format(format, args));
        }

        public void Debug(Exception exception, string message = null)
        {
            if (string.IsNullOrWhiteSpace(message))
                logger.Debug(exception);
            else
                logger.Debug(exception, message);
        }

        public void Info(Exception exception, string message = null)
        {
            if (string.IsNullOrWhiteSpace(message))
                logger.Info(exception);
            else
                logger.Info(exception, message);
        }

        public void Warn(Exception exception, string message = null)
        {
            if (string.IsNullOrWhiteSpace(message))
                logger.Warn(exception);
            else
                logger.Warn(exception, message);
        }

        public void Error(Exception exception, string message = null)
        {
            if (string.IsNullOrWhiteSpace(message))
                logger.Error(exception);
            else
                logger.Error(exception, message);
        }
    }
}

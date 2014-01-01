using System;

namespace OMR.Core.Helpers.Logging
{
    public interface ILogger
    {
        void Debug(object message);
        void Info(object message);
        void Warn(object message);
        void Error(object message);
        void Fatal(object message);
        void Debug(object message, Exception exception);
        void Info(object message, Exception exception);
        void Warn(object message, Exception exception);
        void Error(object message, Exception exception);
        void Fatal(object message, Exception exception);
    }
}

using System;
using LSOne.Utilities.ErrorHandling;

namespace LSOne.DataLayer.GenericConnector
{
    internal class EmptyErrorLogger : IErrorLog
    {
        public void LogMessage(LogMessageType messageType, string message, Exception ex = null)
        {
            // This method does nothing since this is supposed to be empty error logger
        }

        public void LogMessage(LogMessageType messageType, string message, string source, int durationInMilliSec = 0)
        {

        }

        public void LogMessageToFile(LogMessageType messageType, string message, string source)
        {
            
        }
    }
}

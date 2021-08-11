using System;

namespace LSOne.DataLayer.GenericConnector.EventArguments
{
    public class ConnectionLostEventArguments : EventArgs
    {
        public int ErrorCode { get; set; }
        public bool Retry { get; set; }

        public ConnectionLostEventArguments(int errorCode)
        {
            errorCode = 0;
            Retry = false;
        }
    }
}

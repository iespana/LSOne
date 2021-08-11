using System;
using LSOne.Services.Interfaces.Enums;

namespace LSOne.Services.Interfaces.EventArguments
{
    public class LogonFormEventArguments : EventArgs
    {
        /// <summary>
        /// The operation in context to this instance of LogonFormEventArgs
        /// </summary>
        public LogonFormOperation Operation { get; set; }

        /// <summary>
        /// Indicates if action needs to be taken to cancel an operation
        /// </summary>
        public bool Cancel { get; set; }
    }
}

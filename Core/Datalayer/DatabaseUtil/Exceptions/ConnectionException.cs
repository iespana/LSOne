using System;

namespace LSOne.DataLayer.DatabaseUtil.Exceptions
{
    /// <summary>
    /// All functions that depend on the Connection to have been instantiated will throw this exception if the connection is not valid
    /// </summary>
    public class ConnectionException : DatabaseUtilityException
    {
        /// <summary>
        /// Initializes a new instance of the ConnectionException class.
        /// </summary>
        public ConnectionException() : base() { }

        /// <summary>
        /// Initializes a new instance of the ConnectionException class with a specified error message.
        /// </summary>
        /// <param name="message">The message that describes the error. </param>
        public ConnectionException(string message) : base(message) { }
        
        /// <summary>
        /// Initializes a new instance of the ConnectionException class with a specified error message and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception. </param>
        /// <param name="inner">The exception that is the cause of the current exception, or a null reference if no inner exception is specified. </param>
        public ConnectionException(string message, Exception inner) : base(message, inner) { }
    }    
}

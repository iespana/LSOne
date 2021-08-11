using System;

namespace LSOne.DataLayer.DatabaseUtil.Exceptions
{
    /// <summary>
    /// When the SQL Server installation has completed but the connection can still not be made this exception is thrown.
    /// </summary>
    public class SQLServerNotFoundException : DatabaseUtilityException
    {
        /// <summary>
        /// Initializes a new instance of the SQLServerNotFoundException class.
        /// </summary>
        public SQLServerNotFoundException() : base() { }

        /// <summary>
        /// Initializes a new instance of the SQLServerNotFoundException class with a specified error message.
        /// </summary>
        /// <param name="message">The message that describes the error. </param>
        public SQLServerNotFoundException(string message) : base(message) { }

        /// <summary>
        /// Initializes a new instance of the SQLServerNotFoundException class with a specified error message and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception. </param>
        /// <param name="inner">The exception that is the cause of the current exception, or a null reference if no inner exception is specified. </param>
        public SQLServerNotFoundException(string message, Exception inner) : base(message, inner) { }        
    }
}

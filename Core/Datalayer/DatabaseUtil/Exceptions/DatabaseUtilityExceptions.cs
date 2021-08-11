using System;

namespace LSOne.DataLayer.DatabaseUtil.Exceptions
{
    /// <summary>
    /// A generic exception thrown by the Database Utility with information about the original exception (if any) and a message about what led to the exception being thrown.
    /// </summary>
    public class DatabaseUtilityException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the DatabaseUtilityException class.
        /// </summary>
        public DatabaseUtilityException() { }

        /// <summary>
        /// Initializes a new instance of the DatabaseUtilityException class with a specified error message.
        /// </summary>
        /// <param name="message">The message that describes the error. </param>
        public DatabaseUtilityException(string message) : base(message) { }

        /// <summary>
        /// Initializes a new instance of the DatabaseUtilityException class with a specified error message and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception. </param>
        /// <param name="inner">The exception that is the cause of the current exception, or a null reference if no inner exception is specified. </param>
        public DatabaseUtilityException(string message, Exception inner) : base(message, inner) { }        
    }
    
}



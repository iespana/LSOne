using System;

namespace LSOne.DataLayer.DatabaseUtil.Exceptions
{
    /// <summary>
    /// An exception that is thrown when a database to be connected to does not exist. Is not used in default functionality.
    /// </summary>
    public class DatabaseNotExistsException : DatabaseUtilityException
    {
        /// <summary>
        /// Initializes a new instance of the DatabaseNotExistsException class.
        /// </summary>
        public DatabaseNotExistsException() : base() { }

        /// <summary>
        /// Initializes a new instance of the DatabaseNotExistsException class with a specified error message.
        /// </summary>
        /// <param name="message">The message that describes the error. </param>
        public DatabaseNotExistsException(string message) : base(message) { }

        /// <summary>
        /// Initializes a new instance of the DatabaseNotExistsException class with a specified error message and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception. </param>
        /// <param name="inner">The exception that is the cause of the current exception, or a null reference if no inner exception is specified. </param>
        public DatabaseNotExistsException(string message, Exception inner) : base(message, inner) { }
    }
}

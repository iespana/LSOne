using System;

namespace LSOne.DataLayer.DatabaseUtil.Exceptions
{
    /// <summary>
    /// All constructors will throw this error if the DataAreaId (company id) is not valid
    /// </summary>
    public class DataAreaIdNotValidException : DatabaseUtilityException
    {
        /// <summary>
        /// Initializes a new instance of the DataAreaIdNotValidException class.
        /// </summary>
        public DataAreaIdNotValidException() : base() { }

        /// <summary>
        /// Initializes a new instance of the DataAreaIdNotValidException class with a specified error message.
        /// </summary>
        /// <param name="message">The message that describes the error. </param>
        public DataAreaIdNotValidException(string message) : base(message) { }

        /// <summary>
        /// Initializes a new instance of the DataAreaIdNotValidException class with a specified error message and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception. </param>
        /// <param name="inner">The exception that is the cause of the current exception, or a null reference if no inner exception is specified. </param>
        public DataAreaIdNotValidException(string message, Exception inner) : base(message, inner) { }
    }    
}

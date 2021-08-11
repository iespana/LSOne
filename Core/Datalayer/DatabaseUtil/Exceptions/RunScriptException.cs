using System;
using System.Data.SqlClient;

namespace LSOne.DataLayer.DatabaseUtil.Exceptions
{
    /// <summary>
    /// If any of the SQL scripts cannot finish successfully then a RunScriptException is thrown with information about the script and the message returned from the SQL Server.
    /// </summary>
    public class RunScriptException : Exception
    {
        /// <summary>
        /// The SQL Connection object being used to run the script that errored out. Read-only property
        /// </summary>
        public SqlConnection connection { get; private set; }

        /// <summary>
        /// Initializes a new instance of the RunScriptException class.
        /// </summary>
        public RunScriptException() { }

        /// <summary>
        /// Initializes a new instance of the RunScriptException class with a specified error message.
        /// </summary>
        /// <param name="message">The message that describes the error. </param>
        /// <param name="connection">The connection being used when the error occurred</param>
        public RunScriptException(string message, SqlConnection connection)
            : base(message) 
        {
            this.connection = connection;
        }

        /// <summary>
        /// Initializes a new instance of the RunScriptException class with a specified error message and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception. </param>
        /// <param name="connection">The connection used when the error occured</param>
        /// <param name="inner">The exception that is the cause of the current exception, or a null reference if no inner exception is specified. </param>
        public RunScriptException(string message, SqlConnection connection, Exception inner)
            : base(message, inner) 
        {
            this.connection = connection;
        }        
    }

}

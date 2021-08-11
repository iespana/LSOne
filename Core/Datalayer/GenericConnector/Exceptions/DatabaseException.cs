using System;
using System.Data;

namespace LSOne.DataLayer.GenericConnector.Exceptions
{
	/// <summary>
	/// The exception that is thrown when the database server returns a warning or error.
	/// </summary>
	[Serializable]
	public class DatabaseException : LSOneException
	{
        public IDbCommand cmd;

        public DatabaseException() { }
        public DatabaseException(string message) : base(message) { }
        public DatabaseException(string message, Exception inner, IDbCommand cmd) : base(message, inner) 
        {
            this.cmd = cmd;
        }
		protected DatabaseException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
	}
}

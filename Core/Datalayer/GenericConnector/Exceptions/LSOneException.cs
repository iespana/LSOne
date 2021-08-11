using System;

namespace LSOne.DataLayer.GenericConnector.Exceptions
{
    [global::System.Serializable]
    public class LSOneException : Exception
    {
        //
        // For guidelines regarding the creation of new exception types, see
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
        // and
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
        //

        public LSOneException() { }
        public LSOneException(string message) : base(message) { }
        public LSOneException(string message, Exception inner) : base(message, inner) { }
        protected LSOneException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }
}

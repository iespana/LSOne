using System;
using System.Runtime.Serialization;
using LSOne.DataLayer.BusinessObjects;

namespace LSOne.ViewPlugins.Scheduler.DataDirector
{
    [Serializable]
    public class DataDirectorException : Exception
    {
        public DataDirectorException()
        {
        }
        
        public DataDirectorException(int errorCode)
        {
            this.ErrorCode = errorCode;
        }

        public DataDirectorException(string message)
            : base(message)
        {
        }

        public DataDirectorException(string message, int errorCode)
            : base(message)
        {
            ErrorCode = errorCode;
        }
        
        public DataDirectorException(string message, Exception inner)
            : base(message, inner)
        { }

        public int ErrorCode { get; private set; }

        protected DataDirectorException(
          SerializationInfo info,
          StreamingContext context)
            : base(info, context)
        {
            ErrorCode = info.GetInt32("ErrorCode");
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue("ErrorCode", ErrorCode);
        }

        public override string Message
        {
            get
            {
                string msg = base.Message;
                if (ErrorCode != 0)
                {
                    msg = string.Format(Properties.Resources.DataDirectorErrorFormatMsg, msg, ErrorCode);
                }

                return msg;
            }
        }
    }
}

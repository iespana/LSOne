using System;

namespace LSOne.DataLayer.BusinessObjects.Integrations
{
    /// <summary>
    /// IntegrationLog stores log entries for integrations
    /// </summary>
    public class IntegrationLog : DataEntity
    {
        public enum LogTypes
        {
            Error,
            Info,
            Debug,
            MethodEntry,
            MethodExit
        }

        public IntegrationLog()
            : base()
        {
            Stamp = DateTime.Now;
            ErrorID = 0;
        }

        public IntegrationLog(IntegrationLog copy)
        {
            ID = copy.ID;
            Text = copy.Text;
            Stamp = copy.Stamp;
            ErrorID = copy.ErrorID;
            LogType = copy.LogType;
            Message = copy.Message;
        }


        /// <summary>
        /// A timestamp of when the entry was created
        /// </summary>
        public DateTime Stamp { get; set; }

        /// <summary>
        /// The ID of the error (if any)
        /// </summary>
        public int ErrorID { get; set; }

        /// <summary>
        /// The type of log message
        /// </summary>
        public LogTypes LogType { get; set; }

        /// <summary>
        /// The log message
        /// </summary>
        public string Message { get; set; }

        // Accessors
        public string Method { get { return Text; } set { Text = value; } }
    }
}

using LSOne.Utilities.DataTypes;
using System;
using System.Runtime.Serialization;

[assembly: ContractNamespace("http://www.lsretail.com/lsone/2017/12/entities", ClrNamespace = "LSOne.DataLayer.BusinessObjects.IntegrationFramework")]
namespace LSOne.DataLayer.BusinessObjects.IntegrationFramework
{    /// <summary>
     /// EventArgs class used by the MessageHandler event in SiteService
     /// </summary>
    [DataContract]
    [KnownType(typeof(RecordIdentifier))]
    [KnownType(typeof(AccessToken))]
    public class MessageEventArgs : EventArgs
    {
        /// <summary>
        /// The message to be handled by the receiver
        /// </summary>
        [DataMember]
        public string Message { get; set; }

        /// <summary>
        /// The receiver who should handle the message
        /// </summary>
        [DataMember]
        public string Receiver { get; set; }

        /// <summary>
        /// Data received by the receiver
        /// </summary>
        [DataMember]
        public object Data { get; set; }

        /// <summary>
        /// The action that should be performed on the data
        /// </summary>
        [DataMember]
        public MessageAction Action { get; set; }

        /// <summary>
        /// Default MessageHandler constructor
        /// </summary>
        /// <param name="receiver">The name of the receiver who should handle the message</param>
        /// <param name="message">The message to be handled by the receiver</param>
        /// <param name="data">Data received by the receiver</param>
        /// <param name="action">The action that should be performed on the data</param>
        public MessageEventArgs(string receiver, string message, object data, MessageAction action)
        {
            Receiver = receiver;
            Message = message;
            Data = data;
            Action = action;
        }
    }

    /// <summary>
    /// Represents a basic that happened on data
    /// </summary>
    [Serializable]
    public enum MessageAction
    {
        None = 0,
        Add = 1,
        Update = 2,
        Delete = 4
    }
}

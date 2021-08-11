using System;
using System.Runtime.Serialization;
using LSOne.Utilities.DataTypes;

[assembly: ContractNamespace("http://www.lsretail.com/lsone/2017/12/entities", ClrNamespace = "LSOne.DataLayer.BusinessObjects.IntegrationFramework")]
namespace LSOne.DataLayer.BusinessObjects.IntegrationFramework
{
    [DataContract]
    [KnownType(typeof(RecordIdentifier))]
    public class AccessToken
    {
        private string senderDNS;

        /// <summary>
        /// Description of the access token
        /// </summary>
        [DataMember]
        public string Description { get; set; }
        /// <summary>
        /// The LS One user
        /// </summary>
        [DataMember]
        public RecordIdentifier UserID { get; set; }
        /// <summary>
        /// Id of the LS One user
        /// </summary>
        [DataMember]
        public string UserStaffID { get; set; }
        /// <summary>
        /// Login of the LS One user
        /// </summary>
        [DataMember]
        public RecordIdentifier UserLogin { get; set; }
        /// <summary>
        /// Name of the LS One user
        /// </summary>
        [DataMember]
        public string UserName { get; set; }
        /// <summary>
        /// Id of the store in LS One
        /// </summary>
        [DataMember]
        public RecordIdentifier StoreID { get; set; }
        /// <summary>
        /// Name of the store in LS One
        /// </summary>
        [DataMember]
        public string StoreName { get; set; }
        /// <summary>
        /// Address/Name of the machine sending from SAP
        /// </summary>
        [DataMember]
        public string SenderDNS
        {
            get { return senderDNS; }
            set { senderDNS = value.ToLowerInvariant(); }
        }
        /// <summary>
        /// Indicates if the access token is active or not
        /// </summary>
        [DataMember]
        public bool Active { get; set; }
        /// <summary>
        /// Indicates last time when access token was last used for verifacation
        /// </summary>
        [DataMember]
        public DateTime TimeStamp { get; set; }
        /// <summary>
        /// Access token
        /// </summary>
        [DataMember]
        public string Token { get; set; }

        /// <summary>
        /// Default constructor
        /// </summary>
        public AccessToken()
        {

        }
    }
}

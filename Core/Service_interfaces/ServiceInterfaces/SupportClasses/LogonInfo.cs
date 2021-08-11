using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using LSOne.Utilities.DataTypes;

namespace LSOne.Services.Interfaces.SupportClasses
{
    [DataContract]
    [KnownType(typeof(RecordIdentifier))]
    public class LogonInfo
    {
        public LogonInfo()
        {
            UserID = RecordIdentifier.Empty;
            StaffID = "";
        }

        /// <summary>
        /// The ID of the store that is calling the Site Service
        /// </summary>
        [DataMember]
        public string storeId { get; set; }

        /// <summary>
        /// The ID of the terminal that is calling the Site Service
        /// </summary>
        [DataMember]
        public string terminalId { get; set; }

      
        /// <summary>
        /// The ID of the staff that is logged onto the POS that is calling the Site Service
        /// </summary>
        [DataMember]
        public RecordIdentifier StaffID { get; set; }
        
        /// <summary>
        /// Site Manager user identification
        /// </summary>
        [DataMember]
        public RecordIdentifier UserID { get; set; }

        [DataMember]
        public SortedList<Guid, string> Settings { get; set; }

        [DataMember]
        public byte[] Hash { get; set; }

        [DataMember]
        public long Ticks { get; set; }

        [DataMember]
        public Guid ClientID { get; set; }


    }
}

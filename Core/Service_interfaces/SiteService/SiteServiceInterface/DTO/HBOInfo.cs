using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;
using LSOne.Utilities.DataTypes;

namespace LSRetail.SiteService.SiteServiceInterface.DTO
{
    [DataContract]
    [KnownType(typeof(RecordIdentifier))]
    public class HBOInfo
    {

        /// <summary>
        /// The database used
        /// </summary>
        [DataMember]
        public string database { get; set; }

        /// <summary>
        /// The ID of the user for the database
        /// </summary>
        [DataMember]
        public string databaseUser { get; set; }

        // Password for the DB
        [DataMember]
        public string databasePassword { get; set; }

        [DataMember]
        public Guid UserGuid { get; set; }


        [DataMember]
        public string ErrorText { get; set; }
     


    }
}

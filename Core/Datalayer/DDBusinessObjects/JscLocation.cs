using System.Collections.Generic;
using LSOne.DataLayer.BusinessObjects;
using LSOne.Utilities.DataTypes;
using LSRetail.DD.Common;

namespace LSOne.DataLayer.DDBusinessObjects
{
    public class JscLocation : DataEntity
    {
        public bool IsGroup
        {
            get { return this.MemberLocations.Count > 0; }
        }
        
        public string ExDataAreaId { get; set; }
        public string ExCode { get; set; }
        public RecordIdentifier DatabaseDesign { get; set; }
        public LocationKind LocationKind { get; set; }
        public RecordIdentifier DefaultOwner { get; set; }
        public string DDHost { get; set; }
        public string DDPort { get; set; }
        public NetMode DDNetMode { get; set; }
        public bool Enabled { get; set; }
        public string Company { get; set; }
        public string UserId { get; set; }
        public string Password { get; set; }
        public bool DBServerIsUsed { get; set; }
        public string DBServerHost { get; set; }
        public string DBPathName { get; set; }
        public RecordIdentifier DBDriverType { get; set; }
        public string DBConnectionString { get; set; }
        public string SystemTag { get; set; }

        public List<JscLocationMember> MemberLocations { get; set; }
    }
}

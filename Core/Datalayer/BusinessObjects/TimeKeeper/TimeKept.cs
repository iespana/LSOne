using System;
using System.Runtime.Serialization;
using LSOne.Utilities.Attributes;
using LSOne.Utilities.DataTypes;
using LSOne.Utilities.Validation;

namespace LSOne.DataLayer.BusinessObjects.TimeKeeper
{
    [DataContract]
        public enum KeepType
        {
        [EnumMember]
        ClockIn = 0,
        [EnumMember]
        ClockOut = 1
        }
    [DataContract]
    [KnownType(typeof(RecordIdentifier))]
    [KnownType(typeof(Date))]
    public class TimeKept : DataEntity
    {

      
        [DataMember]
        [RecordIdentifierValidation(RecordIdentifier.RecordIdentifierType.Guid)]
        [RecordIdentifierConstruction(typeof(Guid))]
        public RecordIdentifier UserID { get; set; }
        [DataMember]
        public DateTime DateKept { get; set; }
        [DataMember]
        public KeepType TypeToKeep { get; set; }
        [DataMember]
        [RecordIdentifierValidation(RecordIdentifier.RecordIdentifierType.Guid)]
        [RecordIdentifierConstruction(typeof(Guid))]
        public RecordIdentifier ClosesEntry { get; set; }

    }
}

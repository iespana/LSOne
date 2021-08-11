using System.Runtime.Serialization;

namespace LSOne.DataLayer.BusinessObjects.Enums
{

    [DataContract(Name = "ConnectionEnum")]
    public enum ConnectionEnum
    {
        [EnumMember]
        Success = 1,
        [EnumMember]
        ConnectionFailed = 2,
        [EnumMember]
        DatabaseConnectionFailed = 3,
        [EnumMember]
        ExternalConnectionFailed = 4,
        [EnumMember]
        ClientTimeNotSynchronized = 5
    }
}

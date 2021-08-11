using System.Runtime.Serialization;

namespace LSOne.DataLayer.BusinessObjects.Enums
{
    /// <summary>
    /// A property on the user that defines how the user should be identified.
    /// </summary>
    [DataContract(Name = "UserIdentifierEnum")]
    public enum UserIdentifierEnum
    {
        [EnumMember]
        Login = 0,
        [EnumMember]
        StaffID = 1,
        [EnumMember]
        Guid = 2
    }
}

using System.Runtime.Serialization;

namespace LSOne.DataLayer.BusinessObjects.Enums
{
    /// <summary>
    /// A property on the customer that tells the POS if the customer is tax exempt or not
    /// </summary>
    [DataContract(Name = "TaxExemptEnum")]
    public enum TaxExemptEnum
    {
        [EnumMember]
        No = 0,
        [EnumMember]
        Yes = 1,
        [EnumMember]
        EU = 2
    }

    [DataContract(Name = "BlockedEnum")]
    public enum BlockedEnum
    {
        [EnumMember]
        Nothing = 0,
        [EnumMember]
        Invoice = 1,
        [EnumMember]
        All = 2
    }
}

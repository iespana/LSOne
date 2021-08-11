using System.Runtime.Serialization;

namespace LSOne.DataLayer.BusinessObjects.Enums
{
    [DataContract(Name = "OperationAuditEnum")]
    public enum OperationAuditEnum
    {
        Never = 0,
        Always = 1,
        OverridesOnly = 2
    }
}
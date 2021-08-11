using System.Runtime.Serialization;

namespace LSOne.DataLayer.BusinessObjects.Reports
{
    [DataContract]
    public enum ReportResultStatusEnum
    {
        [EnumMember]
        Success,
        [EnumMember]
        Failed,
        [EnumMember]
        NotExists

    }
}

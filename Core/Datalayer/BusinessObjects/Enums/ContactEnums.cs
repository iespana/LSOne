using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace LSOne.DataLayer.BusinessObjects.Enums
{
    [DataContract(Name = "ContactRelationTypeEnum")]
    public enum ContactRelationTypeEnum
    {
        [EnumMember]
        Vendor = 1,
        [EnumMember]
        Customer = 2
    };

    [DataContract(Name = "TypeOfContactEnum")]
    public enum TypeOfContactEnum
    {
        [EnumMember]
        Person = 1,
        [EnumMember]
        Company = 2
    };

    [DataContract(Name = "SaveContactResultEnum")]
    public enum SaveContactResultEnum
    {
        [EnumMember]
        ContactSaved = 0,
        [EnumMember]
        ContactSavedSetAsDefault = 1
    }
}

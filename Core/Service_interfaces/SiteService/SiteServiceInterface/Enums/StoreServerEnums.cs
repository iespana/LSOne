using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace LSRetail.SiteService.SiteServiceInterface.Enums
{
    [DataContract(Name = "GiftCardValidationEnum")]
    public enum GiftCardValidationEnum
    {
        [EnumMember]
        ValidationSuccess = 1,
        [EnumMember]
        ValidationCardNotFound = 2,
        [EnumMember]
        ValidationCardNotActive = 3,
        [EnumMember]
        ValidationBalanceToLow = 4,
        [EnumMember]
        ValidationUnknownError = 5,
        [EnumMember]
        ValidationCardHasZeroBalance = 6
    }

    [DataContract(Name = "CreditVoucherValidationEnum")]
    public enum CreditVoucherValidationEnum
    {
        [EnumMember]
        ValidationSuccess = 1,
        [EnumMember]
        ValidationVoucherNotFound = 2,
        [EnumMember]
        ValidationBalanceToLow = 4,
        [EnumMember]
        ValidationUnknownError = 5,
        [EnumMember]
        ValidationVoucherHasZeroBalance = 6
    }

    [DataContract(Name = "ActivationResultEnum")]
    public enum ActivationResultEnum
    {
        [EnumMember]
        Success = 1,
        [EnumMember]
        AlreadyActivated = 2,
        [EnumMember]
        UnknownError = 3
    }
}

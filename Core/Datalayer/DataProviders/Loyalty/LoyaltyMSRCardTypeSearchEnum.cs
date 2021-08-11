using System;
using LSOne.DataLayer.BusinessObjects;

namespace LSOne.DataLayer.DataProviders.Loyalty
{
    [Flags]
    public enum LoyaltyMSRCardTypeSearchEnum
    {
        CardTender = 0x04,
        ContactTender = 0x08,
        NoTender = 0x10,
        Blocked = 0x20
    }
}

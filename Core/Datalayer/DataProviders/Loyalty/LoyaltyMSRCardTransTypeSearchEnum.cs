using System;
using LSOne.DataLayer.BusinessObjects;

namespace LSOne.DataLayer.DataProviders.Loyalty
{
    [Flags]
    public enum LoyaltyMSRCardTransTypeSearchEnum
    {
        IssuePoints = 0x04,
        UsePoints = 0x08,
        ExpirePoints = 0x10
    }
}
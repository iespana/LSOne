using System;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.Services.Interfaces.SupportInterfaces;
using LSOne.Utilities.DataTypes;

namespace LSOne.Services.Interfaces
{
    public interface IDiscountItem : ICloneable, ISerializable
    {
        decimal Amount { get; set; }
        decimal AmountWithTax { get; set; }
        DateTime BeginDateTime { get; set; }
        RecordIdentifier DiscountID { get; set; }
        string DiscountName { get; set; }
        DiscountTransTypes DiscountType { get; set; }
        DateTime EndDateTime { get; set; }
        DiscountOrigin Origin { get; set; }
        string PartnerInfo { get; set; }
        decimal Percentage { get; set; }
    }
}

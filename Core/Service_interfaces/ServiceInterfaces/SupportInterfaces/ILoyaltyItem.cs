using System;
using LSOne.DataLayer.BusinessObjects.Enums;

namespace LSOne.Services.Interfaces.SupportInterfaces
{
    public interface ILoyaltyItem : ICloneable, ISerializable
    {
        decimal AccumulatedPoints { get; set; }
        decimal AggregatedItemQuantity { get; set; }
        decimal CalculatedPoints { get; set; }
        decimal CalculatedPointsAmount { get; set; }
        string CardNumber { get; set; }
        void Clear();
        ILoyaltyItem CopyForPointsRelation(LoyaltyPointsRelation relation);
        decimal CurrentValue { get; set; }
        LSOne.Utilities.DataTypes.RecordIdentifier CustomerID { get; set; }
        DateTime DateIssued { get; set; }
        bool Empty { get; }
        TransactionStatus EntryStatus { get; set; }
        DateTime ExpirationDate { get; set; }
        TimeUnitEnum ExpireUnit { get; set; }
        int ExpireValue { get; set; }
        decimal LineNum { get; set; }
        bool PartOfAggregatedItemPoints { get; set; }
        bool PointsAdded { get; }
        bool RecalculateDiscount { get; set; }
        LSOne.Utilities.DataTypes.RecordIdentifier ReceiptID { get; set; }
        LoyaltyPointsRelation Relation { get; set; }
        LSOne.Utilities.DataTypes.RecordIdentifier RuleID { get; set; }
        bool SchemeExists { get; }
        LSOne.Utilities.DataTypes.RecordIdentifier SchemeID { get; set; }
        LSOne.Utilities.DataTypes.RecordIdentifier StaffID { get; set; }
        LSOne.Utilities.DataTypes.RecordIdentifier StoreID { get; set; }
        LSOne.Utilities.DataTypes.RecordIdentifier TerminalID { get; set; }
        LSOne.Utilities.DataTypes.RecordIdentifier TransactionID { get; set; }

        /// <summary>
        /// Usage limit as percentage of transaction total
        /// </summary>
        int UsePointsLimit { get; set; }
    }
}

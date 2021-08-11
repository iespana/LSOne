using System;
using System.Runtime.Serialization;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.BusinessObjects.Inventory
{
    [Serializable]
    [DataContract]
    public class InventoryTransaction : DataEntity
    {
        /// <summary>
        /// Creates an empty inventory transaction for a retail item.
        /// </summary>
        public InventoryTransaction()
            : this("", ItemTypeEnum.Item)
        { }

        /// <summary>
        /// Creates a new inventory transaction for the given item and its item type (retail or service).
        /// </summary>
        /// <param name="itemID"></param>
        /// <param name="itemType"></param>
        public InventoryTransaction(RecordIdentifier itemID, ItemTypeEnum itemType)
        {
            OfferID = "";
            ItemID = itemID;
            StoreID = "";
            OfferID = "";
            AdjustmentUnitID = "";
            ReasonCode = "";
            Adjustment = 0;
            AdjustmentInInventoryUnit = 0;
            CostPricePerItem = 0;
            SalesPriceWithoutTaxPerItem = 0;
            SalesPriceWithTaxPerItem = 0;
            DiscountAmountPerItem = 0;
            OfferDiscountAmountPerItem = 0;
            Compatibility = "2016.1";
            ItemType = itemType;
        }

        /// <summary>
        /// Composed of [GUID,PostingDate,StoreID]
        /// </summary>
        [DataMember]
        public override RecordIdentifier ID
        {
            get
            {
                return Guid;
            }
            set
            {
                if (!serializing)
                {
                    throw new NotImplementedException();
                }
            }
        }
        [DataMember]
        public RecordIdentifier Guid { get; set; }
        [DataMember]
        public DateTime PostingDate { get; set; }
        [DataMember]
        public RecordIdentifier ItemID { get; protected set; }
        [DataMember]
        public RecordIdentifier StoreID { get; set; }
        [DataMember]
        public InventoryTypeEnum Type { get; set; }
        [DataMember]
        public string OfferID { get; set; }
        [DataMember]
        public RecordIdentifier InventoryUnitID { get; set; }
        [DataMember]
        public RecordIdentifier AdjustmentUnitID { get; set; }
        [DataMember]
        public decimal Adjustment { get; set; }
        [DataMember]
        public decimal AdjustmentInInventoryUnit { get; set; }
        [DataMember]
        public decimal CostPricePerItem { get; set; }
        [DataMember]
        public decimal SalesPriceWithTaxPerItem { get; set; }
        [DataMember]
        public decimal SalesPriceWithoutTaxPerItem { get; set; }
        [DataMember]
        public decimal DiscountAmountPerItem { get; set; }
        [DataMember]
        public decimal OfferDiscountAmountPerItem { get; set; }
        [DataMember]
        public RecordIdentifier ReasonCode { get; set; }
        [DataMember]
        public string VariantName { get; set; }
        [DataMember]
        public string Reference { get; set; }
        [DataMember]
        public string Compatibility { get; set; }

        /// <summary>
        /// Item's type (retail or service). Items of type service cannot be used in inventory transactions.
        /// </summary>
        public ItemTypeEnum ItemType { get; protected set; }

        /// <summary>
        /// Configures the item id and type for the current inventory transaction.
        /// </summary>
        /// <param name="itemID"></param>
        /// <param name="type"></param>
        public void Initialize(RecordIdentifier itemID, ItemTypeEnum type)
        {
            ItemID = itemID;
            ItemType = type;
        }
    }
}

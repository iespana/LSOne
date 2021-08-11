using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.BusinessObjects.ItemMaster
{
    /// <summary>
    /// Retail item cost price entry
    /// </summary>
    public class RetailItemCost : DataEntity
    {
        /// <summary>
        /// ID of the item
        /// </summary>
        public RecordIdentifier ItemID { get; set; }

        /// <summary>
        /// ID of the store
        /// </summary>
        public RecordIdentifier StoreID { get; set; }

        /// <summary>
        /// Name of the store
        /// </summary>
        public string StoreName { get; set; }

        /// <summary>
        /// Unit purchase price of the item
        /// </summary>
        public decimal Cost { get; set; }

        /// <summary>
        /// Unit purchase price of the item
        /// </summary>
        public string RecalculationReason { get; set; }

        /// <summary>
        /// UserID of the user responsible for recalculation
        /// </summary>
        public RecordIdentifier UserID { get; set; }

        /// <summary>
        /// UserID of the user responsible for recalculation
        /// </summary>
        public RecordIdentifier UserLogin { get; set; }

        /// <summary>
        /// UserID of the user responsible for recalculation
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Available inventory quantity. Used for calculations
        /// </summary>
        public decimal Quantity { get; set; }

        /// <summary>
        /// Sales unit ID
        /// </summary>
        public RecordIdentifier UnitID { get; set; }

        /// <summary>
        /// Name of the unit
        /// </summary>
        public string UnitName { get; set; }

        /// <summary>
        /// Date of entry for the purchase price
        /// </summary>
        public Date EntryDate { get; set; }

        /// <summary>
        /// Total cost value (Inventory quantity * Cost)
        /// </summary>
        public decimal TotalCostValue { get; set; }

        public RetailItemCost()
        {
            ItemID = RecordIdentifier.Empty;
            StoreID = RecordIdentifier.Empty;
            UnitID = RecordIdentifier.Empty;
            StoreName = "";
            EntryDate = Date.Empty;
        }
    }
}

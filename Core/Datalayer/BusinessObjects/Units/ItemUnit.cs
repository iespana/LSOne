using LSOne.Utilities.DataTypes;
using LSOne.Utilities.Development;

namespace LSOne.DataLayer.BusinessObjects.Units
{
    [LSOneUsage(CodeUsage.LSCommerce)]
    public class ItemUnit
    {
        /// <summary>
        /// Item id.
        /// </summary>
        public RecordIdentifier ItemID { get; set; }
        
        /// <summary>
        /// Item inventory unit id.
        /// </summary>
        public RecordIdentifier InventoryUnitID { get; set; }
        
        /// <summary>
        /// Item inventory unit description.
        /// </summary>
        public string InventoryUnitDescription { get; set; }
        
        /// <summary>
        /// Item sales unit id.
        /// </summary>
        public RecordIdentifier SalesUnitID { get; set; }
        
        /// <summary>
        /// Item sales unit description.
        /// </summary>
        public string SalesUnitDescription { get; set; }
        
        /// <summary>
        /// Item purchase unit id.
        /// </summary>
        public RecordIdentifier PurchaseUnitID { get; set; }
        
        /// <summary>
        /// Item purchase unit description.
        /// </summary>
        public string PurchaseUnitDescription { get; set; }
        
        /// <summary>
        /// Conversion factor from item's inventory unit to sales unit.
        /// </summary>
        public decimal SalesUnitFactor { get; set; }
        
        /// <summary>
        /// Conversion factor from item's inventory unit to purchase unit.
        /// </summary>
        public decimal PurchaseUnitFactor { get; set; }
    }
}

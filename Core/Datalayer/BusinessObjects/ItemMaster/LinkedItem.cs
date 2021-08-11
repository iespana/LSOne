using System;
using LSOne.Utilities.DataTypes;
#if !MONO
#endif


namespace LSOne.DataLayer.BusinessObjects.ItemMaster
{
    /// <summary>
    /// Data entity class for a linked items
    /// </summary>
    public class LinkedItem : DataEntity
    {
        /// <summary>
        /// A enum that defines sorting for the linked items
        /// </summary>
        public enum SortEnum
        {
            /// <summary>
            /// Sort by linked item id
            /// </summary>
            LinkedItemID,
            /// <summary>
            /// Sort by linked items unit id
            /// </summary>
            LinkedItemsUnitID,
            /// <summary>
            /// Sort by blocked
            /// </summary>
            Blocked,
            /// <summary>
            /// Sort by linked item quantity
            /// </summary>
            LinkedItemQuantity
        }

        /// <summary>
        /// The constructor for the class. All variables are set to default values
        /// </summary>
        public LinkedItem()
        {
            LinkedItemDescription = "";
            LinkedItemVariantDescription = "";
            LinkedItemUnitDescription = "";
        }

        /// <summary>
        /// The unique ID of the linked item configurations
        /// </summary>
        public override RecordIdentifier ID
        {
            get
            {
                return new RecordIdentifier(OriginalItemID, new RecordIdentifier(LinkedItemID, LinkedItemUnitID));
            }
            set
            {
                if (!serializing)
                {
                    throw new NotImplementedException();
                }
            }
        }

        /// <summary>
        /// Not currently used
        /// </summary>
        public RecordIdentifier OriginalItemID { get; set; }
        /// <summary>
        /// The item ID of the item that is set as a linked item
        /// </summary>
        public RecordIdentifier LinkedItemID { get; set; }
        /// <summary>
        /// The description of the linked item
        /// </summary>
        public string LinkedItemDescription { get; set; }
        /// <summary>
        /// The description of the linked variant item
        /// </summary>
        public string LinkedItemVariantDescription { get; set; }
        /// <summary>
        /// The ID of the unit the linked item is sold in
        /// </summary>
        public RecordIdentifier LinkedItemUnitID { get; set; }
        /// <summary>
        /// Description of the unit the linked item is sold in
        /// </summary>
        public string LinkedItemUnitDescription { get; set; }
        /// <summary>
        /// If true the linked item is blocked and will not be sold on the POS
        /// </summary>
        public bool Blocked { get; set; }
        /// <summary>
        /// The quantity the linked item should be sold in
        /// </summary>
        public decimal LinkedItemQuantity { get; set; }
        public string FormattedLinkedItemQuantity 
        {
            get
            {
                return LinkedItemQuantity.FormatWithLimits(UnitLimiter);
            }
        }

        /// <summary>
        /// Not currently used
        /// </summary>
        public DecimalLimit UnitLimiter { set; get; }
    }
}
 
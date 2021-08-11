using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.Utilities.DataTypes;
using System.Collections.Generic;

namespace LSOne.DataLayer.BusinessObjects.Inventory
{
    /// <summary>
    /// Search criteria for <see cref="InventoryAdjustment">inventory journal</see>.
    /// </summary>
    public class InventoryJournalSearch
    {
        /// <summary>
        /// To search by a specific store.
        /// </summary>
        public RecordIdentifier StoreID;

        /// <summary>
        /// To search for inventory journal id and description(tokenized).
        /// </summary>
        public List<string> Description;

        /// <summary>
        /// If true then the search string is in the beginning of the description or ID r variant.
        /// </summary>
        public bool DescriptionBeginsWith;

        /// <summary>
        /// If null then the Status flag is ignored at search. Non-null values matches the <see cref="InventoryJournalStatus"/> enumeration (Posted, Active, PartialPosted).
        /// </summary>
        public int? Status;

        /// <summary>
        /// To search by created date - minimum date.
        /// </summary>
        public Date CreatedDateFrom;

        /// <summary>
        /// To search by created date - maximum date.
        /// </summary>
        public Date CreatedDateTo;

        /// <summary>
        /// To search by posted date - minimum date
        /// </summary>
        public Date PostedDateFrom;

        /// <summary>
        /// To search by posted date - maximum date
        /// </summary>
        public Date PostedDateTo;

        public InventoryJournalSearch()
        {
            StoreID = RecordIdentifier.Empty;
            Description = new List<string>();
            DescriptionBeginsWith = true;
            Status = null;
            CreatedDateFrom = Date.Empty;
            CreatedDateTo = Date.Empty;
            PostedDateFrom = Date.Empty;
            PostedDateTo = Date.Empty;
        }
    }
}

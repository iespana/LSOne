using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.Utilities.DataTypes;
using LSOne.Utilities.Expressions;
using System;
using System.Collections.Generic;

namespace LSOne.DataLayer.BusinessObjects.Inventory
{
    /// <summary>
    /// Search criteria for <see cref="InventoryJournalTransaction">inventory journal line</see> 
    /// </summary>
    public class InventoryJournalLineSearch
    {
        /// <summary>
        /// To search for lines specific to the given journal id.
        /// </summary>
        public RecordIdentifier JournalId;
        /// <summary>
        /// To search for inventory item id, description(tokenized).
        /// </summary>
        public List<string> Description;

        /// <summary>
        /// If true then the search string is in the beginning of the description or ID.
        /// </summary>
        public bool DescriptionBeginsWith;

        /// <summary>
        /// To search for item variant description (tokenized).
        /// </summary>
        public List<string> VariantDescription;

        /// <summary>
        /// If true then the search string is in the beginning of the variant name.
        /// </summary>
        public bool VariantDescriptionBeginsWith;

        /// <summary>
        /// To search for quantity.
        /// </summary>
        public decimal? Quantity;

        /// <summary>
        /// To search for quantities equal, greater than or less than <see cref="Quantity"/> 
        /// </summary>
        public DoubleValueOperator QuantityOperator;

        /// <summary>
        /// To search by a specific unit ID
        /// </summary>
        public RecordIdentifier UnitID;

        /// <summary>
        /// If null then the Status flag is ignored at search. Non-null values matches the <see cref="InventoryJournalStatus"/> enumeration (Posted, Active, PartialPosted).
        /// </summary>
        public int? Status;

        /// <summary>
        /// 
        /// </summary>
        public RecordIdentifier ReasonCodeID;

        /// <summary>
        /// To search by posted date - minimum date
        /// </summary>
        public Date PostedDateFrom;

        /// <summary>
        /// To search by posted date - maximum date
        /// </summary>
        public Date PostedDateTo;

        /// <summary>
        /// Search by staff ID
        /// </summary>
        public RecordIdentifier StaffID;

        /// <summary>
        /// Search by staff Login
        /// </summary>
        public RecordIdentifier StaffLogin;

        /// <summary>
        /// Search by area ID
        /// </summary>
        public Guid AreaID;

        /// <summary>
        /// Sort by specified field
        /// </summary>
        public InventoryJournalTransactionSorting SortBy;

        /// <summary>
        /// True if should sort descending
        /// </summary>
        public bool SortBackwards;

        /// <summary>
        /// Get rows from specified row number
        /// </summary>
        public int RowFrom;

        /// <summary>
        /// Get rows until specified row number
        /// </summary>
        public int RowTo;

        /// <summary>
        /// Search for barcodes (tokenized)
        /// </summary>
        public List<string> Barcode;

        /// <summary>
        /// If true then the search string is in the beginning of the barcode
        /// </summary>
        public bool BarcodeBeginsWith;
        

        public InventoryJournalLineSearch()
        {
            Description = new List<string>();
            DescriptionBeginsWith = true;
            VariantDescription = new List<string>();
            VariantDescriptionBeginsWith = true;
            Quantity = null;
            UnitID = RecordIdentifier.Empty;
            Status = null;
            PostedDateFrom = Date.Empty;
            PostedDateTo = Date.Empty;
            StaffID = RecordIdentifier.Empty;
            AreaID = Guid.Empty;
            SortBackwards = false;
            SortBy = InventoryJournalTransactionSorting.IdentificationNumber;
            RowFrom = 0;
            RowTo = 0;
            Barcode = new List<string>();            
        }
    }
}

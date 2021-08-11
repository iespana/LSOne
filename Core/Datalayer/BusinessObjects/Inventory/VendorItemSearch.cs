using LSOne.Utilities.DataTypes;
using System;
using System.Collections.Generic;

namespace LSOne.DataLayer.BusinessObjects.Inventory
{
    /// <summary>
    /// Search criteria for VendorItem
    /// </summary>
    public class VendorItemSearch
    {
        /// <summary>
        /// To search by a specific vendor ID
        /// </summary>
        public RecordIdentifier VendorID;

        /// <summary>
        /// To search for vendor item id, item id, description and variant (tokenized)
        /// </summary>
        public List<string> Description;

        /// <summary>
        /// If true then the search string is in the beginning of the description or ID r variant
        /// </summary>
        public bool DescriptionBeginsWith;

        /// <summary>
        /// To search by a specific unit ID
        /// </summary>
        public RecordIdentifier UnitID;

        /// <summary>
        /// To search by last ordering date - minimum date
        /// </summary>
        public Date LastOrderingDateFrom;

        /// <summary>
        /// To search by last ordering date - maximum date
        /// </summary>
        public Date LastOrderingDateTo;

        public VendorItemSearch()
        {
            VendorID = RecordIdentifier.Empty;
            Description = new List<string>();
            DescriptionBeginsWith = true;
            UnitID = RecordIdentifier.Empty;
            LastOrderingDateFrom = Date.Empty;
            LastOrderingDateTo = Date.Empty;
        }
    }
}

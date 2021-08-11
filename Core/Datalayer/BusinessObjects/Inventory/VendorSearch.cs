using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.BusinessObjects.Inventory
{
    public class VendorSearch
    {
        /// <summary>
        /// To search by a specific vendor ID
        /// </summary>
        public RecordIdentifier VendorID;

        /// <summary>
        /// To search for description or ID (tokenized)
        /// </summary>
        public List<string> Description;

        /// <summary>
        /// If true then the search string is in the beginning of the description or ID
        /// </summary>
        public bool DescriptionBeginsWith;

        /// <summary>
        /// If null then the deleted flag is ignored when searching. Otherwise if set the search will return either deleted or active vendors
        /// Default value is false
        /// </summary>
        public bool? Deleted;

        public VendorSearch()
        {
            VendorID = RecordIdentifier.Empty;
            Deleted = false;
            Description = new List<string>();
            DescriptionBeginsWith = true;
        }
    }
}

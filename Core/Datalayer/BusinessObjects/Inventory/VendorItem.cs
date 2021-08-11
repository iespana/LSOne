using System;
using LSOne.Utilities.DataTypes;
using LSOne.Utilities.Validation;
using LSOne.DataLayer.BusinessObjects.Enums;

namespace LSOne.DataLayer.BusinessObjects.Inventory
{
    public class VendorItem : DataEntity
    {
        public VendorItem()
            : base()
        {
            ID = RecordIdentifier.Empty;

            VendorItemID = "";
            RetailItemID = "";
            VariantName = "";
            UnitID = "";
            VendorID = "";
            LastOrderDate = Date.Empty;
        }


        /// <summary>
        /// The ID that the vendor uses to identify the product.
        /// </summary>
        [RecordIdentifierValidation(30)]
        public RecordIdentifier VendorItemID { get; set; }

        /// <summary>
        /// The retail item ID as the LS Retail systems use to identify the product
        /// </summary>
        [RecordIdentifierValidation(30)]
        public RecordIdentifier RetailItemID { get; set; }

        /// <summary>
        /// The retail item type as is in the LS Retail system.
        /// </summary>
        public ItemTypeEnum RetailItemType { get; set; }

        /// <summary>
        /// The Variant name of the specific variant
        /// </summary>
        public string VariantName { get; set; }

        /// <summary>
        /// The unit ID of the product as the vendor delivers it
        /// </summary>
        [RecordIdentifierValidation(20)]
        public RecordIdentifier UnitID { get; set; }

        /// <summary>
        /// The vendor ID
        /// </summary>
        [RecordIdentifierValidation(20)]
        public RecordIdentifier VendorID { get; set; }

        public string VendorDescription { get; set; }

        /// <summary>
        /// The items price for the specific vendor
        /// </summary>
        public decimal LastItemPrice { get; set; }
        public decimal DefaultPurchasePrice { get; set; }
        public Date LastOrderDate { get; set; }
        public string UnitDescription { get; set; }
    }
}

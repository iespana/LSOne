using DevExpress.Data.Filtering.Helpers;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.Utilities.DataTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LSOne.Services.SupportClasses
{
    /// <summary>
    /// Used in the ItemSearchDialog to filter the item search results
    /// </summary>
    internal class ItemSearchFilter
    {
        public RecordIdentifier RetailGroupID { get; set; }
        public RecordIdentifier RetailDepartmentID { get; set; }
        public RecordIdentifier TaxGroupID { get; set; }
        public RecordIdentifier VariantGroupID { get; set; }
        public RecordIdentifier VendorID { get; set; }
        public string BarCode { get; set; }
        public RecordIdentifier SpecialGroupID { get; set; }
        public ItemTypeEnum? ItemType { get; set; }
    }
}

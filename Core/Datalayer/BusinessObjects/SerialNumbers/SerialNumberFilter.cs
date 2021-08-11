using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.Utilities.DataTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LSOne.DataLayer.BusinessObjects.SerialNumbers
{
    public class SerialNumberFilter
    {
        /// <summary>
        /// Filters serial numbers only for specified item. If Empty then all serial numbers for all items are retrived.
        /// </summary>
        public RecordIdentifier ItemMasterID { get; set; }

        /// <summary>
        /// Filter by description
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// If true, use begins with for the filter otherwise contains will be used
        /// </summary>
        public bool DescriptionBeginsWith { get; set; }

        public string Variant { get; set; }
        /// <summary>
        /// If true, use begins with for the filter otherwise contains will be used
        /// </summary>
        public bool VariantBeginsWith { get; set; }

        /// <summary>
        /// Filter by serial number
        /// </summary>
        public string SerialNumber { get; set; }
        /// <summary>
        /// If true, use begins with for the filter otherwise contains will be used
        /// </summary>
        public bool SerialNumberBeginsWith { get; set; }

        /// <summary>
        /// Filter by serial type
        /// </summary>
        public TypeOfSerial? SerialType { get; set; }

        /// <summary>
        /// Filters useddate between SoldStartDate and SoldEndDate
        /// </summary>
        public DateTime? SoldStartDate { get; set; }
        public DateTime? SoldEndDate { get; set; }

        private bool _ShowSoldSerialNumbers = true; 
        /// <summary>
        /// If true sold serial numbers will be retrived. Otherwise only serial numbers that do not have a used date set will be retrieved.
        /// </summary>
        public bool ShowSoldSerialNumbers
        {
            get
            {
                return _ShowSoldSerialNumbers;
            }
            set
            {
                _ShowSoldSerialNumbers = value;
            }
        }

        private bool _ShowReservedSerialNumbers = true;
        /// <summary>
        /// If true reserved serial numbers will be retrived. Otherwise only serial numbers that are not reserved will be retrieved.
        /// </summary>
        public bool ShowReservedSerialNumbers
        {
            get
            {
                return _ShowReservedSerialNumbers;
            }
            set
            {
                _ShowReservedSerialNumbers = value;
            }
        }

        public bool ManualEntrySet { get; set; }
        public bool ManualEntry { get; set; }

        public string Reference { get; set; }
        /// <summary>
        /// If true, use begins with for the filter otherwise contains will be used
        /// </summary>
        public bool ReferenceBeginsWith { get; set; }

        /// <summary>
        /// Filter by retail group. When empty no filter applied
        /// </summary>
        public RecordIdentifier RetailGroup { get; set; }

        /// <summary>
        /// Filter by departament. When empty no filter applied
        /// </summary>
        public RecordIdentifier RetailDepartment { get; set; }

        /// <summary>
        /// Filter by special group. When empty no filter applied
        /// </summary>
        public RecordIdentifier SpecialGroup { get; set; }

        public string Barcode { get; set; }
        /// <summary>
        /// If true, use begins with for the filter otherwise contains will be used
        /// </summary>
        public bool BarcodeBeginsWith { get; set; }

        /// <summary>
        /// Column used for sorting
        /// </summary>
        public SerialNumberSorting SortBy { get; set; }
        public bool SortAscending { get; set; }

        /// <summary>
        /// When filter only rows bwteen RowFrom and RowTo are retrieved
        /// </summary>
        public int RowFrom { get; set; }
        /// <summary>
        /// When filter only rows bwteen RowFrom and RowTo are retrieved
        /// </summary>
        public int RowTo { get; set; }

        /// <summary>
        /// If true, deleted items will be included in the search results
        /// </summary>
        public bool ShowDeletedItems { get; set; }
    }
}

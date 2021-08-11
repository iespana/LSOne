using LSOne.Utilities.DataTypes;
using System;

namespace LSOne.DataLayer.BusinessObjects.ItemMaster
{
    public enum RetailItemCostSort
    {
        Store,
        Price,
        InventoryQuantity,
        TotalCostValue,
        CalculationDate,
        Reason,
        User
    }

    public class RetailItemCostFilter
    {
        public string Description { get; set; }
        public bool DescriptionBeginsWith { get; set; }
        public string City { get; set; }
        public bool CityBeginsWith { get; set; }
        public RecordIdentifier RegionID { get; set; }
        public RecordIdentifier CurrencyID { get; set; }
        public DateTime? CalculationDateFrom { get; set; }
        public DateTime? CalculationDateTo { get; set; }
        public RecordIdentifier User { get; set; }
        public RetailItemCostSort Sort { get; set; }
        public bool SortDescending { get; set; }

        public int RowFrom { get; set; }
        public int RowTo { get; set; }

        public RetailItemCostFilter()
        {
            DescriptionBeginsWith = true;
            CityBeginsWith = true;
            RegionID = RecordIdentifier.Empty;
            CurrencyID = RecordIdentifier.Empty;
            RowTo = 500;
        }
    }
}

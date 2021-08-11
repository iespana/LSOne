using LSOne.DataLayer.DataProviders.StoreManagement;

namespace LSOne.DataLayer.BusinessObjects.StoreManagement.ListItems
{
    /// <summary>
    /// Minimal store information used for display
    /// </summary>
    public class StoreListItem : DataEntity
    {
        public StoreListItem() : base()
        {
            City = "";
        }

        /// <summary>
        /// City name
        /// </summary>
        public string City { get; set; }

        /// <summary>
        /// Formatted display text. Populated on UI when needed.
        /// </summary>
        public string DisplayName { get; set; }
    }

    public class StoreListSearchFilter
    {
        public StoreListSearchFilter()
        {
            DescriptionOrID = "";
            DescriptionOrIDBeginsWith = true;
            City = "";
            CityBeginsWith = true;
            Sort = StoreSorting.ID;
            SortBackwards = false;
        }

        /// <summary>
        /// Filter by store description or ID
        /// </summary>
        public string DescriptionOrID { get; set; }

        /// <summary>
        /// True if the store's description or ID begins with the search text. False otherwise
        /// </summary>
        public bool DescriptionOrIDBeginsWith { get; set; }

        /// <summary>
        /// Filter by store's city
        /// </summary>
        public string City { get; set; }

        /// <summary>
        /// True if the store's city begins with the search text. False otherwise
        /// </summary>
        public bool CityBeginsWith { get; set; }

        /// <summary>
        /// Enum that specifies the sort column
        /// </summary>
        public StoreSorting Sort { get; set; }
        
        /// <summary>
        /// True if results should be sorted descending. False otherwise
        /// </summary>
        public bool SortBackwards { get; set; }

        /// <summary>
        /// Maximum number of stores to retrieve. 0 will retrieve all stores.
        /// </summary>
        public int MaxCount { get; set; }
    }
}

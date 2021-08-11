using LSOne.Utilities.DataTypes;
using System;

namespace LSOne.DataLayer.BusinessObjects.ItemMaster
{
    public enum RetailItemAssemblyStatus
    {
        Disabled,
        EnabledNotStarted,
        Enabled,
        Archived
    }

    public class RetailItemAssemblySearchFilter
    {
        public RecordIdentifier StoreID { get; set; }
        public bool AllStores { get; set; }
        public RetailItemAssemblyStatus? AssemblyStatus { get; set; }
        public DateTime? StartingDateFrom { get; set; }
        public DateTime? StartingDateTo { get; set; }
        public RecordIdentifier ItemID { get; set; }
    }
}

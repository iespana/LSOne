using LSOne.DataLayer.BusinessObjects.Inventory;
using LSOne.DataLayer.DataProviders.Inventory;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.SqlDataProviders.ItemMaster;
using LSOne.Utilities.DataTypes;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace LSOne.DataLayer.SqlDataProviders.Inventory
{
    public partial class InventoryData
    {
        public List<InventoryStatus> GetInventoryListForAssemblyItemAndStore(IConnectionManager entry, RecordIdentifier itemID, RecordIdentifier storeID, RecordIdentifier regionID, InventorySorting sort, bool backwardsSort)
        {
            ValidateSecurity(entry);

            using (var cmd = entry.Connection.CreateCommand())
            {
                string storeCheck = "";

                if (!RecordIdentifier.IsEmptyOrNull(storeID))
                {
                    storeCheck = " RST.STOREID = @STOREID ";
                    MakeParam(cmd, "STOREID", storeID);
                }
                else if (!RecordIdentifier.IsEmptyOrNull(regionID))
                {
                    storeCheck = " RST.REGIONID = @REGIONID ";
                    MakeParam(cmd, "REGIONID", (string)regionID);
                }

                cmd.CommandText = AssemblyItemQueries.AssemblyInventoryQuery
                    .Replace("#1", storeCheck == "" ? "" : " AND " + storeCheck);

                MakeParam(cmd, "ITEMID", (string)itemID);

                return SortList(
                    GroupAssemblyStatuses(entry, Execute<AssemblyInventoryStatus>(entry, cmd, CommandType.Text, PopulateAssemblyInventoryStatus), itemID),
                    sort, backwardsSort);
            }
        }

        public decimal GetInventoryOnHandForAssemblyItem(IConnectionManager entry, RecordIdentifier itemID, RecordIdentifier storeID)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                string storeCheck = "";

                if (!RecordIdentifier.IsEmptyOrNull(storeID))
                {
                    storeCheck = " RST.STOREID = @STOREID ";
                    MakeParam(cmd, "STOREID", storeID);
                }

                cmd.CommandText = AssemblyItemQueries.AssemblyInventoryQuery
                    .Replace("#1", storeCheck == "" ? "" : " AND " + storeCheck);

                MakeParam(cmd, "ITEMID", (string)itemID);

                var listOfStatuses = GroupAssemblyStatuses(entry, Execute<AssemblyInventoryStatus>(entry, cmd, CommandType.Text, PopulateAssemblyInventoryStatus), itemID);

                return (listOfStatuses.Count > 0) ? listOfStatuses[0].InventoryQuantity : 0;
            }
        }

        public List<InventoryStatus> GetInventoryListForAssemblyItem(IConnectionManager entry, RecordIdentifier itemID, RecordIdentifier regionID)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                string storeCheck = "";

                if (!RecordIdentifier.IsEmptyOrNull(regionID))
                {
                    storeCheck = " RST.REGIONID = @REGIONID ";
                    MakeParam(cmd, "REGIONID", (string)regionID);
                }

                cmd.CommandText = AssemblyItemQueries.AssemblyInventoryQuery
                    .Replace("#1", storeCheck == "" ? "" : " AND " + storeCheck);

                MakeParam(cmd, "ITEMID", (string)itemID);

                return SortList(
                    GroupAssemblyStatuses(entry, Execute<AssemblyInventoryStatus>(entry, cmd, CommandType.Text, PopulateAssemblyInventoryStatus), itemID),
                    InventorySorting.Store, false);
            }
        }

        private static void PopulateAssemblyInventoryStatus(IDataReader dr, AssemblyInventoryStatus inventoryStatus)
        {
            inventoryStatus.ItemID = (string)dr["ITEMID"];
            inventoryStatus.ItemName = (string)dr["ITEMNAME"];
            inventoryStatus.ComponentItemID = (string)dr["COMPONENTITEMID"];
            inventoryStatus.InventoryQuantity = (decimal)dr["INVENTORYQUANTITY"];
            inventoryStatus.ComponentQuantity = (decimal)dr["COMPONENTQUANTITY"];
            inventoryStatus.StoreID = (string)dr["STOREID"];
            inventoryStatus.StoreName = (string)dr["STORENAME"];
            inventoryStatus.VariantName = (string)dr["VARIANTNAME"];
            inventoryStatus.InventoryUnitId = (string)dr["UNITID"];
            inventoryStatus.InventoryUnitDescription = (string)dr["UNITTEXT"];
            inventoryStatus.HasHeaderItem = (bool)dr["HEADERITEM"];

            var minUnitDecimals = (int)dr["MINUNITDECIMALS"];
            var maxUnitDecimals = (int)dr["MAXUNITDECIMALS"];
            inventoryStatus.QuantityLimiter = new DecimalLimit(minUnitDecimals, maxUnitDecimals);
        }

        private List<InventoryStatus> GroupAssemblyStatuses(IConnectionManager entry, List<AssemblyInventoryStatus> statuses, RecordIdentifier baseItemID)
        {
            if(statuses == null || statuses.Count == 0)
            {
                return new List<InventoryStatus>();
            }

            InventoryStatus baseItemStatus = statuses.Find(x => x.ItemID == baseItemID);

            //The base item may not be in the results if we have only assembly items as components
            if(baseItemStatus == null)
            {
                List<InventoryStatus> baseStatuses = GetInventoryStatusesForItem(entry, baseItemID, null);

                if(baseStatuses.Count > 0)
                {
                    baseItemStatus = baseStatuses[0];
                }
            }

            List<InventoryStatus> inventoryStatuses = new List<InventoryStatus>();

            var storeGroups = statuses.GroupBy(x => x.StoreID);

            foreach(IGrouping<RecordIdentifier, AssemblyInventoryStatus> group in storeGroups)
            {
                decimal minQuantity = decimal.MaxValue;
                decimal availableQuantity = 0;
                decimal componentQuantity = 0;
                bool hasHeaderItem = false;

                foreach(AssemblyInventoryStatus status in group)
                {
                    if(status.HasHeaderItem)
                    {
                        hasHeaderItem = true;
                        break;
                    }

                    List<AssemblyInventoryStatus> duplicates = group.Where(x => x.ComponentItemID == status.ComponentItemID).ToList();

                    componentQuantity = duplicates.Count > 1 ? duplicates.Sum(x => x.ComponentQuantity) : status.ComponentQuantity;

                    if(componentQuantity != 0)
                    {
                        availableQuantity = status.InventoryQuantity / componentQuantity;
                    }
                    else
                    {
                        availableQuantity = 0;
                    }

                    if (availableQuantity < minQuantity)
                    {
                        minQuantity = availableQuantity;
                    }
                }

                inventoryStatuses.Add(new InventoryStatus
                {
                    ItemID = baseItemStatus.ItemID,
                    ItemName = baseItemStatus.ItemName,
                    InventoryQuantity = minQuantity,
                    StoreID = group.First().StoreID,
                    StoreName = group.First().StoreName,
                    VariantName = baseItemStatus.VariantName,
                    InventoryUnitId = baseItemStatus.InventoryUnitId,
                    InventoryUnitDescription = baseItemStatus.InventoryUnitDescription,
                    QuantityLimiter = baseItemStatus.QuantityLimiter,
                    HasHeaderItem = hasHeaderItem
                });
            }

            return inventoryStatuses;
        }

        private List<InventoryStatus> SortList(List<InventoryStatus> statuses, InventorySorting sort, bool backwards)
        {
            switch(sort)
            {
                case InventorySorting.Store:
                    return backwards ? statuses.OrderByDescending(x => x.StoreID).ToList() : statuses.OrderBy(x => x.StoreID).ToList();
                case InventorySorting.Quantity:
                    return backwards ? statuses.OrderByDescending(x => x.InventoryQuantity).ToList() : statuses.OrderBy(x => x.InventoryQuantity).ToList();
                default:
                    return statuses;
            }
        }
    }
}

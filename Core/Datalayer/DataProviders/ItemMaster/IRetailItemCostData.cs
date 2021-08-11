using LSOne.DataLayer.BusinessObjects.ItemMaster;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LSOne.DataLayer.DataProviders.ItemMaster
{
    public interface IRetailItemCostData : IDataProvider<RetailItemCost>
    {
        /// <summary>
        /// Get the last cost for an item and store
        /// </summary>
        /// <param name="entry">Database connection</param>
        /// <param name="itemID">ID of the item for which to get the cost</param>
        /// <param name="storeID">ID of the store for which to get the cost. Empty ID will return an average of all stores</param>
        /// <returns></returns>
        RetailItemCost Get(IConnectionManager entry, RecordIdentifier itemID, RecordIdentifier storeID);

        /// <summary>
        /// Get a list of item costs for all stores
        /// </summary>
        /// <param name="entry">Database connection</param>
        /// <param name="itemID">Item ID for which to get the cost</param>
        /// <param name="filter">Search filter</param>
        /// <param name="totalCount">Total items found</param>
        /// <returns></returns>
        List<RetailItemCost> GetList(IConnectionManager entry, RecordIdentifier itemID, RetailItemCostFilter filter, out int totalCount);

        /// <summary>
        /// Move all item costs to an archive table except the last calculated cost for each store
        /// </summary>
        /// <param name="entry">Database connection</param>
        void ArchiveRecords(IConnectionManager entry);
    }
}

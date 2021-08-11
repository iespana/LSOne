using System.Collections.Generic;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.Inventory;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.DataProviders.Inventory
{
    public interface IItemLedgerData : IDataProviderBase<ItemLedger>
    {
        List<ItemLedger> GetList(IConnectionManager entry, RecordIdentifier itemLedgerID);
        List<ItemLedger> GetList(IConnectionManager entry, ItemLedgerSearchParameters itmeLedgerSearchParameters);

        /// <summary>
        /// Gets the total number of ledger entries of the given item
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="itemID">The item ID of the item</param>
        /// <returns></returns>
        int GetLedgerEntryCountForItem(IConnectionManager entry, RecordIdentifier itemID);
    }
}
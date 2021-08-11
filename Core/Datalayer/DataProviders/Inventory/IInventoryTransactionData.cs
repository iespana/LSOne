using System;
using System.Collections.Generic;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.Inventory;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.DataProviders.Inventory
{
    public interface IInventoryTransactionData : IDataProvider<InventoryTransaction>
    {
        InventoryTransaction Get(IConnectionManager entry, RecordIdentifier inventoryTransactionID);

        List<InventoryTransaction> GetList(IConnectionManager entry, InventoryTypeEnum inventoryType, string storeID);

        List<InventoryTransaction> GetList(IConnectionManager entry, RecordIdentifier itemID, 
            int rowFrom, int rowTo,
            string storeID, DateTime startDate, DateTime endDate);

        void Save(IConnectionManager entry, InventoryTransaction inventoryTransaction, bool shouldBeReplicated = true);
        void PostStatementToInventory(IConnectionManager entry, RecordIdentifier statementID, RecordIdentifier storeID, DateTime postingDate);
        void UpdateInventoryFromTransaction(IConnectionManager entry, RecordIdentifier transactionID, RecordIdentifier storeID, RecordIdentifier terminalID);
        List<InventoryTransaction> GetInventoryTransactionsFromTransaction(IConnectionManager entry, RecordIdentifier transactionID, RecordIdentifier storeID, RecordIdentifier terminalID);
    }
}
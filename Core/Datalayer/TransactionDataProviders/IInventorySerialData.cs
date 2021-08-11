using System.Collections.Generic;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.TransactionObjects;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.TransactionDataProviders
{
    public interface IInventorySerialData : IDataProviderBase<InventorySerial>
    {
        /// <summary>
        /// Gets a list for the itemID and RFIDTagID, the itemID is mandatory however the RFIDTagID is not
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="itemIDAndRFID">List constraints</param>
        /// <returns></returns>
        List<InventorySerial> Get(IConnectionManager entry, RecordIdentifier itemIDAndRFID);

        bool Exists(IConnectionManager entry, RecordIdentifier inventorySerialID);
        void Save(IConnectionManager entry, InventorySerial serial);
    }
}
using System.Collections.Generic;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.ItemMaster;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.DataProviders.ItemMaster
{
    public interface IItemLedgerData : IDataProviderBase<ItemLedger>
    {
        List<ItemLedger> GetList(IConnectionManager entry, RecordIdentifier itemLedgerID);
    }
}
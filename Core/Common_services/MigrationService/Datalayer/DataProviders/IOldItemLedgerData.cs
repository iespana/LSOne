using System.Collections.Generic;
using LSOne.DataLayer.BusinessObjects.ItemMaster;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Services.Datalayer.BusinessObjects;
using LSOne.Utilities.DataTypes;

namespace LSOne.Services.Datalayer.DataProviders
{
    public interface IOldItemLedgerData : IDataProviderBase<OldItemLedger>
    {
        List<OldItemLedger> GetList(IConnectionManager entry, RecordIdentifier itemLedgerID);
    }
}
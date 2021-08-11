using System.Collections.Generic;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.StoreManagement;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.DataProviders.Operations
{
    public interface IBlankOperationData : IDataProvider<BlankOperation>, ISequenceable
    {
        List<BlankOperation> GetBlankOperations(IConnectionManager entry);

        BlankOperation Get(IConnectionManager entry, RecordIdentifier id);
    }
}
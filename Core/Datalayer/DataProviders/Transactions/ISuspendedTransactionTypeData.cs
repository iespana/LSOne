using System.Collections.Generic;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Transactions;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.DataProviders.Transactions
{
    public interface ISuspendedTransactionTypeData : IDataProvider<SuspendedTransactionType>
    {
        List<SuspendedTransactionType> GetList(IConnectionManager entry);
        bool InUse(IConnectionManager entry, RecordIdentifier suspensionTransactionID);

        SuspendedTransactionType Get(IConnectionManager entry, RecordIdentifier id);
    }
}
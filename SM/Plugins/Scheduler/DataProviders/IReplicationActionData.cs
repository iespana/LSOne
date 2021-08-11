using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;
using LSOne.ViewPlugins.Scheduler.BusinessObjects;

namespace LSOne.ViewPlugins.Scheduler.DataProviders
{
    public interface IReplicationActionData : IDataProviderBase<ReplicationAction>
    {
        List<ReplicationAction> Get(IConnectionManager entry, string objectName);
        List<ReplicationAction> Get(IConnectionManager entry, string objectName, string tableName);
        List<ReplicationAction> Get(IConnectionManager entry, string objectName, string tableName, RecordIdentifier subjob);
        void Delete(IConnectionManager entry, RecordIdentifier actionCounter);
        void DeleteOlder(IConnectionManager entry, string objectName, string tableName, RecordIdentifier actionCounter);
    }
}


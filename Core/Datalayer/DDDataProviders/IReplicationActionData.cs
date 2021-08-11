using System.Collections.Generic;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DDBusinessObjects;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.DDDataProviders
{
    public interface IReplicationActionData : IDataProviderBase<ReplicationAction>
    {
        List<ReplicationAction> Get(IConnectionManager entry, string objectName);
        List<ReplicationAction> Get(IConnectionManager entry, string objectName, string tableName);
        List<ReplicationAction> Get(IConnectionManager entry, string objectName, string tableName, RecordIdentifier subjob);

        /// <summary>
        /// Gets the latest action ID from the given action table
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="tableName">The name of the action table to get the action ID from</param>
        /// <returns></returns>
        int GetMaxActionID(IConnectionManager entry, string tableName);

        void Delete(IConnectionManager entry, RecordIdentifier actionCounter);
        void DeleteOlder(IConnectionManager entry, string objectName, string tableName, RecordIdentifier actionCounter);
    }
}


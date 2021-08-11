using System.Collections.Generic;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.LookupValues;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.DataProviders.LookupValues
{
    public interface IRemoteHostData : IDataProvider<RemoteHost>, ISequenceable
    {
        /// <summary>
        /// Gets a list of all remote hosts
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <returns>A list of RemoteHost objects containing all remote host records</returns>
        List<RemoteHost> GetList(IConnectionManager entry);

        RemoteHost Get(IConnectionManager entry, RecordIdentifier remoteHostID);
    }
}
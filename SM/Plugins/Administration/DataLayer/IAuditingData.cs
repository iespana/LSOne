using System;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;
using LSOne.ViewPlugins.Administration.QueryResults;

namespace LSOne.ViewPlugins.Administration.DataLayer
{
    internal enum DeleteAuditLogsResult
    {
        Success,
        TimeoutException,
        UnknownException
    }

    internal interface IAuditingData : IDataProviderBase<AuditLogResult>
    {
        /// <summary>
        /// Returns Success if everything goes well. Otherwise returns the associated error result.
        /// </summary>
        DeleteAuditLogsResult DeleteAuditLogs(IConnectionManager entry, int commandTimeout, DateTime toDate);
        AuditLogResult GetAuditLog(IConnectionManager entry, string logName, RecordIdentifier contextID,string userName,DateTime from,DateTime to);
    }
}
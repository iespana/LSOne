using System.Collections.Generic;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Terminals;
using LSOne.DataLayer.BusinessObjects.TimeKeeper;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.DataProviders.TimeKeeper
{
    public interface ITimeKeeperData : IDataProvider<TimeKept>
    {
        List<TimeKept> GetListForUser(IConnectionManager entry, RecordIdentifier userID);

        TimeKept Get(IConnectionManager entry, RecordIdentifier timeID);

        TimeKept GetLastTimeKept(IConnectionManager entry, RecordIdentifier user);
        List<TimeInterval> GetReport(IConnectionManager entry, RecordIdentifier userGuid);
    }
}
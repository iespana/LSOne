using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DDBusinessObjects;
using LSOne.DataLayer.GenericConnector.Interfaces;

namespace LSOne.DataLayer.DDDataProviders
{
    public interface IInfoData : IDataProviderBase<JscInfo>
    {
        JscInfo GetInfo(IConnectionManager entry, string name);
        SchedulerSettings GetSchedulerSettings(IConnectionManager entry);
        void Save(IConnectionManager entry, SchedulerSettings schedulerSettings);
    }
}
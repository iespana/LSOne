using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.ViewPlugins.Scheduler.BusinessObjects;

namespace LSOne.ViewPlugins.Scheduler.DataProviders
{
    public interface IInfoData : IDataProviderBase<JscInfo>
    {
        JscInfo GetInfo(IConnectionManager entry, string name);
        SchedulerSettings GetSchedulerSettings(IConnectionManager entry);
        void Save(IConnectionManager entry, SchedulerSettings schedulerSettings);
    }
}
using LSOne.DataLayer.DataProviders;

namespace LSOne.DataLayer.DDDataProviders
{
    public static class DDProviders
    { 
        static public IInfoData InfoData { get { return DataProviderFactory.Instance.Get<IInfoData, DDBusinessObjects.JscInfo>(); } }
        static public IJobData JobData { get { return DataProviderFactory.Instance.Get<IJobData, DDBusinessObjects.JscJob>(); } }
        static public ILocationData LocationData { get { return DataProviderFactory.Instance.Get<ILocationData, DDBusinessObjects.JscLocation>(); } }
        static public IDesignData DesignData { get { return DataProviderFactory.Instance.Get<IDesignData, DDBusinessObjects.JscTableDesign>(); } }
        static public IReplicationActionData ReplicationActionData { get { return DataProviderFactory.Instance.Get<IReplicationActionData, DDBusinessObjects.ReplicationAction>(); } }
        
    }
}

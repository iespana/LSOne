using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.GenericConnector.Interfaces;

namespace LSOne.DataLayer.DataProviders.Info
{
    public interface IPosisInfoData : IDataProvider<DataEntity>
    {
        string Get(IConnectionManager entry, string id);
    }
}
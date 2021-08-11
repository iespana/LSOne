using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.DataProviders
{
    public interface IDataProvider<BusinessObject> : IDataProviderBase<BusinessObject> where BusinessObject : class, new()
    {
        void Save(IConnectionManager entry, BusinessObject item);
        void Delete(IConnectionManager entry, RecordIdentifier ID);
        bool Exists(IConnectionManager entry, RecordIdentifier ID);
    }
}

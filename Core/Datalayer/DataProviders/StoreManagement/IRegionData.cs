using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Customers;
using LSOne.DataLayer.BusinessObjects.StoreManagement;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;
using System.Collections.Generic;

namespace LSOne.DataLayer.DataProviders.StoreManagement
{
    public interface IRegionData : IDataProvider<Region>, ISequenceable
    {
        Region Get(IConnectionManager entry, RecordIdentifier ID);
        List<Region> GetList(IConnectionManager entry, Region.SortEnum sortBy, bool sortDescending);
        List<DataEntity> GetStoresByRegion(IConnectionManager entry, RecordIdentifier regionID, Region.SortEnum sortBy, bool sortDescending);
    }
}

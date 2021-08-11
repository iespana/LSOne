using System.Collections.Generic;
using LSOne.DataLayer.BusinessObjects.ItemMaster.Dimensions;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Services.Datalayer.BusinessObjects.Dimensions;
using LSOne.Utilities.DataTypes;

namespace LSOne.Services.Datalayer.DataProviders.Dimensions
{
    public interface IOldRetailItemDimensionData : IDataProviderBase<OldRetailItemDimension>
    {
        OldRetailItemDimension Get(IConnectionManager entry, RecordIdentifier templateID);

        List<OldRetailItemDimension> GetListForRetailItem(IConnectionManager entry, RecordIdentifier retailItemMasterID);
        
        void Delete(IConnectionManager entry, RecordIdentifier dimensionID);

        void Save(IConnectionManager entry, OldRetailItemDimension dimmension);
    }
}

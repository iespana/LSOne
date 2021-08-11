using System.Collections.Generic;
using LSOne.DataLayer.BusinessObjects.ItemMaster.Dimensions;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Services.Datalayer.BusinessObjects.Dimensions;
using LSOne.Utilities.DataTypes;

namespace LSOne.Services.Datalayer.DataProviders.Dimensions
{
    public interface IOldDimensionAttributeData : IDataProviderBase<OldDimensionAttribute>
    {
        OldDimensionAttribute Get(IConnectionManager entry, RecordIdentifier attributeID);

        List<OldDimensionAttribute> GetListForRetailItemDimension(IConnectionManager entry, RecordIdentifier retailItemDimensionID);

        List<OldDimensionAttribute> GetListForDimensionTemplate(IConnectionManager entry, RecordIdentifier dimensionTemplateID);

        void Delete(IConnectionManager entry, RecordIdentifier templateID);
        void Save(IConnectionManager entry, OldDimensionAttribute attribute);
    }
}

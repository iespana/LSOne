using System.Collections.Generic;
using LSOne.DataLayer.BusinessObjects.ItemMaster.Dimensions;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Services.Datalayer.BusinessObjects.Dimensions;
using LSOne.Utilities.DataTypes;

namespace LSOne.Services.Datalayer.DataProviders.Dimensions
{
    public interface IOldDimensionTemplateData : IDataProviderBase<OldDimensionTemplate>
    {
        OldDimensionTemplate Get(IConnectionManager entry, RecordIdentifier templateID);

        List<OldDimensionTemplate> GetList(IConnectionManager entry);
        

        void Delete(IConnectionManager entry, RecordIdentifier templateID);

        void Save(IConnectionManager entry, OldDimensionTemplate template);
    }
}

using LSOne.DataLayer.BusinessObjects.ItemMaster.Dimensions;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LSOne.DataLayer.DataProviders.ItemMaster.Dimensions
{
    public interface IDimensionTemplateData : IDataProviderBase<DimensionTemplate>
    {
        DimensionTemplate Get(IConnectionManager entry, RecordIdentifier templateID);
        DimensionTemplate GetByName(IConnectionManager entry, RecordIdentifier templateName);

        List<DimensionTemplate> GetList(IConnectionManager entry);
        

        void Delete(IConnectionManager entry, RecordIdentifier templateID);
        
        void Save(IConnectionManager entry, DimensionTemplate template);

        RecordIdentifier SaveAndReturnTemplateID(IConnectionManager entry, DimensionTemplate template);

    }
}

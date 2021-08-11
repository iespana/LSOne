using System.Collections.Generic;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.Tax;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.DataProviders.Tax
{
    public interface ITaxItemData : IDataProviderBase<TaxItem>
    {
        Dictionary<RecordIdentifier, string> GetTaxGroupDictionary(IConnectionManager entry);
        Dictionary<RecordIdentifier, string> GetItemTaxGroupDictionary(IConnectionManager entry);
        List<TaxItem> GetTaxRate(IConnectionManager entry, RecordIdentifier itemID, RecordIdentifier customerID, RecordIdentifier salesTypeID, UseTaxGroupFromEnum useTaxGroupFrom, bool useOverrideTaxGroup, RecordIdentifier overrideTaxGroup,CacheType cacheType = CacheType.CacheTypeNone);
    }
}
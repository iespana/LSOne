using System.Collections.Generic;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.LookupValues;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.DataProviders.LookupValues
{
    public interface IKeyboardMappingData : IDataProviderBase<KeyboardMapping>
    {
        List<KeyboardMapping> GetMappings(IConnectionManager entry, RecordIdentifier profileID);
        Dictionary<int,KeyboardMapping> GetMappingDictionary(IConnectionManager entry, RecordIdentifier profileID);
    }
}
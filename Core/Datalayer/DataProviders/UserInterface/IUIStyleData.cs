using System.Collections.Generic;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.UserInterface;
using LSOne.DataLayer.BusinessObjects.UserInterface.ListItems;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.DataProviders.UserInterface
{
    public interface IUIStyleData : IDataProvider<UIStyle>
    {
        List<UIStyleListItem> GetList(IConnectionManager entry, bool sortBackwards);
        List<UIStyleListItem> GetList(IConnectionManager entry, RecordIdentifier contextID, bool sortBackwards);
        UIStyle Get(IConnectionManager entry, RecordIdentifier id, bool resolveInheritedStyles = false, 
            CacheType cacheType = CacheType.CacheTypeNone);
    }
}
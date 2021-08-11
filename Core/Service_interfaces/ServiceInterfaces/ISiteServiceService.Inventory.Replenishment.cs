using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LSOne.DataLayer.BusinessObjects.Inventory;
using LSOne.DataLayer.BusinessObjects.Profiles;
using LSOne.DataLayer.BusinessObjects.Replenishment;
using LSOne.DataLayer.DataProviders.Inventory;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;

namespace LSOne.Services.Interfaces
{
    public partial interface ISiteServiceService
    {
        void DeleteItemReplenishmentSetting(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier settingsID, bool closeConnection);
        
        ItemReplenishmentSetting GetItemReplenishmentSettingItemSettingForStore(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier itemId, RecordIdentifier storeId, bool closeConnection);
        
        ItemReplenishmentSetting GetItemReplenishmentSettingForItem(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier itemId, bool closeConnection, bool includeUnitData = false);

        ItemReplenishmentSetting GetItemReplenishmentSetting(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier settingID, bool closeConnection);

        void SaveItemReplenishmentSetting(IConnectionManager entry, SiteServiceProfile siteServiceProfile, ItemReplenishmentSetting setting, bool closeConnection);

        List<ItemReplenishmentSetting> GetItemReplenishmentSettingListForStores(IConnectionManager entry, SiteServiceProfile siteServiceProfile, bool closeConnection, RecordIdentifier itemID, bool includeUnitData = false);

        RecordIdentifier GetItemReplenishmentSettingID(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier itemID, RecordIdentifier storeID, bool closeConnection);

        void DeleteItemReplenishmentSettingByItemIDAndStoreID(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier itemID, RecordIdentifier storeID, bool closeConnection);




    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.Inventory;
using LSOne.DataLayer.BusinessObjects.Profiles;
using LSOne.DataLayer.BusinessObjects.Replenishment;
using LSOne.DataLayer.BusinessObjects.Replenishment.ListItems;
using LSOne.DataLayer.DataProviders.Inventory;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;

namespace LSOne.Services
{
    public partial class InventoryService
    {
        public virtual void DeleteItemReplenishmentSetting(IConnectionManager entry, SiteServiceProfile siteServiceProfile,
            RecordIdentifier settingsID, bool closeConnection)
        {
            Interfaces.Services.SiteServiceService(entry).DeleteItemReplenishmentSetting(entry, siteServiceProfile, settingsID, closeConnection);
        }

        public virtual ItemReplenishmentSetting GetItemReplenishmentSettingItemSettingForStore(IConnectionManager entry,
            SiteServiceProfile siteServiceProfile, RecordIdentifier itemId, RecordIdentifier storeId, bool closeConnection)
        {
            return Interfaces.Services.SiteServiceService(entry).GetItemReplenishmentSettingItemSettingForStore(entry, siteServiceProfile, itemId,storeId, closeConnection);
        }

        public virtual ItemReplenishmentSetting GetItemReplenishmentSettingForItem(IConnectionManager entry,
            SiteServiceProfile siteServiceProfile, RecordIdentifier itemId, bool closeConnection, bool includeUnitData = false)
        {
            return Interfaces.Services.SiteServiceService(entry).GetItemReplenishmentSettingForItem(entry, siteServiceProfile, itemId, closeConnection, includeUnitData);

        }

        public virtual ItemReplenishmentSetting GetItemReplenishmentSetting(IConnectionManager entry, SiteServiceProfile siteServiceProfile,
            RecordIdentifier settingID, bool closeConnection)
        {
            return Interfaces.Services.SiteServiceService(entry).GetItemReplenishmentSetting(entry, siteServiceProfile, settingID, closeConnection);
        }

        public virtual void SaveItemReplenishmentSetting(IConnectionManager entry, SiteServiceProfile siteServiceProfile,
            ItemReplenishmentSetting setting, bool closeConnection)
        {
            Interfaces.Services.SiteServiceService(entry)
                .SaveItemReplenishmentSetting(entry, siteServiceProfile, setting, closeConnection);
        }

        public virtual List<ItemReplenishmentSetting> GetItemReplenishmentSettingListForStores(IConnectionManager entry, SiteServiceProfile siteServiceProfile,
            bool closeConnection, RecordIdentifier itemID, bool includeUnitData = false)
        {
            return Interfaces.Services.SiteServiceService(entry).GetItemReplenishmentSettingListForStores(entry, siteServiceProfile,closeConnection,itemID, includeUnitData);

        }

        public virtual RecordIdentifier GetItemReplenishmentSettingID(IConnectionManager entry, SiteServiceProfile siteServiceProfile,
            RecordIdentifier itemID, RecordIdentifier storeID, bool closeConnection)
        {
            return Interfaces.Services.SiteServiceService(entry).GetItemReplenishmentSettingID(entry, siteServiceProfile, itemID, storeID,closeConnection);
        }

        public virtual void DeleteItemReplenishmentSettingByItemIDAndStoreID(IConnectionManager entry, SiteServiceProfile siteServiceProfile,
            RecordIdentifier itemID, RecordIdentifier storeID, bool closeConnection)
        {
            Interfaces.Services.SiteServiceService(entry).DeleteItemReplenishmentSettingByItemIDAndStoreID(entry, siteServiceProfile, itemID, storeID, closeConnection);
        }
    }
}

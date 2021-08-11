using System.Collections.Generic;
using LSOne.DataLayer.BusinessObjects.Inventory;
using LSOne.DataLayer.BusinessObjects.Profiles;
using LSOne.DataLayer.BusinessObjects.Replenishment;
using LSOne.DataLayer.BusinessObjects.Replenishment.ListItems;
using LSOne.DataLayer.DataProviders.Inventory;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;

namespace LSOne.Services
{
    public partial class SiteServiceService
    {
        public virtual void DeleteItemReplenishmentSetting(IConnectionManager entry, SiteServiceProfile siteServiceProfile,
            RecordIdentifier settingsID, bool closeConnection)
        {
            
            DoRemoteWork(entry, siteServiceProfile, () => server.DeleteItemReplenishmentSetting(CreateLogonInfo(entry), settingsID), closeConnection);

            
        }

        public virtual ItemReplenishmentSetting GetItemReplenishmentSettingItemSettingForStore(IConnectionManager entry,
            SiteServiceProfile siteServiceProfile, RecordIdentifier itemId, RecordIdentifier storeId, bool closeConnection)
        {
            ItemReplenishmentSetting result = null;

            DoRemoteWork(entry, siteServiceProfile, () => result = server.GetItemReplenishmentSettingItemSettingForStore(CreateLogonInfo(entry), itemId, storeId), closeConnection);

            return result;
        }

        public virtual ItemReplenishmentSetting GetItemReplenishmentSettingForItem(IConnectionManager entry,
            SiteServiceProfile siteServiceProfile, RecordIdentifier itemId, bool closeConnection, bool includeUnitData = false)
        {
            ItemReplenishmentSetting result = null;

            DoRemoteWork(entry, siteServiceProfile, () => result = server.GetItemReplenishmentSettingForItem(CreateLogonInfo(entry), itemId, includeUnitData), closeConnection);

            return result;
        }

        public virtual ItemReplenishmentSetting GetItemReplenishmentSetting(IConnectionManager entry, SiteServiceProfile siteServiceProfile,
            RecordIdentifier settingID, bool closeConnection)
        {
            ItemReplenishmentSetting result = null;

            DoRemoteWork(entry, siteServiceProfile, () => result = server.GetItemReplenishmentSetting(CreateLogonInfo(entry), settingID), closeConnection);

            return result;
        }

        public virtual void SaveItemReplenishmentSetting(IConnectionManager entry, SiteServiceProfile siteServiceProfile,
            ItemReplenishmentSetting setting, bool closeConnection)
        {
            DoRemoteWork(entry, siteServiceProfile, () => server.SaveItemReplenishmentSetting(CreateLogonInfo(entry), setting), closeConnection);

        }

        public virtual List<ItemReplenishmentSetting> GetItemReplenishmentSettingListForStores(IConnectionManager entry, SiteServiceProfile siteServiceProfile,
            bool closeConnection, RecordIdentifier itemID, bool includeUnitData = false)
        {
            List<ItemReplenishmentSetting> result = null;

            DoRemoteWork(entry, siteServiceProfile, () => result = server.GetItemReplenishmentSettingListForStores(CreateLogonInfo(entry), itemID,includeUnitData), closeConnection);

            return result;
        }

        public virtual RecordIdentifier GetItemReplenishmentSettingID(IConnectionManager entry, SiteServiceProfile siteServiceProfile,
            RecordIdentifier itemID, RecordIdentifier storeID, bool closeConnection)
        {
            RecordIdentifier result = null;

            DoRemoteWork(entry, siteServiceProfile, () => result = server.GetItemReplenishmentSettingID(CreateLogonInfo(entry), itemID, storeID), closeConnection);

            return result;
        }

        public virtual void DeleteItemReplenishmentSettingByItemIDAndStoreID(IConnectionManager entry, SiteServiceProfile siteServiceProfile,
            RecordIdentifier itemID, RecordIdentifier storeID, bool closeConnection)
        {
            DoRemoteWork(entry, siteServiceProfile, () =>  server.DeleteItemReplenishmentSettingByItemIDAndStoreID(CreateLogonInfo(entry), itemID, storeID), closeConnection);

        }
    }
}
using System.Collections.Generic;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Replenishment;
using LSOne.DataLayer.BusinessObjects.Replenishment.Containers;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.DataProviders.Replenishment
{
    public interface IItemReplenishmentSettingData : IDataProvider<ItemReplenishmentSetting>
    {
        ItemReplenishmentSetting GetForItem(IConnectionManager entry, RecordIdentifier itemId, bool includeUnitData = false);
        List<ItemReplenishmentSetting> GetForItemAndStore(IConnectionManager entry, RecordIdentifier itemId, bool includeUnitData = false);

        /// <summary>
        /// Returns ItemReplenishmentSettings for an item. If the store is overwriting some of the values then the overwritten values are returned.
        /// </summary>
        /// <param name="entry"></param>
        /// <param name="itemId"></param>
        /// <param name="storeId"></param>
        /// <returns></returns>
        ItemReplenishmentSetting GetItemSettingForStore(IConnectionManager entry, RecordIdentifier itemId, RecordIdentifier storeId);

        List<ItemReplenishmentSetting> GetListForStores(IConnectionManager entry, RecordIdentifier itemId, bool includeUnitData = false);
        ItemReplenishmentSettingsContainer GetReplenishmentSettingsForExcel(IConnectionManager entry, RecordIdentifier itemId);

        ItemReplenishmentSetting Get(IConnectionManager entry, RecordIdentifier settingsId);

        /// <summary>
        /// Gets ID of a ItemReplenishmentSetting from given ItemID and StoreID
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="itemId">ID of the RetailTem (not MasterID)</param>
        /// <param name="storeId">ID of the store</param>
        /// <returns></returns>
        RecordIdentifier GetItemReplenishmentSettingID(IConnectionManager entry, RecordIdentifier itemId, RecordIdentifier storeId);


        /// <summary>
        /// Deletes a record by a itemID and storeID
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="itemID">ID of the retail item (not MasterID)</param>
        /// <param name="storeID">ID of the store</param>
        void Delete(IConnectionManager entry, RecordIdentifier itemID, RecordIdentifier storeID);
    }
}
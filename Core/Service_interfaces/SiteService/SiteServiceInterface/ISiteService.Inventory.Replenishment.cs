using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.Inventory;
using LSOne.DataLayer.BusinessObjects.Replenishment;
using LSOne.DataLayer.DataProviders.Inventory;
using LSOne.Services.Interfaces.SupportClasses;
using LSOne.Utilities.DataTypes;
using LSRetail.SiteService.SiteServiceInterface.DTO;

namespace LSRetail.SiteService.SiteServiceInterface
{
    public partial interface ISiteService
    {

        [OperationContract]
        void DeleteItemReplenishmentSetting(LogonInfo logonInfo, RecordIdentifier settingsID);

        [OperationContract]
        ItemReplenishmentSetting GetItemReplenishmentSettingItemSettingForStore(LogonInfo logonInfo, RecordIdentifier itemId, RecordIdentifier storeId);

        [OperationContract]
        ItemReplenishmentSetting GetItemReplenishmentSettingForItem(LogonInfo logonInfo, RecordIdentifier itemId, bool includeUnitData = false);

        [OperationContract]
        ItemReplenishmentSetting GetItemReplenishmentSetting(LogonInfo logonInfo, RecordIdentifier settingID);

        [OperationContract]
        void SaveItemReplenishmentSetting(LogonInfo logonInfo, ItemReplenishmentSetting setting);

        [OperationContract]
        List<ItemReplenishmentSetting> GetItemReplenishmentSettingListForStores(LogonInfo logonInfo, RecordIdentifier itemId, bool includeUnitData = false);

        [OperationContract]
        RecordIdentifier GetItemReplenishmentSettingID(LogonInfo logonInfo, RecordIdentifier itemID, RecordIdentifier storeID);

        [OperationContract]
        void DeleteItemReplenishmentSettingByItemIDAndStoreID(LogonInfo logonInfo, RecordIdentifier itemID, RecordIdentifier storeID);



    }
}

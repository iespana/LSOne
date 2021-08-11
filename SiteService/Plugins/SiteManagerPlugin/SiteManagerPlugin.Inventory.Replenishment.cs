using System;
using System.Collections.Generic;
using LSOne.DataLayer.BusinessObjects.Inventory;
using LSOne.DataLayer.BusinessObjects.Replenishment;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Services.Interfaces.SupportClasses;
using LSOne.SiteService.Utilities;
using LSOne.Utilities.DataTypes;

namespace LSOne.SiteService.Plugins.SiteManager
{
    public partial class SiteManagerPlugin
    {
        /// <summary>
        /// Deletes the item replenishment setting with the given ID
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <param name="settingsID"></param>
        public virtual void DeleteItemReplenishmentSetting(LogonInfo logonInfo, RecordIdentifier settingsID)
        {
            IConnectionManager entry = GetConnectionManager(logonInfo);

            try
            {
                Utils.Log(this, $"{Utils.Starting} {nameof(settingsID)}: {settingsID}");

                Providers.ItemReplenishmentSettingData.Delete(entry, settingsID);
            }
            catch (Exception ex)
            {
                Utils.LogException(this, ex);

                throw GetChannelError(ex);
            }
            finally
            {
                ReturnConnection(entry, out entry);

                Utils.Log(this, Utils.Done);
            }
        }

        /// <summary>
        /// Gets the item replenishment setting for the given pair of item ID and store ID
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <param name="itemID"></param>
        /// <param name="storeID"></param>
        /// <returns></returns>
        public virtual ItemReplenishmentSetting GetItemReplenishmentSettingItemSettingForStore(LogonInfo logonInfo, RecordIdentifier itemID, RecordIdentifier storeID)
        {
            IConnectionManager entry = GetConnectionManager(logonInfo);

            try
            {
                Utils.Log(this, $"{Utils.Starting} {nameof(itemID)}: {itemID}, {nameof(storeID)}: {storeID}");

                return Providers.ItemReplenishmentSettingData.GetItemSettingForStore(entry, itemID,storeID);
            }
            catch (Exception ex)
            {
                Utils.LogException(this, ex);

                throw GetChannelError(ex);
            }
            finally
            {
                ReturnConnection(entry, out entry);

                Utils.Log(this, Utils.Done);
            }
        }

        /// <summary>
        /// Gets the item replenishment setting with the given ID
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <param name="settingID"></param>
        /// <returns></returns>
        public virtual ItemReplenishmentSetting GetItemReplenishmentSetting(LogonInfo logonInfo, RecordIdentifier settingID)
        {
            IConnectionManager entry = GetConnectionManager(logonInfo);

            try
            {
                Utils.Log(this, $"{Utils.Starting} {nameof(settingID)}: {settingID}");

                return Providers.ItemReplenishmentSettingData.Get(entry, settingID);
            }
            catch (Exception ex)
            {
                Utils.LogException(this, ex);

                throw GetChannelError(ex);
            }
            finally
            {
                ReturnConnection(entry, out entry);

                Utils.Log(this, Utils.Done);
            }
        }

        /// <summary>
        /// Gets the item replenishment setting with the given item ID
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <param name="itemID"></param>
        /// <param name="includeUnitData"></param>
        /// <returns></returns>
        public virtual ItemReplenishmentSetting GetItemReplenishmentSettingForItem(LogonInfo logonInfo, RecordIdentifier itemID, bool includeUnitData = false)
        {
            IConnectionManager entry = GetConnectionManager(logonInfo);

            try
            {
                Utils.Log(this, $"{Utils.Starting} {nameof(itemID)}: {itemID}, {nameof(includeUnitData)}: {includeUnitData}");

                return Providers.ItemReplenishmentSettingData.GetForItem(entry, itemID, includeUnitData);
            }
            catch (Exception ex)
            {
                Utils.LogException(this, ex);

                throw GetChannelError(ex);
            }
            finally
            {
                ReturnConnection(entry, out entry);

                Utils.Log(this, Utils.Done);
            }
        }

        /// <summary>
        /// Saves the given item replenishment setting
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <param name="setting"></param>
        public virtual void SaveItemReplenishmentSetting(LogonInfo logonInfo, ItemReplenishmentSetting setting)
        {
            IConnectionManager entry = GetConnectionManager(logonInfo);

            try
            {
                Utils.Log(this, Utils.Starting);

                Providers.ItemReplenishmentSettingData.Save(entry, setting);
            }
            catch (Exception ex)
            {
                Utils.LogException(this, ex);

                throw GetChannelError(ex);
            }
            finally
            {
                ReturnConnection(entry, out entry);

                Utils.Log(this, Utils.Done);
            }
        }

        /// <summary>
        /// Gets a list of item replenishment settings for all stores by the given item ID
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <param name="itemID"></param>
        /// <param name="includeUnitData"></param>
        /// <returns></returns>
        public virtual List<ItemReplenishmentSetting> GetItemReplenishmentSettingListForStores(LogonInfo logonInfo, RecordIdentifier itemID, bool includeUnitData = false)
        {
            IConnectionManager entry = GetConnectionManager(logonInfo);

            try
            {
                Utils.Log(this, $"{Utils.Starting} {nameof(itemID)}: {itemID}, {nameof(includeUnitData)}: {includeUnitData}");

                return Providers.ItemReplenishmentSettingData.GetListForStores(entry, itemID,includeUnitData);
            }
            catch (Exception ex)
            {
                Utils.LogException(this, ex);

                throw GetChannelError(ex);
            }
            finally
            {
                ReturnConnection(entry, out entry);

                Utils.Log(this, Utils.Done);
            }
        }

        /// <summary>
        /// Gets the item replenishment setting ID by the given pair of item ID and store ID
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <param name="itemID"></param>
        /// <param name="storeID"></param>
        /// <returns></returns>
        public virtual RecordIdentifier GetItemReplenishmentSettingID(LogonInfo logonInfo, RecordIdentifier itemID, RecordIdentifier storeID)
        {
            IConnectionManager entry = GetConnectionManager(logonInfo);

            try
            {
                Utils.Log(this, $"{Utils.Starting} {nameof(itemID)}: {itemID}, {nameof(storeID)}: {storeID}");

                return Providers.ItemReplenishmentSettingData.GetItemReplenishmentSettingID(entry, itemID, storeID);
            }
            catch (Exception ex)
            {
                Utils.LogException(this, ex);

                throw GetChannelError(ex);
            }
            finally
            {
                ReturnConnection(entry, out entry);

                Utils.Log(this, Utils.Done);
            }
        }

        /// <summary>
        /// Deletes the item replenishment setting by the given pair of item ID and store ID
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <param name="itemID"></param>
        /// <param name="storeID"></param>
        public virtual void DeleteItemReplenishmentSettingByItemIDAndStoreID(LogonInfo logonInfo, RecordIdentifier itemID, RecordIdentifier storeID)
        {
            IConnectionManager entry = GetConnectionManager(logonInfo);

            try
            {
                Utils.Log(this, $"{Utils.Starting} {nameof(itemID)}: {itemID}, {nameof(storeID)}: {storeID}");

                Providers.ItemReplenishmentSettingData.Delete(entry, itemID, storeID);
            }
            catch (Exception ex)
            {
                Utils.LogException(this, ex);

                throw GetChannelError(ex);
            }
            finally
            {
                ReturnConnection(entry, out entry);

                Utils.Log(this, Utils.Done);
            }
        }

        /// <summary>
        /// Check whether an item with the same parameters (eg: same unit) exists in the transfer order containing the given transfer order line
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <param name="line"></param>
        /// <returns></returns>
        public virtual bool ItemWithSameParametersExistsInTransferOrder(LogonInfo logonInfo, InventoryTransferOrderLine line)
        {
            IConnectionManager entry = GetConnectionManager(logonInfo);

            try
            {
                Utils.Log(this, Utils.Starting);

                return Providers.InventoryTransferOrderLineData.ItemWithSameParametersExists(entry, line);
            }
            catch (Exception ex)
            {
                Utils.LogException(this, ex);

                throw GetChannelError(ex);
            }
            finally
            {
                ReturnConnection(entry, out entry);

                Utils.Log(this, Utils.Done);
            }
        }
    }
}
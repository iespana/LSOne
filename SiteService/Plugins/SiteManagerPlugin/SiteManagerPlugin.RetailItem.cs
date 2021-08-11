using System;
using LSOne.DataLayer.BusinessObjects.ItemMaster;
using LSOne.DataLayer.BusinessObjects.Units;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Services.Interfaces.SupportClasses;
using LSOne.SiteService.Utilities;
using LSOne.Utilities.DataTypes;
using LSOne.DataLayer.BusinessObjects.Enums;
using System.Collections.Generic;

namespace LSOne.SiteService.Plugins.SiteManager
{
    public partial class SiteManagerPlugin
    {
        public virtual void SetItemsDefaultVendor(LogonInfo logonInfo, RecordIdentifier itemID, RecordIdentifier vendorItemID)
        {
            IConnectionManager dataModel = GetConnectionManager(logonInfo);

            try
            {
                Utils.Log(this, $"{Utils.Starting} {nameof(itemID)}: {itemID}, {nameof(vendorItemID)}: {vendorItemID}");

                Providers.RetailItemData.SetItemsDefaultVendor(dataModel, itemID, vendorItemID);
            }
            catch (Exception e)
            {
                Utils.LogException(this, e);
                throw GetChannelError(e);
            }
            finally
            {
                ReturnConnection(dataModel, out dataModel);

                Utils.Log(this, Utils.Done);
            }
        }

        public virtual bool ItemHasDefaultVendor(LogonInfo logonInfo, RecordIdentifier itemID)
        {
            IConnectionManager dataModel = GetConnectionManager(logonInfo);

            try
            {
                Utils.Log(this, $"{Utils.Starting} {nameof(itemID)}: {itemID}");

                return Providers.RetailItemData.ItemHasDefaultVendor(dataModel, itemID);
            }
            catch (Exception e)
            {
                Utils.LogException(this, e);
                throw GetChannelError(e);
            }
            finally
            {
                ReturnConnection(dataModel, out dataModel);

                Utils.Log(this, Utils.Done);
            }
        }

        public virtual RecordIdentifier GetItemsDefaultVendor(LogonInfo logonInfo, RecordIdentifier itemID)
        {
            IConnectionManager dataModel = GetConnectionManager(logonInfo);

            try
            {
                Utils.Log(this, $"{Utils.Starting} {nameof(itemID)}: {itemID}");

                return Providers.RetailItemData.GetItemsDefaultVendor(dataModel, itemID);
            }
            catch (Exception e)
            {
                Utils.LogException(this, e);
                throw GetChannelError(e);
            }
            finally
            {
                ReturnConnection(dataModel, out dataModel);

                Utils.Log(this, Utils.Done);
            }
        }

        /// <summary>
        /// Retrieves information about a specific retail item
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <param name="itemID">The unique ID of the item to be retrieved</param>
        /// <returns>Information about the item</returns>
        public virtual RetailItem GetRetailItem(LogonInfo logonInfo, RecordIdentifier itemID)
        {
            IConnectionManager dataModel = GetConnectionManager(logonInfo);

            try
            {
                Utils.Log(this, $"{Utils.Starting} {nameof(itemID)}: {itemID}");

                return Providers.RetailItemData.Get(dataModel, itemID);
            }
            catch (Exception e)
            {
                Utils.LogException(this, e);
                throw GetChannelError(e);
            }
            finally
            {
                ReturnConnection(dataModel, out dataModel);

                Utils.Log(this, Utils.Done);
            }
        }

        /// <summary>
        /// Retrieves information about a specific retail item even if it is deleted
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <param name="itemID">The unique ID of the item to be retrieved</param>
        /// <returns>Information about the item</returns>
        public virtual RetailItem GetRetailItemIncludeDeleted(LogonInfo logonInfo, RecordIdentifier itemID)
        {
            IConnectionManager dataModel = GetConnectionManager(logonInfo);

            try
            {
                Utils.Log(this, $"{Utils.Starting} {nameof(itemID)}: {itemID}");

                return Providers.RetailItemData.Get(dataModel, itemID, true);
            }
            catch (Exception e)
            {
                Utils.LogException(this, e);
                throw GetChannelError(e);
            }
            finally
            {
                ReturnConnection(dataModel, out dataModel);

                Utils.Log(this, Utils.Done);
            }
        }

        /// <summary>
        /// Saves information about a specific retail item
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <param name="retailItem">The item to be saved</param>
        public virtual void SaveRetailItem(LogonInfo logonInfo, RetailItem retailItem)
        {
            IConnectionManager dataModel = GetConnectionManager(logonInfo);

            try
            {
                Utils.Log(this, Utils.Starting);

                Providers.RetailItemData.Save(dataModel, retailItem);
            }
            catch (Exception e)
            {
                Utils.LogException(this, e);
                ThrowChannelError(e);
            }
            finally
            {
                ReturnConnection(dataModel, out dataModel);

                Utils.Log(this, Utils.Done);
            }
        }

        public virtual void SaveUnitConversionRule(LogonInfo logonInfo, UnitConversion unitConversion)
        {
            IConnectionManager dataModel = GetConnectionManager(logonInfo);

            try
            {
                Utils.Log(this, Utils.Starting);

                Providers.UnitConversionData.Save(dataModel, unitConversion);
            }
            catch (Exception e)
            {
                Utils.LogException(this, e);
                ThrowChannelError(e);
            }
            finally
            {
                ReturnConnection(dataModel, out dataModel);

                Utils.Log(this, Utils.Done);
            }
        }

        /// <summary>
        /// Updates the type on a specific item
        /// </summary>
        /// <param name="logonInfo">The logon information</param>
        /// <param name="itemID">The unique ID of the item to update</param>
        /// <param name="newType">The new type</param>
        public virtual void SaveItemType(LogonInfo logonInfo, RecordIdentifier itemID, ItemTypeEnum newType)
        {
            IConnectionManager dataModel = GetConnectionManager(logonInfo);

            try
            {
                Utils.Log(this, $"{Utils.Starting} {nameof(itemID)}: {itemID}, {nameof(newType)}: {newType}");

                Providers.RetailItemData.UpdateItemType(dataModel, itemID, newType);
            }
            catch (Exception e)
            {
                Utils.LogException(this, e);
                ThrowChannelError(e);
            }
            finally
            {
                ReturnConnection(dataModel, out dataModel);

                Utils.Log(this, Utils.Done);
            }
        }

        public virtual RetailItemCost GetRetailItemCost(LogonInfo logonInfo, RecordIdentifier itemID, RecordIdentifier storeID)
        {
            IConnectionManager dataModel = GetConnectionManager(logonInfo);

            try
            {
                Utils.Log(this, $"{Utils.Starting} {nameof(itemID)}: {itemID}, {nameof(storeID)}: {storeID}");

                return Providers.RetailItemCostData.Get(dataModel, itemID, storeID);
            }
            catch (Exception e)
            {
                Utils.LogException(this, e);
                throw GetChannelError(e);
            }
            finally
            {
                ReturnConnection(dataModel, out dataModel);

                Utils.Log(this, Utils.Done);
            }
        }

        public virtual List<RetailItemCost> GetRetailItemCostList(LogonInfo logonInfo, RecordIdentifier itemID, RetailItemCostFilter filter, out int totalCount)
        {
            IConnectionManager dataModel = GetConnectionManager(logonInfo);

            try
            {
                Utils.Log(this, $"{Utils.Starting} {nameof(itemID)}: {itemID}");

                return Providers.RetailItemCostData.GetList(dataModel, itemID, filter, out totalCount);
            }
            catch (Exception e)
            {
                Utils.LogException(this, e);
                throw GetChannelError(e);
            }
            finally
            {
                ReturnConnection(dataModel, out dataModel);

                Utils.Log(this, Utils.Done);
            }
        }

        public void InsertRetailItemCosts(LogonInfo logonInfo, List<RetailItemCost> itemCosts)
        {
            IConnectionManager dataModel = GetConnectionManager(logonInfo);

            try
            {
                Utils.Log(this, $"{Utils.Starting} {nameof(itemCosts)}: {itemCosts}");

                foreach(RetailItemCost cost in itemCosts)
                {
                    Providers.RetailItemCostData.Save(dataModel, cost);
                }
            }
            catch (Exception e)
            {
                Utils.LogException(this, e);
                throw GetChannelError(e);
            }
            finally
            {
                ReturnConnection(dataModel, out dataModel);

                Utils.Log(this, Utils.Done);
            }
        }

        public void ArchiveItemCosts(LogonInfo logonInfo)
        {
            IConnectionManager dataModel = GetConnectionManager(logonInfo);

            try
            {
                Utils.Log(this, $"{Utils.Starting}");

                Providers.RetailItemCostData.ArchiveRecords(dataModel);
            }
            catch (Exception e)
            {
                Utils.LogException(this, e);
                throw GetChannelError(e);
            }
            finally
            {
                ReturnConnection(dataModel, out dataModel);

                Utils.Log(this, Utils.Done);
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.Inventory;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.Inventory;
using LSOne.DataLayer.GenericConnector;
using LSOne.DataLayer.GenericConnector.Constants;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Services.Interfaces.SupportClasses;
using LSOne.SiteService.Utilities;
using LSOne.Utilities.DataTypes;

namespace LSOne.SiteService.Plugins.SiteManager
{
    public partial class SiteManagerPlugin
    {
        public virtual int MaxOverGoodsReceive(LogonInfo logonInfo)
        {
            IConnectionManager entry = GetConnectionManager(logonInfo);

            try
            {
                Utils.Log(this, Utils.Starting);

                Setting systemSettingsMaxOverReceiveGoods = entry.Settings.GetSetting(entry, Settings.MaxOverReceiveGoods, SettingsLevel.System);
                return Convert.ToInt32(systemSettingsMaxOverReceiveGoods.Value);
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
        
        public virtual List<InventoryStatus> GetInventoryListForItemAndStore(RecordIdentifier itemID, RecordIdentifier storeID, RecordIdentifier regionID, InventorySorting sort,
            bool backwardsSort, LogonInfo logonInfo)
        {
            IConnectionManager entry = GetConnectionManager(logonInfo);

            try
            {
                Utils.Log(this, $"{Utils.Starting} {nameof(itemID)}: {itemID}, {nameof(storeID)}: {storeID}, {nameof(regionID)}: {regionID}");
                List<InventoryStatus> inventoryStatuses = Providers.InventoryData.GetInventoryListForItemAndStore(
                    entry,
                    itemID,
                    storeID,
                    regionID,
                    sort,
                    backwardsSort);

                foreach (var inventoryStatus in inventoryStatuses)
                {
                    RecordIdentifier inventoryUnitID = Providers.InventoryData.GetInventoryUnitId(entry, inventoryStatus.ItemID);
                    inventoryStatus.InventoryOnHand = Providers.InventoryData.GetInventoryOnHand(entry, inventoryStatus.ItemID, inventoryStatus.StoreID);
                    inventoryStatus.OrderedQuantity = Providers.PurchaseOrderLineData.GetSumofOrderedItembyStore(entry, inventoryStatus.ItemID, inventoryStatus.StoreID, false, inventoryUnitID);
                    inventoryStatus.ParkedQuantity = Providers.InventoryJournalTransactionData.GetSumOfReservedItemByStore(entry, inventoryStatus.ItemID, inventoryStatus.StoreID, inventoryUnitID, InventoryJournalTypeEnum.Parked);
                    inventoryStatus.ReservedQuantity = Providers.InventoryJournalTransactionData.GetSumOfReservedItemByStore(entry, inventoryStatus.ItemID, inventoryStatus.StoreID, inventoryUnitID, InventoryJournalTypeEnum.Reservation);
                }

                return inventoryStatuses;
            }
            catch (Exception ex)
            {
                Utils.LogException(this, ex);
                throw  GetChannelError(ex);
            }
            finally
            {
                ReturnConnection(entry, out entry);
                Utils.Log(this, Utils.Done);
            }
        }

        public virtual List<InventoryStatus> GetInventoryListForAssemblyItemAndStore(RecordIdentifier itemID, RecordIdentifier storeID, RecordIdentifier regionID, InventorySorting sort,
            bool backwardsSort, LogonInfo logonInfo)
        {
            IConnectionManager entry = GetConnectionManager(logonInfo);

            try
            {
                Utils.Log(this, $"{Utils.Starting} {nameof(itemID)}: {itemID}, {nameof(storeID)}: {storeID}, {nameof(regionID)}: {regionID}");
                List<InventoryStatus> inventoryStatuses = Providers.InventoryData.GetInventoryListForAssemblyItemAndStore(
                    entry,
                    itemID,
                    storeID,
                    regionID,
                    sort,
                    backwardsSort);

                foreach (var inventoryStatus in inventoryStatuses.Where(x => !x.HasHeaderItem))
                {
                    RecordIdentifier inventoryUnitID = Providers.InventoryData.GetInventoryUnitId(entry, inventoryStatus.ItemID);
                    inventoryStatus.InventoryOnHand = Providers.InventoryData.GetInventoryOnHandForAssemblyItem(entry, inventoryStatus.ItemID, inventoryStatus.StoreID);
                    inventoryStatus.OrderedQuantity = Providers.PurchaseOrderLineData.GetSumofOrderedItembyStore(entry, inventoryStatus.ItemID, inventoryStatus.StoreID, false, inventoryUnitID);
                    inventoryStatus.ParkedQuantity = Providers.InventoryJournalTransactionData.GetSumOfReservedItemByStore(entry, inventoryStatus.ItemID, inventoryStatus.StoreID, inventoryUnitID, InventoryJournalTypeEnum.Parked);
                    inventoryStatus.ReservedQuantity = Providers.InventoryJournalTransactionData.GetSumOfReservedItemByStore(entry, inventoryStatus.ItemID, inventoryStatus.StoreID, inventoryUnitID, InventoryJournalTypeEnum.Reservation);
                }

                return inventoryStatuses;
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
        /// Gets inventory status for all items in a store (or all stores)
        /// </summary>
        /// <param name="logonInfo">Entry into the database</param>
        /// <param name="storeID">The store's ID. Note that if this is RecordIdentifier.Empty then results for all stores will be returned</param>
        /// <param name="sort">Sort enum that determines in which order the results will be returned</param>
        /// <param name="backwardsSort">Results will be returned in backwards order</param>
        /// <param name="rowFrom">Start index</param>
        /// <param name="rowTo">End index</param>
        /// <param name="total">Total number of records</param>
        /// <returns>Inventory status for an item in a store (or all stores)</returns>
        public virtual List<InventoryStatus> GetInventoryListForStore(
            LogonInfo logonInfo,
            RecordIdentifier storeID,
            InventorySorting sort,
            bool backwardsSort,
            int rowFrom,
            int rowTo,
            out int total)
        {
            IConnectionManager entry = GetConnectionManager(logonInfo);

            try
            {
                Utils.Log(this, $"{Utils.Starting} {nameof(storeID)}: {storeID}");
                return Providers.InventoryData.GetInventoryListForStore(entry, storeID, sort, backwardsSort, rowFrom, rowTo, out total);
            }
            catch(Exception ex)
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

        public virtual RecordIdentifier GetInventoryUnitId(RecordIdentifier itemID, LogonInfo logonInfo)
        {
            IConnectionManager entry = GetConnectionManager(logonInfo);

            try
            {
                Utils.Log(this, $"{Utils.Starting} {nameof(itemID)}: {itemID}");
                return Providers.InventoryData.GetInventoryUnitId(entry, itemID);
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

        public virtual decimal GetSumOfReservedItemByStore(RecordIdentifier itemID, RecordIdentifier storeID, RecordIdentifier inventoryUnitID,
            InventoryJournalTypeEnum journalType, LogonInfo logonInfo)
        {
            IConnectionManager entry = GetConnectionManager(logonInfo);

            try
            {
                Utils.Log(this, $"{Utils.Starting} {nameof(itemID)}: {itemID}, {nameof(storeID)}: {storeID}, {nameof(inventoryUnitID)}: {inventoryUnitID}");
                return Providers.InventoryJournalTransactionData.GetSumOfReservedItemByStore(entry, itemID, storeID, inventoryUnitID, journalType);
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

        public virtual void UpdateInventoryUnit(LogonInfo logonInfo, RecordIdentifier itemID, decimal conversionFactor)
        {
            IConnectionManager entry = GetConnectionManager(logonInfo);

            try
            {
                Utils.Log(this, $"{Utils.Starting} {nameof(itemID)}: {itemID}");
                Providers.InventoryData.UpdateInventoryUnit(entry,itemID, conversionFactor);
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

        public virtual decimal GetSumofOrderedItembyStore(RecordIdentifier itemID, RecordIdentifier storeID, bool includeReportFormatting,
            RecordIdentifier inventoryUnitId, LogonInfo logonInfo)
        {
            IConnectionManager entry = GetConnectionManager(logonInfo);

            try
            {
                Utils.Log(this, $"{Utils.Starting} {nameof(itemID)}: {itemID}, {nameof(storeID)}: {storeID}, {nameof(inventoryUnitId)}: {inventoryUnitId}");
                return Providers.PurchaseOrderLineData.GetSumofOrderedItembyStore(entry, itemID, storeID, includeReportFormatting, inventoryUnitId);
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

        public virtual List<ItemLedger> GetItemLedgerList(LogonInfo logonInfo, ItemLedgerSearchParameters itemSearch)
        {
            IConnectionManager entry = GetConnectionManager(logonInfo);

            try
            {
                Utils.Log(this, Utils.Starting);
                return Providers.ItemLedgerData.GetList(entry, itemSearch);
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

        public virtual int GetLedgerEntryCountForItem(LogonInfo logonInfo, RecordIdentifier itemID)
        {
            IConnectionManager entry = GetConnectionManager(logonInfo);

            try
            {
                Utils.Log(this, $"{Utils.Starting} {nameof(itemID)}: {itemID}");
                return Providers.ItemLedgerData.GetLedgerEntryCountForItem(entry, itemID);
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

        public virtual List<InventoryStatus> GetInventoryStatuses(LogonInfo logonInfo, RecordIdentifier storeID,
            InventoryGroup inventoryGroup, RecordIdentifier inventoryGroupID)
        {
            IConnectionManager entry = GetConnectionManager(logonInfo);

            try
            {
                Utils.Log(this, $"{Utils.Starting} + {nameof(storeID)}: {storeID}, {nameof(inventoryGroupID)}: {inventoryGroupID}");
                return Providers.InventoryData.GetInventoryStatuses(entry, storeID, inventoryGroup, inventoryGroupID);
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
        public virtual bool PurchaseOrderExists(LogonInfo logonInfo, RecordIdentifier purchaseOrderID)
        {
            IConnectionManager entry = GetConnectionManager(logonInfo);

            try
            {
                Utils.Log(this, $"{Utils.Starting} {nameof(purchaseOrderID)}: {purchaseOrderID}");
                return Providers.PurchaseOrderData.Exists(entry, purchaseOrderID);
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

        public virtual void SaveInventoryTransaction(LogonInfo logonInfo, InventoryTransaction transaction)
        {
            IConnectionManager entry = GetConnectionManager(logonInfo);

            try
            {
                Utils.Log(this, Utils.Starting);
                Providers.InventoryTransactionData.Save(entry, transaction, false);
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

        public virtual List<InventoryTransaction> GetInventoryTransactionsFromTransaction(LogonInfo logonInfo,
            RecordIdentifier transactionID, RecordIdentifier storeID, RecordIdentifier terminalID)
        {
            IConnectionManager entry = GetConnectionManager(logonInfo);

            try
            {
                Utils.Log(this, $"{Utils.Starting} {nameof(transactionID)}: {transactionID}, {nameof(storeID)}: {storeID}, {nameof(terminalID)}: {terminalID}");
                return Providers.InventoryTransactionData.GetInventoryTransactionsFromTransaction( entry, transactionID,storeID, terminalID);
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

        public virtual void MarkTransactionAsInventoryUpdated(LogonInfo logonInfo, RecordIdentifier transactionID, RecordIdentifier storeID,
            RecordIdentifier terminalID)
        {
            IConnectionManager entry = GetConnectionManager(logonInfo);

            try
            {
                Utils.Log(this, $"{Utils.Starting} {nameof(transactionID)}: {transactionID}, {nameof(storeID)}: {storeID}, {nameof(terminalID)}: {terminalID}");
                Providers.TransactionData.MarkTransactionAsInventoryUpdated(entry, transactionID, storeID, terminalID);
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

        public virtual List<InventoryStatus> GetInventoryStatus(
            RecordIdentifier itemID, RecordIdentifier variantID, RecordIdentifier regionID, LogonInfo logonInfo)
        {
            IConnectionManager dataModel = GetConnectionManager(logonInfo);

            try
            {
                Utils.Log(this, $"{Utils.Starting} {nameof(itemID)}: {itemID}, {nameof(variantID)}: {variantID}, {nameof(regionID)}: {regionID}");
                return Providers.InventoryData.GetInventoryStatusesForItem(dataModel, itemID, regionID);
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
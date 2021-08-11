using System;
using System.Collections.Generic;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.Inventory;
using LSOne.DataLayer.BusinessObjects.Profiles;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.Inventory;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;
using LSRetail.SiteService.SiteServiceInterface.DTO;

namespace LSOne.Services
{
    // This code file is for partners to add new functionality to this service
    public partial class SiteServiceService
    {
        public List<InventoryAdjustment> GetInventoryAdjustmentJournalList(
            IConnectionManager entry,
            SiteServiceProfile siteServiceProfile,
            RecordIdentifier storeId,
            InventoryJournalTypeEnum journalType,
            int journalStatus,
            InventoryAdjustmentSorting sortBy,
            bool sortedBackwards,
            bool closeConnection)
        {
            List<InventoryAdjustment> result = null;

            DoRemoteWork(entry, siteServiceProfile, () => result = server.GetInventoryAdjustmentJournalList(CreateLogonInfo(entry), storeId, journalType, journalStatus, sortBy,
                sortedBackwards), closeConnection);

            return result;
        }

        public List<InventoryJournalTransaction> GetJournalTransactionList(
            IConnectionManager entry,
            SiteServiceProfile siteServiceProfile,
            RecordIdentifier journalTransactionId,
            InventoryJournalTransactionSorting sortBy,
            bool sortedBackwards,
            bool closeConnection)
        {
            List<InventoryJournalTransaction> result = null;

            DoRemoteWork(entry, siteServiceProfile, () => result = server.GetJournalTransactionList(CreateLogonInfo(entry), journalTransactionId, sortBy,
                sortedBackwards), closeConnection);

            return result;
        }

        public DataEntity GetJournalTransactionReasonById(
            IConnectionManager entry,
            SiteServiceProfile siteServiceProfile,
            RecordIdentifier id,
            bool closeConnection)
        {
            DataEntity result = null;

            DoRemoteWork(entry, siteServiceProfile, () => result = server.GetJournalTransactionReasonById(CreateLogonInfo(entry), id), closeConnection);

            return result;
        }

        public virtual void CloseInventoryAdjustment(
            IConnectionManager entry,
            SiteServiceProfile siteServiceProfile,
            RecordIdentifier journalId,
            bool closeConnection)
        {
            DoRemoteWork(entry, siteServiceProfile, () => server.CloseInventoryAdjustment(CreateLogonInfo(entry), journalId), closeConnection);
        }


        public virtual void ReserveStockTransaction(IConnectionManager entry, SiteServiceProfile siteServiceProfile, InventoryJournalTransaction reservation, RecordIdentifier storeID, bool closeConnection)
        {
            DoRemoteWork(entry, siteServiceProfile, () => server.ReserveStockTransaction(CreateLogonInfo(entry), reservation, storeID), closeConnection);
        }

        public virtual void ReserveStockItem(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier itemID, 
                                             RecordIdentifier storeID, decimal adjustment, InventoryTypeEnum inventoryType,
                                             decimal costPrice, decimal netSalesPricePerItem, decimal salesPricePerItem, 
                                             string unitID, decimal adjustmentInventoryUnit, RecordIdentifier reasonCode, string reasonText, bool closeConnection)
        {

            DoRemoteWork(entry, siteServiceProfile, () => server.ReserveStockItem(CreateLogonInfo(entry), itemID, storeID, adjustment, inventoryType, costPrice, netSalesPricePerItem, salesPricePerItem, unitID, adjustmentInventoryUnit, reasonCode, reasonText), closeConnection);
        }

        public InventoryAdjustment SaveInventoryAdjustment(IConnectionManager entry, SiteServiceProfile siteServiceProfile, InventoryAdjustment adjustmentJournal, bool closeConnection)
        {
            InventoryAdjustment result = null;

            DoRemoteWork(entry, siteServiceProfile, () => result = server.SaveInventoryAdjustment(CreateLogonInfo(entry), adjustmentJournal), closeConnection);

            return result;
        }

        public virtual InventoryAdjustment FindInventoryAdjustment(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier masterID, bool closeConnection)
        {

            InventoryAdjustment result = null;

            DoRemoteWork(entry, siteServiceProfile, () => result = server.FindInventoryAdjustment(CreateLogonInfo(entry), masterID), closeConnection);

            return result;
        }

        public virtual void MoveInventoryAdjustment(IConnectionManager entry, SiteServiceProfile siteServiceProfile, InventoryAdjustment adjustmentJournal, bool closeConnection)
        {
            DoRemoteWork(entry, siteServiceProfile, () => server.MoveInventoryAdjustment(CreateLogonInfo(entry), adjustmentJournal), closeConnection);
        }

        public virtual void SaveInventoryAdjustmentReason(IConnectionManager entry,SiteServiceProfile siteServiceProfile, DataEntity inventoryAdjustmentReason, bool closeConnection)
        {
            DoRemoteWork(entry, siteServiceProfile, () => server.SaveInventoryAdjustmentReason(CreateLogonInfo(entry), inventoryAdjustmentReason), closeConnection);
        }

        public virtual RecordIdentifier SaveInventoryAdjustmentReasonReturnID(IConnectionManager entry, SiteServiceProfile siteServiceProfile, DataEntity inventoryAdjustmentReason, bool closeConnection)
        {
            RecordIdentifier result = null;

            DoRemoteWork(entry, siteServiceProfile, () => result = server.SaveInventoryAdjustmentReasonReturnID(CreateLogonInfo(entry), inventoryAdjustmentReason), closeConnection);

            return result;
        }

        public virtual List<DataEntity> GetReasonCodes(IConnectionManager entry, SiteServiceProfile siteServiceProfile, InventoryJournalTypeEnum inventoryType, bool closeConnection)
        {
            List<DataEntity> result = null;

            DoRemoteWork(entry, siteServiceProfile, () => result = server.GetReasonCodes(CreateLogonInfo(entry), inventoryType), closeConnection);

            return result;
        }

        public virtual void UpdateReasonCodes(IConnectionManager entry, SiteServiceProfile siteServiceProfile, List<DataEntity> reasonCodes, InventoryJournalTypeEnum inventoryType, bool closeConnection)
        {
            DoRemoteWork(entry, siteServiceProfile, () => server.UpdateReasonCodes(CreateLogonInfo(entry), reasonCodes, inventoryType), closeConnection);
        }

        public virtual bool InventoryAdjustmentReasonIsInUse(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier reasonID, bool closeConnection)
        {
            bool result = false;

            DoRemoteWork(entry, siteServiceProfile, () => result = server.InventoryAdjustmentReasonIsInUse(CreateLogonInfo(entry), reasonID ), closeConnection);

            return result;
        }

        public void DeleteInventoryAdjustmentReason(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier reasonID, bool closeConnection)
        {
            DoRemoteWork(entry, siteServiceProfile, () => server.DeleteInventoryAdjustmentReason(CreateLogonInfo(entry), reasonID), closeConnection);
        }

    }
}
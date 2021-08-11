using System;
using System.Collections.Generic;
using LSOne.DataLayer.BusinessObjects.Inventory.Containers;
using LSOne.DataLayer.BusinessObjects.Profiles;
using LSOne.DataLayer.BusinessObjects.Replenishment;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;

namespace LSOne.Services
{
    public partial class SiteServiceService
    {
        public virtual List<PurchaseWorksheet> GetInventoryWorksheetList(IConnectionManager entry, SiteServiceProfile siteServiceProfile, bool closeConnection)
        {
            List<PurchaseWorksheet> result = null;

            DoRemoteWork(entry, siteServiceProfile, () => result = server.GetInventoryWorksheetList(CreateLogonInfo(entry)), closeConnection);

            return result;
        }

        public virtual PurchaseWorksheet GetWorksheetFromTemplateIDAndStoreID(IConnectionManager entry,
            SiteServiceProfile siteServiceProfile, RecordIdentifier inventoryTemplateID, RecordIdentifier storeID,
            bool closeConnection)
        {
            PurchaseWorksheet result = null;

            DoRemoteWork(entry, siteServiceProfile, () => result = server.GetWorksheetFromTemplateIDAndStoreID(CreateLogonInfo(entry), inventoryTemplateID, storeID), closeConnection);

            return result;
        }


        public virtual List<PurchaseWorksheet> GetInventoryWorksheetListByInventoryTemplate(IConnectionManager entry, SiteServiceProfile siteServiceProfile,
            RecordIdentifier inventoryTemplateID, bool closeConnection)
        {
            List<PurchaseWorksheet> result = null;

            DoRemoteWork(entry, siteServiceProfile, () => result = server.GetInventoryWorksheetListByInventoryTemplate(CreateLogonInfo(entry), inventoryTemplateID), closeConnection);

            return result;
        }

        public virtual bool InventoryWorksheetExistsForStore(IConnectionManager entry, SiteServiceProfile siteServiceProfile,
            RecordIdentifier storeID, bool closeConnection)
        {
            bool result = false;

            DoRemoteWork(entry, siteServiceProfile, () => result = server.InventoryWorksheetExistsForStore(CreateLogonInfo(entry), storeID), closeConnection);

            return result;
        }

        public virtual PurchaseWorksheet GetPurchaseWorksheet(IConnectionManager entry, SiteServiceProfile siteServiceProfile,
            RecordIdentifier worksheetId, bool closeConnection)
        {
            PurchaseWorksheet result = null;

            DoRemoteWork(entry, siteServiceProfile, () => result = server.GetPurchaseWorksheet(CreateLogonInfo(entry), worksheetId), closeConnection);

            return result;
        }

        public virtual RecordIdentifier SaveInventoryWorksheet(IConnectionManager entry, SiteServiceProfile siteServiceProfile,
            PurchaseWorksheet worksheet, bool closeConnection)
        {
            RecordIdentifier result = null;

            DoRemoteWork(entry, siteServiceProfile, () => result = server.SaveInventoryWorksheet(CreateLogonInfo(entry), worksheet), closeConnection);

            return result;
        }

        public virtual void DeleteInventoryWorksheet(IConnectionManager entry, SiteServiceProfile siteServiceProfile,
            RecordIdentifier worksheetId, bool closeConnection)
        {          
            DoRemoteWork(entry, siteServiceProfile, () => server.DeleteInventoryWorksheet(CreateLogonInfo(entry), worksheetId), closeConnection);
        }

        public virtual List<PurchaseWorksheetLine> GetInventoryWorksheetLineListSimple(IConnectionManager entry, SiteServiceProfile siteServiceProfile,
            RecordIdentifier worksheetId, bool includeDeletedItems, bool closeConnection)
        {
            List<PurchaseWorksheetLine> result = null;

            DoRemoteWork(entry, siteServiceProfile, () => result = server.GetInventoryWorksheetLineListSimple(CreateLogonInfo(entry), worksheetId,includeDeletedItems), closeConnection);

            return result;
        }

        public virtual List<PurchaseWorksheetLine> GetInventoryWorksheetLineList(IConnectionManager entry, SiteServiceProfile siteServiceProfile,
            RecordIdentifier worksheetId, bool includeDeletedItems, PurchaseWorksheetLineSortEnum sortEnum, bool sortBackwards,
            bool closeConnection)
        {
            List<PurchaseWorksheetLine> result = null;

            DoRemoteWork(entry, siteServiceProfile, () => result = server.GetInventoryWorksheetLineList(CreateLogonInfo(entry), worksheetId,includeDeletedItems,sortEnum,sortBackwards), closeConnection);

            return result;
        }

        public virtual void DeleteInventoryWorksheetLineForPurchaseWorksheet(IConnectionManager entry, SiteServiceProfile siteServiceProfile,
            RecordIdentifier purchaseWorksheetID, bool closeConnection)
        {
          
            DoRemoteWork(entry, siteServiceProfile, () => server.DeleteInventoryWorksheetLineForPurchaseWorksheet(CreateLogonInfo(entry), purchaseWorksheetID), closeConnection);
        }

        public virtual PurchaseWorksheetLine GetPurchaseWorksheetLine(IConnectionManager entry, SiteServiceProfile siteServiceProfile,
            RecordIdentifier worksheetLineId, bool closeConnection)
        {
            PurchaseWorksheetLine result = null;

            DoRemoteWork(entry, siteServiceProfile, () => result = server.GetPurchaseWorksheetLine(CreateLogonInfo(entry), worksheetLineId), closeConnection);

            return result;
        }

        public virtual RecordIdentifier SaveInventoryWorksheetLine(IConnectionManager entry, SiteServiceProfile siteServiceProfile,
            PurchaseWorksheetLine worksheetLine, bool closeConnection)
        {
            RecordIdentifier result = null;

            DoRemoteWork(entry, siteServiceProfile, () => result = server.SaveInventoryWorksheetLine(CreateLogonInfo(entry), worksheetLine), closeConnection);

            return result;
        }

        public virtual PostPurchaseWorksheetContainer PostPurchaseWorksheet(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier purchaseWorksheetID, bool closeConnection)
        {
            PostPurchaseWorksheetContainer result = null;
            DoRemoteWork(entry, siteServiceProfile, () => result = server.PostPurchaseWorksheet(CreateLogonInfo(entry), purchaseWorksheetID), closeConnection);
            return result;
        }

        public virtual bool PurchaseWorksheetHasInventoryExcludedItems(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier purchaseWorksheetID, bool closeConnection)
        {
            bool result = false;
            DoRemoteWork(entry, siteServiceProfile, () => result = server.PurchaseWorksheetHasInventoryExcludedItems(CreateLogonInfo(entry), purchaseWorksheetID), closeConnection);
            return result;
        }

        public virtual int CreatePurchaseWorksheetLinesFromFilter(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier purchaseWorksheetID, bool closeConnection)
        {
            int result = 0;
            DoRemoteWork(entry, siteServiceProfile, () => result = server.CreatePurchaseWorksheetLinesFromFilter(CreateLogonInfo(entry), purchaseWorksheetID), closeConnection);
            return result;
        }

        public virtual void RefreshPurchaseWorksheetLines(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier purchaseWorksheetID, bool closeConnection)
        {
            DoRemoteWork(entry, siteServiceProfile, () => server.RefreshPurchaseWorksheetLines(CreateLogonInfo(entry), purchaseWorksheetID), closeConnection);
        }

        public virtual decimal CalculateSuggestedQuantity(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier itemID, RecordIdentifier storeID, RecordIdentifier unitID, bool closeConnection)
        {
            decimal result = 0;
            DoRemoteWork(entry, siteServiceProfile, () => result = server.CalculateSuggestedQuantity(CreateLogonInfo(entry), itemID, storeID, unitID), closeConnection);
            return result;
        }
    }
}

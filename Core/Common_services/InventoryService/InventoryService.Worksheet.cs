using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.Inventory;
using LSOne.DataLayer.BusinessObjects.Inventory.Containers;
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
        public virtual List<PurchaseWorksheet> GetInventoryWorksheetList(IConnectionManager entry, SiteServiceProfile siteServiceProfile, bool closeConnection)
        {
             return Interfaces.Services.SiteServiceService(entry).GetInventoryWorksheetList(entry, siteServiceProfile, closeConnection);
        }

        public virtual PurchaseWorksheet GetWorksheetFromTemplateIDAndStoreID(IConnectionManager entry, SiteServiceProfile siteServiceProfile,
            RecordIdentifier inventoryTemplateID, RecordIdentifier storeID, bool closeConnection)
        {
            return Interfaces.Services.SiteServiceService(entry).GetWorksheetFromTemplateIDAndStoreID(entry, siteServiceProfile, inventoryTemplateID, storeID, closeConnection);
        }

        public virtual List<PurchaseWorksheet> GetInventoryWorksheetListByInventoryTemplate(IConnectionManager entry, SiteServiceProfile siteServiceProfile,
            RecordIdentifier inventoryTemplateID, bool closeConnection)
        {
             return Interfaces.Services.SiteServiceService(entry).GetInventoryWorksheetListByInventoryTemplate(entry, siteServiceProfile, inventoryTemplateID, closeConnection);
        }

        public virtual bool InventoryWorksheetExistsForStore(IConnectionManager entry, SiteServiceProfile siteServiceProfile,
            RecordIdentifier storeID, bool closeConnection)
        {
             return Interfaces.Services.SiteServiceService(entry).InventoryWorksheetExistsForStore(entry, siteServiceProfile, storeID, closeConnection);
        }

        public virtual PurchaseWorksheet GetPurchaseWorksheet(IConnectionManager entry, SiteServiceProfile siteServiceProfile,
            RecordIdentifier worksheetId, bool closeConnection)
        {
             return Interfaces.Services.SiteServiceService(entry).GetPurchaseWorksheet(entry, siteServiceProfile, worksheetId, closeConnection);
        }

        public virtual RecordIdentifier SaveInventoryWorksheet(IConnectionManager entry, SiteServiceProfile siteServiceProfile,
            PurchaseWorksheet worksheet, bool closeConnection)
        {
             return Interfaces.Services.SiteServiceService(entry).SaveInventoryWorksheet(entry, siteServiceProfile, worksheet, closeConnection);
        }

        public virtual void DeleteInventoryWorksheet(IConnectionManager entry, SiteServiceProfile siteServiceProfile,
            RecordIdentifier worksheetId, bool closeConnection)
        {
             Interfaces.Services.SiteServiceService(entry).DeleteInventoryWorksheet(entry, siteServiceProfile, worksheetId, closeConnection);
        }

        public virtual List<PurchaseWorksheetLine> GetInventoryWorksheetLineListSimple(IConnectionManager entry, SiteServiceProfile siteServiceProfile,
            RecordIdentifier worksheetId, bool includeDeletedItems, bool closeConnection)
        {
             return Interfaces.Services.SiteServiceService(entry).GetInventoryWorksheetLineListSimple(entry, siteServiceProfile, worksheetId,includeDeletedItems, closeConnection);
        }

        public virtual List<PurchaseWorksheetLine> GetInventoryWorksheetLineList(IConnectionManager entry, SiteServiceProfile siteServiceProfile,
            RecordIdentifier worksheetId, bool includeDeletedItems, PurchaseWorksheetLineSortEnum sortEnum, bool sortBackwards,
            bool closeConnection)
        {
             return Interfaces.Services.SiteServiceService(entry).GetInventoryWorksheetLineList(entry, siteServiceProfile, worksheetId,includeDeletedItems,sortEnum,sortBackwards, closeConnection);
        }

        public virtual void DeleteInventoryWorksheetLineForPurchaseWorksheet(IConnectionManager entry, SiteServiceProfile siteServiceProfile,
            RecordIdentifier purchaseWorksheetID, bool closeConnection)
        {
             Interfaces.Services.SiteServiceService(entry).DeleteInventoryWorksheetLineForPurchaseWorksheet(entry, siteServiceProfile, purchaseWorksheetID, closeConnection);
        }

        public virtual PurchaseWorksheetLine GetPurchaseWorksheetLine(IConnectionManager entry, SiteServiceProfile siteServiceProfile,
            RecordIdentifier worksheetLineId, bool closeConnection)
        {
             return Interfaces.Services.SiteServiceService(entry).GetPurchaseWorksheetLine(entry, siteServiceProfile, worksheetLineId, closeConnection);
        }

        public virtual RecordIdentifier SaveInventoryWorksheetLine(IConnectionManager entry, SiteServiceProfile siteServiceProfile,
            PurchaseWorksheetLine worksheetLine, bool closeConnection)
        {
             return Interfaces.Services.SiteServiceService(entry).SaveInventoryWorksheetLine(entry, siteServiceProfile, worksheetLine, closeConnection);
        }

        public virtual PostPurchaseWorksheetContainer PostPurchaseWorksheet(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier purchaseWorksheetID, bool closeConnection)
        {
            return Interfaces.Services.SiteServiceService(entry).PostPurchaseWorksheet(entry, siteServiceProfile, purchaseWorksheetID, closeConnection);
        }

        public virtual bool PurchaseWorksheetHasInventoryExcludedItems(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier purchaseWorksheetID, bool closeConnection)
        {
            return Interfaces.Services.SiteServiceService(entry).PurchaseWorksheetHasInventoryExcludedItems(entry, siteServiceProfile, purchaseWorksheetID, closeConnection);
        }

        public virtual int CreatePurchaseWorksheetLinesFromFilter(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier purchaseWorksheetID, bool closeConnection)
        {
            return Interfaces.Services.SiteServiceService(entry).CreatePurchaseWorksheetLinesFromFilter(entry, siteServiceProfile, purchaseWorksheetID, closeConnection);
        }

        public virtual void RefreshPurchaseWorksheetLines(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier purchaseWorksheetID, bool closeConnection)
        {
            Interfaces.Services.SiteServiceService(entry).RefreshPurchaseWorksheetLines(entry, siteServiceProfile, purchaseWorksheetID, closeConnection);
        }

        public virtual decimal CalculateSuggestedQuantity(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier itemID, RecordIdentifier storeID, RecordIdentifier unitID, bool closeConnection)
        {
            return Interfaces.Services.SiteServiceService(entry).CalculateSuggestedQuantity(entry, siteServiceProfile, itemID, storeID, unitID, closeConnection);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.Inventory;
using LSOne.DataLayer.BusinessObjects.Profiles;
using LSOne.DataLayer.BusinessObjects.Replenishment;
using LSOne.DataLayer.BusinessObjects.Replenishment.Containers;
using LSOne.DataLayer.BusinessObjects.Replenishment.ListItems;
using LSOne.DataLayer.BusinessObjects.Units;
using LSOne.DataLayer.DataProviders.Inventory;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;

namespace LSOne.Services
{
    public partial class InventoryService
    {
        public virtual List<PurchaseOrderLine> GetPurchaseOrderLines(IConnectionManager entry, SiteServiceProfile siteServiceProfile, PurchaseOrderLineSearch searchCriteria, PurchaseOrderLineSorting sortBy, bool sortBackwards, out int totalRecordsMatching, bool closeConnection)
        {
            return Interfaces.Services.SiteServiceService(entry).GetPurchaseOrderLines(entry, siteServiceProfile, searchCriteria, sortBy, sortBackwards, out totalRecordsMatching, closeConnection);
        }

        public virtual bool PurchaseOrderLineHasPostedGoodsReceivingDocumentLine(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier purchaseOrderLineID, bool closeConnection)
        {
            return Interfaces.Services.SiteServiceService(entry).PurchaseOrderLineHasPostedGoodsReceivingDocumentLine(entry, siteServiceProfile, purchaseOrderLineID, closeConnection);
        }

        public virtual bool DeletePurchaseOrderLine(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier purchaseOrderLineID, bool closeConnection)
        {
            //Check if any posted GR lines are attached to the PO line
            if (!PurchaseOrderLineHasPostedGoodsReceivingDocumentLine(entry, siteServiceProfile, purchaseOrderLineID, closeConnection))
            {
                //First delete the goods receiving lines (if any) attached to this purchase order line
                DeleteGoodsReceivingLinesForAPurchaseOrderLine(entry, siteServiceProfile, purchaseOrderLineID, false);

                //Delete the PO line
                Interfaces.Services.SiteServiceService(entry).DeletePurchaseOrderLine(entry, siteServiceProfile, purchaseOrderLineID, true);

                return true;
            }

            return false;
        }

        public virtual PurchaseOrderLinesDeleteResult DeletePurchaseOrder(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier purchaseOrderLineID, bool closeConnection)
        {
            return Interfaces.Services.SiteServiceService(entry).DeletePurchaseOrder(entry, siteServiceProfile, purchaseOrderLineID, closeConnection);
        }

        public virtual PurchaseOrder GetPurchaseOrder(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier purchaseOrderID, bool closeConnection)
        {
            return Interfaces.Services.SiteServiceService(entry).GetPurchaseOrder(entry, siteServiceProfile, purchaseOrderID, closeConnection);
        }

        public virtual List<PurchaseOrder> PurchaseOrderAdvancedSearch(
            IConnectionManager entry,
            SiteServiceProfile siteServiceProfile,
            bool closeConnection,
            int rowFrom,
            int rowTo,
            InventoryPurchaseOrderSortEnums sortBy,
            bool sortBackwards,
            out int itemCount,
            List<string> idOrDescription = null,
            bool idOrDescriptionBeginsWith = true,
            RecordIdentifier storeID = null,
            RecordIdentifier vendorID = null,
            PurchaseStatusEnum? status = null,
            Date deliveryDateFrom = null,
            Date deliveryDateTo = null,
            Date creationDateFrom = null,
            Date creationDateTo = null,
            bool onlySearchOpenAndNoGoodsReceivingDocument = false)
        {
            return Interfaces.Services.SiteServiceService(entry).PurchaseOrderAdvancedSearch(
                entry,
                siteServiceProfile,
                closeConnection,
                rowFrom,
                rowTo,
                sortBy,
                sortBackwards,
                out itemCount,
                idOrDescription,
                idOrDescriptionBeginsWith,
                storeID,
                vendorID,
                status,
                deliveryDateFrom,
                deliveryDateTo,
                creationDateFrom,
                creationDateTo,
                onlySearchOpenAndNoGoodsReceivingDocument);
        }

        /// <summary>
        /// Generates a new purchase order ID
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="siteServiceProfile">Which site service to use for this operation</param>
        /// <param name="closeConnection">If true then the connection to the site service is closed as soon as the operation has finished otherwise it will stay open</param>
        /// <returns>New purchase order ID</returns>
        public virtual RecordIdentifier GeneratePurchaseOrderID(IConnectionManager entry,
            SiteServiceProfile siteServiceProfile, bool closeConnection)
        {
            return Interfaces.Services.SiteServiceService(entry).GeneratePurchaseOrderID(entry, siteServiceProfile, closeConnection);

        }

        /// <summary>
        /// Copies all item lines and misc charges from oldPurchaseOrderID to newPurchaseOrderID
        /// </summary>
        /// <param name="entry"></param>
        /// <param name="siteServiceProfile"></param>
        /// <param name="fromPurchaseOrderID"></param>
        /// <param name="newPurchaseOrder"></param>
        /// <param name="storeID"></param>
        /// <param name="taxCalculationMethod"></param>
        /// <param name="closeConnection"></param>
        public virtual void CopyLinesAndBetweenMiscChargesPurchaseOrders(IConnectionManager entry,
            SiteServiceProfile siteServiceProfile, RecordIdentifier fromPurchaseOrderID,
            PurchaseOrder newPurchaseOrder, RecordIdentifier storeID, TaxCalculationMethodEnum taxCalculationMethod, bool closeConnection)
        {
            Interfaces.Services.SiteServiceService(entry).CopyLinesAndBetweenMiscChargesPurchaseOrders(entry, siteServiceProfile, fromPurchaseOrderID, newPurchaseOrder, storeID, taxCalculationMethod, closeConnection);
        }


        /// <summary>
        /// Saves a given purchase order into the database
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="siteServiceProfile">Which site service to use for this operation</param>
        /// <param name="purchaseOrder">The Purchase order to save</param>
        /// <param name="closeConnection">If true then the connection to the site service is closed as soon as the operation has finished otherwise it will stay open</param>
        /// <returns>Purchase order</returns>
        public virtual PurchaseOrder SaveAndReturnPurchaseOrder(IConnectionManager entry, SiteServiceProfile siteServiceProfile,
            PurchaseOrder purchaseOrder, bool closeConnection)
        {
            return Interfaces.Services.SiteServiceService(entry).SaveAndReturnPurchaseOrder(entry, siteServiceProfile, purchaseOrder, closeConnection);
        }

        /// <summary>
        /// Saves a purchase order header
        /// </summary>
        /// <param name="siteServiceProfile">Which site service to use for this operation</param>
        /// <param name="entry">The entry into the database</param>
        /// <param name="closeConnection">If true then the connection to the site service is closed as soon as the operation has finished otherwise it will stay open</param>
        /// <param name="purchaseOrder">The purchase order header to be saved</param>
        public virtual void SavePurchaseOrder(IConnectionManager entry, SiteServiceProfile siteServiceProfile, PurchaseOrder purchaseOrder, bool closeConnection)
        {
            Interfaces.Services.SiteServiceService(entry).SavePurchaseOrder(entry, siteServiceProfile, purchaseOrder, closeConnection);
        }

        /// <summary>
        /// Checks if a goods receiving document exists against a specific purchase order
        /// </summary>
        /// <param name="siteServiceProfile">Which site service to use for this operation</param>
        /// <param name="entry">The entry into the database</param>
        /// <param name="closeConnection">If true then the connection to the site service is closed as soon as the operation has finished otherwise it will stay open</param>
        /// <param name="purchaseOrderID">The ID of the purchase order to be checked</param>
        /// <returns>Returns true if a goods receiving document exists</returns>
        public virtual bool PurchaseOrderHasGoodsReceivingDocument(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier purchaseOrderID, bool closeConnection)
        {
            return Interfaces.Services.SiteServiceService(entry).PurchaseOrderHasGoodsReceivingDocument(entry, siteServiceProfile, purchaseOrderID, closeConnection);
        }

        /// <summary>
        /// Creates and posts a goods receiving document for all lines within the purchase order
        /// </summary>
        /// <param name="siteServiceProfile">Which site service to use for this operation</param>
        /// <param name="entry">The entry into the database</param>
        /// <param name="closeConnection">If true then the connection to the site service is closed as soon as the operation has finished otherwise it will stay open</param>
        /// <param name="purchaseOrder">The purchase order that is to be posted and received</param>
        /// <returns>Result of the posting operation</returns>
        public virtual GoodsReceivingPostResult PostAndReceiveAPurchaseOrder(IConnectionManager entry, SiteServiceProfile siteServiceProfile, PurchaseOrder purchaseOrder, bool closeConnection)
        {
            return Interfaces.Services.SiteServiceService(entry).PostAndReceiveAPurchaseOrder(entry, siteServiceProfile, purchaseOrder, closeConnection);
        }

        /// <summary>
        /// Returns true if the purchase order has any items lines
        /// </summary>
        /// <param name="siteServiceProfile">Which site service to use for this operation</param>
        /// <param name="entry">The entry into the database</param>
        /// <param name="closeConnection">If true then the connection to the site service is closed as soon as the operation has finished otherwise it will stay open</param>
        /// <param name="purchaseOrderID">The ID of the purchase order that is being checked</param>
        /// <returns>Return true if any item lines are on the purchase order</returns>
        public virtual bool PurchaseOrderHasPurchaseOrderLines(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier purchaseOrderID, bool closeConnection)
        {
            return Interfaces.Services.SiteServiceService(entry).PurchaseOrderHasPurchaseOrderLines(entry, siteServiceProfile, purchaseOrderID, closeConnection);
        }

        /// <summary>
        /// Goes through all the purchase order lines and updates either the discount amount and discount percentage on each line
        /// </summary>
        /// <param name="siteServiceProfile">Which site service to use for this operation</param>
        /// <param name="entry">The entry into the database</param>
        /// <param name="closeConnection">If true then the connection to the site service is closed as soon as the operation has finished otherwise it will stay open</param>
        /// <param name="purchaseOrderID">The ID of the purchase order that is being checked</param>
        /// <param name="storeID">The ID of the store the purchase order belongs to</param>
        /// <param name="discountPercentage">The discount % that should be used for updating. If null then this value is ignored</param>
        /// <param name="discountAmount">The discount amount that should be used for updating. If null then this value is ignored</param>
        public virtual void ChangeDiscountsForPurchaseOrderLines(
            IConnectionManager entry,
            SiteServiceProfile siteServiceProfile,
            RecordIdentifier purchaseOrderID,
            RecordIdentifier storeID,
            decimal? discountPercentage,
            decimal? discountAmount,
            bool closeConnection)
        {
            Interfaces.Services.SiteServiceService(entry).ChangeDiscountsForPurchaseOrderLines(entry, siteServiceProfile, purchaseOrderID, storeID, discountPercentage, discountAmount, closeConnection);
        }

        /// <summary>
        /// Deletes a specific miscellanious charge on a purchase order
        /// </summary>
        /// <param name="siteServiceProfile">Which site service to use for this operation</param>
        /// <param name="entry">The entry into the database</param>
        /// <param name="closeConnection">If true then the connection to the site service is closed as soon as the operation has finished otherwise it will stay open</param>
        /// <param name="purchaseOrderMiscChargeID">Unique ID of the miscellanious charge line to be deleted</param>
        public virtual void DeletePurchaseOrderMiscCharges(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier purchaseOrderMiscChargeID, bool closeConnection)
        {
            Interfaces.Services.SiteServiceService(entry).DeletePurchaseOrderMiscCharges(entry, siteServiceProfile, purchaseOrderMiscChargeID, closeConnection);
        }

        /// <summary>
        /// Gets a purchase order misc charges for a given purchase order
        /// </summary>
        /// <param name="siteServiceProfile">Which site service to use for this operation</param>
        /// <param name="entry">The entry into the database</param>
        /// <param name="closeConnection">If true then the connection to the site service is closed as soon as the operation has finished otherwise it will stay open</param>
        /// <param name="purchaseOrderID">The ID of the purchase order to get misc charges for</param>
        /// <param name="includeReportFormatting">Set to true if you want price and quantity formatting, usually for reports</param>
        /// <returns>A purchase order misc charge with a given ID</returns>
        public virtual List<PurchaseOrderMiscCharges> GetMischChargesForPurchaseOrder(
            IConnectionManager entry,
            SiteServiceProfile siteServiceProfile,
            RecordIdentifier purchaseOrderID,
            bool includeReportFormatting,
            bool closeConnection)
        {
            return Interfaces.Services.SiteServiceService(entry).GetMischChargesForPurchaseOrder(entry, siteServiceProfile, purchaseOrderID, includeReportFormatting, closeConnection);
        }

        public virtual InventoryTemplate GetInventoryTemplateForPOWorksheet(IConnectionManager entry,
            SiteServiceProfile siteServiceProfile, RecordIdentifier purchaseWorksheetId, bool closeConnection)
        {
            return Interfaces.Services.SiteServiceService(entry).GetInventoryTemplateForPOWorksheet(entry, siteServiceProfile, purchaseWorksheetId, closeConnection);
        }

        public virtual List<PurchaseWorksheetLine> GetPurchaseWorksheetLineData(
            IConnectionManager entry,
            SiteServiceProfile siteServiceProfile,
            RecordIdentifier worksheetId,
            bool includeDeletedItems,
            PurchaseWorksheetLineSortEnum sortEnum,
            bool sortBackwards,
            bool closeConnection)
        {
            return Interfaces.Services.SiteServiceService(entry)
                .GetPurchaseWorksheetLineData(entry, siteServiceProfile, worksheetId, includeDeletedItems, sortEnum,
                    sortBackwards, closeConnection);
        }

        public virtual List<PurchaseWorksheetLine> GetPurchaseWorksheetLineDataPaged(
            IConnectionManager entry,
            SiteServiceProfile siteServiceProfile,
            RecordIdentifier worksheetId,
            bool includeDeletedItems,
            PurchaseWorksheetLineSortEnum sortEnum,
            bool sortBackwards,
            int rowFrom,
            int rowTo,
            bool closeConnection)
        {
            return Interfaces.Services.SiteServiceService(entry)
                .GetPurchaseWorksheetLineDataPaged(entry, siteServiceProfile, worksheetId, includeDeletedItems, sortEnum,
                    sortBackwards, rowFrom, rowTo, closeConnection);
        }

        /// <summary>
        /// Gets information about a specific miscellanious charge
        /// </summary>
        /// <param name="siteServiceProfile">Which site service to use for this operation</param>
        /// <param name="entry">The entry into the database</param>
        /// <param name="closeConnection">If true then the connection to the site service is closed as soon as the operation has finished otherwise it will stay open</param>
        /// <param name="purchaseOrderMiscChargeID">The ID of the misc charge being retrieved</param>
        /// <param name="includeReportFormatting">Set to true if you want price and quantity formatting, usually for reports</param>
        /// <returns>Information about the msc charge</returns>
        public virtual PurchaseOrderMiscCharges GetPurchaseOrderMiscCharge(
            IConnectionManager entry,
            SiteServiceProfile siteServiceProfile,
            RecordIdentifier purchaseOrderMiscChargeID,
            bool includeReportFormatting,
            bool closeConnection)
        {
            return Interfaces.Services.SiteServiceService(entry).GetPurchaseOrderMiscCharge(entry, siteServiceProfile, purchaseOrderMiscChargeID, includeReportFormatting, closeConnection);
        }

        /// <summary>
        /// Saves a purchase order misc. charge. If no ID is on the object a new ID will be created
        /// </summary>
        /// <param name="siteServiceProfile">Which site service to use for this operation</param>
        /// <param name="entry">The entry into the database</param>
        /// <param name="closeConnection">If true then the connection to the site service is closed as soon as the operation has finished otherwise it will stay open</param>
        /// <param name="purchaseOrderMiscCharge">The misc. charge to be saved</param>
        public virtual void SavePurchaseOrderMiscCharge(
            IConnectionManager entry,
            SiteServiceProfile siteServiceProfile,
            PurchaseOrderMiscCharges purchaseOrderMiscCharge,
            bool closeConnection)
        {
            Interfaces.Services.SiteServiceService(entry).SavePurchaseOrderMiscCharge(entry, siteServiceProfile, purchaseOrderMiscCharge, closeConnection);
        }

        /// <summary>
        /// Retrieves the sum of all ordered items per purchase order that is attached to a goods receiving document. 
        /// </summary>
        /// <param name="siteServiceProfile">Which site service to use for this operation</param>
        /// <param name="entry">The entry into the database</param>
        /// <param name="closeConnection">If true then the connection to the site service is closed as soon as the operation has finished otherwise it will stay open</param>
        /// <param name="numberOfDocuments">For how many GR documents should the total be retrieved</param>
        /// <returns></returns>
        public virtual List<InventoryTotals> GetOrderedTotals(IConnectionManager entry,
            SiteServiceProfile siteServiceProfile,
            int numberOfDocuments,
            bool closeConnection)
        {
            return Interfaces.Services.SiteServiceService(entry).GetOrderedTotals(entry, siteServiceProfile, numberOfDocuments, closeConnection);
        }

        /// <summary>
        /// Return the total number of purchase orders that are in the database
        /// </summary>
        /// <param name="siteServiceProfile">Which site service to use for this operation</param>
        /// <param name="entry">The entry into the database</param>
        /// <param name="closeConnection">If true then the connection to the site service is closed as soon as the operation has finished otherwise it will stay open</param>
        /// <returns>the total number of purchase orders that are in the database</returns>
        public virtual int GetTotalNumberOfProductOrders(IConnectionManager entry,
            SiteServiceProfile siteServiceProfile,
            bool closeConnection)
        {
            return Interfaces.Services.SiteServiceService(entry).GetTotalNumberOfProductOrders(entry, siteServiceProfile, closeConnection);
        }

        /// <summary>
        /// Saves a specific purchase order line
        /// </summary>
        /// <param name="siteServiceProfile">Which site service to use for this operation</param>
        /// <param name="entry">The entry into the database</param>
        /// <param name="closeConnection">If true then the connection to the site service is closed as soon as the operation has finished otherwise it will stay open</param>
        /// <param name="purchaseOrderLine">The purchase order line that is to be saved</param>
        public virtual RecordIdentifier SavePurchaseOrderLine(IConnectionManager entry,
            SiteServiceProfile siteServiceProfile,
            PurchaseOrderLine purchaseOrderLine,
            bool closeConnection)
        {
            return Interfaces.Services.SiteServiceService(entry).SavePurchaseOrderLine(entry, siteServiceProfile, purchaseOrderLine, closeConnection);
        }

        /// <summary>
        /// Gets the relevant unists for the purchase order and item
        /// </summary> 
        /// <param name="siteServiceProfile">Which site service to use for this operation</param>
        /// <param name="entry">The entry into the database</param>
        /// <param name="closeConnection">If true then the connection to the site service is closed as soon as the operation has finished otherwise it will stay open</param>
        /// <param name="purchaseOrderID">The purchase order</param>
        /// <param name="itemID">The item</param>
        /// <returns></returns>
        public virtual List<Unit> GetUnitsForPurchaserOrderItemVariant(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier purchaseOrderID, RecordIdentifier itemID, bool closeConnection)
        {
            return Interfaces.Services.SiteServiceService(entry).GetUnitsForPurchaserOrderItemVariant(entry, siteServiceProfile, purchaseOrderID, itemID, closeConnection);
        }

        /// <summary>
        /// Gets the Line number for the item
        /// </summary>
        /// <param name="siteServiceProfile">Which site service to use for this operation</param>
        /// <param name="entry">The entry into the database</param>
        /// <param name="unitID">The unit ID</param>
        /// <param name="closeConnection">If true then the connection to the site service is closed as soon as the operation has finished otherwise it will stay open</param>
        /// <param name="purchaseOrderID">The purchase order</param>
        /// <param name="retailItemID">The item</param>
        /// <returns></returns>
        public virtual string GetPurchaseOrderLineNumberFromItemInfo(
            IConnectionManager entry, SiteServiceProfile siteServiceProfile,
            RecordIdentifier purchaseOrderID,
            RecordIdentifier retailItemID,
            RecordIdentifier unitID,
            bool closeConnection)
        {
            return Interfaces.Services.SiteServiceService(entry).GetPurchaseOrderLineNumberFromItemInfo(entry, siteServiceProfile, purchaseOrderID, retailItemID, unitID, closeConnection);
        }

        /// <summary>
        /// Gets a purchase order line
        /// </summary>
        /// <param name="siteServiceProfile">Which site service to use for this operation</param>
        /// <param name="entry">The entry into the database</param>
        /// <param name="closeConnection">If true then the connection to the site service is closed as soon as the operation has finished otherwise it will stay open</param>
        /// <param name="purchaseOrderLineID">The purchase order line ID</param>
        /// <param name="storeID">the store the purchase order belongs to </param>
        /// <param name="includeReportFormatting">If formatting information should be included</param>
        /// <returns></returns>
        public virtual PurchaseOrderLine GetPurchaseOrderLine(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier purchaseOrderLineID,
            RecordIdentifier storeID, bool includeReportFormatting, bool closeConnection)
        {
            return Interfaces.Services.SiteServiceService(entry).GetPurchaseOrderLine(entry, siteServiceProfile, purchaseOrderLineID, storeID, includeReportFormatting, closeConnection);
        }

        public virtual List<DataEntity> SearchRetailItemsForVendor(IConnectionManager entry, SiteServiceProfile siteServiceProfile,
            RecordIdentifier vendorID, string searchString,
            RecordIdentifier itemGroupId, int rowFrom, int rowTo, bool beginsWith, bool closeConnection)
        {
            return Interfaces.Services.SiteServiceService(entry).SearchRetailItemsForVendor(entry, siteServiceProfile, vendorID, searchString, itemGroupId, rowFrom, rowTo, beginsWith, closeConnection);
        }

        public virtual decimal GetSumofOrderedItembyStore(
            IConnectionManager entry,
            SiteServiceProfile siteServiceProfile,
            RecordIdentifier itemID,
            RecordIdentifier storeID,
            bool includeReportFormatting,
            RecordIdentifier inventoryUnitId,
            bool closeConnection)
        {
            return Interfaces.Services.SiteServiceService(entry).GetSumofOrderedItembyStore(entry, siteServiceProfile, itemID, storeID, includeReportFormatting, inventoryUnitId, closeConnection);
        }

        public virtual List<DataEntity> SearchItemsInPurchaseOrder(IConnectionManager entry, SiteServiceProfile siteServiceProfile,
            RecordIdentifier purchaseOrderID, string searchString, int rowFrom, int rowTo, bool beginsWith, bool closeConnection)
        {
            return Interfaces.Services.SiteServiceService(entry).SearchItemsInPurchaseOrder(entry, siteServiceProfile, purchaseOrderID, searchString, rowFrom, rowTo, beginsWith, closeConnection);
        }

        public virtual RecordIdentifier GeneratePurchaseOrderLineID(IConnectionManager entry, SiteServiceProfile siteServiceProfile,
            bool closeConnection)
        {
            return Interfaces.Services.SiteServiceService(entry).GeneratePurchaseOrderLineID(entry, siteServiceProfile, closeConnection);
        }

        public virtual PurchaseOrder GetPurchaseOrderWithReportFormatting(IConnectionManager entry, SiteServiceProfile siteServiceProfile,
            RecordIdentifier purchaseOrderID, bool closeConnection)
        {
            return Interfaces.Services.SiteServiceService(entry).GetPurchaseOrderWithReportFormatting(entry, siteServiceProfile, purchaseOrderID, closeConnection);
        }

        public virtual List<PurchaseOrderLine> GetPurchaseOrderLinesWithReportFormatting(IConnectionManager entry, SiteServiceProfile siteServiceProfile,
            RecordIdentifier purchaseOrderID, RecordIdentifier purchaseOrderStoreID, bool closeConnection)
        {
            return Interfaces.Services.SiteServiceService(entry).GetPurchaseOrderLinesWithReportFormatting(entry, siteServiceProfile, purchaseOrderID, purchaseOrderStoreID, closeConnection);

        }

        public virtual List<PurchaseOrder> GetPurchaseOrdersForStoreAndVendor(IConnectionManager entry, SiteServiceProfile siteServiceProfile,
            RecordIdentifier storeID, RecordIdentifier vendorID, PurchaseOrderSorting purchaseOrderSorting, bool sortBackwards, bool closeConnection)
        {
            return Interfaces.Services.SiteServiceService(entry).GetPurchaseOrdersForStoreAndVendor(entry, siteServiceProfile, storeID, vendorID, purchaseOrderSorting, sortBackwards, closeConnection);

        }

        public virtual List<PurchaseOrderMiscCharges> GetMischChargesForPurchaseOrderWithSorting(IConnectionManager entry, SiteServiceProfile siteServiceProfile,
            RecordIdentifier purchaseOrderID, PurchaseOrderMiscChargesSorting sorting, bool sortBackwards,
            bool includeReportFormatting, bool closeConnection)
        {
            return Interfaces.Services.SiteServiceService(entry).GetMischChargesForPurchaseOrderWithSorting(entry, siteServiceProfile, purchaseOrderID, sorting, sortBackwards, includeReportFormatting, closeConnection);
        }

        public virtual CreatePurchaseOrderResult CreatePurchaseOrderFromFilter(IConnectionManager entry, SiteServiceProfile siteServiceProfile, PurchaseOrder purchaseOrderHeader, InventoryTemplateFilterContainer filter, ref RecordIdentifier newOrderID, bool closeConnection)
        {
            try
            {
                return Interfaces.Services.SiteServiceService(entry).CreatePurchaseOrderFromFilter(entry, siteServiceProfile, purchaseOrderHeader, filter, ref newOrderID, closeConnection);
            }
            catch
            {
                return CreatePurchaseOrderResult.ErrorCreatingPurchaseOrder;
            }
            finally
            {
                if (closeConnection)
                {
                    Interfaces.Services.SiteServiceService(entry).Disconnect(entry);
                }
            }
        }

        public virtual CreatePurchaseOrderResult CreatePurchaseOrderFromTemplate(IConnectionManager entry, SiteServiceProfile siteServiceProfile, PurchaseOrder purchaseOrderHeader, TemplateListItem template, ref RecordIdentifier newOrderID, bool closeConnection)
        {
            try
            {
                return Interfaces.Services.SiteServiceService(entry).CreatePurchaseOrderFromTemplate(entry, siteServiceProfile, purchaseOrderHeader, template, ref newOrderID, closeConnection);
            }
            catch
            {
                return CreatePurchaseOrderResult.ErrorCreatingPurchaseOrder;
            }
            finally
            {
                if (closeConnection)
                {
                    Interfaces.Services.SiteServiceService(entry).Disconnect(entry);
                }
            }
        }
    }
}

using System.Collections.Generic;
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
    public partial class SiteServiceService
    {
        public virtual List<PurchaseOrderLine> GetPurchaseOrderLines(IConnectionManager entry, SiteServiceProfile siteServiceProfile, PurchaseOrderLineSearch searchCriteria, PurchaseOrderLineSorting sortBy, bool sortBackwards, out int totalRecordsMatching, bool closeConnection)
        {
            List<PurchaseOrderLine> result = new List<PurchaseOrderLine>();
            int rowCount = 0;
            DoRemoteWork(entry, siteServiceProfile, () => result = server.GetPurchaseOrderLines(CreateLogonInfo(entry), searchCriteria, sortBy, sortBackwards, out rowCount), closeConnection);
            totalRecordsMatching = rowCount;
            return result;
        }

        public virtual bool PurchaseOrderLineHasPostedGoodsReceivingDocumentLine(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier purchaseOrderLineID, bool closeConnection)
        {
            bool result = false;

            DoRemoteWork(entry, siteServiceProfile, () => result = server.PurchaseOrderLineHasPostedGoodsReceivingDocumentLine(CreateLogonInfo(entry), purchaseOrderLineID), closeConnection);

            return result;
        }

        public virtual bool DeletePurchaseOrderLine(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier purchaseOrderLineID, bool closeConnection)
        {
            bool result = false;

            DoRemoteWork(entry, siteServiceProfile, () => result = server.DeletePurchaseOrderLine(CreateLogonInfo(entry), purchaseOrderLineID), closeConnection);

            return result;
        }

        public virtual PurchaseOrderLinesDeleteResult DeletePurchaseOrder(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier purchaseOrderLineID, bool closeConnection)
        {
            PurchaseOrderLinesDeleteResult result = 0;

            DoRemoteWork(entry, siteServiceProfile, () => result = server.DeletePurchaseOrder(CreateLogonInfo(entry), purchaseOrderLineID), closeConnection);

            return result;
        }

        public virtual PurchaseOrder GetPurchaseOrder(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier purchaseOrderID, bool closeConnection)
        {
            PurchaseOrder result = new PurchaseOrder();

            DoRemoteWork(entry, siteServiceProfile, () => result = server.GetPurchaseOrder(CreateLogonInfo(entry), purchaseOrderID), closeConnection);

            return result;
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
            List<PurchaseOrder> result = new List<PurchaseOrder>();

            itemCount = 0;
            int itemCountCopy = itemCount;

            DoRemoteWork(entry, siteServiceProfile, () => result = server.PurchaseOrderAdvancedSearch(
                CreateLogonInfo(entry),
                rowFrom,
                rowTo,
                sortBy,
                sortBackwards,
                out itemCountCopy,
                idOrDescription,
                idOrDescriptionBeginsWith,
                storeID,
                vendorID,
                status,
                deliveryDateFrom,
                deliveryDateTo,
                creationDateFrom,
                creationDateTo,
                onlySearchOpenAndNoGoodsReceivingDocument), closeConnection);

            itemCount = itemCountCopy;

            return result;
        }

        public RecordIdentifier GeneratePurchaseOrderLineID(IConnectionManager entry, SiteServiceProfile siteServiceProfile,
            bool closeConnection)
        {
            RecordIdentifier result = null;
            DoRemoteWork(entry, siteServiceProfile, () => result = server.GeneratePurchaseOrderLineID(CreateLogonInfo(entry)), closeConnection);
            return result;
        }

        public PurchaseOrder GetPurchaseOrderWithReportFormatting(IConnectionManager entry, SiteServiceProfile siteServiceProfile,
            RecordIdentifier purchaseOrderID, bool closeConnection)
        {
            PurchaseOrder result = null;
            DoRemoteWork(entry, siteServiceProfile, () => result = server.GetPurchaseOrderWithReportFormatting(CreateLogonInfo(entry), purchaseOrderID), closeConnection);
            return result;
        }

        public List<PurchaseOrderLine> GetPurchaseOrderLinesWithReportFormatting(IConnectionManager entry, SiteServiceProfile siteServiceProfile,
            RecordIdentifier purchaseOrderID, RecordIdentifier purchaseOrderStoreID, bool closeConnection)
        {
            List<PurchaseOrderLine> result = null;
            DoRemoteWork(entry, siteServiceProfile, () => result = server.GetPurchaseOrderLinesWithReportFormatting(CreateLogonInfo(entry), purchaseOrderID, purchaseOrderStoreID), closeConnection);
            return result;
        }

        public virtual List<PurchaseOrder> GetPurchaseOrdersForStoreAndVendor(IConnectionManager entry, SiteServiceProfile siteServiceProfile,
            RecordIdentifier storeID, RecordIdentifier vendorID, PurchaseOrderSorting purchaseOrderSorting, bool sortBackwards,
            bool closeConnection)
        {
            List<PurchaseOrder> result = null;
            DoRemoteWork(entry, siteServiceProfile, () => result = server.GetPurchaseOrdersForStoreAndVendor(CreateLogonInfo(entry), storeID, vendorID, purchaseOrderSorting, sortBackwards), closeConnection);
            return result;
        }

        public virtual List<PurchaseOrderMiscCharges> GetMischChargesForPurchaseOrderWithSorting(IConnectionManager entry, SiteServiceProfile siteServiceProfile,
            RecordIdentifier purchaseOrderID, PurchaseOrderMiscChargesSorting sorting, bool sortBackwards,
            bool includeReportFormatting, bool closeConnection)
        {
            List<PurchaseOrderMiscCharges> result = null;
            DoRemoteWork(entry, siteServiceProfile, () => result = server.GetMischChargesForPurchaseOrderWithSorting(CreateLogonInfo(entry), purchaseOrderID, sorting, sortBackwards, includeReportFormatting), closeConnection);
            return result;
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
            RecordIdentifier result = null;
            DoRemoteWork(entry, siteServiceProfile, () => result = server.GeneratePurchaseOrderID(CreateLogonInfo(entry)), closeConnection);
            return result;

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
            DoRemoteWork(entry, siteServiceProfile, () => server.CopyLinesAndBetweenMiscChargesPurchaseOrders(CreateLogonInfo(entry), fromPurchaseOrderID, newPurchaseOrder, storeID, taxCalculationMethod), closeConnection);
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
            PurchaseOrder result = null;

            DoRemoteWork(entry, siteServiceProfile, () => result = server.SaveAndReturnPurchaseOrder(CreateLogonInfo(entry), purchaseOrder), closeConnection);

            return result;
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
            DoRemoteWork(entry, siteServiceProfile, () => server.SavePurchaseOrder(CreateLogonInfo(entry), purchaseOrder), closeConnection);
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
            bool result = true;

            DoRemoteWork(entry, siteServiceProfile, () => result = server.PurchaseOrderHasGoodsReceivingDocument(CreateLogonInfo(entry), purchaseOrderID), closeConnection);

            return result;
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
            GoodsReceivingPostResult result = GoodsReceivingPostResult.Success;

            DoRemoteWork(entry, siteServiceProfile, () => result = server.PostAndReceiveAPurchaseOrder(CreateLogonInfo(entry), purchaseOrder), closeConnection);

            return result;
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
            bool result = true;

            DoRemoteWork(entry, siteServiceProfile, () => result = server.PurchaseOrderHasPurchaseOrderLines(CreateLogonInfo(entry), purchaseOrderID), closeConnection);

            return result;
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
            DoRemoteWork(entry, siteServiceProfile, () => server.ChangeDiscountsForPurchaseOrderLines(CreateLogonInfo(entry), purchaseOrderID, storeID, discountPercentage, discountAmount), closeConnection);
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
            DoRemoteWork(entry, siteServiceProfile, () => server.DeletePurchaseOrderMiscCharges(CreateLogonInfo(entry), purchaseOrderMiscChargeID), closeConnection);
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
            List<PurchaseOrderMiscCharges> result = new List<PurchaseOrderMiscCharges>();

            DoRemoteWork(entry, siteServiceProfile, () => result = server.GetMischChargesForPurchaseOrder(CreateLogonInfo(entry), purchaseOrderID, includeReportFormatting), closeConnection);

            return result;
        }

        public virtual InventoryTemplate GetInventoryTemplateForPOWorksheet(IConnectionManager entry,
            SiteServiceProfile siteServiceProfile, RecordIdentifier purchaseWorksheetId, bool closeConnection)
        {
            InventoryTemplate result = null;

            DoRemoteWork(entry, siteServiceProfile, () => result = server.GetInventoryTemplateForPOWorksheet(CreateLogonInfo(entry), purchaseWorksheetId), closeConnection);

            return result;
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
            List<PurchaseWorksheetLine> result = null;

            DoRemoteWork(entry, siteServiceProfile, () => result = server.GetPurchaseWorksheetLineData(CreateLogonInfo(entry), worksheetId, includeDeletedItems, sortEnum, sortBackwards), closeConnection);

            return result;
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
            List<PurchaseWorksheetLine> result = null;

            DoRemoteWork(entry, siteServiceProfile, () => result = server.GetPurchaseWorksheetLineDataPaged(CreateLogonInfo(entry), worksheetId, includeDeletedItems, sortEnum, sortBackwards, rowFrom, rowTo), closeConnection);

            return result;
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
            PurchaseOrderMiscCharges result = new PurchaseOrderMiscCharges();

            DoRemoteWork(entry, siteServiceProfile, () => result = server.GetPurchaseOrderMiscCharge(CreateLogonInfo(entry), purchaseOrderMiscChargeID, includeReportFormatting), closeConnection);

            return result;
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
            DoRemoteWork(entry, siteServiceProfile, () => server.SavePurchaseOrderMiscCharge(CreateLogonInfo(entry), purchaseOrderMiscCharge), closeConnection);
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
            List<InventoryTotals> result = new List<InventoryTotals>();

            DoRemoteWork(entry, siteServiceProfile, () => result = server.GetOrderedTotals(CreateLogonInfo(entry), numberOfDocuments), closeConnection);

            return result;
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
            int result = 0;

            DoRemoteWork(entry, siteServiceProfile, () => result = server.GetTotalNumberOfProductOrders(CreateLogonInfo(entry)), closeConnection);

            return result;
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
            RecordIdentifier result = null;

            DoRemoteWork(entry, siteServiceProfile, () => result = server.SavePurchaseOrderLine(CreateLogonInfo(entry), purchaseOrderLine), closeConnection);

            return result;
        }

        /// <summary>
        /// Gets the relevant units for the 
        /// </summary>
        /// <param name="siteServiceProfile">Which site service to use for this operation</param>
        /// <param name="entry">The entry into the database</param>
        /// <param name="closeConnection">If true then the connection to the site service is closed as soon as the operation has finished otherwise it will stay open</param>
        /// <param name="purchaseOrderID">The purchase order ID</param>
        /// <param name="itemID">The item</param>
        /// <returns></returns>
        public virtual List<Unit> GetUnitsForPurchaserOrderItemVariant(IConnectionManager entry, SiteServiceProfile siteServiceProfile,
            RecordIdentifier purchaseOrderID, RecordIdentifier itemID, bool closeConnection)
        {
            List<Unit> result = null;

            DoRemoteWork(entry, siteServiceProfile, () => result = server.GetUnitsForPurchaserOrderItemVariant(CreateLogonInfo(entry), purchaseOrderID, itemID), closeConnection);

            return result;
        }
        /// <summary>
        /// Gets the Line number for the item
        /// </summary>
        /// <param name="siteServiceProfile">Which site service to use for this operation</param>
        /// <param name="entry">The entry into the database</param>
        /// <param name="closeConnection">If true then the connection to the site service is closed as soon as the operation has finished otherwise it will stay open</param>
        /// <param name="purchaseOrderID">The purchase order</param>
        /// <param name="retailItemID">The retail item</param>
        /// <param name="unitID">The unit </param>
        /// <returns></returns>
        public virtual string GetPurchaseOrderLineNumberFromItemInfo(IConnectionManager entry, SiteServiceProfile siteServiceProfile,
            RecordIdentifier purchaseOrderID, RecordIdentifier retailItemID, RecordIdentifier unitID, bool closeConnection)
        {
            string result = null;

            DoRemoteWork(entry, siteServiceProfile, () => result = server.GetPurchaseOrderLineNumberFromItemInfo(CreateLogonInfo(entry), purchaseOrderID, retailItemID, unitID), closeConnection);

            return result;
        }

        public virtual List<DataEntity> SearchItemsInPurchaseOrder(IConnectionManager entry, SiteServiceProfile siteServiceProfile,
            RecordIdentifier purchaseOrderID, string searchString, int rowFrom, int rowTo, bool beginsWith, bool closeConnection)
        {
            List<DataEntity> result = null;

            DoRemoteWork(entry, siteServiceProfile, () => result = server.SearchItemsInPurchaseOrder(CreateLogonInfo(entry), purchaseOrderID, searchString, rowFrom, rowTo, beginsWith), closeConnection);

            return result;
        }

        public virtual CreatePurchaseOrderResult CreatePurchaseOrderFromFilter(IConnectionManager entry, SiteServiceProfile siteServiceProfile, PurchaseOrder purchaseOrderHeader, InventoryTemplateFilterContainer filter, ref RecordIdentifier newOrderID, bool closeConnection)
        {
            try
            {
                RecordIdentifier returnOrderID = RecordIdentifier.Empty;
                CreatePurchaseOrderResult result = CreatePurchaseOrderResult.Success;

                DoRemoteWork(entry, siteServiceProfile, () => result = server.CreatePurchaseOrderFromFilter(CreateLogonInfo(entry), purchaseOrderHeader, filter, out returnOrderID), closeConnection);

                newOrderID = returnOrderID;
                return result;
            }
            finally
            {
                Disconnect(entry);
            }
        }

        public virtual CreatePurchaseOrderResult CreatePurchaseOrderFromTemplate(IConnectionManager entry, SiteServiceProfile siteServiceProfile, PurchaseOrder purchaseOrderHeader, TemplateListItem template, ref RecordIdentifier newOrderID, bool closeConnection)
        {
            try
            {
                RecordIdentifier returnOrderID = RecordIdentifier.Empty;
                CreatePurchaseOrderResult result = CreatePurchaseOrderResult.Success;

                DoRemoteWork(entry, siteServiceProfile, () => result = server.CreatePurchaseOrderFromTemplate(CreateLogonInfo(entry), purchaseOrderHeader, template, out returnOrderID), closeConnection);

                newOrderID = returnOrderID;
                return result;
            }
            finally
            {
                Disconnect(entry);
            }
        }
    }
}

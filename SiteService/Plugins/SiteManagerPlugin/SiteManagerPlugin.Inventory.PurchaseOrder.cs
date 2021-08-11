using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.Inventory;
using LSOne.DataLayer.BusinessObjects.Replenishment;
using LSOne.DataLayer.BusinessObjects.Replenishment.Containers;
using LSOne.DataLayer.BusinessObjects.Replenishment.ListItems;
using LSOne.DataLayer.BusinessObjects.Units;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.Inventory;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Services.Interfaces;
using LSOne.Services.Interfaces.SupportClasses;
using LSOne.SiteService.Utilities;
using LSOne.Utilities.DataTypes;


namespace LSOne.SiteService.Plugins.SiteManager
{
    public partial class SiteManagerPlugin
    {
        /// <summary>
        /// Returns a paginated list of purchase orders based on the search criteria
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <param name="rowFrom"></param>
        /// <param name="rowTo"></param>
        /// <param name="sortBy"></param>
        /// <param name="sortBackwards"></param>
        /// <param name="itemCount"></param>
        /// <param name="idOrDescription"></param>
        /// <param name="idOrDescriptionBeginsWith"></param>
        /// <param name="storeID"></param>
        /// <param name="vendorID"></param>
        /// <param name="status"></param>
        /// <param name="deliveryDateFrom"></param>
        /// <param name="deliveryDateTo"></param>
        /// <param name="creationDateFrom"></param>
        /// <param name="creationDateTo"></param>
        /// <param name="onlySearchOpenAndNoGoodsReceivingDocument"></param>
        /// <returns></returns>
        public virtual List<PurchaseOrder> PurchaseOrderAdvancedSearch(
            LogonInfo logonInfo,
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
            IConnectionManager entry = GetConnectionManager(logonInfo);

            try
            {
                Utils.Log(this, $"{Utils.Starting} {nameof(rowFrom)}: {rowFrom}, {nameof(rowTo)}: {rowTo}, {nameof(sortBy)}: {sortBy}, {nameof(sortBackwards)}: {sortBackwards}");

                return Providers.PurchaseOrderData.AdvancedSearch(
                    entry,
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
        /// Gets the purchase order ID for the giving goods receiving document ID
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <param name="goodsReceivingDocumentID"></param>
        /// <returns></returns>
        public virtual RecordIdentifier GetPurchaseOrderID(LogonInfo logonInfo, RecordIdentifier goodsReceivingDocumentID)
        {
            IConnectionManager entry = GetConnectionManager(logonInfo);

            try
            {
                Utils.Log(this, $"{Utils.Starting} {nameof(goodsReceivingDocumentID)}: {goodsReceivingDocumentID}");

                return Providers.GoodsReceivingDocumentData.GetPurchaseOrderID(entry, goodsReceivingDocumentID);
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
        /// Gets the purchase order line for the giving purchase order line ID
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <param name="purchaseOrderLineID"></param>
        /// <param name="storeID"></param>
        /// <param name="includeReportFormatting"></param>
        /// <returns></returns>
        public virtual PurchaseOrderLine GetPurchaseOrderLine(LogonInfo logonInfo, RecordIdentifier purchaseOrderLineID, RecordIdentifier storeID, bool includeReportFormatting)
        {
            IConnectionManager entry = GetConnectionManager(logonInfo);

            try
            {
                Utils.Log(this, $"{Utils.Starting} {nameof(purchaseOrderLineID)}: {purchaseOrderLineID}, {nameof(storeID)}: {storeID}, {nameof(includeReportFormatting)}, {includeReportFormatting}");

                return Providers.PurchaseOrderLineData.Get(entry, purchaseOrderLineID, storeID, includeReportFormatting);
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
        /// Gets the purchase order lines based on the search criteria
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <param name="searchCriteria"></param>
        /// <param name="sortBy"></param>
        /// <param name="sortBackwards"></param>
        /// <param name="totalRecordsMatching"></param>
        /// <returns></returns>
        public virtual List<PurchaseOrderLine> GetPurchaseOrderLines(LogonInfo logonInfo,
            PurchaseOrderLineSearch searchCriteria, PurchaseOrderLineSorting sortBy, bool sortBackwards, out int totalRecordsMatching)
        {
            IConnectionManager entry = GetConnectionManager(logonInfo);

            try
            {
                Utils.Log(this, $"{Utils.Starting} {nameof(sortBy)}: {sortBy}, {nameof(sortBackwards)}: {sortBackwards}");

                return Providers.PurchaseOrderLineData.GetPurchaseOrderLines(entry, searchCriteria, sortBy, sortBackwards, out totalRecordsMatching);
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
        /// Returns true if the purchase order line with the given ID has posted goods receiving document line; otherwise returns false
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <param name="purchaseOrderLineID"></param>
        /// <returns></returns>
        public virtual bool PurchaseOrderLineHasPostedGoodsReceivingDocumentLine(LogonInfo logonInfo, RecordIdentifier purchaseOrderLineID)
        {
            IConnectionManager entry = GetConnectionManager(logonInfo);

            try
            {
                Utils.Log(this, $"{Utils.Starting} {nameof(purchaseOrderLineID)}: {purchaseOrderLineID}");

                return Providers.PurchaseOrderLineData.HasPostedGoodsReceivingDocumentLines(entry, purchaseOrderLineID);
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
        /// Deletes the purchase order line with the given ID
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <param name="purchaseOrderLineID"></param>
        /// <returns></returns>
        public virtual bool DeletePurchaseOrderLine(LogonInfo logonInfo, RecordIdentifier purchaseOrderLineID)
        {
            IConnectionManager entry = GetConnectionManager(logonInfo);

            try
            {
                Utils.Log(this, $"{Utils.Starting} {nameof(purchaseOrderLineID)}: {purchaseOrderLineID}");

                Providers.PurchaseOrderLineData.Delete(entry, purchaseOrderLineID);

                return true;
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
        /// Deletes the purchase order with its associated goods receiving document, based on the purchase order line ID;
        /// the deletion is performed only if the goods receiving document is not posted
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <param name="purchaseOrderLineID"></param>
        /// <returns></returns>
        public virtual PurchaseOrderLinesDeleteResult DeletePurchaseOrder(LogonInfo logonInfo, RecordIdentifier purchaseOrderLineID)
        {
            IConnectionManager entry = GetConnectionManager(logonInfo);

            try
            {
                Utils.Log(this, $"{Utils.Starting} {nameof(purchaseOrderLineID)}: {purchaseOrderLineID}");

                if (Providers.PurchaseOrderData.HasPostedGoodsReceivingDocument(entry, purchaseOrderLineID))
                {
                    return PurchaseOrderLinesDeleteResult.GoodsReceivingLinesExist;
                }

                Providers.PurchaseOrderData.Delete(entry, purchaseOrderLineID);
                Providers.GoodsReceivingDocumentData.DeleteGoodsReceivingDocumentsForPurchaseOrder(entry, purchaseOrderLineID);

                return PurchaseOrderLinesDeleteResult.CanBeDeleted;
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
        /// Gets the purchase order with the given ID
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <param name="purchaseOrderID"></param>
        /// <returns></returns>
        public virtual PurchaseOrder GetPurchaseOrder(LogonInfo logonInfo, RecordIdentifier purchaseOrderID)
        {
            IConnectionManager entry = GetConnectionManager(logonInfo);

            try
            {
                Utils.Log(this, $"{Utils.Starting} {nameof(purchaseOrderID)}: {purchaseOrderID}");

                PurchaseOrder order = Providers.PurchaseOrderData.Get(entry, purchaseOrderID, false);

                if (order != null)
                {
                    order.HasLines = Providers.PurchaseOrderData.HasPurchaseOrderLines(entry, purchaseOrderID);
                }

                return order;
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
        /// Copies all item lines and miscellaneous charges from oldPurchaseOrderID to newPurchaseOrderID
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <param name="fromPurchaseOrderID"></param>
        /// <param name="newPurchaseOrder"></param>
        /// <param name="storeID"></param>
        public virtual void CopyLinesAndBetweenMiscChargesPurchaseOrders(LogonInfo logonInfo, RecordIdentifier fromPurchaseOrderID, PurchaseOrder newPurchaseOrder, RecordIdentifier storeID, TaxCalculationMethodEnum taxCalculationMethod)
        {
            IConnectionManager entry = GetConnectionManager(logonInfo);

            try
            {
                Utils.Log(this, $"{Utils.Starting} {nameof(fromPurchaseOrderID)}: {fromPurchaseOrderID}, {nameof(storeID)}: {storeID}, {nameof(taxCalculationMethod)}: {taxCalculationMethod}");

                Providers.PurchaseOrderLineData.CopyLinesBetweenPurchaseOrders(entry, fromPurchaseOrderID, newPurchaseOrder, storeID, taxCalculationMethod);
                Providers.PurchaseOrderMiscChargesData.CopyMiscChargesBetweenPOs(entry, fromPurchaseOrderID, newPurchaseOrder.ID);
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
        /// Gets the purchase order line with report formatting for the given purchase order ID
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <param name="purchaseOrderID"></param>
        /// <param name="purchaseOrderStoreID"></param>
        /// <returns></returns>
        public virtual List<PurchaseOrderLine> GetPurchaseOrderLinesWithReportFormatting(LogonInfo logonInfo, RecordIdentifier purchaseOrderID, RecordIdentifier purchaseOrderStoreID)
        {
            IConnectionManager entry = GetConnectionManager(logonInfo);

            try
            {
                Utils.Log(this, $"{Utils.Starting} {nameof(purchaseOrderID)}: {purchaseOrderID}, {nameof(purchaseOrderStoreID)}: {purchaseOrderStoreID}");

                return Providers.PurchaseOrderLineData.GetPurchaseOrderLines(entry, purchaseOrderID, purchaseOrderStoreID, true);
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
        /// Gets the purchase orders for the given search criteria
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <param name="storeID"></param>
        /// <param name="vendorID"></param>
        /// <param name="purchaseOrderSorting"></param>
        /// <param name="sortBackwards"></param>
        /// <returns></returns>
        public virtual List<PurchaseOrder> GetPurchaseOrdersForStoreAndVendor(LogonInfo logonInfo, RecordIdentifier storeID, RecordIdentifier vendorID, PurchaseOrderSorting purchaseOrderSorting, bool sortBackwards)
        {
            IConnectionManager entry = GetConnectionManager(logonInfo);

            try
            {
                Utils.Log(this, $"{Utils.Starting} {nameof(storeID)}: {storeID}, {nameof(vendorID)}: {vendorID}, {nameof(purchaseOrderSorting)}: {purchaseOrderSorting}, {nameof(sortBackwards)}: {sortBackwards}");

                return Providers.PurchaseOrderData.GetPurchaseOrdersForStoreAndVendor(entry, storeID, vendorID,
                    purchaseOrderSorting, sortBackwards);
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
        /// Gets the purchase order with report formatting for the given ID
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <param name="purchaseOrderID"></param>
        /// <returns></returns>
        public virtual PurchaseOrder GetPurchaseOrderWithReportFormatting(LogonInfo logonInfo, RecordIdentifier purchaseOrderID)
        {
            IConnectionManager entry = GetConnectionManager(logonInfo);

            try
            {
                Utils.Log(this, $"{Utils.Starting} {nameof(purchaseOrderID)}: {purchaseOrderID}");

                return Providers.PurchaseOrderData.Get(entry, purchaseOrderID, true);
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
        /// Gets the miscellaneous charges for the purchase order with the given ID
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <param name="purchaseOrderID"></param>
        /// <param name="sorting"></param>
        /// <param name="sortBackwards"></param>
        /// <param name="includeReportFormatting"></param>
        /// <returns></returns>
        public virtual List<PurchaseOrderMiscCharges> GetMischChargesForPurchaseOrderWithSorting(LogonInfo logonInfo, RecordIdentifier purchaseOrderID, PurchaseOrderMiscChargesSorting sorting, bool sortBackwards, bool includeReportFormatting)
        {
            IConnectionManager entry = GetConnectionManager(logonInfo);

            try
            {
                Utils.Log(this, $"{Utils.Starting} {nameof(purchaseOrderID)}: {purchaseOrderID}, {nameof(sorting)}: {sorting}, {nameof(sortBackwards)}: {sortBackwards}, {nameof(includeReportFormatting)}: {includeReportFormatting}");

                return Providers.PurchaseOrderMiscChargesData.GetMischChargesForPurchaseOrder(
                        entry,
                        purchaseOrderID,
                        sorting,
                        sortBackwards,
                        includeReportFormatting);
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
        /// Generates a new purchase order ID
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <returns>New purchase order ID</returns>
        public virtual RecordIdentifier GeneratePurchaseOrderID(LogonInfo logonInfo)
        {
            IConnectionManager entry = GetConnectionManager(logonInfo);

            try
            {
                Utils.Log(this, Utils.Starting);

                return (string)DataProviderFactory.Instance.GenerateNumber<IPurchaseOrderData, PurchaseOrder>(entry);
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
        /// Generates a new purchase order line ID
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <returns>New purchase order ID</returns>
        public virtual RecordIdentifier GeneratePurchaseOrderLineID(LogonInfo logonInfo)
        {
            IConnectionManager entry = GetConnectionManager(logonInfo);

            try
            {
                Utils.Log(this, Utils.Starting);

                return (string)DataProviderFactory.Instance.GenerateNumber<IPurchaseOrderLineData, PurchaseOrderLine>(entry);
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
        /// Saves a given purchase order into the database and returns it
        /// </summary>
        /// <param name="logonInfo">The entry into the database</param>
        /// <param name="purchaseOrder">The Purchase order to save</param>
        /// <returns>Purchase order</returns>
        public virtual PurchaseOrder SaveAndReturnPurchaseOrder(LogonInfo logonInfo, PurchaseOrder purchaseOrder)
        {
            IConnectionManager entry = GetConnectionManager(logonInfo);

            try
            {
                Utils.Log(this, Utils.Starting);

                return Providers.PurchaseOrderData.SaveAndReturnPurchaseOrder(entry, purchaseOrder);
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
        /// Saves a given purchase order into the database
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <param name="purchaseOrder"></param>
        public virtual void SavePurchaseOrder(LogonInfo logonInfo, PurchaseOrder purchaseOrder)
        {
            IConnectionManager entry = GetConnectionManager(logonInfo);

            try
            {
                Utils.Log(this, Utils.Starting);

                Providers.PurchaseOrderData.Save(entry, purchaseOrder);
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
        /// Returns true if the purchase order with the given ID has a goods receiving document; otherwise returns false
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <param name="purchaseOrderID"></param>
        /// <returns></returns>
        public virtual bool PurchaseOrderHasGoodsReceivingDocument(LogonInfo logonInfo, RecordIdentifier purchaseOrderID)
        {
            IConnectionManager entry = GetConnectionManager(logonInfo);

            try
            {
                Utils.Log(this, Utils.Starting);

                return Providers.PurchaseOrderData.HasGoodsReceivingDocument(entry, purchaseOrderID);
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
        /// Creates and posts a goods receiving document for all lines within the purchase order
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <param name="purchaseOrder">The purchase order that is to be posted and received</param>
        /// <returns>Returns true if the goods receiving lines were successfully created</returns>
        public virtual GoodsReceivingPostResult PostAndReceiveAPurchaseOrder(LogonInfo logonInfo, PurchaseOrder purchaseOrder)
        {
            IConnectionManager entry = GetConnectionManager(logonInfo);

            try
            {
                Utils.Log(this, Utils.Starting);

                PurchaseOrder po = Providers.PurchaseOrderData.Get(entry, purchaseOrder.ID, false);

                if(po.ProcessingStatus != InventoryProcessingStatus.None)
                {
                    Utils.Log(this, "Purchase order is currently processing and cannot be posted.");
                    return GoodsReceivingPostResult.AlreadyProcessing;
                }

                GoodsReceivingDocument document = new GoodsReceivingDocument();
                document.PurchaseOrderID = purchaseOrder.ID;
                document.GoodsReceivingID = purchaseOrder.ID;
                document.Status = GoodsReceivingStatusEnum.Active;

                SaveGoodsReceivingDocument(logonInfo, document);

                GoodsReceivingPostResult result = CreatePostedGoodsReceivingDocumentLinesFromPurchaseOrder(logonInfo, purchaseOrder.ID);
                return result;
            }
            catch(Exception ex)
            {
                Utils.LogException(this, ex);
                return GoodsReceivingPostResult.Error;
            }
            finally
            {
                Utils.Log(this, Utils.Done);
                ReturnConnection(entry, out entry);
            }
        }

        /// <summary>
        /// Returns true if the purchase order has any items lines
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <param name="purchaseOrderID">The ID of the purchase order that is being checked</param>
        /// <returns>Return true if any item lines are on the purchase order</returns>
        public virtual bool PurchaseOrderHasPurchaseOrderLines(LogonInfo logonInfo, RecordIdentifier purchaseOrderID)
        {
            IConnectionManager entry = GetConnectionManager(logonInfo);

            try
            {
                Utils.Log(this, $"{Utils.Starting} {nameof(purchaseOrderID)}: {purchaseOrderID}");

                return Providers.PurchaseOrderData.HasPurchaseOrderLines(entry, purchaseOrderID);
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
        /// Goes through all the purchase order lines and updates either the discount amount and discount percentage on each line
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <param name="purchaseOrderID">The ID of the purchase order that is being checked</param>
        /// <param name="storeID">The ID of the store the purchase order belongs to</param>
        /// <param name="discountPercentage">The discount % that should be used for updating. If null then this value is ignored</param>
        /// <param name="discountAmount">The discount amount that should be used for updating. If null then this value is ignored</param>
        public virtual void ChangeDiscountsForPurchaseOrderLines(
            LogonInfo logonInfo,
            RecordIdentifier purchaseOrderID,
            RecordIdentifier storeID,
            decimal? discountPercentage,
            decimal? discountAmount)
        {
            IConnectionManager entry = GetConnectionManager(logonInfo);

            try
            {
                Utils.Log(this, $"{Utils.Starting} {nameof(purchaseOrderID)}: {purchaseOrderID}, {nameof(storeID)}: {storeID}, {nameof(discountPercentage)}: {discountPercentage}, {nameof(discountAmount)}: {discountAmount}");

                Providers.PurchaseOrderLineData.ChangeDiscountsForPurchaseOrderLines(entry, purchaseOrderID, storeID,
                    discountPercentage, discountAmount);
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
        /// Deletes a specific miscellaneous charge on a purchase order
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <param name="purchaseOrderMiscChargeID">Unique ID of the miscellaneous charge line to be deleted</param>
        public virtual void DeletePurchaseOrderMiscCharges(LogonInfo logonInfo, RecordIdentifier purchaseOrderMiscChargeID)
        {
            IConnectionManager entry = GetConnectionManager(logonInfo);

            try
            {
                Utils.Log(this, $"{Utils.Starting} {nameof(purchaseOrderMiscChargeID)}: {purchaseOrderMiscChargeID}");

                Providers.PurchaseOrderMiscChargesData.Delete(entry, purchaseOrderMiscChargeID);
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
        /// Gets purchase order miscellaneous charges for the given purchase order ID
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <param name="purchaseOrderID">The ID of the purchase order to get misc charges for</param>
        /// <param name="includeReportFormatting">Set to true if you want price and quantity formatting, usually for reports</param>
        /// <returns>A purchase order misc charge with a given ID</returns>
        public virtual List<PurchaseOrderMiscCharges> GetMischChargesForPurchaseOrder(LogonInfo logonInfo,
            RecordIdentifier purchaseOrderID,
            bool includeReportFormatting)
        {
            IConnectionManager entry = GetConnectionManager(logonInfo);

            try
            {
                Utils.Log(this, $"{Utils.Starting} {nameof(purchaseOrderID)}: {purchaseOrderID}, {nameof(includeReportFormatting)}: {includeReportFormatting}");

                return Providers.PurchaseOrderMiscChargesData.GetMischChargesForPurchaseOrder(
                    entry,
                    purchaseOrderID,
                    PurchaseOrderMiscChargesSorting.Type,
                    false,
                    includeReportFormatting);

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
        /// Gets the inventory template for the given purchase order worksheet ID
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <param name="purchaseWorksheetId"></param>
        /// <returns></returns>
        public virtual InventoryTemplate GetInventoryTemplateForPOWorksheet(LogonInfo logonInfo, RecordIdentifier purchaseWorksheetId)
        {
            IConnectionManager entry = GetConnectionManager(logonInfo);

            try
            {
                Utils.Log(this, $"{Utils.Starting} {nameof(purchaseWorksheetId)}: {purchaseWorksheetId}");

                PurchaseWorksheet purchaseWorksheet = Providers.PurchaseWorksheetData.Get(entry, purchaseWorksheetId);
                return Providers.InventoryTemplateData.Get(entry, purchaseWorksheet.InventoryTemplateID);
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
        /// Gets the lines for the given purchase order worksheet ID
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <param name="worksheetId"></param>
        /// <param name="includeDeletedItems"></param>
        /// <param name="sortEnum"></param>
        /// <param name="sortBackwards"></param>
        /// <returns></returns>
        public virtual List<PurchaseWorksheetLine> GetPurchaseWorksheetLineData(
            LogonInfo logonInfo,
            RecordIdentifier worksheetId,
            bool includeDeletedItems,
            PurchaseWorksheetLineSortEnum sortEnum,
            bool sortBackwards)
        {
            IConnectionManager entry = GetConnectionManager(logonInfo);

            try
            {
                Utils.Log(this, $"{Utils.Starting} {nameof(worksheetId)}: {worksheetId}, {nameof(includeDeletedItems)}: {includeDeletedItems}, {nameof(sortEnum)}: {sortEnum}, {nameof(sortBackwards)}: {sortBackwards}");

                return Providers.PurchaseWorksheetLineData.GetList(entry, worksheetId, includeDeletedItems, sortEnum, sortBackwards);
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
        /// Gets a paginated list of lines for the given purchase order worksheet ID
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <param name="worksheetId"></param>
        /// <param name="includeDeletedItems"></param>
        /// <param name="sortEnum"></param>
        /// <param name="sortBackwards"></param>
        /// <param name="rowFrom"></param>
        /// <param name="rowTo"></param>
        /// <returns></returns>
        public virtual List<PurchaseWorksheetLine> GetPurchaseWorksheetLineDataPaged(
            LogonInfo logonInfo,
            RecordIdentifier worksheetId,
            bool includeDeletedItems,
            PurchaseWorksheetLineSortEnum sortEnum,
            bool sortBackwards,
            int rowFrom,
            int rowTo)
        {
            IConnectionManager entry = GetConnectionManager(logonInfo);

            try
            {
                Utils.Log(this, $"{Utils.Starting} {nameof(worksheetId)}: {worksheetId}, {nameof(includeDeletedItems)}: {includeDeletedItems}, {nameof(sortEnum)}: {sortEnum}, {nameof(sortBackwards)}: {sortBackwards}, {nameof(rowFrom)}: {rowFrom}, {nameof(rowTo)}: {rowTo}");

                return Providers.PurchaseWorksheetLineData.GetPagedList(entry, worksheetId, includeDeletedItems, sortEnum, sortBackwards, rowFrom, rowTo);
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
        /// Gets information about a specific miscellaneous charge
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <param name="purchaseOrderMiscChargeID">The ID of the misc charge being retrieved</param>
        /// <param name="includeReportFormatting">Set to true if you want price and quantity formatting, usually for reports</param>
        /// <returns>Information about the msc charge</returns>
        public virtual PurchaseOrderMiscCharges GetPurchaseOrderMiscCharge(LogonInfo logonInfo,
            RecordIdentifier purchaseOrderMiscChargeID, bool includeReportFormatting)
        {
            IConnectionManager entry = GetConnectionManager(logonInfo);

            try
            {
                Utils.Log(this, $"{Utils.Starting} {nameof(purchaseOrderMiscChargeID)}: {purchaseOrderMiscChargeID}, {nameof(includeReportFormatting)}: {includeReportFormatting}");

                return Providers.PurchaseOrderMiscChargesData.Get(entry, purchaseOrderMiscChargeID, includeReportFormatting);
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
        /// Saves a purchase order miscellaneous charge. If no line number is on the object a new ID will be created
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <param name="purchaseOrderMiscCharge">The misc. charge to be saved</param>
        public virtual void SavePurchaseOrderMiscCharge(LogonInfo logonInfo, PurchaseOrderMiscCharges purchaseOrderMiscCharge)
        {
            IConnectionManager entry = GetConnectionManager(logonInfo);

            try
            {
                Utils.Log(this, Utils.Starting);

                Providers.PurchaseOrderMiscChargesData.Save(entry, purchaseOrderMiscCharge);
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
        /// Retrieves the sum of all ordered items per purchase order that is attached to a goods receiving document.
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <param name="numberOfDocuments">For how many GR documents should the total be retrieved</param>
        /// <returns></returns>
        public virtual List<InventoryTotals> GetOrderedTotals(LogonInfo logonInfo, int numberOfDocuments)
        {
            IConnectionManager entry = GetConnectionManager(logonInfo);

            try
            {
                Utils.Log(this, $"{Utils.Starting} {nameof(numberOfDocuments)}: {numberOfDocuments}");

                return Providers.PurchaseOrderLineData.GetOrderedTotals(entry, numberOfDocuments);
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
        /// Return the total number of purchase orders that are in the database
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <returns>the total number of purchase orders that are in the database</returns>
        public virtual int GetTotalNumberOfProductOrders(LogonInfo logonInfo)
        {
            IConnectionManager entry = GetConnectionManager(logonInfo);

            try
            {
                Utils.Log(this, Utils.Starting);

                return Providers.PurchaseOrderData.CountOrders(entry);
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
        /// Saves a specific purchase order line
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <param name="purchaseOrderLine">The purchase order line that is to be saved</param>
        public virtual RecordIdentifier SavePurchaseOrderLine(LogonInfo logonInfo, PurchaseOrderLine purchaseOrderLine)
        {
            IConnectionManager entry = GetConnectionManager(logonInfo);

            try
            {
                Utils.Log(this, Utils.Starting);

                Providers.PurchaseOrderLineData.Save(entry, purchaseOrderLine);
                return purchaseOrderLine.LineNumber;
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
        /// Gets the relevant units for the purchase order and item
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <param name="purchaseOrderID">The purchase order</param>
        /// <param name="itemID">The item</param>
        /// <returns></returns>
        public virtual List<Unit> GetUnitsForPurchaserOrderItemVariant(LogonInfo logonInfo, RecordIdentifier purchaseOrderID, RecordIdentifier itemID)
        {
            IConnectionManager entry = GetConnectionManager(logonInfo);

            try
            {
                Utils.Log(this, $"{Utils.Starting} {nameof(purchaseOrderID)}: {purchaseOrderID}, {nameof(itemID)}: {itemID}");

                return Providers.PurchaseOrderData.GetUnitsForPurchaserOrderItemVariant(entry, purchaseOrderID, itemID);
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
        /// Gets the line number of a retail item within a given purchase order
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <param name="purchaseOrderID"></param>
        /// <param name="retailItemID"></param>
        /// <param name="unitID"></param>
        /// <returns></returns>
        public virtual string GetPurchaseOrderLineNumberFromItemInfo(LogonInfo logonInfo,
            RecordIdentifier purchaseOrderID,
            RecordIdentifier retailItemID, RecordIdentifier unitID)
        {
            IConnectionManager entry = GetConnectionManager(logonInfo);

            try
            {
                Utils.Log(this, $"{Utils.Starting} {nameof(purchaseOrderID)}: {purchaseOrderID}, {nameof(retailItemID)}: {retailItemID}, {nameof(unitID)}: {unitID}");

                return Providers.PurchaseOrderLineData.GetPurchaseOrderLineNumberFromItemInfo(entry, purchaseOrderID, retailItemID, unitID);
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
        /// Returns a paginated list of items within a given purchase order
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <param name="purchaseOrderID"></param>
        /// <param name="searchString"></param>
        /// <param name="rowFrom"></param>
        /// <param name="rowTo"></param>
        /// <param name="beginsWith"></param>
        /// <returns></returns>
        public virtual List<DataEntity> SearchItemsInPurchaseOrder(LogonInfo logonInfo, RecordIdentifier purchaseOrderID, string searchString, int rowFrom,
            int rowTo, bool beginsWith)
        {
            IConnectionManager entry = GetConnectionManager(logonInfo);

            try
            {
                Utils.Log(this, $"{Utils.Starting} {nameof(purchaseOrderID)}: {purchaseOrderID}, {nameof(searchString)}: {searchString}, {nameof(rowFrom)}: {rowFrom}, {nameof(rowTo)}: {rowTo}, {nameof(beginsWith)}: {beginsWith}");

                return Providers.PurchaseOrderData.SearchItemsInPurchaseOrder(entry, purchaseOrderID, searchString, rowFrom, rowTo, beginsWith);
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
        /// Create a new purchase order from an filter
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <param name="purchaseOrderHeader">Purchase order header information</param>
        /// <param name="filter">Container with desired item filters</param>
        /// <param name="newOrderID">The ID of the new purchase order that was created</param>
        /// <returns>Status and ID of the created purchase order</returns>
        public virtual CreatePurchaseOrderResult CreatePurchaseOrderFromFilter(LogonInfo logonInfo, PurchaseOrder purchaseOrderHeader, InventoryTemplateFilterContainer filter, out RecordIdentifier newOrderID)
        {
            Utils.Log(this, Utils.Starting);

            newOrderID = RecordIdentifier.Empty;
            IConnectionManager dataModel = GetConnectionManager(logonInfo);
            try
            {
                if (purchaseOrderHeader != null && RecordIdentifier.IsEmptyOrNull(purchaseOrderHeader.ID))
                {
                    purchaseOrderHeader = SaveAndReturnPurchaseOrder(logonInfo, purchaseOrderHeader);
                    Utils.Log(this, "New order created with ID: " + (string)purchaseOrderHeader.ID);
                }

                if (purchaseOrderHeader == null)
                {
                    return CreatePurchaseOrderResult.ErrorCreatingPurchaseOrder;
                }

                // Send the ID back to the caller
                newOrderID = purchaseOrderHeader.ID;

                if(filter.HasFilterCriteria())
                {
                    Utils.Log(this, "Saving lines starting");
                    int savedLinesCount = Providers.PurchaseOrderData.CreatePurchaseOrderLinesFromFilter(dataModel, purchaseOrderHeader.ID, filter);
                    Utils.Log(this, $"Saved {savedLinesCount} lines");

                    if(savedLinesCount > 0)
                    {
                        IConnectionManagerTemporary temporaryConnection = dataModel.CreateTemporaryConnection();
                        Task.Run(() => CalculatePurchaseOrderLinesTax(temporaryConnection, purchaseOrderHeader));
                    }
                }

                return CreatePurchaseOrderResult.Success;
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

        private void CalculatePurchaseOrderLinesTax(IConnectionManager dataModel, PurchaseOrder purchaseOrderHeader)
        {
            Utils.Log(this, $"Started calculating purchase order lines tax for PO {purchaseOrderHeader.ID}");

            try
            {
                //Calculate tax for all lines
                List<PurchaseOrderLine> poLines = Providers.PurchaseOrderLineData.GetPurchaseOrderLines(dataModel, purchaseOrderHeader.ID, purchaseOrderHeader.StoreID, false);

                if (dataModel.ServiceFactory == null)
                {
                    Utils.Log(this, "Service factory = null. Initializing new service factory.", LogLevel.Trace);
                    dataModel.ServiceFactory = new LSOne.DataLayer.GenericConnector.ServiceFactory();
                }

                ITaxService taxService = (ITaxService)dataModel.Service(ServiceType.TaxService);

                foreach (PurchaseOrderLine purchaseOrderLine in poLines)
                {
                    try
                    {
                        if (purchaseOrderLine.UnitPrice == 0 || purchaseOrderLine.TaxAmount != 0 || purchaseOrderLine.TaxCalculationMethod == TaxCalculationMethodEnum.NoTax)
                        {
                            continue;
                        }

                        decimal calcTaxFrom = purchaseOrderLine.GetDiscountedPrice();

                        purchaseOrderLine.TaxAmount = taxService.GetTaxAmountForPurchaseOrderLine(
                                dataModel,
                                Providers.RetailItemData.GetItemsItemSalesTaxGroupID(dataModel, purchaseOrderLine.ItemID),
                                purchaseOrderHeader.VendorID,
                                purchaseOrderHeader.StoreID,
                                calcTaxFrom,
                                purchaseOrderHeader.DefaultDiscountAmount,
                                purchaseOrderHeader.DefaultDiscountPercentage,
                                purchaseOrderLine.TaxCalculationMethod
                                );

                        Providers.PurchaseOrderLineData.Save(dataModel, purchaseOrderLine);
                    }
                    catch
                    {
                        //Don't care if this fails
                    }
                }
            }
            catch(Exception e)
            {
                Utils.Log(this, $"Calculating purchase order lines tax for PO {purchaseOrderHeader.ID} failed.\n\r{e.ToString()}", LogLevel.Error);
            }

            Utils.Log(this, $"Finished calculating purchase order lines tax for PO {purchaseOrderHeader.ID}");
        }

        /// <summary>
        /// Create a new purchase order based on a given template.
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <param name="purchaseOrderHeader">Purchase order header information</param>
        /// <param name="template">The purchase order template.</param>
        /// <param name="newOrderID">The ID of the new purchase order that was created</param>
        /// <returns>Status and ID of the created purchase order.</returns>
        public virtual CreatePurchaseOrderResult CreatePurchaseOrderFromTemplate(LogonInfo logonInfo, PurchaseOrder purchaseOrderHeader, TemplateListItem template, out RecordIdentifier newOrderID)
        {
            IConnectionManager entry = GetConnectionManager(logonInfo);

            try
            {
                Utils.Log(this, $"{Utils.Starting} Creating transfer order from template: " + template?.TemplateID);

                if (template == null
                    || RecordIdentifier.IsEmptyOrNull(template.TemplateID)
                    || !Providers.InventoryTemplateData.Exists(entry, template.TemplateID))
                {
                    Utils.Log(this, $"Template {template?.TemplateID?.StringValue} not found.");
                    newOrderID = RecordIdentifier.Empty;
                    return CreatePurchaseOrderResult.TemplateNotFound;
                }

                List<InventoryTemplateSectionSelection> filters = Providers.InventoryTemplateSectionSelectionData.GetList(entry, template.TemplateID);
                InventoryTemplateFilterContainer filter = new InventoryTemplateFilterContainer(filters);

                CreatePurchaseOrderResult result = CreatePurchaseOrderFromFilter(logonInfo, purchaseOrderHeader, filter, out newOrderID);

                if(result == CreatePurchaseOrderResult.Success && !RecordIdentifier.IsEmptyOrNull(newOrderID))
                {
                    InventoryTemplate inventoryTemplate = Providers.InventoryTemplateData.Get(entry, template.TemplateID);
                    CreateGoodsReceivingForPurchaseOrderTemplate(entry, inventoryTemplate, newOrderID);
                }

                return result;
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
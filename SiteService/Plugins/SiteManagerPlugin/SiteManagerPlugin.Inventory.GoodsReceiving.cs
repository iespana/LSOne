using System;
using System.Collections.Generic;
using System.Reflection;
using LSOne.DataLayer.BusinessObjects.Inventory;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.DataProviders.Inventory;
using LSOne.Services.Interfaces.SupportClasses;
using LSOne.SiteService.Utilities;
using LSOne.DataLayer.BusinessObjects.ItemMaster;
using LSOne.DataLayer.GenericConnector.Constants;
using LSOne.DataLayer.BusinessObjects.Omni;
using LSOne.Utilities.Development;
using LSOne.DataLayer.GenericConnector.Enums;
using System.Diagnostics;
using System.Linq;
using LSOne.DataLayer.BusinessObjects.Replenishment;

namespace LSOne.SiteService.Plugins.SiteManager
{
    public partial class SiteManagerPlugin
    {
        /// <summary>
        /// Gets the goods receiving document line
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <param name="goodsReceivingDocumentLineID">The line ID to get</param>
        /// <param name="storeID">The store the purchase order belongs to</param>
        /// <returns></returns>
        public virtual GoodsReceivingDocumentLine GetGoodsReceivingDocumentLine(LogonInfo logonInfo,
            RecordIdentifier goodsReceivingDocumentLineID, RecordIdentifier storeID)
        {
            IConnectionManager dataModel = GetConnectionManager(logonInfo);

            try
            {
                Utils.Log(this, $"{Utils.Starting} {nameof(goodsReceivingDocumentLineID)}: {goodsReceivingDocumentLineID}, {nameof(storeID)}: {storeID}");

                return Providers.GoodsReceivingDocumentLineData.Get(dataModel, goodsReceivingDocumentLineID, storeID);
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
        /// Posts the goods receiving document line
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <param name="goodsReceivingDocumentLineID">The line ID to post</param>
        /// <returns></returns>
        public virtual GoodsReceivingPostResult PostGoodsReceivingLine(LogonInfo logonInfo, RecordIdentifier goodsReceivingDocumentLineID)
        {
            IConnectionManager dataModel = GetConnectionManager(logonInfo);

            try
            {
                Utils.Log(this, $"{Utils.Starting} {nameof(goodsReceivingDocumentLineID)}: {goodsReceivingDocumentLineID}");

                GoodsReceivingDocumentLine grdl = Providers.GoodsReceivingDocumentLineData.Get(dataModel, goodsReceivingDocumentLineID, RecordIdentifier.Empty);

                if (grdl.Posted)
                {
                    return GoodsReceivingPostResult.AlreadyPosted;
                }

                RecordIdentifier inventoryUnitID = Providers.RetailItemData.GetItemUnitID(dataModel, grdl.purchaseOrderLine.ItemID, RetailItem.UnitTypeEnum.Inventory);
                decimal inventoryUnitQty = Providers.UnitConversionData.ConvertQtyBetweenUnits(dataModel, grdl.purchaseOrderLine.ItemID, grdl.purchaseOrderLine.UnitID, inventoryUnitID, grdl.ReceivedQuantity);

                // This means there does not exist a unit conversion rule between the GRDL unit and inventory unit
                if (inventoryUnitQty == 0)
                {
                    return GoodsReceivingPostResult.MissingUnitConversion;
                }

                InventoryTransaction inventTrans = new InventoryTransaction(grdl.purchaseOrderLine.ItemID, grdl.purchaseOrderLine.ItemType)
                {
                    Guid = Guid.NewGuid(),
                    AdjustmentInInventoryUnit = inventoryUnitQty,
                    PostingDate = DateTime.Now,
                    Adjustment = grdl.ReceivedQuantity,
                    StoreID = grdl.StoreID,
                    Type = InventoryTypeEnum.Purchase,
                    OfferID = "",
                    ReasonCode = "",
                    Reference = (string)grdl.GoodsReceivingDocumentID,
                    CostPricePerItem = grdl.purchaseOrderLine.UnitPrice,
                    AdjustmentUnitID = grdl.purchaseOrderLine.UnitID
                };

                Providers.InventoryTransactionData.Save(dataModel, inventTrans);

                grdl.Posted = true;
                Providers.GoodsReceivingDocumentLineData.Save(dataModel, grdl);
                return GoodsReceivingPostResult.Success;
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
        /// Deletes goods receiving document lines for a specific purchase order line
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <param name="purchaseOrderLineID">The unique ID for the purchase order line</param>
        public virtual void DeleteGoodsReceivingLinesForAPurchaseOrderLine(LogonInfo logonInfo, RecordIdentifier purchaseOrderLineID)
        {
            IConnectionManager entry = GetConnectionManager(logonInfo);
            DoWork(entry, () => Providers.GoodsReceivingDocumentData.DeleteLinesForAPurchaseOrderLine(entry, purchaseOrderLineID), MethodBase.GetCurrentMethod().Name);
        }

        /// <summary>
        /// Checks if a goods receiving document exists for a purchase order
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <param name="purchaseOrderID">The ID for the purchase order that is to be checked</param>
        /// <returns>Return true if a goods receiving document is found for this purchase order ID</returns>
        public virtual bool GoodsReceivingDocumentExists(LogonInfo logonInfo, RecordIdentifier purchaseOrderID)
        {
            bool result = false;
            IConnectionManager entry = GetConnectionManager(logonInfo);
            DoWork(entry, () => result = Providers.GoodsReceivingDocumentData.Exists(entry, purchaseOrderID), MethodBase.GetCurrentMethod().Name);

            return result;
        }

        /// <summary>
        /// Saves a goods receiving document header
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <param name="documentHeader">The goods receiving document header that is to be saved</param>
        public virtual void SaveGoodsReceivingDocument(LogonInfo logonInfo, GoodsReceivingDocument documentHeader)
        {
            IConnectionManager entry = GetConnectionManager(logonInfo);
            DoWork(entry, () => Providers.GoodsReceivingDocumentData.Save(entry, documentHeader), MethodBase.GetCurrentMethod().Name);
        }

        /// <summary>
        /// Returns the purchase orders with no goods receiving document for the given store ID
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <param name="storeID">The store ID</param>
        /// <param name="sorting">The sort column</param>
        /// <param name="sortBackwards">The sort direction</param>
        /// <param name="includeLineTotals">True if the total quantity of items and total number of items should be included in the query. Used in OMNI</param>
        /// <returns></returns>
        public virtual List<PurchaseOrder> GetPurchaseOrdersWithNoGoodsReceivingDocumentForStore(LogonInfo logonInfo, RecordIdentifier storeID, PurchaseOrderSorting sorting, bool sortBackwards, bool includeLineTotals)
        {
            IConnectionManager entry = GetConnectionManager(logonInfo);
            List<PurchaseOrder> result = new List<PurchaseOrder>();

            DoWork(entry, () => result = Providers.PurchaseOrderData.GetPurchaseOrdersWithNoGoodsReceivingDocumentForStore(entry, storeID, PurchaseOrderSorting.PurchaseOrderID, false, includeLineTotals), MethodBase.GetCurrentMethod().Name);

            return result;
        }

        /// <summary>
        /// Creates posted goods receiving document lines for all the lines in the purchase order
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <param name="purchaseOrderID">The ID purchase order that the goods receiving lines should be created from</param>
        /// <returns>Result of the posting operation</returns>
        public virtual GoodsReceivingPostResult CreatePostedGoodsReceivingDocumentLinesFromPurchaseOrder(LogonInfo logonInfo, RecordIdentifier purchaseOrderID)
        {
            IConnectionManager entry = GetConnectionManager(logonInfo);

            try
            {
                Utils.Log(this, Utils.Starting);
                Providers.GoodsReceivingDocumentLineData.CreateGoodsReceivingDocumentLinesFromPurchaseOrder(entry, purchaseOrderID);
                GoodsReceivingPostResult result = Providers.GoodsReceivingDocumentData.PostGoodsReceivingDocument(entry, purchaseOrderID);
                return result;
            }
            catch (Exception ex)
            {
                Utils.LogException(this, ex);
                return GoodsReceivingPostResult.Error;
            }
            finally
            {
                ReturnConnection(entry, out entry);
                Utils.Log(this, Utils.Done);
            }
        }

        /// <summary>
        /// Returns true if the goods receiving document line with the given ID is posted; otherwise returns false
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <param name="goodsReceivingDocumentLineID">The line ID to check</param>
        /// <returns></returns>
        public virtual bool GoodReceivingDocumentLineIsPosted(LogonInfo logonInfo, RecordIdentifier goodsReceivingDocumentLineID)
        {
            IConnectionManager entry = GetConnectionManager(logonInfo);
            bool result = false;

            DoWork(entry, () => result = Providers.GoodsReceivingDocumentLineData.IsPosted(entry,
                        goodsReceivingDocumentLineID), MethodBase.GetCurrentMethod().Name);

            return result;
        }

        /// <summary>
        /// If the goods receiving document does not have any posted lines the document is deleted.
        /// </summary>
        /// <param name="goodsReceivingDocumentID">The document to be deleted</param>
        /// <param name="logonInfo">The login information for the database</param>
        /// <returns>Retrns <see cref="GoodsReceivingDocumentDeleteResult"/> that tells us if there were posted lines or if the document was deleted </returns>
        public virtual GoodsReceivingDocumentDeleteResult DeleteGoodsReceivingDocument(LogonInfo logonInfo, RecordIdentifier goodsReceivingDocumentID)
        {
            IConnectionManager entry = GetConnectionManager(logonInfo);

            try
            {
                Utils.Log(this, $"{Utils.Starting} {nameof(goodsReceivingDocumentID)}: {goodsReceivingDocumentID}");

                if (Providers.GoodsReceivingDocumentData.HasPostedLines(entry, goodsReceivingDocumentID))
                {
                    return GoodsReceivingDocumentDeleteResult.HasPostedLines;
                }
                Providers.GoodsReceivingDocumentData.Delete(entry, goodsReceivingDocumentID);

                return GoodsReceivingDocumentDeleteResult.DocumentDeleted;
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
        /// Searches for all goods receiving documents that match the search criteria as it has been set.
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <param name="searchCriteria">One or more of the variables need to be set so that the search is limited, otherwise all documents are returned</param>
        /// <returns>A list of the goods receiving documents found in the search</returns>
        public virtual List<GoodsReceivingDocument> GetGoodsReceivingDocuments(LogonInfo logonInfo, GoodsReceivingDocumentSearch searchCriteria)
        {
            IConnectionManager entry = GetConnectionManager(logonInfo);
            List<GoodsReceivingDocument> result = new List<GoodsReceivingDocument>();

            DoWork(entry, () => result = Providers.GoodsReceivingDocumentData.AdvancedSearch(entry, searchCriteria, GoodsReceivingDocumentSorting.GoodsReceivingID, false), MethodBase.GetCurrentMethod().Name);

            return result;
        }

        /// <summary>
        /// Gets the goods receiving document
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <param name="goodsReceivingDocumentID">The ID of the document</param>
        /// <returns></returns>
        public virtual GoodsReceivingDocument GetGoodsReceivingDocument(LogonInfo logonInfo, RecordIdentifier goodsReceivingDocumentID)
        {
            IConnectionManager entry = GetConnectionManager(logonInfo);
            GoodsReceivingDocument result = null;

            DoWork(entry, () => result = Providers.GoodsReceivingDocumentData.Get(entry, goodsReceivingDocumentID), MethodBase.GetCurrentMethod().Name);

            return result;
        }

        /// <summary>
        /// Retrieves the sum of all received items per goods receiving document.
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <param name="numberOfDocuments">For how many GR documents should the total be retrieved</param>
        /// <returns></returns>
        public virtual List<InventoryTotals> GetReceivedTotals(LogonInfo logonInfo, int numberOfDocuments)
        {
            IConnectionManager entry = GetConnectionManager(logonInfo);
            List<InventoryTotals> result = new List<InventoryTotals>();

            DoWork(entry, () => result = Providers.GoodsReceivingDocumentLineData.GetReceivedTotals(entry, numberOfDocuments), MethodBase.GetCurrentMethod().Name);

            return result;
        }

        /// <summary>
        /// Returns the total number of GR documents that are in the database
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <returns>the total number of GR documents that are in the database</returns>
        public virtual int GetTotalNumberOfGRDocuments(LogonInfo logonInfo)
        {
            IConnectionManager entry = GetConnectionManager(logonInfo);
            int result = 0;

            DoWork(entry, () => result = Providers.GoodsReceivingDocumentData.CountDocuments(entry), MethodBase.GetCurrentMethod().Name);

            return result;
        }

        /// <summary>
        /// Returns the total number of goods receiving documents that are found using the search criteria.
        /// The GetGoodsReceivingDocuments function limits the result set so this tells us how many there are available in total
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <param name="searchCriteria">One or more of the variables need to be set so that the search is limited, otherwise all documents are returned</param>
        /// <returns>the total number of goods receiving documents that are found using the search criteria</returns>
        public virtual int CountGoodsReceivingDocumentsSearchResults(LogonInfo logonInfo, GoodsReceivingDocumentSearch searchCriteria)
        {
            IConnectionManager entry = GetConnectionManager(logonInfo);
            int result = 0;

            DoWork(entry, () => result = Providers.GoodsReceivingDocumentData.CountSearchResults(entry, searchCriteria), MethodBase.GetCurrentMethod().Name);

            return result;
        }

        /// <summary>
        /// Returns all goods receiving document lines for a specific purchase order line
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <param name="purchaseOrderLineID">the unique ID for the purchase order line</param>
        /// <returns>All goods receiving lines attached to one specific purchase order line</returns>
        public virtual List<GoodsReceivingDocumentLine> GetGoodsReceivingDocumentLinesForAPurchaseOrderLine(LogonInfo logonInfo, RecordIdentifier purchaseOrderLineID)
        {
            IConnectionManager entry = GetConnectionManager(logonInfo);
            List<GoodsReceivingDocumentLine> result = new List<GoodsReceivingDocumentLine>();

            DoWork(entry, () => result = Providers.GoodsReceivingDocumentData.GetGoodsReceivingDocumentLinesForAPurchaseOrderLine(entry, purchaseOrderLineID), MethodBase.GetCurrentMethod().Name);

            return result;
        }

        /// <summary>
        /// Gets a list of document lines matching the search criteria
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <param name="searchCriteria">The search criteria</param>
        /// <param name="sortBy">The sort method</param>
        /// <param name="sortBackwards">The sort direction</param>
        /// <param name="totalCount">Total Rows</param>
        /// <returns></returns>
        public virtual List<GoodsReceivingDocumentLine> SearchGoodsReceivingDocumentLines(LogonInfo logonInfo, GoodsReceivingDocumentLineSearch searchCriteria,
            GoodsReceivingDocumentLineSorting sortBy, bool sortBackwards, out int totalCount)
        {
            IConnectionManager entry = GetConnectionManager(logonInfo);

            try
            {
                Utils.Log(this, Utils.Starting);

                return Providers.GoodsReceivingDocumentLineData.AdvancedSearch(entry, searchCriteria, sortBy, sortBackwards, out totalCount);
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
        /// Updates the status of a given goods receiving document if necessary. Returns whether the document was updated or not.
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <param name="goodsReceivingDocumentID">The ID of the document</param>
        /// <returns></returns>
        public virtual bool UpdateGoodsReceivingDocumentStatus(LogonInfo logonInfo, RecordIdentifier goodsReceivingDocumentID)
        {
            IConnectionManager entry = GetConnectionManager(logonInfo);
            bool result = false;

            DoWork(entry, () => result = Providers.GoodsReceivingDocumentData.UpdateStatus(entry, goodsReceivingDocumentID), MethodBase.GetCurrentMethod().Name);

            return result;
        }

        /// <summary>
        /// Saves a goods receiving document line
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <param name="goodsReceivingDocumentLine">The line to save</param>
        public virtual void SaveGoodsReceivingDocumentLine(LogonInfo logonInfo, GoodsReceivingDocumentLine goodsReceivingDocumentLine)
        {
            IConnectionManager entry = GetConnectionManager(logonInfo);
            
            DoWork(entry, () => Providers.GoodsReceivingDocumentLineData.Save(entry, goodsReceivingDocumentLine), MethodBase.GetCurrentMethod().Name);
        }

        /// <summary>
        /// Determines if the supplied document is fully received
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <param name="goodsReceivingDocumentID">The document</param>
        /// <returns></returns>
        public virtual bool GoodsReceivingDocumentFullyReceived(LogonInfo logonInfo, RecordIdentifier goodsReceivingDocumentID)
        {
            IConnectionManager entry = GetConnectionManager(logonInfo);
            bool result = false;

            DoWork(entry, () => result = Providers.GoodsReceivingDocumentData.FullyReceived(entry, goodsReceivingDocumentID), MethodBase.GetCurrentMethod().Name);

            return result;
        }

        /// <summary>
        /// Returns true if all the goods receiving document lines are posted; otherwise returns false
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <param name="goodsReceivingDocumentID">The ID oif the document</param>
        /// <returns></returns>
        public virtual bool GoodsReceivingDocumentAllLinesPosted(LogonInfo logonInfo, RecordIdentifier goodsReceivingDocumentID)
        {
            IConnectionManager entry = GetConnectionManager(logonInfo);
            bool result = false;

            DoWork(entry, () => result = Providers.GoodsReceivingDocumentData.AllLinesPosted(entry, goodsReceivingDocumentID), MethodBase.GetCurrentMethod().Name);

            return result;
        }

        /// <summary>
        /// Deletes the supplied goods receiving document line
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <param name="goodsReceivingDocumentLineID">The line to delete</param>
        public virtual void DeleteGoodsReceivingDocumentLine(LogonInfo logonInfo, RecordIdentifier goodsReceivingDocumentLineID)
        {
            IConnectionManager entry = GetConnectionManager(logonInfo);
            
            DoWork(entry, () => Providers.GoodsReceivingDocumentLineData.Delete(entry, goodsReceivingDocumentLineID), MethodBase.GetCurrentMethod().Name);
        }

        /// <summary>
        /// Creates goods receiving document lines based on a purchase order with the given ID
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <param name="purchaseOrderID"></param>
        public virtual void CreateGoodsReceivingDocumentLinesFromPurchaseOrder(LogonInfo logonInfo, RecordIdentifier purchaseOrderID)
        {
            IConnectionManager entry = GetConnectionManager(logonInfo);

            DoWork(entry, () => Providers.GoodsReceivingDocumentLineData.CreateGoodsReceivingDocumentLinesFromPurchaseOrder(entry, purchaseOrderID), MethodBase.GetCurrentMethod().Name);
        }

        /// <summary>
        /// Returns true if the goods receiving document with the given ID has lines; otherwise returns false
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <param name="goodsReceivingDocumentID">The document to be checked</param>
        /// <returns></returns>
        public virtual bool GoodsReceivingDocumentHasLines(LogonInfo logonInfo, RecordIdentifier goodsReceivingDocumentID)
        {
            IConnectionManager entry = GetConnectionManager(logonInfo);
            bool result = false;

            DoWork(entry, () => result = Providers.GoodsReceivingDocumentData.HasLines(entry, goodsReceivingDocumentID), MethodBase.GetCurrentMethod().Name);

            return result;
        }

        /// <summary>
        /// Gets existing lines for a specific goods receiving document
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <param name="goodsReceivingID">The unique ID for the goods receiving document</param>
        /// <returns>A list of goods receiving lines</returns>
        public virtual List<GoodsReceivingDocumentLine> GetGoodsReceivingDocumentLines(LogonInfo logonInfo, RecordIdentifier goodsReceivingID)
        {
            List<GoodsReceivingDocumentLine> result = null;
            IConnectionManager entry = GetConnectionManager(logonInfo);

            DoWork(entry, () => result = Providers.GoodsReceivingDocumentLineData.GetGoodsReceivingDocumentLines(entry, goodsReceivingID), MethodBase.GetCurrentMethod().Name);

            return result;
        }

        /// <summary>
        /// Post all lines from a goods receiving document
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <param name="goodsReceivingDocumentID">The ID of the goods receiving document to post</param>
        /// <returns>Result of the operation</returns>
        public virtual GoodsReceivingPostResult PostGoodsReceivingDocument(LogonInfo logonInfo, RecordIdentifier goodsReceivingDocumentID)
        {
            IConnectionManager entry = GetConnectionManager(logonInfo);

            try
            {
                Utils.Log(this, Utils.Starting);
                GoodsReceivingPostResult result = Providers.GoodsReceivingDocumentData.PostGoodsReceivingDocument(entry, goodsReceivingDocumentID);
                return result;
            }
            catch (Exception ex)
            {
                Utils.LogException(this, ex);
                return GoodsReceivingPostResult.Error;
            }
            finally
            {
                ReturnConnection(entry, out entry);
                Utils.Log(this, Utils.Done);
            }
        }

        internal void CreateGoodsReceivingForPurchaseOrderTemplate(IConnectionManager entry, InventoryTemplate template, RecordIdentifier purchaseOrderID)
        {
            if (template.CreateGoodsReceivingDocument)
            {
                GoodsReceivingDocument gDoc = new GoodsReceivingDocument
                {
                    GoodsReceivingID = purchaseOrderID,
                    PurchaseOrderID = purchaseOrderID
                };

                Providers.GoodsReceivingDocumentData.Save(entry, gDoc);

                if (template.AutoPopulateGoodsReceivingDocument)
                {
                    Providers.GoodsReceivingDocumentLineData.CreateGoodsReceivingDocumentLinesFromPurchaseOrder(entry, purchaseOrderID);
                }
            }
        }
    }
}
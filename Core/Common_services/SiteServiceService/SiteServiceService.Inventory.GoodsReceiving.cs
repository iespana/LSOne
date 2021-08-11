using System.Collections.Generic;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.Inventory;
using LSOne.DataLayer.BusinessObjects.Omni;
using LSOne.DataLayer.BusinessObjects.Profiles;
using LSOne.DataLayer.DataProviders.Inventory;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;
using LSOne.Utilities.Development;

namespace LSOne.Services
{
    public partial class SiteServiceService
    {
        public virtual PurchaseOrderLine GetPurchaseOrderLine(IConnectionManager entry, SiteServiceProfile siteServiceProfile,
            RecordIdentifier purchaseOrderLineID, RecordIdentifier storeID, bool includeReportFormatting, bool closeConnection)
        {
            PurchaseOrderLine result = null;
            DoRemoteWork(entry, siteServiceProfile, () => result = server.GetPurchaseOrderLine(CreateLogonInfo(entry), purchaseOrderLineID, storeID, includeReportFormatting), closeConnection);
            return result;
        }

        /// <summary>
        /// Deletes all goods receiving lines attached to a purchase order line. Use PurchaseOrderLineHasPostedGoodsReceivingDocumentLine to validate that no goods receiving lines have been posted before deleting them
        /// </summary>
        /// <param name="siteServiceProfile">Which site service to use for this operation</param>
        /// <param name="entry">The entry into the database</param>
        /// <param name="purchaseOrderLineID">The ID for the purchase order line. PrimaryID is the purchase Order ID and SecondaryID is the line number</param>
        /// <param name="closeConnection">If true then the connection to the site service is closed as soon as the operation has finished otherwise it will stay open</param>
        public virtual void DeleteGoodsReceivingLinesForAPurchaseOrderLine(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier purchaseOrderLineID, bool closeConnection)
        {
            DoRemoteWork(entry, siteServiceProfile, () => server.DeleteGoodsReceivingLinesForAPurchaseOrderLine(CreateLogonInfo(entry), purchaseOrderLineID), closeConnection);
        }

        /// <summary>
        /// Checks if a goods receiving document exists for a purchase order
        /// </summary>
        /// <param name="siteServiceProfile">Which site service to use for this operation</param>
        /// <param name="entry">The entry into the database</param>
        /// <param name="closeConnection">If true then the connection to the site service is closed as soon as the operation has finished otherwise it will stay open</param>
        /// <param name="purchaseOrderID">The ID for the purchase order that is to be checked</param>
        /// <returns>Return true if a goods receiving document is found for this purchase order ID</returns>
        public virtual bool GoodsReceivingDocumentExists(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier purchaseOrderID, bool closeConnection)
        {
            bool result = true;
            DoRemoteWork(entry, siteServiceProfile, () => result = server.GoodsReceivingDocumentExists(CreateLogonInfo(entry), purchaseOrderID), closeConnection);
            return result;
        }

        /// <summary>
        /// Saves a goods receiving document header
        /// </summary>
        /// <param name="siteServiceProfile">Which site service to use for this operation</param>
        /// <param name="entry">The entry into the database</param>
        /// <param name="closeConnection">If true then the connection to the site service is closed as soon as the operation has finished otherwise it will stay open</param>
        /// <param name="documentHeader">The goods receiving document header that is to be saved</param>
        public virtual void SaveGoodsReceivingDocument(IConnectionManager entry, SiteServiceProfile siteServiceProfile, GoodsReceivingDocument documentHeader, bool closeConnection)
        {
            DoRemoteWork(entry, siteServiceProfile, () => server.SaveGoodsReceivingDocument(CreateLogonInfo(entry), documentHeader), closeConnection);
        }

        public virtual List<PurchaseOrder> GetPurchaseOrdersWithNoGoodsReceivingDocumentForStore(
            IConnectionManager entry,
            SiteServiceProfile siteServiceProfile,
            RecordIdentifier storeID,
            PurchaseOrderSorting sorting,
            bool sortBackwards,
            bool includeLineTotals,
            bool closeConnection)
        {
            List<PurchaseOrder> result = null;
            DoRemoteWork(entry, siteServiceProfile, () => result = server.GetPurchaseOrdersWithNoGoodsReceivingDocumentForStore(CreateLogonInfo(entry), storeID, sorting, sortBackwards, includeLineTotals), closeConnection);
            return result;

        }

        /// <summary>
        /// Creates posted goods receiving lines for all the lines in the purchase order
        /// </summary>
        /// <param name="siteServiceProfile">Which site service to use for this operation</param>
        /// <param name="entry">The entry into the database</param>
        /// <param name="closeConnection">If true then the connection to the site service is closed as soon as the operation has finished otherwise it will stay open</param>
        /// <param name="purchaseOrderID">The ID purchase order that the goods receiving lines should be created from</param>
        /// <returns>Result of the posting operation</returns>
        public virtual GoodsReceivingPostResult CreatePostedGoodsReceivingDocumentLinesFromPurchaseOrder(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier purchaseOrderID, bool closeConnection)
        {
            GoodsReceivingPostResult result = GoodsReceivingPostResult.Success;
            DoRemoteWork(entry, siteServiceProfile, () => result = server.CreatePostedGoodsReceivingDocumentLinesFromPurchaseOrder(CreateLogonInfo(entry), purchaseOrderID), closeConnection);
            return result;
        }

        /// <summary>
        /// If the Goods receiving document does not have any posted lines the document is deleted.
        /// </summary>
        /// <param name="goodsReceivingDocumentID">The document to be deleted</param>
        /// <param name="siteServiceProfile">Which site service to use for this operation</param>
        /// <param name="entry">The entry into the database</param>
        /// <param name="closeConnection">If true then the connection to the site service is closed as soon as the operation has finished otherwise it will stay open</param>
        /// <returns>Retrns <see cref="GoodsReceivingDocumentDeleteResult"/> that tells us if there were posted lines or if the document was deleted </returns>
        public virtual GoodsReceivingDocumentDeleteResult DeleteGoodsReceivingDocument(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier goodsReceivingDocumentID, bool closeConnection)
        {
            GoodsReceivingDocumentDeleteResult result = GoodsReceivingDocumentDeleteResult.HasPostedLines;
            DoRemoteWork(entry, siteServiceProfile, () => result = server.DeleteGoodsReceivingDocument(CreateLogonInfo(entry), goodsReceivingDocumentID), closeConnection);
            return result;
        }

        /// <summary>
        /// Searches for all goods receiving documents that match the search criteria as it has been set.
        /// </summary>
        /// <param name="siteServiceProfile">Which site service to use for this operation</param>
        /// <param name="entry">The entry into the database</param>
        /// <param name="closeConnection">If true then the connection to the site service is closed as soon as the operation has finished otherwise it will stay open</param>
        /// <param name="searchCriteria">One or more of the variables need to be set so that the search is limited, otherwise all documents are returned</param>
        /// <returns>A list of the goods receiving documents found in the search</returns>
        public virtual List<GoodsReceivingDocument> GetGoodsReceivingDocuments(IConnectionManager entry, SiteServiceProfile siteServiceProfile, GoodsReceivingDocumentSearch searchCriteria, bool closeConnection)
        {
            List<GoodsReceivingDocument> result = new List<GoodsReceivingDocument>();
            DoRemoteWork(entry, siteServiceProfile, () => result = server.GetGoodsReceivingDocuments(CreateLogonInfo(entry), searchCriteria), closeConnection);
            return result;
        }

        public virtual GoodsReceivingDocument GetGoodsReceivingDocument(IConnectionManager entry, SiteServiceProfile siteServiceProfile,
            RecordIdentifier goodsReceivingDocumentID, bool closeConnection)
        {
            GoodsReceivingDocument result = null;
            DoRemoteWork(entry, siteServiceProfile, () => result = server.GetGoodsReceivingDocument(CreateLogonInfo(entry), goodsReceivingDocumentID), closeConnection);
            return result;
        }

        /// <summary>
        /// Get goods receivingdocument ID
        /// </summary>
        /// <param name="siteServiceProfile">Which site service to use for this operation</param>
        /// <param name="entry">The entry into the database</param>
        /// <param name="closeConnection">If true then the connection to the site service is closed as soon as the operation has finished otherwise it will stay open</param>
        /// <param name="goodsReceivingDocumentID">The ID of the document</param>
        /// <param name="storeID">The ID of the store</param>
         /// <returns></returns>
        public virtual GoodsReceivingDocumentLine GetGoodsReceivingDocumentLine(IConnectionManager entry, SiteServiceProfile siteServiceProfile,
            RecordIdentifier goodsReceivingDocumentID, RecordIdentifier storeID, bool closeConnection)
        {
            GoodsReceivingDocumentLine result = null;
            DoRemoteWork(entry, siteServiceProfile, () => result = server.GetGoodsReceivingDocumentLine(CreateLogonInfo(entry), goodsReceivingDocumentID,storeID), closeConnection);
            return result;
        }

        /// <summary>
        /// Retrieves the sum of all received items per goods receiving document. The list of documents is controlled by the search criteria <see cref="GoodsReceivingDocumentLineSearch"/>
        /// </summary>
        /// <param name="siteServiceProfile">Which site service to use for this operation</param>
        /// <param name="entry">The entry into the database</param>
        /// <param name="closeConnection">If true then the connection to the site service is closed as soon as the operation has finished otherwise it will stay open</param>
        /// <param name="numberOfDocuments">For how many GR documents should the total be retrieved</param>
        /// <returns></returns>
        public virtual List<InventoryTotals> GetReceivedTotals(IConnectionManager entry, SiteServiceProfile siteServiceProfile, int numberOfDocuments, bool closeConnection)
        {
            List<InventoryTotals> result = new List<InventoryTotals>();
            DoRemoteWork(entry, siteServiceProfile, () => result = server.GetReceivedTotals(CreateLogonInfo(entry), numberOfDocuments), closeConnection);
            return result;
        }


        /// <summary>
        /// Return the total number of GR documents that are in the database
        /// </summary>
        /// <param name="siteServiceProfile">Which site service to use for this operation</param>
        /// <param name="entry">The entry into the database</param>
        /// <param name="closeConnection">If true then the connection to the site service is closed as soon as the operation has finished otherwise it will stay open</param>
        /// <returns>the total number of GR documents that are in the database</returns>
        public virtual int GetTotalNumberOfGRDocuments(IConnectionManager entry, SiteServiceProfile siteServiceProfile, bool closeConnection)
        {
            int result = 0;
            DoRemoteWork(entry, siteServiceProfile, () => result = server.GetTotalNumberOfGRDocuments(CreateLogonInfo(entry)), closeConnection);
            return result;
        }

        /// <summary>
        /// Returns the total number of goods receiving documents that are found using the search criteria.
        /// The GetGoodsReceivingDocuments function limits the result set so this tells us how many there are available in total
        /// </summary>
        /// <param name="siteServiceProfile">Which site service to use for this operation</param>
        /// <param name="entry">The entry into the database</param>
        /// <param name="closeConnection">If true then the connection to the site service is closed as soon as the operation has finished otherwise it will stay open</param>
        /// <param name="searchCriteria">One or more of the variables need to be set so that the search is limited, otherwise all documents are returned</param>
        /// <returns>the total number of goods receiving documents that are found using the search criteria</returns>
        public virtual int CountGoodsReceivingDocumentsSearchResults(IConnectionManager entry, SiteServiceProfile siteServiceProfile, GoodsReceivingDocumentSearch searchCriteria, bool closeConnection)
        {
            int result = 0;
            DoRemoteWork(entry, siteServiceProfile, () => result = server.CountGoodsReceivingDocumentsSearchResults(CreateLogonInfo(entry), searchCriteria), closeConnection);
            return result;
        }

        /// <summary>
        /// Returns all goods receiving lines for a specific purchase order line
        /// </summary>
        /// <param name="siteServiceProfile">Which site service to use for this operation</param>
        /// <param name="entry">The entry into the database</param>
        /// <param name="closeConnection">If true then the connection to the site service is closed as soon as the operation has finished otherwise it will stay open</param>
        /// <param name="purchaseOrderLineID">the unique ID for the purchase order line</param>
        /// <returns>All goods receiving lines attached to one specific purchase order line</returns>
        public virtual List<GoodsReceivingDocumentLine> GetGoodsReceivingDocumentLinesForAPurchaseOrderLine(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier purchaseOrderLineID, bool closeConnection)
        {
            List<GoodsReceivingDocumentLine> result = new List<GoodsReceivingDocumentLine>();
            DoRemoteWork(entry, siteServiceProfile, () => result = server.GetGoodsReceivingDocumentLinesForAPurchaseOrderLine(CreateLogonInfo(entry), purchaseOrderLineID), closeConnection);
            return result;
        }

        /// <summary>
        /// Search for goods receiving lines 
        /// </summary>        
        /// <param name="entry">The entry into the database</param>
        /// <param name="siteServiceProfile">Which site service to use for this operation</param>
        /// <param name="searchCriteria">Search paramters</param>
        /// <param name="sortBy">Sort method</param>
        /// <param name="sortBackwards">Sort direction</param>
        /// <param name="closeConnection">If true then the connection to the site service is closed as soon as the operation has finished otherwise it will stay open</param>
        /// <param name="totalCount">The total rows</param>
        /// <returns></returns>
        public virtual List<GoodsReceivingDocumentLine> SearchGoodsReceivingDocumentLines(IConnectionManager entry, SiteServiceProfile siteServiceProfile, GoodsReceivingDocumentLineSearch searchCriteria,
            GoodsReceivingDocumentLineSorting sortBy, bool sortBackwards, bool closeConnection, out int totalCount)
        {
            List<GoodsReceivingDocumentLine> result = new List<GoodsReceivingDocumentLine>();
            int count = 0;
            DoRemoteWork(entry, siteServiceProfile, () => 
            result = server.SearchGoodsReceivingDocumentLines(CreateLogonInfo(entry), searchCriteria,sortBy,sortBackwards, out count), closeConnection);
            totalCount = count;
            return result;
        }

        /// <summary>
        /// Posts the goods receiving line with the supplied ID
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="siteServiceProfile">Which site service to use for this operation</param>
        /// <param name="goodsReceivingDocumentLineID">The ID of the line</param>        
        /// <param name="closeConnection">If true then the connection to the site service is closed as soon as the operation has finished otherwise it will stay open</param>
        /// <returns></returns>
        public virtual GoodsReceivingPostResult PostGoodsReceivingLine(IConnectionManager entry, SiteServiceProfile siteServiceProfile,
            RecordIdentifier goodsReceivingDocumentLineID, bool closeConnection)
        {
            GoodsReceivingPostResult result = GoodsReceivingPostResult.Success;
            DoRemoteWork(entry, siteServiceProfile, () => result = server.PostGoodsReceivingLine(CreateLogonInfo(entry), goodsReceivingDocumentLineID), closeConnection);
            return result;
        }

        /// <summary>
        /// Updates the status of a given goods receiving document if necessary. Returns whether the document was updated or not.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="siteServiceProfile">Which site service to use for this operation</param>
        /// <param name="goodsReceivingDocumentID">The ID of the document</param>
        /// <param name="closeConnection">If true then the connection to the site service is closed as soon as the operation has finished otherwise it will stay open</param>
        /// <returns></returns>
        public virtual bool UpdateGoodsReceivingDocumentStatus(IConnectionManager entry, SiteServiceProfile siteServiceProfile,
            RecordIdentifier goodsReceivingDocumentID, bool closeConnection)
        {
            bool result = false;
            DoRemoteWork(entry, siteServiceProfile,
                () =>
                    result = server.UpdateGoodsReceivingDocumentStatus(CreateLogonInfo(entry), goodsReceivingDocumentID),
                closeConnection);

            return result;
        }

        /// <summary>
        /// Saves a given goods receiving document line into the database
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="siteServiceProfile">Which site service to use for this operation</param>
        /// <param name="goodsReceivingDocumentLine">The line to save</param>
        /// <param name="closeConnection">If true then the connection to the site service is closed as soon as the operation has finished otherwise it will stay open</param>
        public virtual void SaveGoodsReceivingDocumentLine(IConnectionManager entry, SiteServiceProfile siteServiceProfile,
            GoodsReceivingDocumentLine goodsReceivingDocumentLine, bool closeConnection)
        {
            DoRemoteWork(entry, siteServiceProfile, () => server.SaveGoodsReceivingDocumentLine(CreateLogonInfo(entry), goodsReceivingDocumentLine), closeConnection);
            
        }

        /// <summary>
        /// Deletes the supplied goods receiving document line
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="siteServiceProfile">Which site service to use for this operation</param>
        /// <param name="closeConnection">If true then the connection to the site service is closed as soon as the operation has finished otherwise it will stay open</param>
        /// <param name="goodsReceivingDocumentLineID">The line to delete</param>
        public virtual void DeleteGoodsReceivingDocumentLine(IConnectionManager entry, SiteServiceProfile siteServiceProfile,
            RecordIdentifier goodsReceivingDocumentLineID, bool closeConnection)
        {
            DoRemoteWork(entry, siteServiceProfile,() => server.DeleteGoodsReceivingDocumentLine(CreateLogonInfo(entry), goodsReceivingDocumentLineID),
                closeConnection);
        }

        public virtual void CreateGoodsReceivingDocumentLinesFromPurchaseOrder(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier purchaseOrderID, bool closeConnection)
        {
            DoRemoteWork(entry, siteServiceProfile, () => server.CreateGoodsReceivingDocumentLinesFromPurchaseOrder(CreateLogonInfo(entry), purchaseOrderID), closeConnection);
        }

        public virtual bool GoodsReceivingDocumentHasLines(IConnectionManager entry, SiteServiceProfile siteServiceProfile,
            RecordIdentifier goodsReceivingDocumentID, bool closeConnection)
        {
            bool result = false;
            DoRemoteWork(entry, siteServiceProfile,
                () =>
                    result = server.GoodsReceivingDocumentHasLines(CreateLogonInfo(entry), goodsReceivingDocumentID),
                closeConnection);

            return result;
        }

        /// <summary>
        /// Has the document been fully recieved
        /// </summary>
        /// <param name="siteServiceProfile">Which site service to use for this operation</param>
        /// <param name="entry">The entry into the database</param>
        /// <param name="closeConnection">If true then the connection to the site service is closed as soon as the operation has finished otherwise it will stay open</param>
        /// <param name="goodsReceivingDocumentID">The ID if the document</param>
        /// <returns></returns>
        public virtual bool GoodsReceivingDocumentFullyReceived(IConnectionManager entry, SiteServiceProfile siteServiceProfile,
            RecordIdentifier goodsReceivingDocumentID, bool closeConnection)
        {
            bool result = false;
            DoRemoteWork(entry, siteServiceProfile,
                () =>
                    result = server.GoodsReceivingDocumentFullyReceived(CreateLogonInfo(entry), goodsReceivingDocumentID),
                closeConnection);

            return result;
        }

        /// <summary>
        /// Are all the lines on the document posted
        /// </summary>
        /// <param name="siteServiceProfile">Which site service to use for this operation</param>
        /// <param name="entry">The entry into the database</param>
        /// <param name="closeConnection">If true then the connection to the site service is closed as soon as the operation has finished otherwise it will stay open</param>
        /// <param name="goodsReceivingDocumentID">The ID of the document</param>
        /// <returns></returns>
        public virtual bool GoodsReceivingDocumentAllLinesPosted(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier goodsReceivingDocumentID, bool closeConnection)
        {
            bool result = false;
            DoRemoteWork(entry, siteServiceProfile,
                () =>
                    result = server.GoodsReceivingDocumentAllLinesPosted(CreateLogonInfo(entry), goodsReceivingDocumentID),
                closeConnection);

            return result;
        }

        /// <summary>
        /// Gets the purchase order ID for the goods receiving dosument
        /// </summary>
        /// <param name="siteServiceProfile">Which site service to use for this operation</param>
        /// <param name="entry">The entry into the database</param>
        /// <param name="closeConnection">If true then the connection to the site service is closed as soon as the operation has finished otherwise it will stay open</param>
        /// <param name="goodsReceivingDocumentID">The ID if the document</param>
        /// <returns></returns>
        public virtual RecordIdentifier GetPurchaseOrderID(IConnectionManager entry, SiteServiceProfile siteServiceProfile,
          RecordIdentifier goodsReceivingDocumentID, bool closeConnection)
        {
            RecordIdentifier result = null;

            DoRemoteWork(entry, siteServiceProfile, () => result = server.GetPurchaseOrderID(CreateLogonInfo(entry), goodsReceivingDocumentID), closeConnection);

            return result;
        }

        public int MaxOverGoodsReceive(IConnectionManager entry, SiteServiceProfile siteServiceProfile, bool closeConnection)
        {
            int result = 0;

            DoRemoteWork(entry, siteServiceProfile, () => result = server.MaxOverGoodsReceive(CreateLogonInfo(entry)), closeConnection);

            return result;
        }

        /// <summary>
        /// Gets existing lines for a specific goods receiving document
        /// </summary>
        /// <param name="siteServiceProfile">Which site service to use for this operation</param>
        /// <param name="entry">The entry into the database</param>
        /// <param name="closeConnection">If true then the connection to the site service is closed as soon as the operation has finished otherwise it will stay open</param>
        /// <param name="goodsReceivingID">The unique ID for the goods receiving document</param>
        /// <returns>A list of goods receiving lines</returns>
        public virtual List<GoodsReceivingDocumentLine> GetGoodsReceivingDocumentLines(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier goodsReceivingID, bool closeConnection)
        {
            List<GoodsReceivingDocumentLine> result = null;

            DoRemoteWork(entry, siteServiceProfile, () => result = server.GetGoodsReceivingDocumentLines(CreateLogonInfo(entry), goodsReceivingID), closeConnection);

            return result;
        }

        public virtual bool GoodReceivingDocumentLineIsPosted(IConnectionManager entry, SiteServiceProfile siteServiceProfile,
            RecordIdentifier goodsReceivingDocumentLineID, bool closeConnection)
        {
            bool result = false;

            DoRemoteWork(entry, siteServiceProfile, () => result = server.GoodReceivingDocumentLineIsPosted(CreateLogonInfo(entry), goodsReceivingDocumentLineID), closeConnection);

            return result;
        }

        /// <summary>
        /// Post all lines from a goods receiving document
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="siteServiceProfile">Which site service to use for this operation</param>
        /// <param name="goodsReceivingDocumentID">The ID of the goods receiving document to post</param>
        /// <param name="closeConnection">If true then the connection to the site service is closed as soon as the operation has finished otherwise it will stay open</param>
        /// <returns>Result of the operation</returns>
        public virtual GoodsReceivingPostResult PostGoodsReceivingDocument(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier goodsReceivingDocumentID, bool closeConnection)
        {
            GoodsReceivingPostResult result = GoodsReceivingPostResult.Success;

            DoRemoteWork(entry, siteServiceProfile, () => result = server.PostGoodsReceivingDocument(CreateLogonInfo(entry), goodsReceivingDocumentID), closeConnection);

            return result;
        }
    }
}

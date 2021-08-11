using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.Inventory;
using LSOne.DataLayer.BusinessObjects.Profiles;
using LSOne.DataLayer.DataProviders.Inventory;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;

namespace LSOne.Services
{
    public partial class InventoryService
    {
        /// <summary>
        /// Deletes goods receiving lines for a specific purchase order line
        /// </summary>
        /// <param name="siteServiceProfile">Which site service to use for this operation</param>
        /// <param name="entry">The entry into the database</param>
        /// <param name="closeConnection">If true then the connection to the site service is closed as soon as the operation has finished otherwise it will stay open</param>
        /// <param name="purchaseOrderLineID">The unique ID for the purchase order line</param>
        public virtual void DeleteGoodsReceivingLinesForAPurchaseOrderLine(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier purchaseOrderLineID, bool closeConnection)
        {
            Interfaces.Services.SiteServiceService(entry).DeleteGoodsReceivingLinesForAPurchaseOrderLine(entry, siteServiceProfile, purchaseOrderLineID, closeConnection);
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
            return Interfaces.Services.SiteServiceService(entry).GoodsReceivingDocumentExists(entry, siteServiceProfile, purchaseOrderID, closeConnection);
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
            Interfaces.Services.SiteServiceService(entry).SaveGoodsReceivingDocument(entry, siteServiceProfile, documentHeader, closeConnection);
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
            return Interfaces.Services.SiteServiceService(entry).CreatePostedGoodsReceivingDocumentLinesFromPurchaseOrder(entry, siteServiceProfile, purchaseOrderID, closeConnection);
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
            return Interfaces.Services.SiteServiceService(entry).DeleteGoodsReceivingDocument(entry, siteServiceProfile, goodsReceivingDocumentID, closeConnection);
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
            return Interfaces.Services.SiteServiceService(entry).GetGoodsReceivingDocuments(entry, siteServiceProfile, searchCriteria, closeConnection);
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
            return Interfaces.Services.SiteServiceService(entry).GetReceivedTotals(entry, siteServiceProfile, numberOfDocuments, closeConnection);
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
            return Interfaces.Services.SiteServiceService(entry).GetTotalNumberOfGRDocuments(entry, siteServiceProfile, closeConnection);
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
            return Interfaces.Services.SiteServiceService(entry).CountGoodsReceivingDocumentsSearchResults(entry, siteServiceProfile, searchCriteria, closeConnection);
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
            return Interfaces.Services.SiteServiceService(entry).GetGoodsReceivingDocumentLinesForAPurchaseOrderLine(entry, siteServiceProfile, purchaseOrderLineID, closeConnection);
        }

        /// <summary>
        /// Gets a list of purchase orders without a goods receiving document
        /// </summary>
        /// <param name="siteServiceProfile">Which site service to use for this operation</param>
        /// <param name="entry">The entry into the database</param>
        /// <param name="closeConnection">If true then the connection to the site service is closed as soon as the operation has finished otherwise it will stay open</param>
        /// <param name="storeID">Filter by store, use RecordIdentifier.Empty for no filter</param>
        /// <param name="sorting">the sort method</param>
        /// <param name="sortBackwards">The sort direction</param>
        /// <param name="includeLineTotals">True if the total quantity of items and total number of items should be included in the query. Used in OMNI</param>
        /// <returns></returns>
        public virtual List<PurchaseOrder> GetPurchaseOrdersWithNoGoodsReceivingDocumentForStore(
            IConnectionManager entry,
            SiteServiceProfile siteServiceProfile,
            RecordIdentifier storeID,
            PurchaseOrderSorting sorting,
            bool sortBackwards,
            bool includeLineTotals,
            bool closeConnection)
        {
            return Interfaces.Services.SiteServiceService(entry).GetPurchaseOrdersWithNoGoodsReceivingDocumentForStore(entry, siteServiceProfile, storeID, sorting, sortBackwards, includeLineTotals, closeConnection);
        }

        /// <summary>
        /// Gets a specific Goods recieveing document
        /// </summary>
        /// <param name="siteServiceProfile">Which site service to use for this operation</param>
        /// <param name="entry">The entry into the database</param>
        /// <param name="goodsReceivingDocumentID">The ID of the document</param>
        /// <param name="closeConnection">If true then the connection to the site service is closed as soon as the operation has finished otherwise it will stay open</param>
        /// <returns></returns>
        public virtual GoodsReceivingDocument GetGoodsReceivingDocument(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier goodsReceivingDocumentID, bool closeConnection)
        {
            return Interfaces.Services.SiteServiceService(entry).GetGoodsReceivingDocument(entry, siteServiceProfile, goodsReceivingDocumentID, closeConnection);
        }

        /// <summary>
        /// Gets a goods receiving document line
        /// </summary>
        /// <param name="siteServiceProfile">Which site service to use for this operation</param>
        /// <param name="entry">The entry into the database</param>
        /// <param name="goodsReceivingDocumentID">The ID of the document</param>
        /// <param name="storeID">The ID of the store</param>
        /// <param name="closeConnection">If true then the connection to the site service is closed as soon as the operation has finished otherwise it will stay open</param>
        /// <returns></returns>
        public virtual GoodsReceivingDocumentLine GetGoodsReceivingDocumentLine(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier goodsReceivingDocumentID, RecordIdentifier storeID, bool closeConnection)
        {
            return Interfaces.Services.SiteServiceService(entry).GetGoodsReceivingDocumentLine(entry, siteServiceProfile, goodsReceivingDocumentID, storeID, closeConnection);
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
        /// <param name="totalCount">Total rows available</param>
        /// <returns></returns>
        public virtual List<GoodsReceivingDocumentLine> SearchGoodsReceivingDocumentLines(IConnectionManager entry, SiteServiceProfile siteServiceProfile, 
            GoodsReceivingDocumentLineSearch searchCriteria, GoodsReceivingDocumentLineSorting sortBy, bool sortBackwards, bool closeConnection, out int totalCount)
        {
            return Interfaces.Services.SiteServiceService(entry).SearchGoodsReceivingDocumentLines(entry, siteServiceProfile, searchCriteria, sortBy, sortBackwards, closeConnection, out totalCount);
        }


        /// <summary>
        /// Posts the goods receiving line with the supplied ID
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="siteServiceProfile">Which site service to use for this operation</param>
        /// <param name="goodsReceivingDocumentLineID">The ID of the line</param>        
        /// <param name="closeConnection">If true then the connection to the site service is closed as soon as the operation has finished otherwise it will stay open</param>
        /// <returns></returns>
        public virtual GoodsReceivingPostResult PostGoodsReceivingLine(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier goodsReceivingDocumentLineID, bool closeConnection)
        {
            return Interfaces.Services.SiteServiceService(entry).PostGoodsReceivingLine(entry, siteServiceProfile, goodsReceivingDocumentLineID, closeConnection);
        }

        /// <summary>
        /// Updates the status of a given goods receiving document if necessary. Returns whether the document was updated or not.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="siteServiceProfile">Which site service to use for this operation</param>
        /// <param name="goodsReceivingDocumentID">The ID of the document</param>
        /// <param name="closeConnection">If true then the connection to the site service is closed as soon as the operation has finished otherwise it will stay open</param>
        /// <returns></returns>
        public virtual bool UpdateGoodsReceivingDocumentStatus(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier goodsReceivingDocumentID, bool closeConnection)
        {
            return Interfaces.Services.SiteServiceService(entry).UpdateGoodsReceivingDocumentStatus(entry, siteServiceProfile, goodsReceivingDocumentID, closeConnection);
        }

        /// <summary>
        /// Saves a given goods receiving document line into the database
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="siteServiceProfile">Which site service to use for this operation</param>
        /// <param name="goodsReceivingDocumentLine">The line to save</param>
        /// <param name="closeConnection">If true then the connection to the site service is closed as soon as the operation has finished otherwise it will stay open</param>
        public virtual void SaveGoodsReceivingDocumentLine(IConnectionManager entry, SiteServiceProfile siteServiceProfile, GoodsReceivingDocumentLine goodsReceivingDocumentLine, bool closeConnection)
        {
            Interfaces.Services.SiteServiceService(entry).SaveGoodsReceivingDocumentLine(entry, siteServiceProfile, goodsReceivingDocumentLine, closeConnection);
        }

        /// <summary>
        /// Deletes the supplied goods receiving document line
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="siteServiceProfile">Which site service to use for this operation</param>
        /// <param name="closeConnection">If true then the connection to the site service is closed as soon as the operation has finished otherwise it will stay open</param>
        /// <param name="goodsReceivingDocumentLineID">The line to delete</param>
        public virtual void DeleteGoodsReceivingDocumentLine(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier goodsReceivingDocumentLineID, bool closeConnection)
        {
            Interfaces.Services.SiteServiceService(entry).DeleteGoodsReceivingDocumentLine(entry, siteServiceProfile, goodsReceivingDocumentLineID, closeConnection);
        }

        public virtual void CreateGoodsReceivingDocumentLinesFromPurchaseOrder(IConnectionManager entry,
            SiteServiceProfile siteServiceProfile, RecordIdentifier purchaseOrderID, bool closeConnection)
        {
            Interfaces.Services.SiteServiceService(entry).CreateGoodsReceivingDocumentLinesFromPurchaseOrder(entry, siteServiceProfile, purchaseOrderID, closeConnection);
        }

        public virtual bool GoodsReceivingDocumentHasLines(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier goodsReceivingDocumentID, bool closeConnection)
        {
            return Interfaces.Services.SiteServiceService(entry).GoodsReceivingDocumentHasLines(entry, siteServiceProfile, goodsReceivingDocumentID, closeConnection);
        }

        /// <summary>
        /// Gets the centralized Overreceiving setting
        /// </summary>  
        /// <param name="siteServiceProfile">Which site service to use for this operation</param>
        /// <param name="entry">The entry into the database</param>
        /// <param name="closeConnection">If true then the connection to the site service is closed as soon as the operation has finished otherwise it will stay open</param>
        /// <returns></returns>
        public virtual int MaxOverGoodsReceive(IConnectionManager entry, SiteServiceProfile siteServiceProfile, bool closeConnection)
        {
            return Interfaces.Services.SiteServiceService(entry).MaxOverGoodsReceive(entry, siteServiceProfile, closeConnection);
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
            return Interfaces.Services.SiteServiceService(entry).GetGoodsReceivingDocumentLines(entry, siteServiceProfile, goodsReceivingID, closeConnection);
        }

        public virtual bool GoodReceivingDocumentLineIsPosted(IConnectionManager entry, SiteServiceProfile siteServiceProfile,
            RecordIdentifier goodsReceivingDocumentLineID, bool closeConnection)
        {
            return Interfaces.Services.SiteServiceService(entry).GoodReceivingDocumentLineIsPosted(entry, siteServiceProfile, goodsReceivingDocumentLineID, closeConnection);
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
            return Interfaces.Services.SiteServiceService(entry).GetPurchaseOrderID(entry, siteServiceProfile, goodsReceivingDocumentID, closeConnection);
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
            return Interfaces.Services.SiteServiceService(entry).GoodsReceivingDocumentFullyReceived(entry, siteServiceProfile, goodsReceivingDocumentID, closeConnection);
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
            return Interfaces.Services.SiteServiceService(entry).GoodsReceivingDocumentAllLinesPosted(entry, siteServiceProfile, goodsReceivingDocumentID, closeConnection);
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
            return Interfaces.Services.SiteServiceService(entry).PostGoodsReceivingDocument(entry, siteServiceProfile, goodsReceivingDocumentID, closeConnection);
        }
    }
}

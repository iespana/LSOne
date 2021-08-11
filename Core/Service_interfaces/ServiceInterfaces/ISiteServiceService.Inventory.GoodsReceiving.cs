using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.Inventory;
using LSOne.DataLayer.BusinessObjects.Omni;
using LSOne.DataLayer.BusinessObjects.Profiles;
using LSOne.DataLayer.DataProviders.Inventory;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;
using LSOne.Utilities.Development;

namespace LSOne.Services.Interfaces
{
    public partial interface ISiteServiceService
    {
        /// <summary>
        /// Has the document been fully recieved
        /// </summary>
        /// <param name="siteServiceProfile">Which site service to use for this operation</param>
        /// <param name="entry">The entry into the database</param>
        /// <param name="closeConnection">If true then the connection to the site service is closed as soon as the operation has finished otherwise it will stay open</param>
        /// <param name="goodsReceivingDocumentID">The ID if the document</param>
        /// <returns></returns>
        bool GoodsReceivingDocumentFullyReceived(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier goodsReceivingDocumentID, bool closeConnection);

        /// <summary>
        /// Are all the lines on the document posted
        /// </summary>
        /// <param name="siteServiceProfile">Which site service to use for this operation</param>
        /// <param name="entry">The entry into the database</param>
        /// <param name="closeConnection">If true then the connection to the site service is closed as soon as the operation has finished otherwise it will stay open</param>
        /// <param name="goodsReceivingDocumentID">The ID of the document</param>
        /// <returns></returns>
        bool GoodsReceivingDocumentAllLinesPosted(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier goodsReceivingDocumentID, bool closeConnection);

        /// <summary>
        /// Gets the purchase order ID for the goods receiving dosument
        /// </summary>
        /// <param name="siteServiceProfile">Which site service to use for this operation</param>
        /// <param name="entry">The entry into the database</param>
        /// <param name="closeConnection">If true then the connection to the site service is closed as soon as the operation has finished otherwise it will stay open</param>
        /// <param name="goodsReceivingDocumentID">The ID if the document</param>
        /// <returns></returns>
        RecordIdentifier GetPurchaseOrderID(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier goodsReceivingDocumentID, bool closeConnection);

        /// <summary>
        /// Deletes all goods receiving lines attached to a purchase order line. Use PurchaseOrderLineHasPostedGoodsReceivingDocumentLine to validate that no goods receiving lines have been posted before deleting them
        /// </summary>
        /// <param name="siteServiceProfile">Which site service to use for this operation</param>
        /// <param name="entry">The entry into the database</param>
        /// <param name="purchaseOrderLineID">The ID for the purchase order line. PrimaryID is the purchase Order ID and SecondaryID is the line number</param>
        /// <param name="closeConnection">If true then the connection to the site service is closed as soon as the operation has finished otherwise it will stay open</param>
        void DeleteGoodsReceivingLinesForAPurchaseOrderLine(IConnectionManager entry,
            SiteServiceProfile siteServiceProfile,
            RecordIdentifier purchaseOrderLineID,
            bool closeConnection);

        /// <summary>
        /// Checks if a goods receiving document exists for a purchase order
        /// </summary>
        /// <param name="siteServiceProfile">Which site service to use for this operation</param>
        /// <param name="entry">The entry into the database</param>
        /// <param name="closeConnection">If true then the connection to the site service is closed as soon as the operation has finished otherwise it will stay open</param>
        /// <param name="purchaseOrderID">The ID for the purchase order that is to be checked</param>
        /// <returns>Return true if a goods receiving document is found for this purchase order ID</returns>
        bool GoodsReceivingDocumentExists(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier purchaseOrderID, bool closeConnection);

        /// <summary>
        /// Saves a goods receiving document header
        /// </summary>
        /// <param name="siteServiceProfile">Which site service to use for this operation</param>
        /// <param name="entry">The entry into the database</param>
        /// <param name="closeConnection">If true then the connection to the site service is closed as soon as the operation has finished otherwise it will stay open</param>
        /// <param name="documentHeader">The goods receiving document header that is to be saved</param>
        void SaveGoodsReceivingDocument(IConnectionManager entry, SiteServiceProfile siteServiceProfile, GoodsReceivingDocument documentHeader, bool closeConnection);

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
        List<PurchaseOrder> GetPurchaseOrdersWithNoGoodsReceivingDocumentForStore(
            IConnectionManager entry,
            SiteServiceProfile siteServiceProfile,
            RecordIdentifier storeID,
            PurchaseOrderSorting sorting,
            bool sortBackwards,
            bool includeLineTotals,
            bool closeConnection);

        /// <summary>
        /// Creates posted goods receiving lines for all the lines in the purchase order
        /// </summary>
        /// <param name="siteServiceProfile">Which site service to use for this operation</param>
        /// <param name="entry">The entry into the database</param>
        /// <param name="closeConnection">If true then the connection to the site service is closed as soon as the operation has finished otherwise it will stay open</param>
        /// <param name="purchaseOrderID">The ID purchase order that the goods receiving lines should be created from</param>
        /// <returns>Result of the posting operation</returns>
        GoodsReceivingPostResult CreatePostedGoodsReceivingDocumentLinesFromPurchaseOrder(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier purchaseOrderID, bool closeConnection);

        /// <summary>
        /// If the Goods receiving document does not have any posted lines the document is deleted.
        /// </summary>
        /// <param name="goodsReceivingDocumentID">The document to be deleted</param>
        /// <param name="siteServiceProfile">Which site service to use for this operation</param>
        /// <param name="entry">The entry into the database</param>
        /// <param name="closeConnection">If true then the connection to the site service is closed as soon as the operation has finished otherwise it will stay open</param>
        /// <returns>Retrns <see cref="GoodsReceivingDocumentDeleteResult"/> that tells us if there were posted lines or if the document was deleted </returns>
        GoodsReceivingDocumentDeleteResult DeleteGoodsReceivingDocument(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier goodsReceivingDocumentID, bool closeConnection);

        /// <summary>
        /// Searches for all goods receiving documents that match the search criteria as it has been set.
        /// </summary>
        /// <param name="siteServiceProfile">Which site service to use for this operation</param>
        /// <param name="entry">The entry into the database</param>
        /// <param name="closeConnection">If true then the connection to the site service is closed as soon as the operation has finished otherwise it will stay open</param>
        /// <param name="searchCriteria">One or more of the variables need to be set so that the search is limited, otherwise all documents are returned</param>
        /// <returns>A list of the goods receiving documents found in the search</returns>
        List<GoodsReceivingDocument> GetGoodsReceivingDocuments(IConnectionManager entry, SiteServiceProfile siteServiceProfile, GoodsReceivingDocumentSearch searchCriteria, bool closeConnection);
        /// <summary>
        /// Gets a specific Goods recieveing document
        /// </summary>
        /// <param name="siteServiceProfile">Which site service to use for this operation</param>
        /// <param name="entry">The entry into the database</param>
        /// <param name="goodsReceivingDocumentID">The ID of the document</param>
        /// <param name="closeConnection">If true then the connection to the site service is closed as soon as the operation has finished otherwise it will stay open</param>
        /// <returns></returns>
        GoodsReceivingDocument GetGoodsReceivingDocument(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier goodsReceivingDocumentID, bool closeConnection);

        /// <summary>
        /// Gets a goods receiving document line
        /// </summary>
        /// <param name="siteServiceProfile">Which site service to use for this operation</param>
        /// <param name="entry">The entry into the database</param>
        /// <param name="goodsReceivingDocumentID">The ID of the document</param>
        /// <param name="storeID">The ID of the store</param>
        /// <param name="closeConnection">If true then the connection to the site service is closed as soon as the operation has finished otherwise it will stay open</param>
        /// <returns></returns>
        GoodsReceivingDocumentLine GetGoodsReceivingDocumentLine(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier goodsReceivingDocumentID, RecordIdentifier storeID, bool closeConnection);

        /// <summary>
        /// Retrieves the sum of all received items per goods receiving document. The list of documents is controlled by the search criteria <see cref="GoodsReceivingDocumentLineSearch"/>
        /// </summary>
        /// <param name="siteServiceProfile">Which site service to use for this operation</param>
        /// <param name="entry">The entry into the database</param>
        /// <param name="closeConnection">If true then the connection to the site service is closed as soon as the operation has finished otherwise it will stay open</param>
        /// <param name="numberOfDocuments">For how many GR documents should the total be retrieved</param>
        /// <returns></returns>
        List<InventoryTotals> GetReceivedTotals(IConnectionManager entry, SiteServiceProfile siteServiceProfile, int numberOfDocuments, bool closeConnection);

        /// <summary>
        /// Return the total number of GR documents that are in the database
        /// </summary>
        /// <param name="siteServiceProfile">Which site service to use for this operation</param>
        /// <param name="entry">The entry into the database</param>
        /// <param name="closeConnection">If true then the connection to the site service is closed as soon as the operation has finished otherwise it will stay open</param>
        /// <returns>the total number of GR documents that are in the database</returns>
        int GetTotalNumberOfGRDocuments(IConnectionManager entry, SiteServiceProfile siteServiceProfile, bool closeConnection);

        /// <summary>
        /// Returns the total number of goods receiving documents that are found using the search criteria.
        /// The GetGoodsReceivingDocuments function limits the result set so this tells us how many there are available in total
        /// </summary>
        /// <param name="siteServiceProfile">Which site service to use for this operation</param>
        /// <param name="entry">The entry into the database</param>
        /// <param name="closeConnection">If true then the connection to the site service is closed as soon as the operation has finished otherwise it will stay open</param>
        /// <param name="searchCriteria">One or more of the variables need to be set so that the search is limited, otherwise all documents are returned</param>
        /// <returns>the total number of goods receiving documents that are found using the search criteria</returns>
        int CountGoodsReceivingDocumentsSearchResults(IConnectionManager entry, SiteServiceProfile siteServiceProfile, GoodsReceivingDocumentSearch searchCriteria, bool closeConnection);

        /// <summary>
        /// Returns all goods receiving lines for a specific purchase order line
        /// </summary>
        /// <param name="siteServiceProfile">Which site service to use for this operation</param>
        /// <param name="entry">The entry into the database</param>
        /// <param name="closeConnection">If true then the connection to the site service is closed as soon as the operation has finished otherwise it will stay open</param>
        /// <param name="purchaseOrderLineID">the unique ID for the purchase order line</param>
        /// <returns>All goods receiving lines attached to one specific purchase order line</returns>
        List<GoodsReceivingDocumentLine> GetGoodsReceivingDocumentLinesForAPurchaseOrderLine(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier purchaseOrderLineID, bool closeConnection);

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
        List<GoodsReceivingDocumentLine> SearchGoodsReceivingDocumentLines(IConnectionManager entry, SiteServiceProfile siteServiceProfile, GoodsReceivingDocumentLineSearch searchCriteria,
            GoodsReceivingDocumentLineSorting sortBy, bool sortBackwards, bool closeConnection, out int totalCount);


        /// <summary>
        /// Posts the goods receiving line with the supplied ID
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="siteServiceProfile">Which site service to use for this operation</param>
        /// <param name="goodsReceivingDocumentLineID">The ID of the line</param>        
        /// <param name="closeConnection">If true then the connection to the site service is closed as soon as the operation has finished otherwise it will stay open</param>
        /// <returns></returns>
        GoodsReceivingPostResult PostGoodsReceivingLine(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier goodsReceivingDocumentLineID, bool closeConnection);

        /// <summary>
        /// Updates the status of a given goods receiving document if necessary. Returns whether the document was updated or not.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="siteServiceProfile">Which site service to use for this operation</param>
        /// <param name="goodsReceivingDocumentID">The ID of the document</param>
        /// <param name="closeConnection">If true then the connection to the site service is closed as soon as the operation has finished otherwise it will stay open</param>
        /// <returns></returns>
        bool UpdateGoodsReceivingDocumentStatus(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier goodsReceivingDocumentID, bool closeConnection);

        /// <summary>
        /// Saves a given goods receiving document line into the database
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="siteServiceProfile">Which site service to use for this operation</param>
        /// <param name="goodsReceivingDocumentLine">The line to save</param>
        /// <param name="closeConnection">If true then the connection to the site service is closed as soon as the operation has finished otherwise it will stay open</param>
        void SaveGoodsReceivingDocumentLine(IConnectionManager entry, SiteServiceProfile siteServiceProfile, GoodsReceivingDocumentLine goodsReceivingDocumentLine, bool closeConnection);

        /// <summary>
        /// Deletes the supplied goods receiving document line
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="siteServiceProfile">Which site service to use for this operation</param>
        /// <param name="closeConnection">If true then the connection to the site service is closed as soon as the operation has finished otherwise it will stay open</param>
        /// <param name="goodsReceivingDocumentLineID">The line to delete</param>
        void DeleteGoodsReceivingDocumentLine(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier goodsReceivingDocumentLineID, bool closeConnection);

        /// <summary>
        /// Create unposted goods receiving lines from purchase order lines.
        /// </summary>
        /// <param name="entry">Database connection</param>
        /// <param name="siteServiceProfile">Site service profile to use</param>
        /// <param name="purchaseOrderID">Purchase order ID from which to create the lines</param>
        /// <param name="closeConnection">If true then the connection to the site service is closed as soon as the operation has finished otherwise it will stay open</param>
        void CreateGoodsReceivingDocumentLinesFromPurchaseOrder(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier purchaseOrderID, bool closeConnection);

        /// <summary>
        /// Check if a goods receiving document contains any lines
        /// </summary>
        /// <param name="entry">Database connection</param>
        /// <param name="siteServiceProfile">Site service profile to use</param>
        /// <param name="goodsReceivingDocumentID">Goods receiving document ID to check</param>
        /// <param name="closeConnection">If true then the connection to the site service is closed as soon as the operation has finished otherwise it will stay open</param>
        /// <returns>True if the document contains any lines, false otherwise</returns>
        bool GoodsReceivingDocumentHasLines(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier goodsReceivingDocumentID, bool closeConnection);

        /// <summary>
        /// Gets the centralized Overreceiving setting
        /// </summary>  
        /// <param name="siteServiceProfile">Which site service to use for this operation</param>
        /// <param name="entry">The entry into the database</param>
        /// <param name="closeConnection">If true then the connection to the site service is closed as soon as the operation has finished otherwise it will stay open</param>
        /// <returns></returns>
        int MaxOverGoodsReceive(IConnectionManager entry, SiteServiceProfile siteServiceProfile, bool closeConnection);

        /// <summary>
        /// Gets existing lines for a specific goods receiving document
        /// </summary>
        /// <param name="siteServiceProfile">Which site service to use for this operation</param>
        /// <param name="entry">The entry into the database</param>
        /// <param name="closeConnection">If true then the connection to the site service is closed as soon as the operation has finished otherwise it will stay open</param>
        /// <param name="goodsReceivingID">The unique ID for the goods receiving document</param>
        /// <returns>A list of goods receiving lines</returns>
        List<GoodsReceivingDocumentLine> GetGoodsReceivingDocumentLines(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier goodsReceivingID, bool closeConnection);

        /// <summary>
        /// Check if a goods receiving line is posted
        /// </summary>
        /// <param name="entry">Database connection</param>
        /// <param name="siteServiceProfile">Site service profile to use</param>
        /// <param name="goodsReceivingDocumentLineID">Goods receiving line ID to check</param>
        /// <param name="closeConnection">If true then the connection to the site service is closed as soon as the operation has finished otherwise it will stay open</param>
        /// <returns>True if the line is posted, false otherwise</returns>
        bool GoodReceivingDocumentLineIsPosted(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier goodsReceivingDocumentLineID, bool closeConnection);

        /// <summary>
        /// Post all lines from a goods receiving document
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="siteServiceProfile">Which site service to use for this operation</param>
        /// <param name="goodsReceivingDocumentID">The ID of the goods receiving document to post</param>
        /// <param name="closeConnection">If true then the connection to the site service is closed as soon as the operation has finished otherwise it will stay open</param>
        /// <returns>Result of the operation</returns>
        GoodsReceivingPostResult PostGoodsReceivingDocument(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier goodsReceivingDocumentID, bool closeConnection);
    }
}

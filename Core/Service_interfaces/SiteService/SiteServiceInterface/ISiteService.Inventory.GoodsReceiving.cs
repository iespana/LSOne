using System.Collections.Generic;
using System.ServiceModel;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.Inventory;
using LSOne.DataLayer.BusinessObjects.Omni;
using LSOne.DataLayer.DataProviders.Inventory;
using LSOne.Services.Interfaces.SupportClasses;
using LSOne.Utilities.DataTypes;
using LSOne.Utilities.Development;
using LSRetail.SiteService.SiteServiceInterface.DTO;

namespace LSRetail.SiteService.SiteServiceInterface
{
    public partial interface ISiteService
    {
        /// <summary>
        /// Gets the centralized Overreceiving setting
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <returns></returns>
        [OperationContract]
        int MaxOverGoodsReceive(LogonInfo logonInfo);

        /// <summary>
        /// Deletes goods receiving lines for a specific purchase order line
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <param name="purchaseOrderLineID">The unique ID for the purchase order line</param>
        [OperationContract]
        void DeleteGoodsReceivingLinesForAPurchaseOrderLine(LogonInfo logonInfo, RecordIdentifier purchaseOrderLineID);

        /// <summary>
        /// Checks if a goods receiving document exists for a purchase order
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <param name="purchaseOrderID">The ID for the purchase order that is to be checked</param>
        /// <returns>Return true if a goods receiving document is found for this purchase order ID</returns>
        [OperationContract]
        bool GoodsReceivingDocumentExists(LogonInfo logonInfo, RecordIdentifier purchaseOrderID);

        /// <summary>
        /// Saves a goods receiving document header
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <param name="documentHeader">The goods receiving document header that is to be saved</param>
        [OperationContract]
        void SaveGoodsReceivingDocument(LogonInfo logonInfo, GoodsReceivingDocument documentHeader);


        /// <summary>
        /// Gets a list of purchase orders without a goods receiving document 
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <param name="storeID">The ID of the store</param>
        /// <param name="sorting">The sort method</param>
        /// <param name="sortBackwards">The sort direction</param>
        /// <param name="includeLineTotals">True if the total quantity of items and total number of items should be included in the query. Used in OMNI</param>
        /// <returns></returns>
        [OperationContract]
        List<PurchaseOrder> GetPurchaseOrdersWithNoGoodsReceivingDocumentForStore(LogonInfo logonInfo,
            RecordIdentifier storeID, PurchaseOrderSorting sorting, bool sortBackwards, bool includeLineTotals);

        /// <summary>
        /// Creates posted goods receiving lines for all the lines in the purchase order
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <param name="purchaseOrderID">The ID purchase order that the goods receiving lines should be created from</param>
        /// <returns>Result of the posting operation</returns>
        [OperationContract]
        GoodsReceivingPostResult CreatePostedGoodsReceivingDocumentLinesFromPurchaseOrder(LogonInfo logonInfo, RecordIdentifier purchaseOrderID);

        /// <summary>
        /// If the Goods receiving document does not have any posted lines the document is deleted.
        /// </summary>
        /// <param name="goodsReceivingDocumentID">The document to be deleted</param>
        /// <param name="logonInfo">The login information for the database</param>
        /// <returns>Retrns <see cref="GoodsReceivingDocumentDeleteResult"/> that tells us if there were posted lines or if the document was deleted </returns>
        [OperationContract]
        GoodsReceivingDocumentDeleteResult DeleteGoodsReceivingDocument(LogonInfo logonInfo, RecordIdentifier goodsReceivingDocumentID);


        /// <summary>
        /// Searches for all goods receiving documents that match the search criteria as it has been set. 
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <param name="searchCriteria">One or more of the variables need to be set so that the search is limited, otherwise all documents are returned</param>
        /// <returns>A list of the goods receiving documents found in the search</returns>
        [OperationContract]
        List<GoodsReceivingDocument> GetGoodsReceivingDocuments(LogonInfo logonInfo, GoodsReceivingDocumentSearch searchCriteria);

        /// <summary>
        /// Gets a specific Goods recieveing document
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <param name="goodsReceivingDocumentID">The ID of the document </param>
        /// <returns></returns>
        [OperationContract]
        GoodsReceivingDocument GetGoodsReceivingDocument(LogonInfo logonInfo,  RecordIdentifier goodsReceivingDocumentID);

        /// <summary>
        /// Retrieves the sum of all received items per goods receiving document. 
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <param name="numberOfDocuments">For how many GR documents should the total be retrieved</param>
        /// <returns></returns>
        [OperationContract]
        List<InventoryTotals> GetReceivedTotals(LogonInfo logonInfo, int numberOfDocuments);

        /// <summary>
        /// Return the total number of GR documents that are in the database
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <returns>the total number of GR documents that are in the database</returns>
        [OperationContract]
        int GetTotalNumberOfGRDocuments(LogonInfo logonInfo);

        /// <summary>
        /// Returns the total number of goods receiving documents that are found using the search criteria.
        /// The GetGoodsReceivingDocuments function limits the result set so this tells us how many there are available in total
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <param name="searchCriteria">One or more of the variables need to be set so that the search is limited, otherwise all documents are returned</param>
        /// <returns>the total number of goods receiving documents that are found using the search criteria</returns>
        [OperationContract]
        int CountGoodsReceivingDocumentsSearchResults(LogonInfo logonInfo, GoodsReceivingDocumentSearch searchCriteria);

        /// <summary>
        /// Returns all goods receiving lines for a specific purchase order line
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <param name="purchaseOrderLineID">the unique ID for the purchase order line</param>
        /// <returns>All goods receiving lines attached to one specific purchase order line</returns>
        [OperationContract]
        List<GoodsReceivingDocumentLine> GetGoodsReceivingDocumentLinesForAPurchaseOrderLine(LogonInfo logonInfo, RecordIdentifier purchaseOrderLineID);

        /// <summary>
        /// Returns all goods receiving lines matching search pararmeters
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <param name="searchCriteria"></param>
        /// <param name="sortBy"></param>
        /// <param name="sortBackwards"></param>
        /// <param name="totalRecords"></param>
        /// <returns>All goods receiving lines matching search pararmeters</returns>
        [OperationContract]
        List<GoodsReceivingDocumentLine> SearchGoodsReceivingDocumentLines(LogonInfo logonInfo, GoodsReceivingDocumentLineSearch searchCriteria, GoodsReceivingDocumentLineSorting sortBy, bool sortBackwards, out int totalRecords);

        /// <summary>
        /// Returns a goods receiving line
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <param name="goodsReceivingDocumentLineID">The ID of the document line</param>
        /// <param name="storeID">The ID of the store</param>
        /// <returns>All goods receiving lines matching search pararmeters</returns>
        [OperationContract]
        GoodsReceivingDocumentLine GetGoodsReceivingDocumentLine(LogonInfo logonInfo, RecordIdentifier goodsReceivingDocumentLineID, RecordIdentifier storeID);

        /// <summary>
        /// Posts the goods receiving documentline with the supplied ID
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <param name="goodsReceivingDocumentLineID">The ID of the line</param>
        /// <returns></returns>
        [OperationContract]
        GoodsReceivingPostResult PostGoodsReceivingLine(LogonInfo logonInfo, RecordIdentifier goodsReceivingDocumentLineID);

        /// <summary>
        /// Updates the status of a given goods receiving document if necessary. Returns whether the document was updated or not.
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <param name="goodsReceivingDocumentID">The ID oif the document</param>
        /// <returns></returns>
        [OperationContract]
        bool UpdateGoodsReceivingDocumentStatus(LogonInfo logonInfo, RecordIdentifier goodsReceivingDocumentID);

        /// <summary>
        /// Saves a goods receiving document line
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <param name="goodsReceivingDocumentLine">The line to save</param>
        [OperationContract]
        void SaveGoodsReceivingDocumentLine(LogonInfo logonInfo, GoodsReceivingDocumentLine goodsReceivingDocumentLine);

        /// <summary>
        /// Has the document been fully recieved
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <param name="goodsReceivingDocumentID">The ID oif the document</param>
        /// <returns></returns>
        [OperationContract]
        bool GoodsReceivingDocumentFullyReceived(LogonInfo logonInfo, RecordIdentifier goodsReceivingDocumentID);

        /// <summary>
        /// Are all the lines on the document posted
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <param name="goodsReceivingDocumentID">The ID oif the document</param>
        /// <returns></returns>
        [OperationContract]
        bool GoodsReceivingDocumentAllLinesPosted(LogonInfo logonInfo, RecordIdentifier goodsReceivingDocumentID);

        /// <summary>
        /// Gets the purchase order ID for the goods recievbing document
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <param name="goodsReceivingDocumentID">The ID oif the document</param>
        /// <returns></returns>
        [OperationContract]
        RecordIdentifier GetPurchaseOrderID(LogonInfo logonInfo, RecordIdentifier goodsReceivingDocumentID);

        /// <summary>
        /// Deletes the goods receiving document line supplied 
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <param name="goodsReceivingDocumentLineID">The ID of the line</param>
        [OperationContract]
        void DeleteGoodsReceivingDocumentLine(LogonInfo logonInfo, RecordIdentifier goodsReceivingDocumentLineID);

        /// <summary>
        /// Extracts order lines from a purchase order into a goods receiving document
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <param name="purchaseOrderID">The purchase order</param>
        [OperationContract]
        void CreateGoodsReceivingDocumentLinesFromPurchaseOrder(LogonInfo logonInfo, RecordIdentifier purchaseOrderID);

        /// <summary>
        /// Linea exist for the goods receiving document
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <param name="goodsReceivingDocumentID">The goods receiving document ID</param>
        /// <returns></returns>
        [OperationContract]
        bool GoodsReceivingDocumentHasLines(LogonInfo logonInfo, RecordIdentifier goodsReceivingDocumentID);

        /// <summary>
        /// Gets existing lines for a specific goods receiving document
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <param name="goodsReceivingID">The unique ID for the goods receiving document</param>
        /// <returns>A list of goods receiving lines</returns>
        [OperationContract]
        List<GoodsReceivingDocumentLine> GetGoodsReceivingDocumentLines(LogonInfo logonInfo, RecordIdentifier goodsReceivingID);

        /// <summary>
        /// Checks if the supplied goodes receiving document line is posted
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <param name="goodsReceivingDocumentLineID">The id of the line</param>
        /// <returns></returns>
        [OperationContract]
        bool GoodReceivingDocumentLineIsPosted(LogonInfo logonInfo, RecordIdentifier goodsReceivingDocumentLineID);

        /// <summary>
        /// Post all lines from a goods receiving document
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <param name="goodsReceivingDocumentID">The ID of the goods receiving document to post</param>
        /// <returns>Result of the operation</returns>
        [OperationContract]
        GoodsReceivingPostResult PostGoodsReceivingDocument(LogonInfo logonInfo, RecordIdentifier goodsReceivingDocumentID);
    }
}

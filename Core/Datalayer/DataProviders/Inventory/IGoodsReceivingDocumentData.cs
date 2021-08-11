using System.Collections.Generic;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.Inventory;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.DataProviders.Inventory
{
    public interface IGoodsReceivingDocumentData : IDataProvider<GoodsReceivingDocument>
    {
        /// <summary>
        /// Gets a list of all Goods Receiving Documents for a specific store. The list is sorted by the the parameter 'sort'
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="storeID">The store ID</param>
        /// <param name="sortBy">A enum that defines how the result should be sorted</param>
        /// <param name="sortBackwards">Set to true if wanting backwards sort</param>
        /// <returns>A list of all Goods Receiving Documents for a specific store</returns>
        List<GoodsReceivingDocument> GetGoodsReceivingDocumentsForStore(IConnectionManager entry, RecordIdentifier storeID, 
            GoodsReceivingDocumentSorting sortBy, bool sortBackwards);

        /// <summary>
        /// Gets a list of all Goods Receiving Documents for a specific vendor . The list is sorted by the the parameter 'sort'
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="vendorID">The ID of the vendor</param>
        /// <param name="sortBy">A enum that defines how the result should be sorted</param>
        /// <param name="sortBackwards">Set to true if wanting backwards sort</param>
        /// <returns>A list of all Goods Receiving Documents</returns>
        List<GoodsReceivingDocument> GetGoodsReceivingDocuments(IConnectionManager entry, RecordIdentifier vendorID, 
            GoodsReceivingDocumentSorting sortBy, bool sortBackwards);

        /// <summary>
        /// Gets a list of all Goods Receiving Documents . The list is sorted by the the parameter 'sort'
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="sortBy">A enum that defines how the result should be sorted</param>
        /// <param name="sortBackwards">Set to true if wanting backwards sort</param>
        /// <returns>A list of all Goods Receiving Documents</returns>
        List<GoodsReceivingDocument> GetGoodsReceivingDocuments(IConnectionManager entry, GoodsReceivingDocumentSorting sortBy, bool sortBackwards);

        bool HasPostedLines(IConnectionManager entry, RecordIdentifier goodsReceivingDocumentID);
        bool HasLines(IConnectionManager entry, RecordIdentifier goodsReceivingDocumentID);
        RecordIdentifier GetPurchaseOrderID(IConnectionManager entry, RecordIdentifier goodsReceivingDocumentID);

        /// <summary>
        /// Returns true if the purchase order has been fully received in this document
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="goodsReceivingDocumentID">The ID of the goods receiving document that is to be checked</param>
        /// <returns></returns>
        bool FullyReceived(IConnectionManager entry, RecordIdentifier goodsReceivingDocumentID);

        /// <summary>
        /// Returns true if all the lines on the goods receiving document have been posted
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="goodsReceivingDocumentID">The ID of the goods receiving document that is to be checked</param>
        /// <returns></returns>
        bool AllLinesPosted(IConnectionManager entry, RecordIdentifier goodsReceivingDocumentID);

        /// <summary>
        /// Updates the status of a given goods receiving document if necessary. Returns whether the document was updated or not.
        /// </summary>
        /// <param name="entry"></param>
        /// <param name="goodsReceivingDocumentID"></param>
        /// <returns></returns>
        bool UpdateStatus(IConnectionManager entry, RecordIdentifier goodsReceivingDocumentID);

        List<GoodsReceivingDocumentLine> GetGoodsReceivingDocumentLinesForAPurchaseOrderLine(IConnectionManager entry, RecordIdentifier purchaseOrderLineID);
        void DeleteLinesForAPurchaseOrderLine(IConnectionManager entry, RecordIdentifier purchaseOrderLineID);
        void DeleteGoodsReceivingDocumentsForPurchaseOrder(IConnectionManager entry, RecordIdentifier purchaseOrderID);

        GoodsReceivingDocument Get(IConnectionManager entry, RecordIdentifier goodsReceivingDocumentID);

        /// <summary>
        /// Returns all goods receiving documents that are found using the search criteria set in parameter <see cref="GoodsReceivingDocumentSearch"/>
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="searchCriteria">The search criteria to search by</param>
        /// <param name="sortBy">The sorting of the result set</param>
        /// <param name="sortBackwards">If true the sorting is backwards</param>
        /// <returns></returns>
        List<GoodsReceivingDocument> AdvancedSearch(IConnectionManager entry, GoodsReceivingDocumentSearch searchCriteria, GoodsReceivingDocumentSorting sortBy, bool sortBackwards);

        /// <summary>
        /// Returns the total number of goods receiving documents
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <returns>The total number of goods receiving documents</returns>
        int CountDocuments(IConnectionManager entry);

        /// <summary>
        /// Returns the total number of goods receiving documents that are found using the search criteria.
        /// The AdvancedSearch function limits the result set so this tells us how many there are available in total
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="searchCriteria">The search criteria to search by</param>
        /// <returns>the total number of goods receiving documents that are found using the search criteria</returns>
        int CountSearchResults(IConnectionManager entry, GoodsReceivingDocumentSearch searchCriteria);

        /// <summary>
        /// Post all lines from a goods receiving document
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="goodsReceivingDocumentID">The ID of the goods receiving document to post</param>
        /// <returns>Result of the operation</returns>
        GoodsReceivingPostResult PostGoodsReceivingDocument(IConnectionManager entry, RecordIdentifier goodsReceivingDocumentID);
    }
}
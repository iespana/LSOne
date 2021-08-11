using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.BusinessObjects.Inventory
{
    public class GoodsReceivingDocumentSearch
    {
        /// <summary>
        /// If the search should only return a specific amount of documents then set the limit here. 
        /// The default value is 500
        /// </summary>
        public int LimitResultTo;
        /// <summary>
        /// To search by the Goods receiving document ID
        /// </summary>
        public RecordIdentifier DocumentID;

        /// <summary>
        /// To search by the purchase order ID the GR document is attached to
        /// </summary>
        public RecordIdentifier PurchaseOrderID;

        /// <summary>
        /// To search by a description on the purchase order, document ID or purchase order ID. This is a tokanized search.
        /// </summary>
        public List<string> Description;

        /// <summary>
        /// If true then the description search string has to be at the beginning of the description otherwise the search will be "contains"
        /// </summary>
        public bool DescriptionBeginsWith;

        /// <summary>
        /// To search by a vendor 
        /// </summary>
        public RecordIdentifier VendorID;

        /// <summary>
        /// To search by the store the purchase order is for
        /// </summary>
        public RecordIdentifier StoreID;

        /// <summary>
        /// To search by a Goods receiving document status
        /// </summary>
        public GoodsReceivingStatusEnum? Status;

        /// <summary>
        /// The user can search between to dates for the Created date this is the From value
        /// </summary>
        public Date CreatedFrom;

        /// <summary>
        /// The user can search between to dates for the Created date this is the To value
        /// </summary>
        public Date CreatedTo;

        /// <summary>
        /// The user can search between to dates for the Posted date this is the From value
        /// </summary>
        public Date PostedFrom;

        /// <summary>
        /// The user can search between to dates for the Posted date this is the To value
        /// </summary>
        public Date PostedTo;

        /// <summary>
        /// True if the results should contain the total quantity of all lines and total number of lines for each document
        /// </summary>
        public bool IncludeLineTotals;

        public GoodsReceivingDocumentSearch()
        {
            CreatedFrom = Date.Empty;
            CreatedTo = Date.Empty;
            PostedFrom = Date.Empty;
            PostedTo = Date.Empty;
            Status = null;
            StoreID = RecordIdentifier.Empty;
            VendorID = RecordIdentifier.Empty;
            Description = new List<string>();
            DocumentID = RecordIdentifier.Empty;
            PurchaseOrderID = RecordIdentifier.Empty;
            IncludeLineTotals = false;
            LimitResultTo = 500;
        }

        public GoodsReceivingDocumentSearch(int limitResultTo) : this()
        {
            LimitResultTo = limitResultTo;
        }
    }
}

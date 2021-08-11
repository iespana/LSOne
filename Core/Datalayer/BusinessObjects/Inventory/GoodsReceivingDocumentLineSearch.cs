using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.Utilities.DataTypes;
using LSOne.Utilities.Expressions;
using LSOne.Utilities.Validation;

namespace LSOne.DataLayer.BusinessObjects.Inventory
{
    [DataContract]
    public class GoodsReceivingDocumentLineSearch
    {
        /// <summary>
        /// If the search should only return a specific amount of documents then set the limit here. 
        /// </summary>
        [DataMember]
        public int LimitResultTo { get; set; }

        /// <summary>
        /// The start record for paging
        /// </summary>
        [DataMember]
        public int RecordsFrom { get; set; }

        /// <summary>
        /// The end record for paging
        /// </summary>
        [DataMember]
        public int RecordsTo { get; set; }

        /// <summary>
        /// To search by the Goods receiving document ID
        /// </summary>
        [DataMember]
        [RecordIdentifierValidation(RecordIdentifier.RecordIdentifierType.String)]
        public RecordIdentifier DocumentID { get; set; }

        /// <summary>
        /// To search by the purchase order ID the GR document is attached to
        /// </summary>
        [DataMember]

        [RecordIdentifierValidation(RecordIdentifier.RecordIdentifierType.String)]
        public RecordIdentifier PurchaseOrderID { get; set; }

        /// <summary>
        /// Setting this search condition will search in Item ID, Vendor item ID, Item name and variant. The search will be tokanized
        /// </summary>
        [DataMember]
        public List<string> ItemNameSearch { get; set; }

        /// <summary>
        /// To search for the variants (tokenized)
        /// </summary>
        [DataMember]
        public List<string> VariantSearch { get; set; }


        [DataMember]
        public bool VariantSearchBeginsWith { get; set; }

        /// <summary>
        /// If true then the description search string has to be at the beginning of the description otherwise the search will be "contains"
        /// </summary>
        [DataMember]
        public bool DescriptionBeginsWith { get; set; }

        /// <summary>
        /// To search by a vendor 
        /// </summary>
        [DataMember]
        public RecordIdentifier VendorID { get; set; }

        /// <summary>
        /// To search by the store the purchase order is for
        /// </summary>
        [DataMember]
        public RecordIdentifier StoreID { get; set; }

        /// <summary>
        /// The user can search between to dates for the Received date this is the From value
        /// </summary>
        [DataMember]
        public Date ReceivedFrom { get; set; }

        /// <summary>
        /// The user can search between to dates for the Received date this is the To value
        /// </summary>
        [DataMember]
        public Date ReceivedTo { get; set; }

        /// <summary>
        /// To search either only posted or not posted lines set this field. If it's null it is ignored
        /// </summary>
        [DataMember]
        public bool? Posted { get; set; }

        /// <summary>
        /// To search by specific received quantity. If null this search criteria is ignored
        /// </summary>
        [DataMember]
        public decimal? ReceivedQuantity { get; set; }

        /// <summary>
        /// Should the search be limited to greater than, equal, or less than
        /// </summary>
        [DataMember]
        public DoubleValueOperator ReceivedQuantityOperator { get; set; }

        /// <summary>
        /// To search by specific ordered quantity. If null this search criteria is ignored
        /// </summary>
        [DataMember]
        public decimal? OrderedQuantity { get; set; }

        /// <summary>
        /// Should the search be limited to greater than, equal, or less than
        /// </summary>
        [DataMember]
        public DoubleValueOperator OrderedQuantityOperator { get; set; }

    }
}

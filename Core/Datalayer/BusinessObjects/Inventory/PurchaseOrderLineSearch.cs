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
    [KnownType(typeof(RecordIdentifier))]
    public class PurchaseOrderLineSearch
    {
        /// <summary>
        /// To search by the ID of the purchase order 
        /// </summary>
        [DataMember]
        [RecordIdentifierValidation(RecordIdentifier.RecordIdentifierType.String)]
        public RecordIdentifier PurchaseOrderID;

        /// <summary>
        /// To search for purchase orders by store ID
        /// </summary>
        [DataMember]
        [RecordIdentifierValidation(RecordIdentifier.RecordIdentifierType.String)]
        public RecordIdentifier StoreID;

        /// <summary>
        /// Setting this search condition will search in Item ID, Vendor item ID, Item name and variant. The search will be tokanized
        /// </summary>
        [DataMember]
        public List<string> ItemNameSearch;

        /// <summary>
        /// To search for the variants (tokenized)
        /// </summary>
        [DataMember]
        public List<string> VariantSearch;

        [DataMember]
        public bool VariantSearchBeginsWith;

        [DataMember]
        public string UnitName;

        [DataMember]
        public decimal? Quantity;

        /// <summary>
        /// Should the search be limited to greater than, equal, or less than
        /// </summary>
        [DataMember]
        public DoubleValueOperator QuantityOperator;

        [DataMember]
        public bool DescriptionBeginsWith;

        [DataMember]
        public bool? HasDiscount;

        [DataMember]
        public int StartRecord;

        [DataMember]
        public int EndRecord;

        [DataMember]
        public bool ShowDeleted = false;

        public PurchaseOrderLineSearch()
        {
            PurchaseOrderID = RecordIdentifier.Empty;
            StoreID = RecordIdentifier.Empty;
            ItemNameSearch = new List<string>();
            VariantSearch = new List<string>();
            UnitName = "";
            DescriptionBeginsWith = true;
            Quantity = null;
            StartRecord = 1;
            EndRecord = 500;
        }
    }
}

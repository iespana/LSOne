using LSOne.Utilities.DataTypes;
using System.Runtime.Serialization;

[assembly: ContractNamespace("http://www.lsretail.com/lsone/2017/12/entities", ClrNamespace = "LSOne.DataLayer.BusinessObjects.Inventory")]
namespace LSOne.DataLayer.BusinessObjects.Inventory
{
    [DataContract]
    public class InventoryStatus : DataEntity
    {   
        public InventoryStatus()
            : base()
        {
            ItemName = "";
            InventoryUnitDescription = "";
            InventoryUnitId = "";
            StoreName = "";
            RetrievedFromExternalSource = false;
        }

        public override RecordIdentifier ID
        {
            get
            {
                return new RecordIdentifier(ItemID, StoreID);
            }
            set
            {
                base.ID = value;
            }
        }

        [DataMember]
        public string InventoryUnitDescription { get; set; }

        [DataMember]
        public RecordIdentifier InventoryUnitId { get; set; }

        [DataMember]
        public RecordIdentifier ItemID { get; set; }

        [DataMember]
        public string ItemName { get; set; }

        [DataMember]
        public decimal InventoryQuantity { get; set; }

        public string InventoryQuantityFormatted 
        {
            get
            {
                if (QuantityLimiter != null)
                {
                    return InventoryQuantity.FormatWithLimits(QuantityLimiter);
                }
                else
                {
                    return "";
                }
            }
        }

        [DataMember]
        public DecimalLimit QuantityLimiter { get; set; }

        [DataMember]
        public RecordIdentifier StoreID { get; set; }

        [DataMember]
        public string StoreName { get; set; }

        public string VariantName { get; set; }

        [DataMember]
        public decimal ParkedQuantity { get; set; }

        [DataMember]
        public decimal InventoryOnHand { get; set; }

        /// <summary>
        /// This field is populated when the data is retrieved from an external source (ex. SAP Business One).
        /// In LS One use GetSumofOrderedItembyStore(..)
        /// </summary>
        [DataMember]
        public decimal OrderedQuantity { get; set; }

        /// <summary>
        /// This field is populated when the data is retrieved from an external source (ex. SAP Business One).
        /// In LS One use GetSumOfReservedItemByStore(..)
        /// </summary>
        [DataMember]
        public decimal ReservedQuantity { get; set; }

        //Specifies if the data was retrieved from an external source instead of LS One (ex. SAP Business One)
        [DataMember]
        public bool RetrievedFromExternalSource { get; set; }

        /// <summary>
        /// True if the inventory status contains a header item. Can happen in a chain of assembly items and inventory cannot be properly calculated.
        /// </summary>
        [DataMember]
        public bool HasHeaderItem { get; set; }
    }
}

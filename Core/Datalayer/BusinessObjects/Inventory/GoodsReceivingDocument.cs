using System;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.Utilities.DataTypes;
using LSOne.Utilities.Development;

namespace LSOne.DataLayer.BusinessObjects.Inventory
{
    /// <summary>
    /// Data entity class for a goods receiving documents
    /// </summary>
    public class GoodsReceivingDocument : DataEntity
    {
        public override RecordIdentifier ID
        {
            get
            {
                return GoodsReceivingID;
            }
            set
            {
                if (!serializing)
                {
                    throw new NotImplementedException();
                }
            }
        }

        // Currently Goods receiving documents and purchase orders share ids
        private RecordIdentifier PurchaseOrderAndGoodsReceivingID;

        public RecordIdentifier GoodsReceivingID {
            get
            {
                return PurchaseOrderAndGoodsReceivingID;
            }
            set
            {
                PurchaseOrderAndGoodsReceivingID = value;
            }
        }

        /// <summary>
        /// The ID of the purchase order the goods receiving document is attached ot
        /// </summary>
        public RecordIdentifier PurchaseOrderID
        {
            get
            {
                return PurchaseOrderAndGoodsReceivingID;
            }
            set
            {
                PurchaseOrderAndGoodsReceivingID = value;
            }
        }

        /// <summary>
        /// The current status of the goods receiving document
        /// </summary>
        public GoodsReceivingStatusEnum Status;
        /// <summary>
        /// The unique ID of the vendor sending the goods
        /// </summary>
        public RecordIdentifier VendorID;
        /// <summary>
        /// The name of the vendor sending the goods
        /// </summary>
        public string VendorName;
        /// <summary>
        /// The unique ID of the store the purchase order and goods receiving document is for
        /// </summary>
        public RecordIdentifier StoreID;
        /// <summary>
        /// The name of the store the purchase order and goods receiving document is for
        /// </summary>
        public string StoreName;

        /// <summary>
        /// The description on the purchase order that the Goods Receiving document is attached to
        /// </summary>
        public string Description;

        /// <summary>
        /// The total number of ordered items on the purchase order
        /// </summary>
        public decimal Ordered;
        /// <summary>
        /// The total number of received items on the goods receiving document
        /// </summary>
        public decimal Received;

        /// <summary>
        /// The date when the document was created
        /// </summary>
        public Date CreatedDate;

        /// <summary>
        /// The date when the last line was posted on the goods receiving document
        /// </summary>
        public Date PostedDate;

        /// <summary>
        /// The translated texts for the statuses
        /// </summary>
        public string StatusText
        {
            get
            {
                if (Status == GoodsReceivingStatusEnum.Active)
                {
                    return Properties.Resources.Active;
                }
                else
                {
                    return Properties.Resources.Posted;
                }
            }
        }

        /// <summary>
        /// Total quantity of all lines in the document. Displayed in mobile inventory.
        /// </summary>
        [LSOneUsage(CodeUsage.LSCommerce, "Displays total quantity in goods receiving documents")]
        public decimal TotalQuantity;

        /// <summary>
        /// Total number of lines in the document. Displayed in mobile inventory.
        /// </summary>
        [LSOneUsage(CodeUsage.LSCommerce, "Displays total number of lines in goods receiving documents")]
        public int NumberOfLines;

        public override string this[int index]
        {
            get
            {
                switch (index)
                {
                    case 0:
                        return (string)ID;

                    case 1:
                        return StoreName;

                    case 2:
                        return VendorName;

                    case 3:
                        return StatusText;

                    default:
                        return "";
                }
            }
        }
    }
}
 
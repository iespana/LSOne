using System;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.BusinessObjects.Inventory
{
    /// <summary>
    /// Data entity class for a goods receiving document line
    /// </summary>
    public class GoodsReceivingDocumentLine : DataEntity
    {
        public override RecordIdentifier ID
        {
            get
            {
                return new RecordIdentifier(GoodsReceivingDocumentID, new RecordIdentifier(PurchaseOrderLineNumber, LineNumber));
            }
            set
            {
                if (!serializing)
                {
                    throw new NotImplementedException();
                }
            }
        }

        //public DecimalLimit QuantityLimiter { get; internal set; }

        public RecordIdentifier GoodsReceivingDocumentID { get; set; }
        public RecordIdentifier PurchaseOrderLineNumber { get; set; }
        public RecordIdentifier LineNumber { get; set; }
        public RecordIdentifier StoreID { get; set; }
        public string StoreName { get; set; }
        public string PurchaseOrderID { get; set; }
        public decimal ReceivedQuantity { get; set; }
        public DateTime ReceivedDate { get; set; }
        public bool Posted { get; set; }

        public PurchaseOrderLine purchaseOrderLine { get; set; }

        public string FormatedReceivedQuantity
        {
            get
            {
                if (purchaseOrderLine.QuantityLimiter != null)
                {
                    return ReceivedQuantity.FormatWithLimits(purchaseOrderLine.QuantityLimiter);
                }
                else
                {
                    return "";
                }
            }
        }

    }
}
 
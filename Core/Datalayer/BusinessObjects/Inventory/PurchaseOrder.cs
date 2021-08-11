using System;
using System.Collections.Generic;
using System.Drawing;
using LSOne.DataLayer.BusinessObjects.Companies;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.Utilities.DataTypes;
using LSOne.Utilities.Development;

namespace LSOne.DataLayer.BusinessObjects.Inventory
{
    /// <summary>
    /// Data entity class for a purchase orders
    /// </summary>
    public class PurchaseOrder : DataEntity
    {

        public PurchaseOrder()
        {
            VendorID = "";
            VendorName = "";
            CurrencyCode = "";
            CurrencyDescription = "";
            Orderer = Guid.Empty;
            Dirty = false;
            HasLines = false;

            DeliveryDate = DateTime.Now;
            CreatedDate = DateTime.Now;
            TemplateID = "";
        }

        public override RecordIdentifier ID
        {
            get
            {
                return PurchaseOrderID;
            }
            set
            {
                if (!serializing)
                {
                    throw new NotImplementedException();
                }
            }
        }

        public override string Text
        {
            get
            {
                return VendorName;
            }
            set
            {
                if (!serializing)
                {
                    throw new NotImplementedException();
                }
            }
        }

        public RecordIdentifier PurchaseOrderID { get; set; }
        public string Description { get; set; }
        public string VendorID { get; set; }
        public string VendorName { get; set; }
        public PurchaseStatusEnum PurchaseStatus { get; set; }
        public DateTime DeliveryDate { get; set; }
        public RecordIdentifier CurrencyCode { get; set; }
        public string CurrencyDescription { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid Orderer { get; set; }
        public Name OrdererName { get; set; }
        public Address DeliveryAddress { get; set; }
        public RecordIdentifier StoreID { get; set; }
        public string StoreName { get; set; }
        public string CompanyName { get; set; }
        public CompanyInfo CompanyInfo { get; set; }
        public bool AddTaxToPurchaseOrder { get; internal set; }
        public decimal DefaultDiscountPercentage { get; set; }
        public decimal DefaultDiscountAmount { get; set; }
        public Address CompanyAddress { get; set; }
#if !MONO
        public Image CompanyLogo { get; set; }
#endif
        public Address VendorAddress { get; set; }
        public Date OrderingDate { get; set; }
        public string OrderingDateShortFormat
        {
            get
            {
                return OrderingDate.ToShortDateString();
            }
        }

        public string DeliveryAddressFormatted { get; set; }
        public string CompanyAddressFormatted { get; set; }
        public string VendorAddressFormatted { get; set; }
        public string OrdererNameFormatted { get; set; }

        public bool HasLines; 

        public bool Dirty { get; set; }

        /// <summary>
        /// Total quantity of all lines in the document. Displayed in mobile inventory.
        /// </summary>
        [LSOneUsage(CodeUsage.LSCommerce, "Displays total quantity in goods receiving documents")]
        public decimal TotalQuantity { get; set; }

        /// <summary>
        /// Total number of lines in the document. Displayed in mobile inventory.
        /// </summary>
        [LSOneUsage(CodeUsage.LSCommerce, "Displays total number of lines in goods receiving documents")]
        public int NumberOfLines;

        /// <summary>
        /// True if the purchase order was created from the mobile inventory app.
        /// </summary>
        public bool CreatedFromOmni { get; set; }

        /// <summary>
        /// The ID of the template used to create the purchase order
        /// </summary>
        public RecordIdentifier TemplateID { get; set; }

        /// <summary>
        /// Current processing status of the purchase order
        /// </summary>
        public InventoryProcessingStatus ProcessingStatus { get; set; }

        public string PurchaseStatusText
        {
            get
            {
                switch (PurchaseStatus)
                {
                    case PurchaseStatusEnum.Open:
                        return Properties.Resources.Open;
                    case PurchaseStatusEnum.Closed:
                        return Properties.Resources.PurchaseOrder_Posted;
                    case PurchaseStatusEnum.PartiallyRecieved:
                        return Properties.Resources.PartiallyReceived;
                    case PurchaseStatusEnum.Placed:
                        return Properties.Resources.Placed;
                    default:
                        return Properties.Resources.UnknownStatus;
                }
            }
        }

        public List<DataEntity> PurchaseStatusTextList()
        {
            List<DataEntity> list = new List<DataEntity>();
            list.Add(new DataEntity((int)PurchaseStatusEnum.Open, Properties.Resources.Open));
            list.Add(new DataEntity((int)PurchaseStatusEnum.Closed, Properties.Resources.PurchaseOrder_Posted));
            list.Add(new DataEntity((int)PurchaseStatusEnum.PartiallyRecieved, Properties.Resources.PartiallyReceived));
            list.Add(new DataEntity((int)PurchaseStatusEnum.Placed, Properties.Resources.Placed));

            return list;
        }

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
                        return PurchaseStatusText;

                    case 4:
                        return DeliveryDate.ToShortDateString();

                    default:
                        return "";
                }


            }
        }
    }
}
 
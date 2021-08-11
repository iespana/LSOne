using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.Utilities.DataTypes;
using LSOne.Utilities.Validation;

[assembly: ContractNamespace("http://www.lsretail.com/lsone/2017/12/entities", ClrNamespace = "LSOne.DataLayer.BusinessObjects.BarCodes")]
namespace LSOne.DataLayer.BusinessObjects.BarCodes
{
    [DataContract]
    [KnownType(typeof(RecordIdentifier))]
    public class BarCode : OptimizedUpdateDataEntity
    {
        /// <summary>
        /// Describes, how the barcode was entered. See member list for details.
        /// </summary>
        public enum BarcodeEntryType
        {
            /// <summary>
            /// Was the barcode scanned as a single item.
            /// </summary>
            SingleScanned = 0,

            /// <summary>
            /// Was the barcode scanned with a device capable of scannig multible items.
            /// </summary>
            MultiScanned = 0,

            /// <summary>
            /// Was the barcode manually entered.
            /// </summary>
            ManuallyEntered = 1,

            /// <summary>
            /// Selected from a button or a screen.
            /// </summary>
            Selected = 2
        }

        private RecordIdentifier itemID;
        private RecordIdentifier itemBarcodeID;
        private RecordIdentifier unitID;
        private RecordIdentifier barCodeSetupID;
        private RecordIdentifier variantID;
        private RecordIdentifier inventDimID;
        private bool showForItem;
        private bool useForPrinting;
        private bool useForInput;
        private decimal quantity;

        public BarCode()
            : base()
        {
            Initialize();
        }

        protected sealed override void Initialize()
        {
            itemID = RecordIdentifier.Empty;
            barCodeSetupID = RecordIdentifier.Empty;
            unitID = RecordIdentifier.Empty;
            itemBarcodeID = RecordIdentifier.Empty;
            variantID = RecordIdentifier.Empty;
            inventDimID = RecordIdentifier.Empty;
            showForItem = false;
            useForPrinting = false;
            useForInput = false;
            quantity = 0;

            UnitDescription = "";
            BarCodeSetupDescription = "";
        }

        /// <summary>
        /// Unique ID of the barcode
        /// </summary>
        [DataMember]
        [RecordIdentifierValidation(80, Depth = 1)]
        public RecordIdentifier ItemBarcodeID
        {
            get { return itemBarcodeID; }
            set { itemBarcodeID = value; }
        }

        [DataMember]
        [RecordIdentifierValidation(80, Depth = 1)]
        public override RecordIdentifier ID
        {
            get { return base.ID; }
            set { base.ID = value; }
        }

        [DataMember]
        [RecordIdentifierValidation(30, Depth = 1)]
        public RecordIdentifier ItemID
        {
            get { return itemID; }
            set
            {
                if (itemID != value)
                {
                    PropertyChanged("ITEMID", value);
                    itemID = value;
                }
            }
        }

        [DataMember]
        [RecordIdentifierValidation(80, Depth = 1)]
        public RecordIdentifier ItemBarCode
        {
            get { return base.ID; }
            set
            {
                if (base.ID != value)
                {
                    PropertyChanged("ITEMBARCODE", value);
                    base.ID = value;
                }
            }
        }

        [RecordIdentifierValidation(20, Depth = 1)]
        public RecordIdentifier VariantID
        {
            get { return variantID; }
            set
            {
                if (variantID != value)
                {
                    PropertyChanged("RBOVARIANTID", value);
                    variantID = value;
                }
            }
        }

        [RecordIdentifierValidation(20, Depth = 1)]
        public RecordIdentifier InventDimID
        {
            get { return inventDimID; }
            set
            {
                if (inventDimID != value)
                {
                    PropertyChanged("INVENTDIMID", value);
                    inventDimID = value;
                }
            }
        }

        public bool ShowForItem
        {
            get { return showForItem; }
            set
            {
                if (showForItem != value)
                {
                    PropertyChanged("RBOSHOWFORITEM", value);
                    showForItem = value;
                }
            }
        }

        public bool UseForPrinting
        {
            get { return useForPrinting; }
            set
            {
                if (useForPrinting != value)
                {
                    PropertyChanged("USEFORPRINTING", value);
                    useForPrinting = value;
                }
            }
        }

        public bool UseForInput
        {
            get { return useForInput; }
            set
            {
                if (useForInput != value)
                {
                    PropertyChanged("USEFORINPUT", value);
                    useForInput = value;
                }
            }
        }

        [DataMember]
        [RecordIdentifierValidation(20, Depth = 1)]
        public RecordIdentifier BarCodeSetupID
        {
            get { return barCodeSetupID; }
            set
            {
                if (barCodeSetupID != value)
                {
                    PropertyChanged("BARCODESETUPID", value);
                    barCodeSetupID = value;
                }
            }
        }

        public string BarCodeSetupDescription { get; internal set; }

        public string UnitDescription { get; internal set; }

        public decimal Quantity
        {
            get { return quantity; }
            set
            {
                if (quantity != value)
                {
                    PropertyChanged("QTY", value);
                    quantity = value;
                }
            }
        }

        public string Store { get; set; }
        public string Terminal { get; set; }
        public string ReceiptID { get; set; }
        public bool Deleted { get; set; }

        public override List<string> GetIgnoredColumns()
        {
            return new List<string> { "BARCODESETUPID", "RBOVARIANTID" };
        }

        #region Migrated from POS BarcodeInfo

        public BarcodeEntryType EntryType { get; set; }
        /// <summary>
        /// The type of item sold, i.e item,BOM,service.
        /// </summary>
        public int ItemType { get; set; }
        /// <summary>
        /// Is set true if color,style,size needs to be manually entered, else false
        /// </summary>
        public bool EnterDimensions { get; set; }
        /// <summary>
        /// The retail group of the item.
        /// </summary>
        public string ItemGroupId { get; set; }
        /// <summary>
        /// The retail departement of the item.
        /// </summary>
        public string ItemDepartmentId { get; set; }
        /// <summary>
        /// Returns true if the barcode was found, else it returns false.
        /// </summary>
        public bool Found { get; set; }
        /// <summary>
        /// The price as found in the barcode.
        /// </summary>
        public decimal BarcodePrice { get; set; }
        /// <summary>
        /// The quantity as found in the barcode.
        /// </summary>
        public decimal BarcodeQuantity { get; set; }
        /// <summary>
        /// If the barcode is for a scale item 
        /// </summary>
        public bool ScaleItem { get; set; }
        /// <summary>
        /// If set to true then zero price is allowed for the item.
        /// </summary>
        public bool ZeroPriceValid { get; set; }
        /// <summary>
        /// If set to true then sign of the quantity is changed.
        /// </summary>
        public bool QtyBecomesNegative { get; set; }
        /// <summary>
        /// If set to true then discount is not allowed for the item.
        /// </summary>
        public bool NoDiscountAllowed { get; set; }
        /// <summary>The barcode prefix as found in the mask table.</summary>
        public string Prefix { get; set; }
        /// <summary>The mask found for the barcode.</summary>
        public string MaskId { get; set; }
        /// <summary>Number of decimals as defined for the barcode, if the barcode
        /// is storing either a price or quantity.</summary>
        public int Decimals { get; set; }

        /// <summary>The specific unit ID associated with the barcode </summary>
        [DataMember]
        [RecordIdentifierValidation(20, Depth = 1)]
        public RecordIdentifier UnitID
        {
            get { return unitID; }
            set
            {
                if (unitID != value)
                {
                    PropertyChanged("UNITID", value);
                    unitID = value;
                }
            }
        }

        /// <summary>Returns true if the barcode should be blocked, else it returns false.</summary>
        public bool Blocked { get; set; }
        /// <summary>Specifies how many of the found item is sold.</summary>
        public decimal QtySold { get; set; }
        /// <summary> The date when the blocking becomes active</summary>
        public DateTime DateToBeBlocked { get; set; }
        /// <summary>The date when the item becomes active</summary>
        public DateTime DateToBeActivated { get; set; }
        /// <summary>Item description as found in the InventItemBarcode table.</summary>
        public string Description { get; set; }
        /// <summary> Whether the checkdigit in the barcode has been validated. </summary>
        public bool CheckDigitValidated { get; set; }
        /// <summary> Returns the type of barcode, i.e EAN13, UPC-E, etc. </summary>        
        public BarcodeType Type { get; set; }
        /// <summary>
        /// Internal type of a barcode None = 0,Item = 1, Customer = 2, Employee = 3, Coupon = 4, DataEntry = 5.</summary>      
        /// <seealso cref="BarcodeInternalType"/>
        public BarcodeInternalType InternalType { get; set; }
        /// <summary> Returns the type of barcode, from opos driver </summary>        
        public OPOSScanDataType BarcodeType { get; set; }
        /// <summary>
        /// The customer id found in the barcode - Internaltype = 2.
        /// </summary>
        public String CustomerId { get; set; }
        /// <summary>
        /// The loyalty card id found the barcode
        /// </summary>
        public string LoyaltyCardId { get; set; }
        /// <summary>Gets or sets the employee id found in the barcode - Internaltype = 3.</summary>
        public string EmployeeId { get; set; }
        /// <summary>The salespersons id found in the barcode.</summary>
        public string SalespersonId { get; set; }
        /// <summary>The couponId found in the barcode.</summary>
        public string CouponId { get; set; }
        /// <summary>The message found in the barcode.</summary>
        public string Message { get; set; }
        /// <summary>The timestamp when processing of the transaction started.</summary>
        public DateTime TimeStarted { get; set; }
        /// <summary>The timestamp when processing of the transaction finished.</summary>
        public DateTime TimeFinished { get; set; }
        /// <summary>The time difference between start and finish.</summary>
        public TimeSpan TimeElapsed { get; set; }
        /// <summary>The EANLicenseId.</summary>
        public string EANLicenseId { get; set; }
        /// <summary>The dataEntry of the barcode.</summary>
        public string DataEntry { get; set; }
        /// <summary>The line discount group that the item belongs to. Should be set when the item info is found. </summary>
        public string LineDiscountGroup { get; set; }
        /// <summary> The multiline discount group that the item is a part of. Should be set when the item info is found. </summary>
        public string MultiLineDiscountGroup { get; set; }
        /// <summary>Is 'true' if the item included in the calculation of a combined total discount. 
        /// Should be set when the item info is found.</summary>
        public bool IncludedInTotalDiscount { get; set; }
        /// <summary>Rules for keying in prices for the item.</summary>
        /// <seealso cref="KeyInPrices"/>
        public KeyInPrices KeyInPrice { get; set; }
        /// <summary>Rules for keying in quantities for the item.</summary>
        /// <seealso cref="KeyInQuantities"/>
        public KeyInQuantities KeyInQuantity { get; set; }
        /// <summary>The cost price of the item.</summary>
        public decimal CostPrice { get; set; }
        /// <summary>The pharmacy prescription id.</summary>
        public string PharmacyPrescriptionId { get; set; }
        /// <summary>
        /// The id of the batch that the item belongs to.
        /// </summary>
        public string BatchId { get; set; }
        /// <summary>
        /// The name of the retail item
        /// </summary>
        public string ItemName;
        /// <summary>
        /// The variant name of the retail item
        /// </summary>
        public string VariantName;
        #endregion

        public override string this[int index]
        {
            get
            {
                switch (index)
                {
                    case 0:
                        return ID.ToString();

                    case 1:
                        return (string)ItemBarCode;

                    case 2:
                        //return SizeName;
                        return "";

                    case 3:
                        //return ColorName;
                        return "";
                    case 4:
                        //return StyleName;
                        return "";
                    case 5:
                        return UnitDescription;

                    case 6:
                        return Quantity.FormatTruncated();

                    default:
                        return "";
                }                
            }
        }

        /// <summary>
        /// Indicates if the barcode represents an UPCA type 2 (random weight) barcode
        /// </summary>
        public bool IsUPCAType2()
        {
            return InternalType == BarcodeInternalType.Item && Type == Enums.BarcodeType.UPCA && Prefix == "2" && ID.StringValue.Length == 12;
        }
    }
}

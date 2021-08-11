using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.Utilities.DataTypes;
using LSOne.Utilities.ErrorHandling;
using LSOne.Utilities.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Xml.Linq;

[assembly: ContractNamespace("http://www.lsretail.com/lsone/2017/12/entities", ClrNamespace = "LSOne.DataLayer.BusinessObjects.ItemMaster")]
namespace LSOne.DataLayer.BusinessObjects.ItemMaster
{
    [DataContract]
    [KnownType(typeof(RecordIdentifier))]
    public partial class RetailItem : SimpleRetailItem
    {
        /// <summary>
        /// Determines the type of information that is being viewed. Default value is Sales
        /// </summary>
        public enum UnitTypeEnum
        {
            /// <summary>
            /// Information that have to do with inventory pricing, storage, units and etc. Not currently used
            /// </summary>
            Inventory = 0,
            /// <summary>
            /// Information that have to do with purchase pricing, units and etc. Not currently used
            /// </summary>
            Purchase = 1,
            /// <summary>
            /// Information that have to do with sale pricing, storage, units and etc.
            /// </summary>
            Sales = 2
        }

        /// <summary>
        /// Different types of configurations for entering a price for an item
        /// </summary>
        public enum KeyInPriceEnum
        {
            /// <summary>
            /// Keying in the price is not mandatory. Default configuration for a new item
            /// </summary>
            NotMandatory = 0,
            /// <summary>
            /// The cashier must key in a new price when the item is sold
            /// </summary>
            MustKeyInNewPrice = 1,
            /// <summary>
            /// The cashier must key in a higher or an equal price that the item already has
            /// </summary>
            MustKeyInHigherEqualPrice = 2,
            /// <summary>
            /// The cashier must key in a lowe ror an equal price that the item already has
            /// </summary>
            MustKeyInLowerEqualPrice = 3,
            /// <summary>
            /// The cashier cannot enter a new price
            /// </summary>
            MustNotKeyInNewPrice = 4
        }

        /// <summary>
        /// Different types of configurations for entering a quantity for an item
        /// </summary>
        public enum KeyInQuantityEnum
        {
            /// <summary>
            /// Keying in the quantity is not mandatory. Default configuration for a new item
            /// </summary>
            NotMandatory = 0,
            /// <summary>
            /// The cashier must key in a new quantity when the item is sold
            /// </summary>
            MustKeyInQuantity = 1,
            /// <summary>
            /// The cashier cannot enter a new quantity
            /// </summary>
            MustNotKeyInQuantity = 2
        }

        /// <summary>
        /// Different types of configuration for entering a serial number for the item
        /// </summary>
        public enum KeyInSerialNumberEnum
        {
            /// <summary>
            /// Serial numbers are not used when selling an item. Default configuration for a new item.
            /// </summary>
            Never = 0,
            /// <summary>
            /// The cashier must select or key in a serial number.
            /// </summary>
            MustKeyInSerialNumber = 1,
            /// <summary>
            /// Keying in the serial number is not mandatory. 
            /// </summary>
            NotMandatory = 2
        }

        bool dirty;
        private string extendedDescription;
        private string searchKeywords;
        private bool zeroPriceValid;
        private bool quantityBecomesNegative;
        private bool noDiscountAllowed;
        private KeyInPriceEnum keyInPrice;
        private bool scaleItem;
        private int tareWeight;
        private KeyInQuantityEnum keyInQuantity;
        private KeyInSerialNumberEnum keyInSerialNumber;
        private bool blockedOnPOS;
        private RecordIdentifier barCodeSetupID;
        private bool printVariantsShelfLabels;
        private bool isFuelItem;
        private string gradeID;
        private bool mustKeyInComment;
        private Date dateToBeBlocked;
        private Date dateToActivateItem;
        private decimal profitMargin;
        private bool mustSelectUOM;
        private RecordIdentifier inventoryUnitID;
        private RecordIdentifier purchaseUnitID;
        private RecordIdentifier salesUnitID;
        private decimal purchasePrice;
        private decimal salesMarkup;
        private RecordIdentifier salesLineDiscount;
        private RecordIdentifier salesMultiLineDiscount;
        private bool salesAllowTotalDiscount;
        private bool returnable;
        private bool canBeSold;
        private RecordIdentifier defaultVendorID;
        private RecordIdentifier retailGroupID;

        public RetailItem()
        {
            Initialize();            
        }

        protected sealed override void Initialize()
        {
            extendedDescription = "";
            searchKeywords = "";
            defaultVendorID = RecordIdentifier.Empty;
            zeroPriceValid = false;
            quantityBecomesNegative = false;
            noDiscountAllowed = false;
            keyInPrice = KeyInPriceEnum.NotMandatory;
            scaleItem = false;
            tareWeight = 0;
            keyInQuantity = KeyInQuantityEnum.NotMandatory;
            blockedOnPOS = false;
            barCodeSetupID = RecordIdentifier.Empty;
            printVariantsShelfLabels = false;
            isFuelItem = false;
            gradeID = "";
            mustKeyInComment = false;
            dateToBeBlocked = Date.Empty;
            dateToActivateItem = Date.Empty;
            profitMargin = 0;
            mustSelectUOM = false;
            inventoryUnitID = RecordIdentifier.Empty;
            purchaseUnitID = RecordIdentifier.Empty;
            salesUnitID = RecordIdentifier.Empty;
            purchasePrice = 0;
            salesMarkup = 0;
            salesLineDiscount = RecordIdentifier.Empty;
            salesMultiLineDiscount = RecordIdentifier.Empty;
            salesAllowTotalDiscount = true;
            SalesTaxItemGroupName = "";
            Deleted = false;
            dirty = false;
            retailGroupID = RecordIdentifier.Empty;
            retailGroupMasterID = RecordIdentifier.Empty;
            RetailDepartmentMasterID = RecordIdentifier.Empty;
            RetailDivisionMasterID = RecordIdentifier.Empty;
            headerItemID = RecordIdentifier.Empty;
            canBeSold = true;
            returnable = true;
            keyInSerialNumber = KeyInSerialNumberEnum.Never;
        }

        [RecordIdentifierValidation(30)]
        public override RecordIdentifier ID
        {
	        get 
	        { 
		        return base.ID;
	        }
	        set 
	        { 
		        base.ID = value;
	        }
        }
      
        [DataMember]
        [StringLength(60)]
        public override string Text
        {
	        get 
	        { 
		        return base.Text;
	        }
	        set 
	        {
                if (base.Text != value)
                {
                    PropertyChanged("ITEMNAME", value);
                    base.Text = value;
                }
	        }
        }

        /// <summary>
        /// The vendor that has been selected as the default vendor for the item
        /// </summary>
        [DataMember]
        [StringLength(20)]
        public RecordIdentifier DefaultVendorID
        {
            get { return defaultVendorID; }
            set
            {
                if (defaultVendorID != value)
                {
                    defaultVendorID = value;
                    PropertyChanged("DEFAULTVENDORID", value);
                }
            }
        }

        /// <summary>
        /// The long note field on the Retail item card where notes about the item can be written.
        /// </summary>
        [DataMember]
        public string ExtendedDescription
        {
            get { return extendedDescription; }
            set
            {
                if (extendedDescription != value)
                {
                    PropertyChanged("EXTENDEDDESCRIPTION", value);
                    extendedDescription = value;
                }
            }
        }

        /// <summary>
        /// A string of keywords used when searching for the item
        /// </summary>
        [DataMember]
        public string SearchKeywords
        {
            get { return searchKeywords; }
            set
            {
                if (searchKeywords != value)
                {
                    PropertyChanged("SEARCHKEYWORDS", value);
                    searchKeywords = value;
                }
            }
        }

        /// <summary>
        /// Marks the master retail item record as dirty. The record will not be saved when calling Save if 
        /// the Dirty property is not true. If Dirty is false and Save is called then sub-records might be saved
        /// if one or more Dirty flags are true.
        /// </summary>
        public bool Dirty
        {
            get { return dirty; }
            set { dirty = value; }
        }

        /// <summary>
        /// If true then the item can have a price of zero
        /// </summary>
        [DataMember]
        public bool ZeroPriceValid
        {
            get { return zeroPriceValid; }
            set
            {
                if (zeroPriceValid != value)
                {
                    PropertyChanged("ZEROPRICEVALID", value);
                    zeroPriceValid = value;
                }
            }
        }

        /// <summary>
        /// If true the quantity of the item automatically becomes negative when the item is sold
        /// </summary>
        [DataMember]
        public bool QuantityBecomesNegative
        {
            get { return quantityBecomesNegative; }
            set
            {
                if (quantityBecomesNegative != value)
                {
                    PropertyChanged("QTYBECOMESNEGATIVE", value);
                    quantityBecomesNegative = value;
                }
            }
        }

        /// <summary>
        /// If true no discounts are allowed on the item
        /// </summary>
        [DataMember]
        public bool NoDiscountAllowed
        {
            get { return noDiscountAllowed; }
            set
            {
                if (noDiscountAllowed != value)
                {
                    PropertyChanged("NODISCOUNTALLOWED", value);
                    noDiscountAllowed = value;
                }
            }
        }

        /// <summary>
        /// When the item is sold this configuration controls how or if the price can be changed manually
        /// </summary>
        [DataMember]
        public KeyInPriceEnum KeyInPrice
        {
            get { return keyInPrice; }
            set
            {
                if (keyInPrice != value)
                {
                    PropertyChanged("KEYINPRICE", value);
                    keyInPrice = value;
                }
            }
        }

        /// <summary>
        /// If true the item is a scale item and will get the quantity from a scale
        /// </summary>
        [DataMember]
        public bool ScaleItem
        {
            get { return scaleItem; }
            set
            {
                if (scaleItem != value)
                {
                    PropertyChanged("SCALEITEM", value);
                    scaleItem = value;
                }
            }
        }

        /// <summary>
        /// Packaging weight to be subtracted from weighted value
        /// </summary>
        [DataMember]
        public int TareWeight
        {
            get { return tareWeight; }
            set
            {
                if (tareWeight != value)
                {
                    PropertyChanged("TAREWEIGHT", value);
                    tareWeight = value;
                }
            }
        }

        /// <summary>
        /// When the item is sold this configuration controls how or if the quantity can be changed manually
        /// </summary>
        [DataMember]
        public KeyInQuantityEnum KeyInQuantity
        {
            get { return keyInQuantity; }
            set
            {
                if (keyInQuantity != value)
                {
                    PropertyChanged("KEYINQTY", value);
                    keyInQuantity = value;
                }
            }
        }

        /// <summary>
        /// When an item is sold this configuration controls if the cashier should select a serial number coresponding for the item
        /// </summary>
        [DataMember]
        public KeyInSerialNumberEnum KeyInSerialNumber
        {
            get { return keyInSerialNumber; }
            set
            {
                if (keyInSerialNumber != value)
                {
                    PropertyChanged("KEYINSERIALNUMBER", value);
                    keyInSerialNumber = value;
                }
            }
        }

        /// <summary>
        /// If true the item is blocked and cannot be sold at the POS
        /// </summary>
        [DataMember]
        public bool BlockedOnPOS
        {
            get { return blockedOnPOS; }
            set
            {
                if (blockedOnPOS != value)
                {
                    PropertyChanged("BLOCKEDONPOS", value);
                    blockedOnPOS = value;
                }
            }
        }

        /// <summary>
        /// The currently selected barcode setup on the retail item
        /// </summary>
        [DataMember]
        [RecordIdentifierValidation(20)]
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

        /// <summary>
        /// If true then a shelf label for the variant item shoud be printed
        /// </summary>
        [DataMember]
        public bool PrintVariantsShelfLabels
        {
            get { return printVariantsShelfLabels; }
            set
            {
                if (printVariantsShelfLabels != value)
                {
                    PropertyChanged("PRINTVARIANTSSHELFLABELS", value);
                    printVariantsShelfLabels = value;
                }
            }
        }

        /// <summary>
        /// If true then the item is a fuel item
        /// </summary>
        [DataMember]
        public bool IsFuelItem
        {
            get { return isFuelItem; }
            set
            {
                if (isFuelItem != value)
                {
                    PropertyChanged("FUELITEM", value);
                    isFuelItem = value;
                }
            }
        }

        public bool IsVariantItem => HeaderItemID != RecordIdentifier.Empty;

        public bool IsHeaderItem => ItemType == ItemTypeEnum.MasterItem;

        public bool IsServiceItem => ItemType == ItemTypeEnum.Service;

        public bool IsRetailItem => ItemType == ItemTypeEnum.Item;

        public bool IsAssemblyItem => ItemType == ItemTypeEnum.AssemblyItem;

        /// <summary>
        /// If the item is a fuel item the fuel grade needs to be set
        /// </summary>
        [DataMember]
        public string GradeID
        {
            get { return gradeID; }
            set
            {
                if (gradeID != value)
                {
                    PropertyChanged("GRADEID", value);
                    gradeID = value;
                }
            }
        }

        /// <summary>
        /// If true the cashier must enter a comment when the item is sold
        /// </summary>
        [DataMember]
        public bool MustKeyInComment
        {
            get { return mustKeyInComment; }
            set
            {
                if (mustKeyInComment != value)
                {
                    PropertyChanged("MUSTKEYINCOMMENT", value);
                    mustKeyInComment = value;
                }
            }
        }

        /// <summary>
        /// The item cannot be sold on the POS on or after this date.
        /// </summary>
        [DataMember]
        public Date DateToBeBlocked
        {
            get { return dateToBeBlocked; }
            set
            {
                if (dateToBeBlocked != Date.FromAxaptaDate(value.DateTime))
                {
                    PropertyChanged("DATETOBEBLOCKED", value);
                    dateToBeBlocked = value;
                }
            }
        }

        /// <summary>
        /// The item cannot be sold on the POS until on or after this date.
        /// </summary>
        [DataMember]
        public Date DateToActivateItem
        {
            get { return dateToActivateItem; }
            set
            {
                if (dateToActivateItem != Date.FromAxaptaDate(value.DateTime))
                {
                    PropertyChanged("DATETOACTIVATEITEM", value);
                    dateToActivateItem = value;
                }
            }
        }

        /// <summary>
        /// The profit margin of the product
        /// </summary>
        [DataMember]
        public decimal ProfitMargin
        {
            get { return profitMargin; }
            set
            {
                if (profitMargin != value)
                {
                    PropertyChanged("PROFITMARGIN", value);
                    profitMargin = value;
                }
            }
        }

        /// <summary>
        /// The description of the validation period. This property is for viewing purposes only and will not be saved
        /// </summary>
        public string ValidationPeriodDescription { get; set; }

        /// <summary>
        /// If true the cashier must select a UOM when the item is sold
        /// </summary>
        [DataMember]
        public bool MustSelectUOM
        {
            get { return mustSelectUOM; }
            set
            {
                if (mustSelectUOM != value)
                {
                    PropertyChanged("MUSTSELECTUOM", value);
                    mustSelectUOM = value;
                }
            }
        }

        /// <summary>
        /// The ID of the inventory unit
        /// </summary>
        [DataMember]
        [RecordIdentifierValidation(20)]
        public RecordIdentifier InventoryUnitID
        {
            get { return inventoryUnitID; }
            set
            {
                if (inventoryUnitID != value)
                {
                    PropertyChanged("INVENTORYUNITID", value);
                    inventoryUnitID = value;
                }
            }
        }

        /// <summary>
        /// The description of inventory unit (this property is for viewing purpose only, setting it has no meaning and will not be saved from within LS One)
        /// </summary>
        [DataMember]
        public string InventoryUnitName { get; set; }

        /// <summary>
        /// The ID of the purchase unit
        /// </summary>
        [DataMember]
        [RecordIdentifierValidation(20)]
        public RecordIdentifier PurchaseUnitID
        {
            get { return purchaseUnitID; }
            set
            {
                if (purchaseUnitID != value)
                {
                    PropertyChanged("PURCHASEUNITID", value);
                    purchaseUnitID = value;
                }
            }
        }

        /// <summary>
        /// The description of the purchase unit (this property is for viewing purpose only, setting it has no meaning and will not be saved from within LS One)
        /// </summary>
        [DataMember]
        public string PurchaseUnitName { get; set; }

        /// <summary>
        /// The ID of the sales unit
        /// </summary>
        [DataMember]
        [RecordIdentifierValidation(20)]
        public RecordIdentifier SalesUnitID
        {
            get { return salesUnitID; }
            set
            {
                if (salesUnitID != value)
                {
                    PropertyChanged("SALESUNITID", value);
                    salesUnitID = value;
                }
            }
        }

        /// <summary>
        /// The description of the sales unit (this property is for viewing purpose only, setting it has no meaning and will not be saved from within LS One)
        /// </summary>
        [DataMember]
        public string SalesUnitName { get; set; }

        /// <summary>
        /// The purchase price, based on <see cref="PurchaseUnitID">purchase unit</see> 
        /// </summary>
        /// <remarks>To compare it to SalesPrice, it must be recalculated based on <see cref="SalesUnitID"/> </remarks>
        [DataMember]
        public decimal PurchasePrice
        {
            get { return purchasePrice; }
            set
            {
                if (purchasePrice != value)
                {
                    PropertyChanged("PURCHASEPRICE", value);
                    purchasePrice = value;
                }
            }
        }
        
        /// <summary>
        /// The sales markup
        /// </summary>
        [DataMember]
        public decimal SalesMarkup
        {
            get { return salesMarkup; }
            set
            {
                if (salesMarkup != value)
                {
                    PropertyChanged("SALESMARKUP", value);
                    salesMarkup = value;
                }
            }
        }

        /// <summary>
        /// The ID of the sales line discount group the item is a part of
        /// </summary>
        [DataMember]
        [RecordIdentifierValidation(20)]
        public RecordIdentifier SalesLineDiscount
        {
            get { return salesLineDiscount; }
            set
            {
                if (salesLineDiscount != value)
                {
                    PropertyChanged("SALESLINEDISC", value);
                    salesLineDiscount = value;
                }
            }
        }

        /// <summary>
        /// The description of the line discount group (Setting this property has no meaning and will not be saved, use it as read only)
        /// </summary>
        public string SalesLineDiscountName { get; set; }

        /// <summary>
        /// The ID of the multiline discount group the item is a part of
        /// </summary>
        [DataMember]
        [RecordIdentifierValidation(20)]
        public RecordIdentifier SalesMultiLineDiscount
        {
            get { return salesMultiLineDiscount; }
            set
            {
                if (salesMultiLineDiscount != value)
                {
                    PropertyChanged("SALESMULTILINEDISC", value);
                    salesMultiLineDiscount = value;
                }
            }
        }

        /// <summary>
        /// The description of the multiline discount group (this property is for viewing purpose only, setting it has no meaning and will not be saved)
        /// </summary>
        public string SalesMultiLineDiscountName { get; set; }

        /// <summary>
        /// If true then the item can be included in a total discount
        /// </summary>
        [DataMember]
        public bool SalesAllowTotalDiscount
        {
            get { return salesAllowTotalDiscount; }
            set
            {
                if (salesAllowTotalDiscount != value)
                {
                    PropertyChanged("SALESALLOWTOTALDISCOUNT", value);
                    salesAllowTotalDiscount = value;
                }
            }
        }

        /// <summary>
        /// The description of the sales tax group this item belongs to (this property is for viewing purpose only, setting it has no meaning and will not be saved)
        /// </summary>
        public string SalesTaxItemGroupName { get; set; }
        

        /// <summary>
        /// The description of the tax group (this property is for viewing purpose only, setting it has no meaning and will not be saved)
        /// </summary>
        public string TaxItemGroupName { get; set; }

        /// <summary>
        /// The barcode setup description (this property is for viewing purpose only, setting it has no meaning and will not be saved)
        /// </summary>
        public string BarCodeSetupDescription { get; set; }

        /// <summary>
        /// The retail department description (this property is for viewing purpose only, setting it has no meaning and will not be saved)
        /// </summary>
        public string RetailDepartmentName { get; set; }

        /// <summary>
        /// The retail group description (this property is for viewing purpose only, setting it has no meaning and will not be saved)
        /// </summary>
        public string RetailGroupName { get; set; }

        /// <summary>
        /// The ID of the retail group the item belongs to (this is not the master ID of the retail group and will not be saved, only <see cref="SimpleRetailItem.RetailGroupMasterID"/> is saved)
        /// </summary>
        /// <remarks>When this value is sent to the Integration Framework via a web service call then this property will be used to look up the corresponding value for <see cref="SimpleRetailItem.RetailGroupMasterID"/> if the master ID has not been set.</remarks>
        [DataMember]
        [RecordIdentifierValidation(20)]
        public RecordIdentifier RetailGroupID
        {
            get { return retailGroupID; }
            set
            {
                if(retailGroupID != value)
                {
                    PropertyChanged("RETAILGROUPID", value);
                    retailGroupID = value;
                }
            }
        }

        /// <summary>
        /// The ID of the retail department the item belongs to (this property is for viewing/reporting purposes only and will not be saved)
        /// </summary>
        [RecordIdentifierValidation(20)]
        public RecordIdentifier RetailDepartmentID { get; internal set; }
     
        /// <summary>
        /// The retail division description (this property is for viewing purpose only, setting it has no meaning and will not be saved)
        /// </summary>
        public string RetailDivisionName { get; set; }

        /// <summary>
        /// Indicates wether this item can be returned at the POS
        /// </summary>
        [DataMember]
        public bool Returnable
        {
            get { return returnable; }
            set
            {
                if (returnable != value)
                {
                    PropertyChanged("RETURNABLE", value);
                    returnable = value;
                }
            }
        }
        
        /// <summary>
        /// Indicates wether this item can be sold at the POS
        /// </summary>
        [DataMember]
        public bool CanBeSold
        {
            get { return canBeSold; }
            set
            {
                if (canBeSold != value)
                {
                    PropertyChanged("CANBESOLD", value);
                    canBeSold = value;
                }
            }
        }

        /// <summary>
        /// Indicates if item is deleted or is a service type item. Used in situation when items will not take part of the inventory.
        /// </summary>
        public bool InventoryExcluded
        {
            get
            {
                return Deleted || ItemType == ItemTypeEnum.Service;
            }
        }

        /// <summary>
        /// Returns the cost price of the item = purchase price * conversion factor between purchase unit and sales unit.
        /// </summary>
        /// <param name="unitFactor">The unit conversion factor between purchase unit and sales unit.</param>
        /// <returns></returns>
        public decimal GetCostPrice(decimal unitFactor)
        {
            return PurchasePrice * unitFactor;
        }

        /// <summary>
        /// The production time of an item. This tells how long it takes to cook and prepare the item.
        /// </summary>
        [DataMember]
        public int ProductionTime { get; set; }

        // TODO: What does this do and is this even used ?
        public override object this[string field]
        {
            get
            {
                if (field == "BarCodeSetupID")
                {
                    return BarCodeSetupID;
                }

                return base[field];
            }
            set
            {
                if (field == "BarCodeSetupID")
                {
                    BarCodeSetupID = (RecordIdentifier)value;
                }
            }
        }

        /// <summary>
        /// Get the creation date
        /// </summary>
        public DateTime Created { get; private set; }

        /// <summary>
        /// Get the last modified date
        /// </summary>
        public DateTime Modified { get; private set; }

        public override void ToClass(XElement element, IErrorLog errorLogger = null)
        {
            IEnumerable<XElement> elements = element.Elements();
            foreach (XElement current in elements)
            {
                if (!current.IsEmpty)
                {
                    try
                    {
                        switch (current.Name.ToString())
                        {
                            case "itemId":
                                ID = current.Value;
                                break;
                            case "itemName":
                                Text = current.Value;
                                break;
                            case "nameAlias":
                                nameAlias = current.Value;
                                break;
                            case "itemType":
                                itemType = (ItemTypeEnum)Convert.ToInt32(current.Value);
                                break;
                            case "dateToBeBlocked":
                                dateToBeBlocked = new Date(Date.XmlStringToDateTime(current.Value));
                                break;
                            case "barCodeSetupID":
                                barCodeSetupID = current.Value;
                                break;
                            case "barCodeSetupDescription":
                                BarCodeSetupDescription = current.Value;
                                break;
                            case "scaleItem":
                                scaleItem = current.Value == "true";
                                break;
                            case "tareWeight":
                                tareWeight = Convert.ToInt32(current.Value, XmlCulture);
                                break;
                            case "keyInPrice":
                                keyInPrice = (KeyInPriceEnum)Convert.ToInt32(current.Value);
                                break;
                            case "keyInSerialNumber":
                                keyInSerialNumber = (KeyInSerialNumberEnum)Convert.ToInt32(current.Value);
                                break;
                            case "keyInQuantity":
                                keyInQuantity = (KeyInQuantityEnum)Convert.ToInt32(current.Value);
                                break;
                            case "mustKeyInComment":
                                mustKeyInComment = current.Value == "true";
                                break;
                            case "mustSelectUOM":
                                mustSelectUOM = current.Value == "true";
                                break;
                            case "zeroPriceValid":
                                zeroPriceValid = current.Value == "true";
                                break;
                            case "quantityBecomesNegative":
                                quantityBecomesNegative = current.Value == "true";
                                break;
                            case "noDiscountAllowed":
                                noDiscountAllowed = current.Value == "true";
                                break;
                            case "dateToActivateItem":
                                dateToActivateItem = new Date(Date.XmlStringToDateTime(current.Value));
                                break;
                            case "isFuelItem":
                                isFuelItem = current.Value == "true";
                                break;
                            case "gradeID":
                                gradeID = current.Value;
                                break;
                            case "printVariantsShelfLabels":
                                printVariantsShelfLabels = current.Value == "true";
                                break;
                            case "defaultVendorID":
                                defaultVendorID = current.Value;
                                break;
                            case "validationPeriod":
                                validationPeriodID = current.Value;
                                break;

                            case "retailDepartmentID":
                                RetailDepartmentID = current.Value;
                                break;
                            case "retailGroupID":
                                RetailGroupID = current.Value;
                                break;
                            case "retailDepartmentName":
                                RetailDepartmentName = current.Value;
                                break;
                            case "retailGroupName":
                                RetailGroupName = current.Value;
                                break;
                            case "profitMargin":
                                profitMargin = Convert.ToDecimal(current.Value, XmlCulture);
                                break;
                            case "validationPeriodDescription":
                                ValidationPeriodDescription = current.Value;
                                break;
                            case "blockedOnPOS":
                                blockedOnPOS = current.Value == "true";
                                break;
                            case "CustomFields":
                                CustomFieldsToClass(current);
                                break;

                            // From the old module class
                            case "salesPriceIncludingTax":
                                salesPriceIncludingTax = Convert.ToDecimal(current.Value, XmlCulture);
                                break;

                            case "salesLineDiscount":
                                salesLineDiscount = current.Value;
                                break;
                            case "salesLineDiscountName":
                                SalesLineDiscountName = current.Value;
                                break;
                            case "salesMarkup":
                                salesMarkup = Convert.ToDecimal(current.Value, XmlCulture);
                                break;
                            case "salesMultilineDiscount":
                                salesMultiLineDiscount = current.Value;
                                break;
                            case "salesMultiLineDiscountName":
                                SalesMultiLineDiscountName = current.Value;
                                break;
                            case "salesPrice":
                                salesPrice = Convert.ToDecimal(current.Value, XmlCulture);
                                break;
                            case "purchasePrice":
                                purchasePrice = Convert.ToDecimal(current.Value, XmlCulture);
                                break;
                            case "salesTaxItemGroupID":
                                salesTaxItemGroupID = current.Value;
                                break;
                            case "salesTaxItemGroupName":
                                SalesTaxItemGroupName = current.Value;
                                break;
                            case "salesAllowTotalDiscount":
                                salesAllowTotalDiscount = (current.Value == "true");
                                break;
                            case "salesUnitID":
                                salesUnitID = current.Value;
                                break;
                            case "purchaseUnitID":
                                purchaseUnitID = current.Value;
                                break;
                            case "inventoryUnitID":
                                inventoryUnitID = current.Value;
                                break;

                            case "salesUnitName":
                                SalesUnitName = current.Value;
                                break;

                            case "purchaseUnitName":
                                PurchaseUnitName = current.Value;
                                break;

                            case "inventoryUnitName":
                                InventoryUnitName = current.Value;
                                break;

                            case "created":
                                Created = Conversion.XmlStringToDateTime(current.Value);
                                break;
                            case "modified":
                                Modified = Conversion.XmlStringToDateTime(current.Value);
                                break;
                        }
                    }
                    catch (Exception ex)
                    {
                        if (errorLogger != null)
                        {
                            errorLogger.LogMessage(LogMessageType.Error, current.Name.ToString(), ex);
                        }
                    }
                }
            }
        }

        public override XElement ToXML(IErrorLog errorLogger = null)
        {
            XElement xml = new XElement("RetailItem",
                    new XElement("itemId", (string)ID),
                    new XElement("itemName", Text),
                    new XElement("nameAlias", NameAlias),
                    new XElement("itemType", (int)ItemType),
                    new XElement("dateToBeBlocked", new Date(DateToBeBlocked).ToXmlString()),
                    new XElement("barCodeSetupID", (string) BarCodeSetupID),
                    new XElement("barCodeSetupDescription", BarCodeSetupDescription),
                    new XElement("scaleItem", ScaleItem),
                    new XElement("tareWeight", TareWeight),
                    new XElement("keyInPrice", (int)KeyInPrice),
                    new XElement("keyInSerialNumber",(int)KeyInSerialNumber),
                    new XElement("keyInQuantity", (int)KeyInQuantity),
                    new XElement("mustKeyInComment", MustKeyInComment),
                    new XElement("mustSelectUOM", MustSelectUOM),
                    new XElement("zeroPriceValid", ZeroPriceValid),
                    new XElement("quantityBecomesNegative", QuantityBecomesNegative),
                    new XElement("noDiscountAllowed", NoDiscountAllowed),
                    new XElement("dateToActivateItem", new Date(DateToActivateItem).ToXmlString()),
                    new XElement("isFuelItem", IsFuelItem),
                    new XElement("gradeID", GradeID),
                    new XElement("printVariantsShelfLabels", PrintVariantsShelfLabels),
                    new XElement("defaultVendorID", (string)DefaultVendorID),
                    new XElement("validationPeriod", (string)ValidationPeriodID),

                    new XElement("retailDepartmentID", (string)RetailDepartmentID),
                    new XElement("retailGroupID", (string)RetailGroupID),
                    new XElement("retailDepartmentName", RetailDepartmentName),
                    new XElement("retailGroupName", RetailGroupName),
                    new XElement("noDiscountAllowed", NoDiscountAllowed),
                    new XElement("profitMargin", ProfitMargin.ToString(XmlCulture)),
                    new XElement("validationPeriodDescription", ValidationPeriodDescription),
                    new XElement("blockedOnPOS", BlockedOnPOS),
                    new XElement("salesPriceIncludingTax", SalesPriceIncludingTax.ToString(XmlCulture)),
                    new XElement("salesLineDiscount", (string)SalesLineDiscount),
                    new XElement("salesLineDiscountName", SalesLineDiscount),
                    new XElement("salesMarkup", SalesMarkup.ToString(XmlCulture)),
                    new XElement("salesMultilineDiscount", (string)SalesMultiLineDiscount),
                    new XElement("salesMultiLineDiscountName", SalesMultiLineDiscountName),      
                    new XElement("salesPrice", SalesPrice.ToString(XmlCulture)),
                    new XElement("purchasePrice", PurchasePrice.ToString(XmlCulture)),
                    new XElement("salesTaxItemGroupID", (string)SalesTaxItemGroupID),             
                    new XElement("salesTaxItemGroupName", SalesTaxItemGroupName), 
                    new XElement("salesAllowTotalDiscount", SalesAllowTotalDiscount), 
                    new XElement("salesUnitID", (string)SalesUnitID), 
                    new XElement("purchaseUnitID", (string)PurchaseUnitID), 
                    new XElement("inventoryUnitID", (string)InventoryUnitID), 
                    new XElement("salesUnitName", SalesUnitName),       
                    new XElement("purchaseUnitName", PurchaseUnitName),   
                    new XElement("inventoryUnitName", InventoryUnitName),
                    new XElement("created", Conversion.ToXmlString(Created)),
                    new XElement("modified", Conversion.ToXmlString(Modified)));

            CustomFieldsToXML(xml);

            return xml;
        }

        public override List<string> GetIgnoredColumns()
        {
            return new List<string> { "DEFAULTVENDORID", "BARCODESETUPID", "SALESLINEDISC", "SALESMULTILINEDISC"};
        }

        protected override void PropertyChanged(string columnName, object value = null)
        {
            base.PropertyChanged(columnName, value);

            AddColumnInfo("MODIFIED");
        }
    }
}
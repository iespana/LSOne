using LSOne.DataLayer.BusinessObjects.ItemMaster.ListItems;
using LSOne.Utilities.DataTypes;
using LSOne.Utilities.ErrorHandling;
using LSOne.Utilities.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using LSOne.DataLayer.BusinessObjects.Enums;

namespace LSOne.DataLayer.BusinessObjects.ItemMaster
{
    public partial class RetailItemNew : SimpleRetailItem
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

        public RetailItemNew() 
            :base()
        {
            ExtendedDescription = "";
            VariantName = "";
            ItemType = ItemTypeEnum.NA;
            DefaultVendorID = "";
            NameAlias = "";
            RetailGroupID = "";
            RetailDepartmentID = "";
            RetailDivisionID = "";
            ZeroPriceValid = false;
            NoDiscountAllowed = false;
            KeyInPrice = KeyInPriceEnum.NotMandatory;
            BarCodeSetupID = RecordIdentifier.Empty;
            IsFuelItem = false;
            GradeID = "";
            MustKeyInComment = false;
            DateToBeBlocked = Date.Empty;
            DateToActivateItem = Date.Empty;
            InventoryUnitID = "";
            PurchaseUnitID = "";
            SalesUnitID = "";
            SalesLineDiscount = "";
            SalesMultiLineDiscount = "";
            SalesAllowTotalDiscount = true;
            SalesTaxItemGroupID = "";
            SalesTaxItemGroupName = "";
            Deleted = false;
        }
        
        [RecordIdentifierValidation(20)]
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

        [RecordIdentifierValidation(Utilities.DataTypes.RecordIdentifier.RecordIdentifierType.Guid)]
        public  RecordIdentifier MasterID
        {
	        get;
            set;
        }

        [RecordIdentifierValidation(Utilities.DataTypes.RecordIdentifier.RecordIdentifierType.Guid)]
        public RecordIdentifier HeaderItemID
        {
	        get;
	        set;
        }

        [StringLength(60)]
        public override string Text
        {
	            get 
	        { 
		            return base.Text;
	        }
	            set 
	        { 
		        base.Text = value;
	        }
        }

        [StringLength(60)]
        public string VariantName { get; set; }


        /// <summary>
        /// The vendor that has been selected as the default vendor for the item
        /// </summary>
        [StringLength(20)]
        public RecordIdentifier DefaultVendorID { get; set; }



        /// <summary>
        /// The long note field on the Retail item card where notes about the item can be written.
        /// </summary>
        public string ExtendedDescription
        {
            get;
            set;
        }

    
        /// <summary>
        /// If true then the item can have a price of zero
        /// </summary>
        public bool ZeroPriceValid { get; set;}

        /// <summary>
        /// If true the quantity of the item automatically becomes negative when the item is sold
        /// </summary>
        public bool QuantityBecomesNegative{ get; set;}

        /// <summary>
        /// If true no discounts are allowed on the item
        /// </summary>
        public bool NoDiscountAllowed;

        /// <summary>
        /// When the item is sold this configuration controls how or if the price can be changed manually
        /// </summary>
        public KeyInPriceEnum KeyInPrice { get; set;}

        /// <summary>
        /// If true the item is a scale item and will get the quantity from a scale
        /// </summary>
        public bool ScaleItem  { get; set;}

        /// <summary>
        /// When the item is sold this configuration controls how or if the quantity can be changed manually
        /// </summary>
        public KeyInQuantityEnum KeyInQuantity { get; set;}

        public bool BlockedOnPOS { get; set; }

        /// <summary>
        /// The currently selected barcode setup on the retail item
        /// </summary>
        [RecordIdentifierValidation(20)]
        public RecordIdentifier BarCodeSetupID { get; set; }

        public bool PrintVariantsShelfLabels { get; set; }

        /// <summary>
        /// If true then the item is a fuel item
        /// </summary>
        public bool IsFuelItem { get; set; }

        /// <summary>
        /// If the item is a fuel item the fuel grade needs to be set
        /// </summary>
        public string GradeID { get; set; }

        /// <summary>
        /// If true the cashier must enter a comment when the item is sold
        /// </summary>
        public bool MustKeyInComment { get; set; }

        /// <summary>
        /// The item cannot be sold on the POS on or after this date.
        /// </summary>
        public Date DateToBeBlocked  { get; set; }

        /// <summary>
        /// The item cannot be sold on the POS until on or after this date.
        /// </summary>
        public Date DateToActivateItem { get; set; }

        /// <summary>
        /// The profit margin of the product
        /// </summary>
        public decimal ProfitMargin { get; set; }

        public string ValidationPeriodDescription { get; set; }

        /// <summary>
        /// If true the cashier must select a UOM when the item is sold
        /// </summary>
        public bool MustSelectUOM { get; set; }


        [RecordIdentifierValidation(20)]
        public RecordIdentifier InventoryUnitID { get; set; }

        /// <summary>
        /// The description of inventory unit (this property is for viewing purpose only, setting it has no meaning and will not be saved)
        /// </summary>
        public string InventoryUnitName { get; set; }

        [RecordIdentifierValidation(20)]
        public RecordIdentifier PurchaseUnitID { get; set; }

        /// <summary>
        /// The description of the purchase unit (this property is for viewing purpose only, setting it has no meaning and will not be saved)
        /// </summary>
        public string PurchaseUnitName { get; set; }
        
        [RecordIdentifierValidation(20)]
        public RecordIdentifier SalesUnitID { get; set; }

        /// <summary>
        /// The description of the sales unit (this property is for viewing purpose only, setting it has no meaning and will not be saved)
        /// </summary>
        public string SalesUnitName { get; set; }

        public decimal PurchasePrice { get; set; }
        public decimal PurchasePriceIncludingTax { get; set; }
        public decimal PurchasePriceUnit { get; set; }
        public decimal PurchaseMarkup { get; set; }

        public decimal SalesPrice { get; set; }
        public decimal SalesPriceIncludingTax { get; set; }
        public decimal SalesPriceUnit { get; set; }
        public decimal SalesMarkup { get; set; }

        /// <summary>
        /// The description of the line discount group
        /// </summary>
        [RecordIdentifierValidation(20)]
        public RecordIdentifier SalesLineDiscount { get; set; }

        /// <summary>
        /// The description of the line discount group (Setting this property has no meaning and will not be saved, use it as read only)
        /// </summary>
        public string SalesLineDiscountName { get; set; }

        /// <summary>
        /// The ID of the multiline discount group the item is a part of
        /// </summary>
        [RecordIdentifierValidation(20)]
        public RecordIdentifier SalesMultiLineDiscount { get; set; }

        public string SalesMultiLineDiscountName { get; set; }

        /// <summary>
        /// If true then the item can be included in a total discount
        /// </summary>
        public bool SalesAllowTotalDiscount { get; set; }

        /// <summary>
        /// The ID of the tax group the item belongs to
        /// </summary>
        [RecordIdentifierValidation(20)]
        public RecordIdentifier SalesTaxItemGroupID { get; set; }

        public string SalesTaxItemGroupName { get; set; }

        public bool Deleted { get; set; }

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
        /// The retail division description (this property is for viewing purpose only, setting it has no meaning and will not be saved)
        /// </summary>
        public string RetailDivisionName { get; set; }

        /// <summary>
        /// The retail group description (this property is for viewing purpose only, setting it has no meaning and will not be saved)
        /// </summary>
        public string RetailGroupName { get; set; }

                        

        // WTF We will want to investigate this one down the road
        public override object this[string field]
        {
            get
            {
                if (field == "BarCodeSetupID")
                {
                    return BarCodeSetupID;
                }

                return null;
            }
            set
            {
                if (field == "BarCodeSetupID")
                {
                    BarCodeSetupID = (RecordIdentifier)value;
                }
            }
        }

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
                                NameAlias = current.Value;
                                break;
                            case "itemType":
                                ItemType = (ItemTypeEnum)Convert.ToInt32(current.Value);
                                break;
                            case "dateToBeBlocked":
                                DateToBeBlocked = new Date(Date.XmlStringToDateTime(current.Value));
                                break;
                            case "barCodeSetupID":
                                BarCodeSetupID = current.Value;
                                break;
                            case "barCodeSetupDescription":
                                BarCodeSetupDescription = current.Value;
                                break;
                            case "scaleItem":
                                ScaleItem = current.Value == "true";
                                break;
                            case "keyInPrice":
                                KeyInPrice = (KeyInPriceEnum)Convert.ToInt32(current.Value);
                                break;
                            case "keyInQuantity":
                                KeyInQuantity = (KeyInQuantityEnum)Convert.ToInt32(current.Value);
                                break;
                            case "mustKeyInComment":
                                MustKeyInComment = current.Value == "true";
                                break;
                            case "mustSelectUOM":
                                MustSelectUOM = current.Value == "true";
                                break;
                            case "zeroPriceValid":
                                ZeroPriceValid = current.Value == "true";
                                break;
                            case "quantityBecomesNegative":
                                QuantityBecomesNegative = current.Value == "true";
                                break;
                            case "noDiscountAllowed":
                                NoDiscountAllowed = current.Value == "true";
                                break;
                            case "dateToActivateItem":
                                DateToActivateItem = new Date(Date.XmlStringToDateTime(current.Value));
                                break;
                            case "isFuelItem":
                                IsFuelItem = current.Value == "true";
                                break;
                            case "gradeID":
                                GradeID = current.Value;
                                break;
                            case "printVariantsShelfLabels":
                                PrintVariantsShelfLabels = current.Value == "true";
                                break;
                            case "defaultVendorID":
                                DefaultVendorID = current.Value;
                                break;
                            case "validationPeriod":
                                ValidationPeriodID = current.Value;
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
                                ProfitMargin = Convert.ToDecimal(current.Value, XmlCulture);
                                break;
                            case "validationPeriodDescription":
                                ValidationPeriodDescription = current.Value;
                                break;
                            case "blockedOnPOS":
                                BlockedOnPOS = current.Value == "true";
                                break;
                            case "CustomFields":
                                CustomFieldsToClass(current);
                                break;

                            // From the old module class
                            case "salesPriceIncludingTax":
                                SalesPriceIncludingTax = Convert.ToDecimal(current.Value, XmlCulture);
                                break;

                            case "salesLineDiscount":
                                SalesLineDiscount = current.Value;
                                break;
                            case "salesLineDiscountName":
                                SalesLineDiscountName = current.Value;
                                break;
                            case "salesMarkup":
                                SalesMarkup = Convert.ToDecimal(current.Value, XmlCulture);
                                break;
                            case "purchaseMarkup":
                                PurchaseMarkup = Convert.ToDecimal(current.Value, XmlCulture);
                                break;
                            case "salesMultilineDiscount":
                                SalesMultiLineDiscount = current.Value;
                                break;
                            case "salesMultiLineDiscountName":
                                SalesMultiLineDiscountName = current.Value;
                                break;
                            case "salesPrice":
                                SalesPrice = Convert.ToDecimal(current.Value, XmlCulture);
                                break;
                            case "purchasePrice":
                                PurchasePrice = Convert.ToDecimal(current.Value, XmlCulture);
                                break;
                            case "purchasePriceUnit":
                                PurchasePriceUnit = Convert.ToDecimal(current.Value, XmlCulture);
                                break;
                            case "salesPriceUnit":
                                SalesPriceUnit = Convert.ToDecimal(current.Value, XmlCulture);
                                break;
                            case "salesTaxItemGroupID":
                                SalesTaxItemGroupID = current.Value;
                                break;
                            case "salesTaxItemGroupName":
                                SalesTaxItemGroupName = current.Value;
                                break;
                            case "salesAllowTotalDiscount":
                                SalesAllowTotalDiscount = (current.Value == "true");
                                
                                break;
                            case "salesUnitID":
                                SalesUnitID = current.Value;
                                break;
                            case "purchaseUnitID":
                                PurchaseUnitID = current.Value;
                                break;
                            case "inventoryUnitID":
                                InventoryUnitID = current.Value;
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
                        }
                    }
                    catch (Exception ex)
                    {
                        if (errorLogger != null)
                        {
                            errorLogger.LogMessage(LogMessageType.Error, current.Name.ToString(),
                                                   ex);
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
                    new XElement("keyInPrice", (int)KeyInPrice),
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
                    new XElement("purchaseMarkup", PurchaseMarkup.ToString(XmlCulture)),
                    new XElement("salesMultilineDiscount", (string)SalesMultiLineDiscount),
                    new XElement("salesMultiLineDiscountName", SalesMultiLineDiscountName),      
                    new XElement("salesPrice", SalesPrice.ToString(XmlCulture)),      
                    new XElement("purchasePrice", PurchasePrice.ToString(XmlCulture)),  
                    new XElement("purchasePriceUnit", PurchasePriceUnit.ToString(XmlCulture)),  
                    new XElement("salesPriceUnit", PurchasePriceUnit.ToString(XmlCulture)),  
                    new XElement("salesTaxItemGroupID", (string)SalesTaxItemGroupID),             
                    new XElement("salesTaxItemGroupName", SalesTaxItemGroupName), 
                    new XElement("salesAllowTotalDiscount", SalesAllowTotalDiscount), 
                    new XElement("salesUnitID", (string)SalesUnitID), 
                    new XElement("purchaseUnitID", (string)PurchaseUnitID), 
                    new XElement("inventoryUnitID", (string)InventoryUnitID), 
                    new XElement("salesUnitName", SalesUnitName),       
                    new XElement("purchaseUnitName", PurchaseUnitName),   
                    new XElement("inventoryUnitName", InventoryUnitName));
                        

            CustomFieldsToXML(xml);

            return xml;
        }
         
    }
}

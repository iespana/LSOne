using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.Tax;
using LSOne.DataLayer.BusinessObjects.Transactions.Line;
using LSOne.DataLayer.TransactionObjects.DiscountItems;
using LSOne.DataLayer.TransactionObjects.Line.CustomerOrder;
using LSOne.Services.Interfaces;
using LSOne.Services.Interfaces.Enums;
using LSOne.Services.Interfaces.SupportClasses.Price;
using LSOne.Services.Interfaces.SupportInterfaces;
using LSOne.Utilities.DataTypes;
using LSOne.Utilities.ErrorHandling;

namespace LSOne.DataLayer.TransactionObjects.Line
{
    [Serializable]
    public abstract class BaseSaleItem : LineItem, IBaseSaleItem
    {
        #region Member variables
        //The transaction the item belongs to
        private RetailTransaction transaction;
        //Item 
        private string itemId = "";                 //The item id as stored in the ERP system
        private Guid headerItemID;
        private Guid masterID;
        private string variantName = "";
        private ItemTypeEnum itemType;                 //The type of item sold, i.e item,BOM,service.
        private string barcodeId = "";              //The barcode as stored in the ERP system
        private string itemGroupId = "";            //The retail group of the item.
        private string itemDepartmentId = "";       //The retail departement of the item.
        private string retailItemGroupId = "";      //The retail group of the item.
        //Price
        private decimal price;                      //Price excluding tax
        private decimal priceWithTax;               //Price including tax
        private decimal originalDiscountVoucherPriceWithTax;
        private decimal costPrice;                  //The cost price of the item.
        private string priceID;                     //The ID of the trade agreement
        private TradeAgreementPriceType priceType;  //The type of the trade agreement
        private decimal priceOverrideAmount;        //The amount that the cashier entered for the price override.
        private bool priceOverridden;                //Is set to true if price was manually overrided.
        private decimal originalPrice;              //Stores the original price excluding the tax, if price was manually overrided.
        private decimal originalPriceWithTax;       //Stores the original price including the tax, if price was manually overrided.
        private decimal standardRetailPrice;        //The standard retail price.  Used to store a retail price for comparison with the customer price.
        private decimal standardRetailPriceWithTax; //The standard retail price uncluding the tax.  Used to store a retail price for comparison with the customer price.
        private decimal priceUnit;                  //If a price is given for a for more than one unit the price unit tells how many.
        private decimal priceUnitWithTax;           //If a price is given for a for more than one unit the price unit tells how many.
        private bool noPriceCalculation;            //The price cannot be calculated - always use the price as is set

        //If the price if given for six units (priceUnit=6) as 100 EUR, then when 12 units are sold the price will be 200 EUR.
        //Discount
        private decimal totalDiscount;              //Total discount given in this line minus the linediscount (tax exclued).
        private decimal totalDiscountWithTax;       //Total discount given in this line minus the linediscount (tax included).
        private decimal totalPctDiscount;           //The percentage discount given in this line excluding the line discount.
        private decimal lineDiscount;               //Total line discount given in this transaction(tax exluded). 
        private decimal lineDiscountWithTax;        //Total line discount given in this transaction(tax inclued). 
        private decimal linePctDiscount;            //The percentage discount given in this line excluding the total discount.
        private bool noDiscountAllowed;             //Is true if the item should never be discounted. Should be set when the item info is found.
        private bool noManualDiscountAllowed;       //Is true if the item should never be allowed a manual discount.
        private bool discountUnsuccessfullyApplied; //Is true if the item was supposed to be discounted but the noDiscountAllowed property did not allow it.
        private decimal customerPrice;              //Price that is set for the customer only - Trade Agreements
        private decimal promotionPrice;             //Promotion price if it exists
        private decimal basicPrice;                 //Price from the item card
        private decimal tradeAgreementPrice;        //Price from the Trade Agreements that does not come from a customer relation
        //Customer discount
        private decimal customerDiscount;           //Total customer discount given in this transaction (tax excluded).
        private decimal customerDiscountWithTax;     //Total customer discount given in this transaction (tax included).
        //Group Discount information
        private string lineDiscountGroup;           //The line discount group that the item belongs to. Should be set when the item info is found.
        private string multiLineDiscountGroup;      //The multiline discount group that the item is a part of. Should be set when the item info is found.
        private bool includedInTotalDiscount;       //Is true if the item included in the calculation of a combined total discount. Should be set when the item info is found.
        //Periodic discount
        private decimal quantityDiscounted;         //Used in calculation of the periodic discount.  Keeps track of how many items have been discounted.
        private string periodicDiscountOfferId;     //The periodic discount offer id that the item is connected to.
        private string periodicDiscountOfferName;   //The periodic discount offer name that the item is connected to.
        private decimal periodicDiscount;           //Total periodic discount given in this transaction (tax excluded)
        private decimal periodicDiscountWithTax;    //Total periodic discount given in this transaction (tax included)
        private decimal periodicPctDiscount;        //The percentage discount given in this line excluding the total and line discount.
        private LineEnums.PeriodicDiscountType periodicDiscType; //The type of periodic discount type.

        //Amounts
        private decimal netAmount;                  //Amount excluding tax. Amount = (price-discount)*quantity
        private decimal netAmountWithTax;           //The total rounded amount including tax. amountWithTax = (priceWithTax-discountWithTax)*quantity
        private decimal netAmountWithTaxWithoutDiscountVoucher;
        private decimal grossAmount;                //The total line amount(price*quantity) excluding tax;
        private decimal grossAmountWithTax;         //The total line amount(price*quantity) including tax;
        //Tax
        private string taxGroupId;                  //The tax group used for calculation of tax.

        private decimal taxAmount;                  //The tax amount for the item. 
        private decimal taxRatePct;                 //The taxrate as a percentage, i.e 14,5%.
        private bool taxIncludedInItemPrice;        //Does the original price include tax        
        private bool orgTaxIncludedInItemPrice;
        //the original setting - used for switching out the setting regarding to customers
        //Quantity
        private decimal quantity;                   //The quantity sold/returned
        private string salesOrderUnitOfMeasure = "";     //The item unit of the sales order
        private string salesOrderUnitOfMeasureText = "";     //The item unit of the sales order
        private string salesOrderUnitOfMeasureName = ""; //The name of the unit of the sales order.
        private string orgUnitOfMeasure = "";    //The item unit of the sales order
        private string orgUnitOfMeasureName = "";//The name of the unit of the sales order.
        protected decimal unitQuantityFactor;         //To be used in CalculateLine when calculating unitQuantity
        private decimal unitOfMeasureMarkupAmount = 0M;  //the amount that is used as markup for a specific unit of measure
        private decimal unitQuantity;               //How many items within this item i.e if item is a box of items
        private bool unitOfMeasureChanged;          //Has the UnitOfMeasure been changed for this item?        
        //SaleDetails
        private bool lineWasDiscounted;             //Is set to true if discounted was calculated for the item.
        //Comment
        private string comment;                     //An insertable comment/remark by the clerk
        //Changed
        private bool wasChanged;                    //Is set to true if the item was changed by price or discount calculation. Is set to false in the totalcalulation.
        private bool canBeModified;                 //A variable designed for our partners to use when dealing with fiscal printers.
        //Blocked and found
        private bool blocked;                       //Is true if the item is blocked from being sold on the POS system.
        private bool canBeSold;                       //Is true if the item can be sold on the POS system.
        private Date dateToBeBlocked = Date.Empty;           //The date when the blocking becomes active
        private bool found;                         //Is set as true if a item is found.
        private bool discountsWereRemoved;          //Was the discount overrided,  was set mainly for the ;
        //Activate item
        private Date dateToActivateItem = Date.Empty;        //Date when the item is to be activate for selling
        //Serial id and RFID                
        private string inventSerialId;              //Serial no on item
        private string rfidTagId;                   //RFID tag on item
        //Dimension group
        private string dimGroupId;                  //Dimension group that the item belongs to

        private string salesTaxGroupId;             //For US tax implementation                   

        /// <summary>
        /// Used when terminal setting is set to have prices including tax
        /// </summary>
        public decimal NetAmountPerUnit
        {
            get
            {
                if (this.Quantity != 0M)
                {
                    return (this.NetAmount / this.Quantity);
                }
                return 0M;
            }
        }

        /// <summary>
        /// Used when terminal setting is set to have prices including tax
        /// </summary>
        public decimal NetAmountWithAllInclusiveTaxPerUnit
        {
            get
            {
                if (this.Quantity != 0M)
                {
                    return (this.PriceWithTax / this.Quantity);
                }
                return 0M;
            }
        }

        /// <summary>
        /// Describes a group of sales tax codes
        /// </summary>
        public string SalesTaxGroupId
        {
            get { return salesTaxGroupId; }
            set { salesTaxGroupId = value; }
        }

        #endregion

        #region Properties

        /// <summary>
        /// The transaction that this sale item belongs to
        /// </summary>
        public IRetailTransaction Transaction
        {
            get { return transaction; }
            set { transaction = (RetailTransaction)value; }
        }

        /// <summary>
        /// The item id as stored in the ERP system.
        /// </summary>
        public string ItemId
        {
            get { return itemId; }
            set { itemId = value; }
        }

        /// <summary>
        /// If this item is a variant item, the header ID is the item that this item is a variation of
        /// </summary>
        public Guid HeaderItemID
        {
            get { return headerItemID; }
            set { headerItemID = value; }
        }

        public Guid MasterID
        {
            get { return masterID; }
            set { masterID = value; }
        }

        public string VariantName
        {
            get { return variantName; }
            set { variantName = value; }
        }

        /// <summary>
        /// The type of item sold, i.e item, BOM, service.
        /// </summary>
        public ItemTypeEnum ItemType
        {
            get { return itemType; }
            set { itemType = value; }
        }

        /// <summary>
        /// The barcode as stored in the ERP system.
        /// </summary>
        public string BarcodeId
        {
            get { return barcodeId; }
            set { barcodeId = value; }
        }

        /// <summary>
        /// The retail group of the item.
        /// </summary>
        public string ItemGroupId
        {
            get { return itemGroupId; }
            set { itemGroupId = value; }
        }

        /// <summary>
        /// The retail group of the item.
        /// </summary>
        public string RetailItemGroupId
        {
            get { return retailItemGroupId; }
            set { retailItemGroupId = value; }
        }

        /// <summary>
        /// The retail departement of the item.
        /// </summary>
        public string ItemDepartmentId
        {
            get { return itemDepartmentId; }
            set { itemDepartmentId = value; }
        }

        /// <summary>
        /// Price excluding the tax.
        /// </summary>
        public decimal Price
        {
            get { return price; }
            set { price = value; }
        }

        /// <summary>
        /// Price including the tax.
        /// </summary>
        public decimal PriceWithTax
        {
            get { return priceWithTax; }
            set { priceWithTax = value; }
        }

        public decimal OriginalDiscountVoucherPriceWithTax
        {
            get { return originalDiscountVoucherPriceWithTax; }
            set { originalDiscountVoucherPriceWithTax = value; }
        }

        /// <summary>
        /// The cost price of the item.
        /// </summary>
        public decimal CostPrice
        {
            get { return costPrice; }
            set { costPrice = value; }
        }

        /// <summary>
        /// The cost price of the item.
        /// </summary>
        public string PriceID
        {
            get { return priceID; }
            set { priceID = value; }
        }

        /// <summary>
        /// Type of the price. BasePrice = 0, SalesPrice = 1, Promotion = 2
        /// </summary>
        public TradeAgreementPriceType PriceType
        {
            get { return priceType; }
            set { priceType = value; }
        }

        /// <summary>
        /// Stores the amount entered when the price was overridden
        /// </summary>
        public decimal PriceOverrideAmount
        {
            get { return priceOverrideAmount; }
            set { priceOverrideAmount = value; }
        }

        /// <summary>
        /// It is set to true if price was manually overrided
        /// </summary>
        public bool PriceOverridden
        {
            get { return priceOverridden; }
            set { priceOverridden = value; }
        }

        /// <summary>
        /// Stores the original price excluding the tax, if price was manually overrided.
        /// </summary>
        public decimal OriginalPrice
        {
            get { return originalPrice; }
            set { originalPrice = value; }
        }

        /// <summary>
        /// Stores the original price including the tax, if price was manually overrided.
        /// </summary>
        public decimal OriginalPriceWithTax
        {
            get { return originalPriceWithTax; }
            set { originalPriceWithTax = value; }
        }

        /// <summary>
        /// The standard retail price. Used to store a retail price for comparison with the customer price.
        /// </summary>
        public decimal StandardRetailPrice
        {
            get { return standardRetailPrice; }
            set { standardRetailPrice = value; }
        }

        /// <summary>
        /// The standard retail price uncluding the tax. Used to store a retail price for comparison with the customer price.
        /// </summary>
        public decimal StandardRetailPriceWithTax
        {
            get { return standardRetailPriceWithTax; }
            set { standardRetailPriceWithTax = value; }
        }

        /// <summary>
        /// If a price is given for a for more than one unit the price unit tells how many. (tax excluded)
        /// If the price is given for six units (priceUnit=6) as 100 EUR, then when 12 units are sold the price will be 200 EUR.
        /// </summary>
        public decimal PriceUnit
        {
            get { return priceUnit; }
            set { priceUnit = value; }
        }

        /// <summary>
        /// If a price is given for a for more than one unit the price unit tells how many. (tax included)
        /// If the price is given for six units (priceUnit=6) as 100 EUR, then when 12 units are sold the price will be 200 EUR.
        /// </summary>
        public decimal PriceUnitWithTax
        {
            get { return priceUnitWithTax; }
            set { priceUnitWithTax = value; }
        }

        /// <summary>
        /// The price cannot be calculated - always use the price as is set
        /// </summary>
        public bool NoPriceCalculation
        {
            get { return noPriceCalculation; }
            set { noPriceCalculation = value; }
        }

        /// <summary>
        /// Total discount given in this line minus the linediscount (tax exclued).
        /// The total discount for the transaction containing this line is proportionally distributed among each line of the transaction.
        /// TotalDiscount contains the amount associated to the current line.
        /// </summary>
        public decimal TotalDiscount
        {
            get { return totalDiscount; }
            set { totalDiscount = value; }
        }

        /// <summary>
        /// Total discount given in this transaction minus the linediscount (including the tax).
        /// </summary>
        public decimal TotalDiscountWithTax
        {
            get { return totalDiscountWithTax; }
            set { totalDiscountWithTax = value; }
        }

        /// <summary>
        /// The percentage discount given in this line excluding the linediscount.
        /// </summary>
        public decimal TotalPctDiscount
        {
            get { return totalPctDiscount; }
            set { totalPctDiscount = value; }
        }

        /// <summary>
        /// Total line discount given in this transaction (excluding the tax). 
        /// </summary>
        public decimal LineDiscount
        {
            get { return lineDiscount; }
            set { lineDiscount = value; }
        }

        /// <summary>
        /// Total line discount given in this transaction (including the tax)
        /// </summary>
        public decimal LineDiscountWithTax
        {
            get { return lineDiscountWithTax; }
            set { lineDiscountWithTax = value; }
        }

        /// <summary>
        /// The percentage discount given in this line excluding the total discount.
        /// </summary>
        public decimal LinePctDiscount
        {
            get { return linePctDiscount; }
            set { linePctDiscount = value; }
        }

        /// <summary>
        /// It is true if the item should never be discounted. Should be set when the item info is found.
        /// </summary>
        public bool NoDiscountAllowed
        {
            get { return noDiscountAllowed; }
            set { noDiscountAllowed = value; }
        }

        /// <summary>
        /// It is true if the item should never be allowed a manual discount.
        /// </summary>
        public bool NoManualDiscountAllowed
        {
            get { return noManualDiscountAllowed; }
            set { noManualDiscountAllowed = value; }
        }

        /// <summary>
        /// Price that is set for the customer only - Trade Agreements
        /// </summary>
        public decimal CustomerPrice
        {
            get { return customerPrice; }
            set { customerPrice = value; }
        }

        /// <summary>
        /// Promotion price if it exists
        /// </summary>
        public decimal PromotionPrice
        {
            get { return promotionPrice; }
            set { promotionPrice = value; }
        }

        /// <summary>
        /// Price from the item card
        /// </summary>
        public decimal BasicPrice
        {
            get { return basicPrice; }
            set { basicPrice = value; }
        }

        /// <summary>
        /// Price from the Trade Agreements that does not come from a customer relation
        /// </summary>
        public decimal TradeAgreementPrice
        {
            get { return tradeAgreementPrice; }
            set { tradeAgreementPrice = value; }
        }

        /// <summary>
        /// True if the item was supposed to be discounted but the NoDiscountAllowed property did not allow it.
        /// </summary>
        public bool DiscountUnsuccessfullyApplied
        {
            get { return discountUnsuccessfullyApplied; }
            set { discountUnsuccessfullyApplied = value; }
        }

        /// <summary>
        /// Total customer discount given in this transaction (excluding tax)
        /// </summary>
        public decimal CustomerDiscount
        {
            get { return customerDiscount; }
            set { customerDiscount = value; }
        }

        /// <summary>
        /// Total customer discount given in this transaction (including tax)
        /// </summary>
        public decimal CustomerDiscountWithTax
        {
            get { return customerDiscountWithTax; }
            set { customerDiscountWithTax = value; }
        }

        /// <summary>
        /// The line discount group that the item belongs to. Should be set when the item info is found.
        /// </summary>
        public string LineDiscountGroup
        {
            get { return lineDiscountGroup; }
            set { lineDiscountGroup = value; }
        }

        /// <summary>
        /// The multiline discount group that the item is a part of. Should be set when the item info is found.
        /// </summary>
        public string MultiLineDiscountGroup
        {
            get { return multiLineDiscountGroup; }
            set { multiLineDiscountGroup = value; }
        }

        /// <summary>
        /// It is true if the item is included in the calculation of a combined total discount. Should be set when the item info is found.
        /// </summary>
        public bool IncludedInTotalDiscount
        {
            get { return includedInTotalDiscount; }
            set { includedInTotalDiscount = value; }
        }

        /// <summary>
        /// Used in calculation of the periodic discount. Keeps track of how many items have been discounted.
        /// </summary>
        public decimal QuantityDiscounted
        {
            get { return quantityDiscounted; }
            set { quantityDiscounted = value; }
        }

        /// <summary>
        /// The periodic discount offer id that the item is connected to.
        /// </summary>
        public string PeriodicDiscountOfferId
        {
            get { return periodicDiscountOfferId; }
            set { periodicDiscountOfferId = value; }
        }

        /// <summary>
        /// The periodic discount offer name that the item is connected to.
        /// </summary>        
        public string PeriodicDiscountOfferName
        {
            get { return periodicDiscountOfferName; }
            set { periodicDiscountOfferName = value; }
        }

        /// <summary>
        /// Total periodic discount given in this transaction (tax excluded)
        /// </summary> 
        public decimal PeriodicDiscount
        {
            get { return periodicDiscount; }
            set { periodicDiscount = value; }
        }

        /// <summary>
        /// Total periodic discount given in this transaction (tax included)
        /// </summary> 
        public decimal PeriodicDiscountWithTax
        {
            get { return periodicDiscountWithTax; }
            set { periodicDiscountWithTax = value; }
        }

        /// <summary>
        /// The percentage discount given in this line excluding the total and line discount.
        /// </summary> 
        public decimal PeriodicPctDiscount
        {
            get { return periodicPctDiscount; }
            set { periodicPctDiscount = value; }
        }

        /// <summary>
        /// The periodic discount type 0 = None, 1 = Multibuy, 2 = Mix And Match, 3 = Discount offer
        /// </summary>
        public LineEnums.PeriodicDiscountType PeriodicDiscType
        {
            get { return periodicDiscType; }
            set { periodicDiscType = value; }
        }

        /// <summary>
        /// The line amount excluding tax.
        /// </summary>
        public decimal NetAmount
        {
            get { return netAmount; }
            set { netAmount = value; }
        }

        /// <summary>
        /// The total rounded amount including tax.
        /// </summary>
        public decimal NetAmountWithTax
        {
            get { return netAmountWithTax; }
            set { netAmountWithTax = value; }
        }

        public decimal NetAmountWithTaxWithoutDiscountVoucher
        {
            get { return netAmountWithTaxWithoutDiscountVoucher; }
            set { netAmountWithTaxWithoutDiscountVoucher = value; }
        }

        /// <summary>
        /// The total line amount (price * quantity) excluding tax
        /// </summary>
        public decimal GrossAmount
        {
            get { return grossAmount; }
            set { grossAmount = value; }
        }

        /// <summary>
        /// The total line amount (price * quantity) including tax
        /// </summary>
        public decimal GrossAmountWithTax
        {
            get { return grossAmountWithTax; }
            set { grossAmountWithTax = value; }
        }

        /// <summary>
        /// The tax group used for calculation of tax.
        /// </summary>
        public string TaxGroupId
        {
            get { return taxGroupId; }
            set { taxGroupId = value; }
        }

        /// <summary>
        /// The tax amount for the item.
        /// </summary>
        public decimal TaxAmount
        {
            get { return taxAmount; }
            set { taxAmount = value; }
        }

        /// <summary>
        /// The taxrate as a percentage, i.e 14,5%. 
        /// </summary>
        public decimal TaxRatePct
        {
            get { return taxRatePct; }
            set { taxRatePct = value; }
        }

        /// <summary>
        /// The quantity sold/returned
        /// </summary>
        public virtual decimal Quantity
        {
            get { return quantity; }
            set { quantity = value; }
        }

        /// <summary>
        /// The original quantity if the line is part of a return transaction
        /// </summary>
        public decimal OriginalQuantity { get; set; }

        /// <summary>
        /// The original quantity of the item if it is a linked item.
        /// Has to be saved to that Set Qty and Clear Qty can multiply/divide the qty correctly
        /// </summary>
        public decimal LinkedItemOrgQuantity { get; set; }

        /// <summary>
        /// The item unit
        /// </summary>
        public string SalesOrderUnitOfMeasure //TODO: change to unitofmeasure
        {
            get { return salesOrderUnitOfMeasure; }
            set { salesOrderUnitOfMeasure = value; }
        }

        /// <summary>
        /// The item unit
        /// </summary>
        public string SalesOrderUnitOfMeasureText
        {
            get { return salesOrderUnitOfMeasureText; }
            set { salesOrderUnitOfMeasureText = value; }
        }

        /// <summary>
        /// The item unit name
        /// </summary>
        public string SalesOrderUnitOfMeasureName
        {
            get { return salesOrderUnitOfMeasureName; }
            set { salesOrderUnitOfMeasureName = value; }
        }

        /// <summary>
        /// The item unit
        /// </summary>
        public string OrgUnitOfMeasure
        {
            get { return orgUnitOfMeasure; }
            set { orgUnitOfMeasure = value; }
        }

        /// <summary>
        /// The amount that is used as markup for a specific unit of measure
        /// </summary>
        public decimal MarkupAmount
        {
            get { return unitOfMeasureMarkupAmount; }
            set { unitOfMeasureMarkupAmount = value; }
        }

        /// <summary>
        /// The item unit name
        /// </summary>
        public string OrgUnitOfMeasureName
        {
            get { return orgUnitOfMeasureName; }
            set { orgUnitOfMeasureName = value; }
        }

        /// <summary>
        /// The unit quantity
        /// </summary>
        public decimal UnitQuantity
        {
            get
            {
                return unitQuantity;
            }
            set { unitQuantity = value; }
        }

        /// <summary>
        /// To be used in CalculateLine when calculating unitQuantity
        /// </summary>
        public decimal UnitQuantityFactor
        {
            get { return unitQuantityFactor; }
            set { unitQuantityFactor = value; }
        }

        /// <summary>
        /// Is set to true if a discount was calculated for the item.
        /// </summary>
        public bool LineWasDiscounted
        {
            get { return lineWasDiscounted; }
            set { lineWasDiscounted = value; }
        }

        /// <summary>
        /// An insertable comment/remark by the clerk
        /// </summary>
        public string Comment
        {
            get { return comment; }
            set { comment = value; }
        }

        /// <summary>
        /// It is set to true if the item was changed by price or discount calculation. 
        /// It is set to false in the totalcalulation.
        /// </summary>
        public bool WasChanged
        {
            get { return wasChanged; }
            set { wasChanged = value; }
        }

        /// <summary>
        /// A variable designed for our partners to use when dealing with fiscal printers.
        /// Dictates whether the line has already been sent to the fiscal printer, etc, and cannot therefore be changed subsequently.
        /// </summary>
        public bool CanBeModified
        {
            get { return canBeModified; }
            set { canBeModified = value; }
        }

        /// <summary>
        /// True if the item can be sold on the POS system.
        /// </summary>
        public bool CanBeSold
        {
            get { return canBeSold; }
            set { canBeSold = value; }
        }
        /// <summary>
        /// True if the item is blocked from being sold on the POS system.
        /// </summary>
        public bool Blocked
        {
            // If dateToBeBlocked is something and that day has arrived, then the item is blocked.
            get { return (!dateToBeBlocked.IsEmpty && (DateTime.Now >= dateToBeBlocked.DateTime)); }
            set { if (!value) dateToBeBlocked = Date.Empty; }
        }

        /// <summary>
        /// The date when the blocking becomes active
        /// </summary>
        public DateTime DateToBeBlocked
        {
            get { return dateToBeBlocked.ToAxaptaSQLDate(); }
            set { dateToBeBlocked = new Date(value, true); }
        }

        public bool Active
        {
            get { return dateToActivateItem.DateTime == Date.Empty.DateTime || dateToActivateItem.DateTime < transaction.BeginDateTime; }
        }

        /// <summary>
        /// The date when the items becomes active
        /// </summary>
        public Date DateToActivateItem
        {
            get { return dateToActivateItem; }
            set { dateToActivateItem = value; }
        }

        /// <summary>
        /// The item's serial no.
        /// </summary>
        public string SerialId
        {
            get { return inventSerialId; }
            set { inventSerialId = value; }
        }

        /// <summary>
        /// The item's RFID tag
        /// </summary>
        public string RFIDTagId
        {
            get { return rfidTagId; }
            set { rfidTagId = value; }
        }

        /// <summary>
        /// If true, the serial number was manually entered
        /// </summary>
        public bool SerialIdManualInput { get; set; }

        /// <summary>
        /// The item's dimension group id
        /// </summary>
        public string DimensionGroupId
        {
            get { return dimGroupId; }
            set { dimGroupId = value; }
        }

        /// <summary>
        /// Is set as true if a item is found.
        /// </summary>
        public bool Found
        {
            get { return found; }
            set { found = value; }
        }

        /// <summary>
        /// Whether the discount was removed form the item.
        /// </summary>
        public bool DiscountsWereRemoved
        {
            get { return discountsWereRemoved; }
            set { discountsWereRemoved = value; }
        }

        /// <summary>
        /// Whether the item line is to be excluded
        /// </summary>
        public bool Excluded
        {
            get { return blocked || !found || (dateToActivateItem.DateTime > DateTime.Now); }
        }

        /// <summary>
        /// Does the original price include tax?
        /// </summary>
        public bool TaxIncludedInItemPrice
        {
            get
            {
                return taxIncludedInItemPrice;
            }
            set
            {
                taxIncludedInItemPrice = value;
            }
        }

        /// <summary>
        /// Original setting of the <see cref="TaxIncludedInItemPrice"/>
        /// </summary>
        public bool OriginalTaxIncludedInItemPrice
        {
            get
            {
                return orgTaxIncludedInItemPrice;
            }
            set
            {
                orgTaxIncludedInItemPrice = value;
            }
        }

        /// <summary>
        /// Has the unit of measure been changed for the current sale item?
        /// </summary>
        public bool UnitOfMeasureChanged
        {
            get { return unitOfMeasureChanged; }
            set { unitOfMeasureChanged = value; }
        }

        /// <summary>
        /// If true, then this sale item is tax exempt
        /// </summary>
        public bool TaxExempt { get; set; }

        /// <summary>
        /// The code entered by the user when the item was tax exempted
        /// </summary>
        public string TaxExemptionCode { get; set; }

        /// <summary>
        /// The percentage discount given using a loyalty discounts
        /// </summary>
        public decimal LoyaltyPctDiscount { get; set; }

        /// <summary>
        /// The calculated loyalty discount including tax
        /// </summary>
        public decimal LoyaltyDiscountWithTax { get; set; }

        /// <summary>
        /// The calculated loyalty discount excluding tax
        /// </summary>
        public decimal LoyaltyDiscount { get; set; }

        /// <summary>
        /// Item information for the Customer order 
        /// </summary>
        public IOrderItem Order { get; set; }

        /// <summary>
        /// Indicates wether this item can be returned at the POS
        /// </summary>
        public bool Returnable { get; set; }
        #endregion

        /// <summary>
        /// A collection of all discounts belonging to the salesitem.
        /// </summary>
        public List<IDiscountItem> DiscountLines { get; set; }

        public List<TaxItem> TaxLines { get; set; }

        public IEnumerable<TaxItem> ITaxLines
        {
            get
            {
                return TaxLines.Cast<TaxItem>();
            }
        }

        /// <summary>
        /// A collection of all discounts belonging to the salesitem.
        /// </summary>
        public IEnumerable<IDiscountItem> IDiscountLines
        {
            get
            {
                return DiscountLines.Cast<IDiscountItem>();
            }
        }

        /// <inheritdoc/>
        public Guid ID { get; set; }

        public void ClearDiscountLines()
        {
            DiscountLines.Clear();
        }

        /// <summary>
        /// The tax group lines that come with the tax group on the item itself. 
        /// This list will not change when a customer is added/cleared from the transaction. 
        /// This list is only used to find the originl price without tax when Pos is configured to have prices as "Tax included in price".         
        /// To see current tax group configuration use TaxLines.
        /// </summary>
        public BaseSaleItem()
            : base()
        {
            this.BeginDateTime = DateTime.Now;

            DiscountLines = new List<IDiscountItem>();
            TaxLines = new List<TaxItem>();
            TaxGroupId = "";
            dateToBeBlocked = Date.Empty;

            inventSerialId = "";
            rfidTagId = "";
            SerialIdManualInput = false;
            periodicDiscountOfferId = "";
            noPriceCalculation = false;
            LinkedItemOrgQuantity = 1M;
            Returnable = true;
            CanBeSold = true;
            Order = new OrderItem();
            ID = Guid.NewGuid();
        }

        ~BaseSaleItem()
        {
            if (DiscountLines != null)
                DiscountLines.Clear();
            if (TaxLines != null)
                TaxLines.Clear();
        }

        protected void Populate(BaseSaleItem item)
        {
            base.Populate(item);
            item.PeriodicDiscType = PeriodicDiscType;
            item.BarcodeId = BarcodeId;
            item.BeginDateTime = BeginDateTime;
            item.Blocked = Blocked;
            item.CanBeSold = CanBeSold;
            item.Comment = Comment;
            item.CostPrice = CostPrice;
            item.PriceID = PriceID;
            item.PriceType = PriceType;
            item.DateToBeBlocked = DateToBeBlocked;
            item.DateToActivateItem = DateToActivateItem;
            item.Description = Description;
            item.DescriptionAlias = DescriptionAlias;
            item.DescriptionAltLanguage = DescriptionAltLanguage;
            item.RFIDTagId = RFIDTagId;
            item.SerialId = SerialId;
            item.SerialIdManualInput = SerialIdManualInput;
            item.EndDateTime = EndDateTime;
            item.Found = Found;
            item.GrossAmount = GrossAmount;
            item.GrossAmountWithTax = GrossAmountWithTax;
            item.IncludedInTotalDiscount = IncludedInTotalDiscount;
            item.ItemDepartmentId = ItemDepartmentId;
            item.ItemGroupId = ItemGroupId;
            item.ItemId = ItemId;
            item.ItemType = ItemType;
            item.LineDiscount = LineDiscount;
            item.LineDiscountGroup = LineDiscountGroup;
            item.LineDiscountWithTax = LineDiscountWithTax;
            item.LineId = LineId;
            item.LinePctDiscount = LinePctDiscount;
            item.PeriodicDiscount = PeriodicDiscount;
            item.PeriodicDiscountWithTax = PeriodicDiscountWithTax;
            item.PeriodicDiscountOfferId = PeriodicDiscountOfferId;
            item.PeriodicDiscountOfferName = PeriodicDiscountOfferName;
            item.PeriodicPctDiscount = PeriodicPctDiscount;
            item.LineWasDiscounted = LineWasDiscounted;
            item.MultiLineDiscountGroup = MultiLineDiscountGroup;
            item.NetAmount = NetAmount;
            item.NetAmountWithTax = NetAmountWithTax;
            item.NetAmountWithTaxWithoutDiscountVoucher = NetAmountWithTaxWithoutDiscountVoucher;
            item.NoDiscountAllowed = NoDiscountAllowed;
            item.OriginalPrice = OriginalPrice;
            item.OriginalPriceWithTax = OriginalPriceWithTax;
            item.Price = Price;
            item.PriceOverrideAmount = PriceOverrideAmount;
            item.PriceOverridden = PriceOverridden;
            item.PriceUnit = PriceUnit;
            item.PriceUnitWithTax = PriceUnitWithTax;
            item.PriceWithTax = PriceWithTax;
            item.Quantity = Quantity;
            item.QuantityDiscounted = QuantityDiscounted;
            item.OriginalQuantity = OriginalQuantity;
            item.LinkedItemOrgQuantity = LinkedItemOrgQuantity;
            item.StandardRetailPrice = StandardRetailPrice;
            item.StandardRetailPriceWithTax = StandardRetailPriceWithTax;
            item.TaxAmount = TaxAmount;
            item.TaxGroupId = TaxGroupId;
            item.TaxLines = CollectionHelper.Clone<TaxItem, List<TaxItem>>(TaxLines);
            item.TaxRatePct = TaxRatePct;
            item.TotalDiscount = TotalDiscount;
            item.TotalDiscountWithTax = TotalDiscountWithTax;
            item.TotalPctDiscount = TotalPctDiscount;
            item.LoyaltyPctDiscount = LoyaltyPctDiscount;
            item.LoyaltyDiscount = LoyaltyDiscount;
            item.LoyaltyDiscountWithTax = LoyaltyDiscountWithTax;
            item.SalesOrderUnitOfMeasure = SalesOrderUnitOfMeasure;
            item.SalesOrderUnitOfMeasureName = SalesOrderUnitOfMeasureName;
            item.Voided = Voided;
            item.ChangedForPreparation = ChangedForPreparation;
            item.WasChanged = WasChanged;
            item.CustomerDiscount = CustomerDiscount;
            item.CustomerDiscountWithTax = CustomerDiscountWithTax;
            item.InfoCodeLines = CollectionHelper.Clone<InfoCodeLineItem, List<InfoCodeLineItem>>(InfoCodeLines);
            item.NoManualDiscountAllowed = NoManualDiscountAllowed;
            item.CustomerPrice = CustomerPrice;
            item.BasicPrice = BasicPrice;
            item.TradeAgreementPrice = TradeAgreementPrice;
            item.PromotionPrice = PromotionPrice;
            item.DiscountsWereRemoved = DiscountsWereRemoved;
            item.CanBeModified = CanBeModified;
            item.TaxIncludedInItemPrice = TaxIncludedInItemPrice;
            item.retailItemGroupId = retailItemGroupId;
            item.periodicDiscountOfferId = periodicDiscountOfferId;
            item.orgTaxIncludedInItemPrice = orgTaxIncludedInItemPrice;
            item.DiscountLines = CollectionHelper.Clone<IDiscountItem, List<IDiscountItem>>(DiscountLines);
            item.salesTaxGroupId = salesTaxGroupId;
            item.orgUnitOfMeasure = orgUnitOfMeasure;
            item.orgUnitOfMeasureName = orgUnitOfMeasureName;
            item.unitQuantityFactor = unitQuantityFactor;
            item.unitQuantity = unitQuantity;
            item.dimGroupId = dimGroupId;
            item.TaxExemptionCode = TaxExemptionCode;
            item.TaxExempt = TaxExempt;
            item.HeaderItemID = HeaderItemID;
            item.MasterID = MasterID;
            item.VariantName = VariantName;
            item.Order = (OrderItem) Order.Clone();
            item.Returnable = Returnable;
            item.ID = ID;
        }

        public void ForceTaxIncludingPriceSetting(bool priceIncludingTax)
        {
            orgTaxIncludedInItemPrice = taxIncludedInItemPrice;
            taxIncludedInItemPrice = priceIncludingTax;
        }

        public void ResetTaxIncludingPrice()
        {
            taxIncludedInItemPrice = orgTaxIncludedInItemPrice;
        }

        //NEW (for U.S. tax implementation):
        public void Add(TaxItem taxLineItem)
        {
            this.TaxLines.Add(taxLineItem);
        }

        /// <summary>
        /// Deserialize the info code lines
        /// </summary>
        public List<InfoCodeLineItem> CreateInfocodeLines(XElement xItems, IErrorLog errorLogger = null)
        {
            List<InfoCodeLineItem> InfocodeLines = new List<InfoCodeLineItem>();

            if (xItems.HasElements)
            {
                IEnumerable<XElement> xInfocodeItems = xItems.Elements("InfoCodeLineItem");
                foreach (XElement xInfocodeItem in xInfocodeItems)
                {
                    if (xInfocodeItem.HasElements)
                    {
                        InfoCodeLineItem newInfocode = new InfoCodeLineItem();
                        newInfocode.ToClass(xInfocodeItem, errorLogger);
                        InfocodeLines.Add(newInfocode);
                    }
                }
            }
            return InfocodeLines;
        }

        /// <summary>
        /// Deserialize the discount lines
        /// </summary>
        public List<IDiscountItem> CreateDiscountLines(XElement xItem, IErrorLog errorLogger = null)
        {
            List<IDiscountItem> DiscountLines = new List<IDiscountItem>();

            if (xItem.HasElements)
            {
                IEnumerable<XElement> classElements = xItem.Elements();
                foreach (XElement xClass in classElements)
                {
                    switch (xClass.Name.ToString())
                    {
                        case "LineDiscountItem":
                            LineDiscountItem ldi = new LineDiscountItem();
                            ldi.ToClass(xClass, errorLogger);
                            DiscountLines.Add(ldi);
                            break;
                        case "TotalDiscountItem":
                            TotalDiscountItem tdi = new TotalDiscountItem();
                            tdi.ToClass(xClass, errorLogger);
                            DiscountLines.Add(tdi);
                            break;
                        case "PeriodicDiscountItem":
                            PeriodicDiscountItem pdi = new PeriodicDiscountItem();
                            pdi.ToClass(xClass, errorLogger);
                            DiscountLines.Add(pdi);
                            break;
                        case "CustomerDiscountItem":
                            CustomerDiscountItem cdi = new CustomerDiscountItem();
                            cdi.ToClass(xClass, errorLogger);
                            DiscountLines.Add(cdi);
                            break;
                        case "LoyaltyDiscountItem":
                            LoyaltyDiscountItem di = new LoyaltyDiscountItem();
                            di.ToClass(xClass, errorLogger);
                            DiscountLines.Add(di);
                            break;
                    }
                }
            }

            return DiscountLines;
        }

        /// <summary>
        /// Deserialize the tax lines
        /// </summary>
        public List<TaxItem> CreateTaxLines(XElement xItem, IErrorLog errorLogger = null)
        {
            List<TaxItem> TaxLines = new List<TaxItem>();

            if (xItem.HasElements)
            {
                IEnumerable<XElement> classElements = xItem.Elements("TaxItem");
                foreach (XElement xClass in classElements)
                {
                    if (xClass.HasElements)
                    {
                        if (!xClass.IsEmpty)
                        {
                            TaxItem taxLine = new TaxItem();
                            taxLine.ToClass(xClass, errorLogger);
                            TaxLines.Add(taxLine);
                        }

                    }
                }
            }

            return TaxLines;
        }

        private string FixComments(string comment)
        {
            if (comment == null || comment == "")
                return comment;

            if (comment.IndexOf("\r\n") > 0)
            {
                return comment;
            }

            if (comment.IndexOf("\n") > 0)
            {
                //comment.Replace("\n", "\r\n");
                string[] split = comment.Split(new string[] { "\n" }, StringSplitOptions.None);
                string newStr = "";
                for (int i = 0; i < split.Length; i++)
                {
                    if (newStr != "") { newStr += "\r\n"; }
                    newStr += split[i];
                }
                if (newStr != "")
                    return newStr;
            }

            return comment;
        }

        public override XElement ToXML(IErrorLog errorLogger = null)
        {
            try
            {
                /*
                 * Strings      added as is
                 * Int          added as is
                 * Bool         added as is
                 * 
                 * Decimal      added with ToString()
                 * DateTime     added with DevUtilities.Utility.DateTimeToXmlString()
                 * 
                 * Enums        added with an (int) cast   
                 * 
                */
                XElement xBaseItem = new XElement("BaseItem",
                    new XElement("ItemID", itemId),
                    new XElement("ItemType", Conversion.ToXmlString((int)itemType)),
                    new XElement("BarcodeId", barcodeId),
                    new XElement("itemGroupId", itemGroupId),
                    new XElement("itemDepartmentId", itemDepartmentId),
                    new XElement("retailItemGroupId", retailItemGroupId),
                    new XElement("price", Conversion.ToXmlString(price)),
                    new XElement("priceID", priceID),
                    new XElement("priceType", Conversion.ToXmlString((int)priceType)),
                    new XElement("priceWithTax", Conversion.ToXmlString(priceWithTax)),
                    new XElement("originalDiscountVoucherPriceWithTax", Conversion.ToXmlString(originalDiscountVoucherPriceWithTax)),
                    new XElement("costPrice", Conversion.ToXmlString(costPrice)),
                    new XElement("priceOverridden", Conversion.ToXmlString(priceOverridden)),
                    new XElement("priceOverrideAmount", Conversion.ToXmlString(priceOverrideAmount)),
                    new XElement("originalPrice", Conversion.ToXmlString(originalPrice)),
                    new XElement("originalPriceWithTax", Conversion.ToXmlString(originalPriceWithTax)),
                    new XElement("standardRetailPrice", Conversion.ToXmlString(standardRetailPrice)),
                    new XElement("standardRetailPriceWithTax", Conversion.ToXmlString(standardRetailPriceWithTax)),
                    new XElement("priceUnit", Conversion.ToXmlString(priceUnit)),
                    new XElement("priceUnitWithTax", Conversion.ToXmlString(priceUnitWithTax)),
                    new XElement("totalDiscount", Conversion.ToXmlString(totalDiscount)),
                    new XElement("totalDiscountWithTax", Conversion.ToXmlString(totalDiscountWithTax)),
                    new XElement("totalPctDiscount", Conversion.ToXmlString(totalPctDiscount)),
                    new XElement("loyaltyDiscountWithTax", Conversion.ToXmlString(LoyaltyDiscountWithTax)),
                    new XElement("loyaltyDiscount", Conversion.ToXmlString(LoyaltyDiscount)),
                    new XElement("loyaltyPctDiscount", Conversion.ToXmlString(LoyaltyPctDiscount)),
                    new XElement("lineDiscount", Conversion.ToXmlString(lineDiscount)),
                    new XElement("lineDiscountWithTax", Conversion.ToXmlString(lineDiscountWithTax)),
                    new XElement("linePctDiscount", Conversion.ToXmlString(linePctDiscount)),
                    new XElement("noDiscountAllowed", Conversion.ToXmlString(noDiscountAllowed)),
                    new XElement("noManualDiscountAllowed", Conversion.ToXmlString(noManualDiscountAllowed)),
                    new XElement("noPriceCalculation", Conversion.ToXmlString(noPriceCalculation)),
                    new XElement("discountUnsuccessfullyApplied", Conversion.ToXmlString(discountUnsuccessfullyApplied)),
                    new XElement("customerPrice", Conversion.ToXmlString(customerPrice)),
                    new XElement("promotionPrice", Conversion.ToXmlString(promotionPrice)),
                    new XElement("basicPrice", Conversion.ToXmlString(basicPrice)),
                    new XElement("tradeAgreementPrice", Conversion.ToXmlString(tradeAgreementPrice)),
                    new XElement("customerDiscount", Conversion.ToXmlString(customerDiscount)),
                    new XElement("customerDiscountWithTax", Conversion.ToXmlString(customerDiscountWithTax)),
                    new XElement("lineDiscountGroup", lineDiscountGroup),
                    new XElement("multiLineDiscountGroup", multiLineDiscountGroup),
                    new XElement("includedInTotalDiscount", Conversion.ToXmlString(includedInTotalDiscount)),
                    new XElement("quantityDiscounted", Conversion.ToXmlString(quantityDiscounted)),
                    new XElement("OriginalQuantity", Conversion.ToXmlString(OriginalQuantity)),
                    new XElement("LinkedItemOrgQuantity", Conversion.ToXmlString(LinkedItemOrgQuantity)),
                    new XElement("periodicDiscountOfferId", periodicDiscountOfferId),
                    new XElement("periodicDiscountOfferName", periodicDiscountOfferName),
                    new XElement("periodicDiscount", Conversion.ToXmlString(periodicDiscount)),
                    new XElement("periodicDiscountWithTax", Conversion.ToXmlString(periodicDiscountWithTax)),
                    new XElement("periodicPctDiscount", Conversion.ToXmlString(periodicPctDiscount)),
                    new XElement("periodicDiscType", Conversion.ToXmlString((int)periodicDiscType)),
                    new XElement("netAmount", Conversion.ToXmlString(netAmount)),
                    new XElement("netAmountWithTax", Conversion.ToXmlString(netAmountWithTax)),
                    new XElement("netAmountWithTaxWithoutDiscountVoucher", Conversion.ToXmlString(netAmountWithTaxWithoutDiscountVoucher)),
                    new XElement("grossAmount", Conversion.ToXmlString(grossAmount)),
                    new XElement("grossAmountWithTax", Conversion.ToXmlString(grossAmountWithTax)),
                    new XElement("taxGroupId", taxGroupId),
                    new XElement("taxAmount", Conversion.ToXmlString(taxAmount)),
                    new XElement("taxRatePct", Conversion.ToXmlString(taxRatePct)),
                    new XElement("taxIncludedInItemPrice", Conversion.ToXmlString(taxIncludedInItemPrice)),
                    new XElement("orgTaxIncludedInItemPrice", Conversion.ToXmlString(orgTaxIncludedInItemPrice)),
                    new XElement("quantity", Conversion.ToXmlString(quantity)),
                    new XElement("salesOrderUnitOfMeasure", salesOrderUnitOfMeasure),
                    new XElement("salesOrderUnitOfMeasureName", salesOrderUnitOfMeasureName),
                    new XElement("inventOrderUnitOfMeasure", orgUnitOfMeasure),
                    new XElement("inventOrderUnitOfMeasureName", orgUnitOfMeasureName),
                    new XElement("unitQuantityFactor", Conversion.ToXmlString(unitQuantityFactor)),
                    new XElement("unitQuantity", Conversion.ToXmlString(unitQuantity)),
                    new XElement("unitOfMeasureChanged", Conversion.ToXmlString(unitOfMeasureChanged)),
                    new XElement("lineWasDiscounted", Conversion.ToXmlString(lineWasDiscounted)),
                    new XElement("comment", FixComments(comment)),
                    new XElement("wasChanged", Conversion.ToXmlString(wasChanged)),
                    new XElement("canBeModified", Conversion.ToXmlString(canBeModified)),
                    new XElement("blocked", Conversion.ToXmlString(blocked)),
                    new XElement("canBeSold", Conversion.ToXmlString(canBeSold)),
                    new XElement("dateToBeBlocked", dateToBeBlocked.ToXmlString()),
                    new XElement("found", Conversion.ToXmlString(found)),
                    new XElement("discountsWereRemoved", Conversion.ToXmlString(discountsWereRemoved)),
                    new XElement("dateToActivateItem", dateToActivateItem.ToXmlString()),
                    new XElement("inventSerialId", inventSerialId),
                    new XElement("rfidTagId", rfidTagId),
                    new XElement("SerialIdManualInput", Conversion.ToXmlString(SerialIdManualInput)),
                    new XElement("dimGroupId", dimGroupId),
                    new XElement("taxExempt", Conversion.ToXmlString(TaxExempt)),
                    new XElement("taxExemptionCode", TaxExemptionCode),
                    new XElement("salesTaxGroupId", salesTaxGroupId),
                    new XElement("headerItemID", Conversion.ToXmlString(headerItemID)),
                    new XElement("masterID", Conversion.ToXmlString(masterID)),
                    new XElement("variantName", variantName),
                    new XElement("returnable", Conversion.ToXmlString(Returnable)),
                    new XElement("ID", Conversion.ToXmlString(ID)),
                    new XElement("Order", Order.ToXML())
                    
                );

                #region Tax
                XElement xTaxLines = new XElement("TaxLines");
                foreach (TaxItem ti in TaxLines)
                {
                    xTaxLines.Add(ti.ToXML(errorLogger));
                }
                xBaseItem.Add(xTaxLines);
                #endregion

                #region Discounts
                XElement xDiscountLines = new XElement("DiscountLines");
                foreach (DiscountItem ldi in DiscountLines)
                {
                    xDiscountLines.Add(ldi.ToXML(errorLogger));
                }
                xBaseItem.Add(xDiscountLines);
                #endregion

                #region Infocodes
                XElement xInfocodes = new XElement("InfocodeLines");
                foreach (InfoCodeLineItem ici in InfoCodeLines)
                {
                    xInfocodes.Add(ici.ToXML(errorLogger));
                }
                xBaseItem.Add(xInfocodes);
                #endregion

                xBaseItem.Add(base.ToXML(errorLogger));
                return xBaseItem;
            }
            catch (Exception ex)
            {
                errorLogger?.LogMessage(LogMessageType.Error, "BaseSaleItem.ToXML", ex);

                throw;
            }
        }

        public override void ToClass(XElement xItem, IErrorLog errorLogger = null)
        {
            try
            {
                if (xItem.HasElements)
                {
                    IEnumerable<XElement> classVariables = xItem.Elements();
                    foreach (XElement xVariable in classVariables)
                    {
                        if (!xVariable.IsEmpty)
                        {
                            try
                            {
                                switch (xVariable.Name.ToString())
                                {
                                    case "ItemID":
                                        itemId = xVariable.Value;
                                        break;
                                    case "ItemType":
                                        itemType = (ItemTypeEnum)Conversion.XmlStringToInt(xVariable.Value);
                                        break;
                                    case "BarcodeId":
                                        barcodeId = xVariable.Value;
                                        break;
                                    case "itemGroupId":
                                        itemGroupId = xVariable.Value;
                                        break;
                                    case "itemDepartmentId":
                                        itemDepartmentId = xVariable.Value;
                                        break;
                                    case "retailItemGroupId":
                                        retailItemGroupId = xVariable.Value;
                                        break;
                                    case "price":
                                        price = Conversion.XmlStringToDecimal(xVariable.Value);
                                        break;
                                    case "priceID":
                                        priceID = xVariable.Value;
                                        break;
                                    case "priceType":
                                        priceType = (TradeAgreementPriceType)Conversion.XmlStringToInt(xVariable.Value);
                                        break;
                                    case "priceWithTax":
                                        priceWithTax = Conversion.XmlStringToDecimal(xVariable.Value);
                                        break;
                                    case "originalDiscountVoucherPriceWithTax":
                                        originalDiscountVoucherPriceWithTax = Conversion.XmlStringToDecimal(xVariable.Value);
                                        break;
                                    case "costPrice":
                                        costPrice = Conversion.XmlStringToDecimal(xVariable.Value);
                                        break;
                                    case "priceOverrideAmount":
                                        priceOverrideAmount = Conversion.XmlStringToDecimal(xVariable.Value);
                                        break;
                                    case "priceOverridden":
                                        priceOverridden = Conversion.XmlStringToBool(xVariable.Value);
                                        break;
                                    case "originalPrice":
                                        originalPrice = Conversion.XmlStringToDecimal(xVariable.Value);
                                        break;
                                    case "originalPriceWithTax":
                                        originalPriceWithTax = Conversion.XmlStringToDecimal(xVariable.Value);
                                        break;
                                    case "standardRetailPrice":
                                        standardRetailPrice = Conversion.XmlStringToDecimal(xVariable.Value);
                                        break;
                                    case "standardRetailPriceWithTax":
                                        standardRetailPriceWithTax = Conversion.XmlStringToDecimal(xVariable.Value);
                                        break;
                                    case "priceUnit":
                                        priceUnit = Conversion.XmlStringToDecimal(xVariable.Value);
                                        break;
                                    case "priceUnitWithTax":
                                        priceUnitWithTax = Conversion.XmlStringToDecimal(xVariable.Value);
                                        break;
                                    case "totalDiscount":
                                        totalDiscount = Conversion.XmlStringToDecimal(xVariable.Value);
                                        break;
                                    case "totalDiscountWithTax":
                                        totalDiscountWithTax = Conversion.XmlStringToDecimal(xVariable.Value);
                                        break;
                                    case "totalPctDiscount":
                                        totalPctDiscount = Conversion.XmlStringToDecimal(xVariable.Value);
                                        break;
                                    case "loyaltyDiscountWithTax":
                                        LoyaltyDiscountWithTax = Conversion.XmlStringToDecimal(xVariable.Value);
                                        break;
                                    case "loyaltyDiscount":
                                        LoyaltyDiscount = Conversion.XmlStringToDecimal(xVariable.Value);
                                        break;
                                    case "loyaltyPctDiscount":
                                        LoyaltyPctDiscount = Conversion.XmlStringToDecimal(xVariable.Value);
                                        break;
                                    case "lineDiscount":
                                        lineDiscount = Conversion.XmlStringToDecimal(xVariable.Value);
                                        break;
                                    case "lineDiscountWithTax":
                                        lineDiscountWithTax = Conversion.XmlStringToDecimal(xVariable.Value);
                                        break;
                                    case "linePctDiscount":
                                        linePctDiscount = Conversion.XmlStringToDecimal(xVariable.Value);
                                        break;
                                    case "noDiscountAllowed":
                                        noDiscountAllowed = Conversion.XmlStringToBool(xVariable.Value);
                                        break;
                                    case "noManualDiscountAllowed":
                                        noManualDiscountAllowed = Conversion.XmlStringToBool(xVariable.Value);
                                        break;
                                    case "noPriceCalculation":
                                        noPriceCalculation = Conversion.XmlStringToBool(xVariable.Value);
                                        break;
                                    case "discountUnsuccessfullyApplied":
                                        discountUnsuccessfullyApplied = Conversion.XmlStringToBool(xVariable.Value);
                                        break;
                                    case "customerPrice":
                                        customerPrice = Conversion.XmlStringToDecimal(xVariable.Value);
                                        break;
                                    case "promotionPrice":
                                        promotionPrice = Conversion.XmlStringToDecimal(xVariable.Value);
                                        break;
                                    case "basicPrice":
                                        basicPrice = Conversion.XmlStringToDecimal(xVariable.Value);
                                        break;
                                    case "tradeAgreementPrice":
                                        tradeAgreementPrice = Conversion.XmlStringToDecimal(xVariable.Value);
                                        break;
                                    case "customerDiscount":
                                        customerDiscount = Conversion.XmlStringToDecimal(xVariable.Value);
                                        break;
                                    case "customerDiscountWithTax":
                                        customerDiscountWithTax = Conversion.XmlStringToDecimal(xVariable.Value);
                                        break;
                                    case "lineDiscountGroup":
                                        lineDiscountGroup = xVariable.Value;
                                        break;
                                    case "multiLineDiscountGroup":
                                        multiLineDiscountGroup = xVariable.Value;
                                        break;
                                    case "includedInTotalDiscount":
                                        includedInTotalDiscount = Conversion.XmlStringToBool(xVariable.Value);
                                        break;
                                    case "quantityDiscounted":
                                        quantityDiscounted = Conversion.XmlStringToDecimal(xVariable.Value);
                                        break;
                                    case "OriginalQuantity":
                                        OriginalQuantity = Conversion.XmlStringToDecimal(xVariable.Value);
                                        break;
                                    case "LinkedItemOrgQuantity":
                                        LinkedItemOrgQuantity = Conversion.XmlStringToDecimal(xVariable.Value);
                                        break;
                                    case "periodicDiscountOfferId":
                                        periodicDiscountOfferId = xVariable.Value;
                                        break;
                                    case "periodicDiscountOfferName":
                                        periodicDiscountOfferName = xVariable.Value;
                                        break;
                                    case "periodicDiscount":
                                        periodicDiscount = Conversion.XmlStringToDecimal(xVariable.Value);
                                        break;
                                    case "periodicDiscountWithTax":
                                        periodicDiscountWithTax = Conversion.XmlStringToDecimal(xVariable.Value);
                                        break;
                                    case "periodicPctDiscount":
                                        periodicPctDiscount = Conversion.XmlStringToDecimal(xVariable.Value);
                                        break;
                                    case "periodicDiscType":
                                        periodicDiscType = (LineEnums.PeriodicDiscountType)Conversion.XmlStringToInt(xVariable.Value);
                                        break;
                                    case "netAmount":
                                        netAmount = Conversion.XmlStringToDecimal(xVariable.Value);
                                        break;
                                    case "netAmountWithTax":
                                        netAmountWithTax = Conversion.XmlStringToDecimal(xVariable.Value);
                                        break;
                                    case "netAmountWithTaxWithoutDiscountVoucher":
                                        netAmountWithTaxWithoutDiscountVoucher = Conversion.XmlStringToDecimal(xVariable.Value);
                                        break;
                                    case "grossAmount":
                                        grossAmount = Conversion.XmlStringToDecimal(xVariable.Value);
                                        break;
                                    case "grossAmountWithTax":
                                        grossAmountWithTax = Conversion.XmlStringToDecimal(xVariable.Value);
                                        break;
                                    case "taxGroupId":
                                        taxGroupId = xVariable.Value;
                                        break;
                                    case "taxAmount":
                                        taxAmount = Conversion.XmlStringToDecimal(xVariable.Value);
                                        break;
                                    case "taxRatePct":
                                        taxRatePct = Conversion.XmlStringToDecimal(xVariable.Value);
                                        break;
                                    case "taxIncludedInItemPrice":
                                        taxIncludedInItemPrice = Conversion.XmlStringToBool(xVariable.Value);
                                        break;
                                    case "orgTaxIncludedInItemPrice":
                                        orgTaxIncludedInItemPrice = Conversion.XmlStringToBool(xVariable.Value);
                                        break;
                                    case "quantity":
                                        quantity = Conversion.XmlStringToDecimal(xVariable.Value);
                                        break;
                                    case "salesOrderUnitOfMeasure":
                                        salesOrderUnitOfMeasure = xVariable.Value;
                                        break;
                                    case "salesOrderUnitOfMeasureName":
                                        salesOrderUnitOfMeasureName = xVariable.Value;
                                        break;
                                    case "inventOrderUnitOfMeasure":
                                        orgUnitOfMeasure = xVariable.Value;
                                        break;
                                    case "inventOrderUnitOfMeasureName":
                                        orgUnitOfMeasureName = xVariable.Value;
                                        break;
                                    case "unitQuantityFactor":
                                        unitQuantityFactor = Conversion.XmlStringToDecimal(xVariable.Value);
                                        break;
                                    case "unitQuantity":
                                        unitQuantity = Conversion.XmlStringToDecimal(xVariable.Value);
                                        break;
                                    case "unitOfMeasureChanged":
                                        unitOfMeasureChanged = Conversion.XmlStringToBool(xVariable.Value);
                                        break;
                                    case "lineWasDiscounted":
                                        lineWasDiscounted = Conversion.XmlStringToBool(xVariable.Value);
                                        break;
                                    case "comment":
                                        comment = xVariable.Value;
                                        break;
                                    case "wasChanged":
                                        wasChanged = Conversion.XmlStringToBool(xVariable.Value);
                                        break;
                                    case "canBeModified":
                                        canBeModified = Conversion.XmlStringToBool(xVariable.Value);
                                        break;
                                    case "blocked":
                                        blocked = Conversion.XmlStringToBool(xVariable.Value);
                                        break;
                                    case "canBeSold":
                                        canBeSold = Conversion.XmlStringToBool(xVariable.Value);
                                        break;
                                    case "dateToBeBlocked":
                                        dateToBeBlocked = new Date(Conversion.XmlStringToDateTime(xVariable.Value), true);
                                        break;
                                    case "found":
                                        found = Conversion.XmlStringToBool(xVariable.Value);
                                        break;
                                    case "discountsWereRemoved":
                                        discountsWereRemoved = Conversion.XmlStringToBool(xVariable.Value);
                                        break;
                                    case "dateToActivateItem":
                                        dateToActivateItem = new Date(Conversion.XmlStringToDateTime(xVariable.Value));
                                        break;
                                    case "inventSerialId":
                                        inventSerialId = xVariable.Value;
                                        break;
                                    case "rfidTagId":
                                        rfidTagId = xVariable.Value;
                                        break;
                                    case "SerialIdManualInput":
                                        SerialIdManualInput = Conversion.XmlStringToBool(xVariable.Value);
                                        break;
                                    case "dimGroupId":
                                        dimGroupId = xVariable.Value;
                                        break;
                                    case "taxExempt":
                                        TaxExempt = Conversion.XmlStringToBool(xVariable.Value);
                                        break;
                                    case "taxExemptionCode":
                                        TaxExemptionCode = xVariable.Value;
                                        break;
                                    case "TaxLines":
                                        TaxLines = CreateTaxLines(xVariable, errorLogger);
                                        break;
                                    case "DiscountLines":
                                        DiscountLines = CreateDiscountLines(xVariable, errorLogger);
                                        break;
                                    case "InfocodeLines":
                                        InfoCodeLines = CreateInfocodeLines(xVariable, errorLogger);
                                        break;
                                    case "salesTaxGroupId":
                                        salesTaxGroupId = xVariable.Value;
                                        break;
                                    case "headerItemID":
                                        headerItemID = Conversion.XmlStringToGuid(xVariable.Value);
                                        break;
                                    case "masterID":
                                        masterID = Conversion.XmlStringToGuid(xVariable.Value);
                                        break;
                                    case "variantName":
                                        variantName = xVariable.Value;
                                        break;
                                    case "ID":
                                        ID = Conversion.XmlStringToGuid(xVariable.Value);
                                        break;
                                    case "Order":
                                        Order.ToClass(xVariable);
                                        break;
                                    case "returnable":
                                        Returnable = Conversion.XmlStringToBool(xVariable.Value);
                                        break;
                                    default:
                                        base.ToClass(xVariable, errorLogger);
                                        break;
                                }
                            }
                            catch (Exception ex)
                            {
                                errorLogger?.LogMessage(LogMessageType.Error, "BaseSaleItem:" + xVariable.Name, ex);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                errorLogger?.LogMessage(LogMessageType.Error, "BaseSaleItem.ToClass", ex);

                throw;
            }
        }
    }
}
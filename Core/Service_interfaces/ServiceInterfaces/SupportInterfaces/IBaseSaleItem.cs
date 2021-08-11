using System;
using System.Collections.Generic;
using System.Xml.Linq;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.Tax;
using LSOne.DataLayer.BusinessObjects.Transactions.Line;
using LSOne.Services.Interfaces.Enums;
using LSOne.Utilities.DataTypes;
using LSOne.Utilities.ErrorHandling;

namespace LSOne.Services.Interfaces.SupportInterfaces
{
    public interface IBaseSaleItem : ILineItem  
    {
        bool Active { get; }
        void Add(TaxItem taxLineItem);
        /// <summary>
        /// The barcode as stored in the ERP system.
        /// </summary>
        string BarcodeId { get; set; }
        /// <summary>
        /// Price from the item card
        /// </summary>
        decimal BasicPrice { get; set; }
        /// <summary>
        /// ID generated from number sequence to be sent to ERP systems
        /// </summary>
        string PriceID { get; set; }
        /// <summary>
        /// Type of Price. BasePrice = 0, SalesPrice = 1, Promotion = 2
        /// </summary>
        TradeAgreementPriceType PriceType { get; set; }
        /// <summary>
        /// True if the item is can be sold on the POS system.
        /// </summary>
        bool CanBeSold { get; set; }
        /// <summary>
        /// True if the item is blocked from being sold on the POS system.
        /// </summary>
        bool Blocked { get; set; }
        /// <summary>
        /// A variable designed for our partners to use when dealing with fiscal printers.
        /// Dictates whether the line has already been sent to the fiscal printer, etc, and cannot therefore be changed subsequently.
        /// </summary>
        bool CanBeModified { get; set; }
        void ClearDiscountLines();
        /// <summary>
        /// An insertable comment/remark by the clerk
        /// </summary>
        string Comment { get; set; }
        /// <summary>
        /// The cost price of the item.
        /// </summary>
        decimal CostPrice { get; set; }
        /// <summary>
        /// Deserialize the discount lines
        /// </summary>
        List<IDiscountItem> CreateDiscountLines(XElement xItem, IErrorLog errorLogger = null);
        /// <summary>
        /// Deserialize the info code lines
        /// </summary>
        List<InfoCodeLineItem> CreateInfocodeLines(XElement xItems, IErrorLog errorLogger = null);
        /// <summary>
        /// Deserialize the tax lines
        /// </summary>
        List<TaxItem> CreateTaxLines(XElement xItem, IErrorLog errorLogger = null);
        /// <summary>
        /// Total customer discount given in this transaction (excluding tax)
        /// </summary>
        decimal CustomerDiscount { get; set; }
        /// <summary>
        /// Total customer discount given in this transaction (including tax)
        /// </summary>
        decimal CustomerDiscountWithTax { get; set; }
        /// <summary>
        /// Price that is set for the customer only - Trade Agreements
        /// </summary>
        decimal CustomerPrice { get; set; }
        /// <summary>
        /// The date when the items becomes active
        /// </summary>
        Date DateToActivateItem { get; set; }
        /// <summary>
        /// The date when the blocking becomes active
        /// </summary>
        DateTime DateToBeBlocked { get; set; }
        /// <summary>
        /// The item's dimension group id
        /// </summary>
        string DimensionGroupId { get; set; }
        /// <summary>
        /// Whether the discount was removed form the item.
        /// </summary>
        bool DiscountsWereRemoved { get; set; }
        /// <summary>
        /// True if the item was supposed to be discounted but the NoDiscountAllowed property did not allow it.
        /// </summary>
        bool DiscountUnsuccessfullyApplied { get; set; }
        /// <summary>
        /// Whether the item line is to be excluded
        /// </summary>
        bool Excluded { get; }
        void ForceTaxIncludingPriceSetting(bool priceIncludingTax);
        /// <summary>
        /// Is set as true if a item is found.
        /// </summary>
        bool Found { get; set; }
        /// <summary>
        /// The total line amount (price * quantity) excluding tax
        /// </summary>
        decimal GrossAmount { get; set; }
        /// <summary>
        /// The total line amount (price * quantity) including tax
        /// </summary>
        decimal GrossAmountWithTax { get; set; }
        /// <summary>
        /// A collection of all discounts belonging to the salesitem.
        /// </summary>
        IEnumerable<IDiscountItem> IDiscountLines { get; }
        /// <summary>
        /// It is true if the item is included in the calculation of a combined total discount. Should be set when the item info is found.
        /// </summary>
        bool IncludedInTotalDiscount { get; set; }
        IEnumerable<TaxItem> ITaxLines { get; }
        /// <summary>
        /// The retail departement of the item.
        /// </summary>
        string ItemDepartmentId { get; set; }
        /// <summary>
        /// The retail group of the item.
        /// </summary>
        string ItemGroupId { get; set; }
        /// <summary>
        /// The item id as stored in the ERP system.
        /// </summary>
        string ItemId { get; set; }
        /// <summary>
        /// If this item is a variant item, the header ID is the item that this item is a variation of
        /// </summary>
        Guid HeaderItemID { get; set; }
        Guid MasterID { get; set; }
        string VariantName { get; set; }
        /// <summary>
        /// The type of item sold, i.e item, BOM, service.
        /// </summary>
        ItemTypeEnum ItemType { get; set; }
        /// <summary>
        /// Total line discount given in this transaction (excluding the tax). 
        /// </summary>
        decimal LineDiscount { get; set; }
        /// <summary>
        /// The line discount group that the item belongs to. Should be set when the item info is found.
        /// </summary>
        string LineDiscountGroup { get; set; }
        /// <summary>
        /// Total line discount given in this transaction (including the tax)
        /// </summary>
        decimal LineDiscountWithTax { get; set; }
        /// <summary>
        /// The percentage discount given in this line excluding the total discount.
        /// </summary>
        decimal LinePctDiscount { get; set; }
        /// <summary>
        /// Is set to true if a discount was calculated for the item.
        /// </summary>
        bool LineWasDiscounted { get; set; }
        /// <summary>
        /// The original quantity if the line is part of a return transaction
        /// </summary>
        decimal OriginalQuantity { get; set; }
        /// <summary>
        /// The original quantity of the item if it is a linked item.
        /// Has to be saved to that Set Qty and Clear Qty can multiply/divide the qty correctly
        /// </summary>
        decimal LinkedItemOrgQuantity { get; set; }
        /// <summary>
        /// The calculated loyalty discount excluding tax
        /// </summary>
        decimal LoyaltyDiscount { get; set; }
        /// <summary>
        /// The calculated loyalty discount including tax
        /// </summary>
        decimal LoyaltyDiscountWithTax { get; set; }
        /// <summary>
        /// The percentage discount given using a loyalty discounts
        /// </summary>
        decimal LoyaltyPctDiscount { get; set; }
        /// <summary>
        /// The multiline discount group that the item is a part of. Should be set when the item info is found.
        /// </summary>
        string MultiLineDiscountGroup { get; set; }
        /// <summary>
        /// The line amount excluding tax.
        /// </summary>
        decimal NetAmount { get; set; }
        /// <summary>
        /// Used when terminal setting is set to have prices including tax
        /// </summary>
        decimal NetAmountPerUnit { get; }
        /// <summary>
        /// Used when terminal setting is set to have prices including tax
        /// </summary>
        decimal NetAmountWithAllInclusiveTaxPerUnit { get; }
        /// <summary>
        /// The total rounded amount including tax.
        /// </summary>
        decimal NetAmountWithTax { get; set; }
        decimal NetAmountWithTaxWithoutDiscountVoucher { get; set; }
        /// <summary>
        /// It is true if the item should never be discounted. Should be set when the item info is found.
        /// </summary>
        bool NoDiscountAllowed { get; set; }
        /// <summary>
        /// It is true if the item should never be allowed a manual discount.
        /// </summary>
        bool NoManualDiscountAllowed { get; set; }
        /// <summary>
        /// The price cannot be calculated - always use the price as is set
        /// </summary>
        bool NoPriceCalculation { get; set; }
        /// <summary>
        /// The item unit
        /// </summary>
        string OrgUnitOfMeasure { get; set; }
        /// <summary>
        /// The item unit name
        /// </summary>
        string OrgUnitOfMeasureName { get; set; }
        decimal OriginalDiscountVoucherPriceWithTax { get; set; }
        /// <summary>
        /// Stores the original price excluding the tax, if price was manually overrided.
        /// </summary>
        decimal OriginalPrice { get; set; }
        /// <summary>
        /// Stores the original price including the tax, if price was manually overrided.
        /// </summary>
        decimal OriginalPriceWithTax { get; set; }
        /// <summary>
        /// Total periodic discount given in this transaction (tax excluded)
        /// </summary> 
        decimal PeriodicDiscount { get; set; }
        /// <summary>
        /// The periodic discount offer id that the item is connected to.
        /// </summary>
        string PeriodicDiscountOfferId { get; set; }
        /// <summary>
        /// The periodic discount offer name that the item is connected to.
        /// </summary>     
        string PeriodicDiscountOfferName { get; set; }
        /// <summary>
        /// Total periodic discount given in this transaction (tax included)
        /// </summary> 
        decimal PeriodicDiscountWithTax { get; set; }
        /// <summary>
        /// The periodic discount type 0 = None, 1 = Multibuy, 2 = Mix And Match, 3 = Discount offer
        /// </summary>
        LineEnums.PeriodicDiscountType PeriodicDiscType { get; set; }
        /// <summary>
        /// The percentage discount given in this line excluding the total and line discount.
        /// </summary> 
        decimal PeriodicPctDiscount { get; set; }
        /// <summary>
        /// Price excluding the tax.
        /// </summary>
        decimal Price { get; set; }
        /// <summary>
        /// Stores the amount entered when the price was overridden. If you are rebuilding a transaction (e.g. from returns) then this property is not populated.
        /// For returns you need to examine the properties <see cref="PriceOverridden"/> and the fields <see cref="Price"/>, <see cref="PriceWithTax"/>, <see cref="OriginalPrice"/> and <see cref="OriginalPriceWithTax"/>
        /// </summary>
        decimal PriceOverrideAmount { get; set; }
        /// <summary>
        /// It is set to true if price was manually overrided
        /// </summary>
        bool PriceOverridden { get; set; }
        /// <summary>
        /// If a price is given for a for more than one unit the price unit tells how many. (tax excluded)
        /// If the price is given for six units (priceUnit=6) as 100 EUR, then when 12 units are sold the price will be 200 EUR.
        /// </summary>
        decimal PriceUnit { get; set; }
        /// <summary>
        /// If a price is given for a for more than one unit the price unit tells how many. (tax included)
        /// If the price is given for six units (priceUnit=6) as 100 EUR, then when 12 units are sold the price will be 200 EUR.
        /// </summary>
        decimal PriceUnitWithTax { get; set; }
        /// <summary>
        /// Price including the tax.
        /// </summary>
        decimal PriceWithTax { get; set; }
        /// <summary>
        /// Promotion price if it exists
        /// </summary>
        decimal PromotionPrice { get; set; }
        /// <summary>
        /// The quantity sold/returned
        /// </summary>
        decimal Quantity { get; set; }
        /// <summary>
        /// Used in calculation of the periodic discount. Keeps track of how many items have been discounted.
        /// </summary>
        decimal QuantityDiscounted { get; set; }
        void ResetTaxIncludingPrice();
        /// <summary>
        /// The retail group of the item.
        /// </summary>
        string RetailItemGroupId { get; set; }
        /// <summary>
        /// The item's RFID tag
        /// </summary>
        string RFIDTagId { get; set; }
        /// <summary>
        /// If true, the serial number was manually entered
        /// </summary>
        bool SerialIdManualInput { get; set; }
        /// <summary>
        /// The item unit
        /// </summary>
        string SalesOrderUnitOfMeasure { get; set; }
        /// <summary>
        /// The item unit name
        /// </summary>
        string SalesOrderUnitOfMeasureName { get; set; }
        /// <summary>
        /// The item unit
        /// </summary>
        string SalesOrderUnitOfMeasureText { get; set; }
        /// <summary>
        /// Describes a group of sales tax codes
        /// </summary>
        string SalesTaxGroupId { get; set; }
        /// <summary>
        /// The item's serial no.
        /// </summary>
        string SerialId { get; set; }
        /// <summary>
        /// The standard retail price. Used to store a retail price for comparison with the customer price.
        /// </summary>
        decimal StandardRetailPrice { get; set; }
        /// <summary>
        /// The standard retail price uncluding the tax. Used to store a retail price for comparison with the customer price.
        /// </summary>
        decimal StandardRetailPriceWithTax { get; set; }
        /// <summary>
        /// The tax amount for the item.
        /// </summary>
        decimal TaxAmount { get; set; }
        /// <summary>
        /// If true, then this sale item is tax exempt
        /// </summary>
        bool TaxExempt { get; set; }
        /// <summary>
        /// The code entered by the user when the item was tax exempted
        /// </summary>
        string TaxExemptionCode { get; set; }
        /// <summary>
        /// The tax group used for calculation of tax.
        /// </summary>
        string TaxGroupId { get; set; }
        /// <summary>
        /// Does the original price include tax?
        /// </summary>
        bool TaxIncludedInItemPrice { get; set; }
        /// <summary>
        /// Original setting of the <see cref="TaxIncludedInItemPrice"/>
        /// </summary>
        bool OriginalTaxIncludedInItemPrice { get; set; }
        List<TaxItem> TaxLines { get; set; }
        /// <summary>
        /// The taxrate as a percentage, i.e 14,5%. 
        /// </summary>
        decimal TaxRatePct { get; set; }
        /// <summary>
        /// Total discount given in this line minus the linediscount (tax exclued).
        /// The total discount for the transaction containing this line is proportionally distributed among each line of the transaction.
        /// TotalDiscount contains the amount associated to the current line.
        /// </summary>
        decimal TotalDiscount { get; set; }
        /// <summary>
        /// Total discount given in this transaction minus the linediscount (including the tax).
        /// The total discount with tax for the transaction containing this line is proportionally distributed among each line of the transaction.
        /// TotalDiscountWithTax contains the amount associated to the current line.
        /// </summary>
        decimal TotalDiscountWithTax { get; set; }
        /// <summary>
        /// The percentage discount given in this line excluding the linediscount.
        /// </summary>
        decimal TotalPctDiscount { get; set; }
        /// <summary>
        /// Price from the Trade Agreements that does not come from a customer relation
        /// </summary>
        decimal TradeAgreementPrice { get; set; }
        /// <summary>
        /// The transaction that this sale item belongs to
        /// </summary>
        IRetailTransaction Transaction { get; set; }
        /// <summary>
        /// Has the unit of measure been changed for the current sale item?
        /// </summary>
        bool UnitOfMeasureChanged { get; set; }
        /// <summary>
        /// The amount that is used as markup for a specific unit of measure
        /// </summary>
        decimal MarkupAmount { get; set; }
        /// <summary>
        /// The unit quantity
        /// </summary>
        decimal UnitQuantity { get; set; }
        /// <summary>
        /// To be used in CalculateLine when calculating unitQuantity
        /// </summary>
        decimal UnitQuantityFactor { get; set; }
        /// <summary>
        /// It is set to true if the item was changed by price or discount calculation. 
        /// It is set to false in the totalcalulation.
        /// </summary>
        bool WasChanged { get; set; }
        /// <summary>
        /// A collection of all discounts belonging to the salesitem.
        /// </summary>
        List<IDiscountItem> DiscountLines { get; set; }
        /// <summary>
        /// Item information for the Customer order 
        /// </summary>
        IOrderItem Order { get; set; }
        /// <summary>
        /// Indicates wether this item can be returned at the POS
        /// </summary>
        bool Returnable { get; set; }

        /// <summary>
        /// The unique ID of this tender line. This is not the same as <see cref="ILineItem.LineId"/> since this is ID is not tied to the position of this 
        /// line within the transaction. This is not saved to the database and is only used at runtime to uniquely identify the line
        /// </summary>
        Guid ID { get; set; }
    }
}
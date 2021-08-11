using System;
using LSOne.Utilities.DataTypes;
using LSRetail.StoreController.BusinessObjects.ItemMaster.Dimensions;
using LSRetail.StoreController.BusinessObjects.BarCodes;
using LSRetail.StoreController.BusinessObjects.Transactions;

namespace LSRetail.ServiceInterfaces.SupportInterfaces
{
    public enum LineMultilineDiscountOnItem
    {
        None = 0,
        Line = 1,
        Multiline = 2,
        Both = 3,
    }

    public interface ISaleLineItem : IBaseSaleItem
    {
        decimal Quantity { get; set; }
        decimal PriceWithTax { get; set; }
        decimal OriginalDiscountVoucherPriceWithTax { get; set; }
        decimal NetAmount { get; set; }
        decimal NetAmountWithTax { get; set; }
        decimal ReturnableAmount { get; set; }
        string ReturnTransId { get; set; }
        BarCode.BarcodeEntryType IEntryType { get; set; }
        string PeriodicDiscountOfferName { get; set; }
        string PeriodicDiscountOfferId { get; set; }
        bool WasChanged { get; set; }
        decimal LineDiscount { get; set; }
        decimal LineDiscountWithTax { get; set; }
        decimal LinePctDiscount { get; set; }
        decimal PeriodicDiscount { get; set; }
        decimal PeriodicPctDiscount { get; set; }
        decimal TotalDiscount { get; set; }
        decimal TotalPctDiscount { get; set; }
        decimal LoyaltyDiscount { get; set; }
        decimal LoyaltyDiscountWithTax { get; set;}
        decimal LoyaltyPctDiscount { get; set; }
        LineMultilineDiscountOnItem ILineMultiLineDiscOnItem { get; }
        
        decimal Price { get; set; }
        decimal GrossAmount { get; set; }
        decimal GrossAmountWithTax { get; set; }
        bool TaxIncludedInItemPrice { get; set; }
        decimal QuantityDiscounted { get; set; }
        decimal PeriodicDiscountWithTax { get; set; }
        decimal TotalDiscountWithTax { get; set; }
        decimal NetAmountWithTaxWithoutDiscountVoucher { get; set; }
        decimal TaxAmount { get; set; }
        decimal UnitQuantity { get; set; }
        decimal UnitQuantityFactor { get; set; }
        string TaxGroupId { get; set; }
        bool Blocked { get; set; }
        bool Found { get; set; }
        Date DateToActivateItem { get; set; }
        decimal Oiltax { get; set; }


        void ClearCustomerDiscountLines(bool deleteTotalCustDisc);
        void ClearDiscountLines(Type discountTypeToBeCleared);
        
        ISaleLineItem CreateSaleLineItem(ISaleLineItem saleLineItem, RecordIdentifier currencyCode);


        decimal StandardRetailPrice { get; set; }

        decimal PriceUnit { get; set; }

        decimal StandardRetailPriceWithTax { get; set; }

        decimal PriceUnitWithTax { get; set; }

        SalesTransaction.ItemClassTypeEnum ItemClassType { get; set; }

        Dimension Dimension { get; set; }
        string BarcodeId { get; set; }
        bool PriceInBarcode { get; set;}
        string SalesOrderUnitOfMeasure { get; set; }
    }
}

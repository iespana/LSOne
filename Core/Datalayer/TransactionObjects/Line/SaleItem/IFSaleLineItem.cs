using System;

namespace LSOne.DataLayer.TransactionObjects.Line.IFSaleItem
{
    /// <summary>
    /// A standard sale item line in a transaction.
    /// </summary>
    [Serializable]
    public class IFSaleLineItem
    {
        public string ItemID { get; set; }
        public string DepartmentID { get; set; }
        public string ItemGroupID { get; set; }
        public string TaxGroupID { get; set; }
        public string Unit { get; set; }
        public string Currency { get; set; }
        public decimal Price { get; set; }
        public decimal NetPrice { get; set; }
        public decimal Quantity { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal CostAmount { get; set; }
        public decimal NetAmount { get; set; }
        public decimal DiscountAmountFromPrice { get; set; }
        public decimal TotalRoundedAmount { get; set; }
        public decimal LineDiscountAmount { get; set; }
        public decimal CustomerDiscountAmount { get; set; }
        public decimal InfocodeDiscountAmount { get; set; }
        public decimal CustomerInvoiceDiscountAmount { get; set; }
        public decimal UnitQuantity { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal TotalDiscountAmount { get; set; }
        public decimal PeriodiclDiscountAmount { get; set; }
        public decimal WoleDiscountAmountWithTax { get; set; }
        public decimal TotalDiscountAmountWithTax { get; set; }
        public decimal LineDiscountAmountWithTax { get; set; }
        public decimal PeriodicDiscountAmountWithTax { get; set; }
        public decimal ReturnQuantity { get; set; }
        public decimal Oiltax { get; set; }
        public decimal PriceUnit { get; set; }
        public decimal NetAmountIncludingTax { get; set; }
    }
}

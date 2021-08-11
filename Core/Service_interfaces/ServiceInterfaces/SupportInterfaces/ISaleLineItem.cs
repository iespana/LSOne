using System;
using LSOne.DataLayer.BusinessObjects.BarCodes;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.ItemMaster.Dimensions;
using LSOne.DataLayer.BusinessObjects.Transactions;
using LSOne.Services.Interfaces.Enums;
using LSOne.Services.Interfaces.SupportClasses.Employee;
using LSOne.Utilities.DataTypes;
using LSOne.DataLayer.BusinessObjects.Inventory;
using LSOne.DataLayer.BusinessObjects.ItemMaster;

namespace LSOne.Services.Interfaces.SupportInterfaces
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
        RecordIdentifier ActiveHospitalitySalesType { get; set; }
        void Add(ILoyaltyItem loyaltyPoints);
        void Add(ICustomerDiscountItem discountItem);
        void Add(IDiscountItem discountItem);
        void Add(IPeriodicDiscountItem discountItem);
        void ResetLineMultiLineDiscountSettings(ICustomerDiscountItem discountItem);
        void ResetLineMultiLineDiscountSettings();

        DateTime BatchExpDate { get; set; }
        string BatchId { get; set; }
        void ClearCustomerDiscountLines(bool deleteTotalCustDisc);
        void ClearDiscountLines(Type discountTypeToBeCleared);
        int CoverNo { get; set; }
        ISaleLineItem CreateSaleLineItem(ISaleLineItem saleLineItem);
        //Dimension Dimension { get; set; }
        IDiscountVoucherItem DiscountVoucher { get; set; }
        BarCode.BarcodeEntryType EntryType { get; set; }
        string FleetCardNumber { get; set; }
        bool HasDiscounts();
        LineMultilineDiscountOnItem ILineMultiLineDiscOnItem { get; }
        bool IsInfoCodeItem { get; set; }
        bool IsLinkedItem { get; set; }
        bool IsReferencedByLinkItems { get; set; }
        SalesTransaction.ItemClassTypeEnum ItemClassType { get; set; }
        KeyInPrices KeyInPrice { get; set; }
        KeyInQuantities KeyInQuantity { get; set; }
        decimal LastPrintedQty { get; set; }
        LineMultilineDiscountOnItem LineMultiLineDiscOnItem { get; set; }
        int LinkedToLineId { get; set; }
        ILoyaltyItem LoyaltyPoints { get; set; }
        IMenuTypeItem MenuTypeItem { get; set; }
        bool MustKeyInComment { get; set; }
        bool MustSelectUOM { get; set; }
        decimal Oiltax { get; set; }
        bool OriginatesFromForecourt { get; set; }

        /// <summary>
        /// Set by the discount service to block an item sale from setting split items added by the discount service as linked items
        /// </summary>
        bool BlockDiscountLinkItem { get; set; }

        /// <summary>
        /// The index of the payment line on the transaction that was used to pay for this item. This only applies for limited payments.
        /// </summary>
        int PaymentIndex { get; set; }
        bool PriceInBarcode { get; set; }
        SalesTransaction.PrintStatus PrintingStatus { get; set; }
        bool QtyBecomesNegative { get; set; }
        bool ReceiptReturnItem { get; set; }
        bool ReturnItem { get; set; }
        decimal ReturnableAmount { get; set; }
        decimal ReturnedQty { get; set; }
        int ReturnLineId { get; set; }
        string ReturnStoreId { get; set; }
        string ReturnTerminalId { get; set; }
        string ReturnTransId { get; set; } 
        string ReturnReceiptId { get; set; }
        ReasonCode ReasonCode { get; set; }
        Employee SalesPerson { get; set; }
        bool ScaleItem { get; set; }
        int TareWeight { get; set; }
        bool ShouldBeManuallyRemoved { get; set; }
        int SplitGroup { get; set; }
        bool SplitItem { get; set; }
        int SplitLineId { get; set; }
        bool SplitMarked { get; set; }
        int SplitOriginLineNo { get; set; }
        int SplitParentLineId { get; set; }
        RecordIdentifier LimitationMasterID { get; set; }

        string LimitationCode { get; set; }

        SaleType TypeOfSale { get; set; }
        bool WeightInBarcode { get; set; }
        bool WeightManuallyEntered { get; set; }
        bool ZeroPriceValid { get; set; }
        bool UsedForPriceCheck { get; set; }
        RecordIdentifier ValidationPeriod { get; set; }

        /// <summary>
        /// ID of an item used by the KDS system
        /// </summary>
        Guid KdsId { get; set; }

        /// <summary>
        /// Total discount amount applied to the item without rounding (Not saved)
        /// </summary>
        decimal TotalDiscountExact { get; set; }

        /// <summary>
        /// Total discount amount with tax applied to the item without rounding (Not saved)
        /// </summary>
        decimal TotalDiscountWithTaxExact { get; set; }

        /// <summary>
        /// Line discount amount applied to the item without rounding (Not saved)
        /// </summary>
        decimal LineDiscountExact { get; set; }

        /// <summary>
        /// Line discount amount with tax applied to the item without rounding (Not saved)
        /// </summary>
        decimal LineDiscountWithTaxExact { get; set; }

        /// <summary>
        /// Net amount without rounding (Not saved)
        /// </summary>
        decimal NetAmountExact { get; set; }

        /// <summary>
        /// Net amount with tax without rounding (Not saved)
        /// </summary>
        decimal NetAmountWithTaxExact { get; set; }

        /// <summary>
        /// The amount that has been paid for. This is linked to <see cref="PaymentIndex"/> and is only valid when there is a limited payment on the transaction (Not saved). 
        /// If <see cref="PaymentIndex"/> does not point to a valid tender line then this will return 0.
        /// </summary>
        decimal PaymentAmount { get; }        

        /// <summary>
        /// The parent line ID that this line item was created from. This happens when a line item is partially paid with a limited payment and the remaining amount of the item is split into anoter line.
        /// </summary>
        int LimitationSplitParentLineId { get; set; }

        /// <summary>
        /// The line ID of the child item that was split away from this line. This happens when a line item is partially paid with a limited payment and the remaining amount of the item is split into anoter line.
        /// </summary>
        int LimitationSplitChildLineId { get; set; }

        /// <summary>
        /// The active assembly for the current store for the sold item if it is an assembly item
        /// </summary>
        RetailItemAssembly ItemAssembly { get; set; }

        /// <summary>
        /// The active assembly for the current store that the sold item is a component of
        /// </summary>
        RetailItemAssembly ParentAssembly { get; set; }

        /// <summary>
        /// True if an item is a component of an assembly
        /// </summary>
        bool IsAssemblyComponent { get; set; }
        
        /// <summary>
        /// True if an item is an assembly
        /// </summary>
        bool IsAssembly { get; set; }

        /// <summary>
        /// True if this assembly item should calculate it's price normally
        /// </summary>
        /// <returns></returns>
        bool ShouldCalculateAndDisplayAssemblyPrice();

        /// <summary>
        /// True if an assembly should display it's total price from the components if the components are not displayed
        /// </summary>
        /// <param name="assemblyLocation">Assembly display location</param>
        /// <returns></returns>
        bool ShouldDisplayTotalAssemblyComponentPrice(ExpandAssemblyLocation assemblyLocation);

        /// <summary>
        /// ID of the assembly
        /// </summary>
        RecordIdentifier AssemblyID { get; set; }

        /// <summary>
        /// ID of the assembly component
        /// </summary>
        RecordIdentifier AssemblyComponentID { get; set; }

        /// <summary>
        /// Line ID of the parent assembly
        /// </summary>
        int AssemblyParentLineID { get; set; }

        /// <summary>
        /// The production time of an item. This tells how long it takes to cook and prepare the item.
        /// </summary>
        int ProductionTime { get; set; }

        /// <summary>
        /// Returns the net amount for the sale line item, with or without tax.
        /// If the item as an assembly item, this function returns sum of the net amounts for all 
        /// assembly components that are linked to the item that has a price on them.
        /// </summary>
        /// <param name="withTax">if true, the returned net amount is with tax included</param>
        /// <returns>the net amount for the item</returns>
        decimal GetCalculatedNetAmount(bool withTax);

        /// <summary>
        /// Returns the price for the sale line item, with or without tax.
        /// If the item as an assembly item, this function returns sum of the prices for all 
        /// assembly components that are linked to the item that has a price on them.
        /// </summary>
        /// <param name="withTax">if true, the returned price is with tax included</param>
        /// <returns>the price for the item</returns>
        decimal GetCalculatedPrice(bool withTax);

        /// <summary>
        /// Returns the sale line item of the root assembly item on the transaction
        /// for an assembly component sales line, 
        /// i.e. the parent assembly (of the parent assembly etc...) 
        /// of the component item.
        /// </summary>
        /// <returns>The root assembly item on the transaction, or null if no parent assembly item is found</returns>
        ISaleLineItem GetRootAssemblyLineItem();
    }
}

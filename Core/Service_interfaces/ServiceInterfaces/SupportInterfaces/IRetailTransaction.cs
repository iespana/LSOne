using System;
using System.Collections.Generic;
using System.Xml.Linq;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.CustomerOrders;
using LSOne.DataLayer.BusinessObjects.Customers;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.PricesAndDiscounts;
using LSOne.DataLayer.BusinessObjects.Tax;
using LSOne.DataLayer.BusinessObjects.Transactions;
using LSOne.DataLayer.BusinessObjects.Transactions.Line;
using LSOne.Services.Interfaces.SupportClasses;
using LSOne.Services.Interfaces.SupportClasses.Employee;
using LSOne.Services.Interfaces.SupportInterfaces.EFT;
using LSOne.Utilities.DataTypes;

namespace LSOne.Services.Interfaces.SupportInterfaces
{
    public interface IRetailTransaction : IPosTransaction
    {        
        /// <summary>
        /// Inserts a discount voucher item 
        /// </summary>
        /// <param name="discountVoucher">The discount coucher item that is added</param>
        /// <param name="lineId"></param>
        void Add(IDiscountVoucherItem discountVoucher, int lineId);
        /// <summary>
        /// Adds a fuel sale line to the collection of saleitems that belong to this transaction
        /// </summary>
        /// <param name="fuelSaleLineItem">The fuel line that is added</param>
        void Add(IFuelSalesLineItem fuelSaleLineItem);
        /// <summary>
        /// Adds a sale line item to the collection of sale line items that belong to this transaction
        /// If the aggregate items mode is Full or Barcode then the item added doesn't necessarily have to be a
        /// new item line but rather an updated line.
        /// </summary>
        /// <param name="saleLineItem">The sale line that is added</param>
        /// <param name="aggregateItems">Which aggregation mode is to be used when adding the sales line</param>  
        void Add(ISaleLineItem saleLineItem, AggregateItemsModes aggregateItems);
        /// <summary>
        /// Adds a sale line item to the collection of sale line items that belong to this transaction
        /// If the aggregate items mode is Full or Barcode then the item added doesn't necessarily have to be a
        /// new item line but rather an updated line. So the item line is returned through lineIdBeingAdded
        /// </summary>
        /// <param name="saleLineItem">The sale line that is added</param>
        /// <param name="aggregateItems">Which aggregation mode is to be used when adding the sales line</param>
        /// <param name="lineIdBeingAdded">The Line id of the item that was added</param>
        /// <returns>Aggregation performed</returns>
        bool Add(ISaleLineItem saleLineItem, AggregateItemsModes aggregateItems, ref int lineIdBeingAdded);
        /// <summary>
        /// Used when splitting a line to two lines. And lines have to get a new line id.
        /// </summary>
        /// <param name="saleLineItem">The sale line that is added</param>
        /// <param name="lineId">The lineId</param>
        void Add(ISaleLineItem saleLineItem, int lineId);
        /// <summary>
        /// Adds a tendeline to the collection of tenderlines that belong to this transaction
        /// </summary>
        /// <param name="tenderLineItem">The tender line item to be added to the transaction</param>
        void Add(ITenderLineItem tenderLineItem);
        /// <summary>
        /// Adds a sale line item to the collection of sale line items that belong to this transaction
        /// </summary>
        /// <param name="saleLineItem">The sale line item that is added</param>
        void Add(ISaleLineItem saleLineItem);
        /// <summary>
        /// Adds a customer to the current transaction
        /// </summary>
        /// <param name="customer">The customer to be added</param>
        bool Add(Customer customer);
        /// <summary>
        /// Adds a customer to the current transaction, specifying whether it is added during a return transaction
        /// </summary>
        /// <param name="customer">The customer to be added</param>
        /// <param name="returnCustomer">Set to true if this is a return transaction</param>
        bool Add(Customer customer, bool returnCustomer);
        /// <summary>
        /// Adds an infocode to the transaction header
        /// </summary>
        /// <param name="infoCodeLineItem">The infocode line to be added to the transaction</param>
        void Add(InfoCodeLineItem infoCodeLineItem);
        /// <summary>
        /// Adds a customer that will be invoiced for the items.
        /// </summary>
        /// <param name="customer">The customer to be added</param>
        void AddInvoicedCustomer(Customer customer);
        /// <summary>
        /// Adds a total discount amount line to the item lines
        /// </summary>
        /// <param name="banCompoundDiscounts">If true, the total amount discount is not added to sale lines that already have a discount applied</param>
        /// <returns>The discount percentage applied</returns>
        decimal AddTotalDiscAmountLines(bool banCompoundDiscounts);
        /// <summary>
        /// Adds total discount percent lines to the item lines.
        /// </summary>
        void AddTotalDiscPctLines();
        /// <summary>
        /// The total amount posted to customer account.
        /// </summary>
        decimal AmountToAccount { get; set; }
        /// <summary>
        /// The total net amount (grossamount minus discounts) exluding tax. 
        /// </summary>
        decimal BalanceNetAmountWithTax { get; set; }
        /// <summary>
        /// Tells the transaction if a total discount should be calculated
        /// </summary>
        bool CalculateTotalDiscount { get; set; }
        /// <summary>
        /// Clears all discounts of all sale line items
        /// </summary>     
        void ClearAllDiscountLines();
        /// <summary>
        /// Clears all customer discounts of all sale line items
        /// </summary> 
        void ClearCustomerDiscountLines();
        /// <summary>
        /// Clears all line discounts of all sale line items
        /// </summary>
        void ClearLineDiscountLines();
        /// <summary>
        /// Clears all loyalty discount lines of all sale line items
        /// </summary>
        void ClearLoyaltyDiscountLines();
        /// <summary>
        /// Clears all periodic discounts of all sale line items
        /// </summary> 
        void ClearPeriodicDiscountLines();        
        /// <summary>
        /// Clears all total discount lines of all sale line items
        /// </summary>
        void ClearTotalDiscountLines();
        /// <summary>
        /// Clears all manually triggered periodic discount lines of all sale line items
        /// </summary>
        void ClearManuallyTriggeredPeriodicDiscountLines();
        /// <summary>
        /// Clears all total amounts on the transaction header such as net amount, gross amount, tax amount and etc.
        /// </summary>
        void ClearTotalAmounts();
        /// <summary>
        /// Clears the customer from the transaction
        /// </summary>
        void ClearCustomer();
        /// <summary>
        /// Deserializes the suspended transaction answers
        /// </summary>
        /// <param name="xItem">The XElement to be deserialized</param>
        List<SuspendedTransactionAnswer> CreateAnswerLines(XElement xItem);
        /// <summary>
        /// Deserializes the info code line items
        /// </summary>
        /// <param name="xItems">The XElement to be deserialized</param>
        List<InfoCodeLineItem> CreateInfocodeLines(XElement xItems);
        /// <summary>
        /// Deserializes the sale line items
        /// </summary>
        /// <param name="xItems">The XElement to be deserialized</param>
        LinkedList<ISaleLineItem> CreateSaleLineItems(XElement xItems);
        /// <summary>
        /// Deserializes the tax items
        /// </summary>
        /// <param name="xItem">The XElement to be deserialized</param>
        List<TaxItem> CreateTaxLines(XElement xItem);
        /// <summary>
        /// Deserializes the tender line items
        /// </summary>
        /// <param name="xItems">The XElement to be deserialized</param>
        List<ITenderLineItem> CreateTenderLines(XElement xItems);
        /// <summary>
        /// An issued Credit Memo in the transaction
        /// </summary>
        ICreditMemoItem CreditMemoItem { get; set; }
        /// <summary>
        /// Information about the customer on the transaction. This is a read-only property.
        /// In order to set the customer use the Add function
        /// </summary>
        Customer Customer { get; }
        /// <summary>
        /// Is set to true if the customer pays tax, else false.
        /// </summary>
        bool CustomerPaysTax { get; set; }

        /// <summary>
        /// The ID of the purchase order this transaction belongs to
        /// </summary>
        string CustomerPurchRequestId { get; set; }
        /// <summary>
        /// Returns a sale line item for a certain line id.
        /// </summary>
        /// <param name="lineId">The line ID of the item. This corresponds to <see cref="ILineItem.LineId"/>.</param>
        /// <returns>The sale line item.</returns>
        ISaleLineItem GetItem(int lineId);

        /// <summary>
        /// Returns a sale line item for the given line Id.        
        /// </summary>
        /// <param name="lineId">The unique line ID of the item. This corresponds to <see cref="IBaseSaleItem.ID"/></param>
        /// <returns>The sale line item.</returns>
        ISaleLineItem GetItem(Guid lineId);
        /// <summary>
        /// Returns a tender item for a certain tender line id.
        /// </summary>
        /// <param name="lineId">The unique line id of the tender line.</param>
        /// <returns>The sale line item.</returns>
        ITenderLineItem GetTenderItem(int lineId);
        /// <summary>
        /// The total amount excluding tax
        /// </summary>
        decimal GrossAmount { get; set; }
        /// <summary>
        /// The total amount including tax
        /// </summary>
        decimal GrossAmountWithTax { get; set; }
        /// <summary>
        /// The total time the terminal was idle during the transaction.
        /// </summary>
        TimeSpan IdleElapsedTime { get; set; }
        IEnumerable<InfoCodeLineItem> IInfocodeLines { get; }
        /// <summary>
        /// Determines how the linediscount is found/calculated.
        /// </summary>
        LineDiscCalculationTypes ILineDiscCalculationType { get; }
        /// <summary>
        /// Markup information for the transaction
        /// </summary>
        IMarkupItem IMarkupItem { get; }
        /// <summary>
        /// The total amount of income and expense accounts in the transaction
        /// </summary>
        decimal IncomeExpenseAmounts { get; set; }
        /// <summary>
        /// Returns true if the Infocode Id "is needed" i.e. cannot be found already on the transaction header. 
        /// It is used when an Infocode is set to be "once per transaction".
        /// </summary>
        /// <param name="infoCodeId">The Infocode Id to check</param>
        /// <returns>Returns true if the infocode can not be found</returns>
        bool InfoCodeNeeded(string infoCodeId);
        /// <summary>
        /// Inserts a sale line between salelines. Needed when working with item modifing sale items.
        /// </summary>
        /// <param name="currentSaleLineItem">The current saleLineItem after which the new item line should be inserted</param>
        /// <param name="newSaleLineItem">The new saleLineItem.</param>
        void Insert(ISaleLineItem currentSaleLineItem, ISaleLineItem newSaleLineItem);
        /// <summary>
        /// Inserts a sale line between salelines. Needed when working with item modifing sale items.
        /// </summary>
        /// <param name="saleLineItem"></param>
        /// <param name="lineId">The lineid, after the item should be inserted.</param>
        void Insert(ISaleLineItem saleLineItem, int lineId);
        /// <summary>
        /// Comment to be printed on the invoice
        /// </summary>
        string InvoiceComment { get; set; }
        /// <summary>
        /// If the transaction is to be a customer order the <see cref="CustomerOrderItem"/> contains all the information needed
        /// </summary>
        CustomerOrderItem CustomerOrder { get; set; }
        /// <summary>
        /// Information about the customer that will be invoiced for the items.
        /// </summary>
        Customer InvoicedCustomer { get; set; }

        /// <summary>
        /// Gets a generic <see cref="IEnumerable{ISaleLineItem}"/> for all <see cref="ISaleLineItem"/> on the transaction
        /// </summary>
        IEnumerable<ISaleLineItem> ISaleItems { get; }
        /// <summary>
        /// When coming from Split Bill, a transaction can either be a Table transaction or Guest transaction. If true, this
        /// transaction is the table transaction, otherwise this is the guest transaction.
        /// </summary>
        bool IsTableTransaction { get; set; }

        /// <summary>
        /// Gets a generic <see cref="IEnumerable{TaxItem}"/> for all <see cref="TaxItem"/> on the transaction
        /// </summary>
        IEnumerable<TaxItem> ITaxLines { get; }
        /// <summary>
        /// The total time the terminal was handling items.
        /// </summary>
        TimeSpan ItemElapsedTime { get; set; }
        /// <summary>
        /// The number of items that were sold on an item group.
        /// </summary>
        long KeyItemGroupCount { get; set; }
        /// <summary>
        /// The percentage of items that were sold as an item group.
        /// </summary>
        decimal KeyItemGroupPercent { get; set; }
        /// <summary>
        /// Determines how the linediscount is found/calculated.
        /// </summary>
        LineDiscCalculationTypes LineDiscCalculationType { get; set; }
        /// <summary>
        /// The total line discount given in this transaction minus the total discount (tax excluded).
        /// </summary>
        decimal LineDiscount { get; set; }
        /// <summary>
        /// The total line discount given in this transaction minus the total discount (tax included).
        /// </summary>
        decimal LineDiscountWithTax { get; set; }
        /// <summary>
        /// The number of items that were keyed in.
        /// </summary>
        long LineItemsKeyedCount { get; set; }
        /// <summary>
        /// The perenctage of items that were keyed in.
        /// </summary>
        decimal LineItemsKeyedPercent { get; set; }
        /// <summary>
        /// The number of items that were scanned with a multi scanning device
        /// </summary>
        long LineItemsMultiScannedCount { get; set; }
        /// <summary>
        /// The percentage of items that were scanned with a multi scanning device
        /// </summary>
        decimal LineItemsMultiScannedPercent { get; set; }
        /// <summary>
        /// The number of items that were scanned with a single scanning device
        /// </summary>
        long LineItemsSingleScannedCount { get; set; }
        /// <summary>
        /// The percentage of items that were scanned with a single scanning device
        /// </summary>
        decimal LineItemsSingleScannedPercent { get; set; }
        /// <summary>
        /// The total time the terminal was locked during the transaction.
        /// </summary>
        TimeSpan LockElapsedTime { get; set; }
        /// <summary>
        /// Loyalty information for the transaction
        /// </summary>
        ILoyaltyItem LoyaltyItem { get; set; }
        /// <summary>
        /// Markup information for the transaction
        /// </summary>
        IMarkupItem MarkupItem { get; set; }
        /// <summary>
        /// The currently selected menu type of the transaction
        /// </summary>
        IMenuTypeItem MenuTypeItem { get; set; }
        /// <summary>
        /// The total net amount (grossamount minus discounts) exluding tax. 
        /// </summary>
        decimal NetAmount { get; set; }
        /// <summary>
        /// The total net amount (grossamount minus discounts) including tax. 
        /// </summary>
        decimal NetAmountWithTax { get; set; }
        /// <summary>
        /// Sometimes NetAmountWithTax is rounded (eg: in CalculationService, when certain conditions are met).
        /// Because sometimes we want to know when NetAmountWithTax is rounded (eg: when we post transactions to SAP), we store this information in this property.
        /// </summary>
        bool IsNetAmountWithTaxRounded { get; set; }
        /// <summary>
        /// Number of the item lines, not the quantity of the items
        /// </summary>
        decimal NoOfItemLines { get; }
        /// <summary>
        /// Number of items in the transaction - the total quantity.
        /// </summary>
        decimal NoOfItems { get; set; }
        /// <summary>
        /// Number of payment lines.
        /// </summary>
        decimal NoOfPaymentLines { get; }
        /// <summary>
        /// Gets the total number of item lines and payment lines
        /// </summary>
        int NumberOfLines();
        /// <summary>
        /// The total calculated oiltax amount for the fuel items.
        /// </summary>
        decimal Oiltax { get; set; }
        /// <summary>
        /// The original receipt id if the transaction is being returned
        /// </summary>
        string OrgReceiptId { get; set; }
        /// <summary>
        /// The original store id if the transaction is being returned
        /// </summary>
        string OrgStore { get; set; }
        /// <summary>
        /// The original terminal id if the transaction is being returned
        /// </summary>
        string OrgTerminal { get; set; }
        /// <summary>
        /// The original transaction id if the transaction is being returned
        /// </summary>
        string OrgTransactionId { get; set; }
        /// <summary>
        /// The ID of the tax group that should be used when calculating Tax if UseOverrideTaxGroup has been set to true.
        /// </summary>
        string OverrideTaxGroup { get; set; }
        /// <summary>
        /// An object which can hold any information that localization might want to add to the RetailTransaction
        /// </summary>
        PartnerObjectBase PartnerObject { get; set; }

        /// <summary>
        /// The element containing the serialized <see cref="PartnerObject"/>
        /// </summary>
        XElement PartnerXElement { get; }
        /// <summary>
        /// The total payment including the vat
        /// </summary>
        decimal Payment { get; set; }
        /// <summary>
        /// Infomation about periodic discount validation period
        /// </summary>
        IPeriod Period { get; }
        /// <summary>
        /// Infomation for periodic discount.
        /// </summary>
        IPeriodicDiscount PeriodicDiscount { get; }
        /// <summary>
        /// List of manually triggered periodic discounts 
        /// </summary>
        List<DataEntity> ManualPeriodicDiscounts { get; set; }
        /// <summary>
        /// Coupons used in the transaction
        /// </summary>
        List<Coupon> Coupons { get; set; }
        /// <summary>
        /// Coupon items used in the transaction (needed when coupons have retail groups on them)
        /// </summary>
        List<CouponItem> CouponItems { get; set; }
        /// <summary>
        /// The total periodic discount given in this transaction (tax exluded).
        /// </summary>
        decimal PeriodicDiscountAmount { get; set; }
        /// <summary>
        /// The total periodic discount given in this transaction (tax included)
        /// </summary>
        decimal PeriodicDiscountWithTax { get; set; }
        /// <summary>
        /// Specifies whether the sale should be posted as an invoice or a shipment. Only used if customer is selected.
        /// </summary>
        bool PostAsShipment { get; set; }
        /// <summary>
        /// Used to override the price manually.
        /// </summary>
        /// <param name="saleLineItem">The unique line id of the item line.</param>
        /// <param name="amount">The override price, including the tax.</param>
        ISaleLineItem PriceOverride(ISaleLineItem saleLineItem, decimal amount);
        /// <summary>
        /// The id of the number sequence for the receipt id
        /// </summary>
        string ReceiptIdNumberSequence { get; set; }
        /// <summary>
        /// Id of a receiption for items that will be refunded.
        /// </summary>
        string RefundReceiptId { get; set; }
        /// <summary>
        /// Removes a tender line item
        /// </summary>
        /// <param name="tender">The tender line item to be removed</param>
        /// <param name="updateLineIDs">True if we want to reset the line ids starting from 1</param>
        void Remove(ITenderLineItem tender, bool updateLineIDs);
        /// <summary>
        /// Removes a sale line item
        /// </summary>
        /// <param name="item">The sale line item to be removed</param>
        /// <param name="updateLineIDs">True if we want to reset the line ids starting from 1</param>
        void Remove(ISaleLineItem item, bool updateLineIDs);
        /// <summary>
        /// Rounding difference between summed up item lines and what the customer should be charged
        /// </summary>
        decimal RoundingDifference { get; set; }
        /// <summary>
        /// Rouding difference between payment and sales amount. Sales can be 9,99 but payment can be 10
        /// </summary>
        decimal RoundingSalePmtDiff { get; set; }
        /// <summary>
        /// True if the sale is a credit sale.
        /// </summary>
        bool SaleIsReturnSale { get; set; }
        /// <summary>
        /// The total amount of sales invoice payments in the transaction
        /// </summary>
        decimal SalesInvoiceAmounts { get; set; }
        /// <summary>
        /// The total amount of sales orders payments in the transaction
        /// </summary>
        decimal SalesOrderAmounts { get; set; }
        /// <summary>
        /// The sales person if other than the cashier on the transaction
        /// </summary>
        Employee SalesPerson { get; set; }
        /// <summary>
        /// Used to set the cost price of a sale line item
        /// </summary>
        /// <param name="saleLineItem">The sale line item to be changed</param>
        /// <param name="amount">The new cost price amount</param>
        /// <returns>The sale line item that was changed</returns>
        ISaleLineItem SetCostPrice(ISaleLineItem saleLineItem, decimal amount);
        /// <summary>
        /// Sets the RetailTransaction.NetAmountWithTax. Should only be used in Calculation service when calculating the remaining payment after a partial payment has been made.
        /// </summary>
        void SetNetAmountWithTax(decimal NetAmountWithTax, bool CalculateTransSalePmtDiff);
        /// <summary>
        /// Sets the amount given as a total discount for the transaction
        /// </summary>
        /// <param name="totalDiscountAmount">The amount given as a total discount.</param>
        void SetTotalDiscAmount(decimal totalDiscountAmount);
        /// <summary>
        /// Sets the total percentage discount given for the transaction
        /// </summary>
        /// <param name="totalDiscountPercentage">The percentage discount</param>
        void SetTotalDiscPercent(decimal totalDiscountPercentage);

        /// <summary>
        /// An ID that identifies which split this transaction belongs to. This is used in split-bill.
        /// </summary>
        RecordIdentifier SplitID { get; set; }
        /// <summary>
        /// True if this transaction was created as a split from another transaction
        /// </summary>
        bool SplitTransaction { get; set; }

        /// <summary>
        /// A description of where the transaction was suspended, i.e. the terminal number or if it was suspended centrally
        /// </summary>
        string SuspendDestination { get; set; }

        /// <summary>
        /// A list of answers the user entered when the transaction was suspended
        /// </summary>
        List<SuspendedTransactionAnswer> SuspendTransactionAnswers { get; set; }        
        /// <summary>
        /// The total tax in transaction
        /// </summary>
        decimal TaxAmount { get; set; }
        /// <summary>
        /// Tells the transaction whether it should be tax exempted
        /// </summary>
        bool TaxExempt { get; set; }
        /// <summary>
        /// Does the BO system provide the price including a tax. 
        /// </summary>
        bool TaxIncludedInPrice { get; set; }
        /// <summary>
        /// Indicates the original "price includes tax" setting. If <see cref="IRetailTransaction.TaxIncludedInPrice"/> is not the same as <see cref="IRetailTransaction.OrgTaxIncludedInPrice"/> 
        /// then that indicates that something changed (i.e. customer added) that has changed the calculation of this transaction.
        /// </summary>
        bool OrgTaxIncludedInPrice { get; }
        /// <summary>
        /// The total time the terminal was handling payments.
        /// </summary>
        TimeSpan TenderElapsedTime { get; set; }
        /// <summary>
        /// The time of the first payment.
        /// </summary>
        int TimeWhenTotalPressed { get; set; }
        /// <summary>
        /// The total discount given in this transaction excluding the linediscount (tax excluded).
        /// </summary>
        decimal TotalDiscount { get; set; }
        /// <summary>
        /// The total discount given in this transaction excluding the linediscount (tax included).
        /// </summary>
        decimal TotalDiscountWithTax { get; set; }
        /// <summary>
        /// The total amount discount given manually for the total discount.
        /// </summary>
        decimal TotalManualDiscountAmount { get; set; }
        /// <summary>
        /// The percentage discount given manually for the total discount.
        /// </summary>
        decimal TotalManualPctDiscount { get; }
        /// <summary>
        /// The tax exemption code entered by the user when running the TaxExemptTransaction operation.
        /// This field is not saved to the database, it is copied to TaxItem.TaxExemptionCode
        /// </summary>
        string TransactionTaxExemptionCode { get; set; }
        /// <summary>
        /// The difference between payment and grossAmount plus rounded
        /// </summary>
        decimal TransSalePmtDiff { get; set; }
        /// <summary>
        /// Same as TransSalePmtDiff, but calculated for the ongoing payment transaction
        /// </summary>
        /// <remarks>
        /// This property is useful when we have the following scenario:
        ///     One transaction with one item, price with tax = $10
        ///     Pay without limitation: $5 => TransSalePmtDiff = $10 - $5 = $5
        ///     Pay with limitation the remaining part 
        ///         => the item becomes tax exempt during the process (even though only 50% of its quantity should become tax exempt) 
        ///         => total to pay: $9.52 (it should be $9.76)
        ///         => TransSalePmtDiff = $9.52 - $5 = $4.52 which is wrong
        ///         we should be able to pay 4.76 because only half of the item is tax exempt
        ///         So here TransSalePmtDiffForCurrentPaymentOperation comes into play; TransSalePmtDiffForCurrentPaymentOperation = $4.76
        /// </remarks>
        decimal TransSalePmtDiffForCurrentPaymentOperation { get; set; }
        
        /// <summary>
        /// Unmarks the given items by setting <see cref="ISaleLineItem.SplitMarked"/> as <see langword="false"/>
        /// </summary>
        /// <param name="SelectedLineIds">A list containing the line IDs of the items to unmark</param>
        /// <param name="UnMarkAll">If true, then all items on the transaction are unmarked</param>
        void UnMarkSelectedItems(List<int> SelectedLineIds, bool UnMarkAll);
        /// <summary>
        /// Updates all total amounts on the transaction header such as net amount, gross amount, tax amount etc.
        /// </summary>
        /// <param name="saleLineItem">Sale line item with the values to be added to the total amounts</param>
        void UpdateTotalAmounts(ISaleLineItem saleLineItem);
        /// <summary>
        /// Used to indicate if the POS should use the OverrideTaxGroup tax group code when calculating tax.
        /// </summary>
        bool UseOverrideTaxGroup { get; set; }
        /// <summary>
        /// This is used to tell the Tax service whether it should use the tax group defined on the Store or
        /// the tax group defined on the Customer when a customer has been added to the transaction.
        /// </summary>
        UseTaxGroupFromEnum UseTaxGroupFrom { get; set; }
        /// <summary>
        /// Tells whether the TransactionService should be used or not (in connection with returning a transaction)
        /// </summary>
        bool UseTransactionService { get; set; }
        /// <summary>
        /// Voids an item line if it is not already voided, else if the void flag is on, it takes the void flag off.
        /// </summary>
        /// <param name="lineId">The unique line id of the item</param>
        string VoidItemLine(int lineId);
        /// <summary>
        /// Voids a payment line on the transaction
        /// </summary>
        /// <param name="lineId">The tender line id to be voided</param>
        /// <returns>The tender line that was voided</returns>
        ITenderLineItem VoidPaymentLine(int lineId);
        /// <summary>
        /// Contains the tender line items
        /// </summary>
        List<ITenderLineItem> TenderLines { get; set; }

        /// <summary>
        /// The list of all <see cref="ISaleLineItem"/> on the transaction
        /// </summary>
        LinkedList<ISaleLineItem> SaleItems { get; set; }

        /// <summary>
        /// A list of all <see cref="InfoCodeLineItem"/> on the transaction
        /// </summary>
        List<InfoCodeLineItem> InfoCodeLines { get; set; }

        /// <summary>
        /// A list of all <see cref="TaxItem"/> on the transaction
        /// </summary>
        List<TaxItem> TaxLines { get; set; }

        /// <summary>
        /// Returns the topmost limitation split parent item of the given line item.
        /// </summary>
        /// <param name="currentItem">The current child- or parent item</param>
        /// <returns>The top most parent item or the same item if it is already the parent item or not part of a limited payment split</returns>
        ISaleLineItem GetTopMostLimitationSplitParentItem(ISaleLineItem currentItem);

        /// <summary>
        /// Tells whether this transaction is a return transaction or if it's a credit sale (a return transaction without any original transaction).
        /// </summary>
        bool IsReturnTransaction { get; }

        /// <summary>
        /// The ID of the inventory adjustment (return, reserve) that this transaction is related to. For example when making a customer order an item might be included in a reservation
        /// </summary>
        string JournalID { get; set; }

        /// <summary>
        /// Adds the discount amounts from the given <see cref="ISaleLineItem"/> to the transaction totals
        /// </summary>
        /// <param name="saleLineItem">The sale item that should be added to the discount totals</param>
        void UpdateDiscountAmounts(ISaleLineItem saleLineItem);

        /// <summary>
        /// Gets or sets the implementation of <see cref="IEFTTransactionExtraInfo"/>. This will be persisted on the transaction if it is not <see langword="null"/>
        /// </summary>
        IEFTTransactionExtraInfo EFTTransactionExtraInfo { get; set; }

        /// <summary>
        /// The element containing the serialized <see cref="IEFTTransactionExtraInfo"/>
        /// </summary>
        XElement EFTTransactionExtraInfoXElement { get; }

        /// <summary>
        /// Uniqure ID of the order sent to KDS
        /// </summary>
        Guid KDSOrderID { get; set; }
    }
}
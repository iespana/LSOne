using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Xml.Linq;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.BarCodes;
using LSOne.DataLayer.BusinessObjects.CustomerOrders;
using LSOne.DataLayer.BusinessObjects.Customers;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.Hospitality;
using LSOne.DataLayer.BusinessObjects.PricesAndDiscounts;
using LSOne.DataLayer.BusinessObjects.StoreManagement;
using LSOne.DataLayer.BusinessObjects.Tax;
using LSOne.DataLayer.BusinessObjects.Transactions;
using LSOne.DataLayer.BusinessObjects.Transactions.Line;
using LSOne.DataLayer.TransactionObjects.DiscountItems;
using LSOne.DataLayer.TransactionObjects.Line.CreditMemoItem;
using LSOne.DataLayer.TransactionObjects.Line.GiftCertificateItem;
using LSOne.DataLayer.TransactionObjects.Line.Hospitality;
using LSOne.DataLayer.TransactionObjects.Line.IncomeExpenseItem;
using LSOne.DataLayer.TransactionObjects.Line.Loyalty;
using LSOne.DataLayer.TransactionObjects.Line.MarkupItem;
using LSOne.DataLayer.TransactionObjects.Line.SaleItem;
using LSOne.DataLayer.TransactionObjects.Line.SalesInvoiceItem;
using LSOne.DataLayer.TransactionObjects.Line.SalesOrderItem;
using LSOne.DataLayer.TransactionObjects.Line.TenderItem;
using LSOne.DataLayer.TransactionObjects.MemoryTables;
using LSOne.DataLayer.TransactionObjects.Receipts;
using LSOne.Services.Interfaces;
using LSOne.Services.Interfaces.SupportClasses;
using LSOne.Services.Interfaces.SupportClasses.Employee;
using LSOne.Services.Interfaces.SupportInterfaces;
using LSOne.Services.Interfaces.SupportInterfaces.EFT;
using LSOne.Utilities.Attributes;
using LSOne.Utilities.DataTypes;
using LSOne.Utilities.ErrorHandling;

namespace LSOne.DataLayer.TransactionObjects
{
    /// <summary>
    /// The standard sale or return transaction, containing items and payments.
    /// </summary>
    [Serializable]
    public class RetailTransaction : PosTransaction, IRetailTransaction
    {
        #region Member variables
        //Customer
        private Customer customer;              //Information about the customer that receives/buys the items.
        private Customer invoicedCustomer;      //Information about the customer that will be invoiced for the items.
        private bool customerPaysTax;           //Is set to true if the customer pays tax, else false.
        private string customerPurchRequestId = "";   //The reference number from the customer's firm (�sl. bei�ni).
        private LineDiscCalculationTypes lineDiscCalculationType; //Determines how the linediscount is found/calculated.
        private decimal amountToAccount;        //The total amount posted to customer account.
        //Tax
        private bool taxIncludedInPrice;        //Does the BO system provide the price including a tax.
        private bool orgTaxIncludedInPrice;     //Necessary due to customer being able to switch the calculation method. private variable
        private bool taxSettingsChanged;        //Necessary due to customer being able to switch the calculation method. private variable
        private decimal taxAmount;              //The total tax in transaction
        //Amounts
        private decimal balanceNetAmountWithTax;//The total net amount rounded to an amount that the customer is capable of paying considering the currency
        private decimal netAmount;              //The total net amount(grossamount minus discounts) exluding tax 
        private decimal netAmountWithTax;
        private bool isNetAmountWithTaxRounded; //Sometimes NetAmountWithTax is rounded; because sometimes we want to know when NetAmountWithTax is rounded (eg: when we post transactions to SAP), we keep this information in this field

        //The total net amount(grossamount minus discounts) including tax
        private decimal netAmountWithTaxWithoutDiscountVoucher;       //The total net amount(grossamount minus discounts) including tax
        private decimal grossAmount;            //The total amount excluding tax;
        private decimal grossAmountWithTax;     //The total amount including tax;
        private decimal payment;                //The total payment minus (amountWithTax plus rounded) should be zero
        private decimal lineDiscount;           //The total line discount given in this transaction minus the total discount(tax excluded).
        private decimal lineDiscountWithTax;    //The total line discount given in this transaction minus the total discount(tax included).
        private decimal periodicDiscountAmnt;   //The total periodic discount given in this transaction (tax exluded).
        private decimal periodicDiscountWithTax;//The total periodic discount given in this transaction (tax included)
        private decimal totalDiscount;          //The total discount given in this transaction minus the linediscount(tax excluded).
        private decimal totalDiscountWithTax;   //The total discount given in this transaction minus the linediscount(tax included).
        private decimal totalManualPctDiscount; //The percentage discount given manually for the total discount.
        private decimal totalManualDiscountAmount; //The total amount discount given manually for the total discount.
        private bool calculateTotalDiscount = false;  //If this is set to true, when total discount is needs to be calculated.
        private decimal roundingDifference;     //Rounding difference between summed up item lines and what the customer should be charged.
        private decimal roundingSalePmtDiff;    //Rouding difference between payment and sales amount. Sales can be 9,99 but payment can be 10
        private decimal transSalePmtDiff;       //If there is a difference between payment and sales amount, the difference is registered in this field. The pos should not allow posting if this field is not zero
        private decimal totalSalesInvoice;      //The total amount of sales invoices paid in the transaction
        private decimal totalSalesOrder;        //The total amount of sales orders paid in the transaction
        private decimal totalIncomeExpense;     //The tatal amount of income and expense accounts in the transaction
       
        //TotalLines
        private decimal noOfItems;              //Number of items in the transaction - the total quantity.
        //Sale
        private bool saleIsReturnSale;          //True if the sale is a credit sale.
        private bool postAsShipment;            //If the sale should be posted as an invoice or a shipment. Only used if customer is selected        
        ////Refund
        private string refundReceiptId = "";         //Id of a receiption for items that will be refunded.
        private string journalId;               //ID of the journal created when returning items with parked inventory action        
        //TimeOfTransaction
        private int timeWhenTotalPressed;       //The time of the first payment
        //Statistics
        private TimeSpan itemElapsedTime;       //The total time the terminal was handling items.
        private TimeSpan tenderElapsedTime;     //The total time the terminal was handling payments.
        private TimeSpan idleElapsedTime;       //The total time the terminal was idle during the transaction.
        private TimeSpan lockElapsedTime;       //The total time the terminal was locked during the transaction.
        private Int64 lineItemsSingleScannedCount;//The number of items that where scanned
        private decimal lineItemsSingleScannedPercent;//The percentage of items that where scanned
        private Int64 lineItemsMultiScannedCount;      //The number of items that where scanned
        private decimal lineItemsMultiScannedPercent;//The percentage of items that where scanned
        private Int64 lineItemsKeyedCount;        //The number of items that where keyed in
        private decimal lineItemsKeyedPercent;  //The perenctage of items that where keyed in
        private Int64 keyItemGroupCount;          //The number of items that where sold on an item group
        private decimal keyItemGroupPercent;    //The percentage of items that where sold as an item group
        //Memory tables used for calculation of discount 
        //Set to tune up performance.
        private Period period;                  //Class to store infomation if period is valid or not.
        private PeriodicDiscount periodicDiscount; //Class to store infomation for periodic discount.
        private List<DataEntity> manualPeriodicDiscounts; //Periodic discounts that have been triggered manually 
        private List<Coupon> coupons; //Coupons used on the transaction
        private List<CouponItem> couponItems; //CouponItems for the coupons (needed for coupons with retail groups) 
        private decimal oiltax;                 //The total oiltax for the item
        private string suspendDestination = "";
        private bool splitTransaction = false;  // If this transaction was created as a split from another transaction
        //Loyalty
        private LoyaltyItem loyaltyItem;        //Loyalty information for the transaction

        //Markup
        private MarkupItem markupItem;          //Markup information for the transaction

        // Issued Credit Memo
        private CreditMemoItem creditMemoItem;  //An issued credit memo in the transaction.
                
        //The id of the number sequence for the receipt ids
        private string receiptIdNumberSequence = "";

        //Invoice Comment to be printed on the invoice
        private string invoiceComment = "";          //Comment to be printed on the invoice  

        private PartnerObjectBase partnerObject;
        private XElement partnerXElement;

        private XElement eftTransactionExtraInfoXElement;

        ///  <inheritdoc/>
        public string OrgTransactionId { get; set; }

        ///  <inheritdoc/>
        public string OrgStore { get; set; }

        ///  <inheritdoc/>
        public string OrgTerminal { get; set; }

        ///  <inheritdoc/>
        public string OrgReceiptId { get; set; }

        //Tells whether the TransactionService should be used or not (in connection with returning a transaction)
        private bool useTransactionService = false; 

        ///  <inheritdoc/>
        public IMenuTypeItem MenuTypeItem{get;set;}        

        #endregion

        #region Properties
        ///  <inheritdoc/>
        public Customer Customer
        {
            get { return customer; }
            //set { customer = value; } 
        }

        ///  <inheritdoc/>
        public Customer InvoicedCustomer
        {
            get { return invoicedCustomer; }
            set { invoicedCustomer = value; }
        }

        ///  <inheritdoc/>
        public bool CustomerPaysTax
        {
            get { return customerPaysTax; }
            set { customerPaysTax = value; }
        }

        ///  <inheritdoc/>
        public string CustomerPurchRequestId
        {
            get { return customerPurchRequestId; }
            set { customerPurchRequestId = value; }
        }

        ///  <inheritdoc/>
        public LineDiscCalculationTypes LineDiscCalculationType
        {
            get { return lineDiscCalculationType; }
            set { lineDiscCalculationType = value; }
        }

        ///  <inheritdoc/>
        public LineDiscCalculationTypes ILineDiscCalculationType
        {
            get { return lineDiscCalculationType; }
            //set { lineDiscCalculationType = value; }
        }

        ///  <inheritdoc/>
        public bool TaxIncludedInPrice
        {
            get { return taxIncludedInPrice; }
            set { taxIncludedInPrice = value; }
        }

        ///  <inheritdoc/>
        public bool OrgTaxIncludedInPrice => orgTaxIncludedInPrice;        

        ///  <inheritdoc/>
        public decimal TaxAmount
        {
            get { return taxAmount; }
            set { taxAmount = value; }
        }

        ///  <inheritdoc/>
        public decimal BalanceNetAmountWithTax
        {
            get { return balanceNetAmountWithTax; }
            set { balanceNetAmountWithTax = value; }
        }
        
        ///  <inheritdoc/>
        public decimal NetAmount
        {
            get { return netAmount; }
            set { netAmount = value; }
        }

        ///  <inheritdoc/>
        public decimal NetAmountWithTax
        {
            get { return netAmountWithTax; }
            set { 
                    netAmountWithTax = value;
                    transSalePmtDiff = (netAmountWithTax + roundingDifference) - payment;
                }
        }

        ///  <inheritdoc/>
        public bool IsNetAmountWithTaxRounded
        {
            get { return isNetAmountWithTaxRounded; }
            set { isNetAmountWithTaxRounded = value; }
        }

        ///  <inheritdoc/>
        public decimal GrossAmount
        {
            get { return grossAmount; }
            set { grossAmount = value; }
        }

        ///  <inheritdoc/>
        public decimal GrossAmountWithTax
        {
            get { return grossAmountWithTax; }
            set { grossAmountWithTax = value; }
        }

        ///  <inheritdoc/>
        public decimal Payment
        {
            get { return payment; }
            set { 
                    payment = value;
                    transSalePmtDiff = (netAmountWithTax + roundingDifference) - payment;
                }
        }

        ///  <inheritdoc/>
        public decimal LineDiscount
        {
            get { return lineDiscount; }
            set { lineDiscount = value; }
        }

        ///  <inheritdoc/>
        public decimal LineDiscountWithTax
        {
            get { return lineDiscountWithTax; }
            set { lineDiscountWithTax = value; }
        }

        ///  <inheritdoc/>
        public decimal PeriodicDiscountAmount
        {
            get { return periodicDiscountAmnt; }
            set { periodicDiscountAmnt = value; }
        }

        ///  <inheritdoc/>
        public decimal PeriodicDiscountWithTax
        {
            get { return periodicDiscountWithTax; }
            set { periodicDiscountWithTax = value; }
        }

        ///  <inheritdoc/>
        public decimal TotalDiscount
        {
            get { return totalDiscount; }
            set { totalDiscount = value; }
        }

        ///  <inheritdoc/>
        public decimal TotalDiscountWithTax
        {
            get { return totalDiscountWithTax; }
            set { totalDiscountWithTax = value; }
        }

        ///  <inheritdoc/>
        public decimal TotalManualPctDiscount
        {
            get { return totalManualPctDiscount; }
        }

        ///  <inheritdoc/>
        public decimal TotalManualDiscountAmount
        {
            get { return totalManualDiscountAmount; }
            set { totalManualDiscountAmount = value; }
        }

        ///  <inheritdoc/>
        public decimal RoundingDifference
        {
            get { return roundingDifference; }
            set { roundingDifference = value; }
        }

        ///  <inheritdoc/>
        public decimal RoundingSalePmtDiff
        {
            get { return roundingSalePmtDiff; }
            set { roundingSalePmtDiff = value; }
        }
        
        ///  <inheritdoc/>
        public decimal TransSalePmtDiff
        {
            get { return transSalePmtDiff; }
            set { transSalePmtDiff = value; }
        }

        ///  <inheritdoc/>
        public decimal TransSalePmtDiffForCurrentPaymentOperation { get; set; }

        ///  <inheritdoc/>
        public decimal NoOfItemLines
        {
            get { return this.SaleItems.Count;}
        }

        ///  <inheritdoc/>
        public decimal NoOfPaymentLines
        {
            get { return this.TenderLines.Count; }
        }

        ///  <inheritdoc/>
        public decimal NoOfItems
        {
            get { return noOfItems; }
            set { noOfItems = value; }
        }

        ///  <inheritdoc/>
        public bool SaleIsReturnSale
        {
            get { return saleIsReturnSale; }
            set { saleIsReturnSale = value; }
        }

        ///  <inheritdoc/>
        public bool PostAsShipment
        {
            get { return postAsShipment; }
            set { postAsShipment = value; }
        }

        ///  <inheritdoc/>
        public Employee SalesPerson { get; set; }
        
        ///  <inheritdoc/>
        public string RefundReceiptId
        {
            get { return refundReceiptId; }
            set { refundReceiptId = value; }
        }

        ///  <inheritdoc/>
        public string JournalID
        {
            get { return journalId; }
            set { journalId = value; }
        }                

        ///  <inheritdoc/>
        public int TimeWhenTotalPressed
        {
            get { return timeWhenTotalPressed; }
            set { timeWhenTotalPressed = value; }
        }

        ///  <inheritdoc/>
        public TimeSpan ItemElapsedTime
        {
            get { return itemElapsedTime; }
            set { itemElapsedTime = value; }
        }

        ///  <inheritdoc/>
        public TimeSpan TenderElapsedTime
        {
            get { return tenderElapsedTime; }
            set { tenderElapsedTime = value; }
        }

        ///  <inheritdoc/>
        public TimeSpan IdleElapsedTime
        {
            get { return idleElapsedTime; }
            set { idleElapsedTime = value; }
        }

        ///  <inheritdoc/>
        public TimeSpan LockElapsedTime
        {
            get { return lockElapsedTime; }
            set { lockElapsedTime = value; }
        }

        ///  <inheritdoc/>
        public long LineItemsSingleScannedCount
        {
            get { return lineItemsSingleScannedCount; }
            set { lineItemsSingleScannedCount = value; }
        }

        ///  <inheritdoc/>
        public long LineItemsMultiScannedCount
        {
            get { return lineItemsMultiScannedCount; }
            set { lineItemsMultiScannedCount = value; }
        }

        ///  <inheritdoc/>
        public decimal LineItemsSingleScannedPercent
        {
            get { return lineItemsSingleScannedPercent; }
            set { lineItemsSingleScannedPercent = value; }
        }

        ///  <inheritdoc/>
        public decimal LineItemsMultiScannedPercent
        {
            get { return lineItemsMultiScannedPercent; }
            set { lineItemsMultiScannedPercent = value; }
        }

        ///  <inheritdoc/>
        public long LineItemsKeyedCount
        {
            get { return lineItemsKeyedCount; }
            set { lineItemsKeyedCount = value; }
        }

        ///  <inheritdoc/>
        public decimal LineItemsKeyedPercent
        {
            get { return lineItemsKeyedPercent; }
            set { lineItemsKeyedPercent = value; }
        }

        ///  <inheritdoc/>
        public long KeyItemGroupCount
        {
            get { return keyItemGroupCount; }
            set { keyItemGroupCount = value; }
        }

        ///  <inheritdoc/>
        public decimal KeyItemGroupPercent
        {
            get { return keyItemGroupPercent; }
            set { keyItemGroupPercent = value; }
        }

        ///  <inheritdoc/>
        public IPeriod Period
        {
            get { return period; }
        }

        ///  <inheritdoc/>
        public IPeriodicDiscount PeriodicDiscount
        {
            get { return periodicDiscount; }
        }

        ///  <inheritdoc/>
        public List<DataEntity> ManualPeriodicDiscounts
        {
            get { return manualPeriodicDiscounts; }
            set { manualPeriodicDiscounts = value; }
        }

        ///  <inheritdoc/>
        public List<Coupon> Coupons
        {
            get { return coupons; } 
            set { coupons = value; }
        }

        ///  <inheritdoc/>
        public List<CouponItem> CouponItems
        {
            get { return couponItems; }
            set { couponItems = value; }
        }

        ///  <inheritdoc/>
        public ILoyaltyItem LoyaltyItem
        {
            get { return loyaltyItem; }
            set { loyaltyItem = (LoyaltyItem)value; }
        }

        ///  <inheritdoc/>
        public IMarkupItem MarkupItem
        {
            get { return markupItem; }
            set { markupItem = (MarkupItem)value; }
        }

        ///  <inheritdoc/>
        public IMarkupItem IMarkupItem
        {
            get { return markupItem; }
        }

        ///  <inheritdoc/>
        public ICreditMemoItem CreditMemoItem
        {
            get { return creditMemoItem; }
            set { creditMemoItem = (CreditMemoItem)value; }
        }

        ///  <inheritdoc/>
        public string ReceiptIdNumberSequence
        {
            get { return receiptIdNumberSequence; }
            set { receiptIdNumberSequence = value; }
        }

        ///  <inheritdoc/>
        public string InvoiceComment
        {
            get { return invoiceComment; }
            set { invoiceComment = value; }
        }

        ///  <inheritdoc/>
        public decimal AmountToAccount
        {
            get { return amountToAccount; }
            set { amountToAccount = value; }
        }

        ///  <inheritdoc/>
        public decimal Oiltax
        {
            get { return oiltax; }
            set { oiltax = value; }
        }

        ///  <inheritdoc/>
        public decimal SalesOrderAmounts
        {
            get { return totalSalesOrder; }
            set { totalSalesOrder = value; }
        }

        ///  <inheritdoc/>
        public decimal SalesInvoiceAmounts
        {
            get { return totalSalesInvoice; }
            set { totalSalesInvoice = value; }
        }

        ///  <inheritdoc/>
        public decimal IncomeExpenseAmounts
        {
            get { return totalIncomeExpense; }
            set { totalIncomeExpense = value; }
        }

        ///  <inheritdoc/>
        public PartnerObjectBase PartnerObject
        {
            get { return partnerObject; }
            set { partnerObject = value; }
        }

        ///  <inheritdoc/>
        public XElement PartnerXElement
        {
            get { return partnerXElement; }            
        }

        /// <inheritdoc/>
        public IEFTTransactionExtraInfo EFTTransactionExtraInfo { get; set; }

        /// <inheritdoc/>
        public XElement EFTTransactionExtraInfoXElement => eftTransactionExtraInfoXElement;

        /// <inheritdoc/>
        public Guid KDSOrderID { get; set; }

        ///  <inheritdoc/>
        public string SuspendDestination
        {
            get { return suspendDestination; }
            set { suspendDestination = value; }
        }

        ///  <inheritdoc/>
        public bool SplitTransaction
        {
            get { return splitTransaction; }
            set { splitTransaction = value; }
        }

        ///  <inheritdoc/>
        public bool UseTransactionService
        {
            get { return useTransactionService; }
            set { useTransactionService = value; }
        }

        ///  <inheritdoc/>
        public bool CalculateTotalDiscount
        {
            get { return calculateTotalDiscount; }
            set { calculateTotalDiscount = value; }
        }

        ///  <inheritdoc/>
        public bool TaxExempt { get; set; }

        ///  <inheritdoc/>
        public string TransactionTaxExemptionCode { get; set; }

        ///  <inheritdoc/>
        public UseTaxGroupFromEnum UseTaxGroupFrom { get; set; }

        ///  <inheritdoc/>
        public bool UseOverrideTaxGroup { get; set; }

        ///  <inheritdoc/>
        public string OverrideTaxGroup { get; set; }

        ///  <inheritdoc/>
        public bool IsTableTransaction { get; set; }

        ///  <inheritdoc/>
        [RecordIdentifierConstruction(typeof(Guid))]
        public RecordIdentifier SplitID { get; set; }

        #endregion

        ///  <inheritdoc/>
        public List<ITenderLineItem> TenderLines { get; set; }

        public List<ITenderLineItem> OriginalTenderLines { get; set; }

        public List<ReprintInfo> Reprints;

        ///  <inheritdoc/>
        public LinkedList<ISaleLineItem> SaleItems { get; set; }
        
        ///  <inheritdoc/>        
        public List<InfoCodeLineItem> InfoCodeLines { get; set; }

        ///  <inheritdoc/>
        public List<TaxItem> TaxLines { get; set; }

        ///  <inheritdoc/>
        public List<SuspendedTransactionAnswer> SuspendTransactionAnswers { get; set; }

        ///  <inheritdoc/>
        public CustomerOrderItem CustomerOrder { get; set; }

        ///  <inheritdoc/>
        public override IEnumerable<ITenderLineItem> ITenderLines
        {
            get
            {
                return TenderLines.Cast<ITenderLineItem>();
            }
        }


        ///  <inheritdoc/>
        public override IEnumerable<ITenderLineItem> IOriginalTenderLines
        {
            get
            {
                return OriginalTenderLines.Cast<ITenderLineItem>();
            }
            set
            {
                OriginalTenderLines = value.ToList();
            }
        }

        ///  <inheritdoc/>
        public IEnumerable<ISaleLineItem> ISaleItems
        {
            get
            {
                return SaleItems.Cast<ISaleLineItem>();
            }
        }

        ///  <inheritdoc/>
        public IEnumerable<InfoCodeLineItem> IInfocodeLines
        {
            get
            {
                return InfoCodeLines.Cast<InfoCodeLineItem>();
            }
        }

        ///  <inheritdoc/>
        public IEnumerable<TaxItem> ITaxLines
        {
            get
            {
                return TaxLines.Cast<TaxItem>();
            }
        }

        ///  <inheritdoc/>
        public IEnumerable<IReprintInfo> IReprints
        {
            get
            {
                return Reprints.Cast<IReprintInfo>();
            }
        }

        ///  <inheritdoc/>
        public bool IsReturnTransaction => SaleIsReturnSale && !string.IsNullOrEmpty(OrgTransactionId);

        ///  <inheritdoc/>
        public RetailTransaction(string storeID, string storeCurrencyCode, bool taxIncludedInPrice)
        {
            this.StoreId = storeID;
            this.StoreCurrencyCode = storeCurrencyCode;
            this.taxIncludedInPrice = taxIncludedInPrice;
            this.orgTaxIncludedInPrice = taxIncludedInPrice;

            Initialize();
        }

        private void Initialize()
        {
            this.BeginDateTime = DateTime.Now;
            MenuTypeItem = new MenuTypeItem();
            SaleItems = new LinkedList<ISaleLineItem>();
            TenderLines = new List<ITenderLineItem>();
            OriginalTenderLines = new List<ITenderLineItem>();
            InfoCodeLines = new List<InfoCodeLineItem>();
            TaxLines = new List<TaxItem>();
            Reprints = new List<ReprintInfo>();
            SuspendTransactionAnswers = new List<SuspendedTransactionAnswer>();
            customer = new Customer();            
            invoicedCustomer = new Customer();
            period = new Period();
            periodicDiscount = new PeriodicDiscount();
            manualPeriodicDiscounts = new List<DataEntity>();
            coupons = new List<Coupon>();
            couponItems = new List<CouponItem>();
            loyaltyItem = new LoyaltyItem();
            markupItem = new MarkupItem();
            creditMemoItem = new CreditMemoItem(this);
            refundReceiptId = "";
            journalId = "";
            totalSalesInvoice = 0M;
            totalSalesOrder = 0M;
            totalIncomeExpense = 0M;
            TaxExempt = false;
            TransactionTaxExemptionCode = "";
            UseTaxGroupFrom = UseTaxGroupFromEnum.Customer;
            UseOverrideTaxGroup = false;
            OverrideTaxGroup = "";            
            taxSettingsChanged = false;
            OrgStore = "";
            OrgTransactionId = "";
            OrgTerminal = "";
            OrgReceiptId = "";
            IsTableTransaction = true;
            SplitID = RecordIdentifier.Empty;
            SalesPerson = new Employee();
            CustomerOrder = new CustomerOrderItem();
            Hospitality.Clear();
            KDSOrderID = Guid.Empty;
        }

        ~RetailTransaction()
        {
            if(SaleItems != null)
                SaleItems.Clear();

            if(TenderLines != null)
                TenderLines.Clear();

            if (OriginalTenderLines != null)
                OriginalTenderLines.Clear();

            if (InfoCodeLines != null)
                InfoCodeLines.Clear();

            if(TaxLines != null)
                TaxLines.Clear();

            if (Reprints != null)
                Reprints.Clear();

            customer = null;
            invoicedCustomer = null;
            period = null;
            periodicDiscount = null;
            loyaltyItem = null;
            markupItem = null;
            creditMemoItem = null;
            MenuTypeItem = null;
        }

        public override TypeOfTransaction TransactionType()
        {
            return TypeOfTransaction.Sales;
        }

        public override object Clone()
        {
            RetailTransaction transaction = new RetailTransaction(StoreId, StoreCurrencyCode, taxIncludedInPrice);
            Populate(transaction);
            return transaction;
        }

        protected void Populate(RetailTransaction transaction)
        {
            base.Populate(transaction);
            transaction.partnerXElement = partnerObject != null ? partnerObject.ToXML() : null;
            transaction.customer = (Customer)customer.Clone();
            transaction.invoicedCustomer = (Customer)invoicedCustomer.Clone();
            transaction.customerPaysTax = customerPaysTax;
            transaction.customerPurchRequestId = customerPurchRequestId;
            transaction.lineDiscCalculationType = lineDiscCalculationType;
            transaction.amountToAccount = amountToAccount;
            transaction.taxIncludedInPrice = taxIncludedInPrice;
            transaction.orgTaxIncludedInPrice = orgTaxIncludedInPrice;
            transaction.taxSettingsChanged = taxSettingsChanged;
            transaction.taxAmount = taxAmount;
            transaction.balanceNetAmountWithTax = balanceNetAmountWithTax;
            transaction.netAmount = netAmount;
            transaction.netAmountWithTax = netAmountWithTax;
            transaction.isNetAmountWithTaxRounded = isNetAmountWithTaxRounded;
            transaction.netAmountWithTaxWithoutDiscountVoucher = netAmountWithTaxWithoutDiscountVoucher;
            transaction.grossAmount = grossAmount;
            transaction.grossAmountWithTax = grossAmountWithTax;
            transaction.payment = payment;
            transaction.lineDiscount = lineDiscount;
            transaction.lineDiscountWithTax = lineDiscountWithTax;
            transaction.periodicDiscountAmnt = periodicDiscountAmnt;
            transaction.periodicDiscountWithTax = periodicDiscountWithTax;
            transaction.totalDiscount = totalDiscount;
            transaction.totalDiscountWithTax = totalDiscountWithTax;
            transaction.totalManualPctDiscount = totalManualPctDiscount;
            transaction.totalManualDiscountAmount = totalManualDiscountAmount;
            transaction.calculateTotalDiscount = calculateTotalDiscount;
            transaction.roundingDifference = roundingDifference;
            transaction.roundingSalePmtDiff = roundingSalePmtDiff;
            transaction.transSalePmtDiff = transSalePmtDiff;
            transaction.totalSalesInvoice = totalSalesInvoice;
            transaction.totalSalesOrder = totalSalesOrder;
            transaction.totalIncomeExpense = totalIncomeExpense;
            transaction.noOfItems = noOfItems;
            transaction.saleIsReturnSale = saleIsReturnSale;
            transaction.postAsShipment = postAsShipment;
            transaction.SalesPerson = (Employee)SalesPerson.Clone();            
            transaction.refundReceiptId = refundReceiptId;
            transaction.journalId = journalId;                        
            transaction.timeWhenTotalPressed = timeWhenTotalPressed;
            transaction.itemElapsedTime = itemElapsedTime;
            transaction.tenderElapsedTime = tenderElapsedTime;
            transaction.idleElapsedTime = idleElapsedTime;
            transaction.lockElapsedTime = lockElapsedTime;
            transaction.lineItemsSingleScannedCount = lineItemsSingleScannedCount;
            transaction.lineItemsSingleScannedPercent  = lineItemsSingleScannedPercent;
            transaction.lineItemsMultiScannedCount = lineItemsMultiScannedCount;
            transaction.lineItemsMultiScannedPercent = lineItemsMultiScannedPercent;
            transaction.lineItemsKeyedCount = lineItemsKeyedCount;
            transaction.lineItemsKeyedPercent = lineItemsKeyedPercent;
            transaction.keyItemGroupCount = keyItemGroupCount;
            transaction.keyItemGroupPercent = keyItemGroupPercent;
            transaction.oiltax = oiltax;
            transaction.suspendDestination = suspendDestination;
            transaction.splitTransaction = splitTransaction;
            transaction.loyaltyItem = (LoyaltyItem)loyaltyItem.Clone();
            transaction.markupItem = (MarkupItem)markupItem.Clone();
            transaction.creditMemoItem = (CreditMemoItem) creditMemoItem.Clone();
            transaction.receiptIdNumberSequence = receiptIdNumberSequence;
            transaction.invoiceComment = invoiceComment;
            transaction.OrgTransactionId = OrgTransactionId;
            transaction.OrgStore = OrgStore;
            transaction.OrgTerminal = OrgTerminal;
            transaction.OrgReceiptId = OrgReceiptId;
            transaction.useTransactionService = useTransactionService;
            transaction.TaxExempt = TaxExempt;
            transaction.TransactionTaxExemptionCode = TransactionTaxExemptionCode;
            transaction.UseOverrideTaxGroup = UseOverrideTaxGroup;
            transaction.OverrideTaxGroup = OverrideTaxGroup;
            transaction.MenuTypeItem = (MenuTypeItem)MenuTypeItem.Clone();            
            transaction.ManualPeriodicDiscounts = CollectionHelper.Clone<DataEntity, List<DataEntity>>(ManualPeriodicDiscounts);
            transaction.Coupons = CollectionHelper.Clone<Coupon, List<Coupon>>(Coupons);
            transaction.CouponItems = CollectionHelper.Clone<CouponItem, List<CouponItem>>(CouponItems);
            transaction.TenderLines = CollectionHelper.Clone<ITenderLineItem, List<ITenderLineItem>>(TenderLines);
            transaction.OriginalTenderLines = CollectionHelper.Clone<ITenderLineItem, List<ITenderLineItem>>(OriginalTenderLines);
            transaction.SaleItems = CloneSaleLineItems(SaleItems, transaction);
            transaction.InfoCodeLines = CollectionHelper.Clone<InfoCodeLineItem, List<InfoCodeLineItem>>(InfoCodeLines);
            transaction.TaxLines = CollectionHelper.Clone<TaxItem, List<TaxItem>>(TaxLines);
            transaction.Reprints = CollectionHelper.Clone<ReprintInfo, List<ReprintInfo>>(Reprints);
            transaction.SuspendTransactionAnswers = CollectionHelper.Clone<SuspendedTransactionAnswer, List<SuspendedTransactionAnswer>>(SuspendTransactionAnswers);
            transaction.IsTableTransaction = IsTableTransaction;
            transaction.UseTaxGroupFrom = UseTaxGroupFrom;
            transaction.SplitID = SplitID;
            transaction.CustomerOrder = (CustomerOrderItem)CustomerOrder.Clone();
            transaction.Hospitality = (HospitalityItem)Hospitality.Clone();
            transaction.KeepRowSelectionOnBlankOperation = KeepRowSelectionOnBlankOperation;
            transaction.eftTransactionExtraInfoXElement = EFTTransactionExtraInfo?.ToXml();
            transaction.KDSOrderID = KDSOrderID;
        }

        private LinkedList<ISaleLineItem> CloneSaleLineItems(LinkedList<ISaleLineItem> saleLineItems, IRetailTransaction transaction)
        {
            var cloneItems = new LinkedList<ISaleLineItem>();

            foreach (SaleLineItem saleLineItem in saleLineItems)
            {
                cloneItems.AddLast(saleLineItem.Clone(transaction));
            }

            return cloneItems;
        }

        ///  <inheritdoc/>
        public bool Add(Customer customer)
        {
            if (customer == null)
            {
                customer = new Customer();
            }

            this.customer = customer;
            this.customer.ReturnCustomer = false;
            

            // Recalculate price with tax if the customer changes tax information
            if (this.customer.ID != RecordIdentifier.Empty && taxIncludedInPrice && !taxSettingsChanged
                && UseTaxGroupFrom == UseTaxGroupFromEnum.Customer && customer.TaxGroup != StoreTaxGroup)
            {
                orgTaxIncludedInPrice = taxIncludedInPrice;
                taxIncludedInPrice = false;
                taxSettingsChanged = true;
                ForceSaleItemSettings(taxIncludedInPrice);
            }

            if (this.customer.ID == RecordIdentifier.Empty && this.taxSettingsChanged)
            {
                this.taxIncludedInPrice = this.orgTaxIncludedInPrice;
                this.taxSettingsChanged = false;
                ResetSaleItemSettings();
            }
            return !RecordIdentifier.IsEmptyOrNull(customer.ID);
        }

        ///  <inheritdoc/>
        public void AddInvoicedCustomer(Customer customer)
        {
            if (customer != null && customer.ID != RecordIdentifier.Empty)
            {
                invoicedCustomer = customer;
            }
            else
            {
                invoicedCustomer = this.customer;
            }
        }

        ///  <inheritdoc/>
        public bool Add(Customer customer, bool returnCustomer)
        {         
            bool returnValue = Add(customer);
            if (customer.ID != RecordIdentifier.Empty)
            {
                this.customer.ReturnCustomer = returnCustomer;
            }
         
            return returnValue;
        }

        ///  <inheritdoc/>
        public void Add(ITenderLineItem tenderLineItem)
        {
            tenderLineItem.Transaction = this;
            this.payment += tenderLineItem.Amount;
            this.transSalePmtDiff = this.netAmountWithTax - this.payment;
            tenderLineItem.EndDateTime = DateTime.Now;
            this.TenderLines.Add(tenderLineItem);
            tenderLineItem.LineId = this.TenderLines.Count;
        }

        private void AddToTaxItems(ISaleLineItem saleLineItem)
        {
            // If a taxgroup has been assigned to the saleItem
            if (saleLineItem.TaxGroupId != null)
            {
                // Looping through each of the tax lines currently set to the ietm
                foreach (TaxItem saleLineTaxItem in ((SaleLineItem)saleLineItem).TaxLines)
                {
                    // For every taxLine it is checked whether it has been added before to the transaction
                    // If found, add the amount to the existing tax item.
                    // If not a new taxItem is added to the transaction
                    bool found = false;

                    // Creating a new Tax item in case it needs to be added.
                    TaxItem taxItem = new TaxItem();
                    taxItem.ItemTaxGroup = saleLineTaxItem.ItemTaxGroup;
                    taxItem.TaxCode = saleLineTaxItem.TaxCode;
                    taxItem.Percentage = saleLineTaxItem.Percentage;
                    taxItem.Amount = saleLineTaxItem.Amount;
                    taxItem.PriceWithTax = saleLineItem.NetAmountWithTax;
                    taxItem.TaxCodeDisplay = saleLineTaxItem.TaxCodeDisplay;
                    taxItem.ItemTaxGroupDisplay = saleLineTaxItem.ItemTaxGroupDisplay;
                        
                    // Looping to see if tax group already exists
                    foreach (TaxItem transTaxItem in this.TaxLines)
                    {
                        if ((transTaxItem.TaxCode != null) && (transTaxItem.TaxCode == saleLineTaxItem.TaxCode) )
                        {
                            transTaxItem.Amount += saleLineTaxItem.Amount;
                            transTaxItem.PriceWithTax += saleLineItem.NetAmountWithTax;
                            found = true;
                            break;
                        }
                    }
                    if (!found)
                    {
                        this.TaxLines.Add(taxItem);
                    }
                }
            }
        }

        private void ForceSaleItemSettings(bool taxIncludedInPrice)
        {
            foreach (SaleLineItem sli in SaleItems)
            {
                sli.ForceTaxIncludingPriceSetting(taxIncludedInPrice);
            }
        }

        private void ResetSaleItemSettings()
        {
            foreach (SaleLineItem sli in SaleItems)
            {
                sli.ResetTaxIncludingPrice();
            }
        }

        private bool AllowGeneralAggregation(ISaleLineItem itemBeingAdded, ISaleLineItem itemToAggregate)
        {
            return (itemToAggregate.IsInfoCodeItem == false) &&             // Not allowed for infocod items
            (itemToAggregate.Voided == false) &&                            // Not allowed for voided items
            ((itemToAggregate.Quantity > 0 && itemBeingAdded.Quantity > 0) || (itemToAggregate.Quantity < 0 && itemBeingAdded.Quantity < 0)) &&
            (itemToAggregate.IsLinkedItem == false) &&                      // Not allowed for linked items  
            (itemToAggregate.PriceOverridden == false) &&                   // Not allowed for overrided prices
            (itemToAggregate.BatchId == itemBeingAdded.BatchId) &&          // Only allow for items with the same batch ID
            (itemBeingAdded.KeyInPrice == KeyInPrices.NotMandatory) &&      // It must not be mandatory to key in price 
            (itemBeingAdded.KeyInQuantity == KeyInQuantities.NotMandatory) && //It must not be mandatory to key in quantity
            (itemBeingAdded.RFIDTagId == "") &&                             // Only allowed if RFIDTag is empty
            (itemBeingAdded.PriceInBarcode == false) &&                     // Only allowed if price is not in barcode
            (itemBeingAdded.MustKeyInComment == false) &&                   // Not allowed for items that have to have comments
            (itemToAggregate.SalesOrderUnitOfMeasure == itemBeingAdded.SalesOrderUnitOfMeasure); 
        }

        ///  <inheritdoc/>
        public void Add(ISaleLineItem saleLineItem, AggregateItemsModes aggregateItems)
        {
            int temp = 0;
            Add(saleLineItem, aggregateItems, ref temp);
        }

        ///  <inheritdoc/>
        public bool Add(ISaleLineItem saleLineItem, AggregateItemsModes aggregateItems, ref int lineIdBeingAdded)
        {
            if(SaleItems.Any(p => p.ID == saleLineItem.ID))
            {
                saleLineItem.ID = Guid.NewGuid();
            }

            LinkedListNode<ISaleLineItem> lastListItem = this.SaleItems.Last;
            if (lastListItem != null)
            {
                SaleLineItem lastItem = (SaleLineItem)lastListItem.Value;                
                switch (aggregateItems)
                {
                    case AggregateItemsModes.Normal:
                        {
                            if (AllowGeneralAggregation(saleLineItem, lastItem) )
                            {
                                if (string.Equals(lastItem.ItemId, saleLineItem.ItemId, StringComparison.InvariantCultureIgnoreCase))  // Only allowed for items with the same item ID
                                {
                                    lastItem.Quantity += saleLineItem.Quantity;
                                    lastItem.UnitQuantity += saleLineItem.UnitQuantity;
                                    lastItem.TareWeight += saleLineItem.TareWeight;
                                    saleLineItem.LineId = this.SaleItems.Count;
                                    lineIdBeingAdded = saleLineItem.LineId;
                                    return true;
                                }
                            }
                            break;
                        }
                }
            }
            Add(saleLineItem);
            lineIdBeingAdded = this.SaleItems.Count;
            return false;
        }

        
        /// <inheritdoc/>
        public int ReApplyLineIDs()
        {
            int count = 1;
            int lastFoundSaleLineItemID = -1;
            Dictionary<RecordIdentifier, int> lastFoundAssemblyItemLineID = new Dictionary<RecordIdentifier, int>();
            
            foreach (LineItem item in SaleItems)
            {
                int oldLineID = item.LineId;
                item.LineId = count++;

                if (item is SaleLineItem saleItem)
                {
                    if (item is DiscountVoucherItem discountItem)
                    {
                        discountItem.SourceLineNum = lastFoundSaleLineItemID;
                    }
                    else if (saleItem.IsLinkedItem)
                    {
                        saleItem.LinkedToLineId = lastFoundSaleLineItemID;
                    }
                    else if(!saleItem.IsAssemblyComponent)
                    {
                        lastFoundSaleLineItemID = item.LineId;
                    }

                    if (saleItem.IsAssemblyComponent)
                    {
                        bool parentFound = lastFoundAssemblyItemLineID.TryGetValue(saleItem.ParentAssembly.ID, out int assemblyLineID);
                        saleItem.AssemblyParentLineID = parentFound ? assemblyLineID : -1;
                    }

                    if (saleItem.IsAssembly)
                    {
                        lastFoundAssemblyItemLineID[saleItem.AssemblyID] = item.LineId;
                    }
                }
            }
            return count;
        }

        ///  <inheritdoc/>
        public void Add(ISaleLineItem saleLineItem)
        {
            if(SaleItems.Any(p => p.ID == saleLineItem.ID))
            {
                saleLineItem.ID = Guid.NewGuid();
            }

            if (SaleItems.Count == 0)
            {
                BeginDateTime = DateTime.Now;
            }
            saleLineItem.Transaction = this;
            saleLineItem.ResetLineMultiLineDiscountSettings();
            SaleItems.AddLast(saleLineItem);
            ReApplyLineIDs();
        }

        ///  <inheritdoc/>
        public void Add(ISaleLineItem saleLineItem, int lineId)
        {
            if(SaleItems.Any(p => p.ID == saleLineItem.ID))
            {
                saleLineItem.ID = Guid.NewGuid();
            }

            if (SaleItems.Count == 0)
            {
                BeginDateTime = DateTime.Now;
            }
            saleLineItem.Transaction = this;
            saleLineItem.LineId = lineId;
            SaleItems.AddLast(saleLineItem);
            ReApplyLineIDs();
        }

        ///  <inheritdoc/>
        public void Add(IFuelSalesLineItem fuelSaleLineItem)
        {
            if(ISaleItems.Any(p => p.ID == fuelSaleLineItem.ID))
            {
                fuelSaleLineItem.ID = new Guid();
            }

            if (SaleItems.Count == 0)
            {
                BeginDateTime = DateTime.Now;
            }
            fuelSaleLineItem.Transaction = this;
            SaleItems.AddLast(fuelSaleLineItem);
            ReApplyLineIDs();
        }

        ///  <inheritdoc/>
        public void Insert(ISaleLineItem saleLineItem, int lineId)
        {
            if(ISaleItems.Any(p => p.ID == saleLineItem.ID))
            {
                saleLineItem.ID = new Guid();
            }

            LinkedListNode<ISaleLineItem> node = this.SaleItems.First;
            while (node != null)
            {
                if (node.Value is SaleLineItem)
                {
                    if (node.Value.LineId == lineId)
                    {
                        saleLineItem.Transaction = this;
                        saleLineItem.LineId = this.SaleItems.Count + 1;
                        this.SaleItems.AddAfter(node, saleLineItem);
                        ReApplyLineIDs();
                        break;
                    }
                }
                node = node.Next;
            }
        }

        ///  <inheritdoc/>
        public void Insert(ISaleLineItem currentSaleLineItem, ISaleLineItem newSaleLineItem)
        {
            LinkedListNode<ISaleLineItem> node = this.SaleItems.Find(currentSaleLineItem);
            newSaleLineItem.Transaction = this;
            newSaleLineItem.LineId = this.SaleItems.Count + 1;
            this.SaleItems.AddAfter(node, newSaleLineItem);
            ReApplyLineIDs();
        }

        ///  <inheritdoc/>
        public void Remove(ISaleLineItem item, bool updateLineIDs)
        {
            SaleItems.Remove(item);
            if (updateLineIDs)
            {
                ReApplyLineIDs();
            }
        }

        ///  <inheritdoc/>
        public void Remove(ITenderLineItem tender, bool updateLineIDs)
        {
            TenderLines.Remove(tender);
            if (updateLineIDs)
            {
                ReApplyTenderLineIDs();
            }
        }

        private int ReApplyTenderLineIDs()
        {
            int count = 1;
            foreach (ITenderLineItem saleItem in TenderLines)
            {
                saleItem.LineId = count++;
            }
            return count;
        }

        private bool Remove(int lineId, bool updateLineIDs)
        {
            LinkedListNode<ISaleLineItem> node = this.SaleItems.First;
            while (node != null)
            {
                if (node.Value.LineId == lineId)
                {
                    if (node.Value is DiscountVoucherItem)
                    {
                        // if the salelineitem (above) exists, the discount voucher is nulled
                        if (node.Previous != null)
                            node.Previous.Value.DiscountVoucher = null;
                        // remove the discount voucher
                        this.SaleItems.Remove(node);
                        if (updateLineIDs)
                        {
                            ReApplyLineIDs();
                        }
                        return true;
                    }
                    else if (node.Value is SaleLineItem)
                    {
                        // set the discount voucher value of the salelineitem as null
                        node.Value.DiscountVoucher = null;
                        // if the attached dicount voucher is in plcae (below), it is removed
                        if ((node.Next != null) && (node.Next.Value is DiscountVoucherItem))
                        {
                            this.SaleItems.Remove(node.Next);
                            if (updateLineIDs)
                            {
                                ReApplyLineIDs();
                            }
                            return true;
                        }
                    }
                }
                node = node.Next;
            }
            return false;
        }

        ///  <inheritdoc/>
        public void Add(IDiscountVoucherItem discountVoucher, int lineId)
        {
            if(SaleItems.Any(p => p.ID == discountVoucher.ID))
            {
                discountVoucher.ID = Guid.NewGuid();
            }

            discountVoucher.Transaction = this;

            LinkedListNode<ISaleLineItem> node = this.SaleItems.First;
            while (node != null)
            {
                if (node.Value.LineId == lineId)
                {
                    // checking to see if the pointer is to a saleItem that already has discountVoucher item, if so the discountVoucher is removed
                    if ((node.Next != null) && (node.Next.Value.GetType() == typeof(DiscountVoucherItem)))
                    {
                        // There is no need to remove an existing voucher if it is the same as the one before
                        if (node.Next.Value.ItemId == discountVoucher.ItemId)
                        {
                            if (node.Next.Value.Voided )
                            {
                                node.Next.Value.Quantity = -1;
                                node.Next.Value.Voided = false;
                            }
                            else
                                node.Next.Value.Quantity--;
                            break;
                        }
                        this.SaleItems.Remove(node.Next);
                    }
                    // checking to see if the pointer is to a discount voucher item, if so the discountVoucher is removed
                    if (node.Value.GetType() == typeof(DiscountVoucherItem))
                    {
                        if (node.Value.ItemId == discountVoucher.ItemId)
                        {
                            if (node.Value.Voided )
                            {
                                node.Value.Quantity = -1;
                                node.Value.Voided = false;
                            }
                            else
                                node.Value.Quantity--;
                            break;
                        }
                        node = node.Previous;
                        this.SaleItems.Remove(node.Next);
                    }
                    // adding the discount voucher item to the saleItem
                    node.Value.DiscountVoucher = (IDiscountVoucherItem)discountVoucher;
                    this.SaleItems.AddAfter(node, discountVoucher);
                    break;
                }
                node = node.Next;
            }
            ReApplyLineIDs();
        }

        ///  <inheritdoc/>
        public int NumberOfLines()
        {
            return this.SaleItems.Count + this.TenderLines.Count;
        }        

        ///  <inheritdoc/>
        public void Add(InfoCodeLineItem infoCodeLineItem)
        {
            infoCodeLineItem.LineId = this.InfoCodeLines.Count + 1;                        

            this.InfoCodeLines.Add(infoCodeLineItem);
        }

        ///  <inheritdoc/>
        public Boolean InfoCodeNeeded(string infoCodeId)
        {
            //Checks the current transaction
            InfoCodeLineItem infocode = this.InfoCodeLines.FirstOrDefault(f => f.InfocodeId == infoCodeId);
            if (infocode != null)
            {
                return false;
            }
            
            //Check all existing salesItems in the transaction, whether one already has the infocode. If such a case is found, then 
            //do not display it again (because oncePerTransaction )
            foreach (SaleLineItem mySaleLineItem in this.SaleItems.Where(mySaleLineItem => mySaleLineItem.InfoCodeLines.Count > 0))
            {
                infocode = mySaleLineItem.InfoCodeLines.FirstOrDefault(f => f.InfocodeId == infoCodeId);
                if (infocode != null)
                {
                    return false;
                }
            }

            //Check TenderLines...
            foreach (TenderLineItem myTenderLineItem in this.TenderLines.Where(myTenderLineItem => myTenderLineItem.InfoCodeLines.Count > 0))
            {
                infocode = myTenderLineItem.InfoCodeLines.FirstOrDefault(f => f.InfocodeId == infoCodeId);
                if (infocode != null)
                {
                    return false;
                }
            }

            //Nothing has been found such that we display the Infocode
            return true;
        }

        ///  <inheritdoc/>
        public void ClearTotalAmounts()
        {
            this.netAmount = 0;
            this.netAmountWithTax = 0;
            this.netAmountWithTaxWithoutDiscountVoucher = 0;
            this.grossAmount = 0;
            this.grossAmountWithTax = 0;
            this.lineDiscount = 0;
            this.lineDiscountWithTax = 0;
            this.periodicDiscountAmnt = 0;
            this.periodicDiscountWithTax = 0;
            this.totalDiscount = 0;
            this.totalDiscountWithTax = 0;
            this.taxAmount = 0;
            this.oiltax = 0;
            this.TaxLines.Clear();
        }

        ///  <inheritdoc/>
        public void UpdateTotalAmounts(ISaleLineItem saleLineItem)
        {
            //Total amounts
            this.netAmount += saleLineItem.NetAmount;
            this.netAmountWithTax += saleLineItem.NetAmountWithTax;
            this.netAmountWithTaxWithoutDiscountVoucher += saleLineItem.NetAmountWithTaxWithoutDiscountVoucher;
            this.grossAmount += saleLineItem.GrossAmount;
            this.grossAmountWithTax += saleLineItem.GrossAmountWithTax;            
            this.taxAmount += saleLineItem.TaxAmount;
            this.oiltax += saleLineItem.Oiltax;
            UpdateDiscountAmounts(saleLineItem);
            AddToTaxItems(saleLineItem);
        }

        ///  <inheritdoc/>
        public void UpdateDiscountAmounts(ISaleLineItem saleLineItem)
        {
            this.lineDiscount += saleLineItem.LineDiscountExact;
            this.LineDiscountWithTax += saleLineItem.LineDiscountWithTaxExact;
            this.periodicDiscountAmnt += saleLineItem.PeriodicDiscount;
            this.periodicDiscountWithTax += saleLineItem.PeriodicDiscountWithTax;
            this.totalDiscount += saleLineItem.TotalDiscountExact;
            this.totalDiscountWithTax += saleLineItem.TotalDiscountWithTaxExact;
        }

        ///  <inheritdoc/>
        public void SetNetAmountWithTax(decimal NetAmountWithTax, bool CalculateTransSalePmtDiff)
        {
            if (CalculateTransSalePmtDiff)
            {
                this.NetAmountWithTax = NetAmountWithTax;
            }
            else
            {
                this.netAmountWithTax = NetAmountWithTax;
            }
        }

        ///  <inheritdoc/>
        public ISaleLineItem GetTopMostLimitationSplitParentItem(ISaleLineItem currentItem)
        {
            if (currentItem.LimitationSplitParentLineId < 0)
            {
                return currentItem;
            }
            
            return GetTopMostLimitationSplitParentItem(GetItem(currentItem.LimitationSplitParentLineId));
        }

        #region Clear discounts

        ///  <inheritdoc/> 
        public void ClearAllDiscountLines()
        {
            lineDiscount = 0;
            lineDiscountWithTax = 0;
            periodicDiscountAmnt = 0;
            periodicDiscountWithTax = 0;
            totalDiscount = 0;
            totalDiscountWithTax = 0;
            totalManualPctDiscount = 0;
            totalManualDiscountAmount = 0;
            manualPeriodicDiscounts.Clear();            

            /*
             * Note -> If this function is changed then the same changes need to be done in clear discount functions 
             *         that applies to that type of discounts f.ex. ClearPeriodicDiscountLines and etc.
             * 
             */

            foreach (SaleLineItem saleLineItem in this.SaleItems.Where(x => !x.ReceiptReturnItem))
            {
                //Periodic Discounts
                saleLineItem.PeriodicDiscountOfferId = "";
                saleLineItem.PeriodicDiscountOfferName = "";
                saleLineItem.QuantityDiscounted = 0;
                saleLineItem.PeriodicDiscountWithTax = 0;
                saleLineItem.PeriodicDiscount = 0;
                //Line discounts
                saleLineItem.LineDiscount = 0;
                saleLineItem.LineDiscountWithTax = 0;
                saleLineItem.LinePctDiscount = 0;
                //Total discount
                saleLineItem.TotalDiscount = 0;
                saleLineItem.TotalDiscountWithTax = 0;
                saleLineItem.TotalPctDiscount = 0;
                //Customer discount
                saleLineItem.CustomerDiscount = 0;
                saleLineItem.CustomerDiscountWithTax = 0;
                //Loyalty discount
                saleLineItem.LoyaltyDiscount = 0;
                saleLineItem.LoyaltyDiscountWithTax = 0;
                saleLineItem.LoyaltyPctDiscount = 0;

                if ((saleLineItem.ExcludedActions & ExcludedActions.PriceOverride) == ExcludedActions.PriceOverride)
                {
                    saleLineItem.ExcludedActions &= ~ExcludedActions.PriceOverride;
                }

                saleLineItem.ClearDiscountLines(typeof(CustomerDiscountItem));
                saleLineItem.ClearDiscountLines(typeof(TotalDiscountItem));
                saleLineItem.ClearDiscountLines(typeof(LineDiscountItem));
                saleLineItem.ClearDiscountLines(typeof(PeriodicDiscountItem));
                saleLineItem.ClearDiscountLines(typeof(LoyaltyDiscountItem));
            }
        }

        ///  <inheritdoc/>
        public void ClearPeriodicDiscountLines()
        {
            /*
             * Note -> If this function is changed then the same changes need to be done in ClearAllDiscounts             
             * 
             */

            foreach (SaleLineItem saleLineItem in this.SaleItems.Where(x => !x.ReceiptReturnItem))
            {
                saleLineItem.PeriodicDiscountOfferId = "";
                saleLineItem.PeriodicDiscountOfferName = "";
                saleLineItem.QuantityDiscounted = 0;
                saleLineItem.PeriodicDiscountWithTax = 0;
                saleLineItem.PeriodicDiscount = 0;

                if ((saleLineItem.ExcludedActions & ExcludedActions.PriceOverride) == ExcludedActions.PriceOverride)
                {
                    saleLineItem.ExcludedActions &= ~ExcludedActions.PriceOverride;
                }

                saleLineItem.ClearDiscountLines(typeof(PeriodicDiscountItem));
            }
        }

        ///  <inheritdoc/>
        public void ClearLineDiscountLines()
        {
            /*
             * Note -> If this function is changed then the same changes need to be done in ClearAllDiscounts             
             * 
             */
            foreach (SaleLineItem saleLineItem in this.SaleItems.Where(x => !x.ReceiptReturnItem))
            {
                saleLineItem.LineDiscount = 0;
                saleLineItem.LineDiscountWithTax = 0;
                saleLineItem.LinePctDiscount = 0;

                saleLineItem.ClearDiscountLines(typeof(LineDiscountItem));
            }
        }        

        ///  <inheritdoc/>
        public void ClearCustomerDiscountLines()
        {
            /*
             * Note -> If this function is changed then the same changes need to be done in ClearAllDiscounts             
             * 
             */

            foreach (SaleLineItem saleLineItem in this.SaleItems.Where(x => !x.ReceiptReturnItem))
            {
                saleLineItem.CustomerDiscount = 0;
                saleLineItem.CustomerDiscountWithTax = 0;

                saleLineItem.ClearDiscountLines(typeof(CustomerDiscountItem));
            }
        }                

        ///  <inheritdoc/>
        public void ClearTotalDiscountLines()
        {
            /*
             * Note -> If this function is changed then the same changes need to be done in ClearAllDiscounts             
             * 
             */

            totalManualPctDiscount = 0;
            totalManualDiscountAmount = 0;

            foreach (SaleLineItem saleItem in this.SaleItems.Where(x => !x.ReceiptReturnItem))
            {
                saleItem.TotalDiscount = 0;
                saleItem.TotalDiscountWithTax = 0;
                saleItem.TotalPctDiscount = 0;
                saleItem.ClearDiscountLines(typeof(TotalDiscountItem));
            }
        }

        ///  <inheritdoc/>
        public void ClearLoyaltyDiscountLines()
        {
            /*
             * Note -> If this function is changed then the same changes need to be done in ClearAllDiscounts             
             * 
             */
            foreach (SaleLineItem saleLineItem in SaleItems.Where(x => !x.ReceiptReturnItem))
            {
                saleLineItem.LoyaltyDiscount = 0;
                saleLineItem.LoyaltyDiscountWithTax = 0;
                saleLineItem.LoyaltyPctDiscount = 0;

                saleLineItem.ClearDiscountLines(typeof(LoyaltyDiscountItem));
                saleLineItem.LoyaltyPoints.Clear();
            }
        }

        ///  <inheritdoc/>
        public void ClearManuallyTriggeredPeriodicDiscountLines()
        {
            /*
             * Note -> If this function is changed then the same changes need to be done in ClearAllDiscounts             
             * 
             */
            manualPeriodicDiscounts.Clear();
        }

        #endregion

        ///  <inheritdoc/>
        public void ClearCustomer()
        {
            customer = new Customer();
        }

        ///  <inheritdoc/>
        public string VoidItemLine(int lineId)
        {
            string productName = "";

            LinkedListNode<ISaleLineItem> saleLineItemNode = this.SaleItems.First;
            while (saleLineItemNode != null)
            {
                if (saleLineItemNode.Value.LineId == lineId)
                {
                    decimal sign;
                    if (saleLineItemNode.Value.Voided)
                    {
                        // Don't do anything if trying to void a discountVoucherItem that has a voided parent salelineitem
                        if ((saleLineItemNode.Previous != null) && (saleLineItemNode.Previous.Value is SaleLineItem) && (saleLineItemNode.Value is DiscountVoucherItem) && (saleLineItemNode.Previous.Value.Voided ))
                        {
                            break;
                        }

                        sign = 1;
                        // Unvoiding the current item
                        saleLineItemNode.Value.Voided = false;

                        // If this line is SaleLineItem and the next line is not null and the next line is DiscountVoucher
                        if ((saleLineItemNode.Next != null) && (saleLineItemNode.Value is SaleLineItem) && (saleLineItemNode.Next.Value is DiscountVoucherItem))
                        {
                            // .. then we also unvoid the voucher
                            saleLineItemNode.Next.Value.Voided = false;
                        }
                    }
                    else
                    {
                        sign = -1;
                        saleLineItemNode.Value.Voided = true;

                        // If this line is SaleLineItem, then next line is not null and the next line is DiscountVoucher
                        if ((saleLineItemNode.Next != null) && (saleLineItemNode.Value is SaleLineItem) && (saleLineItemNode.Next.Value is DiscountVoucherItem))
                        {
                            // .. then we also void the voucher
                            saleLineItemNode.Next.Value.Voided = true;
                        }
                    }

                    this.netAmount += sign * saleLineItemNode.Value.NetAmount;
                    this.netAmountWithTax += sign * saleLineItemNode.Value.NetAmountWithTax;
                    this.netAmountWithTaxWithoutDiscountVoucher += sign * saleLineItemNode.Value.NetAmountWithTaxWithoutDiscountVoucher;
                    this.lineDiscount += sign * saleLineItemNode.Value.LineDiscount;
                    this.lineDiscountWithTax += sign * saleLineItemNode.Value.LineDiscountWithTax;
                    this.periodicDiscountAmnt += sign * saleLineItemNode.Value.PeriodicDiscount;
                    this.periodicDiscountWithTax += sign * saleLineItemNode.Value.PeriodicDiscountWithTax;
                    this.totalDiscount += sign * saleLineItemNode.Value.TotalDiscount;
                    this.totalDiscountWithTax += sign * saleLineItemNode.Value.TotalDiscountWithTax;

                    if (saleLineItemNode.Value.GetType() == typeof(SalesInvoiceLineItem))
                    {
                        this.SalesInvoiceAmounts += sign * saleLineItemNode.Value.PriceWithTax;
                    }
                    if (saleLineItemNode.Value.GetType() == typeof(SalesOrderLineItem))
                    {
                        this.SalesOrderAmounts += sign * saleLineItemNode.Value.PriceWithTax;
                    }
                    if (saleLineItemNode.Value.GetType() == typeof(IncomeExpenseItem))
                    {
                        this.IncomeExpenseAmounts += (-1 * sign) * saleLineItemNode.Value.PriceWithTax;
                    }

                    this.transSalePmtDiff = this.netAmountWithTax - this.payment;
                    productName = saleLineItemNode.Value.Description;

                    break;
                }
                saleLineItemNode = saleLineItemNode.Next;
            }

            return productName;
        }

        ///  <inheritdoc/>
        public ITenderLineItem VoidPaymentLine(int lineId)
        {
            TenderLineItem result = null;

            foreach (TenderLineItem tenderLineItem in this.TenderLines.Where(tenderLineItem => tenderLineItem.LineId == lineId))
            {
                decimal sign;
                if (tenderLineItem.Voided)
                {
                    sign = 1;
                    tenderLineItem.Voided = false;
                }
                else
                {
                    sign = -1;
                    tenderLineItem.Voided = true;
                }

                this.payment += sign * tenderLineItem.Amount;
                this.transSalePmtDiff = this.netAmountWithTax - this.payment;

                result = tenderLineItem;
            }
            return result;
        }

        ///  <inheritdoc/>
        public ISaleLineItem PriceOverride(ISaleLineItem saleLineItem, decimal amount)
        {
            if (saleLineItem.OriginalPriceWithTax == 0)
            {
                saleLineItem.OriginalPriceWithTax = saleLineItem.PriceWithTax;
            }
            if (saleLineItem.OriginalPrice == 0)
            {
                saleLineItem.OriginalPrice = saleLineItem.Price;
            }

            if (this.KeyedInPriceContainsTax)
            {
                //Price that is entered by user is always entered with tax 
                saleLineItem.PriceWithTax = amount;
                saleLineItem.TaxIncludedInItemPrice = true;
            }
            else
            {
                saleLineItem.Price = amount;
                saleLineItem.TaxIncludedInItemPrice = false;
            }

            saleLineItem.PriceOverrideAmount = amount;

            saleLineItem.PriceOverridden = true;
            return saleLineItem;
        }

        ///  <inheritdoc/>
        public ISaleLineItem SetCostPrice(ISaleLineItem saleLineItem, decimal amount)
        {
            saleLineItem.CostPrice = amount;
            return saleLineItem;
        }

        ///  <inheritdoc/>
        public ISaleLineItem GetItem(int lineId)
        {
            return this.SaleItems.FirstOrDefault(f => f.LineId == lineId);
        }

        /// <inheritdoc/>
        public ISaleLineItem GetItem(Guid lineId)
        {
            return this.SaleItems.FirstOrDefault(f => f.ID == lineId);
        }

        ///  <inheritdoc/>
        public ITenderLineItem GetTenderItem(int lineId)
        {
            return this.TenderLines.FirstOrDefault(f => f.LineId == lineId);
        }

        ///  <inheritdoc/>
        public void SetTotalDiscPercent(decimal totalDiscountPercentage)
        {
            this.calculateTotalDiscount = true;
            this.totalManualPctDiscount = totalDiscountPercentage;                        
        }

        ///  <inheritdoc/>
        public void SetTotalDiscAmount(decimal totalDiscountAmount)
        {
            this.calculateTotalDiscount = true;
            this.totalManualDiscountAmount = totalDiscountAmount;            
        }        

        ///  <inheritdoc/>
        public void AddTotalDiscPctLines()
        {
            if (this.TotalManualPctDiscount != 0)
            {
                ClearTotalAmounts();

                //Add the total discount to each item.
                foreach (SaleLineItem saleItem in this.SaleItems.Where(saleItem => saleItem.Voided == false && saleItem.IncludedInTotalDiscount))
                {
                    //Clear an total discount if found
                    saleItem.ClearDiscountLines(typeof(TotalDiscountItem));
                    if ((saleItem.NoDiscountAllowed == false) && (Math.Abs(saleItem.Quantity) > 0))
                    {
                        //Add a new total discount
                        TotalDiscountItem totalDiscountItem = new TotalDiscountItem();
                        totalDiscountItem.Percentage = this.TotalManualPctDiscount;
                        saleItem.Add(totalDiscountItem);
                    }
                    else
                    {
                        saleItem.DiscountUnsuccessfullyApplied = true;
                    }
                }
            }
            else  //Remove the total discount line if it is set to zero.
            {
                ClearTotalDiscountLines();
                this.calculateTotalDiscount = false;
            }
        }

        ///  <inheritdoc/>
        public decimal AddTotalDiscAmountLines(bool banCompoundDiscounts)
        {
            decimal totalAmt = 0;
            decimal discPct = 0;

            if (totalManualDiscountAmount != 0)
            {
                ClearTotalAmounts();
                
                //The total amount that can be discounted
                foreach (SaleLineItem saleItem in this.SaleItems.Where(saleItem => saleItem.Voided == false && saleItem.IncludedInTotalDiscount))
                {
                    if (banCompoundDiscounts)
                    {
                        bool anyDiscountFound = false;

                        foreach (var discountItem in saleItem.DiscountLines)
                        {
                            if (!(discountItem is TotalDiscountItem))
                            {
                                anyDiscountFound = true;
                            }
                        }

                        if (anyDiscountFound)
                        {
                            // Clear total amount discount, cannot support that with other discounts
                            if (TotalManualDiscountAmount != 0m)
                            {
                                continue;
                            }
                        }
                    }

                    if ((saleItem.NoDiscountAllowed == false) && (Math.Abs(saleItem.Quantity) > 0))
                    {
                        switch (this.CalculateDiscountFrom)
                        {
                            case Store.CalculateDiscountsFromEnum.PriceWithTax:
                                totalAmt += saleItem.GrossAmountWithTax - saleItem.LineDiscountWithTax - saleItem.PeriodicDiscountWithTax - saleItem.LoyaltyDiscountWithTax;
                                break;
                            case Store.CalculateDiscountsFromEnum.Price:
                                totalAmt += saleItem.GrossAmount - saleItem.LineDiscount - saleItem.PeriodicDiscount - saleItem.LoyaltyDiscount;
                                break;
                        }
                    }
                    else
                    {
                        saleItem.DiscountUnsuccessfullyApplied = true;
                    }                   
                }                

                //The percentage discount
                if (totalAmt != 0)
                {
                    discPct = 100 * Math.Abs(this.totalManualDiscountAmount) / Math.Abs(totalAmt);
                }
                
                //Add the total discount to each item.
                foreach (SaleLineItem saleItem in this.SaleItems.Where(saleItem => saleItem.Voided == false && saleItem.IncludedInTotalDiscount))
                {
                    if (banCompoundDiscounts)
                    {
                        bool anyDiscountFound = false;

                        foreach (var discountItem in saleItem.DiscountLines)
                        {
                            if (!(discountItem is TotalDiscountItem))
                            {
                                anyDiscountFound = true;
                            }
                        }

                        if (anyDiscountFound)
                        {
                            // Clear total amount discount, cannot support that with other discounts
                            if (TotalManualDiscountAmount != 0m)
                            {
                                continue;
                            }
                        }
                    }

                    //Clear an total discount if found
                    saleItem.ClearDiscountLines(typeof(TotalDiscountItem));
                    if ((saleItem.NoDiscountAllowed == false) && (Math.Abs(saleItem.Quantity) > 0))
                    {
                        //Add a new total discount
                        TotalDiscountItem totalDiscountItem = new TotalDiscountItem();                        
                        totalDiscountItem.Percentage = discPct;                                                
                        saleItem.Add(totalDiscountItem);                            
                    }
                    else
                    {
                        saleItem.DiscountUnsuccessfullyApplied = true;                        
                    }                   
                }
            }
            else  //Remove the total discount line if it is set to zero.
            {
                ClearTotalDiscountLines();
                this.calculateTotalDiscount = false;
            }

            return discPct;
        }

        public override void Save() 
        {          
        }

        ///  <inheritdoc/>
        public void UnMarkSelectedItems(List<int> SelectedLineIds, bool UnMarkAll)
        {
            if (UnMarkAll)
            {
                foreach (SaleLineItem sli in SaleItems)
                {
                    sli.SplitMarked = false;
                }
            }
            else
            {
                foreach (int sel in SelectedLineIds)
                {
                    ISaleLineItem sli = SaleItems.FirstOrDefault(line => line.LineId == sel);
                    if (sli != null)
                    {
                        sli.SplitMarked = false;
                    }
                }
            }
        }

        public override int CalculateTotalNumberOfTransactionLines()
        {
            int totalLines = InfoCodeLines.Count;

            if (Hospitality.ActiveHospitalitySalesType.StringValue != "") { totalLines++; }
            if (!LoyaltyItem.Empty) { totalLines++; }

            foreach(ISaleLineItem line in SaleItems)
            {
                if(line is SalesOrderLineItem || line is SalesInvoiceLineItem || line is IncomeExpenseItem)
                {
                    totalLines++;
                }
                else
                {
                    if(!line.Excluded)
                    {
                        totalLines++;

                        totalLines += line.InfoCodeLines.Count + line.DiscountLines.Count + line.TaxLines.Count;

                        if(line.LoyaltyPoints.PointsAdded)
                        {
                            totalLines++;
                        }

                        if(line is FuelSalesLineItem)
                        {
                            totalLines++;
                        }
                    }
                }
            }

            foreach(ITenderLineItem tenderLine in TenderLines)
            {
                totalLines++;

                if(tenderLine is CardTenderLineItem)
                {
                    CardTenderLineItem cardTenderLine = (CardTenderLineItem)tenderLine;

                    if(cardTenderLine.EFTInfo != null)
                    {
                        totalLines++;
                    }
                }
            }

            return totalLines;
        }

        #region Customer Orders



        #endregion

        #region XML code

        ///  <inheritdoc/>
        public List<SuspendedTransactionAnswer> CreateAnswerLines(XElement xItem)
        {
            List<SuspendedTransactionAnswer> AnswerLines = new List<SuspendedTransactionAnswer>();

            if (xItem.HasElements)
            {
                IEnumerable<XElement> classElements = xItem.Elements("SuspendedTransactionAnswer");
                foreach (XElement xClass in classElements)
                {
                    if (xClass.HasElements)
                    {
                        if (!xClass.IsEmpty)
                        {
                            SuspendedTransactionAnswer answer = new SuspendedTransactionAnswer();
                            answer.ToClass(xClass);
                            AnswerLines.Add(answer);
                        }
                    }
                }
            }
            return AnswerLines;
        }

        private List<DataEntity> CreateManualPeriodicDiscounts(XElement xItems)
        {
            List<DataEntity> discounts = new List<DataEntity>();
            if (xItems.HasElements)
            {
                IEnumerable<XElement> xTenderItems = xItems.Elements();
                foreach (XElement xTender in xTenderItems)
                {
                    if (xTender.HasElements)
                    {
                        DataEntity disc = new DataEntity();
                        disc.ToClass(xTender);
                        discounts.Add(disc);
                    }
                }
            }
            return discounts;
        }

        ///  <inheritdoc/>
        public List<ITenderLineItem> CreateTenderLines(XElement xItems)
        {
            List<ITenderLineItem> TenderLines = new List<ITenderLineItem>();

            if (xItems.HasElements)
            {
                IEnumerable<XElement> xTenderItems = xItems.Elements();
                foreach (XElement xTender in xTenderItems)
                {
                    if (xTender.HasElements)
                    {
                        TenderLineItem tli = new TenderLineItem();

                        switch (xTender.Name.ToString())
                        {
                            case "TenderLineItem":
                                break;
                            case "CardTenderLineItem":
                                tli = new CardTenderLineItem();
                                break;
                            case "ChequeTenderLineItem":
                                tli = new ChequeTenderLineItem();
                                break;
                            case "CorporateCardTenderLineItem":
                                tli = new CorporateCardTenderLineItem();
                                break;
                            case "CouponTenderLineItem":
                                tli = new CouponTenderLineItem();
                                break;
                            case "CreditMemoTenderLineItem":
                                tli = new CreditMemoTenderLineItem();
                                break;
                            case "CustomerTenderLineItem":
                                tli = new CustomerTenderLineItem();
                                break;
                            case "GiftCertificateTenderLineItem":
                                tli = new GiftCertificateTenderLineItem();
                                break;
                            case "LoyaltyTenderLineItem":
                                tli = new LoyaltyTenderLineItem();
                                break;
                            case "TradeInTenderLineItem":
                                tli = new TradeInTenderLineItem();
                                break;
                            case "DepositTenderLineItem":
                                tli = new DepositTenderLineItem();
                                break;
                        }
                        tli.ToClass(xTender);
                        tli.Transaction = this;
                        TenderLines.Add(tli);

                    }
                }
            }
            return TenderLines;
        }

        ///  <inheritdoc/>
        public List<TaxItem> CreateTaxLines(XElement xItem)
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
                            taxLine.ToClass(xClass);//, transLog);
                            TaxLines.Add(taxLine);
                        }
                    }
                }
            }
            return TaxLines;
        }

        public List<ReprintInfo> CreateReprintLines(XElement xItems)
        {
            List<ReprintInfo> lines = new List<ReprintInfo>();

            if (xItems.HasElements)
            {
                IEnumerable<XElement> xInfocodeItems = xItems.Elements("ReprintInfo");
                foreach (XElement xInfocodeItem in xInfocodeItems)
                {
                    if (xInfocodeItem.HasElements)
                    {
                        ReprintInfo newReprint = new ReprintInfo();
                        newReprint.ToClass(xInfocodeItem);
                        lines.Add(newReprint);
                    }
                }
            }

            return lines;
        }

        ///  <inheritdoc/>
        public List<InfoCodeLineItem> CreateInfocodeLines(XElement xItems)
        {
            List<InfoCodeLineItem> SaleLines = new List<InfoCodeLineItem>();

            if (xItems.HasElements)
            {
                IEnumerable<XElement> xInfocodeItems = xItems.Elements("InfoCodeLineItem");
                foreach (XElement xInfocodeItem in xInfocodeItems)
                {
                    if (xInfocodeItem.HasElements)
                    {
                        InfoCodeLineItem newInfocode = new InfoCodeLineItem();
                        newInfocode.ToClass(xInfocodeItem);
                        SaleLines.Add(newInfocode);
                    }
                }
            }

            return SaleLines;
        }

        ///  <inheritdoc/>
        public LinkedList<ISaleLineItem> CreateSaleLineItems(XElement xItems)
        {
            LinkedList<ISaleLineItem> SaleLines = new LinkedList<ISaleLineItem>();

            if (xItems.HasElements)
            {
                IEnumerable<XElement> xSaleItems = xItems.Elements();
                foreach (XElement xSaleItem in xSaleItems)
                {
                    if (xSaleItem.HasElements)
                    {
                        SaleLineItem sli = new SaleLineItem(this);
                        switch (xSaleItem.Name.ToString())
                        {
                            case "SaleLineItem":
                                break;
                            case "DiscountVoucherItem":
                                sli = new DiscountVoucherItem(this, "", "", 0);
                                SaleLines.Last.Value.DiscountVoucher = (IDiscountVoucherItem)sli;
                                break;
                            case "FuelSalesLineItem":
                                sli = new FuelSalesLineItem(new BarCode(), this);
                                break;
                            case "GiftCertificateItem":
                                sli = new GiftCertificateItem(this);
                                break;
                            case "IncomeExpenseItem":
                                sli = new IncomeExpenseItem(this);
                                break;
                            case "SalesInvoiceLineItem":
                                sli = new SalesInvoiceLineItem(this);
                                break;
                            case "SalesOrderLineItem":
                                sli = new SalesOrderLineItem(this);
                                break;
                            case "PharmacySalesLineItem":
                                sli = new PharmacySalesLineItem(this);
                                break;
                        }
                        sli.ToClass(xSaleItem);
                        SaleLines.AddLast(sli);
                    }
                }
            }

            return SaleLines;
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
                XElement xRetailTrans = new XElement("RetailTransaction",
                    new XElement("customer", customer.ToXML()),
                    new XElement("invoicedCustomer", invoicedCustomer.ToXML()),
                    new XElement("customerPaysTax", Conversion.ToXmlString(customerPaysTax)),
                    new XElement("customerPurchRequestId", customerPurchRequestId),
                    new XElement("lineDiscCalculationType", Conversion.ToXmlString((int)lineDiscCalculationType)),
                    new XElement("amountToAccount", Conversion.ToXmlString(amountToAccount)),
                    new XElement("taxIncludedInPrice", Conversion.ToXmlString(taxIncludedInPrice)),
                    new XElement("orgTaxIncludedInPrice", Conversion.ToXmlString(orgTaxIncludedInPrice)),
                    new XElement("taxSettingsChanged", Conversion.ToXmlString(taxSettingsChanged)),
                    new XElement("taxAmount", Conversion.ToXmlString(taxAmount)),
                    new XElement("balanceNetAmountWithTax", Conversion.ToXmlString(balanceNetAmountWithTax)),
                    new XElement("netAmount", Conversion.ToXmlString(netAmount)),
                    new XElement("netAmountWithTax", Conversion.ToXmlString(netAmountWithTax)),
                    new XElement("isNetAmountWithTaxRounded", Conversion.ToXmlString(isNetAmountWithTaxRounded)),
                    new XElement("netAmountWithTaxWithoutDiscountVoucher", Conversion.ToXmlString(netAmountWithTaxWithoutDiscountVoucher)),
                    new XElement("grossAmount", Conversion.ToXmlString(grossAmount)),
                    new XElement("grossAmountWithTax", Conversion.ToXmlString(grossAmountWithTax)),
                    new XElement("payment", Conversion.ToXmlString(payment)),
                    new XElement("lineDiscount", Conversion.ToXmlString(lineDiscount)),
                    new XElement("lineDiscountWithTax", Conversion.ToXmlString(lineDiscountWithTax)),
                    new XElement("periodicDiscountAmnt", Conversion.ToXmlString(periodicDiscountAmnt)),
                    new XElement("periodicDiscountWithTax", Conversion.ToXmlString(periodicDiscountWithTax)),
                    new XElement("totalDiscount", Conversion.ToXmlString(totalDiscount)),
                    new XElement("totalDiscountWithTax", Conversion.ToXmlString(totalDiscountWithTax)),
                    new XElement("totalManualPctDiscount", Conversion.ToXmlString(totalManualPctDiscount)),
                    new XElement("totalManualDiscountAmount", Conversion.ToXmlString(totalManualDiscountAmount)),
                    new XElement("calculateTotalDiscount", Conversion.ToXmlString(calculateTotalDiscount)),
                    new XElement("roundingDifference", Conversion.ToXmlString(roundingDifference)),
                    new XElement("roundingSalePmtDiff", Conversion.ToXmlString(roundingSalePmtDiff)),
                    new XElement("transSalePmtDiff", Conversion.ToXmlString(transSalePmtDiff)),
                    new XElement("totalSalesInvoice", Conversion.ToXmlString(totalSalesInvoice)),
                    new XElement("totalSalesOrder", Conversion.ToXmlString(totalSalesOrder)),
                    new XElement("totalIncomeExpense", Conversion.ToXmlString(totalIncomeExpense)),
                    new XElement("noOfItems", Conversion.ToXmlString(noOfItems)),
                    new XElement("saleIsReturnSale", Conversion.ToXmlString(saleIsReturnSale)),
                    new XElement("postAsShipment", Conversion.ToXmlString(postAsShipment)),
                    new XElement("employee", SalesPerson.ToXML()),                    
                    new XElement("refundReceiptId", refundReceiptId),
                    new XElement("journalId", journalId),                                        
                    new XElement("timeWhenTotalPressed", timeWhenTotalPressed),
                    new XElement("itemElapsedTime", itemElapsedTime.ToString()), //TODO: not deserialized in ToClass method
                    new XElement("tenderElapsedTime", tenderElapsedTime.ToString()), //TODO: not deserialized in ToClass method
                    new XElement("idleElapsedTime", idleElapsedTime.ToString()), //TODO: not deserialized in ToClass method
                    new XElement("lockElapsedTime", lockElapsedTime.ToString()), //TODO: not deserialized in ToClass method
                    new XElement("lineItemsSingleScannedCount", Conversion.ToXmlString(lineItemsSingleScannedCount)),
                    new XElement("lineItemsSingleScannedPercent", Conversion.ToXmlString(lineItemsSingleScannedPercent)),
                    new XElement("lineItemsMultiScannedCount", Conversion.ToXmlString(lineItemsMultiScannedCount)),
                    new XElement("lineItemsMultiScannedPercent", Conversion.ToXmlString(lineItemsMultiScannedPercent)),
                    new XElement("lineItemsKeyedCount", Conversion.ToXmlString(lineItemsKeyedCount)),
                    new XElement("lineItemsKeyedPercent", Conversion.ToXmlString(lineItemsKeyedPercent)),
                    new XElement("keyItemGroupCount", Conversion.ToXmlString(keyItemGroupCount)),
                    new XElement("keyItemGroupPercent", Conversion.ToXmlString(keyItemGroupPercent)),
                    //Don't need to be xml'd if there is nothing in there - the next time periodic discounts are calculated it will be filled out
                    //private Period period; 
                    //private PeriodicDiscount periodicDiscount; 
                    new XElement("oiltax", Conversion.ToXmlString(oiltax)),
                    new XElement("suspendDestination", suspendDestination),
                    new XElement("splitTransaction", Conversion.ToXmlString(splitTransaction)),
                    loyaltyItem.ToXML(),
                    new XElement("markupItem", markupItem.ToXML()),
                    new XElement("creditMemoItem", creditMemoItem.ToXML()),
                    new XElement("receiptIdNumberSequence", receiptIdNumberSequence),
                    new XElement("invoiceComment", invoiceComment),                    
                    new XElement("orgTransactionId", OrgTransactionId),
                    new XElement("orgStore", OrgStore),
                    new XElement("orgTerminal", OrgTerminal),
                    new XElement("orgReceiptId", OrgReceiptId),
                    new XElement("useTransactionService", Conversion.ToXmlString(useTransactionService)),
                    new XElement("taxExempt", Conversion.ToXmlString(TaxExempt)),
                    new XElement("transactionTaxExemptionCode", TransactionTaxExemptionCode),
                    new XElement("useTaxGroupFrom", Conversion.ToXmlString((int)UseTaxGroupFrom)),
                    new XElement("CustomerOrder", CustomerOrder.ToXML()),
                    new XElement("useOverrideTaxGroup", Conversion.ToXmlString(UseOverrideTaxGroup)),
                    new XElement("overrideTaxGroup", OverrideTaxGroup),
                    MenuTypeItem.ToXML(),                    
                    new XElement("isTableTransaction", Conversion.ToXmlString(IsTableTransaction)),
                    new XElement("SplitID", SplitID),
                    new XElement("eftTransactionExtraInfo", EFTTransactionExtraInfo?.ToXml()),
                    new XElement("kdsOrderID", KDSOrderID)
                );

                if (partnerObject != null)
                {                    
                    xRetailTrans.Add(partnerObject.ToXML());
                }                

                #region Tenders
                XElement xTenderLines = new XElement("TenderLines");
                foreach (TenderLineItem tli in TenderLines)
                {                    
                    xTenderLines.Add(tli.ToXML());
                }
                xRetailTrans.Add(xTenderLines);

                XElement xOrgTenderLines = new XElement("OriginalTenderLines");
                foreach (TenderLineItem tli in OriginalTenderLines)
                {
                    xOrgTenderLines.Add(tli.ToXML());
                }
                xRetailTrans.Add(xOrgTenderLines);

                #endregion

                #region Sale Items
                XElement xSalesItems = new XElement("SaleItems");
                foreach (SaleLineItem si in SaleItems)
                {
                    xSalesItems.Add(si.ToXML());
                }
                xRetailTrans.Add(xSalesItems);

                #endregion

                #region Infocodes
                XElement xInfocodes = new XElement("InfocodeLines");
                foreach (InfoCodeLineItem ici in InfoCodeLines)
                {
                    xInfocodes.Add(ici.ToXML());
                }
                xRetailTrans.Add(xInfocodes);
                #endregion

                #region Reprints
                XElement xReprints = new XElement("ReprintLines");
                foreach (ReprintInfo ici in Reprints)
                {
                    xReprints.Add(ici.ToXML());
                }
                xRetailTrans.Add(xReprints);
                #endregion

                #region Tax
                XElement xTaxLines = new XElement("TaxLines");
                foreach (TaxItem ti in TaxLines)
                {
                    xTaxLines.Add(ti.ToXML());
                }
                xRetailTrans.Add(xTaxLines);

                #endregion

                #region Suspended Transaction Answers

                XElement xSuspendAnswers = new XElement("SuspendTransactionAnswers");
                foreach (SuspendedTransactionAnswer ans in SuspendTransactionAnswers)
                {
                    xSuspendAnswers.Add(ans.ToXML());
                }
                xRetailTrans.Add(xSuspendAnswers);

                #endregion

                #region Manual Discounts
                XElement xManualDiscounts = new XElement("ManualDiscounts");
                foreach (DataEntity disc in ManualPeriodicDiscounts)
                {
                    xManualDiscounts.Add(disc.ToXML());
                }
                xRetailTrans.Add(xManualDiscounts);
                #endregion

                xRetailTrans.Add(base.ToXML());
                return xRetailTrans;
            }
            catch (Exception ex)
            {
                errorLogger?.LogMessage(LogMessageType.Error, ex.Message, ex);
                throw;
            }   
        }

        public override void ToClass(XElement xmlTrans, IErrorLog errorLogger = null)
        {
            if (xmlTrans.HasElements)
            {
                IEnumerable<XElement> transElements = xmlTrans.Elements();
                foreach (XElement transElem in transElements)
                {
                    if (!transElem.IsEmpty)
                    {
                        try
                        {
                            switch (transElem.Name.ToString())
                            {
                                case "customer":
                                    customer.ToClass(transElem);
                                    break;
                                case "invoicedCustomer":
                                    invoicedCustomer.ToClass(transElem);
                                    break;
                                case "customerPaysTax":
                                    customerPaysTax = Conversion.XmlStringToBool(transElem.Value);
                                    break;
                                case "customerPurchRequestId":
                                    customerPurchRequestId = transElem.Value;
                                    break;
                                case "lineDiscCalculationType":
                                    lineDiscCalculationType = (LineDiscCalculationTypes)Conversion.XmlStringToInt(transElem.Value);
                                    break;
                                case "amountToAccount":
                                    amountToAccount = Conversion.XmlStringToDecimal(transElem.Value);
                                    break;
                                case "taxIncludedInPrice":
                                    taxIncludedInPrice = Conversion.XmlStringToBool(transElem.Value);
                                    break;
                                case "orgTaxIncludedInPrice":
                                    orgTaxIncludedInPrice = Conversion.XmlStringToBool(transElem.Value);
                                    break;
                                case "taxSettingsChanged":
                                    taxSettingsChanged = Conversion.XmlStringToBool(transElem.Value);
                                    break;
                                case "taxAmount":
                                    taxAmount = Conversion.XmlStringToDecimal(transElem.Value);
                                    break;
                                case "balanceNetAmountWithTax":
                                    balanceNetAmountWithTax = Conversion.XmlStringToDecimal(transElem.Value);
                                    break;
                                case "netAmount":
                                    netAmount = Conversion.XmlStringToDecimal(transElem.Value);
                                    break;
                                case "netAmountWithTax":
                                    netAmountWithTax = Conversion.XmlStringToDecimal(transElem.Value);
                                    break;
                                case "isNetAmountWithTaxRounded":
                                    isNetAmountWithTaxRounded = Conversion.XmlStringToBool(transElem.Value);
                                    break;
                                case "netAmountWithTaxWithoutDiscountVoucher":
                                    netAmountWithTaxWithoutDiscountVoucher = Conversion.XmlStringToDecimal(transElem.Value);
                                    break;
                                case "grossAmount":
                                    grossAmount = Conversion.XmlStringToDecimal(transElem.Value);
                                    break;
                                case "grossAmountWithTax":
                                    grossAmountWithTax = Conversion.XmlStringToDecimal(transElem.Value);
                                    break;
                                case "payment":
                                    payment = Conversion.XmlStringToDecimal(transElem.Value);
                                    break;
                                case "lineDiscount":
                                    lineDiscount = Conversion.XmlStringToDecimal(transElem.Value);
                                    break;
                                case "lineDiscountWithTax":
                                    lineDiscountWithTax = Conversion.XmlStringToDecimal(transElem.Value);
                                    break;
                                case "periodicDiscountAmnt":
                                    periodicDiscountAmnt = Conversion.XmlStringToDecimal(transElem.Value);
                                    break;
                                case "periodicDiscountWithTax":
                                    periodicDiscountWithTax = Conversion.XmlStringToDecimal(transElem.Value);
                                    break;
                                case "totalDiscount":
                                    totalDiscount = Conversion.XmlStringToDecimal(transElem.Value);
                                    break;
                                case "totalDiscountWithTax":
                                    totalDiscountWithTax = Conversion.XmlStringToDecimal(transElem.Value);
                                    break;
                                case "totalManualPctDiscount":
                                    totalManualPctDiscount = Conversion.XmlStringToDecimal(transElem.Value);
                                    break;
                                case "totalManualDiscountAmount":
                                    totalManualDiscountAmount = Conversion.XmlStringToDecimal(transElem.Value);
                                    break;
                                case "calculateTotalDiscount":
                                    calculateTotalDiscount = Conversion.XmlStringToBool(transElem.Value);
                                    break;
                                case "roundingDifference":
                                    roundingDifference = Conversion.XmlStringToDecimal(transElem.Value);
                                    break;
                                case "roundingSalePmtDiff":
                                    roundingSalePmtDiff = Conversion.XmlStringToDecimal(transElem.Value);
                                    break;
                                case "transSalePmtDiff":
                                    transSalePmtDiff = Conversion.XmlStringToDecimal(transElem.Value);
                                    break;
                                case "totalSalesInvoice":
                                    totalSalesInvoice = Conversion.XmlStringToDecimal(transElem.Value);
                                    break;
                                case "totalSalesOrder":
                                    totalSalesOrder = Conversion.XmlStringToDecimal(transElem.Value);
                                    break;
                                case "totalIncomeExpense":
                                    totalIncomeExpense = Conversion.XmlStringToDecimal(transElem.Value);
                                    break;
                                case "noOfItems":
                                    noOfItems = Conversion.XmlStringToDecimal(transElem.Value);
                                    break;
                                case "saleIsReturnSale":
                                    saleIsReturnSale = Conversion.XmlStringToBool(transElem.Value);
                                    break;
                                case "postAsShipment":
                                    postAsShipment = Conversion.XmlStringToBool(transElem.Value);
                                    break;
                                case "employee":
                                    SalesPerson.ToClass(transElem);
                                    break;
                                case "salesPersonId": //FOR BACKWARDS COMPATABILITY SUSPENDED TRANSACTIONS F.EX.
                                    SalesPerson.ID = transElem.Value;
                                    break;
                                case "salesPersonName": //FOR BACKWARDS COMPATABILITY SUSPENDED TRANSACTIONS F.EX.
                                    SalesPerson.Name = transElem.Value;
                                    break;
                                case "salesPersonNameOnReceipt": //FOR BACKWARDS COMPATABILITY SUSPENDED TRANSACTIONS F.EX.
                                    SalesPerson.NameOnReceipt = transElem.Value;
                                    break;
                                case "refundReceiptId":
                                    refundReceiptId = transElem.Value;
                                    break;
                                case "journalId":
                                    journalId = transElem.Value;
                                    break;                                                                
                                case "timeWhenTotalPressed":
                                    timeWhenTotalPressed = Conversion.XmlStringToInt(transElem.Value);
                                    break;
                                case "itemElapsedTime":
                                    //todo - itemElapsedTime = Conversion.XmlStringToInt(transElem.Value);
                                    break;
                                case "tenderElapsedTime":
                                    //todo - tenderElapsedTime = Conversion.XmlStringToInt(transElem.Value);
                                    break;
                                case "idleElapsedTime":
                                    //todo - idleElapsedTime = Conversion.XmlStringToInt(transElem.Value);
                                    break;
                                case "lockElapsedTime":
                                    //todo - lockElapsedTime = Conversion.XmlStringToInt(transElem.Value);
                                    break;
                                case "lineItemsSingleScannedCount":
                                    lineItemsSingleScannedCount = Conversion.XmlStringToLong(transElem.Value);
                                    break;
                                case "lineItemsSingleScannedPercent":
                                    lineItemsSingleScannedPercent = Conversion.XmlStringToDecimal(transElem.Value);
                                    break;
                                case "lineItemsMultiScannedCount":
                                    lineItemsMultiScannedCount = Conversion.XmlStringToLong(transElem.Value);
                                    break;
                                case "lineItemsMultiScannedPercent":
                                    lineItemsMultiScannedPercent = Conversion.XmlStringToDecimal(transElem.Value);
                                    break;
                                case "lineItemsKeyedCount":
                                    lineItemsKeyedCount = Conversion.XmlStringToLong(transElem.Value);
                                    break;
                                case "lineItemsKeyedPercent":
                                    lineItemsKeyedPercent = Conversion.XmlStringToDecimal(transElem.Value);
                                    break;
                                case "keyItemGroupCount":
                                    keyItemGroupCount = Conversion.XmlStringToLong(transElem.Value);
                                    break;
                                case "keyItemGroupPercent":
                                    keyItemGroupPercent = Conversion.XmlStringToDecimal(transElem.Value);
                                    break;
                                case "oiltax":
                                    oiltax = Conversion.XmlStringToDecimal(transElem.Value);
                                    break;
                                case "suspendDestination":
                                    suspendDestination = transElem.Value;
                                    break;
                                case "splitTransaction":
                                    splitTransaction = Conversion.XmlStringToBool(transElem.Value);
                                    break;
                                case "LoyaltyItem":
                                    loyaltyItem.ToClass(transElem);
                                    break;
                                case "markupItem":
                                    markupItem.ToClass(transElem);
                                    break;
                                case "creditMemoItem":
                                    creditMemoItem.ToClass(transElem);
                                    break;
                                case "receiptIdNumberSequence":
                                    receiptIdNumberSequence = transElem.Value;
                                    break;
                                case "invoiceComment":
                                    invoiceComment = transElem.Value;
                                    break;
                                case "orgTransactionId":
                                    OrgTransactionId = transElem.Value;
                                    break;
                                case "orgStore":
                                    OrgStore = transElem.Value;
                                    break;
                                case "orgTerminal":
                                    OrgTerminal = transElem.Value;
                                    break;
                                case "orgReceiptId":
                                    OrgReceiptId = transElem.Value;
                                    break;
                                case "useTransactionService":
                                    useTransactionService = Conversion.XmlStringToBool(transElem.Value);
                                    break;
                                case "taxExempt":
                                    TaxExempt = Conversion.XmlStringToBool(transElem.Value);
                                    break;
                                case "transactionTaxExemptionCode":
                                    TransactionTaxExemptionCode = transElem.Value;
                                    break;
                                case "useTaxGroupFrom":
                                    UseTaxGroupFrom = (UseTaxGroupFromEnum)Conversion.XmlStringToInt(transElem.Value);
                                    break;
                                case "CustomerOrder":
                                    CustomerOrder.ToClass(transElem);
                                    break;
                                case "useOverrideTaxGroup":
                                    UseOverrideTaxGroup = Conversion.XmlStringToBool(transElem.Value);
                                    break;
                                case "overrideTaxGroup":
                                    OverrideTaxGroup = transElem.Value;
                                    break;
                                case "ManualDiscounts":
                                    ManualPeriodicDiscounts = CreateManualPeriodicDiscounts(transElem);
                                    break;
                                case "TenderLines":
                                    TenderLines = CreateTenderLines(transElem);
                                    break;
                                case "OriginalTenderLines":
                                    OriginalTenderLines = CreateTenderLines(transElem);
                                    break;
                                case "SaleItems":
                                    SaleItems = CreateSaleLineItems(transElem);
                                    break;
                                case "InfocodeLines":
                                    InfoCodeLines = CreateInfocodeLines(transElem);
                                    break;
                                case "ReprintLines":
                                    Reprints = CreateReprintLines(transElem);
                                    break;
                                case "TaxLines":
                                    TaxLines = CreateTaxLines(transElem);
                                    break;
                                case "SuspendTransactionAnswers":
                                    SuspendTransactionAnswers = CreateAnswerLines(transElem);
                                    break;
                                case "PartnerObject":
                                    //Partner Object is created outside of the ToClass functionality in DataProviderFactory.Instance.GetProvider<ITransactionData, Transaction>().CreateTransFromXML                                        
                                    partnerXElement = transElem;
                                    break;
                                case "menuTypeItem":
                                    MenuTypeItem.ToClass(transElem);
                                    break;                                
                                case "isTableTransaction":
                                    IsTableTransaction = Conversion.XmlStringToBool(transElem.Value);
                                    break;
                                case "SplitID":
                                    //if (transElem.Value.Trim() != string.Empty)
                                        //SplitID = new Guid(transElem.Value);
                                    SplitID = Conversion.XmlStringToGuid(transElem.Value);
                                    break;
                                case "eftTransactionExtraInfo":
                                    // When we read from file and not at runtime we need to fetch the nested xml. The format would be:
                                    // <eftTransactionExtraInfo>
                                    //   <TheSerializedClass>
                                    //     <Variable1>Value1</Variable1>
                                    //   </TheSerializedClass>
                                    // </eftTransactionExtraInfo>
                                    //
                                    // We need to return <TheSerializedClass> element, so that is why we grab the first element if this is serialized in that way.
                                    //
                                    // At runtime we already have <TheSerializedClass> element so we just return it.
                                    if (transElem.Elements().Count() == 1)
                                    {
                                        eftTransactionExtraInfoXElement = transElem.Elements().First();
                                    }
                                    else
                                    {
                                        eftTransactionExtraInfoXElement = transElem;
                                    }
                                    break;
                                case "kdsOrderID":
                                    KDSOrderID = Conversion.XmlStringToGuid(transElem.Value);
                                    break;
                                default:
                                    base.ToClass(transElem);
                                    break;
                            }
                        }
                        catch (Exception ex)
                        {
                            errorLogger?.LogMessage(LogMessageType.Error, transElem.ToString(), ex);
                            throw;
                        }
                    }
                }
            }                             
        }        

        #endregion
    }
}
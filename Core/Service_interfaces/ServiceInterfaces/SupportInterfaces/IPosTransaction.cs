using System;
using System.Collections.Generic;
using System.Xml.Linq;
using LSOne.DataLayer.BusinessObjects.Auditing;
using LSOne.DataLayer.BusinessObjects.EFT;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.Hospitality;
using LSOne.DataLayer.BusinessObjects.StoreManagement;
using LSOne.Services.Interfaces.SupportClasses.Employee;
using LSOne.Utilities.DataTypes;

namespace LSOne.Services.Interfaces.SupportInterfaces
{
    public interface IPosTransaction : ICloneable, ISerializable
    {
        /// <summary>
        /// The start date and time of the transaction
        /// </summary>
        DateTime BeginDateTime { get; set; }
        /// <summary>
        /// Controls if the customer discounts should always be calculated when an item is added to the transaction or only at payment
        /// </summary>
        CalculateCustomerDiscountEnums CalcCustomerDiscounts { get; set; }
        /// <summary>
        /// Controls if the periodic discounts should always be calculated when an item is added to the transaction or only at payment
        /// </summary>
        CalculatePeriodicDiscountEnums CalcPeriodicDiscounts { get; set; }
        Store.CalculateDiscountsFromEnum CalculateDiscountFrom { get; set; }
        /// <summary>
        /// A comment insertable by the operator
        /// </summary>
        string Comment { get; set; }
        /// <summary>
        /// The orginal terminal the transaction was created on, if the transaction was transfered from another terminal
        /// </summary>
        string CreatedOnTerminalId { get; set; }
        /// <summary>
        /// Serialize the transaction
        /// </summary>
        XDocument CreateXmlTransaction();
        /// <summary>
        /// Controls if amounts are displayed with or without tax in the POS
        /// </summary>
        bool DisplayAmountsIncludingTax { get; set; }
        /// <summary>
        /// The finishing date and time of the transaction
        /// </summary>
        DateTime EndDateTime { get; set; }
        /// <summary>
        /// The status of the transaction, i.e voided, posted, training etc.
        /// </summary>
        TransactionStatus EntryStatus { get; set; }
        /// <summary>
        /// Used in Hospitality to know if the transaction has already been sent to kitchen or printing station
        /// </summary>
        bool HasBeenSentToStation { get; set; }
        IEnumerable<ITenderLineItem> ITenderLines { get; }
        IEnumerable<ITenderLineItem> IOriginalTenderLines { get; set; }

        /// <summary>
        /// This is used for price override and when it is required to key in an item price.
        /// When true, then the value entered contains tax such that tax will not be added on top of this value.
        /// </summary>
        bool KeyedInPriceContainsTax { get; set; }
        /// <summary>
        /// The last operation that was run on the transaction
        /// </summary>
        POSOperations LastRunOperation { get; set; }
        /// <summary>
        /// If the last run operation was a valid payment operation, that is a successful one.
        /// </summary>
        bool LastRunOperationIsValidPayment { get; set; }
        /// <summary>
        /// Was drawer opened during the transaction
        /// </summary>
        bool OpenDrawer { get; set; }
        /// <summary>
        /// The Operator entering the transaction on the POS
        /// </summary>
        Employee Cashier { get; set; }
        /// <summary>
        /// This is the internal primary key in the POSTRANSACTION table. Consists of the terminal id and a sequential number.
        /// </summary>
        string ReceiptId { get; set; }
        void Save();
        bool SelectiveDiscountCalculation();
        /// <summary>
        /// The shift date
        /// </summary>
        DateTime ShiftDate { get; set; }
        /// <summary>
        /// The shift id
        /// </summary>
        string ShiftId { get; set; }
        /// <summary>
        /// The way a statement is done for the store (staff, terminal, total).
        /// </summary>
        StatementGroupingMethod StatementMethod { get; set; }
        /// <summary>
        /// The store address
        /// </summary>
        string StoreAddress { get; set; }
        /// <summary>
        /// The Store currency code - i.e. GBP, USD, EUR etc.
        /// </summary>
        string StoreCurrencyCode { get; set; }
        /// <summary>
        /// The exchange rate between HO currency and Store currency.        
        /// </summary>
        decimal StoreExchangeRate { get; set; }
        /// <summary>
        /// The store id
        /// </summary>
        string StoreId { get; set; }
        /// <summary>
        /// The store name
        /// </summary>
        string StoreName { get; set; }
        /// <summary>
        /// The store tax group
        /// </summary>
        string StoreTaxGroup { get; set; }
        /// <summary>
        /// The terminal id. A unique id for each terminal inside each store.
        /// </summary>
        string TerminalId { get; set; }
        /// <summary>
        /// If the till is in training mode.
        /// </summary>
        bool Training { get; set; }
        /// <summary>
        /// It is assigned quite late in the transaction process and thus not available from the beginning.
        /// The assignment happens in TransactionData.cs with the call to its method 
        /// "public string GetNextTransactionId(string transactionIdNumberSequence)"
        /// that fetches the next number from the table NUMBERSEQUENCETABLE.
        /// </summary>
        string TransactionId { get; set; }
        /// <summary>
        /// Holds the SUSPENDEDTRANS value
        /// </summary>
        string SuspendedId { get; set; }
        /// <summary>
        /// A running number according to the SALESSEQUENCE number series
        /// </summary>
        string SalesSequenceID { get; set; }
        /// <summary>
        /// The id of the number sequence for the transaction ids
        /// </summary>
        string TransactionIdNumberSequence { get; set; }
        TypeOfTransaction TransactionType();
        /// <summary>
        /// If true then this transaction was recalled and re-created when the POS was started
        /// </summary>
        bool UnconcludedTransaction { get; set; }
        List<OperationAuditing> AuditingLines { get; set; }
        /// <summary>
        /// If true then the transaction has been returned using the local database
        /// </summary>
        bool ReturnedLocally { get; set; }
        DateTime BusinessDay { get; set; }
        DateTime BusinessSystemDay { get; set; }
        List<IReceiptInfo> Receipts { get; set; }

        /// <summary>
        /// Should the receipt for the transaction be printed, emailed or even both.
        /// </summary>
        ReceiptSettingsEnum ReceiptSettings { get; set; }

        /// <summary>
        /// If the receipt for the transaction should be emailed the adderss is kept here.
        /// </summary>
        string ReceiptEmailAddress { get; set; }

        List<POSOperations> OperationStack { get; set; }

        void AddReceipt(string receiptString, RecordIdentifier formType, string documentName, string documentLocation, int formWidth, bool isEmailReceipt);
        
        /// <summary>
        /// Set to true if the current selected item in the transaction should remain selected after executing a blank operation.
        /// This value is not saved to the database and must be set after each blank operation.
        /// </summary>
        bool KeepRowSelectionOnBlankOperation { get; set; }

        /// <summary>
        /// Gets or sets the original number of lines (sales, payments, tax, discounts, infocodes, etc.) created with the transaction.
        /// This should only be set when the transaction is saved on the POS.
        /// This value can be used to check if the current transaction contains the correct amount of lines as when it was created (ex. replication).
        /// </summary>
        int OriginalNumberOfTransactionLines { get; set; }

        /// <summary>
        /// Sums up all transaction lines from the current transaction. This value should be set as the <see cref="OriginalNumberOfTransactionLines"/> before saving the new transaction.
        /// This should be overriden and implemented based on the type of transaction.
        /// </summary>
        /// <returns>Total transaction lines</returns>
        int CalculateTotalNumberOfTransactionLines();

        /// <summary>
        /// Transaction information in HTML format that can be displayed in a HTML information panel
        /// </summary>
        string HTMLInformation { get; set; }

        /// <summary>
        /// Hospitality information if the transaction was created through the hospitality functionality
        /// </summary>
        HospitalityItem Hospitality { get; set; }

        /// <summary>
        /// Temporary data saved before performing an EFT payment
        /// </summary>
        PendingEFTTransaction PendingEFTTransaction { get; set; }

        /// <summary>
        /// A list of all generated EFT transactions
        /// </summary>
        List<PendingEFTTransaction> EFTTransactions { get; set; }

        /// <summary>
        /// The unique ID of this POS transaction.
        /// </summary>
        Guid ID { get; set; }
    }
}
using System;
using System.Collections.Generic;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Companies;
using LSOne.DataLayer.BusinessObjects.Customers;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.EOD;
using LSOne.DataLayer.TransactionObjects.Enums;

namespace LSOne.DataLayer.TransactionObjects.EOD
{

    /// <summary>
    /// 
    /// The class EODInfo is used to gather (and carry) all data that is necessary to perform the report. After this instance has been 
    /// assigned it's data, it is passed to the function that calculates the report layout.
    /// For example, a call occurs from class ReportData.cs.
    /// 
    /// </summary>
    /// 
    public class EODInfo
    {
        /// <summary>
        /// In the constructor, three linked lists are created in order to carry items of type VatInfo, TenderInfo and NoSaleInfo. 
        /// </summary>
        public EODInfo()
        {
            LineDiscountAmount = decimal.Zero;
            PeriodicDiscountAmount = decimal.Zero;
            CompleteDiscountAmount = decimal.Zero;
            TotalDiscountAmount = decimal.Zero;
            LineDiscountAmountInclTax = decimal.Zero;
            PeriodicDiscountAmountInclTax = decimal.Zero;
            TotalDiscountAmountInclTax = decimal.Zero;
            CompleteDiscountAmountInclTax = decimal.Zero;
            TotalBankDrop = decimal.Zero;
            TotalSafeDrop = decimal.Zero;
            Terminal = "";
            TotalSalesInvoice = decimal.Zero;
            TotalSalesOrder = decimal.Zero;
            TotalTenderRemoval = decimal.Zero;
            TotalFloatEntry = decimal.Zero;
            TotalNetAmountInclTax = decimal.Zero;
            TransUpperBound = "";
            TransLowerBound = "";
            Time = "";
            Date = "";
            StoreName = "";
            StoreID = "";
            OperatorName = "";
            OperatorID = "";
            TotalSafeDropRev = decimal.Zero;
            TotalBankDropRev = decimal.Zero;
            StartAmountDeclaration = decimal.Zero;
            DataAreaId = "";
            ZNetAmount = decimal.Zero;
            ZGrossAmount = decimal.Zero;
            TotalNetAmount = decimal.Zero;
            DiscountLines = new List<DiscountInfo>();
            VatInfoLines = new List<VatInfo>();
            TenderInfoLines = new List<TenderInfo>();
            OverShortTenderInfoLines = new List<TenderInfo>();
            CurrencyInfoLines = new List<CurrenciesInfo>();
            NoSaleInfoLines = new List<NoSaleInfo>();
            ChangeBackLines = new List<ChangeBackLine>();
            ZReports = new List<ZReport>();
            IncomeExpenseAccountLines = new List<IncomeExpenseAccount>();
            OtherInfoLines = new List<OtherInfo>();
            TenderDeclarationLines = new List<TenderDeclarationLine>();
            CustomerDepositLines = new List<CustomerDepositLine>();

            CurrentZReport = new ZReport();
            CancelReport = false;
        }

        #region Properties

        public ZReport CurrentZReport { get; set; }
        public bool CancelReport { get; set; }
        public List<ZReport> ZReports { get; set; }
        public List<ChangeBackLine> ChangeBackLines { get; set; }
        public CompanyInfo CompanyInformation { get; set; }

        /// <summary>
        /// The number of receipt copies printed
        /// </summary>
        public int NumberOfReceiptCopies { get; set; }
        /// <summary>
        /// The total sum of the transactions where the receipt copies were made
        /// </summary>
        public decimal TotalOfReceiptCopiesTransactions { get; set; }
        /// <summary>
        /// The number of training transactions done 
        /// </summary>
        public int NumberOfTrainingTransactions { get; set; }
        /// <summary>
        /// The total sum of the training transactions
        /// </summary>
        public decimal TotalOfTrainingTransactions { get; set; }

        /// <summary>
        /// Contains all counted amounts for each tender type counted
        /// </summary>
        public List<TenderDeclarationLine> TenderDeclarationLines { get; set; }

        public TenderDeclarationCalculation TenderDeclarationCalculation { get; set; }

        /// <summary>
        /// Contains all the customer deposits that were done for the X/Z report period
        /// </summary>
        public List<CustomerDepositLine> CustomerDepositLines { get; set; }

        /// <summary>
        /// The total of all retail transactions.
        /// The amount of all NetAmount * (-1).
        /// </summary>
        public decimal TotalNetAmount { get; set; }

        /// <summary>
        /// Total of all returned transactions including tax
        /// </summary>
        public decimal TotalReturnNetAmountInclTax { get; set; }

        /// <summary>
        /// Total of all returned transactions excluding tax
        /// </summary>
        public decimal TotalReturnNetAmount { get; set; }

        /// <summary>
        /// Total of all gross amounts ever sold
        /// </summary>
        public decimal ZGrossAmount { get; set; }

        /// <summary>
        /// Total of all net amounts ever sold
        /// </summary>
        public decimal ZNetAmount { get; set; }

        /// <summary>
        /// Total of all gross amounts ever returned
        /// </summary>
        public decimal ZReturnGrossAmount { get; set; }

        /// <summary>
        /// Total of all net amounts ever returned
        /// </summary>
        public decimal ZReturnNetAmount { get; set; }

        /// <summary>
        /// Total rounding difference 
        /// </summary>
        public decimal RoundingDifference { get; set; }

        /// <summary>
        /// The currently used company id
        /// </summary>
        public string DataAreaId { get; set; }

        /// <summary>
        /// A list of all discounts to be included in the X or Z report
        /// </summary>
        public List<DiscountInfo> DiscountLines { get; set; }

        /// <summary>
        /// A list of all VAT (tax) information to be included in the X or Z report
        /// </summary>
        public List<VatInfo> VatInfoLines { get; set; }

        /// <summary>
        /// A list of all tender information to be included in the X or Z report
        /// </summary>
        public List<TenderInfo> TenderInfoLines { get; set; }

        /// <summary>
        /// A list of all tender information to be included in the Z report, showing over / short amounts
        /// </summary>
        public List<TenderInfo> OverShortTenderInfoLines { get; set; }

        public List<CurrenciesInfo> CurrencyInfoLines { get; set; }

        /// <summary>
        /// A list of "No sale" items to be included in the X or Z report
        /// </summary>
        public List<NoSaleInfo> NoSaleInfoLines { get; set; }

        /// <summary>
        /// A list of "Other information" items to be included in the X or Z report
        /// </summary>
        public List<OtherInfo> OtherInfoLines { get; set; }

        /// <summary>
        /// A list of Income/Expense accounts to be included in the X or Z report
        /// </summary>
        public List<IncomeExpenseAccount> IncomeExpenseAccountLines { get; set; }

        public decimal StartAmountDeclaration { get; set; }

        public decimal TotalBankDropRev { get; set; }

        public decimal TotalSafeDropRev { get; set; }

        /// <summary>
        /// Total amount of all deposits paid for customer orders
        /// </summary>
        public decimal TotalDeposits;

        /// <summary>
        /// Total amount of all redeemed deposits 
        /// </summary>
        public decimal TotalRedeemedDepositsAmount;

        /// <summary>
        /// The operation that executes the report printing, takes as a parameter an internal POSTransaction,
        /// which contains the ID of the current operator.
        /// </summary>
        public string OperatorID { get; set; }

        /// <summary>
        /// The operation that executes the report printing, takes as a parameter an internal POSTransaction,
        /// which contains the name of the current operator from the Applicationsettings.
        /// </summary>
        public string OperatorName { get; set; }

        /// <summary>
        /// The operation that executes the report printing, takes as a parameter an internal POSTransaction,
        /// which contains the ID of the current store from the Applicationsettings.
        /// </summary>
        public string StoreID { get; set; }

        /// <summary>
        /// The current store name from the Applicationsettings that is contained by the InternalTransaction.
        /// </summary>
        public string StoreName { get; set; }

        public string BusinessDay { get; set; }

        /// <summary>
        /// When a report is requested, a new posTransaction is created. At creation time, the current date and time is stored in the transaction instance.
        /// The date is the date when this (internal) transaction was created.
        /// </summary>
        public string Date { get; set; }

        /// <summary>
        /// When a report is requested, a new posTransaction is created. At creation time, the current date and time is stored in the transaction instance.
        /// The time is the time when this (internal) transaction was created.
        /// </summary>
        public string Time { get; set; }

        /// <summary>
        /// The number of normal transactions as identified by the stored procedure LSR_ZREPORT, where a transaction is 
        /// 'normal' when being of TypeOfTransaction == 2 (sales transaction; amount being less than 0).
        /// </summary>
        public int NumberOfNormalTransactions { get; set; }

        /// <summary>
        /// As in 'NumberOfNormalTransactions', the transaction is also '2', but since the amount is bigger than zero, they count as return transactions.
        /// </summary>
        public int NumberOfReturnTransactions { get; set; }

        /// <summary>
        /// Refers to retail transactions, having entrystatus == 1.
        /// </summary>
        public int NumberOfVoidTransactions { get; set; }

        /// <summary>
        /// Number of deposit transactions created by customer orders
        /// </summary>
        public int NumberOfDepositTransactions;

        /// <summary>
        /// Number of transactions with a customer on them.
        /// </summary>
        public int NumberOfCustomerTransactions { get; set; }


        /// <summary>
        /// Total number of items sold in the transactions included in the Z report
        /// </summary>
        public decimal NumberOfItemsSold { get; set; }

        /// <summary>
        /// Total number of items returned in the transactions included in the Z report
        /// </summary>
        public decimal NumberOfItemsReturned { get; set; }

        /// <summary>
        /// Number of drawer openings in the transactions included in the Z report
        /// </summary>
        public int NumberOfDrawerOpenings { get; set; }

        /// <summary>
        /// Number of times the Open drawer operation was run
        /// </summary>
        public int NumberOfOpenDrawerOperations { get; set; }

        /// <summary>
        /// Number of login transactions included in the Z report
        /// </summary>
        public int NumberOfLogins { get; set; }

        /// <summary>
        /// A total count of all transactions no matter what type
        /// </summary>
        public int TotalNumberOfTransactions { get; set; }

        /// <summary>
        /// The last transaction ID that has been found.
        /// </summary>
        public string TransLowerBound { get; set; }

        /// <summary>
        /// The first transaction ID that has been found.
        /// </summary>
        public string TransUpperBound { get; set; }

        /// <summary>
        /// Date specifying that transactions younger than this date are considered. Limits the date that oldest records may have.
        /// </summary>
        public DateTime TransDateLowerBound { get; set; }

        /// <summary>
        /// Date specifying that transactions older than this date are considered. Limits the date that youngest records may have.
        /// </summary>
        public DateTime TransDateUpperBound { get; set; }

        /// <summary>
        /// The total of all retail transactions.
        /// The amount of all GrossAmount * (-1).
        /// </summary>
        public decimal TotalNetAmountInclTax { get; set; }

        /// <summary>
        /// When the cash box is filled with change money, this is done via an transaction called "FloatEntry".
        /// When the transaction is of type FloatEntry, the (positive) GrossAmount is added to the existing FloatEntry.
        /// </summary>
        public decimal TotalFloatEntry { get; set; }

        /// <summary>
        /// When the transaction is of type TenderRemoval (i.e. it is possible to take money from the cash box; to be configured in table
        /// RBOSTORETENDERTYPETABLE), the (positive) GrossAmount is added to the existing TenderRemoval, thus yielding the total amount of the 
        /// tender that has been removed.
        /// </summary>
        public decimal TotalTenderRemoval { get; set; }

        /// <summary>
        /// A SalesOrder being a special case of a RetailTransaction, where a customer orders something that is not simultaneously delivered.
        /// TotalSalesOrder is the (positive) GrossAmount of SalesOrder values added together.
        /// </summary>
        public decimal TotalSalesOrder { get; set; }

        /// <summary>
        /// The total amount of the invoices that have been paid by customers. 
        /// An entry to this database field is done when activating the sales invoice operation at the POS.
        /// </summary>
        public decimal TotalSalesInvoice { get; set; }

        /// <summary>
        /// Used from class ReportLogic to get or set the report type (i.e. X report or Z report).
        /// </summary>
        public ReportType ReportType { get; set; }

        /// <summary>
        /// The terminal ID that belongs to the current transaction.
        /// </summary>
        public string Terminal { get; set; }

        public string ExtraHeaderText { get; set; }

        public decimal TotalSafeDrop { get; set; }

        public decimal TotalBankDrop { get; set; }

        /// <summary>
        /// A total sum of all discounts including tax
        /// </summary>
        public decimal CompleteDiscountAmountInclTax { get; set; }

        /// <summary>
        /// A sum of all total discounts including tax
        /// </summary>
        public decimal TotalDiscountAmountInclTax { get; set; }

        /// <summary>
        /// A sum of all periodic discounts including tax
        /// </summary>
        public decimal PeriodicDiscountAmountInclTax { get; set; }

        /// <summary>
        /// A sum of all line discounts including tax
        /// </summary>
        public decimal LineDiscountAmountInclTax { get; set; }

        /// <summary>
        /// A sum of all total discounts (not including tax)
        /// </summary>
        public decimal TotalDiscountAmount { get; set; }

        /// <summary>
        /// A sum of all discounts (not including tax)
        /// </summary>
        public decimal CompleteDiscountAmount { get; set; }

        /// <summary>
        /// A sum of all periodic discounts (not including tax)
        /// </summary>
        public decimal PeriodicDiscountAmount { get; set; }

        /// <summary>
        /// A sum of all line discounts (not including tax)
        /// </summary>
        public decimal LineDiscountAmount { get; set; }

        public decimal GrossSalesInclTax
        {
            get
            {
                return TotalNetAmountInclTax + CompleteDiscountAmountInclTax;
            }
        }

        public decimal GrossSales
        {
            get
            {
                return TotalNetAmount + CompleteDiscountAmount;
            }
        }

        public decimal GrossReturnsInclTax
        {
            get
            {
                return TotalReturnNetAmountInclTax;
            }
        }

        public decimal GrossReturns
        {
            get
            {
                return TotalReturnNetAmount;
            }
        }

        public decimal TotalNetSale
        {
            get
            {
                return TotalNetAmount + TotalReturnNetAmount;
            }
        }

        public decimal TotalNetSaleInclTax
        {
            get
            {
                return TotalNetAmountInclTax + TotalReturnNetAmountInclTax;
            }
        }

        /// <summary>
        /// Returns 'true' in the case of at least one sales transaction.
        /// </summary>
        public bool SalesInfoExist
        {
            get
            {
                return (TotalNetAmountInclTax
              + NumberOfReturnTransactions
              + NumberOfNormalTransactions
              + NumberOfVoidTransactions) > 0;
            }
        }

        #endregion

        public bool PrintSalesReport()
        {
            return (CurrentZReport.EntryType != 2);
        }
    }
}

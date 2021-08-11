using System;
using System.Collections.Generic;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.TransactionObjects.Line.IFSaleItem;
using LSOne.DataLayer.TransactionObjects.Line.IFTenderItem;
using LSOne.DataLayer.TransactionObjects.Line.MarkupItem;
using LSOne.Services.Interfaces.SupportInterfaces;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.TransactionObjects
{
    /// <summary>
    /// The standard sale or return transaction, containing items and payments.
    /// </summary>
    [Serializable]
    public class IFRetailTransaction
    {
        #region Member variables
        private Date beginDateTime = Date.Empty;     //The start date and time of the transaction
        private Date endDateTime = Date.Empty;       //The end date and time of the transaction
        private decimal netAmount;              //The total net amount(grossamount minus discounts) exluding tax 
        private decimal netAmountWithTax;

        private decimal payment;                //The total payment minus (amountWithTax plus rounded) should be zero
        private decimal totalDiscount;          //The total discount given in this transaction minus the linediscount(tax excluded).
        private decimal roundingDifference;     //Rounding difference between summed up item lines and what the customer should be charged.
        private decimal roundingSalePmtDiff;    //Rouding difference between payment and sales amount. Sales can be 9,99 but payment can be 10
        private decimal totalSalesInvoice;      //The total amount of sales invoices paid in the transaction
        private decimal totalSalesOrder;        //The total amount of sales orders paid in the transaction
        private decimal totalIncomeExpense;     //The tatal amount of income and expense accounts in the transaction

        //TotalLines
        private decimal noOfItems;              //Number of items in the transaction - the total quantity.
        private decimal oiltax;                 //The total oiltax for the item

        //Markup
        private MarkupItem markupItem;          //Markup information for the transaction
        public RecordIdentifier TransactionID { get; set; }

        public TransactionStatus EntryStatus { get; set; }
        public string StoreCurrencyCode { get; set; }
        public TypeOfTransaction TypeOfTransaction { get; set; }
        public int TransactionSubType { get; set; }

        /// <summary>
        /// The start date and time of the transaction
        /// </summary>
        public DateTime BeginDateTime
        {
            get { return beginDateTime.DateTime; }
            set { beginDateTime = new Date(value, true); }
        }
        /// <summary>
        /// The finishing date and time of the transaction
        /// </summary>
        public DateTime EndDateTime
        {
            get { return endDateTime.DateTime; }
            set { endDateTime = new Date(value, true); }
        }

        /// <summary>
        /// The total net amount(grossamount minus discounts) exluding tax. 
        /// </summary>
        public decimal NetAmount
        {
            get { return netAmount; }
            set { netAmount = value; }
        }
        /// <summary>
        /// The total net amount(grossamount minus discounts) including tax. 
        /// </summary>
        public decimal NetAmountWithTax
        {
            get { return netAmountWithTax; }
            set
            {
                netAmountWithTax = value;
            }
        }

        ///// <summary>
        ///// The total payment including the vat
        ///// </summary>
        public decimal Payment
        {
            get { return payment; }
            set
            {
                payment = value;
            }
        }

        /// <summary>
        /// The total discount given in this transaction excluding the linediscount(tax excluded).
        /// </summary>
        public decimal TotalDiscount
        {
            get { return totalDiscount; }
            set { totalDiscount = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public decimal RoundingDifference
        {
            get { return roundingDifference; }
            set { roundingDifference = value; }
        }

        /// <summary>
        /// Rouding difference between payment and sales amount. Sales can be 9,99 but payment can be 10
        /// </summary>
        public decimal RoundingSalePmtDiff
        {
            get { return roundingSalePmtDiff; }
            set { roundingSalePmtDiff = value; }
        }
        
        /// <summary>
        /// Number of items in the transaction - the total quantity.
        /// </summary>
        public decimal NoOfItems
        {
            get { return noOfItems; }
            set { noOfItems = value; }
        }

        /// <summary>
        /// Markup information for the transaction
        /// </summary>
        public IMarkupItem MarkupItem
        {
            get { return markupItem; }
            set { markupItem = (MarkupItem)value; }
        }

        /// <summary>
        /// The total calculated oiltax amount for the fuel items.
        /// </summary>
        public decimal Oiltax
        {
            get { return oiltax; }
            set { oiltax = value; }
        }

        /// <summary>
        /// The total amount of sales orders payments in the transaction
        /// </summary>
        public decimal SalesOrderAmounts
        {
            get { return totalSalesOrder; }
            set { totalSalesOrder = value; }
        }

        /// <summary>
        /// The total amount of sales invoice payments in the transaction
        /// </summary>
        public decimal SalesInvoiceAmounts
        {
            get { return totalSalesInvoice; }
            set { totalSalesInvoice = value; }
        }

        /// <summary>
        /// The total amount of income and expense accounts in the transaction
        /// </summary>
        public decimal IncomeExpenseAmounts
        {
            get { return totalIncomeExpense; }
            set { totalIncomeExpense = value; }
        }

        #endregion

        /// <summary>
        /// Contains RetailTransactionLineItems
        /// </summary>
        public List<IFTenderLineItem> TenderLines { get; set; }
        public List<IFSaleLineItem> SaleItems { get; set; }

        public IFRetailTransaction()
        {
            Initialize();
        }

        private void Initialize()
        {
            TransactionID = "";
            TenderLines = new List<IFTenderLineItem>();
            SaleItems = new List<IFSaleLineItem>();
            markupItem = new MarkupItem();
            BeginDateTime = DateTime.Now;
            EndDateTime = DateTime.Now;
            totalSalesInvoice = 0M;
            totalSalesOrder = 0M;
            totalIncomeExpense = 0M;
        }
    }
}

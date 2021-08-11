using System;
using System.Collections.Generic;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.BusinessObjects.Transactions
{
    [Serializable]
    public class Transaction : BaseTransaction
    {        
        public Transaction() 
            : base()
        {
            CurrencyID = "";
            ReceiptID = "";
            EmployeeID = "";
            CreatedOnPosTerminal = "";
            TransactionDate = Date.Empty;
            ShiftID = "";
            ShiftDate = Date.Empty;
            StatementCode = "";
            CustomerID = "";
            CustomerName = "";
            CustomerPurchRequestId = "";
            InvoiceComment = "";
        }
        
        public TypeOfTransaction Type { get; set; }
        public RecordIdentifier CurrencyID { get; set; }
        public RecordIdentifier ReceiptID { get; set; }
        public RecordIdentifier EmployeeID { get; set; }
        public RecordIdentifier CreatedOnPosTerminal { get; set; }
        public TransactionStatus EntryStatus { get; set; }
        public Date TransactionDate { get; set; }
        public RecordIdentifier ShiftID { get; set; }
        public Date ShiftDate { get; set; }
        public bool OpenDrawer { get; set; }
        public string StatementCode { get; set; }
        public decimal ExchangeRate { get; set; }
        public RecordIdentifier CustomerID {get; set;}
        public string CustomerName { get; set; }
        public RecordIdentifier CustomerPurchRequestId { get; set; }
        public decimal NetAmount { get; set; }
        public decimal NetAmountWithTax { get; set; }
        public decimal SalesOrderAmounts { get; set; }
        public decimal SalesInvoceAmounts { get; set; }
        public decimal IncomeExpenseAmounts { get; set; }
        public decimal RoundingDifference { get; set; }
        public decimal RoundingSalePmtDiff { get; set; }
        public decimal PaymentAmount { get; set; }
        public decimal MarkupAmount { get; set; }
        public bool HasMarkup { get; set; }
        public string MarkupDescription { get; set; }
        public decimal AmountToAccount { get; set; }
        public decimal TotalDiscountAmount { get; set; }
        public decimal NumberOfItems { get; set; }
        public string InvoiceComment { get; set; }
        public string ReceiptEmailAddress { get; set; }
        public bool HasOilTax { get; set; }
        public decimal OilTax { get; set; }
        public bool TaxIncludedInPrice { get; set; }
        public bool HasTaxIncludedInPriceFlag { get; set; }
        public DateTime EndDateTime { get; set; }

        /// <summary>
        /// Login of the employee. Only used for display
        /// </summary>
        public string Login { get; set; }

        public List<SalesTransaction> SalesTransactionList { get; set; }

        public string TypeDescription
        {
            get
            {
                switch (Type)
                {
                    case TypeOfTransaction.Sales :
                        return Properties.Resources.Sale;
                    case TypeOfTransaction.SalesOrder :
                        return Properties.Resources.SalesOrders;
                    case TypeOfTransaction.SalesInvoice :
                        return Properties.Resources.SalesInvoices;
                    case TypeOfTransaction.Payment:
                        return Properties.Resources.Payment;
                    case TypeOfTransaction.Deposit:
                        return Properties.Resources.Deposit;
                    default : return "";
                }
            }
        }

        public static List<DataEntity> GetTypes()
        {
            List<DataEntity> list = new List<DataEntity>();
            list.Add(new DataEntity((int)TypeOfTransaction.Sales, Properties.Resources.Sale));
            list.Add(new DataEntity((int)TypeOfTransaction.SalesOrder, Properties.Resources.SalesOrders));
            list.Add(new DataEntity((int)TypeOfTransaction.SalesInvoice, Properties.Resources.SalesInvoices));
            list.Add(new DataEntity((int)TypeOfTransaction.Payment, Properties.Resources.Payment));
            list.Add(new DataEntity((int)TypeOfTransaction.Deposit, Properties.Resources.Deposit));

            return list;
        }
    }
}
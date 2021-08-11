using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.HelperObjects;
using LSOne.Services.Interfaces.SupportInterfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace LSOne.Services.Interfaces.EventArguments
{
    public enum ReceiptOrigin
    {
        POS = 0,
        Hospitality = 1,
        Journal = 2,
        DualDisplay = 3,
        RecallSuspended = 4,
        ReturnTransaction = 5,
        CustomerTransactions = 6,
        RecallCustomerOrder = 7
    }

    public class PreDisplayReceiptItemArgs : EventArgs
    {
        public ISaleLineItem SaleLine { get; private set; }
        public bool DisplayReceiptItem { get; set; }
        public ReceiptOrigin DisplayedWhere { get; set; }

        public PreDisplayReceiptItemArgs(ISaleLineItem saleLine, bool displayReceiptItem, ReceiptOrigin displayedWhere)
            : base()
        {
            this.SaleLine = saleLine;
            this.DisplayReceiptItem = displayReceiptItem;
            this.DisplayedWhere = displayedWhere;
        }
    }

    public class SaleItemDataChangeArgs : EventArgs
    {
        public ReceiptItemInfo ReceiptItem { get; private set; }
        public ISaleLineItem SaleLine { get; private set; }

        public SaleItemDataChangeArgs(ReceiptItemInfo ReceiptItem, ISaleLineItem SaleLine)
            : base()
        {
            this.ReceiptItem = ReceiptItem;
            this.SaleLine = SaleLine;
        }
    }

    public class CustomerDepositDataChangeArgs : EventArgs
    {
        public ReceiptItemInfo ReceiptItem { get; private set; }
        public ICustomerDepositItem DepositItem { get; private set; }

        public CustomerDepositDataChangeArgs(ReceiptItemInfo ReceiptItem, ICustomerDepositItem DepositItem)
            : base()
        {
            this.ReceiptItem = ReceiptItem;
            this.DepositItem = DepositItem;
        }
    }

    public class TenderLineDataChangeArgs : EventArgs
    {
        public DataRow PaymentItem { get; private set; }
        public ITenderLineItem TenderItem { get; private set; }
        /// <summary>
        /// The RetailTransaction (type Sales) and CustomerPayment (type Payment) transaction are the only ones that can fire off a Tender Data Change event
        /// </summary>
        public TypeOfTransaction TransactionType { get; private set; }


        public TenderLineDataChangeArgs(DataRow PaymentItem, ITenderLineItem TenderItem, TypeOfTransaction TransactionType)
            : base()
        {
            this.PaymentItem = PaymentItem;
            this.TenderItem = TenderItem;
            this.TransactionType = TransactionType;
        }
    }

    public class TotalsDataChangeArgs : EventArgs
    {
        public TotalsInfo Totals { get; private set; }
        public IPosTransaction Transaction { get; private set; }

        /// <summary>
        /// The RetailTransaction (type Sales) and CustomerPayment (type Payment) transaction are the only ones that can fire off a Tender Data Change event
        /// </summary>
        public TypeOfTransaction TransactionType { get; private set; }

        public TotalsDataChangeArgs(TotalsInfo Totals, TypeOfTransaction TransactionType, IPosTransaction posTransaction)
            : base()
        {
            this.Totals = Totals;
            this.TransactionType = TransactionType;
            this.Transaction = posTransaction;
        }
    }
}

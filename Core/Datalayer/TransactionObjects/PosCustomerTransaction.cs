using System;
using LSOne.DataLayer.BusinessObjects;
using LSOne.Utilities.Attributes;
using LSOne.Utilities.DataTypes;
using LSOne.Utilities.Validation;

namespace LSOne.DataLayer.TransactionObjects
{
    public class PosCustomerTransaction : DataEntity
    {
        public enum TransactionTypes
        {
            Sales = 0,
            Payment = 1,
            NotCharged = 3
        }

        public PosCustomerTransaction()
        {
            CustomerID = RecordIdentifier.Empty;
            StoreID = RecordIdentifier.Empty;
            TerminalID = RecordIdentifier.Empty;
            TransactionID = RecordIdentifier.Empty;
            ReceiptID = RecordIdentifier.Empty;
            Amount = 0;
            Currency = RecordIdentifier.Empty;
        }
        /// <summary>
        /// CustomerID, TransactionID, TransactionDate, TerminalID, StoreID.
        /// </summary>
        [RecordIdentifierConstruction(typeof(string), typeof(string), typeof(DateTime), typeof(string), typeof(string))]
        public override RecordIdentifier ID
        {
            get
            {
                return new RecordIdentifier(CustomerID, new RecordIdentifier(TransactionID, new RecordIdentifier(TransactionDate, new RecordIdentifier(TerminalID, StoreID))));
            }
            set
            {
                CustomerID = value.PrimaryID;
                TransactionID = value.SecondaryID.PrimaryID;
                TransactionDate = (DateTime)value.SecondaryID.SecondaryID.PrimaryID;
                TerminalID = value.SecondaryID.SecondaryID.SecondaryID.PrimaryID;
                StoreID = value.SecondaryID.SecondaryID.SecondaryID.SecondaryID.PrimaryID;
            }
        }

        public string TransactionTypeString
        {
            get
            {
                if (TransactionType == TransactionTypes.Payment)
                {
                    return Properties.Resources.PaymentTransactionType;
                }
                return Properties.Resources.SalesTransactionType;
            }
        }

        [RecordIdentifierValidation(20)]
        public RecordIdentifier CustomerID { get; set; }
        public DateTime TransactionDate { get; set; }
        [RecordIdentifierValidation(20)]
        public RecordIdentifier StoreID { get; set; }
        [RecordIdentifierValidation(20)]
        public RecordIdentifier TerminalID { get; set; }
        [RecordIdentifierValidation(20)]
        public RecordIdentifier TransactionID { get; set; }
        [RecordIdentifierValidation(61)]
        public RecordIdentifier ReceiptID { get; set; }
        public TransactionTypes TransactionType { get; set; }
        public decimal Amount { get; set; }
        public RecordIdentifier Currency { get; set; }

        public override object Clone()
        {
            var trans = new PosCustomerTransaction();
            Populate(trans);
            return trans;
        }

        protected void Populate(PosCustomerTransaction trans)
        {
            base.Populate(trans);
            trans.CustomerID = (RecordIdentifier)CustomerID.Clone();
            trans.TransactionDate = TransactionDate;
            trans.StoreID = (RecordIdentifier)StoreID.Clone();
            trans.TerminalID = (RecordIdentifier)TerminalID.Clone();
            trans.TransactionID = (RecordIdentifier)TransactionID.Clone();
            trans.ReceiptID = (RecordIdentifier)ReceiptID.Clone();
            trans.TransactionType = TransactionType;
            trans.Amount = Amount;
            trans.Currency = (RecordIdentifier)Currency.Clone();
        }
    }
}

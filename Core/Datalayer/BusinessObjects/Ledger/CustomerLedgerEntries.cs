using System;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.BusinessObjects.Ledger
{

	public class CustomerLedgerEntries : DataEntity
	{

        public enum StatusEnum
        {
		    Closed = 0,
		    Open = 1
        }

	    public enum TypeEnum
	    {
		    Payment = 0,
		    Invoice = 1,
		    CreditMemo = 2,
            Sale = 3,
            Discount = 4
	    };

        public int EntryNo { get; set; }
        public string DataAreaId { get; set; }
        public DateTime PostingDate { get; set; }
        public RecordIdentifier Customer { get; set; }
        public TypeEnum EntryType { get; set; }
        public RecordIdentifier DocumentNo { get; set; }
        public string Description { get; set; }
        public string Currency { get; set; }
        public decimal CurrencyAmount { get; set; }
        public decimal Amount { get; set; }
        public decimal RemainingAmount { get; set; }
        public RecordIdentifier StoreId { get; set; }
        public RecordIdentifier TerminalId { get; set; }
        public RecordIdentifier TransactionId { get; set; }
        public RecordIdentifier ReceiptId { get; set; }
        public StatusEnum Status { get; set; }
        public Guid UserId { get; set; }

		public static string AsString(decimal Value)
		{
			return Value.ToString("0.##");
		}

		public static string AsString(TypeEnum Value)
		{
			switch (Value)
			{
                case TypeEnum.Payment: return Properties.Resources.EntryType_Payment;
                case TypeEnum.Invoice: return Properties.Resources.EntryType_Invoice;
                case TypeEnum.CreditMemo: return Properties.Resources.EntryType_CreditMemo;
                case TypeEnum.Sale: return Properties.Resources.EntryType_Sale;
                case TypeEnum.Discount: return Properties.Resources.EntryType_Discount;
				default: return Enum.GetName(typeof(TypeEnum), Value);
			}
		}

		public static string AsString(StatusEnum Value)
		{
			switch (Value)
			{
                case StatusEnum.Closed: return Properties.Resources.EntryStatus_Closed;
                case StatusEnum.Open: return Properties.Resources.EntryStatus_Open;
				default: return Enum.GetName(typeof(StatusEnum), Value);
			}
		}

		/// <summary>
        /// Initializes a new instance of the <see cref="CustomerLedgerEntries" /> class.
		/// </summary>
        public CustomerLedgerEntries()
		{

            EntryNo = 0;
            DataAreaId = "";
            PostingDate = DateTime.MinValue;
            Customer = RecordIdentifier.Empty;
            EntryType = TypeEnum.Payment;
            DocumentNo = RecordIdentifier.Empty;
            Description = "";
            Currency = "";
            CurrencyAmount = 0;
            Amount = 0;
            RemainingAmount = 0;
            StoreId  = RecordIdentifier.Empty;
            TerminalId  = RecordIdentifier.Empty;
            TransactionId  = RecordIdentifier.Empty; 
            ReceiptId  = RecordIdentifier.Empty;
            Status  = StatusEnum.Closed;
            UserId = new Guid("00000000-0000-0000-0000-000000000000");

		}
	}
}


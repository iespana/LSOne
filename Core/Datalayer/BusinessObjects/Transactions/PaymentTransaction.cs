using System;
using System.ComponentModel.DataAnnotations;
using LSOne.DataLayer.BusinessObjects.Card;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.Utilities.DataTypes;
using LSOne.Utilities.Validation;
#if !MONO

#endif

namespace LSOne.DataLayer.BusinessObjects.Transactions
{
    public class PaymentTransaction : BaseTransaction
    {
        /// <summary>
        /// The current status of the transaction.
        /// </summary>
        public enum TransactionStatuses
        {
            /// <summary>
            /// Transaction is in process
            /// </summary>
            Normal = 0,
            /// <summary>
            /// Transaction has been voided
            /// </summary>
            Voided = 1,
            /// <summary>
            /// Transaction is suspended
            /// </summary>
            OnHold = 2,
            /// <summary>
            /// Transaction is concluded and is about to be saved to the database
            /// </summary>
            Concluded = 3,
            /// <summary>
            /// The transaction has been cancelled
            /// </summary>
            Cancelled = 4
        }
        public PaymentTransaction()
        {
            LineNumber = RecordIdentifier.Empty;
            ReceiptID = RecordIdentifier.Empty;
            TenderType = RecordIdentifier.Empty;
            StatementCode = RecordIdentifier.Empty;
            Currency = RecordIdentifier.Empty;
            Employee = RecordIdentifier.Empty;
            CardOrAccount = RecordIdentifier.Empty;
            Shift = RecordIdentifier.Empty;
            AuthenticationCode = RecordIdentifier.Empty;
            BatchID = RecordIdentifier.Empty;
            EFTAuthenticationCode = RecordIdentifier.Empty;
            GiftCardID = RecordIdentifier.Empty;
            CreditVoucherID = RecordIdentifier.Empty;
            LoyaltyCardID = RecordIdentifier.Empty;
            ExchangeRate = 0;
            AmountOfCurrency = 0;
            AmountTenderd = 0;
            EXCHRATEMST = 0;
            AMOUNTMST = 0;
            LoyalityPoints = 0;
            MarkupAmount = 0;
            TransactionDate = new DateTime();
            ShiftDate = new DateTime();
            this.TransactionStatus = TransactionStatuses.Normal;
            Description = "";
            Comment = "";
            ChangeLine = false;

            CardTypeID = RecordIdentifier.Empty;
            CardName = RecordIdentifier.Empty;
            CardIssuer = RecordIdentifier.Empty;
            ExpiryDate = RecordIdentifier.Empty;
            CardEntry = CardEntryTypesEnum.ManuallyEntered;
            CardHolderName = RecordIdentifier.Empty;
            MerchantID = RecordIdentifier.Empty;
            CardBIN = "";
            Comment = "";
            CardAuthenticationCode = "";
            CardMessage = "";
            CardNumberHidden = false;

            DriverID = RecordIdentifier.Empty;
            VechicleID = RecordIdentifier.Empty;
            ODOMeterReading = 0;
        }
        [RecordIdentifierValidation(28)]
        public RecordIdentifier LineNumber { get; set; }
        [RecordIdentifierValidation(61)]
        public RecordIdentifier ReceiptID { get; set; }
        [RecordIdentifierValidation(20)]
        public RecordIdentifier TenderType { get; set; }
        [RecordIdentifierValidation(20)]
        public RecordIdentifier StatementCode { get; set; }
        [RecordIdentifierValidation(3)]
        public RecordIdentifier Currency { get; set; }
        [RecordIdentifierValidation(20)]
        public RecordIdentifier Employee { get; set; }
        [RecordIdentifierValidation(30)]
        public RecordIdentifier CardOrAccount { get; set; }
        [RecordIdentifierValidation(20)]
        public RecordIdentifier Shift { get; set; }
        [RecordIdentifierValidation(20)]
        public RecordIdentifier AuthenticationCode { get; set; }
        [RecordIdentifierValidation(20)]
        public RecordIdentifier BatchID { get; set; }
        [RecordIdentifierValidation(20)]
        public RecordIdentifier EFTAuthenticationCode { get; set; }
        [RecordIdentifierValidation(30)]
        public RecordIdentifier GiftCardID { get; set; }
        [RecordIdentifierValidation(30)]
        public RecordIdentifier CreditVoucherID { get; set; }
        [RecordIdentifierValidation(30)]
        public RecordIdentifier LoyaltyCardID { get; set; }

        public decimal ExchangeRate { get; set; }   
        public decimal AmountOfCurrency { get; set; }
        public decimal AmountTenderd { get; set; }
        public decimal EXCHRATEMST { get; set; }
        public decimal AMOUNTMST { get; set; }
        public decimal LoyalityPoints { get; set; }
        public decimal MarkupAmount { get; set; }

        public DateTime TransactionDate { get; set; }
        public DateTime ShiftDate { get; set; }

        public TransactionStatuses TransactionStatus { get; set; }
        [StringLength(60)]
        public string Description { get; set; }
        [StringLength(255)]
        public string Comment { get; set; }
        public bool ChangeLine { get; set; }

        [RecordIdentifierValidation(20)]
        public RecordIdentifier CardTypeID { get; set; }
        [RecordIdentifierValidation(30)]
        public RecordIdentifier CardName { get; set; }
        [RecordIdentifierValidation(30)]
        public RecordIdentifier CardIssuer { get; set; }
        [RecordIdentifierValidation(10)]
        public RecordIdentifier ExpiryDate { get; set; }
        public CardEntryTypesEnum CardEntry { get; set; }
        [RecordIdentifierValidation(50)]
        public RecordIdentifier CardHolderName { get; set; }
        [RecordIdentifierValidation(50)]
        public RecordIdentifier MerchantID { get; set; }
        [StringLength(50)]
        public string CardBIN { get; set; }
        [StringLength(50)]
        public string CardAuthenticationCode { get; set; }
        [StringLength(255)]
        public string CardMessage { get; set; }
        [StringLength(50)]
        public string CardResponeCode { get; set; }
        public bool CardNumberHidden { get; set; }

        [RecordIdentifierValidation(50)]
        public RecordIdentifier DriverID { get; set; }
        [RecordIdentifierValidation(50)]
        public RecordIdentifier VechicleID { get; set; }
        public decimal ODOMeterReading { get; set; }

        public TypeOfTransaction TransactionType { get; set; }

    }
}

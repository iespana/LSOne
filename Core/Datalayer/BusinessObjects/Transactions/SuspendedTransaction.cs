#if !MONO
#endif
using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.Utilities.DataTypes;
using LSOne.Utilities.Validation;

namespace LSOne.DataLayer.BusinessObjects.Transactions
{
    public class SuspendedTransaction : DataEntity
    {


        /// <summary>
        /// A enum that defines sorting for suspended transactions
        /// </summary>
        [DataContract(Name = "SuspendedTransactionSortEnum")]
        public enum SortEnum
        {
            [EnumMember]
            TransactionDate,
            [EnumMember]
            StaffID,
            [EnumMember]
            StoreID,
            [EnumMember]
            TerminalID,
            [EnumMember]
            RecalledByID,
            [EnumMember]
            TransactionID,
            [EnumMember]
            Description,
            [EnumMember]
            AllowEOD,
            [EnumMember]
            Active,
            [EnumMember]
            SuspensionTypeID
        };
        

        public SuspendedTransaction()
            : base()
        {
            TransactionXML = "";
            StaffID = RecordIdentifier.Empty;
            StoreID = RecordIdentifier.Empty;
            TerminalID = RecordIdentifier.Empty;
            RecalledByID = RecordIdentifier.Empty;            
            SuspensionTypeID = RecordIdentifier.Empty;
            Text = "";
            StoreName = "";
            TerminalName = "";
            SuspensionTypeName = "";
            TransactionDate = DateTime.MinValue;
            Balance = 0;
            BalanceWithTax = 0;
            IsLocallySuspended = false;
        }

        [DataMember]
        public DateTime TransactionDate { get; set; }
        [DataMember]
        [RecordIdentifierValidation(20)]
        public RecordIdentifier StaffID { get; set; }
        [DataMember]
        [RecordIdentifierValidation(20)]
        public RecordIdentifier StoreID { get; set; }
        [DataMember]
        [RecordIdentifierValidation(20)]
        public RecordIdentifier TerminalID { get; set; }
        [DataMember]
        public string TransactionXML { get; set; }
        [DataMember]
        [RecordIdentifierValidation(20)]
        public RecordIdentifier RecalledByID { get; set; }
        [DataMember]
        public SuspendedTransactionsStatementPostingEnum AllowStatementPosting { get; set; }       
        [DataMember]
        [RecordIdentifierValidation(40)]
        public RecordIdentifier SuspensionTypeID { get; set; }

        [DataMember]
        [StringLength(60)]
        public override string Text
        {
            get
            {
                return base.Text;
            }
            set
            {
                base.Text = value;
            }
        }

        [DataMember]
        public decimal Balance { get; set; }

        [DataMember]
        public decimal BalanceWithTax { get; set; }
        
        public bool IsLocallySuspended { get; set; }
        public string StoreName { get; set; }
        public string TerminalName { get; set; }
        public string SuspensionTypeName { get; set; }

        /// <summary>
        /// Login of the staff. Only used for display.
        /// </summary>
        public string Login { get; set; }
    }
}

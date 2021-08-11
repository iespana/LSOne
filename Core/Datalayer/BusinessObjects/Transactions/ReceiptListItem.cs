using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Currencies;
using LSOne.Utilities.DataTypes;
using LSOne.Utilities.Validation;

namespace LSOne.DataLayer.BusinessObjects.Transactions
{
    [Serializable]
    [DataContract]
    [KnownType(typeof(RecordIdentifier))]
    [KnownType(typeof(Date))]
    public class ReceiptListItem : DataEntity
    {
        public ReceiptListItem() 
            : base()
        {
            Clear();
        }

        public void Clear()
        {
            ReceiptID = "";
            TransactionDate = Date.Empty;
            StoreID = RecordIdentifier.Empty;
            TerminalID = RecordIdentifier.Empty;
            EmployeeID = RecordIdentifier.Empty;
            EmployeeLogin = RecordIdentifier.Empty;
            Currency = RecordIdentifier.Empty;
            StoreDescription = "";
            TerminalDescription = "";
            EmployeeDescription = "";
            PaidAmount = decimal.Zero;
        }

        public bool Empty()
        {
            return ReceiptID == "" && TransactionDate == Date.Empty && StoreID == RecordIdentifier.Empty && TerminalID == RecordIdentifier.Empty && EmployeeID == RecordIdentifier.Empty;
        }

        [DataMember]
        public RecordIdentifier Currency { get; set; }
        
        [DataMember]
        public decimal PaidAmount { get; set; }

        [DataMember]
        [StringLength(20)]
        public string ReceiptID { get; set; }

        [DataMember]
        public Date TransactionDate { get; set; }

        [DataMember]
        [RecordIdentifierValidation(20)]
        public RecordIdentifier StoreID { get; set; }

        [DataMember]
        [RecordIdentifierValidation(20)]
        public RecordIdentifier TerminalID { get; set; }

        [DataMember]
        [RecordIdentifierValidation(20)]
        public RecordIdentifier EmployeeID { get; set; }

        [DataMember]
        [RecordIdentifierValidation(20)]
        public RecordIdentifier EmployeeLogin { get; set; }

        [DataMember]
        [StringLength(60)]
        public string StoreDescription { get; set; }

        [DataMember]
        [StringLength(60)]
        public string TerminalDescription { get; set; }

        [DataMember]
        [StringLength(32)]
        public string EmployeeDescription { get; set; }

        [DataMember]
        public RecordIdentifier TaxRefundID { get; set; }
    }
}

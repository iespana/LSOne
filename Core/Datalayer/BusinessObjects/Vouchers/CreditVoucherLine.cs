using System;
using System.Runtime.Serialization;
using LSOne.Utilities.DataTypes;
using LSOne.Utilities.Validation;

namespace LSOne.DataLayer.BusinessObjects.Vouchers
{
    public class CreditVoucherLine : DataEntity
    {
        public enum CreditVoucherLineEnum
        {
            Create = 0,
            AddCreditVoucher = 2,
            TakeFromCreditVoucher = 3,
        }

        public CreditVoucherLine()
            : base()
        {
            CreditVoucherID = RecordIdentifier.Empty;
            CreditVoucherLineID = RecordIdentifier.Empty;
            StoreID = "";
            TerminalID = "";
            ReceiptID = "";
            TransactionNumber = "";
            StaffID = "";
            UserID = Guid.Empty;
            TransactionDateTime = DateTime.Now;
            Amount = 0.0M;
            Operation = CreditVoucherLineEnum.Create;
            UserName = new Name();
        }

        [DataMember]
        [RecordIdentifierValidation(20, Depth = 2)]
        public override RecordIdentifier ID
        {
            get
            {
                return new RecordIdentifier(CreditVoucherID, CreditVoucherLineID);
            }
            set
            {
                if (!serializing)
                {
                    throw new NotImplementedException();
                }
            }
        }

        [DataMember]
        [RecordIdentifierValidation(20)]
        public RecordIdentifier CreditVoucherID { get; set; }

        [DataMember]
        [RecordIdentifierValidation(RecordIdentifier.RecordIdentifierType.Guid)]
        public RecordIdentifier CreditVoucherLineID { get; set; }

        [DataMember]
        [RecordIdentifierValidation(20)]
        public RecordIdentifier StoreID { get; set; }

        [DataMember]
        public string StoreName { get; set; }

        [DataMember]
        [RecordIdentifierValidation(20)]
        public RecordIdentifier TerminalID { get; set; }

        [DataMember]
        public string TerminalName { get; set; }

        [DataMember]
        [RecordIdentifierValidation(20)]
        public RecordIdentifier TransactionNumber { get; set; }

        [DataMember]
        [RecordIdentifierValidation(61)]
        public RecordIdentifier ReceiptID { get; set; }

        [DataMember]
        [RecordIdentifierValidation(20)]
        public RecordIdentifier StaffID { get; set; }

        [DataMember]
        public string StaffName { get; set; }

        [DataMember]
        [RecordIdentifierValidation(RecordIdentifier.RecordIdentifierType.Guid)]
        public RecordIdentifier UserID { get; set; }

        [DataMember]
        public Name UserName { get; set; }

        [DataMember]
        public DateTime TransactionDateTime { get; set; }

        [DataMember]
        public decimal Amount { get; set; }

        [DataMember]
        public CreditVoucherLineEnum Operation { get; set; }

        public string OperationText
        {
            get
            {
                switch (Operation)
                {
                    case CreditVoucherLineEnum.Create:
                        return Properties.Resources.Create;

                    case CreditVoucherLineEnum.AddCreditVoucher:
                        return Properties.Resources.AddCreditVoucher;

                    case CreditVoucherLineEnum.TakeFromCreditVoucher:
                        return Properties.Resources.TakeFromCreditVoucher;


                    default:
                        return "";
                }
            }
        }
    }
}

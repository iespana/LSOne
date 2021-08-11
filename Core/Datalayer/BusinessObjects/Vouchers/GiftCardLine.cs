using System;
using System.Runtime.Serialization;
using LSOne.Utilities.DataTypes;
using LSOne.Utilities.Validation;

namespace LSOne.DataLayer.BusinessObjects.Vouchers
{
    public class GiftCardLine : DataEntity
    {
        public enum GiftCardLineEnum
        {
            Create = 0,
            Activate = 1,
            AddToGiftCard = 2,
            TakeFromGiftCard = 3,
            Deactivate = 4,
        }

        public GiftCardLine()
            : base()
        {
            GiftCardID = RecordIdentifier.Empty;
            GiftCardLineID = RecordIdentifier.Empty;
            StoreID = "";
            TerminalID = "";
            ReceiptID = "";
            TransactionID = "";
            StaffID = "";
            UserID = Guid.Empty;
            TransactionDateTime = DateTime.Now;
            Amount = 0.0M;
            Operation = GiftCardLineEnum.AddToGiftCard;
            UserName = new Name();
            
        }

        // Does not make sense to serialize this attribute
        [DataMember]        
        [RecordIdentifierValidation(20, Depth=2)]
        public override RecordIdentifier ID
        {
            get
            {
                return new RecordIdentifier(GiftCardID, GiftCardLineID);
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
        public RecordIdentifier GiftCardID { get; set; }

        [DataMember]
        [RecordIdentifierValidation(RecordIdentifier.RecordIdentifierType.Guid)]
        public RecordIdentifier GiftCardLineID { get; set; }

        [DataMember]
        [RecordIdentifierValidation(20)]
        public RecordIdentifier StoreID { get; set; }

        [DataMember]
        public string StoreName { get; set; }

        [DataMember]
        [RecordIdentifierValidation(20)]
        public RecordIdentifier TerminalID { get; set; }

        [DataMember]
        public string TerminalName { get;  set; }

        [DataMember]
        [RecordIdentifierValidation(20)]
        public RecordIdentifier TransactionID { get; set; }

        [DataMember]
        [RecordIdentifierValidation(61)]
        public RecordIdentifier ReceiptID { get; set; }

        [DataMember]
        [RecordIdentifierValidation(20)]
        public RecordIdentifier StaffID { get; set; }

        [DataMember]
        public string StaffName { get;  set; }

        [DataMember]
        [RecordIdentifierValidation(RecordIdentifier.RecordIdentifierType.Guid)]
        public RecordIdentifier UserID { get; set; }

        [DataMember]
        public Name UserName { get; internal set; }

        [DataMember]
        public DateTime TransactionDateTime { get; set; }

        [DataMember]
        public decimal Amount { get; set; }

       

        [DataMember]
        public GiftCardLineEnum Operation { get; set; }

        public string OperationText
        {
            get
            {
                switch (Operation)
                {
                    case GiftCardLineEnum.Activate:
                        return Properties.Resources.Activate;

                    case GiftCardLineEnum.Deactivate:
                        return Properties.Resources.Deactivate;

                    case GiftCardLineEnum.Create:
                        return Properties.Resources.Create;

                    case GiftCardLineEnum.AddToGiftCard:
                        return Properties.Resources.AddToGiftCard;

                    case GiftCardLineEnum.TakeFromGiftCard:
                        return Properties.Resources.TakeFromGiftCard;

                    default:
                        return "";
                }
            }
        }
    }
}

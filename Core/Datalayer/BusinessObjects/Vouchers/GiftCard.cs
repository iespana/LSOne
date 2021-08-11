using System.Runtime.Serialization;
using LSOne.Utilities.DataTypes;
using LSOne.Utilities.Validation;

namespace LSOne.DataLayer.BusinessObjects.Vouchers
{
    [DataContract]
    [KnownType(typeof(RecordIdentifier))]
    public class GiftCard : DataEntity
    {

        /// <summary>
        /// A enum that defines sorting for the gift cards
        /// </summary>
        [DataContract(Name = "GiftCardSortEnum")]
        public enum SortEnum
        {
            /// <summary>
            /// No sort
            /// </summary>
            [EnumMember]
            None,
            /// <summary>
            /// Sort by gift card ID
            /// </summary>
            [EnumMember]
            ID,
            /// <summary>
            /// Sort by balance
            /// </summary>
            [EnumMember]
            Balance,
            /// <summary>
            /// Sort by currency
            /// </summary>
            [EnumMember]
            Currency,
            /// <summary>
            /// Sort by active status
            /// </summary>
            [EnumMember]
            Active,
            /// <summary>
            /// Sort by Refillable status
            /// </summary>
            [EnumMember]
            Refillable,
            /// <summary>
            /// Sort by creation date
            /// </summary>
            [EnumMember]
            CreatedDate,
            /// <summary>
            /// Sort by last used date
            /// </summary>
            [EnumMember]
            LastUsedDate
        };

        public GiftCard()
            : base()
        {
            Currency = "";
            Balance = 0.0M;
            Active = false;
            Issued = false;
            Refillable = false;
            CreatedDate = Date.Empty;
            LastUsedDate = Date.Empty;
            BarCode = "";

        }

        [DataMember]
        [RecordIdentifierValidation(20)]
        public override RecordIdentifier ID
        {
            get
            {
                return base.ID;
            }
            set
            {
                base.ID = value;
            }
        }

        [DataMember]
        public decimal Balance { get; set; }

        [DataMember]
        public bool Active { get; set; }

        [DataMember]
        public bool Issued { get; set; }

        [DataMember]
        public bool Refillable { get; set; }

        [DataMember]
        [RecordIdentifierValidation(3)]
        public RecordIdentifier Currency { get; set; }

        [DataMember]
        public Date CreatedDate { get; set; }

        [DataMember]
        public Date LastUsedDate { get; set; }

        [DataMember]
        public string BarCode { get; set; }

    }
}

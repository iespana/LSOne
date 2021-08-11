using System.Runtime.Serialization;
using LSOne.Utilities.DataTypes;
using LSOne.Utilities.Validation;

namespace LSOne.DataLayer.BusinessObjects.Vouchers
{
    [DataContract]
    [KnownType(typeof(RecordIdentifier))]
    public class CreditVoucher : DataEntity
    {
        /// <summary>
        /// A enum that defines sorting for the gift cards
        /// </summary>
        [DataContract(Name = "CreditVoucherSortEnum")]
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

        public CreditVoucher()
            : base()
        {
            Currency = "";
            Balance = 0.0M;
            CreatedDate = new Date();
            LastUsedDate = new Date();
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
        [RecordIdentifierValidation(3)]
        public RecordIdentifier Currency { get; set; }

        [DataMember]
        public Date CreatedDate { get; set; }

        [DataMember]
        public Date LastUsedDate { get; set; }
    }
}

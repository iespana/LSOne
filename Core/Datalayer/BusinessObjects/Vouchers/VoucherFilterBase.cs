using LSOne.Utilities.DataTypes;
using System;

namespace LSOne.DataLayer.BusinessObjects.Vouchers
{
    /// <summary>
    /// Base class for filtering credit vouchers and gift cards
    /// </summary>
    public class VoucherFilterBase
    {
        /// <summary>
        /// Number of the first row to fetch
        /// </summary>
        public int RowFrom { get; set; }

        /// <summary>
        /// Number of the last row to fetch
        /// </summary>
        public int RowTo { get; set; }

        /// <summary>
        /// The ID of the voucher to search
        /// </summary>
        public string VoucherID { get; set; }

        /// <summary>
        /// True if the ID begins with the search text, false if it contains
        /// </summary>
        public bool VoucherIDBeginsWith { get; set; }

        /// <summary>
        /// Currency ID to search
        /// </summary>
        public RecordIdentifier CurrencyID { get; set; }

        /// <summary>
        /// True if results should be sorted asceding, false if descending
        /// </summary>
        public bool SortAscending { get; set; }

        /// <summary>
        /// Filter result from created date
        /// </summary>
        public Date FromCreatedDate { get; set; }

        /// <summary>
        /// Filter result to created date
        /// </summary>
        public Date ToCreatedDate { get; set; }

        /// <summary>
        /// Filter result from last used date
        /// </summary>
        public Date FromLastUsedDate { get; set; }

        /// <summary>
        /// Filter results to last used date
        /// </summary>
        public Date ToLastUsedDate { get; set; }

        public VoucherFilterBase()
        {
            RowFrom = 0;
            RowTo = 0;
            VoucherID = null;
            VoucherIDBeginsWith = true;
            CurrencyID = null;
            SortAscending = true;
            FromCreatedDate = Date.Empty;
            ToCreatedDate = Date.Empty;
            FromLastUsedDate = Date.Empty;
            ToLastUsedDate = Date.Empty;
        }
    }

    /// <summary>
    /// Credit voucher search filter
    /// </summary>
    public class CreditVoucherFilter : VoucherFilterBase
    {
        /// <summary>
        /// Credit voucher sort enum
        /// </summary>
        public CreditVoucher.SortEnum Sort { get; set; }

        /// <summary>
        /// Credit voucher status
        /// </summary>
        public CreditVoucherStatusEnum? Status { get; set; }

        public CreditVoucherFilter() : base()
        {
            Sort = CreditVoucher.SortEnum.ID;
            Status = null;
        }
    }

    /// <summary>
    /// Gift card search filter
    /// </summary>
    public class GiftCardFilter : VoucherFilterBase
    {
        /// <summary>
        /// Gift card sort enum
        /// </summary>
        public GiftCard.SortEnum Sort { get; set; }
        
        /// <summary>
        /// Gift card status
        /// </summary>
        public GiftCardStatusEnum? Status { get; set; }

        /// <summary>
        /// True if the gift card is refillable
        /// </summary>
        public bool? Refillable { get; set; }

        /// <summary>
        /// The lower limit of the gift card balance
        /// </summary>
        public double? FromBalance { get; set; }

        /// <summary>
        /// The upper limit of the gift card balance
        /// </summary>
        public double? ToBalance { get; set; }

        public GiftCardFilter() : base()
        {
            Sort = GiftCard.SortEnum.ID;
            Status = null;
            Refillable = null;
            FromBalance = null;
            ToBalance = null;
        }
    }
}

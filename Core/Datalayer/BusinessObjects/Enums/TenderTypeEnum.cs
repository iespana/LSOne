using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LSOne.DataLayer.BusinessObjects.Enums
{
    public enum TenderTypeEnum
    {
        /// <summary>
        /// None is only used for payment lines that were created before this enum was introduced
        /// </summary>
        None = 0,
        CardTender = 1,
        ChequeTender = 2,
        CorporateCardTender = 3,
        CouponTender = 4,
        CreditMemoTender = 5,
        CustomerTender = 6,
        DepositTender = 7,
        GiftCertificateTender = 8,
        LoyaltyTender = 9,
        TenderLine = 10
    }
}

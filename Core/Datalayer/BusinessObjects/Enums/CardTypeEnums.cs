using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LSOne.DataLayer.BusinessObjects.Enums
{
    /// <summary>
    /// Different types of cards are supported by the system.
    /// </summary>
    public enum CardTypesEnum
    {
        /// <summary> 
        /// International cards - Brokered by an EFT Service Provider
        /// </summary>
        InternationalCreditcard = 0,

        /// <summary>
        /// International cards - Brokered by an EFT Service Provider
        /// </summary>
        InternationalDebitcard = 1,

        /// <summary>
        /// Loyalty cards
        /// </summary>
        LoyaltyCard = 2,

        /// <summary>
        /// Local corporate credit cards
        /// </summary>
        CorporateCard = 3,

        /// <summary>
        /// Customer card - used to identify customers.
        /// </summary>
        CustomerCard = 4,

        /// <summary>
        /// Employee card - used to log employees on the pos.
        /// </summary>
        EmployeeCard = 5,

        /// <summary>
        /// Salesperson card - used to identify salespersons.
        /// </summary>
        SalespersonCard = 6,

        /// <summary>
        /// Unspecified card type.
        /// </summary>
        Unknown = 500
    }

    /// <summary>
    /// To distinguish whether the card has been read with a stripe card reader or has been manually entered.
    /// </summary>
    public enum CardEntryTypesEnum
    {
        /// <summary>
        /// Using the magnetic stripe reader.
        /// </summary>
        MagneticStripeRead = 1,

        /// <summary>
        /// Entering the number manually. 
        /// </summary>
        ManuallyEntered,

        /// <summary>
        /// Device with a chip reader
        /// </summary>
        ChipReader
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LSOne.Services.Interfaces.Enums
{
    /// <summary>
    /// Type of the price
    /// </summary>
    public enum TradeAgreementPriceType
    {
        /// <summary>
        /// Base price of the item
        /// </summary>
        BasePrice = 0,
        /// <summary>
        /// Item sales price
        /// </summary>
        SalesPrice = 1,
        /// <summary>
        /// Promotion price
        /// </summary>
        Promotion = 2
    }
}

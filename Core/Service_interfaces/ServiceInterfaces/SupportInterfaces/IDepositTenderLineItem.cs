using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LSOne.DataLayer.BusinessObjects.Transactions.Line;

namespace LSOne.Services.Interfaces.SupportInterfaces
{
    public interface IDepositTenderLineItem : ITenderLineItem
    {
        /// <summary>
        /// If true then this is a tender line with information about the sum of previously paid deposits for the items that are being sold
        /// </summary>
        bool RedeemedDeposit { get; set; }

        /// <summary>
        /// How much of the original payment has been redeemed
        /// </summary>
        decimal RedeemedAmount { get; set; }
    }
}

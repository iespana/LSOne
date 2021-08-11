using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LSOne.Services.Interfaces.SupportClasses.Calculation
{
    /// <summary>
    /// Contains change back amounts. Used by <see cref="ICalculationService.CalculateChangeBackAmounts(DataLayer.GenericConnector.Interfaces.IConnectionManager, SupportInterfaces.IPosTransaction, SupportInterfaces.ITenderLineItem)"/>
    /// </summary>
    public class ChangeBackAmountsInfo
    {
        /// <summary>
        /// The calculated unrounded balance amount
        /// </summary>
        public decimal CalculatedAmount { get; set; }

        /// <summary>
        /// The rounded calculated balance amount
        /// </summary>
        public decimal CalculatedRoundedAmount { get; set; }

        /// <summary>
        /// The rounded payment amound
        /// </summary>
        public decimal CalculatedRoundedAmountForPayment { get; set; }

        /// <summary>
        /// The total amount already paid on a transaction
        /// </summary>
        public decimal TenderedAmount { get; set; }

        /// <summary>
        /// The rounded changeback amount
        /// </summary>
        public decimal CalculatedChangeBack { get; set; }

        /// <summary>
        /// The rounding difference for the changeback
        /// </summary>
        public decimal CalculatedRoundingDifference { get; set; }
    }
}

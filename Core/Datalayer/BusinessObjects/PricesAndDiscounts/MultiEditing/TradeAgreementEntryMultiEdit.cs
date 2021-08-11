using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LSOne.DataLayer.BusinessObjects.PricesAndDiscounts.MultiEditing
{
    public class TradeAgreementEntryMultiEdit : TradeAgreementEntry
    {
        /// <summary>
        /// True if we are calculating from price, false if we are calculating from price with tax
        /// </summary>
        public bool CalculateFromPrice;
    }
}

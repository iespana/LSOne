using System.Collections.Generic;

namespace LSOne.DataLayer.BusinessObjects.Currencies
{
    /// <summary>
    /// Class that encapulates all properties of a given currency, how it is composed (with regard to banknotes and coins)
    /// and also what the exchange rate is.
    /// </summary>
    public class CurrencyInfo
    {
        #region Member variables
        private decimal posCurrencyRate;                               // The rate as seen on the POS.

        /// <summary>
        /// The currency details are stored in a linked list.
        /// </summary>
        public List<CashDenominator> CurrencyItems;
        #endregion

        #region Properties

        /// <summary>
        /// The exchange rate as seen by the POS.
        /// </summary>
        public decimal PosCurrencyRate
        {
            get { return posCurrencyRate; }
            set { posCurrencyRate = value; }
        }
        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        public CurrencyInfo()
        {
            this.CurrencyItems = new List<CashDenominator>();
        }
    }
}

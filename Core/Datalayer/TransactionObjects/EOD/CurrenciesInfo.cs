namespace LSOne.DataLayer.TransactionObjects.EOD
{
    /// <summary>
    /// Declares the class TenderInfo. A linked list of that type is created in the constructor.
    /// </summary>
    public class CurrenciesInfo
    {
        /// <summary>
        /// The summed amount of a certain tender. Used in the class ReportData.cs. Filled through the call to a stored procedure LSR_ZREPORT_TENDERS.
        /// </summary>
        public decimal amountCur = decimal.Zero;

        /// <summary>
        /// The Id of a certain tender. 
        /// </summary>
        public string tenderID = "";

        /// <summary>
        /// The name that belongs to the Id of a certain tender.
        /// </summary>
        public string tenderName = "";

        /// <summary>
        /// The 3-letter code of a foreign currency (for example EUR)
        /// </summary>
        public string currencyCode = "";

        /// <summary>
        /// The full spelled name of a foreign currency (for example 'Canadian Dollar')
        /// </summary>
        public string currencyName = "";

        /// <summary>
        /// the default function of a tender, as provided from the RBOTENDERTYPE table. For the reports, we omit those having 
        /// default function = 4
        /// </summary>
        public int defaultTenderFunction;
    }
}

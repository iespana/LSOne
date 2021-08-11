using System.Collections.Generic;
using LSOne.DataLayer.BusinessObjects.Enums;

namespace LSOne.DataLayer.TransactionObjects.EOD
{
    /// <summary>
    /// Declares the class TenderInfo. A linked list of that type is created in the constructor.
    /// </summary>
    public class TenderInfo
    {
        /// <summary>
        /// The summed amount of a certain tender. Used in the class ReportData.cs. Filled through the call to a stored procedure LSR_ZREPORT_TENDERS.
        /// </summary>
        public decimal amountTendered = decimal.Zero;

        /// <summary>
        /// The Id of a certain tender. 
        /// </summary>
        public string tenderID = "";

        /// <summary>
        /// The name that belongs to the Id of a certain tender.
        /// </summary>
        public string tenderName = "";

        public POSOperations operationID = POSOperations.PayCash;

        public List<TenderInfo> SubLines { get; set; }
 
    }
}

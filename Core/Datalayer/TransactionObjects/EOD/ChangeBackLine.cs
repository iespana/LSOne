using System.Collections.Generic;

namespace LSOne.DataLayer.TransactionObjects.EOD
{
    /// <summary>
    /// Represents a single tender and a change back amount for that tender
    /// </summary>
    public class ChangeBackLine
    {
        public string TenderId { get; set; }
        public string TenderName { get; set; }
        public decimal Amount { get; set; }
    }
}

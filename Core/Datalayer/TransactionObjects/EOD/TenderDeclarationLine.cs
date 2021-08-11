using System.Collections.Generic;

namespace LSOne.DataLayer.TransactionObjects.EOD
{
    /// <summary>
    /// Represents a single tender declaration line and a total counted amount for that tender
    /// </summary>
    public class TenderDeclarationLine
    {
        public string TenderId { get; set; }
        public string TenderName { get; set; }
        public decimal CountedAmount { get; set; }
    }
}

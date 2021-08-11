using System;

namespace LSOne.DataLayer.BusinessObjects
{
    /// <summary>
    /// A data entity class for statement transactions.
    /// </summary>
    public class StatementTransaction : DataEntity
    {
        public string TransactionNumber { get; set; }
        public int TransactionType { get; set; }
        public string TenderTypeID { get; set; }
        public string CurrencyCode { get; set; }
        public decimal Amount { get; set; }
        public DateTime TransactionTime { get; set; }
        public string StaffID { get; set; }
        public string TerminalID { get; set; }
        public bool IsSCTenderDeclaration { get; set; }
    }
}
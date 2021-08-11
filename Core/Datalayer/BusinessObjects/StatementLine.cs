using System;
using LSOne.Services.Interfaces.Enums;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.BusinessObjects
{
    /// <summary>
    /// A data entity class for statement lines.
    /// </summary>
    public class StatementLine : DataEntity
    {
        public StatementLine()
        {
            TerminalID = "";
            StaffID = "";
            //TenderTypeName = "";
        }

        public override RecordIdentifier ID
        {
            get
            {
                return new RecordIdentifier(StatementID, LineNumber);
            }
            set
            {
                if (!serializing)
                {
                    throw new NotImplementedException();
                }
            }
        }

        public RecordIdentifier StatementID { get; set; }
        public RecordIdentifier LineNumber { get; set; }

        public string TerminalID { get; set; }
        public string StaffID { get; set; }
        /// <summary>
        /// This is actually Tender Type ID
        /// </summary>
        public string TenderID { get; set; }
        public string TenderName { get; set; }
        public string CurrencyCode { get; set; }
        public decimal TransactionAmount { get; set; }
        public decimal BankedAmount { get; set; }
        public decimal SafeAmount { get; set; }
        public decimal CountedAmount { get; set; }
        public decimal Difference { get; set; }
        public AllowEODEnums StatementStatus { get; set; }

        public string TenderDescription
        {
            get
            {
                return TenderID + " - " + TenderName + "(" + CurrencyCode + ")";
            }
        }
    }
}
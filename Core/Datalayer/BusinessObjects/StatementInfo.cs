using System;
using System.Collections.Generic;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.BusinessObjects
{
    /// <summary>
    /// A data entity class for statements.
    /// </summary>
    public class StatementInfo : DataEntity
    {
        public List<StatementLine> statementLines;

        public StatementInfo()
            : base()
        {
            statementLines = new List<StatementLine>();
            PostingDate = Date.Empty;
            CalculatedTime = new DateTime();
            Calculated = false;
            Posted = false;
            StoreID = "";
        }

        public DateTime StartingTime { get; set; }
        public DateTime EndingTime { get; set; }
        public RecordIdentifier StoreID { get; set; }
        public DateTime CalculatedTime { get; set; }
        public Date PostingDate { get; set; }
        public bool Posted { get; set; }
        public bool Calculated { get; set; }
        public bool ERPProcessed { get; set; }
        public StatementPeriodFormEnum ToType { get; set; }
        public StatementPeriodFormEnum FromType { get; set; }
        public StatementPeriodTypeEnum ToPeriodType { get; set; }
        public StatementPeriodTypeEnum FromPeriodType { get; set; }
    }
}

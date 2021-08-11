
namespace LSOne.DataLayer.TransactionObjects.Enums
{
    /// <summary>
    /// This enum specifies the report type.    
    /// </summary>
    public enum ReportType
    {
        /// <summary>
        /// 
        /// Can be called repeatedly; will evaluate all transactions that follow the last Z-report marking, which is the table column ZREPORTID in 
        /// the table RBOTRANSACTIONTABLE.
        /// </summary>
        XReport = 1,

        /// <summary>
        /// Evaluates all transactions that follow the last Z-report marking (RBOTRANSACTIONTABLE.ZREPORTID)and 
        /// marks afterwards the fetched transactions such that the X- and Z- report will not consider these transactions again in subsequent calls.
        /// </summary>
        ZReport = 2,
    }

    /// <summary>
    /// This enum specifies the pad direction, or alignment of text in a report.
    /// 0 = Left,
    /// 1 = Right.
    /// </summary>
    public enum PadDirection
    {
        /// <summary>
        /// Used to set the pad direction (alignment) LEFT.
        /// </summary>
        Left = 0,
        
        /// <summary>
        /// Used to set the pad direction (alignment) RIGHT.
        /// </summary>
        Right = 1
    }
    
}


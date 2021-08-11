using LSOne.DataLayer.BusinessObjects;

namespace LSOne.DataLayer.DataProviders.Statements
{
    /// <summary>
    /// A enum that defines sorting for the Statements
    /// </summary>
    public enum StatementInfoSorting
    {
        /// <summary>
        /// Sort by StatementID
        /// </summary>
        ID,
        /// <summary>
        /// Sort by PERIODSTARTINGTIME
        /// </summary>
        StartingTime,
        /// <summary>
        /// Sort by PERIODENDINGTIME
        /// </summary>
        EndingTime,
        /// <summary>
        /// Sort by CALCULATEDTIME
        /// </summary>
        CalculatedTime,
        /// <summary>
        /// Sort by POSTINGDATE
        /// </summary>
        PostingDate
    };
}
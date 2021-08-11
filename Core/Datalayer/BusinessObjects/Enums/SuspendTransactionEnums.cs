using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LSOne.DataLayer.BusinessObjects.Enums
{
    /// <summary>
    /// When retrieving information about suspended information we need to decide if all, locally or centrally saved suspensions should be retrieved
    /// </summary>
    public enum RetrieveSuspendedTransactions
    {
        /// <summary>
        /// All suspended transactions found in the table 
        /// </summary>
        All,
        /// <summary>
        /// Only those that are marked as saved locally
        /// </summary>
        OnlyLocallySaved,
        /// <summary>
        /// Only those that are NOT marked as saved locally
        /// </summary>
        OnlyCentrallySaved
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LSOne.Services.Interfaces.Enums
{
    /// <summary>
    /// Status of a prescription
    /// </summary>
    public enum PrescriptionStatus
    {
        /// <summary>
        /// Unfetched
        /// </summary>
        UnFetched = 0,
        /// <summary>
        /// Fetched
        /// </summary>
        Fetched = 1,
        /// <summary>
        /// Paid
        /// </summary>
        Paid = 2
    }
}

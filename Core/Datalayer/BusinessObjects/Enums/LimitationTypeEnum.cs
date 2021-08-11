using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LSOne.DataLayer.BusinessObjects.Enums
{
    /// <summary>
    /// The type of limitations that can be configured
    /// </summary>
    public enum LimitationType
    {
        /// <summary>
        /// For a specific item
        /// </summary>
        Item = 0,
        /// <summary>
        /// For all items within a retail group
        /// </summary>
        RetailGroup = 1,
        /// <summary>
        /// For all items within a special group
        /// </summary>
        SpecialGroup = 2,
        /// <summary>
        /// For all items within a retail department
        /// </summary>
        RetailDepartment = 3,
        /// <summary>
        /// All items are either included or excluded
        /// </summary>
        Everything = 99
    }

    /// <summary>
    /// The result of the tender restrictions checks and calculations
    /// </summary>
    public enum TenderRestrictionResult
    {
        /// <summary>
        /// None of the items can be paid for using this payment method
        /// </summary>
        NothingCanBePaidFor = 0,
        /// <summary>
        /// The user cancelled the payment after seeing which items are excluded
        /// </summary>
        CancelledByUser = 1,
        /// <summary>
        /// There are no tender restrictions applicable
        /// </summary>
        NoTenderRestrictions = 98,
        /// <summary>
        /// Continue with the tender restriction calculations/display of items
        /// </summary>
        Continue = 99
    }
}

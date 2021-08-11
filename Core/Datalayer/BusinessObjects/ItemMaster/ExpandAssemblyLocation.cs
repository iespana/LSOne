using System;

namespace LSOne.DataLayer.BusinessObjects.ItemMaster
{
    /// <summary>
    /// Data type used for the ExpandAssembly attribute in the RetailItemAssembly class that holds 
    /// the setting for displaying assembly components in all 3 locations (POS/receipt/KDS).
    /// 
    /// Get/set the value for individual display locations by passing a value of this type to methods SetDisplayWithComponents() and ShallDisplayWithComponents().
    /// </summary>
    [Flags]
    public enum ExpandAssemblyLocation
    {
        /// <summary>
        /// Means components shall not be displayed in any of the 3 locations 
        /// </summary>
        None = 0,

        /// <summary>
        /// Display assembly components on the POS
        /// </summary>
        OnPOS = 1,

        /// <summary>
        ///  Display assembly components on the receipt
        /// </summary>
        OnReceipt = 2
    }
}

using System;

namespace LSOne.DataLayer.BusinessObjects.ItemMaster
{
    /// <summary>
    /// Data type used for the SendAssemblyComponentsToKds attribute in the RetailItemAssembly class that holds 
    /// the setting for if and how to send assembly components to KDS
    /// 
    /// Get/set the value for individual display locations by passing a value of this type to methods SetDisplayWithComponents() and ShallDisplayWithComponents().
    /// </summary>
    public enum KitchenDisplayAssemblyComponentType
    {
        /// <summary>
        /// Means components shall not be sent to KDS
        /// </summary>
        DontSend = 0,

        /// <summary>
        /// Send assembly components to KDS as item modifiers
        /// </summary>
        SendAsItemModifiers = 1,

        /// <summary>
        ///  Send assembly components to KDS as separate items
        /// </summary>
        SendAsSeparateItems = 2
    }
}

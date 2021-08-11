using LSOne.Utilities.Development;
using System.Collections.Generic;

namespace LSOne.DataLayer.BusinessObjects.Replenishment
{
    /// <summary>
    /// The quantity method for inventory templates    
    /// </summary>
    [LSOneUsage(CodeUsage.LSCommerce, "Specifies how the quantity is set when a new line is added to a document in the mobile inventory app.")]
    public enum QuantityMethodEnum
    {
        Ask = 0,
        QuickScan = 1,
        SuggestedQuickScan = 2
    }

    public class QuantityMethodEnumHelper
    {
        public static List<object> GetList()
        {
            return new List<object>
            {
                Properties.Resources.Ask,
                Properties.Resources.QuickScan,
                Properties.Resources.SuggestedQuickScan
            };
        }
    }
}

using System.Collections.Generic;

namespace LSOne.DataLayer.BusinessObjects.Replenishment
{
    /// <summary>
    /// The unit type for inventory templates
    /// </summary>
    public enum UnitSelectionEnum
    {
        InventoryUnit = 0,
        PurchaseUnit = 1,
        SalesUnit = 2
    }

    public class UnitSelectionEnumHelper
    {
        public static List<object> GetList()
        {
            return new List<object>
            {
                Properties.Resources.InventoryUnit,
                Properties.Resources.PurchaseUnit,
                Properties.Resources.SalesUnit
            };
        }
    }
}

using System.Collections.Generic;

namespace LSOne.DataLayer.BusinessObjects.Replenishment
{
    /// <summary>
    /// The type of entering for inventory templates
    /// Should be used only for stock counting
    /// 
    /// AddToQty - Aggregates the lines
    /// ChangeQty - Change the quantity of an existing line. If the line does not exist, a new line is added
    /// AddLine - Creates a new line
    /// </summary>
    public enum EnteringTypeEnum
    {
        AddToQty = 0,
        ChangeQty = 1,
        AddLine = 2
    }

    public class EnteringTypeEnumHelper
    {
        public static List<object> GetList()
        {
            return new List<object>
            {
                Properties.Resources.AddToQuantity,
                Properties.Resources.ChangeQuantity,
                Properties.Resources.AddLine
            };
        }
    }
}

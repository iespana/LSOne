using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.BusinessObjects.Inventory
{
    /// <summary>
    /// A data entity that can hold an ID and a quantity. Used for ordered/received information in Goods receiving
    /// </summary>
    public class InventoryTotals
    {
        public RecordIdentifier ID;
        public decimal Quantity;

        public InventoryTotals()
        {
            ID = RecordIdentifier.Empty;
            Quantity = decimal.Zero;
        }

        public InventoryTotals(RecordIdentifier id, decimal quantity)
        {
            ID = id;
            Quantity = quantity;
        }
    }
}

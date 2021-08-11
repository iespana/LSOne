using LSOne.Utilities.DataTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LSOne.DataLayer.BusinessObjects.Inventory
{
    public class AssemblyInventoryStatus : InventoryStatus
    {
        public RecordIdentifier ComponentItemID { get; set; }
        public decimal ComponentQuantity { get; set; }
    }
}

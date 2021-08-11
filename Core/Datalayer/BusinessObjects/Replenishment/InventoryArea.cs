using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LSOne.DataLayer.BusinessObjects.Replenishment
{
    public class InventoryArea : DataEntity
    {
        public int Type { get; set; }
        public List<InventoryAreaLine> InventoryAreaLines { get; set; }

        public InventoryArea()
        {
            ID = Guid.Empty;
            InventoryAreaLines = new List<InventoryAreaLine>();
        }
    }
}

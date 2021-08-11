using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LSOne.DataLayer.BusinessObjects.Replenishment
{
    public class InventoryAreaLine : DataEntity
    {
        public Guid AreaID { get; set; }

        public InventoryAreaLine()
        {
            ID = Guid.Empty;
            AreaID = Guid.Empty;
        }
    }
}

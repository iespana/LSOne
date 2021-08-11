using LSOne.DataLayer.BusinessObjects.ItemMaster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LSOne.ViewPlugins.RMSMigration.Model
{
    public class RMSRetailGroup : RetailGroup
    {
        public int RMS_ID { get; set; }
        public int RMS_Department_ID { get; set; }
    }
}

using LSOne.DataLayer.BusinessObjects.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LSOne.ViewPlugins.RMSMigration.Model
{
    public class RMSVendor : Vendor
    {
        public int RMS_ID { get; set; }
        public int RMS_CurrencyID { get; set; }
        public string ZIP { get; set; }
        public string DefaultContact { get; set; }
    }
}

using LSOne.DataLayer.BusinessObjects.Tax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LSOne.ViewPlugins.RMSMigration.Model
{
    public class RMSTax : TaxCode
    {
        public int RMS_ID { get; set; }
        public decimal Percentage { get; set; }
    }
}

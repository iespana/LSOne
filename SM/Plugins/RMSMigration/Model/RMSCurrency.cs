using LSOne.DataLayer.BusinessObjects.Currencies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LSOne.ViewPlugins.RMSMigration.Model
{
    public class RMSCurrency : Currency
    {
        public int RMS_ID { get; set; }
        public decimal ExchangeRate { get; set; }
    }
}

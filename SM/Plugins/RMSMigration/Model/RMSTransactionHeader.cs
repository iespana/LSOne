using LSOne.DataLayer.TransactionObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LSOne.ViewPlugins.RMSMigration.Model
{
    public class RMSTransactionHeader
    {
        public RMSTransactionHeader() : base()
        {

        }
        public string RMS_ID { get; set; }
        public int RMS_StoreID { get; set; }
        public int RMS_TerminalID { get; set; }
        public int RMS_CustomerID { get; set; }

        public DateTime RMSPostingDate { get; set; }
        public int NoOfLines { get; set; }
        public decimal TransactionAmount { get; set; }
        public decimal VATAmount { get; set; }

    }
}

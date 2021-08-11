using LSOne.DataLayer.BusinessObjects.Customers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LSOne.ViewPlugins.RMSMigration.Model
{
    public class RMSCustomer : Customer
    {
        public int RMS_ID { get; set; }
        
        public string Title
        {
            get
            {
                return Name.Prefix;
            }
            set
            {
                Name.Prefix = value;
            }
        }

        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZIP { get; set; }
        public string Country { get; set; }
        public string Contact { get; set; }
    }
}

using LSOne.DataLayer.BusinessObjects.UserManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LSOne.ViewPlugins.RMSMigration.Model
{
    public class RMSUser : User
    {
        public int RMS_ID { get; set; }
        public string FirstName
        {
            get
            {
                return Name.First;
            }
            set
            {
                Name.First = value;
            }
        }
        public string LastName
        {
            get
            {
                return Name.Last;
            }
            set
            {
                Name.Last = value;
            }
        }

        public int RMS_StoreID { get; set; }
    }
}

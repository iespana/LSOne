using LSOne.DataLayer.BusinessObjects.StoreManagement.ListItems;
using LSOne.Utilities.DataTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LSOne.ViewPlugins.RMSMigration.Model
{
    public class RMSStore
    {
        public int StoreID { get; set; }
        public string Name { get; set; }
        public string StoreCode { get; set; }
        public string DisplayName
        {
            get
            {
                return string.Format("{0} - {1}", this.StoreID, this.Name);
            }
        }

        private RecordIdentifier _LSOneStore = RecordIdentifier.Empty;
        public RecordIdentifier LSOneStore
        {
            get
            {
                return _LSOneStore;
            }
            set
            {
                _LSOneStore = value;
            }
        }
    }
}

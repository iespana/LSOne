using LSOne.Utilities.DataTypes;
using LSOne.ViewPlugins.RMSMigration.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LSOne.ViewPlugins.RMSMigration.Model
{
    public class RMSTerminal
    {
        public int TerminalID { get; set; }
        public int StoreID { get; set; }
        public string Name { get; set; }
        public string StoreCode { get; set; }
        public string DisplayName
        {
            get
            {
                if (this.StoreID == 0 && string.IsNullOrEmpty(Name))
                {
                    this.Name = Resources.N_AStore;
                }
                return string.Format("{0} - {1}", this.StoreID, this.Name);
            }
        }
        public string TerminalDescription { get; set; }

        private RecordIdentifier _LSOneTerminal = RecordIdentifier.Empty;
        public RecordIdentifier LSOneTerminal
        {
            get
            {
                return _LSOneTerminal;
            }
            set
            {
                _LSOneTerminal = value;
            }
        }
    }
}

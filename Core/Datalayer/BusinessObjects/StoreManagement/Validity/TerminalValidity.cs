using LSOne.Utilities.DataTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LSOne.DataLayer.BusinessObjects.StoreManagement.Validity
{
    public class TerminalValidity : DataEntity
    {
        public bool VisualProfileExists { get; set; }
        public bool HardwareProfileExists { get; set; }
        public RecordIdentifier StoreID { get; set; }
    }
}

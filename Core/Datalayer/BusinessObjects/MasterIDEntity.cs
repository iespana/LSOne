using LSOne.Utilities.DataTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LSOne.DataLayer.BusinessObjects
{
    public class MasterIDEntity : DataEntity
    {
        public RecordIdentifier ReadadbleID { get; set; }
        public string ExtendedText { get; set; }
    }
}

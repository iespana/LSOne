using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.BusinessObjects.Inventory
{

    [DataContract]
    public class ItemLedgerSearchParameters
    {
        [DataMember]
        public RecordIdentifier ItemID { get; set; }
        [DataMember]
        public DateTime? FromDateTime { get; set; }
        [DataMember]
        public DateTime? ToDateTime { get; set; }
        [DataMember]
        public ItemLedgerSearchOptions Source { get; set; }
        [DataMember]
        public bool IncludeVoided { get; set; }
        [DataMember]
        public RecordIdentifier StoreID { get; set; }
        [DataMember]
        public RecordIdentifier TerminalID { get; set; }
        [DataMember]
        public int rowFrom { get; set; }
        [DataMember]
        public int rowTo { get; set; }
        [DataMember]
        public bool SortAscending { get; set; }

        [DataMember]
        public bool DoNotAggregatePostedSales { get; set; }

        [DataMember]
        public bool ShowObsoleteEntries { get; set; }

    }
}

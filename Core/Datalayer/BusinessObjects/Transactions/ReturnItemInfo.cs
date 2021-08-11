using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace LSOne.DataLayer.BusinessObjects.Transactions
{
    /// <summary>
    /// Used to store information about returned sale lines. Used only when sending this information through the Site Service
    /// </summary>
    [DataContract]
    public class ReturnItemInfo : DataEntity
    {
        [DataMember]
        public string ReturnTransId { get; set; }

        [DataMember]
        public string ReturnStoreId { get; set; }

        [DataMember]
        public int ReturnLineId { get; set; }

        [DataMember]
        public string ReturnTerminalId { get; set; }

        [DataMember]
        public decimal ReturnedQty { get; set; }
        
    }
}

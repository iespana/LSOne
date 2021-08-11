using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace StoreServerInterface.DTO
{
    public class DineInTableDTO
    {
        [DataMember]
        public int TableID { get; set; }
        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public int DiningTableStatus { get; set; }
        [DataMember]
        public string TransactionXML { get; set; }
        [DataMember]
        public int NumberOfGuests { get; set; }
        [DataMember]
        public string TerminalID { get; set; }
        [DataMember]
        public string StaffID { get; set; }
        [DataMember]
        public string StoreID { get; set; }
        [DataMember]
        public string SalesType { get; set; }
        [DataMember]
        public int Sequence { get; set; }
        [DataMember]
        public string DiningTableLayoutID { get; set; }
        [DataMember]
        public string DataAreaId { get; set; }

    }
}

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.BusinessObjects.TaxFree
{
    [DataContract]
    public enum TaxRefundStatus
    {
        [EnumMember]
        Active,
        [EnumMember]
        Canceled
    }

    [Serializable]
    [DataContract]
    [KnownType(typeof(RecordIdentifier))]
    [KnownType(typeof(Tourist))]
    [KnownType(typeof(Address))]
    [KnownType(typeof(TaxRefundTransaction))]
    [KnownType(typeof(TaxRefundStatus))]
    public class TaxRefund : DataEntity
    {
        [DataMember]
        public DateTime Created { get; set; }
        [DataMember]
        public RecordIdentifier StoreID { get; set; }
        [DataMember]
        public RecordIdentifier TerminalID { get; set; }
        [DataMember]
        public RecordIdentifier TouristID { get; set; }
        [DataMember]
        public string Booking { get; set; }
        [DataMember]
        public string Running { get; set; }
        [DataMember]
        public decimal Total { get; set; }
        [DataMember]
        public decimal Tax { get; set; }
        [DataMember]
        public decimal TaxRefundValue { get; set; }

        // Populated by data provider from external tables
        [DataMember]
        public Tourist Tourist { get; set; }
        [DataMember]
        public TaxRefundStatus Status { get; set; }
        [DataMember]
        public string Comment { get; set; }
        [DataMember]
        public List<TaxRefundTransaction> Transactions { get; set; }
    }
}

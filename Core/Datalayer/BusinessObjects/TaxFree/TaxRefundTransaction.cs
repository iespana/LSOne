using System;
using System.Runtime.Serialization;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.BusinessObjects.TaxFree
{
    [Serializable]
    [DataContract]
    [KnownType(typeof(RecordIdentifier))]
    public class TaxRefundTransaction : DataEntity
    {
        [DataMember]
        public RecordIdentifier TaxRefundID { get; set; }
        [DataMember]
        public RecordIdentifier StoreID { get; set; }
        [DataMember]
        public RecordIdentifier TerminalID { get; set; }
        [DataMember]
        public RecordIdentifier TransactionID { get; set; }
        [DataMember]
        public decimal Total { get; set; }
        [DataMember]
        public decimal TotalTax { get; set; }
        [DataMember]
        public decimal TotalTaxRefund { get; set; }
    }
}

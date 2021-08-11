using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.Utilities.DataTypes;
using LSOne.Utilities.Validation;

namespace LSOne.DataLayer.BusinessObjects.CustomerOrders
{
    [DataContract]
    [KnownType(typeof(RecordIdentifier))]
    public class CustomerOrderSearch
    {
        [DataMember]
        [RecordIdentifierValidation(RecordIdentifier.RecordIdentifierType.Guid)]
        public RecordIdentifier ID;

        [DataMember]
        [RecordIdentifierValidation(RecordIdentifier.RecordIdentifierType.String)]
        public RecordIdentifier Reference;

        [DataMember]
        public bool ReferenceBeginsWith;

        [DataMember]
        [RecordIdentifierValidation(RecordIdentifier.RecordIdentifierType.String)]
        public RecordIdentifier CustomerID;

        public string CustomerName;

        [DataMember]
        [RecordIdentifierValidation(RecordIdentifier.RecordIdentifierType.String)]
        public RecordIdentifier Delivery;

        [DataMember]
        [RecordIdentifierValidation(RecordIdentifier.RecordIdentifierType.String)]
        public RecordIdentifier Source;

        [DataMember]
        [RecordIdentifierValidation(RecordIdentifier.RecordIdentifierType.String)]
        public RecordIdentifier DeliveryLocation;

        [DataMember]
        public string Comment;

        [DataMember]
        public int Status;

        [DataMember]
        public bool? Expired;

        [DataMember]
        public CustomerOrderType OrderType;

        [DataMember]
        public bool RetrieveOrderXML;



        public CustomerOrderSearch()
        {
            Reference = RecordIdentifier.Empty;
            CustomerID = RecordIdentifier.Empty;
            CustomerName = "";
            Delivery = RecordIdentifier.Empty;
            Source = RecordIdentifier.Empty;
            DeliveryLocation = RecordIdentifier.Empty;
            Comment = "";
            Expired = false;
            Status = -1;
            RetrieveOrderXML = true;
            OrderType = CustomerOrderType.None;
        }
    }
}

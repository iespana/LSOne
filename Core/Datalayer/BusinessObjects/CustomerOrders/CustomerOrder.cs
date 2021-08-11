using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.Utilities.Attributes;
using LSOne.Utilities.DataTypes;
using LSOne.Utilities.Enums;
using LSOne.Utilities.Validation;

namespace LSOne.DataLayer.BusinessObjects.CustomerOrders
{
    [DataContract]
    [KnownType(typeof(RecordIdentifier))]
    [KnownType(typeof(Date))]
    public class CustomerOrder : OptimizedUpdateDataEntity
    {
        private RecordIdentifier reference;
        private RecordIdentifier source;
        private RecordIdentifier delivery;
        private RecordIdentifier customerID;
        private Date expiryDate;
        private RecordIdentifier deliveryLocation;
        private CustomerOrderType orderType;
        private string orderXML;
        private RecordIdentifier storeID;
        private RecordIdentifier terminalID;
        private RecordIdentifier staffID;
        private string comment;
        private CustomerOrderStatus status;


        [DataMember]
        [RecordIdentifierValidation(RecordIdentifier.RecordIdentifierType.String)]
        public RecordIdentifier Reference
        {
            get { return reference; }
            set
            {
                reference = value;
                AddColumnInfo("REFERENCE");
            }
        }

        [DataMember]
        [RecordIdentifierValidation(RecordIdentifier.RecordIdentifierType.Guid)]
        [RecordIdentifierConstruction(typeof (Guid))]
        public RecordIdentifier Source
        {
            get { return source; }
            set
            {
                source = value;
                AddColumnInfo("SOURCE");
            }
        }

        [DataMember]
        [RecordIdentifierValidation(RecordIdentifier.RecordIdentifierType.Guid)]
        [RecordIdentifierConstruction(typeof (Guid))]
        public RecordIdentifier Delivery
        {
            get { return delivery; }
            set
            {
                delivery = value;
                AddColumnInfo("DELIVERY");
            }
        }

        [DataMember]
        [RecordIdentifierValidation(RecordIdentifier.RecordIdentifierType.String)]
        public RecordIdentifier CustomerID
        {
            get { return customerID; }
            set
            {
                customerID = value;
                AddColumnInfo("CUSTOMERID");
            }
        }

        [DataMember]
        public Date ExpiryDate
        {
            get { return expiryDate; }
            set
            {
                expiryDate = value;
                AddColumnInfo("EXPIRES");
            }
        }

        [DataMember]
        [RecordIdentifierValidation(20)]
        public RecordIdentifier DeliveryLocation
        {
            get { return deliveryLocation; }
            set
            {
                deliveryLocation = value;
                AddColumnInfo("DELIVERYLOCATIONID");
            }
        }

        [DataMember]
        public CustomerOrderType OrderType
        {
            get { return orderType; }
            set
            {
                orderType = value;
                AddColumnInfo("ORDERTYPE");
            }
        }

        [DataMember]
        public string OrderXML
        {
            get { return orderXML; }
            set
            {
                orderXML = value;
                AddColumnInfo("ORDERXML", OptimizedUpdateColumnAction.Continue);
            }
        }

        [DataMember]
        [RecordIdentifierValidation(20)]
        public RecordIdentifier StoreID
        {
            get { return storeID; }
            set
            {
                storeID = value;
                AddColumnInfo("CREATEDSTOREID");
            }
        }

        [DataMember]
        [RecordIdentifierValidation(20)]
        public RecordIdentifier StaffID
        {
            get { return staffID; }
            set
            {
                staffID = value;
                AddColumnInfo("CREATEDBY");
            }
        }

        [DataMember]
        [RecordIdentifierValidation(20)]
        public RecordIdentifier TerminalID
        {
            get { return terminalID; }
            set
            {
                terminalID = value;
                AddColumnInfo("CREATEDTERMINALID");
            }
        }

        [DataMember]
        public DateTime CreatedDate { get; set; }

        [DataMember]
        [StringLength(250)]
        public string Comment
        {
            get { return comment; }
            set
            {
                comment = value;
                AddColumnInfo("COMMENT");
            }
        }

        [DataMember]
        public CustomerOrderStatus Status
        {
            get { return status; }
            set
            {
                status = value;
                AddColumnInfo("STATUS");
            }
        }

        /// <summary>
        /// Used when managing the customer orders in lists. This value is not saved 
        /// </summary>
        [DataMember]
        public bool Selected { get; set; }

        public CustomerOrder()
        {
            Clear();
        }

        public void Clear()
        {
            reference = RecordIdentifier.Empty;
            source = RecordIdentifier.Empty;
            delivery = RecordIdentifier.Empty;
            expiryDate = Date.Now;
            deliveryLocation = RecordIdentifier.Empty;
            orderType = CustomerOrderType.None;
            orderXML = null;
            storeID = RecordIdentifier.Empty;
            staffID = RecordIdentifier.Empty;
            terminalID = RecordIdentifier.Empty;
            CreatedDate = DateTime.MinValue;
            comment = "";
            status = CustomerOrderStatus.Open;
            Selected = false;
        }

        public string StatusText()
        {
            switch (Status)
            {
                case CustomerOrderStatus.Open:
                    return Properties.Resources.COStatus_Open;
                    
                case CustomerOrderStatus.Closed:
                    return Properties.Resources.COStatus_Closed;
                    
                case CustomerOrderStatus.Cancelled:
                    return Properties.Resources.COStatus_Cancelled;
                    
                case CustomerOrderStatus.New:
                    return Properties.Resources.COStatus_New;
                    
                case CustomerOrderStatus.Printed:
                    return Properties.Resources.COStatus_PickingList;
                    
                case CustomerOrderStatus.Ready:
                    return Properties.Resources.COStatus_ReadyForDelivery;
                    
                case CustomerOrderStatus.Delivered:
                    return Properties.Resources.COStatus_Delivered;
                    
                default:
                    return Status.ToString();
            }
        }
    }
}

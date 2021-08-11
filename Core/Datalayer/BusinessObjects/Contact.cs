using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.Utilities.DataTypes;
using LSOne.Utilities.Validation;

namespace LSOne.DataLayer.BusinessObjects
{
    [DataContract]
    public class Contact : DataEntity
    {
        public Contact()
            : base()
        {
            Name = new Name();
            Address = new Address();

            OwnerType = ContactRelationTypeEnum.Customer;
            OwnerID = "";
            ContactType = TypeOfContactEnum.Person;
            Phone = "";
            Phone2 = "";
            Fax = "";
            Email = "";
        }

        [DataMember]
        [RecordIdentifierValidation(20)]
        public override RecordIdentifier ID
        {
            get {return base.ID;}
            set {base.ID = value;}
        }

        [DataMember]
        public ContactRelationTypeEnum OwnerType { get; set; }

        [DataMember]
        [RecordIdentifierValidation(20)]
        public RecordIdentifier OwnerID { get; set; }

        [DataMember]
        public TypeOfContactEnum ContactType { get; set; }

        [DataMember]
        [StringLength(60)]
        public string CompanyName { get; set; }

        [DataMember]
        [StringLength(80)]
        public string Phone { get; set; }

        [DataMember]
        [StringLength(80)]
        public string Phone2 { get; set; }

        [DataMember]
        [StringLength(80)]
        public string Fax { get; set; }

        [DataMember]
        [StringLength(80)]
        public string Email { get; set; }

        [DataMember]
        public Name Name { get; set; }

        [DataMember]
        [AddressValidation]
        public Address Address { get; set; }
    }
}

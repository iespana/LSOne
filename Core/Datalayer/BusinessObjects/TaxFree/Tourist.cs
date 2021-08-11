using System;
using System.Runtime.Serialization;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.BusinessObjects.TaxFree
{
    [Serializable]
    [DataContract]
    [KnownType(typeof(Address))]
    public class Tourist : DataEntity
    {
        [DataMember]
        public string Name { get { return Text; } set { Text = value; } }
        [DataMember]
        public string Email { get; set; }
        [DataMember]
        public Address Address { get; set; }
        [DataMember]
        public string Nationality { get; set; }
        [DataMember]
        public string PassportNumber { get; set; }
        [DataMember]
        public string PassportIssuedBy { get; set; }
        [DataMember]
        public DateTime PassportIssuedOn { get; set; }
        [DataMember]
        public string FlightNumber { get; set; }
        [DataMember]
        public DateTime ArrivalDate { get; set; }
        [DataMember]
        public DateTime DepartureDate { get; set; }

        public override object Clone()
        {
            var tourist = new Tourist();
            Populate(tourist);
            return tourist;
        }

        protected void Populate(Tourist tourist)
        {
            tourist.Name = Name;
            tourist.Email = Email;
            tourist.Address = Address;
            tourist.Nationality = Nationality;
            tourist.PassportNumber = PassportNumber;
            tourist.PassportIssuedBy = PassportIssuedBy;
            tourist.PassportIssuedOn = PassportIssuedOn;
            tourist.FlightNumber = FlightNumber;
            tourist.ArrivalDate = ArrivalDate;
            tourist.DepartureDate = DepartureDate;
        }
    }
}

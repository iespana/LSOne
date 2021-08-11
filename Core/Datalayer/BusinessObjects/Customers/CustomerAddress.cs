#if !MONO
#endif
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Xml.Linq;
using LSOne.Utilities.DataTypes;
using LSOne.Utilities.ErrorHandling;
using LSOne.Utilities.Validation;

[assembly: ContractNamespace("http://www.lsretail.com/lsone/2017/12/entities", ClrNamespace = "LSOne.DataLayer.BusinessObjects.Customers")]
namespace LSOne.DataLayer.BusinessObjects.Customers
{
    [Serializable]
    [DataContract]
    [KnownType(typeof(Address))]
    [KnownType(typeof(RecordIdentifier))]
    public class CustomerAddress : DataEntity, IEquatable<CustomerAddress>
    {
        public CustomerAddress() 
            : base()
        {
            Address = new Address();
            CustomerID = RecordIdentifier.Empty;
            TaxGroup = "";
            Telephone = "";
            MobilePhone = "";
            Email = "";

            TaxGroup = new RecordIdentifier();
        }

        public CustomerAddress(CustomerAddress address)
            : base()
        {
            ID = (RecordIdentifier)address.ID.Clone();
            CustomerID = (RecordIdentifier)address.CustomerID.Clone();
            Address = new Address(address.Address);
            
            Telephone = address.Telephone;
            MobilePhone = address.MobilePhone;
            Email = address.Email;

            TaxGroup = (RecordIdentifier)address.TaxGroup.Clone();
            IsDefault = address.IsDefault;
        }


        [DataMember]
        [AddressValidation]
        public Address Address { get; set; }

        [DataMember]
        [RecordIdentifierValidation(20)]
        public RecordIdentifier TaxGroup { get; set; }

        [DataMember]
        public RecordIdentifier CustomerID { get; set; }

        [DataMember]
        public string TaxGroupName { get; set; }

        [DataMember]
        [StringLength(50)]
        public string ContactName { get; set; }

        [DataMember]
        [StringLength(80)]
        public string Telephone { get; set; }

        [DataMember]
        [StringLength(80)]
        public string MobilePhone { get; set; }

        [DataMember]
        [StringLength(80)]
        public string Email { get; set; }

        public bool InDatabase { get; set; }
        public bool Dirty { get; set; }

        [DataMember]
        public bool IsDefault { get; set; }

        public override object Clone()
        {
            var address = new CustomerAddress
                {
                    ID = (RecordIdentifier)ID.Clone(),
                    Text = Text,
                    Address = new Address(Address),
                    TaxGroup = (RecordIdentifier)TaxGroup.Clone(),
                    Telephone = Telephone,
                    MobilePhone = MobilePhone,
                    Email = Email
                };

            return address;
        }

        /// <summary>
        /// Sets all variables in the Customer class with the values in the xml
        /// </summary>
        /// <param name="aAddress">The xml element with the customer values</param>
        /// <param name="errorLogger"></param>
        public override void ToClass(XElement aAddress, IErrorLog errorLogger = null)
        {
            if (aAddress.HasElements)
            {
                IEnumerable<XElement> addressVariables = aAddress.Elements();
                foreach (XElement addressElement in addressVariables)
                {
                    if (!addressElement.IsEmpty)
                    {
                        try
                        {
                            switch (addressElement.Name.ToString())
                            {
                                case "Id":
                                    ID = new Guid(addressElement.Value);
                                    break;
                                case "customerId":
                                    CustomerID = addressElement.Value;
                                    break;
                                case "telephone":
                                    Telephone = addressElement.Value;
                                    break;
                                case "mobilePhone":
                                    MobilePhone = addressElement.Value;
                                    break;
                                case "email":
                                    Email = addressElement.Value;
                                    break;
                                case "taxGroup":
                                    TaxGroup = addressElement.Value;
                                    break;
                                default:
                                    Address.ToClass(addressElement);
                                    break;
                            }
                        }
                        catch (Exception ex)
                        {
                            if (errorLogger != null)
                            {
                                errorLogger.LogMessage(LogMessageType.Error,
                                                       addressElement.Name.ToString(), ex);
                            }
                        }
                    }
                }
            }
        }


        /// <summary>
        /// Creates an xml element from all the variables in the Customer class
        /// </summary>
        /// <param name="errorLogger"></param>
        /// <returns>An XML element</returns>
        public override XElement ToXML(IErrorLog errorLogger = null)
        {
            var xAddress = new XElement("Address",
                new XElement("Id", ID.ToString()),
                new XElement("customerId", (string)CustomerID),
                new XElement("telephone", Telephone),
                new XElement("mobilePhone", MobilePhone),
                new XElement("email", Email),
                new XElement("taxGroup", (string)TaxGroup),
                Address.ToXML()
            );

            return xAddress;
        }

        #region IEquatable<CustomerAddress>

        public bool Equals(CustomerAddress other)
        {
            if (ReferenceEquals(other, null))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            Func<string, string> stringEmptyIfNull = input => input ?? string.Empty;

            return ID.Equals(other.ID) && 
                CustomerID.Equals(other.CustomerID) &&
                stringEmptyIfNull(Telephone).Equals(stringEmptyIfNull(other.Telephone)) &&
                stringEmptyIfNull(MobilePhone).Equals(stringEmptyIfNull(other.MobilePhone)) &&
                stringEmptyIfNull(Email).Equals(stringEmptyIfNull(other.Email)) &&
                TaxGroup.Equals(other.TaxGroup) &&
                Address.Equals(other.Address);
        }

        // If Equals() returns true for a pair of objects then GetHashCode() must return the same value for these objects.
        // Equals() is called only if GetHashCode returns the same value for two objects.
        public override int GetHashCode()
        {
            int hashID = ID == null ? 0 : ID.GetHashCode();
            int hashCustomerID = CustomerID == null ? 0 : CustomerID.GetHashCode();
            int hashTelephone = Telephone == null ? 0 : Telephone.GetHashCode();
            int hashMobilePhone = MobilePhone == null ? 0 : MobilePhone.GetHashCode();
            int hashEmail = Email == null ? 0 : Email.GetHashCode();
            int hashTaxGroup = TaxGroup == null ? 0 : TaxGroup.GetHashCode();
            int hashAddress = Address == null ? 0 : Address.GetHashCode();

            // Calculate the hash code for the customer address
            return hashID ^ hashCustomerID ^ hashTelephone ^ hashMobilePhone ^ hashEmail ^ hashTaxGroup ^ hashAddress;
        }

        #endregion IEquatable<CustomerAddress>
    }
}
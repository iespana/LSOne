using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Security.Authentication.ExtendedProtection.Configuration;
using LSOne.Utilities.DataTypes;
using LSOne.Utilities.Localization;
using LSOne.Utilities.Validation;

[assembly: ContractNamespace("http://www.lsretail.com/lsone/2017/12/entities", ClrNamespace = "LSOne.DataLayer.BusinessObjects.Customers")]
namespace LSOne.DataLayer.BusinessObjects.Customers
{
    [Serializable]
    [DataContract]
    [KnownType(typeof(CustomerAddress))]
    [KnownType(typeof(Name))]
    [KnownType(typeof(RecordIdentifier))]
    public class CustomerListItem : OptimizedUpdateDataEntity
    {
        private string accountNumber;
        private string identificationNumber;
        private bool locallySaved;
        private string searchName;
        private Guid masterID;

        public CustomerListItem()
            :base()
        {
            InitializeBase();
        }

        protected sealed override void InitializeBase()
        {
            accountNumber = "";
            Name = new Name();
            Addresses = new List<CustomerAddress>();
            Deleted = false;
            identificationNumber = "";
            locallySaved = false;
            searchName = "";
            masterID = Guid.Empty;
        }

        protected override bool NestedDataIsEqualTo(IOptimizedUpdate optimizedUpdateEntity)
        {
            var otherObject = optimizedUpdateEntity as CustomerListItem;

            if ((Addresses == null ^ otherObject.Addresses == null) ||
                (Addresses == null && Addresses.Count != otherObject.Addresses.Count))
            {
                return false;
            }

            Func<CustomerAddress, object> joinKey = x => new
            {
                x.IsDefault,
                x.Address
            };

            var distinctAddresses = Addresses.Distinct().ToList();
            var otherObjectDistinctAddresses = otherObject.Addresses.Distinct().ToList();

            var numberOfMatchedAddresses = distinctAddresses.Join(otherObjectDistinctAddresses, joinKey, joinKey, (x, y) => 1).Count();

            return numberOfMatchedAddresses == distinctAddresses.Count && numberOfMatchedAddresses == otherObjectDistinctAddresses.Count;
        }

        public string GetFormattedName(INameFormatter formatter)
        {
            return formatter.Format(Name);
        }

        public bool CompareNames(Name name)
        {
            return name == Name;
        }

        public Name CopyName()
        {
            return new Name(Name);
        }

        [DataMember]
        protected Name Name { get;  private set; }

        public void SetName(Name name)
        {
            Name = new Name(name);

            PropertyChanged("FIRSTNAME", FirstName);
            PropertyChanged("LASTNAME", LastName);
            PropertyChanged("MIDDLENAME", MiddleName);
            PropertyChanged("NAMEPREFIX", Prefix);
            PropertyChanged("NAMESUFFIX", Suffix);
        }

        [DataMember]
        public string FirstName
        {
            get
            {
                return Name.First;
            }
            set
            {
                if (Name.First != value)
                {
                    PropertyChanged("FIRSTNAME", value);
                    Name.First = value;
                }
            }
        }

        [DataMember]
        public string MiddleName
        {
            get
            {
                return Name.Middle;
            }
            set
            {
                if (Name.Middle != value)
                {
                    PropertyChanged("MIDDLENAME", value);
                    Name.Middle = value;
                }
            }
        }

        [DataMember]
        public string LastName
        {
            get
            {
                return Name.Last;
            }
            set
            {
                if (Name.Last != value)
                {
                    PropertyChanged("LASTNAME", value);
                    Name.Last = value;
                }
            }
        }

        [DataMember]
        public string Prefix
        {
            get
            {
                return Name.Prefix;
            }
            set
            {
                if (Name.Prefix != value)
                {
                    PropertyChanged("NAMEPREFIX", value);
                    Name.Prefix = value;
                }
            }
        }

        [DataMember]
        public string Suffix
        {
            get
            {
                return Name.Suffix;
            }
            set
            {
                if (Name.Suffix != value)
                {
                    PropertyChanged("NAMESUFFIX", value);
                    Name.Suffix = value;
                }
            }
        }

        [StringLength(60)]
        [DataMember]
        public override string Text
        {
            get
            {
                return base.Text;
            }
            set
            {
                if (base.Text != value)
                {
                    PropertyChanged("NAME", value);
                    base.Text = value;
                }
            }
        }

        [DataMember]
        public Guid MasterID
        {
            get
            {
                return masterID;
            }
            set
            {
                masterID = value;
            }
        }

        [DataMember]
        public string AccountNumber
        {
            get
            {
                return accountNumber;
            }
            set
            {
                if (accountNumber != value)
                {
                    PropertyChanged("INVOICEACCOUNT", value);
                    accountNumber = value;
                }
            }
        }

        //Displayed on POS customer search
        [DataMember]
        public string InvoiceAccountName { get; set; }

        [DataMember]
        public bool Deleted { get; set; }

        [AddressValidation]
        public Address DefaultShippingAddress
        {
            get
            {
                if (Addresses != null && Addresses.Count > 0)
                {
                    List<CustomerAddress> shippingAddresses = new List<CustomerAddress>();

                    foreach (var address in Addresses)
                    {
                        if (address.Address != null && address.Address.AddressType == Address.AddressTypes.Shipping)
                        {
                            if (address.IsDefault)
                            {
                                return address.Address;
                            }
                            else
                            {
                                shippingAddresses.Add(address);
                            }
                        }
                    }

                    if (shippingAddresses.Count > 0 && shippingAddresses[0].Address != null)
                    {
                        return shippingAddresses[0].Address;
                    }

                    if (Addresses[0].Address != null)
                    {
                        return Addresses[0].Address;
                    }
                }
                    
                // No addresses
                return new Address();
            }
        }

        [AddressValidation]
        public Address DefaultBillingAddress
        {
            get
            {
                if (Addresses != null && Addresses.Count > 0)
                {
                    List<CustomerAddress> billingAddresses = new List<CustomerAddress>();

                    foreach (var address in Addresses)
                    {
                        if (address.Address != null && address.Address.AddressType == Address.AddressTypes.Billing)
                        {
                            if(address.IsDefault)
                            {
                                return address.Address;
                            }
                            else
                            {
                                billingAddresses.Add(address);
                            }
                        }
                    }

                    if(billingAddresses.Count > 0 && billingAddresses[0].Address != null)
                    {
                        return billingAddresses[0].Address;
                    }

                    if (Addresses[0].Address != null)
                    {
                        return Addresses[0].Address;
                    }
                }

                // No addresses
                return new Address();
            }
        }

        [DataMember]
        public List<CustomerAddress> Addresses { get; set; }

        /// <summary>
        /// The organisation id of the customer, a.k.a. "The Icelandic Kennitala".
        /// </summary>
        [DataMember]
        public string IdentificationNumber
        {
            get
            {
                return identificationNumber; 
            }
            set
            {
                if (identificationNumber != value)
                {
                    PropertyChanged("ORGID", value);
                    identificationNumber = value;
                }
            }
        }

        [DataMember]
        public bool LocallySaved
        {
            get
            {
                return locallySaved; 
            }
            set
            {
                if (locallySaved != value)
                {
                    PropertyChanged("LOCALLYSAVED", value);
                    locallySaved = value;
                }
            }
        }

        /// <summary>
        /// A search alias that can be anything f.ex. mobile number
        /// </summary>
        [DataMember]
        [StringLength(80)]
        public string SearchName
        {
            get
            {
                return searchName; 
            }
            set
            {
                if (searchName != value)
                {
                    PropertyChanged("NAMEALIAS", value);
                    searchName = value;
                }
            }
        }
    }
}
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.Utilities.DataTypes;
using LSOne.Utilities.Validation;
using LSOne.DataLayer.GenericConnector.Interfaces;
#if !MONO
#endif

namespace LSOne.DataLayer.BusinessObjects.Inventory
{
    public class Vendor : OptimizedUpdateDataEntity
    {
        string phone;
        string fax;
        string email;
        RecordIdentifier languageID;
        RecordIdentifier currencyID;
        RecordIdentifier defaultContactID;
        RecordIdentifier taxGroupID;
        string currencyDescription;
        string longDescription;
        TaxCalculationMethodEnum taxCalculationMethod;
        int defaultDeliveryTime;
        DeliveryDaysTypeEnum deliveryDaysType;

        protected Address Address { get; private set; }

        public bool Dirty { get; set; }

        public Vendor() : base()
        {
            Address = new Address();
            phone = "";
            email = "";
            languageID = RecordIdentifier.Empty;
            currencyID = RecordIdentifier.Empty;
            longDescription = "";
            currencyDescription = "";
            fax = "";
            defaultContactID = RecordIdentifier.Empty;
            taxGroupID = RecordIdentifier.Empty;
            taxCalculationMethod = TaxCalculationMethodEnum.NoTax;
            deliveryDaysType = DeliveryDaysTypeEnum.Days;
            Dirty = false;
            defaultDeliveryTime = 0;
            Deleted = false;
        }

        [RecordIdentifierValidation(20)]
        public override RecordIdentifier ID
        {
            get { return base.ID; }
            set { base.ID = value; }
        }

        [StringLength(200)]
        public override string Text
        {
            get {return base.Text;}
            set
            {
                if (base.Text != value)
                {
                    base.Text = value;
                    PropertyChanged("NAME", value);
                }
            }
        }

        [StringLength(80)]
        public string Phone
        {
            get { return phone; }
            set
            {
                if (phone != value)
                {
                    phone = value;
                    PropertyChanged("PHONE", value);
                }
            }
        }

        [StringLength(20)]
        public string Fax
        {
            get { return fax; }
            set
            {
                if (fax != value)
                {
                    fax = value;
                    PropertyChanged("FAX", value);
                }
            }
        }

        [StringLength(80)]
        public string Email
        {
            get { return email; }
            set
            {
                if (email != value)
                {
                    email = value;
                    PropertyChanged("EMAIL", value);
                }
            }
        }

        [RecordIdentifierValidation(7)]
        public RecordIdentifier LanguageID
        {
            get { return languageID; }
            set
            {
                if (languageID != value)
                {
                    languageID = value;
                    PropertyChanged("LANGUAGEID", value);
                }
            }
        }

        [RecordIdentifierValidation(3)]
        public RecordIdentifier CurrencyID
        {
            get { return currencyID; }
            set
            {
                if (currencyID != value)
                {
                    currencyID = value;
                    PropertyChanged("CURRENCY", value);
                }
            }
        }

        [RecordIdentifierValidation(20)]
        public RecordIdentifier DefaultContactID
        {
            get { return defaultContactID; }
            set
            {
                if (defaultContactID != value)
                {
                    defaultContactID = value;
                    PropertyChanged("DEFAULTCONTACTID", value);
                }
            }
        }

        public string CurrencyDescription
        {
            get { return currencyDescription; }
            set { currencyDescription = value; }
        }

        public string LongDescription
        {
            get { return longDescription; }
            set
            {
                if (longDescription != value)
                {
                    longDescription = value;
                    PropertyChanged("MEMO", value);
                }
            }
        }

        [RecordIdentifierValidation(20)]
        public RecordIdentifier TaxGroup
        {
            get { return taxGroupID; }
            set
            {
                if (taxGroupID != value)
                {
                    taxGroupID = value;
                    PropertyChanged("TAXGROUP", value);
                }
            }
        }

        public string TaxGroupName { get; set; }

        public TaxCalculationMethodEnum TaxCalculationMethod
        {
            get { return taxCalculationMethod; }
            set
            {
                if (taxCalculationMethod != value)
                {
                    taxCalculationMethod = value;
                    PropertyChanged("TAXCALCULATIONMETHOD", value);
                }
            }
        }

        public int DefaultDeliveryTime
        {
            get { return defaultDeliveryTime; }
            set
            {
                if (defaultDeliveryTime != value)
                {
                    defaultDeliveryTime = value;
                    PropertyChanged("DEFAULTDELIVERYTIME", value);
                }
            }
        }

        public DeliveryDaysTypeEnum DeliveryDaysType
        {
            get { return deliveryDaysType; }
            set
            {
                if (deliveryDaysType != value)
                {
                    deliveryDaysType = value;
                    PropertyChanged("DELIVERYDAYSTYPE", value);
                }
            }
        }

        /// <summary>
        /// If true then the Vendor has been deleted. Default value is false
        /// </summary>
        public bool Deleted { get; set; }

        public string Address1
        {
            get { return Address.Address1; }
            set
            {
                if (Address.Address1 != value)
                {
                    Address.Address1 = value;
                    PropertyChanged("STREET", value);
                }
            }
        }

        public string Address2
        {
            get { return Address.Address2; }
            set
            {
                if (Address.Address2 != value)
                {
                    Address.Address2 = value;
                    PropertyChanged("ADDRESS", value);
                }
            }
        }

        public string City
        {
            get { return Address.City; }
            set
            {
                if (Address.City != value)
                {
                    Address.City = value;
                    PropertyChanged("CITY", value);
                }
            }
        }

        public string ZipCode
        {
            get { return Address.Zip; }
            set
            {
                if (Address.Zip != value)
                {
                    Address.Zip = value;
                    PropertyChanged("ZIPCODE", value);
                }
            }
        }

        public string State
        {
            get { return Address.State; }
            set
            {
                if (Address.State != value)
                {
                    Address.State = value;
                    PropertyChanged("STATE", value);
                }
            }
        }

        public RecordIdentifier Country
        {
            get { return Address.Country; }
            set
            {
                if (Address.Country != value)
                {
                    Address.Country = value;
                    PropertyChanged("COUNTRY", value);
                }
            }
        }

        public Address.AddressFormatEnum AddressFormat
        {
            get { return Address.AddressFormat; }
            set
            {
                if (Address.AddressFormat != value)
                {
                    Address.AddressFormat = value;
                    PropertyChanged("ADDRESSFORMAT", value);
                }
            }
        }

        public void SetAddress(Address address)
        {
            Address = new Address(address);

            PropertyChanged("ADDRESS", Address.Address2);
            PropertyChanged("STREET", Address.Address1);
            PropertyChanged("CITY", Address.City);
            PropertyChanged("ZIPCODE", Address.Zip);
            PropertyChanged("STATE", Address.State);
            PropertyChanged("COUNTRY", Address.Country);
            PropertyChanged("ADDRESSFORMAT", Address.AddressFormat);
        }

        public bool CompareAddress(Address address)
        {
            return address == Address;
        }

        public string GetFormattedAddress(IConnectionManager entry)
        {
            return entry.Settings.LocalizationContext.FormatSingleLine(Address, entry.Cache);
        }

        public Address CopyAddress()
        {
            return new Address(Address);
        }
    }
}
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.BusinessObjects.Companies
{
    public class CompanyInfo : DataEntity
    {
        Address address;

        public CompanyInfo()
        {
            ID = 1;
            CurrencyCode = "";
            CurrencyCodeText = "";
            Dirty = false;
#if !MONO
            CompanyLogo = null;
#endif

            LanguageCode = "";
            RegistrationNumber = "";
            address = new Address();
        }

        public CompanyInfo(Address.AddressFormatEnum addressFormatEnum) : this()
        {
            address.AddressFormat = addressFormatEnum;
        }

        public bool Dirty { get; set; }

        /// <summary>
        /// This field contains the default currency for the head office. It is used for example when getting the item prices.
        /// </summary>
        public RecordIdentifier CurrencyCode { get; set; }

        public string CurrencyCodeText { get; set; }

        public Address Address
        {
            get
            {
                return address;
            }
            set
            {
                address = value;
            }
        }


        public string AddressFormatted { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }
        public string Email { get; set; }
        public string TaxNumber { get; set; }
        public string LanguageCode { get; set; }
#if !MONO
        public System.Drawing.Image CompanyLogo { get; set; }
#endif
        /// <summary>
        /// Company registration number
        /// </summary>
        public string RegistrationNumber { get; set; }
    }
}

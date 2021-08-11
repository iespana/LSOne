#if !MONO
#endif
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Xml.Linq;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.Utilities.DataTypes;
using LSOne.Utilities.ErrorHandling;
using LSOne.Utilities.Validation;

[assembly: ContractNamespace("http://www.lsretail.com/lsone/2017/12/entities", ClrNamespace = "LSOne.DataLayer.BusinessObjects.Customers")]
namespace LSOne.DataLayer.BusinessObjects.Customers
{
	[Serializable]
	[DataContract]
	[KnownType(typeof(CustomerAddress))]
	[KnownType(typeof(Name))]
	[KnownType(typeof(RecordIdentifier))]
	public partial class Customer : CustomerListItem, ICloneable
	{
		private string currency;
		private string languageCode;
		private RecordIdentifier taxGroup;
		private decimal maxCredit;
		private bool nonChargableAccount;
		private BlockedEnum blocked;
		private RecordIdentifier priceGroupID;
		private RecordIdentifier finalDiscountID;
		private RecordIdentifier lineDiscountID;
		private RecordIdentifier multiLineDiscountID;
		private bool pricesIncludeSalesTax;
		private RecordIdentifier customerGroupID;
		private string telephone;
		private string mobilePhone;
		private string email;
		private ReceiptSettingsEnum receiptSettings;
		private string receiptEmailAddress;
		private string vatNum;
		private string url;
		private string taxOffice;
		private bool usePurchaseRequest;
		private GenderEnum gender;
		private DateTime dateOfBirth;
		private TaxExemptEnum taxExempt;

		public Customer() 
			: base()
		{
			Initialize();

			SetDefaults();
		}

		protected sealed override void Initialize()
		{
			lineDiscountID = RecordIdentifier.Empty;
			LineDiscountDescription = "";
			priceGroupID = RecordIdentifier.Empty;
			PriceGroupDescription = "";
			multiLineDiscountID = RecordIdentifier.Empty;
			MultiLineDiscountDescription = "";
			finalDiscountID = RecordIdentifier.Empty;
			FinalDiscountDescription = "";
			taxGroup = RecordIdentifier.Empty;
			blocked = BlockedEnum.Nothing;
			nonChargableAccount = false;
			maxCredit = 0;
			languageCode = "";
			currency = "";
			customerGroupID = RecordIdentifier.Empty;
			telephone = "";
			mobilePhone = "";
			receiptEmailAddress = "";
			receiptSettings = ReceiptSettingsEnum.Printed;
			Dirty = false;
			Deleted = false;
			vatNum = "";
			email = "";
			url = "";
			taxOffice = "";
			Balance = 0.0M;
			AllowCustomerDiscounts = true;
			gender = GenderEnum.None;
			dateOfBirth = new DateTime(1900, 1, 1);
			TaxExempt = TaxExemptEnum.No;
		}

		[DataMember]
		public bool Dirty { get; set; }

		[DataMember]
		[StringLength(3)]
		public string Currency
		{
			get { return currency; }
			set
			{
                if (currency != value)
                {
                    PropertyChanged("CURRENCY", value);
                    currency = value;
                }
			}
		}

		[DataMember]
		public string CurrencyDescription { get; set; }

		[DataMember]
		[StringLength(7)]
		public string LanguageCode
		{
			get { return languageCode; }
			set
			{
                if (languageCode != value)
                {
                    PropertyChanged("LANGUAGEID", value);
                    languageCode = value;
                }
			}
		}

		[DataMember]
		[RecordIdentifierValidation(20)]
		public RecordIdentifier TaxGroup
		{
			get { return taxGroup; }
			set
			{
                if (taxGroup != value)
                {
                    PropertyChanged("TAXGROUP", value);
                    taxGroup = value;
                }
			}
		}

		[DataMember]
		public string TaxGroupName { get; set; }

		[DataMember]
		public decimal MaxCredit
		{
			get { return maxCredit; }
			set
			{
                if (maxCredit != value)
                {
                    PropertyChanged("CREDITMAX", value);
                    maxCredit = value;
                }
			}
		}

		[DataMember]
		public bool NonChargableAccount
		{
			get { return nonChargableAccount; }
			set
			{
                if (nonChargableAccount != value)
                {
                    PropertyChanged("NONCHARGABLEACCOUNT", value);
                    nonChargableAccount = value;
                }
			}
		}

		[DataMember]
		public BlockedEnum Blocked
		{
			get { return blocked; }
			set
			{
                if (blocked != value)
                {
                    PropertyChanged("BLOCKED", value);
                    blocked = value;
                }
			}
		}

		[DataMember]
		public RecordIdentifier PriceGroupID
		{
			get { return priceGroupID; }
			set
			{
                if (priceGroupID != value)
                {
                    PropertyChanged("PRICEGROUP", value);
                    priceGroupID = value;
                }
			}
		}

		[DataMember]
		public string PriceGroupDescription { get; set; }

		[DataMember]
		public string LineDiscountDescription { get; set; }

		[DataMember]
		public string MultiLineDiscountDescription { get; set; }

		[DataMember]
		public string FinalDiscountDescription { get; set; }

		[DataMember]
		public RecordIdentifier FinalDiscountID
		{
			get { return finalDiscountID; }
			set
			{
                if (finalDiscountID != value)
                {
                    PropertyChanged("ENDDISC", value);
                    finalDiscountID = value;
                }
			}
		}

		[DataMember]
		public RecordIdentifier LineDiscountID
		{
			get { return lineDiscountID; }
			set
			{
                if (lineDiscountID != value)
                {
                    PropertyChanged("LINEDISC", value);
                    lineDiscountID = value;
                }
			}
		}

		[DataMember]
		public RecordIdentifier MultiLineDiscountID
		{
			get { return multiLineDiscountID; }
			set
			{
                if (multiLineDiscountID != value)
                {
                    PropertyChanged("MULTILINEDISC", value);
                    multiLineDiscountID = value;
                }
			}
		}

		[DataMember]
		public bool PricesIncludeSalesTax
		{
			get { return pricesIncludeSalesTax; }
			set
			{
                if (pricesIncludeSalesTax != value)
                {
                    PropertyChanged("INCLTAX", value);
                    pricesIncludeSalesTax = value;
                }
			}
		}

		// The following are currently missing on the StoreController
		[DataMember]
		public RecordIdentifier CustomerGroupID
		{
			get { return customerGroupID; }
			set
			{
                if (customerGroupID != value)
                {
                    PropertyChanged("CUSTGROUP", value);
                    customerGroupID = value;
                }
			}
		}

		[DataMember]
		[StringLength(80)]
		public string Telephone
		{
			get { return telephone; }
			set
			{
                if (telephone != value)
                {
                    PropertyChanged("PHONE", value);
                    telephone = value;
                }
			}
		}

		[DataMember]
		[StringLength(80)]
		public string MobilePhone
		{
			get { return mobilePhone; }
			set
			{
                if (mobilePhone != value)
                {
                    PropertyChanged("CELLULARPHONE", value);
                    mobilePhone = value;
                }
			}
		}

		[DataMember]
		[StringLength(80)]
		public string Email
		{
			get { return email; }
			set
			{
                if (email != value)
                {
                    PropertyChanged("EMAIL", value);
                    email = value;
                }
			}
		}

		/// <summary>
		/// Does the customer request a printed receipt or perhaps one sent in an email.
		/// </summary>
		[DataMember]
		public ReceiptSettingsEnum ReceiptSettings
		{
			get { return receiptSettings; }
			set
			{
                if (receiptSettings != value)
                {
                    PropertyChanged("RECEIPTOPTION", value);
                    receiptSettings = value;
                }
			}
		}

		[StringLength(80)]
		[DataMember]
		public string ReceiptEmailAddress
		{
			get { return receiptEmailAddress; }
			set
			{
                if (receiptEmailAddress != value)
                {
                    PropertyChanged("RECEIPTEMAIL", value);
                    receiptEmailAddress = value;
                }
			}
		}

		[StringLength(20)]
		[DataMember]
		public string VatNum
		{
			get { return vatNum; }
			set
			{
                if (vatNum != value)
                {
                    PropertyChanged("VATNUM", value);
                    vatNum = value;
                }
			}
		}

		[StringLength(255)]
		[DataMember]
		public string Url
		{
			get { return url; }
			set
			{
                if (url != value)
                {
                    PropertyChanged("URL", value);
                    url = value;
                }
			}
		}

		[StringLength(20)]
		[DataMember]
		public string TaxOffice
		{
			get { return taxOffice; }
			set
			{
                if (taxOffice != value)
                {
                    PropertyChanged("TAXOFFICE", value);
                    taxOffice = value;
                }
			}
		}

		/// <summary>
		/// Should the till ask for a purchase request id for this customer, a.k.a. "Beiðni".
		/// </summary>
		[DataMember]
		public bool UsePurchaseRequest
		{
			get { return usePurchaseRequest; }
			set
			{
                if (usePurchaseRequest != value)
                {
                    PropertyChanged("USEPURCHREQUEST", value);
                    usePurchaseRequest = value;
                }
			}
		}

		[DataMember]
		public string ExtraInfo1 { get; set; }

		[DataMember]
		public string ExtraInfo2 { get; set; }

		[DataMember]
		public string ExtraInfo3 { get; set; }

		[DataMember]
		public string ExtraInfo4 { get; set; }

		[DataMember]
		public string ExtraInfo5 { get; set; }

		/// <summary>
		/// Was the customer added to the transaction during a Return Transaction operation?
		/// This property is used only for when customer is on a transaction, it is not propagated to or from the database.
		/// </summary>
		[DataMember]
		public bool ReturnCustomer {get; set;}

		/// <summary>
		/// The customer balance
		/// This property is used not propagated to or from the database. That is balance has to be loaded as special case.
		/// </summary>
		[DataMember]
		public decimal Balance { get; set; }

		/// <summary>
		/// If this customer is allowed to receive customer discounts
		/// </summary>
		[DataMember]
		public bool AllowCustomerDiscounts { get; set; }

		/// <summary>
		/// The customers gender
		/// </summary>
		[DataMember]
		public GenderEnum Gender
		{
			get { return gender; }
			set
			{
                if (gender != value)
                {
                    PropertyChanged("GENDER", value);
                    gender = value;
                }
			}
		}

		/// <summary>
		/// The customers date of birth
		/// </summary>
		[DataMember]
		public DateTime DateOfBirth
		{
			get { return dateOfBirth; }
			set
			{
                if (dateOfBirth != value)
                {
                    PropertyChanged("DATEOFBIRTH", value);
                    dateOfBirth = value;
                }
			}
		}

		[DataMember]
		public string ExternalID { get; set; }

		[DataMember]
		public TaxExemptEnum TaxExempt
		{
			get { return taxExempt; }
			set
			{
                if (taxExempt != value)
                {
                    PropertyChanged("TAXEXEMPT", value);
                    taxExempt = value;
                }
			}
		}

		public override object Clone()
		{
			var customer = new Customer();
			Populate(customer);
			return customer;
		}

		protected void Populate(Customer customer)
		{
			customer.ID = (RecordIdentifier)ID.Clone();
			customer.AccountNumber = AccountNumber;
			customer.Text = Text;
			customer.SetName(Name);
			customer.CustomerGroupID = (RecordIdentifier)CustomerGroupID.Clone();
			customer.Currency = Currency;
			customer.LanguageCode = LanguageCode;
			customer.MaxCredit = MaxCredit;
			customer.Balance = Balance;
			customer.Blocked = Blocked;
			customer.IdentificationNumber = IdentificationNumber;
			customer.UsePurchaseRequest = UsePurchaseRequest;
			customer.ReturnCustomer = ReturnCustomer;
			customer.AllowCustomerDiscounts = AllowCustomerDiscounts;
			customer.ReceiptSettings = ReceiptSettings;
			customer.ReceiptEmailAddress = ReceiptEmailAddress;

			if (Addresses != null && Addresses.Count > 0)
			{
				foreach (var address in Addresses)
					customer.Addresses.Add(new CustomerAddress(address));
			}

			customer.Telephone = Telephone;
			customer.MobilePhone = MobilePhone;
			customer.Email = Email;
			customer.Url = Url;
			customer.MultiLineDiscountID = (RecordIdentifier)MultiLineDiscountID.Clone();
			customer.FinalDiscountID = (RecordIdentifier)FinalDiscountID.Clone();
			customer.LineDiscountID = (RecordIdentifier)LineDiscountID.Clone();
			customer.PriceGroupID = (RecordIdentifier)PriceGroupID.Clone();
			customer.TaxGroup = (RecordIdentifier)TaxGroup.Clone();
			customer.VatNum = VatNum;
			customer.TaxOffice = TaxOffice;
			customer.NonChargableAccount = NonChargableAccount;
			customer.LocallySaved = LocallySaved;
			customer.Gender = Gender;
			customer.DateOfBirth = DateOfBirth;
			customer.TaxExempt = TaxExempt;

			customer.ExtraInfo1 = ExtraInfo1;
			customer.ExtraInfo2 = ExtraInfo2;
			customer.ExtraInfo3 = ExtraInfo3;
			customer.ExtraInfo4 = ExtraInfo4;
			customer.ExtraInfo5 = ExtraInfo5;

			PopulateCustom(customer);
		}

        public override List<string> GetIgnoredColumns()
        {
            return new List<string> { "LINEDISC", "ENDDISC", "MULTILINEDISC" };
        }

        /// <summary>
        /// Sets all variables in the Customer class with the values in the xml
        /// </summary>
        /// <param name="xCustomer">The xml element with the customer values</param>
        /// <param name="errorLogger"></param>
        public override void ToClass(XElement xCustomer, IErrorLog errorLogger = null)
		{
			if (xCustomer.HasElements)
			{
				IEnumerable<XElement> custElements = xCustomer.Elements("Customer");
				foreach (XElement aCustomer in custElements)
				{
					if (aCustomer.HasElements)
					{
						Address defaultAddress = new Address();
						IEnumerable<XElement> custVariables = aCustomer.Elements();
						foreach (XElement custElem in custVariables)
						{
							//No customer id -> no customer -> no need to go any further
							if (custElem.Name.ToString() == "customerId" && custElem.Value == string.Empty)
							{
								return;
							}

							if (!custElem.IsEmpty)
							{
								try
								{
									switch (custElem.Name.ToString())
									{
										case "customerId":
											ID = custElem.Value;
											break;
										case "masterId":
											MasterID =Conversion.XmlStringToGuid(custElem.Value);
											break;
										case "invoiceAccount":
											AccountNumber = custElem.Value;
											break;
										case "name":
											Text = custElem.Value;
											break;
										case "firstName":
											FirstName = custElem.Value;
											break;
										case "middleName":
											MiddleName = custElem.Value;
											break;
										case "lastName":
											LastName = custElem.Value;
											break;
										case "namePrefix":
											Prefix = custElem.Value;
											break;
										case "nameSuffix":
											Suffix = custElem.Value;
											break;
										case "searchName":
											SearchName = custElem.Value;
											break;
										case "custGroup":
											CustomerGroupID = custElem.Value;
											break;
										case "currency":
											Currency = custElem.Value;
											break;
										case "language":
											LanguageCode = custElem.Value;
											break;
										case "creditLimit":
											MaxCredit = Conversion.XmlStringToDecimal(custElem.Value);
											break;
										case "balance":
											Balance = Conversion.XmlStringToDecimal(custElem.Value);
											break;
										case "blocked":
											Blocked = (BlockedEnum)Conversion.XmlStringToInt(custElem.Value);
											break;
										case "TaxExempt":
											TaxExempt = (TaxExemptEnum)Conversion.XmlStringToInt(custElem.Value);
											break;
										case "orgId":
											IdentificationNumber = custElem.Value;
											break;
										case "usePurchRequest":
											UsePurchaseRequest = Conversion.XmlStringToBool(custElem.Value);
											break;
										case "returnCustomer":
											ReturnCustomer = Conversion.XmlStringToBool(custElem.Value);
											break;
										case "allowCustomerDiscounts":
											AllowCustomerDiscounts = Conversion.XmlStringToBool(custElem.Value);
											break;
										case "receiptSettings":
											ReceiptSettings = (ReceiptSettingsEnum)Conversion.XmlStringToInt(custElem.Value);
											break;
										case "receiptEmailAddress":
											ReceiptEmailAddress = custElem.Value;
											break;
										case "telephone":
											Telephone = custElem.Value;
											break;
										case "mobilePhone":
											MobilePhone = custElem.Value;
											break;
										case "email":
											Email = custElem.Value;
											break;
										case "wwwAddress":
											Url = custElem.Value;
											break;
										case "multiLineDiscountGroup":
											MultiLineDiscountID = custElem.Value;
											break;
										case "totalDiscountGroup":
											FinalDiscountID = custElem.Value;
											break;
										case "lineDiscountGroup":
											LineDiscountID = custElem.Value;
											break;
										case "priceGroup":
											PriceGroupID = custElem.Value;
											break;
										case "salesTaxGroup":
											TaxGroup = custElem.Value;
											break;
										case "vatNum":
											VatNum = custElem.Value;
											break;
										case "taxOffice":
											TaxOffice = custElem.Value;
											break;
										case "nonChargableAccount":
											NonChargableAccount = Conversion.XmlStringToBool(custElem.Value);
											break;
										case "gender":
											Gender = (GenderEnum)Conversion.XmlStringToInt(custElem.Value);
											break;
										case "dateOfBirth":
											DateOfBirth = Conversion.XmlStringToDateTime(custElem.Value);
											break;
										case "Addresses":
											{
												if (custElem.HasElements)
												{
													Addresses = new List<CustomerAddress>();
													IEnumerable<XElement> addressElements = custElem.Elements();
													foreach (XElement addressElement in addressElements)
													{
														if ("Address" == addressElement.Name.ToString())
														{
															var address = new CustomerAddress();
															address.ToClass(addressElement, errorLogger);
															Addresses.Add(address);
														}
													}
												}
											}
											break;
										case "ExtraInfo1":
											ExtraInfo1 = custElem.Value;
											break;
										case "ExtraInfo2":
											ExtraInfo2 = custElem.Value;
											break;
										case "ExtraInfo3":
											ExtraInfo3 = custElem.Value;
											break;
										case "ExtraInfo4":
											ExtraInfo4 = custElem.Value;
											break;
										case "ExtraInfo5":
											ExtraInfo5 = custElem.Value;
											break;
										case "CustomFields":
											CustomFieldsToClass(custElem);
											break;
										#region Elements moved to Addresses
										default :
											defaultAddress.ToClass(custElem);
											break;
										#endregion
									}
								}
								catch (Exception ex)
								{
									errorLogger?.LogMessage(LogMessageType.Error, custElem.Name.ToString(), ex);
								}
							}
						}

						if (!defaultAddress.IsEmpty)
						{
							defaultAddress.AddressType = Address.AddressTypes.Shipping;
							Addresses = new List<CustomerAddress> {new CustomerAddress {Address = defaultAddress}};
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
			/*
				* Strings      added as is
				* Int          added as is
				* Bool         added as is
				* 
				* Decimal      added with ToString()
				* DateTime     added with DevUtilities.Utility.DateTimeToXmlString()
				* 
				* Enums        added with an (int) cast   
				* 
			   */
			var xCustomer = new XElement("Customer",
				new XElement("customerId", (string)ID),
				new XElement("masterID", Conversion.ToXmlString(MasterID)),
				new XElement("invoiceAccount", AccountNumber),
				new XElement("name", Text),
				new XElement("firstName", FirstName),
				new XElement("middleName", MiddleName),
				new XElement("lastName", LastName),
				new XElement("namePrefix", Prefix),
				new XElement("nameSuffix", Suffix),                
				new XElement("custGroup", (string)CustomerGroupID),
				new XElement("currency", Currency),
				new XElement("language", LanguageCode),
				new XElement("creditLimit", Conversion.ToXmlString(MaxCredit)),
				new XElement("balance", Conversion.ToXmlString(Balance)),
				new XElement("blocked", Conversion.ToXmlString((int)Blocked)),
				new XElement("TaxExempt", Conversion.ToXmlString((int)TaxExempt)),
				new XElement("orgId", IdentificationNumber),
				new XElement("usePurchRequest", Conversion.ToXmlString(UsePurchaseRequest)),
				new XElement("returnCustomer", Conversion.ToXmlString(ReturnCustomer)),
				new XElement("allowCustomerDiscounts", Conversion.ToXmlString(AllowCustomerDiscounts)),
				new XElement("receiptSettings", Conversion.ToXmlString((int)ReceiptSettings)),
				new XElement("receiptEmailAddress", ReceiptEmailAddress),
				new XElement("telephone", Telephone),
				new XElement("mobilePhone", MobilePhone),
				new XElement("ExtraInfo1", ExtraInfo1),
				new XElement("ExtraInfo2", ExtraInfo2),
				new XElement("ExtraInfo3", ExtraInfo3),
				new XElement("ExtraInfo4", ExtraInfo4),
				new XElement("ExtraInfo5", ExtraInfo5),
				new XElement("email", Email),
				new XElement("wwwAddress", Url),
				new XElement("multiLineDiscountGroup", (string)MultiLineDiscountID),
				new XElement("totalDiscountGroup", (string)FinalDiscountID),
				new XElement("lineDiscountGroup", (string)LineDiscountID),
				new XElement("priceGroup", (string)PriceGroupID),
				new XElement("salesTaxGroup", (string)TaxGroup),
				new XElement("vatNum", VatNum),
				new XElement("taxOffice", TaxOffice),
				new XElement("nonChargableAccount", Conversion.ToXmlString(NonChargableAccount)),
				new XElement("gender", Conversion.ToXmlString((int)Gender)),
				new XElement("dateOfBirth", Conversion.ToXmlString(DateOfBirth))
			);

			CustomFieldsToXML(xCustomer);

			if (Addresses != null && Addresses.Count > 0)
			{
				var xAddresses = new XElement("Addresses");
				foreach (var address in Addresses)
				{
					xAddresses.Add(address.ToXML(errorLogger));
				}
				xCustomer.Add(xAddresses);
			}

			return xCustomer;
		}
	}
}

using LSOne.DataLayer.BusinessObjects.Attributes;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.Utilities.Cryptography;
using LSOne.Utilities.DataTypes;
using LSOne.Utilities.ErrorHandling;
using System;
using System.Collections.Generic;
using System.Security;
using System.Xml.Linq;

namespace LSOne.DataLayer.BusinessObjects.Profiles
{
    public class SiteServiceProfile : DataEntity
    {
        public enum IssueGiftCardOptionEnum
        {
            ID = 0,
            Amount = 1,
            IDAndAmount = 2
        }

        public enum GiftCardRefillSettingEnum
        {
            AlwaysYes = 0,
            AlwaysNo = 1
        }

        public enum CashCustomerSettingEnum
        {
            AlwaysYes = 0,
            AlwaysNo = 1,
            YesAndSetOnPos = 2,
            NoAndSetOnPos = 3
        }

        public SiteServiceProfile(RecordIdentifier id, string text)
            : this()
        {
            ID = id;
            Text = text;
        }

        public SiteServiceProfile()
            : base()
        {
            UseAxTransactionServices = false;
            AosInstance = "";
            AosServer = "";
            AosPort = 0;
            SiteServiceAddress = "";
            SiteServicePortNumber = 0;
            Timeout = 60;
            MaxMessageSize = 55000000;
            UserName = "";
            Password = "";
            Company = "";
            Domain = "";
            AxVersion = -1;
            Configuration = "";
            Language = "";
            CheckCustomer = false;
            CheckStaff = false;
            UseInventoryLookup = false;
            IssueGiftCardOption = IssueGiftCardOptionEnum.ID;
            UseGiftCards = false;
            UseCentralSuspensions = false;
            UserConfirmation = true;
            UseCreditVouchers = false;
            NewCustomerDefaultTaxGroup = RecordIdentifier.Empty;
            NewCustomerDefaultTaxGroupName = "";
            CashCustomerSetting = CashCustomerSettingEnum.AlwaysYes;
            UseCentralReturns = false;
            SendReceiptEmails = ReceiptEmailOptionsEnum.OnRequest;
            EmailWindowsPrinterConfigurationID = RecordIdentifier.Empty;
            UseSerialNumbers = false;
            ProfileIsUsed = false;
            IFAuthToken = "";
            IFTcpPort = "";
            IFHttpPort = "";
            IFProtocols = "";
            IFSSLCertificateThumbnail = new SecureString();
        }

        public bool UseAxTransactionServices { get; set; }
        public int AXVersion { get; set; }
        public string ObjectServer { get; set; }
        public string AosInstance { get; set; }
        public string AosServer { get; set; }
        public int AosPort { get; set; }
        public string SiteServiceAddress { get; set; }
        public int SiteServicePortNumber { get; set; }
        /// <summary>
        /// The Site Service timeout in seconds. The default value is 60.
        /// </summary>
        public int Timeout { get; set; }
        /// <summary>
        /// The Site Service maximum message size in bytes. The default value is 55000000.
        /// </summary>
        public int MaxMessageSize { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Company { get; set; }
        public string Domain { get; set; }
        public int AxVersion { get; set; }
        public string Configuration { get; set; }
        public string Language { get; set; }
        public bool CheckCustomer { get; set; }
        public bool CheckStaff { get; set; }
        public bool UseInventoryLookup { get; set; }
        public IssueGiftCardOptionEnum IssueGiftCardOption { get; set; }
        public bool UseGiftCards { get; set; }
        public bool UseCentralSuspensions { get; set; }
        public bool UserConfirmation { get; set; }
        public bool CentralizedReturns { get; set; }
        public bool SalesOrders { get; set; }
        public bool SalesInvoices { get; set; }

        /// <summary>
        /// Indicates wether credit voucher functionality should be used on the POS
        /// </summary>
        public bool UseCreditVouchers { get; set; }

        /// <summary>
        /// If true then when returning a transaction the POS looks first for the receipt centrally before looking locally
        /// </summary>
        public bool UseCentralReturns { get; set; }

        public RecordIdentifier NewCustomerDefaultTaxGroup { get; set; }
        public string NewCustomerDefaultTaxGroupName { get; set; }
        public CashCustomerSettingEnum CashCustomerSetting { get; set; }
        public GiftCardRefillSettingEnum GiftCardRefillSetting { get; set; }
        public decimal MaximumGiftCardAmount { get; set; }
        public bool UseSerialNumbers { get; set; }
        public ReceiptEmailOptionsEnum SendReceiptEmails { get; set; }

        /// <summary>
        /// ID of the windows printer configuration to be used when sending emails
        /// </summary>
        public RecordIdentifier EmailWindowsPrinterConfigurationID { get; set; }

        /// <summary>
        /// Windows printer configuration for emails loaded at POS startup if <see cref="EmailWindowsPrinterConfigurationID"/> is set.
        /// </summary>
        public WindowsPrinterConfiguration EmailWindowsPrinterConfiguration { get; set; }

        public bool ProfileIsUsed { get; set; }

        public bool AllowCustomerManualID { get; set; }

        public decimal DefaultCreditLimit { get; set; }

        public string IFAuthToken { get; set; }
        public string IFTcpPort { get; set; }
        public string IFHttpPort { get; set; }
        public string IFProtocols { get; set; }
        public SecureString IFSSLCertificateThumbnail { get; set; }
        
        [CustomerMandatoryProperty(CustomerMandatoryPropertyTypeEnum.Name)]
        public bool CustomerNameIsMandatory { get; set; }
        [CustomerMandatoryProperty(CustomerMandatoryPropertyTypeEnum.SearchAlias)]
        public bool CustomerSearchAliasIsMandatory { get; set; }
        [CustomerMandatoryProperty(CustomerMandatoryPropertyTypeEnum.Address)]
        public bool CustomerAddressIsMandatory { get; set; }
        [CustomerMandatoryProperty(CustomerMandatoryPropertyTypeEnum.Phone)]
        public bool CustomerPhoneIsMandatory { get; set; }
        [CustomerMandatoryProperty(CustomerMandatoryPropertyTypeEnum.EmailAddress)]
        public bool CustomerEmailIsMandatory { get; set; }
        [CustomerMandatoryProperty(CustomerMandatoryPropertyTypeEnum.ReceiptEmailAddress)]
        public bool CustomerReceiptEmailIsMandatory { get; set; }
        [CustomerMandatoryProperty(CustomerMandatoryPropertyTypeEnum.Gender)]
        public bool CustomerGenderIsMandatory { get; set; }
        [CustomerMandatoryProperty(CustomerMandatoryPropertyTypeEnum.DateOfBirth)]
        public bool CustomerBirthDateIsMandatory { get; set; }

        /// <summary>
        /// The url of the KDS SOAP web service.
        /// </summary>
        public string KDSWebServiceUrl { get; set; }

        public override void ToClass(XElement element, IErrorLog errorLogger = null)
        {
            IEnumerable<XElement> elements = element.Elements();
            foreach (XElement current in elements)
            {
                if (!current.IsEmpty)
                {
                    try
                    {
                        switch (current.Name.ToString())
                        {
                            case "siteServiceProfileID":
                                ID = current.Value;
                                break;
                            case "siteServiceProfileName":
                                Text = current.Value;
                                break;
                            case "useAxTransactionServices":
                                UseAxTransactionServices = current.Value != "false";
                                break;
                            case "objectServer":
                                ObjectServer = current.Value;
                                break;
                            case "aosInstance":
                                AosInstance = current.Value;
                                break;
                            case "aosServer":
                                AosServer = current.Value;
                                break;
                            case "aosPort":
                                AosPort = Convert.ToInt32(current.Value);
                                break;
                            case "sCentralTableServer":
                                SiteServiceAddress = current.Value;
                                break;
                            case "sCentralTableServerPort":
                                SiteServicePortNumber = Convert.ToInt32(current.Value);
                                break;
                            case "timeout":
                                Timeout = Convert.ToInt32(current.Value);
                                break;
                            case "maxMessageSize":
                                MaxMessageSize = Convert.ToInt32(current.Value);
                                break;
                            case "userName":
                                UserName = current.Value;
                                break;
                            case "password":
                                Password = current.Value;
                                break;
                            case "company":
                                Company = current.Value;
                                break;
                            case "domain":
                                Domain = current.Value;
                                break;
                            case "axVersion":
                                AxVersion = Convert.ToInt32(current.Value);
                                break;
                            case "configuration":
                                Configuration = current.Value;
                                break;
                            case "language":
                                Language = current.Value;
                                break;
                            case "checkCustomer":
                                CheckCustomer = current.Value != "false";
                                break;
                            case "checkStaff":
                                CheckStaff = current.Value != "false";
                                break;
                            case "useInventoryLookup":
                                UseInventoryLookup = current.Value != "false";
                                break;
                            case "issueGiftCardOption":
                                IssueGiftCardOption = (IssueGiftCardOptionEnum)Convert.ToInt32(current.Value);
                                break;
                            case "useGiftCards":
                                UseGiftCards = current.Value != "false";
                                break;
                            case "useCentralSuspensions":
                                UseCentralSuspensions = current.Value != "false";
                                break;
                            case "userConfirmation":
                                UserConfirmation = current.Value != "false";
                                break;
                            case "centralizedReturns":
                                CentralizedReturns = current.Value != "false";
                                break;
                            case "salesOrders":
                                SalesOrders = current.Value != "false";
                                break;
                            case "salesInvoices":
                                SalesInvoices = current.Value != "false";
                                break;
                            case "useCreditVouchers":
                                UseCreditVouchers = current.Value != "false";
                                break;
                            case "newCustomerDefaultTaxGroup":
                                NewCustomerDefaultTaxGroup = current.Value;
                                break;
                            case "newCustomerDefaultTaxGroupName":
                                NewCustomerDefaultTaxGroupName = current.Value;
                                break;
                            case "cashCustomerSetting":
                                CashCustomerSetting = (CashCustomerSettingEnum)Convert.ToInt32(current.Value);
                                break;
                            case "useSerialNumbers":
                                UseSerialNumbers = current.Value != "false";
                                break;
                            case "SendReceiptEmails":
                                SendReceiptEmails = (ReceiptEmailOptionsEnum)Convert.ToInt32(current.Value);
                                break;
                            case "EmailWindowsPrinterConfigurationID":
                                EmailWindowsPrinterConfigurationID = current.Value;
                                break;
                            case "profileIsUsed":
                                ProfileIsUsed = current.Value != "false";
                                break;
                            case "allowCustomerManualID":
                                AllowCustomerManualID = current.Value != "false";
                                break;
                            case "customerDefaultCreditLimit":
                                DefaultCreditLimit = Convert.ToDecimal(current.Value);
                                break;
                            case "customerNameMandatory":
                                CustomerNameIsMandatory = current.Value != "false";
                                break;
                            case "customerSearchAliasMandatory":
                                CustomerSearchAliasIsMandatory = current.Value != "false";
                                break;
                            case "customerAddressMandatory":
                                CustomerAddressIsMandatory = current.Value != "false";
                                break;
                            case "customerPhoneMandatory":
                                CustomerPhoneIsMandatory = current.Value != "false";
                                break;
                            case "customerEmailMandatory":
                                CustomerEmailIsMandatory = current.Value != "false";
                                break;
                            case "customerReceiptEmailMandatory":
                                CustomerReceiptEmailIsMandatory = current.Value != "false";
                                break;
                            case "customerGenderMandatory":
                                CustomerGenderIsMandatory = current.Value != "false";
                                break;
                            case "customerBirthDateMandatory":
                                CustomerBirthDateIsMandatory = current.Value != "false";
                                break;
                            case "ifAuthToken":
                                IFAuthToken = current.Value;
                                break;
                            case "ifTcpPort":
                                IFTcpPort = current.Value;
                                break;
                            case "ifHttpPort":
                                IFHttpPort = current.Value;
                                break;
                            case "ifProtocols":
                                IFProtocols = current.Value;
                                break;
                            case "ifSSLThumbnail":
                                IFSSLCertificateThumbnail = SecureStringHelper.FromString(current.Value);
                                break;
                        }
                    }
                    catch (Exception ex)
                    {
                        if (errorLogger != null)
                        {
                            errorLogger.LogMessage(LogMessageType.Error, current.Name.ToString(), ex);
                        }
                    }
                }
            }
        }

        public override XElement ToXML(IErrorLog errorLogger = null)
        {
            XElement xml = new XElement("siteServiceProfile",
                new XElement("siteServiceProfileID", (string)ID),
                new XElement("siteServiceProfileName", Text),
                new XElement("useAxTransactionServices", UseAxTransactionServices),
                new XElement("objectServer", ObjectServer),
                new XElement("aosInstance", AosInstance),
                new XElement("aosServer", AosServer),
                new XElement("aosPort", AosPort),
                new XElement("sCentralTableServer", SiteServiceAddress),
                new XElement("sCentralTableServerPort", SiteServicePortNumber),
                new XElement("timeout", Timeout),
                new XElement("maxMessageSize", MaxMessageSize),
                new XElement("userName", UserName),
                new XElement("password", Password),
                new XElement("company", Company),
                new XElement("domain", Domain),
                new XElement("axVersion", AxVersion),
                new XElement("configuration", Configuration),
                new XElement("language", Language),
                new XElement("checkCustomer", CheckCustomer),
                new XElement("checkStaff", CheckStaff),
                new XElement("useInventoryLookup", UseInventoryLookup),
                new XElement("issueGiftCardOption", (int)IssueGiftCardOption),
                new XElement("useGiftCards", UseGiftCards),
                new XElement("useCentralSuspensions", UseCentralSuspensions),
                new XElement("userConfirmation", UserConfirmation),
                new XElement("centralizedReturns", CentralizedReturns),
                new XElement("salesOrders", SalesOrders),
                new XElement("salesInvoices", SalesInvoices),
                new XElement("useCreditVouchers", UseCreditVouchers),
                new XElement("newCustomerDefaultTaxGroup", NewCustomerDefaultTaxGroup),
                new XElement("newCustomerDefaultTaxGroupName", NewCustomerDefaultTaxGroupName),
                new XElement("SendReceiptEmails", (int)SendReceiptEmails),
                new XElement("EmailWindowsPrinterConfigurationID", EmailWindowsPrinterConfigurationID),
                new XElement("useSerialNumbers", UseSerialNumbers),
                new XElement("cashCustomerSetting", (int)CashCustomerSetting),
                new XElement("profileIsUsed", ProfileIsUsed),
                new XElement("allowCustomerManualID", AllowCustomerManualID),
                new XElement("customerDefaultCreditLimit", DefaultCreditLimit),
                new XElement("customerNameMandatory", CustomerNameIsMandatory),
                new XElement("customerSearchAliasMandatory", CustomerSearchAliasIsMandatory),
                new XElement("customerAddressMandatory", CustomerAddressIsMandatory),
                new XElement("customerPhoneMandatory", CustomerPhoneIsMandatory),
                new XElement("customerEmailMandatory", CustomerEmailIsMandatory),
                new XElement("customerReceiptEmailMandatory", CustomerReceiptEmailIsMandatory),
                new XElement("customerGenderMandatory", CustomerGenderIsMandatory),
                new XElement("customerBirthDateMandatory", CustomerBirthDateIsMandatory),
                new XElement("ifAuthToken", IFAuthToken),
                new XElement("ifTcpPort", IFTcpPort),
                new XElement("ifHttpPort", IFHttpPort),
                new XElement("ifProtocols", IFProtocols),
                new XElement("ifSSLThumbnail", SecureStringHelper.ToString(IFSSLCertificateThumbnail)));
            return xml;
        }
    }
}
using System;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Xml.Linq;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.Utilities.DataTypes;
using LSOne.Utilities.ErrorHandling;
using LSOne.Utilities.Validation;

namespace LSOne.DataLayer.BusinessObjects.StoreManagement
{
    public class Store : DataEntity
    {
        public enum CalculateDiscountsFromEnum
        {
            PriceWithTax = 0,
            Price = 1
        }

        public enum StorePriceSettingsEnum
        {
            UsePriceGroupSettings,
            PricesIncludeTax,
            PricesExcludeTax
        }

        public Store() : this(RecordIdentifier.Empty, "", Address.AddressFormatEnum.GenericWithState)
        {
        }

        public Store(RecordIdentifier storeID, string description, Address.AddressFormatEnum defaultFormat )
            : base(storeID, description)
        {
            FunctionalityProfile = "";
            LanguageCode = "";
            TaxGroup = "";
            Currency = "";
            BackupDatabaseName = "";
            BackupDatabaseWindowsAuthentication = false;
            BackupDatabaseServer = "";
            BackupDatabaseUser = "";
            BackupDatabasePassword = "";
            UseDefaultCustomerAccount = false;
            DefaultCustomerAccount = "";
            KeyedInPriceIncludesTax = true;
            FunctionalityProfileDescription = "";
            LayoutID = "";
            DisplayAmountsWithTax = true;
            DisplayBalanceWithTax = true;
            UseTaxRounding = false;
            UseTaxGroupFrom = UseTaxGroupFromEnum.Customer;
            TransactionServiceProfileID = "";
            TransactionServiceProfileName = "";
            StorePriceSetting = StorePriceSettingsEnum.PricesIncludeTax;
            //StatementMethod = StatementGroupingMethod.POSTerminal;

            Address = new Address(defaultFormat);
            StyleProfile = "";

            KitchenServiceProfileID = Guid.Empty;
            KitchenServiceProfileName = "";
            KeyboardCode = "";
            KeyboardLayoutName = "";
            FormProfileID = Guid.Empty;
            EmailFormProfileID = Guid.Empty;
            FormProfileDescription = "";
            EmailFormProfileDescription = "";

            FormInfoField1 = "";
            FormInfoField2 = "";
            FormInfoField3 = "";
            FormInfoField4 = "";

            BarcodeSymbology = BarcodeType.Code39;
            OperationAuditSetting = OperationAuditEnum.Never;

            StartAmount = 0M;

            ReturnsPrintedTwice = false; 
            TenderReceiptAreReprinted = true;
            ReturnReasonCodeID = RecordIdentifier.Empty;
            RegionID = RecordIdentifier.Empty;

            PictureID = RecordIdentifier.Empty;
            LogoSize = StoreLogoSizeType.Normal;
        }

        [RecordIdentifierValidation(20)]
        public override RecordIdentifier  ID
        {

	        get 
	        { 
		         return base.ID;
	        }
	        set 
	        { 
		        base.ID = value;
	        }
        }
        [StringLength(60)]
        public override string  Text
        {
	        get 
	        { 
		         return base.Text;
	        }
	        set 
	        { 
		        base.Text = value;
	        }
        }

        public Address Address { get; set; }
        [StringLength(60)]
        public string FunctionalityProfile { get; set; }

        /// <summary>
        /// This field contains the default currency for the store. It is used for example when getting the item prices.
        /// </summary>
        [StringLength(3)]
        public string Currency { get; set; }
        [StringLength(30)]
        public string CurrencyDescription { get; set; }

        /// <summary>
        /// Stores the culture info for the store.  en-US for English(United Stated),is-IS for Iceland, etc.
        /// </summary>
        [StringLength(20)]
        public string LanguageCode { get; set; }
        [StringLength(20)]
        public string TaxGroup { get; set; }
        [StringLength(60)]
        public string TaxGroupName { get; set; }
        [StringLength(50)]
        public string BackupDatabaseName { get; set; }
        [StringLength(50)]
        public string BackupDatabaseServer { get; set; }
        public bool BackupDatabaseWindowsAuthentication { get; set; }
        [StringLength(50)]
        public string BackupDatabaseUser { get; set; }
        [StringLength(50)]
        public string BackupDatabasePassword { get; set; }
        [RecordIdentifierValidation(20)]
        public RecordIdentifier DefaultCustomerAccount { get; set; }
        [StringLength(20)]
        public string DefaultCustomerAccountDescription { get; set; }
        public bool UseDefaultCustomerAccount { get; set; }
        /// <summary>
        /// This is used for price override and when it is required to key in an item price.
        /// When true, then the value entered contains tax such that tax will not be added on top of this value.
        /// </summary>
        public bool KeyedInPriceIncludesTax { get; set; }
        [StringLength(60)]
        public string FunctionalityProfileDescription { get; set; }
        [RecordIdentifierValidation(20)]
        public RecordIdentifier LayoutID { get; set; }
        [StringLength(50)]
        public string LayoutDescription { get; set; }
        /// <summary>
        /// This field specifies the statement method of your business, that is, how Statement Line in statements are divided.
        /// There are 3 options:
        /// Total: The program creates a statement line for each tender type in the transactions.
        /// Staff: For each staff member, the program creates a statement line for each tender type in transactions transacted by the Staff.
        /// POS Terminal: For each POS Terminal, the program creates a statement line for each tender type in transactions transacted on the POS terminal.
        /// You can use the Staff/POS Terminal Filter in the Statement table to select to see the turnover for a particular POS terminal or staff member if the Statement Method is POS Terminal or Staff respectively. 
        /// </summary>
        //public StatementGroupingMethod StatementMethod { get; set; }
        public TenderDeclarationCalculation TenderDeclarationCalculation { get; set; }
        public decimal MaximumPostingDifference { get; set; }
        public decimal MaximumTransactionDifference { get; set; }
        public CalculateDiscountsFromEnum CalculateDiscountsFrom { get; set; }     
        public bool DisplayAmountsWithTax { get; set; }
        public bool DisplayBalanceWithTax { get; set; }
        public bool UseTaxRounding { get; set; }
        public UseTaxGroupFromEnum UseTaxGroupFrom { get; set; }
        [StringLength(20)]
        public string TransactionServiceProfileID { get; set;}
        [StringLength(60)]
        public string TransactionServiceProfileName { get; set; }
        public bool AllowEOD { get; set; }
        public StorePriceSettingsEnum StorePriceSetting { get; set; }
        public string StyleProfile { get; set; }

        [StringLength(20)]
        public string KeyboardCode { get; set; }
        [StringLength(50)]
        public string KeyboardLayoutName { get; set; }

        public RecordIdentifier KitchenServiceProfileID { get; set; }
        [StringLength(60)]
        public string KitchenServiceProfileName { get; set; }

        public RecordIdentifier FormProfileID { get; set; }
        public RecordIdentifier EmailFormProfileID { get; set; }
        public string FormProfileDescription { get; set; }
        public string EmailFormProfileDescription { get; set; }
        public RecordIdentifier ReturnReasonCodeID { get; set; }
        public RecordIdentifier PictureID { get; set; }

        /// <summary>
        /// Generic information field that can be printed on a form
        /// </summary>
        [StringLength(60)]
        public string FormInfoField1 { get; set; }
        /// <summary>
        /// Generic information field that can be printed on a form
        /// </summary>
        [StringLength(60)]
        public string FormInfoField2 { get; set; }
        /// <summary>
        /// Generic information field that can be printed on a form
        /// </summary>
        [StringLength(60)]
        public string FormInfoField3 { get; set; }
        /// <summary>
        /// Generic information field that can be printed on a form
        /// </summary>
        [StringLength(60)]
        public string FormInfoField4 { get; set; }

        public BarcodeType BarcodeSymbology { get; set; }

        public OperationAuditEnum OperationAuditSetting { get; set; }

        public string AddressFormatted { get; set; }

        public decimal StartAmount { get; set; }

        public bool ReturnsPrintedTwice { get; set; }

        public bool TenderReceiptAreReprinted { get; set; }

        /// <summary>
        /// Logo size to be used by WinPrinter.
        /// </summary>
        public StoreLogoSizeType LogoSize { get; set; }

        public RecordIdentifier RegionID { get; set; }
        public string RegionDescription { get; set; }

        /// <summary>
        /// The ID of the main menu header used for the inventory app
        /// </summary>
        public string InventoryMainMenuID { get; set; }

        public int StoreTransferDefaultDeliveryTime { get; set; }
        public DeliveryDaysTypeEnum StoreTransferDeliveryDaysType { get; set; }

        public DateTime StoreTransferExpectedDeliveryDate()
        {
            return StoreTransferDeliveryDaysType == DeliveryDaysTypeEnum.Days
                   ? DateTime.Now.AddDays(StoreTransferDefaultDeliveryTime)
                   : DateTime.Now.AddBusinessDays(StoreTransferDefaultDeliveryTime);
        }

        /// <summary>
        /// Sets all variables in the BusinessTemplate class with the values in the xml
        /// </summary>
        /// <param name="xStoreSetting">The xml element with the business template values</param>
        /// <param name="errorLogger"></param>
        public override void ToClass(XElement xStoreSetting, IErrorLog errorLogger = null)
        {
            if (!xStoreSetting.HasElements)
            {
                return;
            }
            var storeVariables = xStoreSetting.Elements();
            foreach (XElement storeElem in storeVariables)
            {
                if (storeElem.IsEmpty)
                {
                    continue;
                }
                try
                {
                    switch (storeElem.Name.ToString())
                    {
                        case "storeID":
                            ID = storeElem.Value;
                            break;
                        case "storeName":
                            Text = storeElem.Value;
                            break;
                        case "storeAddress":
                            Address.Address2 = storeElem.Value;
                            break;
                        case "street":
                            Address.Address1 = storeElem.Value;
                            break;
                        case "zipCode":
                            Address.Zip = storeElem.Value;
                            break;
                        case "city":
                            Address.City = storeElem.Value;
                            break;
                        case "country":
                            Address.Country = storeElem.Value;
                            break;
                        case "state":
                            Address.State = storeElem.Value;
                            break;
                        case "currency":
                            Currency = storeElem.Value;
                            break;
                        case "maximumPostingDifference":
                            MaximumPostingDifference = Convert.ToDecimal(storeElem.Value, XmlCulture);
                            break;
                        case "maxTransactionDifferenceAmount":
                            MaximumTransactionDifference = Convert.ToDecimal(storeElem.Value, XmlCulture);
                            break;
                        case "functionalityProfile":
                            FunctionalityProfile = storeElem.Value;
                            break;
                        case "tenderDeclarationCalculation":
                            TenderDeclarationCalculation =
                                (TenderDeclarationCalculation) Convert.ToInt32(storeElem.Value);
                            break;
                        case "taxGroup":
                            TaxGroup = storeElem.Value;
                            break;
                        case "cultureName":
                            LanguageCode = storeElem.Value;
                            break;
                        case "layoutID":
                            LayoutID = storeElem.Value;
                            break;
                        case "sqlServerName":
                            BackupDatabaseServer = storeElem.Value;
                            break;
                        case "dataBaseName":
                            BackupDatabaseName = storeElem.Value;
                            break;
                        case "userName":
                            BackupDatabaseUser = storeElem.Value;
                            break;
                        case "password":
                            BackupDatabasePassword = storeElem.Value;
                            break;
                        case "defaultCustAccount":
                            DefaultCustomerAccount = storeElem.Value;
                            break;
                        case "useDefaultCustAccount":
                            UseDefaultCustomerAccount = storeElem.Value != "false";
                            break;
                        case "windowsAuthentication":
                            BackupDatabaseWindowsAuthentication = Convert.ToBoolean(storeElem.Value);
                            break;
                        case "addressFormate":
                            Address.AddressFormat = (Address.AddressFormatEnum) Convert.ToInt32(storeElem.Value);
                            break;
                        case "keyedInPriceContainsTax":
                            KeyedInPriceIncludesTax = storeElem.Value != "false";
                            break;
                        case "calcDiscFrom":
                            CalculateDiscountsFrom =
                                (Store.CalculateDiscountsFromEnum) Convert.ToInt32(storeElem.Value);
                            break;
                        case "displayAmountsWithTax":
                            DisplayAmountsWithTax = storeElem.Value != "false";
                            break;
                        case "useTaxGroupFrom":
                            UseTaxGroupFrom = (UseTaxGroupFromEnum) Convert.ToInt32(storeElem.Value);
                            break;
                        case "suspendAllowEOD":
                            AllowEOD = Convert.ToInt32(storeElem.Value) == 2;
                            break;
                        case "storePriceSetting":
                            StorePriceSetting = (Store.StorePriceSettingsEnum) (Convert.ToInt32(storeElem.Value));
                            break;
                        case "transactionServiceProfile":
                            TransactionServiceProfileID = storeElem.Value;
                            break;
                        case "styleProfile":
                            StyleProfile = storeElem.Value;
                            break;
                        case "keyboardCode":
                            KeyboardCode = storeElem.Value;
                            break;
                        case "keyboardLayoutName":
                            KeyboardLayoutName = storeElem.Value;
                            break;
                        case "kitchenManagerProfileID":
                            KitchenServiceProfileID = new Guid(storeElem.Value);
                            break;
                        case "receiptProfileID":
                            FormProfileID = new Guid(storeElem.Value);
                            break;
                        case "EmailFormProfileID":
                            EmailFormProfileID = new Guid(storeElem.Value);
                            break;
                        case "pictureID":
                            PictureID = storeElem.Value;
                            break;
                        case "formInfoField1":
                            FormInfoField1 = storeElem.Value;
                            break;
                        case "formInfoField2":
                            FormInfoField2 = storeElem.Value;
                            break;
                        case "formInfoField3":
                            FormInfoField3 = storeElem.Value;
                            break;
                        case "formInfoField4":
                            FormInfoField4 = storeElem.Value;
                            break;
                        case "barcodeSymbology":
                            BarcodeSymbology = (BarcodeType)Convert.ToInt32(storeElem.Value);
                            break;
                        case "operationAuditSetting":
                            OperationAuditSetting = (OperationAuditEnum) Convert.ToInt32(storeElem.Value);
                            break;

                        case "startAmount":
                            StartAmount = Convert.ToDecimal(storeElem.Value);
                            break;
                        case "returnsPrintedTwice":
                            ReturnsPrintedTwice = storeElem.Value != "false";
                            break;
                        case "logoSize":
                            LogoSize = (StoreLogoSizeType)Convert.ToByte(storeElem.Value);
                            break;
                        case "returnReasonCodeID":
                            ReturnReasonCodeID = storeElem.Value;
                            break;
                        case "regionID":
                            RegionID = storeElem.Value;
                            break;
                        case "storeTransferDefaultDeliveryTime":
                            StoreTransferDefaultDeliveryTime = Convert.ToInt32(storeElem.Value);
                            break;
                        case "storeTransferDeliveryDaysType":
                            StoreTransferDeliveryDaysType = (DeliveryDaysTypeEnum)Convert.ToByte(storeElem.Value);
                            break;
                    }
                }
                catch (Exception ex)
                {
                    if (errorLogger != null)
                    {
                        errorLogger.LogMessage(LogMessageType.Error,
                                               storeElem.Name.ToString(), ex);
                    }
                }
            }
        }

        /// <summary>
        /// Creates an xml element from all the variables in the StoreSetting class
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
            var xStoreSetting = new XElement("store",
                new XElement("storeID", (string)ID),
                new XElement("storeName", Text),
                //new XElement("dataAreaID", store.DataAreaID),
                new XElement("storeAddress", Address.Address2),
                new XElement("street", Address.Address1),
                new XElement("zipCode", Address.Zip),
                new XElement("city", Address.City),
                new XElement("country", Address.Country),
                new XElement("state", Address.State),
                new XElement("currency", Currency),
                new XElement("maximumPostingDifference", MaximumPostingDifference.ToString(XmlCulture)),
                new XElement("maxTransactionDifferenceAmount", MaximumTransactionDifference.ToString(XmlCulture)),
                new XElement("functionalityProfile", FunctionalityProfile),
                new XElement("tenderDeclarationCalculation", (int)TenderDeclarationCalculation),
                new XElement("taxGroup", TaxGroup),
                new XElement("cultureName", LanguageCode),
                new XElement("layoutID", LayoutID),
                new XElement("sqlServerName", BackupDatabaseServer),
                new XElement("dataBaseName", BackupDatabaseName),
                new XElement("userName", BackupDatabaseUser),
                new XElement("password", BackupDatabasePassword),
                new XElement("defaultCustAccount", DefaultCustomerAccount),
                new XElement("useDefaultCustAccount", UseDefaultCustomerAccount),
                new XElement("windowsAuthentication", BackupDatabaseWindowsAuthentication),
                new XElement("addressFormate", (int)Address.AddressFormat),
                new XElement("keyedInPriceContainsTax", KeyedInPriceIncludesTax),
                new XElement("calcDiscFrom", (int)CalculateDiscountsFrom),
                new XElement("displayAmountsWithTax", DisplayAmountsWithTax),
                new XElement("useTaxGroupFrom", (int)UseTaxGroupFrom),
                new XElement("suspendAllowEOD", AllowEOD ? 2 : 3),
                new XElement("storePriceSetting", (int)StorePriceSetting),
                new XElement("transactionServiceProfile", TransactionServiceProfileID),
                new XElement("styleProfile", StyleProfile),
                new XElement("keyboardCode", KeyboardCode),
                new XElement("keyboardLayoutName", KeyboardLayoutName),
                new XElement("kitchenManagerProfileID", KitchenServiceProfileID),
                new XElement("receiptProfileID", FormProfileID),
                new XElement("EmailFormProfileID", EmailFormProfileID),
                new XElement("pictureID", PictureID),
                new XElement("formInfoField1", FormInfoField1),
                new XElement("formInfoField2", FormInfoField2),
                new XElement("formInfoField3", FormInfoField3),
                new XElement("formInfoField4", FormInfoField4),
                new XElement("barcodeSymbology", (int)BarcodeSymbology),
                new XElement("operationAuditSetting", (int)OperationAuditSetting,
                new XElement("startAmount", StartAmount),
                new XElement("returnsPrintedTwice", ReturnsPrintedTwice)),
                new XElement("logoSize", ((byte)LogoSize).ToString()),
                new XElement("returnReasonCodeID", ReturnReasonCodeID),
                new XElement("regionID", RegionID),
                new XElement("storeTransferDefaultDeliveryTime", StoreTransferDefaultDeliveryTime),
                new XElement("storeTransferDeliveryDaysType", (int)StoreTransferDeliveryDaysType)
            );
            return xStoreSetting;
        }
    }
}

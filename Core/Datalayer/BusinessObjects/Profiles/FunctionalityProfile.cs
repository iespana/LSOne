using System;
using System.Drawing;
using System.Xml.Linq;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.EOD;
using LSOne.DataLayer.BusinessObjects.Omni;
using LSOne.DataLayer.BusinessObjects.Replenishment;
using LSOne.Utilities.DataTypes;
using LSOne.Utilities.Development;
using LSOne.Utilities.ErrorHandling;

namespace LSOne.DataLayer.BusinessObjects.Profiles
{
    public class FunctionalityProfile : DataEntity
    {
        public enum LogTraceLevel
        {
            Trace = 1,
            Debug = 2,
            Error = 3,
        };

        public enum SettingsClear
        {
            None = 0,
            AtLogOff = 1,
            AfterLogOff = 2
        };

        public enum StartOfTransactionTypes
        {
            Normal = 0,
            OnlyOnItemSale = 1
        }

        public enum LimitationDisplayType
        {
            ItemsIncluded = 0,
            ItemsExcluded = 1,
            NotDisplayed = 2
        }

        public string PriceDecimalPlaces { get; set; }

        public bool SyncTransactions { get; set; }

        public decimal MaximumQTY { get; set; }

        public decimal MaximumPrice { get; set; }

        public int PollingInterval { get; set; }

        /// <summary>
        /// used in combination with the numpad; tells how many decimals it has
        /// </summary>
        public int DecimalsInNumpad { get; set; }

        /// <summary>
        /// A flag saying whether the number that is to be entered into the numpad starts with decimals. 
        /// </summary>
        public bool EntryStartsInDecimals { get; set; }

        // infocode settings

        public bool IsHospitality { get; set; }
        public bool SkipHospitalityTableView { get; set; }
        public bool AllowItemChangesAfterSplitBill { get; set; }

        public bool SafeDropUsesDenomination { get; set; }
        public bool SafeDropRevUsesDenomination { get; set; }
        public bool BankDropUsesDenomination { get; set; }
        public bool BankDropRevUsesDenomination { get; set; }
        public bool TenderDeclUsesDenomination { get; set; }
        public bool CustomerRequiredOnReturn { get; set; }
        public bool KeepDailyJournalOpenAfterPrintingReceipt { get; set; }
        public ZReportConfig ZReportConfig { get; set; }

        /// <summary>
        /// If true, voided items will be displayed in the receipt control in the POS
        /// </summary>
        public bool DisplayVoidedItems { get; set; }

        /// <summary>
        /// If true, voided payments will be displayed in the receipt control in the POS
        /// </summary>
        public bool DisplayVoidedPayments { get; set; }

        /// <summary>
        /// If true, image view will be added to item lookup screen
        /// </summary>
        public bool AllowImageViewInItemLookup { get; set; }

        /// <summary>
        /// If AllowImageViewInItemLookup is true, then this flag controls whether the POS will remember the last used option
        /// </summary>
        public bool RememberListImageSelection { get; set; }

        public Image DefaultItemImage { get; set; }

        /// <summary>
        /// If it is true, and a sold item is the same as the last item the quantiy will increase, instead of add adding a new item line.
        /// </summary>
        public AggregateItemsModes AggregateItems { get; set; }

        /// <summary>
        /// If it is true, then payments like cash will be aggregated
        /// </summary>
        public bool AggregatePayments { get; set; }

        public bool TsCentralTableServer { get; set; }

        public string CentralTableServer { get; set; }

        public int CentralTableServerPort { get; set; }

        public bool TsCustomer { get; set; }

        public bool TsStaff { get; set; }

        public bool TsSuspendRetrieveTransactions { get; set; }

        public LogTraceLevel LogLevel { get; set; }

        /// <summary>
        /// If it is true, and a sold item is the same as the last item the quantiy will increase, instead of add adding a new item line.
        /// </summary>
        public bool AggregateItemsForPrinting { get; set; }

        /// <summary>
        /// If set to true then the application will display the login screen in the form of a list of users to select from.
        /// If set to false then the application prompt the user for both the userid and password, through a numpad.
        /// </summary>
        public bool ShowStaffListAtLogon { get; set; }

        /// <summary>
        /// Only applies if ShowStaffListAtLogon is true: If set to true then the user list will be limited to store.        
        /// </summary>
        public bool LimitStaffListToStore { get; set; }

        /// <summary>
        /// If set to true it indicates that the terminal will allow new sales transactions to commence, even if the till drawer is open from the
        /// previous transaction.
        /// </summary>
        public bool AllowSalesIfDrawerIsOpen { get; set; }

        public bool StaffBarcodeLogon { get; set; }

        /// <summary>
        /// If this field is true then it indicates that a cashier (staff member) must log into the POS terminals, by using a card. 
        /// </summary>
        public bool StaffCardLogon { get; set; }

        /// <summary>
        /// If this field is true then it indicates that the cashier must key in the price of an item, if the item price is zero. 
        /// If the Zero Price Valid field is checkmarked for the item, however, then the cashier is not to key in the price. 
        /// </summary>
        public bool MustKeyInPriceIfZero { get; set; }

        /// <summary>
        /// If this field is true then the logon form will not remember the last user when a Logoff is performed on the POS
        /// </summary>
        public bool ClearUserBetweenLogins { get; set; }

        /// <summary>
        /// Setting for how to display limitation payments in the POS
        /// </summary>
        public LimitationDisplayType DialogLimitationDisplayType { get; set; }

        /// <summary>
        /// If true, limitation totals are displayd in POS
        /// </summary>
        public bool DisplayLimitationsTotalsInPOS { get; set; }

        /// <summary>
        /// Infocode which is activated at the start of transaction
        /// </summary>
        public RecordIdentifier InfocodeStartOfTransaction { get; set; }

        /// <summary>
        /// Infocode which is activated at the end of transaction
        /// </summary>
        public StartOfTransactionTypes InfocodeStartOfTransactionType { get; set; }

        /// <summary>
        /// Infocode which is activated at the end of transaction
        /// </summary>
        public RecordIdentifier InfocodeEndOfTransaction { get; set; }

        /// <summary>
        /// Infocode which is activated at tender declaration
        /// </summary>
        public RecordIdentifier InfocodeTenderDecl { get; set; }

        /// <summary>
        /// Infocode which is activated when item is not found
        /// </summary>
        public RecordIdentifier InfocodeItemNotFound { get; set; }

        /// <summary>
        /// Infocode which is activated when a line discount is issued.
        /// </summary>
        public RecordIdentifier InfocodeItemDiscount { get; set; }

        /// <summary>
        /// Infocode which is activated when total discount is given
        /// </summary>
        public RecordIdentifier InfocodeTotalDiscount { get; set; }

        /// <summary>
        /// Infocode which is activated when item price is overriden
        /// </summary>
        public RecordIdentifier InfocodePriceOverride { get; set; }

        /// <summary>
        /// Infocode which is activated when a sales line has a negative value.
        /// </summary>
        public RecordIdentifier InfocodeReturnItem { get; set; }

        /// <summary>
        /// Infocode which is activated when a sale is refunded
        /// </summary>
        public RecordIdentifier InfocodeReturnTransaction { get; set; }

        /// <summary>
        /// Infocode which is activated when the Void operation is started
        /// </summary>
        public RecordIdentifier InfocodeVoidItem { get; set; }

        /// <summary>
        /// Infocode which is activated when a payment is voided
        /// </summary>
        public RecordIdentifier InfocodeVoidPayment { get; set; }

        /// <summary>
        /// Infocode which is activated when the transaction is voided
        /// </summary>
        public RecordIdentifier InfocodeVoidTransaction { get; set; }

        /// <summary>
        /// Infocode which is activated when sales person is selected
        /// </summary>
        public RecordIdentifier InfocodeAddSalesPerson { get; set; }

        /// <summary>
        /// Infocode which is activated when sales person is selected
        /// </summary>
        public RecordIdentifier InfocodeOpenDrawer { get; set; }

        public SettingsClear POSSettingsClear { get; set; }

        public int SettingsClearGracePeriod{ get; set; }

        public bool AllowSaleAndReturnInSameTransaction { get; set; }

        public string PostTransactionDDJob { get; set; }

        public string DDSchedulerLocation { get; set; }

        public bool UseStartOfDay { get; set;  }

        /// <summary>
        /// Setting which controls how a sales person is set on a transaction
        /// </summary>
        public SalesPersonPrompt SalesPersonPrompt { get; set; }

        /// <summary>
        /// True if the profile is used by a store or terminal
        /// </summary>
        public bool ProfileIsUsed { get; set; }
        
        /// <summary>
        /// If true, prices will be shown by default when searching for items in the POS
        /// </summary>
        public bool ShowPricesByDefault { get; set; }

        /// <summary>
        /// If true, the item ID will be displayed in the retun item popup dialog
        /// </summary>
        public bool DisplayItemIDInReturnDialog { get; set; }
        
        /// <summary>
        /// LS Commerce settings related to functionality profile.
        /// </summary>
        [LSOneUsage(CodeUsage.LSCommerce)]
        public OmniFunctionalityProfile OmniProfile { get; set; }

        /// <summary>
        /// Initializes a new <see cref="FunctionalityProfile"/> object.
        /// </summary>
        public FunctionalityProfile()
        {
            CommonInit();
        }
        
        /// <summary>
        /// Initializes a new <see cref="FunctionalityProfile"/> object with the given id and name.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="text"></param>
        public FunctionalityProfile(RecordIdentifier id, string text)
            : base(id, text)
        {
            CommonInit();
        }

        private void CommonInit()
        {
            AggregateItems = 0;
            AggregatePayments = false;
            TsCentralTableServer = false;
            CentralTableServer = string.Empty;
            CentralTableServerPort = 0;
            TsCustomer = false;
            TsStaff = false;
            TsSuspendRetrieveTransactions = false;
            LogLevel = LogTraceLevel.Error;
            AggregateItemsForPrinting = false;
            ShowStaffListAtLogon = false;
            LimitStaffListToStore = false;
            AllowSalesIfDrawerIsOpen = false;
            IsHospitality = false;
            SkipHospitalityTableView = false;
            AllowItemChangesAfterSplitBill = false;
            InfocodeTenderDecl = string.Empty;
            InfocodeItemNotFound = string.Empty;
            InfocodeItemDiscount = string.Empty;
            InfocodeTotalDiscount = string.Empty;
            InfocodePriceOverride = string.Empty;
            InfocodeReturnItem = string.Empty;
            InfocodeReturnTransaction = string.Empty;
            InfocodeVoidItem = string.Empty;
            InfocodeVoidPayment = string.Empty;
            InfocodeVoidTransaction = string.Empty;
            InfocodeAddSalesPerson = string.Empty;
            InfocodeOpenDrawer = string.Empty;
            InfocodeEndOfTransaction = string.Empty;
            InfocodeStartOfTransaction = string.Empty;
            EntryStartsInDecimals = false;
            DecimalsInNumpad = 3;
            SafeDropUsesDenomination = true;
            SafeDropRevUsesDenomination = true;
            BankDropUsesDenomination = true;
            BankDropRevUsesDenomination = true;
            TenderDeclUsesDenomination = true;
            CustomerRequiredOnReturn = false;
            KeepDailyJournalOpenAfterPrintingReceipt = false;
            PollingInterval = 0;
            MaximumPrice = 0;
            MaximumQTY = 0;
            SyncTransactions = false;
            PriceDecimalPlaces = string.Empty;
            DisplayVoidedItems = true;
            DisplayVoidedPayments = true;
            AllowImageViewInItemLookup = false;
            DefaultItemImage = null;
            UseStartOfDay = false;
            SalesPersonPrompt = SalesPersonPrompt.None;
            ZReportConfig = new ZReportConfig();
            ProfileIsUsed = false;
            ShowPricesByDefault = false;
            DisplayItemIDInReturnDialog = false;
            
            OmniProfile = new OmniFunctionalityProfile();
        }

        public override void ToClass(XElement element, IErrorLog errorLogger = null)
        {
            var elements = element.Elements();
            foreach (XElement current in elements)
            {
                if (!current.IsEmpty)
                {
                    try
                    {
                        switch (current.Name.ToString())
                        {
                            case "functionalityProfileID":
                                ID = current.Value;
                                break;
                            case "functionalityProfileName":
                                Text = current.Value;
                                break;
                            case "aggregateItems":
                                AggregateItems = (AggregateItemsModes) Conversion.XmlStringToInt(current.Value);
                                break;
                            case "aggregatePayments":
                                AggregatePayments = Conversion.XmlStringToBool(current.Value);
                                break;
                            case "tsCentralTableServer":
                                TsCentralTableServer = Conversion.XmlStringToBool(current.Value);
                                break;
                            case "fCentralTableServer":
                                CentralTableServer = current.Value;
                                break;
                            case "fCentralTableServerPort":
                                CentralTableServerPort = Conversion.XmlStringToInt(current.Value);
                                break;
                            case "tsCustomer":
                                TsCustomer = Conversion.XmlStringToBool(current.Value);
                                break;
                            case "tsStaff":
                                TsStaff = Conversion.XmlStringToBool(current.Value);
                                break;
                            case "tsSuspendRetrieveTransactions":
                                TsSuspendRetrieveTransactions = Conversion.XmlStringToBool(current.Value);
                                break;
                            case "logLevel":
                                LogLevel = (LogTraceLevel) Conversion.XmlStringToInt(current.Value);
                                break;
                            case "aggregateItemsForPrinting":
                                AggregateItemsForPrinting = Conversion.XmlStringToBool(current.Value);
                                break;
                            case "showStaffListAtLogon":
                                ShowStaffListAtLogon = Conversion.XmlStringToBool(current.Value);
                                break;
                            case "limitStaffListToStore":
                                LimitStaffListToStore = Conversion.XmlStringToBool(current.Value);
                                break;
                            case "allowSalesIfDrawerIsOpen":
                                AllowSalesIfDrawerIsOpen = Conversion.XmlStringToBool(current.Value);
                                break;
                            case "staffBarcodeLogon":
                                StaffBarcodeLogon = Conversion.XmlStringToBool(current.Value);
                                break;
                            case "staffCardLogon":
                                StaffCardLogon = Conversion.XmlStringToBool(current.Value);
                                break;
                            case "mustKeyInPriceIfZero":
                                MustKeyInPriceIfZero = Conversion.XmlStringToBool(current.Value);
                                break;
                            case "clearUserBetweenLogins":
                                ClearUserBetweenLogins = Conversion.XmlStringToBool(current.Value);
                                break;
                            case "displayLimitationsTotalsInPOS":
                                DisplayLimitationsTotalsInPOS = Conversion.XmlStringToBool(current.Value);
                                break;
                            case "infocodeStartOfTransaction":
                                InfocodeStartOfTransaction = current.Value;
                                break;
                            case "infocodeEndOfTransaction":
                                InfocodeEndOfTransaction = current.Value;
                                break;
                            case "infocodeTenderDecl":
                                InfocodeTenderDecl = current.Value;
                                break;
                            case "infocodeItemNotFound":
                                InfocodeItemNotFound = current.Value;
                                break;
                            case "infocodeItemDiscount":
                                InfocodeItemDiscount = current.Value;
                                break;
                            case "infocodeTotalDiscount":
                                InfocodeTotalDiscount = current.Value;
                                break;
                            case "infocodePriceOverride":
                                InfocodePriceOverride = current.Value;
                                break;
                            case "infocodeReturnItem":
                                InfocodeReturnItem = current.Value;
                                break;
                            case "infocodeReturnTransaction":
                                InfocodeReturnTransaction = current.Value;
                                break;
                            case "infocodeVoidItem":
                                InfocodeVoidItem = current.Value;
                                break;
                            case "infocodeVoidPayment":
                                InfocodeVoidPayment = current.Value;
                                break;
                            case "infocodeVoidTransaction":
                                InfocodeVoidTransaction = current.Value;
                                break;
                            case "infocodeAddSalesPerson":
                                InfocodeAddSalesPerson = current.Value;
                                break;
                            case "infocodeOpenDrawer":
                                InfocodeOpenDrawer = current.Value;
                                break;
                            case "priceDecimalPlaces":
                                PriceDecimalPlaces = current.Value;
                                break;
                            case "syncTransactions":
                                SyncTransactions = Conversion.XmlStringToBool(current.Value);
                                break;
                            case "maximumQTY":
                                MaximumQTY = Conversion.XmlStringToDecimal(current.Value);
                                break;
                            case "maximumPrice":
                                MaximumPrice = Conversion.XmlStringToDecimal(current.Value);
                                break;
                            case "pollingInterval":
                                PollingInterval = Conversion.XmlStringToInt(current.Value);
                                break;
                            case "decimalsInNumpad":
                                DecimalsInNumpad = Conversion.XmlStringToInt(current.Value);
                                break;
                            case "entryStartsInDecimals":
                                EntryStartsInDecimals = Conversion.XmlStringToBool(current.Value);
                                break;
                            case "isHospitality":
                                IsHospitality = Conversion.XmlStringToBool(current.Value);
                                break;
                            case "skipHospitalityTableView":
                                SkipHospitalityTableView = Conversion.XmlStringToBool(current.Value);
                                break;
                            case "AllowVoidAfterSplitBill":
                                AllowItemChangesAfterSplitBill = Conversion.XmlStringToBool(current.Value);
                                break;
                            case "safeDropUsesDenomination":
                                SafeDropUsesDenomination = Conversion.XmlStringToBool(current.Value);
                                break;
                            case "safeDropRevUsesDenomination":
                                SafeDropRevUsesDenomination = Conversion.XmlStringToBool(current.Value);
                                break;
                            case "bankDropUsesDenomination":
                                BankDropUsesDenomination = Conversion.XmlStringToBool(current.Value);
                                break;
                            case "bankDropRevUsesDenomination":
                                BankDropRevUsesDenomination = Conversion.XmlStringToBool(current.Value);
                                break;
                            case "tenderDeclUsesDenomination":
                                TenderDeclUsesDenomination = Conversion.XmlStringToBool(current.Value);
                                break;
                            case "customerRequiredOnReturn":
                                CustomerRequiredOnReturn = Conversion.XmlStringToBool(current.Value);
                                break;
                            case "keepDailyJournalOpenAfterPrintingReceipt":
                                KeepDailyJournalOpenAfterPrintingReceipt = Conversion.XmlStringToBool(current.Value);
                                break;
                            case "displayVoidedItems":
                                DisplayVoidedItems = Conversion.XmlStringToBool(current.Value);
                                break;
                            case "displayVoidedPayments":
                                DisplayVoidedPayments = Conversion.XmlStringToBool(current.Value);
                                break;
                            case "allowImageViewInItemLookup":
                                AllowImageViewInItemLookup = Conversion.XmlStringToBool(current.Value);
                                break;
                            case "rememberListImageSelection":
                                RememberListImageSelection = Conversion.XmlStringToBool(current.Value);
                                break;
                            case "defaultItemImage":
                                DefaultItemImage = FromBase64(current.Value);
                                break;
                            case "pOSSettingsClear":
                                POSSettingsClear = (SettingsClear)Conversion.XmlStringToInt(current.Value);
                                break;
                            case "settingsClearGracePeriod":
                                SettingsClearGracePeriod = Conversion.XmlStringToInt(current.Value);
                                break;
                            case "allowSaleAndReturnInSameTransaction":
                                AllowSaleAndReturnInSameTransaction = Conversion.XmlStringToBool(current.Value);
                                break;
                            case "useStartOfDay":
                                UseStartOfDay = Conversion.XmlStringToBool(current.Value);
                                break;
                            case "salesPersonPrompt":
                                SalesPersonPrompt = (SalesPersonPrompt)Conversion.XmlStringToInt(current.Value);
                                break;
                            case "ZReportConfig":
                                ZReportConfig.ToClass(current);
                                break;
                            case "postTransactionDDJob":
                                PostTransactionDDJob = current.Value;
                                break;
                            case "dDSchedulerLocation":
                                DDSchedulerLocation = current.Value;
                                break;
                            case "infocodeStartOfTransactionType":
                                InfocodeStartOfTransactionType = (StartOfTransactionTypes)Conversion.XmlStringToInt(current.Value);
                                break;
                            case "dialogLimitationDisplayType":
                                DialogLimitationDisplayType = (LimitationDisplayType)Conversion.XmlStringToInt(current.Value);
                                break;
                            case "profileIsUsed":
                                ProfileIsUsed = Conversion.XmlStringToBool(current.Value);
                                break;
                            case "showPricesByDefault":
                                ShowPricesByDefault = Conversion.XmlStringToBool(current.Value);
                                break;
                            case "displayItemIDInReturnDialog":
                                DisplayItemIDInReturnDialog = Conversion.XmlStringToBool(current.Value);
                                break;
                            
                            case "omniFunctionalityProfile":
                                OmniProfile.ToClass(current);
                                break;
                        }
                    }
                    catch (Exception ex)
                    {
                        errorLogger?.LogMessage(LogMessageType.Error, current.Name.ToString(), ex);
                    }
                }
            }
        }

        public override XElement ToXML(IErrorLog errorLogger = null)
        {
            var xml = new XElement("functionalityProfile",
                                   new XElement("functionalityProfileID", ID),
                                   new XElement("functionalityProfileName", Text),
                                   new XElement("aggregateItems", Conversion.ToXmlString((int) AggregateItems)),
                                   new XElement("aggregatePayments", Conversion.ToXmlString(AggregatePayments)),
                                   new XElement("tsCentralTableServer", Conversion.ToXmlString(TsCentralTableServer)),
                                   new XElement("fCentralTableServer", CentralTableServer),
                                   new XElement("fCentralTableServerPort", Conversion.ToXmlString(CentralTableServerPort)),
                                   new XElement("tsCustomer", Conversion.ToXmlString(TsCustomer)),
                                   new XElement("tsStaff", Conversion.ToXmlString(TsStaff)),
                                   new XElement("tsSuspendRetrieveTransactions", Conversion.ToXmlString(TsSuspendRetrieveTransactions)),
                                   new XElement("logLevel", Conversion.ToXmlString((int) LogLevel)),
                                   new XElement("aggregateItemsForPrinting", Conversion.ToXmlString(AggregateItemsForPrinting)),
                                   new XElement("showStaffListAtLogon", Conversion.ToXmlString(ShowStaffListAtLogon)),
                                   new XElement("limitStaffListToStore", Conversion.ToXmlString(LimitStaffListToStore)),
                                   new XElement("allowSalesIfDrawerIsOpen", Conversion.ToXmlString(AllowSalesIfDrawerIsOpen)),
                                   new XElement("staffBarcodeLogon", Conversion.ToXmlString(StaffBarcodeLogon)),
                                   new XElement("staffCardLogon", Conversion.ToXmlString(StaffCardLogon)),
                                   new XElement("mustKeyInPriceIfZero", Conversion.ToXmlString(MustKeyInPriceIfZero)),
                                   new XElement("clearUserBetweenLogins", Conversion.ToXmlString(ClearUserBetweenLogins)),
                                   new XElement("displayLimitationsTotalsInPOS", Conversion.ToXmlString(DisplayLimitationsTotalsInPOS)),
                                   //new XElement("minimumPasswordLength", MinimumPasswordLength),
                                   new XElement("infocodeStartOfTransaction", InfocodeStartOfTransaction),
                                   new XElement("infocodeEndOfTransaction", InfocodeEndOfTransaction),
                                   new XElement("infocodeTenderDecl", InfocodeTenderDecl),
                                   new XElement("infocodeItemNotFound", InfocodeItemNotFound),
                                   new XElement("infocodeItemDiscount", InfocodeItemDiscount),
                                   new XElement("infocodeTotalDiscount", InfocodeTotalDiscount),
                                   new XElement("infocodePriceOverride", InfocodePriceOverride),
                                   new XElement("infocodeReturnItem", InfocodeReturnItem),
                                   new XElement("infocodeReturnTransaction", InfocodeReturnTransaction),
                                   new XElement("infocodeVoidItem", InfocodeVoidItem),
                                   new XElement("infocodeVoidPayment", InfocodeVoidPayment),
                                   new XElement("infocodeVoidTransaction", InfocodeVoidTransaction),
                                   new XElement("infocodeAddSalesPerson", InfocodeAddSalesPerson),
                                   new XElement("infocodeOpenDrawer", InfocodeOpenDrawer),
                                   new XElement("priceDecimalPlaces", PriceDecimalPlaces),
                                   new XElement("syncTransactions", Conversion.ToXmlString(SyncTransactions)),
                                   //new XElement("alwaysAskForPassword", AlwaysAskForPassword),
                                   new XElement("maximumQTY", Conversion.ToXmlString(MaximumQTY)),
                                   new XElement("maximumPrice", Conversion.ToXmlString(MaximumPrice)),
                                   new XElement("pollingInterval", Conversion.ToXmlString(PollingInterval)),
                                   new XElement("decimalsInNumpad", Conversion.ToXmlString(DecimalsInNumpad)),
                                   new XElement("entryStartsInDecimals", Conversion.ToXmlString(EntryStartsInDecimals)),
                                   new XElement("isHospitality", Conversion.ToXmlString(IsHospitality)),
                                   new XElement("skipHospitalityTableView", Conversion.ToXmlString(SkipHospitalityTableView)),
                                   new XElement("AllowVoidAfterSplitBill", Conversion.ToXmlString(AllowItemChangesAfterSplitBill)),
                                   new XElement("safeDropUsesDenomination", Conversion.ToXmlString(SafeDropUsesDenomination)),
                                   new XElement("safeDropRevUsesDenomination", Conversion.ToXmlString(SafeDropRevUsesDenomination)),
                                   new XElement("bankDropUsesDenomination", Conversion.ToXmlString(BankDropUsesDenomination)),
                                   new XElement("bankDropRevUsesDenomination", Conversion.ToXmlString(BankDropRevUsesDenomination)),
                                   new XElement("tenderDeclUsesDenomination", Conversion.ToXmlString(TenderDeclUsesDenomination)),
                                   new XElement("customerRequiredOnReturn", Conversion.ToXmlString(CustomerRequiredOnReturn)),
                                   new XElement("keepDailyJournalOpenAfterPrintingReceipt", Conversion.ToXmlString(KeepDailyJournalOpenAfterPrintingReceipt)),
                                   new XElement("displayVoidedItems", Conversion.ToXmlString(DisplayVoidedItems)),
                                   new XElement("displayVoidedPayments", Conversion.ToXmlString(DisplayVoidedPayments)),
                                   new XElement("allowImageViewInItemLookup", Conversion.ToXmlString(AllowImageViewInItemLookup)),
                                   new XElement("rememberListImageSelection", Conversion.ToXmlString(RememberListImageSelection)),
                                   new XElement("defaultItemImage", ToBase64(DefaultItemImage)),
                                   new XElement("pOSSettingsClear", Conversion.ToXmlString((int)POSSettingsClear)),
                                   new XElement("settingsClearGracePeriod", Conversion.ToXmlString(SettingsClearGracePeriod)),
                                   new XElement("allowSaleAndReturnInSameTransaction", Conversion.ToXmlString(AllowSaleAndReturnInSameTransaction)),
                                   new XElement("useStartOfDay", Conversion.ToXmlString(UseStartOfDay)),
                                   new XElement("salesPersonPrompt", SalesPersonPrompt),
                                   new XElement("ZReportConfig", ZReportConfig.ToXML()),
                                   new XElement("postTransactionDDJob", PostTransactionDDJob),
                                   new XElement("dDSchedulerLocation", DDSchedulerLocation),
                                   new XElement("infocodeStartOfTransactionType", Conversion.ToXmlString((int)InfocodeStartOfTransactionType)),
                                   new XElement("dialogLimitationDisplayType", Conversion.ToXmlString((int)DialogLimitationDisplayType)),
                                   new XElement("profileIsUsed", Conversion.ToXmlString(ProfileIsUsed)),
                                   new XElement("showPricesByDefault", Conversion.ToXmlString(ShowPricesByDefault)),
                                   new XElement("displayItemIDInReturnDialog", Conversion.ToXmlString(DisplayItemIDInReturnDialog)),
                                   
                                   new XElement("omniFunctionalityProfile", OmniProfile.ToXML(errorLogger))
            );
            return xml;
        }

        public override object Clone()
        {
            var clone = new FunctionalityProfile();
            Populate(clone);
            
            return clone;
        }

        protected void Populate(FunctionalityProfile clone)
        {
            clone.ID = (RecordIdentifier)ID.Clone();
            clone.Text = Text;
            clone.AggregateItems = AggregateItems;
            clone.AggregatePayments = AggregatePayments;
            clone.TsCentralTableServer = TsCentralTableServer;
            clone.CentralTableServer = CentralTableServer;
            clone.CentralTableServerPort = CentralTableServerPort;
            clone.TsCustomer = TsCustomer;
            clone.TsStaff = TsStaff;
            clone.TsSuspendRetrieveTransactions = TsSuspendRetrieveTransactions;
            clone.LogLevel = LogLevel;
            clone.AggregateItemsForPrinting = AggregateItemsForPrinting;
            clone.ShowStaffListAtLogon = ShowStaffListAtLogon;
            clone.LimitStaffListToStore = LimitStaffListToStore;
            clone.AllowSalesIfDrawerIsOpen = AllowSalesIfDrawerIsOpen;
            clone.StaffBarcodeLogon = StaffBarcodeLogon;
            clone.StaffCardLogon = StaffCardLogon;
            clone.MustKeyInPriceIfZero = MustKeyInPriceIfZero;
            clone.ClearUserBetweenLogins = ClearUserBetweenLogins;
            clone.DisplayLimitationsTotalsInPOS = DisplayLimitationsTotalsInPOS;
            clone.InfocodeStartOfTransaction = InfocodeStartOfTransaction;
            clone.InfocodeEndOfTransaction = InfocodeEndOfTransaction;
            clone.InfocodeTenderDecl = InfocodeTenderDecl;
            clone.InfocodeItemNotFound = InfocodeItemNotFound;
            clone.InfocodeItemDiscount = InfocodeItemDiscount;
            clone.InfocodeTotalDiscount = InfocodeTotalDiscount;
            clone.InfocodePriceOverride = InfocodePriceOverride;
            clone.InfocodeReturnItem = InfocodeReturnItem;
            clone.InfocodeReturnTransaction = InfocodeReturnTransaction;
            clone.InfocodeVoidItem = InfocodeVoidItem;
            clone.InfocodeVoidPayment = InfocodeVoidPayment;
            clone.InfocodeVoidTransaction = InfocodeVoidTransaction;
            clone.InfocodeAddSalesPerson = InfocodeAddSalesPerson;
            clone.InfocodeOpenDrawer = InfocodeOpenDrawer;
            clone.PriceDecimalPlaces = PriceDecimalPlaces;
            clone.SyncTransactions = SyncTransactions;
            clone.MaximumQTY = MaximumQTY;
            clone.MaximumPrice = MaximumPrice;
            clone.PollingInterval = PollingInterval;
            clone.DecimalsInNumpad = DecimalsInNumpad;
            clone.EntryStartsInDecimals = EntryStartsInDecimals;
            clone.IsHospitality = IsHospitality;
            clone.SkipHospitalityTableView = SkipHospitalityTableView;
            clone.AllowItemChangesAfterSplitBill = AllowItemChangesAfterSplitBill;
            clone.SafeDropUsesDenomination = SafeDropUsesDenomination;
            clone.SafeDropRevUsesDenomination = SafeDropRevUsesDenomination;
            clone.BankDropUsesDenomination = BankDropUsesDenomination;
            clone.BankDropRevUsesDenomination = BankDropRevUsesDenomination;
            clone.TenderDeclUsesDenomination = TenderDeclUsesDenomination;
            clone.CustomerRequiredOnReturn = CustomerRequiredOnReturn;
            clone.KeepDailyJournalOpenAfterPrintingReceipt = KeepDailyJournalOpenAfterPrintingReceipt;
            clone.DisplayVoidedItems = DisplayVoidedItems;
            clone.DisplayVoidedPayments = DisplayVoidedPayments;
            clone.AllowImageViewInItemLookup = AllowImageViewInItemLookup;
            clone.RememberListImageSelection = RememberListImageSelection;
            clone.DefaultItemImage = DefaultItemImage;
            clone.POSSettingsClear = POSSettingsClear;
            clone.SettingsClearGracePeriod = SettingsClearGracePeriod;
            clone.AllowSaleAndReturnInSameTransaction = AllowSaleAndReturnInSameTransaction;
            clone.PostTransactionDDJob = PostTransactionDDJob;
            clone.DDSchedulerLocation = DDSchedulerLocation;
            clone.UseStartOfDay = UseStartOfDay;
            clone.SalesPersonPrompt = SalesPersonPrompt;
            clone.ZReportConfig = (ZReportConfig) ZReportConfig.Clone();
            clone.InfocodeStartOfTransactionType = InfocodeStartOfTransactionType;
            clone.DialogLimitationDisplayType = DialogLimitationDisplayType;
            clone.ProfileIsUsed = ProfileIsUsed;
            clone.DisplayItemIDInReturnDialog = DisplayItemIDInReturnDialog;

            clone.OmniProfile = (OmniFunctionalityProfile)OmniProfile.Clone();
        }
    }
}
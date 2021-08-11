using System;
using System.Windows.Forms;
using System.Xml.Linq;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.Profiles;
using LSOne.DataLayer.BusinessObjects.Tax;
using LSOne.DataLayer.BusinessObjects.Units;
using LSOne.Utilities.DataTypes;
using LSOne.Utilities.ErrorHandling;

namespace LSOne.DataLayer.BusinessObjects.ConfigurationWizard
{
    /// <summary>
    /// Business entity class of StoreSettings page
    /// </summary>
    public class StoreSettings : DataEntity
    {        
        public StoreSettings()
        {
            ID = RecordIdentifier.Empty;
            TemplateID = RecordIdentifier.Empty;
            TaxGroupId = RecordIdentifier.Empty;
            TaxGroupName = "";
            TaxGroupType = null;

            SalesTaxGroup = new SalesTaxGroup();
            ItemSalesTaxGroup = new ItemSalesTaxGroup();
            Unit = new Unit();
            VisualProfile = new VisualProfile();
            FunctionalityProfile = new FunctionalityProfile();
            SiteServiceProfile = new SiteServiceProfile();
        }

        /// <summary>
        /// Wizard TemplateId 
        /// </summary>
        public RecordIdentifier TemplateID { get; set; }

        /// <summary>
        /// StoreSetting TaxGroupId
        /// </summary>
        public RecordIdentifier TaxGroupId { get; set; }

        /// <summary>
        /// StoreSetting TaxGroupName
        /// </summary>
        public string TaxGroupName { get; set; }

        /// <summary>
        /// StoreSetting TaxGroupType, 0 = Store, 1 = Item
        /// </summary>
        public int? TaxGroupType { get; set; }

        /// <summary>
        /// Creating object of standard SalesTaxGroup class.
        /// </summary>
        public SalesTaxGroup SalesTaxGroup { get; set; }

        /// <summary>
        /// Creating object of standard ItemSalesTaxGroup class.
        /// </summary>
        public ItemSalesTaxGroup ItemSalesTaxGroup { get; set; }

        /// <summary>
        /// Creating object of standard Unit class.
        /// </summary>
        public Unit Unit { get; set; }

        /// <summary>
        /// </summary>
        /// Creating object of standard FunctionalityProfile class.
        public FunctionalityProfile FunctionalityProfile { get; set; }

        /// <summary>
        /// Creating object of standard VisualProfile class.
        /// </summary>
        public VisualProfile VisualProfile { get; set; }

        /// <summary>
        /// Creating object of standard SiteServiceProfile class.
        /// </summary>
        public SiteServiceProfile SiteServiceProfile { get; set; }

        /// <summary>
        /// Sets all variables in the StoreSetting class with the values in the xml
        /// </summary>
        /// <param name="xStoreSetting">The xml element with the store setting values</param>
        /// <param name="errorLogger"></param>
        public override void ToClass(XElement xStoreSetting, IErrorLog errorLogger = null)
        {
            if (xStoreSetting.Name.ToString() == "salesTaxGroup")
            {
                var storeVariables = xStoreSetting.Elements();
                foreach (var storeElem in storeVariables)
                {
                    //No tax group id -> no store setting -> no need to go any further
                    if (storeElem.Name.ToString() == "taxGroupID" && storeElem.Value == "")
                    {
                        return;
                    }

                    if (storeElem.Value.Length > 0)
                    {
                        try
                        {
                            switch (storeElem.Name.ToString())
                            {                                
                                case "taxGroupID":
                                    SalesTaxGroup.ID = storeElem.Value;
                                    break;
                                case "taxGroupName":
                                    SalesTaxGroup.Text = storeElem.Value;
                                    break;
                                case "searchField1":
                                    SalesTaxGroup.SearchField1 = storeElem.Value;
                                    break;
                                case "SearchField2":
                                    SalesTaxGroup.SearchField2 = storeElem.Value;
                                    break;
                            }
                        }
                        catch (Exception ex)
                        {
                            if (errorLogger != null)
                            {
                                errorLogger.LogMessage(LogMessageType.Error, storeElem.Name.ToString(), ex);
                            }
                        }
                    }
                }
            }
            if (xStoreSetting.Name.ToString() == "itemSalesTaxGroup")
            {
                var storeVariables = xStoreSetting.Elements();
                foreach (var storeElem in storeVariables)
                {
                    //No tax group id -> no store setting -> no need to go any further
                    if (storeElem.Name.ToString() == "taxGroupID" && storeElem.Value == "")
                    {
                        return;
                    }

                    if (storeElem.Value.Length > 0)
                    {
                        try
                        {
                            switch (storeElem.Name.ToString())
                            {                               
                                case "taxGroupID":
                                    ItemSalesTaxGroup.ID = storeElem.Value;
                                    break;
                                case "taxGroupName":
                                    ItemSalesTaxGroup.Text = storeElem.Value;
                                    break;
                                case "receiptDisplay":
                                    ItemSalesTaxGroup.ReceiptDisplay = storeElem.Value;
                                    break;
                            }
                        }
                        catch (Exception ex)
                        {
                            if (errorLogger != null)
                            {
                                errorLogger.LogMessage(LogMessageType.Error, storeElem.Name.ToString(), ex);
                            }
                        }
                    }
                }
            }
            if (xStoreSetting.Name.ToString() == "unit")
            {
                var storeVariables = xStoreSetting.Elements();
                foreach (var storeElem in storeVariables)
                {
                    //No unit id -> no store setting -> no need to go any further
                    if (storeElem.Name.ToString() == "unitID" && storeElem.Value == "")
                    {
                        return;
                    }

                    if (storeElem.Value.Length > 0)
                    {
                        try
                        {
                            switch (storeElem.Name.ToString())
                            {
                                case "unitID":
                                    Unit.ID = storeElem.Value;
                                    break;
                                case "unitName":
                                    Unit.Text = storeElem.Value;
                                    break;
                                case "minimumDecimals":
                                    Unit.MinimumDecimals = Convert.ToInt32(storeElem.Value);
                                    break;
                                case "maximumDecimals":
                                    Unit.MaximumDecimals = Convert.ToInt32(storeElem.Value);
                                    break;
                            }
                        }
                        catch (Exception ex)
                        {
                            if (errorLogger != null)
                            {
                                errorLogger.LogMessage(LogMessageType.Error, storeElem.Name.ToString(), ex);
                            }
                        }
                    }
                }
            }
            if (xStoreSetting.HasElements)
            {
                if (xStoreSetting.Name.ToString() == "visualProfile")
                {
                    var storeVariables = xStoreSetting.Elements();
                    foreach (var storeElem in storeVariables)
                    {
                        //No Profile id -> no store setting -> no need to go any further
                        if (storeElem.Name.ToString() == "visualProfileID" && storeElem.Value == "")
                        {
                            return;
                        }

                        if (!storeElem.IsEmpty)
                        {
                            try
                            {
                                switch (storeElem.Name.ToString())
                                {
                                    case "visualProfileID":
                                        VisualProfile.ID = storeElem.Value;
                                        break;
                                    case "visualProfileName":
                                        VisualProfile.Text= storeElem.Value;
                                        break;
                                    case "resolution":
                                        VisualProfile.Resolution = (ResolutionsEnum)Convert.ToInt32(storeElem.Value);
                                        break;
                                    case "terminalType":
                                        VisualProfile.TerminalType = (VisualProfile.HardwareTypes)Convert.ToInt32(storeElem.Value);
                                        break;
                                    case "hideCursor":
                                        VisualProfile.HideCursor = Convert.ToBoolean(storeElem.Value);
                                        break;
                                    case "designAllowedOnPos":
                                        VisualProfile.DesignAllowedOnPos = Convert.ToBoolean(storeElem.Value);
                                        break;
                                    case "opaqueBackgroundForm":
                                        VisualProfile.OpaqueBackgroundForm = Convert.ToBoolean(storeElem.Value);
                                        break;
                                    case "showCurrencySymbolOnColumns":
                                        VisualProfile.ShowCurrencySymbolOnColumns = Convert.ToBoolean(storeElem.Value);
                                        break;
                                    case "screenNumber":
                                        VisualProfile.ScreenNumber = (ScreenNumberEnum) Convert.ToInt32(storeElem.Value);
                                        break;
                                    case "opacity":
                                        VisualProfile.Opacity = Convert.ToInt32(storeElem.Value);
                                        break;
                                    case "useFormBackgroundImage":
                                        VisualProfile.UseFormBackgroundImage = Convert.ToBoolean(storeElem.Value);
                                        break;
                                    case "receiptPaymentLinesSize":
                                        VisualProfile.ReceiptPaymentLinesSize = Convert.ToInt32(storeElem.Value);
                                        break;
                                    case "receiptReturnBackgroundImageID":
                                        VisualProfile.ReceiptReturnBackgroundImageID = storeElem.Value;
                                        break;
                                    case "receiptReturnBackgroundImageLayout":
                                        VisualProfile.ReceiptReturnBackgroundImageLayout = (ImageLayout)Convert.ToInt32(storeElem.Value);
                                        break;
                                    case "receiptReturnBorderColor":
                                        VisualProfile.ReceiptReturnBorderColor = Convert.ToInt32(storeElem.Value);
                                        break;
                                }
                            }
                            catch (Exception ex)
                            {
                                if (errorLogger != null)
                                {
                                    errorLogger.LogMessage(LogMessageType.Error, storeElem.Name.ToString(), ex);
                                }
                            }
                        }
                    }
                }

                if (xStoreSetting.Name.ToString() == "siteServiceProfile")
                {
                    var storeVariables = xStoreSetting.Elements();
                    foreach (var storeElem in storeVariables)
                    {
                        //No Profile id -> no store setting -> no need to go any further
                        if (storeElem.Name.ToString() == "siteServiceProfileID" && storeElem.Value == "")
                        {
                            return;
                        }

                        if (!storeElem.IsEmpty)
                        {
                            try
                            {
                                switch (storeElem.Name.ToString())
                                {
                                    case "siteServiceProfileID":
                                        SiteServiceProfile.ID = storeElem.Value;
                                        break;
                                    case "siteServiceProfileName":
                                        SiteServiceProfile.Text = storeElem.Value;
                                        break;
                                    case "useAxTransactionServices":
                                        SiteServiceProfile.UseAxTransactionServices = Convert.ToBoolean(storeElem.Value);
                                        break;
                                    case "objectServer":
                                        SiteServiceProfile.ObjectServer = storeElem.Value;
                                        break;
                                    case "aosInstance":
                                        SiteServiceProfile.AosInstance = storeElem.Value;
                                        break;
                                    case "aosServer":
                                        SiteServiceProfile.AosServer = storeElem.Value;
                                        break;
                                    case "aosPort":
                                        SiteServiceProfile.AosPort = Convert.ToInt32(storeElem.Value);
                                        break;
                                    case "sCentralTableServer":
                                        SiteServiceProfile.SiteServiceAddress = storeElem.Value;
                                        break;
                                    case "sCentralTableServerPort":
                                        SiteServiceProfile.SiteServicePortNumber = Convert.ToInt32(storeElem.Value);
                                        break;
                                    case "userName":
                                        SiteServiceProfile.UserName = storeElem.Value;
                                        break;
                                    case "password":
                                        SiteServiceProfile.Password = storeElem.Value;
                                        break;
                                    case "company":
                                        SiteServiceProfile.Company = storeElem.Value;
                                        break;
                                    case "domain":
                                        SiteServiceProfile.Domain = storeElem.Value;
                                        break;
                                    case "axVersion":
                                        SiteServiceProfile.AxVersion = Convert.ToInt32(storeElem.Value);
                                        break;
                                    case "configuration":
                                        SiteServiceProfile.Configuration = storeElem.Value;
                                        break;
                                    case "language":
                                        SiteServiceProfile.Language = storeElem.Value;
                                        break;
                                    case "checkCustomer":
                                        SiteServiceProfile.CheckCustomer = Convert.ToBoolean(storeElem.Value);
                                        break;
                                    case "checkStaff":
                                        SiteServiceProfile.CheckStaff = Convert.ToBoolean(storeElem.Value);
                                        break;
                                    case "useInventoryLookup":
                                        SiteServiceProfile.UseInventoryLookup = Convert.ToBoolean(storeElem.Value);
                                        break;
                                    case "issueGiftCardOption":
                                        SiteServiceProfile.IssueGiftCardOption = (SiteServiceProfile.IssueGiftCardOptionEnum)Convert.ToInt32(storeElem.Value);
                                        break;
                                    case "useGiftCards":
                                        SiteServiceProfile.UseGiftCards = Convert.ToBoolean(storeElem.Value);
                                        break;
                                    case "useCentralSuspensions":
                                        SiteServiceProfile.UseCentralSuspensions = Convert.ToBoolean(storeElem.Value);
                                        break;
                                    case "userConfirmation":
                                        SiteServiceProfile.UserConfirmation = Convert.ToBoolean(storeElem.Value);
                                        break;
                                    case "centralizedReturns":
                                        SiteServiceProfile.CentralizedReturns = Convert.ToBoolean(storeElem.Value);
                                        break;
                                    case "salesOrders":
                                        SiteServiceProfile.SalesOrders = Convert.ToBoolean(storeElem.Value);
                                        break;
                                    case "salesInvoices":
                                        SiteServiceProfile.SalesInvoices = Convert.ToBoolean(storeElem.Value);
                                        break;
                                    case "useCreditVouchers":
                                        SiteServiceProfile.UseCreditVouchers = Convert.ToBoolean(storeElem.Value);
                                        break;
                                    case "newCustomerDefaultTaxGroup":
                                        SiteServiceProfile.NewCustomerDefaultTaxGroup = storeElem.Value;
                                        break;
                                    case "newCustomerDefaultTaxGroupName":
                                        SiteServiceProfile.NewCustomerDefaultTaxGroupName = storeElem.Value;
                                        break;
                                    case "cashCustomerSetting":
                                        SiteServiceProfile.CashCustomerSetting = (SiteServiceProfile.CashCustomerSettingEnum)Convert.ToInt32(storeElem.Value);
                                        break;
                                }
                            }
                            catch (Exception ex)
                            {
                                if (errorLogger != null)
                                {
                                    errorLogger.LogMessage(LogMessageType.Error, storeElem.Name.ToString(), ex);
                                }
                            }
                        }
                    }
                }
                if (xStoreSetting.Name.ToString() == "functionalityProfile")
                {
                    var storeVariables = xStoreSetting.Elements();
                    foreach (var storeElem in storeVariables)
                    {
                        //No Profile id -> no store setting -> no need to go any further
                        if (storeElem.Name.ToString() == "functionalityProfileID" && storeElem.Value == "")
                        {
                            return;
                        }

                        if (!storeElem.IsEmpty)
                        {
                            try
                            {
                                switch (storeElem.Name.ToString())
                                {
                                    case "functionalityProfileID":
                                        FunctionalityProfile.ID = storeElem.Value;
                                        break;
                                    case "functionalityProfileName":
                                        FunctionalityProfile.Text = storeElem.Value;
                                        break;
                                    case "aggregateItems":
                                        FunctionalityProfile.AggregateItems = (AggregateItemsModes)Convert.ToInt32(storeElem.Value);
                                        break;
                                    case "aggregatePayments":
                                        FunctionalityProfile.AggregatePayments = Convert.ToBoolean(storeElem.Value);
                                        break;
                                    case "tsCentralTableServer":
                                        FunctionalityProfile.TsCentralTableServer = Convert.ToBoolean(storeElem.Value);
                                        break;
                                    case "fCentralTableServer":
                                        FunctionalityProfile.CentralTableServer = storeElem.Value;
                                        break;
                                    case "fCentralTableServerPort":
                                        FunctionalityProfile.CentralTableServerPort = Convert.ToInt32(storeElem.Value);
                                        break;
                                    case "tsCustomer":
                                        FunctionalityProfile.TsCustomer = Convert.ToBoolean(storeElem.Value);
                                        break;
                                    case "tsStaff":
                                        FunctionalityProfile.TsStaff = Convert.ToBoolean(storeElem.Value);
                                        break;
                                    case "tsSuspendRetrieveTransactions":
                                        FunctionalityProfile.TsSuspendRetrieveTransactions = Convert.ToBoolean(storeElem.Value);
                                        break;
                                    case "logLevel":
                                        FunctionalityProfile.LogLevel = (FunctionalityProfile.LogTraceLevel)Convert.ToInt32(storeElem.Value);
                                        break;
                                    case "aggregateItemsForPrinting":
                                        FunctionalityProfile.AggregateItemsForPrinting = Convert.ToBoolean(storeElem.Value);
                                        break;
                                    case "showStaffListAtLogon":
                                        FunctionalityProfile.ShowStaffListAtLogon = Convert.ToBoolean(storeElem.Value);
                                        break;
                                    case "limitStaffListToStore":
                                        FunctionalityProfile.LimitStaffListToStore = Convert.ToBoolean(storeElem.Value);
                                        break;
                                    case "allowSalesIfDrawerIsOpen":
                                        FunctionalityProfile.AllowSalesIfDrawerIsOpen = Convert.ToBoolean(storeElem.Value);
                                        break;
                                    case "staffBarcodeLogon":
                                        FunctionalityProfile.StaffBarcodeLogon = Convert.ToBoolean(storeElem.Value);
                                        break;
                                    case "staffCardLogon":
                                        FunctionalityProfile.StaffCardLogon = Convert.ToBoolean(storeElem.Value);
                                        break;
                                    case "mustKeyInPriceIfZero":
                                        FunctionalityProfile.MustKeyInPriceIfZero = Convert.ToBoolean(storeElem.Value);
                                        break;
                                    //case "minimumPasswordLength":
                                    //    functionalityProfile.MinimumPasswordLength = Convert.ToInt32(storeElem.Value);
                                    //    break;
                                    case "infocodeStartOfTransaction":
                                        FunctionalityProfile.InfocodeStartOfTransaction = storeElem.Value;
                                        break;
                                    case "infocodeEndOfTransaction":
                                        FunctionalityProfile.InfocodeEndOfTransaction = storeElem.Value;
                                        break;
                                    case "infocodeTenderDecl":
                                        FunctionalityProfile.InfocodeTenderDecl = storeElem.Value;
                                        break;
                                    case "infocodeItemNotFound":
                                        FunctionalityProfile.InfocodeItemNotFound = storeElem.Value;
                                        break;
                                    case "infocodeItemDiscount":
                                        FunctionalityProfile.InfocodeItemDiscount = storeElem.Value;
                                        break;
                                    case "infocodeTotalDiscount":
                                        FunctionalityProfile.InfocodeTotalDiscount = storeElem.Value;
                                        break;
                                    case "infocodePriceOverride":
                                        FunctionalityProfile.InfocodePriceOverride = storeElem.Value;
                                        break;
                                    case "infocodeReturnItem":
                                        FunctionalityProfile.InfocodeReturnItem = storeElem.Value;
                                        break;
                                    case "infocodeReturnTransaction":
                                        FunctionalityProfile.InfocodeReturnTransaction = storeElem.Value;
                                        break;
                                    case "infocodeVoidItem":
                                        FunctionalityProfile.InfocodeVoidItem = storeElem.Value;
                                        break;
                                    case "infocodeVoidPayment":
                                        FunctionalityProfile.InfocodeVoidPayment = storeElem.Value;
                                        break;
                                    case "infocodeVoidTransaction":
                                        FunctionalityProfile.InfocodeVoidTransaction = storeElem.Value;
                                        break;
                                    case "infocodeAddSalesPerson":
                                        FunctionalityProfile.InfocodeAddSalesPerson = storeElem.Value;
                                        break;
                                    case "infocodeOpenDrawer":
                                        FunctionalityProfile.InfocodeOpenDrawer = storeElem.Value;
                                        break;
                                    case "priceDecimalPlaces":
                                        FunctionalityProfile.PriceDecimalPlaces = storeElem.Value;
                                        break;
                                    case "syncTransactions":
                                        FunctionalityProfile.SyncTransactions = Convert.ToBoolean(storeElem.Value);
                                        break;
                                    //case "alwaysAskForPassword":
                                    //    functionalityProfile.AlwaysAskForPassword = Convert.ToBoolean(storeElem.Value);
                                    //    break;
                                    case "maximumQTY":
                                        FunctionalityProfile.MaximumQTY = Convert.ToDecimal(storeElem.Value);
                                        break;
                                    case "maximumPrice":
                                        FunctionalityProfile.MaximumPrice = Convert.ToDecimal(storeElem.Value);
                                        break;
                                    case "pollingInterval":
                                        FunctionalityProfile.PollingInterval = Convert.ToInt32(storeElem.Value);
                                        break;
                                    case "decimalsInNumpad":
                                        FunctionalityProfile.DecimalsInNumpad = Convert.ToInt32(storeElem.Value);
                                        break;
                                    case "entryStartsInDecimals":
                                        FunctionalityProfile.EntryStartsInDecimals = Convert.ToBoolean(storeElem.Value);
                                        break;
                                    case "isHospitality":
                                        FunctionalityProfile.IsHospitality = Convert.ToBoolean(storeElem.Value);
                                        break;
                                    case "skipHospitalityTableView":
                                        FunctionalityProfile.SkipHospitalityTableView = Convert.ToBoolean(storeElem.Value);
                                        break;
                                    case "safeDropUsesDenomination":
                                        FunctionalityProfile.SafeDropUsesDenomination = Convert.ToBoolean(storeElem.Value);
                                        break;
                                    case "safeDropRevUsesDenomination":
                                        FunctionalityProfile.SafeDropRevUsesDenomination = Convert.ToBoolean(storeElem.Value);
                                        break;
                                    case "bankDropUsesDenomination":
                                        FunctionalityProfile.BankDropUsesDenomination = Convert.ToBoolean(storeElem.Value);
                                        break;
                                    case "bankDropRevUsesDenomination":
                                        FunctionalityProfile.BankDropRevUsesDenomination = Convert.ToBoolean(storeElem.Value);
                                        break;
                                    case "tenderDeclUsesDenomination":
                                        FunctionalityProfile.TenderDeclUsesDenomination = Convert.ToBoolean(storeElem.Value);
                                        break;
                                    case "customerRequiredOnReturn":
                                        FunctionalityProfile.CustomerRequiredOnReturn = Convert.ToBoolean(storeElem.Value);
                                        break;
                                    case "keepDailyJournalOpenAfterPrintingReceipt":
                                        FunctionalityProfile.KeepDailyJournalOpenAfterPrintingReceipt = Convert.ToBoolean(storeElem.Value);
                                        break;
                                }
                            }
                            catch (Exception ex)
                            {
                                if (errorLogger != null)
                                {
                                    errorLogger.LogMessage(LogMessageType.Error, storeElem.Name.ToString(), ex);
                                }
                            }
                        }
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
            XElement xStoreSetting = null;
            if (SalesTaxGroup.ID != RecordIdentifier.Empty)
            {
                xStoreSetting = new XElement("salesTaxGroup",
                        new XElement("taxGroupID", SalesTaxGroup.ID),
                        new XElement("taxGroupName", SalesTaxGroup.Text),
                        new XElement("searchField1", SalesTaxGroup.SearchField1),
                        new XElement("searchField2", SalesTaxGroup.SearchField2)
                    );
            }
            if (ItemSalesTaxGroup.ID != RecordIdentifier.Empty)
            {
                xStoreSetting = new XElement("itemSalesTaxGroup",
                        new XElement("taxGroupID", ItemSalesTaxGroup.ID),
                        new XElement("taxGroupName", ItemSalesTaxGroup.Text),
                        new XElement("receiptDisplay", ItemSalesTaxGroup.ReceiptDisplay)
                    );
            }
            if (Unit.ID.StringValue != string.Empty)
            {
                xStoreSetting = new XElement("unit",
                    new XElement("unitID", Unit.ID),
                    new XElement("unitName", Unit.Text),
                    new XElement("minimumDecimals", Unit.MinimumDecimals),
                    new XElement("maximumDecimals", Unit.MaximumDecimals)
                    );                
            }
            if (VisualProfile.ID != null && VisualProfile.ID != RecordIdentifier.Empty)
            {
                xStoreSetting = new XElement("visualProfile",
                    new XElement("visualProfileID", VisualProfile.ID),
                    new XElement("visualProfileName", VisualProfile.Text),
                    new XElement("resolution", (int)VisualProfile.Resolution),
                    new XElement("terminalType", (int)VisualProfile.TerminalType),
                    new XElement("hideCursor", VisualProfile.HideCursor),
                    new XElement("designAllowedOnPos", VisualProfile.DesignAllowedOnPos),
                    new XElement("opaqueBackgroundForm", VisualProfile.OpaqueBackgroundForm),
                    new XElement("showCurrencySymbolOnColumns", VisualProfile.ShowCurrencySymbolOnColumns),
                    new XElement("screenNumber", VisualProfile.ScreenNumber),
                    new XElement("opacity", VisualProfile.Opacity),
                    new XElement("useFormBackgroundImage", VisualProfile.UseFormBackgroundImage),
                    new XElement("receiptPaymentLinesSize", VisualProfile.ReceiptPaymentLinesSize),
                    new XElement("receiptReturnBackgroundImageID", VisualProfile.ReceiptReturnBackgroundImageID),
                    new XElement("receiptReturnBackgroundImageLayout", (int)VisualProfile.ReceiptReturnBackgroundImageLayout),
                    new XElement("receiptReturnBorderColor", VisualProfile.ReceiptReturnBorderColor)
                    );                
            }
            if (SiteServiceProfile.ID != null && SiteServiceProfile.ID != RecordIdentifier.Empty)
            {
                xStoreSetting = new XElement("siteServiceProfile",
                    new XElement("siteServiceProfileID", (string)SiteServiceProfile.ID),
                    new XElement("siteServiceProfileName", SiteServiceProfile.Text),
                    new XElement("useAxTransactionServices", SiteServiceProfile.UseAxTransactionServices),
                    new XElement("objectServer", SiteServiceProfile.ObjectServer),
                    new XElement("aosInstance", SiteServiceProfile.AosInstance),
                    new XElement("aosServer", SiteServiceProfile.AosServer),
                    new XElement("aosPort", SiteServiceProfile.AosPort),
                    new XElement("sCentralTableServer", SiteServiceProfile.SiteServiceAddress),
                    new XElement("sCentralTableServerPort", SiteServiceProfile.SiteServicePortNumber),
                    new XElement("userName", SiteServiceProfile.UserName),
                    new XElement("password", SiteServiceProfile.Password),
                    new XElement("company", SiteServiceProfile.Company),
                    new XElement("domain", SiteServiceProfile.Domain),
                    new XElement("axVersion", SiteServiceProfile.AxVersion),
                    new XElement("configuration", SiteServiceProfile.Configuration),
                    new XElement("language", SiteServiceProfile.Language),
                    new XElement("checkCustomer", SiteServiceProfile.CheckCustomer),
                    new XElement("checkStaff", SiteServiceProfile.CheckStaff),
                    new XElement("useInventoryLookup", SiteServiceProfile.UseInventoryLookup),
                    new XElement("issueGiftCardOption", (int)SiteServiceProfile.IssueGiftCardOption),
                    new XElement("useGiftCards", SiteServiceProfile.UseGiftCards),
                    new XElement("useCentralSuspensions", SiteServiceProfile.UseCentralSuspensions),
                    new XElement("userConfirmation", SiteServiceProfile.UserConfirmation),
                    new XElement("centralizedReturns", SiteServiceProfile.CentralizedReturns),
                    new XElement("salesOrders", SiteServiceProfile.SalesOrders),
                    new XElement("salesInvoices", SiteServiceProfile.SalesInvoices),
                    new XElement("useCreditVouchers", SiteServiceProfile.UseCreditVouchers),
                    new XElement("newCustomerDefaultTaxGroup", SiteServiceProfile.NewCustomerDefaultTaxGroup),
                    new XElement("newCustomerDefaultTaxGroupName", SiteServiceProfile.NewCustomerDefaultTaxGroupName),
                    new XElement("cashCustomerSetting", (int)SiteServiceProfile.CashCustomerSetting)
                    );
            }
            if (FunctionalityProfile.ID != null && FunctionalityProfile.ID != RecordIdentifier.Empty)
            {
                xStoreSetting = new XElement("functionalityProfile",
                    new XElement("functionalityProfileID", FunctionalityProfile.ID),
                    new XElement("functionalityProfileName", FunctionalityProfile.Text),
                    new XElement("aggregateItems", (int)FunctionalityProfile.AggregateItems),
                    new XElement("aggregatePayments", FunctionalityProfile.AggregatePayments),
                    new XElement("tsCentralTableServer", FunctionalityProfile.TsCentralTableServer),
                    new XElement("fCentralTableServer", FunctionalityProfile.CentralTableServer),
                    new XElement("fCentralTableServerPort", FunctionalityProfile.CentralTableServerPort),
                    new XElement("tsCustomer", FunctionalityProfile.TsCustomer),
                    new XElement("tsStaff", FunctionalityProfile.TsStaff),
                    new XElement("tsSuspendRetrieveTransactions", FunctionalityProfile.TsSuspendRetrieveTransactions),
                    new XElement("logLevel", (int)FunctionalityProfile.LogLevel),
                    new XElement("aggregateItemsForPrinting", FunctionalityProfile.AggregateItemsForPrinting),
                    new XElement("showStaffListAtLogon", FunctionalityProfile.ShowStaffListAtLogon),
                    new XElement("limitStaffListToStore", FunctionalityProfile.LimitStaffListToStore),
                    new XElement("allowSalesIfDrawerIsOpen", FunctionalityProfile.AllowSalesIfDrawerIsOpen),
                    new XElement("staffBarcodeLogon", FunctionalityProfile.StaffBarcodeLogon),
                    new XElement("staffCardLogon", FunctionalityProfile.StaffCardLogon),
                    new XElement("mustKeyInPriceIfZero", FunctionalityProfile.MustKeyInPriceIfZero),
                    //new XElement("minimumPasswordLength", functionalityProfile.MinimumPasswordLength),
                    new XElement("infocodeStartOfTransaction", FunctionalityProfile.InfocodeStartOfTransaction),
                    new XElement("infocodeEndOfTransaction", FunctionalityProfile.InfocodeEndOfTransaction),
                    new XElement("infocodeTenderDecl", FunctionalityProfile.InfocodeTenderDecl),
                    new XElement("infocodeItemNotFound", FunctionalityProfile.InfocodeItemNotFound),
                    new XElement("infocodeItemDiscount", FunctionalityProfile.InfocodeItemDiscount),
                    new XElement("infocodeTotalDiscount", FunctionalityProfile.InfocodeTotalDiscount),
                    new XElement("infocodePriceOverride", FunctionalityProfile.InfocodePriceOverride),
                    new XElement("infocodeReturnItem", FunctionalityProfile.InfocodeReturnItem),
                    new XElement("infocodeReturnTransaction", FunctionalityProfile.InfocodeReturnTransaction),
                    new XElement("infocodeVoidItem", FunctionalityProfile.InfocodeVoidItem),
                    new XElement("infocodeVoidPayment", FunctionalityProfile.InfocodeVoidPayment),
                    new XElement("infocodeVoidTransaction", FunctionalityProfile.InfocodeVoidTransaction),
                    new XElement("infocodeAddSalesPerson", FunctionalityProfile.InfocodeAddSalesPerson),
                    new XElement("infocodeOpenDrawer", FunctionalityProfile.InfocodeOpenDrawer),
                    new XElement("priceDecimalPlaces", FunctionalityProfile.PriceDecimalPlaces),
                    new XElement("syncTransactions", FunctionalityProfile.SyncTransactions),
                    //new XElement("alwaysAskForPassword", functionalityProfile.AlwaysAskForPassword),
                    new XElement("maximumQTY", FunctionalityProfile.MaximumQTY),
                    new XElement("maximumPrice", FunctionalityProfile.MaximumPrice),
                    new XElement("pollingInterval", FunctionalityProfile.PollingInterval),
                    new XElement("decimalsInNumpad", FunctionalityProfile.DecimalsInNumpad),
                    new XElement("entryStartsInDecimals", FunctionalityProfile.EntryStartsInDecimals),
                    new XElement("isHospitality", FunctionalityProfile.IsHospitality),
                    new XElement("skipHospitalityTableView", FunctionalityProfile.SkipHospitalityTableView),
                    new XElement("safeDropUsesDenomination", FunctionalityProfile.SafeDropUsesDenomination),
                    new XElement("safeDropRevUsesDenomination", FunctionalityProfile.SafeDropRevUsesDenomination),
                    new XElement("customerRequiredOnReturn", FunctionalityProfile.CustomerRequiredOnReturn),
                    new XElement("keepDailyJournalOpenAfterPrintingReceipt", FunctionalityProfile.KeepDailyJournalOpenAfterPrintingReceipt),
                    new XElement("bankDropUsesDenomination", FunctionalityProfile.BankDropUsesDenomination),
                    new XElement("bankDropRevUsesDenomination", FunctionalityProfile.BankDropRevUsesDenomination),
                    new XElement("TenderDeclUsesDenomination", FunctionalityProfile.TenderDeclUsesDenomination)
                    );
            }
            return xStoreSetting;
        }        
    }
}

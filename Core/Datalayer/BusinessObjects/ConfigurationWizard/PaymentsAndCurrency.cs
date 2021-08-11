using System;
using System.Xml.Linq;
using LSOne.DataLayer.BusinessObjects.Currencies;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.LookupValues;
using LSOne.DataLayer.BusinessObjects.StoreManagement;
using LSOne.Utilities.DataTypes;
using LSOne.Utilities.ErrorHandling;

namespace LSOne.DataLayer.BusinessObjects.ConfigurationWizard
{
    /// <summary>
    /// Business entity class of PaymentsAndCurrency page
    /// </summary>
    public class PaymentsAndCurrency : DataEntity
    {   
        public PaymentsAndCurrency()
        {
            CurrencyCode = RecordIdentifier.Empty;
            TenderTypeID = RecordIdentifier.Empty;
            PaymentMethod = new PaymentMethod();
            StorePaymentMethod = new StorePaymentMethod();
            Currency = new Currency();
            CardInfo = new StoreCardType();
        }

        /// <summary>
        /// CurrencyCode used in WIZARDTEMPLATECURRENCY table
        /// </summary>
        public RecordIdentifier CurrencyCode { get; set; }

        /// <summary>
        /// TenderTypeID used in WIZARDTEMPLATETENDERS table
        /// </summary>
        public RecordIdentifier TenderTypeID { get; set; }

        /// <summary>
        /// PaymentMethodName
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Object of standard PaymentMethod class.
        /// </summary>
        public PaymentMethod PaymentMethod { get; set; }
        
        /// <summary>
        /// Object of standard StorePaymentMethod class.
        /// </summary>
        public StorePaymentMethod StorePaymentMethod { get; set; }

        /// <summary>
        /// Object of standard Currency class.
        /// </summary>
        public Currency Currency { get; set; }

        /// <summary>
        /// Object of standard CardType class
        /// </summary>
        public StoreCardType CardInfo { get; set; }

        /// <summary>
        /// Sets all variables in the PaymentsAndCurrency class with the values in the xml
        /// </summary>
        /// <param name="xPaymentAndCurrency">The xml element with the payment and currency setting values</param>
        /// <param name="errorLogger"></param>
        public override void ToClass(XElement xPaymentAndCurrency, IErrorLog errorLogger = null)
        {
            if (xPaymentAndCurrency.HasElements)
            {
                if (xPaymentAndCurrency.Name.ToString() == "paymentMethods")
                {
                    var storeVariables = xPaymentAndCurrency.Elements();
                    foreach (var storeElem in storeVariables)
                    {
                        //No tender type id -> no payment type -> no need to go any further
                        if (storeElem.Name.ToString() == "paymentMethodID" && storeElem.Value == "")
                        {
                            return;
                        }

                        if (!storeElem.IsEmpty)
                        {
                            try
                            {
                                switch (storeElem.Name.ToString())
                                {
                                    case "name":
                                        PaymentMethod.Text = storeElem.Value;
                                        break;
                                    case "paymentMethodID":
                                        PaymentMethod.ID = storeElem.Value;
                                        break;
                                    case "defaultFunction":
                                        PaymentMethod.DefaultFunction = (PaymentMethodDefaultFunctionEnum)Convert.ToInt32(storeElem.Value);
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
                if (xPaymentAndCurrency.Name.ToString() == "storePaymentMethods")
                {
                    foreach (XElement storeElem in xPaymentAndCurrency.Elements())
                    {
                        if (!storeElem.IsEmpty)
                        {
                            try
                            {
                                switch (storeElem.Name.ToString())
                                {
                                    case "name":
                                        StorePaymentMethod.Text = storeElem.Value;
                                        break;
                                    case "storeID":
                                        StorePaymentMethod.StoreID = storeElem.Value;
                                        break;
                                    case "paymentTypeID":
                                        StorePaymentMethod.PaymentTypeID = storeElem.Value;
                                        break;
                                    case "aboveMinimumTenderID":
                                        StorePaymentMethod.AboveMinimumTenderID = storeElem.Value;
                                        break;
                                    case "changeTenderID":
                                        StorePaymentMethod.ChangeTenderID = storeElem.Value;
                                        break;
                                    case "minimumChangeAmount":
                                        StorePaymentMethod.MinimumChangeAmount = Convert.ToDecimal(storeElem.Value);
                                        break;
                                    case "posOperation":
                                        StorePaymentMethod.PosOperation = Convert.ToInt32(storeElem.Value);
                                        break;
                                    case "rounding":
                                        StorePaymentMethod.Rounding = Convert.ToDecimal(storeElem.Value);
                                        break;
                                    case "roundingMethod":
                                        StorePaymentMethod.RoundingMethod = (StorePaymentMethod.PaymentRoundMethod)Convert.ToInt32(storeElem.Value);
                                        break;
                                    case "openDrawer":
                                        StorePaymentMethod.OpenDrawer = Convert.ToBoolean(storeElem.Value);
                                        break;
                                    case "underTenderAmount":
                                        StorePaymentMethod.UnderTenderAmount = Convert.ToDecimal(storeElem.Value);
                                        break;
                                    case "maximumOverTenderAmount":
                                        StorePaymentMethod.MaximumOverTenderAmount = Convert.ToDecimal(storeElem.Value);
                                        break;
                                    case "allowUnderTender":
                                        StorePaymentMethod.AllowUnderTender = Convert.ToBoolean(storeElem.Value);
                                        break;
                                    case "allowOverTender":
                                        StorePaymentMethod.AllowOverTender = Convert.ToBoolean(storeElem.Value);
                                        break;
                                    case "maximumAmountEntered":
                                        StorePaymentMethod.MaximumAmountEntered = Convert.ToDecimal(storeElem.Value);
                                        break;
                                    case "maximumAmountAllowed":
                                        StorePaymentMethod.MaximumAmountAllowed = Convert.ToDecimal(storeElem.Value);
                                        break;
                                    case "minimumAmountEntered":
                                        StorePaymentMethod.MinimumAmountEntered = Convert.ToDecimal(storeElem.Value);
                                        break;
                                    case "minimumAmountAllowed":
                                        StorePaymentMethod.MinimumAmountAllowed = Convert.ToDecimal(storeElem.Value);
                                        break;
                                    case "allowFloat":
                                        StorePaymentMethod.AllowFloat = Convert.ToBoolean(storeElem.Value);
                                        break;
                                    case "allowBankDrop":
                                        StorePaymentMethod.AllowBankDrop = Convert.ToBoolean(storeElem.Value);
                                        break;
                                    case "allowSafeDrop":
                                        StorePaymentMethod.AllowSafeDrop = Convert.ToBoolean(storeElem.Value);
                                        break;
                                    case "countingRequired":
                                        StorePaymentMethod.CountingRequired = Convert.ToBoolean(storeElem.Value);
                                        break;
                                    case "pOSOperationID":
                                        StorePaymentMethod.POSOperationID = Convert.ToInt32(storeElem.Value);
                                        break;
                                    case "maxRecount":
                                        StorePaymentMethod.MaxRecount = Convert.ToInt32(storeElem.Value);
                                        break;
                                    case "maxCountingDifference":
                                        StorePaymentMethod.MaxCountingDifference = Convert.ToDecimal(storeElem.Value);
                                        break;
                                    case "paymentTypeCanBeVoided":
                                        StorePaymentMethod.PaymentTypeCanBeVoided = Convert.ToBoolean(storeElem.Value);
                                        break;
                                    case "allowNegativePaymentAmounts":
                                        StorePaymentMethod.AllowNegativePaymentAmounts = Convert.ToBoolean(storeElem.Value);
                                        break;
                                    case "paymentTypeCanBePartOfSplitPayment":
                                        StorePaymentMethod.PaymentTypeCanBePartOfSplitPayment = Convert.ToBoolean(storeElem.Value);
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

                if (xPaymentAndCurrency.Name.ToString() == "currency")
                {
                    var currencyElements = xPaymentAndCurrency.Elements();
                    foreach (XElement storeElem in currencyElements)
                    {
                        if (!storeElem.IsEmpty)
                        {
                            try
                            {
                                switch (storeElem.Name.ToString())
                                {
                                    case "name":
                                        Currency.Text = storeElem.Value;
                                        break;
                                    case "currencyCode":
                                        Currency.ID = storeElem.Value;
                                        break;
                                    case "roundOffPurchase":
                                        Currency.RoundOffPurchase = Convert.ToDecimal(storeElem.Value);
                                        break;
                                    case "roundOffAmount":
                                        Currency.RoundOffAmount = Convert.ToDecimal(storeElem.Value);
                                        break;
                                    case "roundOffSales":
                                        Currency.RoundOffSales = Convert.ToDecimal(storeElem.Value);
                                        break;
                                    case "roundOffTypeAmount":
                                        Currency.RoundOffTypeAmount = Convert.ToInt32(storeElem.Value);
                                        break;
                                    case "roundOffTypePurchase":
                                        Currency.RoundOffTypePurchase = Convert.ToInt32(storeElem.Value);
                                        break;
                                    case "roundOffTypeSales":
                                        Currency.RoundOffTypeSales = Convert.ToInt32(storeElem.Value);
                                        break;
                                    case "currencyPrefix":
                                        Currency.CurrencyPrefix = storeElem.Value;
                                        break;
                                    case "currencySuffix":
                                        Currency.CurrencySuffix = storeElem.Value;
                                        break;
                                    case "symbol":
                                        Currency.Symbol = storeElem.Value;
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

                if (xPaymentAndCurrency.Name.ToString() == "card")
                {
                    var cardElements = xPaymentAndCurrency.Elements();
                    foreach (var cardElem in cardElements)
                    {
                        if (!cardElem.IsEmpty)
                        {
                            try
                            {
                                switch (cardElem.Name.ToString())
                                {
                                    case "storeId":
                                        CardInfo.StoreID = cardElem.Value;
                                        break;
                                    case "tenderTypeId":
                                        CardInfo.TenderTypeID = cardElem.Value;
                                        break;
                                    case "cardTypeId":
                                        CardInfo.CardTypeID = cardElem.Value;
                                        break;
                                    case "name":
                                        CardInfo.Description = cardElem.Value;
                                        break;
                                    case "checkModulus":
                                        CardInfo.CheckModulus = Convert.ToBoolean(cardElem.Value);
                                        break;
                                    case "checkExpiredDate":
                                        CardInfo.CheckExpiredDate = Convert.ToBoolean(cardElem.Value);
                                        break;
                                    case "processLocally":
                                        CardInfo.ProcessLocally = Convert.ToBoolean(cardElem.Value);
                                        break;
                                    case "allowManualInput":
                                        CardInfo.AllowManualInput = Convert.ToBoolean(cardElem.Value);
                                        break;
                                }
                            }
                            catch (Exception ex)
                            {
                                if (errorLogger != null)
                                {
                                    errorLogger.LogMessage(LogMessageType.Error, cardElem.Name.ToString(), ex);
                                }
                            }

                        }
                    }
                }


            }
        }

        /// <summary>
        /// Creates an xml element from all the variables in the PaymentsAndCurrency class
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
            XElement payments = null;
            if (PaymentMethod.ID.StringValue != string.Empty)
            {
                payments = new XElement("paymentMethods",
                    new XElement("paymentMethodID", (string)PaymentMethod.ID),
                    new XElement("name", PaymentMethod.Text),
                    new XElement("defaultFunction", (int)PaymentMethod.DefaultFunction)
                    );
            }
            if (StorePaymentMethod.StoreID != null && StorePaymentMethod.StoreID != RecordIdentifier.Empty)
            {
                payments = new XElement("storePaymentMethods",
                    new XElement("name", StorePaymentMethod.Text),
                    new XElement("storeID", (string)StorePaymentMethod.StoreID),
                    new XElement("paymentTypeID", (string)StorePaymentMethod.PaymentTypeID),
                    new XElement("aboveMinimumTenderID", (string)StorePaymentMethod.AboveMinimumTenderID),
                    new XElement("changeTenderID", (string)StorePaymentMethod.ChangeTenderID),
                    new XElement("minimumChangeAmount", StorePaymentMethod.MinimumChangeAmount),
                    new XElement("posOperation", (int)StorePaymentMethod.PosOperation),
                    new XElement("rounding", StorePaymentMethod.Rounding),
                    new XElement("roundingMethod", (int)StorePaymentMethod.RoundingMethod),
                    new XElement("openDrawer", StorePaymentMethod.OpenDrawer),
                    new XElement("underTenderAmount", StorePaymentMethod.UnderTenderAmount),
                    new XElement("maximumOverTenderAmount", StorePaymentMethod.MaximumOverTenderAmount),
                    new XElement("allowUnderTender", StorePaymentMethod.AllowUnderTender),
                    new XElement("allowOverTender", StorePaymentMethod.AllowOverTender),
                    new XElement("maximumAmountEntered", StorePaymentMethod.MaximumAmountEntered),
                    new XElement("maximumAmountAllowed", StorePaymentMethod.MaximumAmountAllowed),
                    new XElement("minimumAmountEntered", StorePaymentMethod.MinimumAmountEntered),
                    new XElement("minimumAmountAllowed", StorePaymentMethod.MinimumAmountAllowed),
                    new XElement("allowFloat", StorePaymentMethod.AllowFloat),
                    new XElement("allowBankDrop", StorePaymentMethod.AllowBankDrop),
                    new XElement("allowSafeDrop", StorePaymentMethod.AllowSafeDrop),
                    new XElement("countingRequired", StorePaymentMethod.CountingRequired),
                    new XElement("pOSOperationID", StorePaymentMethod.POSOperationID),
                    new XElement("maxRecount", StorePaymentMethod.MaxRecount),
                    new XElement("maxCountingDifference", StorePaymentMethod.MaxCountingDifference),
                    new XElement("paymentTypeCanBeVoided", StorePaymentMethod.PaymentTypeCanBeVoided),
                    new XElement("allowNegativePaymentAmounts", StorePaymentMethod.AllowNegativePaymentAmounts),
                    new XElement("paymentTypeCanBePartOfSplitPayment", StorePaymentMethod.PaymentTypeCanBePartOfSplitPayment)                   
                    );
            }
            if (Currency.ID.StringValue != string.Empty)
            {
                payments = new XElement("currency",
                    new XElement("currencyCode", (string)Currency.ID),
                    new XElement("name", Currency.Text),
                    new XElement("roundOffPurchase", Currency.RoundOffPurchase),
                    new XElement("roundOffAmount", Currency.RoundOffAmount),
                    new XElement("roundOffSales", Currency.RoundOffSales),
                    new XElement("roundOffTypeAmount", Currency.RoundOffTypeAmount),
                    new XElement("roundOffTypePurchase", Currency.RoundOffTypePurchase),
                    new XElement("roundOffTypeSales", Currency.RoundOffTypeSales),
                    new XElement("currencyPrefix", Currency.CurrencyPrefix),
                    new XElement("currencySuffix", Currency.CurrencySuffix),
                    new XElement("symbol", Currency.Symbol)                   
                    );
            }
            if (CardInfo.CardTypeID != null && CardInfo.CardTypeID.StringValue != string.Empty)
            {
                payments = new XElement("card",
                    new XElement("storeId", (string)CardInfo.StoreID),
                    new XElement("tenderTypeId", CardInfo.TenderTypeID),
                    new XElement("cardTypeId", CardInfo.CardTypeID),
                    new XElement("name", CardInfo.Description),
                    new XElement("checkModulus", CardInfo.CheckModulus),
                    new XElement("checkExpiredDate", CardInfo.CheckExpiredDate),
                    new XElement("processLocally", CardInfo.ProcessLocally),
                    new XElement("allowManualInput", CardInfo.AllowManualInput)
                    );
            }
            return payments;
        }

        /* TODO: this class incorrectly does toXml for other entities
        public override object Clone()
        {
            var o = new PaymentsAndCurrency();
            Populate(o);
            return o;
        }

        protected void Populate(PaymentsAndCurrency o)
        {
            o.ID = ID.Clone();
            o.Text = Text;
            o.DefaultFunction = DefaultFunction;
        }*/
    }
}

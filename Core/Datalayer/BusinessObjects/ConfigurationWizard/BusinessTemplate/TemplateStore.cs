using System;
using System.Xml.Linq;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.StoreManagement;
using LSOne.Utilities.DataTypes;
using LSOne.Utilities.ErrorHandling;

namespace LSOne.DataLayer.BusinessObjects.ConfigurationWizard.BusinessTemplate
{    
    public class TemplateStore : DataEntity
    {
        public TemplateStore()
        {
            Store = new Store();
        }

        public Store Store { get; set; }

        /// <summary>
        /// Sets all variables in the BusinessTemplate class with the values in the xml
        /// </summary>
        /// <param name="xStoreSetting">The xml element with the business template values</param>
        /// <param name="errorLogger"></param>
        public override void ToClass(XElement xStoreSetting, IErrorLog errorLogger = null)
        {
            if (xStoreSetting.HasElements)
            {
                var storeElements = xStoreSetting.Elements("store");
                foreach (XElement aStore in storeElements)
                {
                    if (aStore.HasElements)
                    {
                        var storeVariables = aStore.Elements();
                        foreach (XElement storeElem in storeVariables)
                        {
                            //No tax group id -> no store setting -> no need to go any further
                            if (storeElem.Name.ToString() == "storeID" && storeElem.Value == "")
                            {
                                return;
                            }

                            if (!storeElem.IsEmpty)
                            {
                                try
                                {
                                    switch (storeElem.Name.ToString())
                                    {
                                        case "storeID":
                                            Store.ID = storeElem.Value;
                                            break;
                                        case "storeName":
                                            Store.Text = storeElem.Value;
                                            break;
                                        case "storeAddress":
                                            Store.Address.Address2 = storeElem.Value;
                                            break;
                                        case "street":
                                            Store.Address.Address1 = storeElem.Value;
                                            break;
                                        case "zipCode":
                                            Store.Address.Zip = storeElem.Value;
                                            break;
                                        case "city":
                                            Store.Address.City = storeElem.Value;
                                            break;
                                        case "country":
                                            Store.Address.Country = storeElem.Value;
                                            break;
                                        case "state":
                                            Store.Address.State = storeElem.Value;
                                            break;
                                        case "currency":
                                            Store.Currency = storeElem.Value;
                                            break;
                                        case "maximumPostingDifference":
                                            Store.MaximumPostingDifference = Convert.ToDecimal(storeElem.Value);
                                            break;
                                        case "maxTransactionDifferenceAmount":
                                            Store.MaximumTransactionDifference = Convert.ToDecimal(storeElem.Value);
                                            break;
                                        case "functionalityProfile":
                                            Store.FunctionalityProfile = storeElem.Value;
                                            break;
                                        case "tenderDeclarationCalculation":
                                            Store.TenderDeclarationCalculation = (TenderDeclarationCalculation)Convert.ToInt32(storeElem.Value);
                                            break;
                                        case "taxGroup":
                                            Store.TaxGroup = storeElem.Value;
                                            break;
                                        case "cultureName":
                                            Store.LanguageCode = storeElem.Value;
                                            break;
                                        case "layoutID":
                                            Store.LayoutID = storeElem.Value;
                                            break;
                                        case "sqlServerName":
                                            Store.BackupDatabaseServer = storeElem.Value;
                                            break;
                                        case "dataBaseName":
                                            Store.BackupDatabaseName = storeElem.Value;
                                            break;
                                        case "userName":
                                            Store.BackupDatabaseUser = storeElem.Value;
                                            break;
                                        case "password":
                                            Store.BackupDatabasePassword = storeElem.Value;
                                            break;
                                        case "defaultCustAccount":
                                            Store.DefaultCustomerAccount = storeElem.Value;
                                            break;
                                        case "useDefaultCustAccount":
                                            Store.UseDefaultCustomerAccount = Convert.ToBoolean(storeElem.Value);
                                            break;
                                        case "windowsAuthentication":
                                            Store.BackupDatabaseWindowsAuthentication = Convert.ToBoolean(storeElem.Value);
                                            break;
                                        case "addressFormate":
                                            Store.Address.AddressFormat = (Address.AddressFormatEnum)Convert.ToInt32(storeElem.Value);
                                            break;
                                        case "keyedInPriceContainsTax":
                                            Store.KeyedInPriceIncludesTax = Convert.ToBoolean(storeElem.Value);
                                            break;
                                        case "calcDiscFrom":
                                            Store.CalculateDiscountsFrom = (Store.CalculateDiscountsFromEnum)Convert.ToInt32(storeElem.Value);                                        
                                            break;
                                        case "displayAmountsWithTax":
                                            Store.DisplayAmountsWithTax = Convert.ToBoolean(storeElem.Value);
                                            break;
                                        case "useTaxGroupFrom":
                                            Store.UseTaxGroupFrom = (UseTaxGroupFromEnum)Convert.ToInt32(storeElem.Value);
                                            break;
                                        case "suspendAllowEOD":
                                            Store.AllowEOD = Convert.ToInt32(storeElem.Value) == 2;
                                            break;
                                        case "storePriceSetting":
                                            Store.StorePriceSetting = (Store.StorePriceSettingsEnum)(Convert.ToInt32(storeElem.Value));
                                            break;
                                        case "transactionServiceProfile":
                                            Store.TransactionServiceProfileID = storeElem.Value;
                                            break;
                                        case "styleProfile":
                                            Store.StyleProfile = storeElem.Value;
                                            break;
                                        case "keyboardCode":
                                            Store.KeyboardCode = storeElem.Value;
                                            break;
                                        case "keyboardLayoutName":
                                            Store.KeyboardLayoutName = storeElem.Value;
                                            break;
                                        case "kitchenManagerProfileID":
                                            Store.KitchenServiceProfileID = new Guid(storeElem.Value);
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
                new XElement("storeID", (string)Store.ID),
                new XElement("storeName", Store.Text),
                //new XElement("dataAreaID", store.DataAreaID),
                new XElement("storeAddress", Store.Address.Address2),
                new XElement("street", Store.Address.Address1),
                new XElement("zipCode", Store.Address.Zip),
                new XElement("city", Store.Address.City),
                new XElement("country", Store.Address.Country),
                new XElement("state", Store.Address.State),
               // new XElement("statementMethod", (int)store.StatementMethod),
                new XElement("currency", Store.Currency),
                new XElement("maximumPostingDifference", Store.MaximumPostingDifference),
                new XElement("maxTransactionDifferenceAmount", Store.MaximumTransactionDifference),
                new XElement("functionalityProfile", Store.FunctionalityProfile),
                new XElement("tenderDeclarationCalculation", (int)Store.TenderDeclarationCalculation),
                new XElement("taxGroup", Store.TaxGroup),
                new XElement("cultureName", Store.LanguageCode),
                new XElement("layoutID", Store.LayoutID),
                new XElement("sqlServerName", Store.BackupDatabaseServer),
                new XElement("dataBaseName", Store.BackupDatabaseName),
                new XElement("userName", Store.BackupDatabaseUser),
                new XElement("password", Store.BackupDatabasePassword),
                new XElement("defaultCustAccount", Store.DefaultCustomerAccount),
                new XElement("useDefaultCustAccount", Store.UseDefaultCustomerAccount),
                new XElement("windowsAuthentication", Store.BackupDatabaseWindowsAuthentication),
                new XElement("addressFormate", (int)Store.Address.AddressFormat),
                new XElement("keyedInPriceContainsTax", Store.KeyedInPriceIncludesTax),
                new XElement("calcDiscFrom", (int)Store.CalculateDiscountsFrom),
                new XElement("displayAmountsWithTax", Store.DisplayAmountsWithTax),
                new XElement("useTaxGroupFrom", (int)Store.UseTaxGroupFrom),
                new XElement("suspendAllowEOD", Store.AllowEOD ? 2 : 3),
                new XElement("storePriceSetting", (int)Store.StorePriceSetting),
                new XElement("transactionServiceProfile", Store.TransactionServiceProfileID),
                new XElement("styleProfile", Store.StyleProfile),
                new XElement("keyboardCode", Store.KeyboardCode),
                new XElement("keyboardLayoutName", Store.KeyboardLayoutName),
                new XElement("kitchenManagerProfileID", Store.KitchenServiceProfileID)
            );
            return xStoreSetting;
        }
    }
}

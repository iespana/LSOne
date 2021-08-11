using System;
using System.Xml.Linq;
using LSOne.DataLayer.BusinessObjects.StoreManagement;
using LSOne.Utilities.ErrorHandling;

namespace LSOne.DataLayer.BusinessObjects.ConfigurationWizard.BusinessTemplate
{    
    public class TemplateTerminal : DataEntity
    {        
        public TemplateTerminal()
        {
            Terminal = new Terminal();
        }

        public Terminal Terminal { get; set; }

        /// <summary>
        /// Sets all variables in the BusinessTemplate class with the values in the xml
        /// </summary>
        /// <param name="xStoreSetting">The xml element with the business template values</param>
        /// <param name="errorLogger"></param>
        public override void ToClass(XElement xStoreSetting, IErrorLog errorLogger = null)
        {
            if (xStoreSetting.HasElements)
            {
                var businessElements = xStoreSetting.Elements("terminal");
                foreach (var aTerminal in businessElements)
                {
                    if (aTerminal.HasElements)
                    {
                        var terminalVariables = aTerminal.Elements();
                        foreach (var terminalElem in terminalVariables)
                        {
                            //No terminal id -> no store setting -> no need to go any further
                            if (terminalElem.Name.ToString() == "terminalID" && terminalElem.Value == "")
                            {
                                return;
                            }

                            if (!terminalElem.IsEmpty)
                            {
                                try
                                {
                                    switch (terminalElem.Name.ToString())
                                    {
                                        case "terminalID":
                                            Terminal.ID = terminalElem.Value;
                                            break;
                                        case "terminalName":
                                            Terminal.Name = terminalElem.Value;
                                            break;
                                        case "storeID":
                                            Terminal.StoreID = terminalElem.Value;
                                            break;
                                        case "autoLogOffTimeout":
                                            Terminal.AutoLogOffTimeout = Convert.ToInt32(terminalElem.Value);
                                            break;
                                        case "hardwareProfileID":
                                            Terminal.HardwareProfileID = terminalElem.Value;
                                            break;
                                        case "visualProfileID":
                                            Terminal.VisualProfileID = terminalElem.Value;
                                            break;
                                        case "functionalityProfileID":
                                            Terminal.FunctionalityProfileID = terminalElem.Value;
                                            break;
                                        case "customerDisplayText1":
                                            Terminal.CustomerDisplayText1 = terminalElem.Value;
                                            break;
                                        case "customerDisplayText2":
                                            Terminal.CustomerDisplayText2 = terminalElem.Value;
                                            break;
                                        case "openDrawerAtLoginLogout":
                                            Terminal.OpenDrawerAtLoginLogout = Convert.ToBoolean(terminalElem.Value);
                                            break;
                                        case "layoutID":
                                            Terminal.LayoutID = terminalElem.Value;
                                            break;                                        
                                        case "exitAfterEachTransaction":
                                            Terminal.ExitAfterEachTransaction = Convert.ToBoolean(terminalElem.Value);
                                            break;
                                        case "updateServicePort":
                                            Terminal.UpdateServicePort = Convert.ToInt32(terminalElem.Value);
                                            break;
                                        case "transactionIDNumberSequence":
                                            Terminal.TransactionIDNumberSequence = terminalElem.Value;
                                            break;
                                        case "iPAddress":
                                            Terminal.IPAddress = terminalElem.Value;
                                            break;
                                        case "eftTerminalID":
                                            Terminal.EftTerminalID = terminalElem.Value;
                                            break;
                                        case "eftStoreID":
                                            Terminal.EftStoreID = terminalElem.Value;
                                            break;
                                        case "transactionServiceProfileID":
                                            Terminal.TransactionServiceProfileID = terminalElem.Value;
                                            break;
                                        case "hospTransServiceProfileID":
                                            Terminal.HospTransServiceProfileID = terminalElem.Value;
                                            break;
                                        case "salesTypeFilter":
                                            Terminal.SalesTypeFilter = terminalElem.Value;
                                            break;
                                        case "receiptIDNumberSequence":
                                            Terminal.ReceiptIDNumberSequence = terminalElem.Value;
                                            break;
                                        case "databaseName":
                                            Terminal.DatabaseName = terminalElem.Value;
                                            break;
                                        case "databaseServer":
                                            Terminal.DatabaseServer = terminalElem.Value;
                                            break;
                                        case "databaseUserName":
                                            Terminal.DatabaseUserName = terminalElem.Value;
                                            break;
                                        case "databaseUserPassword":
                                            Terminal.DatabaseUserPassword = terminalElem.Value;
                                            break;
                                        case "kitchenManagerProfileID":
                                            Terminal.KitchenServiceProfileID =new Guid(terminalElem.Value);
                                            break;
                                    }
                                }
                                catch (Exception ex)
                                {
                                    if (errorLogger != null)
                                    {
                                        errorLogger.LogMessage(LogMessageType.Error, terminalElem.Name.ToString(), ex);
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
            var xStoreSetting = new XElement("terminal",
                new XElement("terminalID", (string)Terminal.ID),
                new XElement("terminalName", Terminal.Name),
                new XElement("storeID", (string)Terminal.StoreID),
                new XElement("autoLogOffTimeout", Terminal.AutoLogOffTimeout),
                new XElement("hardwareProfileID", Terminal.HardwareProfileID),
                new XElement("visualProfileID", Terminal.VisualProfileID),
                new XElement("functionalityProfileID", Terminal.FunctionalityProfileID),
                new XElement("customerDisplayText1", Terminal.CustomerDisplayText1),
                new XElement("customerDisplayText2", Terminal.CustomerDisplayText2),
                new XElement("openDrawerAtLoginLogout", Terminal.OpenDrawerAtLoginLogout),
                new XElement("layoutID", Terminal.LayoutID),                
                new XElement("exitAfterEachTransaction", Terminal.ExitAfterEachTransaction),
                new XElement("updateServicePort", Terminal.UpdateServicePort),
                new XElement("transactionIDNumberSequence", Terminal.TransactionIDNumberSequence),
                new XElement("iPAddress", Terminal.IPAddress),
                new XElement("eftTerminalID", Terminal.EftTerminalID),
                new XElement("eftStoreID", Terminal.EftStoreID),
                new XElement("transactionServiceProfileID", Terminal.TransactionServiceProfileID),
                new XElement("hospTransServiceProfileID", Terminal.HospTransServiceProfileID),
                new XElement("salesTypeFilter", Terminal.SalesTypeFilter),
                new XElement("receiptIDNumberSequence", Terminal.ReceiptIDNumberSequence),
                new XElement("databaseName", Terminal.DatabaseName),
                new XElement("databaseServer", Terminal.DatabaseServer),
                new XElement("databaseUserName", Terminal.DatabaseUserName),
                new XElement("databaseUserPassword", Terminal.DatabaseUserPassword),
                new XElement("kitchenManagerProfileID", Terminal.KitchenServiceProfileID)
            );
            return xStoreSetting;
        }
    }
}

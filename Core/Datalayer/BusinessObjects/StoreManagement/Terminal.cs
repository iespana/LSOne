using System;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.StoreManagement.ListItems;
using LSOne.Utilities.DataTypes;
using LSOne.Utilities.ErrorHandling;
using LSOne.Utilities.Validation;
#if !MONO

#endif

namespace LSOne.DataLayer.BusinessObjects.StoreManagement
{
    public class Terminal : TerminalListItem
    {

        public Terminal()
            : this(RecordIdentifier.Empty,"")
        {

        }

        public Terminal(RecordIdentifier terminalID, string description) :
            base(terminalID, description)
        {
            
            AutoLogOffTimeout = 0;
            HardwareProfileID = "";
            VisualProfileID = "";
            VisualProfileName = "";
            HardwareProfileName = "";
            FunctionalityProfileID = "";
            FunctionalityProfileName = "";
            CustomerDisplayText1 = "";
            CustomerDisplayText2 = "";
            OpenDrawerAtLoginLogout = false;
            LayoutID = "";            
            ExitAfterEachTransaction = false;
            UpdateServicePort = 0;
            TransactionIDNumberSequence = "";
            IPAddress = "";
            EftTerminalID = "";
            EftStoreID = "";
            EftCustomField1 = EftCustomField2 = "";
            LSPayUseLocalServer = false;
            LSPayServerName = "";
            LSPayServerPort = 0;
            LSPayPlugin = new DataEntity("", "");
            TransactionServiceProfileID = "";
            TransactionServiceProfileName = "";
            HospTransServiceProfileID = "";
            HospTransServiceProfileName = "";
            SalesTypeFilter = "";
            ReceiptIDNumberSequence = "";
            FormInfoField1 = "";
            FormInfoField2 = "";
            FormInfoField3 = "";
            FormInfoField4 = "";
            DatabaseName = "";
            DatabaseServer = "";
            DatabaseUserName = "";
            DatabaseUserPassword = "";
            KitchenServiceProfileID = Guid.Empty;
            KitchenServiceProfileName = "";
            SalesPersonID = RecordIdentifier.Empty;
        }

        [RecordIdentifierValidation(20)]
        public override RecordIdentifier ID
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
        public override string Text
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
        /// <summary>
        ///  This field contains the identification code of the POS Hardware Profile assigned to the POS  
        /// </summary>
        [RecordIdentifierValidation(20)]
        public RecordIdentifier HardwareProfileID { get; set; }
        /// <summary>
        /// This field contains the identification code of the POS Visual Profile assigned to the POS  
        /// </summary>
        [RecordIdentifierValidation(20)]
        public RecordIdentifier VisualProfileID { get; set; }
        [RecordIdentifierValidation(20)]
        public RecordIdentifier LayoutID { get; set; }
        /// <summary>
        /// This field contains the identification code of the POS Functionality Profile assigned to the POS  
        /// </summary>
        [RecordIdentifierValidation(20)]
        public RecordIdentifier FunctionalityProfileID { get; set; }
        [StringLength(60)]
        public string FunctionalityProfileName { get; set; }
        [StringLength(60)]
        public string VisualProfileName { get; set; }
        [StringLength(50)]
        public string LayoutName { get; set; }
        [StringLength(60)]
        public string HardwareProfileName { get; set; }
        /// <summary>
        /// This field contains the time in minutes that must elapse after the last entry on the POS terminal before the operator is automatically signed off.
        /// If this field is zero (0), automatic sign off is not implemented.
        /// </summary>
        public int AutoLogOffTimeout { get; set; }
        public int AutoLockTimeout { get; set; }
        /// <summary>
        /// Customerdisplay text 1.  
        /// </summary>
        [StringLength(40)]
        public string CustomerDisplayText1 { get; set; }
        /// <summary>
        ///  Customerdisplay text 2.
        /// </summary>
        [StringLength(40)]
        public string CustomerDisplayText2 { get; set; }
        /// <summary>
        /// This field contains the identification code of the POS Store Server Service Profile assigned to the POS  
        /// </summary>
        public RecordIdentifier TransactionServiceProfileID { get; set; }
        [StringLength(50)]
        public string TransactionServiceProfileName { get; set; }
        /// <summary>
        /// This field contains the identification code of the POS Hospitality Store Server Service Profile assigned to the POS  
        /// </summary>
        [RecordIdentifierValidation(20)]
        public RecordIdentifier HospTransServiceProfileID { get; set; }
        [StringLength(50)]
        public string HospTransServiceProfileName { get; set; }
        /// <summary>
        /// The transaction id number sequence identifier
        /// </summary>
        [StringLength(50)]
        public string TransactionIDNumberSequence { get; set; }
        [StringLength(100)]
        public string IPAddress { get; set; }
        /// <summary>
        /// This field contains the POS terminal number used for Electronic Funds Transfer transactions.
        /// </summary>
        [StringLength(20)]
        public string EftTerminalID { get; set; }
        /// <summary>
        /// This field contains the POS terminal number used for Electronic Funds Transfer transactions.
        /// </summary>
        [StringLength(20)]
        public string EftStoreID { get; set; }
        /// <summary>
        /// This field contains any custom data that an EFT provider may require for further setup
        /// </summary>
        [StringLength(200)]
        public string EftCustomField1 { get; set; }
        /// <summary>
        /// This field contains any custom data that an EFT provider may require for further setup
        /// </summary>
        [StringLength(200)]
        public string EftCustomField2 { get; set; }

        /// <summary>
        /// If true, the server name and server port on the terminal are used for LS Pay, 
        /// otherwise server name and port from the hardware profile are used.
        /// </summary>
        public bool LSPayUseLocalServer { get; set; }

        /// <summary>
        /// Host address to the server that runs the LS Pay service if LS Pay is used
        /// </summary>
        [StringLength(100)]
        public string LSPayServerName { get; set; }

        /// <summary>
        /// The LS Pay server port
        /// </summary>
        public int LSPayServerPort { get; set; }

        /// <summary>
        /// LS Pay plugin, identifies the payment terminal to use when communicating with the LS Pay service
        /// </summary>
        public DataEntity LSPayPlugin { get; set; }

        /// <summary>
        /// Determines if the LS Pay device supports reference refunds
        /// </summary>
        public bool LSPaySupportReferenceRefund { get; set; }

        /// <summary>
        /// If this is true then cash drawer opens after the cashier logs on and off the POS 
        /// If the ExitAfterEachTransaction is true, the drawer opens after each transaction if this field is marked with a check mark too.
        /// </summary>
        public bool OpenDrawerAtLoginLogout { get; set; }
        /// <summary>
        /// If this is true then the POS terminal automatically signs the cashier off after each transaction.
        /// If the Open Drawer at LI/LO field is set to true , the drawer opens after each transaction if this field is also true.
        /// </summary>
        public bool ExitAfterEachTransaction { get; set; }
        public int UpdateServicePort { get; set; }
        [StringLength(250)]
        public string SalesTypeFilter { get; set; }
        public SuspendedTransactionsStatementPostingEnum SuspendedTransactionsStatementPosting { get; set; }
        public RecordIdentifier ReceiptIDNumberSequence { get; set; }

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

        /// <summary>
        /// A field that LSRetail does not use currently. Partners can use it for whatever they want
        /// </summary>
        [StringLength(50)]
        public string DatabaseName { get; set; }

        /// <summary>
        /// A field that LSRetail does not use currently. Partners can use it for whatever they want
        /// </summary>
        [StringLength(50)]
        public string DatabaseServer { get; set; }

        /// <summary>
        /// A field that LSRetail does not use currently. Partners can use it for whatever they want
        /// </summary>
        [StringLength(50)]
        public string DatabaseUserName { get; set; }

        /// <summary>
        /// A field that LSRetail does not use currently. Partners can use it for whatever they want
        /// </summary>
        [StringLength(50)]
        public string DatabaseUserPassword { get; set; }

        public RecordIdentifier KitchenServiceProfileID { get; set; }
        [StringLength(60)]
        public string KitchenServiceProfileName { get; set; }

        /// <summary>
        /// Should the POS user be prompted with a switch user dialog when entering the POS from table view (hospitality only)
        /// </summary>
        public bool SwitchUserWhenEnteringPOS { get; set; }

        public RecordIdentifier SalesPersonID { get; set; }

        /// <summary>
        /// To include transactions from terminal in statements or not
        /// </summary>
        public bool IncludeTerminalInStatement { get; set; }

        /// <summary>
        /// When terminal allows statement posting. Only applicable when IncludeTerminalInStatement is true
        /// </summary>
        public AllowTerminalStatementPostingEnum AllowTerminalStatementPosting { get; set; }

        public bool Activated { get; set; }

        public DateTime LastActivatedDate { get; set; }

        /// <summary>
        /// The ID of the main menu header used for the inventory app
        /// </summary>
        public string InventoryMainMenuID { get; set; }

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
                            case "terminalID":
                                ID = current.Value;
                                break;
                            case "terminalName":
                                Name = current.Value;
                                break;
                            case "storeID":
                                StoreID = current.Value;
                                break;
                            case "autoLogOffTimeout":
                                AutoLogOffTimeout = Convert.ToInt32(current.Value);
                                break;
                            case "hardwareProfileID":
                                HardwareProfileID = current.Value;
                                break;
                            case "visualProfileID":
                                VisualProfileID = current.Value;
                                break;
                            case "functionalityProfileID":
                                FunctionalityProfileID = current.Value;
                                break;
                            case "customerDisplayText1":
                                CustomerDisplayText1 = current.Value;
                                break;
                            case "customerDisplayText2":
                                CustomerDisplayText2 = current.Value;
                                break;
                            case "openDrawerAtLoginLogout":
                                OpenDrawerAtLoginLogout = current.Value != "false";
                                break;
                            case "layoutID":
                                LayoutID = current.Value;
                                break;                            
                            case "exitAfterEachTransaction":
                                ExitAfterEachTransaction = current.Value != "false";
                                break;
                            case "updateServicePort":
                                UpdateServicePort = Convert.ToInt32(current.Value);
                                break;
                            case "transactionIDNumberSequence":
                                TransactionIDNumberSequence = current.Value;
                                break;
                            case "iPAddress":
                                IPAddress = current.Value;
                                break;
                            case "eftTerminalID":
                                EftTerminalID = current.Value;
                                break;
                            case "eftStoreID":
                                EftStoreID = current.Value;
                                break;
                            case "eftCustomField1":
                                EftCustomField1 = current.Value;
                                break;
                            case "eftCustomField2":
                                EftCustomField2 = current.Value;
                                break;
                            case "transactionServiceProfileID":
                                TransactionServiceProfileID = current.Value;
                                break;
                            case "hospTransServiceProfileID":
                                HospTransServiceProfileID = current.Value;
                                break;
                            case "salesTypeFilter":
                                SalesTypeFilter = current.Value;
                                break;
                            case "receiptIDNumberSequence":
                                ReceiptIDNumberSequence = current.Value;
                                break;
                            case "formInfoField1":
                                FormInfoField1 = current.Value;
                                break;
                            case "formInfoField2":
                                FormInfoField2 = current.Value;
                                break;
                            case "formInfoField3":
                                FormInfoField3 = current.Value;
                                break;
                            case "formInfoField4":
                                FormInfoField4 = current.Value;
                                break;
                            case "databaseName":
                                DatabaseName = current.Value;
                                break;
                            case "databaseServer":
                                DatabaseServer = current.Value;
                                break;
                            case "databaseUserName":
                                DatabaseUserName = current.Value;
                                break;
                            case "databaseUserPassword":
                                DatabaseUserPassword = current.Value;
                                break;
                            case "kitchenManagerProfileID":
                                KitchenServiceProfileID = new Guid(current.Value);
                                break;
                            case "SalesPersonID":
                                SalesPersonID = current.Value;
                                break;
                            case "switchUserWhenEnteringPOS":
                                SwitchUserWhenEnteringPOS = current.Value != "false";
                                break;
                            case "allowTerminalStatementPosting":
                                AllowTerminalStatementPosting = (AllowTerminalStatementPostingEnum)Convert.ToInt32(current.Value);
                                break;
                            case "includeTerminalInStatement":
                                IncludeTerminalInStatement = current.Value != "false";
                                break;
                            case "activated":
                                Activated = current.Value != "false";
                                break;
                            case "lastActivatedDate":
                                LastActivatedDate =  Conversion.XmlStringToDateTime(current.Value);
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
            XElement xml = new XElement("terminal",
                new XElement("terminalID", (string)ID),
                new XElement("terminalName", Name),
                new XElement("storeID", (string)StoreID),
                new XElement("autoLogOffTimeout", AutoLogOffTimeout),
                new XElement("hardwareProfileID", HardwareProfileID),
                new XElement("visualProfileID", VisualProfileID),
                new XElement("functionalityProfileID", FunctionalityProfileID),
                new XElement("customerDisplayText1", CustomerDisplayText1),
                new XElement("customerDisplayText2", CustomerDisplayText2),
                new XElement("openDrawerAtLoginLogout", OpenDrawerAtLoginLogout),
                new XElement("layoutID", LayoutID),                
                new XElement("exitAfterEachTransaction", ExitAfterEachTransaction),
                new XElement("updateServicePort", UpdateServicePort),
                new XElement("transactionIDNumberSequence", TransactionIDNumberSequence),
                new XElement("iPAddress", IPAddress),
                new XElement("eftTerminalID", EftTerminalID),
                new XElement("eftStoreID", EftStoreID),
                new XElement("eftCustomField1", EftCustomField1),
                new XElement("eftCustomField2", EftCustomField2),
                new XElement("transactionServiceProfileID", TransactionServiceProfileID),
                new XElement("hospTransServiceProfileID", HospTransServiceProfileID),
                new XElement("salesTypeFilter", SalesTypeFilter),
                new XElement("receiptIDNumberSequence", ReceiptIDNumberSequence),
                new XElement("formInfoField1", FormInfoField1),
                new XElement("formInfoField2", FormInfoField2),
                new XElement("formInfoField3", FormInfoField3),
                new XElement("formInfoField4", FormInfoField4),
                new XElement("databaseName", DatabaseName),
                new XElement("databaseServer", DatabaseServer),
                new XElement("databaseUserName", DatabaseUserName),
                new XElement("databaseUserPassword", DatabaseUserPassword),
                new XElement("kitchenManagerProfileID", KitchenServiceProfileID),
                new XElement("SalesPersonID", SalesPersonID),
                new XElement("switchUserWhenEnteringPOS", SwitchUserWhenEnteringPOS),
                new XElement("allowTerminalStatementPosting", AllowTerminalStatementPosting),
                new XElement("includeTerminalInStatement", IncludeTerminalInStatement),
                new XElement("activated", Activated),
                new XElement("lastActivatedDate", LastActivatedDate)
            );
            return xml;
        }
    }
}
